/* ------------------------------------------------
 * SqlQueryBuilder.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Next2Friends.WebServices.Properties;

namespace Next2Friends.WebServices.GeoMessage.Data
{
    static class SqlQueryBuilder
    {
        /// <summary>
        /// Builds INSERT INTO query for the specified <c>table</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>table</c> is <c>null</c>, or
        /// if the specified <c>connection</c> is <c>null</c>.
        /// </exception>
        public static Int32? Insert<T>(T table, SqlConnection connection)
        {
            return Insert<T>(table, connection, null);
        }

        /// <summary>
        /// Builds INSERT INTO query for the specified <c>table</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="connection"></param>
        /// <param name="transaction">Can be <c>null</c>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>table</c> is <c>null</c>, or
        /// if the specified <c>connection</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <c>table</c> is not marked with <see cref="SqlTableAttribute"/> attribute, or
        /// if the specified <c>table</c> does not contain any propoerties marked with <see cref="SqlParameterAttribute"/> attribute, or
        /// if one of the properties is marked as being not nullable but its value is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Failed to insert a new record into the specified <c>table</c>.
        /// </exception>
        public static Int32? Insert<T>(T table, SqlConnection connection, SqlTransaction transaction)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlTableAttribute tableInfo = GetTableInfo<T>();

            IDictionary<PropertyInfo, SqlParameterAttribute> tableProperties = GetSqlEntityProperties<T>();
            if (tableProperties.Count == 0)
                throw new ArgumentException(Resources.Argument_NoSqlParameterProps);

            IDictionary<SqlParameter, Object> paramValueMap = new Dictionary<SqlParameter, Object>();

            foreach (PropertyInfo tableProperty in tableProperties.Keys)
            {
                if (IsPrimaryKey(tableProperty))
                    continue;

                Object qualifiedValue = GetQualifiedValue(tableProperty.GetValue(table, null));
                SqlParameterAttribute propInfo = tableProperties[tableProperty];

                if (qualifiedValue == null)
                {
                    if (!propInfo.IsNullable)
                        throw new ArgumentException(String.Format(Resources.Argument_NotNullablePropIsNull, tableProperty.Name));
                    continue;
                }

                paramValueMap.Add(CreateSqlParameter(tableProperties[tableProperty]), qualifiedValue);
            }

            if (paramValueMap.Count == 0)
                return null;

            SqlParameter[] parameters = new SqlParameter[paramValueMap.Keys.Count];
            paramValueMap.Keys.CopyTo(parameters, 0);

            SqlCommand command = new SqlCommand(CreateInsertQuery(tableInfo.Name, parameters), connection, transaction);
            command.AddNamedParameterRange(parameters);

            Object[] values = new Object[paramValueMap.Values.Count];
            paramValueMap.Values.CopyTo(values, 0);
            for (var i = 0; i < values.Length; i++)
                command.Parameters[i].Value = values[i];

            command.ExecuteNonQuery();

            Decimal? identity = SqlExtensions.GetIdentity(connection, transaction);
            if (identity.HasValue)
                return Convert.ToInt32(identity.Value);

            throw new InvalidOperationException(String.Format(Resources.Argument_InsertIntoFailed, tableInfo.Name));
        }

        public static void Update<T>(T table, SqlConnection connection)
        {
            Update<T>(table, connection, null);
        }

        public static void Update<T>(T table, SqlConnection connection, SqlTransaction transaction)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            if (connection == null)
                throw new ArgumentNullException("connection");

            SqlTableAttribute tableInfo = GetTableInfo<T>();
            IDictionary<PropertyInfo, SqlParameterAttribute> tableProperties = GetSqlEntityProperties<T>();
            if (tableProperties.Count == 0)
                throw new ArgumentException(Resources.Argument_NoSqlParameterProps);

            IDictionary<SqlParameter, Object> paramValueMap = new Dictionary<SqlParameter, Object>();
            SqlParameter pkParameter = null;

            foreach (PropertyInfo tableProperty in tableProperties.Keys)
            {
                if (IsForeignKey(tableProperty))
                    continue;

                Object qualifiedValue = GetQualifiedValue(tableProperty.GetValue(table, null));
                SqlParameterAttribute propInfo = tableProperties[tableProperty];

                if (qualifiedValue == null)
                {
                    if (!propInfo.IsNullable)
                        throw new ArgumentException(String.Format(Resources.Argument_NotNullablePropIsNull, tableProperty.Name));
                    continue;
                }

                SqlParameter currentParam = CreateSqlParameter(tableProperties[tableProperty]);

                if (IsPrimaryKey(tableProperty))
                {
                    pkParameter = currentParam;
                    pkParameter.Value = qualifiedValue;
                }
                else
                {
                    paramValueMap.Add(currentParam, qualifiedValue);
                }
            }

            if (paramValueMap.Count == 0)
                return;

            SqlParameter[] parameters = new SqlParameter[paramValueMap.Keys.Count];
            paramValueMap.Keys.CopyTo(parameters, 0);

            SqlCommand command = new SqlCommand(CreateUpdateQuery(tableInfo.Name, pkParameter, parameters), connection, transaction);
            command.AddNamedParameterRange(parameters);
            command.AddNamedParameter(pkParameter);

            Object[] values = new Object[paramValueMap.Values.Count + 1];
            paramValueMap.Values.CopyTo(values, 0);
            values[values.Length - 1] = pkParameter.Value;

            for (var i = 0; i < values.Length; i++)
                command.Parameters[i].Value = values[i];

            command.ExecuteNonQuery();
        }

        private static Boolean IsForeignKey(PropertyInfo propertyInfo)
        {
            Object[] attributes = propertyInfo.GetCustomAttributes(typeof(SqlForeignKeyAttribute), false);
            return attributes != null && attributes.Length > 0;
        }

        private static Boolean IsPrimaryKey(PropertyInfo propertyInfo)
        {
            Object[] attributes = propertyInfo.GetCustomAttributes(typeof(SqlPrimaryKeyAttribute), false);
            return attributes != null && attributes.Length > 0;
        }

        private static String CreateInsertQuery(String tableName, SqlParameter[] parameters)
        {
            StringBuilder insertQuery = new StringBuilder();
            insertQuery.Append("insert into ").Append(tableName).Append("(");

            var firstParameter = true;
            foreach (SqlParameter sqlParameter in parameters)
            {
                if (firstParameter)
                {
                    insertQuery.Append(sqlParameter.ParameterName);
                    firstParameter = false;
                }
                else
                {
                    insertQuery.Append(",").Append(sqlParameter.ParameterName);
                }
            }

            insertQuery.Append(") values(");

            firstParameter = true;
            foreach (SqlParameter sqlParameter in parameters)
            {
                if (firstParameter)
                {
                    insertQuery.Append(sqlParameter.NamedParameter());
                    firstParameter = false;
                }
                else
                {
                    insertQuery.Append(",").Append(sqlParameter.NamedParameter());
                }
            }

            insertQuery.Append(")");
            return insertQuery.ToString();
        }

        private static String CreateUpdateQuery(String tableName, SqlParameter primaryKey, SqlParameter[] parameters)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append("update ").Append(tableName).Append(" set ");
            var firstParameter = true;
            foreach (SqlParameter sqlParameter in parameters)
            {
                if (firstParameter)
                {
                    updateQuery.Append(sqlParameter.ParameterName).Append("=").Append(sqlParameter.NamedParameter());
                    firstParameter = false;
                }
                else
                {
                    updateQuery.Append(",").Append(sqlParameter.ParameterName).Append("=").Append(sqlParameter.NamedParameter());
                }
            }

            updateQuery.Append(" where ").Append(primaryKey.ParameterName).Append("=").Append(primaryKey.NamedParameter());
            return updateQuery.ToString();
        }

        private static SqlParameter CreateSqlParameter(SqlParameterAttribute paramInfo)
        {
            return new SqlParameter(paramInfo.Name, paramInfo.Type, paramInfo.Size)
            {
                IsNullable = paramInfo.IsNullable
            };
        }

        private static Object GetQualifiedValue(Object value)
        {
            if (value == null)
                return value;

            Type valueType = value.GetType();

            if (valueType.Equals(typeof(Single)))
            {
                if (Single.IsNaN(Convert.ToSingle(value)))
                    return null;
            }

            if (valueType.Equals(typeof(Double)))
            {
                if (Double.IsNaN(Convert.ToDouble(value)))
                    return null;
            }

            return value;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// If the table denoted by <c>T</c> parameter is not marked with <see cref="SqlTableAttribute"/> attribute.
        /// </exception>
        private static SqlTableAttribute GetTableInfo<T>()
        {
            Object[] attributes = typeof(T).GetCustomAttributes(typeof(SqlTableAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return (SqlTableAttribute)attributes[0];

            throw new ArgumentException(Resources.Argument_NotSqlTable);
        }

        private static IDictionary<PropertyInfo, SqlParameterAttribute> GetSqlEntityProperties<T>()
        {
            Type entityType = typeof(T);
            IDictionary<PropertyInfo, SqlParameterAttribute> result = new Dictionary<PropertyInfo, SqlParameterAttribute>();
            PropertyInfo[] properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (properties != null)
            {
                foreach (PropertyInfo prop in properties)
                {
                    Object[] attributes = prop.GetCustomAttributes(typeof(SqlParameterAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                        result.Add(prop, (SqlParameterAttribute)attributes[0]);
                }
            }

            return result;
        }
    }
}

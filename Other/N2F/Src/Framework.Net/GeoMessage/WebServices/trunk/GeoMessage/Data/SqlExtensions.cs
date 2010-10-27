/* ------------------------------------------------
 * SqlExtensions.cs
 * Copyright © 2008 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * ---------------------------------------------- */

using System;
using System.Data;
using System.Data.SqlClient;

namespace Next2Friends.GeoMessage.Data
{
    static class SqlExtensions
    {
        /// <summary>
        /// Adds new named parameter to the specified <c>command</c>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>command</c> is <c>null</c>, or if the specified <c>parameter</c> is <c>null</c>.
        /// </exception>
        public static void AddNamedParameter(this SqlCommand command, SqlParameter parameter)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            SqlParameter namedParameter = new SqlParameter(parameter.NamedParameter(), parameter.SqlDbType, parameter.Size);
            namedParameter.IsNullable = parameter.IsNullable;
            command.Parameters.Add(namedParameter);
        }

        /// <summary>
        /// Adds a range of named parameters to the specified <c>command</c>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters">Can be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// If the specified <c>command</c> is <c>null</c>.
        /// </exception>
        public static void AddNamedParameterRange(this SqlCommand command, params SqlParameter[] parameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            foreach (var parameter in parameters)
                AddNamedParameter(command, parameter);
        }

        public static Decimal? GetIdentity(SqlConnection connection)
        {
            return GetIdentity(connection, null);
        }

        public static Decimal? GetIdentity(SqlConnection connection, SqlTransaction transaction)
        {
            SqlCommand command = new SqlCommand("select @@identity", connection, transaction);
            return (Decimal?)command.ExecuteScalar();
        }

        /// <summary>
        /// Returns named parameter for the specified <c>parameter</c>.
        /// </summary>
        /// <param name="parameter">Specifies the <c>SqlParameter</c> to create named parameter for.</param>
        /// <returns>Named parameter for the specified <c>parameter</c>.</returns>
        public static String NamedParameter(this SqlParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return String.Format("@{0}", parameter.ParameterName.ToLower());
        }

        /// <summary>
        /// Returns the typed value of the specified <c>parameter</c>.
        /// </summary>
        /// <typeparam name="T">Specifies the type to cast the value of the specified <c>parameter</c> to.</typeparam>
        /// <param name="parameter">Specifies the <c>SqlParameter</c> to get the value for.</param>
        /// <returns>Typed value of the specified <c>parameter</c>.</returns>
        public static T Value<T>(this SqlParameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            Object value = parameter.Value;
            if (value == null)
                return default(T);
            return (T)value;
        }
    }
}

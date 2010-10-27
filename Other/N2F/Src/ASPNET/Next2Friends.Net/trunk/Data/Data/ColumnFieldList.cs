using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// A helper class to handle the columns returned from an ADO.NET operation
    /// </summary>
    public class ColumnFieldList
    {
        public List<string> Columns = new List<string>();

        public ColumnFieldList(IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                Columns.Add(dr.GetName(i).ToUpper());
            }
        }


        /// <summary>
        /// Checks if the specified column is present in the column list
        /// </summary>
        /// <param name="Name">The name of the column</param>
        /// <returns>True if the column exists. False otherwise.</returns>
        public bool IsColumnPresent(string Name)
        {
            Name = Name.ToUpper();

            for (int i = 0; i < Columns.Count; i++)
            {
                if (Columns[i] == Name)
                    return true;
            }

            return false;
        }
    }
}

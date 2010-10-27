using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// 
    /// </summary>
    public enum PlanLevel { Lite, Premium, Enterprise }


    /// <summary>
    /// Represents a Business in the system
    /// </summary>
    public partial class Business
    {
        
        /// <summary>
        /// Gets all the businesses in the system performing joins with other tables
        /// </summary>
        /// <returns>A list of Businesses</returns>
        public static List<Business> GetAllBusinessWithJoin()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllBusinessWithJoin");

            List<Business> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }
    }
}

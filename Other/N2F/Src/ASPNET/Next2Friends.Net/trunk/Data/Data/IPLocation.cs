using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class IPLocation
    {
        /// <summary>
        /// Gets all the IPLocation in the database 
        /// </summary>
        public static List<IPLocation> SearchLocation(string LocationText)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_SearchLocation");
            db.AddInParameter(dbCommand, "LocationText", DbType.String, LocationText);

            List<IPLocation> arr = null;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        /// <summary>
        /// Gets all the IPLocation in the database 
        /// </summary>
        public static IPLocation GetIPLocationByCountry(string Country)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetIPLocationByCountry");
            db.AddInParameter(dbCommand, "Country", DbType.String, Country);

            List<IPLocation> arr = null;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }


    }
}


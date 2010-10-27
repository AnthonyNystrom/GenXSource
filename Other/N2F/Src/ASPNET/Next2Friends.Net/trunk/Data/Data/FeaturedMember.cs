using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// Deals with the member that are featured on the front page of the site
    /// </summary>
    public partial class FeaturedMember
    {
               
        /// <summary>
        /// Calls the database and gets the top FeaturedMembers for the site
        /// </summary>
        /// <returns>A list of FeaturedMembers</returns>
        public static List<FeaturedMember> GetTopFeaturedMembers()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTopFeaturedMembers");

            List<FeaturedMember> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
               arr = PopulateObject(dr);
               dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    /// <summary>
    /// Stores favourite member list against a single member
    /// </summary>
    public partial class FavouriteMember
    {
        /// <summary>
        /// Save to the database after checking if the record already exists
        /// If the record already exists no new row is inserted
        /// </summary>
        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveFavouriteMemberWithCheck");

            db.AddInParameter(dbCommand, "FavouriteMemberID", DbType.Int32, FavouriteMemberID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "TheFavouriteMemberID", DbType.Int32, TheFavouriteMemberID);
            db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.FavouriteMemberID = ID;
                }

                dr.Close();
            }

        }
    }
}

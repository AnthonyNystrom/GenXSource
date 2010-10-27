using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class Vote
    {
        /// <summary>
        /// Saves a Vote to the DB if it already doesn't exist
        /// </summary>
        /// <returns>True if a new row was entered. False otherwise.</returns>
        public bool SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveVoteWithCheck");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "PhotoID", DbType.Int32, PhotoID);
            db.AddInParameter(dbCommand, "VideoID", DbType.Int32, VideoID);
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);
            db.AddInParameter(dbCommand, "GroupID", DbType.Int32, GroupID);
            db.AddInParameter(dbCommand, "Value", DbType.Int32, Value);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int voted = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (voted == 1)
                        return true;
                    else
                        return false;
                }

                dr.Close();
            }

            return false;
        }
    }
}

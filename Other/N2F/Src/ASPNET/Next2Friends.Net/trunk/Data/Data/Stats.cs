using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public class SystemStatistics
    {
        public string Members { get; set; }
        public string ProximityMatches { get; set; }
        public string FriendConnections { get; set; }
        public string NSpots { get; set; }
        //public string Groups { get; set; }

        public SystemStatistics()
        {

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_CommunityStats");

            // Create the object array from the datareader
            List<AskAFriend> arr = new List<AskAFriend>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    Members = ((int)dr["Members"]).ToString();
                    ProximityMatches = ((int)dr["ProximityMatches"]).ToString();
                    FriendConnections = ((int)dr["FriendConnections"]).ToString();
                    NSpots = ((int)dr["NSpots"]).ToString();
                }
                dr.Close();
            }


        }
    }
}

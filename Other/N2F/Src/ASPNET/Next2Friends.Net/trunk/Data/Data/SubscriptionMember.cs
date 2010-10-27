using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class SubscriptionMember
    {

        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveSubscriptionMemberWithCheck");

            db.AddInParameter(dbCommand, "SubscriptionMemberID", DbType.Int32, SubscriptionMemberID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "SubscribeToMemberID", DbType.Int32, SubscribeToMemberID);
            db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.SubscriptionMemberID = ID;
                }

                dr.Close();
            }

        }

        public static int GetSubscriberCountByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetSubscriberCountByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);           

            return (int)db.ExecuteScalar(dbCommand);
        }

        public static List<SubscriptionItem> GetSubscriptionMembersByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetSubscriptionMembersByMemberID");

            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<SubscriptionItem> Subscribers = new List<SubscriptionItem>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                while (dr.Read())
                {
                    SubscriptionItem item = new SubscriptionItem();
                    item.WebMemberID = (string)dr["WebMemberID"];
                    item.NickName = (string)dr["NickName"];
                    item.PhotoURL = "user/" + (string)dr["PhotoURL"];
                    item.LastOnline = (DateTime)dr["DTOnline"];
                    item.ISOCountry = (string)dr["ISOCountry"];

                    Subscribers.Add(item);
                }

                dr.Close();
            }

            return Subscribers;
        }
    }

    public class SubscriptionItem
    {
        public string WebMemberID { get; set; }
        public string NickName { get; set; }
        public string PhotoURL { get; set; }
        public DateTime LastOnline { get; set; }
        public string ISOCountry { get; set; }
    }
}

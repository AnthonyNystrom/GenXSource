using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class ChatOnline
    {
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string WebMemberID { get; set; }
        public string AvatorUrl { get; set; }
        public string CustomMessage { get; set; }

        /// <summary>
        /// Gets all online/offline friends
        /// </summary>
        public static List<ChatOnline> GetChatFriendsByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetChatFriendsByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<ChatOnline> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoinCustom(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        /// <summary>
        /// Gets all the ChatOnline in the database 
        /// </summary>
        public static ChatOnline GetChatOnlineByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetChatOnlineByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<ChatOnline> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of ChatOnlines
        /// </summary>
        public static List<ChatOnline> PopulateObjectWithJoinCustom(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<ChatOnline> arr = new List<ChatOnline>();

            ChatOnline obj;

            while (dr.Read())
            {
                obj = new ChatOnline();
                if (list.IsColumnPresent("ChatOnlineID")) { obj._chatOnlineID = (int)dr["ChatOnlineID"]; }
                if (list.IsColumnPresent("MemberID")) { obj._memberID = (int)dr["MemberID"]; }
                if (list.IsColumnPresent("LastCommDt")) { obj._lastCommDt = (DateTime)dr["LastCommDt"]; }
                if (list.IsColumnPresent("Status")) { obj._status = (int)dr["Status"]; }

                if (list.IsColumnPresent("NickName")) { obj.NickName = (string)dr["NickName"]; }
                if (list.IsColumnPresent("FirstName")) { obj.FirstName = (string)dr["FirstName"]; }
                if (list.IsColumnPresent("LastName")) { obj.LastName = (string)dr["LastName"]; }
                if (list.IsColumnPresent("Email")) { obj.Email = (string)dr["Email"]; }
                if (list.IsColumnPresent("WebMemberID")) { obj.WebMemberID = (string)dr["WebMemberID"]; }
                if (list.IsColumnPresent("AvatorUrl")) { obj.AvatorUrl = (string)dr["AvatorUrl"]; }
                if (list.IsColumnPresent("CustomMessage")) { obj.CustomMessage = (string)dr["CustomMessage"]; }

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{
    public partial class ChatMessage
    {
        public string WebMemberIDFrom { get; set; }

        /// <summary>
        /// Gets all the ChatMessage in the database 
        /// </summary>
        public static List<ChatMessage> GetNewChatMessageByMemberID(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNewChatMessageByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<ChatMessage> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObjectWithJoinCustom(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        public static void MarkChatMessageDelivered(string ChatMessageWebID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_MarkChatMessageDelivered");
            db.AddInParameter(dbCommand, "ChatMessageWebID", DbType.String, ChatMessageWebID);

            //execute the stored procedure
            int effectedRows = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of ChatMessages
        /// </summary>
        public static List<ChatMessage> PopulateObjectWithJoinCustom(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<ChatMessage> arr = new List<ChatMessage>();

            ChatMessage obj;

            while (dr.Read())
            {
                obj = new ChatMessage();
                if (list.IsColumnPresent("ChatMessageID")) { obj._chatMessageID = (int)dr["ChatMessageID"]; }
                if (list.IsColumnPresent("ChatMessageWebID")) { obj._chatMessageWebID = (string)dr["ChatMessageWebID"]; }
                if (list.IsColumnPresent("MemberIDFrom")) { obj._memberIDFrom = (int)dr["MemberIDFrom"]; }
                if (list.IsColumnPresent("MemberIDTo")) { obj._memberIDTo = (int)dr["MemberIDTo"]; }
                if (list.IsColumnPresent("Message")) { obj._message = (string)dr["Message"]; }
                if (list.IsColumnPresent("Delivered")) { obj._delivered = (bool)dr["Delivered"]; }
                if (list.IsColumnPresent("DTCreated")) { obj._dTCreated = (DateTime)dr["DTCreated"]; }
                if (list.IsColumnPresent("WebMemberIDFrom")) { obj.WebMemberIDFrom = (String)dr["WebMemberIDFrom"]; }

                
                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }
    }
}

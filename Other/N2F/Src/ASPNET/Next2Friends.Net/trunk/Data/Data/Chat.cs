using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Next2Friends.Data
{
    public partial class Chat
    {
        public static List<Chat> GetNewChats(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNewChats");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            // Create the object array from the datareader
            return PopulateObject(dr);
        }

        public static void MarkChatRead(string ChatWebId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_MarkChatRead");
            db.AddInParameter(dbCommand, "ChatWebId", DbType.String, ChatWebId);

            //execute the stored procedure
            int effectedRows = db.ExecuteNonQuery(dbCommand);
        }

        //public static List<ChatOnline> GetFriends()
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = db.GetStoredProcCommand("HG_MarkChatRead");
        //    db.AddInParameter(dbCommand, "ChatWebId", DbType.String, ChatWebId);

        //    //execute the stored procedure
        //    int effectedRows = db.ExecuteNonQuery(dbCommand);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum MessageSendResponse {Sent,BadAddress,UnexpectedError,EmailMustHaveVideoMessage }

    [Serializable]
    public partial class Message
    {
        public Message(bool CreateMessageWebID)
        {
            if (CreateMessageWebID)
            {
                //this.MessageWebID = Next2Friends.Misc.MiniGuid.
            }
        }

        public string FromNickName { get; set; }
        public string WebMemberIDFrom { get; set; }

        public static Message[] GetNewMessages(int MemberID)
        {
            List<Message> messages = new List<Message>();

            try
            {
                Database db = DatabaseFactory.CreateDatabase();

                DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNewMessages");
                db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

                

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    messages = Message.PopulateMessageWithJoin(dr);

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return messages.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="Page">Zero based Page indexer</param>
        /// <returns></returns>
        public static Message[] GetTrash(int MemberID, int Page, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTrashMessages");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }

            return messages.ToArray();
        }


        /// <summary>
        /// Gets the number of message in the trash
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static int GetTrashMessageCount(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetTrashMessageCount");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            int count = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }

                dr.Close();
            }

            return count;
        }

        /// <summary>
        /// Gets the number of message in the sent
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static int GetSentMessageCount(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetSentMessageCount");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            int count = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }

                dr.Close();
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="Page">Zero based Page indexer</param>
        /// <returns></returns>
        public static Message[] GetSent(int MemberID, int Page, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetSentMessages");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }

            return messages.ToArray();
        }

        /// <summary>
        /// Gets the number of message in the Inbox
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static int GetMessageCount(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMessageCount");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            int count = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    count = dr.GetInt32(0);
                }

                dr.Close();
            }

            return count;
        }

        public static Message[] GetExternalMessageHeader(string WebMessageID,string PassKey)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetExternalMessageHeader");            
            db.AddInParameter(dbCommand, "WebMessageID", DbType.String, WebMessageID);
            db.AddInParameter(dbCommand, "PassKey", DbType.String, PassKey);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }


            return messages.ToArray();
        }
        
        public static Message[] GetMessageHeaderWithReply(string WebMessageID, int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMessageHeaderWithReply");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebMessageID", DbType.String, WebMessageID);            

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }


            return messages.ToArray();
        }

        public static Message[] GetSentMessageHeaderWithReply(string WebMessageID, int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetSentMessageHeaderWithReply");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebMessageID", DbType.String, WebMessageID);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }


            return messages.ToArray();
        }

        public static Message GetMessageWithJoin(string WebMessageID, int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMessageWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebMessageID", DbType.String, WebMessageID);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }

            if (messages.Count > 0)
                return messages[0];
            else
                return null;
        }

        public static Message GetExternalMessageWithJoin(string WebMessageID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetExternalMessageWithJoin");
            db.AddInParameter(dbCommand, "WebMessageID", DbType.String, WebMessageID);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }

            if (messages.Count > 0)
                return messages[0];
            else
                return null;
        }

        public static Message[] GetAllMessagesByMemberID(int MemberID, int Page, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAllMessagesByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);

            List<Message> messages = new List<Message>();

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                messages = Message.PopulateMessageWithJoin(dr);

                dr.Close();
            }

            return messages.ToArray();
        }

        public static void DeleteChatMessageList(int MemberID, string WebMessageIDList, bool EmptyTrash)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteChatMessageList");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebMessageIDList", DbType.String, WebMessageIDList);
            db.AddInParameter(dbCommand, "EmptyTrash", DbType.Boolean, EmptyTrash);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
    

        }

        public static void DeleteSentMessageList(int MemberID, string WebMessageIDList, bool EmptyTrash)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteSentMessageList");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "WebMessageIDList", DbType.String, WebMessageIDList);
            db.AddInParameter(dbCommand, "EmptyTrash", DbType.Boolean, EmptyTrash);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Message> PopulateMessageWithJoin(IDataReader dr)
        {
            List<Message> arr = new List<Message>();

            Message obj;

            while (dr.Read())
            {
                obj = new Message();
                obj._messageID = (int)dr["MessageID"];
                obj._webMessageID = (string)dr["WebMessageID"];
                obj._videoMessageResourceFileID = (int)dr["VideoMessageResourceFileID"];
                obj._videoMessageToken = (string)dr["VideoMessageToken"];
                obj._memberIDFrom = (int)dr["MemberIDFrom"];
                obj._inReplyToID = (int)dr["InReplyToID"];
                obj.FromNickName = (string)dr["NickName"];
                obj._memberIDTo = (int)dr["MemberIDTo"];
                obj._body = (string)dr["Body"];
                obj._isRead = (bool)dr["IsRead"];
                obj._isDeleted = (bool)dr["IsDeleted"];
                obj._isFetched = (bool)dr["IsFetched"];
                obj._dTCreated = (DateTime)dr["DTCreated"];
                obj.WebMemberIDFrom = (string)dr["WebMemberIDFrom"];

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }



        

    }

}

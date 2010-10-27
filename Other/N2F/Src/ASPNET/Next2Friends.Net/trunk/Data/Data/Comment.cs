using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Next2Friends.Data
{
    public enum CommentType
    {
        None = 0,
        Video = 1,
        Photo = 2,
        Wall = 3,
        NSpot = 4,
        LiveBroadcast = 5,
        Blog = 6,
        AskAFriend = 7,
        PhotoGallery = 21,
        Member = 22
    }

    public partial class Comment
    {
        public string WebMemberID { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }

        public static Comment[] GetCommentByObjectIDWithJoin(Int32 objectID, CommentType commentType)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetCommentByObjectIDWithJoin");
            db.AddInParameter(dbCommand, "ObjectID", DbType.Int32, objectID);
            db.AddInParameter(dbCommand, "CommentType", DbType.Int32, (Int32)commentType);

            List<Comment> comments = null;

            using (var dr = db.ExecuteReader(dbCommand))
            {
                comments = Comment.PopulateObject(dr);
                dr.Close();
            }

            return comments.ToArray();
        }

        public static Comment GetComment(Int32 commentID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetCommentWithJoin");
            db.AddInParameter(dbCommand, "CommentID", DbType.String, commentID);

            List<Comment> arr = null;

            // Populate the datareader
            using (var dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.Comment.PopulateCommentWithJoin(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }

        public static Int32[] GetCommentIDs(Int32 objectID, Int32 lastCommentID, CommentType commentType)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetCommentIDs");
            db.AddInParameter(dbCommand, "objectID", DbType.Int32, objectID);
            db.AddInParameter(dbCommand, "lastCommentID", DbType.Int32, lastCommentID);
            db.AddInParameter(dbCommand, "commentType", DbType.Int32, (Int32)commentType);

            var commentIDs = new List<Int32>();

            // Populate the datareader
            using (var dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    commentIDs.Add(Convert.ToInt32(dr["CommentID"]));
                dr.Close();
            }

            return commentIDs.ToArray();
        }

        public static Boolean HasNewComments(Int32 objectID, Int32 lastCommentID, CommentType commentType)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_HasNewComments");
            db.AddInParameter(dbCommand, "objectID", DbType.Int32, objectID);
            db.AddInParameter(dbCommand, "lastCommentID", DbType.Int32, lastCommentID);
            db.AddInParameter(dbCommand, "commentType", DbType.Int32, (Int32)commentType);
            return Convert.ToInt32(db.ExecuteScalar(dbCommand)) > 0;
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of Comments
        /// </summary>
        public static List<Comment> PopulateCommentWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<Comment> arr = new List<Comment>();

            Comment obj;

            while (dr.Read())
            {
                obj = new Comment();
                if (list.IsColumnPresent("CommentID")) { obj._commentID = (int)dr["CommentID"]; }
                if (list.IsColumnPresent("WebCommentID")) { obj._webCommentID = (string)dr["WebCommentID"]; }
                if (list.IsColumnPresent("ObjectID")) { obj._objectID = (int)dr["ObjectID"]; }
                if (list.IsColumnPresent("MemberIDFrom")) { obj._memberIDFrom = (int)dr["MemberIDFrom"]; }
                if (list.IsColumnPresent("InReplyToCommentID")) { obj._inReplyToCommentID = (int)dr["InReplyToCommentID"]; }
                if (list.IsColumnPresent("Text")) { obj._text = (string)dr["Text"]; }
                if (list.IsColumnPresent("IsDeleted")) { obj._isDeleted = (bool)dr["IsDeleted"]; }
                if (list.IsColumnPresent("Path")) { obj._path = (string)dr["Path"]; }
                if (list.IsColumnPresent("CommentType")) { obj._commentType = (int)dr["CommentType"]; }
                if (list.IsColumnPresent("DTCreated")) { obj._dTCreated = (DateTime)dr["DTCreated"]; }
                if (list.IsColumnPresent("ThreadNo")) { obj._threadNo = (int)dr["ThreadNo"]; }
                if (list.IsColumnPresent("WebMemberID")) { obj.WebMemberID = (string)dr["WebMemberID"]; }
                if (list.IsColumnPresent("NickName")) { obj.NickName = (string)dr["NickName"]; }
                if (list.IsColumnPresent("PhotoUrl")) { obj.PhotoUrl = (string)dr["PhotoUrl"]; }


                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        public static void DeleteComment(string WebCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteComment");            
            db.AddInParameter(dbCommand, "WebCommentID", DbType.String, WebCommentID);
            
            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Comment GetCommentByWebCommentID(string WebCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetCommentByWebCommentID");
            db.AddInParameter(dbCommand, "WebCommentID", DbType.String, WebCommentID);

            List<Comment> arr = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.Comment.PopulateCommentWithJoin(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }
    }
}

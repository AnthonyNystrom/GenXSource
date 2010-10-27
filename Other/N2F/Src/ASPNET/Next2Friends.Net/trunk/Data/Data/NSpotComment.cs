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
    public partial class NSpotComment
    {

        /// <summary>
        /// gets all the comments for an AskAFriend Question
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <returns></returns>
        public static AjaxAAFComment[] GetAAFCommentsByWebAskAFriendIDWithJoin(string WebAskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAAFCommentsByWebAskAFriendIDWithJoin");
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, WebAskAFriendID);

            //execute the stored procedure
            AjaxAAFComment[] AskAFriendComments = new AjaxAAFComment[0];

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                AskAFriendComments = AjaxAAFComment.PopulateAjaxComment(dr);
                dr.Close();
            }

            return AskAFriendComments;
        }

        /// <summary>
        /// gets all the comments for an AskAFriend Question since the last request
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <returns></returns>
        public static AjaxAAFComment[] GetAAFCommentsByWebAskAFriendSinceLastIDWithJoin(string WebAskAFriendID, string LastWebAskAFriendCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAAFCommentsByWebAskAFriendSinceLastIDWithJoin");
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, WebAskAFriendID);
            db.AddInParameter(dbCommand, "LastWebAskAFriendCommentID", DbType.String, LastWebAskAFriendCommentID);

            //execute the stored procedure
            AjaxAAFComment[] AskAFriendComments = new AjaxAAFComment[0];

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                AskAFriendComments = AjaxAAFComment.PopulateAjaxComment(dr);
                dr.Close();
            }
            return AskAFriendComments;
        }


        public static int GetNumberOfNewComments(string WebAskAFriendID, string LastWebAskAFriendCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNumberOfNewComments");
            db.AddInParameter(dbCommand, "WebAskAFriendID", DbType.String, WebAskAFriendID);
            db.AddInParameter(dbCommand, "LastWebAskAFriendCommentID", DbType.String, LastWebAskAFriendCommentID);

            int NumberOfNewComments = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    NumberOfNewComments = (int)dr[0];
                }

                dr.Close();
            }

            return NumberOfNewComments;
        }
    }
}

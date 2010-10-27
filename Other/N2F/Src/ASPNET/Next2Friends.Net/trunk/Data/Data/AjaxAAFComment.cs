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
    /// <summary>
    /// Summary description for AjaxComment
    /// </summary>
    public class AjaxAAFComment
    {
        public string WebCommentID { get; set; }
        public string WebMemberID { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }
        public string Text { get; set; }
        public string DateTimePosted { get; set; }
        public DateTime DTCreated { get; set; }
        public string HTML { get { return ToHTML(); } }
        public string TotalNumberOfComments { get { return ToHTML(); } }

        public AjaxAAFComment()
        {

        }

        /// <summary>
        /// gets all the comments for an AskAFriend Question
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <returns></returns>
        public static AjaxAAFComment[] GetAAFCommentsByAskAFriendIDWithJoin(int AskAFriendID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAAFCommentsByAskAFriendIDWithJoin");
            db.AddInParameter(dbCommand, "AskAFriendID", DbType.Int32, AskAFriendID);

            //execute the stored procedure
            AjaxAAFComment[] ajaxComments = PopulateAjaxComment(db.ExecuteReader(dbCommand));

            return ajaxComments;
        }

        /// <summary>
        /// Gets all the comments for an AskAFriend Question.
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <exception cref="ArgumentException">
        /// If there is not AAF Comment with the specified <code>AskAFriendCommentID</code>.
        /// </exception>
        public static AjaxAAFComment GetAAFCommentByAAFCommentIDWithJoin(int AskAFriendCommentID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetAAFCommentByAAFCommentIDWithJoin");
            db.AddInParameter(dbCommand, "AskAFriendCommentID", DbType.Int32, AskAFriendCommentID);

            var ajaxComments = PopulateAjaxComment(db.ExecuteReader(dbCommand));

            if (ajaxComments.Length > 0)
                return ajaxComments[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidAskQuestionCommentID, AskAFriendCommentID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static AjaxAAFComment[] PopulateAjaxComment(IDataReader dr)
        {
            List<AjaxAAFComment> commentArr = new List<AjaxAAFComment>();
            AjaxAAFComment ajaxComment;

            while (dr.Read())
            {
                ajaxComment = new AjaxAAFComment();

                ajaxComment.WebCommentID = (string)dr["WebCommentID"];
                ajaxComment.WebMemberID = (string)dr["WebMemberID"];
                ajaxComment.NickName = (string)dr["NickName"];
                ajaxComment.PhotoUrl = "user/" + (string)dr["PhotoUrl"];
                ajaxComment.Text = (string)dr["Text"];
                ajaxComment.DateTimePosted = TimeDistance.TimeAgo((DateTime)dr["DTCreated"]);
                ajaxComment.DTCreated = (DateTime)dr["DTCreated"];

                commentArr.Add(ajaxComment);
            }

            return commentArr.ToArray();
        }


        /// <summary>
        /// Converts the object into HTML for rendering in browser
        /// </summary>
        /// <returns></returns>
        public string ToHTML()
        {
            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[5];

            parameters[0] = ParallelServer.Get(this.PhotoUrl) + this.PhotoUrl;
            parameters[1] = this.NickName;
            parameters[2] = this.DateTimePosted;
            parameters[3] = this.Text.Replace("\n", "<br/>").Replace("\r", "<br/>");
            parameters[4] = this.WebMemberID;

            sbHTML.AppendFormat(@"<li class='clearfix'><a href='view.aspx?m={4}'>
								<img src='{0}' width='73' class='commenter_avatar' /></a>
								<div class='comment_entry'>
									<p class='commenter'>
										<cite><a href='view.aspx?m={4}'>{1}</a></cite><br />
										<small>{2}</small><br />
									</p>
									<p>{3}</p>							
								</div>
							</li>", parameters);

            //return SafeHTML.FormatForHTML(sbHTML.ToString());
            return sbHTML.ToString();
        }
    }
}
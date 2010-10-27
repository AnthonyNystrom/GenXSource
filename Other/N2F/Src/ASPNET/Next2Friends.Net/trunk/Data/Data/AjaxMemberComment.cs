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
    public class AjaxComment
    {
        public string WebMemberID { get; set; }
        public string WebCommentID { get; set; }
        public int InReplyToCommentID { get; set; }
        public string WebPhotoID { get; set; }

        public string WebObjectID { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }
        public string Text { get; set; }
        public string DateTimePosted { get; set; }
        public string HTML { get { return ToHTML(); } }
        public string TotalNumberOfComments { get; set; }
        public DateTime DTCreated { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
        public bool SentFromMobile { get; set; }
        public string CommentHTML { get; set; }


        // Used in HTML generation
        public string Type { get; set; }
        public int Depth { get; set; }        


        public AjaxComment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static AjaxComment[] GetCommentByObjectIDWithJoin(int ObjectID, int CommentType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetCommentByObjectIDWithJoin");
            db.AddInParameter(dbCommand, "ObjectID", DbType.Int32, ObjectID);
            db.AddInParameter(dbCommand, "CommentType", DbType.Int32, CommentType);

            AjaxComment[] ajaxComments = new AjaxComment[0];

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ajaxComments = PopulateAjaxComment(dr);
            }

            return ajaxComments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <returns></returns>
        public static AjaxComment GetAjaxCommentByCommentIDWithJoin(int CommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetAjaxCommentByCommentIDWithJoin");
            db.AddInParameter(dbCommand, "CommentID", DbType.Int32, CommentID);

            AjaxComment[] ajaxComments = new AjaxComment[0];

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ajaxComments = PopulateAjaxComment(db.ExecuteReader(dbCommand));
            }

            if (ajaxComments.Length > 0)
                return ajaxComments[0];
            else
                throw new ArgumentException(String.Format(Properties.Resources.Argument_InvalidBlogCommentID, CommentID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static int GetNumberOfCommentByObjectID(int ObjectID, int CommentType)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNumberOfCommentByObjectID");
            db.AddInParameter(dbCommand, "ObjectID", DbType.Int32, ObjectID);
            db.AddInParameter(dbCommand, "CommentType", DbType.Int32, CommentType);

            int NumberOfComments = 0;

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    NumberOfComments = (int)dr[0];
                }

            }

            return NumberOfComments;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NSpotID"></param>
        /// <returns></returns>
        public static AjaxComment[] GetNSpotCommentByNSpotIDWithJoin(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotCommentByNSpotIDWithJoin");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            AjaxComment[] ajaxComments = new AjaxComment[0];

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                ajaxComments = PopulateAjaxComment(db.ExecuteReader(dbCommand));
            }

            return ajaxComments;
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberCommentID"></param>
        /// <returns></returns>
        public static AjaxComment GetAjaxNSpotCommentByNSpotCommentIDWithJoin(int NSpotCommentID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetAjaxNSpotCommentByNSpotCommentIDWithJoin");
            db.AddInParameter(dbCommand, "NSpotCommentID", DbType.Int32, NSpotCommentID);

            var ajaxComments = new AjaxComment[0];

            using (var dr = db.ExecuteReader(dbCommand))
            {
                ajaxComments = PopulateAjaxComment(db.ExecuteReader(dbCommand));
            }

            if (ajaxComments.Length > 0)
                return ajaxComments[0];
            else
                throw new Exception(String.Format(Properties.Resources.Argument_InvalidNSpotCommentID, NSpotCommentID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static AjaxComment[] PopulateAjaxComment(IDataReader dr)
        {
            List<AjaxComment> commentArr = new List<AjaxComment>();
            AjaxComment ajaxComment;

            ColumnFieldList list = new ColumnFieldList(dr);

            while (dr.Read())
            {
                ajaxComment = new AjaxComment();
                
                ajaxComment.WebMemberID = (string)dr["WebMemberID"];                
                ajaxComment.NickName = (string)dr["NickName"];
                ajaxComment.PhotoUrl = ParallelServer.Get((string)dr["PhotoUrl"]) + "user/" + (string)dr["PhotoUrl"];
                ajaxComment.Text = (string)dr["Text"];
                ajaxComment.SentFromMobile = ((int)dr["SentFromMobile"]) == 1 ? true : false;
                ajaxComment.DateTimePosted = TimeDistance.TimeAgo((DateTime)dr["DTCreated"]);
                ajaxComment.DTCreated = (DateTime)dr["DTCreated"];

                if (list.IsColumnPresent("WebCommentID")) { ajaxComment.WebCommentID = (string)dr["WebCommentID"]; }
                if (list.IsColumnPresent("Path")) 
                { 
                    ajaxComment.Path = (string)dr["Path"];
                    ajaxComment.Depth = GetDepth(ajaxComment.Path);
                }
                if (list.IsColumnPresent("IsDeleted")) { ajaxComment.IsDeleted = (bool)dr["IsDeleted"]; }
                
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
            if (this.WebCommentID == null)
                return null;

            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[10];

            parameters[0] = this.PhotoUrl;
            parameters[1] = this.NickName;
            parameters[2] = this.DateTimePosted;
            parameters[3] = Next2Friends.Misc.SafeHTML.AutoLink(Next2Friends.Misc.SafeHTML.FormatForHTML(this.Text));
            parameters[4] = this.WebMemberID;
            parameters[5] = this.WebCommentID;
            parameters[6] = Type;
            parameters[7] = Next2Friends.Misc.SafeHTML.FormatForText(this.Text);
            parameters[8] = WebObjectID;

            if (this.SentFromMobile)
            {
                parameters[9] = "<p> Sent from my mobile </p>";
            }
            else
            {
                parameters[9] = "";
            }


            if (Type == null || Type == "")
            {
                sbHTML.AppendFormat(@"<li class='clearfix' id='li{5}'><img src='{0}' alt='' width='50' height='50' class='commenter_avatar' />
							<div class='comment_entry' id='{5}'>								
								<p class='commenter'> <cite><a href='view.aspx?m={4}'>{1}</a></cite><br />
								<small>{2}</small></p>
								<p id='commentBody{5}'>{3}{9}</p>                                
							</div>

						</li>", parameters);
            }

            else
            {
                sbHTML.AppendFormat(@"<li class=""clearfix"" id=""li{5}""><img src=""{0}"" alt="""" width=""50"" height=""50"" class=""commenter_avatar"" />
							<div class=""comment_entry"" id=""{5}"">
								<p class=""reply""><small><a id=""edit{5}"" href=""javascript:editComment('{6}','{5}','{8}');void(0);"">Edit</a><a  id=""delete{5}"" href=""javascript:deleteComment('{6}','{5}','{8}');"">Delete</a></small><a id=""reply{5}"" href=""javascript:replyToComment('{6}','{5}','{8}');void(0);"">Reply</a></p>
								<p class=""commenter""> <cite><a href=""view.aspx?m={4}"">{1}</a></cite><br />
								<small>{2}</small></p>
								<p id=""commentBody{5}"">{3}{9}</p>
                                <p id=""commentBodyHidden{5}"" style=""visibility:hidden;display:none"">{7}.</p>
							</div>

						</li>", parameters);
            }


            return sbHTML.ToString();
        }

        private static int GetDepth(string s)
        {
            return s.Length - s.Replace("/", "").Length;
        }
    }
}
using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class PhotoComment
    {
        /// <summary>
        /// Gets all the PhotoComment in the database 
        /// </summary>
        public static List<AjaxComment> GetPhotoCommentsByGalleryID(int PhotoCollectionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoCommentsByGalleryID");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, PhotoCollectionID);

            List<AjaxComment> commentArr = new List<AjaxComment>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                while (dr.Read())
                {
                    AjaxComment ajaxComment = new AjaxComment();

                    ajaxComment.WebMemberID = (string)dr["WebMemberID"];
                    ajaxComment.WebPhotoID = (string)dr["WebPhotoID"];
                    ajaxComment.NickName = (string)dr["NickName"];
                    ajaxComment.PhotoUrl = ParallelServer.Get((string)dr["PhotoUrl"]) + "user/" + (string)dr["PhotoUrl"];
                    ajaxComment.Text = (string)dr["Text"];
                    ajaxComment.DateTimePosted = TimeDistance.TimeAgo((DateTime)dr["DTCreated"]);

                    commentArr.Add(ajaxComment);
                }


                dr.Close();
            }

            // Create the object array from the datareader
            return commentArr;
        }

        public static string PhotoCommentsToJsArray(List<AjaxComment> PhotoComments)
        {
            StringBuilder sbCommentArray = new StringBuilder();

            sbCommentArray.AppendFormat(@"var comments = new Array();");


            string LastWebPhotoID = string.Empty;
            int NextIndex = -1;
            int NumberOfComments = 0;

            // loop through the comments adding a sub arrary each time
            for (int i = 0; i < PhotoComments.Count; i++)
            {
                bool CloseArray = false;

                if (LastWebPhotoID != PhotoComments[i].WebPhotoID)
                {
                    NextIndex++;
                    LastWebPhotoID = PhotoComments[i].WebPhotoID;
                    sbCommentArray.AppendFormat(@"comments[" + NextIndex + "] = new Array('" + LastWebPhotoID + "',\" ");
                    NumberOfComments = 0;
                }

                NumberOfComments++;

                sbCommentArray.AppendFormat(@"{0}", PhotoComments[i].HTML.Replace("\"", "x").Replace("\r\n", ""));

                if (i < PhotoComments.Count-1)
                {
                    if (LastWebPhotoID != PhotoComments[i + 1].WebPhotoID)
                    {
                        CloseArray = true;
                    }
                }
                else
                {
                    CloseArray = true;
                }

                if (CloseArray)
                {
                    sbCommentArray.AppendFormat(@" ""," + NumberOfComments + ");");
                }
            }

            //sbCommentArray.AppendFormat(@");");

            return sbCommentArray.ToString();
        }

      
    }
}

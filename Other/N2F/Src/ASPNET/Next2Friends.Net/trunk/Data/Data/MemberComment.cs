using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class MemberComment
    {
        public string WebMemberID { get; set; }
        public string NickName { get; set; }
        public string PhotoUrl { get; set; }

        //public static List<BlogComment> GetBlogCommentsByWebBlogEntryID(int BlogEntryID)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();

        //    DbCommand dbCommand = db.GetStoredProcCommand("HG_GetBlogCommentByBlogEntryID");
        //    db.AddInParameter(dbCommand, "BlogEntryID", DbType.Int32, BlogEntryID);

        //    List<BlogComment> arr = null;

        //    // Populate the datareader
        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        // Call the PopulateObject method passing the datareader to return the object array
        //        arr = Next2Friends.Data.BlogComment.PopulateBlogCommentWithJoin(dr);
        //        dr.Close();
        //    }

        //    return arr;
        //}

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of MemberComments
        /// </summary>
        public static List<MemberComment> PopulateMemberCommentWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<MemberComment> arr = new List<MemberComment>();

            MemberComment obj;

            while (dr.Read())
            {
                obj = new MemberComment();
                if (list.IsColumnPresent("MemberCommentID")) { obj._memberCommentID = (int)dr["MemberCommentID"]; }
                if (list.IsColumnPresent("MemberID")) { obj._memberID = (int)dr["MemberID"]; }
                if (list.IsColumnPresent("MemberIDFrom")) { obj._memberIDFrom = (int)dr["MemberIDFrom"]; }
                if (list.IsColumnPresent("Text")) { obj._text = (string)dr["Text"]; }
                if (list.IsColumnPresent("DTCreated")) { obj._dTCreated = (DateTime)dr["DTCreated"]; }
                
                if (list.IsColumnPresent("WebMemberID")) { obj.WebMemberID = (string)dr["WebMemberID"]; }
                if (list.IsColumnPresent("NickName")) { obj.NickName = (string)dr["NickName"]; }
                if (list.IsColumnPresent("PhotoUrl")) { obj.PhotoUrl = (string)dr["PhotoUrl"]; }


                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        public static void DeleteMemberComment(string WebMemberCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteMemberComment");
            db.AddInParameter(dbCommand, "WebMemberCommentID", DbType.String, WebMemberCommentID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MemberComment GetMemberCommentByWebMemberCommentID(string WebMemberCommentID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberCommentByWebMemberCommentID");
            db.AddInParameter(dbCommand, "WebMemberCommentID", DbType.String, WebMemberCommentID);

            List<MemberComment> arr = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.MemberComment.PopulateMemberCommentWithJoin(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }
    }
}

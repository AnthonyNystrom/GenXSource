using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class BlogEntry
    {
        public static void DeleteBlogEntry(Int32 blogEntryID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteBlogEntry");
            db.AddInParameter(dbCommand, "id", DbType.Int32, blogEntryID);
            db.ExecuteNonQuery(dbCommand);
        }

        public static BlogEntry GetBlogEntry(Int32 blogEntryID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetBlogEntry");
            db.AddInParameter(dbCommand, "entryID", DbType.Int32, blogEntryID);

            List<BlogEntry> BlogEntList = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                BlogEntList = BlogEntry.PopulateObject(dr);
                dr.Close();
            }

            if (BlogEntList.Count > 0)
                return BlogEntList[0];
            else
                return null;
        }

        /// <summary>
        /// Calls the database and gets all the BlogEntry objects for this Member
        /// </summary>
        public static BlogEntry GetBlogEntryByWebBlogEntryID(string WebBlogEntryID)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_GetBlogEntryByWebBlogEntryID");
            db.AddInParameter(dbCommand, "WebBlogEntryID", DbType.String, WebBlogEntryID);

            List<BlogEntry> arr = null;

            // Populate the datareader
            using (var dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.BlogEntry.PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else
                return null;
        }

        public static Int32[] GetBlogEntryIDs(Int32 memberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetBlogEntryIDs");
            db.AddInParameter(dbCommand, "memberID", DbType.Int32, memberID);

            IList<Int32> entryIDs = new List<Int32>();

            /* Execute the stored procedure. */
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                while (dr.Read())
                    entryIDs.Add(Convert.ToInt32(dr["BlogEntryID"]));
                dr.Close();
            }

            Int32[] result = new Int32[entryIDs.Count];
            entryIDs.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Gets the blog entries for the mobile application.
        /// </summary>
        public static BlogEntry[] GetMobileBlogEntry(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMobileBlogEntry");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<BlogEntry> BlogEntList = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                BlogEntList = BlogEntry.PopulateObject(dr);
                dr.Close();
            }

            return BlogEntList.ToArray();
        }


        public static List<BlogEntry> GetBlogEntryByMemberID(string WebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetBlogEntryByMemberID");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);

            List<BlogEntry> BlogEntList = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                BlogEntList = BlogEntry.PopulateObject(dr);
                dr.Close();
            }

            return BlogEntList;
        }
    }
}


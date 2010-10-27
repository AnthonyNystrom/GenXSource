using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace Next2Friends.Data
{
    public partial class ContentView
    {
        public string WebMemberID { get; set; }
        public string PhotoURL { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void SaveWithCheck()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveContentViewWithCheck");

            db.AddInParameter(dbCommand, "ContentViewID", DbType.Int32, ContentViewID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "ObjectID", DbType.Int32, ObjectID);
            db.AddInParameter(dbCommand, "ObjectType", DbType.Int32, ObjectType);
            db.AddInParameter(dbCommand, "DTCreated", DbType.DateTime, DTCreated);

            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {

                // get the returned ID
                if (dr.Read())
                {
                    int ID = Int32.Parse(dr[0].ToString());
                    //if the ID is NOT zero then the query was an insert
                    if (ID != 0)
                        this.ContentViewID = ID;
                }

                dr.Close();
            }

        }

        /// <summary>
        /// Calls the database and gets all the ContentView objects for this Member
        /// </summary>
        public static List<ContentView> GetMemberProfileViews(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetMemberProfileViews");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<ContentView> arr = null;

            // Populate the datareader
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                // Call the PopulateObject method passing the datareader to return the object array
                arr = Next2Friends.Data.ContentView.PopulateContentViewObject(dr);
                dr.Close();
            }

            return arr;
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of ContentViews
        /// </summary>
        public static List<ContentView> PopulateContentViewObject(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<ContentView> arr = new List<ContentView>();

            ContentView obj;

            while (dr.Read())
            {
                obj = new ContentView();
                if (list.IsColumnPresent("WebMemberID")) { obj.WebMemberID = (string)dr["WebMemberID"]; }
                if (list.IsColumnPresent("PhotoURL")) { obj.PhotoURL = (string)dr["PhotoURL"]; }
                if (list.IsColumnPresent("NickName")) { obj.NickName = (string)dr["NickName"]; }
                if (list.IsColumnPresent("FirstName")) { obj.FirstName = (string)dr["FirstName"]; }
                if (list.IsColumnPresent("LastName")) { obj.LastName = (string)dr["LastName"]; }
                if (list.IsColumnPresent("DTCreated")) { obj.DTCreated = (DateTime)dr["DTCreated"]; }

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }
    }
}
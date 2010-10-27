using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;

namespace Next2Friends.Data
{
    public partial class FriendTag
    {
        public int MemberID { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WebMemberID { get; set; }
        public string ProfilePhotoURL { get; set; }
        public string CountryName { get; set; }

        public static List<FriendTag> GetProximityTags(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetProximityTagByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.String, MemberID);

            List<FriendTag> FriendTags = new List<FriendTag>();


            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                FriendTags = PopulateFriendTagWithJoin(dr);
                dr.Close();
            }

            return FriendTags;
        }

        public static List<FriendTag> PopulateFriendTagWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<FriendTag> arr = new List<FriendTag>();

            FriendTag obj;

            while (dr.Read())
            {
                obj = new FriendTag();

                if (list.IsColumnPresent("MemberID")) { obj.MemberID = (int)dr["MemberID"]; }
                if (list.IsColumnPresent("WebMemberID")) { obj.WebMemberID = (string)dr["WebMemberID"]; }
                if (list.IsColumnPresent("CountryName")) { obj.CountryName = (string)dr["CountryName"]; }
                if (list.IsColumnPresent("NickName")) { obj.NickName = (string)dr["NickName"]; }
                if (list.IsColumnPresent("FirstName")) { obj.FirstName = (string)dr["FirstName"]; }
                if (list.IsColumnPresent("LastName")) { obj.LastName = (string)dr["LastName"]; }
                if (list.IsColumnPresent("ProfilePhotoURL")) { obj.ProfilePhotoURL = (string)dr["ProfilePhotoURL"]; }
                if (list.IsColumnPresent("TaggedDT")) { obj.TaggedDT = (DateTime)dr["TaggedDT"]; }
                if (list.IsColumnPresent("CreatedDT")) { obj._createdDT = (DateTime)dr["CreatedDT"]; }

                arr.Add(obj);
            }

            return arr;
        }


		public void SaveWithFriendRequest()
		{
            db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_SaveWithFriendRequest");

			db.AddInParameter(dbCommand, "FriendTagID", DbType.Int32, FriendTagID);
			db.AddInParameter(dbCommand, "FirstMemberID", DbType.Int32, FirstMemberID);
			db.AddInParameter(dbCommand, "TagValidationString", DbType.String, TagValidationString);
			db.AddInParameter(dbCommand, "TaggedDT", DbType.DateTime, TaggedDT);
			db.AddInParameter(dbCommand, "SecondMemberID", DbType.Int32, SecondMemberID);
			db.AddInParameter(dbCommand, "CreatedDT", DbType.DateTime, CreatedDT);
            db.AddInParameter(dbCommand, "WebFriendRequestID1", DbType.String, UniqueID.NewWebID());
            db.AddInParameter(dbCommand, "WebFriendRequestID2", DbType.String, UniqueID.NewWebID());

			using (IDataReader dr = db.ExecuteReader(dbCommand))
			{

				if(dr.Read())
				{
					int ID = Int32.Parse(dr[0].ToString());
					//if the ID is NOT zero then the query was an insert
					if(ID!=0)
						this.FriendTagID = ID;
				}
	
				dr.Close();			
            }
		}  
    }
}

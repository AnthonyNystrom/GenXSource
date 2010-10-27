using System;
using System.Drawing;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class LiveBroadcast
    {

        public string ThumbnailURL { get; set; }
        public Member Member { get; set; }

        /// <summary>
        /// Gets the top 10 archived Broadcasts
        /// </summary>
        public static List<LiveBroadcast> GetArchivedBroadcasts()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetArchivedBroadcasts");

            List<LiveBroadcast> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        public static LiveBroadcast GetLiveBroadcastByMemberID(string WebMemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetLiveBroadcastByMemberID");
            db.AddInParameter(dbCommand, "WebMemberID", DbType.String, WebMemberID);

            List<LiveBroadcast> arr = new List<LiveBroadcast>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
            {
                return arr[0];
            }
            else
            {
                return null;
            }
        }

        public static LiveBroadcast GetPrivateLiveBroadcastByMemberID(string FriendWebMemberID,int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPrivateLiveBroadcastByMemberID");
            db.AddInParameter(dbCommand, "FriendWebMemberID", DbType.String, FriendWebMemberID);
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<LiveBroadcast> arr = new List<LiveBroadcast>();

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
            {
                return arr[0];
            }
            else
            {
                return null;
            }
        }

        


        

        /// <summary>
        /// Gets all the LiveBroadcast in the database 
        /// </summary>
        public static List<LiveBroadcast> GetAllLiveBroadcastNOW2()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetLiveBroadcasts2");

            List<LiveBroadcast> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateLiveBroadcastWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        /// <summary>
        /// Gets all the LiveBroadcast in the database 
        /// </summary>
        public static List<LiveBroadcast> GetAllLiveBroadcastNOW()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetLiveBroadcasts");

            List<LiveBroadcast> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return arr;
        }

        /// <summary>
        /// Sets the thumbnail for a Video
        /// </summary>
        /// <param name="Thumbnail"></param>
        public void SetLiveThumbnail(Image Thumbnail)
        {
            Image Resized = Photo.ResizeTo124x91(Thumbnail);
            Member member = new Member(this.MemberID);

            ResourceFile thumbnailRes = new ResourceFile();
            thumbnailRes.WebResourceFileID = Misc.UniqueID.NewWebID();
            thumbnailRes.ResourceType = (int)ResourceFileType.VideoThumbnail;
            thumbnailRes.FileName = member.NickName + "/" + thumbnailRes.WebResourceFileID + ".jpg";

            string ThumbnailSaveLocation = member.NickName + @"\vthmb\" + thumbnailRes.WebResourceFileID + ".jpg";

            Photo.SaveToDisk(Resized, ThumbnailSaveLocation);

            thumbnailRes.Save();

            //need to add this to the LiveBroadcast table
            this.ThumbnailResourceFileID = thumbnailRes.ResourceFileID;
            this.Save();

        }

         /// <summary>
        /// Gets all the LiveBroadcast in the database 
        /// </summary>
        public static LiveBroadcast GetLiveBroadcastByWebLiveBroadcastID(string WebLiveBroadcastID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetLiveBroadcastByWebLiveBroadcastID");
            db.AddInParameter(dbCommand, "WebLiveBroadcastID", DbType.String, WebLiveBroadcastID);

            List<LiveBroadcast> arr = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                arr = PopulateObject(dr);
                dr.Close();
            }

            if (arr.Count > 0)
                return arr[0];
            else 
                return null;

        }


        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of LiveBroadcasts
        /// </summary>
        public static List<LiveBroadcast> PopulateLiveBroadcastWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<LiveBroadcast> arr = new List<LiveBroadcast>();

            LiveBroadcast obj;

            while (dr.Read())
            {
                obj = new LiveBroadcast();
                if (list.IsColumnPresent("LiveBroadcastID")) { obj._liveBroadcastID = (int)dr["LiveBroadcastID"]; }
                if (list.IsColumnPresent("WebLiveBroadcastID")) { obj._webLiveBroadcastID = (string)dr["WebLiveBroadcastID"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileID")) { obj._thumbnailResourceFileID = (int)dr["ThumbnailResourceFileID"]; }
                if (list.IsColumnPresent("MemberID")) { obj._memberID = (int)dr["MemberID"]; }
                if (list.IsColumnPresent("BroadcastSource")) { obj._broadcastSource = (int)dr["BroadcastSource"]; }
                if (list.IsColumnPresent("Title")) { obj._title = (string)dr["Title"]; }
                if (list.IsColumnPresent("Description")) { obj._description = (string)dr["Description"]; }
                if (list.IsColumnPresent("DTStart")) { obj._dTStart = (DateTime)dr["DTStart"]; }
                if (list.IsColumnPresent("DTEnd")) { obj._dTEnd = (DateTime)dr["DTEnd"]; }
                if (list.IsColumnPresent("Duration")) { obj._duration = (int)dr["Duration"]; }
                if (list.IsColumnPresent("LiveNow")) { obj._liveNow = (bool)dr["LiveNow"]; }

                if (list.IsColumnPresent("ThumbnailURL")) { obj.ThumbnailURL = (string)dr["ThumbnailURL"]; }

                obj.Member = new Member();
                if (list.IsColumnPresent("Nickname")) { obj.Member.NickName = (string)dr["Nickname"]; }
                if (list.IsColumnPresent("ISOCountry")) { obj.Member.ISOCountry = (string)dr["ISOCountry"]; }



                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }
        
        
    }
}

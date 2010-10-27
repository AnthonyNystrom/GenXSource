using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum OrderByType { Featured=1, Viewed=2, Discussed=3, Rated=4 }
    
    public enum PrivacyType { Public = 1, Network = 2 }

    public partial class Video
    {
        public string ShortDescription
        {
            get
            {
                if (this.Description.Length > 200)
                    return this.Description.Substring(0, 200) + "..";
                else
                    return this.Description;
            }

        }

        public string VeryShortDescription
        {
            get
            {
                if (this.Description.Length > 100)
                    return this.Description.Substring(0, 98) + "..";
                else
                    return this.Description;
            }

        }

        public string ShortTitle
        {
            get
            {
                if (this.Title.Length > 40)
                    return this.Title.Substring(0, 40) + "..";
                else
                    return this.Title;
            }

        }

        public string VeryShortTitle
        {
            get
            {
                if (this.Title.Length > 20)
                    return this.Title.Substring(0, 18) + "..";
                else
                    return this.Title;
            }

        }


        /// <summary>
        /// Searches the database through title
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static List<Video> SearchVideoByKeyword(string Keyword)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetVideoByKeywordSearch");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);

            List<Video> VideosByFeature;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                VideosByFeature = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return VideosByFeature;
        }


        public static void IncreaseWatchedCount(string WebVideoID, int MemberID, string IPAddress)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_IncreaseWatchedCount");
            db.AddInParameter(dbcommand, "WebVideoID", DbType.String, WebVideoID);
            db.AddInParameter(dbcommand, "MemberID", DbType.Int16, MemberID);
            db.AddInParameter(dbcommand, "IPAddress", DbType.String, IPAddress);


            try
            {
                db.ExecuteNonQuery(dbcommand);
            }
            catch { }
        }

        /// <summary>
        /// Get the latest archived video broadcasts
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static List<Video> GetArchivedBroadcasts()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetArchivedBroadcasts");

            List<Video> ArchivedVideos;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                ArchivedVideos = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return ArchivedVideos;
        }

        /// <summary>
        /// This method is incomplete and needs to also delete the ResourceFile
        /// </summary>
        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeleteVideoByVideoID");
            db.AddInParameter(dbCommand, "VideoID", DbType.Int32, VideoID);

            //execute the stored procedure
            db.ExecuteNonQuery(dbCommand);
        }


        /// <summary>
        /// Gets the video to display on the home page
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static Video GetHomePageVideo()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetHomePageVideo");

            List<Video> video;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                video = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            if (video.Count > 0)
                return video[0];
            else
                return null;


                
        }

        public static List<Video> GetTop100Videos(OrderByType topVideoType)
        {
            string OrderByClause = string.Empty;

            switch (topVideoType)
            {
                case OrderByType.Featured:
                    OrderByClause = "Featured";
                    break;
                case OrderByType.Viewed:
                    OrderByClause = "NumberOfViews";
                    break;
                case OrderByType.Discussed:
                    OrderByClause = "NumberOfComments";
                    break;
                case OrderByType.Rated:
                    OrderByClause = "TotalVoteScore";
                    break;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetTop100Videos");
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, OrderByClause);

            List<Video> VideosByFeature;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                VideosByFeature = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return VideosByFeature;
        }

        public static List<Video> GetVideosByTag(OrderByType topVideoType, string Tag)
        {
            string OrderByClause = string.Empty;

            switch (topVideoType)
            {
                case OrderByType.Featured:
                    OrderByClause = "Latest";
                    break;
                case OrderByType.Viewed:
                    OrderByClause = "NumberOfViews";
                    break;
                case OrderByType.Discussed:
                    OrderByClause = "NumberOfComments";
                    break;
                case OrderByType.Rated:
                    OrderByClause = "TotalVoteScore";
                    break;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetVideosByTag");
            db.AddInParameter(dbcommand, "Tag", DbType.String, Tag);
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, OrderByClause);

            List<Video> VideosByTag;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                VideosByTag = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return VideosByTag;
        }

        public static List<Video> GetVideosByCategoryID(OrderByType topVideoType, int CategoryID)
        {
            string OrderByClause = string.Empty;

            switch (topVideoType)
            {
                case OrderByType.Featured:
                    OrderByClause = "Latest";
                    break;
                case OrderByType.Viewed:
                    OrderByClause = "NumberOfViews";
                    break;
                case OrderByType.Discussed:
                    OrderByClause = "NumberOfComments";
                    break;
                case OrderByType.Rated:
                    OrderByClause = "TotalVoteScore";
                    break;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetVideosByCategory");
            db.AddInParameter(dbcommand, "CategoryID", DbType.String, CategoryID);
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, OrderByClause);

            List<Video> VideosByCategory;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                VideosByCategory = PopulateObjectWithJoin(dr);
                dr.Close();
            }

            return VideosByCategory;
        }

        /// <summary>
        /// Gets the Video in the database With a full join with all the manually specified tables in SP code 
        /// </summary>
        public static Video GetVideoByWebVideoIDWithJoin(string WebVideoID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetVideoByWebVideoIDWithJoin");
            db.AddInParameter(dbCommand, "WebVideoID", DbType.String, WebVideoID);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader

            if (Videos.Count > 0)
                return Videos[0];
            else
                return null;
                //throw new Exception("There is no video with the WebvideoID " + WebVideoID);
        }

        /// <summary>
        /// 
        /// </summary>
        public static int GetVideoIDByWebVideoID(string WebVideoID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetVideoIDByWebVideoID");
            db.AddInParameter(dbCommand, "WebVideoID", DbType.String, WebVideoID);

            int VideoID = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    VideoID = (int)dr[0];
                }

                dr.Close();
            }

            return VideoID;
        } 

        public static List<Video> GetTopVideosByMemberIDWithJoin(int MemberID,PrivacyType privacyFlag)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetTopVideosByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "PrivacyFlag", DbType.Int32, (int)privacyFlag);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Videos;
        }

        public static List<Video> GetMemberVideosWithJoinOrdered(int MemberID, PrivacyType privacyFlag, string OrderByClause)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetMemberVideosWithJoinOrdered");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "PrivacyFlag", DbType.Int32, (int)privacyFlag);
            db.AddInParameter(dbCommand, "OrderByClause", DbType.String, OrderByClause);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Videos;
        }

        


        public static List<Video> GetVideosByMemberIDWithJoin(int MemberID, PrivacyType PrivacyFlag)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetVideosByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "PrivacyFlag", DbType.Int32, PrivacyFlag);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Videos;
        }


        //public static List<Video> GetVideosByMemberIDWithJoin(int MemberID, PrivacyType privacyFlag)
        //{
        //    //PrivacyType privacyFlag = PrivacyType.Network;

        //    Database db = DatabaseFactory.CreateDatabase();
        //    DbCommand dbCommand = null;

        //    dbCommand = db.GetStoredProcCommand("HG_GetVideosByMemberIDWithJoin");
        //    db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
        //    db.AddInParameter(dbCommand, "PrivacyFlag", DbType.Int32, (int)privacyFlag);

        //    List<Video> Videos = null;

        //    //execute the stored procedure
        //    using (IDataReader dr = db.ExecuteReader(dbCommand))
        //    {
        //        Videos = Video.PopulateObjectWithJoin(dr);
        //        dr.Close();
        //    }

        //    // Create the object array from the datareader
        //    return Videos;
        //}

        public static List<Video> GetFavouriteVideosByMemberIDWithJoin(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetFavouriteVideosByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Videos;
        }


        public static List<Video> GetVideosByMemberIDWithJoinPager(int MemberID, int Page)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetVideosByMemberIDWithJoinPager");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);

            List<Video> Videos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Videos = Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Videos;
        }

       public static int GetVideosCountByMemberID(int MemberID)
       {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetVideosCountByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            int NumberOfVideos = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    NumberOfVideos = (int)dr[0];
                    dr.Close();
                }
            }

            // Create the object array from the datareader
            return NumberOfVideos;
        }
        

        public void UpdateTags()
        {

            char[] sep = { ',' };
            List<VideoTag> tags = this.VideoTag;

            string[] NewTags = this.Tags.Split(sep);

            string FormatedTags = string.Empty;
            List<string> TrimmedTags = new List<string>();
            List<string> DedupedTags = new List<string>();


            //1. Trim all tags
            for (int i = 0; i < NewTags.Length; i++)
            {
                if (NewTags[i].Trim() != string.Empty)
                {
                    TrimmedTags.Add(NewTags[i].Trim());
                }
            }

            //2. dedupe the list
            for (int i = 0; i < TrimmedTags.Count; i++)
            {
                bool Duplicate = false;

                for (int j = 0; j < DedupedTags.Count; j++)
                {
                    if (DedupedTags[j] == TrimmedTags[i])
                    {
                        Duplicate = true;
                    }
                }

                if (!Duplicate)
                {
                    // make sure isnt a blank string
                    if (FormatedTags != string.Empty)
                    {
                        FormatedTags += ", ";
                    }

                    DedupedTags.Add(TrimmedTags[i]);
                    FormatedTags += TrimmedTags[i];
                }

            }


            //3. remove all the tags that arent in the new set
            for (int i = 0; i < tags.Count; i++)
            {
                bool Delete = true;

                for (int j = 0; j < DedupedTags.Count; j++)
                {
                    if (DedupedTags[j] == tags[i].Tag)
                    {
                        Delete = false;
                    }
                }

                if (Delete)
                {
                    tags[i].Delete();
                }
            }


            // 4. insert new tags
            for (int j = 0; j < DedupedTags.Count; j++)
            {
                bool Insert = true;

                for (int i = 0; i < tags.Count; i++)
                {
                    if (DedupedTags[j] == tags[i].Tag)
                    {
                        Insert = false;
                    }
                }


                if (Insert)
                {
                    VideoTag VideoTag = new VideoTag();
                    VideoTag.VideoID = this.VideoID;
                    VideoTag.DTCreated = DateTime.Now;
                    VideoTag.Tag = DedupedTags[j];
                    VideoTag.Save();
                }
            }

            this.Tags = FormatedTags;
        }

        public static List<Video> GetVideosByFullSearch(string Keyword, string ExactKeyword, string WithoutKeyword, string AtLeastKeyword, int CategoryID, string Tag, string OrderByClause, string UploadTime, int Page, int PageSize, out int RowCount)
        {
            RowCount = 1;

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetVideosByFullSearch");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);
            db.AddInParameter(dbcommand, "ExactKeyword", DbType.String, ExactKeyword);
            db.AddInParameter(dbcommand, "WithoutKeyword", DbType.String, WithoutKeyword);
            db.AddInParameter(dbcommand, "AtLeastKeyword", DbType.String, AtLeastKeyword);
            db.AddInParameter(dbcommand, "CategoryID", DbType.Int16, CategoryID);
            db.AddInParameter(dbcommand, "Tag", DbType.String, Tag);
            db.AddInParameter(dbcommand, "UploadTime", DbType.DateTime, UploadTime);
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, OrderByClause);
            db.AddInParameter(dbcommand, "Page", DbType.Int16, Page);
            db.AddInParameter(dbcommand, "PageSize", DbType.Int16, PageSize);

            List<Video> VideosByFeature;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                VideosByFeature = Next2Friends.Data.Video.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                if (dr.Read())
                {
                    RowCount = (int)dr["TotalRow"];
                }

                dr.Close();
            }

            return VideosByFeature;
        }
    }
}
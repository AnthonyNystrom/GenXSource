using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Globalization;

namespace Next2Friends.Data
{
    public enum TopPhotoType { Featured=1,Viewed=2, Discussed=3, Rated=4 }

    public partial class Photo
    {
        public string PhotoCollectionName { get; set; }
        public string PhotoCollectionDescription { get; set; }
        public string WebPhotoCollectionID { get; set; }

        public static Boolean MemberHasAnyPhotos(Int32 memberId)
        {
            var db = DatabaseFactory.CreateDatabase();
            var dbCommand = db.GetStoredProcCommand("HG_MemberHasAnyPhotos");
            db.AddInParameter(dbCommand, "memberId", DbType.Int32, memberId);

            var photoCount = Convert.ToInt32(db.ExecuteScalar(dbCommand), CultureInfo.InvariantCulture);
            return photoCount > 0;
        }

        /// <summary>
        /// Gets all the Photo related to a PhotoCollectionID
        /// </summary>
        public static Photo[] GetPhotoByPhotoCollectionIDWithJoin(int PhotoCollectionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoByPhotoCollectionIDWithJoin");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, PhotoCollectionID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos.ToArray();
        }


        /// <summary>
        /// Gets all the Photo related to a PhotoCollectionID
        /// </summary>
        public static List<Photo> GetPhotoByPhotoCollectionIDWithJoinGeneric(int PhotoCollectionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoByPhotoCollectionIDWithJoin");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, PhotoCollectionID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos;
        }


        public static List<Photo> GetTop100Photos(TopPhotoType topPhotoType)
        {
            string OrderByClause = string.Empty;

            switch (topPhotoType)
            {
                case TopPhotoType.Featured:
                    OrderByClause = "Featured";
                    break;
                case TopPhotoType.Discussed:
                    OrderByClause = "NumberOfComments";
                    break;
                case TopPhotoType.Viewed:
                    OrderByClause = "NumberOfViews";
                    break;
                case TopPhotoType.Rated:
                    OrderByClause = "TotalVoteScore";
                    break;
            }

            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetTop100Photos");
            db.AddInParameter(dbcommand, "OrderByClause", DbType.String, OrderByClause);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Photos = Photo.PopulatePhotoWithJoin(dr);
            }

            // Create the object array from the datareader
            return Photos;
        }

        /// <summary>
        /// Gets the Photo in the database With a full join with all the manually specified tables in SP code 
        /// </summary>
        public static Photo GetPhotoByWebPhotoIDWithJoin(string WebPhotoID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoByWebPhotoIDWithJoin");
            db.AddInParameter(dbCommand, "WebPhotoID", DbType.String, WebPhotoID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader

            if (Photos.Count > 0)
                return Photos[0];
            else
                return null;
        }

         /// <summary>
        /// Gets all the photos for an NSpot
        /// </summary>
        public static List<Photo> GetNSpotPhotosByNSpotID(int NSpotID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNSpotPhotosByNSpotID");
            db.AddInParameter(dbCommand, "NSpotID", DbType.Int32, NSpotID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulatePhotoWithJoin(dr);
                dr.Close();
            }

            return Photos;
        }


        
        /// <summary>
        /// 
        /// </summary>
        public static int GetPhotoIDByWebPhotoID(string WebPhotoID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoIDByWebPhotoID");
            db.AddInParameter(dbCommand, "WebPhotoID", DbType.String, WebPhotoID);

            int PhotoID = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    PhotoID = (int)dr[0];
                }

                dr.Close();
            }

            return PhotoID;
        }



        /// <summary>
        /// Gets all the phots for a memberWith a full join
        /// </summary>
        public static List<Photo> GetPhotoByPhotoCollectionIDWithJoinPager(int PhotoCollectionID, int Page, int PageSize)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoByPhotoCollectionIDWithJoinPager");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, PhotoCollectionID);
            db.AddInParameter(dbCommand, "Page", DbType.Int32, Page);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos;
        }

        public static int GetPhotoCountPhotoCollectionID(int PhotoCollectionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = null;

            dbCommand = db.GetStoredProcCommand("HG_GetPhotoCountPhotoCollectionID");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, PhotoCollectionID);

            int NumberOfPhotos = 0;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                if (dr.Read())
                {
                    NumberOfPhotos = (int)dr[0];
                    dr.Close();
                }
            }

            // Create the object array from the datareader
            return NumberOfPhotos;
        }

        /// <summary>
        /// Gets all the phots for a memberWith a full join
        /// Need this for Community page PhotoList
        /// </summary>
        public static List<Photo> GetPhotosByMemberIDWithJoin(int MemberID, int StartAt)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotosByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);
            db.AddInParameter(dbCommand, "StartAt", DbType.Int32, StartAt);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos;
        }


        /// <summary>
        /// Gets all the phots for a memberWith a full join
        /// </summary>
        public static List<Photo> GetPhotoByMemberIDWithJoinNoPager(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoByMemberIDWithJoinNoPager");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos;
        }

        /// <summary>
        /// Gets all the phots for a memberWith a full join
        /// </summary>
        public static List<Photo> GetFavouritePhotosByMemberIDWithJoin(int MemberID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetFavouritePhotosByMemberIDWithJoin");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<Photo> Photos = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                Photos = Photo.PopulateObjectWithJoin(dr);
                dr.Close();
            }

            // Create the object array from the datareader
            return Photos;
        }


        /// <summary>
        /// This method is incomplete and needs to also delete the ResourceFile
        /// </summary>
        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeletePhotoByPhotoID");
            db.AddInParameter(dbCommand, "PhotoID", DbType.Int32, PhotoID);

            //execute the stored procedure
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of Photos
        /// </summary>
        public static List<Photo> PopulatePhotoWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<Photo> arr = new List<Photo>();

            Photo obj;

            while (dr.Read())
            {
                obj = new Photo();
                obj._photoID = (int)dr["PhotoID"];
                obj._webPhotoID = (string)dr["WebPhotoID"];
                obj._photoCollectionID = (int)dr["PhotoCollectionID"];
                obj._memberID = (int)dr["MemberID"];
                obj._photoResourceFileID = (int)dr["PhotoResourceFileID"];
                obj._thumbnailResourceFileID = (int)dr["ThumbnailResourceFileID"];
                obj._width = (int)dr["Width"];
                obj._height = (int)dr["Height"];
                obj._totalVoteScore = (int)dr["TotalVoteScore"];
                obj._numberOfViews = (int)dr["NumberOfViews"];
                obj._numberOfComments = (int)dr["NumberOfComments"];
                obj._active = (bool)dr["Active"];
                obj._takenDT = (DateTime)dr["TakenDT"];
                obj._createdDT = (DateTime)dr["CreatedDT"];

                if (list.IsColumnPresent("Caption")) { obj.Caption = (string)dr["Caption"]; }
                if (list.IsColumnPresent("Title")) { obj.Title = (string)dr["Title"]; }

                if (list.IsColumnPresent("PhotoCollectionName")) { obj.PhotoCollectionName = (string)dr["PhotoCollectionName"]; }
                if (list.IsColumnPresent("PhotoCollectionDescription")) { obj.PhotoCollectionDescription = (string)dr["PhotoCollectionDescription"]; }
                if (list.IsColumnPresent("PhotoCollectionWebPhotoCollectionID")) { obj.WebPhotoCollectionID = (string)dr["PhotoCollectionWebPhotoCollectionID"]; }

                obj.PhotoResourceFile = new ResourceFile();
                if (list.IsColumnPresent("PhotoResourceFileResourceFileID")) { obj.PhotoResourceFile.ResourceFileID = (int)dr["PhotoResourceFileResourceFileID"]; }
                if (list.IsColumnPresent("PhotoResourceFileWebResourceFileID")) { obj.PhotoResourceFile.WebResourceFileID = (string)dr["PhotoResourceFileWebResourceFileID"]; }
                if (list.IsColumnPresent("PhotoResourceFileResourceType")) { obj.PhotoResourceFile.ResourceType = (int)dr["PhotoResourceFileResourceType"]; }
                if (list.IsColumnPresent("PhotoResourceFileStorageLocation")) { obj.PhotoResourceFile.StorageLocation = (int)dr["PhotoResourceFileStorageLocation"]; }
                if (list.IsColumnPresent("PhotoResourceFileServer")) { obj.PhotoResourceFile.Server = (int)dr["PhotoResourceFileServer"]; }
                if (list.IsColumnPresent("PhotoResourceFilePath")) { obj.PhotoResourceFile.Path = (string)dr["PhotoResourceFilePath"]; }
                if (list.IsColumnPresent("PhotoResourceFileFileName")) { obj.PhotoResourceFile.FileName = (string)dr["PhotoResourceFileFileName"]; }
                if (list.IsColumnPresent("PhotoResourceFileCreatedDT")) { obj.PhotoResourceFile.CreatedDT = (DateTime)dr["PhotoResourceFileCreatedDT"]; }

                obj.ThumbnailResourceFile = new ResourceFile();
                if (list.IsColumnPresent("ThumbnailResourceFileResourceFileID")) { obj.ThumbnailResourceFile.ResourceFileID = (int)dr["ThumbnailResourceFileResourceFileID"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileWebResourceFileID")) { obj.ThumbnailResourceFile.WebResourceFileID = (string)dr["ThumbnailResourceFileWebResourceFileID"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileResourceType")) { obj.ThumbnailResourceFile.ResourceType = (int)dr["ThumbnailResourceFileResourceType"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileStorageLocation")) { obj.ThumbnailResourceFile.StorageLocation = (int)dr["ThumbnailResourceFileStorageLocation"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileServer")) { obj.ThumbnailResourceFile.Server = (int)dr["ThumbnailResourceFileServer"]; }
                if (list.IsColumnPresent("ThumbnailResourceFilePath")) { obj.ThumbnailResourceFile.Path = (string)dr["ThumbnailResourceFilePath"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileFileName")) { obj.ThumbnailResourceFile.FileName = (string)dr["ThumbnailResourceFileFileName"]; }
                if (list.IsColumnPresent("ThumbnailResourceFileCreatedDT")) { obj.ThumbnailResourceFile.CreatedDT = (DateTime)dr["ThumbnailResourceFileCreatedDT"]; }

                obj.Member = new Member();
                if (list.IsColumnPresent("MemberMemberID")) { obj.Member.MemberID = (int)dr["MemberMemberID"]; }
                if (list.IsColumnPresent("MemberWebMemberID")) { obj.Member.WebMemberID = (string)dr["MemberWebMemberID"]; }
                if (list.IsColumnPresent("MemberAdminStatusID")) { obj.Member.AdminStatusID = (int)dr["MemberAdminStatusID"]; }
                if (list.IsColumnPresent("MemberNickName")) { obj.Member.NickName = (string)dr["MemberNickName"]; }
                if (list.IsColumnPresent("MemberChannelID")) { obj.Member.ChannelID = (int)dr["MemberChannelID"]; }
                if (list.IsColumnPresent("MemberPassword")) { obj.Member.Password = (string)dr["MemberPassword"]; }
                if (list.IsColumnPresent("MemberEmail")) { obj.Member.Email = (string)dr["MemberEmail"]; }
                if (list.IsColumnPresent("MemberGender")) { obj.Member.Gender = (int)dr["MemberGender"]; }
                if (list.IsColumnPresent("MemberFirstName")) { obj.Member.FirstName = (string)dr["MemberFirstName"]; }
                if (list.IsColumnPresent("MemberLastName")) { obj.Member.LastName = (string)dr["MemberLastName"]; }
                if (list.IsColumnPresent("MemberProfilePhotoResourceFileID")) { obj.Member.ProfilePhotoResourceFileID = (int)dr["MemberProfilePhotoResourceFileID"]; }
                if (list.IsColumnPresent("MemberDOB")) { obj.Member.DOB = (DateTime)dr["MemberDOB"]; }
                if (list.IsColumnPresent("MemberISOCountry")) { obj.Member.ISOCountry = (string)dr["MemberISOCountry"]; }
                if (list.IsColumnPresent("MemberZipPostcode")) { obj.Member.ZipPostcode = (string)dr["MemberZipPostcode"]; }
                if (list.IsColumnPresent("MemberCreatedDT")) { obj.Member.CreatedDT = (DateTime)dr["MemberCreatedDT"]; }


                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }

        /// <summary>
        /// Searches the database through title
        /// </summary>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        public static List<Photo> SearchPhotoByKeyword(string Keyword)
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbcommand = db.GetStoredProcCommand("HG_GetPhotoByKeywordSearch");
            db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);

            List<Photo> Photos;

            using (IDataReader dr = db.ExecuteReader(dbcommand))
            {
                Photos = PopulatePhotoWithJoin(dr);
                dr.Close();
            }

            return Photos;
        }

        public void UpdateTags()
        {

            char[] sep = {','};
            List<PhotoTag> tags = this.PhotoTag;

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
                        FormatedTags += ",";
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
                    PhotoTag photoTag = new PhotoTag();
                    photoTag.PhotoID = this.PhotoID;
                    photoTag.DTCreated = DateTime.Now;
                    photoTag.Tag = DedupedTags[j];
                    photoTag.Save();
                }
            }

            this.Tags = FormatedTags;
        }

    }
}

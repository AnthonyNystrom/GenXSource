using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public partial class PhotoCollection
    {
        public int NumberOfPhotos { get; set; }
        public string DefaultThumbnailURL { get; set; }

        public string ShortDescription
        {
            get {

                if (_description.Length > 105)
                {
                    return (_description.Substring(0, 105) + "...");
                }
                else
                {
                    return _description;
                }
            
            }

        }

        public string ShortName
        {
            get {

                if (_name.Length > 26)
                {
                    return (_name.Substring(0, 23) + "...");
                }
                else
                {
                    return _name;
                }
            
            }

        }



        
        /// <summary>
        /// Gets all the PhotoComment in the database for the member
        /// </summary>
        public static List<PhotoCollection> GetAllPhotoCollectionByMemberID(int MemberID)
        {
            IDataReader dr = null;

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoCollectionByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<PhotoCollection> PhotoCollections = new List<PhotoCollection>();

            //execute the stored procedure
            using (dr = db.ExecuteReader(dbCommand))
            {
                PhotoCollections = PopulatePhotoCollectionWithJoin(dr);
                dr.Close();
            }

            return PhotoCollections;
        }

        /// <summary>
        /// Gets all the PhotoComment in the database for the member
        /// </summary>
        public static List<PhotoCollection> GetPhotoGalleryForToday(int MemberID)
        {
            IDataReader dr = null;

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoGalleryForToday");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<PhotoCollection> PhotoCollections = new List<PhotoCollection>();

            //execute the stored procedure
            using (dr = db.ExecuteReader(dbCommand))
            {
                PhotoCollections = PopulateObject(dr);
                dr.Close();
            }

            return PhotoCollections;
        }

                /// <summary>
        /// Gets all the PhotoComment in the database for the member
        /// </summary>
        public static PhotoCollection GetOrCreatePhotoGalleryForToday(int MemberID)
        {
            List<PhotoCollection> photoCollections = PhotoCollection.GetPhotoGalleryForToday(MemberID);

            PhotoCollection photoCollection = null;

            bool CreateGallery = true;

            if (photoCollections.Count > 0)
            {
                photoCollection = photoCollections[0];

                if (photoCollection.DTCreated.Date == DateTime.Now.Date)
                {
                    CreateGallery = false;
                }
            }

            if (CreateGallery)
            {
                photoCollection = new PhotoCollection()
                {
                    MemberID = MemberID,
                    WebPhotoCollectionID = Next2Friends.Misc.UniqueID.NewWebID(),
                    Name = DateTime.Now.Date.ToLongDateString(),
                    DTCreated = DateTime.Now
                };

                photoCollection.Save();

            }

            return photoCollection;
        }

        /// <summary>
        /// Gets all the PhotoComment in the database for the member (even if the gallery has no photos)
        /// </summary>
        public static List<PhotoCollection> GetAllPhotoCollectionWithEmptyByMemberID(int MemberID)
        {
            IDataReader dr = null;

            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoCollectionWithEmptyByMemberID");
            db.AddInParameter(dbCommand, "MemberID", DbType.Int32, MemberID);

            List<PhotoCollection> PhotoCollections = new List<PhotoCollection>();

            //execute the stored procedure
            using (dr = db.ExecuteReader(dbCommand))
            {
                PhotoCollections = PopulatePhotoCollectionWithJoin(dr);
                dr.Close();
            }

            return PhotoCollections;
        }

        /// <summary>
        /// Deletes the gallery and all photos attached
        /// </summary>
        public void Delete()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_DeletePhotoCollectionAndPhotos");
            db.AddInParameter(dbCommand, "PhotoCollectionID", DbType.Int32, this.PhotoCollectionID);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch { }
        }

        /// <summary>
        /// Gets the PhotoColletion by WebPhotoCollectionID
        /// </summary>
        public static PhotoCollection GetPhotoCollectionByWebPhotoCollectionID(string WebPhotoCollectionID)
        {
            IDataReader dr = null;

            Database db = DatabaseFactory.CreateDatabase();
   
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetPhotoCollectionByWebPhotoCollectionID");
            db.AddInParameter(dbCommand, "WebPhotoCollectionID", DbType.String, WebPhotoCollectionID);

            List<PhotoCollection> PhotoCollections = null; 

            //execute the stored procedure
            using (dr = db.ExecuteReader(dbCommand))
            {
                PhotoCollections = PopulateObject(dr);
                dr.Close();
            }

            if (PhotoCollections.Count > 0)
                return PhotoCollections[0];
            else
                return null;
        }


        /// <summary>
        /// Takes an prepopulated IDataReader and creates an array of PhotoCollections
        /// </summary>
        public static List<PhotoCollection> PopulatePhotoCollectionWithJoin(IDataReader dr)
        {
            ColumnFieldList list = new ColumnFieldList(dr);

            List<PhotoCollection> arr = new List<PhotoCollection>();

            PhotoCollection obj;

            while (dr.Read())
            {
                obj = new PhotoCollection();
                obj._photoCollectionID = (int)dr["PhotoCollectionID"];
                obj._webPhotoCollectionID = (string)dr["WebPhotoCollectionID"];
                obj._memberID = (int)dr["MemberID"];
                obj._name = (string)dr["Name"];
                obj._description = (string)dr["Description"];
                obj._dTCreated = (DateTime)dr["DTCreated"];

                if (list.IsColumnPresent("NumberOfPhotos")) { obj.NumberOfPhotos = (int)dr["NumberOfPhotos"]; }
                if (list.IsColumnPresent("DefaultThumbnailURL")) { obj.DefaultThumbnailURL = (string)dr["DefaultThumbnailURL"]; }

                arr.Add(obj);
            }

            dr.Close();

            return arr;
        }
    }
}

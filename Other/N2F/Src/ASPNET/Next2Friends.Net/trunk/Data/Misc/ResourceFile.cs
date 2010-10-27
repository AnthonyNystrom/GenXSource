using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum StorageLocation { Images1, Images2, Images3 }
    public enum ResourceFileType { None, PhotoLarge, PhotoMedium, PhotoThumbnail, Video, VideoThumbnail, ProfilePhoto, ProfileThumbnail, AAFLarge, AAFThumbnail, NspotLarge, NspotPhoto, NspotThumbnail, Banner, AdvertImage }

    public partial class ResourceFile
    {
        public string FullyQualifiedURL 
        { 
            get 
            {
                return "user/" + this.Path + this.FileName; 
            } 
        }

        public string SavePath
        {
            get
            {
                return (this.Path + this.FileName).Replace(@"/", @"\"); ;
                
            }
        }

        public string URLPath
        {
            get
            {
                return this.Path + this.FileName;
            }
        }

        public string CustomPathFullyQualifiedURL(string SubDirectory)
        {
            return "user/" + SubDirectory + "/" + this.FileName;
        }

        public ResourceFile(string WebResourceFileID)
        {
            db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetResourceFileByWebResourceFileID");
            db.AddInParameter(dbCommand, "WebResourceFileID", DbType.String, WebResourceFileID);

            //execute the stored procedure
            IDataReader dr = db.ExecuteReader(dbCommand);

            if (dr.Read())
            {
                this._resourceFileID = (int)dr["ResourceFileID"];
                this._webResourceFileID = (string)dr["WebResourceFileID"];
                this._resourceType = (int)dr["ResourceType"];
                this._storageLocation = (int)dr["StorageLocation"];
                this._server = (int)dr["Server"];
                this._path = (string)dr["Path"];
                this._fileName = (string)dr["FileName"];
                this._createdDT = (DateTime)dr["CreatedDT"];

            }

            dr.Close();
        }
    }
}

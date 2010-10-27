using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public class NspotPhoto
    {
        public string ThumbnailURL { get; set; }// the URL of the thumbnail of the photo
        public string MainPhotoURL { get; set; }// the URL of the main photo
        public string ThumbnailWebResourceFileID { get; set; }// the WebResource file of the thumbnail (not currently used in the NSpot viewer)
        public string PhotoWebResourceFileID { get; set; }// the WebResource file of the photo (not currently used in the NSpot viewer)
        public DateTime TimeStamp { get; set; }// the time the photo was taken

        public static List<NspotPhoto> PopulateNSpotMember(IDataReader dr)
        {
            List<NspotPhoto> arr = new List<NspotPhoto>();

            while (dr.Read())
            {
                NspotPhoto nsPhoto = new NspotPhoto();

                nsPhoto.ThumbnailURL = WebNSpot.RootURL + (string)dr["ThumbnailURL"];
                nsPhoto.MainPhotoURL = WebNSpot.RootURL + (string)dr["MainPhotoURL"];
                nsPhoto.ThumbnailWebResourceFileID = (string)dr["ThumbnailWebResourceFileID"];
                nsPhoto.PhotoWebResourceFileID = (string)dr["PhotoWebResourceFileID"];
                nsPhoto.TimeStamp = (DateTime)dr["TimeStamp"];

                arr.Add(nsPhoto);
            }

            return arr;
        }


    }
}

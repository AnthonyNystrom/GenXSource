using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public class NspotVideo
    {
        public string ThumbnailURL { get; set; } // the URL of the thumbnail of the video
        public string MainVideoURL { get; set; }// the URL of the flv video
        public string ThumbnailWebResourceFileID { get; set; } // the WebResource file of the thumbnail (not currently used in the NSpot viewer)
        public string VideoWebResourceFileID { get; set; }// the WebResource file of the Video (not currently used in the NSpot viewer)
        public DateTime TimeStamp { get; set; } // the time the video was filmed

        public static List<NspotVideo> PopulateNSpotMemberWithJoin(IDataReader dr)
        {
            List<NspotVideo> arr = new List<NspotVideo>();

            while (dr.Read())
            {
                NspotVideo nsVideo = new NspotVideo();

                nsVideo.ThumbnailURL = WebNSpot.RootURL + (string)dr["ThumbnailURL"];
                nsVideo.MainVideoURL = WebNSpot.RootURL + (string)dr["MainVideoURL"];
                nsVideo.ThumbnailWebResourceFileID = (string)dr["ThumbnailWebResourceFileID"];
                nsVideo.VideoWebResourceFileID = (string)dr["VideoWebResourceFileID"];
                nsVideo.TimeStamp = (DateTime)dr["TimeStamp"];

                arr.Add(nsVideo);
            }

            return arr;
        }


    }
}
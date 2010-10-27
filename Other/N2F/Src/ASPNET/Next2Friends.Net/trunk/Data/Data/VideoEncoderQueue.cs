using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Data
{
    public enum VideoEncoderStatus { Ready,Picked, Completed,Errored}
    public enum ThumbnailEncoderStatus { Ready, Picked, Completed, Errored, VideoFileNotFound }

    public partial class VideoEncoderQueue
    {
        public static VideoEncoderQueue GetNextVideoToEncode()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNextVideoToEncode");

            List<VideoEncoderQueue> VideoEncode = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            {
                VideoEncode = PopulateObject(dr);
                dr.Close();
            }

            if (VideoEncode.Count > 0)
                return VideoEncode[0];
            else
                return null;
        }

    }

    /// <summary>
    /// this is not an entity in the databse
    /// </summary>
    public partial class LiveThumbnailEncoderQueue
    {
        public static LiveThumbnailEncoderQueue GetNextThumbnailToEncode()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DbCommand dbCommand = db.GetStoredProcCommand("HG_GetNextThumbnailToEncode");

            List<LiveThumbnailEncoderQueue> thumbnailEncoderQueue = null;

            //execute the stored procedure
            using (IDataReader dr = db.ExecuteReader(dbCommand))
            { 
               // thumbnailEncoderQueue = PopulateObject(dr);

                dr.Close();
            }

            if (thumbnailEncoderQueue.Count > 0)
                return thumbnailEncoderQueue[0];
            else
                return null;
        }

    }
}

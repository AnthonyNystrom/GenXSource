using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;

namespace Next2Friends.WebServices.Video
{
    public enum FlashPollType {LiveOnly,LastClip,LiveAndLastClip }

    public class FlashVideoDescriptor
    {
        /// <summary>
        /// The thumbnail of the URL
        /// </summary>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// The duration of the Video (if archived)
        /// </summary>
        public string Duration { get; set; }

        /// <summary>
        /// The display title of the video
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The display Description of the video
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Date time that the video was recorded
        /// </summary>
        public string RecordedDateTime { get; set; }

        /// <summary>
        /// Is the Clip live?
        /// </summary>
        public bool IsLive { get; set; }

        /// <summary>
        /// The FLV file URL (only available if IsLive = false)
        /// </summary>
        public string ArchivedFLVURL { get; set; }

        /// <summary>
        /// The LiveID of the clip for RED5 (only available if IsLive = true)
        /// </summary>
        public string LiveID { get; set; }

        /// <summary>
        /// Parse the livebroadcast into a FlashVideoDescriptor
        /// </summary>
        public static FlashVideoDescriptor ParseLive(LiveBroadcast livebroadcast)
        {
            FlashVideoDescriptor VideoDescriptor = null;

            if (livebroadcast != null)
            {
                VideoDescriptor = new FlashVideoDescriptor();
                VideoDescriptor.IsLive = true;
                VideoDescriptor.Title = livebroadcast.Title;
                VideoDescriptor.LiveID = livebroadcast.WebLiveBroadcastID;
            }

            return VideoDescriptor;
        }

        /// <summary>
        /// Parse the Video into a FlashVideoDescriptor
        /// </summary>
        public static FlashVideoDescriptor ParseLive(Next2Friends.Data.Video video)
        {
            FlashVideoDescriptor VideoDescriptor = null;

            if (video != null)
            {
                VideoDescriptor = new FlashVideoDescriptor();
                VideoDescriptor.IsLive = false;
                VideoDescriptor.Title = video.Title;
                VideoDescriptor.Description = video.Description;
                //VideoDescriptor.Duration = video.Duration;
                VideoDescriptor.RecordedDateTime = video.DTCreated.ToLongDateString();
                VideoDescriptor.ThumbnailURL = video.ThumbnailResourceFile.FullyQualifiedURL;
                VideoDescriptor.ArchivedFLVURL = video.VideoResourceFile.FullyQualifiedURL;
            }

            return VideoDescriptor;
        }
    }
}

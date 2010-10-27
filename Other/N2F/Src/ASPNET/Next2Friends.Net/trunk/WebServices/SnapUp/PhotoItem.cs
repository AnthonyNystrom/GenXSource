using System;

namespace Next2Friends.WebServices.SnapUp
{
    /// <summary>
    /// Data structure representing a photo.
    /// </summary>
    public class PhotoItem
    {
        public String WebPhotoID { get; set; }
        public String MainPhotoURL { get; set; }
        public String ThumbnailURL { get; set; }
    }
}

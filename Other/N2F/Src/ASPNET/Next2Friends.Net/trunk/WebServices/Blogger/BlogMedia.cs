using System;

namespace Next2Friends.WebServices.Blogger
{
    public abstract class BlogMedia
    {
        public Int32 ID { get; set; }
        public String CreatedDT { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Base64ThumbnailString { get; set; }
    }
}

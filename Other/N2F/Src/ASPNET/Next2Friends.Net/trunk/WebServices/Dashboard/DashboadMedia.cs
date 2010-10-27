using System;

namespace Next2Friends.WebServices.Dashboard
{
    public abstract class DashboadMedia
    {
        public String DateTime { get; set; }
        public String Nickname { get; set; }
        public String Text { get; set; }
        public String Title { get; set; }
        public String ThumbnailUrl { get; set; }
    }
}

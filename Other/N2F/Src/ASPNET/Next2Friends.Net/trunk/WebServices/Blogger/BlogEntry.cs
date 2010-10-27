using System;

namespace Next2Friends.WebServices.Blogger
{
    public class BlogEntry
    {
        /// <summary>
        /// Creates a new instance of the <code>BlogEntry</code> class.
        /// </summary>
        public BlogEntry()
        {
        }

        public Int32 ID { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public String DTCreated { get; set; }
    }
}

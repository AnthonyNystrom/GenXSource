using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct Event
    {
        /// <summary>
        /// The unique integer ItemID of the item being returned.
        /// </summary>
        [XmlRpcMember("itemid")]
        public Int32 ItemID;

        /// <summary>
        /// The time the user posted (or said they posted, rather, since users can back-date posts) the item being returned.
        /// </summary>
        [XmlRpcMember("eventtime")]
        public String EventTime;

        /// <summary>
        /// If this variable is not returned, then the security of the post is public, otherwise this value will be private or usemask.
        /// </summary>
        [XmlRpcMember("security")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public String Security;

        /// <summary>
        /// The subject of the journal entry. This won't be returned if "prefersubjects" is set, instead the subjects will show up as the events.
        /// </summary>
        [XmlRpcMember("subject")]
        public String Subject;

        /// <summary>
        /// The event text itself. This value is first truncated if the truncate variable is set, and then it is URL-encoded (alphanumerics stay the same, weird symbols to %hh, and spaces to + signs, just like URLs or post request). This allows posts with line breaks to come back on one line.
        /// </summary>
        [XmlRpcMember("event")]
        public String Content;

        /// <summary>
        /// The authentication number generated for this entry. It can be used by the client to generate URLs, but that is not recommended. (See the returned 'url' element if you want to link to a post.)
        /// </summary>
        [XmlRpcMember("anum")]
        public Int32 ANum;

        /// <summary>
        /// The permanent link address to this post. This is an opaque string--you should store it as is. While it will generally follow a predictable pattern, there is no guarantee of any particular format for these, and it may change in the future.
        /// </summary>
        [XmlRpcMember("url")]
        public String Url;
        
        /// <summary>
        /// If the poster of this event is different from the user value sent above, then this key will be included and will specify the username of the poster of this event. If this key is not present, then it is safe to assume that the poster of this event is none other than user.
        /// </summary>
        [XmlRpcMember("poster")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public String Poster;
    }
}

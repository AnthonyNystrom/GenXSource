using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct PostResponse
    {
        /// <summary>
        /// The unique number the server assigned to this post. Currently nothing else in the protocol requires the use of this number so it's pretty much useless, but somebody requested it be returned, so it is.
        /// </summary>
        [XmlRpcMember("itemid")]
        public Int32 ItemID;

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
    }
}

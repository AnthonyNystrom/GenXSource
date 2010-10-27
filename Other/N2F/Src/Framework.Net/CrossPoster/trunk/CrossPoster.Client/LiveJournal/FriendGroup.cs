using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct FriendGroup
    {
        /// <summary>
        /// The bit number for this friend group, from 1-30.
        /// </summary>
        [XmlRpcMember("id")]
        public Int32 ID;

        /// <summary>
        /// The name of this friend group.
        /// </summary>
        [XmlRpcMember("name")]
        public String Name;

        /// <summary>
        /// The sort integer for this friend group, from 0-255.
        /// </summary>
        [XmlRpcMember("sortorder")]
        public Int32 SortOrder;

        /// <summary>
        /// Either '0' or '1' for if this friend group is public.
        /// </summary>
        [XmlRpcMember("public")]
        public Int32 Public;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct UserBlog
    {
        public string url;
        public string blogid;
        public string blogName;
    }
}

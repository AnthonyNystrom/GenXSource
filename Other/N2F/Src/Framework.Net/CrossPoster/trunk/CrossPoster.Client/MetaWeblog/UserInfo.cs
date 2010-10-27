using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct UserInfo
    {
        public string url;
        public string blogid;
        public string blogName;
        public string firstname;
        public string lastname;
        public string email;
        public string nickname;
    }
}

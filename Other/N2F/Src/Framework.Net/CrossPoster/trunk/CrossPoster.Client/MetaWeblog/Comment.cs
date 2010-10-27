using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct Comment
    {
        public string postid;
        public DateTime dateCreated;
        public string description;
        public string title;
        public string commentUserName;
        public string commentUserEmail;
        public string commentUserUrl;
        public string commentUserIp;
    }
}

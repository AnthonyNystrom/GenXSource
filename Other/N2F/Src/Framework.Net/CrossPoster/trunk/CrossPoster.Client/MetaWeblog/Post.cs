using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct Post
    {
        public DateTime dateCreated;
        public string description;
        public string title;
        public string postid;
        public string[] categories;
        public override string ToString()
        {
            return title;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct NewCategory
    {
        public string name;
        public string slug;
        public int parent_id;
        public string description;
    }
}

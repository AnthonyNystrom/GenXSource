using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct Link
    {
        public string link_id;
        public string link_url;
        public string link_title;
        public string link_description;
        public string link_visible;
        public string link_rel;
    }
}

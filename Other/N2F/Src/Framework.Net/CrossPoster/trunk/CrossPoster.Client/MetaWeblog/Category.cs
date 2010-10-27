using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    struct Category
    {
        public string categoryId;
        public string parentId;
        public string description;
        public string categoryName;
        public string htmlUrl;
        public string rssUrl;
        public string title;
        public override string ToString()
        {
            if (categoryName != null)
                return categoryName;
            if (title != null)
                return title;
            return description;
        }
    }
}

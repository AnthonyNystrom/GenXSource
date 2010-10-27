using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Next2Friends.WebServices
{
    public class VideoSuggestion
    {
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
    }
}

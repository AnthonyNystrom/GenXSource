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
    public class VideoAdvert
    {
        /// <summary>
        /// Advert title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// the description text
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The URL to fetch the advert thumbnail from
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// the link to clickthrough if the user clicks on the link
        /// </summary>
        public string HyperLink { get; set; }

        /// <summary>
        /// the text to wrap the hyperlink in 
        /// </summary>
        public string HyperLinkText { get; set; }

        /// <summary>
        /// Should the popup be displayed (if false the rest of the parameters will be blank and the advert will not show)
        /// </summary>
        public bool Show { get; set; }

        public string TextColor0 { get; set; }

        public string TextColor1 { get; set; }

        public string BGColor { get; set; }

        public string OutlineColor { get; set; }

        public string CloseColor { get; set; }



   }
}

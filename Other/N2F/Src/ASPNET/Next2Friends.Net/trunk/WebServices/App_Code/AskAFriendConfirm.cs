using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Next2Friends.WebServices
{
    public class AskAFriendConfirm
    {
        private string _webAskAFriendID;
        private string _advertImage;
        private string _advertURL;

        /// <summary>
        /// The submission URL if the user clicks on the image
        /// </summary>
        public string AdvertURL
        {
            get { return _advertURL; }
            set { _advertURL = value; }
        }
	
        /// <summary>
        /// The advert image array as a JPEG byte stream
        /// </summary>
        public string AdvertImage
        {
            get { return _advertImage; }
            set { _advertImage = value; }
        }

        /// <summary>
        /// The 128 bit ID of the Question
        /// </summary>
        public string WebAskAFriendID
        {
            get { return _webAskAFriendID; }
            set { _webAskAFriendID = value; }
        }
    }
}

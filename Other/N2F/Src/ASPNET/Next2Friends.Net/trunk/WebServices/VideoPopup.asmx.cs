using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Next2Friends.Data;
using Next2Friends.WebServices.Video;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Summary description for AskAFriend
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class VideoPopup : System.Web.Services.WebService
    {
        public VideoPopup()
        {

        }


        [WebMethod]
        public PopupAd Get(string Nickname, String WebVideoID)
        {
            PopupAd pa = new PopupAd();

            pa.Timecode = 6000;
            pa.Duration = 4000;
            pa.Show = true;
            pa.ImageURL = "http://m1.2mdn.net/1501992/300x35.jpg";
            pa.ClickThroughURL = "http://www.cnet.com";

            return pa;
        }
    }
}

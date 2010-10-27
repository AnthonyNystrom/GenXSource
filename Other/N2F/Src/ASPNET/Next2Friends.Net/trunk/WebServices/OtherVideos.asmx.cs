using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Linq;
using Next2Friends.WebServices.Video;

namespace Next2Friends.WebServices
{
    /// <summary>
    /// Summary description for OtherVideos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OtherVideos : System.Web.Services.WebService
    {
        [WebMethod]
        public VideoSuggestion[] GetVideoSuggestions(string MemberID, String VideoID)
        {
            VideoSuggestion[] videoSuggestions = new VideoSuggestion[4];


            videoSuggestions[0] = new VideoSuggestion();
            videoSuggestions[0].Thumbnail = "http://img.youtube.com/vi/Rqx3R8qmqlo/default.jpg";
            videoSuggestions[0].Title = "Hannah Montana and Jonas Brothers: Blog";
            videoSuggestions[0].URL = "http://www.next2friends.com/";

            videoSuggestions[1] = new VideoSuggestion();
            videoSuggestions[1].Thumbnail = "http://img.youtube.com/vi/nFr1e37VqPc/default.jpg";
            videoSuggestions[1].Title = "It's Only Divine Right";
            videoSuggestions[1].URL = "http://www.next2friends.com/";

            videoSuggestions[2] = new VideoSuggestion();
            videoSuggestions[2].Thumbnail = "http://www.youtube.com/watch?v=pZ9jrBg4Lwc";
            videoSuggestions[2].Title = "Edgar Cruz - Bohemian Rhapsody (classical guitar)";
            videoSuggestions[2].URL = "http://www.next2friends.com/";

            videoSuggestions[3] = new VideoSuggestion();
            videoSuggestions[3].Thumbnail = "http://img.youtube.com/vi/Ua3hZXfNZOE/default.jpg";
            videoSuggestions[3].Title = "Guitar Hero 2 Rush YYZ on Expert";
            videoSuggestions[3].URL = "http://www.next2friends.com/";


            return videoSuggestions;
        }
    }
}

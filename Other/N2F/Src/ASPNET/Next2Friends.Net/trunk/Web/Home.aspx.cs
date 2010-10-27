using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;

public partial class Home : System.Web.UI.Page
{
    public string LiveStreamJS = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home));

        LiveStreamJS = GetArchivedBroadcasts();
    }

    public string GetArchivedBroadcasts()
    {
        List<Video> ArchivedBroadcasts = Video.GetArchivedBroadcasts();

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = ArchivedBroadcasts.Count-1; i >= 0; i--)
        {
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[4];

            parameters[0] = "http://www.next2friends.com/"+ArchivedBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = "http://www.next2friends.com/" + ArchivedBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[2] = "http://www.next2friends.com/" + ArchivedBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[3] = "http://www.next2friends.com/" + ArchivedBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;


            string HTMLItem = @"videoSlider.insert( 0, '{1}', '{2}', '{3}', true, true );";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        return sbHTMLList.ToString();
    }

    [AjaxPro.AjaxMethod]
    public string GetLiveBroadcasts()
    {
        List<LiveBroadcast> LiveBroadcasts = LiveBroadcast.GetAllLiveBroadcastNOW();

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 3; i++)
        {
            if (LiveBroadcasts.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[4];


            //parameters[0] = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            //parameters[1] = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            //parameters[2] = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            //parameters[3] = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;


            string HTMLItem = @"videoSlider.insert( 0, '{1}', '{2}', '{3}', false, true );";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        return sbHTMLList.ToString();
    }
}

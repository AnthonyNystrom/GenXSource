using System;
using System.Collections;
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

public partial class ClickThrough : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string WebBannerID = Context.Items["bannerid"].ToString();



        Banner banner = Banner.GetBannerByWebBannerID(WebBannerID);
        banner.TotalClicks++;
        banner.Save();

        BannerClick bannerClick = new BannerClick();
        bannerClick.BannerID = banner.BannerID;
        bannerClick.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
        // use the member id if available
        Member member = (Member)Session["Member"];
        bannerClick.MemberClickID = (member != null) ? member.MemberID : 0;

        bannerClick.Save();

        Response.Redirect(banner.ClickThroughURL);
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Routing;

/// <summary>
/// Summary description for URLRewritingRoutes
/// </summary>
public class URLRewritingRoutes
{
    public URLRewritingRoutes()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.Add(new Route("home/", new WebFormRouteHandler<Page>("~/Index.aspx")));
        routes.Add(new Route("forgotpassword/", new WebFormRouteHandler<Page>("~/ForgottenPassword.aspx")));
        routes.Add(new Route("ask/", new WebFormRouteHandler<Page>("~/AskAFriend.aspx")));
        routes.Add(new Route("ask/{WebAskID}/", new WebFormRouteHandler<Page>("~/AskAFriend.aspx")));
        routes.Add(new Route("dashboard/", new WebFormRouteHandler<Page>("~/Feed.aspx")));
        routes.Add(new Route("friends/", new WebFormRouteHandler<Page>("~/Friend.aspx")));
        routes.Add(new Route("proximity-tags/", new WebFormRouteHandler<Page>("~/Friend.aspx")));
        routes.Add(new Route("friends/", new WebFormRouteHandler<Page>("~/Friend.aspx")));
        routes.Add(new Route("friend-requests/", new WebFormRouteHandler<Page>("~/FriendRequest.aspx")));
        routes.Add(new Route("gallery/", new WebFormRouteHandler<Page>("~/ViewGallery.aspx")));
        routes.Add(new Route("myvideos/", new WebFormRouteHandler<Page>("~/MyVideoGallery.aspx")));
        routes.Add(new Route("myphotos/", new WebFormRouteHandler<Page>("~/MyPhotoGallery.aspx")));
        routes.Add(new Route("inbox/", new WebFormRouteHandler<Page>("~/Inbox.aspx")));
        routes.Add(new Route("video/", new WebFormRouteHandler<Page>("~/Video.aspx")));
        routes.Add(new Route("video/{params}/", new WebFormRouteHandler<Page>("~/Video.aspx")));
        routes.Add(new Route("community/", new WebFormRouteHandler<Page>("~/community.aspx")));
        routes.Add(new Route("community/{params}/", new WebFormRouteHandler<Page>("~/community.aspx")));
        routes.Add(new Route("signup", new WebFormRouteHandler<Page>("~/SignUp.aspx")));
        routes.Add(new Route("login", new WebFormRouteHandler<Page>("~/Login.aspx")));
        routes.Add(new Route("features", new WebFormRouteHandler<Page>("~/Features.aspx")));
        routes.Add(new Route("users/{nickname}/",  new WebFormRouteHandler<Page>("~/View.aspx")));
        routes.Add(new Route("users/{nickname}/videos",  new WebFormRouteHandler<Page>("~/MVideo.aspx")));
        routes.Add(new Route("users/{nickname}/photos", new WebFormRouteHandler<Page>("~/MPhoto.aspx")));
        routes.Add(new Route("users/{nickname}/wall",  new WebFormRouteHandler<Page>("~/Wall.aspx")));
        routes.Add(new Route("users/{nickname}/blog", new WebFormRouteHandler<Page>("~/Blog.aspx")));
        routes.Add(new Route("users/{nickname}/live", new WebFormRouteHandler<Page>("~/MLive.aspx")));
        routes.Add(new Route("blog/{title}/{webblogentryid}", new WebFormRouteHandler<Page>("~/Blog.aspx")));
        routes.Add(new Route("users/{nickname}/friends", new WebFormRouteHandler<Page>("~/MFriends.aspx")));
        routes.Add(new Route("video/{title}/{webvideoid}", new WebFormRouteHandler<Page>("~/VideoView.aspx")));
        routes.Add(new Route("download/", new WebFormRouteHandler<Page>("~/Download.aspx")));
        //routes.Add(new Route("download/", new WebFormRouteHandler<Page>("~/DownloadsDown.aspx")));
        routes.Add(new Route("termsofuse/", new WebFormRouteHandler<Page>("~/TOS.aspx")));
        routes.Add(new Route("labs/", new WebFormRouteHandler<Page>("~/Labs.aspx")));
        routes.Add(new Route("developers/",  new WebFormRouteHandler<Page>("~/developers.aspx")));
        routes.Add(new Route("termsofuse/", new WebFormRouteHandler<Page>("~/TOU.aspx")));
        routes.Add(new Route("privacypolicy/", new WebFormRouteHandler<Page>("~/PP.aspx")));
        
        routes.Add(new Route("feedback/", new WebFormRouteHandler<Page>("~/FeedBack.aspx")));
        routes.Add(new Route("businessopportunities/", new WebFormRouteHandler<Page>("~/BusinessOpps.aspx")));
        routes.Add(new Route("howto/", new WebFormRouteHandler<Page>("~/HowTo.aspx")));
        routes.Add(new Route("ad/{bannerid}", new WebFormRouteHandler<Page>("~/ClickThrough.aspx")));
        routes.Add(new Route("users/{nickname}/editaboutme", new WebFormRouteHandler<Page>("~/AboutMeEdit.aspx")));
        routes.Add(new Route("settings/", new WebFormRouteHandler<Page>("~/Settings.aspx")));
        routes.Add(new Route("m/", new WebFormRouteHandler<Page>("~/MobileSignup.aspx")));
        // import is to be removed
        routes.Add(new Route("import/", new WebFormRouteHandler<Page>("~/ImportEmails.aspx")));
        routes.Add(new Route("invite/", new WebFormRouteHandler<Page>("~/ImportEmails.aspx")));
        routes.Add(new Route("invite/{params}", new WebFormRouteHandler<Page>("~/ImportEmails.aspx")));

        //statics
        routes.Add(new Route("aboutus/", new WebFormRouteHandler<Page>("~/static/AboutUs.aspx")));
        routes.Add(new Route("managementteam/", new WebFormRouteHandler<Page>("~/static/ManagementTeam.aspx")));
        routes.Add(new Route("investors-partners/", new WebFormRouteHandler<Page>("~/static/InvestorsPartners.aspx")));
        routes.Add(new Route("news/", new WebFormRouteHandler<Page>("~/static/News.aspx")));
        routes.Add(new Route("events/", new WebFormRouteHandler<Page>("~/static/events.aspx")));
        routes.Add(new Route("ref/{encryptedparams}", new WebFormRouteHandler<Page>("~/ReferralPage.aspx")));
        routes.Add(new Route("rules-regulations/", new WebFormRouteHandler<Page>("~/RulesAndRegulations.aspx")));
        routes.Add(new Route("interactive-marketing-services/", new WebFormRouteHandler<Page>("~/static/IMS.aspx")));

        routes.Add(new Route("rss/", new WebFormRouteHandler<Page>("~/rssfeed/rss.aspx")));

        routes.Add(new Route("signupnow/", new WebFormRouteHandler<Page>("~/NotMemberStop.aspx")));

        routes.Add(new Route("welcome/", new WebFormRouteHandler<Page>("~/Index.aspx")));
		routes.Add(new Route("maximiseadspend/", new WebFormRouteHandler<Page>("~/static/MaximiseAdSpend.aspx")));
		routes.Add(new Route("whitelabel/", new WebFormRouteHandler<Page>("~/static/whitelabel.aspx")));
		
		routes.Add(new Route("photos/", new WebFormRouteHandler<Page>("~/photos.aspx")));

		
		

        

    }
}

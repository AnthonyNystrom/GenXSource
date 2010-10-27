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
using Next2Friends.Misc;

public partial class MiniVideoPage : System.Web.UI.Page
{
    public Video DefaultVideo;
    public Member ViewingMember;
    public string DefaultVideoURL;
    public string VideoURL;
    public string DefaultVoteUpLink;
    public string DefaultVoteDownLink;
    public string DefaultVoteCount = "0";
    public string AddFavouritesLink = "";
    public int NumberOfComments = 0;
    public string DefaultNumberOfViews = "0";
    public string PermaLink = string.Empty;
    public string EmbedLink = string.Empty;
    public string MainTitle = string.Empty;
    public string MainSubTitle = string.Empty;
    public Member member;
    public bool IsMyPage = false;
    public bool IsLoggedIn = false;
    public string DefaultMediaID;
    public string ReportAbuseLink;
    public string DefaultNewCommentParams;
    public string PageComments = string.Empty;

    public string WebRoot = ASP.global_asax.WebServerRoot;
    public DefaultPageType PageType;
    string LoginUrl = string.Empty;
    public string ThisURL = string.Empty;
    public bool VideoOnly = false;



    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MiniVideoPage));

        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        if (member != null)
        {
            IsLoggedIn = true;
        }

        string IsVideoOnly = Request.Params["VideoOnly"];

        if (IsVideoOnly != null)
        {
            VideoOnly = true;
        }

        // the value we pass to this page is the webvideoid from the url
        DefaultVideo = ExtractPageParams.GetVideo(this.Page, this.Context);

        string FormattedTitle = RegexPatterns.FormatHTMLTitle(DefaultVideo.Title);
        FormattedTitle = RegexPatterns.FormatStringForURL(FormattedTitle);
        ThisURL = "/video/" + FormattedTitle + "/" + DefaultVideo.WebVideoID;

        if (DefaultVideo != null)
        {
            string VideoTitle = ExtractPageParams.GetVideoTitle(this.Page, this.Context);

            NumberOfComments = DefaultVideo.NumberOfComments;
            ViewingMember = new Member(DefaultVideo.MemberID);
            PageType = DefaultPageType.Video;
            DefaultVideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            DefaultMediaID = DefaultVideo.WebVideoID;
            DefaultVoteCount = DefaultVideo.TotalVoteScore.ToString();
            VideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            DefaultNumberOfViews = (++DefaultVideo.NumberOfViews).ToString();

            int ViewerMemberID = (member != null) ? member.MemberID : 0;
            string IPAddress = Request.UserHostAddress;

            Video.IncreaseWatchedCount(DefaultVideo.WebVideoID, ViewerMemberID, IPAddress);

            // no need to save anymore
            //DefaultVideo.Save();
            PermaLink = WebRoot + "video/" + RegexPatterns.FormatStringForURL(DefaultVideo.Title) + "/" + DefaultVideo.WebVideoID;
            EmbedLink = @"<object width=""480"" height=""400""><param name=""movie"" value=""http://www.next2friends.com/flvplayer.swf""></param><param name=""wmode"" value=""transparent""></param><embed src=""http://www.next2friends.com/flvplayer.swf?file=" + VideoURL + @""" type=""application/x-shockwave-flash"" wmode=""transparent"" width=""480"" height=""400""></embed></object>";

            MainTitle = DefaultVideo.Title;
            MainSubTitle = DefaultVideo.Description;

            if (IsLoggedIn)
            {
                ReportAbuseLink = "/ReportAbuse.aspx?r=" + DefaultMediaID;
                DefaultVoteUpLink = @"javascript:vote('" + DefaultMediaID + "', true);";
                DefaultVoteDownLink = @"javascript:vote('" + DefaultMediaID + "', false);";
                AddFavouritesLink = @"javascript:addToFavourites('" + CommentType.Video.ToString() + "','" + DefaultMediaID + "');";
            }
            else
            {
                ReportAbuseLink = @"/signup.aspx?u=ReportAbuse.aspx?r=" + DefaultMediaID + "&url=" + Request.Url.AbsoluteUri;
                DefaultVoteUpLink = LoginUrl;
                DefaultVoteDownLink = LoginUrl;
                AddFavouritesLink = LoginUrl;
            }

            if (member != null)
            {
                if (!Utility.IsMe(ViewingMember, member))
                {
                    Utility.ContentViewed(member, ViewingMember.MemberID, CommentType.Member);
                }
            }

        //    Comments1.ObjectId = DefaultVideo.VideoID;
        //    Comments1.ObjectWebId = DefaultVideo.WebVideoID;
        //    Comments1.CommentType = CommentType.Video;

        //    forwardToFriend.ObjectWebID = DefaultVideo.WebVideoID;
        //    forwardToFriend.ContentType = CommentType.Video;
        }
        else
        {
            //404 - The video was not found
            HTTPResponse.FileNotFound404(Context);
            Server.Transfer("/NotAvailable.aspx?rt=v");
        }
    }

    [AjaxPro.AjaxMethod]
    public int Vote(string WebID, bool up)
    {
        member = (Member)HttpContext.Current.Session["Member"];

        Vote v = new Vote();
        v.MemberID = member.MemberID;
        v.Value = (up) ? 1 : -1;
        v.VideoID = Video.GetVideoIDByWebVideoID(WebID);

        bool value = v.SaveWithCheck();

        if (value)
        {
            if (up)
                return 1;
            else
                return -1;
        }
        else
        {
            return 0;
        }
    }
}

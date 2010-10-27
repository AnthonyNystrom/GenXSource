using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.Misc;

public partial class VideoView : System.Web.UI.Page
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
    public string SliderJS = string.Empty;
    public string ThisURL = string.Empty;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginUrl = @"/signup/?u=" + Request.Url.AbsoluteUri;
        string strLiveBroadcastID = Request.Params["l"];

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        DefaultVideo = ExtractPageParams.GetVideo(this.Page, this.Context);
        string FormattedTitle = RegexPatterns.FormatHTMLTitle(DefaultVideo.Title);
        FormattedTitle = RegexPatterns.FormatStringForURL(FormattedTitle);
        ThisURL = "/video/" + FormattedTitle + "/" + DefaultVideo.WebVideoID;

        if (DefaultVideo != null)
        {
            string VideoTitle = ExtractPageParams.GetVideoTitle(this.Page, this.Context);

            // SEO: if the title has changed then send a redirect request to the browser
            if (FormattedTitle != VideoTitle)
            {
                HTTPResponse.PermamentlyMoved301(Context, ThisURL);

                //Context.Response.Status = "301 Moved Permanently";
                //Context.Response.StatusCode = 301;
                //Context.Response.AddHeader("location", "/videos/" + FormattedTitle + "/" + DefaultVideo.WebVideoID);
                //Context.Response.Redirect("/video/" + FormattedTitle + "/" + DefaultVideo.WebVideoID);
            }

            NumberOfComments = DefaultVideo.NumberOfComments;
            ViewingMember = new Member(DefaultVideo.MemberID);
            PageType = DefaultPageType.Video;
            DefaultVideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            DefaultMediaID = DefaultVideo.WebVideoID;
            DefaultVoteCount = DefaultVideo.TotalVoteScore.ToString();
            VideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            DefaultNumberOfViews = (++DefaultVideo.NumberOfViews).ToString();
            
            int ViewerMemberID = (member!=null) ? member.MemberID : 0;
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
                DefaultVoteUpLink = @"javascript:vote('v','" + DefaultMediaID + "', true);";
                DefaultVoteDownLink = @"javascript:vote('v','" + DefaultMediaID + "', false);";
                AddFavouritesLink = @"javascript:addToFavourites('" + CommentType.Video.ToString() + "','" + DefaultMediaID + "');";                        
            }
            else
            {
                ReportAbuseLink = @"/signup.aspx?u=ReportAbuse.aspx?r=" + DefaultMediaID + "&url=" + Request.Url.AbsoluteUri;
                DefaultVoteUpLink = LoginUrl;
                DefaultVoteDownLink = LoginUrl;
                AddFavouritesLink = LoginUrl;
            }

            Comments1.ObjectId = DefaultVideo.VideoID;
            Comments1.ObjectWebId = DefaultVideo.WebVideoID;
            Comments1.CommentType = CommentType.Video;

            forwardToFriend.ObjectWebID = DefaultVideo.WebVideoID;
            forwardToFriend.ContentType = CommentType.Video;             
        }        
        else
        {
            //404 - The video was not found
            HTTPResponse.FileNotFound404(Context);
            Server.Transfer("/NotAvailable.aspx?rt=v");
        }

        //RenderVideoSlider();
       
    }

    public void RenderVideoSlider()
    {
        List<Video> Videos = Video.GetVideosByMemberIDWithJoin(ViewingMember.MemberID,PrivacyType.Network);

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = Videos.Count - 1; i >= 0; i--)
        {
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[8];

            parameters[0] = "";
            parameters[1] = "http://www.next2friends.com/" + Videos[i].VideoResourceFile.FullyQualifiedURL;
            parameters[2] = "http://www.next2friends.com/" + Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[3] = "";
            parameters[4] = "";
            parameters[5] = "{";
            parameters[6] = "}";
            parameters[7] = TimeDistance.TimeAgo(Videos[i].DTCreated);


            string HTMLItem = @"videoSlider.insert(0, null, '{1}', '{2}', '{3}', false, false, {5} c: '{4}', dt: '{7}' {6}  );";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        SliderJS = sbHTMLList.ToString();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetVideoViewTitle(DefaultVideo);
    }
}

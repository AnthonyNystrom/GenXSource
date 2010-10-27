using System;
using System.IO;
using System.Text;
using System.Threading;
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
using System.Web.Caching;
using Next2Friends.Data;
using Next2Friends.Misc;

public partial class IndexPage : System.Web.UI.Page
{
    public Member member;
    public bool IsLoggedIn = false;
    public int NumberOfFriend = 0;
    public string InitialLiveBroadcasts = string.Empty;
    public string DefaultVideoURL = string.Empty;
    public string FeatureSidebarHTML = string.Empty;
    public Video DefaultVideo;
    public List<Video> Videos;
    public string TopVideoHTML = string.Empty;
    public SystemStatistics stats;
    public string InitialLiveStreams = string.Empty;
    public string FlashStreamJS = string.Empty;
    public string DefaultFLV = string.Empty;
    public string DefaultLiveTitle = string.Empty;
    public string DefaultLiveWebVideoID = string.Empty;
    public string IsLive = "false";
    public string DefaultPlayerJS = string.Empty;
    public int NumberOfLiveStreams = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(IndexPage));

        string strMemberNickname = Request.Params["n"];

        if (strMemberNickname != null)
        {
            Member ShortcutMember = Member.GetMemberViaNicknameNoEx(strMemberNickname);

            if (ShortcutMember != null)
            {
                Server.Transfer("view.aspx?m=" + ShortcutMember.WebMemberID);
            }
        }

        if (IsLoggedIn)
        {
            NumberOfFriend = FriendRequest.GetNumberOfFriends(member.MemberID);

        }

        GenerateStats();

        //DefaultVideo = @"http://www.next2friends.com/user/Lawrence/video/MWM0YWJmYjhmNTdlNDBhYW.flv";//;Video.GetHomePageVideo();

        if (DefaultVideo != null)
        {
            DefaultVideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
        }

        string LiveStreams = GetLiveBroadcasts();
        FlashStreamJS = GetArchivedBroadcasts(NumberOfLiveStreams);
        FlashStreamJS += LiveStreams;

        TopVideoHTML = GetTopVideos(0);
        FeatureSidebarHTML = GenerateFeaturedMemberSidebar();
    }

    public string GetArchivedBroadcasts(int LiveStreams)
    {
        List<Video> ArchivedBroadcasts = Video.GetArchivedBroadcasts();

        StringBuilder sbHTMLList = new StringBuilder();
       
        for (int i = ArchivedBroadcasts.Count - 1; i >= 0; i--)
        {
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[4];

            parameters[0] = ArchivedBroadcasts[i].WebVideoID;
            parameters[1] = "http://www.next2friends.com/" + ArchivedBroadcasts[i].VideoResourceFile.FullyQualifiedURL;
            parameters[2] = "http://www.next2friends.com/" + ArchivedBroadcasts[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[3] = ArchivedBroadcasts[i].Member.NickName + ":" + ArchivedBroadcasts[i].Member.ISOCountry;


            string HTMLItem = @"videoSlider.insert(0, '_{0}', '{1}', '{2}', '{3}', false, false );";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        if (NumberOfLiveStreams == 0 && ArchivedBroadcasts.Count>0)
        {
            string flv = "http://www.next2friends.com/" + ArchivedBroadcasts[0].VideoResourceFile.FullyQualifiedURL;

            DefaultLiveTitle = ArchivedBroadcasts[0].Member.NickName + ":" + ArchivedBroadcasts[0].Member.ISOCountry;
            DefaultLiveWebVideoID = ArchivedBroadcasts[0].WebVideoID;
            IsLive = "false";

            //player(fl,fr,play,live,id){
            DefaultPlayerJS = "player('" + flv + "','_" + ArchivedBroadcasts[0].WebVideoID+ "',true,false,'" + ArchivedBroadcasts[0].WebVideoID + "');";
        }

        return sbHTMLList.ToString();
    }

    public string GetLiveBroadcasts()
    {
        List<LiveBroadcast> LiveBroadcasts = LiveBroadcast.GetAllLiveBroadcastNOW2();

        NumberOfLiveStreams = LiveBroadcasts.Count;

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = LiveBroadcasts.Count - 1; i >= 0; i--)
        {
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[4];

            parameters[0] = LiveBroadcasts[i].WebLiveBroadcastID;
            parameters[1] = "";
            parameters[2] = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailURL;
            parameters[3] = LiveBroadcasts[i].Member.NickName + ":" + LiveBroadcasts[i].Member.ISOCountry;

            string HTMLItem = @"livePush('{0}', '{1}', '{2}', '{3}', true, false);";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        if (LiveBroadcasts.Count > 0)
        {
            //player(fl,fr,play,live,id){
            DefaultPlayerJS = "player('','',true,true,'"+LiveBroadcasts[0].WebLiveBroadcastID+"');";
            DefaultLiveTitle = LiveBroadcasts[0].Member.NickName + ":" + LiveBroadcasts[0].Member.ISOCountry;
            IsLive = "true";
        }

        return sbHTMLList.ToString();
    }

    [AjaxPro.AjaxMethod]
    public AjaxLB[] GetLB()
    {
        List<LiveBroadcast> LiveBroadcasts = LiveBroadcast.GetAllLiveBroadcastNOW2();

        AjaxLB[] AjaxLBs = new AjaxLB[LiveBroadcasts.Count];

        for (int i = LiveBroadcasts.Count - 1; i >= 0; i--)
        {
            AjaxLBs[i] = new AjaxLB();

            AjaxLBs[i].UniqueID = LiveBroadcasts[i].WebLiveBroadcastID;
            AjaxLBs[i].ThumbnailURL = "http://www.next2friends.com/" + LiveBroadcasts[i].ThumbnailURL;
            AjaxLBs[i].Title = LiveBroadcasts[i].Member.NickName + ":" + LiveBroadcasts[i].Member.ISOCountry;
        }

        return AjaxLBs;
    }

    public string GenerateFeaturedMemberSidebar()
    {
        string CacheKey = "FeaturedMemberHomePage";

        List<Member> FeaturedMembers = (List<Member>)System.Web.HttpContext.Current.Cache[CacheKey];

        //if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        //{
            FeaturedMembers = new List<Member>();
            FeaturedMembers.Add(Member.GetMembersByMemberIDWithFullJoin(5)); // Sqeaks
            FeaturedMembers.Add(Member.GetMembersByMemberIDWithFullJoin(136)); // Dazzala
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, FeaturedMembers, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration);
        //}

       

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 2; i++)
        {
            if (FeaturedMembers.Count <= i)
            {
                break;
            }

            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[11];

            parameters[0] = ParallelServer.Get() + FeaturedMembers[i].DefaultPhoto.FullyQualifiedURL;
            parameters[1] = TimeDistance.TimeAgo(FeaturedMembers[i].LastOnline);
            parameters[2] = FeaturedMembers[i].WebMemberID;
            parameters[3] = FeaturedMembers[i].MemberProfile[0].NumberOfViews;
            parameters[4] = FeaturedMembers[i].NickName;




            sbHTML.AppendFormat(@"<li>
						<div class='vid_thumb'><a href='view.aspx?m={2}'><img width='124' height='91' src='{0}' alt='thumb' /></a></div>

						<div class='vid_info'>
							<a href='view.aspx?m={2}'><strong>{4}</strong></a>
							<p class='metadata'>
								Views: {3} <br/>Last Online: <a href='view.aspx?m={2}'>{1}</a>

							</p>
						</div>
					</li>", parameters);

            sbHTMLList.Append(sbHTML.ToString());
        }

        //<h3><a href='view.aspx?v={2}'>{9}</a></h3>
        return sbHTMLList.ToString();

    }

    public string GenerateFeatureSidebar(List<Video> Videos)
    {
        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 2; i++)
        {
            if (Videos.Count <= i)
            {
                break;
            }

            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[12];

            parameters[0] = ParallelServer.Get() + Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = TimeDistance.TimeAgo(Videos[i].DTCreated);
            parameters[2] = Videos[i].WebVideoID;
            parameters[3] = Videos[i].NumberOfViews;
            parameters[4] = Videos[i].NumberOfComments;
            parameters[5] = Videos[i].TotalVoteScore;
            parameters[6] = Videos[i].NumberOfComments;
            parameters[7] = Videos[i].Member.NickName;
            parameters[8] = Videos[i].Member.WebMemberID;
            parameters[9] = Videos[i].Title;
            parameters[10] = Videos[i].Description;
            

            sbHTML.AppendFormat(@"<li>
						<div class='vid_thumb'><a href='view.aspx?v={2}'><img width='124' height='91' src='{0}' alt='thumb' /></a></div>

						<div class='vid_info'>
							<a href='view.aspx?v={2}'><strong>{9}</strong></a>
							<p class='metadata'>
								2 days ago<br />
								by: <a href='view.aspx?m={8}'>{7}</a><br />
								Views: {3} Comments: <a href='view.aspx?v={2}'>{4}</a>

							</p>
						</div>
					</li>", parameters);

            sbHTMLList.Append(sbHTML.ToString());
        }

        //<h3><a href='view.aspx?v={2}'>{9}</a></h3>
        return sbHTMLList.ToString();

    }

    public void GenerateStats()
    {
        // disable caching for dev
        // implement caching for 1 minute
        string CacheKey = "Stats";
        stats = (SystemStatistics)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {
            stats = new SystemStatistics();
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, stats, null, DateTime.Now.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }

//    [AjaxPro.AjaxMethod]
//    public string GetLiveBroadcasts()
//    {
//        List<LiveBroadcast> LiveBroadcasts = LiveBroadcast.GetAllLiveBroadcastNOW();

//        StringBuilder sbHTMLList = new StringBuilder();

//        for (int i = 0; i < 3; i++)
//        {
//            if (LiveBroadcasts.Count <= i)
//            {
//                break;
//            }

//            StringBuilder sbHTMLItem = new StringBuilder();

//            object[] parameters = new object[3];

//            parameters[0] = LiveBroadcasts[i].WebLiveBroadcastID;
//            parameters[1] = LiveBroadcasts[i].Title;
//            parameters[2] = TimeDistance.TimeAgo(LiveBroadcasts[i].DTStart);

//            string HTMLItem = @"<li class='no_border'>
//							<div class='steam_thumb'><a href='view.aspx?l={0}'><img src='images/LiveStream.gif' alt='thumb' /></a></div>
//							<div class='steam_info'>
//								<h3><a href='view.aspx?l={0}'>{1}</a></h3>
//								<p>Started: {2}</p>
//							</div>
//						</li>";

//                sbHTMLItem.AppendFormat(HTMLItem, parameters);
//                sbHTMLList.Append(sbHTMLItem.ToString());
//        }

//        return sbHTMLList.ToString();
//    }

    [AjaxPro.AjaxMethod]
    public string GetTest(string x)
    {
        return "Jquery rules:" + x;
    }

    [AjaxPro.AjaxMethod]
    public void Watched(string WebVideoID)
    {
        if (WebVideoID != string.Empty)
        {
            WebVideoID = WebVideoID.Substring(1);
            member = (Member)Session["Member"];
            int WatchedByMemberID = 0;

            if (member != null)
            {
                WatchedByMemberID = member.MemberID;
            }
            string IPAddress = Context.Request.UserHostAddress;

            Video.IncreaseWatchedCount(WebVideoID, WatchedByMemberID, IPAddress);
        }
    }

    [AjaxPro.AjaxMethod]
    public string GetTopVideos(OrderByType TabType)
    {
        string CacheKey = "TopVideo" + TabType;

        Videos = (List<Video>)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {
            Videos = Video.GetTop100Videos((OrderByType)TabType);
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, Videos, null, DateTime.Now.AddSeconds(30.00), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 7; i++)
        {
            if (Videos.Count <= i)
            {
                break;
            }
            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = ParallelServer.Get() + Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = Videos[i].TimeAgo;
            parameters[2] = Videos[i].Title;
            parameters[3] = Videos[i].Description;
            parameters[4] = Videos[i].NumberOfViews;
            parameters[5] = Videos[i].NumberOfComments;
            parameters[6] = Videos[i].Member.NickName;
            parameters[7] = Videos[i].Category;
            parameters[8] = Videos[i].WebVideoID;
            parameters[9] = Videos[i].TotalVoteScore;
            parameters[10] = Videos[i].Member.WebMemberID;
            parameters[11] = Videos[i].FormattedTags();
            parameters[12] = NCache.GetCategoryName(Videos[i].Category);
            parameters[13] = Videos[i].Category;
            parameters[14] = RegexPatterns.FormatStringForURL(Videos[i].Title);

            string HTMLItem = @"<li><div class='vid_thumb'>
                                    <a href='/video/{14}/{8}/'><img src='{0}' alt='{2}' width='124' height='91' />

                                {1}
                                </div>
                                <div class='vid_info'>
                                    <h3>
                                        <a href='/video/{14}/{8}'>
                                            {2}</a></h3>
                                    <div class='vote vote_condensed'><span class='vote_count'>{9}</span></div>
                                        {3}</p>
                                    <p class='metadata'>
                                        Views: {4} Comments: {5}<br />
                                        Category: <a href='video.aspx?cat={13}'>{12}</a><br />
                                        Tags: {11}<br />
                                        From: <a href='/users/{6}'>{6}</a></p>
                                </div>
                            </li>";


                sbHTMLItem.AppendFormat(HTMLItem, parameters);
                sbHTMLList.Append(sbHTMLItem.ToString());
        }

        return sbHTMLList.ToString();
    }

    //protected void btnInvite_Click(object sender, EventArgs e)
    //{
    //    string Email1 = txtFriend1.Text;
    //    string Email2 = txtFriend2.Text;
    //    string Email3 = txtFriend3.Text;
    //    string Email4 = txtFriend4.Text;
    //    string Email5 = txtFriend5.Text;

    //    MemberInvite memberInvite1 = new MemberInvite();
    //    MemberInvite memberInvite2 = new MemberInvite();
    //    MemberInvite memberInvite3 = new MemberInvite();
    //    MemberInvite memberInvite4 = new MemberInvite();
    //    MemberInvite memberInvite5 = new MemberInvite();

    //    bool Go1 = false;
    //    bool Go2 = false;
    //    bool Go3 = false;
    //    bool Go4 = false;
    //    bool Go5 = false;

    //    bool go = true;
    //    bool AtLeast1 = false;

    //    if (Email1 != string.Empty)
    //    {
    //        AtLeast1 = true;

    //        if (RegexPatterns.TestEmailRegex(Email1))
    //        {
    //            memberInvite1.DTCreated = DateTime.Now;
    //            memberInvite1.EmailAddress = Email1;
    //            txtFriend1.CssClass = "form_txt";
    //            Go1 = true;
    //        }
    //        else
    //        {
    //            go = false;
    //            txtFriend1.CssClass = "form_txt formerror";
    //        }
    //    }
    //    else
    //    {
    //        txtFriend1.CssClass = "form_txt";
    //    }


    //    if (Email2 != string.Empty)
    //    {
    //        AtLeast1 = true;

    //        if (RegexPatterns.TestEmailRegex(Email2))
    //        {
    //            memberInvite2.DTCreated = DateTime.Now;
    //            memberInvite2.EmailAddress = Email2;
    //            txtFriend2.CssClass = "form_txt";
    //            Go2 = true;
    //        }
    //        else
    //        {
    //            go = false;
    //            txtFriend2.CssClass = "form_txt formerror";
    //        }
    //    }
    //    else
    //    {
    //        txtFriend2.CssClass = "form_txt";
    //    }


    //    if (Email3 != string.Empty)
    //    {
    //        AtLeast1 = true;

    //        if (RegexPatterns.TestEmailRegex(Email3))
    //        {
    //            memberInvite3.DTCreated = DateTime.Now;
    //            memberInvite3.EmailAddress = Email3;
    //            txtFriend3.CssClass = "form_txt";
    //            Go3 = true;
    //        }
    //        else
    //        {
    //            go = false;
    //            txtFriend3.CssClass = "form_txt formerror";
    //        }
    //    }
    //    else
    //    {
    //        txtFriend3.CssClass = "form_txt";
    //    }


    //    if (Email4 != string.Empty)
    //    {
    //        AtLeast1 = true;

    //        if (RegexPatterns.TestEmailRegex(Email4))
    //        {
    //            memberInvite4.DTCreated = DateTime.Now;
    //            memberInvite4.EmailAddress = Email4;
    //            txtFriend4.CssClass = "form_txt";
    //            Go4 = true;
    //        }
    //        else
    //        {
    //            go = false;
    //            txtFriend4.CssClass = "form_txt formerror";
    //        }
    //    }
    //    else
    //    {
    //        txtFriend4.CssClass = "form_txt";
    //    }

    //    if (Email5 != string.Empty)
    //    {
    //        AtLeast1 = true;

    //        if (RegexPatterns.TestEmailRegex(Email5))
    //        {
    //            memberInvite5.DTCreated = DateTime.Now;
    //            memberInvite5.EmailAddress = Email5;
    //            txtFriend5.CssClass = "form_txt";
    //            Go5 = true;

    //        }
    //        else
    //        {
    //            go = false;
    //            txtFriend5.CssClass = "form_txt formerror";
    //        }
    //    }
    //    else
    //    {
    //        txtFriend5.CssClass = "form_txt";
    //    }

    //    if (!AtLeast1)
    //    {

    //        litInvite.Text = "<p class='error_alert'>Please enter at least one email address</p>";
    //    }
    //    else if (go)
    //    {
    //        litInvite.Text = "<p>Thank you, we have invited your friends</p>";


    //        string AttachedMessage = (txtInviteMessage.Text == "Your personal message here!") ? string.Empty : txtInviteMessage.Text;

    //        memberInvite1.CustomMessage = AttachedMessage;
    //        memberInvite2.CustomMessage = AttachedMessage;
    //        memberInvite3.CustomMessage = AttachedMessage;
    //        memberInvite4.CustomMessage = AttachedMessage;
    //        memberInvite5.CustomMessage = AttachedMessage;

    //        member = (Member)Session["Member"];
    //        if (member != null)
    //        {
    //            memberInvite1.MemberID = member.MemberID;
    //            memberInvite2.MemberID = member.MemberID;
    //            memberInvite3.MemberID = member.MemberID;
    //            memberInvite4.MemberID = member.MemberID;
    //            memberInvite5.MemberID = member.MemberID;
    //        }

    //        if(Go1)memberInvite1.Save();
    //        if (Go2) memberInvite2.Save();
    //        if (Go3) memberInvite3.Save();
    //        if (Go4) memberInvite4.Save();
    //        if (Go5) memberInvite5.Save();

    //        txtFriend1.Text = string.Empty;
    //        txtFriend2.Text = string.Empty;
    //        txtFriend3.Text = string.Empty;
    //        txtFriend4.Text = string.Empty;
    //        txtFriend5.Text = string.Empty;
    //    }
    //    else
    //    {
    //        litInvite.Text = "<p class='error_alert'>One or more invalid email address</p>";
    //    }
    //}

    /// <summary>
    /// set the home page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {



        Master.SkinID = "home";
        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }
      
        base.OnPreInit(e);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {

        Master.HTMLTitle = PageTitle.GetHomePageTitle();
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }




}


public class AjaxLB
{
    public string ThumbnailURL { get; set; }
    public string Title { get; set; }
    public string UniqueID { get; set; }
}
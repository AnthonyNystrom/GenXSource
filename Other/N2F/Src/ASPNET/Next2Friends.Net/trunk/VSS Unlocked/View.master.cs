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


public partial class ViewMaster : System.Web.UI.MasterPage
{
    public enum FriendRequestStatus { AlreadyFriend, RequestSent, NotAFriend }

    public Member ViewingMember;
    public MemberProfile ViewingMemberProfile;

    public Member member;

    public string MainTitle = string.Empty;
    public string MainSubTitle = string.Empty;

    public FriendRequestStatus FriendshipStatus = FriendRequestStatus.NotAFriend;
    public bool ShowSubscribe = true;

    public string LoginUrl;

    public string PhotoURL;
    public string LargePhotoURL;

    public string VideoURL;
    public string MemberSubscribers = string.Empty;
    public string NumberOfMemberSubscribers = string.Empty;
    public int NumberOfComments = 0;
    public int NumberOfVideos = 0;
    public int NumberOfPhotos = 0;
    public int NumberOfFriends = 0;
    public bool IsMyPage = false;
    public string SubscribeLink;
    public string AddToFriendsLink;
    public string AddToFriendText = string.Empty;
    public string SendMessageLink;
    public string ColorScheme = string.Empty;

    public Photo DefaultPhoto;
    public Video DefaultVideo;

    public string FriendLister = string.Empty;
    public string WebMemberID = string.Empty;
    public bool IsLoggedIn = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ViewMaster));
        
        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        // set the default forwarding if the member is not logged in
        LoginUrl = @"signup.aspx?u=" + Request.Url.AbsoluteUri;
        SubscribeLink = LoginUrl;
        SendMessageLink = LoginUrl;
        AddToFriendsLink = LoginUrl;

        SetVariables();
        SetFriendShipStatus();
        CreateEmbeddedLinks();       
        SetColorScheme();

        if (member != null)
        {
            if (ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }        
    }

    private void SetColorScheme()
    {
        MemberProfile ViewingMemberProfile = ViewingMember.GetProfile();
        ProfileScheme scheme = ProfileScheme.GetScheme(ViewingMemberProfile.ColorScheme);
        ColorScheme = "background-color:" + scheme.BackgroundColor + ";\r\n" + "border-color:" + scheme.BorderColor + ";\r\n"; 
    }

    private void SetFriendShipStatus()
    {
        bool friendStatus = Friend.IsFriend(member.MemberID, ViewingMember.MemberID);

        if (friendStatus)
        {
            FriendshipStatus = FriendRequestStatus.AlreadyFriend;
            return;
        }

        //friendStatus = FriendRequest.IsFriendRequestSent(member.MemberID, ViewingMember.MemberID);

        //if (friendStatus)
        //{
        //    FriendshipStatus = FriendRequestStatus.RequestSent;
        //    return;
        //}
        
    }

    private void SetVariables()
    {
        string strWebMemberID = Request.Params["m"];
        string strWebVideoID = Request.Params["v"];
        string strPhotoID = Request.Params["p"];
        string strLiveBroadcastID = Request.Params["l"];

        if (strWebMemberID != null)
        {
            ViewingMember = Member.GetMembersViaWebMemberIDWithFullJoin(strWebMemberID);
            //ProfileDefaultPageType = DefaultPageType.Member;
            //ViewingMemberProfile = ViewingMember.MemberProfile[0];
            //DefaultNumberOfViews = (++ViewingMemberProfile.NumberOfViews).ToString();
            //ViewingMemberProfile.Save();
            //PermaLink = WebRoot + "view.aspx?m=" + strWebMemberID;
            MemberSubscribers = GetSubscriberLister();

            GenerateFriendLister();            
        }
        else if (strWebVideoID != null)
        {
            DefaultVideo = Video.GetVideoByWebVideoIDWithJoin(strWebVideoID);
            ViewingMember = new Member(DefaultVideo.MemberID);
            //ViewingMemberProfile = ViewingMember.MemberProfile[0];
            //ProfileDefaultPageType = DefaultPageType.Video;
            //DefaultVideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            //DefaultMediaID = strWebVideoID;
            //DefaultVoteCount = DefaultVideo.TotalVoteScore.ToString();
            //VideoURL = DefaultVideo.VideoResourceFile.FullyQualifiedURL;
            //DefaultNumberOfViews = (++DefaultVideo.NumberOfViews).ToString();
            //DefaultVideo.Save();
            //PermaLink = WebRoot + "view.aspx?v=" + strWebVideoID;
            //EmbedLink = @"<object width=""480"" height=""400""><param name=""movie"" value=""http://www.next2friends.com/flvplayer.swf""></param><param name=""wmode"" value=""transparent""></param><embed src=""http://www.next2friends.com/flvplayer.swf?file=" + VideoURL + @""" type=""application/x-shockwave-flash"" wmode=""transparent"" width=""480"" height=""400""></embed></object>";

            //MainTitle = DefaultVideo.Title;
            //MainSubTitle = DefaultVideo.Description;

            if (IsLoggedIn)
            {
                //ReportAbuseLink = "ReportAbuse.aspx?r=" + strWebVideoID;
            }
            else
            {
                //ReportAbuseLink = @"signup.aspx?u=ReportAbuse.aspx?r=" + strWebVideoID + "&url=" + Request.Url.AbsoluteUri;
            }
        }
        else if (strPhotoID != null)
        {
            DefaultPhoto = Photo.GetPhotoByWebPhotoIDWithJoin(strPhotoID);

            //DefaultNumberOfViews = (++DefaultPhoto.NumberOfViews).ToString();
            //DefaultPhoto.Save();

            ViewingMember = new Member(DefaultPhoto.MemberID);
            //ViewingMemberProfile = ViewingMember.MemberProfile[0];
            //ProfileDefaultPageType = DefaultPageType.Photo;
            //DefaultPhotoURL = ParallelServer.Get(DefaultPhoto.PhotoResourceFile.FullyQualifiedURL) + DefaultPhoto.PhotoResourceFile.FullyQualifiedURL;
            //DefaultMediaID = strPhotoID;
            //DefaultVoteCount = DefaultPhoto.TotalVoteScore.ToString();
            //PermaLink = WebRoot + "view.aspx?p=" + strPhotoID;

            MainTitle = DefaultPhoto.Caption;

            //MainSubTitle = "From Gallery " + DefaultPhoto.;

            if (IsLoggedIn)
            {
                //ReportAbuseLink = "ReportAbuse.aspx?r=" + strPhotoID;
            }
            else
            {
                //ReportAbuseLink = @"signup.aspx?u=ReportAbuse.aspx?r=" + strPhotoID + "&url=" + Request.Url.AbsoluteUri;
            }
        }
        else if (strLiveBroadcastID != null)
        {
            VideoURL = strLiveBroadcastID;
            LiveBroadcast live = LiveBroadcast.GetLiveBroadcastByWebLiveBroadcastID(strLiveBroadcastID);

            ViewingMember = new Member(live.MemberID);
            //ViewingMemberProfile = ViewingMember.MemberProfile[0];
            //ProfileDefaultPageType = DefaultPageType.LiveBroadcast;
            //PermaLink = ASP.global_asax.WebRoot + "view.aspx?v="+strWebVideoID;
        }
        else
        {
            throw new Exception("No WebID");
            //ViewingMember = new Member(1);
            //ProfileDefaultPageType = DefaultPageType.Member;
            //PermaLink = ASP.global_asax.WebRoot + "view.aspx?v="+strWebVideoID;
        }

        NumberOfComments = AjaxComment.GetNumberOfMemberCommentByMemberID(ViewingMember.MemberID);
        ViewingMemberProfile = ViewingMember.MemberProfile[0];
             
        try
        {

            NumberOfVideos = ViewingMemberProfile.NumberOfVideos;            
            NumberOfPhotos = ViewingMemberProfile.NumberOfPhotos;
            NumberOfFriends = FriendRequest.GetNumberOfFriends(ViewingMember.MemberID);
        }
        catch { }



        try
        {
            ResourceFile PhotoRes = new ResourceFile(ViewingMember.ProfilePhotoResourceFileID);
            PhotoURL = ParallelServer.Get(PhotoRes.FullyQualifiedURL) + PhotoRes.FullyQualifiedURL;
            LargePhotoURL = ParallelServer.Get("/pmed/" + PhotoRes.FileName) + @"user/" + ViewingMember.NickName + "/pmed/" + PhotoRes.FileName;
        }
        catch { }

        ViewingMemberProfile = ViewingMember.MemberProfile[0];

        ViewingMemberProfile.NumberOfViews++;
    }

    /// <summary>
    /// Creates a lister with members friends
    /// </summary>
    public void GenerateFriendLister()
    {
        List<Member> Friends = Member.GetAllFriendsByMemberIDForPageLister(ViewingMember.MemberID);
        //NumberOfFriends = Friends.Count;

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            if (Friends.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[5];

            parameters[0] = Friends[i].WebMemberID;
            parameters[1] = ParallelServer.Get(Friends[i].DefaultPhoto.FullyQualifiedURL) + Friends[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = Friends[i].NickName;
            parameters[3] = Friends[i].ISOCountry;
            parameters[4] = TimeDistance.TimeAgo(Friends[i].LastOnline);

            string HTMLItem = @"<li><a href='view.aspx?m={0}'>
                            <img src='{1}' alt='friend' width='45' height='45' /></a>
                            <p>
                                <a href='view.aspx?m={0}'><strong>{2}</strong></a><br />
                                Active: <span style='notes'>{4}</span><br />
                                Country: <span style='metadata'>{3}</span></p>
                        </li>";

            //<p class='notes'>You and Lawrence made friend {}. <br />

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        FriendLister = sbHTMLList.ToString();
    }

    private string GetSubscriberLister()
    {
        List<SubscriptionItem> Subscribers = SubscriptionMember.GetSubscriptionMembersByMemberID(ViewingMember.MemberID);

        NumberOfMemberSubscribers = Subscribers.Count.ToString();

        StringBuilder sbHTML = new StringBuilder();

        for (int i = 0; i < 4; i++)
        {
            if (Subscribers.Count <= i)
            {
                break;
            }

            object[] parameters = new object[5];

            parameters[0] = Subscribers[i].WebMemberID;
            parameters[1] = Subscribers[i].NickName;
            parameters[2] = ParallelServer.Get(Subscribers[i].PhotoURL) + Subscribers[i].PhotoURL;
            parameters[3] = Subscribers[i].ISOCountry;
            parameters[4] = TimeDistance.TimeAgo(Subscribers[i].LastOnline);




            sbHTML.AppendFormat(@"<li><img src='{2}' alt='{1}' width='45' height='45' />
                            <p>
                                <a href='view.aspx?m={0}'><strong>{1}</strong></a><br />
                                Logged in: <span style='notes'>{4}</span><br />
                                Country:<span style='metadata'>{3}</span></p></li>", parameters);
        }

        return sbHTML.ToString();
    }
     


    protected void btnPhotoChange_Click(object sender, EventArgs e)
    {
        Member member = (Member)Session["Member"];

        if (member != null)
        {
            if (filePhoto.HasFile)
            {
                System.Drawing.Image ProfilePhoto = Photo.ByteArrayToImage(filePhoto.FileBytes);
                Photo.ProcessProfilePhoto(member, ProfilePhoto);
                Session["Member"] = member;
                ResourceFile PhotoRes = new ResourceFile(member.ProfilePhotoResourceFileID);
                PhotoURL = PhotoRes.FullyQualifiedURL;

            }
        }
    }

    private void CreateEmbeddedLinks()
    {
        if (member != null)
        {
            SubscribeLink = @"javascript:subscribeToMember('" + ViewingMember.WebMemberID + "', this);";
            SendMessageLink = @"Inbox.aspx?s=" + ViewingMember.WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);

            if (FriendshipStatus == FriendRequestStatus.NotAFriend)
            {
                AddToFriendsLink = @"javascript:addTofriends('" + ViewingMember.WebMemberID + "', this);";
                AddToFriendText = "Send Friend Request";
            }
            else if( FriendshipStatus == FriendRequestStatus.RequestSent )
            {
                AddToFriendsLink = "javascript:void(0);";
                AddToFriendText = "Request Sent";
            }
            else if( FriendshipStatus == FriendRequestStatus.AlreadyFriend )
            {
                AddToFriendsLink = "javascript:void(0);";
                AddToFriendText = "Already a Friend";
            }

        }
    }

    /// <summary>
    /// Creates a friend request for the Member
    /// </summary>
    /// <param name="WebMemberID">The WebMemberID of the friend request</param>
    /// <returns>1 = OK, 2 = Not logged in, 3 = already a friend</returns>
    [AjaxPro.AjaxMethod]
    public bool AddToFriends(string WebMemberID)
    {
        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
        member = (Member)HttpContext.Current.Session["Member"];

        bool MadeFriendRequest = FriendRequest.CreateWebFriendRequest(member.MemberID, ViewingMember.MemberID);

        return MadeFriendRequest;
    }

    [AjaxPro.AjaxMethod]
    public void ChangeSkin(string WebMemberID, int ColorIndex)
    {
        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
        member = (Member)HttpContext.Current.Session["Member"];

        if (Utility.IsMe(ViewingMember, member))
        {
            MemberProfile memberProfile = member.GetProfile();

            if (ProfileScheme.IsValidSchemeValue(ColorIndex))
            {
                memberProfile.ColorScheme = ColorIndex;
                memberProfile.Save();
            }

        }

    }

    /// <summary>
    /// Creates a subscribe request for the member
    /// </summary>
    /// <param name="WebMemberID">The WebMemberID of the subscribe request</param>
    /// <returns>1 = OK, 2 = Not logged in, 3 = already subscribed</returns>
    [AjaxPro.AjaxMethod]
    public int SubscribeToMember(string WebMemberID)
    {
        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
        member = (Member)HttpContext.Current.Session["Member"];

        SubscriptionMember subscription = new SubscriptionMember();

        subscription.MemberID = member.MemberID;
        subscription.SubscribeToMemberID = ViewingMember.MemberID;
        subscription.DTCreated = DateTime.Now;
        subscription.SaveWithCheck();

        return 1;
    }
}

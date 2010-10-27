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
    public string TagLine = "";

    public string MainTitle = string.Empty;
    public string MainSubTitle = string.Empty;

    public FriendRequestStatus FriendshipStatus = FriendRequestStatus.NotAFriend;
    public bool ShowSubscribe = true;

    public string LoginUrl;

    public string PhotoURL;
    public string LargePhotoURL;

    public string VideoURL;    
    public string NumberOfMemberSubscribers = string.Empty;

    public string PageComments = string.Empty;
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
    public bool ShowAge = true;
    public bool ShowCountry = true;

    public Photo DefaultPhoto;
    public Video DefaultVideo;
    public BlogEntry DefaultBlog;

    public int Distance = 0;

    public string WebMemberID = string.Empty;

    public bool IsLoggedIn = false;
    public DefaultPageType ProfileDefaultPageType;

    // Fix so that comments are posted on video, photo and pages other then 
    // wall & member the wall/member comment count doesn't change
    public string spanNumberOfComments1Fix = "";

    public string HTMLTitle { set { Master.HTMLTitle = value; } }
    public string MetaDescription { set { Master.MetaDescription = value; } }
    public string MetaKeywords { set { Master.MetaKeywords = value; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ViewMaster));
        


        // set the default forwarding if the member is not logged in
        LoginUrl = @"/signup/?u=" + Request.Url.AbsoluteUri;
        SubscribeLink = LoginUrl;
        SendMessageLink = LoginUrl;
        AddToFriendsLink = LoginUrl;
        AddToFriendText = "Send Friend Request";

        

        if (IsLoggedIn)
        {
            SetFriendShipStatus();

            try
            {
               // Distance = (int)Utility.GetDistance(ViewingMember, member);
            }
            catch { }
        }
        CreateEmbeddedLinks();
        SetColorScheme();
        //NumberOfComments = GetCommentCount();



        if (ProfileDefaultPageType != DefaultPageType.Member)
            //any string to change the id of element in HTML
            spanNumberOfComments1Fix = "h";

        if (ViewingMember.AccountType == 1 || ViewingMember.DOB.Year == 1900)
        {
            ShowAge = false;
        }

        if (ViewingMember.ISOCountry == "UNS")
        {
            ShowCountry = false;
        }

        try
        {
            TagLine = ViewingMember.MemberProfile[0].TagLine;
        }
        catch { }

        if (!Utility.IsMe(ViewingMember, member))
        {
            Utility.ContentViewed(member, ViewingMember.MemberID, CommentType.Member);
        }
    }

    protected override void OnInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        ExtractURLParams();

        if (member != null)
        {
            IsLoggedIn = true;
        }

        if (member != null)
        {
            if (ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }

        if (IsMyPage)
        {
            Master.SetSkin("profile");
        }
        else
        {
            Master.SetSkin(string.Empty);
        }
        base.OnInit(e);
    }

    private void SetColorScheme()
    {
        MemberProfile ViewingMemberProfile = ViewingMember.GetProfile();
        ProfileScheme scheme = ProfileScheme.GetScheme(ViewingMemberProfile.ColorScheme);
        ColorScheme = "background-color:" + scheme.BackgroundColor + ";\r\n" +"border-color:" + scheme.BorderColor + ";\r\n"; 
    }

    private void SetFriendShipStatus()
    {
        bool friendStatus = Friend.IsFriend(member.MemberID, ViewingMember.MemberID);

        if (friendStatus)
        {
            FriendshipStatus = FriendRequestStatus.AlreadyFriend;
            return;
        }       
    }

    private void ExtractURLParams()
    {
        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);
        
        // if no member name was givent then it is likely to be a video page
        if (ViewingMember == null)
        {
            // get the video and extact the member 
            DefaultVideo = ExtractPageParams.GetVideo(this.Page, this.Context);
            ViewingMember = new Member(DefaultVideo.MemberID);
        }

        NumberOfComments = AjaxComment.GetNumberOfCommentByObjectID(ViewingMember.MemberID,(int)CommentType.Wall);
        NumberOfMemberSubscribers = SubscriptionMember.GetSubscriberCountByMemberID(ViewingMember.MemberID).ToString();
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


    private void CreateEmbeddedLinks()
    {
        if (member != null)
        {
            SubscribeLink = @"javascript:subscribeToMember('" + ViewingMember.WebMemberID + "', this);";
            SendMessageLink = @"/inbox.aspx?s=" + ViewingMember.WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);

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

    [AjaxPro.AjaxMethod]
    public int AddToFavourites(string type, string WebID)
    {
        member = (Member)HttpContext.Current.Session["Member"];
        CommentType contentType = (CommentType)Enum.Parse(typeof(CommentType), type);

        Favourite favourite = new Favourite();

        favourite.MemberID = member.MemberID;
        favourite.TheFavouriteObjectID = GetObjectID(contentType, WebID);
        favourite.ObjectType = (int)contentType;
        favourite.DTCreated = DateTime.Now;
        favourite.SaveWithCheck();

        return 1;
    }

    private static int GetObjectID(CommentType type, string WebID)
    {
        if (type == CommentType.Wall)
        {
            Member m = Member.GetMemberViaWebMemberID(WebID);
            return m.MemberID;
        }
        else if (type == CommentType.Photo)
        {
            Photo p = Photo.GetPhotoByWebPhotoIDWithJoin(WebID);
            return p.PhotoID;
        }
        else if (type == CommentType.Video)
        {
            Video v = Video.GetVideoByWebVideoIDWithJoin(WebID);
            return v.VideoID;
        }
        else if (type == CommentType.AskAFriend)
        {
            AskAFriend aaf = AskAFriend.GetAskAFriendByWebAskAFriendID(WebID);
            return aaf.AskAFriendID;
        }
        else if (type == CommentType.Blog)
        {
            BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(WebID);
            return blog.BlogEntryID;
        }

        return -1;
    }

    [AjaxPro.AjaxMethod]
    public int Vote(string param, string WebID, bool up)
    {
        member = (Member)HttpContext.Current.Session["Member"];

        Vote v = new Vote();
        v.MemberID = member.MemberID;
        v.Value = (up) ? 1 : -1;

        switch (param)
        {
            case "v":
                v.VideoID = Video.GetVideoIDByWebVideoID(WebID);
                break;
            case "p":
                v.PhotoID = Photo.GetPhotoIDByWebPhotoID(WebID);
                break;
        }


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

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

public enum NSDefaultPageType { None, Video, Photo, Member, NSpot }

public partial class NSpotPage : System.Web.UI.Page
{
    public Member ViewingMember;
    public NSpot ViewingNSpot;
    public MemberProfile ViewingMemberProfile;
    public string PhotoURL;
    public string VideoURL;
    public Member member;
    public string PageComments = string.Empty;
    public string MemberSubscribers = string.Empty;
    public string NumberOfMemberSubscribers = string.Empty;
    public string DefaultLister = string.Empty;
    public int NumberOfComments = 0;
    public int NumberOfVideos = 0;
    public int NumberOfPhotos = 0;
    public int NumberOfMembers = 0;

    public string PermaLink = string.Empty;

    public bool IsMyPage = false;
    public bool IsLoggedIn = false;
    public NSDefaultPageType ProfileDefaultPageType;
    public string DefaultPhotoURL;
    public string DefaultVideoURL;

    public string LoginUrl;
    public string SubscribeLink;
    public string SendMessageLink;
    public string BlockMemberLink;
    public string AddToFriendsLink;
    public string AddFavouritesLink;
    public string ReportAbuseLink;

    public string DefaultVoteCount = "0";
    public string DefaultVoteUpLink;
    public string DefaultVoteDownLink;

    public string DefaultMediaID;
    public Photo DefaultPhoto;
    public Video DefaultVideo;
    public string DefaultMemberLister = string.Empty;
    public string LargePhotoURL;

    public bool LoadLargeNSpotPhoto = false;

    public string DefaultNewCommentParams;
    public string DefaultNumberOfViews = "0";

    public string WebRoot = "http://prerelease.next2friends.com/";

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(NSpotPage));

        member = (Member)Session["Member"];

        // set the default forwarding if the member is not logged in
        LoginUrl = @"signup.aspx?u=" + Request.Url.AbsoluteUri;
        SubscribeLink = LoginUrl;
        SendMessageLink = LoginUrl;
        BlockMemberLink = LoginUrl;
        AddToFriendsLink = LoginUrl;
        DefaultVoteUpLink = LoginUrl;
        DefaultVoteDownLink = LoginUrl;
        AddFavouritesLink = LoginUrl;

        string strWebNSpotID = Request.Params["n"];
        // load the members photo
        string strMemberPhoto = Request.Params["np"];

        if (strWebNSpotID != null)
        {
            ViewingNSpot = NSpot.GetNSpotByNSpotWebIDWithJoin(strWebNSpotID);
            PhotoURL = ViewingNSpot.PhotoResourceFile.FullyQualifiedURL;
            ProfileDefaultPageType = NSDefaultPageType.NSpot;
            DefaultNumberOfViews = (++ViewingNSpot.NumberOfViews).ToString();
            ViewingNSpot.Save();

            PermaLink = WebRoot + "nspot.aspx?m=" + strWebNSpotID;

            GenerateNSpotMemberLister();
        }
      

        if (strMemberPhoto != null)
        {
            ResourceFile PhotoRes = new ResourceFile(ViewingNSpot.PhotoResourceFileID);
            LoadLargeNSpotPhoto = true;
            LargePhotoURL = @"user/" + ViewingNSpot.Member.NickName + "/nslrge/" + PhotoRes.FileName;
        }

        if (member != null)
        {
            IsLoggedIn = true;

            if (ViewingNSpot.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }

        GeneratePageComments();
        //CreateEmbeddedLinks();
    }


    /// <summary>
    /// Creates a lister with members friends
    /// </summary>
    public void GenerateNSpotMemberLister()
    {
        List<Member> NspotMembers = NSpot.GetNSpotMembersByNSpotIDWithJoin(ViewingNSpot.NSpotID);

        NumberOfMembers = NspotMembers.Count;

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            if (NspotMembers.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[3];

            parameters[0] = NspotMembers[i].WebMemberID;
            parameters[1] = NspotMembers[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = NspotMembers[i].NickName;

            string HTMLItem = @"<li><a href='view.aspx?m={0}'>
								<img src='{1}' alt='friend' width='45' height='45' /></a>
								<p><a href='view.aspx?m={0}'><strong>{2}</strong></a><br />
								Videos: <a href='#'>2</a><br />
								Friends: <a href='#'>39</a></p>

							</li>";


            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        DefaultMemberLister = sbHTMLList.ToString();
    }


    //    /// <summary>
    //    /// Creates a subscribe request for the member
    //    /// </summary>
    //    /// <param name="WebMemberID">The WebMemberID of the subscribe request</param>
    //    /// <returns>1 = OK, 2 = Not logged in, 3 = already subscribed</returns>
    //    [AjaxPro.AjaxMethod]
    //    public int SubscribeToMember(string WebMemberID)
    //    {
    //        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
    //        member = (Member)Session["Member"];

    //        SubscriptionMember subscription = new SubscriptionMember();

    //        subscription.MemberID = member.MemberID;
    //        subscription.SubscribeToMemberID = ViewingMember.MemberID;
    //        subscription.DTCreated = DateTime.Now;
    //        subscription.SaveWithCheck();

    //        return 1;
    //    }

    //    /// <summary>
    //    /// Creates a favourite request for the member
    //    /// </summary>
    //    /// <param name="WebMemberID">The WebMemberID of the favourite request</param>
    //    /// <returns>1 = OK, 2 = Not logged in, 3 = already subscribed</returns>
    //    [AjaxPro.AjaxMethod]
    //    public int AddToFavourites(string WebMemberID)
    //    {
    //        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
    //        member = (Member)Session["Member"];

    //        FavouriteMember subscription = new FavouriteMember();

    //        subscription.MemberID = member.MemberID;
    //        subscription.TheFavouriteMemberID = ViewingMember.MemberID;
    //        subscription.DTCreated = DateTime.Now;
    //        subscription.SaveWithCheck();

    //        return 1;
    //    }

    //    /// <summary>
    //    /// Creates a member block
    //    /// </summary>
    //    /// <param name="WebMemberID">The WebMemberID of the block request</param>
    //    /// <returns>1 = OK, 2 = Not logged in, 3 = already blocked</returns>
    //    [AjaxPro.AjaxMethod]
    //    public int BlockMember(string WebMemberID)
    //    {
    //        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
    //        member = (Member)Session["Member"];

    //        MemberBlock block = new MemberBlock();

    //        block.MemberID = member.MemberID;
    //        block.BlockMemberID = ViewingMember.MemberID;
    //        block.DTCreated = DateTime.Now;

    //        block.SaveWithCheck();

    //        return 1;
    //    }

    //    /// <summary>
    //    /// Creates a friend request for the Member
    //    /// </summary>
    //    /// <param name="WebMemberID">The WebMemberID of the friend request</param>
    //    /// <returns>1 = OK, 2 = Not logged in, 3 = already a friend</returns>
    //    [AjaxPro.AjaxMethod]
    //    public bool AddToFriends(string WebMemberID)
    //    {
    //        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);
    //        member = (Member)Session["Member"];

    //        bool MadeFriendRequest = FriendRequest.CreateWebFriendRequest(member.MemberID, ViewingMember.MemberID);

    //        return MadeFriendRequest;
    //    }


    //    private void CreateEmbeddedLinks()
    //    {
    //        if (member != null)
    //        {
    //            SubscribeLink = @"javascript:subscribeToMember('" + ViewingMember.WebMemberID + "', this);";
    //            SendMessageLink = @"Inbox.aspx?s=" + ViewingMember.WebMemberID;
    //            BlockMemberLink = @"javascript:blockMember('" + ViewingMember.WebMemberID + "', this);";
    //            AddToFriendsLink = @"javascript:addTofriends('" + ViewingMember.WebMemberID + "', this);";
    //            AddFavouritesLink = @"javascript:addToFavourites('" + ViewingMember.WebMemberID + "', this);";

    //            string param = ProfileDefaultPageType.ToString().Substring(0, 1).ToLower();

    //            DefaultVoteUpLink = @"javascript:vote('" + param + "','" + DefaultMediaID + "', true);";
    //            DefaultVoteDownLink = @"javascript:vote('" + param + "','" + DefaultMediaID + "', false);";

    //        }
    //    }

    //    [AjaxPro.AjaxMethod]
    //    public int Vote(string param, string WebID, bool up)
    //    {
    //        member = (Member)Session["Member"];

    //        Vote v = new Vote();
    //        v.MemberID = member.MemberID;
    //        v.Value = (up) ? 1 : -1;

    //        switch (param)
    //        {
    //            case "v":
    //                v.VideoID = Video.GetVideoIDByWebVideoID(WebID);
    //                break;
    //            case "p":
    //                v.PhotoID = Photo.GetPhotoIDByWebPhotoID(WebID);
    //                break;
    //        }


    //        bool value = v.SaveWithCheck();

    //        if (value)
    //        {
    //            if (up)
    //                return 1;
    //            else
    //                return -1;
    //        }
    //        else
    //        {
    //            return 0;
    //        }
    //    }

    /// <summary>
    /// Gets the Comments from the page from either Member, Photo or video (depending on the default page item)
    /// </summary>
    private void GeneratePageComments()
    {
        AjaxComment[] comments = new AjaxComment[0];

        comments = AjaxComment.GetNSpotCommentByNSpotIDWithJoin(ViewingNSpot.NSpotID);

        DefaultNewCommentParams = "'n','" + ViewingNSpot.WebNSpotID + "'";

        NumberOfComments = comments.Length;

        for (int i = 0; i < comments.Length; i++)
        {
            PageComments += comments[i].HTML;
        }
    }

    //    /// <summary>
    //    /// Concatonates the videos into HTML for the lister control
    //    /// </summary>
    //    private string GetVideoLister()
    //    {
    //        List<Next2Friends.Data.Video> videos = Next2Friends.Data.Video.GetTopVideosByMemberIDWithJoin(ViewingMember.MemberID);
    //        NumberOfVideos = videos.Count;

    //        StringBuilder sbHTML = new StringBuilder();

    //        for (int i = 0; i < 6; i++)
    //        {
    //            if (videos.Count <= i)
    //            {
    //                break;
    //            }

    //            object[] parameters = new object[7];

    //            parameters[0] = videos[i].ThumbnailResourceFile.FullyQualifiedURL;
    //            parameters[1] = videos[i].Duration.ToString();
    //            parameters[2] = videos[i].ShortTitle;
    //            parameters[3] = TimeDistance.TimeAgo(videos[i].DTCreated);
    //            parameters[4] = videos[i].ShortDescription;
    //            parameters[5] = videos[i].NumberOfComments;
    //            parameters[6] = videos[i].WebVideoID;

    //            sbHTML.AppendFormat(@"<li>
    //								<div class='vid_thumb'> <a href='view.aspx?v={6}'><img src='{0}' width='124' height='91' alt='thumb' /></a></div>
    //								<div class='vid_info'>
    //									<h3><a href='view.aspx?v={6}'>{2}</a></h3>
    //									<p class='timestamp'>{3}</p>
    //
    //									<p>{4}</p>
    //									<p class='metadata'>Views: {5} Comments: <a href='#'>3</a></p>
    //								</div>
    //							</li>", parameters);

    //        }

    //        string jCarousel = @"<script type='text/javascript'>
    //				$('.thumbs').jCarouselLite({
    //					btnNext: '.featured_nspots .next',
    //					btnPrev: '.featured_nspots .prev',
    //					visible: 4,
    //					speed: 500,
    //					scroll: 1,
    //					circular: false
    //				});    
    //				</script>";

    //        //string returnString = sbHTML.ToString() + "<ul><div class='prev'></div><div class='next'></div></ul>" + jCarousel;
    //        string returnString = sbHTML.ToString();

    //        return returnString;
    //    }

    //    /// <summary>
    //    /// Concatonates the NSpots into HTML for the lister control
    //    /// </summary>
    //    private string GetNSpotLister()
    //    {

    //        return string.Empty;
    //    }

    //    /// <summary>
    //    /// returns the gallyer listers
    //    /// </summary>
    //    private string GetPhotoLister()
    //    {
    //        #region Old lister code
    //        //        List<Next2Friends.Data.Photo> photos = Next2Friends.Data.Photo.GetPhotosByMemberIDWithJoin(ViewingMember.MemberID,0);

    //        //        StringBuilder sbHTML = new StringBuilder();

    //        //        for (int i = 0; i < 6; i++)
    //        //        {
    //        //            if (photos.Count <= i)
    //        //            {
    //        //                break;
    //        //            }

    //        //            object[] parameters = new object[7];

    //        //            parameters[0] = photos[i].ThumbnailResourceFile.FullyQualifiedURL;
    //        //            parameters[1] = TimeDistance.TimeAgo(photos[i].CreatedDT);
    //        //            parameters[2] = photos[i].WebPhotoID;
    //        //            parameters[3] = photos[i].NumberOfViews;
    //        //            parameters[4] = photos[i].NumberOfComments;


    //        //            sbHTML.AppendFormat(@"<li>
    //        //							<div class='vid_thumb'> <a href='view.aspx?p={2}'><img src='{0}' width='124' height='91' alt='thumb' /></a></div>
    //        //							<div class='vid_info'>
    //        //								<h3><a href='view.aspx?p={2}'>xxx</a></h3>
    //        //								<p class='timestamp'>{1}</p>
    //        //								<p class='metadata'>Views: {3} Comments: {4}</p>
    //        //							</div>
    //        //						</li>", parameters);

    //        //        }

    //        //        return sbHTML.ToString();
    //        #endregion

    //        string GalleryHTML = @"<object classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' codebase='http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0' width='628' height='500' id='gallery' align='middle'>
    //                        <param name='allowScriptAccess' value='sameDomain' />
    //                        <param name='movie' value='gallery.swf' />
    //                        <param name='quality' value='high' />
    //                        <param name='bgcolor' value='#ffffff' />
    //                        <param name='FlashVars' value='xmlfile=MyImagesXML.aspx' />
    //                        <embed src='gallery.swf' flashvars='xmlfile=MyImagesXML.aspx?m=" + ViewingMember.WebMemberID + @"' quality='high' bgcolor='#ffffff' width='628' height='500' name='gallery' align='middle' allowScriptAccess='sameDomain' type='application/x-shockwave-flash' pluginspage='http://www.macromedia.com/go/getflashplayer' />
    //                       </object>";

    //        return GalleryHTML;
    //    }

    //    private string GetSubscriberLister()
    //    {
    //        List<SubscriptionItem> Subscribers = SubscriptionMember.GetSubscriptionMembersByMemberID(ViewingMember.MemberID);

    //        NumberOfMemberSubscribers = Subscribers.Count.ToString();

    //        StringBuilder sbHTML = new StringBuilder();

    //        for (int i = 0; i < 4; i++)
    //        {
    //            if (Subscribers.Count <= i)
    //            {
    //                break;
    //            }

    //            object[] parameters = new object[3];

    //            parameters[0] = Subscribers[i].WebMemberID;
    //            parameters[1] = Subscribers[i].NickName;
    //            parameters[2] = Subscribers[i].PhotoURL;

    //            sbHTML.AppendFormat(@"<li><img src='{2}' alt='{1}' width='45' height='45' />
    //                            <p>
    //                                <a href='view.aspx?m=0'><strong>{1}</strong></a><br />
    //                                </p></li>", parameters);
    //        }

    //        return sbHTML.ToString();
    //    }

    //    /// <summary>
    //    /// Returns the tab content for the member
    //    /// </summary>
    //    /// <param name="TabType"></param>
    //    /// <returns></returns>
    //    [AjaxPro.AjaxMethod]
    //    public TabContents GetListerContent(string WebMemberID, int TabType)
    //    {
    //        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);

    //        TabContents tabContents = new TabContents();

    //        if (TabType == 1)
    //        {
    //            tabContents.HTML = GetVideoLister();
    //        }
    //        else if (TabType == 2)
    //        {
    //            tabContents.HTML = GetPhotoLister();
    //        }
    //        else if (TabType == 3)
    //        {
    //            tabContents.HTML = "Hotspots";
    //        }
    //        else if (TabType == 4)
    //        {
    //            tabContents.HTML = "Live Broadcasts";
    //        }

    //        tabContents.TabType = TabType;

    //        return tabContents;
    //    }


    //    /// <summary>
    //    /// Saves the About me section to the database
    //    /// </summary>
    //    /// <param name="NewText">The new text</param>
    //    /// <returns>The new text</returns>
    //    [AjaxPro.AjaxMethod]
    //    public string UpdateAboutMe(string NewText)
    //    {
    //        member = (Member)Session["Member"];

    //        member.AboutMe = SafeHTML.CleanText(NewText);
    //        member.Save();

    //        Session["Member"] = member;

    //        return SafeHTML.FormatForHTML(NewText);
    //    }

    //    /// <summary>
    //    /// gets the About me section in plain text
    //    /// </summary>
    //    [AjaxPro.AjaxMethod]
    //    public string GetAboutMeText()
    //    {
    //        member = (Member)Session["Member"];

    //        return member.AboutMe;
    //    }

    /// <summary>
    /// Post a new comment to the members page
    /// </summary>
    /// <param name="WebMemberID">The WebMemberID who owns the page</param>
    /// <param name="CommentText">The text body of the comment</param>
    /// <returns>An AjaxComment object populated with all the comments properties</returns>
    [AjaxPro.AjaxMethod]
    public AjaxComment PostComment(string type, string WebID, string CommentText)
    {
        AjaxComment ajaxComment = new AjaxComment();
        member = (Member)Session["Member"];

        if (type == "n")
        {
            NSpot nspot = NSpot.GetNSpotByNSpotWebID(WebID);

            NSpotComment comment = new NSpotComment();

            comment.NSpotID = nspot.NSpotID;
            comment.MemberID = member.MemberID;
            comment.Text = CommentText;
            comment.DTCreated = DateTime.Now;

            comment.Save();

            ajaxComment = AjaxComment.GetAjaxNSpotCommentByNSpotCommentIDWithJoin(comment.NSpotCommentID);

            ajaxComment.TotalNumberOfComments = "0";

        }


        return ajaxComment;
    }


    //    protected void btnPhotoChange_Click(object sender, EventArgs e)
    //    {
    //        Member member = (Member)Session["Member"];

    //        if (member != null)
    //        {
    //            //if (filePhoto.HasFile)
    //            //{
    //            //    System.Drawing.Image ProfilePhoto = Photo.ByteArrayToImage(filePhoto.FileBytes);
    //            //    Photo.ProcessProfilePhoto(member, ProfilePhoto);
    //            //    Session["Member"] = member;
    //            //    ResourceFile PhotoRes = new ResourceFile(member.ProfilePhotoResourceFileID);
    //            //    PhotoURL = PhotoRes.FullyQualifiedURL;

    //            //}
    //        }
    //    }


    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        Master.SkinID = "Community";
        base.OnPreInit(e);
    }

}

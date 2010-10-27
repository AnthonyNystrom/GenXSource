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

public partial class MemberView : System.Web.UI.Page
{
    public string WebRoot = ASP.global_asax.WebServerRoot;
    
    public Member ViewingMember;
    public MemberProfile ViewingMemberProfile;

    public Member member;

    public string AboutMeHTML = string.Empty;
    public string MyLife = string.Empty;
    public string Gender = string.Empty;
    public string LastActive = string.Empty;
    public string ProfileViews = string.Empty;

    public string Hometown = string.Empty;
    public string Country = string.Empty;
    public string DirectUrl = string.Empty;
    public string DirectUrlText = string.Empty;
    public string Nick = string.Empty;
    public string MemberSince = string.Empty;
    public string Movies = string.Empty;
    public string Books = string.Empty;
    public string Music = string.Empty;
    public string OtherHalf = string.Empty;
    public string RelationshipStatus = string.Empty;
    public string MySpaceURL = string.Empty;
    public string FaceBookURL = string.Empty;
    public string BlogURL = string.Empty;
    public string BlogFeedURL = string.Empty;

    public string MySpaceURLText = string.Empty;
    public string FaceBookURLText = string.Empty;
    public string BlogURLText = string.Empty;
    public string BlogFeedURLText = string.Empty;

    public bool IsMyPage = false;
    public string GalleryDetailsHTML = string.Empty;
    public string GalleryListerHTML = string.Empty;

    public string DefaultLister = string.Empty;
    public string DefaultPager = string.Empty;

    public int NumberOfVideos = 0;
    public int NumberOfComments = 0;

    public string MemberSubscribers = string.Empty;
    public string FriendLister = string.Empty;

    public string NumberOfMemberSubscribers = string.Empty;

    public int NumberOfFriends = 0;
    public bool IsLoggedIn = false;

    public string LeftPagerHTML = string.Empty;
    public string RightPagerHTML = string.Empty;

    public bool ShowGalleryArrows = true;

    public string DefaultNewCommentParams;
    public string PageComments = string.Empty;

    public string LoginUrl;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MemberView));
        
        member = (Member)Session["Member"];

        LoginUrl = @"signup.aspx?u=" + Request.Url.AbsoluteUri;
        
        string strWebMemberID = Request.Params["m"];

        ViewingMember = Member.GetMembersViaWebMemberIDWithFullJoin(strWebMemberID);

        Comments1.ObjectId = ViewingMember.MemberID;
        Comments1.ObjectWebId = ViewingMember.WebMemberID;
        Comments1.CommentType = CommentType.Wall;        

        if (member != null)
        {
            IsLoggedIn = true;

            if (ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }

        MemberSubscribers = GetSubscriberLister();

        PopulateMemberVariables();
        GenerateFriendLister();

        List<PhotoCollection> Galleries = PhotoCollection.GetAllPhotoCollectionByMemberID(ViewingMember.MemberID);        
        GetPhotoLister(ViewingMember.WebMemberID, Galleries, 0);

        TabContents tabContents = GetVideoLister(ViewingMember.WebMemberID, 0);
        DefaultLister = tabContents.HTML;
        DefaultPager = tabContents.PagerHTML;        
    }

    private void PopulateMemberVariables()
    {
        MyLife = ViewingMember.MemberProfile[0].MyLife.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        Gender = GetGender(ViewingMember.Gender);
        LastActive = TimeDistance.TimeAgo(ViewingMember.LastOnline);
        ProfileViews = ViewingMember.MemberProfile[0].NumberOfViews.ToString();

        Hometown = ViewingMember.MemberProfile[0].HomeTown;
        Country = ViewingMember.CountryName;
        Nick = ViewingMember.NickName;
        DirectUrl = WebRoot + "?n=" + ViewingMember.NickName;
        DirectUrlText = WebRoot + "?n=" + ViewingMember.NickName;

        Movies = ViewingMember.MemberProfile[0].Movies.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        Books = ViewingMember.MemberProfile[0].Books.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        Music = ViewingMember.MemberProfile[0].Music.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");

        MemberSince = ViewingMember.CreatedDT.ToString("dd MMMM yyyy");
        RelationshipStatus = GetRelationShipStatus(ViewingMember.MemberProfile[0].RelationshipStatus);

        if (ViewingMember.MemberProfile[0].OtherHalfID != -1)
        {
            // Married
            if (ViewingMember.MemberProfile[0].RelationshipStatus == 2)
            {
                RelationshipStatus += " To";
            }
            //Kinda have a thing
            else if (ViewingMember.MemberProfile[0].RelationshipStatus == 6)
            {
                RelationshipStatus += " With";
            }
        }

        OtherHalf = GetOtherHalf(ViewingMember.MemberProfile[0].OtherHalfID);

        BlogFeedURL = ViewingMember.MemberProfile[0].BlogFeedURL;
        BlogFeedURLText = BreakText(BlogFeedURL);

        BlogURL = ViewingMember.MemberProfile[0].BlogURL;
        BlogURLText = BreakText(BlogURL);

        MySpaceURL = ViewingMember.MemberProfile[0].MySpaceURL;
        MySpaceURLText = BreakText(MySpaceURL);

        FaceBookURL = ViewingMember.MemberProfile[0].FaceBookURL;
        FaceBookURLText = BreakText(FaceBookURL);
    }

    private string GetGender(int gender)
    {
        if (gender == 0)
        {
            return "Female";
        }
        else if (gender == 1)
        {
            return "Male";
        }
        else
        {
            return "";
        }
    }

    private string GetRelationShipStatus(int status)
    {
        switch (status)
        {
            case -1:
                return "Not Saying";
            case 1:
                return "Single";
            case 2:
                return "Married";
            case 3:
                return "Divorced";
            case 4:
                return "Dating";
            case 5:
                return "Seeing";
            case 6:
                return "Kinda Have A Thing";
            default:
                return "";
        }
    }

    private string GetOtherHalf(int otherHalfId)
    {
        try
        {
            if (otherHalfId <= 0)
            {
                return "Not Saying";
            }
            else
            {
                Member m = new Member(otherHalfId);
                return "<a href=\"view.aspx?m=" + m.WebMemberID + "\">" + m.NickName + "</a>";
            }
        }
        catch { }

        return "Not Saying";
    }

    private string BreakText(string inText)
    {
        StringBuilder sb = new StringBuilder(inText);
        for (int i = 45; i < sb.Length; i += 48)
        {
            sb.Insert(i, "<br/>");
        }

        return sb.ToString();
    }

    /// <summary>
    /// gets the About me section in plain text
    /// </summary>
    [AjaxPro.AjaxMethod]
    public string GetAboutMeText()
    {
        member = (Member)Session["Member"];

        return member.MemberProfile[0].EmbeddedContent;
    }

    /// <summary>
    /// Creates a lister with members friends
    /// </summary>
    public void GenerateFriendLister()
    {
        List<Member> Friends = Member.GetAllFriendsByMemberIDForPageLister(ViewingMember.MemberID);
        NumberOfFriends = Friends.Count;

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

    [AjaxPro.AjaxMethod]
    public TabContents GetPhotoLister(string WebMemberID, int Page)
    {
        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);

        List<PhotoCollection> Galleries = PhotoCollection.GetAllPhotoCollectionByMemberID(ViewingMember.MemberID);

        return GetPhotoLister(WebMemberID, Galleries, Page);
    }

    /// <summary>
    /// generates the gallery HTML
    /// </summary>
    private TabContents GetPhotoLister(string WebMemberID, List<PhotoCollection> Galleries, int Page)
    {
        StringBuilder sbHTML = new StringBuilder();
        int DisplayNumberOfGalleries = 10;
        int NumberOfGalleries = Galleries.Count + 1;
        int NumberOfPhotos = 0;
        int NumberOfPopulatedGalleries = 0;
        int StartIndex = Page * DisplayNumberOfGalleries;
        int EndIndex = StartIndex + DisplayNumberOfGalleries;

        for (int i = StartIndex; i < EndIndex; i++)
        {
            if (Galleries.Count <= i)
            {
                break;
            }

            // only show galleries with at least one photo
            if (Galleries[i].Photo.Count > 0)
            {
                object[] parameters = new object[5];

                parameters[0] = ParallelServer.Get(Galleries[i].DefaultThumbnailURL) + "user/" + Galleries[i].DefaultThumbnailURL;
                parameters[1] = Galleries[i].WebPhotoCollectionID;
                parameters[2] = Galleries[i].Name;
                parameters[3] = Galleries[i].Photo.Count;
                parameters[4] = Galleries[i].Description;

                sbHTML.AppendFormat(@"<li style='height:182px'><a href='ViewGallery.aspx?g={1}'><img src='{0}' alt='thumb' /></a>
                                <p class='cat_details'><a href='ViewGallery.aspx?g={1}'><strong>{2}</strong> ({3})</a><br />
                                {4}</p>

                            </li>", parameters);
            }
        }



        for (int i = 0; i < Galleries.Count; i++)
        {
            if (Galleries[i].Photo.Count > 0)
            {
                NumberOfPopulatedGalleries++;
                NumberOfPhotos += Galleries[i].Photo.Count;
            }
        }

        if (NumberOfPopulatedGalleries <= 3)
        {
            ShowGalleryArrows = false;
        }


        TabContents tabContents = new TabContents();


        // previous button
        string DisplayPrev = (Page > 1) ? "block" : "none";

        int PrevPage = Page - 1;
        tabContents.PagerHTML = "<li class='gallery_prev'><a style='display:" + DisplayPrev + ";' href='javascript:PageGallery(\"" + WebMemberID + "\"," + PrevPage + "," + NumberOfGalleries + ");'  id='aGallPrev'><img src='images/nspots-prev.gif' alt='previous' /></a></li>";
        RightPagerHTML = tabContents.PagerHTML;

        string DisplayNext = (DisplayNumberOfGalleries < NumberOfGalleries) ? "block" : "none";

        // Next Button
        int NextPage = Page + 1;
        LeftPagerHTML = "<li class='gallery_next'><a style='display:" + DisplayNext + ";' href='javascript:PageGallery(\"" + WebMemberID + "\"," + NextPage + "," + NumberOfGalleries + ");'  id='aGallNext'><img src='images/nspots-next.gif' alt='next' /></a></li>";
        tabContents.PagerHTML = LeftPagerHTML;

        GalleryDetailsHTML = "(" + NumberOfPhotos + " photos in " + NumberOfPopulatedGalleries + " galleries)";
        GalleryListerHTML = sbHTML.ToString();

        tabContents.HTML = GalleryListerHTML;

        return tabContents;
    }

    protected void btnInvite_Click(object sender, EventArgs e)
    {
        string Email1 = txtFriend1.Text;
        string Email2 = txtFriend2.Text;
        string Email3 = txtFriend3.Text;
        string Email4 = txtFriend4.Text;
        string Email5 = txtFriend5.Text;

        ProfileInvite profileInvite1 = new ProfileInvite();
        ProfileInvite profileInvite2 = new ProfileInvite();
        ProfileInvite profileInvite3 = new ProfileInvite();
        ProfileInvite profileInvite4 = new ProfileInvite();
        ProfileInvite profileInvite5 = new ProfileInvite();

        bool Go1 = false;
        bool Go2 = false;
        bool Go3 = false;
        bool Go4 = false;
        bool Go5 = false;

        bool go = true;
        bool AtLeast1 = false;

        if (Email1 != string.Empty)
        {
            AtLeast1 = true;

            if (RegexPatterns.TestEmailRegex(Email1))
            {
                profileInvite1.DTCreated = DateTime.Now;
                profileInvite1.EmailAddress = Email1;
                txtFriend1.CssClass = "form_txt_small";
                Go1 = true;
            }
            else
            {
                go = false;
                txtFriend1.CssClass = "form_txt_small formerror";
            }
        }
        else
        {
            txtFriend1.CssClass = "form_txt_small";
        }


        if (Email2 != string.Empty)
        {
            AtLeast1 = true;

            if (RegexPatterns.TestEmailRegex(Email2))
            {
                profileInvite2.DTCreated = DateTime.Now;
                profileInvite2.EmailAddress = Email2;
                txtFriend2.CssClass = "form_txt_small";
                Go2 = true;
            }
            else
            {
                go = false;
                txtFriend2.CssClass = "form_txt_small formerror";
            }
        }
        else
        {
            txtFriend2.CssClass = "form_txt_small";
        }


        if (Email3 != string.Empty)
        {
            AtLeast1 = true;

            if (RegexPatterns.TestEmailRegex(Email3))
            {
                profileInvite3.DTCreated = DateTime.Now;
                profileInvite3.EmailAddress = Email3;
                txtFriend3.CssClass = "form_txt_small";
                Go3 = true;
            }
            else
            {
                go = false;
                txtFriend3.CssClass = "form_txt_small formerror";
            }
        }
        else
        {
            txtFriend3.CssClass = "form_txt_small";
        }


        if (Email4 != string.Empty)
        {
            AtLeast1 = true;

            if (RegexPatterns.TestEmailRegex(Email4))
            {
                profileInvite4.DTCreated = DateTime.Now;
                profileInvite4.EmailAddress = Email4;
                txtFriend4.CssClass = "form_txt_small";
                Go4 = true;
            }
            else
            {
                go = false;
                txtFriend4.CssClass = "form_txt_small formerror";
            }
        }
        else
        {
            txtFriend4.CssClass = "form_txt_small";
        }

        if (Email5 != string.Empty)
        {
            AtLeast1 = true;

            if (RegexPatterns.TestEmailRegex(Email5))
            {
                profileInvite5.DTCreated = DateTime.Now;
                profileInvite5.EmailAddress = Email5;
                txtFriend5.CssClass = "form_txt_small";
                Go5 = true;

            }
            else
            {
                go = false;
                txtFriend5.CssClass = "form_txt_small formerror";
            }
        }
        else
        {
            txtFriend5.CssClass = "form_txt_small";
        }

        if (!AtLeast1)
        {

            litInvite.Text = "<p class='error_alert'>Please enter at least one email address</p>";
        }
        else if (go)
        {
            litInvite.Text = "Thank you, we have invited your friends</p>";


            string AttachedMessage = (txtInviteMessage.Text == "Your personal message here!") ? string.Empty : txtInviteMessage.Text;

            profileInvite1.CustomMessage = AttachedMessage;
            profileInvite2.CustomMessage = AttachedMessage;
            profileInvite3.CustomMessage = AttachedMessage;
            profileInvite4.CustomMessage = AttachedMessage;
            profileInvite5.CustomMessage = AttachedMessage;

            member = (Member)Session["Member"];
            if (member != null)
            {
                profileInvite1.MemberID = member.MemberID;
                profileInvite2.MemberID = member.MemberID;
                profileInvite3.MemberID = member.MemberID;
                profileInvite4.MemberID = member.MemberID;
                profileInvite5.MemberID = member.MemberID;
            }

            if (Go1) profileInvite1.Save();
            if (Go2) profileInvite2.Save();
            if (Go3) profileInvite3.Save();
            if (Go4) profileInvite4.Save();
            if (Go5) profileInvite5.Save();

            txtFriend1.Text = string.Empty;
            txtFriend2.Text = string.Empty;
            txtFriend3.Text = string.Empty;
            txtFriend4.Text = string.Empty;
            txtFriend5.Text = string.Empty;
        }
        else
        {
            litInvite.Text = "<p class='error_alert'>One or more invalid email address</p>";
        }
    }

    /// <summary>
    /// Concatonates the videos into HTML for the lister control
    /// </summary>
    private TabContents GetVideoLister(string WebMemberID, int Page)
    {
        PrivacyType privacyType = PrivacyType.Public;

        if (member != null)
        {
            if (
                ViewingMember.MemberID == member.MemberID ||
                Friend.IsFriend(member.MemberID, ViewingMember.MemberID))
            {
                privacyType = PrivacyType.Network;
            }
        }

        List<Next2Friends.Data.Video> videos = Next2Friends.Data.Video.GetTopVideosByMemberIDWithJoin(ViewingMember.MemberID, privacyType);
        NumberOfVideos = videos.Count;
        int DisplayNumberOfVideos = 6;
        int StartIndex = Page * DisplayNumberOfVideos;
        int EndIndex = StartIndex + DisplayNumberOfVideos;

        StringBuilder sbHTML = new StringBuilder();

        for (int i = StartIndex; i < EndIndex; i++)
        {
            if (videos.Count <= i)
            {
                break;
            }

            object[] parameters = new object[8];

            parameters[0] = ParallelServer.Get(videos[i].ThumbnailResourceFile.FullyQualifiedURL) + videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = videos[i].Duration.ToString();
            parameters[2] = videos[i].VeryShortTitle;
            parameters[3] = TimeDistance.TimeAgo(videos[i].DTCreated);
            parameters[4] = videos[i].VeryShortDescription;
            parameters[5] = videos[i].NumberOfViews;
            parameters[6] = videos[i].WebVideoID;
            parameters[7] = videos[i].NumberOfComments;

            sbHTML.AppendFormat(@"<li>
								<div class='vid_thumb'> <a href='view?v={6}'><img src='{0}' width='124' height='91' alt='thumb' /></a></div>
								<div class='vid_info'>
									<h3><a href='view?v={6}'>{2}</a></h3>
									<p class='timestamp'>{3}</p>
									<p>{4}</p>
									<p class='metadata'>Views: {5} Comments: {7}</p>
								</div>
							</li>", parameters);

        }

        TabContents tabContents = new TabContents();

        int PreviousPage = Page - 1;
        int NextPage = Page + 1;

        tabContents.PagerHTML = (Page > 0) ? @"<p class='view_all'><a href='javascript:ajaxGetListerContent(""" + WebMemberID + @""",1," + PreviousPage + @");' class='previous'>Previous</a>" : string.Empty;
        tabContents.PagerHTML += (videos.Count > (NextPage * DisplayNumberOfVideos)) ? @"<a href='javascript:ajaxGetListerContent(""" + WebMemberID + @""",1," + NextPage + @");' class='next'>Next</a></p>" : string.Empty;

        tabContents.HTML = sbHTML.ToString();

        return tabContents;
    }


   

    /// <summary>
    /// Returns the tab content for the member
    /// </summary>
    /// <param name="TabType"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public TabContents GetListerContent(string WebMemberID, int TabType, int Page)
    {
        ViewingMember = Member.GetMemberViaWebMemberID(WebMemberID);

        TabContents tabContents = new TabContents();

        if (TabType == 1)
        {
            tabContents = GetVideoLister(WebMemberID, Page);

        }
        else if (TabType == 2)
        {
            //tabContents.HTML = GetPhotoLister();
            //tabContents.PagerHTML = " ";
        }        
        else if (TabType == 4)
        {
            tabContents.HTML = "Live Broadcasts";
            tabContents.TabType = TabType;
            tabContents.PagerHTML = " ";
        }



        return tabContents;
    }

    /// <summary>
    /// Saves the About me section to the database
    /// </summary>
    /// <param name="NewText">The new text</param>
    /// <returns>The new text</returns>
    [AjaxPro.AjaxMethod]
    public string UpdateAboutMe(string NewText)
    {
        member = (Member)Session["Member"];

        MemberProfile mp = member.MemberProfile[0];
        mp.EmbeddedContent = HTMLUtility.CleanText(NewText);
        mp.Save();

        Session["Member"] = member;

        return "AboutMe.aspx?m=" + member.WebMemberID;
    }

    
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.Misc;

public partial class MemberView : System.Web.UI.Page
{
    public string WebRoot = ASP.global_asax.WebServerRoot;

    public Member ViewingMember;
    public MemberProfile ViewingMemberProfile;
    public bool ShowCarousel = true;
    public string DivCarouselClass = string.Empty;
    public Member member;
    public string EmbedLink = string.Empty;
    public bool ShowAge = true;
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
    public string DayJob = string.Empty;
    public string NightJob = string.Empty;
    public string Hobby = string.Empty;

    public int DayJobID = -1;
    public int NightJobID = -1;
    public int HobbyID = -1;


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
    public bool HideCarousel = false;
    public bool IsPersonalAccount = true;

    public string Field1Title = string.Empty;
    public string Field2Title = string.Empty;
    public string Field3Title = string.Empty;
    public string Field4Title = string.Empty;
    public string Field5Title = string.Empty;
    public string Field6Title = string.Empty;

    public string AboutTitle = string.Empty;

    public string BasicInfo=string.Empty;
    public string CompanyName = string.Empty;
    public string CompanyWebsite = string.Empty;
    public string CompanyTagLine = string.Empty;
    public string ContactFirst=string.Empty;
    public string ContactLast=string.Empty;
    public string IndustrySector=string.Empty;
    public int YearFounded=1900;
    public string CompanySize=string.Empty;
    public string CompanyStreetAddress=string.Empty;
    public string CompanyState=string.Empty;
    public string CompanyCity = string.Empty;
    public string CompanyCountry = string.Empty;
    public string CompanyZipCode = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MemberView));

        member = (Member)Session["Member"];

        LoginUrl = @"/signup/?u=" + Request.Url.AbsoluteUri;

        MemberSubscribers = GetSubscriberLister();

        PopulateMemberVariables();
        GenerateFriendLister();

        List<PhotoCollection> Galleries = PhotoCollection.GetAllPhotoCollectionByMemberID(ViewingMember.MemberID);
        GetPhotoLister(ViewingMember.WebMemberID, Galleries, 0);

        TabContents tabContents = GetVideoLister(ViewingMember.WebMemberID, 0, true);
        DefaultLister = tabContents.HTML;
        DefaultPager = tabContents.PagerHTML;

        IncrementProfileViews();

        InviteWebmemberID.Value = ViewingMember.WebMemberID;

        EmbedLink = @"<object width=""420"" height=""320""><param name=""flashvars"" value=""nickname=" + ViewingMember.NickName + @""" /><param name=""movie"" value=""http://services.next2friends.com/livewidget/n2flw1.swf""></param><param name=""allowFullScreen"" value=""true""></param><embed src=""http://services.next2friends.com/livewidget/n2flw1.swf"" flashvars=""nickname=" + ViewingMember.NickName + @"""  type=""application/x-shockwave-flash"" allowfullscreen=""true"" width=""420"" height=""320""></embed></object>";

        if (ViewingMember.AccountType == 1 || ViewingMember.DOB.Year == 1900)
        {
            ShowAge = false;
        }
    
    }

    private void IncrementProfileViews()
    {
        try
        {
            if (ViewingMember != null)
            {
                ViewingMember.MemberProfile[0].NumberOfViews++;
                ViewingMember.MemberProfile[0].Save();
            }
        }
        catch { }
    }

    private void PopulateMemberVariables()
    {

        //if(ViewingMember.AccountType == (int)AccountType.Personal)
        if (ViewingMember.AccountType == 0)
        {
            AboutTitle = "About me";

            Field1Title = "My Life";
            Field2Title = "Music";
            Field3Title = "Books";
            Field4Title = "Movies";
            Field5Title = "";
            Field6Title = "";

            MyLife = HTMLUtility.AutoLink(ViewingMember.MemberProfile[0].MyLife.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Gender = GetGender(ViewingMember.Gender);
            LastActive = TimeDistance.TimeAgo(ViewingMember.LastOnline);
            ProfileViews = ViewingMember.MemberProfile[0].NumberOfViews.ToString();

            Hometown = HTMLUtility.AutoLink(ViewingMember.MemberProfile[0].HomeTown);
            Country = HTMLUtility.AutoLink(ViewingMember.CountryName);
            Nick = ViewingMember.NickName;
            DirectUrl = WebRoot + "users/" + ViewingMember.NickName;
            DirectUrlText = WebRoot + "users/" + ViewingMember.NickName;

            Movies = HTMLUtility.AutoLink(ViewingMember.MemberProfile[0].Movies.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Books = HTMLUtility.AutoLink(ViewingMember.MemberProfile[0].Books.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Music = HTMLUtility.AutoLink(ViewingMember.MemberProfile[0].Music.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));

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

            BlogFeedURL = QualifyURL(ViewingMember.MemberProfile[0].BlogFeedURL);
            BlogFeedURLText = BreakText(BlogFeedURL);

            BlogURL = QualifyURL(ViewingMember.MemberProfile[0].BlogURL);
            BlogURLText = BreakText(BlogURL);

            MySpaceURL = QualifyURL(ViewingMember.MemberProfile[0].MySpaceURL);
            MySpaceURLText = BreakText(MySpaceURL);

            FaceBookURL = QualifyURL(ViewingMember.MemberProfile[0].FaceBookURL);
            FaceBookURLText = BreakText(FaceBookURL);

            try
            {
                DayJobID = ViewingMember.MemberProfile[0].DayProfessionID;
                NightJobID = ViewingMember.MemberProfile[0].NightProfessionID;
                HobbyID = ViewingMember.MemberProfile[0].HobbyID;

                if (DayJobID != -1)
                {
                    DayJob = new Profession(DayJobID).Name;
                }

                if (NightJobID != -1)
                {
                    NightJob = new Profession(NightJobID).Name;
                }

                if (HobbyID != -1)
                {
                    Hobby = new Hobby(HobbyID).Name;
                }
            }
            catch { }


        }
        //else if(ViewingMember.AccountType == (int)AccountType.Business)
        else if (ViewingMember.AccountType == 1)
        {
           
            Business business = ViewingMember.Business[0];

            AboutTitle = "About us";

            Field1Title = "Our Company";
            Field2Title = "Our Products / Services";
            Field3Title = "What we offer";
            Field4Title = "Where you can find us";
            Field5Title = "What sets us apart";
            Field6Title = "";

            MyLife = HTMLUtility.AutoLink(business.OurCompany.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Music = HTMLUtility.AutoLink(business.BusinessDescription1.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Books = HTMLUtility.AutoLink(business.BusinessDescription2.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));
            Movies = HTMLUtility.AutoLink(business.BusinessDescription3.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"));

            Nick = ViewingMember.NickName;
            DirectUrl = WebRoot + "users/" + ViewingMember.NickName;
            DirectUrlText = WebRoot + "users/" + ViewingMember.NickName;

            MemberSince = ViewingMember.CreatedDT.ToString("dd MMMM yyyy");

            BasicInfo=HTMLUtility.AutoLink(business.BasicInfo);
            CompanyName = HTMLUtility.AutoLink(business.CompanyName);
            CompanyWebsite=HTMLUtility.AutoLink(business.CompanyWebsite);
            CompanyTagLine=HTMLUtility.AutoLink(business.TagLine);
            ContactFirst=HTMLUtility.AutoLink(business.ContactFirst);
            ContactLast=HTMLUtility.AutoLink(business.ContactLast);
            IndustrySector=HTMLUtility.AutoLink(business.IndustrySector);
            YearFounded=business.YearFounded;
            CompanySize=HTMLUtility.AutoLink(business.CompanySize);
            CompanyStreetAddress=HTMLUtility.AutoLink(business.StreetAddress);
            CompanyState=HTMLUtility.AutoLink(business.State);
            CompanyCity=HTMLUtility.AutoLink(business.City);

           
            CompanyCountry = HTMLUtility.AutoLink((new ISOCountry(ViewingMember.ISOCountry)).CountryText);
       


            CompanyZipCode=HTMLUtility.AutoLink(business.ZipCode);

            

            BlogFeedURL = QualifyURL(business.BlogFeedURL);
            BlogFeedURLText = BreakText(BlogFeedURL);

            BlogURL = QualifyURL(business.BlogURL);
            BlogURLText = BreakText(BlogURL);

            MySpaceURL = QualifyURL(business.MySpaceURL);
            MySpaceURLText = BreakText(MySpaceURL);

            FaceBookURL = QualifyURL(business.FaceBookURL);
            FaceBookURLText = BreakText(FaceBookURL);

        }


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
                return "<a href=\"/users/" + m.NickName + "\">" + m.NickName + "</a>";
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

    private string QualifyURL(string URL)
    {
        if (URL.Length >= 7)
        {
            if (URL.ToLower().Substring(0, 7) != "http://")
            {
                URL = "http://" + URL;
            }
        }

        return URL;
    }

    /// <summary>
    /// gets the About me section in plain text
    /// </summary>
    [AjaxPro.AjaxMethod]
    public string GetAboutMeText()
    {
        member = (Member)Session["Member"];

        if (member.AccountType == 0)

            return member.MemberProfile[0].EmbeddedContent;

        else

            return member.Business[0].EmbeddedContent;
    }

    /// <summary>
    /// Saves the members display status
    /// </summary>
    [AjaxPro.AjaxMethod]
    public void SaveProfileStatus(string Status)
    {
        member = (Member)Session["Member"];

        if(member!=null)
        {
            member.MyMemberProfile.TagLine = Server.HtmlEncode(Status);
            member.MyMemberProfile.Save();
        }
    }

    //private bool GetViewingMember()
    //{
    //    string strNickname = Context.Items["nickname"].ToString();

    //    ViewingMember = Member.GetMemberViaNicknameNoEx(strNickname);

    //    bool ValidViewingMember = ViewingMember != null;

    //    return ValidViewingMember;
    //}


    [AjaxPro.AjaxMethod]
    public string GetLiveBroadcast(string WebMemberID)
    {
        member = (Member)Session["Member"];

        string WebBroadcastID = null;

        if (member != null)
        {
            //LiveBroadcast PrivateBroadcast = LiveBroadcast.GetPrivateLiveBroadcastByMemberID(WebMemberID, member.MemberID);

            //if (PrivateBroadcast != null)
            //{
            //    WebBroadcastID = PrivateBroadcast.WebLiveBroadcastID;
            //}
        }

        return WebBroadcastID;
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

            string HTMLItem = @"<li><a href='/users/{2}'>
                            <img src='{1}' alt='{2}' width='45' height='45' /></a>
                            <p>
                                <a href='/users/{2}'><strong>{2}</strong></a><br />
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




            sbHTML.AppendFormat(@"<li><a href='/users/{1}'><img src='{2}' alt='{1}' width='45' height='45' />
                            <p>
                                <strong>{1}</strong></a><br />
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

        if (Galleries.Count < 4)
        {
            HideCarousel = true;
        }

        for (int i = StartIndex; i < EndIndex; i++)
        {
            if (Galleries.Count <= i)
            {
                break;
            }

            // only show galleries with at least one photo
            if (Galleries[i].Photo.Count > 0)
            {
                object[] parameters = new object[6];

                parameters[0] = ParallelServer.Get(Galleries[i].DefaultThumbnailURL) + "user/" + Galleries[i].DefaultThumbnailURL;
                parameters[1] = Galleries[i].WebPhotoCollectionID;
                parameters[2] = Galleries[i].Name;
                parameters[3] = Galleries[i].Photo.Count;
                parameters[4] = Galleries[i].ShortDescription;
                parameters[5] = ViewingMember.WebMemberID;


                sbHTML.AppendFormat(@"<li style='height:182px'><a href='/gallery/?g={1}&m={5}'><img src='{0}' alt='thumb' /></a>
                                <p class='cat_details'><a href='/gallery/?g={1}&m={5}'><strong>{2}</strong> ({3})</a><br />
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

        //GalleryDetailsHTML = "(" + NumberOfPhotos + " photos in " + NumberOfPopulatedGalleries + " galleries)";
        //GalleryListerHTML = sbHTML.ToString();

        if (NumberOfPhotos > 0)
        {
            GalleryListerHTML = sbHTML.ToString();
            ShowCarousel = true;
            DivCarouselClass = "carousel";
            GalleryDetailsHTML = "(" + NumberOfPhotos + " photos in " + NumberOfPopulatedGalleries + " galleries)";
        }
        else
        {
            GalleryListerHTML = "<p>Member currently has no Photos.</p>";
            ShowCarousel = false;
            DivCarouselClass = string.Empty;
            GalleryDetailsHTML = string.Empty;
        }

        tabContents.HTML = GalleryListerHTML;

        return tabContents;
    }

    protected void btnInvite_Click(object sender, EventArgs e)
    {
        string Email1 = txtFriend1.Text;

        ProfileInvite profileInvite1 = new ProfileInvite();

        if (Email1 != string.Empty)
        {
            if (RegexPatterns.TestEmailRegex(Email1))
            {
                profileInvite1.DTCreated = DateTime.Now;
                profileInvite1.EmailAddress = Email1;
                txtFriend1.CssClass = "form_txt_small";

                string AttachedMessage = (txtInviteMessage.Text == "Your personal message here!") ? string.Empty : txtInviteMessage.Text;

                profileInvite1.CustomMessage = AttachedMessage;

                Member ProfileMember = Member.GetMemberViaWebMemberID(InviteWebmemberID.Value);

                profileInvite1.MemberID = ProfileMember.MemberID;

                profileInvite1.Save();

                txtFriend1.Text = string.Empty;

                litInvite.Text = "<span style='color:#0257AE;font-size:smaller;'>" + Email1 + "<br/> has been sent an invite</span>";

            }
            else
            {

                txtFriend1.CssClass = "form_txt_small formerror";
                litInvite.Text = "<p class='error_alert'>Invalid email address</p>";
            }
        }
        else
        {
            txtFriend1.CssClass = "form_txt_small";
            litInvite.Text = "<p class='error_alert'>Please enter an email address</p>";
        }

    }

    /// <summary>
    /// Concatonates the videos into HTML for the lister control
    /// </summary>
    private TabContents GetVideoLister(string WebMemberID, int Page, bool WrapInP)
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
        int DisplayNumberOfVideos = 12;
        int StartIndex = Page * DisplayNumberOfVideos;
        int EndIndex = StartIndex + DisplayNumberOfVideos;

        StringBuilder sbHTML = new StringBuilder();

        for (int i = StartIndex; i < EndIndex; i++)
        {
            if (videos.Count <= i)
            {
                break;
            }

            object[] parameters = new object[11];

            parameters[0] = ParallelServer.Get(videos[i].ThumbnailResourceFile.FullyQualifiedURL) + videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = videos[i].Duration.ToString();
            parameters[2] = videos[i].VeryShortTitle;
            parameters[3] = TimeDistance.TimeAgo(videos[i].DTCreated);
            parameters[4] = videos[i].VeryShortDescription;
            parameters[5] = videos[i].NumberOfViews;
            parameters[6] = videos[i].WebVideoID;
            parameters[7] = videos[i].NumberOfComments;
            parameters[8] = videos[i].Title;
            parameters[9] = RegexPatterns.FormatStringForURL(videos[i].Title);

            

//            sbHTML.AppendFormat(@"<li>
//								<div class='vid_thumb'> <a href='/video/{9}/{6}'><img src='{0}' width='124' height='91' alt='{8}' /></a></div>
//								<div class='vid_info'>
//									<h3><a href='/video/{9}/{6}'>{2}</a></h3>
//									<p class='timestamp'>{3}</p>
//									<p>{4}</p>
//									<p class='metadata'>Views: {5} Comments: {7}</p>
//								</div>
//							</li>", parameters);

            sbHTML.AppendFormat(@"<li style='width:145px;clear: none;margin-left:3px'>
								<div class='vid_thumb'> <a href='javascript:displayMiniVideo(""{6}"",""{8}"");'><img src='{0}' width='124' height='91' alt='{8}' /></a></div>
							</li>", parameters);

        }

        TabContents tabContents = new TabContents();

        int PreviousPage = Page - 1;
        int NextPage = Page + 1;

        tabContents.PagerHTML = (WrapInP) ? "<p class='view_all' id='pPager'>" : string.Empty;
        tabContents.PagerHTML += (Page > 0) ? @"<p class='view_all'><a href='javascript:ajaxGetListerContent(""" + WebMemberID + @""",1," + PreviousPage + @");' class='previous'>Previous</a>" : string.Empty;
        tabContents.PagerHTML += (videos.Count > (NextPage * DisplayNumberOfVideos)) ? @"<a href='javascript:ajaxGetListerContent(""" + WebMemberID + @""",1," + NextPage + @");' class='next'>Next</a></p>" : string.Empty;
        tabContents.PagerHTML += (WrapInP) ? "</p>" : string.Empty;
        tabContents.HTML = (NumberOfVideos > 0) ? "<ul class='profile_vid_list' id='ulContentLister'>" + sbHTML.ToString() + "</ul>" : "<p>Member currently has no Videos.</p>";

        // tabContents.HTML = sbHTML.ToString();

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
            tabContents = GetVideoLister(WebMemberID, Page, false);

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

        if (member.AccountType == 0)
        {
            MemberProfile mp = member.MemberProfile[0];
            mp.EmbeddedContent = HTMLUtility.CleanText(NewText);
            mp.Save();
        }
        else
        {
            Business business = member.Business[0];
            business.EmbeddedContent = HTMLUtility.CleanText(NewText);
            business.Save();
        }

        return "/AboutMe.aspx?m=" + member.WebMemberID;
    }

//    [AjaxPro.AjaxMethod]
//    public string BuildMP3CopyList(string WebMP3UploadID)
//    {
//        //if (!File.Exists(mP3Upload.Path))
//        //{
//        //    Response[0] = "MP3 no longer exists";
//        //    return Response;
//        //}

//        StringBuilder sbHTML = new StringBuilder();

//        string[] Parameters = new string[3];

//        Parameters[0] = mP3Upload.WebMP3UploadID;
//        Parameters[1] = mP3Upload.Title;

//        sbHTML.AppendFormat(@"<li id='mp3{0}'><a href=\""javascript:copySong('{0}')\"" class='deletePlaylistItem'>
//                              <img src='/images/unfriend-bg.gif' width='12' height='11' alt='Copy song' /></a>{1}</li>", Parameters);


//    }

    //[AjaxPro.AjaxMethod]
    //public string CopySong(string WebMP3UploadID)
    //{
    //    Utility.RememberMeLogin();

    //    member = (Member)Session["Member"];

    //    string Response = string.Empty;

    //    Next2Friends.Data.MP3Upload mP3Upload = Next2Friends.Data.MP3Upload.GetMP3UploadByWebMP3UploadID(WebMP3UploadID);

    //    if (mP3Upload == null)
    //    {
    //        Response[0] = "MP3 no longer exists";
    //        return Response;
    //    }

    //    mP3Upload.Copy(member.MemberID);

    //    return Response;
    //}


    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        string strWebVideoID = Request.Params["v"];   
        string strWebMemberID = Request.Params["m"];

        if (strWebVideoID != null)
        {
            Video RedirectVideo = Video.GetVideoByWebVideoIDWithJoin(strWebVideoID);
            if (RedirectVideo == null)
            {
                //404
         //       HTTPResponse.FileNotFound404(Context);
                Server.Transfer("/NotAvailable.aspx?rt=v");
            }
            else
            {
           //     HTTPResponse.PermamentlyMoved301(Context, RedirectVideo.SEOUrl);
            }
        }

        if (strWebMemberID != null)
        {
            Member RedirectMember = Member.GetMemberViaWebMemberIDNoEx(strWebMemberID);
            if (RedirectMember == null)
            {
                //404
          //      HTTPResponse.FileNotFound404(Context);
                Server.Transfer("/NotAvailable.aspx?rt=m");
            }
            else
            {
                string RedirectTo = "/users/" + RedirectMember.NickName;
         //       HTTPResponse.PermamentlyMoved301(Context, RedirectTo);
            }
        }


        member = (Member)Session["Member"];

        string strNickname = Context.Items["nickname"].ToString();

        ViewingMember = Member.GetMemberViaNicknameNoEx(strNickname);
        
        if (member != null)
        {
            IsLoggedIn = true;

            if (ViewingMember != null && ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }

        if (ViewingMember == null)
        {
    //        HTTPResponse.FileNotFound404(Context);
            Server.Transfer("/NotAvailable.aspx");
        }
        
        if (Utility.IsMe(ViewingMember, member))
        {
            //Master.Master.SkinID = "Profile";
        }

        base.OnPreInit(e);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetProfileTitle(ViewingMember);
        //Master.Master.MetaDescription = "Live mobile video broadcasting networking";
        //Master.Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    
    }

}

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

public partial class ViewGalleryPB : System.Web.UI.Page
{
    public Member ViewingMember;
    public MemberProfile ViewingMemberProfile;
    public string PhotoURL;
    public string VideoURL;
    public Member member;
    public string PageComments = string.Empty;
    public string MemberSubscribers = string.Empty;
    public string NumberOfMemberSubscribers = string.Empty;
    public string DefaultLister = string.Empty;
    public string DefaultPager = string.Empty;
    public int NumberOfComments = 0;
    public int NumberOfVideos = 0;
    public int NumberOfPhotos = 0;
    public int NumberOfFriends = 0;
    public string GalleryDetailsHTML = string.Empty;
    public string GalleryListerHTML = string.Empty;
    public string GalleryNameHTML = string.Empty;
    public string NumberOfPhotosHTML = string.Empty;
    public string JSPhotoArrayHTML = string.Empty;
    public Photo[] Photos;

    public string DefaultPhotoComments;


    public string DefaultTab = string.Empty;
    public string PermaLink = string.Empty;
    
    public bool IsMyPage = false;
    public bool IsLoggedIn = false;
    public string DefaultPhotoURL;
    public string DefaultVideoURL;
    public string DefaultPhotoCaption;

    public string MainTitle = string.Empty;
    public string MainSubTitle = string.Empty;

    public string LoginUrl;
    public string SubscribeLink;
    public string SendMessageLink;
    public string BlockMemberLink;
    public string AddToFriendsLink;
    public string AddFavouritesLink;
    public string ReportAbuseLink;

    public string DisplayComments = "none";
    public string DisplayGallery = "none";
    public string DisplayPhoto = "none";
    public string DefaultWebPhotoID;
    public string PrevPageHTML;
    public string NextPageHTML;
    public string DisplayCurrentIndex = "none";
    public string CurrentlyShowing;

    public Photo DefaultPhoto;
    public PhotoCollection DefaultGallery;
    public string GalleryDropHTML = string.Empty;

    public bool LoadLargeMemberPhoto = false;

    public string DefaultNewCommentParams;
    public string DefaultNumberOfViews = "0";

    public string WebRoot = ASP.global_asax.WebServerRoot;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ViewGalleryPB));

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        // set the default forwarding if the member is not logged in
        LoginUrl = @"signup.aspx?u=" + Request.Url.AbsoluteUri;
        SubscribeLink = LoginUrl;
        SendMessageLink = LoginUrl;
        BlockMemberLink = LoginUrl;
        AddToFriendsLink = LoginUrl;
        AddFavouritesLink = LoginUrl;

        // load the members photo
        string strGalleryPhoto = Request.Params["g"];
        string strPhoto = Request.Params["p"];

        if (strGalleryPhoto != null)
        {
            ViewingMember = Member.GetMemberByPhotoCollectionID(strGalleryPhoto);
            ViewingMemberProfile = ViewingMember.MemberProfile[0];
            DefaultNumberOfViews = (++ViewingMemberProfile.NumberOfViews).ToString();
            ViewingMemberProfile = ViewingMember.MemberProfile[0];
            DefaultGallery = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(strGalleryPhoto);
            GalleryNameHTML = DefaultGallery.Name;

            GetGalleryDrop();
            GetPhotoJSArray();
            DisplayGallery = "block";

        }
        else
        {
            // the member failed to load from the URL param.. throw a friendly
        }

        if (strPhoto != null)
        {
            DefaultPhoto = Photo.GetPhotoByWebPhotoIDWithJoin(strPhoto);
            DefaultPhotoURL = ParallelServer.Get(DefaultPhoto.PhotoResourceFile.FullyQualifiedURL) + DefaultPhoto.PhotoResourceFile.FullyQualifiedURL;
            DefaultPhotoCaption = DefaultPhoto.Caption;
            DefaultWebPhotoID = DefaultPhoto.WebPhotoID;
            DisplayPhoto = "block";
            DisplayCurrentIndex = "block";
            DisplayComments = "block";            
            SetPagerButtons();

            
        }
        else
        {
            GetGalleryLister();
        }

        if (member != null)
        {
            if (ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }

        try
        {

            NumberOfVideos = ViewingMemberProfile.NumberOfVideos;
            NumberOfPhotos = ViewingMemberProfile.NumberOfPhotos;
            NumberOfFriends = FriendRequest.GetNumberOfFriends(ViewingMember.MemberID);
        }
        catch { }

    }

    /// <summary>
    /// determins the html for the pager buttons
    /// </summary>
    private void SetPagerButtons()
    {
        int Index = 0;
        PrevPageHTML = string.Empty;
        NextPageHTML = string.Empty;

        for (int i = 0; i < DefaultGallery.Photo.Count; i++)
        {
            if (DefaultGallery.Photo[i].WebPhotoID == DefaultWebPhotoID)
            {
                Index = i+1;
                if (i > 0)
                {
                    string PrevPageLink = "ViewGallery.aspx?g=" + DefaultGallery.WebPhotoCollectionID + "&p=" + DefaultGallery.Photo[i - 1].WebPhotoID;
                    PrevPageHTML = "<li class='gallery_prev'><a href='" + PrevPageLink + "' id='aPreviousImage'><img src='images/nspots-prev.gif' alt='previous' /></a></li>";
                }

                if (i < DefaultGallery.Photo.Count - 1)
                {
                    string NextPageLink = "ViewGallery.aspx?g=" + DefaultGallery.WebPhotoCollectionID + "&p=" + DefaultGallery.Photo[i+1].WebPhotoID;
                    NextPageHTML = "<li class='gallery_next'><a href='" + NextPageLink + "' id='aNextImage'><img src='images/nspots-next.gif' alt='next' /></a></li>";
                }
            }
        }  

        CurrentlyShowing = Index + " of " + DefaultGallery.Photo.Count;
    }

    /// <summary>
    /// Populates the gallery photos
    /// </summary>
    private void GetGalleryLister()
    {
        Photos = Photo.GetPhotoByPhotoCollectionIDWithJoin(DefaultGallery.PhotoCollectionID);

        StringBuilder sbHTML = new StringBuilder();

        for (int i = 0; i < Photos.Length; i++)
        {
            object[] parameters = new object[4];

            parameters[0] = ParallelServer.Get(Photos[i].ThumbnailResourceFile.FullyQualifiedURL) + Photos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = i.ToString();
            parameters[2] = Photos[i].WebPhotoID;
            parameters[3] = DefaultGallery.WebPhotoCollectionID;

//            sbHTML.AppendFormat(@"<li>
//                            	    <a href='javascript:showPhoto({1});'><img src='{0}' style='height:91px' alt='thumb' /></a>
//                                </li>", parameters);


            sbHTML.AppendFormat(@"<li>
                            	    <a href='ViewGallery.aspx?g={3}&p={2}'><img src='{0}' style='height:91px' alt='thumb' /></a>
                                </li>", parameters);
        }

        NumberOfPhotosHTML = Photos.Length.ToString();
        GalleryListerHTML = sbHTML.ToString();
    }

    /// <summary>
    /// Populates the Gallery Dropdown
    /// </summary>
    private void GetGalleryDrop()
    {
        List<PhotoCollection> Galleries = PhotoCollection.GetAllPhotoCollectionByMemberID(ViewingMember.MemberID);

        StringBuilder sbHTML = new StringBuilder();
        int NumberOfGalleries = 0;
        int NumberOfPhotos = 0;

        for (int i = 0; i < Galleries.Count; i++)
        {
            // only show galleries with at least one photo
            if (Galleries[i].Photo.Count > 0)
            {
                NumberOfGalleries++;

                object[] parameters = new object[5];

                parameters[0] = ParallelServer.Get(Galleries[i].DefaultThumbnailURL) + "user/" + Galleries[i].DefaultThumbnailURL;
                parameters[1] = Galleries[i].WebPhotoCollectionID;
                parameters[2] = Galleries[i].Name;
                parameters[3] = Galleries[i].Photo.Count;
                parameters[4] = Galleries[i].Description;

                sbHTML.AppendFormat(@"<li>
								<a href='ViewGallery.aspx?g={1}' class='clearfix'><span class='drop_thumb'><img src='{0}' alt='thumb' width='50' height='35' /></span>
								<span class='drop_details'><strong>{2}</strong><br />
								 {4}</span></a>

							</li>", parameters);

                NumberOfPhotos += Galleries[i].Photo.Count;
            }
        }

        GalleryDropHTML = sbHTML.ToString();
    }

    private void GetPhotoJSArray()
    {
        //StringBuilder sbHTML = new StringBuilder();
        //string JSStart = @"<script>var photos = new Array();";

        //for (int i = 0; i < Photos.Length; i++)
        //{

        //        object[] parameters = new object[6];

        //        parameters[0] = ParallelServer.Get() + Photos[i].PhotoResourceFile.FullyQualifiedURL;
        //        parameters[1] = Photos[i].WebPhotoID;
        //        parameters[2] = Photos[i].Caption.Replace("'","&#39;");
        //        parameters[3] = i.ToString();

        //        sbHTML.AppendFormat(@"photos[{3}] = new Array('{0}','{1}','{2}');    ", parameters);

        //}

        //string JSEnd = @"</script>";

        //JSPhotoArrayHTML = JSStart + sbHTML.ToString() + JSEnd;
    }
    
}

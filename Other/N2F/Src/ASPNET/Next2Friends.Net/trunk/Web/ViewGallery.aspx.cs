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
 
public partial class ViewGallery : System.Web.UI.Page
{
    public Member ViewingMember;
    public MemberProfile ViewingMemberProfile;
    public Member member;
    public string DefaultLister = string.Empty;
    public string DefaultPager = string.Empty;
    public int pageCount;
    public int photoLength;
    public string GalleryDetailsHTML = string.Empty;
    public string GalleryListerHTML = string.Empty;
    public string GalleryNameHTML = string.Empty;
    public string NumberOfPhotosHTML = string.Empty;
    public string JSPhotoArrayHTML = string.Empty;
    public List<Photo> photos;

    public string DefaultTab = string.Empty;
    public string PermaLink = string.Empty;
    
    public bool IsMyPage = false;
    public bool IsLoggedIn = false;
    public string LoginUrl;
    

    public string MainTitle = string.Empty;
    public string MainSubTitle = string.Empty;

    public PhotoCollection DefaultGallery;
    public string GalleryDropHTML = string.Empty;

    public string PagerHTML = string.Empty;
    public int CurrentPageIndex = 1;
    public string URLPhotoCollectionID = string.Empty;

    public string WebRoot = ASP.global_asax.WebServerRoot;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ViewGallery));

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        LoginUrl = @"/signup.aspx?u=" +  Server.UrlEncode(Request.Url.AbsoluteUri);

        string strPage = Request.Params["p"];
        CurrentPageIndex = Pager.TryGetPageIndex(strPage);

        // load the members photo
        string strGalleryPhoto = Request.Params["g"];

        if (strGalleryPhoto != null)
        {
            URLPhotoCollectionID = strGalleryPhoto;
            ViewingMember = Member.GetMemberByPhotoCollectionID(strGalleryPhoto);
            DefaultGallery = PhotoCollection.GetPhotoCollectionByWebPhotoCollectionID(strGalleryPhoto);
            GalleryNameHTML = DefaultGallery.Name;
            GetGalleryDrop();
            GetGalleryLister(CurrentPageIndex);
            GetPhotoJSArray();

        }
        else
        {
            // the member failed to load from the URL param.. throw a friendly
        }

        if (member != null)
        {
            if (ViewingMember.MemberID == member.MemberID)
            {
                IsMyPage = true;
            }
        }
        
        Comments1.CommentType = CommentType.Photo;
        forwardToFriend.ObjectWebID = URLPhotoCollectionID;
        forwardToFriend.ContentType = CommentType.PhotoGallery;   



    }

    /// <summary>
    /// Populates the gallery photos
    /// </summary>
    private void GetGalleryLister(int Page)
    {
        int NumberOfPhotos = Photo.GetPhotoCountPhotoCollectionID(DefaultGallery.PhotoCollectionID);

        photos = Photo.GetPhotoByPhotoCollectionIDWithJoinPager(DefaultGallery.PhotoCollectionID, Page, 16);

        StringBuilder sbHTML = new StringBuilder();

        for (int i = 0; i < photos.Count; i++)
        {
            object[] parameters = new object[2];

            parameters[0] = ParallelServer.Get(photos[i].ThumbnailResourceFile.FullyQualifiedURL) + photos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = i.ToString();

            sbHTML.AppendFormat(@"<li>
                            	    <a href='javascript:showPhoto({1});'><img src='{0}' alt='thumb' /></a>
                                </li>", parameters);
        }

        Pager pager = new Pager("/gallery/","g="+URLPhotoCollectionID+"&m="+ViewingMember.WebMemberID, Page, NumberOfPhotos);
        pager.PageSize = 16;
        PagerHTML = pager.ToString();

        NumberOfPhotosHTML = NumberOfPhotos.ToString();
        pageCount =(int)Math.Ceiling((double)NumberOfPhotos / pager.PageSize);
        photoLength = photos.Count;
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

                object[] parameters = new object[6];

                parameters[0] = ParallelServer.Get(Galleries[i].DefaultThumbnailURL) + "user/" + Galleries[i].DefaultThumbnailURL;
                parameters[1] = Galleries[i].WebPhotoCollectionID;
                parameters[2] = Galleries[i].Name;
                parameters[3] = Galleries[i].Photo.Count;
                parameters[4] = Galleries[i].Description;
                parameters[5] = ViewingMember.WebMemberID;

                sbHTML.AppendFormat(@"<li>
								<a href='/gallery/?g={1}&m={5}' class='clearfix'><span class='drop_thumb'><img src='{0}' alt='thumb' width='50' height='35' /></span>
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
        StringBuilder sbHTML = new StringBuilder();
        StringBuilder sbCommentArray = new StringBuilder();

        string JSStart = @"<script>var photos = new Array();";


        for (int i = 0; i < photos.Count; i++)
        {
            object[] parameters = new object[6];

            parameters[0] = ParallelServer.Get(photos[i].PhotoResourceFile.FullyQualifiedURL) + photos[i].PhotoResourceFile.FullyQualifiedURL;
            parameters[1] = photos[i].WebPhotoID;
            parameters[2] = photos[i].Caption.Replace("'", "&#39;").Replace("\r\n"," ");
            parameters[3] = i.ToString();

            sbHTML.AppendFormat(@"photos[{3}] = new Array('{0}','{1}','{2}');    ", parameters);
        }


        //List<AjaxComment> Comments = PhotoComment.GetPhotoCommentsByGalleryID(DefaultGallery.PhotoCollectionID);

        //string JSArray = PhotoComment.PhotoCommentsToJsArray(Comments);

        string JSEnd = @"</script>";

        JSPhotoArrayHTML = JSStart + sbHTML.ToString() + JSEnd;
    }


   
    protected override void OnPreInit(EventArgs e)
    {
        Master.SkinID = "photo";
        base.OnPreInit(e);
    }

    
}

public class CommentBlock
{
    public AjaxComment ajaxComment { get; set; }
    public string WebPhotoID { get; set; }
}

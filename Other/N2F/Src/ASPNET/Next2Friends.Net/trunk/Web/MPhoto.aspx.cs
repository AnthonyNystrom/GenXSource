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

public partial class MPhotos : System.Web.UI.Page
{
    public string GalleryDetailsHTML = string.Empty;
    public string GalleryListerHTML = string.Empty;
    public Member ViewingMember;
    public bool ShowCarousel = true;
    public string DivCarouselClass = string.Empty;
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        List<PhotoCollection> Galleries = PhotoCollection.GetAllPhotoCollectionByMemberID(ViewingMember.MemberID);
        GetPhotoLister(ViewingMember.WebMemberID, Galleries);
    }

    private void GetPhotoLister(string WebMemberID, List<PhotoCollection> Galleries)
    {
        StringBuilder sbHTML = new StringBuilder();
        int DisplayNumberOfGalleries = 10;
        int NumberOfGalleries = Galleries.Count + 1;
        int NumberOfPhotos = 0;
        int NumberOfPopulatedGalleries = 0;
 
        for (int i = 0; i < Galleries.Count; i++)
        {
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


        //string RightPagerHTML = "<li class='gallery_prev'><a style='display:" + DisplayPrev + ";' href='javascript:PageGallery(\"" + WebMemberID + "\"," + PrevPage + "," + NumberOfGalleries + ");'  id='aGallPrev'><img src='images/nspots-prev.gif' alt='previous' /></a></li>";
        //string LeftPagerHTML = "<li class='gallery_next'><a style='display:" + DisplayNext + ";' href='javascript:PageGallery(\"" + WebMemberID + "\"," + NextPage + "," + NumberOfGalleries + ");'  id='aGallNext'><img src='images/nspots-next.gif' alt='next' /></a></li>";

        GalleryDetailsHTML = "(" + NumberOfPhotos + " photos in " + NumberOfPopulatedGalleries + " galleries)";

        if(NumberOfPhotos > 0)
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
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        member = (Member)Session["Member"];

        if (member != null)
        {
            if (member.MemberID == ViewingMember.MemberID)
            {
                Master.SkinID = "profile";
            }
        }

        
        base.OnPreInit(e);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetMPhotosTitle(ViewingMember);
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

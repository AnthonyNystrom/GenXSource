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

public partial class PhotoPage : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string DefaultCurrentBrowsing = string.Empty;
    public string FeatureSidebarHTML = string.Empty;
    public string CurrentTab1 = string.Empty;
    public string CurrentTab2 = string.Empty;
    public string CurrentTab3 = string.Empty;
    public string CurrentTab4 = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(PhotoPage));


        string strTabType = Request.Params["t"];
        string strPage = Request.Params["p"];

        if (strPage != null)
        {
            //int TabType = Int32.Parse(strTabType);
            int Page = Int32.Parse(strPage);

            TabContents tabContents = GeneratePhotoTab(1, Page);

            DefaultHTMLLister = tabContents.HTML;
            DefaultHTMLPager = tabContents.PagerHTML;

            //CurrentTab1 = string.Empty;
            //CurrentTab2 = string.Empty;
            //CurrentTab3 = string.Empty;
            //CurrentTab4 = string.Empty;

            //if (TabType == 1)
            //{
            //    DefaultCurrentBrowsing = "You are browsing: Latest Photos";
            //    CurrentTab1 = " class='current' ";
            //}
            //else if (TabType == 2)
            //{
            //    DefaultCurrentBrowsing = "You are browsing: Most Viewed Photos";
            //    CurrentTab2 = " class='current' ";
            //}
            //else if (TabType == 3)
            //{
            //    DefaultCurrentBrowsing = "You are browsing: Most Discussed Photos";
            //    CurrentTab3 = " class='current' ";
            //}
            //else if (TabType == 4)
            //{
            //    DefaultCurrentBrowsing = "You are browsing: Top Rated Photos";
            //    CurrentTab4 = " class='current' ";
            //}
        }
        else
        {
            TabContents tabContents = GeneratePhotoTab(1, 1);

            DefaultHTMLLister = tabContents.HTML;
            DefaultHTMLPager = tabContents.PagerHTML;

            DefaultCurrentBrowsing = "You are browsing: Latest Photos";
            CurrentTab1 = " class='current' ";
        }
    }

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    string SearchKeyword = txtSearch.Text.ToLower();

    //    if (SearchKeyword != string.Empty)
    //    {
    //        List<Photo> photos = Next2Friends.Data.Photo.SearchPhotoByKeyword(SearchKeyword);

    //        TabContents tabContents = GenerateLister(photos, 0, 1);
    //        DefaultHTMLLister = tabContents.HTML;
    //    }
    //    else
    //    {
    //        DefaultHTMLLister = string.Empty;
    //    }

    //    DefaultCurrentBrowsing = "You searched for: " + txtSearch.Text;
    //}

    [AjaxPro.AjaxMethod]
    public TabContents GetListerContent(int TabType)
    {
        return GeneratePhotoTab(TabType, 1);
    }

    public string GenerateFeatureSidebar(List<Photo> Photos)
    {
        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 2; i++)
        {
            if (Photos.Count <= i)
            {
                break;
            }

            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = ParallelServer.Get(Photos[i].ThumbnailResourceFile.FullyQualifiedURL) + Photos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = TimeDistance.TimeAgo(Photos[i].CreatedDT);
            parameters[2] = Photos[i].WebPhotoID;
            parameters[3] = Photos[i].NumberOfViews;
            parameters[4] = Photos[i].NumberOfComments;
            parameters[5] = Photos[i].TotalVoteScore;
            parameters[6] = Photos[i].NumberOfComments;
            parameters[7] = Photos[i].Member.NickName;
            parameters[8] = Photos[i].Member.WebMemberID;
            parameters[9] = Photos[i].PhotoCollectionName;
            parameters[10] = Photos[i].PhotoCollectionDescription;
            parameters[11] = Photos[i].WebPhotoCollectionID;

            sbHTML.AppendFormat(@"<li>
								<a href='/users/{7}'><img src='{0}' alt='thumb' /></a><br />
								<h3><a href='/gallery/?g={11}&m={8}'>{9}</a></h3>

								<p class='metadata'>by: <a href='/users/{7}'>{7}</a><br />
									Views: {3} Comments: <a href='/gallery/?g={11}&m={8}'>{6}</a><br />
								</p>						
							</li>", parameters);




            sbHTMLList.Append(sbHTML.ToString());
        }

        return sbHTMLList.ToString();

    }

    public TabContents GeneratePhotoTab(int TabType, int Page)
    {
        List<Photo> Photos;

        string CacheKey = "TopPhotos" + TabType;

        Photos = (List<Photo>)System.Web.HttpContext.Current.Cache[CacheKey];

        if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        {
            Photos = Photo.GetTop100Photos((TopPhotoType)TabType);
            System.Web.HttpContext.Current.Cache.Insert(CacheKey, Photos, null, DateTime.Now.AddMinutes(1), System.Web.Caching.Cache.NoSlidingExpiration);
        }

        FeatureSidebarHTML = GenerateFeatureSidebar(Photos);

        return GenerateLister(Photos, TabType, Page);
    }


    public TabContents GenerateLister(List<Photo> Photos, int TabType, int Page)
    {
        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 54;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (Photos.Count <= i)
            {
                break;
            }

            StringBuilder sbHTML = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = ParallelServer.Get(Photos[i].ThumbnailResourceFile.FullyQualifiedURL) + Photos[i].ThumbnailResourceFile.FullyQualifiedURL;
            parameters[1] = TimeDistance.TimeAgo(Photos[i].CreatedDT);
            parameters[2] = Photos[i].WebPhotoID;
            parameters[3] = Photos[i].NumberOfViews;
            parameters[4] = Photos[i].NumberOfComments;
            parameters[5] = Photos[i].TotalVoteScore;
            parameters[6] = Photos[i].NumberOfComments;
            parameters[7] = Photos[i].Member.NickName;
            parameters[8] = Photos[i].Member.WebMemberID;
            parameters[9] = Photos[i].Title;
            parameters[10] = Photos[i].Caption;
            parameters[11] = Photos[i].WebPhotoCollectionID;


			sbHTML.AppendFormat(@"<li style='width:131px;text-align:center;padding:0 0 0px'>
							<div class='vid_thumb' style='text-align:center;width:131px;overflow:hidden;'> 
                            <a href='/gallery/?g={11}&m={8}'><img src='{0}' height='91' alt='thumb' /></a></div>
						    <a href='/users/{7}'>{7}</a>
						</li>", parameters);
						
						
//            sbHTML.AppendFormat(@"<li style='width:115px;text-align:center;padding:0 0 0px'>
//							<div class='vid_thumb' style='text-align:center;width:131px;overflow:hidden;'> 
//                            <a href='/gallery/?g={11}&m={8}'><img src='{0}' height='91' alt='thumb' /></a></div>
//						    <a href='/users/{7}'>{7}</a>
//						</li>", parameters);

//            sbHTML.AppendFormat(@"<li>
//							<div class='vid_thumb' style='text-align:center;width:131px;overflow:hidden;'> <a href='view.aspx?p={2}'><img src='{0}' height='91' alt='thumb' /></a></div>
//							<div class='vid_info'>
//								
//                                <h3><a href='view.aspx?p={2}'>{9}</a></h3>
//								<p class='timestamp'>{1}</p>
//								<div class='vote vote_condensed'><span class='vote_count'>{5}</span></div>
//								<p class='metadata'>Views: {3}<br />
//								Comments: <a href='#'>{4}</a><br />
//								by: <a href='view.aspx?m={8}'>{7}</a></p>
//								
//							</div>
//						</li>", parameters);


            sbHTMLList.Append(sbHTML.ToString());
        }


        //StringBuilder sbPager = new StringBuilder();

        //object[] PagerParameters = new object[4];
        //PagerParameters[0] = TabType;
        //PagerParameters[1] = Page - 1;
        //PagerParameters[2] = Page + 1;
        //PagerParameters[3] = TabType;

        //if (Page != 1)
        //    sbPager.AppendFormat("<a  href='?t={3}&p={1}' class='previous'>Previous</a>", PagerParameters);

        //if (Photos.Count >= (Page * PageSize))
        //    sbPager.AppendFormat("<a  href='?t={3}&p={2}' class='next'>Next</a>", PagerParameters);

        //// create the TabContents to return
        //TabContents tabContents = new TabContents();

        //tabContents.TabType = TabType;
        //tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        Pager pager = new Pager("/photos/", string.Empty, Page, 5000);

        pager.PageSize = PageSize;

        // create the TabContents to return
        TabContents tabContents = new TabContents();

        // tabContents.TabType = TabType;
        tabContents.HTML = sbHTMLList.ToString();
        tabContents.PagerHTML = pager.ToString();

        return tabContents;
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        Master.SkinID = "photo";
        base.OnPreInit(e);
    }


}

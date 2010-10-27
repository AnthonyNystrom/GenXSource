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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;
using Next2Friends.Data;

public enum OrderVideo { Latest = 1, NumberOfViews, NumberOfComments, TotalVoteScore };

public partial class VideoPage : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string CategoryHTML = string.Empty;
    public string DefaultCurrentBrowsing = string.Empty;
    public string TagCloudHTML = string.Empty;
    public string CurrentTab1 = string.Empty;
    public string CurrentTab2 = string.Empty;
    public string CurrentTab3 = string.Empty;
    public string CurrentTab4 = string.Empty;
    public string pageUrl = string.Empty;

    /// <summary>
    /// Query string parametrs
    /// </summary>
    private int page;
    private string keyword = null;
    private string exactKeyword = null;
    private string withoutKeyword = null;
    private string atLeastKeyword = null;
    private string tag = null;
    private int categoryId = -1;
    private OrderVideo order = OrderVideo.Latest;
    private string searchType = null;
    private int pageSize = 10;
    private string miscParams;

    protected void Page_Load(object sender, EventArgs e)
    {
        URLParameterSetup();

        GenerateMiscParams();

        FullSearch();

        SetOrderTab();

        TagCloudHTML = GenerateTagCloud();
        CategoryHTML = GenerateCategories();
    }

    public void URLParameterSetup()
    {
        tag = Request.QueryString["g"];


        string strPage = Request.QueryString["p"];
        keyword = Request.QueryString["search"];
        exactKeyword = Request.QueryString["esearch"];
        withoutKeyword = Request.QueryString["wsearch"];
        atLeastKeyword = Request.QueryString["asearch"];
        string category = Request.QueryString["cat"];
        string sortOrder = Request.QueryString["to"];
        searchType = Request.QueryString["type"];

        page = 1;

        Int32.TryParse(strPage, out page);
        
        // set the page to 1 if no preference is found in the URL
        page = (page == 0) ? 1 : page;

        categoryId = -1;

        if (!string.IsNullOrEmpty(category))
        {
            Int32.TryParse(category, out categoryId);
        }

        order = OrderVideo.Latest;

        try
        {
            order = (OrderVideo)int.Parse(sortOrder);
        }
        catch { }

    }

    public void SetOrderTab()
    {
            int TabType = (int)order;
            CurrentTab1 = string.Empty;
            CurrentTab2 = string.Empty;
            CurrentTab3 = string.Empty;
            CurrentTab4 = string.Empty;

            if (TabType == 1)
            {
                CurrentTab1 = " class='current' ";
            }
            else if (TabType == 2)
            {
                CurrentTab2 = " class='current' ";
            }
            else if (TabType == 3)
            {
                 CurrentTab3 = " class='current' ";
            }
            else if (TabType == 4)
            {
                CurrentTab4 = " class='current' ";
            }
        }

    public void GenerateMiscParams()
    {
        StringBuilder miscBuilder = new StringBuilder();

        if (!string.IsNullOrEmpty("1"))
            miscBuilder.AppendFormat("type={0}", "video");

        if (!string.IsNullOrEmpty(keyword))
            miscBuilder.AppendFormat("&search={0}", keyword);

        if (!string.IsNullOrEmpty(exactKeyword))
            miscBuilder.AppendFormat("&esearch={0}", exactKeyword);

        if (!string.IsNullOrEmpty(withoutKeyword))
            miscBuilder.AppendFormat("&wsearch={0}", withoutKeyword);

        if (!string.IsNullOrEmpty(atLeastKeyword))
            miscBuilder.AppendFormat("&asearch={0}", atLeastKeyword);

        if (!string.IsNullOrEmpty(tag))
            miscBuilder.AppendFormat("&g={0}", tag);

        if (categoryId != -1)
            miscBuilder.AppendFormat("&cat={0}", categoryId);
        
        this.pageUrl = miscBuilder.ToString();

        if (order != OrderVideo.Latest)
            miscBuilder.AppendFormat("&to={0}", (int)order);

        miscParams = miscBuilder.ToString();
    }

    
    public void FullSearch()
    {
        int rowCount;
               
        List<Video> videos = Video.GetVideosByFullSearch(keyword,exactKeyword,withoutKeyword,atLeastKeyword, categoryId,tag,order.ToString(), null,page, 50,out rowCount);
        
        TabContents tabContents = GenerateLister(videos, "", miscParams, page, rowCount,pageSize);

        DefaultHTMLLister = tabContents.HTML;
        DefaultHTMLPager = tabContents.PagerHTML;  
    }
    

    public TabContents GenerateLister(List<Video> Videos, string pageURL, string miscParams, int Page, int maxItems, int PageSize)
    {
        StringBuilder sbHTMLList = new StringBuilder();

        if (Videos.Count > 0)
        {
            for (int i = 0; i < Videos.Count; i++)
            {
                if (Videos.Count <= i)
                {
                    break;
                }

                StringBuilder sbHTMLItem = new StringBuilder();

                object[] parameters = new object[14];

                parameters[0] = ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) + Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
                parameters[1] = Videos[i].TimeAgo;
                parameters[2] = Videos[i].VeryShortTitle;
                parameters[3] = Videos[i].VeryShortDescription;
                parameters[4] = Videos[i].NumberOfViews;
                parameters[5] = Videos[i].NumberOfComments;
                parameters[6] = Videos[i].Member.NickName;
                parameters[7] = Videos[i].Category;
                parameters[8] = Videos[i].WebVideoID;
                parameters[9] = Videos[i].Duration;
                parameters[10] = Videos[i].TotalVoteScore;
                parameters[11] = Videos[i].Member.WebMemberID;
                parameters[12] = RegexPatterns.FormatStringForURL(Videos[i].Title);
                parameters[13] = Videos[i].Title.Replace(@"""","'");

                string HTMLItem = @"<li style='width:120px;text-align:center;padding:0px 0px 0px 0px'>
							<div class='vid_thumb'> <a href='javascript:displayMiniVideo(""{8}"",""{13}"");'><img src='{0}' width='124' height='91' alt='{13}' /></a></div>
                                <a href='/users/{6}'>{6}</a></p>
						</li>";

                sbHTMLItem.AppendFormat(HTMLItem, parameters);
                sbHTMLList.Append(sbHTMLItem.ToString());
            }
        }
        else
        {
            //sbHTMLList.AppendFormat("There is no result{0} page {1}",Videos.Count,page);
            sbHTMLList.AppendFormat("Your <strong>video</strong> search with keyword <strong>{0}</strong> did not match any video.", keyword);
        }
        
        Pager pager = new Pager("/video/", miscParams, page, maxItems);

        pager.PageSize = 20;
      
        // create the TabContents to return
        TabContents tabContents = new TabContents();

        // tabContents.TabType = TabType;
        tabContents.HTML = sbHTMLList.ToString();
        tabContents.PagerHTML = pager.ToString();

        return tabContents;
    }
    
   

    public string GenerateCategories()
    {
        return NCache.GetCategoryLister();
    }


    public string GenerateTagCloud()
    {
        return TagCloudItem.GenerateTagCloud();
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        if (Page.Request.Url.AbsolutePath.ToLower().EndsWith(".aspx"))
        {
            HTTPResponse.PermamentlyMoved301(Context, "/video");
        }

        Master.SkinID = "Video";
        base.OnPreInit(e);
    }
}

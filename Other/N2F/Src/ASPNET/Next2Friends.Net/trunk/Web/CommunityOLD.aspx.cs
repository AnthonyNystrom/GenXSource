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

public partial class CommunityPageOLD : System.Web.UI.Page
{
    public bool IsLoggedIn = false; 
    public Member member;
    public SystemStatistics stats;
    public string DefaultFeaturedNSpotScroller = string.Empty;
    public string DefaultNSpotLister;
    public string DefaultNSpotPager;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(CommunityPageOLD));

        GenerateStats();
        DefaultFeaturedNSpotScroller = GenerateFeaturesNspotLister();

        string strTabType = Request.Params["t"];
        string strPage = Request.Params["p"];

        if (strTabType != null)
        {
            int TabType = Int32.Parse(strTabType);
            int Page = Int32.Parse(strPage);

            TabContents tabContents = GenerateNspotLister(TabType, Page);

            DefaultNSpotLister = tabContents.HTML;
            DefaultNSpotPager = tabContents.PagerHTML;
        }
        else
        {
            TabContents tabContents = GenerateNspotLister((int)TopNspotType.Featured, 1);
            DefaultNSpotLister = tabContents.HTML;
            DefaultNSpotPager = tabContents.PagerHTML;
        }



    }

   
    /// <summary>
    /// Genetate the NSpot lister
    /// </summary>
    public string GenerateFeaturesNspotLister()
    {
        List<NSpot> Nspots = NSpot.GetTop100NSpots(TopNspotType.Featured);

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 6; i++)
        {
            if (Nspots.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[7];

            parameters[0] = Nspots[i].WebNSpotID;
            parameters[1] = Nspots[i].PhotoResourceFile.FullyQualifiedURL;
            parameters[2] = Nspots[i].Name;
            parameters[3] = Nspots[i].Description;
            parameters[4] = Nspots[i].WebNSpotID;
            parameters[5] = Nspots[i].Member.NickName;
            parameters[6] = Nspots[i].NumberOfViews;
 

            string HTMLItem = @"<li><a href='NSpot.aspx?n={0}'><img src='{1}' alt=''></a><br />
							<a href='#'>{2}</a><br/>
							<p class='metadata'>by: <a href='#'>{5}</a> Views: <a href='#'>{6}</a></p>			
						</li>";

                sbHTMLItem.AppendFormat(HTMLItem, parameters);
                sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //<p class='metadata'>{3}</p><br/>

        return sbHTMLList.ToString();
    }


    public void GenerateStats()
    {
        stats = new SystemStatistics();

        // disable caching for dev
        // implement caching for 1 minute
        //Videos = (List<Video>)System.Web.HttpContext.Current.Cache[CacheKey];

        //if (System.Web.HttpContext.Current.Cache.Get(CacheKey) == null)
        //{
        //    Videos = Video.GetTop100Videos((TopVideoType)TabType);
        //    System.Web.HttpContext.Current.Cache.Insert(CacheKey, Videos, null, DateTime.Now.AddMinutes(CacheForMinutes), System.Web.Caching.Cache.NoSlidingExpiration);
        //}
    }

    [AjaxPro.AjaxMethod]
    public TabContents GetListerContent(int TabType)
    {
        return GenerateNspotLister(TabType, 1);

    }

    public TabContents GenerateNspotLister(int TabType,int Page)
    {
        List<NSpot> nspots = NSpot.GetTop100NSpots((TopNspotType)TabType);

        StringBuilder sbHTMLList = new StringBuilder();

        int PageSize = 10;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (nspots.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = nspots[i].WebNSpotID;
            parameters[1] = nspots[i].PhotoResourceFile.FullyQualifiedURL;
            parameters[2] = nspots[i].Name;
            parameters[3] = nspots[i].Description;
            parameters[4] = nspots[i].WebNSpotID;
            parameters[5] = nspots[i].Member.NickName;
            parameters[6] = nspots[i].NumberOfViews;
            parameters[7] = nspots[i].Member.WebMemberID;
            parameters[8] = nspots[i].NumberOfViews;
            parameters[9] = nspots[i].NumberOfComments;
            parameters[10] = nspots[i].TotalVoteScore;
            parameters[11] = TimeDistance.TimeAgo(nspots[i].DTCreated);
     
            string HTMLItem = @"<li>
							<div class='vid_thumb' style='text-align:center;width:131px;overflow:hidden;'> <a href='nspot.aspx?n={0}'><img src='{1}' height='91' alt='thumb' /></a></div>
							<div class='vid_info'>
								<h3><a href='nspot.aspx?n={0}'>{2}</a></h3>
								<p class='timestamp'>{11}</p>

								<div class='vote vote_condensed'><span class='vote_count'>{10}</span></div>
								<p class='metadata'>Views: {8}<br />
								Comments: <a href='#'>{9}</a><br />
								by: <a href='view.aspx?m={7}'>{5}</a></p>
							</div>
						</li>";

            //<li><a href='inbox.aspx?s=Mzk5OWEwN2Y5ZDg5NDg3Mz' class='send_message'>Send Message</a></li>
            //<li><a href='#' class='send_instant'>Send Instant Message</a></li>
            //<li><a href='inbox.aspx?f=Mzk5OWEwN2Y5ZDg5NDg3Mz' class='forward'>Forward to a nspot</a></li>
            //<li><a href='javascript:blocknspot('Mzk5OWEwN2Y5ZDg5NDg3Mz');' class='block'>Block this user</a></li>

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        StringBuilder sbPager = new StringBuilder();

        object[] PagerParameters = new object[4];
        PagerParameters[0] = TabType;
        PagerParameters[1] = Page - 1;
        PagerParameters[2] = Page + 1;
        PagerParameters[3] = TabType;

        if (Page != 1)
            sbPager.AppendFormat("<a  href='?t={3}&p={1}' class='previous'>Previous</a>", PagerParameters);

        sbPager.AppendFormat("<a  href='?t={3}&p={2}' class='next'>next</a>", PagerParameters);


        TabContents tabContents = new TabContents();
        tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        return tabContents;
    }

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

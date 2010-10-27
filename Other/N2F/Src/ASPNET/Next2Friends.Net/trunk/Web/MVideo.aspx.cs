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
using Next2Friends.Misc;

public enum MemberOrderVideo { Latest = 1, NumberOfViews, NumberOfComments, TotalVoteScore };

public partial class MVideo : System.Web.UI.Page
{
    public Member ViewingMember;
    public Member member;
    public int NumberOfVideos;
    public string DefaultLister;
    public string DefaultPager;

    public string DefaultHTMLPager;
    public string DefaultHTMLLister;
    public string CurrentTab1 = string.Empty;
    public string CurrentTab2 = string.Empty;
    public string CurrentTab3 = string.Empty;
    public string CurrentTab4 = string.Empty;
    public MemberOrderVideo CurrentTab = MemberOrderVideo.Latest;
    public int PageTo;
    public string TabParams;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];

        AjaxPro.Utility.RegisterTypeForAjax(typeof(MVideo));

        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        string CurentTab = Request.QueryString["to"];
        string strPage = Request.QueryString["p"];

        try
        {
            CurrentTab = (MemberOrderVideo)int.Parse(CurentTab);
        }
        catch { }

        SetOrderTab();

        PageTo = 1;

        Int32.TryParse(strPage, out PageTo);

        // set the page to 1 if no preference is found in the URL
        PageTo = (PageTo == 0) ? 1 : PageTo;

        if (ViewingMember != null)
        {
            GetVideoLister(ViewingMember.WebMemberID,true);
        }
        else
        {
            // error forward to notavailable.aspx
        }
    }


    private void GetVideoLister(string WebMemberID, bool WrapInP)
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

        string OrderByClause = "";

        switch (CurrentTab)
        {
            case MemberOrderVideo.Latest:
                OrderByClause = "Latest";
                break;
            case MemberOrderVideo.NumberOfViews:
                OrderByClause = "NumberOfViews";
                break;
            case MemberOrderVideo.NumberOfComments:
                OrderByClause = "NumberOfComments";
                break;
            case MemberOrderVideo.TotalVoteScore:
                OrderByClause = "TotalVoteScore";
                break;
        }

        List<Next2Friends.Data.Video> Videos = Next2Friends.Data.Video.GetMemberVideosWithJoinOrdered(ViewingMember.MemberID, privacyType, OrderByClause);
        NumberOfVideos = Videos.Count;
        int DisplayNumberOfVideos = 28;
        int StartIndex = PageTo * DisplayNumberOfVideos - DisplayNumberOfVideos;
        int EndIndex = StartIndex + DisplayNumberOfVideos;

        StringBuilder sbHTML = new StringBuilder();

        for (int i = StartIndex; i < EndIndex; i++)
        {
            if (Videos.Count <= i)
            {
                break;
            }

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
            parameters[13] = Videos[i].Title;

//            sbHTML.AppendFormat(@"<li>
//							<div class='vid_thumb'> <a href='/video/{12}/{8}'><img src='{0}' width='124' height='91' alt='{13}' /></a></div>
//
//							<div class='vid_info'>
//
//								<p class='metadata'><a href='/video/{12}/{8}'>{2}</a></p>
//								<p class='timestamp'>{1}</p>
//								<div class='vote vote_condensed'><span class='vote_count'>{10}</span></div>
//								<p class='metadata'>Views: {4}<br />
//								Comments: <a href='#'>{5}</a><br />
//                                </p>
//							</div>
//						</li>", parameters);

            sbHTML.AppendFormat(@"<li style='width:140px;clear: none;margin-left:3px'>
								<div class='vid_thumb'> <a href='javascript:displayMiniVideo(""{8}"",""{13}"");'><img src='{0}' width='124' height='91' alt='{8}' /></a></div>
							</li>", parameters);

//            object[] parameters = new object[10];

//            parameters[0] = ParallelServer.Get(videos[i].ThumbnailResourceFile.FullyQualifiedURL) + videos[i].ThumbnailResourceFile.FullyQualifiedURL;
//            parameters[1] = videos[i].Duration.ToString();
//            parameters[2] = videos[i].VeryShortTitle;
//            parameters[3] = TimeDistance.TimeAgo(videos[i].DTCreated);
//            parameters[4] = videos[i].VeryShortDescription;
//            parameters[5] = videos[i].NumberOfViews;
//            parameters[6] = videos[i].WebVideoID;
//            parameters[7] = videos[i].NumberOfComments;
//            parameters[8] = videos[i].Title;
//            parameters[9] = RegexPatterns.FormatStringForURL(videos[i].Title);

//            sbHTML.AppendFormat(@"<li>
//								<div class='vid_thumb'> <a href='/video/{9}/{6}'><img src='{0}' width='124' height='91' alt='{8}' /></a></div>
//								<div class='vid_info'>
//									<h3><a href='/video/{9}/{6}'>{2}</a></h3>
//									<p class='timestamp'>{3}</p>
//									<p>{4}</p>
//									<p class='metadata'>Views: {5} Comments: {7}</p>
//								</div>
//							</li>", parameters);

        }


        DefaultHTMLLister = (NumberOfVideos > 0) ? "<ul class='profile_vid_list2' style='padding: 15px 0pt 20px 14px;' id='ulContentLister'>" + sbHTML.ToString() + "</ul>" : "<p>Member currently has no Videos.</p>";

        string MiscPagerParams = string.Empty;

        if (CurrentTab != MemberOrderVideo.Latest)
            MiscPagerParams = "&to=" + ((int)CurrentTab).ToString();

        Pager pager = new Pager("/users/" + ViewingMember.NickName + "/videos/", MiscPagerParams, PageTo, NumberOfVideos);

        pager.PageSize = 20;

        DefaultHTMLPager = pager.ToString();
    }


    public void SetOrderTab()
    {
        int TabType = (int)CurrentTab;
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

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetMVideoTitle(ViewingMember);
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

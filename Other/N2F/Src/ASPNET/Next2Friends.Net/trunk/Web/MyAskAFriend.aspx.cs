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

public partial class AskAFriendMain : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string DefaultTop10Lister = string.Empty;
    public bool IHaveNoQuestions = false;
    public int OrderBy = 1;
    public bool IsLoggedIn = false;
    public Member member = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(AskAFriendMain));

        TabContents tabContents = null;

        string strPage = Request.Params["p"];
        string strType = Request.Params["t"];

        if (strType != null)
        {
            tabContents = GenerateLister(1, strType);
        
        }
        else
        {
            tabContents = GenerateLister(1, "recent");
        }

        GenerateTop10Lister();

        DefaultHTMLLister = tabContents.HTML;
        //DefaultHTMLPager = tabContents.PagerHTML;


        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }
    }

    public void GenerateTop10Lister()
    {
        AskAFriend[] aafTop10 = AskAFriend.GetTopAAFQuestions(1,10);
        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < aafTop10.Length; i++)
		{
		    StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[7];

            parameters[0] = aafTop10[i].WebAskAFriendID;
            parameters[1] = aafTop10[i].DefaultImage.FullyQualifiedURL;
            parameters[2] = aafTop10[i].Question;
            parameters[3] = TimeDistance.TimeAgo(aafTop10[i].WentLiveDT);
            parameters[4] = aafTop10[i].TotalVotes.ToString();
            parameters[5] = aafTop10[i].Member.NickName;

            string HTMLItem = @"<li><a href='AskAFriend.aspx?q={0}'><strong>{2}</strong></a><br />
							<span class='metadata'>By: {5} Total Votes: {4}</span>
						</li>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
		}

        DefaultTop10Lister = sbHTMLList.ToString();
    }


    public TabContents GenerateLister(int Page, string OrderBy)
    {
        Member member = (Member)Session["Member"];

        AskAFriend[] AAFArr = AskAFriend.GetAAFQuestionsByMemberIDWithJoin(member.MemberID, OrderBy);

        IHaveNoQuestions = (AAFArr.Length > 0) ? false : true;

        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 500;
        int StartAt = (Page * PageSize) - PageSize;

        #region code

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (AAFArr.Length <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[7];

            parameters[0] = AAFArr[i].WebAskAFriendID;
            parameters[1] = AAFArr[i].DefaultImage.FullyQualifiedURL;
            parameters[2] = AAFArr[i].Question;
            parameters[3] = TimeDistance.TimeAgo(AAFArr[i].WentLiveDT);
            parameters[4] = AAFArr[i].TotalVotes.ToString();
            parameters[5] = "";
            parameters[6] = "";

            string HTMLItem = @"<li>
							<p class='aaf_img'><a href='AskAFriend.aspx?q={0}'><img src='{1}' alt='img' height='60' /></a></p>
							<h3><a href='AskAFriend.aspx?q={0}'>{2}</a></h3>
							<p class='metadata'> Submitted: {3} Total Votes: {4}</p>
						</li>";
            //By: <a href='view.aspx?m={6}'>{4}</a>
            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //StringBuilder sbPager = new StringBuilder();

        //object[] PagerParameters = new object[4];
        //PagerParameters[0] = TabType;
        //PagerParameters[1] = Page - 1;
        //PagerParameters[2] = Page + 1;
        //PagerParameters[3] = TabType;

        //if (Page != 1)
        //    sbPager.AppendFormat("<a  href='?t={3}&p={1}' class='previous'>Previous</a>", PagerParameters);

        //sbPager.AppendFormat("<a  href='?t={3}&p={2}' class='next'>next</a>", PagerParameters);

        // create the TabContents to return
        TabContents tabContents = new TabContents();

        tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        #endregion

        return tabContents;
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        Member member = (Member)Session["Member"];

        // only allow the member in if logged in
        if (Session["Member"] == null)
        {
            Response.Redirect("signup.aspx");
        }


        Master.SkinID = "None";
        base.OnPreInit(e);
    }


}

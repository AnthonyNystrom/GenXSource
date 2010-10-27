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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

public partial class FriendPage : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string DefaultCurrentBrowsing = string.Empty;
    public Member member;
    public string NumberOfFriends = "0";
    public string NumberOfProximityTags = "0";
    public string NumberOfFriendRequests = "0";
    public FriendStats friendStats;
    public bool ShowStats = false;
    public string PageHeaderTitle = "<h2>My Friends</h2>";
    public string CurrentTab1 = "current";
    public string CurrentTab2 = string.Empty;
    public string CurrentTab3 = string.Empty;
    public string CurrentTab4 = string.Empty;
    public int TabType = 1;

    protected void Page_Load(object sender, EventArgs e)
    {

        AjaxPro.Utility.RegisterTypeForAjax(typeof(FriendPage));

        //string strListerType = Request.Params["t"];
        string strListerType = "px";
        string strSearchkeywords = Request.Params["s"];
        string strPager = Request.Params["p"];

        // determine if a page reqest has been requested otherwise default to page 1
        int PageTo = 1;
        Int32.TryParse(strPager, out PageTo);
        PageTo = (PageTo == 0) ? 1 : PageTo;

        MemberOrderBy OrderBY = SetCurrentTab();

        if (strListerType != null)
        {
            if (strListerType == "blocked")
            {
                List<Member> blocked = Member.GetAllBlockedFriendsByMemberIDForPageLister(member.MemberID);
                NumberOfFriends = blocked.Count.ToString();
                TabContents tabContents = GenerateBlockedLister(blocked, 1, PageTo, true, OrderBY);

                DefaultHTMLLister = tabContents.HTML;
                ShowStats = false;

                PageHeaderTitle = "<h2>Blocked Friends</h2>";
            }
            else if (strListerType == "search")
            {
                ShowStats = false;
                PageHeaderTitle = "<h2>Search</h2>";
            }
            else if (strListerType == "px")
            {
                ShowStats = false;
                PageHeaderTitle = "<h2>Proximity Tags</h2>";
                List<Member> friends = GetProximityTag(member.MemberID);
                TabContents tabContents = GenerateProximityLister(friends, 1, PageTo, true, OrderBY);
                DefaultHTMLLister = tabContents.HTML;
                DefaultHTMLPager = string.Empty;
            }

        }
        else if (strSearchkeywords != null)
        {
            //Search(strSearchkeywords, 1);
            //ShowStats = false;
            //List<Member> friends = member.friend
        }
        else
        {
            List<Member> friends = Member.GetAllFriendsByMemberIDForPageLister(member.MemberID);
            NumberOfFriends = friends.Count.ToString();

            

            TabContents tabContents = GenerateLister(friends, TabType, PageTo, true, OrderBY);

            DefaultHTMLLister = tabContents.HTML;

            ShowStats = true;

            PageHeaderTitle = "<h2>My Friends</h2>";
        }

        friendStats = FriendRequest.GetNumberOfNewFriendRequests(member.MemberID);


        if (friendStats.AllRequests == 0)
        {
            ShowStats = false;
        }
 

        if (!IsPostBack)
        {
            //drpCopuntries.Items.Insert(0, new ListItem("All Countries", "-1"));
            //register the return key default submit button
            //txtSearch.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnSearch.UniqueID + "','')");
        }
    }

    public MemberOrderBy SetCurrentTab()
    {
        CurrentTab1 = string.Empty;
        CurrentTab2 = string.Empty;
        CurrentTab3 = string.Empty;
        CurrentTab4 = string.Empty;

        string strTabType = Request.Params["to"];

        int tab = 1;

        if (strTabType != null)
        {
            Int32.TryParse(strTabType, out tab);
        }

        TabType = (tab == 0) ? 1 : tab;

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

        MemberOrderBy order = MemberOrderBy.FirstName;

        try
        {
            order = (MemberOrderBy)TabType;
        }
        catch { }

        return order;
    }

    [AjaxPro.AjaxMethod]
    public string UnfriendMember(string WebMemberID)
    {
        member = (Member)Session["Member"];
        Member friend = Member.GetMemberViaWebMemberID(WebMemberID);

        Friend.UnFriend(member, friend);

        return WebMemberID;
    }

    public TabContents GenerateBlockedLister(List<Member> Members, int TabType, int Page, bool IsFriend,MemberOrderBy OrderBy)
    {
        Members = SortMembers(Members, OrderBy);

        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 10;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (Members.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[13];

            parameters[0] = Members[i].WebMemberID;
            parameters[1] = ParallelServer.Get() + Members[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = Members[i].NickName;
            parameters[3] = Members[i].FirstName;
            parameters[4] = Members[i].LastName;
            parameters[5] = Members[i].ISOCountry;
            parameters[6] = (Gender)Members[i].Gender;
            parameters[7] = TimeDistance.GetAgeYears(Members[i].DOB);
            parameters[8] = Members[i].CreatedDT.ToString("dd MMMM yyyy");
            parameters[9] = UserStatus.IsUserOnline(Members[i].WebMemberID) ? "<img class=\"online-offline\" src=\"/images/online.gif\" alt=\"Online\" /> Online now" : "<img class=\"online-offline\"  src=\"/images/offline.gif\" alt=\"Offline\" /> Offline";
            parameters[10] = (IsFriend) ? @"<a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")'<img src='images/unfriend.gif' /></a>" : string.Empty;
            parameters[11] = @"/Inbox.aspx?s=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"/Inbox.aspx?f=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);


            string HTMLItem = @"<div class='friend_list blocked clearfix'>
				<div class='profile_pic'>
					<a href='/users/{2}'><img src='{1}' alt='pic' /></a>
				</div>

				
				<div class='friend_data'>
					<p class='friend_name'><a href='/users/{2}'>{3} {4}</a></p>
					<div class='col1'>
					<strong>Location:</strong> {5}<br />
					<strong>Gender:</strong> {6}<br />

					<strong>Age:</strong> 26</div>
					<div class='col2'><strong>Nickname:</strong> <a href='/users/{2}'>{2}</a><br />
						<strong>Joined:</strong> {8}<br />
						<strong>Active:</strong> {9}
					</div>

					<p class='notes'>This member is blocked.<br />
                    <p><a href='javascript:unblockMember(""{0}"")' class='unblock'>Unblock</a></p>	
                    </p>				
				</div>

			</div>";


            //<p class='notes'>You and Lawrence made friend {}. <br />

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        Pager pager = new Pager("/friends/", "to="+TabType, Page, Members.Count);
        pager.PageSize = 15;
        DefaultHTMLPager = pager.ToString();

        // create the TabContents to return
        TabContents tabContents = new TabContents();

        //tabContents.TabType = TabType;
        tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        return tabContents;
    }

    public List<Member> SortMembers(List<Member> Members, MemberOrderBy OrderBy)
    {
        IOrderedEnumerable<Member> Sortedmembers = null;

        if (OrderBy == MemberOrderBy.FirstName)
        {
            Sortedmembers = from M in Members orderby M.FirstName ascending select M;
        }
        else if (OrderBy == MemberOrderBy.LastName)
        {
            Sortedmembers = from M in Members orderby M.LastName ascending select M;
        }
        else if (OrderBy == MemberOrderBy.NickName)
        {
            Sortedmembers = from M in Members orderby M.NickName ascending select M;
        }
        else if (OrderBy == MemberOrderBy.Online)
        {
            Sortedmembers = from M in Members orderby M.NickName ascending select M;
        }

        Members = Sortedmembers.ToList();

        return Members;
    }

    public TabContents GenerateLister(List<Member> Members, int TabType, int Page, bool IsFriend, MemberOrderBy OrderBy)
    {

        Members = SortMembers(Members, OrderBy);

        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 10;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (Members.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[14];

            parameters[0] = Members[i].WebMemberID;
            parameters[1] = ParallelServer.Get() + Members[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = Members[i].NickName;
            parameters[3] = Members[i].FirstName;
            parameters[4] = Members[i].LastName;
            parameters[5] = Members[i].ISOCountry;
            parameters[6] = (Gender)Members[i].Gender;
            parameters[7] = TimeDistance.GetAgeYears(Members[i].DOB);
            parameters[8] = Members[i].CreatedDT.ToString("dd MMMM yyyy");
            //parameters[9] = TimeDistance.TimeAgo(Members[i].LastOnline);
            parameters[9] = UserStatus.IsUserOnline(Members[i].WebMemberID) ? "<img class=\"online-offline\" src=\"/images/online.gif\" alt=\"Online\" /> Online now" : "<img class=\"online-offline\"  src=\"/images/offline.gif\" alt=\"Offline\" /> Offline";
            //parameters[10] = (true) ? @"<a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""">'<img src='images/unfriend.gif' /></a>" : string.Empty;
            //parameters[10] = (IsFriend) ? @"<p><a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")' class='unfriend' >Unfriend</a></p>" : string.Empty;
            parameters[10] = @"<p><a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")' class='unfriend'>UnFriend</a>";

            parameters[11] = @"/Inbox.aspx?s=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"/Inbox.aspx?f=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);

            string HTMLItem = @"<div class='friend_list clearfix' id='divFriend{0}'>

                <div class='profile_pic'>
					<a href='/users/{2}'><img src='{1}' alt='pic' /></a>
				</div>
				<div class='friend_data'>
                    <p class='friend_name'><a href='/users/{2}'>{3} {4}</a></p>
					<div class='col1'>
					<strong>Location:</strong> {5}<br />
					<strong>Gender:</strong> {6}<br />
					<strong>Age:</strong> {7}</div>
                    
					<div class='col2'><strong>Nickname:</strong> <a href='/users/{2}'>{2}</a><br />
						<strong>Joined:</strong> {8}<br />
						 {9}
                        <strong>{13}</strong> 
					</div>

                    <p class='notes'>
                     {10}
                    </p>				
				</div>

				<ul class='friend_actions'>
					<li><a href='{11}' onmouseover='return true;' class='send_message'>Send Message</a></li>	
                    			
					<li><a href='{12}' onmouseover='return true;' class='forward'>Forward to a friend</a></li>
				</ul></div>";
            //<li><a href='javascript:parent.openChatWindowEx(""{0}"");' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>
            //<p class='notes'>You and Lawrence made friend {}. <br />

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //<li><a href='#' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>


        Pager pager = new Pager("/friends/", "to=" + TabType, Page, Members.Count);
        pager.PageSize = 10;
        DefaultHTMLPager = pager.ToString();
        //DefaultHTMLPager = sbPager.ToString();

        //// create the TabContents to return
        TabContents tabContents = new TabContents();

        //tabContents.TabType = TabType;
        tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        return tabContents;
    }

    public TabContents GenerateProximityLister(List<Member> Members, int TabType, int Page, bool IsFriend, MemberOrderBy OrderBy)
    {
        Members = SortMembers(Members, OrderBy);

        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 10;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (Members.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[14];

            parameters[0] = Members[i].WebMemberID;
            parameters[1] = ParallelServer.Get() + Members[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = Members[i].NickName;
            parameters[3] = Members[i].FirstName;
            parameters[4] = Members[i].LastName;
            parameters[5] = Members[i].ISOCountry;
            parameters[6] = (Gender)Members[i].Gender;
            parameters[7] = TimeDistance.GetAgeYears(Members[i].DOB);
            parameters[8] = Members[i].CreatedDT.ToString("dd MMMM yyyy");

            parameters[9] = UserStatus.IsUserOnline(Members[i].WebMemberID) ? "<img class=\"online-offline\" src=\"/images/online.gif\" alt=\"Online\" /> Online now" : "<img class=\"online-offline\"  src=\"/images/offline.gif\" alt=\"Offline\" /> Offline";
            //parameters[10] = (true) ? @"<a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""">'<img src='images/unfriend.gif' /></a>" : string.Empty;
            //parameters[10] = (IsFriend) ? @"<p><a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")' class='unfriend' >Unfriend</a></p>" : string.Empty;
            parameters[10] = @"<p><a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")' class='unfriend'>UnFriend</a>";

            parameters[11] = @"/Inbox.aspx?s=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"/Inbox.aspx?f=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[13] = Members[i].DefaultPhoto.CreatedDT.ToString("dd MMMM yyyy hh:mm tt");

            string HTMLItem = @"<div class='friend_list clearfix' id='divFriend{0}'>

                <div class='profile_pic'>
					<a href='/users/{2}'><img src='{1}' alt='pic' /></a>
				</div>
				<div class='friend_data'>
                    <p class='friend_name'><a href='/users/{2}'>{3} {4}</a></p>
					<div class='col1'>
					<strong>Location:</strong> {5}<br />
					<strong>Gender:</strong> {6}<br />
					<strong>Age:</strong> {7}</div>
                    
					<div class='col2'><strong>Nickname:</strong> <a href='/users/{2}'>{2}</a><br />
						<strong>Joined:</strong> {8}<br />
						<strong>Active:</strong> {9}
					</div>

                    <p class='notes'></p>				
         				
				</div>

				<ul class='friend_actions'>
					<li><a href='{11}' onmouseover='return true;' class='send_message'>Send Message</a></li>	
                    			
					<li><a href='{12}' onmouseover='return true;' class='forward'>Forward to a friend</a></li>
				</ul></div>";
            //<li><a href='javascript:parent.openChatWindowEx(""{0}"");' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>
            //<p class='notes'>You and Lawrence made friend {}. <br />


            //           <p class='notes'>
                     //Tagged: {10}
                   // </p>
            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //<li><a href='#' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>


        Pager pager = new Pager("/friends/", "to=" + TabType, Page, Members.Count);
        pager.PageSize = 10;
        DefaultHTMLPager = pager.ToString();


        //// create the TabContents to return
        TabContents tabContents = new TabContents();

        //tabContents.TabType = TabType;
        tabContents.HTML = sbHTMLList.ToString();
        //tabContents.PagerHTML = sbPager.ToString();

        return tabContents;
    }


    /// <summary>
    /// Creates a friend request for the Member
    /// </summary>
    /// <param name="WebMemberID">The WebMemberID of the friend request</param>
    /// <returns>1 = OK, 2 = Not logged in, 3 = already a friend</returns>
    [AjaxPro.AjaxMethod]
    public AddToFriendsResponse AddToFriends(string WebMemberID)
    {
        Member WouldbeFriend = Member.GetMemberViaWebMemberID(WebMemberID);
        member = (Member)Session["Member"];

        bool MadeFriendRequest = FriendRequest.CreateWebFriendRequest(member.MemberID, WouldbeFriend.MemberID);
       
        AddToFriendsResponse response = new AddToFriendsResponse();
        response.Response = MadeFriendRequest;
        response.WebMemberID = WebMemberID;

        return response;
    }

    [AjaxPro.AjaxMethod]
    public string UnblockMember(string WebMemberID)
    {
        member = (Member)Session["Member"];

        if (member != null)
        {
            Friend.UnblockFriend(member.MemberID, WebMemberID);
        }

        return WebMemberID;
    }


    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("signup.aspx");
        }

        Master.SkinID = "Friend";
        base.OnPreInit(e);
    }


    public static List<Member> GetProximityTag(int MemberID)
    {
        Database db = DatabaseFactory.CreateDatabase();
        DbCommand dbCommand = db.GetStoredProcCommand("HG_GetFriendTagByMemberID");
        db.AddInParameter(dbCommand, "MemberID", DbType.String, MemberID);

        List<Member> members = new List<Member>();


        //execute the stored procedure
        using (IDataReader dr = db.ExecuteReader(dbCommand))
        {
            members = Member.PopulateMemberWithJoin(dr);
            dr.Close();
        }

        return members;
    }


    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = "Proximity tags";
        //Master.MetaDescription = "Live mobile video broadcasting networking";
        //Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

public class AddToFriendsResponse
{
    public bool Response { get; set; }
    public string WebMemberID { get; set; }
}


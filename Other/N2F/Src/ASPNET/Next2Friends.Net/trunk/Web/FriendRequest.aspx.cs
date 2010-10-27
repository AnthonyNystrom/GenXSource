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



public partial class FriendRequestPage : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string DefaultHTMLFriendLister = string.Empty;
    public string NumberOfFriendRequests = string.Empty;
    public string ListerType = string.Empty;
    public bool LoadFriends = false;

    public int PageListerType = -1;
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strTypeListerID = Request.Params["t"];
        string strLoadFriendsLister = Request.Params["lf"];

        ListerType = "Friend Requests";

        if (strTypeListerID != null)
        {
            if (strTypeListerID == "all")
            {
                PageListerType = -1;
            }
            else if (strTypeListerID == "px")
            {
                ListerType = "Proximity Tags";
                PageListerType = 1;
            }
            else if (strTypeListerID == "web")
            {
                ListerType = "Web Friend Requests";
                
                PageListerType = 0;
            }
            else
            {
                PageListerType = -1;
            }

        }
        
        AjaxPro.Utility.RegisterTypeForAjax(typeof(FriendRequestPage));
        List<FriendRequest> friendRequests = FriendRequest.GetAllNewFriendRequestByMemberID(member.MemberID, PageListerType);
        NumberOfFriendRequests = friendRequests.Count.ToString();
        TabContents tabContents = GenerateLister(friendRequests, 0, 1);
        DefaultHTMLLister = tabContents.HTML;
    }

    [AjaxPro.AjaxMethod]
    public string SetFriendStatus(string WebFriendRequestID, bool IsAccepted)
    {
        member = (Member)Session["Member"];

        FriendRequest.SetFriendRequestStatus(member.MemberID, WebFriendRequestID, IsAccepted);
        
        List<FriendRequest> friendRequests = FriendRequest.GetAllNewFriendRequestByMemberID(member.MemberID, PageListerType);

        return friendRequests.Count.ToString();
    }


    [AjaxPro.AjaxMethod]
    public string UnfriendMember(string WebMemberID)
    {
        member = (Member)Session["Member"];
        Member friend = Member.GetMemberViaWebMemberID(WebMemberID);

        Friend.UnFriend(member, friend);

        return WebMemberID;
    }


    public TabContents GenerateLister(List<FriendRequest> friendRequests, int TabType, int Page)
    {
        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 10;
        int StartAt = (Page * PageSize) - PageSize;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {
            if (friendRequests.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[15];

            parameters[0] = friendRequests[i].FriendMember.WebMemberID;
            parameters[1] = "http://www.next2friends.com/user" + "/" + friendRequests[i].PhotoURL;
            parameters[2] = friendRequests[i].FriendMember.NickName;
            parameters[3] = friendRequests[i].FriendMember.FirstName;
            parameters[4] = friendRequests[i].FriendMember.LastName;
            parameters[5] = friendRequests[i].FriendMember.ISOCountry;
            parameters[6] = (Gender)friendRequests[i].FriendMember.Gender;
            parameters[7] = TimeDistance.GetAgeYears(friendRequests[i].FriendMember.DOB);
            parameters[8] = friendRequests[i].DTCreated.ToString("MMMM yyyy");
            parameters[9] = friendRequests[i].WebFriendRequestID;
            parameters[10] = TimeDistance.TimeAgo(friendRequests[i].DTCreated);
            parameters[11] = @"Inbox.aspx?s=" + friendRequests[i].FriendMember.WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"Inbox.aspx?f=" + friendRequests[i].FriendMember.WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[13] = TimeDistance.TimeAgo(friendRequests[i].FriendMember.LastOnline);
            parameters[14] = friendRequests[i].DTCreated.ToString("dd MMM yyy");

//            string HTMLItem = @"<div class='friend_list clearfix' id='divFriendRequest{9}'>
//			<div class='profile_pic'>
//				<a href='view.aspx?m={0}'><img src='{1}' alt='pic' /></a>			
//			</div>
//			
//			<div class='friend_data'>
//				<p class='friend_name'><a href='view.aspx?m={0}'>{3} {4}</a></p>
//
//				<div class='col1'>
//					<strong>Location:</strong> {5}<br />
//					<strong>Gender:</strong> {6}<br />
//					<strong>Age:</strong> {7}</div>
//
//					<div class='col2'><strong>Nickname:</strong> <a href='view.aspx?m={0}'>{2}</a><br />
//						<strong>Joined:</strong> {8}<br />
//						<strong>Last online:</strong> {13}
//				</div>
//					<p class='notes'>{2} has requested to be your friend: {10}<br />
//
//			</div>
//
//			<ul class='friend_actions'>
//				<li><a href='{11}' onmouseover='return true;' class='send_message'>Send Message</a></li>
//				<li><a href='#' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>
//				<li><a href='{12}' onmouseover='return true;' class='forward'>Forward to a friend</a></li>
//    		</ul>
//		</div>";

            string HTMLItem = @"<div class='friend_list clearfix' id='divFriendRequest{9}'>
				<div class='profile_pic'>
					<a href='/users/{2}'><img src='{1}' alt='pic' /></a>	
				</div>
				
				<div class='friend_data'>
					<p class='friend_name'><a href='/users/{2}'>{3} {4}</a></p>
					<div class='col1'>
					<strong>Location:</strong> {5}<br />
					<strong>Gender:</strong> {6}<br />
					<strong>Age:</strong> {7}";

            if (PageListerType == 1)
            {
                //bt
            HTMLItem += @"<br /><strong>Tagged:</strong> {14}";
            }


                HTMLItem += @"</div><div class='col2'><strong>Nickname:</strong> <a href='/users/{2}'>{2}</a><br />
						<strong>Joined:</strong> {8}<br />
						<strong>Active:</strong> {13}
					</div>
					<p class='notes'>{2} has requested to be your friend: {10}<br />
					<p><a href='javascript:setFriendStatus(""{9}"",true);' class='acceptFriendRequest'>accept</a> <a href='javascript:setFriendStatus(""{9}"",false);' class='rejectFriendRequest'>ignore</a></p>				
				</div>

				<ul class='friend_actions'>
				    <li><a href='{11}' onmouseover='return true;' class='send_message'>Send Message</a></li>
				    <li><a href='#' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>
				    <li><a href='{12}' onmouseover='return true;' class='forward'>Forward to a friend</a></li>
				</ul>
			</div>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        // if there are no friend requests then suggest a search
        if (friendRequests.Count == 0)
        {
            if (PageListerType == 0)
            {
                //web
                sbHTMLList.Append("<div class='friend_list clearfix'>You have no new friend requests. Why dont you <a href='Community.aspx'>search for some new friends</a>?</div>");
            }
            else if (PageListerType == 1)
            {
                //bt
                sbHTMLList.Append("<div class='friend_list clearfix'>You have no new Proximity matches. You should probably get out more!<br/><br/>In the mean time why dont you <a href='Community.aspx'>search for some new friends</a>?</div>");
            }
            else
            {
                sbHTMLList.Append("<div class='friend_list clearfix'>You have no new Friend requests or Proximity matches. You should probably get out more!<br/><br/>In the mean time why dont you <a href='Community.aspx'>search for some new friends</a>?</div>");
            }
                
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

        //// create the TabContents to return
        TabContents tabContents = new TabContents();

        //tabContents.TabType = TabType;
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

        // only allow the member in if logged in
        if (Session["Member"] == null)
        {
            Response.Redirect("signup.aspx?u=" + Request.Url.AbsoluteUri);
        }

        Master.SkinID = "Friend";
        base.OnPreInit(e);
    }
}

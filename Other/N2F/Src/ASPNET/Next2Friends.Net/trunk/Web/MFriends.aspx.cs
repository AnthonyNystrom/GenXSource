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

public partial class MFriends : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public Member ViewingMember;
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(MFriends));
        member = (Member)Session["Member"];
        string strPager = Request.Params["p"];

        // determine if a page reqest has been requested otherwise default to page 1
        int PageTo = 1;
        Int32.TryParse(strPager, out PageTo);
        PageTo = (PageTo == 0) ? 1 : PageTo;

        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        if (ViewingMember != null)
        {
            List<Member> friends = Member.GetAllFriendsByMemberIDForPageLister(ViewingMember.MemberID);
            GenerateLister(friends, 1, PageTo, true);
        }
    }

    public void GenerateLister(List<Member> Members, int TabType, int Page, bool IsFriend)
    {
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
            parameters[10] = @"javascript:unfriendMember(""" + Members[i].WebMemberID + @""")";

            parameters[11] = @"/Inbox.aspx?s=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"/Inbox.aspx?f=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);

            string HTMLItem = @"<div class='friend_list clearfix' style='background:transparent;' id='divFriend{0}'>

                <div class='profile_pic'>
					<a href='/users/{2}'><img src='{1}' alt='{3}' /></a>
				</div>
				<div class='friend_data'>
                    <p class='friend_name'><a href='/users/{2}'>{3} {4}</a></p>
					<div class='col1'>
					<strong>Location:</strong> {5}<br />
					<strong>Gender:</strong> {6}<br />
					<strong>Age:</strong> {7}<br />
                    {9}</div>
                    
					<div class='col2'><strong>Nickname:</strong> <a href='/users/{2}'>{2}</a><br />
						<strong>Joined:</strong> {8}
                        
					</div>

                    <p class='notes'>
                    
                    </p>				
				</div>

				<ul class='friend_actions'>
					<li><a href='{11}' onmouseover='return true;' class='send_message'>Send Message</a></li>	
                    			
					<li><a href='{12}' onmouseover='return true;' class='forward'>Forward to a friend</a></li>
                ";

            if (member != null)
            {
                if (member.MemberID == ViewingMember.MemberID)
                {
                    HTMLItem += @"<li><a href='{10}' onmouseover='return true;' class='block'>Unfriend</a></li>";
                }
            }

                    
			HTMLItem += @"</ul></div>";
            //<li><a href='javascript:parent.openChatWindowEx(""{0}"");' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>
            //<p class='notes'>You and Lawrence made friend {}. <br />

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        //<li><a href='#' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>

        Pager pager = new Pager("/users/"+ViewingMember.NickName+"/friends/", "", Page, Members.Count);
        pager.PageSize = 10;

        DefaultHTMLPager = (Members.Count>0) ? "<span>"+pager.ToString()+"</span>" : string.Empty;
        DefaultHTMLLister = (Members.Count>0) ? sbHTMLList.ToString() : "<p>Member currently has no Friends.</p>";
    }

    [AjaxPro.AjaxMethod]
    public string UnfriendMember(string WebMemberID)
    {
        member = (Member)Session["Member"];
        Member friend = Member.GetMemberViaWebMemberID(WebMemberID);

        Friend.UnFriend(member, friend);

        return WebMemberID;
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetMFriendsTitle(ViewingMember);
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

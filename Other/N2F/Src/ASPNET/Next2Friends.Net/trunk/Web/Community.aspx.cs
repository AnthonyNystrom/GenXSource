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


public partial class CommunityPage : System.Web.UI.Page
{
    public string DefaultHTMLLister = string.Empty;
    public string DefaultHTMLPager = string.Empty;
    public string DefaultCurrentBrowsing = string.Empty;
    public Member member;
    public string NumberOfFriends = "0";
    public string NumberOfPeople = "0";
    public string NumberOfProximityTags = "0";
    public string NumberOfFriendRequests = "0";
    public string HTMLPageTitle = "";
    public string CurrentTab1 = "current";
    public string CurrentTab2 = string.Empty;
    public string CurrentTab3 = string.Empty;
    public string CurrentTab4 = string.Empty;
    public string CurrentTab5 = string.Empty;
    public string CurrentTab6 = string.Empty;
    public string CurrentTab7 = string.Empty;
    public string CurrentTab8 = string.Empty;

    private bool AllCountry = false;
    private bool AllHobby = false;
    private bool AllProfession = false;
    private int PageTo = 1;

    public string HideHobby = "visibility:hidden;display:none";
    public string HideProfession = "visibility:hidden;display:none";
    public string HideCountry = "visibility:hidden;display:none";


    public int TabType = 1;
    public string pageUrl=string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(CommunityPage));

        if (!Page.IsPostBack)
        {
            drpProfession.Items.Insert(0, new ListItem("All Professions", "-1"));
            drpProfession.SelectedIndex = 0;
            
            drpHobby.Items.Insert(0, new ListItem("All Interests", "-1"));
            drpHobby.SelectedIndex = 0;
            
            drpCountry.Items.Insert(0, new ListItem("All Countries", "-1"));
            drpCountry.SelectedIndex = 0;            
        }

            member = (Member)Session["Member"];

            string strListerType = Request.Params["t"];
            string strSearchkeywords = Request.Params["search"];
            string strPager = Request.Params["p"];

            Int32.TryParse(strPager, out PageTo);
            PageTo = (PageTo == 0) ? 1 : PageTo;

        //if (!String.IsNullOrEmpty(strListerType) && strListerType.Trim() == "full")
        //    {
        
        //               FullSearch(PageTo);
        //     }
        //    else
        //    {
        //        List<Member> NewestMembers = Member.GetTop100LatestMembers();
        //        NumberOfPeople = NewestMembers.Count.ToString();

        //        TabContents tabContents = GenerateSearchLister(NewestMembers, "", null, PageTo);

        //        DefaultHTMLLister = tabContents.HTML;
        //        DefaultHTMLPager = tabContents.PagerHTML;

        //    }
        
        if ( !Page.IsPostBack )
        {
            FullSearch(PageTo);
        }
    }

    public void drpCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        AllCountry = true;
        FullSearch(PageTo);
    }

    public void drpHobby_SelectedIndexChanged(object sender, EventArgs e)
    {
        AllHobby = true;
        FullSearch(PageTo);
    }

    public void drpProfession_SelectedIndexChanged(object sender, EventArgs e)
    {
        AllProfession = true;
        FullSearch(PageTo);
    }

    public enum OrderType { MostVideos = 1, FirstName, LastName, NewestJoined, DTOnline, Trade, Hobby };

    public OrderType SetCurrentTab()
    {
        CurrentTab1 = string.Empty;
        CurrentTab2 = string.Empty;
        CurrentTab3 = string.Empty;
        CurrentTab4 = string.Empty;
        CurrentTab5 = string.Empty;
        CurrentTab6 = string.Empty;
        CurrentTab8 = string.Empty;

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
        else if (TabType == 5)
        {
            CurrentTab5 = " class='current' ";
        }
        else if (TabType == 6)
        {
            CurrentTab6 = " class='current' ";
            HideProfession = "";
        }
        else if (TabType == 7)
        {
            CurrentTab7 = " class='current' ";
            HideHobby = "";
        }
        else if (TabType == 8)
        {
            CurrentTab8 = " class='current' ";
            HideCountry = "";
        }

        OrderType order = OrderType.MostVideos;

        try
        {
            order = (OrderType)TabType;
        }
        catch { }

        return order;
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

    //public void Search(string SearchKeyword)
    //{
    //    string strPager = Request.Params["p"];
    //    int PageTo = 1;
    //    Int32.TryParse(strPager, out PageTo);
    //    PageTo = (PageTo == 0) ? 1 : PageTo;

    //    if (SearchKeyword != string.Empty)
    //    {
    //        List<Member> People;

    //     //   string Country = (drpCopuntries.SelectedIndex > 0) ? drpCopuntries.SelectedValue : "";
    //        string Country ="";
    //        int Gender = -1;

    //        People = Member.GetMembersByKeywordSearch(SearchKeyword, Country, Gender);

    //        NumberOfFriends = People.Count.ToString();

    //        if (People.Count == 0)
    //        {
    //            string Scope = string.Empty;

    //            Scope = "Your <strong>Next2Friends Network</strong> search ";
    //            HTMLPageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";

    //            DefaultHTMLLister = "<br />" + Scope + " with keyword <strong>" + SearchKeyword + "</strong> did not match any members.<br />";
    //            DefaultHTMLPager = string.Empty;
    //        }
    //        else
    //        {
    //            TabContents tabContents = GenerateSearchLister(People,"",null, PageTo);
    //            DefaultHTMLLister = tabContents.HTML;
    //            DefaultHTMLPager = tabContents.PagerHTML;
    //        }

    //    }
    //    else
    //    {
    //        DefaultHTMLLister = string.Empty;
    //        DefaultHTMLPager = string.Empty;
    //    }


    //    HTMLPageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";

    //}

    public void FullSearch(int pageTo)
    {
        List<Member> People;
        //community.aspx?search=&sex=-1&country=US&city=&avatar=yes

        //string keyword ="omid reyhani";
        //string useWildcard = null;
        //string firstname = null;
        //string lastname = null;
        //string nikename = null;
        //int gender = -1;
        //string country ="";
        //string city = "";
        //int hasAvatarPhoto = -1;

        string keyword = Request.QueryString["search"];
        string firstname = null;
        string lastname = null;
        string nikename = null;
        int gender = -1;
        if (!string.IsNullOrEmpty(Request.QueryString["sex"]))
            int.TryParse(Request.QueryString["sex"], out gender);

        string country = null;// Request.QueryString["country"];
        string city = Request.QueryString["city"];

        bool hasAvatarPhoto = false;
        if (!string.IsNullOrEmpty(Request.QueryString["avatar"]) && Request.QueryString["avatar"].Trim() == "yes")
            hasAvatarPhoto = true;

        string profile = Request.QueryString["profile"];
        string email = Request.QueryString["email"];

        string type = Request.QueryString["type"];

        string sortOrder = Request.QueryString["to"];

        int professionId = -1;
        int hobbyId = -1;
        string ctr = string.Empty;

        try
        {
            if (Request.QueryString["pId"] != null)
            {
                professionId = int.Parse(Request.QueryString["pId"]);                
            }

            //override with drop down value
            if( drpProfession.SelectedValue != "-1" || AllProfession )
            {
                professionId = int.Parse(drpProfession.SelectedValue);
            }
            
            drpProfession.SelectedValue = professionId.ToString();
        }
        catch { }

        try
        {
            if (Request.QueryString["hId"] != null)
            {
                hobbyId = int.Parse(Request.QueryString["hId"]);
            }

            //override with drop down value
            if (drpHobby.SelectedValue != "-1" || AllHobby)
            {
                hobbyId = int.Parse(drpHobby.SelectedValue);
            }

            drpHobby.SelectedValue = hobbyId.ToString();
        }
        catch { }

        try
        {
            if (Request.QueryString["country"] != null)
            {
                country = Request.QueryString["country"];
            }

            //override with drop down value
            if (drpCountry.SelectedValue != "-1" || AllCountry)
            {
                country = drpCountry.SelectedValue;
            }

            if (country == "-1")
                country = null;

            drpCountry.SelectedValue = country;
        }
        catch { }   

        StringBuilder newMiscBuilder = new StringBuilder();

        newMiscBuilder.AppendFormat("t=full");
        if (!string.IsNullOrEmpty(type))
            newMiscBuilder.AppendFormat("&type={0}", type);
        if (!string.IsNullOrEmpty(keyword))
            newMiscBuilder.AppendFormat("&search={0}", keyword);
        if (gender != -1)
            newMiscBuilder.AppendFormat("&sex={0}", gender);
        if (!string.IsNullOrEmpty(country))
            newMiscBuilder.AppendFormat("&country={0}", country);
        if (!string.IsNullOrEmpty(city))
            newMiscBuilder.AppendFormat("&city={0}", city);
        if (hasAvatarPhoto == true)
            newMiscBuilder.Append("&avatar=yes");
        if (!string.IsNullOrEmpty(email))
            newMiscBuilder.AppendFormat("&email={0}", email);
        if (!string.IsNullOrEmpty(profile))
            newMiscBuilder.AppendFormat("&profile={0}", profile);
        if (hobbyId != -1)
            newMiscBuilder.AppendFormat("&hId={0}", hobbyId);
        if (professionId != -1)
            newMiscBuilder.AppendFormat("&pId={0}", professionId);

        this.pageUrl = newMiscBuilder.ToString();

        if (!string.IsNullOrEmpty(sortOrder))
            newMiscBuilder.AppendFormat("&to={0}", sortOrder);


        string miscParams = newMiscBuilder.ToString();

        OrderType orderByType = SetCurrentTab();

        string OrderByString = orderByType.ToString();

        if (orderByType == OrderType.Trade)
        {
            OrderByString = "DayJob, NightJob";
        }

        if (orderByType == OrderType.FirstName || orderByType == OrderType.LastName)
        {
            OrderByString += " ASC";
        }
        else
        {
            OrderByString += " DESC";
        }

        //  People = Member.GetMembersByFullSearch(keyword,useWildcard,firstname,lastname,nikename,gender,country,city,hasAvatarPhoto); //SetCurrentTab
        //if (orderByType != OrderType.Hobby && orderByType != OrderType.Trade)
        //{
        //    People = GetMembersByFullSearch(keyword, useWildcard, firstname, lastname, nikename, gender, country, city, hasAvatarPhoto, email, profile, OrderByString);
        //}
        //else
        {
            People = Member.GetMembersByFullSearch2(keyword, firstname, lastname, nikename, gender, country, city, hasAvatarPhoto, email,professionId,hobbyId, profile, OrderByString);
        }


        NumberOfPeople = People.Count.ToString();

        if (People.Count == 0)
        {
            string Scope = string.Empty;

            Scope = "Your <strong>Next2Friends Network</strong> search ";
            HTMLPageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";

            DefaultHTMLLister = "<br />" + Scope + " with keyword <strong>" + keyword + "</strong> did not match any members.<br />";
            DefaultHTMLPager = string.Empty;
        }
        else
        {
            TabContents tabContents = GenerateSearchLister(People,"",newMiscBuilder.ToString(), pageTo);
            DefaultHTMLLister = tabContents.HTML;
            DefaultHTMLPager = tabContents.PagerHTML;
        }


    //    PageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";

    }

    //public void SearchFriends(string SearchKeyword)
    //{
    //    string strPager = Request.Params["p"];
    //    int PageTo = 1;
    //    Int32.TryParse(strPager, out PageTo);
    //    PageTo = (PageTo == 0) ? 1 : PageTo;
    //    if (SearchKeyword != string.Empty)
    //    {
    //        List<Member> People;

    //        People = Member.GetFriendsByKeywordSearch(member.MemberID, SearchKeyword, "", -1);

    //        NumberOfFriends = People.Count.ToString();

    //        if (People.Count == 0)
    //        {
    //            string Scope = string.Empty;

    //            Scope = "Your <strong>Next2Friends Network</strong> search ";
    //            HTMLPageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";

    //            DefaultHTMLLister = "<br />" + Scope + " with keyword <strong>" + SearchKeyword + "</strong> did not match any members.<br />";
    //            DefaultHTMLPager = string.Empty;
    //        }
    //        else
    //        {
    //            TabContents tabContents = GenerateSearchLister(People, "", "t=friends", PageTo);
    //            DefaultHTMLLister = tabContents.HTML;
    //            DefaultHTMLPager = tabContents.PagerHTML;
    //        }


    //        HTMLPageTitle = "<h2>Search Results <small>(" + NumberOfFriends + " match your criteria)</small></h2>";


    //    }
    //    //else
    //    //    PageTitle = "haha";
    //}

    public static List<Member> GetMembersByFullSearch(string Keyword, string UseWildcard, string FirstName,
    string LastName, string NickName, int Gender, string Country, string City, int HasAvatarPhoto, string email, string profile,string order)
    {
        Database db = DatabaseFactory.CreateDatabase();

        DbCommand dbcommand = db.GetStoredProcCommand("HG_GetMembersByFullSearch");
        db.AddInParameter(dbcommand, "Keyword", DbType.String, Keyword);
        db.AddInParameter(dbcommand, "UseWildcard", DbType.String, UseWildcard);
        db.AddInParameter(dbcommand, "FirstName", DbType.Int16, FirstName);
        db.AddInParameter(dbcommand, "LastName", DbType.String, LastName);
        db.AddInParameter(dbcommand, "NickName", DbType.String, NickName);
        db.AddInParameter(dbcommand, "Gender", DbType.Int16, Gender);
        db.AddInParameter(dbcommand, "Country", DbType.String, Country);
        db.AddInParameter(dbcommand, "City", DbType.String, City);
        db.AddInParameter(dbcommand, "HasAvatarPhoto", DbType.Int16, HasAvatarPhoto);
        db.AddInParameter(dbcommand, "Email", DbType.String, email);
        db.AddInParameter(dbcommand, "ProfileKeywords", DbType.String, profile);
        db.AddInParameter(dbcommand, "OrderByClause", DbType.String, order);
        



        List<Member> Members;

        using (IDataReader dr = db.ExecuteReader(dbcommand))
        {
            Members =Member.PopulateMemberWithJoin(dr);
            dr.Close();
        }

        return Members;
    }

    public TabContents GenerateSearchLister(List<Member> Members,string pageURL,string miscParams, int Page)
    {
       //MemberOrderBy OrderBY = SetCurrentTab();
       //Members = SortMembers(Members, OrderBy);
        StringBuilder sbHTMLList = new StringBuilder();
        int PageSize = 20;
        int StartAt = (Page * PageSize) - PageSize;
        bool isFriend = false;

        for (int i = StartAt; i < StartAt + PageSize; i++)
        {

            if (Members.Count <= i)
            {
                break;
            }

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[23];
            
            parameters[0] = Members[i].WebMemberID;
            parameters[1] = Next2Friends.Data.ParallelServer.Get() + Members[i].DefaultPhoto.FullyQualifiedURL;
            parameters[2] = Members[i].NickName;
            parameters[3] = Members[i].FirstName;
            parameters[4] = Members[i].LastName;
            parameters[5] = (Members[i].ISOCountry!="Unspecified") ?  Members[i].ISOCountry : string.Empty; //+city

            if (Members[i].AccountType == (int)AccountType.Personal)
            {
                //parameters[6] = (Gender)Members[i].Gender;
                //parameters[7] = TimeDistance.GetAgeYears(Members[i].DOB);
                //<strong>Gender:</strong> {6}<br />
				//<strong>Age:</strong> {7}<br />

                parameters[6] =string.Format("<strong>Gender:</strong> {0}<br />", (Gender)Members[i].Gender);

                // if he user signed up from the web service then the year will be 1900
                if (Members[i].DOB.Year == 1900)
                {
                    parameters[7] = "<strong>Age:</strong> unspecified<br />";
                }
                else
                {
                    parameters[7] = string.Format("<strong>Age:</strong> {0}<br />", TimeDistance.GetAgeYears(Members[i].DOB));
                } 
               
            }
            else if (Members[i].AccountType == (int)AccountType.Business)
            {

                parameters[6] = "";
                parameters[7] = "";
            }

            parameters[8] = Members[i].CreatedDT.ToString("dd MMMM yyyy");
            parameters[9] = UserStatus.IsUserOnline(Members[i].WebMemberID) ? "<img class=\"online-offline\" src=\"/images/online.gif\" alt=\"Online\" /> Online now" : "<img class=\"online-offline\"  src=\"/images/offline.gif\" alt=\"Offline\" /> Offline";
            // parameters[10] = (IsFriend) ? @"<a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")'<img src='images/unfriend.gif' /></a>" : string.Empty;
            //parameters[10] = (IsFriend) ? @"<p class='notes'><a href='#' class='addto_friends'>Add to Friends</a>" : "<a href='#' class='addto_friends added'>Already a Friend</a></p>	";
            parameters[10] = "";//@"<p><a href='javascript:unfriendMember(""" + Members[i].WebMemberID + @""")' class='unfriend'>UnFriend</a></p>";

            parameters[11] = @"Inbox.aspx?s=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[12] = @"Inbox.aspx?f=" + Members[i].WebMemberID + @"&r=" + Server.UrlEncode(Request.Url.PathAndQuery);

            parameters[19] = Members[i].NickName;

            parameters[20] = Members[i].ISOCode;
            parameters[21] = Members[i].ISOCountry;


            if (Members[i].MemberProfile[0].RelationshipStatus != -1 && Members[i].MemberProfile[0].RelationshipStatus != 0)
            {
                parameters[22] = "<strong>Status: </strong>" + GetRelationShipStatus(Members[i].MemberProfile[0].RelationshipStatus) + "<br />";
            }
            else
            {
                parameters[22] = "";
            }


            if (Members[i].MemberProfile[0].NumberOfPhotos > 0)
            {
                parameters[13] = "<strong>Photos :</strong><a href= \"/users/" + Members[i].NickName + "/photos\">" + Members[i].MemberProfile[0].NumberOfPhotos + "</a><br />";
            }
            else
            {
                parameters[13] = "";
            }

            if (Members[i].MemberProfile[0].NumberOfVideos > 0)
            {
                parameters[14] = "<strong>Videos :</strong><a href= \"/users/" + Members[i].NickName + "/videos\">" + Members[i].MemberProfile[0].NumberOfVideos + "</a><br />";
            }
            else
            {
                parameters[14] = "";
            }

            if (Members[i].MemberProfile[0].NumberOfViews > 0)
            {
                parameters[15] = "<strong>Views :</strong><a href= \"/users/" + Members[i].NickName + "\">" + Members[i].MemberProfile[0].NumberOfViews + "</a><br />";
            }
            else
            {
                parameters[15] = "";
            }

            try
            {
               isFriend = Friend.IsFriend(member.MemberID, Members[i].MemberID);
            }
            catch { }

            if (isFriend)
            {
                parameters[16] = (member != null) ? "<a href='javascript:void(0);' class='add_to_friends'>Already a Friend</a><span id='spanAddToFriends" + Members[i].WebMemberID + "'><img src='/images/check.gif' /></span>" : string.Empty;
            }
            else
            {
            parameters[16] = (member != null) ? "<a href='javascript:addTofriends(\"" + Members[i].WebMemberID + "\");' class='add_to_friends'>Send Friend Request</a><span id='spanAddToFriends" + Members[i].WebMemberID + "'></span>" : string.Empty;
            }
            
            parameters[17] = (member != null) ? "inbox.aspx?s=" + Members[i].WebMemberID : "signup.aspx?r=" + Server.UrlEncode(Request.Url.PathAndQuery);
            parameters[18] = (member != null) ? "inbox.aspx?f=" + Members[i].WebMemberID : "signup.aspx?r=" + Server.UrlEncode(Request.Url.PathAndQuery);

            string HTMLItem = @"<div class='communitylist clearfix'>
				                <div class='profile_pic'>
					                <a href='/users/{2}'><img src='{1}' alt='{3} {4}' /></a> 
				                </div>
                				
				                <div class='friend_data'>
					                <p class='friend_name'><img src='/images/flags/{20}.gif' title='{21}'  alt='{21}'> <a href='/users/{2}'>{3} {4}</a> </p>
					                <p>
					                <strong>Nickname:</strong> {19}<br />
                                       {6}{7}
                                    {22}
                                    {13}{14}{15}
                                    
                                    {9}<br />
                                    <br />
                                    {16}                                    
                                                                        
                                    </p>
				                </div>
			               </div>";

            //     <p class='notes'>{3} is not your friend yet. <br />
            //						                <strong>Joined:</strong> {8}<br />
            //<li><a href='javascript:parent.openChatWindowEx(""{0}"");' onmouseover='return true;' class='send_instant'>Send Instant Message</a></li>	
            //<p class='notes'>You and Lawrence made friend {}. <br />
            //<li><a href='#' class='block'>Block this user</a></li>
            //<strong>profile views :</strong> {15}
            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        StringBuilder sbPager = new StringBuilder();

        object[] PagerParameters = new object[2];
        PagerParameters[0] = Page - 1;
        PagerParameters[1] = Page + 1;

        int PreviousPage = Page - 1;
        int NextPage = Page + 1;

        if (Page != 1)
            sbPager.AppendFormat("<a  href='?p={0}' class='previous'>Previous</a>", PagerParameters);

        if (Members.Count >= (Page * PageSize))
            sbPager.AppendFormat("<a  href='?p={1}' class='next'>Next</a>", PagerParameters);


       // DefaultHTMLPager = sbPager.ToString();
      
        Pager pager = new Pager("/community/", miscParams, Page, Members.Count);
        pager.PageSize = PageSize;
        DefaultHTMLPager = pager.ToString();

        // create the TabContents to return
        TabContents tabContents = new TabContents();

        tabContents.HTML = sbHTMLList.ToString();
      //  tabContents.PagerHTML = sbPager.ToString();
        
        pager.PageSize = PageSize;
        tabContents.PagerHTML =pager.ToString();

        return tabContents;
    }
    [AjaxPro.AjaxMethod]
    public string UnfriendMember(string WebMemberID)
    {
        member = (Member)Session["Member"];
        Member friend = Member.GetMemberViaWebMemberID(WebMemberID);

        Friend.UnFriend(member, friend);

        return WebMemberID;
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        Master.SkinID = "Community";
        base.OnPreInit(e);
    }

    public class AddToFriendsResponse
    {
        public bool Response { get; set; }
        public string WebMemberID { get; set; }
    }

    private string GetRelationShipStatus(int status)
    {
        switch (status)
        {
            case -1:
                return "Not Saying";
            case 1:
                return "Single";
            case 2:
                return "Married";
            case 3:
                return "Divorced";
            case 4:
                return "Dating";
            case 5:
                return "Seeing";
            case 6:
                return "Kinda Have A Thing";
            default:
                return "";
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (Page.Request.Url.AbsolutePath.ToLower().EndsWith(".aspx"))
        {
            HTTPResponse.PermamentlyMoved301(Context, "/community");
        }

        Master.HTMLTitle = PageTitle.GetCommunityTitle();
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";

    }
}

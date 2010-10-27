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

public partial class Feed : System.Web.UI.Page
{
    public string FeedHTML = string.Empty;
    public string VideoLister = string.Empty;
    public string PhotoLister = string.Empty;
    public string ViewLister = string.Empty;
    public string MemberStatus = string.Empty;
    public string FriendRequestLister = string.Empty;
    public string ProximityTagsLister = string.Empty;
    public int NumberOfFriendRequests = 0;
    public int NumberOfProximityTags = 0;
    public List<JSName> JSNameList;
    public string JsNameString = string.Empty;
    public string MemberLocation = string.Empty;
    public string RSSToken = string.Empty;

    public int VideoCount = 0;
    public int PhotoCount = 0;
    public int ViewCount = 0;

    public Member member = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Feed));

        member = (Member)Session["Member"];
        JSNameList = new List<JSName>();


        List<FeedItem> Feed = FeedItem.GetFeed(member.MemberID);

        IEnumerable<FeedItem> SortedFeed = OrderFeed(Feed);

        foreach (var F in SortedFeed)
        {
            FeedHTML += FeedRow(F);
        }

        GenerateProfileVisitorLister(member);

        RSSToken = Server.UrlEncode(RijndaelEncryption.Encrypt(member.Password));

        //MemberProfile memberProfile = member.GetMemberProfileByMemberID();
        //MemberStatus = memberProfile.TagLine.Replace("'", "&#39;");

        MemberStatus = member.MyMemberProfile.TagLine.Replace("'","&#39;");

        if (member.IPLocationID == 0)
        {
            MemberLocation = "not set";
        }
        else
        {
            IPLocation ipLocation = new IPLocation(member.IPLocationID);
            MemberLocation = ipLocation.city;
        }

        GenerateFriendRequestLister();
        GenerateProximityTagsLister();

        JsNameString = JSName.RenderJSArray(JSNameList);
    }

    public string FeedRow(FeedItem feedItem)
    {
        StringBuilder sbRow = new StringBuilder();
        object[] parameters = new object[16];

        parameters[1] = feedItem.Thumbnail;
        parameters[2] = feedItem.Text;
        parameters[3] = TimeDistance.TimeAgo(feedItem.DateTime);
        parameters[4] = feedItem.Title;
        parameters[5] = feedItem.FriendNickname1;
        parameters[6] = feedItem.FriendNickname2;
        parameters[7] = "/users/" + feedItem.FriendNickname1;
        parameters[8] = "/users/" + feedItem.FriendNickname2;
        string Title = (feedItem.Title != null) ? feedItem.Title : string.Empty;

            
        parameters[11] = "javascript:displayMiniVideo(\"" + feedItem.MainWebID + "\",\"" + Server.HtmlEncode(Title.Replace(@"""","&#39;").Replace(@"'","&#39;")) + "\")";
        parameters[9] = feedItem.Url;
        parameters[10] = feedItem.MainWebID;
        parameters[12] = (feedItem.Friend1FullName.Trim() != string.Empty) ? feedItem.Friend1FullName : feedItem.FriendNickname1;
        parameters[13] = (feedItem.Friend2FullName.Trim() != string.Empty) ? feedItem.Friend2FullName : feedItem.FriendNickname2;
        parameters[14] = new Random().Next().ToString();
        parameters[15] = TimeDistance.GetAgeYears(feedItem.DateTime.AddDays(2));

        // add the member to the js array
        //AddJSMemberArray(FriendTagList[i].WebMemberID, FriendTagList[i].FirstName + " " + FriendTagList[i].LastName);
        //AddJSMemberArray(FriendTagList[i].WebMemberID, FriendTagList[i].FirstName + " " + FriendTagList[i].LastName);

        if (feedItem.FeedItemType == FeedItemType.Video)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedvideo'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> has posted a new <a href='{11}'>Video</a>:</p>
						<div class='feedcontent'>
							<p class='vid_thumb right'><a href='{11}'><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							<h4><a href='{11}'>{4}</a></h4>
							<p>{2}</p>
						</div>
					</div>", parameters);
        }
//        else if (feedItem.FeedItemType == FeedItemType.Photo)
//        {
//            sbRow.AppendFormat(@"<div class='feeditem clearfix feedphoto'>
//						<p class='delete'>{3}</p>
//						<p class='head'><strong><a href='{7}'>{12}</a></strong> has posted a new <a href='javascript:pgp(""{10}"");'>Photo Gallery</a>:</p>
//						<div class='feedcontent'>
//							<p class='vid_thumb right'><a href='javascript:pgp(""{10}"");'><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
//							<h4><a href='javascript:pgp(""{10}"");'>{4}</a></h4>
//							<p>{2}</p>
//						</div>
//					</div>", parameters);
//        }
        else if (feedItem.FeedItemType == FeedItemType.Photo)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedphoto'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> has posted a new <a href='{9}'>Photo Gallery</a>:</p>
						<div class='feedcontent'>
							<p class='vid_thumb right'><a href='{9}'><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							<h4><a href='{9}'>{4}</a></h4>
							<p>{2}</p>
						</div>
					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.WallComment)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedcomment'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{13}</a></strong> has written on <a href='{9}'>{12}'s wall</a>:</p>

						<div class='feedcontent'>
							{2}
						</div>
					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Ask)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedaskafriend'>

						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> has <a href='{9}?return=1'>Asked</a>:</p>
						<div class='feedcontent'>
							<h4>Q: <a href='{9}?return=1'>{2}</a></h4>
						</div>

					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.NewFriend)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfriend'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> and <strong><a href='{8}'>{13}</a></strong> are now friends!</p>

					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Blog)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedcomment'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> has posted a new <a href='{9}'>Blog Entry</a>:</p>

						<div class='feedcontent'>
							{2}
						</div>
					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedVideo)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfavourite'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> would like to share a video: <a href='{11}'>{4}</a>:</p>

						<div class='feedcontent'>
                            <p class='vid_thumb right'><a href='{11}'><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							{2}
						</div>
					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedPhoto)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfavourite'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a></strong> would like to share a <a href='{9}'>Photo</a>:</p>

						<div class='feedcontent'>
                            <p class='vid_thumb right'><a href='{9}'><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							{2}
						</div>
					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.StatusText)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix status-dash'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{12}</a> is:</strong> {2}</p>

					</div>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Birthday)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedbirthday'>
						<p class='delete'>Today</p>
						<p class='head'><strong><a href='{7}'>{12}</a> is {2} today!</strong></p>

					</div>", parameters);
        }
//        else if (feedItem.FeedItemType == FeedItemType.Mp3Upload)
//        {
//            sbRow.AppendFormat(@"<div class='feeditem clearfix feedMP3'>
//						<p class='delete'>{3}</p>
//						<p class='head'><strong><a href='{7}'>{12}</a></strong> has uploaded a new MP3: {2} </p>
//                        
//                         <div class='feedcontent' >
//                            <p class='vid_thumb right' style='width:305px' id='mp3{14}'><script>mp3('{9}','mp3{14}','{2}');</script></p>
//						</div>
//
//
//					</div>", parameters);
//        }

        return sbRow.ToString();
    }

    public static IEnumerable<FeedItem> OrderFeed(List<FeedItem> FeedItems)
    {
        // order desc up to 4 days ago
        var SortedFeed =
            from F in FeedItems
            where F.DateTime > DateTime.Now.AddDays(-4)
            orderby F.DateTime descending 
            select F;


        return SortedFeed;
    }


    /// <summary>
    /// Creates a lister with members friends
    /// </summary>
    public void GenerateProfileVisitorLister(Member LoggedInMember)
    {
        List<ContentView> ProfileViews = ContentView.GetMemberProfileViews(LoggedInMember.MemberID);

        StringBuilder sbHTMLList = new StringBuilder();

        for (int i = 0; i < 10; i++)
        {
            if (ProfileViews.Count <= i)
            {
                break;
            }

            // add the member to the js array
            AddJSMemberArray(ProfileViews[i].WebMemberID, ProfileViews[i].FirstName + " " + ProfileViews[i].LastName);

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[6];

            parameters[0] = ProfileViews[i].NickName;
            parameters[1] = ParallelServer.Get("/user/" + ProfileViews[i].PhotoURL) + "/user/" + ProfileViews[i].PhotoURL;
            parameters[2] = ProfileViews[i].FirstName;
            parameters[3] = ProfileViews[i].LastName;
            parameters[4] = TimeDistance.TimeAgo(ProfileViews[i].DTCreated);
            parameters[5] = ProfileViews[i].WebMemberID;


            string HTMLItem = @"<li><a onclick='dmp(""{5}"");return false;'>
                            <img src='{1}' alt='member' width='45' height='45' /></a>
                            <p><span style='notes'><a onclick='dmp(""{5}"");return false;'>{2} {3}</a></span></p>   
                            <p><span style='notes'>{4}</a></span></p>                              
                        </li>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        ViewLister = sbHTMLList.ToString();

        ViewCount = ProfileViews.Count;
    }

    public void GenerateFriendRequestLister()
    {

        List<FriendRequest> friendRequests = FriendRequest.GetAllNewFriendRequestByMemberID(member.MemberID, 0);
        NumberOfFriendRequests = friendRequests.Count;

        StringBuilder sbHTMLList = new StringBuilder();

        if (friendRequests.Count == 0)
        {
            sbHTMLList.Append("<div>No Friend request</div>");
        }
        else
        {
            sbHTMLList.Append("<ul class='friends_list' id='ulFriendRequests'>");
        }

        for (int i = 0; i < 10; i++)
        {
            if (friendRequests.Count <= i)
            {
                break;
            }

            // add the member to the js array
            AddJSMemberArray(friendRequests[i].FriendMember.WebMemberID, friendRequests[i].FriendMember.FirstName+ " " + friendRequests[i].FriendMember.LastName);

            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[8];

            parameters[0] = friendRequests[i].FriendMember.WebMemberID;
            parameters[1] = "http://www.next2friends.com/user" + "/" + friendRequests[i].PhotoURL;
            parameters[2] = friendRequests[i].FriendMember.NickName;
            parameters[3] = friendRequests[i].FriendMember.FirstName;
            parameters[4] = friendRequests[i].FriendMember.LastName;
            parameters[5] = friendRequests[i].FriendMember.ISOCountry;
            parameters[6] = friendRequests[i].WebFriendRequestID;
            parameters[7] = friendRequests[i].WebFriendRequestID;


            string HTMLItem = @" <li id='liFR{7}'>
							<a onclick='dmp(""{0}"");return false;'>
								<img src='{1}' alt='{2}' height='45' width='45'>
							</a>
							<p>
								<a onclick='dmp(""{0}"");return false;'><strong>{3} {4}</strong></a><br />

								<span style=''>{5}</span><br />
								<a href='javascript:setfr(""{7}"",true);' class='acceptFriendRequest'>accept</a>
								&nbsp;&nbsp;
								<a href='javascript:setfr(""{7}"",false);' class='rejectFriendRequest'>ignore</a>
							</p>
						</li>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        if (friendRequests.Count > 0)
        {
            sbHTMLList.Append("</ul>");
        }


        FriendRequestLister = sbHTMLList.ToString();
    }

    public void GenerateProximityTagsLister()
    {
        List<FriendTag> FriendTagList = FriendTag.GetProximityTags(member.MemberID);


        List<FriendTag> FriendTagListMini = new List<FriendTag>();

        try
        {
            for (int i = 0; i < 10; i++)
            {
                if (i <= FriendTagList.Count)
                {
                    FriendTagListMini.Add(FriendTagList[i]);
                }
            }
        }
        catch { }

        FriendTagListMini.Reverse();

        FriendTagList = FriendTagListMini;


        NumberOfProximityTags = FriendTagList.Count;

        StringBuilder sbHTMLList = new StringBuilder();

        if (FriendTagList.Count == 0)
        {
            sbHTMLList.Append("<div>No Friend request</div>");
        }
        else
        {
            sbHTMLList.Append("<ul class='friends_list'>");
        }

        for (int i = 0; i < 10; i++)
        {
            if (FriendTagList.Count <= i)
            {
                break;
            }

            // add the member to the js array
            AddJSMemberArray(FriendTagList[i].WebMemberID, FriendTagList[i].FirstName + " " + FriendTagList[i].LastName);


            StringBuilder sbHTMLItem = new StringBuilder();

            object[] parameters = new object[9];

            parameters[0] = FriendTagList[i].WebMemberID;
            parameters[1] = "http://www.next2friends.com/user/" + FriendTagList[i].ProfilePhotoURL;
            parameters[2] = FriendTagList[i].NickName;
            parameters[3] = FriendTagList[i].FirstName;
            parameters[4] = FriendTagList[i].LastName;
            parameters[5] = FriendTagList[i].CountryName;
            parameters[7] = TimeDistance.TimeAgo(FriendTagList[i].TaggedDT);


            string HTMLItem = @"<li>
							<a onclick='dmp(""{0}"");return false;'>

								<img src='{1}' alt='{3} {4}' height='45' width='45'>
							</a>
							<p>
								<a onclick='dmp(""{0}"");return false;'>{3} {4}</a><br />
								{7}
							</p>
						</li>";

            sbHTMLItem.AppendFormat(HTMLItem, parameters);
            sbHTMLList.Append(sbHTMLItem.ToString());
        }

        if (FriendTagList.Count > 0)
        {
            sbHTMLList.Append("</ul>");
        }

        ProximityTagsLister = sbHTMLList.ToString();
    }


    [AjaxPro.AjaxMethod]
    public void SetFriendStatus(string WebFriendRequestID, bool IsAccepted)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        FriendRequest.SetFriendRequestStatus(member.MemberID, WebFriendRequestID, IsAccepted);
 

    }

    public void AddJSMemberArray(string WebMemberID, string FullName)
    {
        int Count = JSNameList.Count(n => n.WebMemberID == WebMemberID);

        if (Count == 0)
        {
            JSName JsNameItem = new JSName();
            JsNameItem.WebMemberID = WebMemberID;
            JsNameItem.FullName = FullName;
            JSNameList.Add(JsNameItem);
        }
    }


    /// <summary>
    /// Saves the members display status
    /// </summary>
    [AjaxPro.AjaxMethod]
    public string GetMiniProfile(string WebMemberID)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        if (member == null)
        {
            return string.Empty;
        }

        string MiniProfileHTML = PopupHTML.GetMiniProgileHTML(WebMemberID, member);

        return MiniProfileHTML;
    }

    /// <summary>
    /// Saves the members display status
    /// </summary>
    [AjaxPro.AjaxMethod]
    public string GetMiniGallery(string WebPhotoGallery)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        if (member == null)
        {
            return string.Empty;
        }

        string MiniPhotoGalleryeHTML = "";// PopupHTML.GetMiniPhotoGalleryeHTML(WebPhotoGallery);

        return MiniPhotoGalleryeHTML;
    }

    

    [AjaxPro.AjaxMethod]
    public List<MutualFriend> GetMutualFriends(string WebMemberID)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        Member MutualMember = Member.GetMemberViaWebMemberID(WebMemberID);

        if (member == null)
        {
            return new List<MutualFriend>();
        }

        List<Member> MutualMembers = Friend.GetMutualFriends(MutualMember, member);
        List<MutualFriend> MutualFriends = new List<MutualFriend>();

        for (int i = 0; i < MutualFriends.Count; i++)
        {
            MutualFriend MutualFriend = new MutualFriend();
            MutualFriend.Name = MutualMembers[i].FirstName;
            MutualFriend.PhotoURL = MutualMembers[i].DefaultPhoto.FullyQualifiedURL;

            MutualFriends.Add(MutualFriend);
        }

        return MutualFriends;
    }

    /// <summary>
    /// Saves the members display status
    /// </summary>
    [AjaxPro.AjaxMethod]
    public void SaveProfileStatus(string Status)
    {
        member = (Member)Session["Member"];

        if (member != null)
        {
            string EncodedStatusText = Server.HtmlEncode(Status);
            member.MyMemberProfile.TagLine = EncodedStatusText;
            member.MyMemberProfile.Save();
            MemberStatusText.UpdateStatusText(member.MemberID, EncodedStatusText);
        }
    }

    [AjaxPro.AjaxMethod]
    public string RemoveItem()
    {
        member = (Member)Session["Member"];

        return string.Empty;
    }

    [AjaxPro.AjaxMethod]
    public LocationResponse SaveLocation(string SearchText, bool IsID)
    {
        LocationResponse Response = new LocationResponse();

        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }

        if (member != null)
        {
            if (IsID)
            {
                int IPLocationID = Int32.Parse(SearchText);
                IPLocation loc = new IPLocation(IPLocationID);
                member.IPLocationID = loc.IPLocationID;
                Response.LocationText = loc.city;
                member.Save();
                Response.ResponseType = 1;
            }
            else
            {
                List<IPLocation> IPLocationList = IPLocation.SearchLocation(SearchText);
                Response.LocationList = new List<LocationItem>();

                for (int i = 0; i < IPLocationList.Count; i++)
                {
                    LocationItem locationItem = new LocationItem();
                    locationItem.Lcid = IPLocationList[i].IPLocationID.ToString();
                    locationItem.Text = Server.HtmlEncode(LocationResponse.GetFullLocationName(IPLocationList[i]));
                    Response.LocationList.Add(locationItem);
                }

                

                if (Response.LocationList.Count == 0)
                {
                    Response.ResponseType = 0;
                }
                else if (Response.LocationList.Count == 1)
                {
                    Response.ResponseType = 1;
                    Response.LocationText = Server.HtmlEncode(IPLocationList[0].city);
                }
                else if (Response.LocationList.Count > 1)
                {
                    Response.ResponseType = 2;
                }
            }
        }

        return Response;
    }


    
    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        // only allow the member in if logged in
        if (Session["Member"] == null )
        {
            Response.Redirect("/signup/?u=" + Request.Url.AbsoluteUri);
        }

        Master.SkinID = "dashboard";
        base.OnPreInit(e);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetDashboardTitle();
        //Master.MetaDescription = "Live mobile video broadcasting networking";
        //Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

public class JSName
{
    public string WebMemberID { get; set; }
    public string FullName { get; set; }

    public static string RenderJSArray(List<JSName> Members)
    {
        StringBuilder sbJS = new StringBuilder();
        string RenderedJS = string.Empty;
        string[] Parameters = new string[6];

        for (int i = 0; i < Members.Count; i++)
		{
            Parameters[0] = Members[i].WebMemberID;
            Parameters[1] = Members[i].FullName.Replace("'", "&#39;");
            Parameters[2] = ",";

            if(i==Members.Count-1)
            {
                Parameters[2] = string.Empty;
            }

            sbJS.AppendFormat("new Array('{0}','{1}'){2}",Parameters);

            RenderedJS = sbJS.ToString();
		}

        RenderedJS = "var memberArray = new Array(" + RenderedJS + ");";

        return RenderedJS;

    }
}

public class MutualFriend
{
    public string Name { get; set; }
    public string PhotoURL { get; set; }
}


public class LocationResponse
{
    public List<LocationItem> LocationList { get; set; }
    public int ResponseType { get; set; }
    public string LocationText { get; set; }

    public static string GetFullLocationName(IPLocation item)
    {
        string LocationString = string.Empty;
        LocationString = item.city;
        LocationString += (item.region!=string.Empty) ? ", "+item.region : string.Empty;
        LocationString += ", "+item.country;

        return LocationString;
    }
}


public class LocationItem
{
    public string Lcid { get; set; }
    public string Text { get; set; }
}

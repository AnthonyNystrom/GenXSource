using System;
using System.Collections;
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
using System.Text;

using Next2Friends.Data;
using Next2Friends.Misc;

public partial class RSS : System.Web.UI.Page
{
    private Member member;
    private Member friendMember;
    private DataTable dt = new DataTable();
    private string NickName { get; set; }
    private string Password { get; set; }
    private string FriendNick { get; set; }
    public string FeedType { get; set; }
    public string FeedTitle { get; set; }
    public string FeedDescription { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        NickName = Request.Params["nickname"];
        Password = Request.Params["token"];
        FeedType = Request.Params["feed"];
        FriendNick = Request.Params["friend"];

        if (NickName != null && Password != null)
        {
            try
            {
                Password = RijndaelEncryption.Decrypt(Password);
                member = Member.SafeMemberLogin(NickName, Password);
            }
            catch{}
        }

        if (FriendNick != null)
        {
            try
            {
                friendMember = Member.GetMemberViaNickname(FriendNick);
            }
            catch { }            
        }
        
        
        dt.Columns.Add("Title");
        dt.Columns.Add("Link");
        dt.Columns.Add("DTCreated");
        dt.Columns.Add("Duration");
        dt.Columns.Add("Description");
        dt.Columns.Add("ResourceFileThumb");
        dt.Columns.Add("ResourceLink");


        if (FeedType == null || !(FeedType == "dashboard" || FeedType == "video" || FeedType == "uservideo"))
        {
            FeedType = "nofeedtype";
        }

        if (member == null && FeedType == "dashboard")
        {
            FeedType = "dashboarderror";                
        }
        

        switch (FeedType)
        {
            case "nofeedtype":
                FeedTitle = "Next2Friends RSS Error";
                FeedDescription = "Invalid RSS";
                GetInvalidFeedErrorRss();
                break;

            case "dashboarderror":
                FeedTitle = "Next2Friends RSS Error";
                FeedDescription = "Could not logon to your Next2Friends dashboard feed";
                GetDashboardErrorRss();
            break;
                
            case "video":
                FeedTitle = "Next2Friends Videos";
                FeedDescription = "Next2Friends Videos RSS Feed";                    
                GetVideoRss();
                break;

            case "uservideo":
                if (friendMember != null)
                {
                    FeedTitle = "Next2Friends: " + friendMember.NickName + "'s Videos";
                    FeedDescription = "Next2Friends: " + friendMember.NickName + "'s  Videos RSS Feed";
                    GetFriendVideoRss(friendMember);
                }
                break;

            case "photo":
                FeedTitle = "Photo";
                FeedDescription = "Photo RSS Feed";
                GetPhotoRss(member.MemberID);
                break;

            case "blog":
                FeedTitle = "Blog";
                FeedDescription = "Blog RSS Feed";
                GetBlogRss(member.WebMemberID);
                break;

            case "dashboard":
                FeedTitle = member.NickName + "'s Next2Friends Dashboard";
                FeedDescription = member.NickName + "'s Dashboard RSS Feed";
                GetDashboardRss(member.MemberID);
                break;

            case "live":
                FeedTitle = "live";
                FeedDescription = "Live RSS Feed";
                GetLiveRss(member.MemberID);
                break;

            default:
                break;
        }
        

        rptRssFeed.DataSource = dt;
        rptRssFeed.DataBind();
    }

    private void GetVideoRss()
    {
        List<Video> Videos = Video.GetTop100Videos(OrderByType.Featured);

        for (int i = 0; i < Videos.Count; i++)
        {
            if (i >= 20)
            {
                break;
            }

            DataRow row = dt.NewRow();

            row["Title"] = Videos[i].Title;
            row["Link"] = "/video/" + RegexPatterns.FormatStringForURL(Videos[i].Title) + "/" + Videos[i].WebVideoID;
            row["DTCreated"] = Videos[i].DTCreated;

            row["Description"] = "<a href=\"http://www.next2friends.com/video/" + RegexPatterns.FormatStringForURL(Videos[i].Title) + "/" + Videos[i].WebVideoID + "\"/>";
            row["Description"] += row["Description"] + "<img src=\"" + ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) +
                                                         Videos[i].ThumbnailResourceFile.FullyQualifiedURL + "\"/>";
            
            row["ResourceFileThumb"] = ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) +
                                                         Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            dt.Rows.Add(row);
        }
    }

    private void GetFriendVideoRss(Member friendMember)
    {
        PrivacyType privacyType = PrivacyType.Public;

        bool IsFriend = Friend.IsFriend(member.MemberID, friendMember.MemberID);
        if( IsFriend )
        {
            privacyType = PrivacyType.Network;
        }

        List<Video> Videos = Video.GetMemberVideosWithJoinOrdered(friendMember.MemberID, privacyType, "Latest");

        for (int i = 0; i < Videos.Count; i++)
        {
            if (i >= 20)
            {
                break;
            }

            DataRow row = dt.NewRow();

            row["Title"] = Videos[i].Title;
            row["Link"] = "/video/" + RegexPatterns.FormatStringForURL(Videos[i].Title) + "/" + Videos[i].WebVideoID;
            row["DTCreated"] = Videos[i].DTCreated;

            row["Description"] = "<a href=\"http://www.next2friends.com/video/" + RegexPatterns.FormatStringForURL(Videos[i].Title) + "/" + Videos[i].WebVideoID + "\"/>";
            row["Description"] += row["Description"] + "<img src=\"" + ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) +
                                                         Videos[i].ThumbnailResourceFile.FullyQualifiedURL + "\"/>";

            row["ResourceFileThumb"] = ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) +
                                                         Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            dt.Rows.Add(row);
        }
    }

    private void GetDashboardErrorRss()
    {   
        DataRow row = dt.NewRow();

        row["Title"] = "Error Accessing Dashboard RSS";
        row["Link"] = "/dashboard";
        row["DTCreated"] = DateTime.Now;

        row["Description"] = row["Description"] + "Have you recently changed your password. If so you need to navigate to your <a href=\"http://www.next2friends.com/dashboard\">dashboard</a> and subscribe again";

        dt.Rows.Add(row);        
    }

    private void GetInvalidFeedErrorRss()
    {
        DataRow row = dt.NewRow();

        row["Title"] = "Invalid RSS";
        row["Link"] = "/";
        row["DTCreated"] = DateTime.Now;

        row["Description"] = row["Description"] + "You have not specified the correct feed type.";

        dt.Rows.Add(row);
    }

    private void GetLiveRss(int memberID)
    {
        List<Video> Videos = Video.GetMemberVideosWithJoinOrdered(memberID, PrivacyType.Public, "Latest");

        for (int i = 0; i < Videos.Count; i++)
        {
            if (i > 20)
            {
                break;
            }

            DataRow row = dt.NewRow();
            row["Title"] = Videos[i].Title;
            row["Link"] = "http://www.next2friends.com";
            row["DTCreated"] = Videos[i].DTCreated;
            row["Duration"] = Videos[i].Duration;
            row["Description"] = "";// Page.Server.HtmlEncode(Videos[i].Description);
            row["ResourceFileThumb"] = ParallelServer.Get(Videos[i].ThumbnailResourceFile.FullyQualifiedURL) +
                                                         Videos[i].ThumbnailResourceFile.FullyQualifiedURL;
            row["ResourceLink"] = "/video/" + RegexPatterns.FormatStringForURL(Videos[i].Title) + "/" + Videos[i].WebVideoID;
            dt.Rows.Add(row);
        }
    }

    private void GetPhotoRss(int memberID)
    {
        List<Photo> Photos = Photo.GetTop100Photos(TopPhotoType.Featured);

        int ViewItemsCount = Photos.Count > 10 ? 10 : Photos.Count;

        for (int i = 0; i < ViewItemsCount; i++)
        {
            if (i > 20)
            {
                break;
            }

            DataRow row = dt.NewRow();
            row["Title"] = Photos[i].Title;
            row["Link"] = "";
            row["DTCreated"] = Photos[i].CreatedDT;
            row["ResourceFileThumb"] = "";
            row["ResourceLink"] = "http://next2friends.com/user/" + member.NickName + "/pthmb/" + Photos[i].WebPhotoID + ".jpg"; //"view.aspx?p=" + Photos[i].WebPhotoID;


            

            dt.Rows.Add(row);
        }
    }

    private void GetBlogRss(string webMemberID)
    {
        List<BlogEntry> blogEntries = BlogEntry.GetBlogEntryByMemberID(webMemberID);

        int ViewItemsCount = blogEntries.Count > 10 ? 10 : blogEntries.Count;

        for (int i = 0; i < ViewItemsCount; i++)
        {
            DataRow row = dt.NewRow();
            row["Title"] = blogEntries[i].Title;
            row["Link"] = "http://www.next2friends.com";
            row["DTCreated"] = blogEntries[i].DTCreated;
            row["Description"] = Page.Server.HtmlEncode(blogEntries[i].Body);
            dt.Rows.Add(row);
        }
    }

    private void GetDashboardRss(int memberID)
    {

        List<FeedItem> feed = FeedItem.GetFeed(memberID);
        IEnumerable<FeedItem> dashItems = OrderFeed(feed);

        foreach (var fi in dashItems)
        {
            if (fi.FeedItemType == FeedItemType.Birthday)
                continue;

            DataRow row = dt.NewRow();
            row["Title"] = GetInfo(fi)[0];
            row["Link"] = Server.HtmlEncode(fi.Url);
            row["DTCreated"] = fi.DateTime;

            if (fi.FeedItemType == FeedItemType.NewFriend)
            {
                row["Description"] = "<a href=\"http://www.next2friends.com/users/" + fi.FriendNickname1 + "\">" + fi.FriendNickname1 + "'s profile" + "</a>";
                row["Description"] += "<br><a href=\"http://www.next2friends.com/users/" + fi.FriendNickname2 + "\">" + fi.FriendNickname2 + "'s profile" + "</a>";
            }
            else if (fi.FeedItemType == FeedItemType.WallComment ||
                     fi.FeedItemType == FeedItemType.Blog ||
                     fi.FeedItemType == FeedItemType.StatusText)
            {
                row["Description"] = fi.Text;
            }


            else
            {
                row["Description"] = "<a href=\"http://www.next2friends.com" + fi.Url + "\"/>";
                if (fi.Thumbnail != null)
                {
                    row["Description"] = row["Description"] + "<img src=\"" + fi.Thumbnail + "\"/>";
                }
            }
            
            row["ResourceFileThumb"] = fi.Thumbnail;
            dt.Rows.Add(row);
        }

    }

    //method from Feed.aspx.cs
    private string FeedRow(FeedItem feedItem)
    {
        StringBuilder sbRow = new StringBuilder();
        object[] parameters = new object[11];

        parameters[1] = feedItem.Thumbnail;
        parameters[2] = feedItem.Text;
        parameters[3] = TimeDistance.TimeAgo(feedItem.DateTime);
        parameters[4] = feedItem.Title;
        parameters[5] = feedItem.FriendNickname1;
        parameters[6] = feedItem.FriendNickname2;
        parameters[7] = "/users/" + feedItem.FriendNickname1;
        parameters[8] = "/users/" + feedItem.FriendNickname2;
        parameters[9] = feedItem.Url;
        parameters[10] = feedItem.WebPhotoCollectionID;

        if (feedItem.FeedItemType == FeedItemType.Video)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedvideo'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> has posted a new <a href=''>Video</a>:</p>
						<div class='feedcontent'>
							<p class='vid_thumb right'><a href=''><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							<h4><a href='{9}'>{4}</a></h4>
							<p>{2}</p>
						</div>
					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Photo)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedphoto'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> has posted a new <a href=''>Photo Gallery</a>:</p>
						<div class='feedcontent'>
							<p class='vid_thumb right'><a href=''><img src='{1}' style='width:80px;height:57px;' alt='' /></a></p>
							<h4><a href=''>{4}</a></h4>
							<p>{2}</p>
						</div>
					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.WallComment)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedcomment'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> has written on <a href=''>{6}'s wall</a>:</p>

						<div class='feedcontent'>
							{2}
						</div>
					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Ask)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedaskafriend'>

						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> has <a href=''>Asked</a>:</p>
						<div class='feedcontent'>
							<h4>Q: <a href=''>{2}</a></h4>
						</div>

					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.NewFriend)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfriend'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> and <strong><a href='{8}'>{6}</a></strong> are now friends!</p>

					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Blog)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedcomment'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> has posted a new <a href=''>Blog Entry</a>:</p>

						<div class='feedcontent'>
							{2}
						</div>
					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedVideo)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfavourite'>						
						<p class='head'><strong><a href='{7}'>{5}</a></strong> would like to share a video: <a href=''>{4}</a>:</p>
						<div class='feedcontent'>
                            <p class='vid_thumb right'><a href=''><img src='{1}' style='width:80px;height:57px;' alt='{4}' /></a></p>
							{2}
						</div>
					</div><hr/>", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedPhoto)
        {
            sbRow.AppendFormat(@"<div class='feeditem clearfix feedfavourite'>
						<p class='delete'>{3}</p>
						<p class='head'><strong><a href='{7}'>{5}</a></strong> would like to share a <a href=''>Photo</a>:</p>

						<div class='feedcontent'>
                            <p class='vid_thumb right'><a href=''><img src='{1}' style='width:80px;height:57px;' alt='' /></a></p>
							{2}
						</div>
					</div><hr/>", parameters);
        }

        return sbRow.ToString();
    }

    //method from Feed.aspx.cs
    private string[] GetInfo(FeedItem feedItem)
    {
        string[] ret = new string[3];
        //0 - Title
        //1 - Thumbnail
        //2 - 

        StringBuilder sbTitle = new StringBuilder();
        StringBuilder sbThumb = new StringBuilder();
        StringBuilder sbURL = new StringBuilder();

        object[] parameters = new object[14];

        parameters[1] = feedItem.Thumbnail;
        parameters[2] = feedItem.Text;
        parameters[3] = TimeDistance.TimeAgo(feedItem.DateTime);
        parameters[4] = feedItem.Title;
        parameters[5] = feedItem.FriendNickname1;
        parameters[6] = feedItem.FriendNickname2;
        parameters[7] = "/users/" + feedItem.FriendNickname1;
        parameters[8] = "/users/" + feedItem.FriendNickname2;
        //parameters[11] = "javascript:displayMiniVideo(\"" + feedItem.MainWebID + "\",\"" + Server.HtmlEncode(feedItem.Title) + "\")";
        parameters[9] = feedItem.Url;
        parameters[10] = feedItem.WebPhotoCollectionID;
        parameters[12] = (feedItem.Friend1FullName.Trim() != string.Empty) ? feedItem.Friend1FullName : feedItem.FriendNickname1;
        parameters[13] = (feedItem.Friend2FullName.Trim() != string.Empty) ? feedItem.Friend2FullName : feedItem.FriendNickname2;


        if (feedItem.FeedItemType == FeedItemType.Video)
        {
            sbTitle.AppendFormat(@"{5} has posted a new video: {4}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Photo)
        {
            sbTitle.AppendFormat(@"{5} has posted a new Photo Gallery: {4}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.WallComment)
        {
            sbTitle.AppendFormat(@"{5} has written on {6}'s wall", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Ask)
        {
            sbTitle.AppendFormat(@"{5} has asked: {2}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.NewFriend)
        {
            sbTitle.AppendFormat(@"{5} and {6} are now friends", parameters);
            sbThumb.AppendFormat("/", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Blog)
        {
            sbTitle.AppendFormat(@"{5} has posted a new Blog Entry", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedVideo)
        {
            sbTitle.AppendFormat(@"{5} would like to share a video: {4}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.BookmarkedPhoto)
        {
            sbTitle.AppendFormat(@"{5} would like to share a photo {4}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.Mp3Upload)
        {
            sbTitle.AppendFormat(@"{5} has uploaded a new mp3 : {2}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }
        else if (feedItem.FeedItemType == FeedItemType.StatusText)
        {
            sbTitle.AppendFormat(@"{5} is : {2}", parameters);
            sbThumb.AppendFormat("{1}", parameters);
        }

        ret[0] = sbTitle.ToString();
        ret[1] = sbThumb.ToString();

        return ret;
    }

    //method from Feed.aspx.cs
    private static IEnumerable<FeedItem> OrderFeed(List<FeedItem> FeedItems)
    {
        // order desc up to 40 days ago
        var SortedFeed =
            from F in FeedItems
            where F.DateTime > DateTime.Now.AddDays(-40)
            orderby F.DateTime descending
            select F;

        return SortedFeed;
    }
}

using System;
using System.Data;
using System.Configuration;
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

/// <summary>
/// Extracts params from either a url variable or the Content items (Used in URL rewriting)
/// </summary>
public class ExtractPageParams
{
    public ExtractPageParams()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static Member GetMember(Page page, HttpContext context)
    {
        string strWebMemberID = page.Request.Params["m"];
        string strGalleryID = page.Request.Params["g"];
        string strNickname = (context.Items["nickname"] != null) ? context.Items["nickname"].ToString() : null;
        Member ViewingMember = null;

        if (strNickname != null)
        {
            try
            {
                ViewingMember = Member.GetMemberViaNickname(strNickname);
            }
            catch { }
        }
        else if (strWebMemberID != null)
        {
            ViewingMember = Member.GetMembersViaWebMemberIDWithFullJoin(strWebMemberID);
        }
        else if (strGalleryID != null)
        {
            //ViewingMember = Member.GetMemberByGalleryID(strGalleryID);
        }

        return ViewingMember;
    }

    public static BlogEntry GetBlog(Page page, HttpContext context)
    {
        try
        {
            string strBlogEntryID = (context.Items["webblogentryid"] != null) ? context.Items["webblogentryid"].ToString() : page.Request.Params["v"].ToString();
            BlogEntry blog = BlogEntry.GetBlogEntryByWebBlogEntryID(strBlogEntryID);
            return blog;
        }
        catch
        {
            return null;
        }
    }

    public static Video GetVideo(Page page, HttpContext context)
    {
        string strWebVideoIDURL = page.Request.Params["v"];
        string strWebVideoIDItem = (context.Items["webvideoid"] != null) ? context.Items["webvideoid"].ToString() : null;
        Video DefaultVideo = null;

        if (strWebVideoIDURL != null)
        {
            DefaultVideo = Video.GetVideoByWebVideoIDWithJoin(strWebVideoIDURL);
        }
        else if (strWebVideoIDItem != null)
        {
            DefaultVideo = Video.GetVideoByWebVideoIDWithJoin(strWebVideoIDItem);
        }

        return DefaultVideo;
    }

    /// <summary>
    /// Returns the title of the video
    /// </summary>
    /// <param name="page"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetVideoTitle(Page page, HttpContext context)
    {
        string Title = (context.Items["title"] != null) ? context.Items["title"].ToString() : string.Empty;

        return Title;
    }


    public static AskAFriend ExtractAskAFriendFromURL(Page page, HttpContext context)
    {
        string strWebAskAfriendIDURL = page.Request.Params["q"];
        string strWebAskAFriendItem = (context.Items["WebAskID"] != null) ? context.Items["WebAskID"].ToString() : null;
        AskAFriend DefaultAskAFriend = null;

        if (strWebAskAfriendIDURL != null)
        {
            DefaultAskAFriend = AskAFriend.GetAskAFriendByWebAskAFriendID(strWebAskAfriendIDURL);
            //AskAFriend.GetAskAFriendByWebAskAFriendID(strAAFQuestion);
        }
        else if (strWebAskAFriendItem != null)
        {
            //DefaultAskAFriend = AskAFriend.GetAskAFriendByAskAFriendIDWithJoin(strWebAskAFriendItem);
            DefaultAskAFriend = AskAFriend.GetAskAFriendByWebAskAFriendID(strWebAskAFriendItem);
        }

        return DefaultAskAFriend;
    }

}

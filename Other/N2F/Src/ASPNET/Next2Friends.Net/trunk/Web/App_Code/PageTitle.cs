using System;
using System.Text;
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
/// Summary description for PageTitle
/// </summary>
public class PageTitle
{
    private static string Prefix = "Next2Friends Beta - ";

    public PageTitle()
    {
    }


    public static string GetHomePageTitle()
    {
        return Prefix + "Its Now!";
    }

    public static string GetDashboardTitle()
    {
        return Prefix + "dashboard";
    }

    public static string GetVideoPageTitle()
    {
        return Prefix + "videos";
    }

    public static string GetCommunityTitle()
    {
        return Prefix + "community";
    }

    public static string GetAAFTitle(AskAFriend InitialAAF)
    {
        if (InitialAAF == null) return Prefix;
        return Prefix + "ask - " +  RegexPatterns.FormatHTMLTitleAllowQuestionMark( InitialAAF.Question);
    }

    public static string GetPhotoGalleryTitle(PhotoCollection gallery)
    {
        if (gallery == null) return Prefix;
        return Prefix + "photos - " + RegexPatterns.FormatHTMLTitle(gallery.Name);
    }

    public static string GetVideoViewTitle(Video DefaultVideo)
    {
        if (DefaultVideo == null) return string.Empty;
        return Prefix + " video - " + RegexPatterns.FormatHTMLTitle(DefaultVideo.Title);
    }

    public static string GetFriendsTitle()
    {
        return Prefix + "friends";
    }

    public static string GetInboxTitle()
    {
        return Prefix + "inbox";
    }

    public static string GetMyVideoUploadsTitle()
    {
        return Prefix + "my videos";
    }

    public static string GetMyPhotoUploadsTitle()
    {
        return Prefix + "my photos";
    }

    public static string GetMWallTitle(Member member)
    {
        if (member == null) return "wall";
        return Prefix + member.NickName + "s wall";
    }

    public static string GetMFriendsTitle(Member member)
    {
        if (member == null) return "friends";
        return Prefix + member.NickName + "s friends";
    }

    public static string GetMPhotosTitle(Member member)
    {
        if (member == null) return "photos";
        return Prefix + member.NickName + "s photos";
    }

    public static string GetMVideoTitle(Member member)
    {
        if (member == null) return "videos";
        return Prefix + member.NickName + "s videos";
    }

    public static string GetProfileTitle(Member member)
    {
        if (member == null) return "profile";
        return Prefix + member.NickName + "s profile";
    }


    
    public static string GetMBlogTitle()
    {
        // 
        return Prefix + "Blog";
    }




}

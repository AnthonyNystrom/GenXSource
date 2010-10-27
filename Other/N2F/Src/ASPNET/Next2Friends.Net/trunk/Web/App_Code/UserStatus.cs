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

using System.Collections.Generic;
using Next2Friends.Data;
using System.Web.Caching;

/// <summary>
/// Summary description for UserStatus
/// </summary>
public static class UserStatus
{
    public static CacheItemRemovedCallback onRemove=null;

    public const int expiration=20;

    public static OnlineStatus GetUserStatusByMemebrID(string webMemberID)
    {

        if (IsUserOnline(webMemberID))

            return OnlineStatus.Online;

        else

            return OnlineStatus.Offline; 

    }

    public static void AddUser(string webMemberID)
    {
        lock (typeof(UserStatus))
        {
            if (HttpContext.Current.Cache[webMemberID] == null)
            {

                HttpContext.Current.Cache.Add(webMemberID, webMemberID, null, DateTime.Now.AddMinutes(expiration),
                    Cache.NoSlidingExpiration, CacheItemPriority.High, onRemove);
            }
            else
            {

                HttpContext.Current.Cache.Remove(webMemberID);

                HttpContext.Current.Cache.Add(webMemberID, webMemberID, null, DateTime.Now.AddMinutes(expiration),
                   Cache.NoSlidingExpiration, CacheItemPriority.High, onRemove);
            }

            Next2Friends.Data.Trace.Tracer(webMemberID, "us", "OST");

        }
    }
    public static void RemoveUser(string webMemberID)
    {
        lock (typeof(UserStatus))
        {


            if (HttpContext.Current.Cache[webMemberID] != null)

                HttpContext.Current.Cache.Remove(webMemberID);
        }

    }
    public static bool IsUserOnline(string webMemberID)
    {
        lock (typeof(UserStatus))
        {


            if (HttpContext.Current.Cache[webMemberID] == null)

                return false;

            else

                return true;
        }

    }
    public static void SetUserOnline()
    {
        Member member = (Member)HttpContext.Current.Session["Member"];
        
        if (member != null)
        {
            AddUser(member.WebMemberID);
        }
    }

}

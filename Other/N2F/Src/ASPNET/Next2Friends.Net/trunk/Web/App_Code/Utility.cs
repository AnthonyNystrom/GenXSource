using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.Misc;

/// <summary>
/// Summary description for Utility
/// </summary>
public class Utility
{

    // EARTH average radius in MILES
    public static readonly double RADIUS = 3956.545f;

    public static void AddToLoggedIn()
    {
        try
        {
            System.Web.SessionState.HttpSessionState Session = HttpContext.Current.Session;
            HttpRequest Request = HttpContext.Current.Request;

            Member member = (Member)Session["Member"];

            if (member == null)
                return;

            LoggedIn.DeleteLoggedInByMemberID(member.MemberID);

            LoggedIn loggedIn = new LoggedIn();
            loggedIn.WebLoggedInID = Next2Friends.Misc.UniqueID.NewWebID();
            loggedIn.MemberID = member.MemberID;
            loggedIn.DTCreated = DateTime.Now;

            loggedIn.SaveWithCheck();
        }
        catch { }
    }

    //public static double GetDistance(Member member1, Member member2)
    //{
    //   // IPLocation ipLoc1 = IPLocation.GetIPLocationByCountry(member1.ISOCountry);
    //    //IPLocation ipLoc2 = IPLocation.GetIPLocationByCountry(member2.ISOCountry);

    //    double lon2 = (double)ipLoc2.longitude;
    //    double lon1 = (double)ipLoc1.longitude;

    //    double lat2 = (double)ipLoc2.latitude;
    //    double lat1 = (double)ipLoc1.latitude;

    //    double dlon = lon2 - lon1;
    //    double dlat = lat2 - lat1;

    //    double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2),2);
    //    double c = 2 * Math.Asin(Math.Min(1,Math.Sqrt(a)));

    //    return RADIUS * c;
    //}

    public static void RememberMeLogin()
    {
        System.Web.SessionState.HttpSessionState Session = HttpContext.Current.Session;
        HttpRequest Request = HttpContext.Current.Request;

        // If we are already signed in
        if (Session["Member"] != null)
            return;

        HttpCookie aCookie = Request.Cookies["LastActivity"];

        if (aCookie == null)
            return;

        string autoLogin = aCookie.Values["activityHandle"];

        if (autoLogin == "1")
        {

            string login = aCookie.Values["activityDate"];
            string password = aCookie.Values["activityTime"];

            login = RijndaelEncryption.Decrypt(login);
            password = RijndaelEncryption.Decrypt(password);

            Member member = Member.WebMemberLogin(login, password);

            if (member != null)
            {
                Session["Member"] = member;

                OnlineNow now = new OnlineNow();
                now.MemberID = member.MemberID;
                now.DTOnline = DateTime.Now;
                now.Save();

                string PageName = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

                if (PageName == "/")
                {
                    HttpContext.Current.Response.Redirect("/dashboard");
                }
                
            }
        }

        Utility.AddToLoggedIn();

    }

    /// <summary>
    /// Check if the viewing member is the same as the member logged in
    /// </summary>
    /// <param name="ViewingMember"></param>
    /// <param name="member"></param>
    /// <returns></returns>
    public static bool IsMe(Member ViewingMember, Member member)
    {
        bool IsMeValue = false;

        if (ViewingMember != null)
        {
            if (member != null)
            {
                if (member.MemberID == ViewingMember.MemberID)
                {
                    IsMeValue = true;
                }
            }
        }

        return IsMeValue;
    }


    public static void ContentViewed(Member member,int ObjectID,CommentType contentType)
    {
        if (member != null)
        {
            try
            {
                DateTime dtNow = DateTime.Now;

                ContentView contentView = new ContentView();
                contentView.DTCreated = DateTime.Now;
                contentView.MemberID = member.MemberID;
                contentView.ObjectID = ObjectID;
                contentView.ObjectType = (int)contentType;

                contentView.SaveWithCheck();
            }
            catch { }
        }
    }
}

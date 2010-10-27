using System;
using System.Collections.Generic;
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

/// <summary>
/// Summary description for LockDown
/// </summary>
public class LockDown
{
    public static void IsAllowed(Page page)
    {
        
        string CurrentPage = page.Request.CurrentExecutionFilePath.ToLower();

        //page.Response.Redirect("http://www.cnet.com/?" + CurrentPage);
 

        List<string> AllowedList = new List<string>();

        AllowedList.Add("/features");
        AllowedList.Add("/download");
        AllowedList.Add("/signup");
        AllowedList.Add("/login");
        AllowedList.Add("/signupnow");
        AllowedList.Add("/404.aspx");
        AllowedList.Add("/500.aspx");
        AllowedList.Add("/aboutus");
        AllowedList.Add("/managementteam");
        AllowedList.Add("/investors-partners");
        AllowedList.Add("/news");
        AllowedList.Add("/events");
        AllowedList.Add("/interactive-marketing-services");
        AllowedList.Add("/developers");
        AllowedList.Add("/labs");
        AllowedList.Add("/termsofuse");
        AllowedList.Add("/privacypolicy");
        AllowedList.Add("/welcome");
        AllowedList.Add("/forgotpassword");

        int Exists = AllowedList.Count(p => p == CurrentPage);

        bool ForwardToStatic = false;

        if (!CurrentPage.StartsWith("/press/") && Exists==0)
        {
            ForwardToStatic = true;
        }

        if (ForwardToStatic && (CurrentPage == "/index.aspx" || CurrentPage =="/welcome"))
        {
            page.Response.Redirect("/welcome");
        }
        else if (ForwardToStatic && (CurrentPage != "/index.aspx" && CurrentPage != "/welcome"))
        {
            page.Response.Redirect("/signup");
        }
    }

    public LockDown()
    {
        
    }
}

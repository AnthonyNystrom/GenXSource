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

/// <summary>
/// Summary description for HTTPResponse
/// </summary>
public class HTTPResponse
{
    public HTTPResponse()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void PermamentlyMoved301(HttpContext Context, string RedirectTo)
    {
        Context.Response.Status = "301 Moved Permanently";
        Context.Response.StatusCode = 301;
        //Context.Response.AddHeader(RedirectTo);
        Context.Response.Redirect(RedirectTo);
    }

    public static void FileNotFound404(HttpContext Context)
    {
        Context.Response.Status = "404 file not found";
        Context.Response.StatusCode = 404;
        //Context.Response.AddHeader(RedirectTo);
    }
}

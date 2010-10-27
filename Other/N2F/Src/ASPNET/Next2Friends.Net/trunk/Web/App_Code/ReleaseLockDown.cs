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

public class ReleaseLockDown : IHttpModule
{

    public void Init(HttpApplication context)
    {
        context.AuthorizeRequest += new EventHandler(Handle);
    }

    private void Handle(object sender, EventArgs e)
    {
        HttpContext context = ((HttpApplication)sender).Context;

        string[] IPAdress = new string[] { "127.0.0.1", "119.30.70.254", "139.184.222.96" };

        string CurrentIDAddress = context.Request.UserHostAddress;

        var IP =
        from i in IPAdress
        where i == CurrentIDAddress
        select i;

        bool Deny = true;
        string QueryString = context.Request.Url.PathAndQuery;


        if (QueryString.Contains(".js") || QueryString.Contains(".css") || QueryString.Contains(".swf"))
        {
            Deny = false;
        }
        else if (QueryString.Contains("/images/"))
        {
            Deny = false;
        }
        else if (QueryString.Contains(".css"))
        {
            Deny = false;
        }

        if (Deny)
        {
            if (IP.Count() == 0)
            {
                context.Server.Transfer("/DownForMaintainence.htm");
            }
        }
    }

    public void Dispose()
    {
        //Do nothing here
    }

}

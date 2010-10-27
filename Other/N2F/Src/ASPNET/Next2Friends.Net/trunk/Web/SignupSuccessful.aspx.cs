using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class SignupSuccessful : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SignedUp"] != null)
        {
            Response.Cookies["autoLoadChatMode"].Value = "0";
            Response.Cookies["autoLoadChatMode"].Expires = System.DateTime.Now.AddMonths(30);
            Response.Cookies["autoLoadChat"].Value = "true";
            Response.Cookies["autoLoadChat"].Expires = System.DateTime.Now.AddMonths(30);
        }
    }
}

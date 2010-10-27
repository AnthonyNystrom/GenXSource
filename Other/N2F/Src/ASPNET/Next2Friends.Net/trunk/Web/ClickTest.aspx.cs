using System;
using System.Collections;
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

using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

public partial class ClickTest : System.Web.UI.Page
{
    public string ClickMeURL = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        Banner banner = Banner.GetNextBanner(BannerType.PageBanner,"http://localhost");

        ClickMeURL = banner.ToHTML();
    }
}

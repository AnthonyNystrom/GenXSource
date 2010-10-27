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

public partial class MD : System.Web.UI.Page
{
    public MobilePhone MyMobilePhone;
    public string UserAgent;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        UserAgent = Request.ServerVariables["HTTP_USER_AGENT"];
        MyMobilePhone = MobilePhone.GetMobilePhoneFromUserAgent(UserAgent);

    }
}

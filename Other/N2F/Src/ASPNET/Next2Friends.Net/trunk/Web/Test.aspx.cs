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

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Form["ImportPosted"] != null)
        {
            foreach (string s in Request.Form)
            {
                int i = 10;
                //t1.Text = t1.Text + s.ToString() + "=" + Request.Form[s] + "<br/>";
            }
        }
    }
}

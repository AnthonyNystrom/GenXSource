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

public partial class NotAvailable : System.Web.UI.Page
{
    public string Message = string.Empty;
    public string Referer = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strResourceType = Request.Params["rt"];

        if (strResourceType != null)
        {
            if (strResourceType == "v")
            {
                Message = @"Im sorry but the Video you were looking for is no longer available.";
            }
            else if (strResourceType == "p")
            {
                Message = @"Im sorry but the Photo you were looking for is no longer available.";
            }
            else if (strResourceType == "m")
            {
                Message = @"Im sorry but the Member you were looking for is no longer available.";
            }
            else
            {
                Message = @"Im sorry but the item you were looking for is no longer available.";
            }
        }

        Referer = Request.ServerVariables["HTTP_REFERER"];
    }
}

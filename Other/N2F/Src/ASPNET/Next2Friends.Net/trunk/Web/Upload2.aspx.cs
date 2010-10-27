using System;
using System.Collections.Generic;
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

public partial class Upload2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> photos = (List<string>)Session["UploadedPhotos"];

        int i = 10;
    }
}

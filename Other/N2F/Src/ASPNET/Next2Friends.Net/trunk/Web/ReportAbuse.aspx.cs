using System;
using System.Text;
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
using Next2Friends.Data;

public partial class ReportAbusePage : System.Web.UI.Page
{
    public bool AbuseCompleted = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string strAbuseResourceFileID = Request.Params["r"];
            string strAbuseURL = Request.Params["url"];
            Member member = (Member)Session["Member"];

            if (strAbuseResourceFileID != null && member!=null)
            {
                if(strAbuseURL==null)
                {
                    strAbuseURL = string.Empty;
                }

                Abuse abuse = new Abuse();
                abuse.MemberID = member.MemberID;
                abuse.ResourceFileID = strAbuseResourceFileID;
                abuse.DTCreated = DateTime.Now;
                abuse.URL = strAbuseURL;
                abuse.Save();
                AbuseCompleted = true;
            }
        }
    }
}

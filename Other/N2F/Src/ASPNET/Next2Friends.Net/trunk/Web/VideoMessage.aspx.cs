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
using Next2Friends.Data;

public partial class VideoMessagePage : System.Web.UI.Page
{
    public string WebVideoMessageID = string.Empty;
    public string WebEmailMessageID = string.Empty;
    public EmailMessage emailMessage;
    public bool MessageLoaded = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strMessageID = Request.Params["vm"];

        //Message message = Message
        if (strMessageID != null)
        {
            WebEmailMessageID = strMessageID;

            emailMessage = EmailMessage.GetEmailMessageByWebEmailMessageIDWithJoin(WebEmailMessageID);

            if (emailMessage != null)
            {
                MessageLoaded = true;
            }
        }  
    }
}

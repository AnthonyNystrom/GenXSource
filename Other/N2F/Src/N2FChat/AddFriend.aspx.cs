using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Next2Friends.ChatClient;

public partial class AddFriend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitialiseServerInboxList();
    }

    public bool InitialiseServerInboxList()
    {
        Logic.GetInboxListFromServer(ASP.global_asax.ChatInboxList);
        return true;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        string WebMemberID = string.Empty;
        if (Session["WebMemberID"] == null)
        {
            return;
        }

        WebMemberID = (string)Session["WebMemberID"];

        string[] emails = GetEmails();

        foreach (string email in emails)
        {
            Logic.AddFriend(WebMemberID, email);
        }
    }

    private string[] GetEmails()
    {
        string[] ret = new string[1];
        ret[0] = txtEmailAddresses.Text;

        return ret;
    }
}

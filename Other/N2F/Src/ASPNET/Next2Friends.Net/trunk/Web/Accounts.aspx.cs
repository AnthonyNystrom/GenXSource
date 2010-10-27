using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Next2Friends.Data;

public partial class Accounts : System.Web.UI.Page
{
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MemberAccount Account = MemberAccount.GetMemberAccountByMemberID(member.MemberID);

            if (Account == null)
            {
                Account = new MemberAccount();
                Account.MemberID = member.MemberID;
                Account.Save();
            }

            txtTwitterUserName.Text = Account.Username;
            txtTwitterPassword.Text = Account.Password;
        }

        txtTwitterUserName.Attributes.Add("value", txtTwitterUserName.Text);
    }

    /// <summary>
    /// save the member account
    /// </summary>
    protected void btnSave_click(object sender, EventArgs e)
    {
        MemberAccount Account = MemberAccount.GetMemberAccountByMemberID(member.MemberID);

        Account.Username = txtTwitterUserName.Text;
        Account.Password = txtTwitterPassword.Text;

        Account.Save();
    }

    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("/signup");
        }

        Master.SkinID = "";
        base.OnPreInit(e);
    }
}

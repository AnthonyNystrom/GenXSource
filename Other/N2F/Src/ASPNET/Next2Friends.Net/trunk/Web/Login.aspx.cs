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
using Next2Friends.Data;
//using AdamTibi.Web.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Member"] != null)
        {
            Response.Redirect("index.aspx");
        }

        Form.DefaultButton = "";

        txtEmailLogin.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false}");
        txtPasswordLogin.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false}");

          string strLoginError = Request.Params["err"];

          if (strLoginError != null)
          {
              ShowLoginError();
          }
    }

    public void ShowLoginError()
    {
        errLogin.Text = "<p class='error_alert'>Invalid email or password</p>";
    }

    /// <summary>
    /// The user has clicked the login button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Member member = Member.WebMemberLogin(txtEmailLogin.Text, txtPasswordLogin.Text);

        if (txtEmailLogin.Text == string.Empty || txtPasswordLogin.Text == string.Empty || member == null)
        {
            ShowLoginError();
        }
        else
        {
            Session["Member"] = member;

            // log the login date time
            OnlineNow now = new OnlineNow();
            now.MemberID = member.MemberID;
            now.DTOnline = DateTime.Now;
            now.Save();

            Utility.AddToLoggedIn();

            if (chbRememberMe.Checked)
            {
                WriteRememberMeCookie(txtEmailLogin.Text, txtPasswordLogin.Text);
            }

            //Response.Cookies.Add(aCookie);
            errLogin.Text = string.Empty;
            Response.Redirect("feed.aspx");
        }
    }

    public void WriteRememberMeCookie(string Email, string Password)
    {
        try
        {
            HttpCookie aCookie = new HttpCookie("LastActivity");
            aCookie.Values["activityHandle"] = "1";
            aCookie.Values["activityDate"] = RijndaelEncryption.Encrypt(Email);
            aCookie.Values["activityTime"] = RijndaelEncryption.Encrypt(Password);
            aCookie.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(aCookie);
        }
        catch { }
    }
}

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

public partial class ForgottenPasswordPage : System.Web.UI.Page
{
    public bool PasswordSent = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtEmail.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSend.UniqueID + "','');return false}");
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {

        Member ForgetfulMember = Member.GetMemberByEmail(txtEmail.Text);

        // if the address is not available then the user is valid
        if (ForgetfulMember!= null)
        {
            ForgottenPassword forgottenPassword = new ForgottenPassword();
            forgottenPassword.EmailAddress = txtEmail.Text;
            forgottenPassword.IPAddress = HttpContext.Current.Request.UserHostAddress;
            forgottenPassword.DTCreated = DateTime.Now;
            forgottenPassword.MemberID = ForgetfulMember.MemberID;
            forgottenPassword.Save();

            PasswordSent = true;
        }
        else
        {
            // that email address does not exist
            libMessage.Text = "<p class='error_alert'>Sorry, we do not have an account with that email</p>";

        }

    }
}

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.Misc;

/// <summary>
/// Summary description for Settings
/// </summary>
public partial class SettingsPage : Page
{
    public Member member;
    public MemberSettings settings;
    public string PhotoURL = "";
    public string PasswordErrorMessage = "";
    public string EmailErrorMessage = "";
    public string EmailMessageCss = "";
    public string PasswordMessageCss = "";
    public bool IsSignup = false;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            settings = MemberSettings.GetMemberSettingsByMemberID(member.MemberID);

            chbNewMessage.Checked = settings.NotifyOnNewMessage;
            chbNewAAFComment.Checked = settings.NotifyOnAAFComment;
            chbNewFriendRequest.Checked = settings.NotifyOnFriendRequest;
            chbSubscriberEvent.Checked = settings.NotifyOnSubscriberEvent;
            chbNewsLetter.Checked = settings.NotifyOnNewsLetter;
            chbNewProfileComment.Checked = settings.NotifyNewProfileComment;
            chbNotifyNewPhotoComment.Checked = settings.NotifyNewPhotoComment;
            chbNotifyNewVideoComment.Checked = settings.NotifyNewVideoComment;
            chbNotifyNewVideo.Checked = settings.NotifyOnNewVideo;
            chbNotifyNewBlog.Checked = settings.NotifyOnNewBlog;

            txtEmail.Text = member.Email;
        }

        string strSignup = Request.Params["signup"];

        if (strSignup != null)
        {
            btnSave.Text = "Next";
            IsSignup = true;
        }

        member.DefaultPhoto = new ResourceFile(member.ProfilePhotoResourceFileID);
        PhotoURL = member.DefaultPhoto.FullyQualifiedURL;
        Session["Member"] = member;
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Response.Redirect("Login.aspx");
        }
 
        Master.SkinID = "";
        base.OnPreInit(e);
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        settings = MemberSettings.GetMemberSettingsByMemberID(member.MemberID);

        settings.NotifyOnNewMessage = chbNewMessage.Checked;
        settings.NotifyOnAAFComment = chbNewAAFComment.Checked;
        settings.NotifyOnFriendRequest = chbNewFriendRequest.Checked;
        settings.NotifyOnSubscriberEvent = chbSubscriberEvent.Checked;
        settings.NotifyOnNewsLetter = chbNewsLetter.Checked;
        settings.NotifyNewProfileComment = chbNewProfileComment.Checked;
        settings.NotifyNewPhotoComment = chbNotifyNewPhotoComment.Checked;
        settings.NotifyNewVideoComment = chbNotifyNewVideoComment.Checked;

        settings.NotifyOnNewVideo = chbNotifyNewVideo.Checked;
        settings.NotifyOnNewBlog = chbNotifyNewBlog.Checked;
        

        if (RegexPatterns.TestEmailRegex(txtEmail.Text))
        {
            if (member.Email.ToLower().Trim() != txtEmail.Text.ToLower().Trim())
            {
                if (Member.IsEmailAddressAvailable(txtEmail.Text))
                {
                    member.Email = txtEmail.Text;
                    PasswordErrorMessage = string.Empty;
                    txtEmail.CssClass = "form_txt";
                    EmailErrorMessage = "Email address changed";
                    EmailMessageCss = "floatedOK";
                }
                else
                {
                    EmailMessageCss = "floatedWarning";
                    txtEmail.CssClass = "form_txt formerror";
                    EmailErrorMessage = "Email address already in use";
                }
            }
            else
            {
                PasswordErrorMessage = string.Empty;
                txtEmail.CssClass = "form_txt";
                EmailErrorMessage = "";
                EmailMessageCss = "floatedOK";
            }
            
        }
        else
        {
            txtEmail.CssClass = "form_txt formerror";
            EmailErrorMessage = "Invalid Email address";
            EmailMessageCss = "floatedWarning";
        }

        txtNewPassword.CssClass = "form_txt";
        txtConfirmPassword.CssClass = "form_txt";
        txtOldPassword.CssClass = "form_txt";

        if (txtOldPassword.Text != string.Empty || txtNewPassword.Text != string.Empty || txtConfirmPassword.Text != string.Empty)
        {
            bool PasswordsMatch = false;
            bool PasswordsMeetRequirement = false;
            bool PasswordCorrect = false;

            if (txtOldPassword.Text == member.Password)
            {
                PasswordCorrect = true;
            }

            if (txtNewPassword.Text == txtConfirmPassword.Text)
            {
                PasswordsMatch = true;

                if (RegexPatterns.TestPassword(txtNewPassword.Text))
                {
                    member.Password = txtNewPassword.Text;
                    PasswordsMeetRequirement = true;
                }
            }

            PasswordErrorMessage = string.Empty;

            if (!PasswordCorrect)
            {
                PasswordErrorMessage = "Old password is incorrect";
                txtOldPassword.CssClass = "form_txt formerror";
                PasswordMessageCss = "floatedWarning";
            }
            else if (!PasswordsMatch)
            {
                PasswordErrorMessage = "Passwords dont match";
                txtNewPassword.CssClass = "form_txt formerror";
                txtConfirmPassword.CssClass = "form_txt formerror";
                PasswordMessageCss = "floatedWarning";
            }
            else if (!PasswordsMeetRequirement)
            {
                PasswordErrorMessage = "Password must be 7 characters or more";
                txtNewPassword.CssClass = "form_txt formerror";
                txtConfirmPassword.CssClass = "form_txt formerror";
                PasswordMessageCss = "floatedWarning";
            }
            else
            {
                PasswordErrorMessage = "Password changed";
                PasswordMessageCss = "floatedOK";
            }
        }

        settings.Save();
        member.Save();

        string strSignup= Request.Params["signup"];

        if (strSignup != null)
        {
            Response.Redirect("/invite/?signup=true");
        }

    }
}

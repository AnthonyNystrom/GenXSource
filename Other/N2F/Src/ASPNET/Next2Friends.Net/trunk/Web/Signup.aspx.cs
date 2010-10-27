using System;
using System.IO;

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
using System.Web.Services;
using System.Xml.Linq;
using AjaxPro;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Data;
using Next2Friends.Misc;
 
public partial class Signup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            txtReferralEmail.Text = Referral.GetEmailReferrer(this.Context);

            string strCameFrom = Request.Params["u"];

            if (strCameFrom != null)
            {
                ViewState["CameFromURL"] = strCameFrom;
            }

            drpCopuntries.Items.Insert(0, new ListItem("Select Country", ""));

             string strLoginError = Request.Params["err"];

          if (strLoginError != null)
          {
              errLogin.Text = "<p class='error_alert'>Invalid email or password</p>";
          }

          drpYearFounded.Items.Add(new ListItem("----", "----"));

          for (int i = 2008; i > 1900; i--)
          {
              drpYearFounded.Items.Add(new ListItem(i.ToString(),i.ToString()));
          }
 
        }

        AjaxPro.Utility.RegisterTypeForAjax(typeof(Signup));

        txtEmailLogin.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false;}");
        txtPasswordLogin.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false;}");

        //txtEmail.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) {__doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtFirstName.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtLastName.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtNickName.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) {__doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtPassword1.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtPassword2.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        drpCopuntries.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtZipPostcode.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        rbMale.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        rbFemale.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtZipPostcode.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        drpDay.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        drpMonth.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        drpYear.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        txtCaptcha.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
        chbAgree.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSignup.UniqueID + "','');return false;}");
    }

    [AjaxMethod]
    public bool CheckAvailability(string Nickname)
    {
        bool SatifiesCharacterRestrictions = RegexPatterns.TestNickname(Nickname);

        if (SatifiesCharacterRestrictions)
        {
            return Member.IsNicknameAvailable(Nickname);
        }
        else
        {
            return false;
        }
    }

    protected void btnSignup_Click(object sender, EventArgs e)
    {
        ResetLoginErrorMessages();

        bool SignUpOkay = true;
        Member NewMember = new Member();

        if (txtEmail.Text == string.Empty)
        {
            SignUpOkay = false;
            txtEmail.CssClass = "form_txt formerror";
            errTxtEmail.Text = "<span class='formerror_msg'>Blank field</span>";
        }
        else if (!RegexPatterns.TestEmailRegex(txtEmail.Text))
        {
            SignUpOkay = false;
            txtEmail.CssClass = "form_txt formerror";
            errTxtEmail.Text = "<span class='formerror_msg'>Invalid field</span>";
        }
        else if (!Member.IsEmailAddressAvailable(txtEmail.Text))
        {
            SignUpOkay = false;
            txtEmail.CssClass = "form_txt formerror";
            errTxtEmail.Text = "<span class='formerror_msg'>Address is already registered</span>";
        }
        else
        {
            txtEmail.CssClass = "form_txt";
            errTxtEmail.Text = string.Empty;
        }



        if (txtFirstName.Text == string.Empty)
        {
            SignUpOkay = false;
            txtFirstName.CssClass = "form_txt formerror";
            errTxtFirstName.Text = "<span class='formerror_msg'>Blank field</span>";
        }
        else
        {
            txtFirstName.CssClass = "form_txt";
            errTxtFirstName.Text = string.Empty;
        }

        if (txtLastName.Text == string.Empty)
        {
            SignUpOkay = false;
            txtLastName.CssClass = "form_txt formerror";
            errTxtLastName.Text = "<span class='formerror_msg'>Blank field</span>";
        }
        else
        {
            txtLastName.CssClass = "form_txt";
            errTxtLastName.Text = string.Empty;
        }


        if (!RegexPatterns.TestNickname(txtNickName.Text))
        {              
            SignUpOkay = false;
            txtNickName.CssClass = "form_txt formerror";
            errTxtNickName.Text = "<span id='spanErrNickName' class='formerror_msg'>Incorrect format</span>";
        }
        else if (!Member.IsNicknameAvailable(txtNickName.Text))
        {
            SignUpOkay = false;
            txtFirstName.CssClass = "form_txt formerror";
            errTxtNickName.Text = "<span id='spanErrNickName' class='formerror_msg'>Unavailable</span>";
        }
        else
        {
            txtNickName.CssClass = "form_txt";
            errTxtNickName.Text = string.Empty;
        }

        if (txtPassword1.Text == string.Empty)
        {
            SignUpOkay = false;
            txtPassword1.CssClass = "form_txt formerror";
            txtPassword2.CssClass = "form_txt formerror";
            errTxtPassword1.Text = "<span id='spanErrPassword' class='formerror_msg'>Blank field</span>";
        }
        else if (txtPassword1.Text != txtPassword2.Text)
        {
            SignUpOkay = false;
            txtPassword1.CssClass = "form_txt formerror";
            txtPassword2.CssClass = "form_txt formerror";
            errTxtPassword1.Text = "<span  id='spanErrPassword' class='formerror_msg'>Password do no match</span>";
        }
        else
        {
            txtPassword1.CssClass = "form_txt";
            txtPassword2.CssClass = "form_txt";
            errTxtPassword1.Text = string.Empty;
        }

        if (drpCopuntries.SelectedIndex == 0)
        {
            SignUpOkay = false;
            drpCopuntries.CssClass = "form_txt formerror";
            errDrpCountries.Text = "<span class='formerror_msg'>Blank field</span>";
        }
        else
        {
            drpCopuntries.CssClass = "form_txt";
            errDrpCountries.Text = string.Empty;
        }

        if (txtZipPostcode.Text == string.Empty)
        {
            //SignUpOkay = false;
            //txtZipPostcode.CssClass = "form_txt formerror";
            //errTxtZipPostcode.Text = "<span class='formerror_msg'>Blank zip/postcode</span>";
        }
        else
        {
            txtZipPostcode.CssClass = "form_txt";
            errTxtZipPostcode.Text = string.Empty;
        }

        Gender MemberGender = Gender.NotSet;

        if (rbPersonal.Checked)
        {
            if (rbMale.Checked == false && rbFemale.Checked == false)
            {
                
                SignUpOkay = false;
                rbMale.CssClass = "form_txt formerror";
                rbFemale.CssClass = "form_txt formerror";
                errRblGender.Text = "<span class='formerror_msg'>Blank field</span>";
            }
            else
            {
                MemberGender = rbFemale.Checked ? Gender.Female : Gender.Male;
                rbMale.CssClass = "";
                rbFemale.CssClass = "";
                errRblGender.Text = string.Empty;
            }
        }

        // set to a date to avoid a compile error
        DateTime DOB = DateTime.Now;

        if (rbPersonal.Checked)
        {
            try
            {
                DOB = new DateTime(Int32.Parse(drpYear.SelectedValue),
                                    Int32.Parse(drpMonth.SelectedValue),
                                    Int32.Parse(drpDay.SelectedValue));

                drpDay.CssClass = "form_txt";
                drpMonth.CssClass = "form_txt";
                drpYear.CssClass = "form_txt";
                errDOB.Text = string.Empty;
            }
            catch
            {
                SignUpOkay = false;
                drpDay.CssClass = "form_txt formerror";
                drpMonth.CssClass = "form_txt formerror";
                drpYear.CssClass = "form_txt formerror";
                errDOB.Text = "<span class='formerror_msg'>Invalid date</span>";
            }
        }

        if (rbBusiness.Checked)
        {

            if (txtCompanyName.Text == string.Empty)
            {

                SignUpOkay = false;
                errTxtCompanyName.Text = "<span class='formerror_msg'>Blank field</span><br/>";
                txtCompanyName.CssClass = "form_txt formerror";
            }
            else
            {
                errTxtCompanyName.Text = string.Empty;
                txtCompanyName.CssClass = "form_txt";
            }

            if (drpIndustrySector.SelectedIndex == 0)
            {

                SignUpOkay = false;
                errTxtIndustrySector.Text = "<span class='formerror_msg'>Blank field</span><br/>";
                drpIndustrySector.CssClass = "form_txt formerror";
            }
            else
            {
                errTxtIndustrySector.Text = string.Empty;
                drpIndustrySector.CssClass = "form_txt";
            }

            if (drpCompanySize.SelectedIndex == 0)
            {

                SignUpOkay = false;
                errTxtNumberOfEmployees.Text = "<span class='formerror_msg'>Blank field</span><br/>";
                drpCompanySize.CssClass = "form_txt formerror";
            }
            else
            {
                errTxtNumberOfEmployees.Text = string.Empty;
                drpCompanySize.CssClass = "form_txt";
            }

            if (drpYearFounded.SelectedIndex == 0)
            {

                SignUpOkay = false;
                errTxtYearFounded.Text = "<span class='formerror_msg'>Blank field</span><br/>";
                drpYearFounded.CssClass = "form_txt formerror";
            }
            else
            {
                errTxtYearFounded.Text = string.Empty;
                drpYearFounded.CssClass = "form_txt";
            }
        }

        string CaptchaAttempt = txtCaptcha.Text.Trim().ToUpper();

        if (SignupCaptcha.Validate(CaptchaAttempt))
        {
           // successful
            errCaptcha.Text = string.Empty;
            txtCaptcha.Text = string.Empty;

        }
        else
        {
            errCaptcha.Text = "<span class='formerror_msg'>Incorrect, please try again</span><br/>";
            SignUpOkay = false;
            txtCaptcha.Text = string.Empty;
        }


        if (chbAgree.Checked == false)
        {
            SignUpOkay = false;
            errChbAgree.Text = "<span class='formerror_msg'>Agree to terms and conditions</span><br/>";
        }
        else
        {
            errChbAgree.Text = string.Empty;
        }
                

        if (SignUpOkay)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            DbTransaction Transaction = null;

            try
            {
                conn.Open();
                Transaction = conn.BeginTransaction();
              
                NewMember.Email = txtEmail.Text;
                NewMember.NickName = txtNickName.Text;

                if (rbPersonal.Checked)
                {
                    NewMember.FirstName = txtFirstName.Text;
                    NewMember.LastName = txtLastName.Text;
                }
                else if (rbBusiness.Checked)
                {
                    // set the company name as the first name for display purposes
                    NewMember.FirstName = txtCompanyName.Text;
                }


                NewMember.AccountType = (rbPersonal.Checked) ? 0 : 1;
                NewMember.Password = txtPassword1.Text;
                NewMember.ISOCountry = drpCopuntries.SelectedValue;
                NewMember.Gender = (int)MemberGender;
                NewMember.ZipPostcode = txtZipPostcode.Text;
                NewMember.DOB = DOB;
                NewMember.ProfilePhotoResourceFileID = 1;
                NewMember.WebMemberID = Next2Friends.Misc.UniqueID.NewWebID();
                NewMember.CreatedDT = DateTime.Now;
                NewMember.Save(db);

                // Ceate the business Account
                if (rbBusiness.Checked)
                {
                    Business business = new Business();
                    business.MemberID = NewMember.MemberID;
                    business.CompanyName = txtCompanyName.Text;
                    business.IndustrySector = drpIndustrySector.SelectedValue;
                    business.YearFounded = Int32.Parse(drpYearFounded.SelectedValue);
                    business.CompanySize = drpCompanySize.Text;
                    business.IndustrySector = drpIndustrySector.SelectedValue;
                    business.Save(db);

                    IMSPlan imsPlan = new IMSPlan();
                    imsPlan.BusinessID = business.BusinessID;
                    imsPlan.Save(db);
                }


                //ResourceFile.CreateUserDirectories(NewMember);
                TEMPCreateUserDirectories(NewMember);

                PhotoCollection DefaultGallery = new PhotoCollection();
                DefaultGallery.WebPhotoCollectionID = Next2Friends.Misc.UniqueID.NewWebID();
                DefaultGallery.MemberID = NewMember.MemberID;
                DefaultGallery.DTCreated = DateTime.Now;
                DefaultGallery.Name = NewMember.NickName + "'s Gallery";
                DefaultGallery.Description = "My First Gallery!";
                DefaultGallery.Save(db);

                string StatusText = "New to next2Friends!";

                // create a new member profile for the meber
                Next2Friends.Data.MemberProfile profile = new Next2Friends.Data.MemberProfile();
                profile.MemberID = NewMember.MemberID;
                profile.HomeTown = txtCity.Text;
                profile.DTLastUpdated = DateTime.Now;
                profile.DefaultPhotoCollectionID = DefaultGallery.PhotoCollectionID;
                profile.TagLine = StatusText;
                profile.Save(db);

                MemberStatusText.UpdateStatusText(NewMember.MemberID, StatusText);

                Message message = new Message();
                message.Body = "Welcome to Next2Friends";
                message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
                message.MemberIDFrom = 31;
                message.MemberIDTo = NewMember.MemberID;
                message.DTCreated = DateTime.Now;
                message.Save(db);
                message.InReplyToID = message.MessageID;
                message.Save(db);

                // create the default settings for the member
                MemberSettings settings = new MemberSettings();

                settings.NotifyNewPhotoComment = true;
                settings.NotifyNewProfileComment = true;
                settings.NotifyNewVideoComment = true;
                settings.NotifyOnAAFComment = true;
                settings.NotifyOnFriendRequest = true;
                settings.NotifyOnNewMessage = true;
                settings.NotifyOnNewsLetter = true;
                settings.NotifyOnSubscriberEvent = true;

                settings.MemberID = NewMember.MemberID;
                settings.Save(db);

                MatchProfile matchProfile = new MatchProfile();
                matchProfile.MemberID = NewMember.MemberID;
                matchProfile.Save(db);

                Device Device = new Device();
                Device.MemberID = NewMember.MemberID;
                Device.PrivateEncryptionKey = Next2Friends.Misc.UniqueID.NewEncryptionKey();
                Device.CreatedDT = DateTime.Now;
                Device.DeviceTagID = Guid.NewGuid().ToString();
                Device.Save(db);

                OnlineNow now = new OnlineNow();
                now.MemberID = NewMember.MemberID;
                now.DTOnline = DateTime.Now;
                now.Save(db);

                Session["Member"] = NewMember;                
                Transaction.Commit();

                Utility.AddToLoggedIn();

                EmailReferral emailReferral = new EmailReferral();
                emailReferral.Email = txtReferralEmail.Text;
                emailReferral.NewMemberID = NewMember.MemberID;
                emailReferral.DTCreated = DateTime.Now;
                emailReferral.Save();

                // this denotes that the user landed on the site from a referral
                int ReferralContactID = Referral.ProcessSignupFromReferral(this.Context, NewMember.MemberID);

                // search the contact import table for any users that have files a friend request
                FriendRequest.CreateFriendRequestsFromImport(NewMember, ReferralContactID);

                try
                {
                    // Add Lawrence as Auto Friend
                    Friend.AddFriend(1, NewMember.MemberID);
                    // Add Anthony as Auto Friend
                    Friend.AddFriend(3, NewMember.MemberID);
                    // Add Hans as Auto Friend
                    Friend.AddFriend(24, NewMember.MemberID);
                    // Add Becca as Auto Friend
                    Friend.AddFriend(30, NewMember.MemberID);

                    Utility.ContentViewed(new Member(1), NewMember.MemberID, CommentType.Member);
                    Utility.ContentViewed(new Member(3), NewMember.MemberID, CommentType.Member);
                    Utility.ContentViewed(new Member(24), NewMember.MemberID, CommentType.Member);
                    Utility.ContentViewed(new Member(30), NewMember.MemberID, CommentType.Member);
                }
                catch { }                
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();

                Session["SignedUp"] = true;               

                // send the member to the second page of signup
                Response.Redirect("/settings/?signup=true");
            }
        }
    }

    public static void TEMPCreateUserDirectories(Member member)
    {
        string root = OSRegistry.GetDiskUserDirectory() + member.NickName;

        Directory.CreateDirectory(root);
        Directory.CreateDirectory(root + @"\plrge");
        Directory.CreateDirectory(root + @"\pmed");
        Directory.CreateDirectory(root + @"\pthmb");

        Directory.CreateDirectory(root + @"\video");
        Directory.CreateDirectory(root + @"\vthmb");

        Directory.CreateDirectory(root + @"\aaflrge");
        Directory.CreateDirectory(root + @"\aafthmb");

        Directory.CreateDirectory(root + @"\nslrge");
        Directory.CreateDirectory(root + @"\nsmed");
        Directory.CreateDirectory(root + @"\nsthmb");
    }

    public void ResetSignupErrorMessages()
    {
        txtEmail.Text = string.Empty;
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtNickName.Text = string.Empty;
        txtPassword1.Text = string.Empty;
        txtPassword2.Text = string.Empty;
        drpCopuntries.Text = string.Empty;
        txtZipPostcode.Text = string.Empty;
        rbMale.Text = string.Empty;
        rbFemale.Text = string.Empty;
        txtZipPostcode.Text = string.Empty;
        drpDay.SelectedIndex = 0;
        drpMonth.SelectedIndex = 0;
        drpYear.SelectedIndex = 0;
        chbAgree.Text = string.Empty;

        errTxtEmail.Text = string.Empty;
        errTxtFirstName.Text = string.Empty;
        errTxtLastName.Text = string.Empty;
        errTxtNickName.Text = string.Empty;
        errTxtPassword1.Text = string.Empty;
        errDrpCountries.Text = string.Empty;
        errTxtZipPostcode.Text = string.Empty;
        errRblGender.Text = string.Empty;
        errTxtZipPostcode.Text = string.Empty;
        errDOB.Text = string.Empty;
        errChbAgree.Text = string.Empty;

        txtEmail.CssClass = "form_txt";
        txtFirstName.CssClass = "form_txt";
        txtLastName.CssClass = "form_txt";
        txtNickName.CssClass = "form_txt";
        txtPassword1.CssClass = "form_txt";
        txtPassword2.CssClass = "form_txt";
        drpCopuntries.CssClass = "form_txt";
        txtZipPostcode.CssClass = "form_txt";
        rbFemale.CssClass = "form_txt";
        rbMale.CssClass = "form_txt";
        txtZipPostcode.CssClass = "form_txt";
        drpDay.CssClass = "form_txt";
        drpMonth.CssClass = "form_txt";
        drpYear.CssClass = "form_txt";
        chbAgree.CssClass = "form_txt";
    }

    public void ResetLoginErrorMessages()
    {
        txtEmailLogin.Text = string.Empty;
        txtPasswordLogin.Text = string.Empty;
        errLogin.Text = string.Empty;
    }


    /// <summary>
    /// The user has clicked the login button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        ResetSignupErrorMessages();

        Member member = Member.WebMemberLogin(txtEmailLogin.Text, txtPasswordLogin.Text);

        if ( member == null )
        {
            errLogin.Text = "<p class='error_alert'>Invalid email or password</p>";
        }
        else if (txtEmailLogin.Text == string.Empty || txtPasswordLogin.Text == string.Empty)
        {
            errLogin.Text = "<p class='error_alert'>Invalid email or password</p>";
        }
        else
        {
            Session["Member"] = member;

            string RedirectToURL = (string)ViewState["CameFromURL"];

            if (RedirectToURL == null)
            {
                //Response.Redirect("/chat/");   
                RedirectToURL = "/dashboard";
            }

            // log the login date time
            OnlineNow now = new OnlineNow();
            now.MemberID = member.MemberID;
            now.DTOnline = DateTime.Now;
            now.Save();

            Utility.AddToLoggedIn();

            // write the login cookie
            if (chbRememberMe.Checked)
            {
                WriteRememberMeCookie(txtEmailLogin.Text, txtPasswordLogin.Text);
            }

            Response.Redirect(Server.UrlDecode(RedirectToURL));
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

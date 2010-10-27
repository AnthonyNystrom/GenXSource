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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Data;
using Next2Friends.Misc;
using System.Web.Mobile;

public enum MobileSignupStage {Stage1,Stage2,Complete}

public partial class MobileSignup : System.Web.UI.Page
{
    public MobileSignupStage CurrentStage = MobileSignupStage.Stage1;
    public Member NewMember;
    public MobileCapabilities MobileBrowser;
    public string Agent = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //CurrentStage = MobileSignupStage.Complete;
        MobileBrowser = (MobileCapabilities)Request.Browser;

        Agent = "height:" + MobileBrowser.ScreenPixelsHeight.ToString() + " width:" + MobileBrowser.ScreenPixelsWidth.ToString(); ;

        //Agent = MobilePhone.GetMobilePhoneFromUserAgent(Page.Request.ServerVariables["HTTP_USER_AGENT"]).Model;

        //Agent = Page.Request.ServerVariables["HTTP_ACCEPT"];

        drpCountries.Items.Insert(0, new ListItem("---", ""));
    }

    protected void btnSignup1_Click(object sender, EventArgs e)
    {
        bool SignUpOkay = true;

        if (txtEmail.Text == string.Empty)
        {
            SignUpOkay = false;
            errTxtEmail.Text = "<span class='formerror_msg'>The email field is not valid</span>";
        }
        else if (!RegexPatterns.TestEmailRegex(txtEmail.Text))
        {
            SignUpOkay = false;
            errTxtEmail.Text = "<span class='formerror_msg'>The email field is not valid</span>";
        }
        else if (!Member.IsEmailAddressAvailable(txtEmail.Text))
        {
            SignUpOkay = false;
            errTxtEmail.Text = "<span class='formerror_msg'>Address already registered</span>";
        }
        else
        {
            errTxtEmail.Text = string.Empty;
        }

        if (!RegexPatterns.TestNickname(txtNickName.Text))
        {
            SignUpOkay = false;
            errTxtNickName.Text = "<span class='formerror_msg'>Use only letters, numbers and no spaces</span>";
        }
        else if (!Member.IsNicknameAvailable(txtNickName.Text))
        {
            SignUpOkay = false;
            errTxtNickName.Text = "<span class='formerror_msg'>Unavailable</span>";
        }
        else
        {
            errTxtNickName.Text = string.Empty;
        }

        if (drpGender.SelectedIndex == 0)
        {
            SignUpOkay = false;
            errDrpGender.Text = "<span class='formerror_msg'>Please select your gender</span>";
        }
        else
        {
            errDrpGender.Text = string.Empty;
        }

        if (drpCountries.SelectedIndex == 0)
        {
            SignUpOkay = false;
            errDrpCountries.Text = "<span class='formerror_msg'>Please select your country</span>";
        }
        else
        {
            errDrpCountries.Text = string.Empty;
        }

        DateTime Birthday = DateTime.Now;

        try
        {
            Birthday = new DateTime(Int32.Parse(drpYear.SelectedValue),
                                Int32.Parse(drpMonth.SelectedValue),
                                Int32.Parse(drpDay.SelectedValue));
            errDOB.Text = string.Empty;
        }
        catch
        {
            SignUpOkay = false;
            errDOB.Text = "<span class='formerror_msg'>Please select your birthday</span>";
        }


        if (SignUpOkay)
        {
            NewMember = new Member();

            NewMember.Email = txtEmail.Text;
            NewMember.NickName = txtNickName.Text;
            NewMember.ISOCountry = drpCountries.SelectedValue;
            NewMember.Gender = Int32.Parse(drpGender.SelectedValue);
            NewMember.DOB = Birthday;

            Session["Member"] = NewMember;

            CurrentStage = MobileSignupStage.Stage2;
        }
    }


    protected void btnSignup2_Click(object sender, EventArgs e)
    {
        bool SignUpOkay = true;

        if (txtFirstName.Text.Trim() == string.Empty)
        {
            SignUpOkay = false;
            errTxtFirstName.Text = "<span class='formerror_msg'>No first name</span>";
        }
        else
        {
            errTxtFirstName.Text = string.Empty;
        }

        if (txtLastName.Text.Trim() == string.Empty)
        {
            SignUpOkay = false;
            errTxtLastName.Text = "<span class='formerror_msg'>No last name</span>";
        }
        else
        {
            errTxtLastName.Text = string.Empty;
        }

        if (txtPassword.Text == string.Empty)
        {
            SignUpOkay = false;
            errTxtPassword.Text = "<span class='formerror_msg'>No password</span>";
        }
        else if (txtPassword.Text != txtConfirm.Text)
        {
            SignUpOkay = false;
            errTxtPassword.Text = "<span class='formerror_msg'>Passwords do no match</span>";
        }
        else if (!RegexPatterns.TestPassword(txtPassword.Text))
        {
            SignUpOkay = false;
            errTxtPassword.Text = "minimum length 7 characters";
        }
        else
        {
            errTxtPassword.Text = string.Empty;
        }

        if (!cbTOS.Checked)
        {
            SignUpOkay = false;
            errChbTOS.Text = "<span class='formerror_msg'>You must agree to the tos</span>";
        }
        else
        {
            errChbTOS.Text = string.Empty;
        }

        if (SignUpOkay)
        {
            NewMember = (Member)Session["Member"];

            CurrentStage = MobileSignupStage.Complete;

            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            DbTransaction Transaction = null;

            try
            {
                conn.Open();
                Transaction = conn.BeginTransaction();

                NewMember.FirstName = txtFirstName.Text;
                NewMember.LastName = txtLastName.Text;


                NewMember.AccountType = 0;
                NewMember.Password = txtPassword.Text;

                NewMember.ProfilePhotoResourceFileID = 1;
                NewMember.WebMemberID = Next2Friends.Misc.UniqueID.NewWebID();
                NewMember.CreatedDT = DateTime.Now;
                NewMember.Save(db);

                // Ceate the business Account
                //if (rbBusiness.Checked)
                //{
                //    Business business = new Business();
                //    business.MemberID = NewMember.MemberID;
                //    business.CompanyName = txtCompanyName.Text;
                //    business.IndustrySector = drpIndustrySector.SelectedValue;
                //    business.YearFounded = Int32.Parse(drpYearFounded.SelectedValue);
                //    business.CompanySize = drpCompanySize.Text;
                //    business.IndustrySector = drpIndustrySector.SelectedValue;
                //    business.Save(db);

                //    IMSPlan imsPlan = new IMSPlan();
                //    imsPlan.BusinessID = business.BusinessID;
                //    imsPlan.Save(db);
                //}


                //ResourceFile.CreateUserDirectories(NewMember);
                TEMPCreateUserDirectories(NewMember);

                PhotoCollection DefaultGallery = new PhotoCollection();
                DefaultGallery.WebPhotoCollectionID = Next2Friends.Misc.UniqueID.NewWebID();
                DefaultGallery.MemberID = NewMember.MemberID;
                DefaultGallery.DTCreated = DateTime.Now;
                DefaultGallery.Name = NewMember.NickName + "'s Gallery";
                DefaultGallery.Description = "My First Gallery!";
                DefaultGallery.Save(db);

                // create a new member profile for the meber
                Next2Friends.Data.MemberProfile profile = new Next2Friends.Data.MemberProfile();
                profile.MemberID = NewMember.MemberID;
                profile.DTLastUpdated = DateTime.Now;
                profile.DefaultPhotoCollectionID = DefaultGallery.PhotoCollectionID;
                profile.Save(db);

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


            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        else
        {
            CurrentStage = MobileSignupStage.Stage2;
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

    
}

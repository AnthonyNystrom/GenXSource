using System;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Improsys.ContactImporter;
using Next2Friends.Data;

/// <summary>
/// Summary description for WebForm1.
/// </summary>
public partial class ImportEmails : System.Web.UI.Page
{
    public string[] nameArray = new string[0];
    public string[] emailArray = new string[0];

    public bool LoginOkay = false;
    ContactImporter contactImporter = null;
    public Member member;
    public List<ContactImport> FullContacts;
    public string HTMLContactList;
    public string HTMLIndexList;
    public List<string> LettersPresent = new List<string>();
    public bool Imported = false;
    public int ImportCount;
    public string ErrorMessage = string.Empty;
    public bool ImportStage = false;
    public int CountactCount = 0;
    public string HelperMessage;
    public string ContactBoxMessage = string.Empty;
    public TimeSpan taken;
    public TimeSpan fetch;
    public bool IsIE = false;
    public bool IsSignup = false;


    protected void Page_Load(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];
        SetInternetExplorer();

        if (Request.Form["friendlist"] != null)
        {
            try
            {
                ImportAndInvite();
            }
            catch ( Exception ex)
            {
                Next2Friends.Data.Trace.Tracer(ex.ToString());
                throw ex;
            }
        }
        else if (!IsPostBack)
        {
            BuildLetterIndexList();
            HelperMessage = "<p style='color:#0257AE;'>Log into your webmail account below</p>";
            ContactBoxMessage = "Log into your webmail to invite your friends!";
        }

        string strSignup = Request.Params["signup"];

        if (strSignup != null)
        {
            IsSignup = true;
        }
    }

        protected void btnWebMailLogin_Click(object sender, System.EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ImportEmails));

            member = (Member)Session["Member"];

            if (member == null)
            {
                Response.Redirect("/signup");
            }

            if (this.txtUserID.Text == "" || this.txtPassword.Text == "")
                return;

            Regex isNumeric = new Regex(@"^\d+$");

            //strip email address
            string Login = txtUserID.Text;

            int AtSignIndex = Login.IndexOf("@");

            if (AtSignIndex > 0)
            {
                if (emailCat.SelectedValue.ToLower() != "linkedin.com")
                {
                    Login = Login.Substring(0, AtSignIndex);
                }
            }

            DateTime start = DateTime.Now;

            contactImporter = new ContactImporter(Login, txtPassword.Text, emailCat.SelectedValue);

            contactImporter.login();

            LoginOkay = contactImporter.logged_in;

            if (LoginOkay)
            {
                try
                {
                    contactImporter.getcontacts();
                    fetch = DateTime.Now - start;
                }
                catch { }

                this.nameArray = contactImporter.nameArray;
                this.emailArray = contactImporter.emailArray;

                for (int i = 0; i < emailArray.Length; i++)
                {
                    emailArray[i] = emailArray[i].ToLower();
                }

                string ImportToken = Next2Friends.Misc.UniqueID.NewWebID();

                //insert into the db so they can be referenced by JoinEmailsWithMembers
                for (int i = 0; i < emailArray.Length; i++)
                {
                    ContactImport contact = new ContactImport();

                    contact.WebID = Next2Friends.Misc.UniqueID.NewWebID();
                    contact.ImporterMemberID = member.MemberID;
                    contact.Email = emailArray[i];
                    contact.Name = nameArray[i];
                    contact.FriendState = (int)FriendState.None;
                    contact.InviteState = (int)InviteState.None;
                    contact.ImportToken = ImportToken;
                    contact.IsInitialImport = 1;

                    contact.SaveWithCheck();
                }

                FullContacts = new List<ContactImport>();
                List<ContactImport> MemberList = new List<ContactImport>();

                //List<ContactImport> ExistingContactList = ContactImport.GetAllContactImportByMemberID(member.MemberID);

                if (emailArray.Length > 0)
                {
                    MemberList = ContactImport.JoinEmailsWithMembers(member.MemberID, emailArray, ImportToken);
                }

                List<Member> FriendList = Member.GetAllFriendsByMemberID(member.MemberID);

                for (int i = 0; i < emailArray.Length; i++)
                {
                    int friendState = (int)FriendState.None;
                    int inviteState = (int)InviteState.None;

                    ContactImport Associcate = MemberList.FirstOrDefault(f => f.Email == emailArray[i]);

                    if (Associcate != null)
                    {
                        friendState = Associcate.FriendState;
                        inviteState = Associcate.InviteState;
                    }

                    ContactImport contact = new ContactImport();

                    contact.WebID = Next2Friends.Misc.UniqueID.NewWebID();
                    contact.ImporterMemberID = member.MemberID;
                    contact.Email = emailArray[i];
                    contact.Name = nameArray[i];
                    contact.FriendState = friendState;
                    contact.InviteState = inviteState;

                    FullContacts.Add(contact);
                }

                var sortedContacts =
                    from c in FullContacts
                    where c.Email != string.Empty
                    orderby c.Name
                    select c;

                FullContacts = sortedContacts.ToList();

                Imported = true;
                ImportCount = FullContacts.Count;

                Session["Contacts"] = FullContacts;

                BuildContactList();

                ImportStage = true;

                HelperMessage = "<p style='color:#0257AE'>You may now choose your contacts to invite</p>";
                
            }
            else
            {
                BuildLetterIndexList();
                Session["Contacts"] = null;
                HelperMessage = "<p style='color:red;'>Login details are incorrect, please try again</p>";
            }
        }

        public void AsyncImport()
        {
            FullContacts = (List<ContactImport>)Session["Contacts"];
            DateTime start = DateTime.Now;

            if (FullContacts != null)
            {
                for (int i = 0; i < FullContacts.Count; i++)
                {
                    FullContacts[i].IsInitialImport = 0;
                    FullContacts[i].SaveWithCheck();
                }
            }

            taken = DateTime.Now - start;
        }


        /// <summary>
        /// Builds the main contact list
        /// </summary>
        public void BuildContactList()
        {
            CountactCount = 0;

            //MiniContacts = new List<MiniContact>();

            FullContacts = (List<ContactImport>)Session["Contacts"];

            StringBuilder sbHTML = new StringBuilder();

            string LastLetter = "0";

            for (int i = 0; i < FullContacts.Count; i++)
            {
                string FirstLetter = (FullContacts[i].Name.Length > 0) ? FullContacts[i].Name[0].ToString().ToLower() : string.Empty;

                CountactCount++;

                if (Regex.IsMatch(FirstLetter, "[A-Za-z]"))
                {
                    if (FirstLetter != LastLetter)
                    {
                        LastLetter = FirstLetter;

                        LettersPresent.Add(FirstLetter.ToUpper());


                        sbHTML.AppendFormat(@"<a name='{0}'><h2 class='subcatHeader'><span>{0}</span></h2></a>", LastLetter.ToUpper());
                    }
                }

                string[] Parameters = new string[6];
                Parameters[0] = FullContacts[i].Name.Replace("'", "&#39");
                Parameters[1] = FullContacts[i].Email;

                switch (FullContacts[i].InviteState)
                {
                    case (int)InviteState.None:
                        Parameters[2] = "<input type='checkbox' id='e" + FullContacts[i].WebID + "' class='echb' />  Email Invite";
                        break;
                    case (int)InviteState.AlreadyAMember:
                        Parameters[2] = "<img src='/images/user-check.gif'> Member";
                        break;
                    case (int)InviteState.InviteSent:
                        Parameters[2] = "<img src='/images/check.gif'> Invite Sent";
                        break;
                    case (int)InviteState.InviteAcepted:
                        Parameters[2] = "--";
                        break;
                }

                switch (FullContacts[i].FriendState)
                {
                    case (int)FriendState.None:
                        Parameters[3] = "<input type='checkbox' id='f" + FullContacts[i].WebID + "' class='fchb' /> Friend Request";
                        break;
                    case (int)FriendState.AlreadyAFriend:
                        Parameters[3] = "<img src='/images/friends.gif'> Friend";
                        break;
                    case (int)FriendState.InviteSent:
                        Parameters[3] = "<img src='/images/check.gif'> Request Sent";
                        break;
                    case (int)FriendState.FriendRequestAcepted:
                        Parameters[3] = "--";
                        break;
                }
                
                Parameters[4] = i.ToString();
                Parameters[5] = FullContacts[i].WebID;

                sbHTML.AppendFormat(@"<div class='contactSubCat'>
							<div class='contactName'>
								<h3>
									{0}
									<small>{1}</small>
								</h3>
							</div>
							<div class='contactDetails'>
								<p class='firstCheckbox'>
									{2}
								</p>
								<p class='secondCheckbox'>
									{3}
								</p>
							</div>
							<div class='clear'></div>
						</div>", Parameters);
            }

            HTMLContactList = sbHTML.ToString();

            BuildLetterIndexList();
        }

        [AjaxPro.AjaxMethod]
        public string GetContact()
        {
            BuildContactList();

            if (HTMLContactList == string.Empty)
            {
                HTMLContactList = @"<div style='text-align: center; width: 250px; position: relative; left: 75px; top: 150px;'>
				                    <p>You have no contacts in this account</p></div>";
            }

            return HTMLContactList;
        }

        public void BuildLetterIndexList()
        {
            StringBuilder sbHTML = new StringBuilder();

            string[] Alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            for (int i = 0; i < Alphabet.Length; i++)
            {
                var letter = from l in LettersPresent where l == Alphabet[i] select l;

                if (letter.Count() == 0)
                {
                    sbHTML.AppendFormat(@"<li><span>{0}</span></li>", Alphabet[i]);
                }
                else
                {
                    sbHTML.AppendFormat(@"<li class='selected'><a href='#{0}'>{0}</a></li>", Alphabet[i]);
                }

            }

            HTMLIndexList = sbHTML.ToString();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["Contacts"] = null;
            ImportStage = false;
            HelperMessage = "<p style='color:#0257AE;'>Invite cancelled, log into your webmail account below</p>";
            ContactBoxMessage = "Log into your webmail to invite your friends!";
            BuildLetterIndexList();
        }

        protected void ImportAndInvite()
        {
            FullContacts = (List<ContactImport>)Session["Contacts"];

            if (FullContacts == null)
            {
                ImportStage = false;
                HelperMessage = "<p style='color:#0257AE'>Your session timed out<br /> Log into your webmail account below</p>";
                BuildLetterIndexList();

                return;
            }

            string[] EmailInviteList = Request.Form["emaillist"].Split(new char[]{','});
            string[] FriendnviteList = Request.Form["friendlist"].Split(new char[]{','});

            foreach (string key in EmailInviteList)
            {
                string WebIDKey = key;

                if (key.Length > 0)
                {
                    WebIDKey = key.Substring(1);
                }

                ContactImport contact = FullContacts.FirstOrDefault(e => e.WebID == WebIDKey);

                if (contact != null)
                {
                    // make sure noone has hacked the checkboxes client side to send again..
                    if (contact.InviteState == (int)InviteState.None)
                    {
                        contact.InviteState = (int)InviteState.InviteSent;
                    }
                }
            }

            foreach (string key in FriendnviteList)
            {
                string WebIDKey = key;

                if (key.Length > 0)
                {
                    WebIDKey = key.Substring(1);
                }

                ContactImport contact = FullContacts.FirstOrDefault(e => e.WebID == WebIDKey);

                if (contact != null)
                {
                    if (contact.FriendState == (int)FriendState.None)
                    {
                        //could fire off the friend request here
                        if (contact.InviteState == (int)InviteState.AlreadyAMember)
                        {
                            FriendRequest.CreateWebFriendRequestFromEmail(member.MemberID, contact.Email);
                        }

                        contact.FriendState = (int)FriendState.InviteSent;
                    }
                    
                }
            }

            ImportStage = false;
            HelperMessage = "<p style='color:#0257AE'>Your invites have been sent<br /> Login to another account to invite more</p>";
            ContactBoxMessage = "Your friends have been invited";
            AsyncImport();
            BuildLetterIndexList();

            Session["Contacts"] = FullContacts;
            
        }

        public void SetInternetExplorer()
        {
            // Returns the version of Internet Explorer or a -1
            // (indicating the use of another browser).
            float rv = -1;
            System.Web.HttpBrowserCapabilities browser = Request.Browser;
            if (browser.Browser == "IE")
                rv = (float)(browser.MajorVersion + browser.MinorVersion);


            if (rv != (-1))
            {
                IsIE = true;
            }

        }


        /// <summary>
        /// set the page skin
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            Master.N2FPageURL = "invite";
            member = (Member)Session["Member"];

            if (member==null)
            {
                Response.Redirect("/signup");
            }

            base.OnPreInit(e);
        }
}
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
using Next2Friends.Data;

public partial class Main : System.Web.UI.MasterPage
{
    public string LoginHTMLBlock = @"<a href='/login.aspx'>Login</a> ";
    private SiteSection _CurrentSection;
    public Member member;
    public bool IsLoggedIn = false;
    public bool ChatEnabled = true;
    public bool LoggedIntoChat = false;

    public string IsVideoSelected = string.Empty;
    public string IsPeopleSelected = string.Empty;

    public string IsMaleSelected = string.Empty;
    public string IsFemaleSelected = string.Empty;

    public string txtSearch = string.Empty;
    public string txtEmail = string.Empty;
    public string txtCity = string.Empty;
    public string txtProfile = string.Empty;
    public string AvatarOnly = string.Empty;
    public string DisplayAdvancedSearchLink = "none";
    public string AnnoyPopupMessage = string.Empty;
    public bool showFooter = true;

    private string _N2FPageURL = string.Empty;
    public string N2FPageURL { set { _N2FPageURL = value; } get { return _N2FPageURL; } }

    public bool ShowFooter
    {
        get { return showFooter; }

        set { showFooter = value; }
    }

    public string HTMLTitle
    {

        get { return MasterHeader.Title; }

        set { MasterHeader.Title = value; }

    }

    public string MetaDescription
    {
        set
        {
            HtmlMeta metaDescription = new HtmlMeta();

            metaDescription.Name = "description";
            metaDescription.Content = value;
            Page.Header.Controls.Add(metaDescription);
        }
    }

    public string MetaKeywords
    {
        set
        {
            HtmlMeta metaDescription = new HtmlMeta();

            metaDescription.Name = "keywords";
            metaDescription.Content = value;
            Page.Header.Controls.Add(metaDescription);
        }
    }

    
    public SiteSection CurrentSection
    {
        get
        {
            return _CurrentSection;
        }
        set { _CurrentSection = value; }
    }

    public string N2FPageStyle
    {
        get
        {
            if (this.SkinID == string.Empty)
            {
                return string.Empty;
            }
            else
            {
                return " id='" + SkinID + "'";
            }
        }
    }

    public override string SkinID
    {
        get
        {
            return base.SkinID;
        }
        set
        {
            SetSkin(value);

            base.SkinID = value;
        }
    }

    public void SetSkin(string SkinID)
    {
        switch (SkinID.ToLower())
        {
            case "home":
                CurrentSection = SiteSection.Home;
                break;
            case "friend":
                CurrentSection = SiteSection.Friend;
                break;
            case "dashboard":
                CurrentSection = SiteSection.Dashboard;
                break;
            case "profile":
                CurrentSection = SiteSection.Profile;
                break;
            case "video":
                CurrentSection = SiteSection.Video;
                break;
            case "photo":
                CurrentSection = SiteSection.Photo;
                break;
            case "hotspot":
                CurrentSection = SiteSection.Hotspot;
                break;
            case "inbox":
                CurrentSection = SiteSection.Inbox;
                break;
            case "community":
                CurrentSection = SiteSection.Community;
                break;
            case "askafriend":
                CurrentSection = SiteSection.AskAFriend;
                break;
            case "domore":
                CurrentSection = SiteSection.DoMore;
                break;
            case "feed":
                //     CurrentSection = SiteSection.Feed;
                break;
            case "":
                CurrentSection = SiteSection.None;
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HTMLTitle = "Next2Friends.com";
        if (!Page.IsPostBack)
        {
            string strCameFrom = Request.Params["u"];

            if (strCameFrom != null)
            {
                ViewState["CameFromURL"] = strCameFrom;
            }
        }

        try
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main));
        }
        catch { }

        //txtSearchMain.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnSearch.UniqueID + "','');return false}");
        //drpSearchMain.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){__doPostBack('" + btnSearch.UniqueID + "','');return false}");

        txtEmailLogin.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false}");
        txtPassword.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13){ __doPostBack('" + btnLogin.UniqueID + "','');return false}");
    
        member = (Member)Session["Member"];

        if (member != null)
        {
            UserStatus.AddUser(member.WebMemberID);
            try
            {
                LoggedIntoChat = (bool)Session["LoggedIntoChat"];
            }
            catch { }
        }

        SetSearchParamsToFields();

        //ShowNagScreen();

    }


    public void ShowNagScreen()
    {
        int InitialNagValue = 5;

        member = (Member)Session["Member"];

        AnnoyPopupMessage = string.Empty;

        if (member == null)
        {
            SignupNagScreen nag = new SignupNagScreen();

            if (Session["NagScreen"] == null)
            {
                nag.CurrentCount = InitialNagValue;
                //nag.NextCountValue = InitialNagValue;
                Session["NagScreen"] = nag;
            }

            try
            {
                nag = (SignupNagScreen)Session["NagScreen"];

                if (--nag.CurrentCount <= 0)
                {
                    //nag.NextCountValue = nag.NextCountValue * 2;
                    nag.CurrentCount = InitialNagValue;

                    AnnoyPopupMessage = @"<script>npopup('Why not join Next2Friends?','<a href=""/signup"">Signup now</a> and start connecting with all your friends and family!',450,40);</script>";
                }
                Session["NagScreen"] = nag;
            }
            catch 
            {
                Session["NagScreen"] = null;
            }
        }
    }


    public void SetSearchParamsToFields()
    {
        string SearchType = Request.QueryString["type"];

        if (SearchType!=null)
        {
            txtSearch =  Request.QueryString["search"];

            if (SearchType.ToLower() == "people")
            {
                int gender = -1;
                if (!string.IsNullOrEmpty(Request.QueryString["sex"]))
                    int.TryParse(Request.QueryString["sex"], out gender);

                if (gender == 1)
                {
                    IsMaleSelected = "selected";
                }
                else if (gender == 0)
                {
                    IsFemaleSelected = "selected";
                }


                txtEmail = Request.QueryString["email"];
                txtCity = Request.QueryString["city"];
                txtProfile = Request.QueryString["profile"];

                if (SearchType.ToLower() == "video")
                {
                    IsVideoSelected = " selected ";
                }
                else if (SearchType.ToLower() == "people")
                {
                    IsPeopleSelected = " selected ";
                    DisplayAdvancedSearchLink = "inline";
                }

                int hasAvatarPhoto = -1;
                if (!string.IsNullOrEmpty(Request.QueryString["avatar"]) && Request.QueryString["avatar"].Trim() == "yes")
                    hasAvatarPhoto = 1;

                if (hasAvatarPhoto == 1)
                {
                    AvatarOnly = " checked ";
                }
            }
            else if (SearchType.ToLower() == "video")
            {

            }
        }
    }

    /// <summary>
    /// The user wishes to logout
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        //nullify the session and page object
        member = (Member)Session["Member"];

        if (member != null)
        {
            try
            {
                MemberSettings ms = member.MemberSettings[0];
                ms.AutoLoadChatMode = -1;
                ms.Save();
            }
            catch { }

            try
            {
                UserStatus.RemoveUser(member.WebMemberID);

                //Next2Friends.ChatClient.ChatLogic.LogoutOfChatServer(member.WebMemberID);
            }
            catch { }
        }

        Session["Member"] = null;
        member = null;

        Response.Cookies["LastActivity"].Value = null;
        Response.Cookies["LastActivity"].Expires = System.DateTime.Now.AddMonths(-1);

          

        Response.Redirect("/breakout.aspx");
    }

    /// <summary>
    /// The user has clicked the login button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Member member = Member.WebMemberLogin(txtEmailLogin.Text, txtPassword.Text);

        if (member == null)
        {
           // for the user to the login page and show the message
            string url = "/signup/?err=true";
            if (ViewState["CameFromURL"] != null)
                url += "&u=" + ViewState["CameFromURL"];

            Response.Redirect(url);
        }
        else if (txtEmailLogin.Text == string.Empty || txtPassword.Text == string.Empty)
        {
            // for the user to the login page and show the message
            string url = "/signup/?err=true";
            if (ViewState["CameFromURL"] != null)
                url += "&u=" + ViewState["CameFromURL"];

            Response.Redirect(url);
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


            //HttpCookie aCookie = new HttpCookie("userInfo");
            //aCookie.Values["email"] = member.Email;
            //aCookie.Values["password"] = member.Password;

            // log the login date time
            OnlineNow now = new OnlineNow();
            now.MemberID = member.MemberID;
            now.DTOnline = DateTime.Now;
            now.Save();

            Utility.AddToLoggedIn();

            if (chbRememberMe.Checked)
            {
                WriteRememberMeCookie(txtEmailLogin.Text, txtPassword.Text);
            }

            Response.Redirect(RedirectToURL);
            //Response.Redirect("index.aspx");
            //Response.Redirect("/chat/");   
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


    protected override void OnPreRender(EventArgs e)
    {
        Utility.RememberMeLogin();

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;

            ASP.global_asax.NewMessageCount = member.GetNewMessageCount();
        }
        else
        {
            // lock the site down to all non-members (and evil)
            //LockDown.IsAllowed(this.Page);
        }

        base.OnPreRender(e);
    }

    public class SignupNagScreen
    {
        public int CurrentCount { get; set; }
        public int NextCountValue { get; set; }
    }
   
}

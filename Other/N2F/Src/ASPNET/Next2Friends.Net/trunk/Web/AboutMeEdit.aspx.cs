using System;
using System.Text;
using System.Collections.Generic;
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

public partial class AboutMeEdit : System.Web.UI.Page
{
    public Member member;
    public string DefaultLister = string.Empty;
    public string DefaultPager = string.Empty;

    public string DefaultNewCommentParams;
    public string DefaultNumberOfViews = "0";
    public int AboutMeLength = 0;
    public int MusicLength = 0;
    public int MoviesLength = 0;
    public int BooksLength = 0;

    public int OurCompanyLength = 0;
    public int Description1Length = 0;
    public int Description2Length = 0;
    public int Description3Length = 0;

    public string WebRoot = ASP.global_asax.WebServerRoot;

    protected void Page_Load(object sender, EventArgs e)
    {
      
        AjaxPro.Utility.RegisterTypeForAjax(typeof(AboutMeEdit));

        member = (Member)Session["Member"];

        if (!Page.IsPostBack)
        {
            LoadMember();
            PopulateOtherHalfCombo();

            if (member.AccountType == (int)AccountType.Personal)
            {
                LoadMemberProfile();
            }
            else if (member.AccountType == (int)AccountType.Business)
            {
                LoadBusiness();
            }

            drpDayJob.Items.Insert(0, new ListItem("Select Profession", "-1"));
            drpNightJob.Items.Insert(0, new ListItem("Select Profession", "-1"));
            drpFavInterest.Items.Insert(0, new ListItem("Select Interest", "-1"));
            
        }

    }

    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveMember();
        SaveMemberProfile();
        Response.Redirect("/users/" + member.NickName);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/users/" + member.NickName);
    }
    /// <summary>
    /// Save
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave2_Click(object sender, EventArgs e)
    {
        SaveMember();
        SaveBusiness();
        Response.Redirect("/users/" + member.NickName);
    }

    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        Response.Redirect("/users/" + member.NickName);
    }

    
    /// <summary>
    /// Loads the personal member profile
    /// </summary>
    private void LoadMemberProfile()
    {
        if (member != null)
        {
            List<MemberProfile> mpList = member.MemberProfile;
            MemberProfile mp = null;

            if (mpList.Count > 0)
            {
                mp = mpList[0];
            }

            if (mp != null)
            {
                try
                {
                    cmbRelationShipStat.SelectedValue = mp.RelationshipStatus.ToString();
                    cmbOtherHalf.SelectedValue = mp.OtherHalfID.ToString();
                }
                catch
                {
                }

                drpDayJob.SelectedValue = mp.DayProfessionID.ToString();
                drpNightJob.SelectedValue = mp.NightProfessionID.ToString();
                drpFavInterest.SelectedValue = mp.HobbyID.ToString();

                txtMyLife.Text = Server.HtmlDecode(mp.MyLife);
                txtBook.Text = Server.HtmlDecode(mp.Books);
                txtMovie.Text = Server.HtmlDecode(mp.Movies);
                txtMusic.Text = Server.HtmlDecode(mp.Music);
                txtHomeTown.Text = Server.HtmlDecode(mp.HomeTown);
                //txtTagLine.Text = Server.HtmlDecode(mp.TagLine);

                txtMySpace.Text = mp.MySpaceURL;
                txtFaceBook.Text = mp.FaceBookURL;
                txtBlog.Text = mp.BlogURL;
                txtBlogFeed.Text = mp.BlogFeedURL;

                int MaxFieldLen = 1000;
                AboutMeLength = MaxFieldLen - mp.MyLife.Length;
                MusicLength = MaxFieldLen - mp.Music.Length;
                MoviesLength = MaxFieldLen - mp.Movies.Length;
                BooksLength = MaxFieldLen - mp.Books.Length;
            }            
        }
    }

    /// <summary>
    /// Loads the business profile
    /// </summary>
    private void LoadBusiness()
    {
        if (member != null)
        {
            for (int i = 2008; i > 1900; i--)
            {
                drpYearFounded.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            List<Business> businessList = member.Business;
            Business business = null;

            if (businessList.Count > 0)
            {
                business = businessList[0];
            }

            if (business != null)
            {

                txtCName.Text=Server.HtmlDecode(business.CompanyName );
                txtCWebsite.Text=Server.HtmlDecode(business.CompanyWebsite);
                //txtCTagLine.Text=Server.HtmlDecode(business.TagLine);
                txtContactFirst.Text=Server.HtmlDecode(business.ContactFirst);
                txtContactLast.Text=Server.HtmlDecode(business.ContactLast );
                drpIndustrySector.SelectedValue = business.IndustrySector;
                drpYearFounded.SelectedValue = business.YearFounded.ToString();
                drpCompanySize.SelectedValue = business.CompanySize;

                txtCAddress.Text=Server.HtmlDecode(business.StreetAddress);
                txtCState.Text=Server.HtmlDecode(business.State);
                txtCCity.Text=Server.HtmlDecode(business.City );
                drpCCountery.SelectedValue = business.Country;
                txtCZipcode.Text=Server.HtmlDecode(business.ZipCode);

                txtCMySpaceURL.Text = Server.HtmlDecode(business.MySpaceURL);
                txtCFacebookURL.Text = Server.HtmlDecode(business.FaceBookURL);
                txtCBlogURL.Text = Server.HtmlDecode(business.BlogURL);
                txtCBlogFeedURL.Text = Server.HtmlDecode(business.BlogFeedURL);

                txtOurCompany.Text=Server.HtmlDecode(business.OurCompany);

                txtBusinessDescription1.Text=Server.HtmlDecode(business.BusinessDescription1);
                txtBusinessDescription2.Text=Server.HtmlDecode(business.BusinessDescription2);
                txtBusinessDescription3.Text=Server.HtmlDecode(business.BusinessDescription3);

                int MaxFieldLen = 1000;
                OurCompanyLength = MaxFieldLen - business.OurCompany.Length;
                Description1Length = MaxFieldLen - business.BusinessDescription1.Length;
                Description2Length = MaxFieldLen - business.BusinessDescription2.Length;
                Description3Length = MaxFieldLen - business.BusinessDescription3.Length;

            }
        }
    }

    /// <summary>
    /// Link the members wife/girlfriend/boyfriend/husband with their profile
    /// </summary>
    private void PopulateOtherHalfCombo()
    {
        if (member != null)
        {
            Friend[] friends = Friend.GetAllFriendsByMemberIDWithJoin(member.MemberID);

            IEnumerable<Friend> sortedFriends =
            from friend in friends
            orderby friend.Member.NickName ascending
            select friend;

            foreach(Friend f in sortedFriends)
            {
                cmbOtherHalf.Items.Add(new ListItem(f.Member.NickName, f.MemberID2.ToString()));
            }
        }
    }

    /// <summary>
    /// Set the member values
    /// </summary>
    private void LoadMember()
    {
        if (member != null)
        {
            txtFName.Text = member.FirstName;
            txtLName.Text = member.LastName;
            txtZip.Text = member.ZipPostcode;            
           
            drpCopuntries.SelectedValue = member.ISOCountry;
            cmbGender.SelectedValue = member.Gender.ToString();

            drpDay.SelectedValue = member.DOB.Day.ToString();
            drpMonth.SelectedValue = member.DOB.Month.ToString();
            drpYear.SelectedValue = member.DOB.Year.ToString();

        }
    }

    /// <summary>
    /// resave the member profile to the database
    /// </summary>
    private void SaveMemberProfile()
    {
        if (member != null)
        {
            List<MemberProfile> mpList = member.MemberProfile;
            MemberProfile mp = null;

            if (mpList.Count > 0)
            {
                mp = mpList[0];
            }

            if (mp == null)
            {
                mp = new MemberProfile();
                mp.MemberID = member.MemberID;                
            }

            mp.DayProfessionID = int.Parse(drpDayJob.SelectedValue);
            mp.NightProfessionID = int.Parse(drpNightJob.SelectedValue);
            mp.HobbyID = int.Parse(drpFavInterest.SelectedValue);

            mp.MyLife = Server.HtmlEncode(txtMyLife.Text);
            mp.Books = Server.HtmlEncode(txtBook.Text);
            mp.Movies = Server.HtmlEncode(txtMovie.Text);
            mp.Music = Server.HtmlEncode(txtMusic.Text);
            mp.HomeTown = Server.HtmlEncode(txtHomeTown.Text);
            //mp.TagLine = Server.HtmlEncode(txtTagLine.Text);
            mp.RelationshipStatus = int.Parse(cmbRelationShipStat.SelectedValue);
            mp.OtherHalfID = int.Parse(cmbOtherHalf.SelectedValue);
            mp.MySpaceURL = Server.HtmlEncode(txtMySpace.Text);
            mp.FaceBookURL = Server.HtmlEncode(txtFaceBook.Text);
            mp.BlogURL = Server.HtmlEncode(txtBlog.Text);
            mp.BlogFeedURL = Server.HtmlEncode(txtBlogFeed.Text);
            mp.Save();
        }
    }

    /// <summary>
    /// resave the business profile to the db
    /// </summary>
    private void SaveBusiness()
    {
        if (member != null)
        {
            List<Business> businessList = member.Business;
            Business business = null;

            if (businessList.Count > 0)
            {
                business = businessList[0];
            }

            if (business == null)
            {
                business = new Business();
                business.MemberID = member.MemberID;
            }

            business.CompanyName = Server.HtmlEncode(txtCName.Text);
            business.CompanyWebsite = Server.HtmlEncode(txtCWebsite.Text);
            //business.TagLine = Server.HtmlEncode(txtCTagLine.Text);
            business.ContactFirst = Server.HtmlEncode(txtContactFirst.Text);
            business.ContactLast = Server.HtmlEncode(txtContactLast.Text);
            business.IndustrySector = drpIndustrySector.SelectedValue;

            int YearFounded = 1900;
            int.TryParse(drpYearFounded.SelectedValue,out YearFounded);
            business.YearFounded = YearFounded;

            business.CompanySize = drpCompanySize.SelectedValue;
            business.StreetAddress = Server.HtmlEncode(txtCAddress.Text);
            business.State = Server.HtmlEncode(txtCState.Text);
            business.City = Server.HtmlEncode(txtCCity.Text);
            business.Country = drpCCountery.SelectedValue;
            business.ZipCode = Server.HtmlEncode(txtCZipcode.Text);

            business.MySpaceURL =Server.HtmlEncode(txtCMySpaceURL.Text);
            business.FaceBookURL = Server.HtmlEncode(txtCFacebookURL.Text);
            business.BlogURL =Server.HtmlEncode(txtCBlogURL.Text);
            business.BlogFeedURL = Server.HtmlEncode(txtBlogFeed.Text);
            //business.OtherWebsites = Server.HtmlEncode(drpURL.Items[4].Value);

            business.OurCompany = Server.HtmlEncode(txtOurCompany.Text);

            business.BusinessDescription1 = Server.HtmlEncode(txtBusinessDescription1.Text);
            business.BusinessDescription2 = Server.HtmlEncode(txtBusinessDescription2.Text);
            business.BusinessDescription3 = Server.HtmlEncode(txtBusinessDescription3.Text);


            business.Save();
        }
      }


    /// <summary>
    /// resave the personal profile to the db
    /// </summary>
    public void SaveMember()
    {
        if (member != null)
        {
            if (member.AccountType == 0)
            {
                member.FirstName = Server.HtmlEncode(txtFName.Text);
                member.LastName = Server.HtmlEncode(txtLName.Text);
            }

            member.ZipPostcode = Server.HtmlEncode(txtZip.Text);
            member.ISOCountry = drpCopuntries.SelectedValue;
            

            if (member.AccountType == 0)
            {
                member.Gender = int.Parse(cmbGender.SelectedValue.ToString());

                try
                {
                    DateTime DOB = new DateTime(
                                    int.Parse(drpYear.SelectedValue.ToString()),
                                    int.Parse(drpMonth.SelectedValue.ToString()),
                                    int.Parse(drpDay.SelectedValue.ToString())
                                    );

                    member.DOB = DOB;
                }
                catch { }
            
            }

            

            member.Save();
        }
    }
   

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        member = (Member)Session["Member"];

        // only allow the member in if logged in
        if (Session["Member"] == null)
        {
            Response.Redirect("/signup/?u=" + Request.Url.AbsoluteUri);
        }

        //Master.SkinID = "";
        base.OnPreInit(e);
    }

}

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

public enum AgeRange { All = 1, Age1824 = 2, Age2530 = 3, Age3134 = 4, Age35Plus = 5 }
public enum GenderRange { Both =0, Female = 1, Male = 2}
public enum Sexuality {Undisclosed=0, Straight = 1, Gay = 2, Bisexual = 3  }

public partial class MatchProfilePage : System.Web.UI.Page
{
    public Member member;
    public MatchProfile matchProfile;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = 0; i < MatchProfile.ArrMusicGenre.Length; i++)
            {
                dropMusic.Items.Add(new ListItem(MatchProfile.ArrMusicGenre[i], i.ToString()));
            }

            LoadMatchProfile();

        }
    }

    private void LoadMatchProfile()
    {
        member = (Member)Session["Member"];

        MatchProfile matchProfile = member.MatchProfile[0];

        #region Age
        if (matchProfile.AgeRange == (int)AgeRange.Age1824)
        {
            rb1830.Checked = true;
        }
        else if (matchProfile.AgeRange == (int)AgeRange.Age2530)
        {
            rb2530.Checked = true;
        }
        else if (matchProfile.AgeRange == (int)AgeRange.Age3134)
        {
            rb3134.Checked = true;
        }
        else if (matchProfile.AgeRange == (int)AgeRange.Age35Plus)
        {
            rb35.Checked = true;
        }
        else
        {
            rbAll.Checked = true;
        }
        #endregion

        #region Gender

        if (matchProfile.LookingForGender == (int)GenderRange.Male)
        {
            rbGenderMale.Checked = true;
        }
        else if (matchProfile.LookingForGender == (int)GenderRange.Female)
        {
            rbGenderFemale.Checked = true;
        }
        else
        {
            rbGenderBoth.Checked = true;
        }

        #endregion

        #region Sexuality

        if (matchProfile.Sexuality == (int)Sexuality.Straight)
        {
            rbStraight.Checked = true;
        }
        if (matchProfile.Sexuality == (int)Sexuality.Gay)
        {
            rbGay.Checked = true;
        }
        if (matchProfile.Sexuality == (int)Sexuality.Bisexual)
        {
            rbBisexual.Checked = true;
        }
        else
        {
            rbUndisclosed.Checked = true;
        }

        #endregion

        dropMusic.SelectedValue = matchProfile.Music.ToString();
        //drpinterests.SelectedValue = matchProfile.Interests.ToString();
    }

        

    protected void btnSave_Click(object sender, EventArgs e)
    {
        member = (Member)Session["Member"];

        MatchProfile matchProfile = member.MatchProfile[0];

        #region Age
        if (rb1830.Checked)
        {
            matchProfile.AgeRange = (int)AgeRange.Age1824;
        }
        else if (rb2530.Checked)
        {
            matchProfile.AgeRange = (int)AgeRange.Age2530;
        }
        else if (rb3134.Checked)
        {
            matchProfile.AgeRange = (int)AgeRange.Age3134;
        }
        else if (rb35.Checked)
        {
            matchProfile.AgeRange = (int)AgeRange.Age35Plus;
        }
        else if (rbAll.Checked)
        {
            matchProfile.AgeRange = (int)AgeRange.All;
        }
        #endregion

        #region Gender
        if (rbGenderMale.Checked)
        {
            matchProfile.LookingForGender = (int)GenderRange.Male;
        }
        else if (rbGenderFemale.Checked)
        {
            matchProfile.LookingForGender = (int)GenderRange.Female;
        }
        else if (rbGenderBoth.Checked)
        {
            matchProfile.LookingForGender = (int)GenderRange.Both;
        }

        #endregion

        #region Sexuality
        if (rbStraight.Checked)
        {
            matchProfile.Sexuality = (int)Sexuality.Straight;
        }
        else if (rbGay.Checked)
        {
            matchProfile.Sexuality = (int)Sexuality.Gay;
        }
        else if (rbBisexual.Checked)
        {
            matchProfile.Sexuality = (int)Sexuality.Bisexual;
        }
        else if (rbUndisclosed.Checked)
        {
            matchProfile.Sexuality = (int)Sexuality.Undisclosed;
        }

        #endregion

        #region Music

        try
        {
            //matchProfile.Music = Int32.Parse(dropMusic.SelectedValue);
        }
        catch { }

        try
        {
            //matchProfile.Interests = Int32.Parse(drpinterests.SelectedValue);
        }
        catch { }

        matchProfile.Save();

        member.MatchProfile[0] = matchProfile;

        Session["Member"] = member;

        #endregion


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
            Response.Redirect("Signup.aspx");
        }

        base.OnPreInit(e);
    }
}

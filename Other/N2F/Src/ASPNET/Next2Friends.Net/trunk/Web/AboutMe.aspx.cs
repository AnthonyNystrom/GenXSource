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
using Next2Friends.Data;

public partial class AboutMe : System.Web.UI.Page
{
    public Member ViewingMember;
    public string AboutMeHTML = string.Empty;
    public string BGColor = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strWebMemberID = Request.Params["m"];

        if (strWebMemberID != null)
        {
            ViewingMember = Member.GetMemberViaWebMemberID(strWebMemberID); 
           
            if (ViewingMember != null)
            {
                if (ViewingMember.AccountType == (int)AccountType.Business)
                {

                    AboutMeHTML = ViewingMember.Business[0].EmbeddedContent.Replace("\r", "<br/>").Replace("\n", "<br/>");
                }
                else
                {
                    AboutMeHTML = ViewingMember.MemberProfile[0].EmbeddedContent.Replace("\r", "<br/>").Replace("\n", "<br/>");
                }
                
                SetColorScheme();
            }
        }
    }

    /// <summary>
    /// Sets the color scheme of the profile
    /// </summary>
    private void SetColorScheme()
    {
        MemberProfile ViewingMemberProfile = ViewingMember.GetProfile();
        ProfileScheme scheme = ProfileScheme.GetScheme(ViewingMemberProfile.ColorScheme);
        BGColor = scheme.BackgroundColor;
    }
}


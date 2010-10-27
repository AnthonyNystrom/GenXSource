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

public partial class WallPage : System.Web.UI.Page
{
    public bool IsLoggedIn;
    public int NumberOfComments;
    public string LoginUrl;
    public string DefaultNewCommentParams;
    public string PageComments;
    public Member ViewingMember;
    public Member member;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(WallPage));

        member = (Member)Session["Member"];

        if (member != null)
        {
            IsLoggedIn = true;
        }

        LoginUrl = @"/signup.aspx?u=" + Request.Url.AbsoluteUri;

        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        Comments1.ObjectId = ViewingMember.MemberID;
        Comments1.ObjectWebId = ViewingMember.WebMemberID;
        Comments1.CommentType = CommentType.Wall;
 

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Master.HTMLTitle = PageTitle.GetMWallTitle(ViewingMember);
        Master.MetaDescription = "Live mobile video broadcasting networking";
        Master.MetaKeywords = "Live mobile video, live video, mobile video, Live, live, mobile phone, mobile, phone,social, social networking";
    }
}

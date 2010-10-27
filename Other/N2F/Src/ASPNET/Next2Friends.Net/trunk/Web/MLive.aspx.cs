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

public partial class MLive : System.Web.UI.Page
{
    public Member ViewingMember;
    public Member member;
    public string EmbedLink = string.Empty;
    public string IsFriendKey = string.Empty;
    public string PhotoURL = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ResourceFile PhotoRes = new ResourceFile(ViewingMember.ProfilePhotoResourceFileID);
        PhotoURL = "http://www.next2friends.com/" + PhotoRes.FullyQualifiedURL;
        SetEmbedString();
    }

    public void SetEmbedString()
    {

        EmbedLink = @"<object width=""420"" height=""320""><param name=""flashvars"" value=""nickname=" + ViewingMember.NickName + @""" /><param name=""movie"" value=""http://services.next2friends.com/livewidget/n2flw1.swf""></param><param name=""allowFullScreen"" value=""true""></param><embed src=""http://services.next2friends.com/livewidget/n2flw1.swf"" flashvars=""nickname=" + ViewingMember.NickName + @"""  type=""application/x-shockwave-flash"" allowfullscreen=""true"" width=""420"" height=""320""></embed></object>";
    }

    private void SetFriendKey()
    {
        bool IsFriend = Friend.IsFriend(member.MemberID, ViewingMember.MemberID);

        if (IsFriend || member.MemberID == ViewingMember.MemberID)
        {
            string FToken = RijndaelEncryption.Encrypt(member.NickName);

            FToken = Server.UrlEncode(FToken);
            IsFriendKey = @"so.addVariable(""ftoken"", """ + FToken  + @""");";
        }
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        ViewingMember = ExtractPageParams.GetMember(this.Page, this.Context);

        member = (Member)Session["Member"];

        if (member != null)
        {
            if (member.MemberID == ViewingMember.MemberID)
            {
                Master.SkinID = "profile";
            }

            SetFriendKey();
        }


        base.OnPreInit(e);
    }
}

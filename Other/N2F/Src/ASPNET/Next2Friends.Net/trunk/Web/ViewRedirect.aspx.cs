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

public partial class ViewRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nickname = Request.Params["nickname"];
        string redirect = Request.Params["redirect"];

        Member member = Member.GetMemberViaNicknameNoEx(nickname);
        
        if(member!=null)
        {
            string WebMemberID = member.WebMemberID;

            if(redirect=="mvideo")
            {
                Server.Transfer("~/MVideo.aspx?m=" + WebMemberID);
            }
            else if(redirect=="mphoto")
            {
                Server.Transfer("~/MPhoto.aspx?m=" + WebMemberID);
            }
            else if(redirect=="mfriends")
            {
                Server.Transfer("~/Mfriends.aspx?m=" + WebMemberID);
            }
            else if(redirect=="wall")
            {
                Server.Transfer("~/Wall.aspx?m=" + WebMemberID);
            }
            else if (redirect == "blog")
            {
                Server.Transfer("~/Blog.aspx?m=" + WebMemberID);
            }
        }

        if (redirect == "profile")
        {
            member = (Member)Session["Member"];

            if (member != null)
            {
                Server.Transfer("~/view.aspx?m=" + member.WebMemberID);
            }

            
        }
        
    }
}

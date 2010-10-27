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

public partial class ajax_Popup : System.Web.UI.Page
{
    private Member member;

    protected void Page_Load(object sender, System.EventArgs e)
    {
        member = (Member)Session["Member"];

        if (member == null)
        {
            Utility.RememberMeLogin();
        }
        string FunctionName = Request.Params["funcname"];
        object ReturnObject = null;


        switch (FunctionName.ToLower())
        {
            case "getminiprofile":
                ReturnObject = GetMiniProfile();
                break;
            case "sendmessageshow":
                ReturnObject = SendMessageShow();
                break;
        }

        string ReturnValue = ReturnObject.ToString();
        // or we parse the value through a JSon parser and this can allow jquery to access properties

        Response.Expires = -1;
        Response.ContentType = "text/plain";
        Response.Write(ReturnValue);
        Response.End();
    }


    private string SendMessageShow()
    {
        string NickName = Request.Params["nickname"];

        string ReturnString = "this is a bunch of boxes to send a message";

        return ReturnString;
    }

    private string GetMiniProfile()
    {
        string WebMemberID = Request.Params["WebMemberID"];

        if (member == null)
        {
            return string.Empty;
        }

        string MiniProfileHTML = PopupHTML.GetMiniProgileHTML(WebMemberID, member);

        return MiniProfileHTML;
    }
}

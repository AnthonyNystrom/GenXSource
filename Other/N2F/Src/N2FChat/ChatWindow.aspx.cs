using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Next2Friends.ChatClient;

public partial class ChatWindow : System.Web.UI.Page
{
    public string otherMemberWebID;
    public string otherMemberNick;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ChatWindow));

        if (!Page.IsPostBack)
        {
            otherMemberWebID = Request.QueryString["a"];
            otherMemberNick = Request.QueryString["b"];
        }
    }

    [AjaxPro.AjaxMethod]
    public bool InitialiseServerInboxList()
    {
        Logic.GetInboxListFromServer(ASP.global_asax.ChatInboxList);
        return true;
    }

    [AjaxPro.AjaxMethod]
    public bool SendMessage(string WebMemberID, string ToMemberID, string FromNickName, string Message)
    {
        InitialiseServerInboxList();
        return Logic.SendMessage(WebMemberID, ToMemberID, FromNickName, Message);
    }


}

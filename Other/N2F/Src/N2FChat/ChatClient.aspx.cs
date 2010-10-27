using System;
using System.Threading;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.ChatClient;

public partial class ChatClient : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ChatClient));
        
    }

    [AjaxPro.AjaxMethod]
    public bool InitialiseServerInboxList()
    {
        Logic.GetInboxListFromServer(ASP.global_asax.ChatInboxList);
        return true;
    }

    [AjaxPro.AjaxMethod]
    public AjaxMember LoginToChatServer(string EmailAddress, string Password)
    {
        InitialiseServerInboxList();              
        AjaxMember member = Logic.LoginToChatServer(EmailAddress, Password);
        if (member != null)
        {
            if (Session["WebMemberID"] == null)
            {
                Session.Add("WebMemberID", member.WebMemberID);
            }
            else
            {
                Session["WebMemberID"] = member.WebMemberID;
            }
        }
        return member;
    }

    [AjaxPro.AjaxMethod]
    public bool LogoutOfChatServer(string WebMemberID)
    {
        InitialiseServerInboxList();
        return Logic.LogoutOfChatServer(WebMemberID);        
    }

    [AjaxPro.AjaxMethod]
    public List<AjaxChat> GetNewMessages(string WebMemberID)
    {
        InitialiseServerInboxList();
        return Logic.GetNewMessages(WebMemberID);
    }

    //[AjaxPro.AjaxMethod]
    //public string PollServer(string MemberID)
    //{
    //    InitialiseServerInboxList();

    //    for (int i = 0; i < 10; i++)
    //    {
    //        // byval or byref?
    //        ChatInbox inbox = Chat.GetInbox(MemberID);

    //        if (inbox.NewMessageFlag)
    //        {
    //            return Chat.GetNewMessages(MemberID);
    //            inbox.NewMessageFlag = false;
    //        }

    //        Thread.Sleep(1000);
    //    }

    //    return "";
    //}

    [AjaxPro.AjaxMethod]
    public bool SetOnlineStatus(string MemberID, string OnlineStatusString, string CustomMessage)
    {
        InitialiseServerInboxList();
        OnlineStatus newStatus = (OnlineStatus)Enum.Parse(typeof(OnlineStatus),OnlineStatusString, true);
        return Logic.SetOnlineStatus(MemberID, newStatus, CustomMessage);
    }

    [AjaxPro.AjaxMethod]
    public AjaxMember[] GetFriendsStatus(string WebMemberID)
    {
        InitialiseServerInboxList();
        return Logic.GetFriendsStatus(WebMemberID).ToArray();
    }

    [AjaxPro.AjaxMethod]
    public AjaxMember[] GetFriends(string WebMemberID)
    {
        InitialiseServerInboxList();
        //Get friends as defined in the database
        List<AjaxMember> members = Logic.GetFriends(WebMemberID);

        //Make modifications to the ChatInbox instance of the specific member
        Logic.SetUpFriends(WebMemberID,members);

        return members.ToArray();
    }

    protected void btnKillApplicationStatus_Click(object sender, EventArgs e)
    {
        ASP.global_asax.ChatInboxList = new List<ChatInbox>();
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

// <summary>
/// Every Member logged into the chat server has a single ChatInbox instance
/// When the End their chat, the instance is removed
/// </summary>
public partial class ChatInbox
{
    /// <summary>
    /// The actual MemberID of the member
    /// </summary>
    public int MemberID = 0;

    public AjaxChat ChatObject = null;//new AjaxChat();
}

public partial class ChatInbox
{
    public List<AjaxChatFriend> CFriends
    {
        get { return ChatObject.Friends; }
    }

    public List<AjaxChatMessage> CMessages
    {
        get { return ChatObject.Messages; }
    }

    public string UpdateTicket
    {
        get;
        set;
    }

    List<AjaxChatFriend> friends = new List<AjaxChatFriend>();

    List<AjaxChatMessage> messages = new List<AjaxChatMessage>();

}

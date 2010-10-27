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

/// <summary>
/// Summary description for AjaxChat
/// </summary>
public class AjaxChat
{
    private List<AjaxChatFriend> friends = new List<AjaxChatFriend>();
    private List<AjaxChatMessage> messages = new List<AjaxChatMessage>();
    private AjaxChatFriend owner = null;

    // The token will be initialized with a null
    private string token = null;

    public string Token
    {
        get { return token; }
        set { token = value; }
    }

    public List<AjaxChatFriend> Friends
    {
        get { return friends; }
    }
    
    public List<AjaxChatMessage> Messages
    {
        get { return messages; }
    }

    public AjaxChatFriend Owner
    {
        get { return owner; }
    }

    public AjaxChat(AjaxChatFriend owner)
    {
        this.owner = owner;
    }

    public AjaxChat()
    {   
    }
}

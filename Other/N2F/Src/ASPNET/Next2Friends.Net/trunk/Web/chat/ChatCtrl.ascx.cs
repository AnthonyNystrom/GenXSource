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
using System.Collections.Generic;
using System.Threading;

public partial class ChatCtrl : System.Web.UI.UserControl
{
    public string WebRoot = ASP.global_asax.WebServerRoot;
    public int ChatMode = -1;
    public int AutoLoadChatMode = -1;
    public string MyId = string.Empty;

    private List<AjaxChatMessage> Last10Messages = new List<AjaxChatMessage>();

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ChatCtrl));

        try
        {
            ChatMode = (int)Session["ChatMode"];

        }catch{}

        try
        {
            Member member = (Member)HttpContext.Current.Session["Member"];                
            MyId = member.WebMemberID;                
        }
        catch { }
        
        try
        {
            if (HttpContext.Current.Session["AutoLoadChatMode"] == null)
            {
                Member member = (Member)HttpContext.Current.Session["Member"];
                HttpContext.Current.Session["AutoLoadChatMode"] = member.MemberSettings[0].AutoLoadChatMode;
            }
        }
        catch { }

        try
        {
            AutoLoadChatMode = (int)HttpContext.Current.Session["AutoLoadChatMode"];
        }
        catch { }        
    }


    private void InitLast10Messages()
    {
        List<AjaxChatMessage> L10M = null;
        
        try
        {
            L10M = (List<AjaxChatMessage>)HttpContext.Current.Session["Last10Messages"];
        }catch{}

        if (L10M != null)
            Last10Messages = L10M;
    }

    private void PersistLast10Messages()
    {
        HttpContext.Current.Session["Last10Messages"] = Last10Messages;
    }

    private void AddMessageToLast10Message(AjaxChatMessage msg)
    {
        //Check if the message already exists
        IList<AjaxChatMessage> msgs = (from m in Last10Messages
                                       where m.ChatMessageWebID == msg.ChatMessageWebID
                                       select m).ToList<AjaxChatMessage>();

        if (msgs.Count > 0)
            return;
        
        Last10Messages.Add(msg);

        msgs = (from m in Last10Messages
                                       where m.WebMemberIDFrom == msg.WebMemberIDFrom
                                       &&
                                       m.WebMemberIDTo == msg.WebMemberIDTo
                                       orderby m.DTCreated descending
                                       select m).ToList<AjaxChatMessage>();

        for (int i = 10; i < msgs.Count; i++)
        {
            Last10Messages.Remove(msgs[i]);
        }
    }

    [AjaxPro.AjaxMethod]
    public int MarkDelivered(string ChatMessageWebID)
    {
        try
        {
            ChatMessage.MarkChatMessageDelivered(ChatMessageWebID);            
        }
        catch { }
        return 1;
    }

    /// <summary>
    /// Called from within the SendMessage method as a thread
    /// Persists the Messages to DB
    /// </summary>
    /// <param name="messages"></param>
    private void SaveMessageDB(object data)
    {
        try
        {
            List<AjaxChatMessage> msgArray = (List<AjaxChatMessage>)data;
            foreach (AjaxChatMessage msg in msgArray)
            {
                ChatMessage newMessage = new ChatMessage();
                newMessage.ChatMessageWebID = msg.ChatMessageWebID;
                newMessage.Delivered = msg.Retrieved;
                newMessage.DTCreated = msg.DTCreated;
                newMessage.MemberIDFrom = msg.MemberIDFrom;
                newMessage.WebMemberIDFrom = msg.WebMemberIDFrom;
                newMessage.MemberIDTo = msg.MemberIDTo;
                newMessage.Message = msg.Message;
                newMessage.Save();
            }
        }
        catch { }
    }

    [AjaxPro.AjaxMethod]
    public int SendMessage(string messages)
    {
        string[] msgArray = messages.Split(new char[] { ';' });
        List<AjaxChatMessage> ajaxChtMsgs = new List<AjaxChatMessage>();

        foreach (string msg in msgArray)
        {
            string []singleMsg = msg.Split(new char[]{','});

            if (singleMsg.Length < 2)
                continue;

            string WebMemberIDTo = singleMsg[0];
            string Message = singleMsg[1];
            Member member = (Member)HttpContext.Current.Session["Member"];
            int MemberID = member.MemberID;

            Member memberTo = Member.GetMemberViaWebMemberID(WebMemberIDTo);

            AjaxChatMessage ajaxMsg = new AjaxChatMessage();

            ajaxMsg.ChatMessageWebID = Next2Friends.Misc.UniqueID.NewWebID();
            ajaxMsg.Delivered = false;
            ajaxMsg.DTCreated = DateTime.Now;
            ajaxMsg.Message = Message;
            ajaxMsg.MemberIDFrom = MemberID;
            ajaxMsg.WebMemberIDFrom = member.WebMemberID;
            ajaxMsg.Retrieved = false;
            ajaxMsg.WebMemberIDTo = WebMemberIDTo;
            ajaxMsg.MemberIDTo = memberTo.MemberID;
            ajaxMsg.Outbound = 1;

            try
            {
                ChatLogic.SendMessage(ajaxMsg);
            }
            catch { }

            ajaxChtMsgs.Add(ajaxMsg);

            InitLast10Messages();
            AddMessageToLast10Message(ajaxMsg);

            PersistLast10Messages();
        }

        ParameterizedThreadStart ts = new ParameterizedThreadStart(SaveMessageDB);
        Thread t = new Thread(SaveMessageDB);
        t.Priority = ThreadPriority.BelowNormal;
        t.Start(ajaxChtMsgs);
        
        return 0;
    }

    [AjaxPro.AjaxMethod]
    public AjaxChat GetLast10Msgs(string WebFriendID)
    {
        InitLast10Messages();
        
        AjaxChat chat = new AjaxChat();

        if( WebFriendID != string.Empty )
        {
            foreach( AjaxChatMessage msg in Last10Messages )
            {
                if( msg.WebMemberIDFrom == WebFriendID || msg.WebMemberIDTo == WebFriendID )
                {
                    chat.Messages.Add(msg);
                }
            }
        }
        else
        {
            chat.Messages.AddRange(Last10Messages);
        }

        return chat;
    }


    [AjaxPro.AjaxMethod]
    public AjaxChat GetUpdate(string token)
    {
        Member member = (Member)HttpContext.Current.Session["Member"];
        AjaxChat ret = null;

        bool force = false;

        if (HttpContext.Current.Session["fr"] == null)
            HttpContext.Current.Session["fr"] = DateTime.Now;

        int mod = (((int)(DateTime.Now - (DateTime)HttpContext.Current.Session["fr"]).TotalSeconds) % 30 );

        force = mod >= 0 && mod <= 2 ;

        ret = ChatLogic.GetUpdate(member.WebMemberID, token, force);

        InitLast10Messages();
        foreach (AjaxChatMessage ajaxMsg in ret.Messages)
        {
            AddMessageToLast10Message(ajaxMsg);
        }
        PersistLast10Messages();
        
        return ret;
    }

    //public AjaxChat GetUpdate3(int fR)
    //{
    //    Member member = (Member)HttpContext.Current.Session["Member"];

    //    int MemberID = member.MemberID;
    //    AjaxChat chat = new AjaxChat();

    //    if (fR == 0)
    //    {
    //        List<ChatOnline> chatFriendsList = ChatOnline.GetChatFriendsByMemberID(MemberID);

    //        foreach (ChatOnline chatFriend in chatFriendsList)
    //        {
    //            AjaxChatFriend friend = new AjaxChatFriend();
    //            friend.WebMemberID = chatFriend.WebMemberID;
    //            friend.OnlineStatus = (OnlineStatus)chatFriend.Status;
    //            friend.FirstName = chatFriend.FirstName;
    //            friend.LastName = chatFriend.LastName;
    //            friend.NickName = chatFriend.NickName;
    //            friend.Email = chatFriend.Email;
    //            friend.CustomMessage = chatFriend.CustomMessage;
    //            friend.AvatorUrl = ParallelServer.Get(chatFriend.AvatorUrl) + @"user/" + chatFriend.AvatorUrl;

    //            if (DateTime.Now.Subtract(chatFriend.LastCommDt).TotalSeconds > 60)
    //            {
    //                friend.OnlineStatus = OnlineStatus.Offline;
    //            }

    //            friend.OnlineStatusString = friend.OnlineStatus.ToString();

    //            chat.Friends.Add(friend);
    //        }
    //    }

    //    ChatOnline currentMember = ChatOnline.GetChatOnlineByMemberID(MemberID);

    //    // Update time in ChatOnline
    //    if (currentMember == null)
    //    {
    //        currentMember = new ChatOnline();
    //        currentMember.MemberID = MemberID;
    //        currentMember.Status = 1;
    //    }

    //    currentMember.LastCommDt = DateTime.Now;
    //    currentMember.Save();

    //    List<ChatMessage> messageList = ChatMessage.GetNewChatMessageByMemberID(MemberID);
    //    InitLast10Messages();

    //    foreach (ChatMessage msg in messageList)
    //    {
    //        AjaxChatMessage ajaxMsg = new AjaxChatMessage();

    //        ajaxMsg.ChatMessageWebID = msg.ChatMessageWebID;
    //        ajaxMsg.Delivered = msg.Delivered;
    //        ajaxMsg.DTCreated = msg.DTCreated;
    //        ajaxMsg.Message = msg.Message;
    //        ajaxMsg.MemberIDFrom = msg.MemberIDFrom;
    //        ajaxMsg.WebMemberIDFrom = msg.WebMemberIDFrom;

    //        ajaxMsg.Outbound = 0;

    //        chat.Messages.Add(ajaxMsg);

    //        AddMessageToLast10Message(ajaxMsg);

    //        MarkDelivered(ajaxMsg.ChatMessageWebID);
    //    }

    //    PersistLast10Messages();

    //    return chat;
    //}

    //[AjaxPro.AjaxMethod]
    //public AjaxChat GetUpdate2(int fR)
    //{
    //    Member member = (Member)HttpContext.Current.Session["Member"];
    //    int MemberID = member.MemberID;
    //    AjaxChat chat = new AjaxChat();

    //    if (fR == 0)
    //    {
    //        List<ChatOnline> chatFriendsList = ChatOnline.GetChatFriendsByMemberID(MemberID);

    //        foreach (ChatOnline chatFriend in chatFriendsList)
    //        {
    //            AjaxChatFriend friend = new AjaxChatFriend();
    //            friend.WebMemberID = chatFriend.WebMemberID;
    //            friend.OnlineStatus = (OnlineStatus)chatFriend.Status;
    //            friend.FirstName = chatFriend.FirstName;
    //            friend.LastName = chatFriend.LastName;
    //            friend.NickName = chatFriend.NickName;
    //            friend.Email = chatFriend.Email;
    //            friend.CustomMessage = chatFriend.Email;
    //            friend.AvatorUrl = ParallelServer.Get(chatFriend.AvatorUrl) + @"user/" + chatFriend.AvatorUrl;

    //            if (DateTime.Now.Subtract(chatFriend.LastCommDt).TotalSeconds > 60)
    //            {
    //                friend.OnlineStatus = OnlineStatus.Offline;
    //            }

    //            friend.OnlineStatusString = friend.OnlineStatus.ToString();

    //            chat.Friends.Add(friend);
    //        }
    //    }

    //    ChatOnline currentMember = ChatOnline.GetChatOnlineByMemberID(MemberID);

    //    // Update time in ChatOnline
    //    if (currentMember == null)
    //    {
    //        currentMember = new ChatOnline();
    //        currentMember.MemberID = MemberID;
    //        currentMember.Status = 1;
    //    }

    //    currentMember.LastCommDt = DateTime.Now;
    //    currentMember.Save();

    //    List<ChatMessage> messageList = ChatMessage.GetNewChatMessageByMemberID(MemberID);
    //    InitLast10Messages();

    //    foreach (ChatMessage msg in messageList)
    //    {
    //        AjaxChatMessage ajaxMsg = new AjaxChatMessage();
            
    //        ajaxMsg.ChatMessageWebID = msg.ChatMessageWebID;
    //        ajaxMsg.Delivered = msg.Delivered;
    //        ajaxMsg.DTCreated = msg.DTCreated;
    //        ajaxMsg.Message = msg.Message;
    //        ajaxMsg.MemberIDFrom = msg.MemberIDFrom;
    //        ajaxMsg.WebMemberIDFrom = msg.WebMemberIDFrom;

    //        ajaxMsg.Outbound = 0;

    //        chat.Messages.Add(ajaxMsg);

    //        AddMessageToLast10Message(ajaxMsg);

    //        MarkDelivered(ajaxMsg.ChatMessageWebID);
    //    }

    //    PersistLast10Messages();

    //    return chat;
    //}

    [AjaxPro.AjaxMethod]
    public int Login(int x,bool rem)
    {
        Member member = (Member)HttpContext.Current.Session["Member"];
        HttpContext.Current.Session["fr"] = DateTime.Now;

        try
        {
            ChatLogic.Login(member);
        }
        catch { }

        if (member != null)
        {
            try
            {   
                HttpContext.Current.Session["LoggedIntoChat"] = true;
                HttpContext.Current.Session["ChatMode"] = x;

                // persist autoload settings
                if (rem)
                {
                    HttpContext.Current.Session["AutoLoadChatMode"] = x;
                    MemberSettings ms = member.MemberSettings[0];
                    ms.AutoLoadChatMode = x;
                    ms.Save();
                }                
            }
            catch
            {
                try
                {
                    ChatLogic.Logoff(member.WebMemberID);
                }
                catch { }
            }

            return 0;
        }

        else return -1;
    }

    [AjaxPro.AjaxMethod]
    public int Logout()
    {
        try
        {
            HttpContext.Current.Session["LoggedIntoChat"] = false;
            HttpContext.Current.Session["ChatMode"] = -1;

            HttpContext.Current.Session["AutoLoadChatMode"] = -1;

            Member member = (Member)HttpContext.Current.Session["Member"];

            try
            {
                ChatLogic.Logoff(member.WebMemberID);
            }
            catch { }

            MemberSettings ms = member.MemberSettings[0];
            ms.AutoLoadChatMode = -1;
            ms.Save();        
        

        }
        catch { }

        return 0;
    }
}

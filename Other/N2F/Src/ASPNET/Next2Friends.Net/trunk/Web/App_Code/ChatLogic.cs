using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Next2Friends.Data;
using System.Collections;
using System.Threading;

public class ChatLogic
{
    //System.Collections.Generic.SynchronizedKeyedCollection
    private static Hashtable CD = new Hashtable();
    private static Hashtable ChatData = Hashtable.Synchronized(CD);

    public static void ChatLoop()
    {
        while (true)
        {
            Thread.Sleep(250);
        }
    }

    public static void Login(Member member)
    {
        AjaxChatFriend owner = new AjaxChatFriend();
        owner.CustomMessage = member.MemberProfile[0].TagLine;
        owner.Email = member.Email;
        owner.FirstName = member.FirstName;
        owner.LastName = member.LastName;
        owner.NickName = member.NickName;
        owner.OnlineStatus = OnlineStatus.Online;
        owner.OnlineStatusString = OnlineStatus.Online.ToString();
        owner.WebMemberID = member.WebMemberID;
        owner.LastCommDt = DateTime.Now;

        ResourceFile PhotoRes = new ResourceFile(member.ProfilePhotoResourceFileID);
        owner.AvatorUrl = ParallelServer.Get(PhotoRes.FullyQualifiedURL) + PhotoRes.FullyQualifiedURL; ;

        AjaxChat c = new AjaxChat(owner);

        c = AddToChatData(member.WebMemberID, c);
        c.Owner.OnlineStatus = OnlineStatus.Online;
        c.Token = Next2Friends.Misc.UniqueID.NewWebID();


        FillFriends(member);

        GetMessagesFromDB(member);
    }

    private static AjaxChat AddToChatData(string WebMemberID, AjaxChat c)
    {
        if (!ChatData.Contains(WebMemberID))
        {
            ChatData.Add(WebMemberID, c);
            return c;
        }
        else
        {
            return UpdateChatObject(WebMemberID, c);            
        }
    }

    private static void FillFriends(Member member)
    {
        AjaxChat c = GetChatObject(member.WebMemberID);

        // Clear friend list before refreshing it.
        c.Friends.Clear();        

        List<ChatOnline> chatFriendsList = ChatOnline.GetChatFriendsByMemberID(member.MemberID);

        foreach (ChatOnline chatFriend in chatFriendsList)
        {
            AjaxChat chat = GetChatObject(chatFriend.WebMemberID);

            if (chat == null)
            {
                AjaxChatFriend ajaxChatFriend = new AjaxChatFriend();
                ajaxChatFriend.WebMemberID = chatFriend.WebMemberID;
                ajaxChatFriend.OnlineStatus = (OnlineStatus)chatFriend.Status;
                ajaxChatFriend.FirstName = chatFriend.FirstName;
                ajaxChatFriend.LastName = chatFriend.LastName;
                ajaxChatFriend.NickName = chatFriend.NickName;
                ajaxChatFriend.Email = chatFriend.Email;
                ajaxChatFriend.CustomMessage = chatFriend.CustomMessage;
                ajaxChatFriend.AvatorUrl = ParallelServer.Get(chatFriend.AvatorUrl) + @"user/" + chatFriend.AvatorUrl;

                chat = new AjaxChat(ajaxChatFriend);
            }

            chat = AddToChatData(chatFriend.WebMemberID, chat);
            chat.Token = Next2Friends.Misc.UniqueID.NewWebID();

            c.Friends.Add( chat.Owner );
        }

    }

    private static void GetMessagesFromDB(Member member)
    {
        List<ChatMessage> messageList = ChatMessage.GetNewChatMessageByMemberID(member.MemberID);        

        foreach (ChatMessage msg in messageList)
        {
            AjaxChatMessage ajaxMsg = new AjaxChatMessage();

            ajaxMsg.ChatMessageWebID = msg.ChatMessageWebID;
            ajaxMsg.Delivered = msg.Delivered;
            ajaxMsg.DTCreated = msg.DTCreated;
            ajaxMsg.Message = msg.Message;
            ajaxMsg.MemberIDFrom = msg.MemberIDFrom;
            ajaxMsg.WebMemberIDFrom = msg.WebMemberIDFrom;
            ajaxMsg.MemberIDTo = member.MemberID;
            ajaxMsg.Outbound = 0;
            ajaxMsg.Retrieved = msg.Delivered;
            ajaxMsg.WebMemberIDTo = member.WebMemberID;
            SendMessage(ajaxMsg);
        }
    }

    public static AjaxChat GetUpdate(string WebMemberID, string token, bool force)
    {
        AjaxChat c = GetChatObject(WebMemberID);
        c.Owner.LastCommDt = DateTime.Now;

        string webMessageIds = "";

        // The token has not changed since last fetch so no changes
        if (c.Token == token && !force )
        {
            return null;
        }

        AjaxChat retObject = new AjaxChat();
        retObject.Token = c.Token;

        retObject.Friends.AddRange(c.Friends);

        IList<AjaxChatMessage> msgs = (from m in c.Messages
                                             where m.Retrieved == false
                                             select m).ToList<AjaxChatMessage>();

        foreach (AjaxChatMessage msg in msgs)
        {
            c.Messages.Remove(msg);
            //msg.Retrieved = true;
            webMessageIds += msg.ChatMessageWebID + ",";
            //ChatMessage.MarkChatmMessageDelivered(msg.ChatMessageWebID);
            retObject.Messages.Add(msg);
        }



        if (webMessageIds != null && webMessageIds.Length > 0)
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart(MarkChatMessageDelivered);
            Thread t = new Thread(ts);
            t.Priority = ThreadPriority.BelowNormal;
            t.Start(webMessageIds);
        }

        //retObject.Messages.AddRange(msgs);

        return retObject;
    }

    public static void MarkChatMessageDelivered(object data)
    {
        try
        {
            string ChatMessageWebIDs = (string)data;
            ChatMessage.MarkChatMessageDelivered(ChatMessageWebIDs);
        }
        catch { }
    }    

    public static void SendMessage(AjaxChatMessage message )
    {
        AjaxChat c = GetChatObject(message.WebMemberIDTo);
        c.Messages.Add(message);
        c.Token = Next2Friends.Misc.UniqueID.NewWebID();
    }

    public static void Logoff(string WebMemberID)
    {
        AjaxChat c = GetChatObject(WebMemberID);

        if (c != null)
        {
            RemoveChatObject(WebMemberID);

            c.Messages.Clear();
            c.Friends.Clear();
            c = null;
        }
    }

    public static AjaxChat UpdateChatObject(string WebMemberID,AjaxChat updateInfo)
    {
        AjaxChat c = GetChatObject(WebMemberID);
        
        c.Owner.AvatorUrl = updateInfo.Owner.AvatorUrl;
        c.Owner.CustomMessage = updateInfo.Owner.CustomMessage;
        c.Owner.FirstName = updateInfo.Owner.FirstName;
        c.Owner.LastName = updateInfo.Owner.LastName;

        return c;
    }

    public static AjaxChat GetChatObject(string WebMemberID)
    {
        AjaxChat c = null;
        if (ChatData.Contains(WebMemberID))
        {
            c = (AjaxChat)ChatData[WebMemberID];         
        }

        return c;
    }

    public static void RemoveChatObject(string WebMemberID)
    {
        try
        {
            ChatData.Remove(WebMemberID);
        }
        catch { }
    }
}

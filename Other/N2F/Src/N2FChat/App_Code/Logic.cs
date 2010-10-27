using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Next2Friends.Data;
using System.Collections;

namespace Next2Friends.ChatClient
{
    public class Logic
    {
        /// <summary>
        /// The list containing all the members that are currently online now
        /// </summary>
        public static List<ChatInbox> ChatInboxList; // this would be stored in an application session and passed in as a parameter on each page execution

        public static void GetInboxListFromServer(List<ChatInbox> ChatInboxList)
        {
            // the chatinbox list is a single array stored on the server and is accessible to all applications (even across loadbalanced servers)
            // this enables a single list of all users and their inboxes that any web application can access to send or receive a mesage
            // it also acts as a register to show who is currently online
            Logic.ChatInboxList = ChatInboxList;
        }

        #region Start/End Chat


        public static Next2Friends.Data.Member Login(string EmailAddress, string Password)
        {
            Next2Friends.Data.Member member = Next2Friends.Data.Member.MemberLogin(EmailAddress, Password);

            //Get new messages from DB and put them into the server memory
            try
            {
                List<AjaxChat> chatList = GetNewMessagesDB(member.WebMemberID);                

                foreach (AjaxChat chat in chatList)
                {
                    Member otherMember = new Member(chat.OtherMemberID);
                    SendMessageLive(otherMember.WebMemberID,member.WebMemberID, otherMember.NickName, chat.Message, chat.ChatWebID);
                }
            }
            catch{}
            return member;            
        }

        /// <summary>
        /// Starts a chat session by creating an inbox instance
        /// </summary>
        /// <param name="MemberID"></param>
        public static AjaxMember LoginToChatServer(string EmailAddress,string Password)
        {
            Next2Friends.Data.Member member = Login( EmailAddress,Password);

            AjaxMember m = new AjaxMember();
            m.WebMemberID = member.WebMemberID;
            m.FirstName = member.FirstName;
            m.LastName = member.LastName;
            m.NickName = member.NickName;
            m.Email = member.Email;
            m.OnlineStatus = OnlineStatus.Online;

            ChatInbox inbox = GetInbox(m.WebMemberID);            

            // if no existing instances exist then create one
            if (inbox == null)
            {               
                //log the user in
                inbox = new ChatInbox();
                inbox.MemberID = member.MemberID;
                inbox.MemberInfo = m;
                ChatInboxList.Add(inbox);
            }
            else
            {
                inbox.MemberInfo = m;
            }

            return m;
        }

        /// <summary>
        /// Sets the members online status, passing in a custom string if the member desires
        /// </summary>
        /// <param name="MemberID">The memberId to change the status of</param>
        /// <param name="OnlineStatus">The new status</param>
        /// <param name="CustomMessage">Optional custom message</param>
        public static bool SetOnlineStatus(string WebMemberID, OnlineStatus OnlineStatus, string CustomMessage)
        {
           ChatInbox MemberInbox = GetInbox( WebMemberID );

            // check if the user is in the list (not logged in? timed out?)
            if (MemberInbox != null )
            {
                MemberInbox.MemberInfo.OnlineStatus = OnlineStatus;
                MemberInbox.MemberInfo.CustomMessage = CustomMessage;

                return true;
            }

            return false;
        }

        /// <summary>
        /// End the chat session by removing the inbox instance from the list
        /// </summary>
        /// <param name="MemberID">Ench chat MemberID</param>
        public static bool LogoutOfChatServer(string WebMemberID)
        {
            // select the inbox instance from the memberID
            ChatInbox inbox = GetInbox(WebMemberID);           
            ChatInboxList.Remove(inbox);
            
            return true;
        }

        #endregion End Chat

        public static ChatInbox GetInbox(string MemberID)
        {
            var MemberInbox =
            from i in ChatInboxList
            where i.MemberInfo.WebMemberID == MemberID
            select i;

            // If Member has logged in
            if (MemberInbox.Count() == 1)
            {
                // member should only have one inbox, get the first
                return MemberInbox.First();
            }

            return null;
        }

        /// <summary>
        /// gets all the new messages that are on the server and sets them to delivered satus: true
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static List<AjaxChat> GetNewMessagesLive(string WebMemberID)
        {
            ChatInbox inbox = GetInbox(WebMemberID);            
            List<AjaxChat> retChatMessages = new List<AjaxChat>();
            
            if (inbox == null)
                return retChatMessages;

            var chatMessages =
            from i in inbox.ChatMessages
            where i.Delivered == false
            select i;

            ArrayList ChatWebIDs = new ArrayList();

            foreach( AjaxChat chat in chatMessages )
            {
                chat.Delivered = true;
                retChatMessages.Add( chat );
                ChatWebIDs.Add( chat.ChatWebID);
            }

            MarkDBChatAsRead(ChatWebIDs);

            return retChatMessages;
        }

        public static void MarkDBChatAsRead(ArrayList ChatWebIDs)
        {
            string spParam = string.Empty;
            for (int i = 0; i < ChatWebIDs.Count; i++)
            {
                Chat.MarkChatRead((string)ChatWebIDs[i]);
                //spParam += ChatWebIDs[i];

                //if (i < ChatWebIDs.Length - 1)
                //    spParam += ",";
            }
        }

        public static List<AjaxChat> GetNewMessages(string WebMemberID)
        {
            return GetNewMessagesLive(WebMemberID) ;
        }

        /// <summary>
        /// gets all the new messages that are on the server and sets them to delivered satus: true
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        public static List<AjaxChat> GetNewMessagesDB(string WebMemberID)
        {
            ChatInbox inbox = GetInbox(WebMemberID);            
            List<Next2Friends.Data.Chat> chatMessages = null;
            List<AjaxChat> retChatMessages = new List<AjaxChat>();

            if (inbox == null)
                return retChatMessages;

            chatMessages = Next2Friends.Data.Chat.GetNewChats(inbox.MemberID);
            foreach (Next2Friends.Data.Chat c in chatMessages)
            {
                AjaxChat ajaxC = new AjaxChat();
                ajaxC.ChatWebID = c.ChatWebID;
                ajaxC.DTCreated = c.DTCreated;
                ajaxC.OtherMemberID = c.MemberIDFrom;                    
                ajaxC.Message = c.Message;

                retChatMessages.Add(ajaxC);
            }

            return retChatMessages;
        }

        public static bool SendMessage(string WebMemberID, string WebMemberIDTo, string FromNickName, string Message)
        {
            ChatInbox senderInbox = GetInbox(WebMemberID);
            Member recieverMember = Member.GetMemberViaWebMemberID(WebMemberIDTo);
            Chat chatMessage = SendMessageDB(senderInbox.MemberID, recieverMember.MemberID, FromNickName, Message);
            bool ret = SendMessageLive(WebMemberID, WebMemberIDTo, FromNickName, Message,chatMessage.ChatWebID);
            return ret;
        }
        /// <summary>
        /// Sends a message to a user
        /// </summary>
        /// <param name="WebMemberID">The member posting the message</param>
        /// <param name="ToMemberID">The receiver of the message</param>        
        /// <param name="Message">The text content fo the message</param>
        /// <returns>Returns a chat object</returns>
        public static Next2Friends.Data.Chat SendMessageDB(int MemberIDFrom, int MemberIDTo, string FromNickName, string Message)
        {   
            Next2Friends.Data.Chat m = new Next2Friends.Data.Chat();
            m.ChatWebID = GetGUID();
            m.DTCreated = DateTime.Now;
            m.Delivered = false;
            m.MemberIDFrom = MemberIDFrom;
            m.MemberIDTo = MemberIDTo;
            m.Message = Message;
            m.Save();            
            return m;
        }

        /// <summary>
        /// Sends a message to a user
        /// </summary>
        /// <param name="WebMemberID">The member posting the message</param>
        /// <param name="ToMemberID">The receiver of the message</param>        
        /// <param name="Message">The text content fo the message</param>
        /// <returns>Whether the message was successfully sent</returns>        
        public static bool SendMessageLive(string WebMemberID, string MemberIDTo, string FromNickName, string Message,string GUID)
        {
            ChatInbox senderInbox = GetInbox(WebMemberID);
            ChatInbox MemberInbox = GetInbox(MemberIDTo);

            // check if the user is in the list (not logged in? timed out?)
            if (MemberInbox != null)
            {
                AjaxChat ajaxC = new AjaxChat();
                if( GUID == null)
                    ajaxC.ChatWebID = GetGUID();
                else
                    ajaxC.ChatWebID = GUID;                
                ajaxC.DTCreated = DateTime.Now;
                ajaxC.OtherMemberWebID = WebMemberID;
                ajaxC.OtherMemberNick = FromNickName;
                ajaxC.Message = Message;
                ajaxC.Delivered = false;

                MemberInbox.ChatMessages.Add(ajaxC);                

                return true;
            }

            return false;
        }

        /// <summary>
        /// returns a list of all the members friends with their status.
        /// </summary>
        /// <param name="MemberID">The memberId of the requester</param>        
        /// <returns>A list of AjaxMember objects</returns>
        public static List<AjaxMember> GetFriendsStatus(string WebMemberID)
        {  
            //Select the member inbox according to the MemberID
            // This can be done more efficiently using LINQ in ORCASE Beta 2
            //var FriendsOnline =
            //from j in ChatInboxList
            //where j.MemberID ( 1 )
            //select j; // get all chat members for prototype

            List<AjaxMember> retMembers = new List<AjaxMember>();
            ChatInbox memberInbox = GetInbox( WebMemberID );

            if (memberInbox == null)
                return retMembers;

            List<AjaxMember> Friends = memberInbox.Friends;            

            foreach (AjaxMember member in Friends)
            {
                ChatInbox inbox = GetInbox(member.WebMemberID);

                if (inbox != null)
                {
                    retMembers.Add(inbox.MemberInfo);
                }
            }

            return retMembers;
        }

        public static bool AddFriend(string WebMemberID, string TargetEmail)
        {
            ChatInbox inbox = GetInbox(WebMemberID);

            Next2Friends.Data.Member targetMember = Next2Friends.Data.Member.GetMemberByEmail(TargetEmail);
            if (targetMember == null)
                return false;

            Next2Friends.Data.Friend f = new Next2Friends.Data.Friend();
            f.MemberID1 = inbox.MemberID;
            f.MemberID2 = targetMember.MemberID;
            f.Save();

            return true;
        }


        /// <summary>
        /// returns a list off all the members friends.
        /// </summary>
        /// <param name="MemberID">The MemberId of the requester</param>        
        /// <returns>A list of AjaxMember</returns>
        public static List<AjaxMember> GetFriends(string WebMemberID)
        {
            ChatInbox inbox = GetInbox(WebMemberID);
            List<AjaxMember> retMembers = new List<AjaxMember>();            

            if (inbox == null)
                return retMembers;

            List<Next2Friends.Data.Member> members =  Next2Friends.Data.Member.GetAllFriendsByMemberID(inbox.MemberID);

            foreach (Next2Friends.Data.Member member in members)
            {
                AjaxMember m = new AjaxMember();
                m.WebMemberID = member.WebMemberID;
                m.FirstName = member.FirstName;
                m.LastName = member.LastName;
                m.NickName = member.NickName;
                m.Email = member.Email;

                retMembers.Add(m);
            }


            return retMembers; 
        }

        /// <summary>
        /// returns a list off all the members friends.
        /// </summary>
        /// <param name="MemberID">The MemberId of the requester</param>                
        /// <param name="Friends">A list of friends</param>
        public static void SetUpFriends(string WebMemberID,List<AjaxMember> Friends)
        {
            ChatInbox inbox = GetInbox(WebMemberID);           

            if (inbox == null)
                return;

            inbox.Friends = Friends;
        }

        private static string GetGUID()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }



    
}
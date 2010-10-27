using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;
using Next2Friends.Misc;

public partial class InboxPage : System.Web.UI.Page
{
    public static int PageSize = 10;

    public string URLRedirect = string.Empty;

    public bool InitialSendValue = false;
    public bool InitialForwardValue = false;
    public string InitialSendNickName;
    public string InitialForwardText;

    public string PassKey = string.Empty;
    public string WebMessageID = string.Empty;
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSendToID = Request.Params["s"];
        string strWebForwardID = Request.Params["f"];
        string strRedirectURL = Request.Params["r"];
        PassKey = Request.Params["p"];
        WebMessageID = Request.Params["w"];

        if (strSendToID != null)
        {
            Member InitialSendMember = Member.GetMemberViaWebMemberID(strSendToID);

            if (InitialSendMember != null)
            {
                InitialSendNickName = InitialSendMember.NickName;
                InitialSendValue = true;
            }
        }
        else if (strWebForwardID != null)
        {
            Member InitialSendMember = Member.GetMemberViaWebMemberID(strWebForwardID);

            if (InitialSendMember != null)
            {
                InitialForwardText = "Check out this person&#39;s profile " + ASP.global_asax.WebServerRoot + "/?n=" + InitialSendMember.NickName;
                InitialForwardValue = true;
            }

        }

        if (strRedirectURL != null)
        {
            URLRedirect = Server.HtmlDecode(strRedirectURL);
        }
        AjaxPro.Utility.RegisterTypeForAjax(typeof(InboxPage));
    }

    /// <summary>
    /// Returns all new messsages via an Ajax call. This method is not to be confused LoadInitialInboxContent().
    /// This method is for browser calls only
    /// </summary>
    /// <returns>An array of AjaxMessages</returns>
    [AjaxPro.AjaxMethod]
    public AjaxMessage[] GetInboxMessages2()
    {
        Member member = (Member)Session["Member"];
        AjaxMessage[] AjaxMessages = new AjaxMessage[0];

        if (member != null)
        {
            Message[] NewMessages = Message.GetAllMessagesByMemberID(member.MemberID, 0, PageSize);
            AjaxMessages = AjaxMessage.ConvertToAjaxMessage(member,NewMessages, true, GetMessageType.Inbox);
        }

        return AjaxMessages;
    }

    /// <summary>
    /// Returns all new messsages via an Ajax call. This method is not to be confused LoadInitialInboxContent().
    /// This method is for browser calls only
    /// </summary>
    /// <returns>An array of AjaxMessages</returns>
    [AjaxPro.AjaxMethod]
    public AjaxMessage[] GetNewMessages()
    {
        Member member = (Member)Session["Member"];
        AjaxMessage[] AjaxMessages = new AjaxMessage[0];

        if (member != null)
        {
            Message[] NewMessages = Message.GetNewMessages(member.MemberID);
            AjaxMessages = AjaxMessage.ConvertToAjaxMessage(member,NewMessages, true, GetMessageType.Inbox);
        }

        return AjaxMessages;
    }

    [AjaxPro.AjaxMethod]
    public AjaxMessageList GetInboxMessages(int Page)
    {
        AjaxMessageList ajaxMessageList = new AjaxMessageList();

        try
        {
            Member member = (Member)Session["Member"];

            if (member != null)
            {
                Message[] NewMessages = Message.GetAllMessagesByMemberID(member.MemberID, Page, PageSize);

                int MessageCount = Message.GetMessageCount(member.MemberID);
                int NumberOfPages = (int)Math.Ceiling(Convert.ToDouble(MessageCount / PageSize));

                if ((MessageCount % PageSize) > 0 || NumberOfPages == 0)
                    NumberOfPages = NumberOfPages + 1;

                ajaxMessageList.AjaxMessages = AjaxMessage.ConvertToAjaxMessage(member,NewMessages, true, GetMessageType.Inbox);
                ajaxMessageList.NumberOfPages = NumberOfPages;
                ajaxMessageList.CurrentPage = Page;                
            }
        }
        catch (Exception ex)
        {
            int i = 100;
        }

        return ajaxMessageList;
    }

    [AjaxPro.AjaxMethod]
    public string[] DeleteMessages(string[] WebMessageIDList, bool EmtpyTrash)
    {
        Member member = (Member)Session["Member"];

        string StringList = string.Empty;

        for (int i = 0; i < WebMessageIDList.Length; i++)
        {
            if (!Next2Friends.Misc.UniqueID.IsWebID(WebMessageIDList[i]))
            {
                throw new Exception("not a valid WebMessageID");
            }

            StringList += "'" + WebMessageIDList[i] + "'" + ((i != WebMessageIDList.Length - 1) ? "," : string.Empty);
        }

        Message.DeleteChatMessageList(member.MemberID, StringList, EmtpyTrash);

        return WebMessageIDList;
    }


    [AjaxPro.AjaxMethod]
    public string[] DeleteSentMessages(string[] WebMessageIDList, bool EmtpyTrash)
    {
        Member member = (Member)Session["Member"];

        string StringList = string.Empty;

        for (int i = 0; i < WebMessageIDList.Length; i++)
        {
            if (!Next2Friends.Misc.UniqueID.IsWebID(WebMessageIDList[i]))
            {
                throw new Exception("not a valid WebMessageID");
            }

            StringList += "'" + WebMessageIDList[i] + "'" + ((i != WebMessageIDList.Length - 1) ? "," : string.Empty);
        }

        Message.DeleteSentMessageList(member.MemberID, StringList, EmtpyTrash);

        return WebMessageIDList;
    }

    [AjaxPro.AjaxMethod]
    public AjaxMessageList GetTrash(int Page)
    {
        AjaxMessageList ajaxMessageList = new AjaxMessageList();

        try
        {
            Member member = (Member)Session["Member"];


            if (member != null)
            {
                int MessageCount = Message.GetTrashMessageCount(member.MemberID);
                int NumberOfPages = (int)Math.Ceiling(Convert.ToDouble(MessageCount / PageSize));

                if ((MessageCount % PageSize) > 0 || NumberOfPages == 0)
                    NumberOfPages = NumberOfPages + 1;

                Message[] NewMessages = Message.GetTrash(member.MemberID, Page, PageSize);
                ajaxMessageList.AjaxMessages = AjaxMessage.ConvertToAjaxMessage(member,NewMessages, true, GetMessageType.Trash);
                ajaxMessageList.NumberOfPages = NumberOfPages;
                ajaxMessageList.CurrentPage = Page;
            }
        }
        catch (Exception ex)
        {   
        }

        return ajaxMessageList;
    }

    [AjaxPro.AjaxMethod]
    public AjaxMessageList GetSent(int Page)
    {
        AjaxMessageList ajaxMessageList = new AjaxMessageList();

        try
        {
            Member member = (Member)Session["Member"];


            if (member != null)
            {
                int MessageCount = Message.GetSentMessageCount(member.MemberID);
                int NumberOfPages = (int)Math.Ceiling(Convert.ToDouble(MessageCount / PageSize));

                if ((MessageCount % PageSize) > 0 || NumberOfPages == 0)
                    NumberOfPages = NumberOfPages + 1;

                Message[] NewMessages = Message.GetSent(member.MemberID, Page, PageSize);
                ajaxMessageList.AjaxMessages = AjaxMessage.ConvertToAjaxMessage(member,NewMessages, true, GetMessageType.Trash);
                ajaxMessageList.NumberOfPages = NumberOfPages;
                ajaxMessageList.CurrentPage = Page;
            }
        }
        catch (Exception ex)
        {
        }

        return ajaxMessageList;
    }

    /// <summary>
    /// returns a AjaxMessage and chain of reply headers
    /// </summary>
    /// <param name="MessageID">The Message ID</param>
    /// <returns>The AjaxMethod</returns>
    [AjaxPro.AjaxMethod]
    public AjaxMessageList OpenMessage(string WebMessageID,string PassKey)
    {
        Member member = (Member)Session["Member"];

        AjaxMessage[] ajaxMessages = null;

        Message[] messages = null;

        if (member != null)
        {
            // get the message and all other message that have the same InReplyToID
            messages = Message.GetMessageHeaderWithReply(WebMessageID, member.MemberID);
        }
        else
        {
            // Get external message to display
            messages = Message.GetExternalMessageHeader(WebMessageID, PassKey);
        }

        ajaxMessages = AjaxMessage.ConvertToAjaxMessage(member, messages, false, GetMessageType.ReadMessage);

        AjaxMessageList messageList = new AjaxMessageList();

        messageList.DefaultWebMessageID = WebMessageID;

        //get the video message of the first item
        if (ajaxMessages.Length > 0)
        {
            for (int i = 0; i < ajaxMessages.Length; i++)
            {
                if (ajaxMessages[i].WebMessageID == WebMessageID)
                {
                    messageList.DefaultVideoMessageFile = ajaxMessages[i].VideoMessageFile;
                }
            }
        }

        messageList.AjaxMessages = ajaxMessages;

        if( member != null )
            messageList.NumberOfNewMessages = member.GetNewMessageCount();

        return messageList;
    }

    /// <summary>
    /// returns a AjaxMessage and chain of reply headers
    /// </summary>
    /// <param name="MessageID">The Message ID</param>
    /// <returns>The AjaxMethod</returns>
    [AjaxPro.AjaxMethod]
    public AjaxMessageList OpenSentMessage(string WebMessageID)
    {
        Member member = (Member)Session["Member"];

        AjaxMessage[] ajaxMessages = null;

        if (member != null)
        {
            // get the message and all other message that have the same InReplyToID
            Message[] messages = Message.GetSentMessageHeaderWithReply(WebMessageID, member.MemberID);

            ajaxMessages = AjaxMessage.ConvertToAjaxMessage(member,messages, false, GetMessageType.ReadMessage);
        }

        AjaxMessageList messageList = new AjaxMessageList();

        messageList.DefaultWebMessageID = WebMessageID;

        //get the video message of the first item
        if (ajaxMessages.Length > 0)
        {
            for (int i = 0; i < ajaxMessages.Length; i++)
            {
                if (ajaxMessages[i].WebMessageID == WebMessageID)
                {
                    messageList.DefaultVideoMessageFile = ajaxMessages[i].VideoMessageFile;
                }
            }
        }

        messageList.AjaxMessages = ajaxMessages;

        return messageList;
    }

    [AjaxPro.AjaxMethod]
    public int QuickSend(string Recipient, string MessageBody, string WebMessageID)
    {
        Member member = (Member)Session["Member"];

        // determine if its an email or an n2f member
        // if not a member check the nickname field in the member table
        Member recipient = Member.GetMemberViaNicknameNoEx(Recipient);
        Message InReplyToMessage = Message.GetMessageWithJoin(WebMessageID, member.MemberID);

        if (recipient == null)
        {
            return (int)MessageSendResponse.BadAddress;
        }


        Message message = new Message();

        message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
        message.VideoMessageToken = string.Empty;
        message.InReplyToID = InReplyToMessage.InReplyToID;
        message.MemberIDFrom = member.MemberID;
        message.MemberIDTo = recipient.MemberID;
        message.Body = MessageBody;

        DateTime DTCreated = DateTime.Now;

        message.DTCreated = DTCreated;
        message.Save();


        if (message.MessageID > 0)
            return (int)MessageSendResponse.Sent;
        else
            return (int)MessageSendResponse.UnexpectedError;
    }

    /// <summary>
    /// Sends a message to another member via ajax call
    /// </summary>
    /// <param name="Recipient">The person to receive the message</param>
    /// <param name="MessageBody">The message body (1000 characters)</param>
    /// <returns>The success of the message send</returns>
    [AjaxPro.AjaxMethod]
    public int SendMessage(string Recipient, string MessageBody, string VMToken)
    {
        Member member = (Member)Session["Member"];

        // determine if its an email or an n2f member
        // if not a member check the nickname field in the member table
        Member MemberRecipient = Member.GetMemberViaNicknameNoEx(Recipient);

        //member.HasBlockedMember(MemberRecipient);

        if (Recipient == string.Empty)
        {
            return (int)MessageSendResponse.BadAddress;
        }
        else if (MemberRecipient == null)
        {
            if (RegexPatterns.TestEmailRegex(Recipient))
            {
                Message message = new Message();

                ResourceFile VideoMessage = new ResourceFile(VMToken);


                ///*** TEMPORARY
                //if (VideoMessage.ResourceFileID == 0)
                //{
                //    // An external VM must have a video message. return an error message
                //    return (int)MessageSendResponse.EmailMustHaveVideoMessage;
                //}
                //else
                //{
                //    // yes a video message was recorder
                //    message.VideoMessageResourceFileID = VideoMessage.ResourceFileID;
                //}
                ///*** TEMPORARY
                

                message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
                message.VideoMessageToken = VMToken;
                message.MemberIDFrom = member.MemberID;
                message.MemberIDTo = -1;                
                message.Body = MessageBody;
                message.DTCreated = DateTime.Now;
                message.ExternalEmailTo = Recipient;
                message.PassKey = Next2Friends.Misc.UniqueID.NewWebID();

                try
                {

                    message.Save();
                }
                catch(Exception ex) 
                {
                    ex = ex;
                }
                message.InReplyToID = message.MessageID;
                message.Save();

                if (message.MessageID > 0)
                    return (int)MessageSendResponse.Sent;
                else
                    return (int)MessageSendResponse.UnexpectedError;

                //    ResourceFile VideoMessage = new ResourceFile(VMToken);

                //    Message emailMessage = new Message();
                //    emailMessage.MemberIDFrom = member.MemberID;
                //    emailMessage.MemberIDTo = -1;
                //    emailMessage.ExternalEmailTo = Recipient;
                //    emailMessage.FromNickName = member.NickName;
                //    emailMessage.PassKey = Next2Friends.Misc.UniqueID.NewWebID();

                //    emailMessage.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
                //    emailMessage.Body = "";

                //    emailMessage.DTCreated = DateTime.Now;

                //    if (VideoMessage.ResourceFileID != 0)
                //    {
                //        // yes a video message was recorder
                //        emailMessage.VideoMessageResourceFileID = VideoMessage.ResourceFileID;
                //    }
                //    else
                //    {
                //        // An email must have a video message. return an error message
                //        return (int)MessageSendResponse.EmailMustHaveVideoMessage;
                //    }


                //    emailMessage.Save();
                //    emailMessage.InReplyToID = emailMessage.MessageID;

                //    emailMessage.Save();
                //    return (int)MessageSendResponse.Sent;
                //}
                
            }
            else
            {
                return (int)MessageSendResponse.BadAddress;
            }
        }
        else
        {
            Message message = new Message();

            ResourceFile VideoMessage = new ResourceFile(VMToken);

            if (VideoMessage.ResourceFileID != 0)
            {
                // yes a video message was recorder
                message.VideoMessageResourceFileID = VideoMessage.ResourceFileID;
            }

            message.WebMessageID = Next2Friends.Misc.UniqueID.NewWebID();
            message.VideoMessageToken = VMToken;
            message.MemberIDFrom = member.MemberID;
            message.MemberIDTo = MemberRecipient.MemberID;
            message.Body = MessageBody;
            message.DTCreated = DateTime.Now;

            message.Save();

            message.InReplyToID = message.MessageID;

            message.Save();

            if (message.MessageID > 0)
                return (int)MessageSendResponse.Sent;
            else
                return (int)MessageSendResponse.UnexpectedError;
        }
    }

    [AjaxPro.AjaxMethod]
    public string GetVMToken()
    {
        return Next2Friends.Misc.UniqueID.NewWebID();
    }

    [AjaxPro.AjaxMethod]
    public string GetContacts()
    {
        Member member = (Member)Session["Member"];

        string FriendsXML = Friend.GetFriendsXML(member.MemberID);

        return FriendsXML;
    }

    /// <summary>
    /// Saves the members display status
    /// </summary>
    [AjaxPro.AjaxMethod]
    public string GetMiniProfile(string WebMemberID)
    {
        Member member = (Member)Session["Member"];

        string MiniProfileHTML = PopupHTML.GetMiniProgileHTML(WebMemberID, member);

        return MiniProfileHTML;
    }

    /// <summary>
    /// set the page skin
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        // only allow the member in if logged in
        if (Session["Member"] == null && (Request.Params["p"] == string.Empty || Request.Params["p"] == null || Request.Params["w"] == string.Empty || Request.Params["w"] == null))
        {
            Response.Redirect("signup.aspx?u=" + Request.Url.AbsoluteUri);
        }

        Master.SkinID = "Inbox";
        base.OnPreInit(e);
    }

    public class AjaxMessageList
    {
        public string DefaultWebMessageID { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public string DefaultVideoMessageFile { get; set; }
        public int NumberOfNewMessages { get; set; }
        public AjaxMessage[] AjaxMessages { get; set; }
    }
}
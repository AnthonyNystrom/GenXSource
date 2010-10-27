using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Next2Friends.Data;

public enum GetMessageType {Inbox,Trash, ReadMessage }
/// <summary>
/// The AjaxMessage is browser security safe object that contains formatted values from a Message object.
/// This allows an object to be passed back to the browser with no low level information and values preformtted for HTML display
/// </summary>
[Serializable]
public class AjaxMessage
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

    public string WebMessageID { get; set; }
    public string FromNickName { get; set; }
    public string WebMemberIDFrom { get; set; }
    public bool IsRead { get; set; }
    public string StatusText { get; set; }
    public string Body { get; set; }
    public long DTSent { get; set; }
    public string VideoMessageFile { get; set; }
    public string HTML { get; set; }
    public string TimeAgo { get; set; }
    public int TrashType { get; set; }
    

    public AjaxMessage()
    {

    }

    /// <summary>
    /// Converts an array of the data Message class to AjaxMessage class
    /// </summary>
    /// <param name="Messages">The array of Messages object to convert</param>
    /// <param name="PopulateFullMessage">True if the entire message body is to be populated. otherwise the body is clipped to 89 characters in length</param>
    /// <returns>The array of converted AjaxMessage</returns>
    public static AjaxMessage[] ConvertToAjaxMessage(Member member,Message[] Messages, bool CreateBodySnippet, GetMessageType MessageType)
    {
        AjaxMessage[] AjaxMessages = new AjaxMessage[Messages.Length];

        for (int i = 0; i < Messages.Length; i++)
        {
            // only include the reply to options on the first message item
            bool IncludeReplyOptions = (i == 0);

            AjaxMessages[i] = ConvertToAjaxMessage(member,Messages[i], CreateBodySnippet);

            //if (MessageType == GetMessageType.Inbox)
            //    AjaxMessages[i].ToInboxHTML();
            //else if (MessageType == GetMessageType.Trash)
            //    AjaxMessages[i].ToTrashHTML();
            //else
            //    AjaxMessages[i].ToReadMessageString(IncludeReplyOptions);
        }

        return AjaxMessages;

    }

    /// <summary>
    /// Converts an array of the data Message class to AjaxMessage class
    /// </summary>
    /// <param name="aMessage">The Message object to convert</param>
    /// <param name="PopulateFullMessage">True if the entire message body is to be populated. otherwise the body is clipped to 89 characters in length</param>
    /// <returns>The converted AjaxMessage</returns>
    public static AjaxMessage ConvertToAjaxMessage(Member member,Message aMessage, bool CreateBodySnippet)
    {
        AjaxMessage ajaxMessage = new AjaxMessage();

        ajaxMessage.WebMessageID = aMessage.WebMessageID;
        ajaxMessage.FromNickName = aMessage.FromNickName;
        ajaxMessage.IsRead = aMessage.IsRead;
        ajaxMessage.TimeAgo = TimeDistance.TimeAgo(aMessage.DTCreated);
        ajaxMessage.WebMemberIDFrom = aMessage.WebMemberIDFrom;
        //ajaxMessage.DTSent = aMessage.DTCreated.ToUniversalTime().Ticks;

        ajaxMessage.DTSent = (long)(aMessage.DTCreated - TimeZone.CurrentTimeZone.GetUtcOffset(aMessage.DTCreated) - Epoch).TotalMilliseconds; ;
        //ajaxMessage.DTSent = TimeDistance.TimeAgo(aMessage.DTCreated);

        if (member == null || aMessage.MemberIDFrom == member.MemberID)
        {
            ajaxMessage.TrashType = 2; // Sent messages            
        }
        else
        {
            ajaxMessage.TrashType = 1; // Recieved messages            
        }

        if (CreateBodySnippet)
        {
            ajaxMessage.Body = (aMessage.Body.Length > 90) ? aMessage.Body.Substring(0, 89) + ".." : aMessage.Body;
            ajaxMessage.Body = ajaxMessage.Body.Replace("<br />", " ");
        }
        else
        {
            ajaxMessage.Body = HTMLUtility.AutoLink(
                HTMLUtility.FormatForHTML(aMessage.Body)
                );
        }

        if (aMessage.VideoMessageResourceFileID != 0)
        {
            ajaxMessage.VideoMessageFile = aMessage.VideoMessageToken;
        }
        else
        {
            ajaxMessage.VideoMessageFile = string.Empty;
        }

        return ajaxMessage;
    }

//    public void ToReadMessageString(bool IncludeReplyOptions)
//    {
//        StringBuilder sbReplyTo = new StringBuilder();

//        string[] replyParameters = new string[6];

//        replyParameters[0] = this.FromNickName;
//        replyParameters[1] = this.WebMessageID;

//        sbReplyTo.AppendFormat(@"<p class='quick_reply_buttons'><a href='#' class='quick_reply'>Quick Reply</a> <a href='javascript:replyToMessage(""{0}"");void(0);' class='reply_with_video'>Reply with video attachment</a></p>
//								<div class='quickreply_wrap'>
//									<p><textarea name='' cols='' rows='' id='txtQuickSend' class='form_txt'></textarea></p>
//									<p class='align_right'><input name='Send' type='button' id='btnQuickSend' value='Quick Send' onclick='quickSend(""{0}"", ""{1}"");' class='form_txt' />
//									</p>
//								</div>", replyParameters);

//        StringBuilder sb = new StringBuilder();

//        string[] parameters = new string[6];

//        parameters[0] = this.FromNickName;
//        parameters[1] = this.DTSent;
//        parameters[2] = this.Body;
//        parameters[3] = this.WebMessageID;
//        parameters[4] = IncludeReplyOptions ? sbReplyTo.ToString() : string.Empty;
      

//        sb.AppendFormat(@"<li><p class='message_head'><cite>{0}:</cite> <span class='timestamp'>{1}</span></p>
//                            <div class='message_body'>
//                                <p>{2}</p>{4}
//                            </div>
//                        </li>", parameters);


//        HTML = sb.ToString();
//    }


    //public void ToInboxHTML()
    //{
    //    ToHTML(false);
    //}

    //public void ToTrashHTML()
    //{
    //    ToHTML(true);
    //}

    //public void ToHTML(bool IsTrash)
    //{
    //    this.StatusText = (this.IsRead) ? "&raquo; Read" : "&raquo; <a href='#'><strong>New</strong></a>";

    //    StringBuilder sb = new StringBuilder();

    //    string[] parameters = new string[8];

    //    parameters[0] = this.FromNickName;
    //    parameters[1] = this.StatusText;
    //    parameters[2] = this.Body;
    //    parameters[3] = this.DTSent;
    //    parameters[4] = this.WebMessageID;
    //    parameters[5] = (this.IsRead) ? "read" : "unread";
    //    parameters[6] = "";// (this.VideoMessageFile == string.Empty) ? "" : "<img src='images/video.gif'>";
    //    parameters[7] = (IsTrash) ? "ajaxOpenTrashMessage" : "ajaxOpenMessage";

    //    sb.AppendFormat("<p id='inboxItem{4}' style='cursor:pointer;' class='message_item clearfix {5}'><input class='message_checkbox' type='checkbox' value='' name=''/><span onclick='{7}(\"{4}\");'><span class='from'>&nbsp;&nbsp;&nbsp;{0}</span><span class='status' name='status'>{1}</span><span class='excerpt'>{2}</span><span class='time'>{3}</span></span></p>", parameters);


    //    HTML = sb.ToString();
    //}
}

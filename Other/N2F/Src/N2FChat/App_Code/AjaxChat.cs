using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Next2Friends.ChatClient
{
    /// <summary>
    /// Summary description for AjaxChat
    /// </summary>
    public class AjaxChat
    {
        #region Fields and Properties        
        private string _chatWebID = String.Empty;		//The chat id that can safely go to web browser
        private int _otherMemberID = -1;		// 
        private string _otherMemberWebID = String.Empty;		// 

        
        private string _otherMemberNick = String.Empty;
        private string _message = String.Empty;		//                
        private DateTime _dTCreated = new DateTime(1900, 1, 1);		//   
        private bool _delivered = false;

        /// <summary>
        /// wether the message was succsessfully delivered
        /// </summary>
        public bool Delivered
        {
            get { return _delivered; }
            set { _delivered = value; }
        }

        public string OtherMemberWebID
        {
            get { return _otherMemberWebID; }
            set { _otherMemberWebID = value; }
        }

        /// <summary>
        /// The chat id that can safely go to web browser
        /// </summary>
        public string ChatWebID
        {
            get { return _chatWebID; }
            set { _chatWebID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OtherMemberNick
        {
            get { return _otherMemberNick; }
            set { _otherMemberNick = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OtherMemberID
        {
            get { return _otherMemberID; }
            set { _otherMemberID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DTCreated
        {
            get { return _dTCreated; }
            set { _dTCreated = value; }
        }

        #endregion        
    }
}

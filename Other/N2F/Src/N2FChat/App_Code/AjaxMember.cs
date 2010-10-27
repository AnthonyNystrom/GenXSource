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
    /// Summary description for AjaxMember
    /// </summary>
    public class AjaxMember
    {
        private string _nickName = "";		//The display nickname of the  member
        private string _email = "";		//The members email address
        private string _firstName = "";		//The members First Name
        private string _lastName = "";		//The members Last Name
        private string _webMemberID = "";		//
        private OnlineStatus _onlineStatus = OnlineStatus.Offline;
        private string _onlineStatusString = OnlineStatus.Offline.ToString();
        private string _customMessage = "";


        /// <summary>
        /// The display nickname of the  member
        /// </summary>
        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }               

        /// <summary>
        /// The members email address
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// The members First Name
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// The members Last Name
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }       

        /// <summary>
        /// 
        /// </summary>
        public string WebMemberID
        {
            get { return _webMemberID; }
            set { _webMemberID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public OnlineStatus OnlineStatus
        {
            get { return _onlineStatus; }
            set 
            { 
                _onlineStatus = value;
                _onlineStatusString = _onlineStatus.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string OnlineStatusString
        {
            get { return _onlineStatusString; }
            set { _onlineStatusString = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CustomMessage
        {
            get { return _customMessage; }
            set { _customMessage = value; }
        }

    }
}

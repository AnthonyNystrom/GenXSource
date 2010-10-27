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


namespace Next2Friends.ChatClient
{
        // <summary>
        /// Every Member logged into the chat server has a single ChatInbox instance
        /// When the End their chat, the instance is removed
        /// </summary>
        public class ChatInbox
        {
            /// <summary>
            /// The actual MemberID of the member
            /// </summary>
            public int MemberID = 0;

            public AjaxMember MemberInfo = new AjaxMember();

            /// <summary>
            /// If the inbox has a new message
            /// </summary>
            public bool NewMessageFlag { get; set; }
            /// <summary>
            /// The status of the member
            /// </summary>
            public List<AjaxChat> ChatMessages = new List<AjaxChat>();

            public List<AjaxMember> Friends = new List<AjaxMember>();
        }

    }

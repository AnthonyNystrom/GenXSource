using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct LoginResponse
    {
        /// <summary>
        /// The user's full name. Often, clients use this to change the top-level window's title bar text to say something like "LiveJournal - User name". You can just ignore this if you'd like.
        /// </summary>
        [XmlRpcMember("fullname")]
        public String FullName;

        [XmlRpcMember("friendgroups")]
        public FriendGroup[] FriendGroups;

        /// <summary>
        /// The userid of this user on the system. Not required for any other requests to the server, but some developers have wanted it.
        /// </summary>
        [XmlRpcMember("userid")]
        public Int32 UserId;

        /// <summary>
        /// A message that should be displayed in a dialog box (or to the screen in a console application). The message is rarely present but when used notifies the user of software updates they've requested to hear about, problems with their account (if mail is bouncing to them, we'd like them to give us a current e-mail address), etc. For example, on LiveJournal.com a newly-created account will return a message telling the user the e-mail address used for their account has not yet been validated.
        /// </summary>
        [XmlRpcMember("message")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public String Message;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct GetEventsRequest
    {
        /// <summary>
        /// Username of user logging in.
        /// </summary>
        [XmlRpcMember("username")]
        public String Username;

        /// <summary>
        /// Password of user logging in in plaintext.
        /// </summary>
        [XmlRpcMember("password")]
        public String Password;

        /// <summary>
        /// A value that if greater than or equal to 4, truncates the length of the returned events (after being decoded) to the value specified. Entries less than or equal to this length are left untouched. Values greater than this length are truncated to the specified length minus 3, and then have "... " appended to them, bringing the total length back up to what you specified. This is good for populating list boxes where only the beginning of the entry is important, and you'll double-click it to bring up the full entry.
        /// </summary>
        [XmlRpcMember("truncate")]
        public Int32 Truncate;

        /// <summary>
        /// If this setting is set to true, then no subjects are returned, and the events are actually subjects if they exist, or if not, then they're the real events. This is useful when clients display history and need to give the user something to double-click. The subject is shorter and often more informative, so it'd be best to download only this.
        /// </summary>
        [XmlRpcMember("prefersubject")]
        public Boolean PreferSubject;

        /// <summary>
        /// If this setting is set to true, then no meta-data properties are returned. 
        /// </summary>
        [XmlRpcMember("noprops")]
        public Boolean NoProps;

        /// <summary>
        /// Determines how you want to specify what part of the journal to download. Valid values are day to download one entire day, lastn to get the most recent n entries (where n is specified in the howmany field), one to download just one specific entry, or syncitems to get some number of items (which the server decides) that have changed since a given time (specified in the lastsync parameter>). Not that because the server decides what items to send, you may or may not be getting everything that's changed. You should use the syncitems selecttype in conjunction with the syncitems protocol mode.
        /// </summary>
        [XmlRpcMember("selecttype")]
        public String SelectType;
        
        /// <summary>
        /// For a selecttype of lastn, how many entries to get. Defaults to 20. Maximum is 50.
        /// </summary>
        [XmlRpcMember("howmany")]
        public Int32 HowMany;

        /// <summary>
        /// Specifies the type of line-endings you're using. Possible values are unix (0x0A (\n)), pc (0x0D0A (\r\n)), or mac (0x0D (\r) ). The default is not-Mac. Internally, LiveJournal stores all text as Unix-formatted text, and it does the conversion by removing all \r characters. If you're sending a multi-line event on Mac, you have to be sure and send a lineendings value of mac or your line endings will be removed. PC and Unix clients can ignore this setting, or you can send it. It may be used for something more in the future.
        /// </summary>
        [XmlRpcMember("lineendings")]
        public String LineEndings;
    }
}

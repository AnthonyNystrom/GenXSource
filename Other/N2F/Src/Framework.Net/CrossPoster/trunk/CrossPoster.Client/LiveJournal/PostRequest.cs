using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct PostRequest
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
        /// The event/log text the user is submitting. Carriage returns are okay (0x0A, 0x0A0D, or 0x0D0A), although 0x0D are removed internally to make everything into Unix-style line-endings (just \ns). Posts may also contain HTML, but be aware that the LiveJournal server converts newlines to HTML <BR>s when displaying them, so your client should not try to insert these itself.
        /// </summary>
        [XmlRpcMember("event")]
        public String Content;

        /// <summary>
        /// Specifies the type of line-endings you're using. Possible values are unix (0x0A (\n)), pc (0x0D0A (\r\n)), or mac (0x0D (\r) ). The default is not-Mac. Internally, LiveJournal stores all text as Unix-formatted text, and it does the conversion by removing all \r characters. If you're sending a multi-line event on Mac, you have to be sure and send a lineendings value of mac or your line endings will be removed. PC and Unix clients can ignore this setting, or you can send it. It may be used for something more in the future.
        /// </summary>
        [XmlRpcMember("lineendings")]
        public String LineEndings;

        /// <summary>
        /// The subject for this post. Limited to 255 characters. No newlines.
        /// </summary>
        [XmlRpcMember("subject")]
        public String Subject;

        /// <summary>
        /// The current 4-digit year (from the user's local timezone).
        /// </summary>
        [XmlRpcMember("year")]
        public Int32 Year;

        /// <summary>
        /// The current 1- or 2-digit month (from the user's local timezone).
        /// </summary>
        [XmlRpcMember("mon")]
        public Int32 Month;

        /// <summary>
        /// The current 1- or 2-digit day of the month (from the user's local timezone).
        /// </summary>
        [XmlRpcMember("day")]
        public Int32 Day;

        /// <summary>
        /// The current 1- or 2-digit hour from 0 to 23 (from the user's local timezone).
        /// </summary>
        [XmlRpcMember("hour")]
        public Int32 Hour;

        /// <summary>
        /// The current 1- or 2-digit minute (from the user's local timezone).
        /// </summary>
        [XmlRpcMember("min")]
        public Int32 Minute;
    }
}

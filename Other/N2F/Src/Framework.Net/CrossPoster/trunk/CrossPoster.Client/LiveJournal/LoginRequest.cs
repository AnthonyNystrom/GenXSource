using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    public struct LoginRequest
    {
        [XmlRpcMember("username")]
        public String Username;

        [XmlRpcMember("password")]
        public String Password;
    }
}

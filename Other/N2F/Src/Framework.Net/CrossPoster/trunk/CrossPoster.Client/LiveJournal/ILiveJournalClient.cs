using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;

namespace Next2Friends.CrossPoster.Client.LiveJournal
{
    [XmlRpcUrl("http://www.livejournal.com/interface/xmlrpc")]
    public interface ILiveJournalClient : IXmlRpcProxy
    {
        /// <summary>
        /// Validates user's password and get base information needed for client to function.
        /// </summary>
        /// <returns>The server returns with whether the password is good or not, the user's name, an optional message to be displayed to the user, the list of the user's friend groups, and other things.</returns>
        [XmlRpcMethod("LJ.XMLRPC.login")]
        LoginResponse LogIn(LoginRequest request);

        /// <summary>
        /// Downloads parts of the user's journal. See also syncitems mode.
        /// </summary>
        /// <returns>Given a set of specifications, will return a segment of entries up to a limit set by the server. Has a set of options for less, extra, or special data to be returned.</returns>
        [XmlRpcMethod("LJ.XMLRPC.getevents")]
        GetEventsResponse GetEvents(GetEventsRequest request);

        /// <summary>
        /// The most important mode, this is how a user actually submits a new log entry to the server.
        /// </summary>
        [XmlRpcMethod("LJ.XMLRPC.postevent")]
        PostResponse PostEvent(PostRequest request);
    }
}

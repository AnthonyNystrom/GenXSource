using System;
using System.IO;
using Genetibase.Network.Sockets;
using Genetibase.Network.Sockets.Protocols.Authentication;

namespace Genetibase.Network.Web {
	public delegate void SessionEventHandler(HttpSession Sender);
	public delegate void CreateSessionEventHandler(ContextRFC Sender, out HttpSession OSession);
	public delegate void CreatePostStreamEventHandler(ContextRFC Sender, out Stream OPostStream);
	public delegate void HttpCommandEventHandler(ContextRFC Sender, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo, ref bool OHandled);
	public delegate void HttpInvalidSessionEventHandler(ContextRFC Sender, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo, string AInvalidSessionID, out bool VContinueProcessing);
	public delegate void HttpOnRedirectEventHandler(object ASender, ref string RDest, int RNumRedirect, ref bool RHandled, ref HttpCommandEnum RMethod);
	public delegate void HttpOnSelectAuthorizationEventHandler(object ASender, ref Type RAuthenticationType, HeaderList AAuthInfo);
	public delegate void HttpOnAuthorizationEventHandler(object ASender, AuthenticationBase AAuthentication, ref bool RHandled);
	public delegate void CookieManagerNewCookieEventHandler(object ASender, RFC2109Cookie ACookie, ref bool RAccept);
	public delegate void CookieManagerEventHandler(object ASender, CookieCollection ACookieCollection);
}

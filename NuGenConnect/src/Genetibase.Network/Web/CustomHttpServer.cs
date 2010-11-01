using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Genetibase.Network.Sockets;
using Genetibase.Network.Sockets.Protocols;
using Genetibase.Network.Sockets.Protocols.Authentication;

namespace Genetibase.Network.Web {
	public abstract class CustomHttpServer: TcpServerBase<int, ReplyRFC, ContextRFC> {
		protected bool _AutoStartSession;
		protected bool _KeepAlive;
		protected bool _ParseParams;
		protected string _ServerSoftware = "";
		protected MimeTable _MimeTable;
		protected CustomSessionList _SessionList;
		protected bool _SessionState;
		protected int _SessionTimeOut;
		protected bool _OkToProcessCommand;
		protected Thread _SessionCleanupThread;
		protected int _MaximumHeaderLineCount;

		protected virtual void DoOnCreateSession(ContextRFC AContext, out HttpSession ANewSession) {
			ANewSession = null;
		}

		protected virtual void DoInvalidSession(ContextRFC AContext, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo, string AInvalidSessionId, out bool OContinueProcessing) {
			OContinueProcessing = false;
		}

		private void HandleCommand(ContextRFC AContext, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo) {
			bool LHandled = false;
			DoCommand(AContext, ARequestInfo, AResponseInfo, ref LHandled);
			if (!LHandled) {
				DoRequestNotHandled(AContext, ARequestInfo, AResponseInfo);
			}
		}

		protected abstract void DoCommand(ContextRFC context, HttpRequestInfo request, HttpResponseInfo response, ref bool handled);

		protected abstract void DoRequestNotHandled(ContextRFC AContext, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo);
		protected virtual void CreatePostStream(ContextRFC ASender, out Stream OPostStream) {
			OPostStream = null;
		}

		protected override void DoConnect(ContextRFC AContext) {
//			if (AContext.TcpConnection.Socket is SocketTLS) {
//				((SocketTLS)AContext.TcpConnection.Socket).PassThrough = false;
//			}
			base.DoConnect(AContext);
		}

		private void ReadCookiesFromRequestHeader(HttpRequestInfo ARequestInfo) {
			List<string> LRawCookies = new List<string>();
			ARequestInfo.RawHeaders.Extract("cookie", LRawCookies);
			for (int i = 0; i < LRawCookies.Count - 1; i++) {
				string S = LRawCookies[i];
				while (S.IndexOf(';') > -1) {
					ARequestInfo.Cookies.AddSrcCookie(Global.Fetch(ref S, ";"));
					S = S.Trim();
				}
				if (S != "") {
					ARequestInfo.Cookies.AddSrcCookie(S);
				}
			}
			LRawCookies.Clear();
		}

        protected override void DoExecute(object sender, ContextRunEventArgs<int, ReplyRFC, ContextRFC> eventArgs)
        {
            HttpRequestInfo LRequestInfo;
            HttpResponseInfo LResponseInfo;
            bool ContinueProcessing = true;
            string S = "";            
            string sCmd = "";
            string sVersion = "";
            string sRequestURI = "";


            
            URI LURI;
            bool LCloseConnection = !KeepAlive;
            string LRawHttpCommand = "";
            try
            {
                try
                {
                    do
                    {
                        LRawHttpCommand = eventArgs.Context.TcpConnection.Socket.ReadLn();
                        
                        string[] commandParts = LRawHttpCommand.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        switch (commandParts.Length)
                        { 
                            case 2:
                                sCmd = commandParts[0].ToUpper();
                                sRequestURI = "/";
                                sVersion = commandParts[1];
                                break;
                            case 3:
                                sCmd = commandParts[0].ToUpper();
                                sRequestURI = commandParts[1];
                                sVersion = commandParts[2];
                                break;
                            default:
                                throw new HTTPErrorParsingCommandException(ResourceStrings.ErrorParsingCommand);
                        }

                        // TODO: Check for 1.0 only at this point
                        using (LRequestInfo = new HttpRequestInfo(LRawHttpCommand, eventArgs.Context.TcpConnection.Socket.EndPoint, sCmd,  eventArgs.Context))
                        {
                            LRequestInfo.RawHttpCommand = LRawHttpCommand;
                            LRequestInfo.RawHeaders.Clear();
                            eventArgs.Context.TcpConnection.Socket.MaxCapturedLines = MaximumHeaderLineCount;
                            eventArgs.Context.TcpConnection.Socket.Capture(LRequestInfo.RawHeaders, String.Empty);
                            LRequestInfo.ProcessHeaders();
                            
                            // TODO: This is the area that needs fixed. Ive hacked it for now
                            // Get data can exists with POSTs, but can POST data exist with GETs?
                            // If only the first, the solution is easy. If both - need more
                            // investigation.
                            
                            LRequestInfo.PostStream = null;
                            Stream TempStream = LRequestInfo.__PostStream;
                            CreatePostStream(eventArgs.Context, out TempStream);
                            LRequestInfo.__PostStream = TempStream;
                            if (LRequestInfo.PostStream == null)
                            {
                                LRequestInfo.__PostStream = new MemoryStream();
                            }
                            if (LRequestInfo.ContentLength > 0)
                            {
                                eventArgs.Context.TcpConnection.Socket.ReadStream(LRequestInfo.PostStream, LRequestInfo.ContentLength);
                            }
                            else
                            {
                                if (sCmd == "POST")
                                {
                                    if (!LRequestInfo.HasContentLength)
                                    {
                                        eventArgs.Context.TcpConnection.Socket.ReadStream(LRequestInfo.PostStream, -1, true);
                                    }
                                }
                            }
                            LRequestInfo.__Command = sCmd;
                            LRequestInfo.PostStream.Position = 0;
                            LRequestInfo.__FormParams = new StreamReader(LRequestInfo.PostStream).ReadToEnd();
                            LRequestInfo.__UnparsedParams = LRequestInfo.FormParams;



                            LRequestInfo.__QueryParams = sRequestURI.IndexOf("?") >= 0 ?
                                sRequestURI.Substring(sRequestURI.IndexOf("?") + 1) : string.Empty;
                                                            
                            //              LRequestInfo.__RemoteIP = ((new IPEndPoint(. eventArgs.Context.TcpConnection.Socket.EndPoint).Address;
                            //              LRequestInfo.__RemotePort = ((IPEndPoint)eventArgs.Context.TcpConnection.Socket.EndPoint).Port;
                            if (LRequestInfo.QueryParams.Length > 0)
                            {
                                if (LRequestInfo.UnparsedParams.Length == 0)
                                {
                                    LRequestInfo.__UnparsedParams = LRequestInfo.QueryParams;
                                }
                                else
                                {
                                    LRequestInfo.__UnparsedParams += "&" + LRequestInfo.QueryParams;
                                }
                            }
                            if (ParseParams)
                            {
                                if (LRequestInfo.ContentType.ToLower() == "application/x-www-form-urlencoded")
                                {
                                    LRequestInfo.DecodeAndSetParams(LRequestInfo.UnparsedParams);
                                }
                                else
                                {
                                    LRequestInfo.DecodeAndSetParams(LRequestInfo.QueryParams);
                                }
                            }
                            ReadCookiesFromRequestHeader(LRequestInfo);
                            LRequestInfo.__Version = sVersion;
                            LRequestInfo.RequestURI = sRequestURI;
                            if (sRequestURI == "*")
                            {
                                LRequestInfo.__Document = "*";
                            }
                            else
                            {
                                LURI = new URI(sRequestURI);
                                LRequestInfo.__Document = URI.URLDecode(LURI.Document);
                                if (LURI.Host.Length > 0
                                    && LRequestInfo.Host.Length == 0)
                                {
                                    LRequestInfo.__Host = LURI.Host;
                                }
                            }
                            S = LRequestInfo.RawHeaders.Values("Authorization");
                            LRequestInfo.__AuthExists = S.Length > 0;
                            if (LRequestInfo.AuthExists)
                            {
                                string AuthMethod = Global.Fetch(ref S, " ");
                                if (AuthenticationRegistry.FindAuthenticationMethod(AuthMethod) != null)
                                {
                                    using (AuthenticationBase ab = AuthenticationRegistry.GetAuthenticationMethodHandler(AuthMethod))
                                    {
                                        ab.DecodeAuthParams(S);
                                        LRequestInfo.__AuthUserName = ab.UserName;
                                        LRequestInfo.__AuthPassword = ab.Password;
                                    }
                                }
                                else
                                {
                                    throw new HTTPUnsupportedAuthorisationSchemeException(ResourceStrings.UnsupportedAuthorizationScheme);
                                }
                            }

                            using (LResponseInfo = new HttpResponseInfo(eventArgs.Context.TcpConnection))
                            {
                                try
                                {
                                    LResponseInfo.CloseConnection = !(_KeepAlive && LRequestInfo.Connection.Equals("Keep-alive", StringComparison.InvariantCultureIgnoreCase));
                                    GetSessionFromCookie(eventArgs.Context, LRequestInfo, LResponseInfo, out ContinueProcessing);
                                    if (ServerSoftware.Trim().Length > 0)
                                    {
                                        LResponseInfo.ServerSoftware = ServerSoftware;
                                    }
                                    try
                                    {
                                        if (ContinueProcessing)
                                        {
                                            HandleCommand(eventArgs.Context, LRequestInfo, LResponseInfo);
                                        }
                                    }
                                    catch (SocketException)
                                    {
                                        throw;
                                    }
                                    catch (Exception E)
                                    {
                                        LResponseInfo.ResponseNo = 500;
                                        LResponseInfo.ContentText = E.Message;
                                        LResponseInfo.ContentType = "text/plain";
                                    }
                                    if (!LResponseInfo.HeaderHasBeenWritten)
                                    {
                                        LResponseInfo.WriteHeader();
                                    }
                                    if (LResponseInfo.ContentText.Length > 0
                                        || LResponseInfo.ContentStream != null)
                                    {
                                        LResponseInfo.WriteContent();
                                    }
                                }
                                finally
                                {
                                    LCloseConnection = LResponseInfo.CloseConnection;
                                }
                            }
                        }
                    }
                    while (!LCloseConnection);
                }
                catch (SocketException E)
                {
                    if (E.ErrorCode != 10054) // WSAECONNRESET
                    {
                        throw;
                    }
                }
                catch (ClosedSocketException)
                {
                    eventArgs.Context.TcpConnection.Disconnect();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            finally
            {
                eventArgs.Context.TcpConnection.Disconnect(false);
            }
            eventArgs.ReturnValue = false;
        }



		private void HttpSessionCleanupMethod() {
			try {
				while (true) {
					Thread.Sleep(_SessionTimeOut * 1000);
					_SessionList.PurgeStaleSessions(false);
				}
			} catch (ThreadAbortException) {
				_SessionList.PurgeStaleSessions(true);
			}
		}

		public override void Open() {
			if (_SessionTimeOut != 0) {
				_SessionList.SessionTimeOut = _SessionTimeOut;
			} else {
				SessionState = false;
			}
			base.Open();
			if (SessionState) {
				_SessionCleanupThread = new Thread(HttpSessionCleanupMethod);
				_SessionCleanupThread.Priority = ThreadPriority.Normal;
				_SessionCleanupThread.Start();
			}
		}

		public override void Close() {
			if (_SessionCleanupThread != null) {
				_SessionCleanupThread.Priority = ThreadPriority.Normal;
				_SessionCleanupThread.Abort();
				while (_SessionCleanupThread.ThreadState != ThreadState.Aborted) {
					Thread.Sleep(25);
				}
			}
			_SessionList.Clear();
			base.Close();
		}

		protected HttpSession GetSessionFromCookie(ContextRFC AContext, HttpRequestInfo AHttpRequestInfo, HttpResponseInfo AHttpResponseInfo, out bool OContinueProcessing) {
			OContinueProcessing = true;
			HttpSession TempResult = null;
			if (SessionState) {
				int CurrentCookieIndex = AHttpRequestInfo.Cookies.GetCookieIndex(0, Http.SessionIdCookie);
				string SessionId = "";
				while (TempResult == null
						 && CurrentCookieIndex > -1) {
					SessionId = AHttpRequestInfo.Cookies[CurrentCookieIndex].Value;
					TempResult = _SessionList.GetSession(SessionId, AHttpRequestInfo.RemoteIP);
					if (TempResult == null) {
						DoInvalidSession(AContext, AHttpRequestInfo, AHttpResponseInfo, SessionId, out OContinueProcessing);
					}
					CurrentCookieIndex++;
					CurrentCookieIndex = AHttpRequestInfo.Cookies.GetCookieIndex(CurrentCookieIndex, Http.SessionIdCookie);
				}
				if (_AutoStartSession
					&& OContinueProcessing
					&& TempResult == null) {
					TempResult = CreateSession(AContext, AHttpRequestInfo, AHttpResponseInfo);
				}
			}
			AHttpRequestInfo.SetSession(TempResult);
			AHttpResponseInfo.SetSession(TempResult);
			return TempResult;
		}

		public CustomHttpServer() {
			_SessionList = new DefaultSessionList();
			_MimeTable = new MimeTable(true);
			DefaultPort = TcpPorts.Http;
			MaximumHeaderLineCount = Http.HttpServer_MaximumHeaderLineCount;
			AutoStartSession = Http.HttpServer_AutoStartSession;
			KeepAlive = Http.HttpServer_KeepAlive;
			ParseParams = Http.HttpServer_ParseParams;
			ServerSoftware = Http.HttpServer_ServerSoftware;
			SessionState = Http.HttpServer_SessionState;
			SessionTimeOut = Http.HttpServer_SessionTimeOut;
			_OkToProcessCommand = false;
		}

		~CustomHttpServer() {
			Active = false;
			_MimeTable = null;
			_SessionList.Clear();
			_SessionList = null;
		}

		public HttpSession CreateSession(ContextRFC AContext, HttpRequestInfo AHttpRequestInfo, HttpResponseInfo AHttpResponseInfo) {
			if (SessionState) {
				HttpSession TempResult = null;
				DoOnCreateSession(AContext, out TempResult);
				if (TempResult == null) {
					TempResult = _SessionList.CreateUniqueSession(AHttpRequestInfo.RemoteIP);
				} else {
					_SessionList.Add(TempResult);
				}
				ServerCookie TempCookie = AHttpResponseInfo.Cookies.Add();
				TempCookie.CookieName = Http.SessionIdCookie;
				TempCookie.Value = TempResult.SessionId;
				TempCookie.Path = "/";
				TempCookie.MaxAge = -1;
				AHttpRequestInfo.SetSession(TempResult);
				AHttpResponseInfo.SetSession(TempResult);
				return TempResult;
			}
			return null;
		}

		public bool EndSession(string SessionName) {
			HttpSession ASession = SessionList.GetSession(SessionName, "");
			if (ASession != null) {
				ASession.SessionEnd();
				return true;
			}
			return false;
		}

		public MimeTable MimeTable {
			get {
				return _MimeTable;
			}
		}

		public CustomSessionList SessionList {
			get {
				return _SessionList;
			}
		}

		public int MaximumHeaderLineCount {
			get {
				return _MaximumHeaderLineCount;
			}
			set {
				_MaximumHeaderLineCount = value;
			}
		}

		public bool AutoStartSession {
			get {
				return _AutoStartSession;
			}
			set {
				_AutoStartSession = value;
			}
		}

		public bool KeepAlive {
			get {
				return _KeepAlive;
			}
			set {
				_KeepAlive = value;
			}
		}

		public bool ParseParams {
			get {
				return _ParseParams;
			}
			set {
				_ParseParams = value;
			}
		}

		public string ServerSoftware {
			get {
				return _ServerSoftware;
			}
			set {
				_ServerSoftware = value;
			}
		}

		public bool SessionState {
			get {
				return _SessionState;
			}
			set {
				if (Active) {
					throw new HTTPCannotSwitchSessionStateWhenActiveException(ResourceStrings.CannotSwitchSessionStateWhenActive);
				}
				_SessionState = value;
			}
		}

		public int SessionTimeOut {
			get {
				return _SessionTimeOut;
			}
			set {
				_SessionTimeOut = value;
			}
		}
	}
}

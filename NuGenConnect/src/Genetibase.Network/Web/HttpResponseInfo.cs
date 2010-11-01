using System;
using System.IO;

using Genetibase.Network.Sockets;
using System.Text;

namespace Genetibase.Network.Web
{
	public class HttpResponseInfo: ResponseHeaderInfo
	{
	  protected string _AuthRealm = "";
	  protected TcpConnection<int, ReplyRFC> _RemoteConnection;
	  protected int _ResponseNo;
	  protected ServerCookies _Cookies;
	  protected Stream _ContentStream;
	  protected string _ContentText = "";
	  protected bool _CloseConnection;
	  protected bool _CloseContentStream;
	  protected bool _HeaderHasBeenWritten;
	  protected string _ResponseText = "";	  
	  protected HttpSession _Session;
	  
	  protected void DoCloseContentStream()
	  {
	    if (_ContentStream != null)
	    {
	      if (_CloseContentStream)
	      {
	        _ContentStream.Close();
	      }
	      _ContentStream = null;
	    }
	  }
	  
	  protected override void DoSetHeaders()
	  {
	    base.DoSetHeaders();
	    if (Server != "")
	    {
	      _RawHeaders.Values("Server", Server);
	    }
	    if (ContentType != "")
	    {
	      _RawHeaders.Values("Content-Type", ContentType);
	    }
	    if (Location != "")
	    {
	      _RawHeaders.Values("Location", Location);
	    }
	    if (ContentLength > -1)
	    {
	      _RawHeaders.Values("Content-Length", ContentLength.ToString());
	    }
	    if (LastModified.Ticks > 0)
	    {
				_RawHeaders.Values("Last-Modified", Http.DateTimeGmtToHttpStr(LastModified));
	    }
      if (AuthRealm != "")
      {
        ResponseNo = 401;
        _RawHeaders.Values("WWW-Authenticate", "Basic realm=\"" + AuthRealm + "\"");
        _ContentText = "<HTML><BODY><B>" + ResponseNo + " " + ResourceStrings.Unauthorized + "</B></BODY></HTML>";
      }
	  }

	  internal void SetSession(HttpSession ASession)
	  {
	    _Session = ASession;
	  }

    public HttpResponseInfo(TcpConnection<int, ReplyRFC> AConnection)
	  {
	    _CloseContentStream = true;
			ContentLength = Http.ContentLength;
      RawHeaders.FoldLines = false;
      _Cookies = new ServerCookies();
			ServerSoftware = Http.HttpServer_ServerSoftware;
			ContentType = Http.ContentType;
      _RemoteConnection = AConnection;      
			ResponseNo = Http.ResponseNo;
	  }
	  
	  ~HttpResponseInfo()
	  {
	    _Cookies.Clear();
	    DoCloseContentStream();
	  }
	  
	  public void CloseSession()
	  {
			int i = Cookies.GetCookieIndex(0, Http.SessionIdCookie);
	    if (i > -1)
	    {
	      Cookies.RemoveAt(i);
	    }
			Cookies.Add().CookieName = Http.SessionIdCookie;
      using (_Session)
      {
      }
	  }
	  
	  public void Redirect(string AURL)
	  {
	    ResponseNo = 302;
      Location = AURL;
	  }

        public void WriteHeader()
        {
            if (HeaderHasBeenWritten)
            {
                throw new HTTPHeaderAlreadyWrittenException(ResourceStrings.HeaderAlreadyWritten);
            }
            _HeaderHasBeenWritten = true;
            if (ContentLength == -1)
            {
                if (ContentText != "")
                {
                    ContentLength = ContentText.Length;
                }
                else
                {
                    if (ContentStream != null)
                    {
                        ContentLength = (int)ContentStream.Length;
                    }
                }
            }
            SetHeaders();
#warning TODO: Add WriteBuffer support

            StringBuilder sb = new StringBuilder();

            //      _RemoteConnection.Socket.WriteBufferOpen();
            try
            {
                sb.AppendLine(string.Format("HTTP/1.1 {0} {1}", ResponseNo, ResponseText));

                foreach (string s in RawHeaders)
                {
                    sb.AppendLine(s);                    
                }
                foreach (ServerCookie sc in Cookies)
                {
                    sb.AppendLine("Set-Cookie: " + sc.ServerCookie);
                }
                _RemoteConnection.Socket.WriteLn(sb.ToString());
            }
            finally
            {
                //        _RemoteConnection.Socket.WriteBufferClose();
            }
        }
	  
	  public void WriteContent()
	  {
	    if (!HeaderHasBeenWritten)
	    {
	      WriteHeader();
	    }
	    if (ContentText != "")
	    {
        _RemoteConnection.Socket.Write(ContentText);
	    }
	    else
	    {
	      if (ContentStream != null)
	      {
          _RemoteConnection.Socket.Write(ContentStream);
	      }
	      else
	      {
          _RemoteConnection.Socket.WriteLn("<HTML><BODY><B>" + ResponseNo.ToString() + " " + ResponseText + 
	                                            "</B></BODY></HTML>");
	      }
	    }
	    ContentText = "";
	    DoCloseContentStream();	    
	  }

		public virtual long ServeFile(ContextRFC AContext, string AFile, CustomHttpServer server)
	  {
	    if (ContentType.Length == 0)
	    {
            ContentType = server.MimeTable.GetFileMIMEType(AFile);
	    }
      ContentLength = (int)new FileInfo(AFile).Length;
	    Date = File.GetLastWriteTime(AFile);
	    WriteHeader();
      AContext.TcpConnection.Socket.WriteFile(AFile);
      return ContentLength;
	  }

		public virtual long SmartServeFile(ContextRFC AContext, HttpRequestInfo ARequestInfo, string AFile, CustomHttpServer server)
	  {
	    DateTime LFileDate = File.GetLastWriteTime(AFile);
			DateTime LReqDate = Http.GmtToLocalDateTime(ARequestInfo.RawHeaders["If-Modified-Since"]);
	    if (LReqDate.Ticks > 0
	      &&LReqDate.Subtract(new TimeSpan(LFileDate.Ticks)).Ticks < 20000)
      {
        ResponseNo = 304;
        return 0;
      }
      else
      {
          return ServeFile(AContext, AFile, server);
      }
	  }
	  
    public string AuthRealm
    {
      get
      {
        return _AuthRealm;
      }
      set
      {
        _AuthRealm = value;
      }
    }
    
    public bool CloseConnection
    {
      get
      {
        return _CloseConnection;
      }
      set
      {
        if (value)
        {
          Connection = "close";
        }
        else
        {
          Connection = "keep-alive";
        }
        _CloseConnection = value;
      }
    }
    
    public Stream ContentStream
    {
      get
      {
        return _ContentStream;
      }
      set
      {
        _ContentStream = value;
      }
    }
    
    public string ContentText
    {
      get
      {
        return _ContentText;
      }
      set
      {
        _ContentText = value;
      }
    }
    
    public ServerCookies Cookies
    {
      get
      {
        return _Cookies;
      }
      set
      {
        _Cookies = value;
      }
    }
    
    public bool CloseContentStream
    {
      get
      {
        return _CloseContentStream;
      }
      set
      {
        _CloseContentStream = value;
      }
    }
    
    public bool HeaderHasBeenWritten
    {
      get
      {
        return _HeaderHasBeenWritten;
      }
      set
      {
        _HeaderHasBeenWritten = value;
      }
    }
    
    public int ResponseNo
    {
      get
      {
        return _ResponseNo;
      }
      set
      {
        _ResponseNo = value;
        switch(_ResponseNo)
        {
          case 100: 
            {
              ResponseText = ResourceStrings.Continue;
              break;
            }
          case 200: 
            {
              ResponseText = ResourceStrings.OK;
              break;
            }
          case 201: 
            {
              ResponseText = ResourceStrings.Created;
              break;
            }
          case 202: 
            {
              ResponseText = ResourceStrings.Accepted;
              break;
            }
          case 203: 
            {
              ResponseText = ResourceStrings.NonAuthoritativeInformation;
              break;
            }
          case 204: 
            {
              ResponseText = ResourceStrings.NoContent;
              break;
            }
          case 205: 
            {
              ResponseText = ResourceStrings.ResetContent;
              break;
            }
          case 206: 
            {
              ResponseText = ResourceStrings.PartialContent;
              break;
            }
          case 301: 
            {
              ResponseText = ResourceStrings.MovedPermanently;
              break;
            }
          case 302: 
            {
              ResponseText = ResourceStrings.MovedTemporarily;
              break;
            }
          case 303: 
            {
              ResponseText = ResourceStrings.SeeOther;
              break;
            }
          case 304: 
            {
              ResponseText = ResourceStrings.NotModified;
              break;
            }
          case 305: 
            {
              ResponseText = ResourceStrings.UseProxy;
              break;
            }
          case 400: 
            {
              ResponseText = ResourceStrings.BadRequest;
              break;
            }
          case 401: 
            {
              ResponseText = ResourceStrings.Unauthorized;
              break;
            }
          case 403: 
            {
              ResponseText = ResourceStrings.Forbidden;
              break;
            }
          case 404: 
            {
              ResponseText = ResourceStrings.NotFound;
              CloseConnection = true;
              break;
            }
          case 405: 
            {
              ResponseText = ResourceStrings.MethodNotAllowed;
              break;
            }
          case 406: 
            {
              ResponseText = ResourceStrings.NotAcceptable;
              break;
            }
          case 407: 
            {
              ResponseText = ResourceStrings.ProxyAuthenticationRequired;
              break;
            }
          case 408: 
            {
              ResponseText = ResourceStrings.RequestTimeOut;
              break;
            }
          case 409: 
            {
              ResponseText = ResourceStrings.Conflict;
              break;
            }
          case 410: 
            {
              ResponseText = ResourceStrings.Gone;
              break;
            }
          case 411: 
            {
              ResponseText = ResourceStrings.LengthRequired;
              break;
            }
          case 412: 
            {
              ResponseText = ResourceStrings.PreconditionFailed;
              break;
            }
          case 413: 
            {
              ResponseText = ResourceStrings.RequestEntityTooLong;
              break;
            }
          case 414: 
            {
              ResponseText = ResourceStrings.RequestURITooLong;
              break;
            }
          case 415: 
            {
              ResponseText = ResourceStrings.UnsupportedMediaType;
              break;
            }
          case 500:
            {
              ResponseText = ResourceStrings.InternalServerError;
              break;
            }
          case 501: 
            {
              ResponseText = ResourceStrings.NotImplemented;
              break;
            }
          case 502: 
            {
              ResponseText = ResourceStrings.BadGateway;
              break;
            }
          case 503: 
            {
              ResponseText = ResourceStrings.ServiceUnavailable;
              break;
            }
          case 504: 
            {
              ResponseText = ResourceStrings.GatewayTimeout;
              break;
            }
          case 505: 
            {
              ResponseText = ResourceStrings.HTTPVersionNotSupported;
              break;
            }
          default:
            {
              ResponseText = ResourceStrings.UnknownResponseCode;
              break;
            }
        }
      }
    }
    
    public string ResponseText
    {
      get
      {
        return _ResponseText;
      }
      set
      {
        _ResponseText = value;
      }
    }      
    
    public string ServerSoftware
    {
      get
      {
        return _Server;
      }
      set
      {
        _Server = value;
      }
    }
    
    public HttpSession Session
    {
      get
      {
        return _Session;
      }
    }
	}
}

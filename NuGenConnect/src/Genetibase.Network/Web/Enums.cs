using System;

namespace Genetibase.Network.Web
{
  public enum HttpCommandEnum
  {
    Unknown,
    Head,
    Get,
    Post,
    Delete,
    Put,
    Trace,
    Options,
    Connect
  }
  
  public enum CookieVersionEnum
  {
    Netscape,
    RFC2109,
    RFC2965
  }
  
  public enum HttpWhatsNextEnum
  {
    GoToUrl,
    JustExit,
    DontKnow,
    ReadAndGo,
    AuthRequest
  }
  
  public enum HttpConnectionTypeEnum
  {
    Normal,
    Ssl,
    Proxy,
    SslProxy
  }
  
  [Flags]
  public enum HttpOptionsEnum
  {
    InProcessAuth = 1,
    KeepOrigProtocol = 2,
    ForceEncodeParams = 4
  }

  public enum HttpProtocolVersionEnum
  {
    v1_0,
    v1_1
  }
}

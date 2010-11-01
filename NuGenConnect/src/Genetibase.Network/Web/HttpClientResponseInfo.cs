using System;
using System.IO;

namespace Genetibase.Network.Web
{
	public class HttpClientResponseInfo: ResponseHeaderInfo
	{
	  protected int _ResponseCode;
	  protected string _ResponseText = "";
	  protected bool _KeepAlive;
	  protected Stream _ContentStream;
	  protected HttpProtocolVersionEnum _ResponseVersion;

    public HttpClientResponseInfo()
    {      
    }
    
    public bool KeepAlive
    {
      get
      {
        return _KeepAlive;
      }
      set
      {
        _KeepAlive = value;
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
    
    public int ResponseCode
    {
      get
      {
        string S = _ResponseText;
        Genetibase.Network.Sockets.Global.Fetch(ref S);
        S = S.Trim();
        _ResponseCode = Genetibase.Network.Sockets.Global.StrToInt32Def(Genetibase.Network.Sockets.Global.Fetch(ref S, " ", false), -1);
        return _ResponseCode;
      }
      set
      {
        _ResponseCode = value;
      }
    }
    
    public HttpProtocolVersionEnum ResponseVersion
    {
      get
      {
          return (HttpProtocolVersionEnum)Array.IndexOf(Http.HttpProtocolVersionNames, ResponseText.Substring(5, 3));
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
	}
}

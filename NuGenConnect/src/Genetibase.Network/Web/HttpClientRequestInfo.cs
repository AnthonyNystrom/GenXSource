using System;
using System.IO;

namespace Genetibase.Network.Web {
  public class HttpClientRequestInfo: RequestHeaderInfo {    
    protected string _Url = "";
    protected HttpCommandEnum _Method;
    protected Stream _SourceStream;
    protected HttpConnectionTypeEnum _UseProxy;
    //	  protected IPVersionEnum _IPVersion;

    public HttpClientRequestInfo() {      
      _UseProxy = HttpConnectionTypeEnum.Normal;
      RequestUri = "/";        
    }

    public string RequestUri {
      get {
        return _Url;
      }
      set {
        _Url = value;
      }
    }

    public HttpCommandEnum Method {
      get {
        return _Method;
      }
      set {
        _Method = value;
      }
    }

    public Stream PostStream {
      get {
        return _SourceStream;
      }
      set {
        _SourceStream = value;
      }
    }

    public HttpConnectionTypeEnum UseProxy {
      get {
        return _UseProxy;
      }
      set {
        _UseProxy = value;
      }
    }

//    public IPVersionEnum IPVersion {
//      get {
//        return _IPVersion;
//      }
//      set {
//        _IPVersion = value;
//      }
//    }
  }
}

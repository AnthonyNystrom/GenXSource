using System;
using System.ComponentModel;

using Genetibase.Network.Sockets.Protocols.Authentication;

namespace Genetibase.Network.Web
{
	public class RequestHeaderInfo: EntityHeaderInfo
	{
	  protected string _Accept = "";
	  protected string _AcceptCharSet = "";
	  protected string _AcceptEncoding = "";
	  protected string _AcceptLanguage = "";
	  protected string _Expect = "";
	  protected string _From = "";
	  protected string _Password = "";
	  protected string _Referer = "";
	  protected string _UserAgent = "";
	  protected string _UserName = "";
	  protected string _Host = "";
	  protected bool _BasicByDefault = false;
	  protected string _ProxyConnection = "";
	  protected AuthenticationBase _Authentication = null;
	  protected override void DoProcessHeaders()
	  {
	    _Accept = _RawHeaders.Values("Accept");
	    _AcceptCharSet = _RawHeaders.Values("Accept-Charset");
	    _AcceptEncoding = _RawHeaders.Values("Accept-Encoding");
	    _AcceptLanguage = _RawHeaders.Values("Accept-Language");
	    _Host = _RawHeaders.Values("Host");
	    _From = _RawHeaders.Values("From");
	    _Referer = _RawHeaders.Values("Referer");
	    _UserAgent = _RawHeaders.Values("User-Agent");
        string RangeDecode = _RawHeaders.Values("Content-Range");
	    if (RangeDecode != "")
	    {
	      Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode, "=");
        _ContentRangeStart = Genetibase.Network.Sockets.Global.StrToInt32Def(Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode, "-"), 0);
        _ContentRangeEnd = Genetibase.Network.Sockets.Global.StrToInt32Def(Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode), 0);
	    }
      base.DoProcessHeaders();
	  }
	  
	  protected override void DoSetHeaders()
	  {
	    base.DoSetHeaders();
	    
	    if (_ProxyConnection.Length > 0)
	    {
	      _RawHeaders.Values("Proxy-Connection", _ProxyConnection);
	    }
	    if (_Host.Length > 0)
	    {
	      _RawHeaders.Values("Host", _Host);
	    }
      if (_Accept.Length > 0)
      {
        _RawHeaders.Values("Accept", _Accept);
      }
      if (_AcceptCharSet.Length > 0)
      {
        _RawHeaders.Values("Accept-Charset", _AcceptCharSet);
      }
      if (_AcceptEncoding.Length > 0)
      {
        _RawHeaders.Values("Accept-Language", _AcceptLanguage);
      }
      if (_From.Length > 0)
      {
        _RawHeaders.Values("From", _From);
      }
      if (_Referer.Length > 0)
      {
        _RawHeaders.Values("Referer", _Referer);
      }
      if (_UserAgent.Length > 0)
      {
        _RawHeaders.Values("User-Agent", _UserAgent);
      }
      if (_LastModified.Ticks > 0)
      {
				_RawHeaders.Values("If-Modified-Since", Http.DateTimeGmtToHttpStr(_LastModified));
      }
      if (_ContentRangeStart != 0
        ||_ContentRangeEnd != 0)
      {
        if (_ContentRangeEnd != 0)
        {
          _RawHeaders.Values("Range", "bytes=" + _ContentRangeStart.ToString() + "-" + _ContentRangeEnd.ToString());
        }
        else
        {
          _RawHeaders.Values("Range", "bytes=" + _ContentRangeStart.ToString() + "-");
        }
      }
      if (_Authentication != null)
      {
        string S = _Authentication.Authentication();
        if (S.Length > 0)
        {
          _RawHeaders.Values("Authorization", S);
        }
      }
      else
      {
        if (_BasicByDefault)
        {
          _Authentication = new BasicAuthentication();
          _Authentication.UserName = _UserName;
          _Authentication.Password = _Password;
          string S = _Authentication.Authentication();
          if (S.Length > 0)
          {
            _RawHeaders.Values("Authorization", S);
          }
        }
      }
	  }
	  
	  ~RequestHeaderInfo()
	  {
	    _Authentication = null;
	    Clear();
	  }
	  
	  public override void Clear()
	  {
	    _Accept = "text/html, */*";
      _AcceptCharSet = "";
			_UserAgent = Http.DefaultUserAgent;
      _BasicByDefault = false;
      base.Clear();
	  }
	  
	  [Browsable(false)]
	  public AuthenticationBase Authentication
	  {
	    get
	    {
	      return _Authentication;
	    }
	    set
	    {
	      _Authentication = value;
	    }
	  }
	  
	  public string Accept
	  {
	    get
	    {
	      return _Accept;
	    }
	    set
	    {
	      _Accept = value;
	    }
	  }
	  
	  public string AcceptCharSet
	  {
	    get
	    {
	      return _AcceptCharSet;
	    }
	    set
	    {
	      _AcceptCharSet = value;
	    }
	  }
	  
	  public string AcceptEncoding
	  {
	    get
	    {
	      return _AcceptEncoding;
	    }
	    set
	    {
	      _AcceptEncoding = value;
	    }
	  }
	  
	  public string AcceptLanguage
	  {
	    get
	    {
	      return _AcceptLanguage;
	    }
	    set
	    {
	      _AcceptLanguage = value;
	    }
	  }
	  
	  public bool BasicAuthentication
	  {
	    get
	    {
	      return _BasicByDefault;
	    }
	    set
	    {
	      _BasicByDefault = value;
	    }
	  }
	  
	  public string Host
	  {
	    get
	    {
	      return _Host;
	    }
	    set
	    {
	      _Host = value;
	    }
	  }
	  
	  public string From
	  {
	    get
	    {
	      return _From;
	    }
	    set
	    {
	      _From = value;
	    }
	  }
	  
	  public string Password
	  {
	    get
	    {
	      return _Password;
	    }
	    set
	    {
	      _Password = value;
	    }
	  }
	  
	  public string Referer
	  {
	    get
	    {
	      return _Referer;
	    }
	    set
	    {
	      _Referer = value;
	    }
	  }
	  
	  public string UserAgent
	  {
	    get
	    {
	      return _UserAgent;
	    }
	    set
	    {
	      _UserAgent = value;
	    }
	  }
	  
	  public string UserName
	  {
	    get
	    {
	      return _UserName;
	    }
	    set
	    {
	      _UserName = value;
	    }
	  }
	  
	  public string ProxyConnection
	  {
	    get
	    {
	      return _ProxyConnection;
	    }
	    set
	    {
	      _ProxyConnection = value;
	    }
	  }
	}
}

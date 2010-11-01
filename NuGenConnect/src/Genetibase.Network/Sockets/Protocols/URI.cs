using System;

namespace Genetibase.Network.Sockets.Protocols
{
  [Flags]
  public enum URIOptionalFieldsEnum {
    None,
    AuthInfo,
    Bookmark
  }

  public class URIException: Genetibase.Network.Sockets.IndyException {
    public URIException(string AMsg)
      : base(AMsg) {
    }
  }

	public class URI
	{
	  protected string _Document = "";
	  protected string _Protocol = "";
	  protected string _URI = "";
	  protected string _Port = "";
	  protected string _Path = "";
	  protected string _Host = "";
	  protected string _Bookmark = "";
	  protected string _UserName = "";
	  protected string _Password = "";
	  protected string _Params = "";
#warning TODO: add IPv6 support
	  //protected IPVersionEnum _IPVersion;
    
    public URI():this("")
    {
    }
    
    public URI(string AURI)
    {
      if (AURI.Length > 0)
      {
        URIString = AURI;
      }
    }
    
    public void Clear()
    {
  	  _Document = "";
  	  _Protocol = "";
  	  _URI = "";
  	  _Port = "";
  	  _Path = "";
  	  _Host = "";
  	  _Bookmark = "";
  	  _UserName = "";
  	  _Password = "";
  	  _Params = "";
    }
    
    public string GetFullURI()
    {
      return GetFullURI(URIOptionalFieldsEnum.AuthInfo | URIOptionalFieldsEnum.Bookmark);
    }
    
    public string GetFullURI(URIOptionalFieldsEnum AOptionalFields)
    {
      string LURI = "";
      if (_Protocol == "")
      {
        throw new URIException(ResourceStrings.URINoProtocolSpecified);
      }
      LURI = _Protocol + "://";
      if (_UserName != ""
        ||(AOptionalFields | URIOptionalFieldsEnum.AuthInfo) == AOptionalFields)
      {
        LURI += _UserName;
        if (_Password != "")
        {
          LURI += ":" + _Password;
        }
        LURI += "@";
      }
      if (_Host == "")
      {
        throw new URIException(ResourceStrings.URINoHostSpecified);
      }
      LURI += _Host;
      if (_Port != "")
      {
        LURI += ":" + _Port;
      }
      LURI += _Path + (!string.IsNullOrEmpty(_Document) ? '/' + _Document : string.Empty);
      if (_Params != "")
      {
        LURI += "?" + _Params;
      }
      if (_Bookmark != ""
        ||(AOptionalFields | URIOptionalFieldsEnum.Bookmark) == AOptionalFields)
      {
        LURI += "#" + _Bookmark;
      }
      return LURI;
    }

    public static void NormalizePath(ref string APath)
    {
      int i = 0;
      string TempResult = "";
      while (i < APath.Length)
      {
        if (APath[i] == '\\')
        {       
          TempResult += '/';
        }
        else
        {
          TempResult += APath[i];
        }
        i++;
      }
      APath = TempResult;
    }
    
    public static string URLDecode(string ASrc)
    {
      int i = 0;
      string Esc = "";
      int CharCode = 0;
      string TempResult = "";
      while (i < ASrc.Length)
      {
        if (ASrc[i] != '%')
        {
          TempResult += ASrc[i];
        }
        else
        {
          i ++;
          Esc = ASrc.Substring(i, 2);
          i++;
          CharCode = Genetibase.Network.Sockets.Global.StrToInt32Def("$" + Esc, -1);
          if (CharCode != -1)
          {
            TempResult += ((char)((byte)CharCode));
          }
        }
        i++;
      }
      return TempResult;
    }
    
    public static string URLEncode(string ASrc)
    {
      URI TempUri = new URI(ASrc);
      {
        TempUri.Path = PathEncode(TempUri.Path);
        TempUri.Document = PathEncode(TempUri.Document);
        TempUri.Params = ParamsEncode(TempUri.Params);
        return TempUri.URIString;
      }
    }
    
    private static bool IsSafeChar(char AChar)
    {
      string UnsafeChars = "*#%<> []";
      if (UnsafeChars.IndexOf(AChar) > -1)
      {
        return false;
      }
      if ((byte)AChar < 32
        &&(byte)AChar > 129)
      {
        return false;
      }
      return true;
    }
    
    public static string ParamsEncode(string ASrc)
    {
      string TempResult = "";
      for (int i = 0; i < ASrc.Length; i++)
      {
        if (IsSafeChar(ASrc[i]))
        {
          TempResult += ASrc[i];
        }
        else
        {
          TempResult += "%" + ((byte)ASrc[i]).ToString("XX");
        }
      }
      return TempResult;
    }

    public static string PathEncode(string ASrc)
    {
      const string UnsafeChars = "*#%<>+ []";
      string TempResult = "";
      for (int i = 0; i < ASrc.Length; i++)
      {
        if (UnsafeChars.IndexOf(ASrc[i]) != -1
          ||ASrc[i] >= (char)128
          ||ASrc[i] < 32)
        {
          TempResult += "%" + ((byte)ASrc[i]).ToString("X2");
        }
        else
        {
          TempResult += ASrc[i];
        }
      }
      return TempResult;
    }
    
    public string Bookmark
    {
      get
      {
        return _Bookmark;
      }
      set
      {
        _Bookmark = value;
      }
    }
    
    public string Document
    {
      get
      {
        return _Document;
      }
      set
      {
        _Document = value;
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

    public string Path
    {
      get
      {
        return _Path;
      }
      set
      {
        _Path = value;
      }
    }
    
    public string Params
    {
      get
      {
        return _Params;
      }
      set
      {
        _Params = value;
      }
    }
    
    public string Port
    {
      get
      {
        return _Port;
      }
      set
      {
        _Port = value;
      }
    }
    
    public string Protocol
    {
      get
      {
        return _Protocol;
      }
      set
      {
        _Protocol = value;
      }
    }

    public string URIString
    {
      get
      {
        _URI = GetFullURI(URIOptionalFieldsEnum.None);
        return _URI;
      }
      set
      {
        string LBuffer = "";
        int LTokenPos;
        int LParamsPos;
        string LURI;
        _URI = value;
        NormalizePath(ref _URI);
        LURI = value;
        _Host = "";
        _Protocol = "";
        _Path = "";
        _Document = "";
        _Port = "";
        _Bookmark = "";
        _UserName = "";
        _Password = "";
        _Params = "";
///        _IPVersion = IPVersionEnum.v4;
        LTokenPos = LURI.IndexOf("://");
        if (LTokenPos > -1)
        {
          _Protocol = LURI.Substring(0, LTokenPos);
          LURI = LURI.Substring(LTokenPos + 3);
          LBuffer = Genetibase.Network.Sockets.Global.Fetch(ref LURI, "/", true);
          LTokenPos = LBuffer.IndexOf('@');
          if (LTokenPos > -1)
          {
            _Password = LBuffer.Substring(0, LTokenPos);
            LBuffer = LBuffer.Substring(LTokenPos + 1);
            _UserName = Genetibase.Network.Sockets.Global.Fetch(ref _Password, ":", true);
            if (_UserName.Length == 0)
            {
              _Password = "";
            }
          }
          if (LBuffer.IndexOf('[') > -1
            &&LBuffer.IndexOf(']') > LBuffer.IndexOf('['))
          {
            _Host = Genetibase.Network.Sockets.Global.Fetch(ref LBuffer, "]");
            Genetibase.Network.Sockets.Global.Fetch(ref _Host, "[");
            Genetibase.Network.Sockets.Global.Fetch(ref LBuffer, ":");
//            _IPVersion = IPVersionEnum.v6;
          }
          else
          {
            _Host = Genetibase.Network.Sockets.Global.Fetch(ref LBuffer, ":", true);
          }
          _Port = LBuffer;
          LParamsPos = LURI.IndexOf('?');
          if (LParamsPos > -1)
          {
            LTokenPos = LURI.IndexOf('/', LParamsPos);
          }
          else
          {
            LParamsPos = LURI.IndexOf('=');
            if (LParamsPos > -1)
            {
              LTokenPos = LURI.IndexOf('/', LParamsPos);
            }
            else
            {
              LTokenPos = LURI.IndexOf('/');
            }
          }
          if (LTokenPos == -1)
          {
            _Path = "/";
          }
          else
          {
            _Path = '/' + LURI.Substring(0, LTokenPos);
          }
          if (LParamsPos > 0)
          {
            _Document = LURI.Substring(0, LParamsPos);
            _Params = LURI = LURI.Remove(0, LParamsPos + 1);
          }
          else
          {
            _Document = LURI;
          }
          _Document = _Document.Remove(0, LTokenPos + 1);
          
          _Bookmark = _Document;
          _Document = Genetibase.Network.Sockets.Global.Fetch(ref _Bookmark, "#");
        }
        else
        {
          LParamsPos = LURI.IndexOf('?');
          if (LParamsPos > -1)
          {
            LTokenPos = LURI.IndexOf('/', LParamsPos + 1);
          }
          else
          {
            LParamsPos = LURI.IndexOf('=');
            if (LParamsPos > -1)
            {
              LTokenPos = LURI.IndexOf('/', LParamsPos + 1);
            }
            else
            {
              LTokenPos = LURI.IndexOf('/');
            }
          }
          _Path = LURI.Substring(0, LTokenPos + 1);
          if (LParamsPos > -1)
          {
            _Document = LURI.Substring(0, LParamsPos);
            LURI = LURI.Remove(0, LParamsPos + 1);
            _Params = LURI;
          }
          else
          {
            if (LTokenPos == -1)
            {
              _Document = "";
            }
            else
            {
            }
          }
          if (LTokenPos > 0) {
            _Document = _Document.Remove(0, LTokenPos - 1);
          }
        }
        if (_Bookmark == "")
        {
          _Bookmark = _Params;
          _Params = Genetibase.Network.Sockets.Global.Fetch(ref _Bookmark, "#");
        }
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
    
//    public IPVersionEnum IPVersion
//    {
//      get
//      {
//        return _IPVersion;
//      }
//      set
//      {
//        _IPVersion = value;
//      }
//    }
 	}
}
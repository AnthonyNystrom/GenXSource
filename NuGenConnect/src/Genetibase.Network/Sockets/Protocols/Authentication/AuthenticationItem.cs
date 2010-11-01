using System;
using System.Collections.Generic;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
	public class AuthenticationItem
	{
    protected URI _Url = new URI();
    protected Dictionary<string, string> _Params = new Dictionary<string, string>();
	  
    public AuthenticationItem()
    {
    }

    public URI Url
    {
      get
      {
        return _Url;
      }
      set
      {
        _Url = value;
      }
    }
    
    public Dictionary<string, string> Params
    {
      get
      {
        return _Params;
      }
    }
	}
}

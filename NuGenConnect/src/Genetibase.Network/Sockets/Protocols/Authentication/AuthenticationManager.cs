using System;
using System.Collections.Generic;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
	public class AuthenticationManager
	{
    protected List<AuthenticationItem> _Authentications = new List<AuthenticationItem>();
	  
	  public AuthenticationManager()
	  {
	  }
	  
    public void AddAuthentication(AuthenticationBase AAuthentication, URI AUrl)
    {
      AuthenticationItem TempItem = new AuthenticationItem();
      for (int i = 0; i < AAuthentication.Params.Count; i++) {
        TempItem.Params.Add(AAuthentication.Params.Names(i), AAuthentication.Params.ValueFromIndex(i));
      }
      TempItem.Url = new URI(AUrl.URIString);
      _Authentications.Add(TempItem);
    }

    public List<AuthenticationItem> Authentications
    {
      get
      {
        return _Authentications;
      }
    }
	}
}

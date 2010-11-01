using System;

namespace Genetibase.Network.Web
{
	public class ServerCookies: CookieCollection
	{
		public new ServerCookie Add()
		{
		  ServerCookie Temp = new ServerCookie();
		  base.AddCookie(Temp);
		  return Temp;
		}
		
		public ServerCookie Cookie(string AName)
		{
		  int I = GetCookieIndex(0, AName);
		  if (I == -1)
		  {
		    return null;
		  }
		  else
		  {
		    return (ServerCookie)base[I];
		  }
		}
	}
}

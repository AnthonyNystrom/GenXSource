using System;

namespace Genetibase.Network.Web
{
	public class Cookie
	{
    public const int MaxAge = -1;
	  
	  public static string AddCookieProperty(string AProperty, string AValue, string ACookie)
	  {
	    if (AValue.Length > 0)
	    {
	      if (ACookie.Length > 0)
	      {
	        ACookie += "; ";
	      }
	      ACookie = ACookie + AProperty + "=" + AValue;
	    }
	    return ACookie;
	  }
	  
	  public static string AddCookieFlag(string AFlag, string ACookie)
	  {
	    if (ACookie.Length > 0)
	    {
	      ACookie += "; ";
	    }
	    ACookie += AFlag;
	    return ACookie;
	  }
	}
}

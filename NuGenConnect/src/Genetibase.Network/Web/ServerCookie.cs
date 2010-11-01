using System;

namespace Genetibase.Network.Web
{
	public class ServerCookie: RFC2109Cookie
	{
    protected override string GetCookie()
    {      
      if (_MaxAge > -1)
      {
				DateTime ANow = DateTime.Now.AddMilliseconds(Http.TimeZoneBias().Ticks / 10).AddMilliseconds(_MaxAge);
        _Expires = 
          String.Format("{0}, {1}-{2}-{3} {4} GMT",
								 Http.WeekDays[(int)ANow.DayOfWeek],
                 ANow.Day,
								 Http.MonthNames[ANow.Month],
                 ANow.Year,
                 ANow.ToString("HH\":\"mm\":\"ss"));
        
      }
      return base.GetCookie();
    }
    
    public ServerCookie()
    {
      _InternalVersion = CookieVersionEnum.Netscape;
    }
    
    public void AddAttribute(string AAttribute, string AValue)
    {
      if (AAttribute.ToUpper() == "$PATH")
      {
        Path = AValue;
      }
      else
      {
        if (AAttribute.ToUpper() == "$DOMAIN")
        {
          Domain = AValue;
        }
      }
    }
	}
}

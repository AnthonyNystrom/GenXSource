using System;
using System.Collections.Generic;

namespace Genetibase.Network.Web
{
	public class CookieCollection: List<RFC2109Cookie>, IDisposable
	{
    protected SortedList<string, CookieList> _CookieListByDomain = new SortedList<string, CookieList>(StringComparer.InvariantCultureIgnoreCase);
	  
	  void IDisposable.Dispose()
	  {
      Clear();
      _CookieListByDomain.Clear();
      _CookieListByDomain = null;
      GC.SuppressFinalize(this);
	  }
	  
    protected void SetItem(int AIndex, RFC2109Cookie AValue)
    {
      this[AIndex] = AValue;
    }
    
    public CookieCollection()
    {
    }
    
    public RFC2109Cookie Add()
    {
      RFC2109Cookie Temp = new RFC2109Cookie();
      this.AddCookie(Temp);
      return Temp;
    }
    
    public RFC2965Cookie Add2()
    {
      RFC2965Cookie Temp = new RFC2965Cookie();
      this.AddCookie(Temp);
      return Temp;
    }
    
    public void AddCookie(RFC2109Cookie ACookie)
    {
      SortedList<string, CookieList> cl = GetCookieListByDomain();
      lock(cl)
      {
        if (cl.IndexOfKey(ACookie.Domain) == -1)
        {
          cl.Add(ACookie.Domain, new CookieList());
        }
        int TempInt = cl[ACookie.Domain].IndexOfCookie(ACookie.CookieName);
        if (TempInt == -1)
        {
          cl[ACookie.Domain].Add(ACookie);
        }
        else
        {
          cl[ACookie.Domain][TempInt] = ACookie;
        }
      }
    }
    
    public void AddSrcCookie(string sCookie)
    {
      Add().CookieText = sCookie;
    }
    
    public int GetCookieIndex(int AFirstIndex, string AName)
    {
      for (int i = AFirstIndex; i < Count; i++)
      {
        if (String.Equals(this[i].CookieName, AName, StringComparison.InvariantCultureIgnoreCase))
        {
          return i;
        }
      }
      return -1;
    }
    
    public int GetCookieIndex(int AFirstIndex, string AName, string ADomain)
    {
      for (int i = AFirstIndex; i < Count; i++)
      {
        if (String.Equals(this[i].CookieName, AName, StringComparison.InvariantCultureIgnoreCase)
          && String.Equals(this[i].Domain, ADomain, StringComparison.InvariantCultureIgnoreCase))
        {
          return i;
        }
      }
      return -1;
    }
    
    public SortedList<string, CookieList> GetCookieListByDomain()
    {
      return _CookieListByDomain;
    }
    
    public RFC2109Cookie GetCookie(string AName, string ADomain)
	  {
	    int i = GetCookieIndex(0, AName, ADomain);
      if (i == -1)
      {
        return null;
      }
      else
      {
        return this[i];
      }
	  }
	}
}

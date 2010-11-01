using System;
using System.Collections.Generic;

namespace Genetibase.Network.Web
{
	public class RFC2965Cookie: RFC2109Cookie
	{
	  protected string _CommentURL = "";
	  protected bool _Discard;
	  protected List<int> _PortList = new List<int>();
	  
	  protected override string GetCookie()
	  {
	    return base.GetCookie();
	  }

    protected override void LoadProperties(SortedList<string, string> APropertyList)
	  {
	    base.LoadProperties(APropertyList);
	    _CommentURL = APropertyList["COMMENTURL"];
	    _Discard = APropertyList.ContainsKey("DISCARD");
      string S = APropertyList["Port"];
	    if (S.Length > 0)
	    {
	      if (S[0] == '"'
	        &&S[S.Length - 1] == '"')
        {
          List<string> TempPorts = new List<string>(S.Substring(1, S.Length - 2).Split(','));
          if (TempPorts.Count == 0)
          {
            PortList(0, Genetibase.Network.Sockets.TcpPorts.Http);
          }
          else
          {
            int index = 0;
            TempPorts.ForEach(delegate(string item) {
              int TempValue = -1;
              if (Int32.TryParse(item, out TempValue)) {
                PortList(index, TempValue);
                index++;
              }
            });
          }
        }
	    }
	    else
	    {
	      PortList(0, Genetibase.Network.Sockets.TcpPorts.Http);
	    }
	  }

    public RFC2965Cookie()
    {
      _InternalVersion = CookieVersionEnum.RFC2965;
    }
    
    public void PortList(int AIndex, int AValue)    
    {
      if (AIndex - _PortList.Count > 1
        ||AIndex < 0)
      {
        throw new ArgumentOutOfRangeException("AIndex");
      }
      if (AIndex - _PortList.Count == 1)
      {
        _PortList.Add(AValue);
      }
      _PortList[AIndex] = AValue;
    }
    
    public int PortList(int AIndex)
    {
      if (AIndex > _PortList.Count
        ||AIndex < 0)
      {
        throw new ArgumentOutOfRangeException("AIndex");
      }
      return (int)_PortList[AIndex];
    }
    
    public string CommentURL
    {
      get
      {
        return _CommentURL;
      }
      set
      {
        _CommentURL = value;
      }
    }
    
    public bool Discard
    {
      get
      {
        return _Discard;
      }
      set
      {
        _Discard = value;
      }
    }
  }
}

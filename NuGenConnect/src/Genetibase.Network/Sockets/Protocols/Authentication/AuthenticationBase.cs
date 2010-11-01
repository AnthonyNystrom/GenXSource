using System;
using System.Collections.Generic;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Sockets.Protocols.Authentication
{
	public abstract class AuthenticationBase: IDisposable
	{
	  void IDisposable.Dispose()
	  {
	  }
	  protected int _CurrentStep;
	  protected HeaderList _Params;
	  protected HeaderList _AuthParams;
	  
	  protected string ReadAuthInfo(string AuthName)
	  {
	    for (int i = 0; i < _AuthParams.Count; i++)
	    {
	      if (_AuthParams.Names(i) == AuthName)
	      {
	        return _AuthParams.ValueFromIndex(i);
	      }
	    }
	    return "";
	  }
	  
	  protected abstract AuthenticationWhatsNextEnum DoNext();
	  protected void SetAuthParams(HeaderList AValue)
	  {
      List<string> TempList = new List<string>();
      AValue.ConvertToStdValues(TempList);
      _AuthParams.AddStdValues(TempList);
	  }
	  
	  protected string GetPassword()
	  {
      return _Params.Values("password");
	  }
	  
	  protected string GetUserName()
	  {
	    return _Params.Values("username");
	  }
	  
	  protected virtual int GetSteps()
	  {
	    return 0;
	  }
	  
	  protected virtual void SetPassword(string AValue)
	  {
	    _Params.Values("password", AValue);
	  }
	  
	  protected virtual void SetUserName(string AValue)
	  {
	    _Params.Values("username", AValue);
	  }
	  
	  protected AuthenticationBase()
	  {
	    _Params = new HeaderList();
	    _AuthParams = new HeaderList();
	    _CurrentStep = 0;
	    Reset();
	  }
	  
	  ~AuthenticationBase()
	  {
	    _Params.Clear();
	    _Params = null;
	    _AuthParams.Clear();
	    _AuthParams = null;
	  }
	  
	  public virtual void Reset()
	  {
	  }
	  
	  public abstract string Authentication();
	  public abstract bool KeepAlive();
	  public AuthenticationWhatsNextEnum Next()
	  {
	    return DoNext();
	  }
	  
	  public HeaderList AuthParams
	  {
	    get
	    {
	      return _AuthParams;
	    }
	    set
	    {
	      SetAuthParams(value);
	    }
	  }
	  
	  public HeaderList Params
	  {
	    get
	    {
	      return _Params;
	    }
	  }
	  
	  public string UserName
	  {
	    get
	    {
	      return GetUserName();
	    }
	    set
	    {
	      SetUserName(value);
	    }
	  }
	  
	  public string Password
	  {
	    get
	    {
	      return GetPassword();
	    }
	    set
	    {
	      SetPassword(value);
	    }
	  }
	  
	  public int Steps
	  {
	    get
	    {
	      return GetSteps();
	    }
	  }
	  
	  public int CurrentStep
	  {
	    get
	    {
	      return _CurrentStep;
	    }
	  }
	  
	  public abstract void DecodeAuthParams(string AParams);
	}
}

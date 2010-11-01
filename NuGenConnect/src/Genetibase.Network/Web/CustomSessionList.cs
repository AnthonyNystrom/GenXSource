using System;

namespace Genetibase.Network.Web
{
	public abstract class CustomSessionList: IDisposable
	{
	  private int _SessionTimeOut;
	  private SessionEventHandler _OnSessionStart;
	  private SessionEventHandler _OnSessionEnd;
	  
	  internal void SessionStart(HttpSession ASession)
	  {
	    if (_OnSessionStart != null)
	    {
	      _OnSessionStart(ASession);
	    }
	  }
	  
	  internal void SessionEnd(HttpSession ASession)
	  {
	    if (_OnSessionEnd != null)
	    {
	      _OnSessionEnd(ASession);
	    }
	  }
	  
	  internal void RemoveSession(HttpSession ASession)
	  {
	    DoRemoveSession(ASession);
	  }
	  
	  protected abstract void DoRemoveSession(HttpSession ASession);
    public abstract void Clear();
	  public void PurgeStaleSessions()
	  {
	    PurgeStaleSessions(false);
	  }
	  
    public abstract void PurgeStaleSessions(bool PurgeAll);
	  public abstract HttpSession CreateUniqueSession(string RemoteIP);
	  public abstract HttpSession CreateSession(string RemoteIP, string SessionId);
	  public abstract HttpSession GetSession(string SessionId, string RemoteIP);
	  public abstract void Add(HttpSession ASession);

    // in seconds
    public int SessionTimeOut
    {
      get
      {
        return _SessionTimeOut;
      }
      set
      {
        _SessionTimeOut = value;
      }
    }
    
    public event SessionEventHandler OnSessionStart
    {
      add
      {
        _OnSessionStart += value;
      }
      remove
      {
        _OnSessionStart -= value;
      }
    }

    public event SessionEventHandler OnSessionEnd
    {
      add
      {
        _OnSessionEnd += value;
      }
      remove
      {
        _OnSessionEnd -= value;
      }
    }

		public virtual void Dispose() {
			Clear();
			GC.SuppressFinalize(this);
		}
	}
}

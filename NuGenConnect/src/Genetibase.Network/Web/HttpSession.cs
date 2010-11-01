using System;
using System.Collections.Generic;
using System.Threading;

namespace Genetibase.Network.Web {
  public class HttpSession: IDisposable {
    void IDisposable.Dispose() {
      DoSessionEnd();
      _Content.Clear();
      _Content = null;
      if (_Owner != null) {
        _Owner.RemoveSession(this);
      }
    }

    protected SortedList<string, object> _Content;
    protected DateTime _LastTimeStamp;
    protected CustomSessionList _Owner;
    protected string _SessionId = "";
    protected string _RemoteHost = "";

    internal virtual bool IsSessionStale() {
			return Http.TimeStampInterval(_LastTimeStamp, DateTime.Now) > _Owner.SessionTimeOut;
    }

    internal void SetLastTimeStamp(DateTime ATimeStamp) {
      _LastTimeStamp = ATimeStamp;
    }

    protected virtual void DoSessionEnd() {
      if (_Owner != null) {
        _Owner.SessionEnd(this);
      }
    }

    internal void SessionEnd() {
      DoSessionEnd();
    }

    public HttpSession(CustomSessionList AOwner) {
      this._Owner = AOwner;
      _Content = new SortedList<string,object>();
      if (AOwner != null) {
        AOwner.SessionStart(this);
      }
    }

    public HttpSession(CustomSessionList AOwner, string ASessionId, string ARemoteIP) {
      _SessionId = ASessionId;
      _RemoteHost = ARemoteIP;
      _LastTimeStamp = DateTime.Now;
      _Content = new SortedList<string,object>();
      _Owner = AOwner;
      if (AOwner != null) {
        AOwner.SessionStart(this);
      }
    }

    public SortedList<string, object> Content {
      get {
        return _Content;
      }
    }

    public DateTime LastTimeStamp {
      get {
        return _LastTimeStamp;
      }
    }

    public string RemoteHost {
      get {
        return _RemoteHost;
      }
    }

    public string SessionId {
      get {
        return _SessionId;
      }
    }

    public void Lock() {
      Monitor.Enter(this);
    }

    public void Unlock() {
      Monitor.Exit(this);
    }
  }
}

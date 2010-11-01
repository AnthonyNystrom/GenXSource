using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public abstract class ServerSocket
  {
    private ServerIntercept mIntercept = null;

    public abstract void StartListening();
    public abstract void StopListening();

    public abstract int Port
    {
      get;
      set;
    }


    protected abstract Socket DoAccept();

    public Socket Accept() {
      if (mIntercept == null) {
        Socket TempResult = DoAccept();
        TempResult.Open();
        return TempResult;
      } else {
        Socket TempResult = DoAccept();
        if (TempResult != null) {
          TempResult.Intercept = mIntercept.Accept(TempResult);
        }
        TempResult.Open();
        return TempResult;
      }
    }

    public virtual void Init()
    {
      if (mIntercept != null) {
        mIntercept.Init();
      }
    }

    public virtual void Shutdown()
    {
      if (mIntercept != null) {
        mIntercept.Shutdown();
      }
    }

    public ServerIntercept Intercept {
      get {
        return mIntercept;
      }
      set {
        mIntercept = value;
      }
    }
  }
}
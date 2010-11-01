using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
  public abstract class ServerIntercept {

    private ServerIntercept mIntercept;

    private void CheckForCircularLink(ServerIntercept intercept) {
      if (mIntercept != null) {
        if (mIntercept == intercept) {
          throw new Exception("Circular links with intercepts not allowed!");
        }
        mIntercept.CheckForCircularLink(intercept);
      }
    }

    protected virtual void DoInit() {
    }

    public void Init() {
      DoInit();
      if (mIntercept != null) {
        mIntercept.Init();
      }
    }

    /*
     * MtW:
     *    Change this. Right now when having nested intercepts, you need to have
     *    if statements in Accept, this is not nice.
     * 
     *    Also, should it be possible to have an intercept which does not return a 
     *    value here? 
     *    
     */
    protected virtual ConnectionIntercept DoAccept(object connection) {
      return null;
    }

    public ConnectionIntercept Accept(object connection) {
      ConnectionIntercept TempResult = null;
      if (mIntercept != null) {
        TempResult = mIntercept.Accept(connection);
      }
      if (TempResult != null) {
        TempResult.Intercept = DoAccept(connection);
      } else {
        TempResult = DoAccept(connection);
      }
      return TempResult;
    }

    protected virtual void DoShutdown() {
    }

    public void Shutdown() {
      if (mIntercept != null) {
        mIntercept.Shutdown();
      }
      DoShutdown();
    }

    public ServerIntercept Intercept {
      get {
        return mIntercept;
      }
      set {
        if (mIntercept != value) {
          if (value == null) {
            mIntercept = null;
            return;
          }
          value.CheckForCircularLink(this);
          mIntercept = value;
        }
      }
    }
  }
}

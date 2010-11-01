using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public abstract class ConnectionIntercept: IDisposable
  {
    private ConnectionIntercept mIntercept;

    private void CheckForCircularLink(ConnectionIntercept intercept)
    {
      if (mIntercept != null)
      {
        if (mIntercept == intercept)
        {
          throw new Exception("Circular links with intercepts not allowed!");
        }
        mIntercept.CheckForCircularLink(intercept);
      }
    }

    public abstract void Connect(object sender);
    public abstract void Disconnect();
    public abstract void Receive(ref byte[] buffer);
    public abstract void Send(ref byte[] buffer);

    public ConnectionIntercept Intercept
    {
      get
      {
        return mIntercept;
      }
      set
      {
        if (mIntercept != value)
        {
          if (value == null)
          {
            mIntercept = null;
            return;
          }
          value.CheckForCircularLink(this);
          mIntercept = value;
        }
      }
    }

    public void Dispose() {
      GC.SuppressFinalize(this);
    }
  }
}
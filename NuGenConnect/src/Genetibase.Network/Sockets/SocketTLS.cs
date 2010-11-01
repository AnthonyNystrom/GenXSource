using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
  public abstract class SocketTLS : Socket {
    private bool mPassThrough = true;
    private bool mIsPeer;
    private string mURIToCheck;

    public abstract void StartSSL();

    public virtual bool PassThrough {
      get {
        return mPassThrough;
      }
      set {
        mPassThrough = value;
      }
    }

    public bool IsPeer {
      get {
        return mIsPeer;
      }
      set {
        mIsPeer = value;
      }
    }

    public string URIToCheck {
      get {
        return mURIToCheck;
      }
      set {
        mURIToCheck = value;
      }
    }
  }
}
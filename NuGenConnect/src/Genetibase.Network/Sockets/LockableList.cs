using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public class LockableList<T>: List<T>
  {
    //private ReaderWriterLock mLock = new ReaderWriterLock();
    private object mLockObj = new Object();
    
    public void AcquireReaderLock()
    {
      Monitor.Enter(mLockObj);
    }

    public void AcquireWriterLock()
    {
      Monitor.Enter(mLockObj);
    }

    public void ReleaseWriterLock()
    {
      Monitor.Exit(mLockObj);
    }

    public void ReleaseReaderLock()
    {
      Monitor.Exit(mLockObj);
    }
  }
}
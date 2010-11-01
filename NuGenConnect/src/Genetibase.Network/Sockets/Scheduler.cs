using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Genetibase.Network.Sockets {
  public abstract class Scheduler {
    protected LockableList<Yarn> mActiveYarns = new LockableList<Yarn>();

    public abstract Yarn AcquireYarn();
    public virtual void Init() {
    }

    public virtual void ReleaseYarn(Yarn yarn) {
      mActiveYarns.AcquireWriterLock();
      try {
        mActiveYarns.Remove(yarn);
      } finally {
        mActiveYarns.ReleaseWriterLock();
      }
    }

    public abstract void StartYarn(Yarn yarn, Task task);

    public abstract void TerminateYarn(Yarn yarn);

    public virtual void TerminateAllYarns() {
      while (true) {
        try {
          mActiveYarns.AcquireReaderLock();
          if (mActiveYarns.Count == 0) {
            break;
          }
          for (int i = mActiveYarns.Count - 1; i >= 0; i -= 1) {
            TerminateYarn(mActiveYarns[i]);
          }
        } finally {
          mActiveYarns.ReleaseReaderLock();
        }
        Thread.Sleep(500);
      }
    }

    public LockableList<Yarn> ActiveYarns {
      get {
        return mActiveYarns;
      }
    }
  }
}
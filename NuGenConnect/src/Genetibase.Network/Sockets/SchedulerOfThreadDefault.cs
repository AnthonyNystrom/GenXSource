using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public class SchedulerOfThreadDefault: SchedulerOfThread<ThreadWithTask>
  {
    public override Yarn AcquireYarn()
    {
/*      Yarn TempYarn = NewYarn(NewThread());
      mActiveYarns.AcquireWriterLock();
      try
      {
        this.mActiveYarns.Add(TempYarn);
      }
      finally
      {
        mActiveYarns.ReleaseWriterLock();
      }
      return TempYarn;  */
      return NewYarn(NewThread());
    }
  }
}

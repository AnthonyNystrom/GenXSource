using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Genetibase.Network.Sockets
{
  public class YarnOfThread: Yarn
  {
    protected Scheduler mScheduler;
    protected ThreadWithTask mThread;

    internal void TaskBeforeRun(object sender, EventArgs e)
    {
      mScheduler.ActiveYarns.AcquireWriterLock();
      try
      {
        mScheduler.ActiveYarns.Add(this);
      }
      finally
      {
        mScheduler.ActiveYarns.ReleaseWriterLock();
      }
    }

    public YarnOfThread(Scheduler mScheduler, ThreadWithTask mThread)
    {
      this.mScheduler = mScheduler;
      this.mThread = mThread;
    }

    public override void Dispose()
    {
      mScheduler.ReleaseYarn(this);
      base.Dispose();
    }

    public ThreadWithTask Thread
    {
      get
      {
        return mThread;
      }
    }
  }

  public abstract class SchedulerOfThread<TThreadType>: Scheduler where TThreadType: ThreadWithTask, new()
  {
    protected int mMaxThreads = 0;
    protected ThreadPriority mThreadPriority = ThreadPriority.Normal;

    public SchedulerOfThread()
    {
    }

    ~SchedulerOfThread()
    {
      TerminateAllYarns();
    }

    public TThreadType NewThread()
    {
      ActiveYarns.AcquireWriterLock();
      try
      {
        if ((mMaxThreads != 0)
        && (ActiveYarns.Count >= mMaxThreads))
        {
          throw new IndyException("Maximum thread number reached!");
        }
      }
      finally
      {
        ActiveYarns.ReleaseWriterLock();
      }
      TThreadType thread = new TThreadType();
      //thread.Task = 
      thread.Priority = mThreadPriority;
      return thread;
    }

    public YarnOfThread NewYarn(TThreadType thread)
    {
      if (thread == null)
      {
        throw new ArgumentNullException("thread");
      }
      return new YarnOfThread(this, thread);
    }

    public override void StartYarn(Yarn yarn, Task task)
    {
      YarnOfThread yarnOfThread = ((YarnOfThread)yarn);
      yarnOfThread.Thread.Task = task;
      task.OnBeforeRun += yarnOfThread.TaskBeforeRun;
      yarnOfThread.Thread.Start();
    }

    public override void TerminateYarn(Yarn yarn)
    {
      YarnOfThread LYarn = (YarnOfThread)yarn;
      if (LYarn.Thread.TheThread == null)
      {
        this.ReleaseYarn(yarn);
        return;
      }
      if (LYarn.Thread.TheThread.ThreadState != ThreadState.Unstarted)
      {
        LYarn.Thread.Stop();
      }
    }

    public int MaxThreads
    {
      get
      {
        return MaxThreads;
      }
      set
      {
        mMaxThreads = value;
      }
    }

    public ThreadPriority ThreadPriority
    {
      get
      {
        return mThreadPriority;
      }
      set
      {
        mThreadPriority = value;
      }
    }
  }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Genetibase.Network.Sockets
{
  public class ThreadWithTask: IDisposable
  {
    protected Task mTask;
    protected Thread thread;

    public ThreadWithTask()
      : this(null)
    {
    }

    public ThreadWithTask(Task mTask)
    {
      this.mTask = mTask;
      thread = new Thread(Run);
      thread.IsBackground = true;
    }

    public Thread TheThread
    {
      get
      {
        return thread;
      }
    }

    public ThreadPriority Priority
    {
      get
      {
        return thread.Priority;
      }
      set
      {
        thread.Priority = value;
      }
    }

    protected void Run()
    {
      try {
        Task lTask = mTask;
        GC.SuppressFinalize(lTask);
        try {
          mTask.DoBeforeRun();
          try {
            while (mTask.DoRun()) {
            }
          } finally {
            mTask.DoAfterRun();
          }
        } finally {
          GC.ReRegisterForFinalize(lTask);
        }
      }catch(ThreadAbortException) {
        throw;
      } catch (Exception) {
      }
    }

    public void Start()
    {
      thread.Start();
    }

    public void Stop()
    {
      thread.Abort();
      thread = null;
    }

    public Task Task
    {
      get
      {
        return mTask;
      }
      set
      {
        mTask = value;
      }
    }

    public void Dispose()
    {
      if (mTask != null)
      {
        if (mTask.Yarn != null)
        {
          mTask.Yarn.Dispose();
        }
      }
      GC.SuppressFinalize(this);
    }
  }
}
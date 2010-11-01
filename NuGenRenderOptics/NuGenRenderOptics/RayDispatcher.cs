using System.Collections.Generic;
using System.Threading;

namespace NuGenRenderOptics
{
    interface RayDispatchEventHandler
    {
        void ExecutionComplete(RayDispatch dispatch);
        void QueueDispatch(RayGroup group);
    }

    interface IRayDispatch
    {
        void ExecutionComplete();
        void QueueDispatch(RayGroup group);
        void QueueDispatch(Ray ray);
        void RaysTraced(int add);
    }

    class RayDispatch : IRayDispatch
    {
        public Thread Thread;
        public object Instructions;

        RayDispatcher dispatcher;

        public RayDispatch(ThreadStart threadStart, object instructions,
                           RayDispatcher dispatcher)
        {
            Thread = new Thread(threadStart);
            Instructions = instructions;
            this.dispatcher = dispatcher;

            if (instructions is RayGroup)
                ((RayGroup)instructions).dispatch = this;

            Thread.Start();
        }

        #region IRayDispatch Members

        public void ExecutionComplete()
        {
            dispatcher.ExecutionComplete(this);
        }

        public void QueueDispatch(RayGroup group)
        {
            dispatcher.QueueDispatch(group);
        }

        public void QueueDispatch(Ray ray)
        {
            throw new System.NotImplementedException();
        }

        public void RaysTraced(int add)
        {
            dispatcher.AddRaysTraced(add);
        }
        #endregion
    }

    /// <summary>
    /// Encapsulates a control mechanism responsible for dispatching
    /// thread instructions.
    /// </summary>
    class RayDispatcher : RayDispatchEventHandler
    {
        readonly int maxNumThreads;
        readonly List<RayDispatch> dispatches;
        readonly Queue<object> instQueue;

        uint processedThreads;
        uint processedRays;
        
        public RayDispatcher(int maxNumThreads)
        {
            this.maxNumThreads = maxNumThreads;

            dispatches = new List<RayDispatch>();
            instQueue = new Queue<object>();
        }

        public int MaxNumThreads
        {
            get { return maxNumThreads; }
        }

        public int ActiveThreadsCount
        {
            get { lock (dispatches) { return dispatches.Count; } }
        }

        public uint ProcessedThreads
        {
            get { return processedThreads; }
        }

        public uint ProcessedRays
        {
            get { return processedRays; }
        }

        public void DispatchRayGroupReq(RayGroup group)
        {
            // try and add
            lock (dispatches)
            {
                if (dispatches.Count < maxNumThreads)
                {
                    dispatches.Add(new RayDispatch(group.Trace, group, this));
                }
            }
        }

        public void DispatchRayReq()
        {
            throw new System.NotImplementedException();
        }

        #region RayDispatchEventHandler Members

        public void ExecutionComplete(RayDispatch dispatch)
        {
            lock (dispatches)
            {
                dispatches.Remove(dispatch);

                // check for queued items to replace thread with
                lock (instQueue)
                {
                    if (instQueue.Count > 0)
                    {
                        RayGroup group = (RayGroup)instQueue.Dequeue();
                        dispatches.Add(new RayDispatch(group.Trace, group, this));
                    }
                }
                processedThreads++;
            }
        }

        public void QueueDispatch(RayGroup group)
        {
            lock (instQueue)
            {
                instQueue.Enqueue(group);
            }
        }
        #endregion

        public void WaitForCompletion()
        {
            while (true)
            {
                lock (dispatches)
                {
                    if (dispatches.Count == 0)
                    {
                        break;
                    }
                }
                Thread.Sleep(500);
            }
        }

        public void AddRaysTraced(int add)
        {
            lock (this)
            {
                processedRays += (uint)add;
            }
        }
    }
}
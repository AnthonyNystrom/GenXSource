using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreadOperations {    
    
    delegate float dlgtFunction(float V);

    class TAM : IDisposable { // Threaded Array Manipulator

        #region Class members

        private Operation m_CurrentOperation;
        private int[] m_ArrayIndex;
        private bool m_Ready;
        private double[] m_ArrayA8; // Future Use
        private double[] m_ArrayB8; // Future Use
        private double[] m_ArrayC8; // Future Use
        private double[] m_ArrayD8; // Future Use
        private float[] m_ArrayA4;
        private float[] m_ArrayB4;
        private float[] m_ArrayC4;
        private float[] m_ArrayD4; // Future Use
        private double m_RegA8; 
        private double m_RegB8; // Future Use
        private double m_RegC8; // Future Use
        private double m_RegD8; // Future Use
        private float m_RegA4;
        private float m_RegB4; // Future Use
        private float m_RegC4; // Future Use
        private float m_RegD4; // Future Use

        private dlgtFunction m_Func;

        private object m_SyncObj;
        private Thread[] m_WorkingThreads;        
        private EventWaitHandle m_GoEvent;
        private WaitHandle[] m_ReadyEventWaitVector; 
        private WaitHandle[] m_FinishEventWaitVector;
        private int m_NumberOfWorkers;

        #endregion Class members

        public TAM(int NumberOfThreads) {
            this.m_Ready = false;
            this.m_SyncObj = new object();
            this.m_NumberOfWorkers = NumberOfThreads;
            this.m_GoEvent = new EventWaitHandle(false, EventResetMode.ManualReset);            
            this.m_ArrayIndex = new int[NumberOfThreads + 1];
            this.m_WorkingThreads = new Thread[NumberOfThreads]; // worker threads array
            this.m_FinishEventWaitVector = new WaitHandle[NumberOfThreads]; // finish event vector array
            this.m_ReadyEventWaitVector = new WaitHandle[NumberOfThreads]; // finish event vector array
            for (int i = 0; i < NumberOfThreads; i++) { // create the workers
                this.m_FinishEventWaitVector[i] = new ManualResetEvent(false);
                this.m_ReadyEventWaitVector[i] = new ManualResetEvent(false);
                this.m_WorkingThreads[i] = new Thread(new ParameterizedThreadStart(WorkerThread));
                this.m_WorkingThreads[i].Name = "TAM Worker " + i;
                this.m_WorkingThreads[i].Start(i); // also start them
            }
            System.Threading.WaitHandle.WaitAll(this.m_FinishEventWaitVector);
            for (int i = 0; i < NumberOfThreads; i++) ((ManualResetEvent)this.m_FinishEventWaitVector[i]).Reset();            
            this.m_Ready = true;
        }

        #region IDisposable Members

        public void Dispose() {
            this.m_Ready = false;
            for (int i = 0; i < this.m_WorkingThreads.Length; i++) { // abort all events. TODO add exit event to workers
                if (this.m_WorkingThreads[i] != null) {
                    this.m_WorkingThreads[i].Abort();
                    this.m_WorkingThreads[i] = null;
                }
            }
        }

        #endregion IDisposable Members

        #region Publics

        public bool Ready {
            get { return m_Ready; }            
        }

        public float[] Add(float[] ArrA, float[] ArrB) {
            if (ArrA.Length != ArrB.Length) return null;
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Add;            
            this.m_ArrayA4 = ArrA;
            this.m_ArrayB4 = ArrB;
            this.m_ArrayC4 = new float[ArrA.Length];
            this.SetIndexArray(ArrA.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_ArrayC4;
        }
        
        public double Sum(float[] Arr) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Sum;
            this.m_RegA8 = 0;
            this.m_ArrayA4 = Arr;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_RegA8;
        }

        public double Avg(float[] Arr) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Sum;
            this.m_RegA8 = 0;
            this.m_ArrayA4 = Arr;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_RegA8 / Arr.Length;
        }

        public float Max(float[] Arr) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Max;
            this.m_RegA4 = float.MinValue;
            this.m_ArrayA4 = Arr;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_RegA4;
        }

        public float Min(float[] Arr) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Min;
            this.m_RegA4 = float.MaxValue;
            this.m_ArrayA4 = Arr;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_RegA4;
        }

        public void Power(float[] Arr, double Power) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Power;            
            this.m_ArrayA4 = Arr;
            this.m_RegA8 = Power;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;            
        }

        public void Function(float[] Arr, dlgtFunction Func) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.CostumFunction;
            this.m_ArrayA4 = Arr;
            this.m_Func = Func;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
        }

        public double Test(float[] Arr) {
            this.m_Ready = false;
            this.m_CurrentOperation = Operation.Test;
            this.m_RegA8 = 0;
            this.m_ArrayA4 = Arr;
            this.SetIndexArray(Arr.Length);
            this.GO();
            this.m_Ready = true;
            return this.m_RegA8;
        }

        #endregion Publics

        #region Privates

        ~TAM(){
            this.Dispose();
        }

        private enum Operation {
            Test,
            Sum,
            Add,
            Max,
            Min,
            Power,
            CostumFunction
        }
        
        private void WorkerThread(object ThreadID) {
            int ID = (int)ThreadID;
            
            ManualResetEvent FinishEvent = (ManualResetEvent)this.m_FinishEventWaitVector[ID];
            ManualResetEvent ReadyEvent = (ManualResetEvent)this.m_ReadyEventWaitVector[ID];
            WaitHandle[] MainEventWaitVector = new WaitHandle[1];
            MainEventWaitVector[0] = this.m_GoEvent;
            FinishEvent.Set();
                        
            while (true) {
                try {
                    ReadyEvent.Reset();
                    WaitHandle.WaitAny(MainEventWaitVector); // Wait for GO. TODO add exit event 
                   
                    switch (this.m_CurrentOperation) {
                        case Operation.Test :
                            double TestSum = 0;
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) TestSum += Math.Sin(Math.Sqrt(Math.Sqrt(m_ArrayA4[i] * Math.PI)) * 1.01);
                            lock (this.m_SyncObj) this.m_RegA8 += TestSum;                            
                            break;
                        case Operation.Sum:
                            double TmpSum = 0;
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) TmpSum += m_ArrayA4[i];
                            lock (this.m_SyncObj) this.m_RegA8 += TmpSum;                            
                            break;
                        case Operation.Add:                            
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) m_ArrayC4[i] = m_ArrayA4[i] + m_ArrayB4[i];                                                        
                            break;
                        case Operation.Max:
                            float TmpMax = float.MinValue;
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) TmpMax = Math.Max(m_ArrayA4[i],TmpMax);
                            lock (this.m_SyncObj) this.m_RegA4 = Math.Max(this.m_RegA4, TmpMax);                            
                            break;
                        case Operation.Min:
                            float TmpMin = float.MinValue;
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) TmpMin = Math.Min(m_ArrayA4[i], TmpMin);
                            lock (this.m_SyncObj) this.m_RegA4 = Math.Min(this.m_RegA4, TmpMin);                            
                            break;                        
                        case Operation.Power:                            
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) m_ArrayA4[i] = (float)Math.Pow((double)m_ArrayA4[i],m_RegA8);                            
                            break;
                        case Operation.CostumFunction:
                            for (int i = this.m_ArrayIndex[ID]; i < this.m_ArrayIndex[ID + 1]; i++) m_ArrayA4[i] = this.m_Func(m_ArrayA4[i]);
                            break;
                        default :
                            throw new Exception("Unknown Operation.");
                    }
                    FinishEvent.Set();
                    ReadyEvent.WaitOne();
                }catch(ThreadAbortException){ // On Abort do nothing
                } catch (Exception ex) { // Other exception
                    Console.WriteLine("ex: " + ex.Message); 
                }
            }
        }

        private void SetIndexArray(int ArraySize) { // Populate m_ArrayIndex (devide the array to the workers)
            this.m_ArrayIndex[0] = 0; // make sure none is missed
            this.m_ArrayIndex[this.m_ArrayIndex.Length - 1] = ArraySize; // make sure none is missed
            for (int i = 1; i < this.m_ArrayIndex.Length - 1; i++) {
                this.m_ArrayIndex[i] = ((ArraySize / this.m_NumberOfWorkers) * i);
            }
        }

        private void GO() { // Do the manipulation
            this.m_GoEvent.Set();
            System.Threading.WaitHandle.WaitAll(this.m_FinishEventWaitVector);
            this.m_GoEvent.Reset();
            for (int i = 0; i < this.m_FinishEventWaitVector.Length; i++) {
                ((System.Threading.ManualResetEvent)this.m_ReadyEventWaitVector[i]).Set();
                ((System.Threading.ManualResetEvent)this.m_FinishEventWaitVector[i]).Reset();
            }
        }

        #endregion Privates

        #region Debug

        static float AAA(float Val) {            
            return (float)Math.Sin(Math.Sqrt(Math.Sqrt((double)Val * Math.PI)) * 1.01);
        }

        static void Main() {

            // In this ADT example there function AAA preforms a mathematical oprtation and is sent as a delegate to TAM.
            dlgtFunction Func = AAA;         
            
            // Create a float array
            Random R = new Random();
            float[] A = new float[5000000];            
            for(int i=0;i<A.Length;i++){
                A[i] = (float)R.NextDouble();            
            }
            
            TAM TamA = new TAM(2); // Set number of worker threads.            

            //GC.Collect();

            //Test With TAM                                    
            TamA.Function(A,Func);            
            
            //GC.Collect();

            //Test Without TAM                                   
            for (int i = 0; i < A.Length; i++) {                
                A[i] = (float)Math.Sin(Math.Sqrt(Math.Sqrt(A[i] * Math.PI)) * 1.01);
            }

            TamA.Dispose();
                        
        }

        #endregion Debug
        
    }

}

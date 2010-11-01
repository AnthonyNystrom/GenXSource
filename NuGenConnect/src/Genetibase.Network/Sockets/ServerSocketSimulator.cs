using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace Genetibase.Network.Sockets {
  public class ServerSocketSimulator : ServerSocketTLS {
    private class ServerSocketSimulatorHelperIntercept : ConnectionIntercept {
      public event EventHandler OnDisconnect;

      public ServerSocketSimulatorHelperIntercept(EventHandler onDisconnect) {
        OnDisconnect += onDisconnect;
      }

      public override void Connect(object sender) {
      }

      public override void Disconnect() {
				OnDisconnect(null, null);
      }

      public override void Receive(ref byte[] buffer) {
      }

      public override void Send(ref byte[] buffer) {
      }
    }

    private class ServerSocketSimulatorHelperServerIntercept : ServerIntercept {
      public EventHandler onDisconnect;

      public ServerSocketSimulatorHelperServerIntercept(EventHandler onDisconnect/*, EventHandler onShutdown*/) {
        this.onDisconnect = onDisconnect;
      }

      protected override ConnectionIntercept DoAccept(object connection) {
        return new ServerSocketSimulatorHelperIntercept(onDisconnect);
      }

      protected override void DoShutdown() {
        base.DoShutdown();
      }
    }

    private SortedList<string, XmlDocument> mSimulations = new SortedList<string, XmlDocument>(StringComparer.InvariantCultureIgnoreCase);
    private ReaderWriterLock mSimulationsLock = new ReaderWriterLock();
    private AutoResetEvent mFinishedEvent = new AutoResetEvent(false);
    private int mPort;
    private int mConnectionsLeft = 0;
    private Exception mException;

    protected override Socket DoAccept() {
      mSimulationsLock.AcquireWriterLock(-1);
      try {
        if (mSimulations.Count > 0) {
          SocketSimulator simulator = new SocketSimulator(mSimulations.Keys[0]);
          simulator.InitializeFromXmlDocument(mSimulations.Values[0]);
          mSimulations.RemoveAt(0);
          return simulator;
        } else {
          Thread.Sleep(Timeout.Infinite);
          return null;
        }
      } finally {
        mSimulationsLock.ReleaseWriterLock();
      }
    }

    private void SimulatorDisconnected(object sender, EventArgs e) {
			if (Interlocked.Decrement(ref mConnectionsLeft) == 0) {
        mFinishedEvent.Set();
      }
    }

    public void AddXmlDocument(XmlDocument xmlDoc, string name) {
      mSimulationsLock.AcquireWriterLock(-1);
      try {
        mSimulations.Add(name, xmlDoc);
      } finally {
        mSimulationsLock.ReleaseWriterLock();
      }
    }

    public void DoContextException<TReplyCode, TReply, TContext>(TContext sender, Exception e)
			where TContext: Context<TReplyCode, TReply, TContext>, new()
      where TReplyCode : IEquatable<TReplyCode>
      where TReply : Reply<TReplyCode>, new() {
      if (mException == null) {
        mException = e;
      }
    }

    public void DoListenException(object sender, Exception e) {
      if (mException == null) {
        mException = e;
      }
    }

    public override void Shutdown() {
      base.Shutdown();
      mFinishedEvent.Close();
    }

    public void WaitForFinish() {
      mFinishedEvent.WaitOne();
      if (mException != null) {
        // do not "throw mException", this will hide the stack info.
        throw new Exception("Exception occurred during test. Message: " + mException.Message, mException);
      }
    }

    public int SimulationsLeft {
      get {
        mSimulationsLock.AcquireReaderLock(-1);
        try {
          return mSimulations.Count;
        } finally {
          mSimulationsLock.ReleaseReaderLock();
        }
      }
    }

    public override void StartListening() {
      if (this.Intercept != null) {
        this.Intercept.Intercept = new ServerSocketSimulatorHelperServerIntercept(SimulatorDisconnected);
      } else {
        this.Intercept = new ServerSocketSimulatorHelperServerIntercept(SimulatorDisconnected);
      }
      mConnectionsLeft = SimulationsLeft;
      if (SimulationsLeft == 0) {
        mFinishedEvent.Set();
      }
    }

    public override void StopListening() {
    }

    public override int Port {
      get {
        return mPort;
      }
      set {
        mPort = value;
      }
    }
  }
}
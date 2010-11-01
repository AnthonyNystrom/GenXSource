using System;
using System.Collections.Generic;

namespace Genetibase.Network.Sockets
{
  public class TcpConnection<TReplyCode, TReply> 
    where TReply: Reply<TReplyCode>, new()
    where TReplyCode: IEquatable<TReplyCode>
	{
    protected TReply mGreeting;
    protected Socket mSocket;
    protected TReply mLastCmdResult;
    protected EventHandler mOnDisconnected;
    protected bool mManagedSocket;
    protected TReply NewReply()
    {
      return new TReply();
    }

    protected void CheckConnected()
    {
      if (!Connected())
      {
        throw new IndyException("Not Connected!");
      }
    }

    protected virtual void DoOnDisconnected()
    {
      if (mOnDisconnected != null)
      {
        mOnDisconnected(this, EventArgs.Empty);
      }
    }

//    protected void WorkBeginEvent(object ASender, WorkModeEnum AWorkMode, long AWorkCountMax)
//    {
//      BeginWork(AWorkMode, AWorkCountMax);
//    }
//
//    protected void WorkEvent(object ASender, WorkModeEnum AWorkMode, long AWorkCount)
//    {
//      DoWork(AWorkMode, AWorkCount);
//    }
//
//    protected void WorkEndEvent(object ASender, WorkModeEnum AWorkMode)
//    {
//      EndWork(AWorkMode);
//    }

    public TcpConnection()
    {
      mGreeting = NewReply();
      mLastCmdResult = NewReply();
    }

    public void CreateSocket()
    {
      CreateSocket(null);
    }

    public void CreateSocket(Type ABaseType)
    {
      if (Connected())
      {
        throw new IndyException("Cannot change IOHandler when connected!");
      }
      if (ABaseType != null)
      {
        if (!ABaseType.IsSubclassOf(typeof(Socket)))
        {
          throw new ArgumentException("", "ABaseType");
        }
        Socket = (Socket)Activator.CreateInstance(ABaseType, true);
      }
      else
      {
        throw new IndyException("Implement MakeDefaultIOHandler!");
        //IOHandler = Genetibase.Network.Sockets.IOHandler.MakeDefaultIOHandler();
      }
      ManagedSocket = true;
    }

    public void CheckForGracefulDisconnect()
    {
      CheckForGracefulDisconnect(true);
    }

    public virtual void CheckForGracefulDisconnect(bool ARaiseExceptionIfDisconnected)
    {
      if (Socket != null)
      {
        Socket.CheckForDisconnect(ARaiseExceptionIfDisconnected);
      }
      else
      {
        if (ARaiseExceptionIfDisconnected)
        {
          throw new IndyException(ResourceStrings.NotConnected);
        }
      }
    }

    public virtual TReplyCode CheckResponse(TReplyCode AResponse, params TReplyCode[] AAllowedResponses)
    { 
      if (AAllowedResponses.Length > 0)
      {
        foreach (TReplyCode i in AAllowedResponses)
        {
          if (AResponse.Equals(i))
          {
            return AResponse;
          }
        }
        ThrowExceptionForLastCmdResult();
      }
      return AResponse;
    }

    public virtual TReplyCode CheckResponse(TReplyCode AResponse, TReplyCode AAllowedResponse)
    {
      return CheckResponse(AResponse, new TReplyCode[] { AAllowedResponse });
    }

    public virtual bool Connected()
    {
      return
        ((Socket != null)
        && (Socket.Connected())
        );
    }

//    public override void DoWork(WorkModeEnum AWorkMode, long ACount)
//    { 
//      if (_OnWork != null)
//      {
//        _OnWork(this, AWorkMode, ACount);
//      }
//    }

    public void Disconnect()
    {
      Disconnect(true);
    }

    public virtual void Disconnect(bool ANotifyPeer)
    {
      try
      {
        if (ANotifyPeer)
        {
          if (Connected())
          {
            DisconnectNotifyPeer();
          }
        }
      }
      finally
      {
        if (mSocket != null)
        {
          if (mSocket.Connected())
          {
//            DoStatus(StatusEnum.Disconnecting);
            mSocket.Close();
            DoOnDisconnected();
//            DoStatus(StatusEnum.Disconnected);
          }
        }
      }
    }

    public virtual void DisconnectNotifyPeer()
    {
    }

    public virtual void GetInternalResponse()
    {
      CheckConnected();
      mLastCmdResult.ReadFromSocket(this.Socket);
    }

    public virtual TReplyCode GetResponse(params TReplyCode[] allowedResponses)
    {
      GetInternalResponse();
      return CheckResponse(mLastCmdResult.Code, allowedResponses);
    }

    public virtual TReplyCode GetResponse(TReplyCode allowedResponse)
    {
      GetInternalResponse();
      return CheckResponse(mLastCmdResult.Code, allowedResponse);
    }

    public TReply Greeting
    {
      get
      {
        return mGreeting;
      }
      set
      {
        mGreeting = value;
      }
    }

    public void ThrowExceptionForLastCmdResult()
    {
      mLastCmdResult.ThrowReplyError();
    }

    public TReplyCode SendCmd(byte[] AOut)
    {
      return SendCmd(AOut, default(TReplyCode));
    }

    public TReplyCode SendCmd(string AOut)
    {
      return SendCmd(AOut, default(TReplyCode));
    }

    public TReplyCode SendCmd(byte[] AOut, TReplyCode AResponse)
    {
      if (AResponse == null
        ||AResponse.Equals(default(TReplyCode)))
      {
        return SendCmd(AOut, new TReplyCode[] { });
      }
      else
      {
        return SendCmd(AOut, new TReplyCode[] { AResponse });
      }
    }

    public TReplyCode SendCmd(string AOut, TReplyCode AResponse)
    {
      if (AResponse == null
        ||AResponse.Equals(default(TReplyCode)))
      {
        return SendCmd(AOut, new TReplyCode[] { });
      }
      else
      {
        return SendCmd(AOut, new TReplyCode[] { AResponse });
      }
    }

    public virtual TReplyCode SendCmd(string AOut, params TReplyCode[] AResponses)
    {
      return SendCmd(Socket.Encoding.GetBytes(AOut + Global.EOL), AResponses);
    }

    public virtual TReplyCode SendCmd(byte[] AOut, params TReplyCode[] AResponses)
    {
      CheckConnected();
      Socket.Write(AOut);
      return GetResponse(AResponses);
    }

    public void WriteHeader(Dictionary<string, string> AHeader)
    {
      CheckConnected();
//      Socket.WriteBufferOpen();
      try
      {
        foreach(KeyValuePair<string, string> kvp in AHeader)
        {
          if (!String.IsNullOrEmpty(kvp.Value))
          { 
            Socket.WriteLn(kvp.Key + ": " + kvp.Value);
          }
          else
          {
            Socket.WriteLn(kvp.Key);
          }
        }
      }
      finally
      {
//        IOHandler.WriteBufferClose();
      }
    }

    public void WriteRFCStrings(IList<String> AStrings)
    {
      CheckConnected();
      foreach (string s in AStrings)
      {
        if (s == ".")
        {
          Socket.WriteLn("..");
        }
        else
        {
          Socket.WriteLn(s);
        }
      }
      Socket.WriteLn(".");
    }

    public TReply LastCmdResult
    {
      get
      {
        return mLastCmdResult;
      }
    }

    public bool ManagedSocket
    {
      get
      {
        return mManagedSocket;
      }
      set
      {
        mManagedSocket = value;
      }
    }

    public virtual Socket Socket
    {
      get
      {
        return mSocket;
      }
      set
      {
        if (value != Socket)
        {
          if (ManagedSocket && mSocket != null)
          {
            mSocket = null;
          }
          ManagedSocket = false;
          if (mSocket != null)
          {
            //          mSocket.OnWork -= new WorkEventHandler(WorkEvent);
            //          mSocket.OnWorkBegin -= new WorkBeginEventHandler(WorkBeginEvent);
            //          mSocket.OnWorkEnd -= new WorkEndEventHandler(WorkEndEvent);
          }
        }
        if (value != null)
        {
          //        AValue.OnWork += new WorkEventHandler(WorkEvent);
          //        AValue.OnWorkBegin += new WorkBeginEventHandler(WorkBeginEvent);
          //        AValue.OnWorkEnd += new WorkEndEventHandler(WorkEndEvent);
        }
        //      if (AValue != null
        //        &&AValue is IOHandlerSocket)
        //      {
        //        _Socket = (IOHandlerSocket)AValue;
        //      }
        //      else
        //      {
        //        _Socket = null;
        //      }
        mSocket = value;        
      }
    }

    public event EventHandler OnDisconnected
    {
      add
      {
        mOnDisconnected += value;
      }
      remove
      {
        mOnDisconnected -= value;
      }
    }
    
//    public override event WorkBeginEventHandler OnWorkBegin;
//    public override event WorkEventHandler OnWork;
//    public override event WorkEndEventHandler OnWorkEnd;
  }
}

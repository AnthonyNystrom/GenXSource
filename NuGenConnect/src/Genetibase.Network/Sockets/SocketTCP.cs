using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Genetibase.Network.Sockets {
  public class SocketTCP : Socket {

    #region Constructors
    public SocketTCP(string aEndPoint) : base() {
      string[] xParts = aEndPoint.Split(':');
      mHost = xParts[0];
      mPort = Int32.Parse(xParts[1]);
      mEndPoint = mHost + ":" + mPort;
    }

    public SocketTCP(
      string aHost
      , int aPort
    ) : base() {
      mHost = aHost;
      mPort = aPort;
      mEndPoint = mHost + ":" + mPort;
    }

    public SocketTCP(
      System.Net.Sockets.Socket socket)
    {
      this.mSocket = socket;
    }

    #endregion

    #region Host Property
    protected string mHost;
    public string Host {
      get { return mHost; }
    }
    #endregion

    #region Port Property
    protected int mPort;
    public int Port {
      get { return mPort; }
    }
    #endregion

    #region Socket Property
    // Kudzu
    // We don't use NetworkStream for both reasons of optimizations and proximity, but future expansion
    // for SuperCore to keep our model untied from the Stream class.
    private System.Net.Sockets.Socket mSocket;
    public System.Net.Sockets.Socket Socket {
      get { return mSocket; }
    }
    #endregion

    public override void Close() {
      mSocket.Close();
      mSocket = null;
      base.Close();
    }
    
    #region Open Method
    public override void Open() {
      base.Open();
      if (mSocket == null) {
        mSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, 
            SocketType.Stream, ProtocolType.Tcp);
        mSocket.Connect(mHost, mPort);
      }
    }
    #endregion

    public override void Transmit(ref byte[] aBytes) {
      base.Transmit(ref aBytes);
      mSocket.Send(aBytes);
    }

    private bool Readable(int aTimeout) {
      return mSocket.Poll(aTimeout, SelectMode.SelectRead);
    }

    protected override int ReadFromSource(bool throwExceptionIfDisconnected, int timeOut, bool throwExceptionOnTimeout)
    {
      byte[] TempBuff = new byte[mRecvBufferSize];
      int LByteCount = 0;
      if (timeOut == 0)
      {
        timeOut = Global.InfiniteTimeOut;
      }
      else
      {
        timeOut = mReadTimeout;
      }
      CheckForDisconnect(throwExceptionIfDisconnected);
      //do
      //{
      if (Readable(timeOut))
      {
        if (IsOpen())
        {
          LByteCount = mSocket.Receive(TempBuff);
          byte[] DataBuffer = new byte[LByteCount];
          Array.Copy(TempBuff, DataBuffer, LByteCount);
          //              if (_Intercept != null)
          //              {
          //                _Intercept.Receive(ref LBuffer);
          //                LByteCount = LBuffer.Count;
          //              }
          InterceptReceive(ref DataBuffer);
          LByteCount = DataBuffer.Length;
          mInputBuffer.Write(DataBuffer);
          DataBuffer = null;
          TempBuff = null;
        }
        else
        {
          throw new ClosedSocketException(ResourceStrings.StatusDisconnected);
        }
        if (LByteCount == 0)
        {
          CloseGracefully();
        }
        CheckForDisconnect(throwExceptionIfDisconnected);
        return LByteCount;
      }
      else
      {
        if (throwExceptionOnTimeout)
        {
          throw new ReadTimeoutException();
        }
        return -1;
      }
      //}
      //while ((LByteCount == 0) || (IsOpen()));
      //return 0;
    }

    
    public override void CheckForDisconnect(bool aThrowExceptionIfDisconnected, bool aIgnoreBuffer) {
      bool xDisconnected = false;
      if (mClosedGracefully) {
        if (IsOpen()) {
          Close();
#warning Implement Status
          //DoStatus(StatusEnum.Disconnected);
        }
        xDisconnected = true;
      } else {
        xDisconnected = !IsOpen();
      }
      //LDisconnected = (!(BindingAllocated() && Binding.Handle.Connected));
      if (xDisconnected) {
        if (mInputBuffer != null) {
          if ((mInputBuffer.Size == 0 || aIgnoreBuffer)
            && aThrowExceptionIfDisconnected) {
            ThrowConnClosedGracefully();
          }
        }
      }

    }

    public override void CheckForDataOnSource(int aTimeOut) {
      if (IsOpen()) {
        ReadFromSource(false, aTimeOut, false);
      }
    }

    public override bool IsOpen() {
      bool TempResult = mSocket != null;
      return TempResult;
    }
  }
}

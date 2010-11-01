using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {

  public delegate void TLSNegotiationFailureEventHandler(object sender, ref bool canContinue);
  public abstract class ExplicitTLSClient<TReplyCode, TReply> : TCPClient<TReplyCode, TReply>
    where TReplyCode : IEquatable<TReplyCode>
    where TReply : Reply<TReplyCode>, new() {
    private int mRegularProtocolPort;
    private int mImplicitTLSProtocolPort;
    private UseTLSEnum mUseTLS = TLSUtilities.DefaultUseTLSValue;
    private TLSNegotiationFailureEventHandler mOnTLSNotAvailable;
    private TLSNegotiationFailureEventHandler mOnTLSNegCmdFailed;
    private TLSNegotiationFailureEventHandler mOnTLSHandshakeFailed;
    private List<string> mCapabilities = new List<string>();

    protected virtual void CheckIfCanUseTLS() {
      if (!(Socket is SocketTLS)) {
        throw new IndyException("SocketTLS required!");
      }
    }

    protected void TLSNotAvailable() {
      if (Connected()) {
        Disconnect();
      }
      throw new IndyException("SSL not available");
    }

    protected void DoOnTLSNotAvailable() {
      bool xCanContinue = false;
      if (mOnTLSNotAvailable != null) {
        mOnTLSNotAvailable(this, ref xCanContinue);
      }
      if (!xCanContinue) {
        TLSNotAvailable();
      }
    }

    protected void ProcessTLSNotAvailable() {
      if (UseTLS == UseTLSEnum.UseRequireTLS) {
        TLSNotAvailable();
      } else {
        DoOnTLSNotAvailable();
      }
    }

    protected void TLSNegCmdFailed() {
      if (Connected()) {
        Disconnect();
      }
      throw new IndyException("TLS command failed!");
    }

    protected void DoOnTLSNegCmdFailed() {
      bool xCanContinue = false;
      if (mOnTLSNegCmdFailed != null) {
        mOnTLSNegCmdFailed(this, ref xCanContinue);
      }
      if (!xCanContinue) {
        TLSNegCmdFailed();
      }
    }

    protected void ProcessTLSNegCmdFailed() {
      if (UseTLS == UseTLSEnum.UseRequireTLS) {
        TLSNegCmdFailed();
      } else {
        DoOnTLSNegCmdFailed();
      }
    }

    protected void TLSHandshakeFailed() {
      if (Connected()) {
        Disconnect();
      }
      throw new IndyException("TLS Handshake failed!");
    }

    protected void DoOnTLSHandshakeFailed() {
      bool xCanContinue = false;
      if (mOnTLSHandshakeFailed != null) {
        mOnTLSHandshakeFailed(this, ref xCanContinue);
      }
      if (!xCanContinue) {
        TLSHandshakeFailed();
      }
    }

    protected void ProcessTLSHandshakeFailed() {
      if (UseTLS == UseTLSEnum.UseRequireTLS) {
        TLSHandshakeFailed();
      } else {
        DoOnTLSHandshakeFailed();
      }
    }

    /// <summary>
    /// </summary>
    /// <remarks>This method should be the only method to do the actual TLS handshake for explicit TLS clients.</remarks>
    protected virtual void TLSHandshake() {
      try {
        if (Socket is SocketTLS) {
          SocketTLS.PassThrough = false;
        }
      } catch {
        ProcessTLSHandshakeFailed();
      }
    }

    protected SocketTLS SocketTLS {
      get {
        return Socket as SocketTLS;
      }
    }

    protected UseTLSEnum UseTLS {
      get {
        return mUseTLS;
      }
      set {
        if (!Connected()) {
          if (value != UseTLSEnum.NoTLSSupport) {
            CheckIfCanUseTLS();
          }
          if (UseTLS != value) {
            if (value == UseTLSEnum.UseImplicitTLS) {
              if (Port == mRegularProtocolPort) {
                Port = mImplicitTLSProtocolPort;
              }
            } else {
              if (Port == mImplicitTLSProtocolPort) {
                Port = mRegularProtocolPort;
              }
            }
            mUseTLS = value;
          }
        } else {
          throw new IndyException("Cannot set UseTLS while connected!");
        }
      }
    }

    public override Socket Socket {
      set {
        base.Socket = value;
        if (!(Socket is SocketTLS)) {
          UseTLS = UseTLSEnum.NoTLSSupport;
        }
      }
    }

    public override void Connect(Socket socket) {
      if (Array.IndexOf<UseTLSEnum>(TLSUtilities.ExplicitTLSValues, UseTLS) != -1) {
        SocketTLS.PassThrough = true;
      }
      if (Socket is SocketTLS) {
        if (UseTLS == UseTLSEnum.UseImplicitTLS) {
          SocketTLS.PassThrough = false;
        } else {
          SocketTLS.PassThrough = true;
        }
      }
      base.Connect(socket);
    }

    public abstract bool SupportsTLS {
      get;
    }

    public List<string> Capabilities {
      get {
        return mCapabilities;
      }
    }

    public event TLSNegotiationFailureEventHandler OnTLSHandshakeFailed {
      add {
        mOnTLSHandshakeFailed += value;
      }
      remove {
        mOnTLSHandshakeFailed -= value;
      }
    }

    public event TLSNegotiationFailureEventHandler OnTLSNotAvailable {
      add {
        mOnTLSNotAvailable += value;
      }
      remove {
        mOnTLSNotAvailable -= value;
      }
    }

    public event TLSNegotiationFailureEventHandler OnTLSNegCmdFailed {
      add {
        mOnTLSNegCmdFailed += value;
      }
      remove {
        mOnTLSNegCmdFailed -= value;
      }
    }

    protected int RegularProtocolPort {
      get {
        return mRegularProtocolPort;
      }
      set {
        mRegularProtocolPort = value;
      }
    }

    protected int ImplicitTLSProtocolPort {
      get {
        return mImplicitTLSProtocolPort;
      }
      set {
        mImplicitTLSProtocolPort = value;
      }
    }
  }
}
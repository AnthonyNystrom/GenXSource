using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Genetibase.Network.Sockets {
	public enum UseTLSEnum {
		/// <summary>
		/// No TLS support
		/// </summary>
		NoTLSSupport,
		/// <summary>
		///   <see cref="SocketTLS"/> descendant required, always TLS.
		/// </summary>
		UseImplicitTLS,
		/// <summary>
		///   <see cref="SocketTLS"/> descendant required, user commands only accepted when in TLS.
		/// </summary>
		UseRequireTLS,
		/// <summary>
		///   User can choose to use TLS. When TLS is desired you need a <see cref="SocketTLS"/> descendant.
		/// </summary>
		UseExplicitTLS
	}

	public delegate void ServerContextExceptionEvent<TReplyCode, TReply, TContext>(TContext context, Exception exception)
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TContext: Context<TReplyCode, TReply, TContext>, new();

	public delegate void ServerContextEvent<TReplyCode, TReply, TContext>(TContext context)
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TContext: Context<TReplyCode, TReply, TContext>, new();

	public delegate void ServerListenExceptionEvent(object sender, Exception exception);

	/// <summary>
	/// Base class for TCP servers.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code to use.</typeparam>
	/// <typeparam name="TReply">The type of reply to use.</typeparam>
	/// <typeparam name="TContext">The type of context to use.</typeparam>
	public abstract class TcpServerBase<TReplyCode, TReply, TContext>
		where TContext: Context<TReplyCode, TReply, TContext>, new()
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new() {

		private ServerSocket mServerSocket;
		private Scheduler mScheduler;
		private Thread mListenThread;
		private int mDefaultPort;
		private bool mActive = false;
		private ServerContextExceptionEvent<TReplyCode, TReply, TContext> mOnContextException;
		private LockableList<TContext> mContexts = new LockableList<TContext>();
		private ServerContextEvent<TReplyCode, TReply, TContext> mOnConnected;
		private ServerContextEvent<TReplyCode, TReply, TContext> mOnDisconnected;
		private ServerListenExceptionEvent mOnListenException;
		private int mRegularProtocolPort;
		private int mImplicitTLSProtocolPort;
		private UseTLSEnum mUseTLS = TLSUtilities.DefaultUseTLSValue;

		public UseTLSEnum UseTLS {
			get {
				return mUseTLS;
			}
			set {
				if (!Active) {
					if ((!(ServerSocket is ServerSocketTLS))
						&& (value != UseTLSEnum.NoTLSSupport)) {
						throw new IndyException("No TLS-enabled ServerSocket given");
					}
					if (value != mUseTLS) {
						if (value == UseTLSEnum.UseImplicitTLS) {
							if (DefaultPort == mRegularProtocolPort) {
								DefaultPort = mImplicitTLSProtocolPort;
							}
						} else {
							if (DefaultPort == mImplicitTLSProtocolPort) {
								DefaultPort = mRegularProtocolPort;
							}
						}
						mUseTLS = value;
					}
				} else {
					throw new IndyException("Cannot set UseTLS when active!");
				}
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

		private void ContextConnected(object sender, EventArgs eventArgs) {
			mContexts.AcquireWriterLock();
			try {
				// any way to prevent these casts?
				mContexts.Add((TContext)sender);
			} finally {
				mContexts.ReleaseWriterLock();
			}
			DoConnect((TContext)sender);
		}

		private void ContextDisconnected(object sender, EventArgs eventArgs) {
			mContexts.AcquireWriterLock();
			try {
				// any way to prevent these casts?
				mContexts.Remove((TContext)sender);
			} finally {
				mContexts.ReleaseWriterLock();
			}
			TContext context = (TContext)sender;
			if (context.TcpConnection.Socket != null) {
				if (context.TcpConnection.Socket.Intercept != null) {
					context.TcpConnection.Socket.Intercept.Disconnect();
					context.TcpConnection.Socket.Intercept.Dispose();
					context.TcpConnection.Socket.Intercept = null;
				}
			}
			if (mOnDisconnected != null) {
				mOnDisconnected((TContext)sender);
			}
		}


		/// <summary>
		/// Handles the client connection.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		protected abstract void DoExecute(object sender, ContextRunEventArgs<TReplyCode, TReply, TContext> eventArgs);

		private void ContextCreated(TContext context) {
		}

		private void DoListenException(Exception exception) {
			if (mOnListenException != null) {
				mOnListenException(this, exception);
			}
		}

		/// <summary>
		/// Gets called after a client is connected.
		/// </summary>
		/// <param name="context">The context.</param>
		protected virtual void DoConnect(TContext context) {
			if (mOnConnected != null) {
				mOnConnected(context);
			}
		}


		/// <summary>
		/// Handles exceptions occurring during processing of client connections.
		/// </summary>
		/// <param name="context">The context on which the exception happened.</param>
		/// <param name="exception">The exception.</param>
		protected void DoContextException(Context<TReplyCode, TReply, TContext> context, Exception exception) {
			// check for silent exceptions (ThreadAbortException for example):
			if (exception is ThreadAbortException) {
				return;
			}
			if (mOnContextException != null) {
				mOnContextException((TContext)context, exception);
			}
		}

		private void ListenThreadExecute() {
			bool LAbort = false;
			Yarn LYarn = null;
			TcpConnection<TReplyCode, TReply> LPeer = null;
			Socket LSocket = null;
			TContext LContext;
			while (!LAbort) {
				try {
					LYarn = mScheduler.AcquireYarn();
					LSocket = mServerSocket.Accept();
					if (LSocket == null) {
						LAbort = true;
						return;
					}
					LPeer = new TcpConnection<TReplyCode, TReply>();
					LPeer.Socket = LSocket;
					LPeer.ManagedSocket = true;

					// add support for MaxConnections

					LContext = new TContext();
					LContext.Yarn = LYarn;
					LContext.TcpConnection = LPeer;
					LContext.OnBeforeRun += ContextConnected;
					LContext.OnAfterRun += ContextDisconnected;
					LContext.OnRun += DoExecute;
					LContext.OnException += DoContextException;
					ContextCreated(LContext);
					Scheduler.StartYarn(LYarn, LContext);
					/*
						except
							on E: Exception do begin
								Sys.FreeAndNil(LContext);
								Sys.FreeAndNil(LPeer);
								// Must terminate - likely has not started yet
								if LYarn <> nil then begin
									Server.Scheduler.TerminateYarn(LYarn);
								end;
								// EAbort is used to kick out above and destroy yarns and other, but
								// we dont want to show the user
								if not (E is EAbort) then begin
									Server.DoListenException(Self, E);
								end;
							end;
						end;

					*/
				} catch (Exception E) {
					LContext = null;
					LPeer = null;
					LSocket = null;
					if (LYarn != null) {
						Scheduler.TerminateYarn(LYarn);
						LYarn = null;
					}
					if (!(E is ThreadAbortException)) {
						DoListenException(E);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the default port to listen on.
		/// </summary>
		/// <value>The default port.</value>
		public int DefaultPort {
			get {
				return mDefaultPort;
			}
			set {
				mDefaultPort = value;
			}
		}

		// MtW:
		//    I know, this is not right. I only don't know how to do the
		//    multiple bindings support. Could somebody shed me a light?
		/// <summary>
		/// Starts up the server. If no <see cref="ServerSocket"/> specified, a <see cref="ServerSocketTcp"/> is created.
		/// Is no <see cref="Scheduler"/> specified, a <see cref="SchedulerOfThreadDefault"/> is created.
		/// </summary>
		public virtual void Open() {
			if (Active) {
				throw new IndyException("Server is already active");
			}
			mActive = true;
			if (mServerSocket == null) {
				mServerSocket = new ServerSocketTcp();
			}
			mServerSocket.Init();
			mServerSocket.Port = DefaultPort;
			mServerSocket.StartListening();

			if (mScheduler == null) {
				mScheduler = new SchedulerOfThreadDefault();
			}
			mListenThread = new Thread(ListenThreadExecute);
			mListenThread.IsBackground = true;
			mListenThread.Start();
		}

		private void TerminateListenerThreads() {
			mListenThread.Abort();
		}

		private void TerminateAllThreads() {
			if (mContexts != null) {
				mContexts.AcquireWriterLock();
				try {
					foreach (TContext c in mContexts) {
						c.TcpConnection.Disconnect(false);
					}
				} finally {
					mContexts.ReleaseWriterLock();
				}
			}
			if (mScheduler != null) {
				mScheduler.TerminateAllYarns();
			}
		}

		/// <summary>
		/// Shuts down the server.
		/// </summary>
		/// <exception cref="IndyException">When not <see cref="Active"/>.</exception>
		public virtual void Close() {
			if (!Active) {
				throw new IndyException("TCPServer is not active!");
			}
			mActive = false;
			TerminateListenerThreads();
			TerminateAllThreads();
			mListenThread.Abort();
			mServerSocket.StopListening();
			mServerSocket.Shutdown();
			mScheduler.TerminateAllYarns();
		}

		/// <summary>
		/// Gets or sets the scheduler to use for handling client connections.
		/// </summary>
		/// <value>The scheduler.</value>
		public Scheduler Scheduler {
			get {
				return mScheduler;
			}
			set {
				mScheduler = value;
			}
		}

		/// <summary>
		/// Gets or sets the server socket.
		/// </summary>
		/// <value>The server socket.</value>
		public virtual ServerSocket ServerSocket {
			get {
				return mServerSocket;
			}
			set {
				if (mServerSocket != value) {
					mServerSocket = value;
					if (!(value is ServerSocketTLS)) {
						UseTLS = UseTLSEnum.NoTLSSupport;
					}
				}
			}
		}

		/// <summary>
		/// Occurs when an exception occurs during client connection processing.
		/// </summary>
		public event ServerContextExceptionEvent<TReplyCode, TReply, TContext> OnContextException {
			add {
				mOnContextException += value;
			}
			remove {
				mOnContextException -= value;
			}
		}

		/// <summary>
		/// Occurs after a new client has been connected and it's <typeparamref name="TContext"/> instance
		/// has been created.
		/// </summary>
		public event ServerContextEvent<TReplyCode, TReply, TContext> OnConnected {
			add {
				mOnConnected += value;
			}
			remove {
				mOnConnected -= value;
			}
		}

		/// <summary>
		/// Occurs before a client gets disconnected and it's <typeparamref name="TContext"/> instance
		/// disposed.
		/// </summary>
		public event ServerContextEvent<TReplyCode, TReply, TContext> OnDisconnected {
			add {
				mOnDisconnected += value;
			}
			remove {
				mOnDisconnected -= value;
			}
		}

		/// <summary>
		/// Occurs when an exception occurs which has nothing to do with a <see cref="Context{TReplyCode, TReply, TContext}"/>
		/// </summary>
		public event ServerListenExceptionEvent OnListenException {
			add {
				mOnListenException += value;
			}
			remove {
				mOnListenException -= value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TcpServerBase{TReplyCode, TReply, TContext}"/> is active.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if active; otherwise, <see langword="false"/>.
		/// </value>
		public bool Active {
			get {
				return mActive;
			}
			set {
				if (value != mActive) {
					if (value) {
						Open();
					} else {
						Close();
					}
				}
			}
		}
	}
}

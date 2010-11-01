using System;

namespace Genetibase.Network.Sockets {
	/// <summary>
	/// Arguments type used in <see cref="T:Context{TReplyCode, TReply, TContext}"/> related events.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code used by the specified <typeparamref name="TReply"/></typeparam>
	/// <typeparam name="TReply">The type of reply used by the <typeparamref name="TContext"/></typeparam>
	/// <typeparam name="TContext">The type of context used.</typeparam>
	public class ContextEventArgs<TReplyCode, TReply, TContext>: EventArgs
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TContext: Context<TReplyCode, TReply, TContext> {
		private TContext mContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ContextEventArgs&lt;TReplyCode, TReply, TContext&gt;"/> class.
		/// </summary>
		/// <param name="mContext">The context.</param>
		public ContextEventArgs(TContext mContext) {
			this.mContext = mContext;
		}

		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>The context.</value>
		public TContext Context {
			get {
				return mContext;
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle context exceptions.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code used by the specified <typeparamref name="TReply"/></typeparam>
	/// <typeparam name="TReply">The type of reply used by the <typeparamref name="TContext"/></typeparam>
	/// <typeparam name="TContext">The type of context used.</typeparam>
	/// <param name="context">The context for which the exception occurred.</param>
	/// <param name="exception">The exception which has occurred.</param>
	public delegate void ContextExceptionEvent<TReplyCode, TReply, TContext>(Context<TReplyCode, TReply, TContext> context, Exception exception)
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TContext: Context<TReplyCode, TReply, TContext>;

	/// <summary>
	/// Arguments type used in events which need a return value.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code used by the specified <typeparamref name="TReply"/></typeparam>
	/// <typeparam name="TReply">The type of reply used by the <typeparamref name="TContext"/></typeparam>
	/// <typeparam name="TContext">The type of context used.</typeparam>
	public class ContextRunEventArgs<TReplyCode, TReply, TContext>: ContextEventArgs<TReplyCode, TReply, TContext>
		where TReply: Reply<TReplyCode>, new()
		where TReplyCode: IEquatable<TReplyCode>
		where TContext: Context<TReplyCode, TReply, TContext> {
		private bool mReturnValue = true;
		/// <summary>
		/// Initializes a new instance of the <see cref="T:ContextRunEventArgs&lt;TReplyCode, TReply, TContext&gt;"/> class.
		/// </summary>
		/// <param name="mContext">The context.</param>
		public ContextRunEventArgs(TContext mContext)
			: base(mContext) {
		}

		/// <summary>
		/// Gets or sets a return value.
		/// </summary>
		public bool ReturnValue {
			get {
				return mReturnValue;
			}
			set {
				mReturnValue = value;
			}
		}
	}

	/// <summary>
	/// Base context class. A context represents a client connection. If you need a simple context class, see <see cref="ContextRFC"/>.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code used by the specified <typeparamref name="TReply"/></typeparam>
	/// <typeparam name="TReply">The type of reply used by the <typeparamref name="TContext"/></typeparam>
	/// <typeparam name="TContext">The type of context used.</typeparam>
	public class Context<TReplyCode, TReply, TContext>: Task
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TContext: Context<TReplyCode, TReply, TContext> {
		private EventHandler<ContextRunEventArgs<TReplyCode, TReply, TContext>> mOnRun;
		private TcpConnection<TReplyCode, TReply> mTcpConnection;
		private ContextExceptionEvent<TReplyCode, TReply, TContext> mOnException;
		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		protected override bool Run() {
			if (mOnRun != null) {
				ContextRunEventArgs<TReplyCode, TReply, TContext> crea = new ContextRunEventArgs<TReplyCode, TReply, TContext>((TContext)this);
				mOnRun(this, crea);
				return crea.ReturnValue;
			}
			return true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Context&lt;TReplyCode, TReply, TContext&gt;"/> class.
		/// </summary>
		public Context() {
		}

		/// <summary>
		/// Handles the given <paramref name="exception"/>.
		/// </summary>
		/// <param name="exception">The exception.</param>
		protected override void DoException(Exception exception) {
			if (mOnException != null) {
				mOnException(this, exception);
			}
		}

		/// <summary>
		/// Gets or sets the TCP connection of the client.
		/// </summary>
		/// <value>The TCP connection.</value>
		public TcpConnection<TReplyCode, TReply> TcpConnection {
			get {
				return mTcpConnection;
			}
			set {
				if (value != mTcpConnection) {
					if (mTcpConnection != null) {
						throw new Exception("Can only set the TcpConnection once!");
					}
					mTcpConnection = value;
				}
			}
		}

		/// <summary>
		/// Occurs when the context is ran. The context is ran again if the 
		/// <see cref="T:ContextRunEventArgs{TReplyCode, TReply, TContext}.ReturnValue"/> is
		/// <see langword="true"/>. This could mean forever.
		/// </summary>
		public event EventHandler<ContextRunEventArgs<TReplyCode, TReply, TContext>> OnRun {
			add {
				mOnRun += value;
			}
			remove {
				mOnRun -= value;
			}
		}

		/// <summary>
		/// Occurs when an exception is caught during the run.
		/// </summary>
		public event ContextExceptionEvent<TReplyCode, TReply, TContext> OnException {
			add {
				mOnException += value;
			}
			remove {
				mOnException -= value;
			}
		}
	}
}
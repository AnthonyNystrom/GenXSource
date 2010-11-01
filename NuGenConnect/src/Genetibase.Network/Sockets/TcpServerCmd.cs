using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
	public delegate void CommandHandlersExceptionEventHandler<TCommand, TContext>(TCommand command, TContext AContext, Exception exception);
	public delegate void CommandHandlerEventHandler<TCommandHandlerList, TContext>(TCommandHandlerList ASender, TContext AContext);
	public delegate void CommandEventHandler<TCommand>(TCommand ASender);
	public delegate void CommandHandlersUnhandledCommandEvent<TContext>(TContext context, byte[] command);

	/// <summary>
	/// TCP server with support for command handlers.
	/// </summary>
	/// <typeparam name="TReplyCode">The type of reply code used by the specified <typeparamref name="TReply"/>.</typeparam>
	/// <typeparam name="TReply">The type of reply used by the specified <typeparamref name="TContext"/></typeparam>
	/// <typeparam name="TContext">The type of contexts used by this <see cref="TcpServerCmd{TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand}"/> instance.</typeparam>
	/// <typeparam name="TCommandHandlerList">The type of <see cref="TcpServerCmd{TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand}.CommandHandlerList"/> to use.</typeparam>
	/// <typeparam name="TCommandHandler">The type of <see cref="TcpServerCmd{TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand}.CommandHandler"/> to use.</typeparam>
	/// <typeparam name="TCommand">The type of <see cref="TcpServerCmd{TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand}.Command"/> to use.</typeparam>
	public partial class TcpServerCmd<TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand>:
		TcpServerBase<TReplyCode, TReply, TContext>
		where TContext: Context<TReplyCode, TReply, TContext>, new()
		where TReplyCode: IEquatable<TReplyCode>
		where TReply: Reply<TReplyCode>, new()
		where TCommandHandlerList: TcpServerCmd<TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand>.CommandHandlerList, new()
		where TCommandHandler: TcpServerCmd<TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand>.CommandHandler, new()
		where TCommand: TcpServerCmd<TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand>.Command, new() {

		public abstract class Command {
			private TCommandHandler mCommandHandler;
			private bool mDisconnect;
			private bool mPerformReply = true;
			private TReply mReply = new TReply();
			private TContext mContext;

			internal TContext ContextSet {
				set {
					mContext = value;
				}
			}

			public void SendReply() {
				PerformReply = false;
				Context.TcpConnection.Socket.Write(Reply.FormattedReply);
			}

			public TContext Context {
				get {
					return mContext;
				}
			}

			public bool Disconnect {
				get {
					return mDisconnect;
				}
				set {
					mDisconnect = value;
				}
			}

			public bool PerformReply {
				get {
					return mPerformReply;
				}
				set {
					mPerformReply = value;
				}
			}

			public TReply Reply {
				get {
					return mReply;
				}
			}

			internal TCommandHandler CommandHandlerSet {
				set {
					mCommandHandler = value;
					if (value != null) {
						mDisconnect = value.Disconnect;
					}
				}
			}

			public TCommandHandler CommandHandler {
				get {
					return mCommandHandler;
				}
			}
		}

		/// <summary>
		/// Base class for command handlers.
		/// </summary>
		public abstract class CommandHandler {
			protected TCommandHandlerList mCollection;
			protected object mData;
			protected List<string> mDescription = new List<string>();
			protected bool mDisconnect;
			protected bool mEnabled = true;
			protected TReply mExceptionReply = new TReply();
			protected string mHelpSuperScript = "";
			protected bool mHelpVisible;
			protected TReply mNormalReply = new TReply();

			private TCommandHandler typedThis {
				get {
					return (TCommandHandler)this;
				}
			}

			public abstract byte[] CommandBytes {
				get;
			}

			public CommandHandler() {
			}

			protected virtual void AfterRun(TCommand command) {
			}

			public abstract void DoCommand(TCommand command);
			public virtual void DoCommand(TContext context, byte[] command) {
				TCommand LCommand = new TCommand();
				try {
					try {
						LCommand.Disconnect = this.Disconnect;
						LCommand.CommandHandlerSet = typedThis;
						LCommand.ContextSet = context;
						LCommand.Reply.FormattedReply = NormalReply.FormattedReply;
						DoCommand(LCommand);
					} catch (Exception E) {
						if (LCommand.PerformReply) {
							if (!ExceptionReply.Code.Equals(default(TReplyCode))) {
								LCommand.Reply.FormattedReply = this.ExceptionReply.FormattedReply;
							}
							//            if (!LCommand.Reply.Equals(default(TReplyCode)))
							//            {
							//              LCommand.Reply.Text.Add(E.Message);
							//              LCommand.SendReply();
							//            }
							//            else
							//            {
							//            }
						}
						OnException(LCommand, context, E);
					}
					if (LCommand.PerformReply) {
						LCommand.SendReply();
					}
					AfterRun(LCommand);
				} finally {
					if (LCommand.Disconnect) {
						context.TcpConnection.Disconnect();
					}
				}
			}

			internal event CommandHandlersExceptionEventHandler<TCommand, TContext> OnException;

			public bool CommandBytesIs(byte[] checkBytes) {
				byte[] commandBytes = CommandBytes;
				if (checkBytes.Length != commandBytes.Length) {
					return false;
				}
				for (int i = 0; i < commandBytes.Length; i++) {
					if (commandBytes[i] != checkBytes[i]) {
						return false;
					}
				}
				return true;
			}

			public string HelpSuperScript {
				get {
					return mHelpSuperScript;
				}
				set {
					mHelpSuperScript = value;
				}
			}

			public bool HelpVisible {
				get {
					return mHelpVisible;
				}
				set {
					mHelpVisible = value;
				}
			}

			public List<string> Description {
				get {
					return mDescription;
				}
			}

			public bool Disconnect {
				get {
					return mDisconnect;
				}
				set {
					mDisconnect = value;
				}
			}

			public bool Enabled {
				get {
					return mEnabled;
				}
				set {
					mEnabled = value;
				}
			}

			public TReply ExceptionReply {
				get {
					return mExceptionReply;
				}
			}

			public TReply NormalReply {
				get {
					return mNormalReply;
				}
			}
		}

		/// <summary>
		/// Base class for command handler lists.
		/// </summary>
		public abstract partial class CommandHandlerList: List<TCommandHandler> {
			private TReply mExceptionReply = new TReply();

			/// <summary>
			/// Gets the index of the end of the command in the given <paramref name="buffer"/>, or <c>-1</c> if non-existing.
			/// </summary>
			/// <param name="buffer">The buffer.</param>
			/// <returns>The index of the last by of the command, or <c>-1</c> if no command exists in the <paramref name="buffer"/>.</returns>
			protected abstract int IndexOfCommandEnd(Buffer buffer);

			/// <summary>
			/// Filters the command.
			/// </summary>
			/// <param name="command">The command.</param>
			/// <returns></returns>
			protected virtual byte[] FilterCommand(byte[] command) {
				return command;
			}

			private CommandHandlersUnhandledCommandEvent<TContext> mUnhandledCommand;

			/// <summary>
			/// Occurs when a command is not handled.
			/// </summary>
			public event CommandHandlersUnhandledCommandEvent<TContext> UnhandledCommand {
				add {
					mUnhandledCommand += value;
				}
				remove {
					mUnhandledCommand -= value;
				}
			}

			private CommandHandlerEventHandler<TCommandHandlerList, TContext> mOnAfterCommandHandler;

			/// <summary>
			/// Occurs after a <see cref="CommandHandler"/> has been ran.
			/// </summary>
			public event CommandHandlerEventHandler<TCommandHandlerList, TContext> OnAfterCommandHandler {
				add {
					mOnAfterCommandHandler += value;
				}
				remove {
					mOnAfterCommandHandler -= value;
				}
			}

			private CommandHandlersExceptionEventHandler<TCommand, TContext> mOnCommandHandlersException;

			/// <summary>
			/// Occurs when an unhandled exception occurs during execution of a <see cref="CommandHandler"/>.
			/// </summary>
			public event CommandHandlersExceptionEventHandler<TCommand, TContext> OnCommandHandlersException {
				add {
					mOnCommandHandlersException += value;
				}
				remove {
					mOnCommandHandlersException -= value;
				}
			}

			private CommandHandlerEventHandler<TCommandHandlerList, TContext> mOnBeforeCommandHandler;

			/// <summary>
			/// Occurs before a <see cref="CommandHandler"/> gets executed.
			/// </summary>
			public event CommandHandlerEventHandler<TCommandHandlerList, TContext> OnBeforeCommandHandler {
				add {
					mOnBeforeCommandHandler += value;
				}
				remove {
					mOnBeforeCommandHandler -= value;
				}
			}

			/// <summary>
			/// Handles the command, if possible.
			/// </summary>
			/// <param name="AContext">The context in which the current code is running.</param>
			/// <returns><see langword="true"/> if handled, <see langword="false"/> if not.</returns>
			public virtual bool HandleCommand(TContext AContext) {
				DoBeforeCommandHandler(AContext);
				try {
					if (Count > 0) {
						int commandEnd = IndexOfCommandEnd(AContext.TcpConnection.Socket.mInputBuffer);
						if (commandEnd != -1) {
							byte[] command = AContext.TcpConnection.Socket.ReadBytes(commandEnd);
							command = FilterCommand(command);
							foreach (TCommandHandler handler in this) {
								if (!handler.Enabled) {
									continue;
								}
								if (handler.CommandBytesIs(command)) {
									handler.OnException += CommandHandler_OnException;
									try {
										handler.DoCommand(AContext, command);
									} finally {
										handler.OnException -= CommandHandler_OnException;
									}
									return true;
								}
							}
							DoUnhandledCommand(AContext, command);
							return true;
						}
					}
					return false;
				} finally {
					DoAfterCommandHandler(AContext);
				}
			}

			/// <summary>
			/// Handles command handler exceptions.
			/// </summary>
			/// <param name="command">The command on which the exception occurred.</param>
			/// <param name="AContext">The context on which the exception happened.</param>
			/// <param name="exception">The exception.</param>
			protected virtual void CommandHandler_OnException(TCommand command, TContext AContext, Exception exception) {
				if (mOnCommandHandlersException != null) {
					mOnCommandHandlersException(command, AContext, exception);
				}
			}

			/// <summary>
			/// Gets called when a command is not handled.
			/// </summary>
			/// <param name="context">The context.</param>
			/// <param name="command">The command.</param>
			protected virtual void DoUnhandledCommand(TContext context, byte[] command) {
				if (mUnhandledCommand != null) {
					mUnhandledCommand(context, command);
				}
			}

			/// <summary>
			/// Gets called before a command handler gets ran.
			/// </summary>
			/// <param name="context">The context.</param>
			protected virtual void DoBeforeCommandHandler(TContext context) {
				if (mOnBeforeCommandHandler != null) {
					mOnBeforeCommandHandler(typedThis, context);
				}
			}

			private TCommandHandlerList typedThis {
				get {
					return (TCommandHandlerList)this;
				}
			}

			/// <summary>
			/// Gets called after a command handler has been ran.
			/// </summary>
			/// <param name="context">The context.</param>
			protected virtual void DoAfterCommandHandler(TContext context) {
				if (mOnAfterCommandHandler != null) {
					mOnAfterCommandHandler(typedThis, context);
				}
			}

			/// <summary>
			/// Gets or sets the reply to send when an unhandled exception occurs during 
			/// command execution.
			/// </summary>
			/// <value>The exception reply.</value>
			public TReply ExceptionReply {
				get {
					return mExceptionReply;
				}
				set {
					mExceptionReply = value;
				}
			}
		}

		private TCommandHandlerList mCommandHandlers = new TCommandHandlerList();
		private bool mCommandHandlersInitialized = false;

		/// <summary>
		/// Handles the client connection.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event args.</param>
		protected override void DoExecute(object sender, ContextRunEventArgs<TReplyCode, TReply, TContext> eventArgs) {
			eventArgs.Context.TcpConnection.Socket.CheckForDataOnSource();
			eventArgs.Context.TcpConnection.Socket.CheckForDisconnect();
			eventArgs.ReturnValue = true;
			while (eventArgs.Context.TcpConnection.Connected() &&
				mCommandHandlers.HandleCommand(eventArgs.Context)) {
				System.Threading.Thread.Sleep(5); // prevent cpu from going 100%
			}
			if (eventArgs.ReturnValue
				&& eventArgs.Context.TcpConnection != null) {
				eventArgs.ReturnValue = eventArgs.Context.TcpConnection.Connected();
			}
		}

		/// <summary>
		/// Starts up the server. If no <see cref="ServerSocket"/> specified, a <see cref="ServerSocketTcp"/> is created.
		/// Is no <see cref="Scheduler"/> specified, a <see cref="SchedulerOfThreadDefault"/> is created.
		/// </summary>
		public override void Open() {
			base.Open();
			if (!mCommandHandlersInitialized) {
				mCommandHandlersInitialized = true;
				InitializeCommandHandlers();
			}
		}

		/// <summary>
		/// Initializes the command handlers.
		/// </summary>
		protected virtual void InitializeCommandHandlers() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TcpServerCmd{TReplyCode, TReply, TContext, TCommandHandlerList, TCommandHandler, TCommand}"/> class.
		/// </summary>
		public TcpServerCmd() {
			mCommandHandlers.OnCommandHandlersException += DoCommandHandlersException;
		}

		/// <summary>
		/// Does the command handlers exception.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="AContext">The A context.</param>
		/// <param name="exception">The exception.</param>
		private void DoCommandHandlersException(TCommand command, TContext AContext, Exception exception) {
			this.DoContextException(AContext, exception);
		}

		/// <summary>
		/// Gets the command handlers of this server.
		/// </summary>
		/// <value>The command handlers.</value>
		public TCommandHandlerList CommandHandlers {
			get {
				return mCommandHandlers;
			}
		}
	}
}
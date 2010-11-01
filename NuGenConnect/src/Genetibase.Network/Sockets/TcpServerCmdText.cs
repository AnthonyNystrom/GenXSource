using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
	public delegate void TextCommandEventHandler<TContext>(TcpServerCmdText<TContext>.TextCommand command) where TContext: Context<int, ReplyRFC, TContext>, new();
	public class TcpServerCmdText<TContext>:
		TcpServerCmd
		<int,
			ReplyRFC,
			TContext,
			TcpServerCmdText<TContext>.TextCommandHandlerList,
			TcpServerCmdText<TContext>.TextCommandHandler,
			TcpServerCmdText<TContext>.TextCommand>
		where TContext: Context<int, ReplyRFC, TContext>, new() {
		public class TextCommand: Command {
			private List<string> mParams = new List<string>();
			private string mRawLine;
			private List<string> mResponse = new List<string>();
			private string mUnparsedParams;
			private bool mSendEmptyResponse = false;

			public List<string> Params {
				get {
					return mParams;
				}
			}

			public string RawLine {
				get {
					return mRawLine;
				}
			}

			internal string RawLineSet {
				set {
					mRawLine = value;
				}
			}

			public List<string> Response {
				get {
					return mResponse;
				}
			}

			public string UnparsedParams {
				get {
					return mUnparsedParams;
				}
			}

			internal string UnparsedParamsSet {
				set {
					mUnparsedParams = value;
				}
			}

			public bool SendEmptyResponse {
				get {
					return mSendEmptyResponse;
				}
			}
		}

		public class TextCommandHandler: CommandHandler {
			private string mCmdDelimiter = " ";
			private string mCommand;
			private TextCommandEventHandler<TContext> mOnCommand;
			private string mParamDelimiter = " ";
			private bool mParseParams = true;
			private List<string> mResponse = new List<string>();
			protected bool mAppendExceptionToReply;

			public override byte[] CommandBytes {
				get {
					return Encoding.ASCII.GetBytes(Command.ToUpperInvariant());
				}
			}

			public override void DoCommand(TextCommand command) {
				if (mParseParams) {
					if (command.Context.TcpConnection.Socket.InputBuffer.IndexOf(ParamDelimiter) == 0) {
						command.Context.TcpConnection.Socket.ReadTo(Encoding.ASCII.GetBytes(ParamDelimiter));
					}
					string commandLine = command.Context.TcpConnection.Socket.ReadLn();
					command.RawLineSet = String.Format("{0}{1}{2}",
						Command, CmdDelimiter, commandLine);
					command.UnparsedParamsSet = commandLine;
					command.Params.Clear();
					List<string> TempStrings = new List<string>(commandLine.Split(new string[] { ParamDelimiter }, StringSplitOptions.None));
					foreach (string s in TempStrings) {
						command.Params.Add(s.TrimEnd('\r', '\n'));
					}
				}
				if (mOnCommand != null) {
					mOnCommand(command);
				}
			}

			protected override void AfterRun(TcpServerCmdText<TContext>.TextCommand command) {
				base.AfterRun(command);
				if (command.Response.Count > 0
					|| command.SendEmptyResponse) {
					command.Context.TcpConnection.WriteRFCStrings(command.Response);
				} else {
					if (command.Response.Count > 0) {
						command.Context.TcpConnection.WriteRFCStrings(command.Response);
					}
				}
			}

			public string CmdDelimiter {
				get {
					return mCmdDelimiter;
				}
				set {
					mCmdDelimiter = value;
				}
			}

			public string Command {
				get {
					return mCommand;
				}
				set {
					mCommand = value;
				}
			}

			public string ParamDelimiter {
				get {
					return mParamDelimiter;
				}
				set {
					mParamDelimiter = value;
				}
			}

			public bool ParseParams {
				get {
					return mParseParams;
				}
				set {
					mParseParams = value;
				}
			}

			public List<string> Response {
				get {
					return mResponse;
				}
			}

			public event TextCommandEventHandler<TContext> OnCommand {
				add {
					mOnCommand += value;
				}
				remove {
					mOnCommand -= value;
				}
			}
		}

		public class TextCommandHandlerList: CommandHandlerList {
			public const bool Default_AppendExceptionToReply = true;
			private bool mAppendExceptionToReply = Default_AppendExceptionToReply;
			protected override int IndexOfCommandEnd(Buffer buffer) {
				if (buffer == null) {
					return -1;
				}
				if (buffer.Size == 0) {
					return -1;
				}
				List<string> commandDelimiters = new List<string>();
				for (int i = 0; i < this.Count; i++) {
					TextCommandHandler tch = this[i];
					if (tch.Enabled) {
						if (!commandDelimiters.Contains(tch.CmdDelimiter)) {
							commandDelimiters.Add(tch.CmdDelimiter);
						}
					}
				}
				if (!commandDelimiters.Contains("\r\n")) {
					commandDelimiters.Add("\r\n");
				}
				if (!commandDelimiters.Contains("\n")) {
					commandDelimiters.Add("\n");
				}
				if (!commandDelimiters.Contains("\r")) {
					commandDelimiters.Add("\r");
				}
				int TempResult = buffer.IndexOf(commandDelimiters[0]);
				commandDelimiters.RemoveAt(0);
				while (TempResult == -1
					&& commandDelimiters.Count > 0) {
					TempResult = buffer.IndexOf(commandDelimiters[0]);
					commandDelimiters.RemoveAt(0);
				}
				while (commandDelimiters.Count > 0) {
					int TempInt2 = buffer.IndexOf(commandDelimiters[0]);
					if (TempInt2 > -1) {
						TempResult = Math.Min(TempResult, TempInt2);
					}
					commandDelimiters.RemoveAt(0);
				}
				return TempResult;
			}

			protected override void DoAfterCommandHandler(TContext AContext) {
				base.DoAfterCommandHandler(AContext);
				if (AContext.TcpConnection.Socket.InputBuffer.PeekByte(0) == 13) {
					AContext.TcpConnection.Socket.InputBuffer.Remove(0);
				}
				if (AContext.TcpConnection.Socket.InputBuffer.PeekByte(0) == 10) {
					AContext.TcpConnection.Socket.InputBuffer.Remove(0);
				}
			}

			protected override byte[] FilterCommand(byte[] command) {
				byte[] LCommand = base.FilterCommand(command);
				string CommandStr = Encoding.ASCII.GetString(LCommand);
				return Encoding.ASCII.GetBytes(CommandStr.ToUpperInvariant());
			}

			protected override void CommandHandler_OnException(TextCommand command, TContext AContext, Exception exception) {
				base.CommandHandler_OnException(command, AContext, exception);
				if (mAppendExceptionToReply) {
					command.Response.Add(exception.ToString());
				}
			}

			protected override void DoUnhandledCommand(TContext context, byte[] command) {
				base.DoUnhandledCommand(context, command);
				context.TcpConnection.Socket.ReadBytes(1);
				if (context.TcpConnection.Socket.InputBuffer.IndexOf("\n") == 0) {
					context.TcpConnection.Socket.ReadBytes(1);
				}
			}
		}

		public TcpServerCmdText() {
			this.CommandHandlers.UnhandledCommand += HandleUnhandledCommand;
		}

		private void HandleUnhandledCommand(TContext context, byte[] command) {
			ReplyRFC r = new ReplyRFC();
			r.FormattedReply = this.CommandHandlers.ExceptionReply.FormattedReply;
			r.Text.Clear();
			r.Text.Add("Unhandled command: " + Encoding.ASCII.GetString(command));
			context.TcpConnection.Socket.Write(r.FormattedReply);
		}
	}
}
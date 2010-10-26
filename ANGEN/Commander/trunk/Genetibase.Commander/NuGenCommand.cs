/* -----------------------------------------------
 * NuGenCommand.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Diagnostics;

namespace Genetibase.Commander
{
	/// <summary>
	/// Encapsulates command parameters.
	/// </summary>
	public class NuGenCommand
	{
		#region Properties

		/*
		 * Cmd
		 */

		private int internalCmd;

		/// <summary>
		/// Gets or sets the command identifier.
		/// </summary>
		public int Cmd
		{
			[DebuggerStepThrough()]
			get
			{
				return this.internalCmd;
			}

			[DebuggerStepThrough()]
			set
			{
				this.internalCmd = value;
			}
		}

		/*
		 * LParam
		 */

		/// <summary>
		/// Determines the command additional parameters.
		/// </summary>
		private object lParam;

		/// <summary>
		/// Gets or sets the command additional parameters.
		/// </summary>
		public object LParam
		{
			[DebuggerStepThrough()]
			get
			{
				return this.lParam;
			}

			[DebuggerStepThrough()]
			set
			{
				this.lParam = value;
			}
		}

		/*
		 * Receiver
		 */

		private object receiver;

		/// <summary>
		/// Gets or sets the command receiver.
		/// </summary>
		public object Receiver
		{
			[DebuggerStepThrough()]
			get
			{
				return this.receiver;
			}

			[DebuggerStepThrough()]
			set
			{
				this.receiver = value;
			}
		}
		
		/*
		 * Sender
		 */

		private object sender;

		/// <summary>
		/// Gets or sets the command sender.
		/// </summary>
		public object Sender
		{
			[DebuggerStepThrough()]
			get
			{
				return this.sender;
			}

			[DebuggerStepThrough()]
			set
			{
				this.sender = value;
			}
		}

		/*
		 * WParam
		 */

		private object wParam;

		/// <summary>
		/// Gets or sets the command parameters.
		/// </summary>
		public object WParam
		{
			[DebuggerStepThrough()]
			get
			{
				return this.wParam;
			}

			[DebuggerStepThrough()]
			set
			{
				this.wParam = value;
			}
		}

		#endregion

		#region Methods.Public.Overriden

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format(
				"Sender: {0}; Receiver: {1}; Cmd: {2}; WParam: {3}; LParam: {4}",
				this.Sender != null ? this.Sender.ToString() : "null",
				this.Receiver != null ? this.Receiver.ToString() : "null",
				this.Cmd.ToString(),
				this.WParam != null ? this.WParam.ToString() : "null",
				this.LParam != null ? this.LParam.ToString() : "null"
				);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		public NuGenCommand()
			: this(null, NuGenSystemCommand.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		public NuGenCommand(object sender, int cmd)
			: this(sender, null, cmd)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="receiver">Specifies the command receiver.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		public NuGenCommand(object sender, object receiver, int cmd)
			: this(sender, receiver, cmd, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="receiver">Specifies the command receiver.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		/// <param name="wParam">Specifies parameters for the command.</param>
		public NuGenCommand(object sender, object receiver, int cmd, object wParam)
			: this(sender, receiver, cmd, wParam, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		/// <param name="wParam">Specifies parameters for the command.</param>
		public NuGenCommand(object sender, int cmd, object wParam)
			: this(sender, null, cmd, wParam, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		/// <param name="wParam">Specifies parameters for the command.</param>
		/// <param name="lParam">Specifies additional parameters for the command.</param>
		public NuGenCommand(object sender, int cmd, object wParam, object lParam)
			: this(sender, null, cmd, wParam, lParam)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommand"/> class.
		/// </summary>
		/// <param name="sender">Specifies the command sender.</param>
		/// <param name="receiver">Specifies the command receiver.</param>
		/// <param name="cmd">Specified the command identifier.</param>
		/// <param name="wParam">Specifies parameters for the command.</param>
		/// <param name="lParam">Specifies additional parameters for the command.</param>
		public NuGenCommand(object sender, object receiver, int cmd, object wParam, object lParam)
		{
			this.Sender = sender;
			this.Receiver = receiver;
			this.Cmd = cmd;
			this.WParam = wParam;
			this.LParam = lParam;
		}

		#endregion
	}
}

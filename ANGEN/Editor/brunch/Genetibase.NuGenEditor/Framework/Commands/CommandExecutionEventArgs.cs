/* -----------------------------------------------
 * CommandExecutionEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Framework.Commands
{
	/// <summary>
	/// </summary>
	public class CommandExecutionEventArgs : EventArgs
	{
		private String commandName;

		/// <summary>
		/// </summary>
		public CommandExecutionEventArgs(String commandName)
		{
			this.commandName = commandName;
		}

		/// <summary>
		/// </summary>
		public String CommandName
		{
			get
			{
				return this.commandName;
			}
		}
	}
}

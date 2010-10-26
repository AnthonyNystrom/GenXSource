/* -----------------------------------------------
 * CommandChangedEventArgs.cs
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
	public class CommandChangedEventArgs : EventArgs
	{
		private ICommandCollection added;
		private ICommandCollection removed;

		/// <summary>
		/// </summary>
		public CommandChangedEventArgs(ICommandCollection added, ICommandCollection removed)
		{
			this.added = added;
			this.removed = removed;
		}

		/// <summary>
		/// </summary>
		public ICommandCollection Added
		{
			get
			{
				return this.added;
			}
		}

		/// <summary>
		/// </summary>
		public ICommandCollection Removed
		{
			get
			{
				return this.removed;
			}
		}
	}
}

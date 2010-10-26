/* -----------------------------------------------
 * UndoEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Logic.Text.Undo
{
	/// <summary>
	/// </summary>
	public class UndoEventArgs : EventArgs
	{
		private IUndoUnit _change;

		/// <summary>
		/// Initializes a new instance of the <see cref="UndoEventArgs"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="change"/> is <see langword="null"/>.</para>
		/// </exception>
		public UndoEventArgs(IUndoUnit change)
		{
			if (change == null)
			{
				throw new ArgumentNullException("change");
			}
			_change = change;
		}

		/// <summary>
		/// </summary>
		public IUndoUnit Change
		{
			get
			{
				return _change;
			}
		}
	}
}

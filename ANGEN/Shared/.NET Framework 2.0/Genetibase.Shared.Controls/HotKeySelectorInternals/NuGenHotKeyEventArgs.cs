/* -----------------------------------------------
 * NuGenHotKeyEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.HotKeySelectorInternals
{
	/// <summary>
	/// </summary>
	public class NuGenHotKeyEventArgs : EventArgs
	{
		private Keys _hotKeys;

		/// <summary>
		/// </summary>
		public Keys HotKeys
		{
			get
			{
				return _hotKeys;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHotKeyEventArgs"/> class.
		/// </summary>
		/// <param name="hotKeys"></param>
		public NuGenHotKeyEventArgs(Keys hotKeys)
		{
			_hotKeys = hotKeys;
		}
	}
}

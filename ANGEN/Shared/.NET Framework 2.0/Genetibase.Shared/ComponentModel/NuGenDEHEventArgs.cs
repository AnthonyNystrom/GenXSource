/* -----------------------------------------------
 * NuGenDEHEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Represents basic class that can be used as delayed event data container.
	/// </summary>
	public sealed class NuGenDEHEventArgs : EventArgs
	{
		private object _lParam;

		/// <summary>
		/// Gets the optional event parameter.
		/// </summary>
		public object LParam
		{
			get
			{
				return _lParam;
			}
		}

		private object _wParam;

		/// <summary>
		/// Gets the optional event parameter.
		/// </summary>
		public object WParam
		{
			get
			{
				return _wParam;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDEHEventArgs"/> class.
		/// </summary>
		public NuGenDEHEventArgs(object wParam, object lParam)
		{
			_wParam = wParam;
			_lParam = lParam;
		}
	}
}

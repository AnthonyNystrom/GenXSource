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
	public class NuGenDEHEventArgs : INuGenDEHEventArgs
	{
		#region Properties.Public

		/*
		 * LParam
		 */

		private object _lParam = null;

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

		/*
		 * WParam
		 */

		private object _wParam = null;

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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDEHEventArgs"/> class.
		/// </summary>
		public NuGenDEHEventArgs(object wParam, object lParam)
		{
			_wParam = wParam;
			_lParam = lParam;
		}

		#endregion
	}
}

/* -----------------------------------------------
 * WINDOWPOS.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information about the size and position of a window.
	/// </summary>
	public struct WINDOWPOS
	{
		#region Declarations.Fields.Public

		/// <summary>
		/// Handle to the window.
		/// </summary>
		public IntPtr hwnd;

		/// <summary>
		/// Handle to the window to precede the positioned window in the Z order.
		/// </summary>
		public IntPtr hwndInsertAfter;

		/// <summary>
		/// Specifies the position of the left edge of the window.
		/// </summary>
		public int x;

		/// <summary>
		/// Specifies the position of the top edge of the window.
		/// </summary>
		public int y;

		/// <summary>
		/// Specifies the window width, in pixels.
		/// </summary>
		public int width;

		/// <summary>
		/// Specifies the window height, in pixels.
		/// </summary>
		public int height;

		/// <summary>
		/// Specifies the window position.
		/// </summary>
		public int flags;

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return string.Format("X: {0}; Y: {1}; Width: {2}; Height: {3}; Flags: {4}",
				this.x,
				this.y,
				this.width,
				this.height,
				this.flags
			);
		}

		#endregion
	}
}

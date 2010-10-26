/* -----------------------------------------------
 * WINDOWPOS.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Globalization;

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
		public Int32 x;

		/// <summary>
		/// Specifies the position of the top edge of the window.
		/// </summary>
		public Int32 y;

		/// <summary>
		/// Specifies the window width, in pixels.
		/// </summary>
		public Int32 width;

		/// <summary>
		/// Specifies the window height, in pixels.
		/// </summary>
		public Int32 height;

		/// <summary>
		/// Specifies the window position.
		/// </summary>
		public Int32 flags;

		#endregion

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> containing a fully qualified type name.
		/// </returns>
		public override String ToString()
		{
			return String.Format(
				CultureInfo.InvariantCulture,
				"X: {0}; Y: {1}; Width: {2}; Height: {3}; Flags: {4}",
				this.x,
				this.y,
				this.width,
				this.height,
				this.flags
			);
		}
	}
}

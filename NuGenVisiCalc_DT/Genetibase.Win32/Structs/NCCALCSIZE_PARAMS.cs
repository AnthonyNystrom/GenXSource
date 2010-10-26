/* -----------------------------------------------
 * NCCALCSIZE_PARAMS.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information that an application can use while processing the WM_NCCALCSIZE message to
	/// calculate the size, position, and valid contents of the client area of a window.
	/// </summary>
	/// <remarks>
	/// If the window is a child window, the coordinates are relative to the client area of the parent window.
	/// If the window is a top-level window, the coordinates are relative to the screen origin.
	/// </remarks>
	public struct NCCALCSIZE_PARAMS
	{
		/// <summary>
		/// Contains the new coordinates of a window that has been moved or resized, that is, it is the
		/// proposed new window coordinates. 
		/// </summary>
		public RECT rectProposed;

		/// <summary>
		/// Contains the coordinates of the window before it was moved or resized.
		/// </summary>
		public RECT rectBeforeMove;

		/// <summary>
		/// Contains the coordinates of the window's client area before the window was moved or resized.
		/// </summary>
		public RECT rectClientBeforeMove;

		/// <summary>
		/// Pointer to a WINDOWPOS structure that contains the size and position values specified in the
		/// operation that moved or resized the window.
		/// </summary>
		public IntPtr lppos;
	}
}

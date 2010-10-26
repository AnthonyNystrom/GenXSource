/* -----------------------------------------------
 * MOUSEHOOKSTRUCT.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information about a mouse event passed to a WH_MOUSE hook procedure, MouseProc.
	/// </summary>
	public struct MOUSEHOOKSTRUCT
	{
		/// <summary>
		/// Specifies a <see cref="POINT"/> structure that contains the x- and y-coordinates of the cursor, in screen coordinates.
		/// </summary>
		public POINT pt;

		/// <summary>
		/// Handle to the window that will receive the mouse message corresponding to the mouse event.
		/// </summary>
		public Int32 hwnd;
		
		/// <summary>
		/// Specifies the hit-test value. For a list of hit-test values, see the description of the WM_NCHITTEST message.
		/// </summary>
		public Int32 wHitTestCode;
		
		/// <summary>
		/// Specifies extra information associated with the message.
		/// </summary>
		public Int32 dwExtraInfo;
	}
}

/* -----------------------------------------------
 * TRACKMOUSEEVENT.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Used by the TrackMouseEvent function to track when the mouse pointer leaves a window or hovers over
	/// a window for a specified amount of time.
	/// </summary>
	public struct TRACKMOUSEEVENT
	{
		/// <summary>
		/// </summary>
		public int cbSize;

		/// <summary>
		/// </summary>
		public uint dwFlags;

		/// <summary>
		/// </summary>
		public int dwHoverTime;

		/// <summary>
		/// </summary>
		public int hwndTrack;
	}
}

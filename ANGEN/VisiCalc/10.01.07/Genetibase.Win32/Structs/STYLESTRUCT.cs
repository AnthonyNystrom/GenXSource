/* -----------------------------------------------
 * STYLESTRUCT.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// <para>Contains the styles for a window.</para>
	/// <para>
	/// The styles in styleOld and styleNew can be either the window styles (WS_*)
	/// or the extended window styles (WS_EX_*), depending on the wParam of the message
	/// that includes STYLESTRUCT.
	/// The styleOld and styleNew members indicate the styles through their bit pattern.
	/// Note that several styles are equal to zero; to detect these styles, test for the
	/// negation of their inverse style. For example, to see if WS_EX_LEFT is set, you test
	/// for ~WS_EX_RIGHT.
	/// </para>
	/// </summary>
	public struct STYLESTRUCT
	{
		/// <summary>
		/// Specifies the previous styles for a window.
		/// </summary>
		public Int32 styleOld;

		/// <summary>
		/// Specifies the new styles for a window.
		/// </summary>
		public Int32 styleNew;
	}
}

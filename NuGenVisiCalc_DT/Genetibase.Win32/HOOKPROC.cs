/* -----------------------------------------------
 * HOOKPROC.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines a hook callback function.
	/// </summary>
	/// <returns></returns>
	public delegate IntPtr HOOKPROC(Int32 nCode, IntPtr wParam, IntPtr lParam);
}

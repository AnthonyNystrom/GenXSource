/* -----------------------------------------------
 * Kernel32.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports Kernel32.dll functions.
	/// </summary>
	public static class Kernel32
	{
		/// <summary>
		/// Retrieves the current value of the high-resolution performance counter. 
		/// </summary>
		/// <param name="x">Specifies the variable to receive the current performance-counter value, in counts.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport("kernel32.dll")]
		public static extern bool QueryPerformanceCounter(ref long x);
	}
}

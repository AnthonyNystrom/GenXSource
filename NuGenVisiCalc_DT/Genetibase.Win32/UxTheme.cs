/* -----------------------------------------------
 * UxTheme.cs
 * Copyright © 2005-2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports UxTheme.dll functions.
	/// </summary>
	public static class UxTheme
	{
		/// <summary>
		/// Causes a window to use a different set of visual style information than its class normally uses.
		/// </summary>
		/// <param name="hWnd">Handle to the window whose visual style information is to be changed.</param>
		/// <param name="pszSubAppName">String that contains the application name to use in place of the
		/// calling application's name. If this parameter is <see langword="null"/>, the calling
		/// application's name is used.</param>
		/// <param name="pszSubIdList">String that contains a semicolon-separated list of class
		/// identifier (CLSID) names to use in place of the actual list passed by the window's class. If this
		/// parameter is <see langword="null"/>, the identifier (ID) list from the calling class is used.</param>
		/// <returns>Returns S_OK if successful, or an error value otherwise.</returns>
		[DllImport("UxTheme.dll")]
		public static extern Int32 SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);
	}
}

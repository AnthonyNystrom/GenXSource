/* -----------------------------------------------
 * Kernel32.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		private const String DLL = "Kernel32.dll";

		/// <summary>
		/// Retrieves a module handle for the specified module if the file has been mapped into the address space of the calling process.
		/// </summary>
		/// <param name="lpModuleName">
		/// Pointer to a null-terminated String that contains the name of the module
		/// (either a .dll or .exe file). If the file name extension is omitted, the default library
		/// extension .dll is appended. The file name String can include a trailing point character (.)
		/// to indicate that the module name has no extension. The String does not have to specify a path.
		/// When specifying a path, be sure to use backslashes (\), not forward slashes (/). The name is
		/// compared (case independently) to the names of modules currently mapped into the address space
		/// of the calling process. 
		/// If this parameter is <see langword="null"/>, GetModuleHandle returns a handle to the file
		/// used to create the calling process (.exe file).
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is a handle to the specified module.
		/// If the function fails, the return value is <c>IntPtr.Zero</c>. To get extended error
		/// information, call GetLastError.
		/// </returns>
		[DllImport(DLL, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(String lpModuleName);

		/// <summary>
		/// Retrieves the current value of the high-resolution performance counter. 
		/// </summary>
		/// <param name="x">Specifies the variable to receive the current performance-counter value, in counts.</param>
		/// <returns>If the function succeeds, the return value is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
		[DllImport(DLL)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean QueryPerformanceCounter(ref long x);
	}
}

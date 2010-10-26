/* -----------------------------------------------
 * NuGenOS.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;

namespace Genetibase.Shared.Environment
{
	/// <summary>
	/// Provides methods and properties for NuGenOS version processing.
	/// </summary>
	public static class NuGenOS
	{
		/*
		 * IsCompositionEnabled
		 */

		/// <summary>
		/// Gets the value indicating whether Desktop Window Manager (DWM) composition is enabled.
		/// </summary>
		public static Boolean IsCompositionEnabled
		{
			get
			{
				if (!NuGenOS.IsVista)
				{
					return false;
				}

				return DwmApi.DwmIsCompositionEnabled();
			}
		}

		/*
		 * IsWindows2003
		 */

		/// <summary>
		/// Checks if the current operating system is Microsoft Windows 2003.
		/// </summary>
		public static Boolean IsWindows2003
		{
			get
			{
				return System.Environment.OSVersion.Platform == PlatformID.Win32NT
					&& System.Environment.OSVersion.Version.Major == 5
					&& System.Environment.OSVersion.Version.Minor == 2;
			}
		}

		/*
		 * IsWindowsXP
		 */

		/// <summary>
		/// Checks if the current operating system is Microsoft Windows XP.
		/// </summary>
		public static Boolean IsWindowsXP
		{
			get
			{
				return System.Environment.OSVersion.Platform == PlatformID.Win32NT
					&& System.Environment.OSVersion.Version.Major == 5
					&& System.Environment.OSVersion.Version.Minor == 1;
			}
		}

		/*
		 * IsWindowsVista
		 */

		/// <summary>
		/// Checks if the current operating system is Microsft Vista.
		/// </summary>
		public static Boolean IsVista
		{
			get
			{
				return System.Environment.OSVersion.Platform == PlatformID.Win32NT
					&& !(System.Environment.OSVersion.Version.Major < 6);
			}
		}

		/*
		 * Windows2000Version
		 */

		/// <summary>
		/// </summary>
		public static Version Windows2000Version
		{
			get
			{
				return new Version(5, 0);
			}
		}

		/*
		 * WindowsXPVersion
		 */

		/// <summary>
		/// </summary>
		public static Version WindowsXPVersion
		{
			get
			{
				return new Version(5, 1);
			}
		}

		/*
		 * Windows2003Version
		 */

		/// <summary>
		/// </summary>
		public static Version Windows2003Version
		{
			get
			{
				return new Version(5, 2);
			}
		}

		/*
		 * WindowsVistaVersion
		 */

		/// <summary>
		/// </summary>
		public static Version WindowsVistaVersion
		{
			get
			{
				return new Version(6, 0);
			}
		}
	}
}

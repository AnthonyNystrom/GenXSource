/* -----------------------------------------------
 * NuGenOS.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;

namespace Genetibase.Shared.Service
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
		public static bool IsCompositionEnabled
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
		 * IsWindowsXP
		 */

		/// <summary>
		/// Checks if the current operating system is Microsoft Windows XP.
		/// </summary>
		public static bool IsWindowsXP
		{
			get
			{
				return
					Environment.OSVersion.Platform == PlatformID.Win32NT &&
					Environment.OSVersion.Version.Major == 5 &&
					Environment.OSVersion.Version.Minor == 1;
			}
		}

		/*
		 * IsWindowsVista
		 */

		/// <summary>
		/// Checks if the current operating system is Microsft Vista.
		/// </summary>
		public static bool IsVista
		{
			get
			{
				return
					Environment.OSVersion.Platform == PlatformID.Win32NT && 
					!(Environment.OSVersion.Version.Major < 6);
			}
		}
	}
}

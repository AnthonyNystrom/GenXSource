/* -----------------------------------------------
 * DwmApi.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports DwmApi.dll functions.
	/// </summary>
	public static class DwmApi
	{
		#region DWMWINDOWATTRIBUTE

		/// <summary>
		/// Specifies window attributes for non-client rendering.
		/// </summary>
		public static class DWMWINDOWATTRIBUTE
		{
			/// <summary>
			/// The attribute to determine non-client rendering.
			/// When calling DwmGetWindowAttribute, the value type is BOOL.
			/// </summary>
			public static int DWMWA_NCRENDERING_ENABLED = 1;
			
			/// <summary>
			/// The attribute to retrieve or set non-client rendering policy.
			/// When calling DwmSetWindowAttribute, the value type is DWMNCRENDERINGPOLICY.
			/// </summary>
			public static int DWMWA_NCRENDERING_POLICY = 2;
			
			/// <summary>
			/// The attribute to enable or forcibly disable Desktop Window Manager (DWM)
			/// transitions. When calling DwmSetWindowAttribute, the value type is BOOL.
			/// </summary>
			public static int DWMWA_TRANSITIONS_FORCEDISABLED = 3;
			
			/// <summary>
			/// The attribute to allow content rendered in the non-client area to be visible
			/// on the DWM drawn frame.
			/// </summary>
			public static int DWMWA_ALLOW_NCPAINT = 4;
			
			/// <summary>
			/// Sentinel value.
			/// </summary>
			public static int DWMWA_LAST = 5;
		}

		#endregion

		#region Methods.Public.Static

		/// <summary>
		/// Retrieves the current value of the specified DWMWINDOWATTRIBUTE that is applied to the window.
		/// </summary>
		/// <param name="hWnd">The handle to the window for which the attribute data is retrieved.</param>
		/// <param name="dwAttribute">The DWMWINDOWATTRIBUTE to retrieve.</param>
		/// <param name="pvAttribute">The pointer that receives the current value of the dwAttribute.
		/// The type is dependent on the value of the dwAttribute parameter.</param>
		/// <param name="cbAttribute">The size of the DWMWINDOWATTRIBUTE value being retrieved. The size is
		/// dependent on the type of the pvAttribute parameter.</param>
		/// <returns>S_OK if successful, or an error value otherwise.</returns>
		[DllImport("DwmApi.dll")]
		public static extern int DwmGetWindowAttribute(
			IntPtr hWnd,
			int dwAttribute,
			ref int pvAttribute,
			int cbAttribute
		);

		/// <summary>
		/// Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
		/// Applications can listen for composition state changes by handling the WM_DWMCOMPOSITIONCHANGED
		/// notification.
		/// </summary>
		/// <returns>S_OK if successful, or an error value otherwise.</returns>
		[DllImport("DwmApi.dll")]
		public static extern bool DwmIsCompositionEnabled();

		/// <summary>
		/// Sets the value of the specified DWMWINDOWATTRIBUTE to apply to the window.
		/// </summary>
		/// <param name="hWnd">The window handle to apply the given attribute.</param>
		/// <param name="dwAttribute">The DWMWINDOWATTRIBUTE to apply to the window.</param>
		/// <param name="pvAttribute">The pointer to the DWMWINDOWATTRIBUTE value to apply.
		/// The type is dependent on the value of the dwAttribute parameter.</param>
		/// <param name="cbAttribute">The size of the DWMWINDOWATTRIBUTE value being retrieved.
		/// The size is dependent on the type of the pvAttribute parameter.</param>
		/// <returns>S_OK if successful, or an error value otherwise.</returns>
		[DllImport("DwmApi.dll")]
		public static extern int DwmSetWindowAttribute(
			IntPtr hWnd,
			int dwAttribute,
			ref int pvAttribute,
			int cbAttribute
			);

		#endregion
	}
}

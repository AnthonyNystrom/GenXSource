/* -----------------------------------------------
 * Window.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi.Properties;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Provides functionality to process window parameters.
	/// </summary>
	public static class Window
	{
		/*
		 * GetBorderStyle
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <exception cref="NuGenInvalidHWndException">
		/// <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static FormBorderStyle GetBorderStyle(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			int style = GetStyle(hWnd);
			int exStyle = GetExStyle(hWnd);

			/* SizableToolWindow */
			
			if (
				(style & WinUser.WS_THICKFRAME) == WinUser.WS_THICKFRAME
				&& (exStyle & WinUser.WS_EX_TOOLWINDOW) == WinUser.WS_EX_TOOLWINDOW
				)
			{
				return FormBorderStyle.SizableToolWindow;
			}

			/* Sizable */

			if (
				(style & WinUser.WS_THICKFRAME) == WinUser.WS_THICKFRAME
				)
			{
				return FormBorderStyle.Sizable;
			}

			/* FixedToolWindow */

			if (
				(exStyle & WinUser.WS_EX_TOOLWINDOW) == WinUser.WS_EX_TOOLWINDOW
				)
			{
				return FormBorderStyle.FixedToolWindow;
			}

			/* FixedDialog */

			if (
				(exStyle & WinUser.WS_EX_DLGMODALFRAME) == WinUser.WS_EX_DLGMODALFRAME
				)
			{
				return FormBorderStyle.FixedDialog;
			}

			/* Fixed3D */

			if (
				(exStyle & WinUser.WS_EX_OVERLAPPEDWINDOW) == WinUser.WS_EX_OVERLAPPEDWINDOW
				)
			{
				return FormBorderStyle.Fixed3D;
			}

			/* FixedSingle */

			if (
				(exStyle & WinUser.WS_EX_WINDOWEDGE) == WinUser.WS_EX_WINDOWEDGE
				)
			{
				return FormBorderStyle.FixedSingle;
			}

			/* None */

			return FormBorderStyle.None;
		}

		/*
		 * GetBounds
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <exception cref="NuGenInvalidHWndException">
		/// Specified <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static Rectangle GetBounds(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			RECT rect = new RECT();
			User32.GetWindowRect(hWnd, ref rect);

			return rect;
		}

		/*
		 * GetExStyle
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <exception cref="NuGenInvalidHWndException">
		/// <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static int GetExStyle(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			return User32.GetWindowLong(hWnd, WinUser.GWL_EXSTYLE);
		}

		/*
		 * GetState
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		/// <exception cref="NuGenInvalidHWndException">
		/// <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static FormWindowState GetState(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			int style = GetStyle(hWnd);

			if ((style & WinUser.WS_MAXIMIZE) != 0)
			{
				return FormWindowState.Maximized;
			}

			if ((style & WinUser.WS_MINIMIZE) != 0)
			{
				return FormWindowState.Minimized;
			}

			return FormWindowState.Normal;
		}

		/*
		 * GetStyle
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <exception cref="NuGenInvalidHWndException">
		/// <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static int GetStyle(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			return User32.GetWindowLong(hWnd, WinUser.GWL_STYLE);
		}

		/*
		 * IsRightToLeft
		 */

		/// <summary>
		/// </summary>
		/// <param name="hWnd"></param>
		/// <exception cref="NuGenInvalidHWndException">
		/// <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		public static bool IsRightToLeft(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			int exStyle = Window.GetExStyle(hWnd);

			if (
				(exStyle & WinUser.WS_EX_RIGHT) == WinUser.WS_EX_RIGHT
				)
			{
				return true;
			}

			return false;
		}
	}
}

/* -----------------------------------------------
 * NuGenControlPaint.MouseInterop.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * BuildMouseEventArgs 
		 */

		/// <summary>
		/// </summary>
		public static MouseEventArgs BuildMouseEventArgs(IntPtr wParam, IntPtr lParam)
		{
			return BuildMouseEventArgs(wParam, lParam, 1);
		}

		/// <summary>
		/// </summary>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <param name="clicks">Specifies the number of clicks. Default value is 1.</param>
		public static MouseEventArgs BuildMouseEventArgs(IntPtr wParam, IntPtr lParam, int clicks)
		{
			return new MouseEventArgs(
				MouseButtonFromWParam(wParam),
				clicks,
				MouseXPosFromLParam(lParam),
				MouseYPosFromLParam(lParam),
				0
			);
		}

		/*
		 * BuildMousePos
		 */

		/// <summary>
		/// </summary>
		public static Point BuildMousePos(IntPtr lParam)
		{
			return new Point(
				MouseXPosFromLParam(lParam),
				MouseYPosFromLParam(lParam)
			);
		}

		/*
		 * MouseButtonFromWParam
		 */

		/// <summary>
		/// </summary>
		public static MouseButtons MouseButtonFromWParam(IntPtr wParam)
		{
			MouseButtons mouseButton = MouseButtons.None;

			int fwKeys = wParam.ToInt32();

			if ((fwKeys & WinUser.MK_LBUTTON) != 0)
			{
				mouseButton |= MouseButtons.Left;
			}
			
			if ((fwKeys & WinUser.MK_MBUTTON) != 0)
			{
				mouseButton |= MouseButtons.Middle;
			}
			
			if ((fwKeys & WinUser.MK_RBUTTON) != 0)
			{
				mouseButton |= MouseButtons.Right;
			}
			
			if ((fwKeys & WinUser.MK_XBUTTON1) != 0)
			{
				mouseButton |= MouseButtons.XButton1;
			}
			
			if ((fwKeys & WinUser.MK_XBUTTON2) != 0)
			{
				mouseButton |= MouseButtons.XButton2;
			}

			return mouseButton;
		}

		/*
		 * MouseXPosFromLParam
		 */

		/// <summary>
		/// </summary>
		public static int MouseXPosFromLParam(IntPtr lParam)
		{
			return Common.LoWord(lParam.ToInt32());
		}

		/*
		 * MouseYPosFromLParam
		 */

		/// <summary>
		/// </summary>
		public static int MouseYPosFromLParam(IntPtr lParam)
		{
			return Common.HiWord(lParam.ToInt32());
		}
	}
}

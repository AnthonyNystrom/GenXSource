/* -----------------------------------------------
 * NuGenControlPaint.Color.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
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
		 * DrawToBitmap
		 */

		/// <summary>
		/// Draws the specified <see cref="T:System.Windows.Forms.Control"/> to the specified
		/// <see cref="T:System.Drawing.Bitmap"/>.
		/// </summary>
		/// <param name="ctrl">The <see cref="T:System.Windows.Forms.Control"/> to draw.</param>
		/// <param name="bmp">The <see cref="T:System.Drawing.Bitmap"/> to draw on.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="ctrl"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="bmp"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawToBitmap(Control ctrl, Bitmap bmp)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			if (bmp == null)
			{
				throw new ArgumentNullException("bmp");
			}

			using (Graphics g = Graphics.FromImage(bmp))
			{
				IntPtr hDC = g.GetHdc();

				User32.SendMessage(
					ctrl.Handle,
					WinUser.WM_PRINT,
					hDC,
					(IntPtr)(WinUser.PRF_CHILDREN | WinUser.PRF_CLIENT | WinUser.PRF_NONCLIENT | WinUser.PRF_ERASEBKGND)
					);

				g.ReleaseHdc(hDC);
			}
		}
	}
}

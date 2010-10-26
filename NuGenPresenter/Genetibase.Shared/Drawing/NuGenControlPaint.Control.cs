/* -----------------------------------------------
 * NuGenControlPaint.Color.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Properties;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
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
		/// <exception cref="T:System.ArgumentNullException">
		/// <para>
		///		<paramref name="ctrl"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="bmp"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static void DrawToBitmap(Control ctrl, Bitmap bmp)
		{
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}

			DrawToBitmap(ctrl.Handle, bmp);
		}

		/// <summary>
		/// Draws the specified window to the specified <see cref="T:System.Drawing.Bitmap"/>.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="hWnd"/> does not represent a window.</para>
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="bmp"/> is <see langword="null"/>.</para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public static void DrawToBitmap(IntPtr hWnd, Bitmap bmp)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.Argument_InvalidHWnd, hWnd.ToInt32().ToString(CultureInfo.InvariantCulture)));
			}

			if (bmp == null)
			{
				throw new ArgumentNullException("bmp");
			}

			using (Graphics g = Graphics.FromImage(bmp))
			{
				IntPtr hDC = g.GetHdc();

				User32.SendMessage(
					hWnd,
					WinUser.WM_PRINT,
					hDC,
					(IntPtr)(WinUser.PRF_CHILDREN | WinUser.PRF_CLIENT | WinUser.PRF_NONCLIENT | WinUser.PRF_ERASEBKGND)
					);

				g.ReleaseHdc(hDC);
			}
		}

		/*
		 * DropDownButtonBounds
		 */

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <param name="rightToLeft"></param>
		/// <returns></returns>
		public static Rectangle DropDownButtonBounds(Rectangle clientRectangle, RightToLeft rightToLeft)
		{
			Rectangle buttonBounds = Rectangle.Empty;

			if (rightToLeft == RightToLeft.No)
			{
				buttonBounds = new Rectangle(
					clientRectangle.Right - SystemInformation.VerticalScrollBarWidth - SystemInformation.Border3DSize.Width,
					clientRectangle.Top,
					SystemInformation.VerticalScrollBarWidth + SystemInformation.Border3DSize.Width,
					clientRectangle.Height
				);
			}
			else
			{
				buttonBounds = new Rectangle(
					clientRectangle.Left,
					clientRectangle.Top,
					SystemInformation.VerticalScrollBarWidth + SystemInformation.Border3DSize.Width,
					clientRectangle.Height
				);
			}

			return buttonBounds;
		}

		/*
		 * RTLContentAlignment
		 */

		/// <summary>
		/// Converts the specified <see cref="ContentAlignment"/> to its right-to-left representation.
		/// </summary>
		/// <returns>
		/// The specified <see cref="ContentAlignment"/> is converted only if right-to-left layout is specified;
		/// otherwise, it is left as is.
		/// </returns>
		public static ContentAlignment RTLContentAlignment(ContentAlignment contentAlignment, RightToLeft rightToLeft)
		{
			if (rightToLeft == RightToLeft.Yes)
			{
				switch (contentAlignment)
				{
					case ContentAlignment.BottomLeft: return ContentAlignment.BottomRight;
					case ContentAlignment.BottomRight: return ContentAlignment.BottomLeft;
					case ContentAlignment.MiddleLeft: return ContentAlignment.MiddleRight;
					case ContentAlignment.MiddleRight: return ContentAlignment.MiddleLeft;
					case ContentAlignment.TopLeft: return ContentAlignment.TopRight;
					case ContentAlignment.TopRight: return ContentAlignment.TopLeft;
					default: return contentAlignment;
				}
			}

			return contentAlignment;
		}
	}
}

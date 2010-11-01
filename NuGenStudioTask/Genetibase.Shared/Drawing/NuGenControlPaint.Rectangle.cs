/* -----------------------------------------------
 * NuGenControlPaint.Rectangles.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * DeflateRectangle
		 */

		/// <summary>
		/// Deflates the specified <see cref="T:System.Drawing.Rectangle"/> according to the specified 
		/// <see cref="T:System.Windows.Forms.Padding"/>.
		/// </summary>
		/// <param name="rect">Specifies the <see cref="T:System.Drawing.Rectangle"/> to deflate.</param>
		/// <param name="padding">Specifies the <see cref="T:System.Windows.Forms.Padding"/> to deflate by.</param>
		public static Rectangle DeflateRectangle(Rectangle rect, Padding padding)
		{
			rect.X += padding.Left;
			rect.Y += padding.Top;
			rect.Width -= padding.Horizontal;
			rect.Height -= padding.Vertical;

			return rect;
		}

		/*
		 * DrawReversibleRectangle
		 */

		/// <summary>
		/// Draws a reversible rectangle with the specified bounds on the specified <see cref="T:Control"/>.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="controlToDrawOn"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawReversibleRectangle(Rectangle reversibleRectangleBounds, Control ctrlToDrawOn)
		{
			if (ctrlToDrawOn == null)
			{
				throw new ArgumentNullException("ctrlToDrawOn");
			}

			if (!reversibleRectangleBounds.IsEmpty)
			{
				IntPtr hDC = User32.GetDCEx(ctrlToDrawOn.Handle, IntPtr.Zero, WinUser.DCX_PARENTCLIP);
				Gdi32.SetROP2(hDC, WinGdi.R2_XORPEN);

				IntPtr hPen = Gdi32.CreatePen(WinGdi.PS_DOT, 1, ColorTranslator.ToWin32(Color.Black));

				IntPtr oldBrush = Gdi32.SelectObject(hDC, Gdi32.GetStockObject(WinGdi.NULL_BRUSH));
				IntPtr oldPen = Gdi32.SelectObject(hDC, hPen);

				Gdi32.SetBkColor(hDC, ColorTranslator.ToWin32(Color.White));

				/*
				 * Do not use Gdi32.Rectangle here to get more regular frame corners.
				 */

				Gdi32.MoveTo(hDC, reversibleRectangleBounds.Left, reversibleRectangleBounds.Top);
				Gdi32.LineTo(hDC, reversibleRectangleBounds.Right, reversibleRectangleBounds.Top);
				Gdi32.LineTo(hDC, reversibleRectangleBounds.Right, reversibleRectangleBounds.Bottom);
				Gdi32.LineTo(hDC, reversibleRectangleBounds.Left, reversibleRectangleBounds.Bottom);
				Gdi32.LineTo(hDC, reversibleRectangleBounds.Left, reversibleRectangleBounds.Top);

				Gdi32.SetROP2(hDC, WinGdi.R2_NOP);

				Gdi32.SelectObject(hDC, oldBrush);
				Gdi32.SelectObject(hDC, oldPen);

				Gdi32.DeleteObject(hPen);

				User32.ReleaseDC(ctrlToDrawOn.Handle, hDC);
			}
		}

		/*
		 * DrawRoundRectangle
		 */

		/// <summary>
		/// Draws a round rectangle on the specified graphics surface with the specified pen within the
		/// specified bounds and with the specified radius.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="pen"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void DrawRoundRectangle(Graphics g, Pen pen, Rectangle rectangleBounds, float radius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			using (GraphicsPath path = NuGenControlPaint.GetRoundRectangleGraphicsPath(rectangleBounds, radius))
			{
				g.DrawPath(pen, path);
			}
		}

		/*
		 * DrawHalfRoundRectangle
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="pen"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void DrawHalfRoundRectangle(Graphics g, Pen pen, Rectangle rectangleBounds, float radius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			using (GraphicsPath path = NuGenControlPaint.GetHalfRoundRectangleGraphicsPath(rectangleBounds, radius))
			{
				g.DrawPath(pen, path);
			}
		}

		/*
		 * FillRoundRectangle
		 */

		/// <summary>
		/// Fills a round rectangle on the specified graphics surface with the specified pen within the
		/// specified bounds and with the specified radius.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="brush"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void FillRoundRectangle(Graphics g, Brush brush, Rectangle rectangleBounds, float radius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			
			using (GraphicsPath path = NuGenControlPaint.GetRoundRectangleGraphicsPath(rectangleBounds, radius))
			{
				g.FillPath(brush, path);
			}
		}

		/*
		 * FillHalfRoundRectangle
		 */

		/// <summary>
		/// Fills a half-round rectangle on the specified graphics surface with the specified pen within the
		/// specified bounds and with the specified radius.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="brush"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void FillHalfRoundRectangle(Graphics g, Brush brush, Rectangle rectangleBounds, float radius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}

			using (GraphicsPath path = NuGenControlPaint.GetHalfRoundRectangleGraphicsPath(rectangleBounds, radius))
			{
				g.FillPath(brush, path);
			}
		}

		/*
		 * RectBCCorner
		 */

		/// <summary>
		/// Gets the coordinates of the bottom-center rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectBCCorner(Rectangle rect)
		{
			return new Point(
				rect.Left + rect.Width / 2,
				rect.Bottom
				);
		}

		/*
		 * RectBLCorner
		 */

		/// <summary>
		/// Gets the coordinates of the bottom-left rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectBLCorner(Rectangle rect)
		{
			return new Point(
				rect.Left,
				rect.Bottom
				);
		}

		/*
		 * RectBRCorner
		 */

		/// <summary>
		/// Gets the coordinates of the bottom-right rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectBRCorner(Rectangle rect)
		{
			return new Point(
				rect.Right,
				rect.Bottom
				);
		}

		/*
		 * RectTCCorner
		 */

		/// <summary>
		/// Gets the coordinates of the top-center rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectTCCorner(Rectangle rect)
		{
			return new Point(
				rect.Left + rect.Width / 2,
				rect.Top
				);
		}

		/*
		 * RectTLCorner
		 */

		/// <summary>
		/// Gets the coordinates of the top-left rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectTLCorner(Rectangle rect)
		{
			return new Point(
				rect.Left,
				rect.Top
				);
		}

		/*
		 * RectTRCorner
		 */

		/// <summary>
		/// Gets the coordinates of the top-right rectangle corner.
		/// </summary>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle"/> to get the corner coordinates for.</param>
		/// <returns></returns>
		public static Point RectTRCorner(Rectangle rect)
		{
			return new Point(
				rect.Right,
				rect.Top
				);
		}

		/*
		 * GetRoundRectangleGraphicsPath
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		///	<para>
		///		<paramref name="radius"/> should be positive.
		/// </para>
		/// </exception>
		public static GraphicsPath GetRoundRectangleGraphicsPath(Rectangle rectangleBounds, float radius)
		{
			/*
			 * Verify input parameters.
			 */

			if (rectangleBounds.IsEmpty)
			{
				return new GraphicsPath();
			}

			if (radius <= 0)
			{
				throw new ArgumentException(
					Properties.Resources.Argument_NotPositiveRadius
					);
			}

			/*
			 * Build System.Drawing.Drawing2D.GraphicsPath.
			 */

			float size = radius * 2f;

			GraphicsPath path = new GraphicsPath();

			path.AddArc(rectangleBounds.X, rectangleBounds.Y, size, size, 180, 90);
			path.AddArc(rectangleBounds.X + rectangleBounds.Width - size, rectangleBounds.Y, size, size, 270, 90);
			path.AddArc(rectangleBounds.X + rectangleBounds.Width - size, rectangleBounds.Y + rectangleBounds.Height - size, size, size, 0, 90);
			path.AddArc(rectangleBounds.X, rectangleBounds.Y + rectangleBounds.Height - size, size, size, 90, 90);
			path.CloseFigure();

			return path;
		}

		/*
		 * GetHalfRoundRectangleGraphicsPath
		 */

		/// <summary>
		/// </summary>
		public static GraphicsPath GetHalfRoundRectangleGraphicsPath(Rectangle rectangleBounds, float radius)
		{
			/*
			 * Verify input parameters.
			 */

			if (rectangleBounds.IsEmpty)
			{
				return new GraphicsPath();
			}

			if (radius <= 0)
			{
				throw new ArgumentException(
					Properties.Resources.Argument_NotPositiveRadius
					);
			}

			float size = radius * 2f;

			GraphicsPath path = new GraphicsPath();

			path.AddArc(rectangleBounds.X, rectangleBounds.Y, size, size, 180, 90);
			path.AddArc(rectangleBounds.X + rectangleBounds.Width - size, rectangleBounds.Y, size, size, 270, 90);
			path.AddLine(
				NuGenControlPaint.RectBRCorner(rectangleBounds),
				NuGenControlPaint.RectBLCorner(rectangleBounds)
			);
			path.CloseFigure();

			return path;
		}
	}
}

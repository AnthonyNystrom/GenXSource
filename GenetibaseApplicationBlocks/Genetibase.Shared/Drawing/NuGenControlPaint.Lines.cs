/* -----------------------------------------------
 * NuGenControlPaint.Lines.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * DrawLine
		 */

		/// <summary>
		/// A faster method to draw a line on the specified graphics surface with the specified <see cref="Pen"/>.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="pen"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void DrawLine(Graphics g, Pen pen, int x1, int y1, int x2, int y2)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			IntPtr hDC = g.GetHdc();

			IntPtr hPen = Gdi32.CreatePen(pen);

			IntPtr oldBrush = Gdi32.SelectObject(hDC, Gdi32.GetStockObject(WinGdi.NULL_BRUSH));
			IntPtr oldPen = Gdi32.SelectObject(hDC, hPen);

			Gdi32.MoveTo(hDC, x1, y1);
			Gdi32.LineTo(hDC, x2, y2);

			Gdi32.SelectObject(hDC, oldBrush);
			Gdi32.SelectObject(hDC, oldPen);

			Gdi32.DeleteObject(hPen);

			g.ReleaseHdc(hDC);
		}

		/*
		 * DrawLine
		 */

		/// <summary>
		/// A faster method to draw a line on the specified graphics surface with the specified <see cref="T:Pen"/>.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="pen"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void DrawLine(Graphics g, Pen pen, Point pt1, Point pt2)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			NuGenControlPaint.DrawLine(
				g,
				pen,
				pt1.X,
				pt1.Y,
				pt2.X,
				pt2.Y
				);
		}

		/*
		 * DrawReversibleLine
		 */

		/// <summary>
		/// Draws a reversible line.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawReversibleLine(Graphics g, Point pt1, Point pt2)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			DrawReversibleLine(g, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/*
		 * DrawReversibleLine
		 */

		/// <summary>
		/// Draws a reversible line.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawReversibleLine(Graphics g, int x1, int y1, int x2, int y2)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			IntPtr hDC = g.GetHdc();

			Gdi32.SetROP2(hDC, WinGdi.R2_NOT);

			Gdi32.MoveTo(hDC, x1, y1);
			Gdi32.LineTo(hDC, x2, y2);

			Gdi32.SetROP2(hDC, WinGdi.R2_NOP);

			g.ReleaseHdc(hDC);
		}
	}
}

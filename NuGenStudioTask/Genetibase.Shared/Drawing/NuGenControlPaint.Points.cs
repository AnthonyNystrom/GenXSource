/* -----------------------------------------------
 * NuGenControlPaint.Points.cs
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
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * DrawPoint
		 */

		/// <summary>
		/// Draws the point on the specified graphics surface with the specified color at the specified
		/// coordinates.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawPoint(Graphics g, Color color, int x, int y)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			IntPtr hDC = g.GetHdc();
			Gdi32.SetPixelV(hDC, x, y, color);
			g.ReleaseHdc(hDC);
		}

		/*
		 * TranslatePoint
		 */

		/// <summary>
		/// Translates the specified point coordinates from the source control client coordinates into the
		/// target control client coordinates.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="from"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="to"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Point TranslatePoint(int px, int py, Control from, Control to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}

			if (to == null)
			{
				throw new ArgumentNullException("to");
			}

			return TranslatePoint(new Point(px, py), from, to);
		}

		/*
		 * TranslatePoint
		 */

		/// <summary>
		/// Translates the specified point coordinates from the source control client coordinates into the
		/// target control client coordinates.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <para><paramref name="from"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="to"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Point TranslatePoint(Point p, Control from, Control to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}

			if (to == null)
			{
				throw new ArgumentNullException("to");
			}

			return to.PointToClient(from.PointToScreen(p));
		}
	}
}

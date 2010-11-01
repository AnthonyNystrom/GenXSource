/* -----------------------------------------------
 * NuGenControlPaint.Borders.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * DrawBorder
		 */

		/// <summary>
		/// Draws the border with the specified style.
		/// </summary>
		/// <param name="g"><see cref="T:System.Drawing.Graphics"/> to draw on.</param>
		/// <param name="rect">The rectangle to fit the border in.</param>
		/// <param name="borderColor">Border color.</param>
		/// <param name="borderStyle">Border style.</param>
		public static void DrawBorder(Graphics g, Rectangle rect, Color borderColor, NuGenBorderStyle borderStyle)
		{
			if (borderStyle == NuGenBorderStyle.None)
				return;

			SmoothingMode previousSmoothingMode = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.Default;

			if (borderStyle == NuGenBorderStyle.Bump
				|| borderStyle == NuGenBorderStyle.Etched
				|| borderStyle == NuGenBorderStyle.Flat
				|| borderStyle == NuGenBorderStyle.Raised
				|| borderStyle == NuGenBorderStyle.RaisedInner
				|| borderStyle == NuGenBorderStyle.RaisedOuter
				|| borderStyle == NuGenBorderStyle.Sunken
				|| borderStyle == NuGenBorderStyle.SunkenInner
				|| borderStyle == NuGenBorderStyle.SunkenOuter)
			{
				ControlPaint.DrawBorder3D(g, rect, (Border3DStyle)borderStyle);
			}
			else
			{
				using (SolidBrush sb = new SolidBrush(borderColor))
				using (Pen p = new Pen(sb))
				{
					switch (borderStyle)
					{
						case NuGenBorderStyle.Dashed:
						p.DashStyle = DashStyle.Dash;
						break;
						case NuGenBorderStyle.Dotted:
						p.DashStyle = DashStyle.Dot;
						break;
						case NuGenBorderStyle.Solid:
						p.DashStyle = DashStyle.Solid;
						break;
					}

					g.DrawRectangle(p, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1));
				}
			}

			g.SmoothingMode = previousSmoothingMode;
		}
	}
}

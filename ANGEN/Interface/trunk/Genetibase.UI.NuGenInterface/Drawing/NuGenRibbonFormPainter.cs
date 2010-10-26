/* -----------------------------------------------
 * NuGenRibbonFormPainter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Drawing
{
	/// <summary>
	/// Provides helper methods to draw a form that imitates Microsoft Office 2007 style.
	/// </summary>
	static class NuGenRibbonFormPainter
	{
		#region Methods.Public.Static

		/*
		 * AddArcFromDescriptor
		 */

		/// <summary>
		/// Adds an arc specified by the <see cref="NuGenArcDescriptor"/> to the specified
		/// <see cref="GraphicsPath"/>.
		/// </summary>
		/// <param name="path">Specifies the <see cref="GraphicsPath"/> to
		/// add the arc to.</param>
		/// <param name="arcDescriptor">Specifies the <see cref="NuGenArcDescriptor"/> that describes the
		/// arc to add.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// </exception>
		public static void AddArcFromDescriptor(GraphicsPath path, NuGenArcDescriptor arcDescriptor)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}

			path.AddArc(
				arcDescriptor.X,
				arcDescriptor.Y,
				arcDescriptor.Width,
				arcDescriptor.Height,
				arcDescriptor.StartAngle,
				arcDescriptor.SweepAngle
			);
		}

		/*
		 * BuildArcDescriptor
		 */

		/// <summary>
		/// Retrieves <see cref="NuGenArcDescriptor"/> according to the specified form bounds, size of the
		/// arc and the corner to build the arc for.
		/// </summary>
		/// <param name="bounds">Specifies the bounds of the form to imitate Microsoft Office 2007 style.</param>
		/// <param name="arcSize">Specifies the size of the arc to build.</param>
		/// <param name="cornerStyle">Specifies the type of the corner to build the arc for.</param>
		/// <returns></returns>
		public static NuGenArcDescriptor BuildArcDescriptor(
			RectangleF bounds,
			int arcSize,
			NuGenCornerStyle cornerStyle
			)
		{
			arcSize *= 2;

			switch (cornerStyle)
			{
				case NuGenCornerStyle.BottomLeft:
				{
					return new NuGenArcDescriptor(
						bounds.X,
						bounds.Bottom - arcSize,
						arcSize,
						arcSize,
						90,
						90
						);
				}
				case NuGenCornerStyle.BottomRight:
				{
					return new NuGenArcDescriptor(
						bounds.Right - arcSize,
						bounds.Bottom - arcSize,
						arcSize,
						arcSize,
						0,
						90
						);
				}
				case NuGenCornerStyle.TopRight:
				{
					return new NuGenArcDescriptor(
						bounds.Right - arcSize,
						bounds.Y,
						arcSize,
						arcSize,
						270,
						90
						);
				}
				default:
				{
					return new NuGenArcDescriptor(
						bounds.X,
						bounds.Y,
						arcSize,
						arcSize,
						180,
						90
						);
				}
			}
		}

		/*
		 * DrawBorders
		 */

		/// <summary>
		/// Draws borders on the specified GDI+ drawing surface with the specified gradient and the
		/// <see cref="Pen"/> of the specified width.
		/// </summary>
		/// <param name="g">Specifies the drawing surface to draw on.</param>
		/// <param name="borderPath">Specifies the path for the border to draw.</param>
		/// <param name="startColor">Specifies the gradient start color.</param>
		/// <param name="endColor">Specifies the gradient end color.</param>
		/// <param name="gradientAngle">Specifies the gradient angle.</param>
		/// <param name="penWidth">Specifies the width of the pen to draw borders with.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>
		/// -or-
		/// <paramref name="borderPath"/> is <see langword="null"/>.
		/// </exception>
		public static void DrawBorders(
			Graphics g,
			GraphicsPath borderPath,
			Color startColor,
			Color endColor,
			int gradientAngle,
			int penWidth
			)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (borderPath == null)
			{
				throw new ArgumentNullException("borderPath");
			}

			using (Pen pen = new Pen(startColor, penWidth))
			{
				borderPath.Widen(pen);
			}

			RectangleF borderRectangle = borderPath.GetBounds();
			borderRectangle.Inflate(1, 1);

			if (endColor.IsEmpty)
			{
				if (startColor.IsEmpty)
				{
					return;
				}

				using (SolidBrush sb = new SolidBrush(startColor))
				{
					g.FillPath(sb, borderPath);
					return;
				}
			}

			if (!startColor.IsEmpty)
			{
				using (LinearGradientBrush lgb = new LinearGradientBrush(borderRectangle, startColor, endColor, gradientAngle))
				{
					g.FillPath(lgb, borderPath);
				}
			}
		}

		#endregion
	}
}

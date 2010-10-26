/* -----------------------------------------------
 * NuGenControlPaint.Coordinates.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * DegToRad
		 */

		/// <summary>
		/// Convertes the specified angle value in degrees to radians.
		/// </summary>
		/// <param name="angle">Specifies the angle in degrees to convert.</param>
		/// <returns>The appropriate value in radians for the specified angle.</returns>
		public static double DegToRad(double angle)
		{
			return angle * Math.PI / 180.0;
		}

		/*
		 * MillimetersToPixels
		 */

		/// <summary>
		/// Converts the specified size from millimeters to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="size">The <see cref="T:System.Drawing.Size"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static Size MillimetersToPixels(Graphics g, Size size)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Millimeter;

			return new Size(
				(int)((float)size.Width / MM_IN_INCH * g.DpiX),
				(int)((float)size.Height / MM_IN_INCH * g.DpiY)
				);
		}

		/*
		 * MillimetersToPixels
		 */

		/// <summary>
		/// Converts the specified size from millimeters to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static SizeF MillimetersToPixels(Graphics g, SizeF size)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			
			g.PageUnit = GraphicsUnit.Millimeter;
			
			return new SizeF(
				size.Width / MM_IN_INCH * g.DpiX,
				size.Height / MM_IN_INCH * g.DpiY
				);
		}

		/*
		 * MillimetersToPixels
		 */

		/// <summary>
		/// Converts the specified location from millimeters to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="point">The <see cref="T:System.Drawing.Point"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static Point MillimetersToPixels(Graphics g, Point point)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Millimeter;

			return new Point(
				(int)((float)point.X / MM_IN_INCH * g.DpiX),
				(int)((float)point.Y / MM_IN_INCH * g.DpiY)
				);
		}

		/*
		 * MillimetersToPixels
		 */

		/// <summary>
		/// Converts the specified location from millimeters to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="point">The <see cref="T:System.Drawing.PointF"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static PointF MillimetersToPixels(Graphics g, PointF point)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Millimeter;
			
			return new PointF(
				point.X / MM_IN_INCH * g.DpiX,
				point.Y / MM_IN_INCH * g.DpiY
				);
		}

		/*
		 * InchesToPixels
		 */

		/// <summary>
		/// Converts the specified size from inches to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="size">The <see cref="T:System.Drawing.Size"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static Size InchesToPixels(Graphics g, Size size)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Inch;
			
			return new Size(
				(int)((float)size.Width * g.DpiX),
				(int)((float)size.Height * g.DpiY)
				);
		}

		/*
		 * InchesToPixels
		 */

		/// <summary>
		/// Converts the specified size from inches to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="size">The <see cref="T:System.Drawing.SizeF"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static SizeF InchesToPixels(Graphics g, SizeF size)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Inch;
			
			return new SizeF(
				size.Width * g.DpiX,
				size.Height * g.DpiY
				);
		}

		/*
		 * InchesToPixels
		 */

		/// <summary>
		/// Converts the specified location from inches to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="point">The <see cref="T:System.Drawing.Point"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static Point InchesToPixels(Graphics g, Point point)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Inch;
			
			return new Point(
				(int)((float)point.X * g.DpiX),
				(int)((float)point.Y * g.DpiY)
				);
		}

		/*
		 * InchesToPixels
		 */

		/// <summary>
		/// Converts the specified location from inches to pixels.
		/// </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics"/> used for conversion.</param>
		/// <param name="point">The <see cref="T:System.Drawing.PointF"/> to convert.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="g"/> is <see langword="null"/>.
		/// </exception>
		public static PointF InchesToPixels(Graphics g, PointF point)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			g.PageUnit = GraphicsUnit.Inch;
			
			return new PointF(
				point.X * g.DpiX,
				point.Y * g.DpiY
				);
		}
	}
}

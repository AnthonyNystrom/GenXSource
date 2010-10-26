/* -----------------------------------------------
 * NuGenControlPaint.Polygons.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * GetPolygonGraphicsPath
		 */

		/// <summary>
		/// Gets the <see cref="T:GraphicsPath"/> for the polygon with the specified number of points.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="vertextCount"/> is less than 3.
		/// </exception>
		public static GraphicsPath GetPolygonGraphicsPath(int vertexCount, Rectangle polygonBounds)
		{
			if (vertexCount < 3)
			{
				throw new ArgumentOutOfRangeException(
					"vertexCount",
					vertexCount,
					Properties.Resources.ArgumentOutOfRange_PolygonPoints
					);
			}

			PointF[] points = new PointF[vertexCount];

			int currentPoint = 0;

			double offset = 360.0 / (double)vertexCount;

			for (double angle = -90; currentPoint < vertexCount; angle += offset)
			{
				points[currentPoint++] = new PointF(
					(float)(polygonBounds.Left + (double)polygonBounds.Width / 2.0 + GetEllipseX(DegToRad(angle), (double)polygonBounds.Width / 2.0)),
					(float)(polygonBounds.Top + (double)polygonBounds.Height / 2.0 + GetEllipseY(DegToRad(angle), (double)polygonBounds.Height / 2.0))
					);
			}

			GraphicsPath gp = new GraphicsPath();
			gp.AddPolygon(points);

			return gp;
		}
	}
}

/* -----------------------------------------------
 * NuGenControlPaint.Ellipses.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Drawing
{
	partial class NuGenControlPaint
	{
		/*
		 * GetEllipseX
		 */

		/// <summary>
		/// Gets the ellipse x-coordinate according to the specified angle and the major radius of the ellipse.
		/// </summary>
		/// <param name="t">Specifies the angle in radians to retrieve the x-coordinate for.</param>
		/// <param name="majorRadius">Specifies the major radius of the ellipse.</param>
		/// <returns></returns>
		public static double GetEllipseX(double t, double majorRadius)
		{
			return majorRadius * Math.Cos(t);
		}

		/*
		 * GetEllipseY
		 */

		/// <summary>
		/// Gets the ellipse y-coordinate according to the specified angle and the minor radius of the ellipse.
		/// </summary>
		/// <param name="t">Specifies the angle in radians to retrieve the y-coordinate for.</param>
		/// <param name="minorRadius">Specifies the minor radius of the ellipse.</param>
		/// <returns></returns>
		public static double GetEllipseY(double t, double minorRadius)
		{
			return minorRadius * Math.Sin(t);
		}
	}
}

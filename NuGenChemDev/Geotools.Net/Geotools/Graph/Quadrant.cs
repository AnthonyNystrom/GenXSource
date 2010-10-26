/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

#region Using
using System;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Computes the quadrant of a directed line segment.
	/// Quadrants are numbered according to the following scheme:
	///  <pre>
	///  1 | 0
	///  --+--&gt; &lt; pts.length
	///  2 | 3
	/// </pre>
	/// </summary>
	internal class Quadrant
	{
		#region Static Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public static int QuadrantLocation(double dx, double dy)
		{
			if (dx == 0.0 && dy == 0.0)
				throw new ArgumentException("Cannot compute the quadrant for point ( "+ dx + ", " + dy + " )" );
			if (dx >= 0) 
			{
				if (dy >= 0)
					return 0;
				else
					return 3;
			}
			else 
			{
				if (dy >= 0)
					return 1;
				else
					return 2;
			}
		} // public static int QuadrantLocation(double dx, double dy)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns></returns>
		public static int QuadrantLocation(Coordinate p0, Coordinate p1)
		{
			double dx = p1.X - p0.X;
			double dy = p1.Y - p0.Y;
			if (dx == 0.0 && dy == 0.0)
			{
				throw new ArgumentException("Cannot compute the quadrant for two identical points " + p0);
			}
			return QuadrantLocation(dx, dy);
		} // public static int QuadrantLocation(Coordinate p0, Coordinate p1)


		/// <summary>
		/// Determines if quadrants quad1 and quad2 are opposite.
		/// </summary>
		/// <param name="quad1"></param>
		/// <param name="quad2"></param>
		/// <returns>return true if the quadrants quad1 and quad2 are opposite.</returns>
		public static bool IsOpposite(int quad1, int quad2)
		{
			if (quad1 == quad2) 
			{
				return false;
			}
			int diff = (quad1 - quad2 + 4) % 4;
			// if quadrants are not adjacent, they are opposite
			if (diff == 2) 
			{
				return true;
			}
			return false;
		} // public static bool IsOpposite(int quad1, int quad2)

	
		/// <summary>
		/// Two adjacent quadrants have a unique halfplane in common. Halfplanes are indexed with their right
		/// hand quadrant.
		/// </summary>
		/// <param name="quad1"></param>
		/// <param name="quad2"></param>
		/// <returns></returns>
		public static int CommonHalfPlane(int quad1, int quad2)
		{
			// if quadrants are the same they do not determine a unique common halfplane.
			// Simply return one of the two possibilities
			if (quad1 == quad2)
			{	
				return quad1;
			}
			int diff = (quad1 - quad2 + 4) % 4;
			// if quadrants are not adjacent, they do not share a common halfplane
			if (diff == 2) return -1;
			//
			int min = (quad1 < quad2) ? quad1 : quad2;
			int max = (quad1 > quad2) ? quad1 : quad2;
			// for this one case, the righthand plane is NOT the minimum index;
			if (min == 0 && max == 3) 
			{
				return 3;
			}
			// in general, the halfplane index is the minimum of the two adjacent quadrants
			return min;
		} // public static int CommonHalfPlane(int quad1, int quad2)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="quad"></param>
		/// <param name="halfPlane"></param>
		/// <returns></returns>
		public static bool IsInHalfPlane(int quad, int halfPlane)
		{
			if (halfPlane == 3) 
			{
				return quad == 3 || quad == 0;
			}
			return quad == halfPlane || quad == halfPlane + 1;
		} // public static bool IsInHalfPlane(int quad, int halfPlane)

		/// <summary>
		/// Determines if quad is in the northern quadrant.
		/// </summary>
		/// <param name="quad">Quadrant to test.</param>
		/// <returns>Returns true if Quadrant is northern.</returns>
		public static bool IsNorthern(int quad)
		{
			return quad == 0 || quad == 1;
		} // public static bool IsNorthern(int quad)

		#endregion
	}
}

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

#region Using statements
using System;
using Geotools.Geometries;
#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Abstract class CGAlgorithms.
	/// </summary>
	public abstract class CGAlgorithms
	{
		/// <summary>
		/// Clockwise (-1).
		/// </summary>
		public const int CLOCKWISE = -1;
		/// <summary>
		/// Counterclockwise (1);
		/// </summary>
		public const int COUNTERCLOCKWISE = 1;
		/// <summary>
		/// Colinear (0).
		/// </summary>
		public const int COLLINEAR = 0;
	

		/// <summary>
		/// Tests whether a point lies inside a simple ring.  The ring may be oriented in either
		/// direction.  If the point lies on the ring boundary the result of this method is unspecified.
		/// </summary>
		/// <param name="p">Point to test.</param>
		/// <param name="ring">Simple polygon ring to test if point lies inside.</param>
		/// <returns>Returns true if the point lies in the interior of the ring.</returns>
		public abstract bool IsPointInRing(Coordinate p, Coordinates ring);

		/// <summary>
		/// Tests whether a point lies on a linestring.
		/// </summary>
		/// <param name="p">Point to test.</param>
		/// <param name="linestring">LineString to test.</param>
		/// <returns>Returns true if the point is a vertex of the line or lies in the interior of a line
		/// segment in the linestring.</returns>
		public abstract bool IsOnLine(Coordinate p, Coordinates linestring);

		/// <summary>
		/// Tests whether a ring is oriented counter-clockwise. The list of points is assumed to have the first and last points
		/// equal.  If the point list does not form a valid ring the result is undefined.
		/// </summary>
		/// <param name="ring">Ring to test.</param>
		/// <returns>Returns true if ring is oriented counter-clockwise.</returns>
		public abstract bool IsCCW(Coordinates ring);

		/// <summary>
		/// Computes the orientation of a point q to the directed line segment p1-p2.  The
		/// orientation of a point relative to a directed line segment indicates which way you turn
		/// to get to q after traveling from p1 to p2.
		/// </summary>
		/// <param name="p1">First coordinate in the directed line segment.</param>
		/// <param name="p2">Second coordinate in the directed line segment.</param>
		/// <param name="q">Coordinate to test for orientation.</param>
		/// <returns>Returns 1 if q is counter-clockwise from p1 to p2 and returns -1 if q is clockwise 
		/// from p1 to p2.  Returns 0 if q is collinear with p1 to p2.</returns>
		public abstract int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q);

		
		
		/// <summary>
		/// Computes the distance from a point p to a line segment AB
		/// </summary>
		/// <remarks>Note: NON-ROBUST!</remarks>
		/// <param name="p"></param>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		public static double DistancePointLine(Coordinate p, Coordinate A, Coordinate B)
		{
			// if start==end, then use pt distance
			if (  A.Equals(B) ) return p.Distance(A);

			// otherwise use comp.graphics.algorithms Frequently Asked Questions method
			/*(1)     	      AC dot AB
						   r = ---------
								 ||AB||^2
				r has the following meaning:
				r=0 P = A
				r=1 P = B
				r<0 P is on the backward extension of AB
				r>1 P is on the forward extension of AB
				0<r<1 P is interior to AB
			*/

			double r = ( (p.X - A.X) * (B.X - A.X) + (p.Y - A.Y) * (B.Y - A.Y) )
				/
				( (B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y) );

			if (r <= 0.0) return p.Distance(A);
			if (r >= 1.0) return p.Distance(B);


			/*(2)
					 (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
				s = -----------------------------
								L^2

				Then the distance from C to P = |s|*L.
			*/

			double s = ((A.Y - p.Y) *(B.X - A.X) - (A.X - p.X)*(B.Y - A.Y) )
				/
				((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y) );

			return
				Math.Abs(s) *
				Math.Sqrt(((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y)));
		}

		/// <summary>
		/// Computes the distance from a line segment AB to a line segment CD
		/// Note: NON-ROBUST!
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <param name="C"></param>
		/// <param name="D"></param>
		/// <returns></returns>
		public static double DistanceLineLine(Coordinate A, Coordinate B, Coordinate C, Coordinate D)
		{
			// check for zero-length segments
			if (  A.Equals(B) )	return DistancePointLine(A,C,D);
			if (  C.Equals(D) )	return DistancePointLine(D,A,B);

			// AB and CD are line segments
			/* from comp.graphics.algo

			Solving the above for r and s yields
						(Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
					   r = ----------------------------- (eqn 1)
						(Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

					(Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
				s = ----------------------------- (eqn 2)
					(Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
			Let P be the position vector of the intersection point, then
				P=A+r(B-A) or
				Px=Ax+r(Bx-Ax)
				Py=Ay+r(By-Ay)
			By examining the values of r & s, you can also determine some other
		limiting conditions:
				If 0<=r<=1 & 0<=s<=1, intersection exists
				r<0 or r>1 or s<0 or s>1 line segments do not intersect
				If the denominator in eqn 1 is zero, AB & CD are parallel
				If the numerator in eqn 1 is also zero, AB & CD are collinear.

			*/
			double r_top = (A.Y-C.Y)*(D.X-C.X) - (A.X-C.X)*(D.Y-C.Y) ;
			double r_bot = (B.X-A.X)*(D.Y-C.Y) - (B.Y-A.Y)*(D.X-C.X) ;

			double s_top = (A.Y-C.Y)*(B.X-A.X) - (A.X-C.X)*(B.Y-A.Y);
			double s_bot = (B.X-A.X)*(D.Y-C.Y) - (B.Y-A.Y)*(D.X-C.X);

			if  ( (r_bot==0) || (s_bot == 0) ) 
			{
				return
					Math.Min(DistancePointLine(A,C,D),
					Math.Min(DistancePointLine(B,C,D),
					Math.Min(DistancePointLine(C,A,B),
					DistancePointLine(D,A,B)    ) ) );

			}
			double s = s_top/s_bot;
			double r=  r_top/r_bot;

			if ((r < 0) || ( r > 1) || (s < 0) || (s > 1) )	
			{
				//no intersection
				return
					Math.Min(DistancePointLine(A,C,D),
					Math.Min(DistancePointLine(B,C,D),
					Math.Min(DistancePointLine(C,A,B),
					DistancePointLine(D,A,B)    ) ) );
			}
			return 0.0; //intersection exists
		}

		/// <summary>
		/// Returns the signed area for a ring.  The area is positive if
		/// the ring is oriented CW.
		/// </summary>
		/// <param name="ring"></param>
		/// <returns></returns>
		public static double SignedArea(Coordinates ring)
		{
			if (ring.Count < 3) return 0.0;
			double sum = 0.0;
			double ax = ring[0].X;
			double ay = ring[0].Y;
			for (int i = 1; i < ring.Count - 1; i++) 
			{
				double bx = ring[i].X;
				double by = ring[i].Y;
				double cx = ring[i + 1].X;
				double cy = ring[i + 1].Y;
				sum +=  ax * by - ay * bx +
					ay * cx - ax * cy +
					bx * cy - cx * by;
			}
			return -sum  / 2.0;
		}

		/// <summary>
		/// Returns the length of a list of line segments.
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static double Length(Coordinates pts)
		{
			if (pts.Count < 1) return 0.0;
			double sum = 0.0;
			for (int i = 1; i < pts.Count; i++) 
			{
				sum += pts[i].Distance(pts[i - 1]);
			}
			return sum;
		}
	}
}

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

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for NonRobustLineIntersector.
	/// </summary>
	internal class NonRobustLineIntersector : LineIntersector
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the NonRobustLineIntersector class.
		/// </summary>
		public NonRobustLineIntersector()
		{
		}
		#endregion

		#region Static Methods
		/// <summary>
		/// Returns true if both numbers are positive or if both number are negative. Returns false if both numbers are zero.
		/// </summary>
		/// <param name="a">First number in check.</param>
		/// <param name="b">Second number in check.</param>
		/// <returns>Returns true if both numbers are positive or if both number are negative.</returns>
		public static bool IsSameSignAndNonZero(double a, double b) 
		{
			if (a == 0 || b == 0) 
			{
				return false;
			}
			return (a < 0 && b < 0) || (a > 0 && b > 0);
		}
		#endregion

		#region Methods

		/// <summary>
		/// Computes the Intersection of point p with line p1-p2.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		public override void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2)
		{
			double a1;
			double b1;
			double c1;
			//
			//  Coefficients of line eqns.
			//
			double r;
			//
			//  'Sign' values
			//
			_isProper = false;

			//
			//  Compute a1, b1, c1, where line joining points 1 and 2
			//  is "a1 x  +  b1 y  +  c1  =  0".
			//
			a1 = p2.Y - p1.Y;
			b1 = p1.X - p2.X;
			c1 = p2.X * p1.Y - p1.X * p2.Y;

			//
			//  Compute r3 and r4.
			//
			r = a1 * p.X + b1 * p.Y + c1;

			// if r != 0 the point does not lie on the line
			if (r != 0) 
			{
				_result = DONT_INTERSECT;
				return;
			}

			// Point lies on line - check to see whether it lies in line segment.

			double dist = RParameter(p1, p2, p);
			if (dist < 0.0 || dist > 1.0) 
			{
				_result = DONT_INTERSECT;
				return;
			}

			_isProper = true;
			if (p.Equals(p1) || p.Equals(p2)) 
			{
				_isProper = false;
			}
			_result = DO_INTERSECT;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		/// <returns></returns>
		protected override int ComputeIntersect( Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
		{
			double a1;
			double b1;
			double c1;
			//
			//  Coefficients of line eqns.
			//
			double a2;
			//
			//  Coefficients of line eqns.
			//
			double b2;
			//
			//  Coefficients of line eqns.
			//
			double c2;
			double r1;
			double r2;
			double r3;
			double r4;
			//
			//  'Sign' values
			//
			//double denom, offset, num;     // Intermediate values

			_isProper = false;

			//
			//  Compute a1, b1, c1, where line joining points 1 and 2
			//  is "a1 x  +  b1 y  +  c1  =  0".
			//
			a1 = p2.Y - p1.Y;
			b1 = p1.X - p2.X;
			c1 = p2.X * p1.Y - p1.X * p2.Y;

			//
			//  Compute r3 and r4.
			//
			r3 = a1 * p3.X + b1 * p3.Y + c1;
			r4 = a1 * p4.X + b1 * p4.Y + c1;

			//
			//  Check signs of r3 and r4.  If both point 3 and point 4 lie on
			//  same side of line 1, the line segments do not intersect.
			//
			if (r3 != 0 &&
				r4 != 0 &&
				IsSameSignAndNonZero(r3, r4)) 
			{
				return DONT_INTERSECT;
			}

			//
			//  Compute a2, b2, c2
			//
			a2 = p4.Y - p3.Y;
			b2 = p3.X - p4.X;
			c2 = p4.X * p3.Y - p3.X * p4.Y;

			//
			//  Compute r1 and r2
			//
			r1 = a2 * p1.X + b2 * p1.Y + c2;
			r2 = a2 * p2.X + b2 * p2.Y + c2;

			//
			//  Check signs of r1 and r2.  If both point 1 and point 2 lie
			//  on same side of second line segment, the line segments do
			//  not intersect.
			//
			if (r1 != 0 &&
				r2 != 0 &&
				IsSameSignAndNonZero(r1, r2)) 
			{
				return DONT_INTERSECT;
			}

			//
			//  Line segments intersect: compute intersection point.
			//
			double denom = a1 * b2 - a2 * b1;
			if (denom == 0) 
			{
				return ComputeCollinearIntersection(p1, p2, p3, p4);
			}
			double numX = b1 * c2 - b2 * c1;
			_pA.X = numX / denom;
			//
			//  TESTING ONLY
			//  double valX = (( num < 0 ? num - offset : num + offset ) / denom);
			//  double valXInt = (int) (( num < 0 ? num - offset : num + offset ) / denom);
			//  if (valXInt != pa.x)     // TESTING ONLY
			//  System.out.println(val + " - int: " + valInt + ", floor: " + pa.x);
			//
			double numY = a2 * c1 - a1 * c2;
			_pA.Y = numY / denom;

			// check if this is a proper intersection BEFORE truncating values,
			// to avoid spurious equality comparisons with endpoints
			_isProper = true;
			if (_pA.Equals(p1) || _pA.Equals(p2) || _pA.Equals(p3) || _pA.Equals(p4)) 
			{
				_isProper = false;
			}

			// truncate computed point to precision grid
			// TESTING - don't force coord to be precise
			if ( MakePrecise ) 
			{
				_pA.MakePrecise();
			}
			return DO_INTERSECT;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		/// <returns>
		///		Returns:
		///		DONT_INTERSECT: the two segments do not intersect
		///		COLLINEAR: the segments intersect, in the line segment pa-pb.  pa-pb is in 
		///			the same direction as p1-p2
		///		DO_INTERSECT: the inputLines intersect in a single point only, pa
		/// </returns>
		/// <remarks>
		///		p1-p2  and p3-p4 are assumed to be collinear (although not necessarily intersecting).
		/// </remarks>
		private int ComputeCollinearIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
		{
			double r1;
			double r2;
			double r3;
			double r4;
			Coordinate q3;
			Coordinate q4;
			double t3;
			double t4;
			r1 = 0;
			r2 = 1;
			r3 = RParameter(p1, p2, p3);
			r4 = RParameter(p1, p2, p4);
			// make sure p3-p4 is in same direction as p1-p2
			if (r3 < r4) 
			{
				q3 = p3;
				t3 = r3;
				q4 = p4;
				t4 = r4;
			}
			else 
			{
				q3 = p4;
				t3 = r4;
				q4 = p3;
				t4 = r3;
			}
			// check for no intersection
			if (t3 > r2 || t4 < r1) 
			{
				return DONT_INTERSECT;
			}

			// check for single point intersection
			if (q4 == p1) 
			{
				_pA.CopyCoordinate(p1);
				return DO_INTERSECT;
			}
			if (q3 == p2) 
			{
				_pA.CopyCoordinate(p2);
				return DO_INTERSECT;
			}

			// intersection MUST be a segment - compute endpoints
			_pA.CopyCoordinate(p1);
			if (t3 > r1) 
			{
				_pA.CopyCoordinate(q3);
			}
			_pB.CopyCoordinate(p2);
			if (t4 < r2) 
			{
				_pB.CopyCoordinate(q4);
			}
			return COLLINEAR;
		} // private int ComputeCollinearIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)

		/// <summary>
		///		RParameter computes the parameter for the point p
		///		in the parameterized equation of the line from p1 to p2.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p"></param>
		/// <returns>The 'distance' of p along p1-p2</returns>
		private double RParameter(Coordinate p1, Coordinate p2, Coordinate p) 
		{
			double r;
			// compute maximum delta, for numerical stability
			// also handle case of p1-p2 being vertical or horizontal
			double dx = Math.Abs(p2.X - p1.X);
			double dy = Math.Abs(p2.Y - p1.Y);
			if (dx > dy) 
			{
				r = (p.X - p1.X) / (p2.X - p1.X);
			}
			else 
			{
				r = (p.Y - p1.Y) / (p2.Y - p1.Y);
			}
			return r;
		} // private double RParameter(Coordinate p1, Coordinate p2, Coordinate p)

		#endregion

	}
}

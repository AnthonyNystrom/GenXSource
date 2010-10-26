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

#region Using Statements

using System;
using Geotools.Geometries;

#endregion


namespace Geotools.Algorithms
{
	/// <summary>
	/// Implemention of the CGAlgorithms using non-robust algorithms.
	/// </summary>
	/// <remarks>
	/// Geometric algorithms involve a combination of combinatorial and numerical computation.
	/// As with all numerical computation using finite-precision numbers, the algorithms chosen are
	/// susceptible to problems of robustness. A robustness problem occurs when a numerical
	/// calculation produces an inexact answer due to round-off errors. Robustness problems are
	/// especially serious in geometric computation, since the numerical errors can propagate into
	/// the combinatorial computations and result in complete failure of the algorithm.
	/// </remarks>
	public class NonRobustCGAlgorithms : CGAlgorithms
	{

		LineIntersector _lineIntersector = new RobustLineIntersector();		
		
		/// <summary>
		///  Initializes a new instance of the NonRobustCGAlgorithms class.
		/// </summary>
		public NonRobustCGAlgorithms()
		{
		}

		/// <summary>
		/// Tests whether a point lies inside a simple polygon (ring).  The ring may be oriented in either
		/// direction.  If the point lies on the ring boundary the result of this method is unspecified.
		/// </summary>
		/// <param name="p">Point to test.</param>
		/// <param name="ring">Simple polygon ring to test if point lies inside.</param>
		/// <returns>Returns true if the point lies in the interior of the ring.</returns>
		public override bool IsPointInRing(Coordinate p, Coordinates ring)
		{
			int		i, i1;		// point index; i1 = i-1 mod n
			double	xInt;		// x intersection of e with ray
			int		crossings = 0;	// number of edge/ray crossings
			double	x1,y1,x2,y2;
			int     nPts = ring.Count;

			// For each line edge l = (i-1, i), see if it crosses ray from test point in positive x direction.
			for (i = 1; i < nPts; i++ ) 
			{
				i1 = i - 1;
				Coordinate p1 = ring[i];
				Coordinate p2 = ring[i1];
				x1 = p1.X - p.X;
				y1 = p1.Y - p.Y;
				x2 = p2.X - p.X;
				y2 = p2.Y - p.Y;

				if( ( ( y1 > 0 ) && ( y2 <= 0 ) ) ||
					( ( y2 > 0 ) && ( y1 <= 0 ) ) ) 
				{
					// e straddles x axis, so compute intersection.
					xInt = ( x1 * y2 - x2 * y1 ) / (y2 - y1);
					//xsave = xInt;
					// crosses ray if strictly positive intersection.
					if ( 0.0 < xInt )
					{
						crossings++;
					}
				}
			}
			// p is inside if an odd number of crossings.
			if( (crossings % 2) == 1 )
			{
				return	true;
			}
			else
			{
				return	false;
			}

		} // public override bool IsPointInPolygon(Coordinate p, Coordinates ring)

		/// <summary>
		/// Tests whether a point lies on a linestring.
		/// </summary>
		/// <param name="p">Point to test.</param>
		/// <param name="pt">LineString to test.</param>
		/// <returns>Returns true if the point is a vertex of the line or lies in the interior of a line
		/// segment in the linestring.</returns>
		public override bool IsOnLine(Coordinate p, Coordinates pt)
		{
			for (int i = 1; i < pt.Count; i++) 
			{
				Coordinate p0 = pt[i - 1];
				Coordinate p1 = pt[i];
				_lineIntersector.ComputeIntersection(p, p0, p1);
				if ( _lineIntersector.HasIntersection() )
				{
					return true;
				}
			}
			return false;
		} // public override bool IsOnLine(Coordinate p, Coordinates pt)

		/// <summary>
		/// Tests whether a ring is oriented counter-clockwise.
		/// </summary>
		/// <param name="ring">Ring to test.</param>
		/// <returns>Returns true if ring is oriented counter-clockwise.</returns>
		public override bool IsCCW(Coordinates ring)
		{
			Coordinate hip, p, prev, next;
			int hii, i;
			int nPts = ring.Count;

			// check that this is a valid ring - if not, simply return a dummy value
			if ( nPts < 4 ) return false;

			// algorithm to check if a Ring is stored in CCW order
			// find highest point
			hip = ring[0];
			hii = 0;
			for (i = 1; i < nPts; i++)	
			{
				p = ring[i];
				if (p.Y > hip.Y) 
				{
					hip = p;
					hii = i;
				}
			}
			// find points on either side of highest
			int iPrev = hii - 1;
			if (iPrev < 0) iPrev = nPts - 2;
			int iNext = hii + 1;
			if (iNext >= nPts) iNext = 1;
			prev = ring[iPrev];
			next = ring[iNext];
			// translate so that hip is at the origin.
			// This will not affect the area calculation, and will avoid
			// finite-accuracy errors (i.e very small vectors with very large coordinates)
			// This also simplifies the discriminant calculation.
			double prev2x = prev.X - hip.X;
			double prev2y = prev.Y - hip.Y;
			double next2x = next.X - hip.X;
			double next2y = next.Y - hip.Y;
			// compute cross-product of vectors hip->next and hip->prev
			// (e.g. area of parallelogram they enclose)
			double disc = next2x * prev2y - next2y * prev2x;
			// If disc is exactly 0, lines are collinear.  There are two possible cases:
			//	(1) the lines lie along the x axis in opposite directions
			//	(2) the line lie on top of one another
			//  (2) should never happen, so we're going to ignore it!
			//	(Might want to assert this)
			//  (1) is handled by checking if next is left of prev ==> CCW

			if (disc == 0.0) 
			{
				// poly is CCW if prev x is right of next x
				return (prev.X > next.X);
			}
			else 
			{
				// if area is positive, points are ordered CCW
				return (disc > 0.0);
			}

		} // public override bool IsCCW(Coordinates ring)

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
		public override int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q) 
		{
			double dx1 = p2.X - p1.X;
			double dy1 = p2.Y - p1.Y;
			double dx2 = q.X - p2.X;
			double dy2 = q.Y - p2.Y;
			double det = dx1*dy2 - dx2*dy1;
			if (det > 0.0) return 1;
			if (det < 0.0) return -1;
			return 0;
		} // public override int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q)

	} // public class NonRobustCGAlgorithms : CGAlgorithms
}

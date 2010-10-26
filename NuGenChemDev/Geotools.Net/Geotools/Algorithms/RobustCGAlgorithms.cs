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
using System.Collections;
using Geotools.Geometries;
#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Implemention of the CGAlgorithms using robust algorithms.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Geometric algorithms involve a combination of combinatorial and numerical computation.
	/// As with all numerical computation using finite-precision numbers, the algorithms chosen are
	/// susceptible to problems of robustness. A robustness problem occurs when a numerical
	/// calculation produces an inexact answer due to round-off errors. Robustness problems are
	/// especially serious in geometric computation, since the numerical errors can propagate into
	/// the combinatorial computations and result in complete failure of the algorithm.
	/// </para>
	/// </remarks>
	public class RobustCGAlgorithms : CGAlgorithms
	{
		private RobustLineIntersector _lineIntersector = new RobustLineIntersector();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RobustCGAlgorithms.
		/// </summary>
		public RobustCGAlgorithms()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		public static int OrientationIndex(Coordinate p1, Coordinate p2, Coordinate q) 
		{
			// travelling along p1->p2, turn counter clockwise to get to q return 1,
			// travelling along p1->p2, turn clockwise to get to q return -1,
			// p1, p2 and q are colinear return 0.
			double dx1 = p2.X - p1.X;
			double dy1 = p2.Y - p1.Y;
			double dx2 = q.X - p2.X;
			double dy2 = q.Y - p2.Y;
			return RobustDeterminant.SignOfDet2x2(dx1, dy1, dx2, dy2);
		} // public static int OrientationIndex(Coordinate p1, Coordinate p2, Coordinate q)


		/// <summary>
		/// Tests whether a ring is oriented counter-clockwise.
		/// </summary>
		/// <param name="ring">Ring to test.</param>
		/// <returns>Return true if ring is oriented counter-clockwise.</returns>
		public override bool IsCCW(Coordinates ring) 
		{
			Coordinate hip;
			Coordinate p;
			Coordinate prev;
			Coordinate next;
			int hii;
			int i;
			int nPts = ring.Count;

			// check that this is a valid ring - if not, simply return a dummy value.
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
			if (iPrev < 0) 
			{
				iPrev = nPts - 2;
			}
			int iNext = hii + 1;
			if (iNext >= nPts) 
			{
				iNext = 1;
			}
			prev = ring[iPrev];
			next = ring[iNext];
			int disc = ComputeOrientation(prev, hip, next);
			//
			//  If disc is exactly 0, lines are collinear.  There are two possible cases:
			//  (1) the lines lie along the x axis in opposite directions
			//  (2) the line lie on top of one another
			//  (2) should never happen, so we're going to ignore it!
			//  (Might want to assert this)
			//  (1) is handled by checking if next is left of prev ==> CCW
			//
			if (disc == 0) 
			{
				// poly is CCW if prev x is right of next x
				return (prev.X > next.X);
			}
			else 
			{
				// if area is positive, points are ordered CCW
				return (disc > 0);
			}

		} // public override bool IsCCW(Coordinates ring)

		/// <summary>
		///		This algorithm does not attempt to first check the point against the envelope
		///		of the ring.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="ring">Assumed to have first point identical to last point</param>
		/// <returns></returns>
		public override bool IsPointInRing(Coordinate p, Coordinates ring) 
		{
			int i;
			int i1;       // point index; i1 = i-1
			double xInt;  // x intersection of segment with ray
			int crossings = 0;  // number of segment/ray crossings
			double x1;    // translated coordinates
			double y1;
			double x2;
			double y2;
			int nPts = ring.Count;

			//
			//  For each segment l = (i-1, i), see if it crosses ray from test point in positive x direction.
			//
			for (i = 1; i < nPts; i++) 
			{
				i1 = i - 1;
				Coordinate p1 = ring[i];
				Coordinate p2 = ring[i1];
				x1 = p1.X - p.X;
				y1 = p1.Y - p.Y;
				x2 = p2.X - p.X;
				y2 = p2.Y - p.Y;

				if (((y1 > 0) && (y2 <= 0)) ||
					((y2 > 0) && (y1 <= 0))) 
				{
					//
					//  segment straddles x axis, so compute intersection.
					//
					xInt = RobustDeterminant.SignOfDet2x2(x1, y1, x2, y2) / (y2 - y1);
					//xsave = xInt;
					//
					//  crosses ray if strictly positive intersection.
					//
					if ( 0.0 < xInt ) 
					{
						crossings++;
					}
				}
			}
			//
			//  p is inside if number of crossings is odd.
			//
			if ((crossings % 2) == 1) 
			{
				return true;
			}
			else 
			{
				return false;
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
		/// Computes the orientation of a point q to the directed line segment p1-p2.  The
		/// orientation of a point relative to a directed line segment indicates which way you turn
		/// to get to q after traveling from p1 to p2.
		/// </summary>
		/// <param name="p1">Point 1 of line segment.</param>
		/// <param name="p2">Point 2 of line segment.</param>
		/// <param name="q">Point from which to compute the orientation.</param>
		/// <returns>Returns 1 if q is counter-clockwise from p1 to p2 and returns -1 if q is clockwise 
		/// from p1 to p2.  Returns 0 if q is collinear with p1 to p2.</returns>
		public override int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q) 
		{
			return OrientationIndex(p1, p2, q);
		} // public override int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q) 

		/// <summary>
		/// Tests to see if point p is in the envelope of ring.
		/// </summary>
		/// <param name="p">Point to test.</param>
		/// <param name="ring">Geometry from which to create envelope.</param>
		/// <returns>Returns true if point is in envelope of ring.</returns>
		private bool IsInEnvelope(Coordinate p, Coordinates ring) 
		{
			Envelope envelope = new Envelope();
			for (int i = 0; i < ring.Count; i++) 
			{
				envelope.ExpandToInclude( ring[i] );
			}
			return envelope.Contains( p );		
		} // private bool IsInEnvelope(Coordinate p, Coordinates ring)

		#endregion

	}
}

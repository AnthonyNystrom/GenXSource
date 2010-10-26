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
	/// Summary description for RobustLineIntersector.
	/// </summary>
	internal class RobustLineIntersector : LineIntersector
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RobustLineIntersector class.
		/// </summary>
		public RobustLineIntersector()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Computes the intersion of point p and line p1-p2.
		/// </summary>
		/// <param name="p">Coordinate of point p.</param>
		/// <param name="p1">Coordinate of line p1-p2.</param>
		/// <param name="p2">Coordinate of line p1-p2.</param>
		public override void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2) 
		{
			_isProper = false;
			if ( (RobustCGAlgorithms.OrientationIndex(p1, p2, p) == 0)
				&& (RobustCGAlgorithms.OrientationIndex(p2, p1, p) == 0) ) 
			{
				if ( Between(p1, p2, p) ) 
				{
					_isProper = true;
					if ( p.Equals(p1) || p.Equals(p2) ) 
					{
						_isProper = false;
					}
					_result = DO_INTERSECT;
					return;
				}
				else 
				{
					_result = DONT_INTERSECT;
					return;
				}
			}
			else 
			{
				_result = DONT_INTERSECT;
			}
		} // public override void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2)

		/// <summary>
		/// Computes the intersection of line p1-p2 and line q1-q2.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="q1"></param>
		/// <param name="q2"></param>
		/// <returns></returns>
		protected override int ComputeIntersect(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
		{
			_isProper = false;
			// for each endpoint, compute which side of the other segment it lies
			int Pq1 = RobustCGAlgorithms.OrientationIndex(p1, p2, q1);
			int Pq2 = RobustCGAlgorithms.OrientationIndex(p1, p2, q2);
			int Qp1 = RobustCGAlgorithms.OrientationIndex(q1, q2, p1);
			int Qp2 = RobustCGAlgorithms.OrientationIndex(q1, q2, p2);
			// if both endpoints lie on the same side of the other segment, the segments do not intersect
			if (Pq1 > 0 && Pq2 > 0) 
			{
				return DONT_INTERSECT;
			}
			if (Qp1 > 0 && Qp2 > 0) 
			{
				return DONT_INTERSECT;
			}
			if (Pq1 < 0 && Pq2 < 0) 
			{
				return DONT_INTERSECT;
			}
			if (Qp1 < 0 && Qp2 < 0) 
			{
				return DONT_INTERSECT;
			}
			bool isCollinear = ( (Pq1 == 0) && (Pq2 == 0) && (Qp1 == 0) && (Qp2 == 0) );
			if ( isCollinear ) 
			{
				return ComputeCollinearIntersection(p1, p2, q1, q2);
			}
			//
			//  Check if the intersection is an endpoint. If it is, copy the endpoint as
			//  the intersection point. Copying the point rather than computing it
			//  ensures the point has the exact value, which is important for
			//  robustness. It is sufficient to simply check for an endpoint which is on
			//  the other line, since at this point we know that the inputLines must
			//  intersect.
			//
			if ( Pq1 == 0 || Pq2 == 0 || Qp1 == 0 || Qp2 == 0 ) 
			{
				_isProper = false;
				if (Pq1 == 0) 
				{
					_intPt[0] = new Coordinate(q1);
				}
				if (Pq2 == 0) 
				{
					_intPt[0] = new Coordinate(q2);
				}
				if (Qp1 == 0) 
				{
					_intPt[0] = new Coordinate(p1);
				}
				if (Qp2 == 0) 
				{
					_intPt[0] = new Coordinate(p2);
				}
			}
			else 
			{
				_isProper = true;
				_intPt[0] = Intersection(p1, p2, q1, q2);
			}
			return DO_INTERSECT;
		} // protected override int ComputeIntersect(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="q"></param>
		/// <returns></returns>
		private bool Between(Coordinate p1, Coordinate p2, Coordinate q) 
		{
			if (((q.X >= Math.Min(p1.X, p2.X)) && (q.X <= Math.Max(p1.X, p2.X))) &&
				((q.Y >= Math.Min(p1.Y, p2.Y)) && (q.Y <= Math.Max(p1.Y, p2.Y)))) 
			{
				return true;
			}
			else 
			{
				return false;
			}
		}

		private int ComputeCollinearIntersection(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
		{
			bool p1q1p2 = Between(p1, p2, q1);
			bool p1q2p2 = Between(p1, p2, q2);
			bool q1p1q2 = Between(q1, q2, p1);
			bool q1p2q2 = Between(q1, q2, p2);

			if (p1q1p2 && p1q2p2) 
			{
				_intPt[0] = q1;
				_intPt[1] = q2;
				return COLLINEAR;
			}
			if (q1p1q2 && q1p2q2) 
			{
				_intPt[0] = p1;
				_intPt[1] = p2;
				return COLLINEAR;
			}
			if (p1q1p2 && q1p1q2) 
			{
				_intPt[0] = q1;
				_intPt[1] = p1;
				return q1.Equals(p1) && !p1q2p2 && !q1p2q2 ? DO_INTERSECT : COLLINEAR;
			}
			if (p1q1p2 && q1p2q2) 
			{
				_intPt[0] = q1;
				_intPt[1] = p2;
				return q1.Equals(p2) && !p1q2p2 && !q1p1q2 ? DO_INTERSECT : COLLINEAR;
			}
			if (p1q2p2 && q1p1q2) 
			{
				_intPt[0] = q2;
				_intPt[1] = p1;
				return q2.Equals(p1) && !p1q1p2 && !q1p2q2 ? DO_INTERSECT : COLLINEAR;
			}
			if (p1q2p2 && q1p2q2) 
			{
				_intPt[0] = q2;
				_intPt[1] = p2;
				return q2.Equals(p2) && !p1q1p2 && !q1p1q2 ? DO_INTERSECT : COLLINEAR;
			}
			return DONT_INTERSECT;
		}

		/// <summary>
		/// Calculates the intersection point of two lines.
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="q1"></param>
		/// <param name="q2"></param>
		/// <returns></returns>
		private Coordinate Intersection(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2) 
		{
			Coordinate n1 = new Coordinate(p1);
			Coordinate n2 = new Coordinate(p2);
			Coordinate n3 = new Coordinate(q1);
			Coordinate n4 = new Coordinate(q2);
			Coordinate normPt = new Coordinate();
			Normalize(n1, n2, n3, n4, normPt);

			Coordinate intPt = null;
			try 
			{
				intPt = HCoordinate.Intersection(n1, n2, n3, n4);
			}
			catch (NotRepresentableException e) 
			{
				throw new NotRepresentableException("Coordinate for intersection is not calculable", e);
			}

			intPt.X += normPt.X;
			intPt.Y += normPt.Y;

			if (_makePrecise) 
			{
				intPt.MakePrecise();
			}
			return intPt;
		}

		private void Normalize(
			Coordinate n1,
			Coordinate n2,
			Coordinate n3,
			Coordinate n4,
			Coordinate normPt)
		{
			normPt.X = SmallestInAbsValue(n1.X, n2.X, n3.X, n4.X);
			normPt.Y = SmallestInAbsValue(n1.Y, n2.Y, n3.Y, n4.Y);
			n1.X -= normPt.X;    n1.Y -= normPt.Y;
			n2.X -= normPt.X;    n2.Y -= normPt.Y;
			n3.X -= normPt.X;    n3.Y -= normPt.Y;
			n4.X -= normPt.X;    n4.Y -= normPt.Y;
		}

		private double SmallestInAbsValue(double x1, double x2, double x3, double x4)
		{
			double x = x1;
			double xabs = Math.Abs(x);
			if (Math.Abs(x2) < xabs) 
			{
				x = x2;
				xabs = Math.Abs(x2);
			}
			if (Math.Abs(x3) < xabs) 
			{
				x = x3;
				xabs = Math.Abs(x3);
			}
			if (Math.Abs(x4) < xabs) 
			{
				x = x4;
			}
			return x;
		}

		#endregion

	}
}

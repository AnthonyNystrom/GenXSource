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
	///		A LineIntersector is an algorithm that can both test whether
	///		two line segments intersect and compute the intersection point
	///		if they do.
	/// </summary>
	/// <remarks>
	/// 	The intersection point may be computed in a precise or non-precise manner.
	/// 	Computing it precisely involves rounding it to an integer.  (This assumes
	/// 	that the input coordinates have been made precise by scaling them to
	/// 	an integer grid.)
	/// </remarks>
	internal abstract class LineIntersector
	{
		
		#region Member Variables

		/// <summary>
		/// Lines do not intersect;
		/// </summary>
		public const int DONT_INTERSECT = 0;
		/// <summary>
		/// Lines intersect.
		/// </summary>
		public const int DO_INTERSECT = 1;
		/// <summary>
		/// Collinear lines.
		/// </summary>
		public const int COLLINEAR = 2;

		/// <summary>
		/// 
		/// </summary>
		protected bool _makePrecise = false;
		/// <summary>
		/// 
		/// </summary>
		protected bool _isProper = false;
		/// <summary>
		/// 
		/// </summary>
		protected int _result;
		/// <summary>
		/// 
		/// </summary>
		protected Coordinate _pA;
		/// <summary>
		/// 
		/// </summary>
		protected Coordinate _pB;
		/// <summary>
		/// 
		/// </summary>
		protected Coordinate[,] _inputLines = new Coordinate[2,2];
		/// <summary>
		/// 
		/// </summary>
		protected Coordinate[] _intPt = new Coordinate[2];
		/// <summary>
		/// 
		/// </summary>
		protected int[,] _intLineIndex = null;
		#endregion
		#region Constructor

		/// <summary>
		///  Initializes a new instance of the LineIntersector.
		/// </summary>
		public LineIntersector() 
		{
			_intPt[0] = new Coordinate();
			_intPt[1] = new Coordinate();
			// alias the intersection points for ease of reference
			_pA = _intPt[0];
			_pB = _intPt[1];
			_result = 0;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Result property.
		/// </summary>
		protected int Result
		{
			get
			{
				return _result;
			}
		}

		/// <summary>
		/// Intersection Point.
		/// </summary>
		protected Coordinate[] IntersectionPoint
		{
			get
			{
				return _intPt;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected Coordinate IntersectionPointA
		{
			get
			{
				return _pA;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected Coordinate IntersectionPointB
		{
			get
			{
				return _pB;
			}
		}
		/// <summary>
		/// Option used to determine if coordinates will be convert to Fixed.
		/// </summary>
		public bool MakePrecise
		{
			get
			{
				return _makePrecise;
			}
			set
			{
				_makePrecise = value;
			}
		} // public bool MakePrecise

		#endregion

		#region Methods

		/// <summary>
		/// 	Computes the "edge distance" of an intersection point p in an edge.
		/// 	The edge distance is a metric of the point along the edge.
		/// 	The metric used is a robust and easy to compute metric function.
		/// 	It is not equivalent to the usual Euclidean metric.
		/// 	It relies on the fact that either the x or the y ordinates of the
		/// 	points in the edge are unique, depending on whether the edge is longer in
		/// 	the horizontal or vertical direction.
		/// </summary>
		/// <remarks>
		/// 	NOTE: This function may produce incorrect distances
		/// 	 for inputs where p is not precisely on p1-p2
		/// 	(E.g. p = (139,9) p1 = (139,10), p2 = (280,1) produces distance 0.0, which is incorrect.
		/// 	
		/// 	My hypothesis is that the function is safe to use for points which are the
		/// 	result of rounding points which lie on the line,
		///		but not safe to use for truncated points.
		/// </remarks>
		/// <param name="p">Intersecting point from which to compute edge distance.</param>
		/// <param name="p0">Edge point 0.</param>
		/// <param name="p1">Edge point 1.</param>
		/// <returns></returns>
		public static double ComputeEdgeDistance( Coordinate p, Coordinate p0, Coordinate p1 )
		{
			double dx = Math.Abs(p1.X - p0.X);
			double dy = Math.Abs(p1.Y - p0.Y);

			double dist = -1.0;   // sentinel value
			if ( p.Equals( p0 ) ) 
			{
				dist = 0.0;
			}
			else if ( p.Equals( p1 ) ) 
			{
				if ( dx > dy )
				{
					dist = dx;
				}
				else
				{
					dist = dy;
				}
			}
			else 
			{
				if ( dx > dy )
				{
					dist = Math.Abs(p.X - p0.X);
				}
				else
				{
					dist = Math.Abs(p.Y - p0.Y);
				}
			}

			// this assertion was commentted out in the original java code.
			//Assert.isTrue(! (dist == 0.0 && ! p.equals(coordinate1)), "Invalid distance calculation");
			return dist;			

		} // public static double ComputeEdgeDistance( Coordinate coordinate, Coordinate coordinate0, Coordinate coordinate1 )

		/// <summary>
		/// Computes the "edge distance" of an intersection point p in an edge.  This function is non-robust,
		/// since it may compute the square of large numbers.
		/// </summary>
		/// <param name="p">Intersecting point from which to compute edge distance.</param>
		/// <param name="p1">Edge point 0.</param>
		/// <param name="p2">Edge point 1.</param>
		/// <returns>Returns the distance of intersecting point p on edge p1-p2.</returns>
		public static double NonRobustComputeEdgeDistance(Coordinate p, Coordinate p1, Coordinate p2)
		{
			double dx = p.X - p1.X;
			double dy = p.Y - p1.Y;
			double dist = Math.Sqrt(dx * dx + dy * dy);
			if ( (dist == 0.0 && !p.Equals(p1) ) )
			{
				throw new NotRepresentableException("NonRobustComputeEdgeDistance failed.");
			}
			return dist;

		} // public static double NonRobustComputeEdgeDistance(Coordinate coordinate, Coordinate coordinate1, Coordinate coordinate2)

		/// <summary>
		///	Abstract method to compute the intersection of a p and the line p1-p2
		/// </summary>
		/// <param name="p">Point with which to compute intersection.</param>
		/// <param name="p1">Point 1 of line to compute intersection with p.</param>
		/// <param name="p2">Point 2 of line to compute intersection with p.</param>
		public abstract void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2);
	
		/// <summary>
		/// Returns true if result is collinear.
		/// </summary>
		/// <returns>Returns true if result is collinear.</returns>
		protected bool IsCollinear() 
		{
			return _result == COLLINEAR;
		} // protected bool IsCollinear()

		/// <summary>
		///		Computes the intersection of the lines p1-p2 and p3-p4
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		public void ComputeIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
		{
			_inputLines[0,0] = p1;
			_inputLines[0,1] = p2;
			_inputLines[1,0] = p3;
			_inputLines[1,1] = p4;
			_result = ComputeIntersect(p1, p2, p3, p4);
			//numIntersects++;
		} // public virtual void ComputeIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)

		/// <summary>
		/// Computes the intersection of the lines p1-p2 and q1-q2.
		/// </summary>
		/// <param name="p1">Point 1 for line p1-p2.</param>
		/// <param name="p2">Point 2 for line p1-p2.</param>
		/// <param name="q1">Point 1 for line q1-q2.</param>
		/// <param name="q2">Point 2 for line q1-q2.</param>
		/// <returns></returns>
		protected abstract int ComputeIntersect(Coordinate p1, Coordinate p2,Coordinate q1, Coordinate q2);

		/// <summary>
		/// Returns the string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			throw new NotImplementedException();
/*
			String str = inputLines[0][0] + " "
				+ inputLines[0][1] + " "
				+ inputLines[1][0] + " "
				+ inputLines[1][1] + " : ";
			if (isEndPoint()) 
			{
				str += " endpoint";
			}
			if (isProper) 
			{
				str += " proper";
			}
			if (isCollinear()) 
			{
				str += " collinear";
			}
			return str;
*/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected bool IsEndPoint() 
		{
			return HasIntersection() && !_isProper;
		} // protected bool IsEndPoint()

		/// <summary>
		///	 Tests whether the input geometries intersect. 
		/// </summary>
		/// <returns>True if the input geometries intersect.</returns>
		public bool HasIntersection() 
		{
			return _result != DONT_INTERSECT;		
		} // public bool HasIntersection()

		/// <summary>
		///	Returns the number of intersection points found.	
		/// </summary>
		/// <returns>
		///	Returns the number of intersection points found.  This will be either 0, 1 or 2.
		/// </returns>
		public int GetIntersectionNum()
		{
			return _result;
		} // public int GetIntersectionNum()

		/// <summary>
		/// Returns the intersection at the specified index.
		/// </summary>
		/// <param name="index">index is 0 or 1</param>
		/// <returns>
		///	Returns the intersection point at index.
		/// </returns>
		public Coordinate GetIntersection(int index)
		{
			return _intPt[index];
		} // public Coordinate GetIntersection(int index)
		
		/// <summary>
		/// 
		/// </summary>
		protected void ComputeIntLineIndex() 
		{
			if (_intLineIndex == null) 
			{
				_intLineIndex = new int[2,2];
				ComputeIntLineIndex(0);
				ComputeIntLineIndex(1);
			}
		} // protected void ComputeIntLineIndex()
		
		/// <summary>
		///	Test whether a point is a intersection point of two line segments.
		/// </summary>
		/// <param name="pt">Point to test.</param>
		/// <returns>
		/// 	True if the input point is one of the intersection points.
		/// </returns>
		/// <remarks>
		/// 	Note that if the intersection is a line segment, this method only tests for
		/// 	equality with the endpoints of the intersection segment.
		/// 	It does not return true if the input point is internal to the intersection segment.
		/// </remarks>
		public bool IsIntersection(Coordinate pt) 
		{
			for (int i = 0; i < _result; i++) 
			{
				if ( _intPt[i].Equals2D( pt ) )
				{
					return true;
				}
			}
			return false;
		} // public bool IsIntersection(Coordinate pt)

		/// <summary>
		///		Tests whether an intersection is proper.
		/// </summary>
		/// <remarks>
		///		<para>The intersection between two line segments is considered proper if
		///		they intersect in a single point in the interior of both segments
		///		(e.g. the intersection is a single point and is not equal to any of the
		///		endpoints).</para>
		///		
		///		<para>The intersection between a point and a line segment is considered proper
		///		if the point lies in the interior of the segment (e.g. is not equal to
		///		either of the endpoints).</para>
		/// </remarks>
		/// <returns>True if the intersection is proper</returns>
		public bool IsProper() 
		{
			return HasIntersection() && _isProper;
		} // public bool IsProper()

		/// <summary>
		///	Computes the intersection point at index in the direction of a specified input line segment.
		/// </summary>
		/// <param name="segmentIndex">Index for the line segment.</param>
		/// <param name="index">Index for the intersection point.</param>
		/// <returns>
		///	The intersection point at index in the direction of the specified input line segment.
		///	</returns>
		public Coordinate GetIntersectionAlongSegment(int segmentIndex, int index) 
		{
			// lazily compute int line array
			ComputeIntLineIndex();
			return _intPt[ _intLineIndex[segmentIndex,index] ];

		} // public Coordinate GetIntersectionAlongSegment(int segmentIndex, int index)

		/// <summary>
		///		Computes the index of the intersection point at index in the direction of
		///		a specified input line segment
		/// </summary>
		/// <param name="segmentIndex">Index for the line segment.</param>
		/// <param name="index">Index for the intersection point.</param>
		/// <returns>
		///	The index of the intersection point along the segment (0 or 1).
		/// </returns>
		public int GetIndexAlongSegment(int segmentIndex, int index) 
		{
			ComputeIntLineIndex();
			return _intLineIndex[segmentIndex,index];

		} // public int GetIndexAlongSegment(int segmentIndex, int index)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="segmentIndex"></param>
		protected void ComputeIntLineIndex(int segmentIndex) 
		{
			double dist0 = GetEdgeDistance(segmentIndex, 0);
			double dist1 = GetEdgeDistance(segmentIndex, 1);
			if (dist0 > dist1) 
			{
				_intLineIndex[segmentIndex,0] = 0;
				_intLineIndex[segmentIndex,1] = 1;
			}
			else 
			{
				_intLineIndex[segmentIndex,0] = 1;
				_intLineIndex[segmentIndex,1] = 0;
			}
		} // protected void ComputeIntLineIndex(int segmentIndex) 

		/// <summary>
		///	Computes the "edge distance" of an intersection point along the specified input line segment.
		/// </summary>
		/// <param name="segmentIndex"></param>
		/// <param name="index"></param>
		/// <returns>The edge distance of the intersection point</returns>
		public double GetEdgeDistance(int segmentIndex, int index) 
		{
			double dist = ComputeEdgeDistance( _intPt[index], _inputLines[segmentIndex,0],
				_inputLines[segmentIndex,1] );
			return dist;
		}
		#endregion
	}
}

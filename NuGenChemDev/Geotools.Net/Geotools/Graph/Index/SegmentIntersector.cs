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
using Geotools.Algorithms;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph.Index
{
	/// <summary>
	/// Summary description for SegmentIntersector.
	/// </summary>
	internal class SegmentIntersector
	{
		// These variables keep track of what types of intersections were
		// found during All edges that have been intersected.
		private bool _hasIntersection = false;
		private bool _hasProper = false;
		private bool _hasProperInterior = false;
		// the proper intersection point found
		private Coordinate _properIntersectionPoint = null;

		private LineIntersector _lineIntersector;
		private bool _includeProper;
		private bool _recordIsolated;
		//private bool _isSelfIntersection;    This is not used.

		//private boolean intersectionFound;
		private int _numIntersections = 0;

		private ArrayList[] _bdyNodes = null;

		// testing only
		public int _numTests = 0;


		#region Constructors

		public SegmentIntersector(LineIntersector lineIntersector,  bool includeProper, bool recordIsolated)
		{
			_lineIntersector = lineIntersector;
			_includeProper = includeProper;
			_recordIsolated = recordIsolated;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Return the proper intersection point, or null if none was found.
		/// </summary>
		/// <returns></returns>
		public Coordinate ProperIntersectionPoint
		{
			get
			{
				return _properIntersectionPoint; 
			}
		}

		public bool HasIntersection
		{
			get
			{
				return _hasIntersection;
			}
		}

		/// <summary>
		/// A proper intersection is an intersection which is interior to at least two
		/// line segments.  Note that a proper intersection is not necessarily
		/// in the interior of the entire Geometry, since another edge may have
		/// an endpoint equal to the intersection, which according to SFS semantics
		///  can result in the point being on the Boundary of the Geometry.
		/// </summary>
		/// <returns></returns>
		public bool HasProperIntersection
		{ 
			get
			{
				return _hasProper;
			}
		}

		/// <summary>
		/// A proper interior intersection is a proper intersection which is <b>not</b>
		///  contained in the set of boundary nodes set for this SegmentIntersector.
		/// </summary>
		/// <returns></returns>
		public bool HasProperInteriorIntersection
		{
			get
			{ 
				return _hasProperInterior; 
			}
		}

		#endregion

		#region Static Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i1"></param>
		/// <param name="i2"></param>
		/// <returns></returns>
		public static bool IsAdjacentSegments(int i1, int i2)
		{
			return Math.Abs(i1 - i2) == 1;
		}
		#endregion

		#region Methods

		public void SetBoundaryNodes( System.Collections.ArrayList bdyNodes0,
									  System.Collections.ArrayList bdyNodes1)
		{
			_bdyNodes = new ArrayList[2];
			_bdyNodes[0] = bdyNodes0;
			_bdyNodes[1] = bdyNodes1;
		}

		/// <summary>
		/// A trivial intersection is an apparent self-intersection which in fact
		/// is simply the point shared by adjacent line segments.
		/// Note that closed edges require a special check for the point shared by the beginning
		/// and end segments.
		/// </summary>
		/// <param name="e0"></param>
		/// <param name="segIndex0"></param>
		/// <param name="e1"></param>
		/// <param name="segIndex1"></param>
		/// <returns></returns>
		private bool IsTrivialIntersection(Edge e0, int segIndex0, Edge e1, int segIndex1)
		{
			if ( e0 == e1 ) 
			{
				if (_lineIntersector.GetIntersectionNum() == 1) 
				{
					if (IsAdjacentSegments(segIndex0, segIndex1))
						return true;
					if ( e0.IsClosed ) 
					{
						int maxSegIndex = e0.Count - 1;
						if (    (segIndex0 == 0 && segIndex1 == maxSegIndex)
							||  (segIndex1 == 0 && segIndex0 == maxSegIndex) ) 
						{
							return true;
						}
					}
				}
			}
			return false;
		} // private bool IsTrivialIntersection(Edge e0, int segIndex0, Edge e1, int segIndex1)

		/// <summary>
		/// This method is called by clients of the EdgeIntersector class to test for and add
		/// intersections for two segments of the edges being intersected.
		/// Note that clients (such as MonotoneChainEdges) may choose not to intersect
		/// certain pairs of segments for efficiency reasons.
		/// </summary>
		/// <param name="e0"></param>
		/// <param name="segIndex0"></param>
		/// <param name="e1"></param>
		/// <param name="segIndex1"></param>
		public void AddIntersections(Edge e0, int segIndex0, Edge e1, int segIndex1)
		{
			if ( e0 == e1 && segIndex0 == segIndex1) return;
			_numTests++;
			Coordinate p00 = e0.Coordinates[segIndex0];
			Coordinate p01 = e0.Coordinates[segIndex0 + 1];
			Coordinate p10 = e1.Coordinates[segIndex1];
			Coordinate p11 = e1.Coordinates[segIndex1 + 1];

			_lineIntersector.ComputeIntersection( p00, p01, p10, p11);
			
			if ( _lineIntersector.HasIntersection() ) 
			{
				if ( _recordIsolated ) 
				{
					e0.SetIsIsolated( false );
					e1.SetIsIsolated( false );
				
				}
				//intersectionFound = true;
				_numIntersections++;
				// if the segments are adjacent they have at least one trivial intersection,
				// the shared endpoint.  Don't bother adding it if it is the
				// only intersection.
				if ( !IsTrivialIntersection( e0, segIndex0, e1, segIndex1) ) 
				{
					_hasIntersection = true;
					if ( _includeProper || ! _lineIntersector.IsProper() ) 
					{
						//Debug.println(_lineIntersector);
						e0.AddIntersections( _lineIntersector, segIndex0, 0 );
						e1.AddIntersections( _lineIntersector, segIndex1, 1 );
					}
					if ( _lineIntersector.IsProper() )
					{
						_properIntersectionPoint = (Coordinate) _lineIntersector.GetIntersection(0).Clone();
						_hasProper = true;
						if ( !IsBoundaryPoint( _lineIntersector, _bdyNodes) )
						{
							_hasProperInterior = true;
						}
					}
				} // if ( !IsTrivialIntersection( e0, segIndex0, e1, segIndex1) ) 

			} // if ( _lineIntersector.HasIntersection() )

		} // public void AddIntersections(Edge e0, int segIndex0, Edge e1, int segIndex1)

		private bool IsBoundaryPoint(LineIntersector lineIntersector, ArrayList[] bdyNodes)
		{
			//int i = lineIntersector.GetHashCode();
			//int j = _lineIntersector.GetHashCode();
			if ( bdyNodes == null ) return false;
			if ( IsBoundaryPoint( lineIntersector, bdyNodes[0]) ) return true;
			if ( IsBoundaryPoint( lineIntersector, bdyNodes[1]) ) return true;
			return false;
		} // private bool IsBoundaryPoint(LineIntersector lineIntersector, ArrayList[] bdyNodes)

		private bool IsBoundaryPoint(LineIntersector lineIntersector, ArrayList bdyNodes)
		{
			//int i = lineIntersector.GetHashCode();
			//int j = _lineIntersector.GetHashCode();
			foreach (object objectNode in bdyNodes)
			{
				Node node = (Node)objectNode;
				Coordinate pt = node.Coordinate;
				if ( lineIntersector.IsIntersection(pt) )
				{
					return true;
				}
			}
			return false;
		} // private bool IsBoundaryPoint(LineIntersector lineIntersector, ArrayList bdyNodes)


		#endregion

	}
}

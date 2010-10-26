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

namespace Geotools.Graph.Index
{

	/// <summary>
	/// MonotoneChains are a way of partitioning the segments of an edge to allow for fast searching of intersections.
	/// </summary>
	/// <remarks>
	/// They have the following properties:
	/// <ol>
	/// <li>the segments within a monotone chain will never intersect each other</li>
	/// <li>the envelope of any contiguous subset of the segments in a monotone chain is simply the envelope of the endpoints of the subset.</li>
	/// </ol>
	/// Property 1 means that there is no need to test pairs of segments from within the same monotone chain for intersection.
	/// Property 2 allows binary search to be used to find the intersection points of two monotone chains. For many types of real-world data, these properties eliminate a large number of segment comparisons, producing substantial speed gains.
	/// </remarks>
	internal class MonotoneChainEdge
	{
		Edge _e;
		Coordinates _pts; // cache a reference to the coord array, for efficiency
		// the lists of start/end indexes of the monotone chains.
		// Includes the end point of the edge as a sentinel
		int[] _startIndex;
		// these envelopes are created once and reused
		Envelope _env1 = new Envelope();
		Envelope _env2 = new Envelope();


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MonotoneChainEdge class.
		/// </summary>
		public MonotoneChainEdge(Edge e) 
		{
			_e = e;
 			_pts = _e.Coordinates;
 			_startIndex = MonotoneChainIndexer.GetChainStartIndices( _pts );
		}
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public Coordinates Coordinates
		{
			get
			{
				return _pts;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int[] StartIndex
		{ 
			get
			{
				return _startIndex;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public Edge Edge
		{
			get
			{
				return _e;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="chainIndex"></param>
		/// <returns></returns>
		public double GetMinX(int chainIndex)
		{
			double x1 = _pts[_startIndex[chainIndex]].X;
			double x2 = _pts[_startIndex[chainIndex + 1]].X;
			return x1 < x2 ? x1 : x2;
		} // public double GetMinX(int chainIndex)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chainIndex"></param>
		/// <returns></returns>
		public double GetMaxX(int chainIndex)
		{
			double x1 = _pts[_startIndex[chainIndex]].X;
			double x2 = _pts[_startIndex[chainIndex + 1]].X;
			return x1 > x2 ? x1 : x2;
		} // public double GetMaxX(int chainIndex)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mce"></param>
		/// <param name="si"></param>
		public void ComputeIntersects(MonotoneChainEdge mce, SegmentIntersector si)
		{
			for (int i = 0; i < _startIndex.Length - 1; i++) 
			{
			 	for (int j = 0; j <  mce.StartIndex.Length - 1; j++) 
				{
					ComputeIntersectsForChain( i, mce, j, si );
				}
			}
		} // public void ComputeIntersects(MonotoneChainEdge mce, SegmentIntersector si)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chainIndex0"></param>
		/// <param name="mce"></param>
		/// <param name="chainIndex1"></param>
		/// <param name="si"></param>
		public void ComputeIntersectsForChain( int chainIndex0,
												MonotoneChainEdge mce,
												int chainIndex1,
												SegmentIntersector si	)
		{
			ComputeIntersectsForChain(	_startIndex[chainIndex0], 
										_startIndex[chainIndex0 + 1],
			 							mce,
			 							mce.StartIndex[chainIndex1], mce.StartIndex[chainIndex1 + 1],
			 							si );
		} // public void ComputeIntersectsForChain( int chainIndex0,



		private void ComputeIntersectsForChain(		int start0,
													int end0,
													MonotoneChainEdge mce,
													int start1,
													int end1,
													SegmentIntersector ei	)
		{
			Coordinate p00 = _pts[start0];
			Coordinate p01 = _pts[end0];
			Coordinate p10 = mce.Coordinates[start1];
			Coordinate p11 = mce.Coordinates[end1];
			//Debug.println("computeIntersectsForChain:" + p00 + p01 + p10 + p11);
			// terminating condition for the recursion
			if (end0 - start0 == 1 && end1 - start1 == 1) 
			{
			 	ei.AddIntersections( _e, start0, mce.Edge, start1);
			 	return;
			}
			// nothing to do if the envelopes of these chains don't overlap
			_env1.Initialize(p00, p01);
			_env2.Initialize(p10, p11);
			if ( !_env1.Intersects( _env2 ) ) return;
	
			// the chains overlap, so split each in half and iterate  (binary search)
			int mid0 = (start0 + end0) / 2;
			int mid1 = (start1 + end1) / 2;
	
			// Assert: mid != start or end (since we checked above for end - start <= 1)
			// check terminating conditions before recursing
			if (start0 < mid0) 
			{
			 	if (start1 < mid1)
			 	{
			 		ComputeIntersectsForChain( start0, mid0, mce, start1, mid1, ei );
			 	}
			 	if (mid1 < end1)
			 	{
			 		ComputeIntersectsForChain( start0, mid0, mce, mid1, end1, ei );
			 	}
			}
			if (mid0 < end0) 
			{
			 	if (start1 < mid1)
			 	{
			 		ComputeIntersectsForChain( mid0, end0, mce, start1,  mid1, ei);
			 	}
			 	if (mid1 < end1) 
			 	{
			 		ComputeIntersectsForChain( mid0, end0, mce, mid1,    end1, ei);
			 	}
			}
		} // private void ComputeIntersectsForChain(		int start0,...
		#endregion

	}
}

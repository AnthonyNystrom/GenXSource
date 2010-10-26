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

namespace Geotools.Graph.Index
{

	/// <summary>
	/// MonotoneChains are a way of partitioning the segments of an edge to
	///	allow for fast searching of intersections.
	///	</summary>
	/// <remarks>
	/// They have the following properties:
	/// <ol>
	/// <li>the segments within a monotone chain will never intersect each other</li>
	/// <li>the envelope of any contiguous subset of the segments in a monotone chain is simply the envelope of the endpoints of the subset.</li>
	/// </ol>
	/// Property 1 means that there is no need to test pairs of segments from within
	/// the same monotone chain for intersection.
	/// Property 2 allows
	/// binary search to be used to find the intersection points of two monotone chains.
	/// For many types of real-world data, these properties eliminate a large number of
	/// segment comparisons, producing substantial speed gains.
	///</remarks>
	internal class MonotoneChainIndexer
	{
		#region Static Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static int[] GetChainStartIndices(Coordinates pts)
		{
			// find the startpoint (and endpoints) of all monotone chains in this edge
			int start = 0;
			ArrayList startIndexList = new ArrayList();
			startIndexList.Add( start );
			do 
			{
				int last = FindChainEnd( pts, start );
				startIndexList.Add( last );
				start = last;
			} while ( start < pts.Count - 1 );
			// copy list to an array of ints, for efficiency
			int[] startIndex = (int[])startIndexList.ToArray( typeof(System.Int32) );
			return startIndex;
		}

		/// <summary>
		/// Return the index of the last point in the monotone chain
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		private static int FindChainEnd(Coordinates pts, int start)
		{
			// determine quadrant for chain
			int chainQuad = Quadrant.QuadrantLocation( pts[start], pts[start + 1] );
			int last = start + 1;
			while (last < pts.Count ) 
			{
				// compute quadrant for next possible segment in chain
				int quad = Quadrant.QuadrantLocation( pts[last - 1], pts[last] );
				if (quad != chainQuad)
				{
					break;
				}
				last++;
			}
			return last - 1;
		}

		#endregion

	}
}

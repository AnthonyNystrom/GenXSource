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
using Geotools.Graph;
#endregion

namespace Geotools.Index.Chain
{
	/// <summary>
	///  A MonotoneChainBuilder implements functions to determine the monotone chains
	///  in a sequence of points.
	/// </summary>
	internal class MonotoneChainBuilder
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MonotoneChainBuilder class.
		/// </summary>
		public MonotoneChainBuilder()
		{
		}
		#endregion

		#region Static methods

		/// <summary>
		/// Return a list of the monotone chains
		/// for the given list of coordinates.
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static ArrayList GetChains(Coordinates pts)
		{
			ArrayList mcList = new ArrayList();
			int[] startIndex = GetChainStartIndices( pts );
			for ( int i = 0; i < startIndex.Length - 1; i++ ) 
			{
				MonotoneChain mc = new MonotoneChain( pts, startIndex[i], startIndex[i + 1] );
				mcList.Add( mc );
			} // for ( int i = 0; i < startIndex.Length - 1; i++ )
			return mcList;
		} // public static ArrayList GetChains(Coordinates pts)


		/// <summary>
		/// Return an array containing lists of start/end indexes of the monotone chains
		/// for the given list of coordinates.
		/// The last entry in the array points to the end point of the point array,
		/// for use as a sentinel.
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		public static int[] GetChainStartIndices( Coordinates pts )
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
			} while ( start < pts.Count - 1);
			// copy list to an array of ints, for efficiency
			int[] startIndex = ( int[] )startIndexList.ToArray( Type.GetType( "System.Int32" ) );
			return startIndex;
		} // public static int[] GetChainStartIndices( Coordinates pts )

		/// <summary>
		/// Returns the index of the last point in the monotone chain starting at start.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="start"></param>
		/// <returns>Return the index of the last point in the monotone chain starting at start.</returns>
		private static int FindChainEnd( Coordinates pts, int start )
		{
			// determine quadrant for chain
			int chainQuad = Quadrant.QuadrantLocation( pts[start], pts[start + 1]);
			int last = start + 1;
			while ( last < pts.Count ) 
			{
				// compute quadrant for next possible segment in chain
				int quad = Quadrant.QuadrantLocation( pts[last - 1], pts[last] );
				if ( quad != chainQuad ) break;
				last++;
			} // while ( last < pts.Count )
			return last - 1;
		} // private static int FindChainEnd( Coordinates pts, int start )
		#endregion

		#region Properties
		#endregion

		#region Methods
		#endregion

	} // public class MonotoneChainBuilder
}

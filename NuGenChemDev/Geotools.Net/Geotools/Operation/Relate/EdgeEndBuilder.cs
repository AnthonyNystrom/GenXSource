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
using Geotools.Graph;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// An EdgeEndBuilder creates EdgeEnds for all the "split edges" created by the
	/// intersections determined for an edge.
	/// </summary>
	internal class EdgeEndBuilder
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeEndBuilder class.
		/// </summary>
		public EdgeEndBuilder()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="edges"></param>
		/// <returns></returns>
		public ArrayList ComputeEdgeEnds( ArrayList edges ) 
		{
			ArrayList list = new ArrayList();
			foreach ( object obj in edges ) 
			{
				Edge e = (Edge) obj;
				ComputeEdgeEnds( e, list );
			}
			return list;
		} // public ArrayList ComputeEdgeEnds( ArrayList edges )


		/// <summary>
		/// Creates stub edges for all the intersections in this Edge (if any) and inserts them into the graph.
		/// </summary>
		/// <param name="edge"></param>
		/// <param name="list"></param>
		public void ComputeEdgeEnds( Edge edge, ArrayList list )
		{
			EdgeIntersectionList eiList = edge.EdgeIntersectionList;
			//Trace.WriteLine( eiList.ToString() );

			// ensure that the list has entries for the first and last point of the edge
			if ( !edge.IsClosed )
			{
				eiList.AddEndpoints();
			}

			EdgeIntersection eiPrev = null;
			EdgeIntersection eiCurr = null;
			// no intersections, so there is nothing to do
			if ( eiList.IsEmpty() ) return;		// return if the list is empty
			int index = 0;						// index of eiList array.
			EdgeIntersection eiNext = eiList[index]; // gets the first intersection
			do 
			{
				index++;
				eiPrev = eiCurr;		// previouse one or null for first loop
				eiCurr = eiNext;		// current one or the first one
				eiNext = null;
				if ( index < eiList.Count) eiNext = eiList[index];	// if there are more 

				if ( eiCurr != null ) 
				{
					CreateEdgeEndForPrev( edge, list, eiCurr, eiPrev );
					CreateEdgeEndForNext( edge, list, eiCurr, eiNext );
				}

			} while ( eiCurr != null );
		} // public void ComputeEdgeEnds( Edge edge, ArrayList l )

		/// <summary>
		/// Create a EdgeStub for the edge before the intersection eiCurr.
		///	The previous intersection is provided
		///	in case it is the endpoint for the stub edge.
		///	Otherwise, the previous point from the parent edge will be the endpoint.
		/// eiCurr will always be an EdgeIntersection, but eiPrev may be null.
		/// </summary>
		/// <param name="edge"></param>
		/// <param name="list"></param>
		/// <param name="eiCurr"></param>
		/// <param name="eiPrev"></param>
		void CreateEdgeEndForPrev(
			Edge edge,
			ArrayList list,
			EdgeIntersection eiCurr,
			EdgeIntersection eiPrev)
		{
			int iPrev = eiCurr.SegmentIndex;
			if ( eiCurr.Distance == 0.0 ) 
			{
				// if at the start of the edge there is no previous edge
				if ( iPrev == 0 ) return;
				iPrev--;
			}

			Coordinate pPrev = edge.GetCoordinate( iPrev );

			// if prev intersection is past the previous vertex, use it instead
			if ( eiPrev != null && eiPrev.SegmentIndex >= iPrev )
			{
				pPrev = eiPrev.Coordinate;
			}

			Label label = new Label( edge.Label );
			// since edgeStub is oriented opposite to it's parent edge, have to flip sides for edge label
			label.Flip();
			EdgeEnd e = new EdgeEnd( edge, eiCurr.Coordinate, pPrev, label );
			//Trace.WriteLine( e.ToString() );
			list.Add( e );	
		}  //void CreateEdgeEndForPrev(...

		/// <summary>
		/// Create a StubEdge for the edge after the intersection eiCurr.
		/// The next intersection is provided in case it is the endpoint for the stub edge.
		/// Otherwise, the next point from the parent edge will be the endpoint.
		/// eiCurr will always be an EdgeIntersection, but eiNext may be null.
		/// </summary>
		/// <param name="edge"></param>
		/// <param name="list"></param>
		/// <param name="eiCurr"></param>
		/// <param name="eiNext"></param>
		void CreateEdgeEndForNext(
			Edge edge,
			ArrayList list,
			EdgeIntersection eiCurr,
			EdgeIntersection eiNext)
		{
			int iNext = eiCurr.SegmentIndex + 1;

			// if there is no next edge there is nothing to do
			if ( iNext >= edge.NumPoints && eiNext == null ) return;

			Coordinate pNext = edge.GetCoordinate( iNext );

			// if the next intersection is in the same segment as the current, use it as the endpoint
			if ( eiNext != null && eiNext.SegmentIndex == eiCurr.SegmentIndex )
			{
				pNext = eiNext.Coordinate;
			}

			EdgeEnd e = new EdgeEnd( edge, eiCurr.Coordinate, pNext, new Label( edge.Label ) );
			//Trace.WriteLine( e.ToString() );
			list.Add( e );
		} // void CreateEdgeEndForNext(...
		
		#endregion

	} // public class EdgeEndBuilder
}

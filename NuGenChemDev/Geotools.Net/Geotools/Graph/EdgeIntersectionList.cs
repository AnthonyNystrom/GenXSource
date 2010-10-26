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
using System.Text;
using System.Collections;
using System.IO;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for EdgeIntersectionList.
	/// </summary>
	internal class EdgeIntersectionList : IEnumerable 
	{
		
		/// <summary>
		/// The list of EdgeIntersections
		/// </summary>
		protected ArrayList _list = new ArrayList(); 

		/// <summary>
		/// The parent edge
		/// </summary>
		protected Edge _edge;  // the parent edge

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeIntersectionList class.
		/// </summary>
		public EdgeIntersectionList(Edge edge)
		{
			_edge = edge;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of edge intersections.
		/// </summary>
		public int Count
		{
			get
			{
				return _list.Count;
			}
		}
		#endregion

		#region Methods
   
		/// <summary>
		/// Item implementation.
		/// </summary>
		public EdgeIntersection this[int index]
		{
			get
			{
					return (EdgeIntersection)_list[index];
			}
		} // public EdgeIntersection this[int index]

		/// <summary>
		/// Adds an intersection into the list, if it isn't already there. 
		/// The input segmentIndex and distance are expected to be normalized.
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="segmentIndex"></param>
		/// <param name="distance"></param>
		/// <returns>return the EdgeIntersection found or added</returns>
		public EdgeIntersection Add(Coordinate coord, int segmentIndex, double distance)
		{
			// this varies from the java code due to the differences in arraylist.  java can add based on the
			// iterator.
			int indexAt;
			bool isInList = FindInsertionPoint( segmentIndex, distance, out indexAt );
			EdgeIntersection ei;
			if ( !isInList ) 
			{
				ei = new EdgeIntersection(coord, segmentIndex, distance);
				if ( indexAt < 0 )	// not found and greater than all others in list.
				{
					_list.Add( ei );
				}
				else
				{
					_list.Insert( indexAt, ei );	// not found but insert at this index to keep order.
				}
			}
			else
			{
				ei = (EdgeIntersection)_list[indexAt];	// found in list at this index.
			}
			return ei;
		}

		/// <summary>
		/// Returns the Enumerator for this collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		/// <summary>
		/// Returns true if this collection is empty and false otherwise.
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return _list.Count > 0 ? false : true;
		}

		/// <summary>
		/// This routine searches the list for the insertion point for the given intersection
		/// (which must be in normalized form).
		/// </summary>
		/// <remarks>
		/// If the intersection is new, it is inserted into the list.
		/// The insertIt iterator is left pointing at the correct place
		/// to insert the intersection, if the intersection was not found.
		/// </remarks>
		/// <param name="segmentIndex"></param>
		/// <param name="dist"></param>
		/// <param name="indexAt"></param>
		/// <returns>return true if this intersection is already in the list</returns>
		bool FindInsertionPoint( int segmentIndex, double dist, out int indexAt )
		{
			indexAt = -1;	// object not in list.
			foreach( EdgeIntersection ei in _list )
			{
				int compare = ei.Compare(segmentIndex, dist);

				// intersection found - so don't insert into the list. But need the index so add can return this object.
				if (compare == 0) 
				{
					indexAt = _list.IndexOf( ei );
					return true;
				}

				// this ei is past the intersection location, so intersection was not found but want to add it in at this index.
				if (compare > 0)
				{
					indexAt = _list.IndexOf( ei );
					return false; // need the index point at which this happens.
				}
			} // foreach( object objEdgeIntersection in _list )		
			return false;	// not found in list
		}

		/// <summary>
		/// Checks if coordinate pt is an intersection in the edge intersection list.
		/// </summary>
		/// <param name="pt">Coordinate to check if in the intersection list.</param>
		/// <returns>Returns true if pt is in the edge intersection list.</returns>
		public bool IsIntersection(Coordinate pt)
		{			
			foreach( EdgeIntersection ei in _list )
			{
					if ( ei.Coordinate.Equals( pt ) )
					{
						return true;
					}
			} // foreach( object objEdgeIntersection in _list )

			return false;
		} // public bool IsIntersection(Coordinate pt)


		/// <summary>
		/// Adds entries for the first and last points of the edge to the list
		/// </summary>
		public void AddEndpoints()
		{
			int maxSegIndex = _edge.NumPoints - 1;
			Add( _edge.Coordinates[0], 0, 0.0);
			Add( _edge.Coordinates[maxSegIndex], maxSegIndex, 0.0);
		} // public void AddEndpoints()

		/// <summary>
		/// Creates new edges for all the edges that the intersections in this
		/// list split the parent edge into.
		/// </summary>
		/// <remarks>
		/// Adds the edges to the input list (this is so a single list can be used to accumulate all split edges for a Geometry).
		/// </remarks>
		/// <param name="edgeList"></param>
		public void AddSplitEdges(ArrayList edgeList)
		{

			// ensure that the list has entries for the first and last point of the edge
			AddEndpoints();

			// there should always be at least two entries in the list
			EdgeIntersection eiPrev = (EdgeIntersection) _list[0];
			for ( int i = 1; i < _list.Count; i++ )		// start at 1st element cuz eiPrev is equal to 0th element.
			{
				EdgeIntersection ei = (EdgeIntersection) _list[i];
				Edge newEdge = CreateSplitEdge(eiPrev, ei);
				edgeList.Add( newEdge );
				eiPrev = ei;
			} // for ( int i = 1; i < _list.Count; i++ )
			
		} // public void AddSplitEdges(ArrayList edgeList)


		/// <summary>
		/// Create a new "split edge" with the section of points between
		/// (and including) the two intersections.
		/// The label for the new edge is the same as the label for the parent edge.
		/// </summary>
		private Edge CreateSplitEdge(EdgeIntersection ei0, EdgeIntersection ei1)
		{
			//Trace.WriteLine("\ncreateSplitEdge"); Trace.WriteLine(ei0.ToString()); Trace.WriteLine(ei1.ToString());
			int npts = ei1.SegmentIndex - ei0.SegmentIndex + 2;

			Coordinate lastSegStartPt = _edge.Coordinates[ ei1.SegmentIndex ];
			// if the last intersection point is not equal to the its segment start pt,
			// add it to the points list as well.
			// (This check is needed because the distance metric is not totally reliable!)
			bool useIntPt1 = ei1.Distance > 0.0 || ! ei1.Coordinate.Equals( lastSegStartPt );

			Coordinates coordinatePts = new Coordinates();  // new coordinates collection
			coordinatePts.Add( new Coordinate( ei0.Coordinate ) );
			for (int i = ei0.SegmentIndex + 1; i <= ei1.SegmentIndex; i++) 
			{
				coordinatePts.Add( _edge.Coordinates[i] );
			}

			if ( useIntPt1 )
			{
				coordinatePts.Add( ei1.Coordinate );
			}

			return new Edge( coordinatePts, new Label( _edge.Label ));

		} // Edge CreateSplitEdge(EdgeIntersection ei0, EdgeIntersection ei1)


		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Intersections:");
			foreach(object objEdgeIntersection in _list)
			{
				EdgeIntersection ei = (EdgeIntersection) objEdgeIntersection;
				sb.Append(ei.ToString());
			}
			return sb.ToString();
		} //public override string ToString()

		#endregion

	}
}

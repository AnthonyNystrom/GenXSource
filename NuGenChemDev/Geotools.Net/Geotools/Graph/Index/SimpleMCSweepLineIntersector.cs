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
#endregion

namespace Geotools.Graph.Index
{
	/// <summary>
	/// A SimpleMCSweepLineIntersector creates monotone chains from the edges
	/// and compares them using a simple sweep-line along the x-axis.
	/// </summary>
	internal class SimpleMCSweepLineIntersector : EdgeSetIntersector
	{
		ArrayList _events = new ArrayList();

		// statistics information
		int _nOverlaps;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SimpleMCSweepLineIntersector class.
		/// </summary>
		public SimpleMCSweepLineIntersector()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Computes all self-intersections between edges in a set of edges.
		/// </summary>
		/// <param name="edges">Array of edges from which to compute self-intersections.</param>
		/// <param name="si">SegmentIntersector object.</param>
		public override void ComputeIntersections( ArrayList edges, SegmentIntersector si )
		{
			Add(edges, 0);
			ComputeIntersections(si, false);
		}

		/// <summary>
		/// Computes all mutual intersections between two sets of edges.
		/// </summary>
		/// <param name="edges0"></param>
		/// <param name="edges1"></param>
		/// <param name="si"></param>
		public override void ComputeIntersections(ArrayList edges0, ArrayList edges1, SegmentIntersector si)
		{
			Add(edges0, 0);
			Add(edges1, 1);
			ComputeIntersections(si, true);
		}

		private void Add(ArrayList edges, int geomIndex)
		{
			foreach(object objectEdge in edges)
			{
				Edge edge = objectEdge as Edge;
				if ( edge != null )
				{
					Add(edge, geomIndex);
				}
				else
				{
					throw new ArgumentException("List of edges includes object not of type Edge");
				}
			} // foreach(object objectEdge in edges)
		} // private void Add(ArrayList edges, int geomIndex)

		private void Add(Edge edge, int geomIndex)
		{
			MonotoneChainEdge mce = edge.GetMonotoneChainEdge();
			int[] startIndex = mce.StartIndex;
			for (int i = 0; i < startIndex.Length - 1; i++) 
			{
				MonotoneChain mc = new MonotoneChain( mce, i, geomIndex );
				SweepLineEvent insertEvent = new SweepLineEvent( geomIndex, mce.GetMinX(i), null, mc);
				_events.Add( insertEvent );
				_events.Add( new SweepLineEvent( geomIndex, mce.GetMaxX(i), insertEvent, mc) );
			}
		} // private void Add(Edge edge, int geomIndex)

		private void ComputeIntersections(SegmentIntersector si, bool doMutualOnly)
		{
			_nOverlaps = 0;
			PrepareEvents();
  
			for( int i = 0; i < _events.Count; i++ )
			{
 				SweepLineEvent ev = (SweepLineEvent)_events[i];
 				MonotoneChain mc = (MonotoneChain) ev.Object;
 				if ( ev.IsInsert ) 
 				{
 					ProcessOverlaps( i, ev.DeleteEventIndex, mc, si, doMutualOnly );
 				}
 			}
		} // private void ComputeIntersections(SegmentIntersector si, bool doMutualOnly)

		/// <summary>
		/// Because Delete events have a link to their corresponding Insert event,
		/// it is possible to compute exactly the range of events which must be
		/// compared to a given Insert event object.
		/// </summary>
		private void PrepareEvents()
		{
			_events.Sort();
			//TODO:remove this bubble sort!!!! AWC, LAK, RAB - we should find this!!!!!!
			//**************************************************************************8

						// remove this bubble sort.

			//************************************************************************
			/*int count=0;
			for (int a=0;a<_events.Count;a++)
			{
				for (int b=0;b<_events.Count-1;b++)
				{
					SweepLineEvent ev1 = (SweepLineEvent) _events[b];
					SweepLineEvent ev2 = (SweepLineEvent) _events[b+1];
					if (ev1.CompareTo(ev2)==1)
					{
						SweepLineEvent temp = ev1;
						_events[b]=ev2;
						_events[b+1]=temp;    		
						count++;
					}
				}
			}
			*/
			for (int i = 0; i < _events.Count; i++ )
			{
				SweepLineEvent ev = (SweepLineEvent) _events[i];
				if ( ev.IsDelete ) 
				{
					ev.InsertEvent.DeleteEventIndex = i;
				}
			}
		} // private void PrepareEvents()

		private void ProcessOverlaps(int start, int end, MonotoneChain mc0, SegmentIntersector si, bool doMutualOnly)
		{
			// Since we might need to test for self-intersections,
			// include current insert event object in list of event objects to test.
			// Last index can be skipped, because it must be a Delete event.
			for ( int i = start; i < end; i++ ) 
			{
			 	SweepLineEvent ev = (SweepLineEvent) _events[i];
			 	if ( ev.IsInsert ) 
			 	{
			 		MonotoneChain mc1 = (MonotoneChain) ev.Object;
			 		if ( !doMutualOnly || ( mc0.GeomIndex != mc1.GeomIndex )  ) 
			 		{
			 			mc0.ComputeIntersections( mc1, si );
			 			_nOverlaps++;
			 		}
			 	}
			} // for ( int i = start; i < end; i++ )

		} // private void ProcessOverlaps(int start, int end, MonotoneChain mc0, SegmentIntersector si, bool doMutualOnly)

		#endregion

	}
}

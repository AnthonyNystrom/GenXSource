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

namespace Geotools.Index.Sweepline
{
	/// <summary>
	/// Summary description for SweepLineIndex.
	/// </summary>
	internal class SweepLineIndex
	{
		ArrayList _events = new ArrayList();
		private bool _indexBuilt;

		// statistics information
		private int _nOverlaps;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SweepLineIndex class.
		/// </summary>
		public SweepLineIndex()
		{
		}
		#endregion

		#region Properties
		#endregion

		public void Add( SweepLineInterval sweepInt )
		{
			SweepLineEvent insertEvent = new SweepLineEvent( sweepInt.Min, null, sweepInt);
			_events.Add( insertEvent );
			_events.Add( new SweepLineEvent( sweepInt.Max, insertEvent, sweepInt ) );
		}
		
		/// <summary>
		/// Because Delete Events have a link to their corresponding Insert event,
		/// it is possible to compute exactly the range of events which must be
		/// compared to a given Insert event object.
		/// </summary>
		private void BuildIndex()
		{
			if ( _indexBuilt ) 
			{
				return;
			}

			// Sort the events in the arraylist.    
			// Collections.sort(events);

			for (int i = 0; i < _events.Count; i++ )
			{
				SweepLineEvent ev = (SweepLineEvent) _events[i];
				if ( ev.IsDelete ) 
				{
					ev.InsertEvent.DeleteEventIndex = i;
				}
			}
			_indexBuilt = true;
		} // private void BuildIndex()

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		public void ComputeOverlaps( ISweepLineOverlapAction action )
		{
			_nOverlaps = 0;
			BuildIndex();

			for ( int i = 0; i < _events.Count; i++ )
			{
				SweepLineEvent ev = (SweepLineEvent) _events[i];
				SweepLineInterval sweepInt = ev.Interval;
				if ( ev.IsInsert ) 
				{
					ProcessOverlaps( i, ev.DeleteEventIndex, sweepInt, action );
				}

			} // for ( int i = 0; i < _events.Count; i++ )

		} // public void ComputeOverlaps( SweepLineOverlapAction action )


		private void ProcessOverlaps( int start, int end, SweepLineInterval s0, ISweepLineOverlapAction action )
		{
			/**
			 * Since we might need to test for self-intersections,
			 * include current insert event object in list of event objects to test.
			 * Last index can be skipped, because it must be a Delete event.
			 */
			for ( int i = start; i < end; i++ ) 
			{
				SweepLineEvent ev = (SweepLineEvent) _events[i];
				if ( ev.IsInsert ) 
				{
					SweepLineInterval s1 = ev.Interval;
					action.Overlap( s0, s1 );
					_nOverlaps++;
				}
			} // for ( int i = start; i < end; i++ )
		} // private void ProcessOverlaps( int start, int end, SweepLineInterval s0, SweepLineOverlapAction action )

	

	}
}

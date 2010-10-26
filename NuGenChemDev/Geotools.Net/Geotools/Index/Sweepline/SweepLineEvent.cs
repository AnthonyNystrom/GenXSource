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
#endregion

namespace Geotools.Index.Sweepline
{
	/// <summary>
	/// Summary description for SweepLineEvent.
	/// </summary>
	internal class SweepLineEvent : System.IComparable
	{
		public static int INSERT = 1;
		public static int DELETE = 2;

		private double _xValue;
		private int _eventType;
		private SweepLineEvent _insertEvent; // null if this is an INSERT event
		private int _deleteEventIndex;
		SweepLineInterval _sweepInt;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SweepLineEvent class.
		/// </summary>
		public SweepLineEvent( double x, SweepLineEvent insertEvent, SweepLineInterval sweepInt )
		{
			_xValue = x;
			_insertEvent = insertEvent;
			_eventType = INSERT;
			if ( insertEvent != null )
			{
				_eventType = DELETE;
			}
			_sweepInt = sweepInt;
		}

		#endregion

		#region Implementation of IComparable
		/// <summary>
		/// ProjectionEvents are ordered first by their x-value, and then by their eventType.
		/// It is important that Insert events are sorted before Delete events, so that
		/// items whose Insert and Delete events occur at the same x-value will be
		/// correctly handled.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo( object obj )
		{
			SweepLineEvent pe = ( SweepLineEvent ) obj;
			if ( _xValue < pe.XValue ) return  -1;
			if ( _xValue > pe.XValue) return   1;
			if ( _eventType < pe.EventType ) return  -1;
			if ( _eventType > pe.EventType ) return   1;
			return 0;			
		} // public int CompareTo( object obj )
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public bool IsInsert
		{
			get
			{
				return _insertEvent == null;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsDelete 
		{
			get
			{
				return _insertEvent != null;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public SweepLineEvent InsertEvent
		{
			get
			{
				return _insertEvent;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int DeleteEventIndex
		{
			get
			{
				return _deleteEventIndex;
			}
			set
			{
				_deleteEventIndex = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SweepLineInterval Interval 
		{
			get
			{
				return _sweepInt;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public double XValue
		{
			get
			{
				return _xValue;
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public int EventType
		{
			get
			{
				return _eventType;
			}
		}
		#endregion

		#region Methods
		#endregion

	}
}

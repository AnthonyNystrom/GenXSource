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

namespace Geotools.Graph.Index
{
	/// <summary>
	/// Summary description for SweepLineEvent.
	/// </summary>
	internal class SweepLineEvent : System.IComparable
	{
		public static int Insert = 1;
		public static int Delete = 2;

		private int _geomIndex;    // used for red-blue intersection detection
		private double _xValue;
		private int _eventType;
		private SweepLineEvent _insertEvent; // null if this is an INSERT event
		private int _deleteEventIndex;

		object _obj;

		#region Constructors
		/// <summary>
		/// Constructs a SweepLineEvent object.
		/// </summary>
		/// <param name="geomIndex"></param>
		/// <param name="x"></param>
		/// <param name="insertEvent"></param>
		/// <param name="obj"></param>
		public SweepLineEvent(int geomIndex, double x, SweepLineEvent insertEvent, Object obj)
		{
			_geomIndex = geomIndex;
			_xValue = x;
			_insertEvent = insertEvent;
			_eventType = Insert;
			if ( _insertEvent != null )
			{
				_eventType = Delete;
			}
			_obj = obj;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets the Geometry Index.
		/// </summary>
		public int GeomIndex
		{
			get
			{
				return _geomIndex;
			}
		}
		/// <summary>
		/// Gets the event type ( Insert or Delete ).
		/// </summary>
		public int EventType
		{
			get
			{
				return _eventType;
			}
		}

		/// <summary>
		/// Gets the x-value.
		/// </summary>
		public double XValue
		{
			get
			{
				return _xValue;
			}
		}

		/// <summary>
		/// Returns true if event is of type Insert.
		/// </summary>
		public bool IsInsert
		{
			get
			{
				return _insertEvent == null;
			}
		}

		/// <summary>
		/// Returns true if event is of type Delete.
		/// </summary>
		public bool IsDelete
		{
			get
			{
				return _insertEvent != null;
			}
		}

		/// <summary>
		/// Returns this object insert event.
		/// </summary>
		public SweepLineEvent InsertEvent 
		{
			get
			{
				return _insertEvent; 
			}
		}

		/// <summary>
		/// Returns the delete event index.
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
		/// Gets the MonotoneChain for this object.
		/// </summary>
		public object Object
		{ 
			get
			{
				return _obj;
			}
		}

		#endregion

		#region Methods - Implementation of System.Collections.IComparable
		/// <summary>
		/// ProjectionEvents are ordered first by their x-value, and then by their eventType.
		/// It is important that Insert events are sorted before Delete events, so that
		/// items whose Insert and Delete events occur at the same x-value will be
		/// correctly handled.
		/// </summary>
		/// <param name="b">The first object to compare.</param>
		/// <returns>
		/// <list type="table">
		/// <listheader><term>Items</term><description>Descriptions</description></listheader>
		/// <item><term>Less than zero</term><description>a is less than b.</description></item>
		/// <item><term>Zero</term><description>a equals b.</description></item>
		/// <item><term>Greater than zero</term><description>a is greater than b.</description></item>
		/// </list>
		/// </returns>
		public int CompareTo(object b) 
		{
			SweepLineEvent pe = (SweepLineEvent) b;
			if ( _xValue < pe.XValue ) return  -1;
			if ( _xValue > pe.XValue ) return   1;
			if ( _eventType < pe.EventType ) return  -1;
			if ( _eventType > pe.EventType ) return   1;
			return 0;
		}
		#endregion

	}
}
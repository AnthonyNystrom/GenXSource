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

namespace Geotools.Index.IntervalTree
{
	/// <summary>
	/// Summary description for IntervalNode.
	/// </summary>
	internal class IntervalNode
	{
		private double _min;
		private double _max;
		private double _center;
		private IntervalNode _parent;
		private ArrayList _items = new ArrayList();
		private IntervalNode[] _subinterval = new IntervalNode[2];

		/**
		* subquads are numbered as follows:
		*
		*  2 | 3
		*  --+--
		*  0 | 1
		*/

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the IntervalNode class.
		/// </summary>
		public IntervalNode(IntervalNode parent, double min, double max)
		{
			_parent = parent;
			_min = min;
			_max = max;
			_center = (min + max) / 2;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the parent node.
		/// </summary>
		public IntervalNode Parent
		{
			get
			{
				return _parent;
			}
		}
		/// <summary>
		/// Gets the children nodes.
		/// </summary>
		public IntervalNode[] Children 
		{
			get
			{
				return _subinterval;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ArrayList Items
		{
			get
			{
				return _items;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="qmin"></param>
		/// <param name="qmax"></param>
		/// <returns></returns>
		public bool Overlaps( double qmin, double qmax )
		{
			if ( qmin > _max || qmax < _min ) return false;
			return true;
		} // public bool Overlaps( double qmin, double qmax )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Add( object item )
		{
			_items.Add( item );
		} // public void Add( object item )

		/// <summary>
		/// Collect all items for this node and all nodes below it
		/// which overlap the query interval.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="items"></param>
		/// <returns></returns>
		public ArrayList AddAllItemsFromOverlapping( double min, double max, ref ArrayList items )
		{
			items.AddRange( this.Items );
			for ( int i = 0; i < 2; i++ ) 
			{
				if ( _subinterval[i] != null && _subinterval[i].Overlaps( min, max ) ) 
				{
					_subinterval[i].AddAllItemsFromOverlapping( min, max, ref items );	// changes items!!!
				} // if ( _subinterval[i] != null && _subinterval[i].Overlaps( min, max ) )
			} // for ( int i = 0; i < 2; i++ )
			return items;
		} // public ArrayList AddAllItemsFromOverlapping( double min, double max, ArrayList items )

		/// <summary>
		/// Returns the interval containing the envelope.
		/// Creates the interval if it does not already exist.
		/// Note that passing a zero-size interval to this routine results in infinite recursion.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public IntervalNode GetIntervalNode( double min, double max )
		{
			int subintervalIndex = GetSubintervalIndex( min, max );
			if ( subintervalIndex != -1 ) 
			{
				// create the quad if it does not exist
				IntervalNode interval = GetSubinterval( subintervalIndex );
				// recursively search the found quad
				return interval.GetIntervalNode( min, max );
			}
			else 
			{
				return this;
			}
		} // public IntervalNode GetIntervalNode( double min, double max )

		/// <summary>
		/// Returns the smallest existing node containing the envelope.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public IntervalNode Find( double min, double max )
		{
			int subintervalIndex = GetSubintervalIndex( min, max );
			if ( subintervalIndex == -1 || _subinterval[subintervalIndex] == null )
			{
				return this;
			}
			// query lies in subnode, so search it recursively
			IntervalNode node = _subinterval[subintervalIndex];
			return node.Find( min, max );
		} // public IntervalNode Find( double min, double max )

		/// <summary>
		///  Returns the index of the subquad that wholely contains the search envelope.
		///  If none does, returns -1
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		private int GetSubintervalIndex( double min, double max )
		{
			int subintervalIndex = -1;
			if ( min > _center ) 
			{
				subintervalIndex = 1;
			}
			if ( max < _center ) 
			{
				subintervalIndex = 0;
			}
			return subintervalIndex;
		} // private int GetSubintervalIndex( double min, double max )


		/// <summary>
		/// get the subinterval for the index.
		/// If it doesn't exist, create it
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private IntervalNode GetSubinterval( int index )
		{
			if ( _subinterval[index] == null ) 
			{
				// create a new subquad in the appropriate quadrant
				double submin = 0.0;
				double submax = 0.0;

				switch ( index ) 
				{
					case 0:
						submin = _min;
						submax = _center;
						break;
					case 1:
						submin = _center;
						submax = _max;
						break;
				}

				IntervalNode interval = new IntervalNode( this, submin, submax );
				_subinterval[index] = interval;
			} // if ( _subinterval[index] == null )
			return _subinterval[index];
		} // private IntervalNode GetSubinterval( int index )
		#endregion

	} // public class IntervalNode
}

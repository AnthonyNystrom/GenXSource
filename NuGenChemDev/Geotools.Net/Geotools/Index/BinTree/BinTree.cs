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

namespace Geotools.Index.BinTree
{
	/// <summary>
	/// A BinTree (or "Binary Interval Tree")
	/// is a 1-dimensional version of a quadtree.
	/// It indexes 1-dimensional intervals (which of course may
	/// be the projection of 2-D objects on an axis).
	/// It supports range searching
	/// (where the range may be a single point).
	/// <para>
	/// This implementation does not require specifying the extent of the inserted
	/// items beforehand.  It will automatically expand to accomodate any extent
	/// of dataset.</para>
	/// <para>
	/// This index is different to the Interval Tree of Edelsbrunner
	/// or the Segment Tree of Bentley.</para>
	/// </summary>
	internal class BinTree : IEnumerable
	{

		private Root _root;
		
		
		/// <summary>
		///  Statistics
		///
		/// minExtent is the minimum extent of all items
		/// inserted into the tree so far. It is used as a heuristic value
		/// to construct non-zero extents for features with zero extent.
		/// Start with a non-zero extent, in case the first feature inserted has
		/// a zero extent in both directions.  This value may be non-optimal, but
		/// only one feature will be inserted with this value.
		/// </summary>
		private double _minExtent = 1.0;

		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the BinTree class.
		/// </summary>
		public BinTree()
		{
			_root = new Root();
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public int Depth()
		{
			if ( _root != null ) return _root.Depth();
			return 0;
		}

		public int Size()
		{
			if ( _root != null ) return _root.Size();
			return 0;
		}

		/// <summary>
		/// Compute the total number of nodes in the tree.
		/// </summary>
		/// <returns>Returns the total number of nodes in the tree.</returns>
		public int NodeSize()
		{
			if ( _root != null ) return _root.NodeSize();
			return 0;
		}

		public void Insert( Interval itemInterval, object item )
		{
			CollectStats( itemInterval );
			Interval insertInterval = EnsureExtent( itemInterval, _minExtent );
			_root.Insert( insertInterval, item );
			/* DEBUG
			int newSize = size();
			System.out.println("BinTree: size = " + newSize + "   node size = " + nodeSize());
			if (newSize <= oldSize) 
			{
				System.out.println("Lost item!");
				root.insert(insertInterval, item);
				System.out.println("reinsertion size = " + size());
			}*/
		}

		public IEnumerator GetEnumerator()
		{
			ArrayList foundItems = new ArrayList();
			_root.AddAllItems( foundItems );
			return foundItems.GetEnumerator();
		}

		public ArrayList Query(double x)
		{
			return Query( new Interval(x, x) );
		}

		/// <summary>
		/// Min and max may be the same value.
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public ArrayList Query( Interval interval )
		{
			// the items that are matched are all items in intervals
			// which overlap the query interval
			ArrayList foundItems = new ArrayList();
			Query( interval, foundItems );
			return foundItems;
		}

		public void Query(Interval interval, ArrayList foundItems)
		{
			_root.AddAllItemsFromOverlapping( interval, foundItems );
		}

		private void CollectStats( Interval interval )
		{
			double del = interval.GetWidth();
			if (del < _minExtent && del > 0.0)
			{
				_minExtent = del;
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Ensure that the Interval for the inserted item has non-zero extents.
		/// Use the current minExtent to pad it, if necessary.
		/// </summary>
		/// <param name="itemInterval"></param>
		/// <param name="minExtent"></param>
		/// <returns></returns>
		public static Interval EnsureExtent( Interval itemInterval, double minExtent )
		{
			double min = itemInterval.Min;
			double max = itemInterval.Max;
			// has a non-zero extent
			if (min != max) return itemInterval;

			// pad extent
			if (min == max) 
			{
				min = min - minExtent / 2.0;
				max = min + minExtent / 2.0;
			}
			return new Interval(min, max);
		}
		#endregion
	}
}

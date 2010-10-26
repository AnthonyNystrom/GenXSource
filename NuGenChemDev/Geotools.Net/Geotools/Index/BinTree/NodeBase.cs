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
	/// Summary description for NodeBase.
	/// </summary>
	internal abstract class NodeBase
	{
		protected ArrayList _items = new ArrayList();
		/**
		 * subnodes are numbered as follows:
		 *
		 *  0 | 1
		 */
		protected Node[] _subnode = new Node[2];

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the NodeBase class.
		/// </summary>
		public NodeBase() 
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		public ArrayList GetItems() 
		{ 
			return _items; 
		}

		public void Add( object item )
		{
			_items.Add( item );
		}

		public ArrayList AddAllItems( ArrayList items )
		{
			items.AddRange( _items );
			for ( int i = 0; i < 2; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					_subnode[i].AddAllItems( items );
				}
			}
			return items;
		}

		protected abstract bool IsSearchMatch( Interval interval );

		public ArrayList AddAllItemsFromOverlapping( Interval interval, ArrayList resultItems )
		{
			if ( !IsSearchMatch( interval ) )
			{
				return _items;
			}
			resultItems.AddRange( _items );

			for (int i = 0; i < 2; i++) 
			{
				if ( _subnode[i] != null ) 
				{
					_subnode[i].AddAllItemsFromOverlapping( interval, resultItems );
				}
			}
			return _items;
		}

		public int Depth()
		{
			int maxSubDepth = 0;
			for ( int i = 0; i < 2; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					int sqd = _subnode[i].Depth();
					if ( sqd > maxSubDepth )
						maxSubDepth = sqd;
				}
			}
			return maxSubDepth + 1;
		}

		public int Size()
		{
			int subSize = 0;
			for ( int i = 0; i < 2; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					subSize += _subnode[i].Size();
				}
			}
			return subSize + _items.Count;
		}

		public int NodeSize()
		{
			int subSize = 0;
			for ( int i = 0; i < 2; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					subSize += _subnode[i].NodeSize();
				}
			}
			return subSize + 1;
		}

		#endregion

		#region Static Methods
		/// <summary>
		/// Returns the index of the subnode that wholely contains the given interval.
		/// If none does, returns -1.
		/// </summary>
		/// <param name="interval"></param>
		/// <param name="centre"></param>
		/// <returns></returns>
		public static int GetSubnodeIndex( Interval interval, double centre )
		{
			int subnodeIndex = -1;
			if (interval.Min >= centre) subnodeIndex = 1;
			if (interval.Max <= centre) subnodeIndex = 0;
			return subnodeIndex;
		}

		#endregion





	}
}

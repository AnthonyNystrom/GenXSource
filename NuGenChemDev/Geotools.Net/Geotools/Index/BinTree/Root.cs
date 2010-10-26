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
using System.Diagnostics;
using Geotools.Index.Quadtree;
#endregion

namespace Geotools.Index.BinTree
{
	/// <summary>
	/// Summary description for Root.
	/// </summary>
	internal class Root : NodeBase
	{
		// the singleton root node is centred at the origin.
		private static double _origin = 0.0;
		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the Root class.
		/// </summary>
		public Root()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Insert an item into the tree this is the root of.
		/// </summary>
		/// <param name="itemInterval"></param>
		/// <param name="item"></param>
		public void Insert( Interval itemInterval, object item )
		{
			int index = GetSubnodeIndex( itemInterval, _origin );
			// if index is -1, itemEnv must contain the origin.
			if ( index == -1 ) 
			{
				Add( item );
				return;
			}
			// the item must be contained in one interval, so insert it into the
			// tree for that interval (which may not yet exist)
			Node node = _subnode[index];

			//  If the subnode doesn't exist or this item is not contained in it,
			//  have to expand the tree upward to contain the item.
			if ( node == null || !node.Interval.Contains(itemInterval) ) 
			{
				Node largerNode = Node.CreateExpanded( node, itemInterval );
				_subnode[index] = largerNode;
			}

			// At this point we have a subnode which exists and must contain
			// contains the env for the item.  Insert the item into the tree.
			InsertContained( _subnode[index], itemInterval, item );
		}

		/// <summary>
		/// Insert an item which is known to be contained in the tree rooted at the given Noed.
		/// Lower levels of the tree will be created it necessary to hold the item.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="itemInterval"></param>
		/// <param name="item"></param>
		private void InsertContained( Node tree, Interval itemInterval, object item )
		{
			Debug.Assert( tree.Interval.Contains( itemInterval) );

			// Do NOT create a new node for zero-area intervals - this would lead
			// to infinite recursion. Instead, use a heuristic of simply returning
			// the smallest existing node containing the query
			bool isZeroArea = IntervalSize.IsZeroWidth( itemInterval.Min, itemInterval.Max );
			NodeBase node;
			if ( isZeroArea )
			{
				node = tree.Find( itemInterval );
			}
			else
			{
				node = tree.GetNode( itemInterval );
			}
			node.Add( item );
		}

		/// <summary>
		/// The root node matches all searches.
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		protected override bool IsSearchMatch( Interval interval )
		{
			return true;
		}
		#endregion


	}
}

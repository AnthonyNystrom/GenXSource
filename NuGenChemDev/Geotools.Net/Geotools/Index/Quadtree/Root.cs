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
using Geotools.Geometries;
#endregion

namespace Geotools.Index.Quadtree
{
	
	/// <summary>
	/// QuadRoot is the root of a single Quadtree.  It is centred at the origin,
	/// and does not have a defined extent.
	/// </summary>
	internal class Root : NodeBase
	{

		// the singleton root quad is centred at the origin.
		private static Coordinate _origin = new Coordinate(0.0, 0.0);

		public Root()
		{
		}

		/// <summary>
		/// Insert an item into the quadtree this is the root of.
		/// </summary>
		/// <param name="itemEnv"></param>
		/// <param name="item"></param>
		public void Insert(Envelope itemEnv, object item)
		{
			int index = GetSubnodeIndex(itemEnv, _origin);
			// if index is -1, itemEnv must cross the X or Y axis.
			if (index == -1) 
			{
				Add( item );
				return;
			}

			// the item must be contained in one quadrant, so insert it into the
			// tree for that quadrant (which may not yet exist)
			Node node = _subnode[index];

			//  If the subquad doesn't exist or this item is not contained in it,
			//  have to expand the tree upward to contain the item.
			if ( node == null || ! node.Envelope.Contains( itemEnv ) ) 
			{
				Node largerNode = Node.CreateExpanded( node, itemEnv );
				_subnode[index] = largerNode;
			}
	
			 // At this point we have a subquad which exists and must contain
			 // contains the env for the item.  Insert the item into the tree.
			InsertContained( _subnode[index], itemEnv, item );
			//System.out.println("depth = " + root.depth() + " size = " + root.size());
			//System.out.println(" size = " + size());
		}

	
		/// <summary>
		/// insert an item which is known to be contained in the tree rooted at
		/// the given QuadNode root.  Lower levels of the tree will be created
		/// if necessary to hold the item.
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="itemEnv"></param>
		/// <param name="item"></param>
		private void InsertContained(Node tree, Envelope itemEnv, Object item)
		{
			if ( !tree.Envelope.Contains( itemEnv ) )
			{
				throw new TopologyException("Node envelope must contain item envelope");
			}

			// Do NOT create a new quad for zero-area envelopes - this would lead
			// to infinite recursion. Instead, use a heuristic of simply returning
			// the smallest existing quad containing the query
			bool isZeroX = IntervalSize.IsZeroWidth( itemEnv.MinX, itemEnv.MaxX );
			bool isZeroY = IntervalSize.IsZeroWidth( itemEnv.MinX, itemEnv.MaxX );
			NodeBase node;
			if (isZeroX || isZeroY)
			{
				node = tree.Find(itemEnv);
			}
			else
			{
				node = tree.GetNode(itemEnv);
			}
			node.Add(item);
		}

		protected override bool IsSearchMatch(Envelope searchEnv)
		{
			return true;
		}

	}
}

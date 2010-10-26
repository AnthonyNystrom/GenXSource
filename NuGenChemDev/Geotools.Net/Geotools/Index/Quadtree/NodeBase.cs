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
using Geotools.Geometries;
#endregion

namespace Geotools.Index.Quadtree
{
	internal abstract class NodeBase 
	{

		//<<TODO:REFACTOR?>> Several classes in the various tree packages have the
		//same name and duplicate code. This suggests that there should be a generic
		//tree package containing the code that is duplicated, perhaps in abstract
		//base classes. [Jon Aquino]

		//<<TODO:RENAME?>> This little class hierarchy has some naming/conceptual
		//problems. A root node is conceptually a kind of node, yet a Root is not a Node.
		//NodeBase begs to be called BaseNode, but not all BaseNodes would be Nodes
		//(for example, Root). [Jon Aquino]

		//DEBUG private static int itemCount = 0;  // debugging
		/**
		 * Returns the index of the subquad that wholly contains the given envelope.
		 * If none does, returns -1.
		 */
		protected ArrayList _items = new ArrayList();

		/**
		 * subquads are numbered as follows:
		 *  2 | 3
		 *  --+--
		 *  0 | 1
		 */
		protected Node[] _subnode = new Node[4];


		#region Constructors
		public NodeBase() 
		{
		}
		#endregion

		#region Methods
		public ArrayList GetItems()
		{
			return _items; 
		}

		public void Add(object item)
		{
			_items.Add(item);
			//DEBUG itemCount++;
			//DEBUG System.out.print(itemCount);
		}

		//<<TODO:RENAME?>> Sounds like this method adds resultItems to items
		//(like List#addAll). Perhaps it should be renamed to "addAllItemsTo" [Jon Aquino]
		public ArrayList AddAllItems( ArrayList resultItems )
		{
			//<<TODO:ASSERT?>> Can we assert that this node cannot have both items
			//and subnodes? [Jon Aquino]
			resultItems.AddRange( this._items );
			for ( int i = 0; i < 4; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					_subnode[i].AddAllItems( resultItems );
				}
			}
			return resultItems;
		}

		protected abstract bool IsSearchMatch(Envelope searchEnv);

		public void AddAllItemsFromOverlapping( Envelope searchEnv, ArrayList resultItems )
		{
			if ( !IsSearchMatch(searchEnv) )
				return;

			//<<TODO:ASSERT?>> Can we assert that this node cannot have both items
			//and subnodes? [Jon Aquino]
			resultItems.AddRange( _items );

			for (int i = 0; i < 4; i++) 
			{
				if ( _subnode[i] != null ) 
				{
					_subnode[i].AddAllItemsFromOverlapping(searchEnv, resultItems);
				}
			}
		}

		//<<TODO:RENAME?>> In Samet's terminology, I think what we're returning here is
		//actually level+1 rather than depth. (See p. 4 of his book) [Jon Aquino]
		public int Depth()
		{
			int maxSubDepth = 0;
			for ( int i = 0; i < 4; i++ ) 
			{
				if ( _subnode[i] != null ) 
				{
					int sqd = _subnode[i].Depth();
					if ( sqd > maxSubDepth )
					{
						maxSubDepth = sqd;
					}
				}
			}
			return maxSubDepth + 1;
		}

		//<<TODO:RENAME?>> "size" is a bit generic. How about "itemCount"? [Jon Aquino]
		public int Size()
		{
			int subSize = 0;
			for (int i = 0; i < 4; i++) 
			{
				if ( _subnode[i] != null ) 
				{
					subSize += _subnode[i].Size();
				}
			}
			return subSize + _items.Count;
		}

		//<<TODO:RENAME?>> The Java Language Specification recommends that "Methods to
		//get and set an attribute that might be thought of as a variable V should be
		//named getV and setV" (6.8.3). Perhaps this and other methods should be
		//renamed to "get..."? [Jon Aquino]
		public int NodeCount()
		{
			int subSize = 0;
			for (int i = 0; i < 4; i++) 
			{
				if ( _subnode[i] != null ) 
				{
					subSize += _subnode[i].Size();
				}
			}
			return subSize + 1;
		}
		#endregion

		#region Static Methods
		public static int GetSubnodeIndex( Envelope env, Coordinate centre )
		{
			int subnodeIndex = -1;
			if ( env.MinX >= centre.X ) 
			{
				if ( env.MinY >= centre.Y ) subnodeIndex = 3;
				if ( env.MaxY <= centre.Y ) subnodeIndex = 1;
			}
			if ( env.MaxX <= centre.X ) 
			{
				if ( env.MinY >= centre.Y ) subnodeIndex = 2;
				if ( env.MaxY <= centre.Y) subnodeIndex = 0;
			}
			return subnodeIndex;
		}

		#endregion

	}
}

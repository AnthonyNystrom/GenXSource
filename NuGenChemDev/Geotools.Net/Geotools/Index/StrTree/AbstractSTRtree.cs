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
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.Index.STRTree
{
	/// <summary>
	/// Base class for STRtree and SIRtree. STR-packed R-trees are described in:
	/// P. Rigaux, Michel Scholl and Agnes Voisard. Spatial Databases With
	/// Application To GIS. Morgan Kaufmann, San Francisco, 2002.
	/// </summary>
	internal abstract class AbstractSTRtree
	{
		protected AbstractNode _root;

		private bool _built = false;
		private ArrayList _itemBoundables = new ArrayList();
		private int _nodeCapacity;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the AbstractSTRtree class.
		/// </summary>
		public AbstractSTRtree(int nodeCapacity) 
		{
			//Assert.isTrue(nodeCapacity > 1, "Node capacity must be greater than 1");
			if (nodeCapacity<=1)
			{
				throw new InvalidOperationException("Node capacity must be greater than 1.");
			}
			this._nodeCapacity = nodeCapacity;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public void Build() 
		{
			//Assert.isTrue(!built);
			if (!_built)
			{
				throw new InvalidOperationException("Needs to be built.");
			}
			_root = _itemBoundables.Count <= 0
				?CreateNode(0)
				:CreateHigherLevels(_itemBoundables, -1);
			_built = true;
		}

		public void CheckConsistency() 
		{
			if (!_built)
			{
				Build(); 
			}
			ArrayList itemBoundablesInTree = BoundablesAtLevel(-1);
			if ( itemBoundablesInTree.Count != _itemBoundables.Count )
			{
				throw new TopologyException("Items not of same size.");
			}
		}

		protected abstract AbstractNode CreateNode(int level);

		/// <summary>
		/// Sorts the childBoundables then divides them into groups of size M, where
		/// M is the node capacity.
		/// </summary>
		/// <param name="childBoundables"></param>
		/// <param name="newLevel"></param>
		/// <returns></returns>
		protected virtual ArrayList CreateParentBoundables(ArrayList childBoundables, int newLevel) 
		{
			//Assert.isTrue(!childBoundables.isEmpty());
			if (childBoundables.Count==0)
			{
				throw new InvalidOperationException("childBoundables is empty.");
			}
			ArrayList parentBoundables = new ArrayList();
			parentBoundables.Add(CreateNode(newLevel));
			ArrayList sortedChildBoundables = new ArrayList(childBoundables);
			sortedChildBoundables.Sort( GetComparator() );
			//for (Iterator i = sortedChildBoundables.iterator(); i.hasNext(); ) 
			foreach(object obj in sortedChildBoundables)
			{
				IBoundable childBoundable = (IBoundable) obj;
				if (LastNode(parentBoundables).GetChildBoundables().Count == _nodeCapacity) 
				{
					parentBoundables.Add(CreateNode(newLevel));
				}
				LastNode(parentBoundables).AddChildBoundable(childBoundable);
			}
			return parentBoundables;
		}

		protected abstract IComparer GetComparator();

		protected AbstractNode LastNode(ArrayList nodes) 
		{
			return (AbstractNode) nodes[nodes.Count - 1];
		}
		protected int CompareDoubles(double a, double b) 
		{
			return a > b ? 1
				: a < b ? -1
				: 0;
		}

   
		/// <summary>
		/// Creates the levels higher than the given level.
		/// </summary>
		/// <param name="boundablesOfALevel">The level to build on.</param>
		/// <param name="level">The level of the Boundables, or -1 if the boundables are item boundables (that is, below level 0).</param>
		/// <returns>The root, which may be a ParentNode or a LeafNode.</returns>
		private AbstractNode CreateHigherLevels(ArrayList boundablesOfALevel, int level) 
		{
			//Assert.isTrue(!boundablesOfALevel.isEmpty());
			if (boundablesOfALevel.Count==0)
			{
				throw new InvalidOperationException("boundablesOfALevel is empty.");
			}
			ArrayList parentBoundables = CreateParentBoundables(boundablesOfALevel, level + 1);
			if (parentBoundables.Count == 1) 
			{
				return (AbstractNode) parentBoundables[0];
			}
			return CreateHigherLevels(parentBoundables, level + 1);
		}
		protected AbstractNode getRoot() { return _root; }
		public int GetNodeCapacity() { return _nodeCapacity; }

		protected virtual void Insert(object bounds, object item) 
		{
			//Assert.isTrue(!built, "Cannot insert items into an STR packed R-tree after it has been built.");
			if (_built)
			{
				throw new InvalidOperationException("Cannot insert items into an STR packed R-tree after it has been built.");
			}
			_itemBoundables.Add(new ItemBoundable(bounds, item));
		}

		/// <summary>
		/// Also builds the tree, if necessary.
		/// </summary>
		/// <param name="searchBounds"></param>
		/// <returns></returns>
		protected ArrayList Query(object searchBounds) 
		{
			if (!_built) { Build(); }
			ArrayList matches = new ArrayList();
			if (_itemBoundables.Count==0) 
			{
				//Assert.isTrue(root.getBounds() == null);
				if (_root.GetBounds() != null)
				{
					throw new InvalidOperationException("");
				}
				return matches;
			}
			if (GetIntersectsOp().Intersects(_root.GetBounds(), searchBounds)) 
			{
				Query(searchBounds, _root, matches);
			}
			return matches;
		}

		protected abstract IIntersectsOp GetIntersectsOp();

		private void Query(object searchBounds, AbstractNode node, ArrayList matches) 
		{
			//for (Iterator i = node.getChildBoundables().iterator(); i.hasNext(); ) 
			foreach(object obj in node.GetChildBoundables())
			{
				IBoundable childBoundable = (IBoundable) obj;
				if (!GetIntersectsOp().Intersects(childBoundable.GetBounds(), searchBounds)) 
				{
					continue;
				}
				if (childBoundable is AbstractNode) 
				{
					Query(searchBounds, (AbstractNode) childBoundable, matches);
				}
				else if (childBoundable is ItemBoundable) 
				{
					matches.Add(((ItemBoundable)childBoundable).GetItem());
				}
				else 
				{
						 //Assert.shouldNeverReachHere();
					throw new InvalidOperationException("Should never reach here.");
				}
			}
		}
		protected ArrayList BoundablesAtLevel(int level) 
		{
			ArrayList boundables = new ArrayList();
			BoundablesAtLevel(level, _root, boundables);
			return boundables;
		}

		/**
   * @param level -1 to get items
   */
		private void BoundablesAtLevel(int level, AbstractNode top, ArrayList boundables) 
		{
			//Assert.isTrue(level > -2);
			if (level <= -2)
			{
				throw new InvalidOperationException();
			}
			if (top.GetLevel() == level) 
			{
				boundables.Add(top);
				return;
			}
			//for (Iterator i = top.GetChildBoundables().iterator(); i.hasNext(); ) 
			foreach(object obj in top.GetChildBoundables())
			{
				IBoundable boundable = (IBoundable) obj;
				if (boundable is AbstractNode) 
				{
					BoundablesAtLevel(level, (AbstractNode)boundable, boundables);
				}
				else 
				{
					//Assert.isTrue(boundable is ItemBoundable);
					if (!(boundable is ItemBoundable))
					{
						throw new InvalidOperationException();
					}
					if (level == -1) 
					{
						boundables.Add(boundable);
					}
				}
			}
			return;
		}
		#endregion

	}
}

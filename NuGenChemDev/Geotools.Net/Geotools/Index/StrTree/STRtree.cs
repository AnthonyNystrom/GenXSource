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

namespace Geotools.Index.STRTree
{
	/// <summary>
	/// 
	/// An R-tree created using the Sort-Tile-Recursive (STR) algorithm, described
	/// in: P. Rigaux, Michel Scholl and Agnes Voisard. Spatial Databases With
	/// Application To GIS. Morgan Kaufmann, San Francisco, 2002. 
	/// </summary>
	/// <remarks>
	/// <para>The STR packed R-tree is simple to implement and maximizes space
	/// utilization; that is, as many leaves as possible are filled to capacity.
	/// Overlap between nodes is far less than in a basic R-tree. However, once the
	/// tree has been built (explicitly or on the first call to #query), items may
	/// not be added or removed. 
	/// </para>
	/// <para>
	/// This implementation is based on Rectangles rather than Nodes, because the
	/// STR algorithm operates on both nodes and items, both of which are treated
	/// here as Rectangles (using the Composite design pattern). [Jon Aquino]</para>
	/// </remarks> 

	internal class STRtree : AbstractSTRtree, ISpatialIndex
	{
		#region Internal Classes
		internal class XComparator : IComparer
		{
			STRtree _parent;
			public XComparator(STRtree parent)
			{
				_parent = parent;
			}
			public int Compare(object o1, object o2) 
			{
				return _parent.CompareDoubles(
					_parent.CentreX((Envelope)((IBoundable)o1).GetBounds()),
					_parent.CentreX((Envelope)((IBoundable)o2).GetBounds()));
			}
		}
		internal class YComparator : IComparer
		{
			STRtree _parent;
			public YComparator(STRtree parent)
			{
				_parent = parent;
			}
			public int Compare(object o1, object o2) 
			{
				return _parent.CompareDoubles(
					_parent.CentreY((Envelope)((IBoundable)o1).GetBounds()),
					_parent.CentreY((Envelope)((IBoundable)o2).GetBounds()));
			}
		}
		internal class IntersectsOp : IIntersectsOp
		{
			public bool Intersects(object aBounds, object bBounds) 
			{
				return ((Envelope)aBounds).Intersects((Envelope)bBounds);
			}
		}
		internal class AnonClass : AbstractNode
		{
			STRtree _parent;
			public AnonClass(STRtree parent, int level):base(level)
			{
				_parent=parent;
			}
			protected override object ComputeBounds() 
			{
				Envelope bounds = null;
				//for (Iterator i = GetChildBoundables().iterator(); i.hasNext(); ) 
				foreach(object obj in GetChildBoundables())
				{
					IBoundable childBoundable = (IBoundable) obj;
					if (bounds == null) 
					{
						bounds = new Envelope((Envelope)childBoundable.GetBounds());
					}
					else 
					{
						bounds.ExpandToInclude((Envelope)childBoundable.GetBounds());
					}
				}
				return bounds;
			}
		}
		#endregion

		private XComparator _xComparator;
		private YComparator _yComparator;
		private IIntersectsOp _intersectsOp = new IntersectsOp(); 

		#region Constructors
		
		public STRtree() : this(10)
		{
			
		}

		public STRtree(int nodeCapacity): base(nodeCapacity)
		{
			_xComparator = new XComparator(this);
			_yComparator = new YComparator(this);
			
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		private double CentreX(Envelope e) 
		{
			return Avg( e.MinX, e.MaxX );
		}

		private double Avg(double a, double b) 
		{
			return (a + b) / 2d; 
		}

		private double CentreY(Envelope e) 
		{
			return Avg( e.MinY, e.MaxY );
		}
	
		protected override IComparer GetComparator() 
		{
			return _yComparator;
		}
	
		/// <summary>
		/// Creates the parent level for the given child level. First, orders the items
		/// by the x-values of the midpoints, and groups them into vertical slices.
		/// For each slice, orders the items by the y-values of the midpoints, and
		///  group them into runs of size M (the node capacity). For each run, creates
		/// a new (parent) node.
		/// </summary>
		/// <param name="childBoundables"></param>
		/// <param name="newLevel"></param>
		/// <returns></returns>
		protected override ArrayList CreateParentBoundables(ArrayList childBoundables, int newLevel) 
		{
			//Assert.isTrue(!childBoundables.isEmpty());
			if (childBoundables.Count==0)
			{
				throw new InvalidOperationException();
			}
			int minLeafCount = (int) Math.Ceiling((childBoundables.Count / (double) GetNodeCapacity()));
			ArrayList sortedChildBoundables = new ArrayList(childBoundables);
			//Array.Sort(sortedChildBoundables, _xComparator);
			sortedChildBoundables.Sort(_xComparator);
			ArrayList[] verticalSlices = VerticalSlices(sortedChildBoundables,
				(int) Math.Ceiling(Math.Sqrt(minLeafCount)));
			return CreateParentBoundablesFromVerticalSlices(verticalSlices, newLevel);
		}

		private ArrayList CreateParentBoundablesFromVerticalSlices(ArrayList[] verticalSlices, int newLevel) 
		{
			//Assert.isTrue(verticalSlices.length > 0);
			if (verticalSlices.Length==0)
			{
				throw new InvalidOperationException();
			}
			ArrayList parentBoundables = new ArrayList();
			for (int i = 0; i < verticalSlices.Length; i++) 
			{
				parentBoundables.AddRange(
					CreateParentBoundablesFromVerticalSlice(verticalSlices[i], newLevel));
			}
			return parentBoundables;
		}

		protected ArrayList CreateParentBoundablesFromVerticalSlice(ArrayList childBoundables, int newLevel) 
		{
			return base.CreateParentBoundables(childBoundables, newLevel);
		}

		protected ArrayList[] VerticalSlices(ArrayList childBoundables, int sliceCount) 
		{
			int sliceCapacity = (int) Math.Ceiling(childBoundables.Count / (double) sliceCount);
			ArrayList[] slices = new ArrayList[sliceCount];
			IEnumerator i = childBoundables.GetEnumerator();
			for (int j = 0; j < sliceCount; j++) 
			{
				slices[j] = new ArrayList();
				int boundablesAddedToSlice = 0;
				while (i.MoveNext() && boundablesAddedToSlice < sliceCapacity) 
				{
					IBoundable childBoundable = (IBoundable) i.Current;
					slices[j].Add(childBoundable);
					boundablesAddedToSlice++;
				}
			}
			return slices;
		}

		protected override AbstractNode CreateNode(int level) 
		{
			return new AnonClass(this,level);
		}

		protected override IIntersectsOp GetIntersectsOp() 
		{
			return _intersectsOp;
		}

		public void Insert( Envelope itemEnv, object item ) 
		{
			if ( itemEnv.IsNull() ) { return; }
			base.Insert( itemEnv, item );
		}

		public ArrayList Query(Envelope searchEnv) 
		{
			return base.Query(searchEnv);
		}

		#endregion

	}
}

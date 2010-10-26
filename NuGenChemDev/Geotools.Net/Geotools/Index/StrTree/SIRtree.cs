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

namespace Geotools.Index.STRTree
{
	/// <summary>
	/// Summary description for SIRtree.
	/// </summary>
	internal class SIRtree : AbstractSTRtree
	{
		internal class IntersectsOp : IIntersectsOp
		{
			public bool Intersects(object aBounds, object bBounds) 
			{
				return ((Interval)aBounds).Intersects((Interval)bBounds);
			}
		}

		internal class Comparator : IComparer
		{
			SIRtree _parent;
			public Comparator(SIRtree parent)
			{
				_parent = parent;
			}

			public int Compare(object o1, object o2) 
			{
				return _parent.CompareDoubles(
					((Interval)((IBoundable)o1).GetBounds()).GetCentre(),
					((Interval)((IBoundable)o2).GetBounds()).GetCentre());
			}
		}

		internal class Computer : AbstractNode
		{
			SIRtree _parent;
			public Computer(int level, SIRtree parent): base(level)
			{
				_parent = parent;
			}
			protected override object ComputeBounds() 
			{
				Interval bounds = null;
				foreach(object obj in GetChildBoundables() ) 
				{
					IBoundable childBoundable = (IBoundable) obj;
					if (bounds == null) 
					{
						bounds = new Interval((Interval)childBoundable.GetBounds());
					}
					else 
					{
						bounds.ExpandToInclude((Interval)childBoundable.GetBounds());
					}
				}
				return bounds;
			}
		}

		private Comparator _comparator =null;
		private IIntersectsOp _intersectsOp = new IntersectsOp();
		
		public SIRtree() : this(10)
		{
		
		}
		public SIRtree(int nodeCapacity) :base(nodeCapacity)
		{
			_comparator= new Comparator(this) ;
		}
		
		protected override IComparer GetComparator() 
		{
			return _comparator;
		}

		protected override AbstractNode CreateNode(int level) 
		{
			return new Computer(level, this);
		}

		
		public void Insert(double x1, double x2, object item) 
		{
			base.Insert(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)), item);
		}
		public ArrayList Query(double x) 
		{
			return Query(x, x);
		}
	
		/// <summary>
		/// min and max may be the same value
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <returns></returns>
		public ArrayList Query(double x1, double x2) 
		{
			return base.Query(new Interval(Math.Min(x1, x2), Math.Max(x1, x2)));
		}
		protected override IIntersectsOp GetIntersectsOp() 
		{
			return _intersectsOp;
		}
	}
}

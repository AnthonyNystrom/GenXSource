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
	/// A node of the STR tree. A leaf node may not have child nodes, but may have child boundables: ItemBoundables.
	/// </summary>
	internal abstract class AbstractNode : IBoundable 
	{
		private ArrayList _childBoundables = new ArrayList();
		private object _bounds = null;
		private int _level;

		#region Constructors
		public AbstractNode(int level) 
		{
			this._level = level;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public ArrayList GetChildBoundables() 
		{
			return _childBoundables;
		}
		protected abstract object ComputeBounds();

		public object GetBounds() 
		{
			if (_bounds == null) 
			{
				_bounds = ComputeBounds();
			}
			return _bounds;
		}

		public int GetLevel() 
		{
			return _level;
		}

		/// <summary>
		/// childBoundable  either a Node or an ItemBoundable
		/// </summary>
		/// <param name="childBoundable"></param>
		public void AddChildBoundable(IBoundable childBoundable) 
		{
			//Assert.isTrue(bounds == null);
			if (_bounds==null)
			{
				throw new InvalidOperationException("Bounds cannot be null.");
			}
			_childBoundables.Add(childBoundable);
		}
		#endregion

	}
}

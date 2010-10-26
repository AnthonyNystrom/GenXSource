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

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for NodeFactory.
	/// </summary>
	internal class NodeFactory
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the NodeFactory class.
		/// </summary>
		public NodeFactory()
		{
		}
		#endregion

		

		#region Methods
		/// <summary>
		/// Returns the newly created coord object.
		/// </summary>
		/// <param name="coord"></param>
		/// <returns></returns>
		public virtual Node CreateNode( Coordinate coord )		//TODO: should this be a static method??
		{
			return new Node( coord, null );
		}

		#endregion

	}
}

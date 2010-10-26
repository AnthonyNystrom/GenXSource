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
using Geotools.Graph;
using Geotools.Geometries;
using Geotools.Algorithms;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for MinimalEdgeRing.
	/// </summary>
	internal class MinimalEdgeRing: EdgeRing
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MinimalEdgeRing class.
		/// A MinimalEdgeRing is a ring of edges with the property that no node
		/// has degree greater than 2.  These are the form of rings required
		/// to represent polygons under the OGC SFS spatial data model.
		/// </summary>
		public MinimalEdgeRing(DirectedEdge start, GeometryFactory geometryFactory, CGAlgorithms cga): base( start, geometryFactory, cga )
		{
		}
		#endregion

		#region Properties

		#endregion

		#region Methods
		public override void SetEdgeRing( DirectedEdge de, EdgeRing er )
		{
			de.MinEdgeRing = er;
		}		
		public override DirectedEdge GetNext( DirectedEdge de )
		{
			return de.NextMin;
		}
		#endregion

	} // public class MinimalEdgeRing: EdgeRing
}

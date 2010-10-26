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
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for MaximalEdgeRing.
	/// </summary>
	internal class MaximalEdgeRing: EdgeRing
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MaximalEdgeRing class.
		/// A MaximalEdgeRing is a ring of edges which may contain nodes of degree > 2.
		/// A MaximalEdgeRing may represent two different spatial entities:
		/// &lt;ul&gt;
		/// &lt;li&gt;a single polygon possibly containing inversions (if the ring is oriented CW)
		/// &lt;li&gt;a single hole possibly containing exversions (if the ring is oriented CCW)
		/// &lt;/ul&gt;
		/// If the MaximalEdgeRing represents a polygon,
		/// the interior of the polygon is strongly connected.
		/// These are the form of rings used to define polygons under some spatial data models.
		/// However, under the OGC SFS model, {@link MinimalEdgeRings} are required.
		/// A MaximalEdgeRing can be converted to a list of MinimalEdgeRings using the
		/// { BuildMinimalRings() } method.
		/// </summary>
		public MaximalEdgeRing( DirectedEdge start, GeometryFactory geometryFactory, CGAlgorithms cga ) : base( start, geometryFactory, cga )
		{
		}
		#endregion

		#region Properties

		public override DirectedEdge GetNext( DirectedEdge de )
		{
			return de.Next;
		}
		public override void SetEdgeRing( DirectedEdge de, EdgeRing er )
		{
			de.EdgeRing = er;
		}

		#endregion

		#region Methods

		 // For all nodes in this EdgeRing,
		 // link the DirectedEdges at the node to form minimalEdgeRings
		public void LinkDirectedEdgesForMinimalEdgeRings()
		{
			DirectedEdge de = _startDe;
			do 
			{
				Node node = de.Node;
				( (DirectedEdgeStar) node.Edges ).LinkMinimalDirectedEdges( this );
				de = de.Next;
			} while ( de != _startDe ); // uses proper comparison method (equals)
		} // public void LinkDirectedEdgesForMinimalEdgeRings()

		public ArrayList BuildMinimalRings()
		{
			ArrayList minEdgeRings = new ArrayList();
			DirectedEdge de = _startDe;
			do 
			{
				if ( de.MinEdgeRing == null  ) 
				{
					EdgeRing minEr = new MinimalEdgeRing( de, _geometryFactory, _cga ) ;
					minEdgeRings.Add( minEr );
				}
				de = de.Next;
			} while ( de != _startDe );
			return minEdgeRings;
		} // public ArrayList BuildMinimalRings()
		#endregion

	} // public class MaximalEdgeRing: EdgeRing
}

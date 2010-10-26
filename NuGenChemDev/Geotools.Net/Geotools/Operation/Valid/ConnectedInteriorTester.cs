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
using System.Collections;
using Geotools.Operation.Overlay;
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Summary description for ConnectedInteriorTester.
	/// </summary>
	internal class ConnectedInteriorTester
	{
		
		private GeometryFactory _geometryFactory = new GeometryFactory();
		private CGAlgorithms _cga = new RobustCGAlgorithms();

		private GeometryGraph _geomGraph;
		// save a coordinate for any disconnected interior found
		// the coordinate will be somewhere on the ring surrounding the disconnected interior
		private Coordinate _disconnectedRingcoord;

		public ConnectedInteriorTester(GeometryGraph geomGraph)
		{
			this._geomGraph = geomGraph;
		}

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ConnectedInteriorTester class.
		/// </summary>
		public ConnectedInteriorTester()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		private void  InitBlock()
		{
			_geometryFactory = new GeometryFactory();
			_cga = new RobustCGAlgorithms();
		}

		public Coordinate GetCoordinate()
		{
			return _disconnectedRingcoord; 
		}

		
		public bool IsInteriorsConnected()
		{
			
			// node the edges, in case holes touch the shell
			ArrayList splitEdges = new ArrayList();
			_geomGraph.ComputeSplitEdges(splitEdges);

			// polygonize the edges
			PlanarGraph graph = new PlanarGraph(new OverlayNodeFactory());
			graph.AddEdges(splitEdges);
			SetAllEdgesInResult(graph);
			graph.LinkAllDirectedEdges();
			ArrayList edgeRings = buildEdgeRings(graph.EdgeEnds);

			/**
			 * Mark all the edges for the edgeRings corresponding to the shells
			 * of the input polygons.  Note only ONE ring gets marked for each shell.
			 */
			VisitShellInteriors(_geomGraph.Geometry, graph);

			/**
			 * If there are any unvisited shell edges
			 * (i.e. a ring which is not a hole and which has the interior
			 * of the parent area on the RHS)
			 * this means that one or more holes must have split the interior of the
			 * polygon into at least two pieces.  The polygon is thus invalid.
			 */
			return ! HasUnvisitedShellEdge(edgeRings);
			
		}
		private void SetAllEdgesInResult(PlanarGraph graph)
		{
			foreach( object obj in graph.EdgeEnds)
			{
				DirectedEdge de = (DirectedEdge) obj;
				de.InResult=true;
			}
		}


		/// <summary>
		/// for all DirectedEdges in result, form them into EdgeRings
		/// </summary>
		/// <param name="dirEdges"></param>
		/// <returns></returns>
		private ArrayList buildEdgeRings(ArrayList dirEdges)
		{
			ArrayList edgeRings = new ArrayList();
			foreach(object obj in dirEdges)
			{
				DirectedEdge de = (DirectedEdge) obj;
				// if this edge has not yet been processed
				if (de.EdgeRing == null) 
				{
					EdgeRing er = new MaximalEdgeRing(de, _geometryFactory, _cga);
					edgeRings.Add(er);
				}
			}
			return edgeRings;
		}
		
		/// <summary>
		///  Mark all the edges for the edgeRings corresponding to the shells
		///  of the input polygons.  Note only ONE ring gets marked for each shell.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="graph"></param>
		private void VisitShellInteriors(Geometry g, PlanarGraph graph)
		{
			
			if (g is Polygon) 
			{
				Polygon p = (Polygon) g;
				this.VisitInteriorRing(p.GetExteriorRing(), graph);
			}
			if (g is MultiPolygon) 
			{
				MultiPolygon mp = (MultiPolygon) g;
				for (int i = 0; i < mp.GetNumGeometries(); i++) 
				{
					Polygon p = (Polygon) mp.GetGeometryN(i);
					this.VisitInteriorRing(p.GetExteriorRing(), graph);
				}
			}
			
		}
		private void VisitInteriorRing(ILineString iring, PlanarGraph graph)
		{
			LineString ring = (LineString)iring;
			Coordinates pts = ring.GetCoordinates();
			Edge e = graph.FindEdgeInSameDirection(pts[0], pts[1]);
			DirectedEdge de = (DirectedEdge) graph.FindEdgeEnd(e);
			DirectedEdge intDe = null;
			if (de.Label.GetLocation(0, Position.Right) == Location.Interior) 
			{
				intDe = de;
			}
			else if (de.Sym.Label.GetLocation(0, Position.Right) == Location.Interior) 
			{
				intDe = de.Sym;
			}
			//Assert.isTrue(intDe != null, "unable to find dirEdge with Interior on RHS");

			VisitLinkedDirectedEdges(intDe);
			
		}
		protected void VisitLinkedDirectedEdges(DirectedEdge start)
		{
			
			DirectedEdge startDe = start;
			DirectedEdge de = start;
			//Debug.println(de);
			do 
			{
				de.Visited=true;
				de = de.Next;
				//Debug.println(de);
			} while (de != startDe);
		}
	

		/// <summary>
		/// Check if any shell ring has an unvisited edge.
		/// A shell ring is a ring which is not a hole and which has the interior
		/// of the parent area on the RHS.
		/// (Note that there may be non-hole rings with the interior on the LHS,
		/// since the interior of holes will also be polygonized into CW rings
		/// by the linkAllDirectedEdges() step)
		/// </summary>
		/// <param name="edgeRings"></param>
		/// <returns>True if there is an unvisited edge in a non-hole ring</returns>
		private bool HasUnvisitedShellEdge(ArrayList edgeRings)
		{
			
			for (int i = 0; i < edgeRings.Count; i++) 
			{
				EdgeRing er = (EdgeRing) edgeRings[i];
				if (er.IsHole) continue;
				ArrayList edges = er.Edges;
				DirectedEdge de = (DirectedEdge) edges[0];
				// don't check CW rings which are holes
				if (de.Label.GetLocation(0, Position.Right) != Location.Interior)
				{
					continue;
				}

				// must have a CW ring which surrounds the INT of the area, so check all
				// edges have been visited
				for (int j = 0; j < edges.Count; j++) 
				{
					de = (DirectedEdge) edges[j];
					//Debug.print("visted? "); Debug.println(de);
					if (! de.Visited) 
					{
						//Debug.print("not visited "); Debug.println(de);
						_disconnectedRingcoord = de.Coordinate;
						return true;
					}
				}
			}
			return false;
		}
		#endregion

	}
}

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
using Geotools.Graph.Index;
using Geotools.Operation.Overlay;
using Geotools.Utilities;
#endregion

namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// Summary description for BufferOp.
	/// </summary>
	internal class BufferOp : GeometryGraphOperation
	{
		private GeometryFactory _geomFact;
		private Geometry _resultGeom;
		private PlanarGraph _graph;
		private EdgeList _edgeList = new EdgeList();

		public static Geometry Buffer(Geometry g, double distance, int quadrantSegments)
		{
			BufferOp gBuf = new BufferOp(g);
			Geometry geomBuf = gBuf.GetResultGeometry(distance, quadrantSegments);
			return geomBuf;
		}

		public static Geometry GetBuffer(Geometry g, double distance)
		{
			
			BufferOp gBuf = new BufferOp(g);
			Geometry geomBuf = gBuf.GetResultGeometry(distance);
			return geomBuf;
		}

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the BufferOp class.
		/// </summary>
		/// <param name="g0"></param>
		public BufferOp(Geometry g0) : base(g0)
		{
			
			_graph = new PlanarGraph(new OverlayNodeFactory());
			_geomFact = new GeometryFactory( g0.PrecisionModel,	g0.GetSRID() );
		
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		

		/// <summary>
		/// Compute the change in depth as an edge is crossed from R to L
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		private static int DepthDelta(Label label)
		{
			int lLoc = label.GetLocation(0, Position.Left);
			int rLoc = label.GetLocation(0, Position.Right);
			if (lLoc == Location.Interior && rLoc == Location.Exterior)
				return 1;
			else if (lLoc == Location.Exterior && rLoc == Location.Interior)
				return -1;
			return 0;
		}

		public Geometry GetResultGeometry(double distance)
		{
			ComputeBuffer(distance,BufferLineBuilder.DefaultQuadrantSegments);
			return _resultGeom;
		}
		public Geometry GetResultGeometry(double distance, int quadrantSegments)
		{
			ComputeBuffer(distance, quadrantSegments);
			return _resultGeom;
		}

		private void ComputeBuffer(double distance, int quadrantSegments)
		{
			if (_makePrecise) 
			{
				double scale = GetArgGeometry(0).PrecisionModel.Scale;
				distance *= scale;
			}
			BufferEdgeBuilder bufEdgeBuilder = new BufferEdgeBuilder(_cga, _li, distance, _makePrecise, quadrantSegments);
			ArrayList bufferEdgeList = bufEdgeBuilder.GetEdges(GetArgGeometry(0));

			// DEBUGGING ONLY
			//WKTWriter wktWriter = new WKTWriter();
			//Debug.println("Rings: " + wktWriter.write(toLineStrings(bufferEdgeList.iterator())));

			ArrayList nodedEdges = this.NodeEdges(bufferEdgeList);
			//TESTING - node again to ensure edges are noded completely
			/*
			List nodedEdges2 = nodeEdges(nodedEdges);
			List nodedEdges3 = nodeEdges(nodedEdges2);
			List nodedEdges4 = nodeEdges(nodedEdges3);
			List nodedEdges5 = nodeEdges(nodedEdges4);
			List nodedEdges6 = nodeEdges(nodedEdges5);
		  */
			//for (Iterator i = nodedEdges.iterator(); i.hasNext(); ) 
			foreach(object obj in nodedEdges)
			{
				Edge e = (Edge) obj;
				InsertEdge(e);
			}
			ReplaceCollapsedEdges();

			// DEBUGGING ONLY
			//Debug.println("Noded: " + wktWriter.write(toLineStrings(edgeList.iterator())));

			_graph.AddEdges(_edgeList);

			ArrayList subgraphList = CreateSubgraphs();
			PolygonBuilder polyBuilder = new PolygonBuilder(_geomFact, _cga);
			BuildSubgraphs(subgraphList, polyBuilder);
			ArrayList resultPolyList = polyBuilder.GetPolygons();

			_resultGeom = ComputeGeometry(resultPolyList);
			//computeBufferLine(graph);
		}
		/**
		 * Use a GeometryGraph to node the created edges,
		 * and create split edges between the nodes
		 */
		private ArrayList NodeEdges(ArrayList edges)
		{
			// intersect edges again to ensure they are noded correctly
			GeometryGraph graph = new GeometryGraph(0, _geomFact.PrecisionModel, 0);
			//for (Iterator i = edges.iterator(); i.hasNext(); ) 
			foreach(object obj in edges)
			{
				Edge e = (Edge) obj;
				graph.AddEdge(e);
			}
			SegmentIntersector si = graph.ComputeSelfNodes(_li);
			/*
			if (si.hasProperIntersection())
			Debug.println("proper intersection found");
			else
			Debug.println("no proper intersection found");
			*/
			ArrayList newEdges = new ArrayList();
			graph.ComputeSplitEdges(newEdges);
			return newEdges;
		}
		/**
		 * Inserted edges are checked identical edge already exists.
		 * If so, the edge is not inserted, but its label is merged
		 * with the existing edge.
		 */
		protected void InsertEdge(Edge e)
		{
			//Debug.println(e);
			int foundIndex = _edgeList.FindEdgeIndex(e);
			// If an identical edge already exists, simply update its label
			if (foundIndex >= 0) 
			{
				Edge existingEdge = (Edge) _edgeList[foundIndex];
				Label existingLabel = existingEdge.Label;

				Label labelToMerge = e.Label;
				// check if new edge is in reverse direction to existing edge
				// if so, must flip the label before merging it
				if (! existingEdge.IsPointwiseEqual(e)) 
				{
					labelToMerge = new Label(e.Label);
					labelToMerge.Flip();
				}
				existingLabel.Merge(labelToMerge);

				// compute new depth delta of sum of edges
				int mergeDelta = DepthDelta(labelToMerge);
				int existingDelta = existingEdge.DepthDelta;
				int newDelta = existingDelta + mergeDelta;
				existingEdge.DepthDelta=newDelta;

				CheckDimensionalCollapse(labelToMerge, existingLabel);
				//Debug.print("new edge "); Debug.println(e);
				//Debug.print("existing "); Debug.println(existingEdge);

			}
			else 
			{   // no matching existing edge was found
				// add this new edge to the list of edges in this graph
				//e.setName(name + edges.size());
				_edgeList.Add(e);
				e.DepthDelta=DepthDelta(e.Label);
			}
		}
		/**
		 * If either of the GeometryLocations for the existing label is
		 * exactly opposite to the one in the labelToMerge,
		 * this indicates a dimensional collapse has happened.
		 * In this case, convert the label for that Geometry to a Line label
		 */
		private void CheckDimensionalCollapse(Label labelToMerge, Label existingLabel)
		{
			if (existingLabel.IsArea() && labelToMerge.IsArea()) 
			{
				for (int i = 0; i < 2; i++) 
				{
					if (! labelToMerge.IsNull(i)
						&&  labelToMerge.GetLocation(i, Position.Left)  == existingLabel.GetLocation(i, Position.Right)
						&&  labelToMerge.GetLocation(i, Position.Right) == existingLabel.GetLocation(i, Position.Left) )
					{
						existingLabel.ToLine(i);
					}
				}
			}
		}
		/**
		 * If collapsed edges are found, replace them with a new edge which is a L edge
		 */
		private void ReplaceCollapsedEdges()
		{
			ArrayList newEdges = new ArrayList();
			//for (Iterator it = edgeList.iterator(); it.hasNext(); ) 
			foreach(object obj in _edgeList)
			{
				Edge e = (Edge) obj;
				if ( e.IsCollapsed() ) 
				{
					
					_edgeList.Remove(obj);

					newEdges.Add(e.GetCollapsedEdge());
				}
			}
			_edgeList.AddRange(newEdges);
		}

		private ArrayList CreateSubgraphs()
		{
			ArrayList subgraphList = new ArrayList();
			//for (Iterator i = graph.getNodes().iterator(); i.hasNext(); ) 
			foreach(DictionaryEntry obj in _graph.Nodes)
			{
				Node node = (Node) obj.Value;
				if (! node.IsVisited) 
				{
					BufferSubgraph subgraph = new BufferSubgraph(_cga);
					subgraph.Create(node);
					subgraphList.Add(subgraph);
				}
			}
			/**
			 * Sort the subgraphs in descending order of their rightmost coordinate.
			 * This ensures that when the Polygons for the subgraphs are built,
			 * subgraphs for shells will have been built before the subgraphs for
			 * any holes they contain.
			 */
			//
			IComparer reverse = new ReverseOrder();
			object[] objarray = subgraphList.ToArray();
			Array.Sort(objarray,reverse);
			return new ArrayList(objarray);
			//Collections.Sort(subgraphList, Collections.reverseOrder());
			//return subgraphList;
		}

		private void BuildSubgraphs(ArrayList subgraphList, PolygonBuilder polyBuilder)
		{
			//for (Iterator i = subgraphList.iterator(); i.hasNext(); ) 
			foreach(object obj in subgraphList)
			{
				BufferSubgraph subgraph = (BufferSubgraph)obj;
				Coordinate p = subgraph.GetRightmostCoordinate();
				int outsideDepth = 0;
				if (polyBuilder.ContainsPoint(p))
					outsideDepth = 1;
				subgraph.ComputeDepth(outsideDepth);
				subgraph.FindResultEdges();

				polyBuilder.Add(subgraph.GetDirectedEdges(), subgraph.GetNodes());
			}
		}

		private Geometry ComputeGeometry(ArrayList resultPolyList)
		{
			return _geomFact.BuildGeometry(resultPolyList);
		}

		/**
		 * toLineStrings converts a list of Edges to LineStrings.
		 */
		private Geometry ToLineStrings(IEnumerator edges)
		{
			ArrayList geomList = new ArrayList();
			while  (edges.MoveNext()) 
			{
				Edge e = (Edge) edges.Current;
				Coordinates pts = e.Coordinates;
				LineString line = _geomFact.CreateLineString(pts);
				geomList.Add(line);
			}
			Geometry geom = _geomFact.BuildGeometry(geomList);
			return geom;
		}

		#endregion

	}
}

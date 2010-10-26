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
using Geotools.Algorithms;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// A BufferSubgraph is a connected subset of the graph of DirectedEdges and Nodes
	/// in a BufferGraph.  Its edges will generate either
	/// &lt;ul&gt;
	/// &lt;li&gt; a single polygon in the complete buffer, with zero or more holes, or
	/// &lt;li&gt; one or more connected holes
	/// &lt;/ul&gt;
	/// </summary>
	internal class BufferSubgraph : IComparable
	{
		private RightmostEdgeFinder _finder;
		private ArrayList _dirEdgeList= new ArrayList();
		private ArrayList _nodes      = new ArrayList();
		private Coordinate _rightMostCoord = null;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the BufferSubgraph class.
		/// </summary>
		/// <param name="cga"></param>
		public BufferSubgraph(CGAlgorithms cga)
		{
			_finder = new RightmostEdgeFinder(cga);
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		public ArrayList GetDirectedEdges() 
		{ 
			return _dirEdgeList; 
		}

		public ArrayList GetNodes() 
		{ 
			return _nodes; 
		}

		/// <summary>
		/// Get the rightMost coordinate in the edges of the subgraph
		/// </summary>
		/// <returns></returns>
		public Coordinate GetRightmostCoordinate()
		{
			return _rightMostCoord;
		}

		public void Create(Node node)
		{
			
			Add(node);
			_finder.FindEdge(_dirEdgeList);
			_rightMostCoord = _finder.GetCoordinate();
			
		}

		private void Add(Node node)
		{
			
			node.IsVisited=true;
			_nodes.Add(node);
			//for (Iterator i = ((DirectedEdgeStar) node.getEdges()).iterator(); i.hasNext(); )
			foreach(object obj in node.Edges)
			{
				DirectedEdge de = (DirectedEdge) obj;
				_dirEdgeList.Add(de);
				DirectedEdge sym = de.Sym;
				Node symNode = sym.Node;;
	
				 // NOTE: this is a depth-first traversal of the graph.
				 // This will cause a large depth of recursion.
				 // It might be better to do a breadth-first traversal.
				if (! symNode.IsVisited) Add(symNode);
			}
			
		}

		private void ClearVisitedEdges()
		{
			
			//for (Iterator it = dirEdgeList.iterator(); it.hasNext(); ) 
			foreach(object obj in _dirEdgeList)
			{
				DirectedEdge de = (DirectedEdge) obj;
				de.Visited=false;
			}
			
		}

		public void ComputeDepth(int outsideDepth)
		{
			
			ClearVisitedEdges();
			// find an outside edge to assign depth to
			DirectedEdge de = _finder.GetEdge();
			Node n = de.Node;
			Label label = de.Label;
			// right side of line returned by finder is on the outside
			de.SetEdgeDepths(Position.Right, outsideDepth);

			ComputeNodeDepth(n, de);
			
		}

		private void ComputeNodeDepth(Node n, DirectedEdge startEdge)
		{
			
			if (startEdge.Visited) return;

			((DirectedEdgeStar) n.Edges).ComputeDepths(startEdge);

			// copy depths to sym edges
			//for (Iterator i = ((DirectedEdgeStar) n.getEdges()).iterator(); i.hasNext(); ) 
			foreach (object obj in n.Edges)
			{
				DirectedEdge de = (DirectedEdge) obj;// i.next();
				de.Visited=true;
				DirectedEdge sym = de.Sym;
				sym.SetDepth(Position.Left, de.GetDepth(Position.Right));
				sym.SetDepth(Position.Right, de.GetDepth(Position.Left));
			}
			// propagate depth to all linked nodes via the sym edges
			// If a sym edge has been visited already, there is no need to process it further
			//for (Iterator i = ((DirectedEdgeStar) n.getEdges()).iterator(); i.hasNext(); ) 
			foreach(object obj in n.Edges)
			{
				DirectedEdge de = (DirectedEdge)obj;
				DirectedEdge sym = de.Sym;
				Node symNode = sym.Node;

				 // NOTE: this is a depth-first traversal of the graph.
				 // This will cause a large depth of recursion.
				 // It might be better to do a breadth-first traversal.
				ComputeNodeDepth(symNode, sym);
			}
			

		}
		

		/// <summary>
		/// Find all edges whose depths indicates that they are in the result area(s).
		/// Since we want polygon shells to be oriented CW, choose dirEdges with the interior 
		/// of the result on the RHS. Mark them as being in the result. Interior Area edges are
		/// the result of dimensional collapses. They do not form part of the result area boundary.
		/// </summary>
		public void FindResultEdges()
		{
			
			//for (Iterator it = dirEdgeList.iterator(); it.hasNext(); ) 
			foreach(object obj in _dirEdgeList)
			{
				DirectedEdge de = (DirectedEdge) obj;

				 // Select edges which have the EXTERIOR on the L and INTERIOR
				 // on the right.  It doesn't matter how deep the interior is.
				if (    de.GetDepth(Position.Right) >= 1
					&&  de.GetDepth(Position.Left)  == 0
					&&  ! de.IsInteriorAreaEdge) 
				{
					de.InResult=true;
					//Debug.print("in result "); Debug.println(de);
				}
			}
			
		}


		/// <summary>
		/// BufferSubgraphs are compared on the x-value of their rightmost Coordinate. This defines a partial 
		/// ordering on the graphs such that:
		/// g1 &gt;= g2 &lt;==&gt; Ring(g2) does not contain Ring(g1)
		/// where Polygon(g) is the buffer polygon that is built from g.
		/// This relationship is used to sort the BufferSubgraphs so that shells are guaranteed to
		/// be built before holes.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int CompareTo(object obj) 
		{
			
			BufferSubgraph graph = (BufferSubgraph) obj;
			if (this.GetRightmostCoordinate().X < graph.GetRightmostCoordinate().X) 
			{
				return -1;
			}
			if (this.GetRightmostCoordinate().X > graph.GetRightmostCoordinate().X) 
			{
				return 1;
			}
			return 0;
		}


		#endregion

	}
}

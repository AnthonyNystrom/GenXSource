/* ==========================================
* JGraphT : a free Java graph-theory library
* ==========================================
*
* Project Info:  http://jgrapht.sourceforge.net/
* Project Lead:  Barak Naveh (http://sourceforge.net/users/barak_naveh)
*
* (C) Copyright 2003-2004, by Barak Naveh and Contributors.
*
* This library is free software; you can redistribute it and/or modify it
* under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation; either version 2.1 of the License, or
* (at your option) any later version.
*
* This library is distributed in the hope that it will be useful, but
* WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
* or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
* License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this library; if not, write to the Free Software Foundation, Inc.,
* 59 Temple Place, Suite 330, Boston, MA 02111-1307, USA.
*/
/* ----------------
* GraphHelper.java
* ----------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   Mikael Hansen
*
* $Id: GraphHelper.java,v 1.11 2005/04/23 08:25:17 perfecthash Exp $
*
* Changes
* -------
* 10-Jul-2003 : Initial revision (BN);
* 06-Nov-2003 : Change edge sharing semantics (JVS);
*
*/
using System;
using DirectedEdge = org._3pq.jgrapht.edge.DirectedEdge;
using AsUndirectedGraph = org._3pq.jgrapht.graph.AsUndirectedGraph;
namespace org._3pq.jgrapht
{
	
	/// <summary> A collection of utilities to assist the working with graphs.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 31, 2003
	/// </since>
	public sealed class GraphHelper
	{
		private GraphHelper()
		{
		} // ensure non-instantiability.
		
		/// <summary> Creates a new edge and adds it to the specified graph similarly to the
		/// {@link Graph#addEdge(Object, Object)} method.
		/// 
		/// </summary>
		/// <param name="g">the graph for which the edge to be added.
		/// </param>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// </param>
		/// <param name="weight">weight of the edge.
		/// 
		/// </param>
		/// <returns> The newly created edge if added to the graph, otherwise
		/// <code>null</code>.
		/// 
		/// </returns>
		/// <seealso cref="Graph.addEdge(Object, Object)">
		/// </seealso>
		public static Edge addEdge(Graph g, System.Object sourceVertex, System.Object targetVertex, double weight)
		{
			EdgeFactory ef = g.EdgeFactory;
			Edge e = ef.createEdge(sourceVertex, targetVertex);
			
			// we first create the edge and set the weight to make sure that 
			// listeners will see the correct weight upon addEdge.
			e.Weight = weight;
			
			return g.addEdge(e)?e:null;
		}
		
		
		/// <summary> Adds the specified edge to the specified graph including its vertices.
		/// If any of the vertices of the specified edge are not already in the
		/// graph they are also added (before the edge is added).
		/// 
		/// </summary>
		/// <param name="g">the graph for which the specified edge to be added.
		/// </param>
		/// <param name="e">the edge to be added to the graph (including its vertices).
		/// 
		/// </param>
		/// <returns> <code>true</code> if and only if the specified edge was not
		/// already contained in the graph.
		/// </returns>
		public static bool addEdgeWithVertices(Graph g, Edge e)
		{
			g.addVertex(e.Source);
			g.addVertex(e.Target);
			
			return g.addEdge(e);
		}
		
		
		/// <summary> Adds the specified source and target vertices to the graph, if not
		/// already included, and creates a new edge and adds it to the specified
		/// graph similarly to the {@link Graph#addEdge(Object, Object)} method.
		/// 
		/// </summary>
		/// <param name="g">the graph for which the specified edge to be added.
		/// </param>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> The newly created edge if added to the graph, otherwise
		/// <code>null</code>.
		/// </returns>
		public static Edge addEdgeWithVertices(Graph g, System.Object sourceVertex, System.Object targetVertex)
		{
			g.addVertex(sourceVertex);
			g.addVertex(targetVertex);
			
			return g.addEdge(sourceVertex, targetVertex);
		}
		
		
		/// <summary> Adds the specified source and target vertices to the graph, if not
		/// already included, and creates a new weighted edge and adds it to the
		/// specified graph similarly to the {@link Graph#addEdge(Object, Object)}
		/// method.
		/// 
		/// </summary>
		/// <param name="g">the graph for which the specified edge to be added.
		/// </param>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// </param>
		/// <param name="weight">weight of the edge.
		/// 
		/// </param>
		/// <returns> The newly created edge if added to the graph, otherwise
		/// <code>null</code>.
		/// </returns>
		public static Edge addEdgeWithVertices(Graph g, System.Object sourceVertex, System.Object targetVertex, double weight)
		{
			g.addVertex(sourceVertex);
			g.addVertex(targetVertex);
			
			return addEdge(g, sourceVertex, targetVertex, weight);
		}
		
		
		/// <summary> Adds all the vertices and all the edges of the specified source graph to
		/// the specified destination graph. First all vertices of the source graph
		/// are added to the destination graph. Then every edge of the source graph
		/// is added to the destination graph. This method returns
		/// <code>true</code> if the destination graph has been modified as a
		/// result of this operation, otherwise it returns <code>false</code>.
		/// 
		/// <p>
		/// The behavior of this operation is undefined if any of the specified
		/// graphs is modified while operation is in progress.
		/// </p>
		/// 
		/// </summary>
		/// <param name="destination">the graph to which vertices and edges are added.
		/// </param>
		/// <param name="source">the graph used as source for vertices and edges to add.
		/// 
		/// </param>
		/// <returns> <code>true</code> if and only if the destination graph has been
		/// changed as a result of this operation.
		/// </returns>
		public static bool addGraph(Graph destination, Graph source)
		{
			bool modified = destination.addAllVertices(source.vertexSet());
			modified |= destination.addAllEdges(source.edgeSet());
			
			return modified;
		}
		
		
		/// <summary> Adds all the vertices and all the edges of the specified source digraph
		/// to the specified destination digraph, reversing all of the edges.
		/// 
		/// <p>
		/// The behavior of this operation is undefined if any of the specified
		/// graphs is modified while operation is in progress.
		/// </p>
		/// 
		/// </summary>
		/// <param name="destination">the graph to which vertices and edges are added.
		/// </param>
		/// <param name="source">the graph used as source for vertices and edges to add.
		/// </param>
		public static void  addGraphReversed(DirectedGraph destination, DirectedGraph source)
		{
			destination.addAllVertices(source.vertexSet());
			
			System.Collections.IEnumerator edgesIter = source.edgeSet().GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (edgesIter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				DirectedEdge edge = (DirectedEdge) edgesIter.Current;
				DirectedEdge reversedEdge = new DirectedEdge(edge.Target, edge.Source);
				destination.addEdge(reversedEdge);
			}
		}
		
		
		/// <summary> Returns a list of vertices that are the neighbors of a specified vertex.
		/// If the graph is a multigraph vertices may appear more than once in the
		/// returned list.
		/// 
		/// </summary>
		/// <param name="g">the graph to look for neighbors in.
		/// </param>
		/// <param name="vertex">the vertex to get the neighbors of.
		/// 
		/// </param>
		/// <returns> a list of the vertices that are the neighbors of the specified
		/// vertex.
		/// </returns>
		public static System.Collections.IList neighborListOf(Graph g, System.Object vertex)
		{
			System.Collections.IList neighbors = new System.Collections.ArrayList();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = g.edgesOf(vertex).GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) i.Current;
				neighbors.Add(e.oppositeVertex(vertex));
			}
			
			return neighbors;
		}
		
		
		/// <summary> Returns a list of vertices that are the predecessors of a specified
		/// vertex. If the graph is a multigraph, vertices may appear more than
		/// once in the returned list.
		/// 
		/// </summary>
		/// <param name="g">the graph to look for predecessors in.
		/// </param>
		/// <param name="vertex">the vertex to get the predecessors of.
		/// 
		/// </param>
		/// <returns> a list of the vertices that are the predecessors of the
		/// specified vertex.
		/// </returns>
		public static System.Collections.IList predecessorListOf(DirectedGraph g, System.Object vertex)
		{
			System.Collections.IList predecessors = new System.Collections.ArrayList();
			System.Collections.IList edges = g.incomingEdgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) i.Current;
				predecessors.Add(e.oppositeVertex(vertex));
			}
			
			return predecessors;
		}
		
		
		/// <summary> Returns a list of vertices that are the successors of a specified
		/// vertex. If the graph is a multigraph vertices may appear more than once
		/// in the returned list.
		/// 
		/// </summary>
		/// <param name="g">the graph to look for successors in.
		/// </param>
		/// <param name="vertex">the vertex to get the successors of.
		/// 
		/// </param>
		/// <returns> a list of the vertices that are the successors of the specified
		/// vertex.
		/// </returns>
		public static System.Collections.IList successorListOf(DirectedGraph g, System.Object vertex)
		{
			System.Collections.IList successors = new System.Collections.ArrayList();
			System.Collections.IList edges = g.outgoingEdgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) i.Current;
				successors.Add(e.oppositeVertex(vertex));
			}
			
			return successors;
		}
		
		
		/// <summary> Returns an undirected view of the specified graph. If the specified
		/// graph is directed, returns an undirected view of it. If the specified
		/// graph is undirected, just returns it.
		/// 
		/// </summary>
		/// <param name="g">the graph for which an undirected view to be returned.
		/// 
		/// </param>
		/// <returns> an undirected view of the specified graph, if it is directed, or
		/// or the specified graph itself if it is undirected.
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException if the graph is neither DirectedGraph </throws>
		/// <summary>         nor UndirectedGraph.
		/// 
		/// </summary>
		/// <seealso cref="AsUndirectedGraph">
		/// </seealso>
		public static UndirectedGraph undirectedGraph(Graph g)
		{
			if (g is DirectedGraph)
			{
				return new AsUndirectedGraph((DirectedGraph) g);
			}
			else if (g is UndirectedGraph)
			{
				return (UndirectedGraph) g;
			}
			else
			{
				throw new System.ArgumentException("Graph must be either DirectedGraph or UndirectedGraph");
			}
		}
	}
}
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
/* ----------
* Graph.java
* ----------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   John V. Sichi
*
* $Id: Graph.java,v 1.13 2005/05/25 00:38:37 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 06-Nov-2003 : Change edge sharing semantics (JVS);
*
*/
using System;
using CSGraphT;

namespace org._3pq.jgrapht
{
	
	/// <summary> The root interface in the graph hierarchy.  A mathematical graph-theory
	/// graph object <tt>G(V,E)</tt> contains a set <tt>V</tt> of vertices and a
	/// set <tt>E</tt> of edges. Each edge e=(v1,v2) in E connects vertex v1 to
	/// vertex v2. for more information about graphs and their related definitions
	/// see <a href="http://mathworld.wolfram.com/Graph.html">
	/// http://mathworld.wolfram.com/Graph.html</a>.
	/// 
	/// <p>
	/// This library generally follows the terminology found at: <a
	/// href="http://mathworld.wolfram.com/topics/GraphTheory.html">
	/// http://mathworld.wolfram.com/topics/GraphTheory.html</a>. Implementation of
	/// this interface can provide simple-graphs, multigraphs, pseudographs etc.
	/// The package <code>org._3pq.jgrapht.graph</code> provides a gallery of
	/// abstract and concrete graph implementations.
	/// </p>
	/// 
	/// <p>
	/// This library works best when vertices represent arbitrary objects and edges
	/// represent the relationships between them.  Vertex and edge instances may be
	/// shared by more than one graph.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	public interface Graph
	{
		/// <summary> Returns the edge factory using which this graph creates new edges. The
		/// edge factory is defined when the graph is constructed and must not be
		/// modified.
		/// 
		/// </summary>
		/// <returns> the edge factory using which this graph creates new edges.
		/// </returns>
		EdgeFactory EdgeFactory
		{
			get;
			
		}
		/// <summary> Returns a list of all edges connecting source vertex to target vertex if
		/// such vertices exist in this graph. If any of the vertices does not
		/// exist or is <code>null</code>, returns <code>null</code>. If both
		/// vertices exist but no edges found, returns an empty list.
		/// 
		/// <p>
		/// In undirected graphs, some of the returned edges may have their source
		/// and target vertices in the opposite order. In simple graphs the
		/// returned list is either singleton list or empty list.
		/// </p>
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> a list of all edges connecting source vertex to target vertex.
		/// </returns>
		System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Returns an edge connecting source vertex to target vertex if such
		/// vertices and such edge exist in this graph. Otherwise returns
		/// <code>null</code>. If any of the specified vertices is
		/// <code>null</code>  returns <code>null</code>
		/// 
		/// <p>
		/// In undirected graphs, the returned edge may have its source and target
		/// vertices in the opposite order.
		/// </p>
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> an edge connecting source vertex to target vertex.
		/// </returns>
		Edge getEdge(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Adds all of the specified edges to this graph. The behavior of this
		/// operation is undefined if the specified vertex collection is modified
		/// while the operation is in progress. This method will invoke the {@link
		/// #addEdge(Edge)} method.
		/// 
		/// </summary>
		/// <param name="edges">the edges to be added to this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph changed as a result of the call
		/// 
		/// </returns>
		/// <throws>  NullPointerException if the specified edges contains one or more </throws>
		/// <summary>         null edges, or if the specified vertex collection is
		/// <tt>null</tt>.
		/// 
		/// </summary>
		/// <seealso cref="addVertex(Object)">
		/// </seealso>
		bool addAllEdges(System.Collections.ICollection edges);
		
		
		/// <summary> Adds all of the specified vertices to this graph. The behavior of this
		/// operation is undefined if the specified vertex collection is modified
		/// while the operation is in progress. This method will invoke the {@link
		/// #addVertex(Object)} method.
		/// 
		/// </summary>
		/// <param name="vertices">the vertices to be added to this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph changed as a result of the call
		/// 
		/// </returns>
		/// <throws>  NullPointerException if the specified vertices contains one or </throws>
		/// <summary>         more null vertices, or if the specified vertex collection is
		/// <tt>null</tt>.
		/// 
		/// </summary>
		/// <seealso cref="addVertex(Object)">
		/// </seealso>
		bool addAllVertices(System.Collections.ICollection vertices);
		
		
		/// <summary> Creates a new edge in this graph, going from the source vertex to the
		/// target vertex, and returns the created edge. Some graphs do not allow
		/// edge-multiplicity. In such cases, if the graph already contains an edge
		/// from the specified source to the specified target, than this method
		/// does not change the graph and returns <code>null</code>.
		/// 
		/// <p>
		/// The source and target vertices must already be contained in this graph.
		/// If they are not found in graph IllegalArgumentException is thrown.
		/// </p>
		/// 
		/// <p>
		/// This method creates the new edge <code>e</code> using this graph's
		/// <code>EdgeFactory</code>. For the new edge to be added <code>e</code>
		/// must <i>not</i> be equal to any other edge the graph (even if the graph
		/// allows edge-multiplicity). More formally, the graph must not contain
		/// any edge <code>e2</code> such that <code>e2.equals(e)</code>. If such
		/// <code>e2</code> is found then the newly created edge <code>e</code> is
		/// abandoned, the method leaves this graph unchanged returns
		/// <code>null</code>.
		/// </p>
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> The newly created edge if added to the graph, otherwise
		/// <code>null</code>.
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException if source or target vertices are not </throws>
		/// <summary>         found in the graph.
		/// </summary>
		/// <throws>  NullPointerException if any of the specified vertices is </throws>
		/// <summary>         <code>null</code>.
		/// 
		/// </summary>
		/// <seealso cref="getEdgeFactory()">
		/// </seealso>
		Edge addEdge(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Adds the specified edge to this graph. More formally, adds the specified
		/// edge, <code>e</code>, to this graph if this graph contains no edge
		/// <code>e2</code> such that <code>e2.equals(e)</code>. If this graph
		/// already contains such edge, the call leaves this graph unchanged and
		/// returns <tt>false</tt>. If the edge was added to the graph, returns
		/// <code>true</code>.
		/// 
		/// <p>
		/// Some graphs do not allow edge-multiplicity. In such cases, if the graph
		/// already contains an edge going from <code>e.getSource()</code> vertex
		/// to <code>e.getTarget()</code> vertex, than this method does not change
		/// the graph and returns <code>false</code>.
		/// </p>
		/// 
		/// <p>
		/// The source and target vertices of the specified edge must already be in
		/// this graph. If this is not the case, IllegalArgumentException is
		/// thrown. The edge must also be assignment compatible with the class of
		/// the edges produced by the edge factory of this graph. If this is not
		/// the case ClassCastException is thrown.
		/// </p>
		/// 
		/// </summary>
		/// <param name="e">edge to be added to this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph did not already contain the
		/// specified edge.
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException if source or target vertices of </throws>
		/// <summary>         specified edge are not found in this graph.
		/// </summary>
		/// <throws>  ClassCastException if the specified edge is not assignment </throws>
		/// <summary>         compatible with the class of edges produced by the edge factory
		/// of this graph.
		/// </summary>
		/// <throws>  NullPointerException if the specified edge is <code>null</code>. </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="addEdge(Object, Object)">
		/// </seealso>
		/// <seealso cref="getEdgeFactory()">
		/// </seealso>
		/// <seealso cref="EdgeFactory">
		/// </seealso>
		bool addEdge(Edge e);
		
		
		/// <summary> Adds the specified vertex to this graph if not already present. More
		/// formally, adds the specified vertex, <code>v</code>, to this graph if
		/// this graph contains no vertex <code>u</code> such that
		/// <code>u.equals(v)</code>. If this graph already contains such vertex,
		/// the call leaves this graph unchanged and returns <tt>false</tt>. In
		/// combination with the restriction on constructors, this ensures that
		/// graphs never contain duplicate vertices.
		/// 
		/// </summary>
		/// <param name="v">vertex to be added to this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph did not already contain the
		/// specified vertex.
		/// 
		/// </returns>
		/// <throws>  NullPointerException if the specified vertex is </throws>
		/// <summary>         <code>null</code>.
		/// </summary>
		bool addVertex(System.Object v);
		
		
		/// <summary> Returns <tt>true</tt> if and only if this graph contains an edge going
		/// from the source vertex to the target vertex. In undirected graphs the
		/// same result is obtained when source and target are inverted. If any of
		/// the specified vertices does not exist in the graph, or if is
		/// <code>null</code>, returns <code>false</code>.
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph contains the specified edge.
		/// </returns>
		bool containsEdge(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Returns <tt>true</tt> if this graph contains the specified edge.  More
		/// formally, returns <tt>true</tt> if and only if this graph contains an
		/// edge <code>e2</code> such that <code>e.equals(e2)</code>. If the
		/// specified edge is <code>null</code> returns <code>false</code>.
		/// 
		/// </summary>
		/// <param name="e">edge whose presence in this graph is to be tested.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph contains the specified edge.
		/// </returns>
		bool containsEdge(Edge e);
		
		
		/// <summary> Returns <tt>true</tt> if this graph contains the specified vertex.  More
		/// formally, returns <tt>true</tt> if and only if this graph contains a
		/// vertex <code>u</code> such that <code>u.equals(v)</code>. If the
		/// specified vertex is <code>null</code> returns <code>false</code>.
		/// 
		/// </summary>
		/// <param name="v">vertex whose presence in this graph is to be tested.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph contains the specified vertex.
		/// </returns>
		bool containsVertex(System.Object v);
		
		
		/// <summary> Returns a set of the edges contained in this graph. The set is backed by
		/// the graph, so changes to the graph are reflected in the set. If the
		/// graph is modified while an iteration over the set is in progress, the
		/// results of the iteration are undefined.
		/// 
		/// <p>
		/// The graph implementation may maintain a particular set ordering (e.g.
		/// via {@link java.util.LinkedHashSet}) for deterministic iteration, but
		/// this is not required.  It is the responsibility of callers who rely on
		/// this behavior to only use graph implementations which support it.
		/// </p>
		/// 
		/// </summary>
		/// <returns> a set of the edges contained in this graph.
		/// </returns>
		SupportClass.SetSupport edgeSet();
		
		
		/// <summary> Returns a list of all edges touching the specified vertex. If no edges
		/// are touching the specified vertex returns an empty list.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex for which a list of touching edges to be
		/// returned.
		/// 
		/// </param>
		/// <returns> a list of all edges touching the specified vertex.
		/// </returns>
		System.Collections.IList edgesOf(System.Object vertex);
		
		
		/// <summary> Removes all the edges in this graph that are also contained in the
		/// specified edge collection.  After this call returns, this graph will
		/// contain no edges in common with the specified edges. This method will
		/// invoke the {@link #removeEdge(Edge)} method.
		/// 
		/// </summary>
		/// <param name="edges">edges to be removed from this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph changed as a result of the call
		/// 
		/// </returns>
		/// <throws>  NullPointerException if the specified edge collection is </throws>
		/// <summary>         <tt>null</tt>.
		/// 
		/// </summary>
		/// <seealso cref="removeEdge(Edge)">
		/// </seealso>
		/// <seealso cref="containsEdge(Edge)">
		/// </seealso>
		bool removeAllEdges(System.Collections.ICollection edges);
		
		
		/// <summary> Removes all the edges going from the specified source vertex to the
		/// specified target vertex, and returns a list of all removed edges.
		/// Returns <code>null</code> if any of the specified vertices does exist
		/// in the graph. If both vertices exist but no edge found, returns an
		/// empty list. This method will either invoke the {@link
		/// #removeEdge(Edge)} method, or the {@link #removeEdge(Object, Object)}
		/// method.
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> The removed edge, or <code>null</code> if no edge removed.
		/// </returns>
		System.Collections.IList removeAllEdges(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Removes all the vertices in this graph that are also contained in the
		/// specified vertex collection.  After this call returns, this graph will
		/// contain no vertices in common with the specified vertices. This method
		/// will invoke the {@link #removeVertex(Object)} method.
		/// 
		/// </summary>
		/// <param name="vertices">vertices to be removed from this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph changed as a result of the call
		/// 
		/// </returns>
		/// <throws>  NullPointerException if the specified vertex collection is </throws>
		/// <summary>         <tt>null</tt>.
		/// 
		/// </summary>
		/// <seealso cref="removeVertex(Object)">
		/// </seealso>
		/// <seealso cref="containsVertex(Object)">
		/// </seealso>
		bool removeAllVertices(System.Collections.ICollection vertices);
		
		
		/// <summary> Removes an edge going from source vertex to target vertex, if such
		/// vertices and such edge exist in this graph. Returns the edge if removed
		/// or <code>null</code> otherwise.
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// 
		/// </param>
		/// <returns> The removed edge, or <code>null</code> if no edge removed.
		/// </returns>
		Edge removeEdge(System.Object sourceVertex, System.Object targetVertex);
		
		
		/// <summary> Removes the specified edge from the graph. Removes the specified edge
		/// from this graph if it is present. More formally, removes an edge
		/// <code>e2</code> such that <code>e2.equals(e)</code>, if the graph
		/// contains such edge. Returns <tt>true</tt> if the graph contained the
		/// specified edge. (The graph will not contain the specified edge once the
		/// call returns).
		/// 
		/// <p>
		/// If the specified edge is <code>null</code> returns <code>false</code>.
		/// </p>
		/// 
		/// </summary>
		/// <param name="e">edge to be removed from this graph, if present.
		/// 
		/// </param>
		/// <returns> <code>true</code> if and only if the graph contained the
		/// specified edge.
		/// </returns>
		bool removeEdge(Edge e);
		
		
		/// <summary> Removes the specified vertex from this graph including all its touching
		/// edges if present.  More formally, if the graph contains a vertex
		/// <code>u</code> such that <code>u.equals(v)</code>, the call removes all
		/// edges that touch <code>u</code> and then removes <code>u</code> itself.
		/// If no such <code>u</code> is found, the call leaves the graph
		/// unchanged. Returns <tt>true</tt> if the graph contained the specified
		/// vertex. (The graph will not contain the specified vertex once the call
		/// returns).
		/// 
		/// <p>
		/// If the specified vertex is <code>null</code> returns <code>false</code>.
		/// </p>
		/// 
		/// </summary>
		/// <param name="v">vertex to be removed from this graph, if present.
		/// 
		/// </param>
		/// <returns> <code>true</code> if the graph contained the specified vertex;
		/// <code>false</code> otherwise.
		/// </returns>
		bool removeVertex(System.Object v);
		
		
		/// <summary> Returns a set of the vertices contained in this graph. The set is backed
		/// by the graph, so changes to the graph are reflected in the set. If the
		/// graph is modified while an iteration over the set is in progress, the
		/// results of the iteration are undefined.
		/// 
		/// <p>
		/// The graph implementation may maintain a particular set ordering (e.g.
		/// via {@link java.util.LinkedHashSet}) for deterministic iteration, but
		/// this is not required.  It is the responsibility of callers who rely on
		/// this behavior to only use graph implementations which support it.
		/// </p>
		/// 
		/// </summary>
		/// <returns> a set view of the vertices contained in this graph.
		/// </returns>
		SupportClass.SetSupport vertexSet();
	}
}
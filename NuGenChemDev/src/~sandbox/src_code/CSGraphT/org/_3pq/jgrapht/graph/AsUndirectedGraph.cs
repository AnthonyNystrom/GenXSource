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
/* ----------------------
* AsUndirectedGraph.java
* ----------------------
* (C) Copyright 2003, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
* Contributor(s):   -
*
* $Id: AsUndirectedGraph.java,v 1.6 2004/11/19 09:59:26 barak_naveh Exp $
*
* Changes
* -------
* 14-Aug-2003 : Initial revision (JVS);
*
*/
using System;
using CSGraphT;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using Edge = org._3pq.jgrapht.Edge;
using UndirectedGraph = org._3pq.jgrapht.UndirectedGraph;
using UndirectedEdge = org._3pq.jgrapht.edge.UndirectedEdge;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> An undirected view of the backing directed graph specified in the
	/// constructor.  This graph allows modules to apply algorithms designed for
	/// undirected graphs to a directed graph by simply ignoring edge direction. If
	/// the backing directed graph is an <a
	/// href="http://mathworld.wolfram.com/OrientedGraph.html"> oriented graph</a>,
	/// then the view will be a simple graph; otherwise, it will be a multigraph.
	/// Query operations on this graph "read through" to the backing graph.
	/// Attempts to add edges will result in an
	/// <code>UnsupportedOperationException</code>, but vertex addition/removal and
	/// edge removal are all supported (and immediately reflected in the backing
	/// graph).
	/// 
	/// <p>
	/// Note that edges returned by this graph's accessors are really just the edges
	/// of the underlying directed graph.  Since there is no interface distinction
	/// between directed and undirected edges, this detail should be irrelevant to
	/// algorithms.
	/// </p>
	/// 
	/// <p>
	/// This graph does <i>not</i> pass the hashCode and equals operations through
	/// to the backing graph, but relies on <tt>Object</tt>'s <tt>equals</tt> and
	/// <tt>hashCode</tt> methods.  This graph will be serializable if the backing
	/// graph is serializable.
	/// </p>
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Aug 14, 2003
	/// </since>
	[Serializable]
	public class AsUndirectedGraph:GraphDelegator, UndirectedGraph
	{
		private const long serialVersionUID = 3257845485078065462L;
		private const System.String NO_EDGE_ADD = "this graph does not support edge addition";
		private const System.String UNDIRECTED = "this graph only supports undirected operations";
		
		/// <summary> Constructor for AsUndirectedGraph.
		/// 
		/// </summary>
		/// <param name="g">the backing directed graph over which an undirected view is to
		/// be created.
		/// </param>
		public AsUndirectedGraph(DirectedGraph g):base(g)
		{
		}
		
		/// <seealso cref="org._3pq.jgrapht.Graph.getAllEdges(Object, Object)">
		/// </seealso>
		public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			System.Collections.IList forwardList = base.getAllEdges(sourceVertex, targetVertex);
			
			if (sourceVertex.Equals(targetVertex))
			{
				// avoid duplicating loops
				return forwardList;
			}
			
			System.Collections.IList reverseList = base.getAllEdges(targetVertex, sourceVertex);
			System.Collections.IList list = new System.Collections.ArrayList(forwardList.Count + reverseList.Count);
			SupportClass.ICollectionSupport.AddAll(list, forwardList);
			SupportClass.ICollectionSupport.AddAll(list, reverseList);
			
			return list;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.getEdge(Object, Object)">
		/// </seealso>
		public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			Edge edge = base.getEdge(sourceVertex, targetVertex);
			
			if (edge != null)
			{
				return edge;
			}
			
			// try the other direction
			return base.getEdge(targetVertex, sourceVertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.addAllEdges(Collection)">
		/// </seealso>
		public override bool addAllEdges(System.Collections.ICollection edges)
		{
			throw new System.NotSupportedException(NO_EDGE_ADD);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			throw new System.NotSupportedException(NO_EDGE_ADD);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			throw new System.NotSupportedException(NO_EDGE_ADD);
		}
		
		
		/// <seealso cref="UndirectedGraph.degreeOf(Object)">
		/// </seealso>
		public override int degreeOf(System.Object vertex)
		{
			// this counts loops twice, which is consistent with AbstractBaseGraph
			return base.inDegreeOf(vertex) + base.outDegreeOf(vertex);
		}
		
		
		/// <seealso cref="DirectedGraph.inDegreeOf(Object)">
		/// </seealso>
		public override int inDegreeOf(System.Object vertex)
		{
			throw new System.NotSupportedException(UNDIRECTED);
		}
		
		
		/// <seealso cref="DirectedGraph.incomingEdgesOf(Object)">
		/// </seealso>
		public override System.Collections.IList incomingEdgesOf(System.Object vertex)
		{
			throw new System.NotSupportedException(UNDIRECTED);
		}
		
		
		/// <seealso cref="DirectedGraph.outDegreeOf(Object)">
		/// </seealso>
		public override int outDegreeOf(System.Object vertex)
		{
			throw new System.NotSupportedException(UNDIRECTED);
		}
		
		
		/// <seealso cref="DirectedGraph.outgoingEdgesOf(Object)">
		/// </seealso>
		public override System.Collections.IList outgoingEdgesOf(System.Object vertex)
		{
			throw new System.NotSupportedException(UNDIRECTED);
		}
		
		
		/// <seealso cref="AbstractBaseGraph.toString()">
		/// </seealso>
		public override System.String ToString()
		{
			// take care to print edges using the undirected convention
			System.Collections.ICollection edgeSet = new System.Collections.ArrayList();
			
			System.Collections.IEnumerator iter = edgeSet.GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge edge = (Edge) iter.Current;
				SupportClass.ICollectionSupport.Add(edgeSet, new UndirectedEdge(edge.Source, edge.Target));
			}
			
			return base.toStringFromSets(vertexSet(), edgeSet);
		}
	}
}
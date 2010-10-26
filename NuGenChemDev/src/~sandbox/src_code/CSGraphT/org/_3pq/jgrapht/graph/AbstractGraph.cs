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
/* ------------------
* AbstractGraph.java
* ------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: AbstractGraph.java,v 1.11 2005/04/23 08:09:29 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
*
*/
using System;
using CSGraphT;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A skeletal implementation of the <tt>Graph</tt> interface, to minimize the
	/// effort required to implement graph interfaces. This implementation is
	/// applicable to both: directed graphs and undirected graphs.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <seealso cref="org._3pq.jgrapht.Graph">
	/// </seealso>
	/// <seealso cref="org._3pq.jgrapht.DirectedGraph">
	/// </seealso>
	/// <seealso cref="org._3pq.jgrapht.UndirectedGraph">
	/// </seealso>
	[Serializable]
	public abstract class AbstractGraph : Graph
	{
		public abstract org._3pq.jgrapht.EdgeFactory EdgeFactory{get;}
		/// <summary> Construct a new empty graph object.</summary>
		public AbstractGraph()
		{
		}
		
		/// <seealso cref="Graph.addAllEdges(Collection)">
		/// </seealso>
		public virtual bool addAllEdges(System.Collections.ICollection edges)
		{
			bool modified = false;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator iter = edges.GetEnumerator(); iter.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				modified |= addEdge((Edge) iter.Current);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="Graph.addAllVertices(Collection)">
		/// </seealso>
		public virtual bool addAllVertices(System.Collections.ICollection vertices)
		{
			bool modified = false;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator iter = vertices.GetEnumerator(); iter.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				modified |= addVertex(iter.Current);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="Graph.containsEdge(Object, Object)">
		/// </seealso>
		public virtual bool containsEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			return getEdge(sourceVertex, targetVertex) != null;
		}
		
		
		/// <seealso cref="Graph.removeAllEdges(Collection)">
		/// </seealso>
		public virtual bool removeAllEdges(System.Collections.ICollection edges)
		{
			bool modified = false;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator iter = edges.GetEnumerator(); iter.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				modified |= removeEdge((Edge) iter.Current);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="Graph.removeAllEdges(Object, Object)">
		/// </seealso>
		public virtual System.Collections.IList removeAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			System.Collections.IList removed = getAllEdges(sourceVertex, targetVertex);
			removeAllEdges(removed);
			
			return removed;
		}
		
		
		/// <seealso cref="Graph.removeAllVertices(Collection)">
		/// </seealso>
		public virtual bool removeAllVertices(System.Collections.ICollection vertices)
		{
			bool modified = false;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator iter = vertices.GetEnumerator(); iter.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				modified |= removeVertex(iter.Current);
			}
			
			return modified;
		}
		
		
		/// <summary> Returns a string of the parenthesized pair (V, E) representing this
		/// G=(V,E) graph. 'V' is the string representation of the vertex set, and
		/// 'E' is the string representation of the edge set.
		/// 
		/// </summary>
		/// <returns> a string representation of this graph.
		/// </returns>
		public override System.String ToString()
		{
			return toStringFromSets(vertexSet(), edgeSet());
		}
		
		
		/// <summary> Ensures that the specified vertex exists in this graph, or else throws
		/// exception.
		/// 
		/// </summary>
		/// <param name="v">vertex
		/// 
		/// </param>
		/// <returns> <code>true</code> if this assertion holds.
		/// 
		/// </returns>
		/// <throws>  NullPointerException if specified vertex is <code>null</code>. </throws>
		/// <throws>  IllegalArgumentException if specified vertex does not exist in </throws>
		/// <summary>         this graph.
		/// </summary>
		protected internal virtual bool assertVertexExist(System.Object v)
		{
			if (containsVertex(v))
			{
				return true;
			}
			else if (v == null)
			{
				throw new System.NullReferenceException();
			}
			else
			{
				throw new System.ArgumentException("no such vertex in graph");
			}
		}
		
		
		/// <summary> Removes all the edges in this graph that are also contained in the
		/// specified edge array.  After this call returns, this graph will contain
		/// no edges in common with the specified edges. This method will invoke
		/// the {@link Graph#removeEdge(Edge)} method.
		/// 
		/// </summary>
		/// <param name="edges">edges to be removed from this graph.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this graph changed as a result of the call.
		/// 
		/// </returns>
		/// <seealso cref="Graph.removeEdge(Edge)">
		/// </seealso>
		/// <seealso cref="Graph.containsEdge(Edge)">
		/// </seealso>
		protected internal virtual bool removeAllEdges(Edge[] edges)
		{
			bool modified = false;
			
			for (int i = 0; i < edges.Length; i++)
			{
				modified |= removeEdge(edges[i]);
			}
			
			return modified;
		}
		
		
		/// <summary> Helper for subclass implementations of toString(  ).
		/// 
		/// </summary>
		/// <param name="vertexSet">the vertex set V to be printed
		/// </param>
		/// <param name="edgeSet">the edge set E to be printed
		/// 
		/// </param>
		/// <returns> a string representation of (V,E)
		/// </returns>
		protected internal virtual System.String toStringFromSets(System.Collections.ICollection vertexSet, System.Collections.ICollection edgeSet)
		{
			return "(" + SupportClass.CollectionToString(vertexSet) + ", " + SupportClass.CollectionToString(edgeSet) + ")";
		}
		public abstract bool addVertex(System.Object param1);
		public abstract bool removeEdge(org._3pq.jgrapht.Edge param1);
		public abstract bool containsEdge(org._3pq.jgrapht.Edge param1);
		public abstract org._3pq.jgrapht.Edge addEdge(System.Object param1, System.Object param2);
		public abstract SupportClass.SetSupport vertexSet();
		public abstract bool containsVertex(System.Object param1);
		public abstract System.Collections.IList edgesOf(System.Object param1);
		public abstract org._3pq.jgrapht.Edge getEdge(System.Object param1, System.Object param2);
		public abstract bool addEdge(org._3pq.jgrapht.Edge param1);
		public abstract SupportClass.SetSupport edgeSet();
		public abstract System.Collections.IList getAllEdges(System.Object param1, System.Object param2);
		public abstract org._3pq.jgrapht.Edge removeEdge(System.Object param1, System.Object param2);
		public abstract bool removeVertex(System.Object param1);
	}
}
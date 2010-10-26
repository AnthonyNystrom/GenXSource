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
* UnmodifiableGraph.java
* ----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: UnmodifiableGraph.java,v 1.4 2004/11/19 10:21:36 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> An unmodifiable view of the backing graph specified in the constructor. This
	/// graph allows modules to provide users with "read-only" access to internal
	/// graphs. Query operations on this graph "read through" to the backing graph,
	/// and attempts to modify this graph result in an
	/// <code>UnsupportedOperationException</code>.
	/// 
	/// <p>
	/// This graph does <i>not</i> pass the hashCode and equals operations through
	/// to the backing graph, but relies on <tt>Object</tt>'s <tt>equals</tt> and
	/// <tt>hashCode</tt> methods.  This graph will be serializable if the backing
	/// graph is serializable.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 24, 2003
	/// </since>
	[Serializable]
	public class UnmodifiableGraph:GraphDelegator
	{
		private const long serialVersionUID = 3544957670722713913L;
		private const System.String UNMODIFIABLE = "this graph is unmodifiable";
		
		/// <summary> Creates a new unmodifiable graph based on the specified backing graph.
		/// 
		/// </summary>
		/// <param name="g">the backing graph on which an unmodifiable graph is to be
		/// created.
		/// </param>
		public UnmodifiableGraph(Graph g):base(g)
		{
		}
		
		/// <seealso cref="Graph.addAllEdges(Collection)">
		/// </seealso>
		public override bool addAllEdges(System.Collections.ICollection edges)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.addAllVertices(Collection)">
		/// </seealso>
		public override bool addAllVertices(System.Collections.ICollection vertices)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.addVertex(Object)">
		/// </seealso>
		public override bool addVertex(System.Object v)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeAllEdges(Collection)">
		/// </seealso>
		public override bool removeAllEdges(System.Collections.ICollection edges)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeAllEdges(Object, Object)">
		/// </seealso>
		public override System.Collections.IList removeAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeAllVertices(Collection)">
		/// </seealso>
		public override bool removeAllVertices(System.Collections.ICollection vertices)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeEdge(Edge)">
		/// </seealso>
		public override bool removeEdge(Edge e)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeEdge(Object, Object)">
		/// </seealso>
		public override Edge removeEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
		
		
		/// <seealso cref="Graph.removeVertex(Object)">
		/// </seealso>
		public override bool removeVertex(System.Object v)
		{
			throw new System.NotSupportedException(UNMODIFIABLE);
		}
	}
}
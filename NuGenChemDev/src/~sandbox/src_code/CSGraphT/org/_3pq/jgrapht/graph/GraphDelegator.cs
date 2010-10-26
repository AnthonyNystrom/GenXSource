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
/* -------------------
* GraphDelegator.java
* -------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphDelegator.java,v 1.5 2005/01/29 23:26:14 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
*
*/
using System;
using CSGraphT;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using Edge = org._3pq.jgrapht.Edge;
using EdgeFactory = org._3pq.jgrapht.EdgeFactory;
using Graph = org._3pq.jgrapht.Graph;
using UndirectedGraph = org._3pq.jgrapht.UndirectedGraph;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A graph backed by the the graph specified at the constructor, which
	/// delegates all its methods to the backing graph. Operations on this graph
	/// "pass through" to the to the backing graph. Any modification made to this
	/// graph or the backing graph is reflected by the other.
	/// 
	/// <p>
	/// This graph does <i>not</i> pass the hashCode and equals operations through
	/// to the backing graph, but relies on <tt>Object</tt>'s <tt>equals</tt> and
	/// <tt>hashCode</tt> methods.
	/// </p>
	/// 
	/// <p>
	/// This class is mostly used as a base for extending subclasses.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 20, 2003
	/// </since>
	[Serializable]
	public class GraphDelegator:AbstractGraph, Graph
	{
		/// <seealso cref="Graph.getEdgeFactory()">
		/// </seealso>
		override public EdgeFactory EdgeFactory
		{
			get
			{
				return m_delegate.EdgeFactory;
			}
			
		}
		private const long serialVersionUID = 3257005445226181425L;
		
		/// <summary>The graph to which operations are delegated. </summary>
		private Graph m_delegate;
		
		/// <summary> Constructor for GraphDelegator.
		/// 
		/// </summary>
		/// <param name="g">the backing graph (the delegate).
		/// 
		/// </param>
		/// <throws>  NullPointerException </throws>
		public GraphDelegator(Graph g):base()
		{
			
			if (g == null)
			{
				throw new System.NullReferenceException();
			}
			
			m_delegate = g;
		}
		
		/// <seealso cref="Graph.getAllEdges(Object, Object)">
		/// </seealso>
		public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_delegate.getAllEdges(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.getEdge(Object, Object)">
		/// </seealso>
		public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_delegate.getEdge(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			return m_delegate.addEdge(e);
		}
		
		
		/// <seealso cref="Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_delegate.addEdge(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.addVertex(Object)">
		/// </seealso>
		public override bool addVertex(System.Object v)
		{
			return m_delegate.addVertex(v);
		}
		
		
		/// <seealso cref="Graph.containsEdge(Edge)">
		/// </seealso>
		public override bool containsEdge(Edge e)
		{
			return m_delegate.containsEdge(e);
		}
		
		
		/// <seealso cref="Graph.containsVertex(Object)">
		/// </seealso>
		public override bool containsVertex(System.Object v)
		{
			return m_delegate.containsVertex(v);
		}
		
		
		/// <seealso cref="UndirectedGraph.degreeOf(Object)">
		/// </seealso>
		public virtual int degreeOf(System.Object vertex)
		{
			return ((UndirectedGraph) m_delegate).degreeOf(vertex);
		}
		
		
		/// <seealso cref="Graph.edgeSet()">
		/// </seealso>
		public override SupportClass.SetSupport edgeSet()
		{
			return m_delegate.edgeSet();
		}
		
		
		/// <seealso cref="Graph.edgesOf(Object)">
		/// </seealso>
		public override System.Collections.IList edgesOf(System.Object vertex)
		{
			return m_delegate.edgesOf(vertex);
		}
		
		
		/// <seealso cref="DirectedGraph.inDegreeOf(Object)">
		/// </seealso>
		public virtual int inDegreeOf(System.Object vertex)
		{
			return ((DirectedGraph) m_delegate).inDegreeOf(vertex);
		}
		
		
		/// <seealso cref="DirectedGraph.incomingEdgesOf(Object)">
		/// </seealso>
		public virtual System.Collections.IList incomingEdgesOf(System.Object vertex)
		{
			return ((DirectedGraph) m_delegate).incomingEdgesOf(vertex);
		}
		
		
		/// <seealso cref="DirectedGraph.outDegreeOf(Object)">
		/// </seealso>
		public virtual int outDegreeOf(System.Object vertex)
		{
			return ((DirectedGraph) m_delegate).outDegreeOf(vertex);
		}
		
		
		/// <seealso cref="DirectedGraph.outgoingEdgesOf(Object)">
		/// </seealso>
		public virtual System.Collections.IList outgoingEdgesOf(System.Object vertex)
		{
			return ((DirectedGraph) m_delegate).outgoingEdgesOf(vertex);
		}
		
		
		/// <seealso cref="Graph.removeEdge(Edge)">
		/// </seealso>
		public override bool removeEdge(Edge e)
		{
			return m_delegate.removeEdge(e);
		}
		
		
		/// <seealso cref="Graph.removeEdge(Object, Object)">
		/// </seealso>
		public override Edge removeEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_delegate.removeEdge(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.removeVertex(Object)">
		/// </seealso>
		public override bool removeVertex(System.Object v)
		{
			return m_delegate.removeVertex(v);
		}
		
		
		/// <seealso cref="java.lang.Object.toString()">
		/// </seealso>
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return m_delegate.ToString();
		}
		
		
		/// <seealso cref="Graph.vertexSet()">
		/// </seealso>
		public override SupportClass.SetSupport vertexSet()
		{
			return m_delegate.vertexSet();
		}
	}
}
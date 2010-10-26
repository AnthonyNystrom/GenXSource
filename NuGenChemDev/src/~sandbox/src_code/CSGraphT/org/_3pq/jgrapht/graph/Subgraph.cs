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
/* -------------
* Subgraph.java
* -------------
* (C) Copyright 2003-2004, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: Subgraph.java,v 1.19 2005/05/25 00:38:37 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 26-Jul-2003 : Accurate constructors to avoid casting problems (BN);
* 10-Aug-2003 : Adaptation to new event model (BN);
* 23-Oct-2003 : Allowed non-listenable graph as base (BN);
* 07-Feb-2004 : Enabled serialization (BN);
* 20-Mar-2004 : Cancelled verification of element identity to base graph (BN);
* 21-Sep-2004 : Added induced subgraph
*
*/
using System;
using CSGraphT;
using org._3pq.jgrapht.event_Renamed;
using System.Collections;

namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A subgraph is a graph that has a subset of vertices and a subset of edges
	/// with respect to some base graph. More formally, a subgraph G(V,E) that is
	/// based on a base graph Gb(Vb,Eb) satisfies the following <b><i>subgraph
	/// property</i></b>: V is a subset of Vb and E is a subset of Eb. Other than
	/// this property, a subgraph is a graph with any respect and fully complies
	/// with the <code>Graph</code> interface.
	/// 
	/// <p>
	/// If the base graph is a {@link org._3pq.jgrapht.ListenableGraph}, the
	/// subgraph listens on the base graph and guarantees the subgraph property. If
	/// an edge or a vertex is removed from the base graph, it is automatically
	/// removed from the subgraph. Subgraph listeners are informed on such removal
	/// only if it results in a cascaded removal from the subgraph. If the subgraph
	/// has been created as an induced subgraph it also keeps track of edges being
	/// added to its vertices. If  vertices are added to the base graph, the
	/// subgraph remains unaffected.
	/// </p>
	/// 
	/// <p>
	/// If the base graph is <i>not</i> a ListenableGraph, then the subgraph
	/// property cannot be guaranteed. If edges or vertices are removed from the
	/// base graph, they are <i>not</i> removed from the subgraph.
	/// </p>
	/// 
	/// <p>
	/// Modifications to Subgraph are allowed as long as the subgraph property is
	/// maintained. Addition of vertices or edges are allowed as long as they also
	/// exist in the base graph. Removal of vertices or edges is always allowed.
	/// The base graph is <i>never</i> affected by any modification made to the
	/// subgraph.
	/// </p>
	/// 
	/// <p>
	/// A subgraph may provide a "live-window" on a base graph, so that changes made
	/// to its vertices or edges are immediately reflected in the base graph, and
	/// vice versa. For that to happen, vertices and edges added to the subgraph
	/// must be <i>identical</i> (that is, reference-equal and not only
	/// value-equal) to their respective ones in the base graph. Previous versions
	/// of this class enforced such identity, at a severe performance cost.
	/// Currently it is no longer enforced. If you want to achieve a "live-window"
	/// functionality, your safest tactics would be to NOT override the
	/// <code>equals()</code>methods of your vertices and edges. If you use a class
	/// that has already overridden the <code>equals()</code> method, such as
	/// <code>String</code>, than you can use a wrapper around it, or else use it
	/// directly but exercise a great care to avoid having different-but-equal
	/// instances in the subgraph and the base graph.
	/// </p>
	/// 
	/// <p>
	/// This graph implementation guarantees deterministic vertex and edge set
	/// ordering (via {@link LinkedHashSet}).
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <seealso cref="org._3pq.jgrapht.Graph">
	/// </seealso>
	/// <seealso cref="java.util.Set">
	/// </seealso>
	/// <since> Jul 18, 2003
	/// </since>
	[Serializable]
	public class Subgraph : AbstractGraph
	{
		/// <seealso cref="org._3pq.jgrapht.Graph.getEdgeFactory()">
		/// </seealso>
		override public EdgeFactory EdgeFactory
		{
			get
			{
				return m_base.EdgeFactory;
			}
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the value of the verifyIntegrity flag.
		/// 
		/// </summary>
		/// <returns> the value of the verifyIntegrity flag.
		/// 
		/// </returns>
		/// <deprecated> method will be deleted in future versions.
		/// </deprecated>
		/// <summary> Sets the the check integrity flag.
		/// 
		/// </summary>
		/// <param name="verifyIntegrity">*
		/// </param>
		/// <seealso cref="Subgraph">
		/// </seealso>
		/// <deprecated> method will be deleted in future versions. verifyIntegrity
		/// flag has no effect now.
		/// </deprecated>
		virtual public bool VerifyIntegrity
		{
			get
			{
				return m_verifyIntegrity;
			}
			
			set
			{
				m_verifyIntegrity = value;
			}
			
		}
		private const System.String NO_SUCH_EDGE_IN_BASE = "no such edge in base graph";
		private const System.String NO_SUCH_VERTEX_IN_BASE = "no such vertex in base graph";
		
		//
        internal SupportClass.SetSupport m_edgeSet = new SupportClass.HashSetSupport(); // friendly to improve performance
        internal SupportClass.SetSupport m_vertexSet = new SupportClass.HashSetSupport(); // friendly to improve performance
		
		// 
		[NonSerialized]
		private SupportClass.SetSupport m_unmodifiableEdgeSet = null;
		[NonSerialized]
		private SupportClass.SetSupport m_unmodifiableVertexSet = null;
		private Graph m_base;
		private bool m_isInduced = false;
		private bool m_verifyIntegrity = true;
		
		/// <summary> Creates a new Subgraph.
		/// 
		/// </summary>
		/// <param name="base">the base (backing) graph on which the subgraph will be
		/// based.
		/// </param>
		/// <param name="vertexSubset">vertices to include in the subgraph. If
		/// <code>null</code> then all vertices are included.
		/// </param>
		/// <param name="edgeSubset">edges to in include in the subgraph. If
		/// <code>null</code> then all the edges whose vertices found in the
		/// graph are included.
		/// </param>
		public Subgraph(Graph base_Renamed, SupportClass.SetSupport vertexSubset, SupportClass.SetSupport edgeSubset)
		{
			m_base = base_Renamed;
			
			if (m_base is ListenableGraph)
			{
				((ListenableGraph) m_base).addGraphListener(new BaseGraphListener(this));
			}
			
			addVerticesUsingFilter(base_Renamed.vertexSet(), vertexSubset);
			addEdgesUsingFilter(base_Renamed.edgeSet(), edgeSubset);
		}
		
		
		/// <summary> Creates a new induced Subgraph. The subgraph will keep track of edges
		/// being added to its vertex subset as well as deletion of edges and
		/// vertices. If base it not listenable, this is identical to the call
		/// Subgraph(base, vertexSubset, null) .
		/// 
		/// </summary>
		/// <param name="base">the base (backing) graph on which the subgraph will be
		/// based.
		/// </param>
		/// <param name="vertexSubset">vertices to include in the subgraph. If
		/// <code>null</code> then all vertices are included.
		/// </param>
		public Subgraph(Graph base_Renamed, SupportClass.SetSupport vertexSubset):this(base_Renamed, vertexSubset, null)
		{
			m_isInduced = true;
		}
		
		/// <seealso cref="org._3pq.jgrapht.Graph.getAllEdges(Object, Object)">
		/// </seealso>
		public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			System.Collections.IList edges = null;
			
			if (containsVertex(sourceVertex) && containsVertex(targetVertex))
			{
				edges = new System.Collections.ArrayList();
				
				System.Collections.IList baseEdges = m_base.getAllEdges(sourceVertex, targetVertex);
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator i = baseEdges.GetEnumerator(); i.MoveNext(); )
				{
					Edge e = (Edge) i.Current;
					
					if (m_edgeSet.Contains(e))
					{
						// add if subgraph also contains it
						edges.Add(e);
					}
				}
			}
			
			return edges;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.getEdge(Object, Object)">
		/// </seealso>
		public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			System.Collections.IList edges = getAllEdges(sourceVertex, targetVertex);
			
			if (edges == null || (edges.Count == 0))
			{
				return null;
			}
			else
			{
				return (Edge) edges[0];
			}
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			assertVertexExist(sourceVertex);
			assertVertexExist(targetVertex);
			
			if (!m_base.containsEdge(sourceVertex, targetVertex))
			{
				throw new System.ArgumentException(NO_SUCH_EDGE_IN_BASE);
			}
			
			System.Collections.IList edges = m_base.getAllEdges(sourceVertex, targetVertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
			{
				Edge e = (Edge) i.Current;
				
				if (!containsEdge(e))
				{
					m_edgeSet.Add(e);
					
					return e;
				}
			}
			
			return null;
		}
		
		
		/// <summary> Adds the specified edge to this subgraph.
		/// 
		/// </summary>
		/// <param name="e">the edge to be added.
		/// 
		/// </param>
		/// <returns> <code>true</code> if the edge was added, otherwise
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <throws>  NullPointerException </throws>
		/// <throws>  IllegalArgumentException </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="Subgraph">
		/// </seealso>
		/// <seealso cref="org._3pq.jgrapht.Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			if (e == null)
			{
				throw new System.NullReferenceException();
			}
			
			if (!m_base.containsEdge(e))
			{
				throw new System.ArgumentException(NO_SUCH_EDGE_IN_BASE);
			}
			
			assertVertexExist(e.Source);
			assertVertexExist(e.Target);
			
			if (containsEdge(e))
			{
				return false;
			}
			else
			{
				m_edgeSet.Add(e);
				
				return true;
			}
		}
		
		
		/// <summary> Adds the specified vertex to this subgraph.
		/// 
		/// </summary>
		/// <param name="v">the vertex to be added.
		/// 
		/// </param>
		/// <returns> <code>true</code> if the vertex was added, otherwise
		/// <code>false</code>.
		/// 
		/// </returns>
		/// <throws>  NullPointerException </throws>
		/// <throws>  IllegalArgumentException </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="Subgraph">
		/// </seealso>
		/// <seealso cref="org._3pq.jgrapht.Graph.addVertex(Object)">
		/// </seealso>
		public override bool addVertex(System.Object v)
		{
			if (v == null)
			{
				throw new System.NullReferenceException();
			}
			
			if (!m_base.containsVertex(v))
			{
				throw new System.ArgumentException(NO_SUCH_VERTEX_IN_BASE);
			}
			
			if (containsVertex(v))
			{
				return false;
			}
			else
			{
				m_vertexSet.Add(v);
				
				return true;
			}
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.containsEdge(Edge)">
		/// </seealso>
		public override bool containsEdge(Edge e)
		{
			return m_edgeSet.Contains(e);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.containsVertex(Object)">
		/// </seealso>
		public override bool containsVertex(System.Object v)
		{
			return m_vertexSet.Contains(v);
		}
		
		
		/// <seealso cref="UndirectedGraph.degreeOf(Object)">
		/// </seealso>
		public virtual int degreeOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			// sophisticated way to check runtime class of base ;-)
			((UndirectedGraph) m_base).degreeOf(vertex);
			
			int degree = 0;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = m_base.edgesOf(vertex).GetEnumerator(); i.MoveNext(); )
			{
				Edge e = (Edge) i.Current;
				
				if (containsEdge(e))
				{
					degree++;
					
					if (e.Source.Equals(e.Target))
					{
						degree++;
					}
				}
			}
			
			return degree;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.edgeSet()">
		/// </seealso>
		public override SupportClass.SetSupport edgeSet()
		{
			if (m_unmodifiableEdgeSet == null)
			{
				//UPGRADE_ISSUE: Method 'java.util.Collections.unmodifiableSet' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCollections'"
                m_unmodifiableEdgeSet = m_edgeSet;// Collections.unmodifiableSet(m_edgeSet);
			}
			
			return m_unmodifiableEdgeSet;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.edgesOf(Object)">
		/// </seealso>
		public override System.Collections.IList edgesOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			System.Collections.ArrayList edges = new System.Collections.ArrayList();
			System.Collections.IList baseEdges = m_base.edgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = baseEdges.GetEnumerator(); i.MoveNext(); )
			{
				Edge e = (Edge) i.Current;
				
				if (containsEdge(e))
				{
					edges.Add(e);
				}
			}
			
			return edges;
		}
		
		
		/// <seealso cref="DirectedGraph.inDegreeOf(Object)">
		/// </seealso>
		public virtual int inDegreeOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			int degree = 0;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = ((DirectedGraph) m_base).incomingEdgesOf(vertex).GetEnumerator(); i.MoveNext(); )
			{
				if (containsEdge((Edge) i.Current))
				{
					degree++;
				}
			}
			
			return degree;
		}
		
		
		/// <seealso cref="DirectedGraph.incomingEdgesOf(Object)">
		/// </seealso>
		public virtual System.Collections.IList incomingEdgesOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			System.Collections.ArrayList edges = new System.Collections.ArrayList();
			System.Collections.IList baseEdges = ((DirectedGraph) m_base).incomingEdgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = baseEdges.GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) i.Current;
				
				if (containsEdge(e))
				{
					edges.Add(e);
				}
			}
			
			return edges;
		}
		
		
		/// <seealso cref="DirectedGraph.outDegreeOf(Object)">
		/// </seealso>
		public virtual int outDegreeOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			int degree = 0;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = ((DirectedGraph) m_base).outgoingEdgesOf(vertex).GetEnumerator(); i.MoveNext(); )
			{
				if (containsEdge((Edge) i.Current))
				{
					degree++;
				}
			}
			
			return degree;
		}
		
		
		/// <seealso cref="DirectedGraph.outgoingEdgesOf(Object)">
		/// </seealso>
		public virtual System.Collections.IList outgoingEdgesOf(System.Object vertex)
		{
			assertVertexExist(vertex);
			
			System.Collections.ArrayList edges = new System.Collections.ArrayList();
			System.Collections.IList baseEdges = ((DirectedGraph) m_base).outgoingEdgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = baseEdges.GetEnumerator(); i.MoveNext(); )
			{
				Edge e = (Edge) i.Current;
				
				if (containsEdge(e))
				{
					edges.Add(e);
				}
			}
			
			return edges;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.removeEdge(Edge)">
		/// </seealso>
		public override bool removeEdge(Edge e)
		{
			System.Boolean tempBoolean;
			tempBoolean = m_edgeSet.Contains(e);
			m_edgeSet.Remove(e);
			return tempBoolean;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.removeEdge(Object, Object)">
		/// </seealso>
		public override Edge removeEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			Edge e = getEdge(sourceVertex, targetVertex);
			
			System.Boolean tempBoolean;
			tempBoolean = m_edgeSet.Contains(e);
			m_edgeSet.Remove(e);
			return tempBoolean?e:null;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.removeVertex(Object)">
		/// </seealso>
		public override bool removeVertex(System.Object v)
		{
			// If the base graph does NOT contain v it means we are here in 
			// response to removal of v from the base. In such case we don't need 
			// to remove all the edges of v as they were already removed. 
			if (containsVertex(v) && m_base.containsVertex(v))
			{
				removeAllEdges(edgesOf(v));
			}
			
			System.Boolean tempBoolean;
			tempBoolean = m_vertexSet.Contains(v);
			m_vertexSet.Remove(v);
			return tempBoolean;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Graph.vertexSet()">
		/// </seealso>
		public override SupportClass.SetSupport vertexSet()
		{
			if (m_unmodifiableVertexSet == null)
			{
				//UPGRADE_ISSUE: Method 'java.util.Collections.unmodifiableSet' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilCollections'"
                m_unmodifiableVertexSet = m_vertexSet;// Collections.unmodifiableSet(m_vertexSet);
			}
			
			return m_unmodifiableVertexSet;
		}
		
		
		private void  addEdgesUsingFilter(SupportClass.SetSupport edgeSet, SupportClass.SetSupport filter)
		{
			Edge e;
			bool containsVertices;
			bool edgeIncluded;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = edgeSet.GetEnumerator(); i.MoveNext(); )
			{
				e = (Edge)((DictionaryEntry)i.Current).Value;
				
				containsVertices = containsVertex(e.Source) && containsVertex(e.Target);
				
				// note the use of short circuit evaluation            
				edgeIncluded = (filter == null) || filter.Contains(e);
				
				if (containsVertices && edgeIncluded)
				{
					addEdge(e);
				}
			}
		}
		
		
		private void  addVerticesUsingFilter(SupportClass.SetSupport vertexSet, SupportClass.SetSupport filter)
		{
			System.Object v;
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = vertexSet.GetEnumerator(); i.MoveNext(); )
			{
				v = ((DictionaryEntry)i.Current).Value;
				
				// note the use of short circuit evaluation            
				if (filter == null || filter.Contains(v))
				{
					addVertex(v);
				}
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'BaseGraphListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> An internal listener on the base graph.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Jul 20, 2003
		/// </since>
		[Serializable]
		private class BaseGraphListener : GraphListener
		{
			public BaseGraphListener(Subgraph enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(Subgraph enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Subgraph enclosingInstance;
			public Subgraph Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			/// <seealso cref="GraphListener.edgeAdded(GraphEdgeChangeEvent)">
			/// </seealso>
			public virtual void  edgeAdded(System.Object event_sender, GraphEdgeChangeEvent e)
			{
				if (Enclosing_Instance.m_isInduced)
				{
					Enclosing_Instance.addEdge(e.getEdge());
				}
			}
			
			
			/// <seealso cref="GraphListener.edgeRemoved(GraphEdgeChangeEvent)">
			/// </seealso>
			public virtual void  edgeRemoved(System.Object event_sender, GraphEdgeChangeEvent e)
			{
				Edge edge = e.getEdge();
				
				Enclosing_Instance.removeEdge(edge);
			}
			
			
			/// <seealso cref="VertexSetListener.vertexAdded(GraphVertexChangeEvent)">
			/// </seealso>
			public virtual void  vertexAdded(System.Object event_sender, GraphVertexChangeEvent e)
			{
				// we don't care
			}
			
			
			/// <seealso cref="VertexSetListener.vertexRemoved(GraphVertexChangeEvent)">
			/// </seealso>
			public virtual void  vertexRemoved(System.Object event_sender, GraphVertexChangeEvent e)
			{
				System.Object vertex = e.getVertex();
				
				Enclosing_Instance.removeVertex(vertex);
			}
		}
	}
}
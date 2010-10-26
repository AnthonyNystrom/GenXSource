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
* AbstractBaseGraph.java
* ----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   John V. Sichi
*
* $Id: AbstractBaseGraph.java,v 1.17 2005/06/02 03:25:17 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 10-Aug-2003 : General edge refactoring (BN);
* 06-Nov-2003 : Change edge sharing semantics (JVS);
* 07-Feb-2004 : Enabled serialization (BN);
* 01-Jun-2005 : Added EdgeListFactory (JVS);
*
*/
using System;
using System.Runtime.InteropServices;
using CSGraphT;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using Edge = org._3pq.jgrapht.Edge;
using EdgeFactory = org._3pq.jgrapht.EdgeFactory;
using Graph = org._3pq.jgrapht.Graph;
using GraphHelper = org._3pq.jgrapht.GraphHelper;
using UndirectedGraph = org._3pq.jgrapht.UndirectedGraph;
using System.Collections;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> The most general implementation of the {@link org._3pq.jgrapht.Graph}
	/// interface. Its subclasses add various restrictions to get more specific
	/// graphs. The decision whether it is directed or undirected is decided at
	/// construction time and cannot be later modified (see constructor for
	/// details).
	/// 
	/// <p>
	/// This graph implementation guarantees deterministic vertex and edge set
	/// ordering (via {@link LinkedHashMap} and {@link LinkedHashSet}).
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 24, 2003
	/// </since>
	[Serializable]
	public abstract class AbstractBaseGraph : AbstractGraph, Graph, System.ICloneable
	{
		/// <summary> Returns <code>true</code> if and only if self-loops are allowed in this
		/// graph. A self loop is an edge that its source and target vertices are
		/// the same.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if and only if graph loops are allowed.
		/// </returns>
		virtual public bool AllowingLoops
		{
			get
			{
				return m_allowingLoops;
			}
			
		}
		/// <summary> Returns <code>true</code> if and only if multiple edges are allowed in
		/// this graph. The meaning of multiple edges is that there can be many
		/// edges going from vertex v1 to vertex v2.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if and only if multiple edges are allowed.
		/// </returns>
		virtual public bool AllowingMultipleEdges
		{
			get
			{
				return m_allowingMultipleEdges;
			}
			
		}
		/// <seealso cref="Graph.getEdgeFactory()">
		/// </seealso>
		override public EdgeFactory EdgeFactory
		{
			get
			{
				return m_edgeFactory;
			}
			
		}
		/// <summary> Set the {@link EdgeListFactory} to use for this graph. Initially, a
		/// graph is created with a default implementation which always supplies an
		/// {@link java.util.ArrayList} with capacity 1.
		/// 
		/// </summary>
		/// <param name="edgeListFactory">factory to use for subsequently created edge
		/// lists (this call has no effect on existing edge lists)
		/// </param>
		virtual public EdgeListFactory EdgeListFactory
		{
			set
			{
				m_edgeListFactory = value;
			}
			
		}
		private const System.String LOOPS_NOT_ALLOWED = "loops not allowed";
		
		// friendly (to improve performance)
		internal IDictionary m_vertexMap;
		internal bool m_allowingLoops;
		
		// private
		private System.Type m_factoryEdgeClass;
		private EdgeFactory m_edgeFactory;
		private EdgeListFactory m_edgeListFactory;
        private SupportClass.SetSupport m_edgeSet;

		//[NonSerialized]
		private SupportClass.SetSupport m_unmodifiableEdgeSet;
		//[NonSerialized]
        private SupportClass.SetSupport m_unmodifiableVertexSet;

		private Specifics m_specifics;
		private bool m_allowingMultipleEdges;
		
		/// <summary> Construct a new pseudograph. The pseudograph can either be directed or
		/// undirected, depending on the specified edge factory. A sample edge is
		/// created using the edge factory to see if the factory is compatible with
		/// this class of  graph. For example, if this graph is a
		/// <code>DirectedGraph</code> the edge factory must produce
		/// <code>DirectedEdge</code>s. If this is not the case, an
		/// <code>IllegalArgumentException</code> is thrown.
		/// 
		/// </summary>
		/// <param name="ef">the edge factory of the new graph.
		/// </param>
		/// <param name="allowMultipleEdges">whether to allow multiple edges or not.
		/// </param>
		/// <param name="allowLoops">whether to allow edges that are self-loops or not.
		/// 
		/// </param>
		/// <throws>  NullPointerException if the specified edge factory is </throws>
		/// <summary>         <code>null</code>.
		/// </summary>
		public AbstractBaseGraph(EdgeFactory ef, bool allowMultipleEdges, bool allowLoops)
		{
			if (ef == null)
			{
				throw new System.NullReferenceException();
			}

            m_vertexMap = new SupportClass.HashSetSupport();
            m_edgeSet = new SupportClass.HashSetSupport();
			m_edgeFactory = ef;
			m_allowingLoops = allowLoops;
			m_allowingMultipleEdges = allowMultipleEdges;
			
			m_specifics = createSpecifics();
			
			Edge e = ef.createEdge(new System.Object(), new System.Object());
			m_factoryEdgeClass = e.GetType();
			
			m_edgeListFactory = new ArrayListFactory();

            m_unmodifiableEdgeSet = null;
            m_unmodifiableVertexSet = null;
		}
		
		/// <seealso cref="Graph.getAllEdges(Object, Object)">
		/// </seealso>
		public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_specifics.getAllEdges(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.getEdge(Object, Object)">
		/// </seealso>
		public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			return m_specifics.getEdge(sourceVertex, targetVertex);
		}
		
		
		/// <seealso cref="Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			assertVertexExist(sourceVertex);
			assertVertexExist(targetVertex);
			
			if (!m_allowingMultipleEdges && containsEdge(sourceVertex, targetVertex))
			{
				return null;
			}
			
			if (!m_allowingLoops && sourceVertex.Equals(targetVertex))
			{
				throw new System.ArgumentException(LOOPS_NOT_ALLOWED);
			}
			
			Edge e = m_edgeFactory.createEdge(sourceVertex, targetVertex);
			
			if (containsEdge(e))
			{
				// this restriction should stay!
				
				return null;
			}
			else
			{
				m_edgeSet.Add(e);
				m_specifics.addEdgeToTouchingVertices(e);
				
				return e;
			}
		}
		
		
		/// <seealso cref="Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			if (e == null)
			{
				throw new System.NullReferenceException();
			}
			else if (containsEdge(e))
			{
				return false;
			}
			
			System.Object sourceVertex = e.Source;
			System.Object targetVertex = e.Target;
			
			assertVertexExist(sourceVertex);
			assertVertexExist(targetVertex);
			
			assertCompatibleWithEdgeFactory(e);
			
			if (!m_allowingMultipleEdges && containsEdge(sourceVertex, targetVertex))
			{
				return false;
			}
			
			if (!m_allowingLoops && sourceVertex.Equals(targetVertex))
			{
				throw new System.ArgumentException(LOOPS_NOT_ALLOWED);
			}
			
			m_edgeSet.Add(e);
			m_specifics.addEdgeToTouchingVertices(e);
			
			return true;
		}
		
		
		/// <seealso cref="Graph.addVertex(Object)">
		/// </seealso>
		public override bool addVertex(System.Object v)
		{
			if (v == null)
			{
				throw new System.NullReferenceException();
			}
			else if (containsVertex(v))
			{
				return false;
			}
			else
			{
				m_vertexMap[v] = null; // add with a lazy edge container entry
				
				return true;
			}
		}
		
		
		/// <summary> Returns a shallow copy of this graph instance.  Neither edges nor
		/// vertices are cloned.
		/// 
		/// </summary>
		/// <returns> a shallow copy of this set.
		/// 
		/// </returns>
		/// <throws>  RuntimeException </throws>
		/// <summary> 
		/// </summary>
		/// <seealso cref="java.lang.Object.clone()">
		/// </seealso>
		public virtual System.Object Clone()
		{
			try
			{
                AbstractBaseGraph newGraph = (AbstractBaseGraph)this.MemberwiseClone();

                newGraph.m_vertexMap = new SupportClass.HashSetSupport();
                newGraph.m_edgeSet = new SupportClass.HashSetSupport();
				newGraph.m_factoryEdgeClass = this.m_factoryEdgeClass;
				newGraph.m_edgeFactory = this.m_edgeFactory;
				newGraph.m_unmodifiableEdgeSet = null;
				newGraph.m_unmodifiableVertexSet = null;
				
				// NOTE:  it's important for this to happen in an object
				// method so that the new inner class instance gets associated with
				// the right outer class instance
				newGraph.m_specifics = newGraph.createSpecifics();
				
				GraphHelper.addGraph(newGraph, this);
				
				return newGraph;
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				SupportClass.WriteStackTrace(e, Console.Error);
				throw new System.SystemException();
			}
		}
		
		
		/// <seealso cref="Graph.containsEdge(Edge)">
		/// </seealso>
		public override bool containsEdge(Edge e)
		{
			return m_edgeSet.Contains(e);
		}
		
		
		/// <seealso cref="Graph.containsVertex(Object)">
		/// </seealso>
		public override bool containsVertex(System.Object v)
		{
			return m_vertexMap.Contains(v);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.UndirectedGraph.degreeOf(java.lang.Object)">
		/// </seealso>
		public virtual int degreeOf(System.Object vertex)
		{
			return m_specifics.degreeOf(vertex);
		}
		
		
		/// <seealso cref="Graph.edgeSet()">
		/// </seealso>
		public override SupportClass.SetSupport edgeSet()
		{
            if (m_unmodifiableEdgeSet == null && m_edgeSet != null && m_edgeSet.Count != 0)
			{
				m_unmodifiableEdgeSet = m_edgeSet;//Collections.unmodifiableSet(m_edgeSet);
			}
			
			return m_unmodifiableEdgeSet;
		}
		
		
		/// <seealso cref="Graph.edgesOf(Object)">
		/// </seealso>
		public override System.Collections.IList edgesOf(System.Object vertex)
		{
			return m_specifics.edgesOf(vertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.DirectedGraph.inDegreeOf(java.lang.Object)">
		/// </seealso>
		public virtual int inDegreeOf(System.Object vertex)
		{
			return m_specifics.inDegreeOf(vertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.DirectedGraph.incomingEdgesOf(java.lang.Object)">
		/// </seealso>
		public virtual System.Collections.IList incomingEdgesOf(System.Object vertex)
		{
			return m_specifics.incomingEdgesOf(vertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.DirectedGraph.outDegreeOf(java.lang.Object)">
		/// </seealso>
		public virtual int outDegreeOf(System.Object vertex)
		{
			return m_specifics.outDegreeOf(vertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.DirectedGraph.outgoingEdgesOf(java.lang.Object)">
		/// </seealso>
		public virtual System.Collections.IList outgoingEdgesOf(System.Object vertex)
		{
			return m_specifics.outgoingEdgesOf(vertex);
		}
		
		
		/// <seealso cref="Graph.removeEdge(Object, Object)">
		/// </seealso>
		public override Edge removeEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			Edge e = getEdge(sourceVertex, targetVertex);
			
			if (e != null)
			{
				m_specifics.removeEdgeFromTouchingVertices(e);
				m_edgeSet.Remove(e);
			}
			
			return e;
		}
		
		
		/// <seealso cref="Graph.removeEdge(Edge)">
		/// </seealso>
		public override bool removeEdge(Edge e)
		{
			if (containsEdge(e))
			{
				m_specifics.removeEdgeFromTouchingVertices(e);
				m_edgeSet.Remove(e);
				
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <seealso cref="Graph.removeVertex(Object)">
		/// </seealso>
		public override bool removeVertex(System.Object v)
		{
			if (containsVertex(v))
			{
				System.Collections.IList touchingEdgesList = edgesOf(v);
				
				// cannot iterate over list - will cause ConcurrentModificationException
				Edge[] touchingEdges = new Edge[touchingEdgesList.Count];
				SupportClass.ICollectionSupport.ToArray(touchingEdgesList, touchingEdges);
				
				removeAllEdges(touchingEdges);
				
				m_vertexMap.Remove(v); // remove the vertex itself
				
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <seealso cref="Graph.vertexSet()">
		/// </seealso>
		public override SupportClass.SetSupport vertexSet()
		{
            if (m_unmodifiableVertexSet == null && m_vertexMap.Keys != null && m_vertexMap.Keys.Count != 0)
			{
				m_unmodifiableVertexSet = new SupportClass.HashSetSupport(m_vertexMap.Keys);//Collections.unmodifiableSet(new SupportClass.HashSetSupport(m_vertexMap.Keys));
			}
			
			return m_unmodifiableVertexSet;
		}
		
		
		private bool assertCompatibleWithEdgeFactory(Edge e)
		{
			if (e == null)
			{
				throw new System.NullReferenceException();
			}
			else if (!m_factoryEdgeClass.IsInstanceOfType(e))
			{
				throw new System.InvalidCastException("incompatible edge class");
			}
			
			return true;
		}
		
		
		private Specifics createSpecifics()
		{
			if (this is DirectedGraph)
			{
				return new DirectedSpecifics(this);
			}
			else if (this is UndirectedGraph)
			{
				return new UndirectedSpecifics(this);
			}
			else
			{
				throw new System.ArgumentException("must be instance of either DirectedGraph or UndirectedGraph");
			}
		}
		
		/// <summary> .
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// </author>
		[Serializable]
		private abstract class Specifics
		{
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="sourceVertex">
			/// </param>
			/// <param name="targetVertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="sourceVertex">
			/// </param>
			/// <param name="targetVertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract Edge getEdge(System.Object sourceVertex, System.Object targetVertex);
			
			
			/// <summary> Adds the specified edge to the edge containers of its source and
			/// target vertices.
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public abstract void  addEdgeToTouchingVertices(Edge e);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract int degreeOf(System.Object vertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract System.Collections.IList edgesOf(System.Object vertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract int inDegreeOf(System.Object vertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract System.Collections.IList incomingEdgesOf(System.Object vertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract int outDegreeOf(System.Object vertex);
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="vertex">*
			/// </param>
			/// <returns>
			/// </returns>
			public abstract System.Collections.IList outgoingEdgesOf(System.Object vertex);
			
			
			/// <summary> Removes the specified edge from the edge containers of its source
			/// and target vertices.
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public abstract void  removeEdgeFromTouchingVertices(Edge e);
		}
		
		
		private class ArrayListFactory : EdgeListFactory
		{
			/// <seealso cref="EdgeListFactory.createEdgeList">
			/// </seealso>
			public virtual System.Collections.IList createEdgeList(System.Object vertex)
			{
				// NOTE:  use size 1 to keep memory usage under control
				// for the common case of vertices with low degree
				return new System.Collections.ArrayList(1);
			}
		}
		
		
		/// <summary> A container of for vertex edges.
		/// 
		/// <p>
		/// In this edge container we use array lists to minimize memory toll.
		/// However, for high-degree vertices we replace the entire edge container
		/// with a direct access subclass (to be implemented).
		/// </p>
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// </author>
		[Serializable]
		private class DirectedEdgeContainer
		{
			/// <summary> A lazy build of unmodifiable incoming edge list.
			/// 
			/// </summary>
			/// <returns>
			/// </returns>
			virtual public System.Collections.IList UnmodifiableIncomingEdges
			{
				get
				{
					if (m_unmodifiableIncoming == null)
					{
						m_unmodifiableIncoming = (System.Collections.IList) System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList(m_incoming));
					}
					
					return m_unmodifiableIncoming;
				}
				
			}
			/// <summary> A lazy build of unmodifiable outgoing edge list.
			/// 
			/// </summary>
			/// <returns>
			/// </returns>
			virtual public System.Collections.IList UnmodifiableOutgoingEdges
			{
				get
				{
					if (m_unmodifiableOutgoing == null)
					{
						m_unmodifiableOutgoing = (System.Collections.IList) System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList(m_outgoing));
					}
					
					return m_unmodifiableOutgoing;
				}
				
			}
			internal System.Collections.IList m_incoming;
			internal System.Collections.IList m_outgoing;
			[NonSerialized]
			private System.Collections.IList m_unmodifiableIncoming = null;
			[NonSerialized]
			private System.Collections.IList m_unmodifiableOutgoing = null;
			
			internal DirectedEdgeContainer(EdgeListFactory edgeListFactory, System.Object vertex)
			{
				m_incoming = edgeListFactory.createEdgeList(vertex);
				m_outgoing = edgeListFactory.createEdgeList(vertex);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  addIncomingEdge(Edge e)
			{
				m_incoming.Add(e);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  addOutgoingEdge(Edge e)
			{
				m_outgoing.Add(e);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  removeIncomingEdge(Edge e)
			{
				m_incoming.Remove(e);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  removeOutgoingEdge(Edge e)
			{
				m_outgoing.Remove(e);
			}
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'DirectedSpecifics' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> .
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// </author>
		[Serializable]
		private class DirectedSpecifics:Specifics
		{
			public DirectedSpecifics(AbstractBaseGraph enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AbstractBaseGraph enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AbstractBaseGraph enclosingInstance;
			public AbstractBaseGraph Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private const System.String NOT_IN_DIRECTED_GRAPH = "no such operation in a directed graph";
			
			/// <seealso cref="Graph.getAllEdges(Object, Object)">
			/// </seealso>
			public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
			{
				System.Collections.IList edges = null;
				
				if (Enclosing_Instance.containsVertex(sourceVertex) && Enclosing_Instance.containsVertex(targetVertex))
				{
					edges = new System.Collections.ArrayList();
					
					DirectedEdgeContainer ec = getEdgeContainer(sourceVertex);
					
					System.Collections.IEnumerator iter = ec.m_outgoing.GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (iter.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge e = (Edge) iter.Current;
						
						if (e.Target.Equals(targetVertex))
						{
							edges.Add(e);
						}
					}
				}
				
				return edges;
			}
			
			
			/// <seealso cref="Graph.getEdge(Object, Object)">
			/// </seealso>
			public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
			{
				if (Enclosing_Instance.containsVertex(sourceVertex) && Enclosing_Instance.containsVertex(targetVertex))
				{
					DirectedEdgeContainer ec = getEdgeContainer(sourceVertex);
					
					System.Collections.IEnumerator iter = ec.m_outgoing.GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (iter.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge e = (Edge) iter.Current;
						
						if (e.Target.Equals(targetVertex))
						{
							return e;
						}
					}
				}
				
				return null;
			}
			
			
			/// <seealso cref="AbstractBaseGraph.addEdgeToTouchingVertices(Edge)">
			/// </seealso>
			public override void  addEdgeToTouchingVertices(Edge e)
			{
				System.Object source = e.Source;
				System.Object target = e.Target;
				
				getEdgeContainer(source).addOutgoingEdge(e);
				getEdgeContainer(target).addIncomingEdge(e);
			}
			
			
			/// <seealso cref="UndirectedGraph.degreeOf(Object)">
			/// </seealso>
			public override int degreeOf(System.Object vertex)
			{
				throw new System.NotSupportedException(NOT_IN_DIRECTED_GRAPH);
			}
			
			
			/// <seealso cref="Graph.edgesOf(Object)">
			/// </seealso>
			public override System.Collections.IList edgesOf(System.Object vertex)
			{
				System.Collections.ArrayList inAndOut = new System.Collections.ArrayList(getEdgeContainer(vertex).m_incoming);
				inAndOut.AddRange(getEdgeContainer(vertex).m_outgoing);
				
				// we have two copies for each self-loop - remove one of them.
				if (Enclosing_Instance.m_allowingLoops)
				{
					System.Collections.IList loops = getAllEdges(vertex, vertex);
					
					for (int i = 0; i < inAndOut.Count; )
					{
						System.Object e = inAndOut[i];
						
						if (loops.Contains(e))
						{
							inAndOut.RemoveAt(i);
							loops.Remove(e); // so we remove it only once
						}
						else
						{
							i++;
						}
					}
				}
				
				return inAndOut;
			}
			
			
			/// <seealso cref="DirectedGraph.inDegree(Object)">
			/// </seealso>
			public override int inDegreeOf(System.Object vertex)
			{
				return getEdgeContainer(vertex).m_incoming.Count;
			}
			
			
			/// <seealso cref="DirectedGraph.incomingEdges(Object)">
			/// </seealso>
			public override System.Collections.IList incomingEdgesOf(System.Object vertex)
			{
				return getEdgeContainer(vertex).UnmodifiableIncomingEdges;
			}
			
			
			/// <seealso cref="DirectedGraph.outDegree(Object)">
			/// </seealso>
			public override int outDegreeOf(System.Object vertex)
			{
				return getEdgeContainer(vertex).m_outgoing.Count;
			}
			
			
			/// <seealso cref="DirectedGraph.outgoingEdges(Object)">
			/// </seealso>
			public override System.Collections.IList outgoingEdgesOf(System.Object vertex)
			{
				return getEdgeContainer(vertex).UnmodifiableOutgoingEdges;
			}
			
			
			/// <seealso cref="AbstractBaseGraph.removeEdgeFromTouchingVertices(Edge)">
			/// </seealso>
			public override void  removeEdgeFromTouchingVertices(Edge e)
			{
				System.Object source = e.Source;
				System.Object target = e.Target;
				
				getEdgeContainer(source).removeOutgoingEdge(e);
				getEdgeContainer(target).removeIncomingEdge(e);
			}
			
			
			/// <summary> A lazy build of edge container for specified vertex.
			/// 
			/// </summary>
			/// <param name="vertex">a vertex in this graph.
			/// 
			/// </param>
			/// <returns> EdgeContainer
			/// </returns>
			private DirectedEdgeContainer getEdgeContainer(System.Object vertex)
			{
				Enclosing_Instance.assertVertexExist(vertex);
				
				DirectedEdgeContainer ec = (DirectedEdgeContainer) Enclosing_Instance.m_vertexMap[vertex];
				
				if (ec == null)
				{
					ec = new DirectedEdgeContainer(Enclosing_Instance.m_edgeListFactory, vertex);
					Enclosing_Instance.m_vertexMap[vertex] = ec;
				}
				
				return ec;
			}
		}
		
		
		/// <summary> A container of for vertex edges.
		/// 
		/// <p>
		/// In this edge container we use array lists to minimize memory toll.
		/// However, for high-degree vertices we replace the entire edge container
		/// with a direct access subclass (to be implemented).
		/// </p>
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// </author>
		[Serializable]
		private class UndirectedEdgeContainer
		{
			/// <summary> A lazy build of unmodifiable list of vertex edges
			/// 
			/// </summary>
			/// <returns>
			/// </returns>
			virtual public System.Collections.IList UnmodifiableVertexEdges
			{
				get
				{
					if (m_unmodifiableVertexEdges == null)
					{
						m_unmodifiableVertexEdges = (System.Collections.IList) System.Collections.ArrayList.ReadOnly(new System.Collections.ArrayList(m_vertexEdges));
					}
					
					return m_unmodifiableVertexEdges;
				}
				
			}
			internal System.Collections.IList m_vertexEdges;
			[NonSerialized]
			private System.Collections.IList m_unmodifiableVertexEdges = null;
			
			internal UndirectedEdgeContainer(EdgeListFactory edgeListFactory, System.Object vertex)
			{
				m_vertexEdges = edgeListFactory.createEdgeList(vertex);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  addEdge(Edge e)
			{
				m_vertexEdges.Add(e);
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <returns>
			/// </returns>
			public virtual int edgeCount()
			{
				return m_vertexEdges.Count;
			}
			
			
			/// <summary> .
			/// 
			/// </summary>
			/// <param name="e">
			/// </param>
			public virtual void  removeEdge(Edge e)
			{
				m_vertexEdges.Remove(e);
			}
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'UndirectedSpecifics' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> .
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// </author>
		[Serializable]
		private class UndirectedSpecifics:Specifics
		{
			public UndirectedSpecifics(AbstractBaseGraph enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}
			private void  InitBlock(AbstractBaseGraph enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private AbstractBaseGraph enclosingInstance;
			public AbstractBaseGraph Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private const System.String NOT_IN_UNDIRECTED_GRAPH = "no such operation in an undirected graph";
			
			/// <seealso cref="Graph.getAllEdges(Object, Object)">
			/// </seealso>
			public override System.Collections.IList getAllEdges(System.Object sourceVertex, System.Object targetVertex)
			{
				System.Collections.IList edges = null;
				
				if (Enclosing_Instance.containsVertex(sourceVertex) && Enclosing_Instance.containsVertex(targetVertex))
				{
					edges = new System.Collections.ArrayList();
					
					System.Collections.IEnumerator iter = getEdgeContainer(sourceVertex).m_vertexEdges.GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (iter.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge e = (Edge) iter.Current;
						
						bool equalStraight = sourceVertex.Equals(e.Source) && targetVertex.Equals(e.Target);
						
						bool equalInverted = sourceVertex.Equals(e.Target) && targetVertex.Equals(e.Source);
						
						if (equalStraight || equalInverted)
						{
							edges.Add(e);
						}
					}
				}
				
				return edges;
			}
			
			
			/// <seealso cref="Graph.getEdge(Object, Object)">
			/// </seealso>
			public override Edge getEdge(System.Object sourceVertex, System.Object targetVertex)
			{
				if (Enclosing_Instance.containsVertex(sourceVertex) && Enclosing_Instance.containsVertex(targetVertex))
				{
					System.Collections.IEnumerator iter = getEdgeContainer(sourceVertex).m_vertexEdges.GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (iter.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge e = (Edge) iter.Current;
						
						bool equalStraight = sourceVertex.Equals(e.Source) && targetVertex.Equals(e.Target);
						
						bool equalInverted = sourceVertex.Equals(e.Target) && targetVertex.Equals(e.Source);
						
						if (equalStraight || equalInverted)
						{
							return e;
						}
					}
				}
				
				return null;
			}
			
			
			/// <seealso cref="AbstractBaseGraph.addEdgeToTouchingVertices(Edge)">
			/// </seealso>
			public override void  addEdgeToTouchingVertices(Edge e)
			{
				System.Object source = e.Source;
				System.Object target = e.Target;
				
				getEdgeContainer(source).addEdge(e);
				
				if (source != target)
				{
					getEdgeContainer(target).addEdge(e);
				}
			}
			
			
			/// <seealso cref="UndirectedGraph.degree(Object)">
			/// </seealso>
			public override int degreeOf(System.Object vertex)
			{
				if (Enclosing_Instance.m_allowingLoops)
				{
					// then we must count, and add loops twice
					
					int degree = 0;
					System.Collections.IList edges = getEdgeContainer(vertex).m_vertexEdges;
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					for (System.Collections.IEnumerator iter = edges.GetEnumerator(); iter.MoveNext(); )
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge e = (Edge) iter.Current;
						
						if (e.Source.Equals(e.Target))
						{
							degree += 2;
						}
						else
						{
							degree += 1;
						}
					}
					
					return degree;
				}
				else
				{
					return getEdgeContainer(vertex).edgeCount();
				}
			}
			
			
			/// <seealso cref="Graph.edges(Object)">
			/// </seealso>
			public override System.Collections.IList edgesOf(System.Object vertex)
			{
				return getEdgeContainer(vertex).UnmodifiableVertexEdges;
			}
			
			
			/// <seealso cref="DirectedGraph.inDegreeOf(Object)">
			/// </seealso>
			public override int inDegreeOf(System.Object vertex)
			{
				throw new System.NotSupportedException(NOT_IN_UNDIRECTED_GRAPH);
			}
			
			
			/// <seealso cref="DirectedGraph.incomingEdgesOf(Object)">
			/// </seealso>
			public override System.Collections.IList incomingEdgesOf(System.Object vertex)
			{
				throw new System.NotSupportedException(NOT_IN_UNDIRECTED_GRAPH);
			}
			
			
			/// <seealso cref="DirectedGraph.outDegreeOf(Object)">
			/// </seealso>
			public override int outDegreeOf(System.Object vertex)
			{
				throw new System.NotSupportedException(NOT_IN_UNDIRECTED_GRAPH);
			}
			
			
			/// <seealso cref="DirectedGraph.outgoingEdgesOf(Object)">
			/// </seealso>
			public override System.Collections.IList outgoingEdgesOf(System.Object vertex)
			{
				throw new System.NotSupportedException(NOT_IN_UNDIRECTED_GRAPH);
			}
			
			
			/// <seealso cref="AbstractBaseGraph.removeEdgeFromTouchingVertices(Edge)">
			/// </seealso>
			public override void  removeEdgeFromTouchingVertices(Edge e)
			{
				System.Object source = e.Source;
				System.Object target = e.Target;
				
				getEdgeContainer(source).removeEdge(e);
				
				if (source != target)
				{
					getEdgeContainer(target).removeEdge(e);
				}
			}
			
			
			/// <summary> A lazy build of edge container for specified vertex.
			/// 
			/// </summary>
			/// <param name="vertex">a vertex in this graph.
			/// 
			/// </param>
			/// <returns> EdgeContainer
			/// </returns>
			private UndirectedEdgeContainer getEdgeContainer(System.Object vertex)
			{
				Enclosing_Instance.assertVertexExist(vertex);
				
				UndirectedEdgeContainer ec = (UndirectedEdgeContainer) Enclosing_Instance.m_vertexMap[vertex];
				
				if (ec == null)
				{
					ec = new UndirectedEdgeContainer(Enclosing_Instance.m_edgeListFactory, vertex);
					Enclosing_Instance.m_vertexMap[vertex] = ec;
				}
				
				return ec;
			}
		}
	}
}
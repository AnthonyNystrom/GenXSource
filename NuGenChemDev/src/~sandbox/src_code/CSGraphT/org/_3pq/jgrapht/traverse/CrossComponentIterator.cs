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
/* ---------------------------
* CrossComponentIterator.java
* ---------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   John V. Sichi
*
* $Id: CrossComponentIterator.java,v 1.6 2005/04/23 08:09:29 perfecthash Exp $
*
* Changes
* -------
* 31-Jul-2003 : Initial revision (BN);
* 11-Aug-2003 : Adaptation to new event model (BN);
* 31-Jan-2004 : Extracted cross-component traversal functionality (BN);
*
*/
using System;
using org._3pq.jgrapht.event_Renamed;
using System.Collections;

namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> Provides a cross-connected-component traversal functionality for iterator
	/// subclasses.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jan 31, 2004
	/// </since>
	public abstract class CrossComponentIterator : AbstractGraphIterator
	{
		private void  InitBlock()
		{
			m_ccFinishedEvent = new ConnectedComponentTraversalEvent(this, ConnectedComponentTraversalEvent.CONNECTED_COMPONENT_FINISHED);
			m_ccStartedEvent = new ConnectedComponentTraversalEvent(this, ConnectedComponentTraversalEvent.CONNECTED_COMPONENT_STARTED);
		}
		/// <seealso cref="java.util.Iterator.next()">
		/// </seealso>
		public override System.Object Current
		{
			get
			{
				if (m_startVertex != null)
				{
					encounterStartVertex();
				}
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				if (MoveNext())
				{
					if (m_state == CCS_BEFORE_COMPONENT)
					{
						m_state = CCS_WITHIN_COMPONENT;
						fireConnectedComponentStarted(m_ccStartedEvent);
					}
					
					System.Object nextVertex = provideNextVertex();
					fireVertexTraversed(createVertexTraversalEvent(nextVertex));
					
					addUnseenChildrenOf(nextVertex);
					
					return nextVertex;
				}
				else
				{
					throw new System.ArgumentOutOfRangeException();
				}
			}
			
		}
		/// <summary> Returns <tt>true</tt> if there are no more uniterated vertices in the
		/// currently iterated connected component; <tt>false</tt> otherwise.
		/// 
		/// </summary>
		/// <returns> <tt>true</tt> if there are no more uniterated vertices in the
		/// currently iterated connected component; <tt>false</tt>
		/// otherwise.
		/// </returns>
		protected internal abstract bool ConnectedComponentExhausted{get;}
		private const int CCS_BEFORE_COMPONENT = 1;
		private const int CCS_WITHIN_COMPONENT = 2;
		private const int CCS_AFTER_COMPONENT = 3;
		
		//
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_ccFinishedEvent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'm_ccFinishedEvent' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ConnectedComponentTraversalEvent m_ccFinishedEvent;
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_ccStartedEvent '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		//UPGRADE_NOTE: The initialization of  'm_ccStartedEvent' was moved to method 'InitBlock'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1005'"
		private ConnectedComponentTraversalEvent m_ccStartedEvent;
		
		// TODO: support ConcurrentModificationException if graph modified 
		// during iteration. 
		private FlyweightEdgeEvent m_reusableEdgeEvent;
		private FlyweightVertexEvent m_reusableVertexEvent;
		private System.Collections.IEnumerator m_vertexIterator = null;
		
		/// <summary> Stores the vertices that have been seen during iteration and
		/// (optionally) some additional traversal info regarding each vertex.
		/// </summary>
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		private System.Collections.IDictionary m_seen = new System.Collections.Hashtable();
		private System.Object m_startVertex;
		private Specifics m_specifics;
		
		/// <summary>The connected component state </summary>
		private int m_state = CCS_BEFORE_COMPONENT;
		
		/// <summary> Creates a new iterator for the specified graph. Iteration will start at
		/// the specified start vertex. If the specified start vertex is
		/// <code>null</code>, Iteration will start at an arbitrary graph vertex.
		/// 
		/// </summary>
		/// <param name="g">the graph to be iterated.
		/// </param>
		/// <param name="startVertex">the vertex iteration to be started.
		/// 
		/// </param>
		/// <throws>  NullPointerException </throws>
		/// <throws>  IllegalArgumentException </throws>
		public CrossComponentIterator(Graph g, System.Object startVertex)
		{
			InitBlock();
			
			if (g == null)
			{
				throw new System.NullReferenceException("graph must not be null");
			}
			
			m_specifics = createGraphSpecifics(g);
			m_vertexIterator = g.vertexSet().GetEnumerator();
			setCrossComponentTraversal(startVertex == null);
			
			m_reusableEdgeEvent = new FlyweightEdgeEvent(this, null);
			m_reusableVertexEvent = new FlyweightVertexEvent(this, (System.Object) null);
			
			if (startVertex == null)
			{
				// pick a start vertex if graph not empty 
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				if (m_vertexIterator.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					m_startVertex = ((DictionaryEntry)m_vertexIterator.Current).Value;
				}
				else
				{
					m_startVertex = null;
				}
			}
			else if (g.containsVertex(startVertex))
			{
				m_startVertex = startVertex;
			}
			else
			{
				throw new System.ArgumentException("graph must contain the start vertex");
			}
		}
		
		/// <seealso cref="java.util.Iterator.hasNext()">
		/// </seealso>
		public override bool MoveNext()
		{
			if (m_startVertex != null)
			{
				encounterStartVertex();
			}
			
			if (ConnectedComponentExhausted)
			{
				if (m_state == CCS_WITHIN_COMPONENT)
				{
					m_state = CCS_AFTER_COMPONENT;
					fireConnectedComponentFinished(m_ccFinishedEvent);
				}
				
				if (isCrossComponentTraversal())
				{
					while (m_vertexIterator.MoveNext())
					{
						System.Object v = ((DictionaryEntry)m_vertexIterator.Current).Value;
						
						if (!isSeenVertex(v))
						{
							encounterVertex(v, null);
							m_state = CCS_BEFORE_COMPONENT;
							
							return true;
						}
					}
					
					return false;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}

        public override void Reset()
        {
            m_vertexIterator.Reset();
            // ^ Problem?
        }
		
		/// <summary> Update data structures the first time we see a vertex.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex encountered
		/// </param>
		/// <param name="edge">the edge via which the vertex was encountered, or null if
		/// the vertex is a starting point
		/// </param>
		protected internal abstract void  encounterVertex(System.Object vertex, Edge edge);
		
		
		/// <summary> Returns the vertex to be returned in the following call to the iterator
		/// <code>next</code> method.
		/// 
		/// </summary>
		/// <returns> the next vertex to be returned by this iterator.
		/// </returns>
		protected internal abstract System.Object provideNextVertex();
		
		
		/// <summary> Access the data stored for a seen vertex.
		/// 
		/// </summary>
		/// <param name="vertex">a vertex which has already been seen.
		/// 
		/// </param>
		/// <returns> data associated with the seen vertex or <code>null</code> if no
		/// data was associated with the vertex. A <code>null</code> return
		/// can also indicate that the vertex was explicitly associated
		/// with <code>null</code>.
		/// </returns>
		protected internal virtual System.Object getSeenData(System.Object vertex)
		{
			return m_seen[vertex];
		}
		
		
		/// <summary> Determines whether a vertex has been seen yet by this traversal.
		/// 
		/// </summary>
		/// <param name="vertex">vertex in question
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if vertex has already been seen
		/// </returns>
		protected internal virtual bool isSeenVertex(System.Object vertex)
		{
			return m_seen.Contains(vertex);
		}
		
		
		/// <summary> Called whenever we re-encounter a vertex.  The default implementation
		/// does nothing.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex re-encountered
		/// </param>
		/// <param name="edge">the edge via which the vertex was re-encountered
		/// </param>
		protected internal abstract void  encounterVertexAgain(System.Object vertex, Edge edge);
		
		
		/// <summary> Stores iterator-dependent data for a vertex that has been seen.
		/// 
		/// </summary>
		/// <param name="vertex">a vertex which has been seen.
		/// </param>
		/// <param name="data">data to be associated with the seen vertex.
		/// 
		/// </param>
		/// <returns> previous value associated with specified vertex or
		/// <code>null</code> if no data was associated with the vertex. A
		/// <code>null</code> return can also indicate that the vertex was
		/// explicitly associated with <code>null</code>.
		/// </returns>
		protected internal virtual System.Object putSeenData(System.Object vertex, System.Object data)
		{
			System.Object tempObject;
			tempObject = m_seen[vertex];
			m_seen[vertex] = data;
			return tempObject;
		}
		
		
		internal static Specifics createGraphSpecifics(Graph g)
		{
			if (g is DirectedGraph)
			{
				return new DirectedSpecifics((DirectedGraph) g);
			}
			else
			{
				return new UndirectedSpecifics(g);
			}
		}
		
		
		private void  addUnseenChildrenOf(System.Object vertex)
		{
			System.Collections.IList edges = m_specifics.edgesOf(vertex);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) i.Current;
				fireEdgeTraversed(createEdgeTraversalEvent(e));
				
				System.Object v = e.oppositeVertex(vertex);
				
				if (isSeenVertex(v))
				{
					encounterVertexAgain(v, e);
				}
				else
				{
					encounterVertex(v, e);
				}
			}
		}
		
		
		private EdgeTraversalEvent createEdgeTraversalEvent(Edge edge)
		{
			if (ReuseEvents)
			{
				m_reusableEdgeEvent.setEdge(edge);
				
				return m_reusableEdgeEvent;
			}
			else
			{
				return new EdgeTraversalEvent(this, edge);
			}
		}
		
		
		private VertexTraversalEvent createVertexTraversalEvent(System.Object vertex)
		{
			if (ReuseEvents)
			{
				m_reusableVertexEvent.setVertex(vertex);
				
				return m_reusableVertexEvent;
			}
			else
			{
				return new VertexTraversalEvent(this, vertex);
			}
		}
		
		
		private void encounterStartVertex()
		{
			encounterVertex(m_startVertex, null);
			m_startVertex = null;
		}
		
		internal interface SimpleContainer
		{
			/// <summary> Tests if this container is empty.
			/// 
			/// </summary>
			/// <returns> <code>true</code> if empty, otherwise <code>false</code>.
			/// </returns>
			bool Empty
			{
				get;
				
			}
			
			
			/// <summary> Adds the specified object to this container.
			/// 
			/// </summary>
			/// <param name="o">the object to be added.
			/// </param>
			void  add(System.Object o);
			
			
			/// <summary> Remove an object from this container and return it.
			/// 
			/// </summary>
			/// <returns> the object removed from this container.
			/// </returns>
			System.Object remove();
		}
		
		/// <summary> Provides unified interface for operations that are different in directed
		/// graphs and in undirected graphs.
		/// </summary>
		internal abstract class Specifics
		{
			/// <summary> Returns the edges outgoing from the specified vertex in case of
			/// directed graph, and the edge touching the specified vertex in case
			/// of undirected graph.
			/// 
			/// </summary>
			/// <param name="vertex">the vertex whose outgoing edges are to be returned.
			/// 
			/// </param>
			/// <returns> the edges outgoing from the specified vertex in case of
			/// directed graph, and the edge touching the specified vertex
			/// in case of undirected graph.
			/// </returns>
			public abstract System.Collections.IList edgesOf(System.Object vertex);
		}
		
		
		/// <summary> A reusable edge event.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 11, 2003
		/// </since>
		[Serializable]
		internal class FlyweightEdgeEvent:EdgeTraversalEvent
		{
			private const long serialVersionUID = 4051327833765000755L;
			
			/// <seealso cref="EdgeTraversalEvent">
			/// </seealso>
			public FlyweightEdgeEvent(System.Object eventSource, Edge edge):base(eventSource, edge)
			{
			}
			
			/// <summary> Sets the edge of this event.
			/// 
			/// </summary>
			/// <param name="edge">the edge to be set.
			/// </param>
			protected internal virtual void  setEdge(Edge edge)
			{
				m_edge = edge;
			}
		}
		
		
		/// <summary> A reusable vertex event.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 11, 2003
		/// </since>
		[Serializable]
		internal class FlyweightVertexEvent:VertexTraversalEvent
		{
			private const long serialVersionUID = 3834024753848399924L;
			
			/// <seealso cref="VertexTraversalEvent">
			/// </seealso>
			public FlyweightVertexEvent(System.Object eventSource, System.Object vertex):base(eventSource, vertex)
			{
			}
			
			/// <summary> Sets the vertex of this event.
			/// 
			/// </summary>
			/// <param name="vertex">the vertex to be set.
			/// </param>
			protected internal virtual void  setVertex(System.Object vertex)
			{
				m_vertex = vertex;
			}
		}
		
		
		/// <summary> An implementation of {@link TraverseUtils.Specifics} for a directed
		/// graph.
		/// </summary>
		private class DirectedSpecifics:Specifics
		{
			private DirectedGraph m_graph;
			
			/// <summary> Creates a new DirectedSpecifics object.
			/// 
			/// </summary>
			/// <param name="g">the graph for which this specifics object to be created.
			/// </param>
			public DirectedSpecifics(DirectedGraph g)
			{
				m_graph = g;
			}
			
			/// <seealso cref="CrossComponentIterator.Specifics.edgesOf(Object)">
			/// </seealso>
			public override System.Collections.IList edgesOf(System.Object vertex)
			{
				return m_graph.outgoingEdgesOf(vertex);
			}
		}
		
		
		/// <summary> An implementation of {@link TraverseUtils.Specifics} in which edge
		/// direction (if any) is ignored.
		/// </summary>
		private class UndirectedSpecifics:Specifics
		{
			private Graph m_graph;
			
			/// <summary> Creates a new UndirectedSpecifics object.
			/// 
			/// </summary>
			/// <param name="g">the graph for which this specifics object to be created.
			/// </param>
			public UndirectedSpecifics(Graph g)
			{
				m_graph = g;
			}
			
			/// <seealso cref="CrossComponentIterator.Specifics.edgesOf(Object)">
			/// </seealso>
			public override System.Collections.IList edgesOf(System.Object vertex)
			{
				return m_graph.edgesOf(vertex);
			}
		}
	}
}
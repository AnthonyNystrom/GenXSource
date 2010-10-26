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
* DefaultListenableGraph.java
* ---------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: DefaultListenableGraph.java,v 1.13 2005/04/23 08:09:29 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 04-Aug-2003 : Strong refs to listeners instead of weak refs (BN);
* 10-Aug-2003 : Adaptation to new event model (BN);
* 07-Mar-2004 : Fixed unnecessary clone bug #819075 (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
using ListenableGraph = org._3pq.jgrapht.ListenableGraph;
using GraphEdgeChangeEvent = org._3pq.jgrapht.event_Renamed.GraphEdgeChangeEvent;
using GraphListener = org._3pq.jgrapht.event_Renamed.GraphListener;
using GraphVertexChangeEvent = org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent;
using VertexSetListener = org._3pq.jgrapht.event_Renamed.VertexSetListener;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A graph backed by the the graph specified at the constructor, which can be
	/// listened by <code>GraphListener</code>s and by
	/// <code>VertexSetListener</code>s. Operations on this graph "pass through" to
	/// the to the backing graph. Any modification made to this graph or the
	/// backing graph is reflected by the other.
	/// 
	/// <p>
	/// This graph does <i>not</i> pass the hashCode and equals operations through
	/// to the backing graph, but relies on <tt>Object</tt>'s <tt>equals</tt> and
	/// <tt>hashCode</tt> methods.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <seealso cref="GraphListener">
	/// </seealso>
	/// <seealso cref="VertexSetListener">
	/// </seealso>
	/// <since> Jul 20, 2003
	/// </since>
	[Serializable]
	public class DefaultListenableGraph:GraphDelegator, ListenableGraph, System.ICloneable
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Tests whether the <code>reuseEvents</code> flag is set. If the flag is
		/// set to <code>true</code> this class will reuse previously fired events
		/// and will not create a new object for each event. This option increases
		/// performance but should be used with care, especially in multithreaded
		/// environment.
		/// 
		/// </summary>
		/// <returns> the value of the <code>reuseEvents</code> flag.
		/// </returns>
		/// <summary> If the <code>reuseEvents</code> flag is set to <code>true</code> this
		/// class will reuse previously fired events and will not create a new
		/// object for each event. This option increases performance but should be
		/// used with care, especially in multithreaded environment.
		/// 
		/// </summary>
		/// <param name="reuseEvents">whether to reuse previously fired event objects
		/// instead of creating a new event object for each event.
		/// </param>
		virtual public bool ReuseEvents
		{
			get
			{
				return m_reuseEvents;
			}
			
			set
			{
				m_reuseEvents = value;
			}
			
		}
		private const long serialVersionUID = 3977575900898471984L;
		private System.Collections.ArrayList m_graphListeners = new System.Collections.ArrayList();
		private System.Collections.ArrayList m_vertexSetListeners = new System.Collections.ArrayList();
		private FlyweightEdgeEvent m_reuseableEdgeEvent;
		private FlyweightVertexEvent m_reuseableVertexEvent;
		private bool m_reuseEvents;
		
		/// <summary> Creates a new listenable graph.
		/// 
		/// </summary>
		/// <param name="g">the backing graph.
		/// </param>
		public DefaultListenableGraph(Graph g):this(g, false)
		{
		}
		
		
		/// <summary> Creates a new listenable graph. If the <code>reuseEvents</code> flag is
		/// set to <code>true</code> this class will reuse previously fired events
		/// and will not create a new object for each event. This option increases
		/// performance but should be used with care, especially in multithreaded
		/// environment.
		/// 
		/// </summary>
		/// <param name="g">the backing graph.
		/// </param>
		/// <param name="reuseEvents">whether to reuse previously fired event objects
		/// instead of creating a new event object for each event.
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException if the backing graph is already a </throws>
		/// <summary>         listenable graph.
		/// </summary>
		public DefaultListenableGraph(Graph g, bool reuseEvents):base(g)
		{
			m_reuseEvents = reuseEvents;
			m_reuseableEdgeEvent = new FlyweightEdgeEvent(this, - 1, null);
			m_reuseableVertexEvent = new FlyweightVertexEvent(this, - 1, (System.Object) null);
			
			// the following restriction could be probably relaxed in the future.
			if (g is ListenableGraph)
			{
				throw new System.ArgumentException("base graph cannot be listenable");
			}
		}
		
		
		/// <seealso cref="Graph.addEdge(Object, Object)">
		/// </seealso>
		public override Edge addEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			Edge e = base.addEdge(sourceVertex, targetVertex);
			
			if (e != null)
			{
				fireEdgeAdded(e);
			}
			
			return e;
		}
		
		
		/// <seealso cref="Graph.addEdge(Edge)">
		/// </seealso>
		public override bool addEdge(Edge e)
		{
			bool modified = base.addEdge(e);
			
			if (modified)
			{
				fireEdgeAdded(e);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="ListenableGraph.addGraphListener(GraphListener)">
		/// </seealso>
		public event org._3pq.jgrapht.event_Renamed.GraphListenerDelegate GraphListenerDelegateVar;
		protected virtual void  OnGraph(org._3pq.jgrapht.event_Renamed.GraphEdgeChangeEvent eventParam)
		{
			if (GraphListenerDelegateVar != null)
				GraphListenerDelegateVar(this, eventParam);
		}
		public event org._3pq.jgrapht.event_Renamed.GraphListenerDelegate2 GraphListenerDelegate2Var;
		protected virtual void  OnGraph(org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent eventParam)
		{
			if (GraphListenerDelegate2Var != null)
				GraphListenerDelegate2Var(this, eventParam);
		}
		public virtual void  addGraphListener(GraphListener l)
		{
			addToListenerList(m_graphListeners, l);
		}
		
		
		/// <seealso cref="Graph.addVertex(Object)">
		/// </seealso>
		public override bool addVertex(System.Object v)
		{
			bool modified = base.addVertex(v);
			
			if (modified)
			{
				fireVertexAdded(v);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="ListenableGraph.addVertexSetListener(VertexSetListener)">
		/// </seealso>
		public event org._3pq.jgrapht.event_Renamed.VertexSetListenerDelegate VertexSetListenerDelegateVar;
		protected virtual void  OnVertexSet(org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent eventParam)
		{
			if (VertexSetListenerDelegateVar != null)
				VertexSetListenerDelegateVar(this, eventParam);
		}
		public virtual void  addVertexSetListener(VertexSetListener l)
		{
			addToListenerList(m_vertexSetListeners, l);
		}
		
		
		/// <seealso cref="java.lang.Object.clone()">
		/// </seealso>
		public virtual System.Object Clone()
		{
			try
			{
				DefaultListenableGraph g = (DefaultListenableGraph) base.Clone();
				g.m_graphListeners = new System.Collections.ArrayList();
				g.m_vertexSetListeners = new System.Collections.ArrayList();
				
				return g;
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				// should never get here since we're Cloneable
				SupportClass.WriteStackTrace(e, Console.Error);
				throw new System.SystemException("internal error");
			}
		}
		
		
		/// <seealso cref="Graph.removeEdge(Object, Object)">
		/// </seealso>
		public override Edge removeEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			Edge e = base.removeEdge(sourceVertex, targetVertex);
			
			if (e != null)
			{
				fireEdgeRemoved(e);
			}
			
			return e;
		}
		
		
		/// <seealso cref="Graph.removeEdge(Edge)">
		/// </seealso>
		public override bool removeEdge(Edge e)
		{
			bool modified = base.removeEdge(e);
			
			if (modified)
			{
				fireEdgeRemoved(e);
			}
			
			return modified;
		}
		
		
		/// <seealso cref="ListenableGraph.removeGraphListener(GraphListener)">
		/// </seealso>
		public virtual void  removeGraphListener(GraphListener l)
		{
			SupportClass.ICollectionSupport.Remove(m_graphListeners, l);
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
				
				base.removeVertex(v); // remove the vertex itself
				
				fireVertexRemoved(v);
				
				return true;
			}
			else
			{
				return false;
			}
		}
		
		
		/// <seealso cref="ListenableGraph.removeVertexSetListener(VertexSetListener)">
		/// </seealso>
		public virtual void  removeVertexSetListener(VertexSetListener l)
		{
			SupportClass.ICollectionSupport.Remove(m_vertexSetListeners, l);
		}
		
		
		/// <summary> Notify listeners that the specified edge was added.
		/// 
		/// </summary>
		/// <param name="edge">the edge that was added.
		/// </param>
		protected internal virtual void  fireEdgeAdded(Edge edge)
		{
			GraphEdgeChangeEvent e = createGraphEdgeChangeEvent(GraphEdgeChangeEvent.EDGE_ADDED, edge);
			
			for (int i = 0; i < m_graphListeners.Count; i++)
			{
				GraphListener l = (GraphListener) m_graphListeners[i];
				
				l.edgeAdded(this, e);
			}
		}
		
		
		/// <summary> Notify listeners that the specified edge was removed.
		/// 
		/// </summary>
		/// <param name="edge">the edge that was removed.
		/// </param>
		protected internal virtual void  fireEdgeRemoved(Edge edge)
		{
			GraphEdgeChangeEvent e = createGraphEdgeChangeEvent(GraphEdgeChangeEvent.EDGE_REMOVED, edge);
			
			for (int i = 0; i < m_graphListeners.Count; i++)
			{
				GraphListener l = (GraphListener) m_graphListeners[i];
				
				l.edgeRemoved(this, e);
			}
		}
		
		
		/// <summary> Notify listeners that the specified vertex was added.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex that was added.
		/// </param>
		protected internal virtual void  fireVertexAdded(System.Object vertex)
		{
			GraphVertexChangeEvent e = createGraphVertexChangeEvent(GraphVertexChangeEvent.VERTEX_ADDED, vertex);
			
			for (int i = 0; i < m_vertexSetListeners.Count; i++)
			{
				VertexSetListener l = (VertexSetListener) m_vertexSetListeners[i];
				
				l.vertexAdded(this, e);
			}
			
			for (int i = 0; i < m_graphListeners.Count; i++)
			{
				GraphListener l = (GraphListener) m_graphListeners[i];
				
				l.vertexAdded(this, e);
			}
		}
		
		
		/// <summary> Notify listeners that the specified vertex was removed.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex that was removed.
		/// </param>
		protected internal virtual void  fireVertexRemoved(System.Object vertex)
		{
			GraphVertexChangeEvent e = createGraphVertexChangeEvent(GraphVertexChangeEvent.VERTEX_REMOVED, vertex);
			
			for (int i = 0; i < m_vertexSetListeners.Count; i++)
			{
				VertexSetListener l = (VertexSetListener) m_vertexSetListeners[i];
				
				l.vertexRemoved(this, e);
			}
			
			for (int i = 0; i < m_graphListeners.Count; i++)
			{
				GraphListener l = (GraphListener) m_graphListeners[i];
				
				l.vertexRemoved(this, e);
			}
		}
		
		
		//UPGRADE_ISSUE: Interface 'java.util.EventListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilEventListener'"
		private void  addToListenerList(System.Collections.IList list, EventListener l)
		{
			if (!list.Contains(l))
			{
				list.Add(l);
			}
		}
		
		
		private GraphEdgeChangeEvent createGraphEdgeChangeEvent(int eventType, Edge edge)
		{
			if (m_reuseEvents)
			{
				m_reuseableEdgeEvent.setType(eventType);
				m_reuseableEdgeEvent.setEdge(edge);
				
				return m_reuseableEdgeEvent;
			}
			else
			{
				return new GraphEdgeChangeEvent(this, eventType, edge);
			}
		}
		
		
		private GraphVertexChangeEvent createGraphVertexChangeEvent(int eventType, System.Object vertex)
		{
			if (m_reuseEvents)
			{
				m_reuseableVertexEvent.setType(eventType);
				m_reuseableVertexEvent.setVertex(vertex);
				
				return m_reuseableVertexEvent;
			}
			else
			{
				return new GraphVertexChangeEvent(this, eventType, vertex);
			}
		}
		
		/// <summary> A reuseable edge event.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 10, 2003
		/// </since>
		[Serializable]
		private class FlyweightEdgeEvent:GraphEdgeChangeEvent
		{
			private const long serialVersionUID = 3907207152526636089L;
			
			/// <seealso cref="GraphEdgeChangeEvent.GraphEdgeChangeEvent(Object, int, Edge)">
			/// </seealso>
			public FlyweightEdgeEvent(System.Object eventSource, int type, Edge e):base(eventSource, type, e)
			{
			}
			
			/// <summary> Sets the edge of this event.
			/// 
			/// </summary>
			/// <param name="e">the edge to be set.
			/// </param>
			protected internal virtual void  setEdge(Edge e)
			{
				m_edge = e;
			}
			
			
			/// <summary> Set the event type of this event.
			/// 
			/// </summary>
			/// <param name="type">the type to be set.
			/// </param>
			protected internal virtual void  setType(int type)
			{
				m_type = type;
			}
		}
		
		
		/// <summary> A reuseable vertex event.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 10, 2003
		/// </since>
		[Serializable]
		private class FlyweightVertexEvent:GraphVertexChangeEvent
		{
			private const long serialVersionUID = 3257848787857585716L;
			
			/// <seealso cref="GraphVertexChangeEvent.GraphVertexChangeEvent(Object, int,">
			/// Object)
			/// </seealso>
			public FlyweightVertexEvent(System.Object eventSource, int type, System.Object vertex):base(eventSource, type, vertex)
			{
			}
			
			/// <summary> Set the event type of this event.
			/// 
			/// </summary>
			/// <param name="type">type to be set.
			/// </param>
			protected internal virtual void  setType(int type)
			{
				m_type = type;
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
	}
}
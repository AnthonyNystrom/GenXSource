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
/* --------------------------
* ConnectivityInspector.java
* --------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   John V. Sichi
*
* $Id: ConnectivityInspector.java,v 1.11 2005/04/23 08:09:28 perfecthash Exp $
*
* Changes
* -------
* 06-Aug-2003 : Initial revision (BN);
* 10-Aug-2003 : Adaptation to new event model (BN);
*
*/

using CSGraphT;
using org._3pq.jgrapht.event_Renamed;
using org._3pq.jgrapht.graph;
using org._3pq.jgrapht.traverse;

namespace org._3pq.jgrapht.alg
{
	
	/// <summary> Allows obtaining various connectivity aspects of a graph. The <i>inspected
	/// graph</i> is specified at construction time and cannot be modified.
	/// Currently, the inspector supports connected components for an undirected
	/// graph and weakly connected components for a directed graph.  To find
	/// strongly connected components, use {@link StrongConnectivityInspector}
	/// instead.
	/// 
	/// <p>
	/// The inspector methods work in a lazy fashion: no computation is performed
	/// unless immediately necessary. Computation are done once and results and
	/// cached within this class for future need.
	/// </p>
	/// 
	/// <p>
	/// The inspector is also a {@link org._3pq.jgrapht.event.GraphListener}. If
	/// added as a listener to the inspected graph, the inspector will amend
	/// internal cached results instead of recomputing them. It is efficient when a
	/// few modifications are applied to a large graph. If many modifications are
	/// expected it will not be efficient due to added overhead on graph update
	/// operations. If inspector is added as listener to a graph other than the one
	/// it inspects, results are undefined.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// </author>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Aug 6, 2003
	/// </since>
	public class ConnectivityInspector : GraphListener
	{
		/// <summary> Test if the inspected graph is connected. An empty graph is <i>not</i>
		/// considered connected.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if and only if inspected graph is connected.
		/// </returns>
		virtual public bool GraphConnected
		{
			get
			{
				return lazyFindConnectedSets().Count == 1;
			}
			
		}
		internal System.Collections.IList m_connectedSets;
		internal System.Collections.IDictionary m_vertexToConnectedSet;
		private Graph m_graph;
		
		/// <summary> Creates a connectivity inspector for the specified undirected graph.
		/// 
		/// </summary>
		/// <param name="g">the graph for which a connectivity inspector to be created.
		/// </param>
		public ConnectivityInspector(UndirectedGraph g)
		{
			init();
			m_graph = g;
		}
		
		
		/// <summary> Creates a connectivity inspector for the specified directed graph.
		/// 
		/// </summary>
		/// <param name="g">the graph for which a connectivity inspector to be created.
		/// </param>
		public ConnectivityInspector(DirectedGraph g)
		{
			init();
			m_graph = new AsUndirectedGraph(g);
		}
		
		
		/// <summary> Returns a set of all vertices that are in the maximally connected
		/// component together with the specified vertex. For more on maximally
		/// connected component, see <a
		/// href="http://www.nist.gov/dads/HTML/maximallyConnectedComponent.html">
		/// http://www.nist.gov/dads/HTML/maximallyConnectedComponent.html</a>.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex for which the connected set to be returned.
		/// 
		/// </param>
		/// <returns> a set of all vertices that are in the maximally connected
		/// component together with the specified vertex.
		/// </returns>
		public virtual SupportClass.SetSupport connectedSetOf(System.Object vertex)
		{
			SupportClass.SetSupport connectedSet = (SupportClass.SetSupport) m_vertexToConnectedSet[vertex];
			
			if (connectedSet == null)
			{
				//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
				connectedSet = new SupportClass.HashSetSupport();
				
				BreadthFirstIterator i = new BreadthFirstIterator(m_graph, vertex);
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (i.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					connectedSet.Add(i.Current);
				}
				
				m_vertexToConnectedSet[vertex] = connectedSet;
			}
			
			return connectedSet;
		}
		
		
		/// <summary> Returns a list of <code>Set</code>s, where each set contains all
		/// vertices that are in the same maximally connected component. All graph
		/// vertices occur in exactly one set.  For more on maximally connected
		/// component, see <a
		/// href="http://www.nist.gov/dads/HTML/maximallyConnectedComponent.html">
		/// http://www.nist.gov/dads/HTML/maximallyConnectedComponent.html</a>.
		/// 
		/// </summary>
		/// <returns> Returns a list of <code>Set</code>s, where each set contains all
		/// vertices that are in the same maximally connected component.
		/// </returns>
		public virtual System.Collections.IList connectedSets()
		{
			return lazyFindConnectedSets();
		}
		
		
		/// <seealso cref="GraphListener.edgeAdded(GraphEdgeChangeEvent)">
		/// </seealso>
		public virtual void  edgeAdded(System.Object event_sender, GraphEdgeChangeEvent e)
		{
			init(); // for now invalidate cached results, in the future need to amend them. 
		}
		
		
		/// <seealso cref="GraphListener.edgeRemoved(GraphEdgeChangeEvent)">
		/// </seealso>
		public virtual void  edgeRemoved(System.Object event_sender, GraphEdgeChangeEvent e)
		{
			init(); // for now invalidate cached results, in the future need to amend them. 
		}
		
		
		/// <summary> Tests if there is a path from the specified source vertex to the
		/// specified target vertices. For a directed graph, direction is ignored
		/// for this interpretation of path.
		/// 
		/// <p>
		/// Note: Future versions of this method might not ignore edge directions
		/// for directed graphs.
		/// </p>
		/// 
		/// </summary>
		/// <param name="sourceVertex">one end of the path.
		/// </param>
		/// <param name="targetVertex">another end of the path.
		/// 
		/// </param>
		/// <returns> <code>true</code> if and only if there is a path from the source
		/// vertex to the target vertex.
		/// </returns>
		public virtual bool pathExists(System.Object sourceVertex, System.Object targetVertex)
		{
			/*
			* TODO: Ignoring edge direction for directed graph may be
			* confusing. For directed graphs, consider Dijkstra's algorithm.
			*/
			SupportClass.SetSupport sourceSet = connectedSetOf(sourceVertex);
			
			return sourceSet.Contains(targetVertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexAdded(GraphVertexChangeEvent)">
		/// </seealso>
		public virtual void  vertexAdded(System.Object event_sender, GraphVertexChangeEvent e)
		{
			init(); // for now invalidate cached results, in the future need to amend them. 
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexRemoved(GraphVertexChangeEvent)">
		/// </seealso>
		public virtual void  vertexRemoved(System.Object event_sender, GraphVertexChangeEvent e)
		{
			init(); // for now invalidate cached results, in the future need to amend them. 
		}
		
		
		private void  init()
		{
			m_connectedSets = null;
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			m_vertexToConnectedSet = new System.Collections.Hashtable();
		}
		
		
		private System.Collections.IList lazyFindConnectedSets()
		{
			if (m_connectedSets == null)
			{
				//m_connectedSets = new System.Collections.ArrayList();
				
				SupportClass.SetSupport vertexSet = m_graph.vertexSet();
				
				if (vertexSet.Count > 0)
				{
					BreadthFirstIterator i = new BreadthFirstIterator(m_graph, null);
                    i.addTraversalListener(new MyTraversalListener(this));
					
					while (i.MoveNext())
					{
						System.Object generatedAux = i.Current;
					}
				}
			}
			
			return m_connectedSets;
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'MyTraversalListener' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> A traversal listener that groups all vertices according to to their
		/// containing connected set.
		/// 
		/// </summary>
		/// <author>  Barak Naveh
		/// 
		/// </author>
		/// <since> Aug 6, 2003
		/// </since>
		private class MyTraversalListener : TraversalListenerAdapter
		{
			public MyTraversalListener(ConnectivityInspector enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}

			private void  InitBlock(ConnectivityInspector enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}

			private ConnectivityInspector enclosingInstance;
			public ConnectivityInspector Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}

			private SupportClass.SetSupport m_currentConnectedSet;
			
			/// <seealso cref="TraversalListenerAdapter.connectedComponentFinished(ConnectedComponentTraversalEvent)">
			/// </seealso>
			public override void  connectedComponentFinished(ConnectedComponentTraversalEvent e)
			{
				Enclosing_Instance.m_connectedSets.Add(m_currentConnectedSet);
			}
			
			/// <seealso cref="TraversalListenerAdapter.connectedComponentStarted(ConnectedComponentTraversalEvent)">
			/// </seealso>
			public override void connectedComponentStarted(ConnectedComponentTraversalEvent e)
			{
                m_currentConnectedSet = new SupportClass.HashSetSupport();
                enclosingInstance.m_connectedSets = m_currentConnectedSet;
			}
		
			/// <seealso cref="TraversalListenerAdapter.vertexTraversed(Object)">
			/// </seealso>
			public override void vertexTraversed(VertexTraversalEvent e)
			{
				System.Object v = e.getVertex();
				m_currentConnectedSet.Add(v);
				Enclosing_Instance.m_vertexToConnectedSet[v] = m_currentConnectedSet;
			}
		}
	}
}
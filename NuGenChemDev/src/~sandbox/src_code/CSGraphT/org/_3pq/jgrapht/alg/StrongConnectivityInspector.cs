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
* StrongConnectivityInspector.java
* --------------------------
* (C) Copyright 2005, by Christian Soltenborn and Contributors.
*
* Original Author:  Christian Soltenborn
*
* $Id: StrongConnectivityInspector.java,v 1.6 2005/07/17 05:33:25 perfecthash Exp $
*
* Changes
* -------
* 2-Feb-2005 : Initial revision (CS);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using GraphHelper = org._3pq.jgrapht.GraphHelper;
using DirectedEdge = org._3pq.jgrapht.edge.DirectedEdge;
using DefaultDirectedGraph = org._3pq.jgrapht.graph.DefaultDirectedGraph;
using DirectedSubgraph = org._3pq.jgrapht.graph.DirectedSubgraph;
namespace org._3pq.jgrapht.alg
{
	
	/// <summary> <p>
	/// Complements the {@link org._3pq.jgrapht.alg.ConnectivityInspector} class
	/// with the capability to compute the strongly connected components of a
	/// directed graph. The algorithm is implemented after "Corman et al:
	/// Introduction to agorithms", Chapter 25.2. It has a running time of O(V +
	/// E).
	/// </p>
	/// 
	/// <p>
	/// Unlike {@link org._3pq.jgrapht.alg.ConnectivityInspector}, this class does
	/// not implement incremental inspection. The full algorithm is executed at the
	/// first call of {@link StrongConnectivityInspector#stronglyConnectedSets()}
	/// or {@link StrongConnectivityInspector#isStronglyConnected()}.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Christian Soltenborn
	/// 
	/// </author>
	/// <since> Feb 2, 2005
	/// </since>
	public class StrongConnectivityInspector
	{
		/// <summary> Returns the graph inspected by the StrongConnectivityInspector.
		/// 
		/// </summary>
		/// <returns> the graph inspected by this StrongConnectivityInspector
		/// </returns>
		virtual public DirectedGraph Graph
		{
			get
			{
				return m_graph;
			}
			
		}
		/// <summary> Returns true if the graph of this
		/// <code>StronglyConnectivityInspector</code> instance is strongly
		/// connected.
		/// 
		/// </summary>
		/// <returns> true if the graph is strongly connected, false otherwise
		/// </returns>
		virtual public bool StronglyConnected
		{
			get
			{
				return stronglyConnectedSets().Count == 1;
			}
			
		}
		// the graph to compute the strongly connected sets for
		//UPGRADE_NOTE: Final was removed from the declaration of 'm_graph '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private DirectedGraph m_graph;
		
		// stores the vertices, ordered by their finishing time in first dfs
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		private System.Collections.ArrayList m_orderedVertices;
		
		// the result of the computation, cached for future calls
		private System.Collections.IList m_stronglyConnectedSets;
		
		// the result of the computation, cached for future calls
		private System.Collections.IList m_stronglyConnectedSubgraphs;
		
		// maps vertices to their VertexData object
		private System.Collections.IDictionary m_vertexToVertexData;
		
		/// <summary> The constructor of the StrongConnectivityInspector class.
		/// 
		/// </summary>
		/// <param name="directedGraph">the graph to inspect
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException </throws>
		public StrongConnectivityInspector(DirectedGraph directedGraph)
		{
			if (directedGraph == null)
			{
				throw new System.ArgumentException("null not allowed for graph!");
			}
			
			m_graph = directedGraph;
			m_vertexToVertexData = null;
			m_orderedVertices = null;
			m_stronglyConnectedSets = null;
			m_stronglyConnectedSubgraphs = null;
		}
		
		
		/// <summary> Computes a {@link List} of {@link Set}s, where each set contains
		/// vertices which together form a strongly connected component within the
		/// given graph.
		/// 
		/// </summary>
		/// <returns> <code>List</code> of <code>Set</code>s containing the strongly
		/// connected components
		/// </returns>
		public virtual System.Collections.IList stronglyConnectedSets()
		{
			if (m_stronglyConnectedSets == null)
			{
				//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
				m_orderedVertices = new System.Collections.ArrayList();
				m_stronglyConnectedSets = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
				
				// create VertexData objects for all vertices, store them
				createVertexData();
				
				// perform the first round of DFS, result is an ordering
				// of the vertices by decreasing finishing time
				System.Collections.IEnumerator iter = m_vertexToVertexData.Values.GetEnumerator();
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iter.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					VertexData data = (VertexData) iter.Current;
					
					if (!data.m_discovered)
					{
						dfsVisit(m_graph, data, null);
					}
				}
				
				// calculate inverse graph (i.e. every edge is reversed)
				DirectedGraph inverseGraph = new DefaultDirectedGraph();
				GraphHelper.addGraphReversed(inverseGraph, m_graph);
				
				// get ready for next dfs round
				resetVertexData();
				
				// second dfs round: vertices are considered in decreasing
				// finishing time order; every tree found is a strongly
				// connected set
				iter = m_orderedVertices.GetEnumerator();
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iter.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					VertexData data = (VertexData) iter.Current;
					
					if (!data.m_discovered)
					{
						// new strongly connected set
						//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
						SupportClass.SetSupport set_Renamed = new SupportClass.HashSetSupport();
						m_stronglyConnectedSets.Add(set_Renamed);
						dfsVisit(inverseGraph, data, set_Renamed);
					}
				}
				
				// clean up for garbage collection
				m_orderedVertices = null;
				m_vertexToVertexData = null;
			}
			
			return m_stronglyConnectedSets;
		}
		
		
		/// <summary> <p>
		/// Computes a list of {@link DirectedSubgraph}s of the given graph. Each
		/// subgraph will represent a strongly connected component and will contain
		/// all vertices of that component. The subgraph will have an edge (u,v)
		/// iff u and v are contained in the strongly connected component.
		/// </p>
		/// 
		/// <p>
		/// NOTE: Calling this method will first execute {@link
		/// StrongConnectivityInspector#stronglyConnectedSets()}. If you don't need
		/// subgraphs, use that method.
		/// </p>
		/// 
		/// </summary>
		/// <returns> a list of subgraphs representing the strongly connected
		/// components
		/// </returns>
		public virtual System.Collections.IList stronglyConnectedSubgraphs()
		{
			if (m_stronglyConnectedSubgraphs == null)
			{
				System.Collections.IList sets = stronglyConnectedSets();
				m_stronglyConnectedSubgraphs = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(sets.Count));
				
				System.Collections.IEnumerator iter = sets.GetEnumerator();
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				while (iter.MoveNext())
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					m_stronglyConnectedSubgraphs.Add(new DirectedSubgraph(m_graph, (SupportClass.SetSupport) iter.Current, null));
				}
			}
			
			return m_stronglyConnectedSubgraphs;
		}
		
		
		/*
		* Creates a VertexData object for every vertex in the graph and stores them
		* in a HashMap.
		*/
		private void  createVertexData()
		{
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			m_vertexToVertexData = new System.Collections.Hashtable(m_graph.vertexSet().Count);
			
			System.Collections.IEnumerator iter = m_graph.vertexSet().GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object vertex = iter.Current;
				m_vertexToVertexData[vertex] = new VertexData(this, vertex, false, false);
			}
		}
		
		
		/*
		* The subroutine of DFS. NOTE: the set is used to distinguish between 1st
		* and 2nd round of DFS. set == null: finished vertices are stored (1st
		* round). set != null: all vertices found will be saved in the set (2nd
		* round)
		*/
		private void  dfsVisit(DirectedGraph graph, VertexData vertexData, SupportClass.SetSupport vertices)
		{
			System.Collections.ArrayList stack = new System.Collections.ArrayList();
			stack.Add(vertexData);
			
			while (!(stack.Count == 0))
			{
				VertexData data = (VertexData) SupportClass.StackSupport.Pop(stack);
				
				if (!data.m_discovered)
				{
					data.m_discovered = true;
					
					if (vertices != null)
					{
						vertices.Add(data.m_vertex);
					}
					
					// TODO: other way to identify when this vertex is finished!?
					stack.Add(new VertexData(this, data, true, true));
					
					// follow all edges
					System.Collections.IEnumerator iter = graph.outgoingEdgesOf(data.m_vertex).GetEnumerator();
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					while (iter.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						DirectedEdge edge = (DirectedEdge) iter.Current;
						VertexData targetData = (VertexData) m_vertexToVertexData[edge.Target];
						
						if (!targetData.m_discovered)
						{
							// the "recursion"
							stack.Add(targetData);
						}
					}
				}
				else if (data.m_finished)
				{
					if (vertices == null)
					{
						// see TODO above
						m_orderedVertices.Insert(0, data.m_vertex);
					}
				}
			}
		}
		
		
		/*
		* Resets all VertexData objects.
		*/
		private void  resetVertexData()
		{
			System.Collections.IEnumerator iter = m_vertexToVertexData.Values.GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				VertexData data = (VertexData) iter.Current;
				data.m_discovered = false;
				data.m_finished = false;
			}
		}
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'VertexData' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/*
		* Lightweight class storing some data vor every vertex.
		*/
		private sealed class VertexData
		{
			private void  InitBlock(StrongConnectivityInspector enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private StrongConnectivityInspector enclosingInstance;
			public StrongConnectivityInspector Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			//UPGRADE_NOTE: Final was removed from the declaration of 'm_vertex '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			private System.Object m_vertex;
			private bool m_discovered;
			private bool m_finished;
			
			internal VertexData(StrongConnectivityInspector enclosingInstance, System.Object vertex, bool discovered, bool finished)
			{
				InitBlock(enclosingInstance);
				m_vertex = vertex;
				m_discovered = discovered;
				m_finished = finished;
			}
		}
	}
}
/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
* 
*/

using CSGraphT;
using org._3pq.jgrapht;
using org._3pq.jgrapht.alg;
using org._3pq.jgrapht.event_Renamed;
using org._3pq.jgrapht.graph;
using System.Collections;

namespace Org.OpenScience.CDK.Graph
{
	
	/// <summary> Finds the biconnected components of a graph.
	/// Two edges belong to the same biconnected component if and only if they are 
	/// identical or both belong to a simple cycle.
	/// 
	/// </summary>
	/// <author>  Ulrich Bauer <baueru@cs.tum.edu>
	/// 
	/// </author>
	/// <cdk.module>  standard </cdk.module>
	/// <summary> 
	/// </summary>
	/// <cdk.builddepends>  jgrapht-0.5.3.jar </cdk.builddepends>
	/// <cdk.depends>  jgrapht-0.5.3.jar </cdk.depends>
	public class BiconnectivityInspector
	{
		private System.Collections.IList biconnectedSets_Renamed_Field;
		private UndirectedGraph graph;
		
		/// <summary> Creates a biconnectivity inspector for the specified undirected graph.</summary>
		/// <param name="g">the specified graph
		/// </param>
		
		public BiconnectivityInspector(UndirectedGraph g)
		{
			graph = g;
		}
		
		private System.Collections.IList lazyFindBiconnectedSets()
		{
			if (biconnectedSets_Renamed_Field == null)
			{
				biconnectedSets_Renamed_Field = new System.Collections.ArrayList();
				
                IList inspector = new ConnectivityInspector(graph).connectedSets();
                System.Collections.IEnumerator connectedSets = inspector.GetEnumerator();
				
				while (connectedSets.MoveNext())
				{
                    object obj = ((DictionaryEntry)connectedSets.Current).Value;
                    if (!(obj is CSGraphT.SupportClass.HashSetSupport))
                        continue;
					CSGraphT.SupportClass.SetSupport connectedSet = (CSGraphT.SupportClass.SetSupport)obj;
					if (connectedSet.Count == 1)
					{
						continue;
					}
					
					org._3pq.jgrapht.Graph subgraph = new Subgraph(graph, connectedSet, null);
					
					// do DFS
					
					// Stack for the DFS
					System.Collections.ArrayList vertexStack = new System.Collections.ArrayList();
					
					CSGraphT.SupportClass.SetSupport visitedVertices = new CSGraphT.SupportClass.HashSetSupport();
					IDictionary parent = new System.Collections.Hashtable();
					IList dfsVertices = new System.Collections.ArrayList();
					
					CSGraphT.SupportClass.SetSupport treeEdges = new CSGraphT.SupportClass.HashSetSupport();

                    System.Object currentVertex = subgraph.vertexSet()[0];//.ToArray()[0];

					vertexStack.Add(currentVertex);
					visitedVertices.Add(currentVertex);
					
					while (!(vertexStack.Count == 0))
					{
						currentVertex = SupportClass.StackSupport.Pop(vertexStack);
						
						System.Object parentVertex = parent[currentVertex];
						
						if (parentVertex != null)
						{
							Edge edge = subgraph.getEdge(parentVertex, currentVertex);
							
							// tree edge
							treeEdges.Add(edge);
						}
						
						visitedVertices.Add(currentVertex);
						
						dfsVertices.Add(currentVertex);
						
						System.Collections.IEnumerator edges = subgraph.edgesOf(currentVertex).GetEnumerator();
						while (edges.MoveNext())
						{
							// find a neighbour vertex of the current vertex 
							Edge edge = (Edge)edges.Current;
							
							if (!treeEdges.Contains(edge))
							{
								System.Object nextVertex = edge.oppositeVertex(currentVertex);
								
								if (!visitedVertices.Contains(nextVertex))
								{
									vertexStack.Add(nextVertex);
									
									parent[nextVertex] = currentVertex;
								}
								else
								{
									// non-tree edge
								}
							}
						}
					}
					
					// DFS is finished. Now create the auxiliary graph h
					// Add all the tree edges as vertices in h
					SimpleGraph h = new SimpleGraph();
					
					h.addAllVertices(treeEdges);
					
					visitedVertices.Clear();
					
					CSGraphT.SupportClass.SetSupport connected = new CSGraphT.SupportClass.HashSetSupport();
					
					for (System.Collections.IEnumerator it = dfsVertices.GetEnumerator(); it.MoveNext(); )
					{
						System.Object v = it.Current;
						
						visitedVertices.Add(v);
						
						// find all adjacent non-tree edges
						for (System.Collections.IEnumerator adjacentEdges = subgraph.edgesOf(v).GetEnumerator(); adjacentEdges.MoveNext();)
						{
							Edge l = (Edge)adjacentEdges.Current;
							if (!treeEdges.Contains(l))
							{
								h.addVertex(l);
								System.Object u = l.oppositeVertex(v);
								
								// we need to check if (u,v) is a back-edge
								if (!visitedVertices.Contains(u))
								{
									while (u != v)
									{
										System.Object pu = parent[u];
										Edge f = subgraph.getEdge(u, pu);
										
										h.addEdge(f, l);
										
										if (!connected.Contains(f))
										{
											connected.Add(f);
											u = pu;
										}
										else
										{
											u = v;
										}
									}
								}
							}
						}
					}
					
					ConnectivityInspector connectivityInspector = new ConnectivityInspector(h);
					
					biconnectedSets_Renamed_Field.Add(connectivityInspector.connectedSets());
				}
			}
			
			return biconnectedSets_Renamed_Field;
		}
		
		/// <summary> Returns a list of <code>Set</code>s, where each set contains all edge that are
		/// in the same biconnected component. All graph edges occur in exactly one set.
		/// 
		/// </summary>
		/// <returns> a list of <code>Set</code>s, where each set contains all edge that are
		/// in the same biconnected component
		/// </returns>
		public virtual System.Collections.IList biconnectedSets()
		{
			return lazyFindBiconnectedSets();
		}
		
		/*
		public List hopcroftTarjanKnuthFindBiconnectedSets() {
		Map rank;
		Map parent;
		Map untagged;
		Map link;
		Stack activeStack;
		Map min;
		
		int nn;
		
		
		
		
		
		return biconnectedSets;
		}
		*/
		
		
		private void  init()
		{
			biconnectedSets_Renamed_Field = null;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.GraphListener.edgeAdded(GraphEdgeChangeEvent)">
		/// </seealso>
		public virtual void  edgeAdded(GraphEdgeChangeEvent e)
		{
			init();
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.GraphListener.edgeRemoved(GraphEdgeChangeEvent)">
		/// </seealso>
		public virtual void  edgeRemoved(GraphEdgeChangeEvent e)
		{
			init();
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexAdded(GraphVertexChangeEvent)">
		/// </seealso>
		public virtual void  vertexAdded(GraphVertexChangeEvent e)
		{
			init();
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener.vertexRemoved(GraphVertexChangeEvent)">
		/// </seealso>
		public virtual void  vertexRemoved(GraphVertexChangeEvent e)
		{
			init();
		}
	}
}
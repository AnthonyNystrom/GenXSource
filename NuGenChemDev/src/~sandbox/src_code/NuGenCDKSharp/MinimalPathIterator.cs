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
using System;
using org._3pq.jgrapht;
using org._3pq.jgrapht.graph;
using org._3pq.jgrapht.traverse;
using Support;

namespace Org.OpenScience.CDK.Graph
{
	
	/// <summary> Iterates over all shortest paths between two vertices in an undirected, unweighted graph.
	/// 
	/// </summary>
	/// <author>  Ulrich Bauer <baueru@cs.tum.edu>
	/// 
	/// 
	/// </author>
	/// <cdk.module>  standard </cdk.module>
	/// <summary> 
	/// </summary>
	/// <cdk.builddepends>  jgrapht-0.5.3.jar </cdk.builddepends>
	/// <cdk.depends>  jgrapht-0.5.3.jar </cdk.depends>
	public class MinimalPathIterator : System.Collections.IEnumerator
	{
		public virtual System.Object Current
		{
			get
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				if (MoveNext())
				{
					System.Object result = next_Renamed_Field;
					next_Renamed_Field = null;
					return result;
				}
				else
				{
					return null;
				}
			}
			
		}
		
		private System.Object sourceVertex, targetVertex;
        private org._3pq.jgrapht.Graph g;
		private DirectedGraph shortestPathGraph;
		
		private System.Collections.ArrayList edgeIteratorStack;
		private System.Collections.ArrayList vertexStack;
		
		private System.Object next_Renamed_Field;
		
		/// <summary> Creates a minimal path iterator for the specified undirected graph.</summary>
		/// <param name="g">the specified graph
		/// </param>
		/// <param name="sourceVertex">the start vertex for the paths
		/// </param>
		/// <param name="targetVertex">the target vertex for the paths
		/// </param>
		public MinimalPathIterator(SimpleGraph g, System.Object sourceVertex, System.Object targetVertex)
		{
			this.g = g;
			
			this.sourceVertex = sourceVertex;
			this.targetVertex = targetVertex;
			
			createShortestPathGraph();
		}
		
		private void  createShortestPathGraph()
		{
			
			
			/*		shortestPathGraph = new DefaultDirectedGraph();
			//shortestPathGraph.addAllVertices(g.vertexSet());
			
			LinkedList queue = new LinkedList();
			
			//encounter target vertex
			queue.addLast(targetVertex);
			shortestPathGraph.addVertex(targetVertex);
			
			int distance = 0;
			
			Object firstVertexOfNextLevel = targetVertex;
			Collection verticesOfNextLevel = new ArrayList();
			
			while (!queue.isEmpty()) {
			//provide next vertex
			Object vertex = queue.removeFirst();
			
			if (vertex == firstVertexOfNextLevel) {
			distance++;
			firstVertexOfNextLevel = null;
			verticesOfNextLevel.clear();
			}
			
			//add unseen children of next vertex
			List edges = g.edgesOf(vertex);
			
			for(Iterator i = edges.iterator(); i.hasNext();) {
			Edge e = (Edge) i.next(  );
			Object opposite = e.oppositeVertex(vertex);
			
			if (!shortestPathGraph.containsVertex(opposite)) {
			//encounter vertex
			queue.addLast(opposite);
			shortestPathGraph.addVertex(opposite);
			
			verticesOfNextLevel.add(opposite);
			
			if (firstVertexOfNextLevel == null) {
			firstVertexOfNextLevel = opposite;
			}
			}
			
			
			if (verticesOfNextLevel.contains(opposite)) {
			shortestPathGraph.addEdge(opposite, vertex);
			}
			}
			}*/
			
			
			shortestPathGraph = new DefaultDirectedGraph();
			shortestPathGraph.addVertex(targetVertex);
			
			// This map gives the distance of a vertex to the target vertex
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.IDictionary distanceMap = new System.Collections.Hashtable();
			
			for (MyBreadthFirstIterator iter = new MyBreadthFirstIterator(g, targetVertex); iter.MoveNext(); )
			{
				System.Object vertex = iter.Current;
				shortestPathGraph.addVertex(vertex);
				
				int distance = iter.level;
				distanceMap[vertex] = (System.Int32) distance;
				
				//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
				for (System.Collections.IEnumerator edges = g.edgesOf(vertex).GetEnumerator(); edges.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
					Edge edge = (Edge) edges.Current;
					System.Object opposite = edge.oppositeVertex(vertex);
					if (distanceMap[opposite] != null)
					{
						if (((System.Int32) distanceMap[opposite]) + 1 == distance)
						{
							shortestPathGraph.addVertex(opposite);
							shortestPathGraph.addEdge(vertex, opposite);
						}
					}
				}
				
				if (vertex == sourceVertex)
				{
					break;
				}
			}
			
			System.Collections.IEnumerator edgeIterator = shortestPathGraph.outgoingEdgesOf(sourceVertex).GetEnumerator();
			
			edgeIteratorStack = new System.Collections.ArrayList();
			edgeIteratorStack.Add(edgeIterator);
			
			vertexStack = new System.Collections.ArrayList();
			vertexStack.Add(sourceVertex);
		}
		
		//	private void createShortestPathWeightedGraph() {
		//		shortestPathGraph = new DefaultDirectedGraph();
		//		//shortestPathGraph.addAllVertices(g.vertexSet());
		//		shortestPathGraph.addVertex(targetVertex);
		//
		//		// This map gives the distance of a vertex to the target vertex
		//		Map distanceMap = new HashMap();
		//		distanceMap.put(targetVertex, new Integer(0));
		//
		//		for (ClosestFirstIterator iter = new ClosestFirstIterator(g, targetVertex); iter.hasNext(); ) {
		//			Object vertex = iter.next();
		//			shortestPathGraph.addVertex(vertex);
		//			
		//			Edge treeEdge = iter.getSpanningTreeEdge(vertex);
		//			
		//			// in the first iteration, vertex is the target vertex; therefore no tree edge exists
		//			if (treeEdge != null) {
		//				Object parent = treeEdge.oppositeVertex(vertex);
		//				int distance = ((Integer)distanceMap.get(parent)).intValue() + 1;
		//				distanceMap.put(vertex, new Integer(distance));
		//				
		//				for (Iterator edges = g.edgesOf(vertex).iterator(); edges.hasNext();) {
		//					Edge edge = (Edge) edges.next();
		//					Object opposite = edge.oppositeVertex(vertex);
		//					if (distanceMap.get(opposite) != null) {
		//						if (((Integer) distanceMap.get(opposite)).intValue() + 1 == distance) {
		//							shortestPathGraph.addVertex(opposite);
		//							shortestPathGraph.addEdge(vertex, opposite);
		//						}
		//					}
		//				}
		//			}
		//			if (vertex == sourceVertex) {
		//				break;
		//			}
		//		}
		//				
		//		Iterator edgeIterator = shortestPathGraph.outgoingEdgesOf(sourceVertex).iterator();
		//		
		//		edgeIteratorStack = new Stack();
		//		edgeIteratorStack.push(edgeIterator);
		//
		//		vertexStack = new Stack();
		//		vertexStack.push(sourceVertex);
		//		
		//	}
		
		public virtual bool MoveNext()
		{
			
			if (next_Renamed_Field == null)
			{
				
				while (next_Renamed_Field == null && !(edgeIteratorStack.Count == 0))
				{
					System.Collections.IEnumerator edgeIterator = (System.Collections.IEnumerator) edgeIteratorStack[edgeIteratorStack.Count - 1];
					System.Object currentVertex = vertexStack[vertexStack.Count - 1];
					
					//System.out.println(currentVertex);
					
					//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
					if (edgeIterator.MoveNext())
					{
						//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
						Edge edge = (Edge) edgeIterator.Current;
						currentVertex = edge.oppositeVertex(currentVertex);
						edgeIterator = shortestPathGraph.outgoingEdgesOf(currentVertex).GetEnumerator();
						
						edgeIteratorStack.Add(edgeIterator);
						vertexStack.Add(currentVertex);
					}
					else
					{
						if (currentVertex == targetVertex)
						{
							next_Renamed_Field = edgeList(g, vertexStack);
						}
						SupportClass.StackSupport.Pop(edgeIteratorStack);
						SupportClass.StackSupport.Pop(vertexStack);
					}
				}
			}
			
			return (next_Renamed_Field != null);
		}
		
		//UPGRADE_NOTE: The equivalent of method 'java.util.Iterator.remove' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual void  remove()
		{
			throw new System.NotSupportedException();
		}

        private System.Collections.IList edgeList(org._3pq.jgrapht.Graph g, System.Collections.IList vertexList)
		{
			System.Collections.IList edgeList = new System.Collections.ArrayList(vertexList.Count - 1);
			System.Collections.IEnumerator vertices = vertexList.GetEnumerator();
			//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
			System.Object currentVertex = vertices.Current;
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (vertices.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object nextVertex = vertices.Current;
				edgeList.Add(g.getAllEdges(currentVertex, nextVertex)[0]);
				currentVertex = nextVertex;
			}
			
			return edgeList;
		}
		
		private class MyBreadthFirstIterator:BreadthFirstIterator
		{
			
			public MyBreadthFirstIterator(org._3pq.jgrapht.Graph g, System.Object startVertex):base(g, startVertex)
			{
			}
			
			internal int level = - 1;
			private System.Object firstVertexOfNextLevel;
			
			protected internal virtual void  encounterVertex(System.Object vertex, Edge edge)
			{
				base.encounterVertex(vertex, edge);
				if (firstVertexOfNextLevel == null)
				{
					firstVertexOfNextLevel = vertex;
				}
			}
			
			protected internal virtual System.Object provideNextVertex()
			{
				System.Object nextVertex = base.provideNextVertex();
				if (firstVertexOfNextLevel == nextVertex)
				{
					firstVertexOfNextLevel = null;
					level++;
				}
				return nextVertex;
			}
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		virtual public void  Reset()
		{
		}
	}
}
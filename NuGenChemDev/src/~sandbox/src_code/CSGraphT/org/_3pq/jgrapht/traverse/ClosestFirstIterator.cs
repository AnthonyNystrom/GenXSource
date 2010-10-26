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
/* -------------------------
* ClosestFirstIterator.java
* -------------------------
* (C) Copyright 2003, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
* Contributor(s):   Barak Naveh
*
* $Id: ClosestFirstIterator.java,v 1.6 2005/05/30 05:37:29 perfecthash Exp $
*
* Changes
* -------
* 02-Sep-2003 : Initial revision (JVS);
* 31-Jan-2004 : Reparented and changed interface to parent class (BN);
* 29-May-2005 : Added radius support (JVS);
*
*/

using org._3pq.jgrapht.util;
namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> A closest-first iterator for a directed or undirected graph. For this
	/// iterator to work correctly the graph must not be modified during iteration.
	/// Currently there are no means to ensure that, nor to fail-fast. The results
	/// of such modifications are undefined.
	/// 
	/// <p>
	/// The metric for <i>closest</i> here is the path length from a start vertex.
	/// Edge.getWeight() is summed to calculate path length. Negative edge weights
	/// will result in an IllegalArgumentException.  Optionally, path length may be
	/// bounded by a finite radius.
	/// </p>
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 2, 2003
	/// </since>
	public class ClosestFirstIterator:CrossComponentIterator
	{
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.isConnectedComponentExhausted()">
		/// </seealso>
		override protected internal bool ConnectedComponentExhausted
		{
			get
			{
				if (m_heap.size() == 0)
				{
					return true;
				}
				else
				{
					if (m_heap.min().Key > m_radius)
					{
						m_heap.clear();
						
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			
		}
		/// <summary>Priority queue of fringe vertices. </summary>
		private FibonacciHeap m_heap = new FibonacciHeap();
		
		/// <summary>Maximum distance to search. </summary>
		private double m_radius = System.Double.PositiveInfinity;
		
		/// <summary> Creates a new closest-first iterator for the specified graph.
		/// 
		/// </summary>
		/// <param name="g">the graph to be iterated.
		/// </param>
		public ClosestFirstIterator(Graph g):this(g, null)
		{
		}
		
		
		/// <summary> Creates a new closest-first iterator for the specified graph. Iteration
		/// will start at the specified start vertex and will be limited to the
		/// connected component that includes that vertex. If the specified start
		/// vertex is <code>null</code>, iteration will start at an arbitrary
		/// vertex and will not be limited, that is, will be able to traverse all
		/// the graph.
		/// 
		/// </summary>
		/// <param name="g">the graph to be iterated.
		/// </param>
		/// <param name="startVertex">the vertex iteration to be started.
		/// </param>
		public ClosestFirstIterator(Graph g, System.Object startVertex):this(g, startVertex, System.Double.PositiveInfinity)
		{
		}
		
		
		/// <summary> Creates a new radius-bounded closest-first iterator for the specified
		/// graph. Iteration will start at the specified start vertex and will be
		/// limited to the subset of the connected component which includes that
		/// vertex and is reachable via paths of length less than or equal to the
		/// specified radius.  The specified start vertex may not be
		/// <code>null</code>.
		/// 
		/// </summary>
		/// <param name="g">the graph to be iterated.
		/// </param>
		/// <param name="startVertex">the vertex iteration to be started.
		/// </param>
		/// <param name="radius">limit on path length, or Double.POSITIVE_INFINITY for
		/// unbounded search.
		/// </param>
		public ClosestFirstIterator(Graph g, System.Object startVertex, double radius):base(g, startVertex)
		{
			m_radius = radius;
			checkRadiusTraversal(isCrossComponentTraversal());
		}
		
		// override AbstractGraphIterator
		public override void  setCrossComponentTraversal(bool crossComponentTraversal)
		{
			checkRadiusTraversal(crossComponentTraversal);
			base.setCrossComponentTraversal(crossComponentTraversal);
		}
		
		
		/// <summary> Get the length of the shortest path known to the given vertex.  If the
		/// vertex has already been visited, then it is truly the shortest path
		/// length; otherwise, it is the best known upper bound.
		/// 
		/// </summary>
		/// <param name="vertex">vertex being sought from start vertex
		/// 
		/// </param>
		/// <returns> length of shortest path known, or Double.POSITIVE_INFINITY if no
		/// path found yet
		/// </returns>
		public virtual double getShortestPathLength(System.Object vertex)
		{
			QueueEntry entry = (QueueEntry) getSeenData(vertex);
			
			if (entry == null)
			{
				return System.Double.PositiveInfinity;
			}
			
			return entry.ShortestPathLength;
		}
		
		
		/// <summary> Get the spanning tree edge reaching a vertex which has been seen already
		/// in this traversal.  This edge is the last link in the shortest known
		/// path between the start vertex and the requested vertex.  If the vertex
		/// has already been visited, then it is truly the minimum spanning tree
		/// edge; otherwise, it is the best candidate seen so far.
		/// 
		/// </summary>
		/// <param name="vertex">the spanned vertex.
		/// 
		/// </param>
		/// <returns> the spanning tree edge, or null if the vertex either has not
		/// been seen yet or is the start vertex.
		/// </returns>
		public virtual Edge getSpanningTreeEdge(System.Object vertex)
		{
			QueueEntry entry = (QueueEntry) getSeenData(vertex);
			
			if (entry == null)
			{
				return null;
			}
			
			return entry.m_spanningTreeEdge;
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.encounterVertex(java.lang.Object,">
		/// org._3pq.jgrapht.Edge)
		/// </seealso>
		protected internal override void  encounterVertex(System.Object vertex, Edge edge)
		{
			QueueEntry entry = createSeenData(vertex, edge);
			putSeenData(vertex, entry);
			m_heap.insert(entry, entry.ShortestPathLength);
		}
		
		
		/// <summary> Override superclass.  When we see a vertex again, we need to see if the
		/// new edge provides a shorter path than the old edge.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex re-encountered
		/// </param>
		/// <param name="edge">the edge via which the vertex was re-encountered
		/// </param>
		protected internal override void  encounterVertexAgain(System.Object vertex, Edge edge)
		{
			QueueEntry entry = (QueueEntry) getSeenData(vertex);
			
			if (entry.m_frozen)
			{
				// no improvement for this vertex possible
				return ;
			}
			
			double candidatePathLength = calculatePathLength(vertex, edge);
			
			if (candidatePathLength < entry.ShortestPathLength)
			{
				entry.m_spanningTreeEdge = edge;
				m_heap.decreaseKey(entry, candidatePathLength);
			}
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.provideNextVertex()">
		/// </seealso>
		protected internal override System.Object provideNextVertex()
		{
			QueueEntry entry = (QueueEntry) m_heap.removeMin();
			entry.m_frozen = true;
			
			return entry.m_vertex;
		}
		
		
		private void  assertNonNegativeEdge(Edge edge)
		{
			if (edge.Weight < 0)
			{
				throw new System.ArgumentException("negative edge weights not allowed");
			}
		}
		
		
		/// <summary> Determine path length to a vertex via an edge, using the path length for
		/// the opposite vertex.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex for which to calculate the path length.
		/// </param>
		/// <param name="edge">the edge via which the path is being extended.
		/// 
		/// </param>
		/// <returns> calculated path length.
		/// </returns>
		private double calculatePathLength(System.Object vertex, Edge edge)
		{
			assertNonNegativeEdge(edge);
			
			System.Object otherVertex = edge.oppositeVertex(vertex);
			QueueEntry otherEntry = (QueueEntry) getSeenData(otherVertex);
			
			return otherEntry.ShortestPathLength + edge.Weight;
		}
		
		
		private void  checkRadiusTraversal(bool crossComponentTraversal)
		{
			if (crossComponentTraversal && (m_radius != System.Double.PositiveInfinity))
			{
				throw new System.ArgumentException("radius may not be specified for cross-component traversal");
			}
		}
		
		
		/// <summary> The first time we see a vertex, make up a new queue entry for it.
		/// 
		/// </summary>
		/// <param name="vertex">a vertex which has just been encountered.
		/// </param>
		/// <param name="edge">the edge via which the vertex was encountered.
		/// 
		/// </param>
		/// <returns> the new queue entry.
		/// </returns>
		private QueueEntry createSeenData(System.Object vertex, Edge edge)
		{
			double shortestPathLength;
			
			if (edge == null)
			{
				shortestPathLength = 0;
			}
			else
			{
				shortestPathLength = calculatePathLength(vertex, edge);
			}
			
			QueueEntry entry = new QueueEntry(shortestPathLength);
			entry.m_vertex = vertex;
			entry.m_spanningTreeEdge = edge;
			
			return entry;
		}
		
		/// <summary> Private data to associate with each entry in the priority queue.</summary>
		private class QueueEntry:FibonacciHeap.Node
		{
			virtual internal double ShortestPathLength
			{
				get
				{
					return Key;
				}
				
			}
			/// <summary>Best spanning tree edge to vertex seen so far. </summary>
			internal Edge m_spanningTreeEdge;
			
			/// <summary>The vertex reached. </summary>
			internal System.Object m_vertex;
			
			/// <summary>True once m_spanningTreeEdge is guaranteed to be the true minimum. </summary>
			internal bool m_frozen;
			
			internal QueueEntry(double key):base(key)
			{
			}
		}
	}
}
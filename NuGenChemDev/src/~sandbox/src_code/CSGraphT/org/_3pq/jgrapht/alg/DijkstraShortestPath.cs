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
* DijkstraShortestPath.java
* -------------------------
* (C) Copyright 2003, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
*
* $Id: DijkstraShortestPath.java,v 1.3 2005/05/30 05:37:28 perfecthash Exp $
*
* Changes
* -------
* 02-Sep-2003 : Initial revision (JVS);
* 29-May-2005 : Make non-static and add radius support (JVS);
*
*/
using System;
using System.Collections;
using org._3pq.jgrapht.traverse;

namespace org._3pq.jgrapht.alg
{
	
	/// <summary> An implementation of <a
	/// href="http://mathworld.wolfram.com/DijkstrasAlgorithm.html"> Dijkstra's
	/// shortest path algorithm</a> using <code>ClosestFirstIterator</code>.
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 2, 2003
	/// </since>
	public sealed class DijkstraShortestPath
	{
		/// <summary> Return the edges making up the path found.
		/// 
		/// </summary>
		/// <returns> List of Edges, or null if no path exists
		/// </returns>
		public System.Collections.IList PathEdgeList
		{
			get
			{
				return m_edgeList;
			}
			
		}
		/// <summary> Return the length of the path found.
		/// 
		/// </summary>
		/// <returns> path length, or Double.POSITIVE_INFINITY if no path exists
		/// </returns>
		public double PathLength
		{
			get
			{
				return m_pathLength;
			}
			
		}
		private System.Collections.IList m_edgeList;
		private double m_pathLength;
		
		/// <summary> Creates and executes a new DijkstraShortestPath algorithm instance. An
		/// instance is only good for a single search; after construction, it can
		/// be accessed to retrieve information about the path found.
		/// 
		/// </summary>
		/// <param name="graph">the graph to be searched
		/// </param>
		/// <param name="startVertex">the vertex at which the path should start
		/// </param>
		/// <param name="endVertex">the vertex at which the path should end
		/// </param>
		/// <param name="radius">limit on path length, or Double.POSITIVE_INFINITY for
		/// unbounded search
		/// </param>
		public DijkstraShortestPath(Graph graph, System.Object startVertex, System.Object endVertex, double radius)
		{
			ClosestFirstIterator iter = new ClosestFirstIterator(graph, startVertex, radius);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object vertex = iter.Current;
				
				if (vertex.Equals(endVertex))
				{
					createEdgeList(iter, endVertex);
					m_pathLength = iter.getShortestPathLength(endVertex);
					
					return ;
				}
			}
			
			m_edgeList = null;
			m_pathLength = System.Double.PositiveInfinity;
		}
		
		
		/// <summary> Convenience method to find the shortest path via a single static method
		/// call.  If you need a more advanced search (e.g. limited by radius, or
		/// computation of the path length), use the constructor instead.
		/// 
		/// </summary>
		/// <param name="graph">the graph to be searched
		/// </param>
		/// <param name="startVertex">the vertex at which the path should start
		/// </param>
		/// <param name="endVertex">the vertex at which the path should end
		/// 
		/// </param>
		/// <returns> List of Edges, or null if no path exists
		/// </returns>
		public static System.Collections.IList findPathBetween(Graph graph, System.Object startVertex, System.Object endVertex)
		{
			DijkstraShortestPath alg = new DijkstraShortestPath(graph, startVertex, endVertex, System.Double.PositiveInfinity);
			
			return alg.PathEdgeList;
		}
		
		
		private void  createEdgeList(ClosestFirstIterator iter, System.Object endVertex)
		{
			m_edgeList = new System.Collections.ArrayList();
			
			while (true)
			{
				Edge edge = iter.getSpanningTreeEdge(endVertex);
				
				if (edge == null)
				{
					break;
				}
				
				m_edgeList.Add(edge);
				endVertex = edge.oppositeVertex(endVertex);
			}
			
			((ArrayList)m_edgeList).Reverse();
		}
	}
}
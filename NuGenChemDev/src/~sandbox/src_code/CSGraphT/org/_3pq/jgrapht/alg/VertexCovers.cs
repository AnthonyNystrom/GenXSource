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
/* -----------------
* VertexCovers.java
* -----------------
* (C) Copyright 2003, by Linda Buisman and Contributors.
*
* Original Author:  Linda Buisman
* Contributor(s):   Barak Naveh
*
* $Id: VertexCovers.java,v 1.4 2004/11/18 20:46:24 barak_naveh Exp $
*
* Changes
* -------
* 06-Nov-2003 : Initial revision (LB);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
using UndirectedGraph = org._3pq.jgrapht.UndirectedGraph;
using VertexDegreeComparator = org._3pq.jgrapht.alg.util.VertexDegreeComparator;
using Subgraph = org._3pq.jgrapht.graph.Subgraph;
using UndirectedSubgraph = org._3pq.jgrapht.graph.UndirectedSubgraph;
namespace org._3pq.jgrapht.alg
{
	
	/// <summary> Algorithms to find a vertex cover for a graph. A vertex cover is a set of
	/// vertices that touches all the edges in the graph. The graph's vertex set is
	/// a trivial cover. However, a <i>minimal</i> vertex set (or at least an
	/// approximation for it) is usually desired. Finding a true minimal vertex
	/// cover is an NP-Complete problem. For more on the vertex cover problem, see
	/// <a href="http://mathworld.wolfram.com/VertexCover.html">
	/// http://mathworld.wolfram.com/VertexCover.html</a>
	/// 
	/// </summary>
	/// <author>  Linda Buisman
	/// 
	/// </author>
	/// <since> Nov 6, 2003
	/// </since>
	public class VertexCovers
	{
		/// <summary> Finds a 2-approximation for a minimal vertex cover of the specified
		/// graph. The algorithm promises a cover that is at most double the size
		/// of a minimal cover. The algorithm takes O(|E|) time.
		/// 
		/// <p>
		/// For more details see Jenny Walter, CMPU-240: Lecture notes for Language
		/// Theory and Computation, Fall 2002, Vassar College, <a
		/// href="http://www.cs.vassar.edu/~walter/cs241index/lectures/PDF/approx.pdf">
		/// 
		/// http://www.cs.vassar.edu/~walter/cs241index/lectures/PDF/approx.pdf</a>.
		/// </p>
		/// 
		/// </summary>
		/// <param name="g">the graph for which vertex cover approximation is to be found.
		/// 
		/// </param>
		/// <returns> a set of vertices which is a vertex cover for the specified
		/// graph.
		/// </returns>
		public virtual SupportClass.SetSupport find2ApproximationCover(Graph g)
		{
			// C <-- {}
			//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
			SupportClass.SetSupport cover = new SupportClass.HashSetSupport();
			
			// G'=(V',E') <-- G(V,E)
			Subgraph sg = new Subgraph(g, null, null);
			
			// while E' is non-empty
			while (sg.edgeSet().Count > 0)
			{
				// let (u,v) be an arbitrary edge of E'
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				Edge e = (Edge) sg.edgeSet().GetEnumerator().Current;
				
				// C <-- C U {u,v}
				System.Object u = e.Source;
				System.Object v = e.Target;
				cover.Add(u);
				cover.Add(v);
				
				// remove from E' every edge incident on either u or v
				sg.removeVertex(u);
				sg.removeVertex(v);
			}
			
			return cover; // return C
		}
		
		
		/// <summary> Finds a greedy approximation for a minimal vertex cover of a specified
		/// graph. At each iteration, the algorithm picks the vertex with the
		/// highest degree and adds it to the cover, until all edges are covered.
		/// 
		/// <p>
		/// The algorithm works on undirected graphs, but can also work on directed
		/// graphs when their edge-directions are ignored. To ignore edge
		/// directions you can use {@link
		/// org._3pq.jgrapht.GraphHelper#undirectedGraph(Graph)} or {@link
		/// org._3pq.jgrapht.graph.AsUndirectedGraph}.
		/// </p>
		/// 
		/// </summary>
		/// <param name="g">the graph for which vertex cover approximation is to be found.
		/// 
		/// </param>
		/// <returns> a set of vertices which is a vertex cover for the specified
		/// graph.
		/// </returns>
		public virtual SupportClass.SetSupport findGreedyCover(UndirectedGraph g)
		{
			// C <-- {}
			//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
			SupportClass.SetSupport cover = new SupportClass.HashSetSupport();
			
			// G' <-- G
			UndirectedGraph sg = new UndirectedSubgraph(g, null, null);
			
			// compare vertices in descending order of degree
			VertexDegreeComparator comp = new VertexDegreeComparator(sg);
			
			// while G' != {}
			while (sg.edgeSet().Count > 0)
			{
				// v <-- vertex with maximum degree in G'
				System.Object v = SupportClass.CollectionsSupport.Max(sg.vertexSet(), comp);
				
				// C <-- C U {v}
				cover.Add(v);
				
				// remove from G' every edge incident on v, and v itself
				sg.removeVertex(v);
			}
			
			return cover;
		}
	}
}
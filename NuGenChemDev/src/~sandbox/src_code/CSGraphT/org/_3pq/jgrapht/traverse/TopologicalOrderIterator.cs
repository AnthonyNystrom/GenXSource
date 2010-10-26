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
/* -----------------------------
* TopologicalOrderIterator.java
* -----------------------------
* (C) Copyright 2004, by Marden Neubert and Contributors.
*
* Original Author:  Marden Neubert
* Contributor(s):   Barak Naveh, John V. Sichi
*
* $Id: TopologicalOrderIterator.java,v 1.2 2005/04/26 06:46:38 perfecthash Exp $
*
* Changes
* -------
* 17-Dec-2004 : Initial revision (MN);
* 25-Apr-2005 : Fixes for start vertex order (JVS);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using Edge = org._3pq.jgrapht.Edge;
using ModifiableInteger = org._3pq.jgrapht.util.ModifiableInteger;
namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> Implements topological order traversal for a directed graph. A topological
	/// sort is a permutation <tt>p</tt> of the vertices of a graph such that an
	/// edge <tt>(i,j)</tt> implies that <tt>i</tt> appears before <tt>j</tt> in
	/// <tt>p</tt> (Skiena 1990, p. 208). See also <a
	/// href="http://mathworld.wolfram.com/TopologicalSort.html">
	/// http://mathworld.wolfram.com/TopologicalSort.html</a>.
	/// 
	/// <p>
	/// See "Algorithms in Java, Third Edition, Part 5: Graph Algorithms" by Robert
	/// Sedgewick and "Data Structures and Algorithms with Object-Oriented Design
	/// Patterns in Java" by Bruno R. Preiss for implementation alternatives. The
	/// latter can be found online at <a
	/// href="http://www.brpreiss.com/books/opus5/">http://www.brpreiss.com/books/opus5/</a>
	/// </p>
	/// 
	/// <p>
	/// For this iterator to work correctly the graph must not be modified during
	/// iteration. Currently there are no means to ensure that, nor to fail-fast.
	/// The results of such modifications are undefined.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Marden Neubert
	/// 
	/// </author>
	/// <since> Dec 18, 2004
	/// </since>
	public class TopologicalOrderIterator:CrossComponentIterator
	{
		/// <seealso cref="CrossComponentIterator.isConnectedComponentExhausted()">
		/// </seealso>
		override protected internal bool ConnectedComponentExhausted
		{
			get
			{
				// FIXME jvs 25-Apr-2005: This isn't correct for a graph with more than
				// one component.  We will actually exhaust a connected component
				// before the queue is empty, because initialize adds roots from all
				// components to the queue.
				return (m_queue.Count == 0);
			}
			
		}
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		private System.Collections.ArrayList m_queue;
		private System.Collections.IDictionary m_inDegreeMap;
		
		/// <summary> Creates a new topological order iterator over the directed graph
		/// specified. Traversal will start at one of the graphs <i>sources</i>.
		/// See the definition of source at <a
		/// href="http://mathworld.wolfram.com/Source.html">
		/// http://mathworld.wolfram.com/Source.html</a>.
		/// 
		/// </summary>
		/// <param name="dg">the directed graph to be iterated.
		/// </param>
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
		public TopologicalOrderIterator(DirectedGraph dg):this(dg, new System.Collections.ArrayList(), new System.Collections.Hashtable())
		{
		}
		
		
		// NOTE: This is a hack to deal with the fact that CrossComponentIterator
		// needs to know the start vertex in its constructor
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		private TopologicalOrderIterator(DirectedGraph dg, System.Collections.ArrayList queue, System.Collections.IDictionary inDegreeMap):this(dg, initialize(dg, queue, inDegreeMap))
		{
			m_queue = queue;
			m_inDegreeMap = inDegreeMap;
		}
		
		
		// NOTE: This is intentionally private, because starting the sort "in the
		// middle" doesn't make sense.
		private TopologicalOrderIterator(DirectedGraph dg, System.Object start):base(dg, start)
		{
		}
		
		
		/// <seealso cref="CrossComponentIterator.encounterVertex(Object, Edge)">
		/// </seealso>
		protected internal override void  encounterVertex(System.Object vertex, Edge edge)
		{
			putSeenData(vertex, (System.Object) null);
			decrementInDegree(vertex);
		}
		
		
		/// <seealso cref="CrossComponentIterator.encounterVertexAgain(Object, Edge)">
		/// </seealso>
		protected internal override void  encounterVertexAgain(System.Object vertex, Edge edge)
		{
			decrementInDegree(vertex);
		}
		
		
		/// <seealso cref="CrossComponentIterator.provideNextVertex()">
		/// </seealso>
		protected internal override System.Object provideNextVertex()
		{
			System.Object tempObject;
			tempObject = m_queue[0];
			m_queue.RemoveAt(0);
			return tempObject;
		}
		
		
		/// <summary> Decrements the in-degree of a vertex.
		/// 
		/// </summary>
		/// <param name="vertex">the vertex whose in-degree will be decremented.
		/// </param>
		private void  decrementInDegree(System.Object vertex)
		{
			ModifiableInteger inDegree = (ModifiableInteger) m_inDegreeMap[vertex];
			
			if (inDegree.value_Renamed > 0)
			{
				inDegree.value_Renamed--;
				
				if (inDegree.value_Renamed == 0)
				{
					m_queue.Insert(m_queue.Count, vertex);
				}
			}
		}
		
		
		/// <summary> Initializes the internal traversal object structure. Sets up the
		/// internal queue with the directed graph vertices and creates the control
		/// structure for the in-degrees.
		/// 
		/// </summary>
		/// <param name="dg">the directed graph to be iterated.
		/// </param>
		/// <param name="queue">initializer for m_queue
		/// </param>
		/// <param name="inDegreeMap">initializer for m_inDegreeMap
		/// 
		/// </param>
		/// <returns> start vertex
		/// </returns>
		//UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
		private static System.Object initialize(DirectedGraph dg, System.Collections.ArrayList queue, System.Collections.IDictionary inDegreeMap)
		{
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			for (System.Collections.IEnumerator i = dg.vertexSet().GetEnumerator(); i.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object vertex = i.Current;
				
				int inDegree = dg.inDegreeOf(vertex);
				inDegreeMap[vertex] = new ModifiableInteger(inDegree);
				
				if (inDegree == 0)
				{
					queue.Add(vertex);
				}
			}
			
			if ((queue.Count == 0))
			{
				return null;
			}
			else
			{
				return queue[0];
			}
		}
	}
}
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
/* -----------------------
* DepthFirstIterator.java
* -----------------------
* (C) Copyright 2003, by Liviu Rau and Contributors.
*
* Original Author:  Liviu Rau
* Contributor(s):   Barak Naveh
*
* $Id: DepthFirstIterator.java,v 1.10 2004/10/29 21:30:48 barak_naveh Exp $
*
* Changes
* -------
* 29-Jul-2003 : Initial revision (LR);
* 31-Jul-2003 : Fixed traversal across connected components (BN);
* 06-Aug-2003 : Extracted common logic to TraverseUtils.XXFirstIterator (BN);
* 31-Jan-2004 : Reparented and changed interface to parent class (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> A depth-first iterator for a directed and an undirected graph. For this
	/// iterator to work correctly the graph must not be modified during iteration.
	/// Currently there are no means to ensure that, nor to fail-fast. The results
	/// of such modifications are undefined.
	/// 
	/// </summary>
	/// <author>  Liviu Rau
	/// </author>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 29, 2003
	/// </since>
	public class DepthFirstIterator:CrossComponentIterator
	{
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.isConnectedComponentExhausted()">
		/// </seealso>
		override protected internal bool ConnectedComponentExhausted
		{
			get
			{
				return (m_stack.Count == 0);
			}
			
		}
		private System.Collections.IList m_stack = new System.Collections.ArrayList();
		
		/// <summary> Creates a new depth-first iterator for the specified graph.
		/// 
		/// </summary>
		/// <param name="g">the graph to be iterated.
		/// </param>
		public DepthFirstIterator(Graph g):this(g, null)
		{
		}
		
		
		/// <summary> Creates a new depth-first iterator for the specified graph. Iteration
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
		public DepthFirstIterator(Graph g, System.Object startVertex):base(g, startVertex)
		{
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.encounterVertex(java.lang.Object,">
		/// org._3pq.jgrapht.Edge)
		/// </seealso>
		protected internal override void  encounterVertex(System.Object vertex, Edge edge)
		{
			putSeenData(vertex, (System.Object) null);
			m_stack.Add(vertex);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.encounterVertexAgain(java.lang.Object,">
		/// org._3pq.jgrapht.Edge)
		/// </seealso>
		protected internal override void  encounterVertexAgain(System.Object vertex, Edge edge)
		{
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.traverse.CrossComponentIterator.provideNextVertex()">
		/// </seealso>
		protected internal override System.Object provideNextVertex()
		{
			System.Object tempObject;
			tempObject = m_stack[m_stack.Count - 1];
			m_stack.RemoveAt(m_stack.Count - 1);
			return tempObject;
		}
	}
}
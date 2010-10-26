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
/* ------------------
* CycleDetector.java
* ------------------
* (C) Copyright 2004, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
* Contributor(s):   -
*
* $Id: CycleDetector.java,v 1.4 2005/04/23 08:09:28 perfecthash Exp $
*
* Changes
* -------
* 16-Sept-2004 : Initial revision (JVS);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using Edge = org._3pq.jgrapht.Edge;
using Graph = org._3pq.jgrapht.Graph;
using DepthFirstIterator = org._3pq.jgrapht.traverse.DepthFirstIterator;
namespace org._3pq.jgrapht.alg
{
	
	/// <summary> Performs cycle detection on a graph. The <i>inspected graph</i> is specified
	/// at construction time and cannot be modified. Currently, the detector
	/// supports only directed graphs.
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sept 16, 2004
	/// </since>
	public class CycleDetector
	{
		/// <summary>Graph on which cycle detection is being performed. </summary>
		internal Graph m_graph;
		
		/// <summary> Creates a cycle detector for the specified graph.  Currently only
		/// directed graphs are supported.
		/// 
		/// </summary>
		/// <param name="graph">the DirectedGraph in which to detect cycles
		/// </param>
		public CycleDetector(DirectedGraph graph)
		{
			m_graph = graph;
		}
		
		/// <summary> Performs yes/no cycle detection on the entire graph.
		/// 
		/// </summary>
		/// <returns> true iff the graph contains at least one cycle
		/// </returns>
		public virtual bool detectCycles()
		{
			try
			{
				execute(null, (System.Object) null);
			}
			catch (CycleDetectedException ex)
			{
				return true;
			}
			
			return false;
		}
		
		
		/// <summary> Performs yes/no cycle detection on an individual vertex.
		/// 
		/// </summary>
		/// <param name="v">the vertex to test
		/// 
		/// </param>
		/// <returns> true if v is on at least one cycle
		/// </returns>
		public virtual bool detectCyclesContainingVertex(System.Object v)
		{
			try
			{
				execute(null, v);
			}
			catch (CycleDetectedException ex)
			{
				return true;
			}
			
			return false;
		}
		
		
		/// <summary> Finds the vertex set for the subgraph of all cycles.
		/// 
		/// </summary>
		/// <returns> set of all vertices which participate in at least one cycle in
		/// this graph
		/// </returns>
		public virtual SupportClass.SetSupport findCycles()
		{
			//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
			SupportClass.SetSupport set_Renamed = new SupportClass.HashSetSupport();
			execute(set_Renamed, (System.Object) null);
			
			return set_Renamed;
		}
		
		
		/// <summary> Finds the vertex set for the subgraph of all cycles which contain a
		/// particular vertex.
		/// 
		/// </summary>
		/// <param name="v">the vertex to test
		/// 
		/// </param>
		/// <returns> set of all vertices reachable from v via at least one cycle
		/// </returns>
		public virtual SupportClass.SetSupport findCyclesContainingVertex(System.Object v)
		{
			//UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
			SupportClass.SetSupport set_Renamed = new SupportClass.HashSetSupport();
			execute(set_Renamed, v);
			
			return set_Renamed;
		}
		
		
		private void  execute(SupportClass.SetSupport s, System.Object v)
		{
			ProbeIterator iter = new ProbeIterator(this, s, v);
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (iter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object generatedAux = iter.Current;
			}
		}
		
		/// <summary> Exception thrown internally when a cycle is detected during a yes/no
		/// cycle test.  Must be caught by top-level detection method.
		/// </summary>
		[Serializable]
		private class CycleDetectedException:System.SystemException
		{
			private const long serialVersionUID = 3834305137802950712L;
		}
		
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'ProbeIterator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		/// <summary> Version of DFS which maintains a backtracking path used to probe for
		/// cycles.
		/// </summary>
		private class ProbeIterator:DepthFirstIterator
		{
			private void  InitBlock(CycleDetector enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private CycleDetector enclosingInstance;
			public CycleDetector Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			private System.Collections.IList m_path;
			private SupportClass.SetSupport m_cycleSet;
			
			internal ProbeIterator(CycleDetector enclosingInstance, SupportClass.SetSupport cycleSet, System.Object startVertex):base(Enclosing_Instance.m_graph, startVertex)
			{
				InitBlock(enclosingInstance);
				m_cycleSet = cycleSet;
				m_path = new System.Collections.ArrayList();
			}
			
			/// <summary> {@inheritDoc}</summary>
			protected internal override void  encounterVertexAgain(System.Object vertex, Edge edge)
			{
				base.encounterVertexAgain(vertex, edge);
				
				int i = m_path.IndexOf(vertex);
				
				if (i > - 1)
				{
					if (m_cycleSet == null)
					{
						// we're doing yes/no cycle detection
						throw new CycleDetectedException();
					}
					
					for (; i < m_path.Count; ++i)
					{
						m_cycleSet.Add(m_path[i]);
					}
				}
			}
			
			
			/// <summary> {@inheritDoc}</summary>
			protected internal override System.Object provideNextVertex()
			{
				System.Object v = base.provideNextVertex();
				
				// backtrack
				for (int i = m_path.Count - 1; i >= 0; --i)
				{
					if (Enclosing_Instance.m_graph.containsEdge(m_path[i], v))
					{
						break;
					}
					
					m_path.RemoveAt(i);
				}
				
				m_path.Add(v);
				
				return v;
			}
		}
	}
}
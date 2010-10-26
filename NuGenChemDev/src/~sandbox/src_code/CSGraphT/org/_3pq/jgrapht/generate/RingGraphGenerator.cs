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
/* -------------------
* GraphGenerator.java
* -------------------
* (C) Copyright 2003, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
* Contributor(s):   -
*
* $Id: RingGraphGenerator.java,v 1.2 2004/11/18 22:09:37 barak_naveh Exp $
*
* Changes
* -------
* 16-Sep-2003 : Initial revision (JVS);
*
*/
using System;
using Graph = org._3pq.jgrapht.Graph;
using VertexFactory = org._3pq.jgrapht.VertexFactory;
namespace org._3pq.jgrapht.generate
{
	
	/// <summary> Generates a ring graph of any size. A ring graph is a graph that contains a
	/// single cycle that passes through all its vertices exactly once. For a
	/// directed graph, the generated edges are oriented consistently around the
	/// ring.
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 16, 2003
	/// </since>
	public class RingGraphGenerator : GraphGenerator
	{
		private int m_size;
		
		/// <summary> Construct a new RingGraphGenerator.
		/// 
		/// </summary>
		/// <param name="size">number of vertices to be generated
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException if the specified size is negative. </throws>
		public RingGraphGenerator(int size)
		{
			if (size < 0)
			{
				throw new System.ArgumentException("must be non-negative");
			}
			
			m_size = size;
		}
		
		/// <summary> {@inheritDoc}</summary>
		public virtual void  generateGraph(Graph target, VertexFactory vertexFactory, System.Collections.IDictionary resultMap)
		{
			if (m_size < 1)
			{
				return ;
			}
			
			LinearGraphGenerator linearGenerator = new LinearGraphGenerator(m_size);
			//UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
			System.Collections.IDictionary privateMap = new System.Collections.Hashtable();
			linearGenerator.generateGraph(target, vertexFactory, privateMap);
			
			System.Object startVertex = privateMap[LinearGraphGenerator.START_VERTEX];
			System.Object endVertex = privateMap[LinearGraphGenerator.END_VERTEX];
			target.addEdge(endVertex, startVertex);
		}
	}
}
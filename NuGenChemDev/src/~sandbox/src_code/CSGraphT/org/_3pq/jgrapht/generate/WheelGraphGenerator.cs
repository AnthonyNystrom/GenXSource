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
* $Id: WheelGraphGenerator.java,v 1.3 2004/11/18 22:10:06 barak_naveh Exp $
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
	
	/// <summary> Generates a <a href="http://mathworld.wolfram.com/WheelGraph.html">wheel
	/// graph</a> of any size. Reminding a bicycle wheel, a wheel graph has a hub
	/// vertex in the center and a rim of vertices around it that are connected to
	/// each other (as a ring). The rim vertices are also connected to the hub with
	/// edges that are called "spokes".
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 16, 2003
	/// </since>
	public class WheelGraphGenerator : GraphGenerator
	{
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassVertexFactory' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
		private class AnonymousClassVertexFactory : VertexFactory
		{
			public AnonymousClassVertexFactory(org._3pq.jgrapht.VertexFactory vertexFactory, System.Collections.ICollection rim, WheelGraphGenerator enclosingInstance)
			{
				InitBlock(vertexFactory, rim, enclosingInstance);
			}
			private void  InitBlock(org._3pq.jgrapht.VertexFactory vertexFactory, System.Collections.ICollection rim, WheelGraphGenerator enclosingInstance)
			{
				this.vertexFactory = vertexFactory;
				this.rim = rim;
				this.enclosingInstance = enclosingInstance;
			}
			//UPGRADE_NOTE: Final variable vertexFactory was copied into class AnonymousClassVertexFactory. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private org._3pq.jgrapht.VertexFactory vertexFactory;
			//UPGRADE_NOTE: Final variable rim was copied into class AnonymousClassVertexFactory. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private System.Collections.ICollection rim;
			private WheelGraphGenerator enclosingInstance;
			public WheelGraphGenerator Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			public virtual System.Object createVertex()
			{
				System.Object vertex = vertexFactory.createVertex();
				SupportClass.ICollectionSupport.Add(rim, vertex);
				
				return vertex;
			}
		}
		/// <summary>Role for the hub vertex. </summary>
		public const System.String HUB_VERTEX = "Hub Vertex";
		private bool m_inwardSpokes;
		private int m_size;
		
		/// <summary> Creates a new WheelGraphGenerator object. This constructor is more
		/// suitable for undirected graphs, where spokes' direction is meaningless.
		/// In the directed case, spokes will be oriented from rim to hub.
		/// 
		/// </summary>
		/// <param name="size">number of vertices to be generated.
		/// </param>
		public WheelGraphGenerator(int size):this(size, true)
		{
		}
		
		
		/// <summary> Construct a new WheelGraphGenerator.
		/// 
		/// </summary>
		/// <param name="size">number of vertices to be generated.
		/// </param>
		/// <param name="inwardSpokes">if <code>true</code> and graph is directed, spokes
		/// are oriented from rim to hub; else from hub to rim.
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException </throws>
		public WheelGraphGenerator(int size, bool inwardSpokes)
		{
			if (size < 0)
			{
				throw new System.ArgumentException("must be non-negative");
			}
			
			m_size = size;
			m_inwardSpokes = inwardSpokes;
		}
		
		/// <summary> {@inheritDoc}</summary>
		public virtual void  generateGraph(Graph target, VertexFactory vertexFactory, System.Collections.IDictionary resultMap)
		{
			if (m_size < 1)
			{
				return ;
			}
			
			// A little trickery to intercept the rim generation.  This is
			// necessary since target may be initially non-empty, meaning we can't
			// rely on its vertex set after the rim is generated.
			//UPGRADE_NOTE: Final was removed from the declaration of 'rim '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			System.Collections.ICollection rim = new System.Collections.ArrayList();
			VertexFactory rimVertexFactory = new AnonymousClassVertexFactory(vertexFactory, rim, this);
			
			RingGraphGenerator ringGenerator = new RingGraphGenerator(m_size - 1);
			ringGenerator.generateGraph(target, rimVertexFactory, resultMap);
			
			System.Object hubVertex = vertexFactory.createVertex();
			target.addVertex(hubVertex);
			
			if (resultMap != null)
			{
				resultMap[HUB_VERTEX] = hubVertex;
			}
			
			System.Collections.IEnumerator rimIter = rim.GetEnumerator();
			
			//UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
			while (rimIter.MoveNext())
			{
				//UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
				System.Object rimVertex = rimIter.Current;
				
				if (m_inwardSpokes)
				{
					target.addEdge(rimVertex, hubVertex);
				}
				else
				{
					target.addEdge(hubVertex, rimVertex);
				}
			}
		}
	}
}
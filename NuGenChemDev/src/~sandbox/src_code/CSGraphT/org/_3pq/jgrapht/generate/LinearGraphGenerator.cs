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
* $Id: LinearGraphGenerator.java,v 1.4 2005/04/23 08:09:29 perfecthash Exp $
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
	
	/// <summary> Generates a linear graph of any size.  For a directed graph, the edges are
	/// oriented from START_VERTEX to END_VERTEX.
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 16, 2003
	/// </since>
	public class LinearGraphGenerator : GraphGenerator
	{
		/// <summary>Role for the first vertex generated. </summary>
		public const System.String START_VERTEX = "Start Vertex";
		
		/// <summary>Role for the last vertex generated. </summary>
		public const System.String END_VERTEX = "End Vertex";
		private int m_size;
		
		/// <summary> Construct a new LinearGraphGenerator.
		/// 
		/// </summary>
		/// <param name="size">number of vertices to be generated
		/// 
		/// </param>
		/// <throws>  IllegalArgumentException if the specified size is negative. </throws>
		public LinearGraphGenerator(int size)
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
			System.Object lastVertex = null;
			
			for (int i = 0; i < m_size; ++i)
			{
				System.Object newVertex = vertexFactory.createVertex();
				target.addVertex(newVertex);
				
				if (lastVertex == null)
				{
					if (resultMap != null)
					{
						resultMap[START_VERTEX] = newVertex;
					}
				}
				else
				{
					target.addEdge(lastVertex, newVertex);
				}
				
				lastVertex = newVertex;
			}
			
			if ((resultMap != null) && (lastVertex != null))
			{
				resultMap[END_VERTEX] = lastVertex;
			}
		}
	}
}
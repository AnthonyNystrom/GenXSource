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
/* ---------------------------
* VertexDegreeComparator.java
* ---------------------------
* (C) Copyright 2003, by Linda Buisman and Contributors.
*
* Original Author:  Linda Buisman
* Contributor(s):   -
*
* $Id: VertexDegreeComparator.java,v 1.3 2004/09/17 07:24:11 perfecthash Exp $
*
* Changes
* -------
* 06-Nov-2003 : Initial revision (LB);
*
*/
using System;
using UndirectedGraph = org._3pq.jgrapht.UndirectedGraph;
namespace org._3pq.jgrapht.alg.util
{
	
	/// <summary> Compares two vertices based on their degree.
	/// 
	/// <p>
	/// Used by greedy algorithms that need to sort vertices by their degree. Two
	/// vertices are considered equal if their degrees are equal.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Linda Buisman
	/// 
	/// </author>
	/// <since> Nov 6, 2003
	/// </since>
	public class VertexDegreeComparator : System.Collections.IComparer
	{
		/// <summary>The graph that contains the vertices to be compared. </summary>
		private UndirectedGraph m_graph;
		
		/// <summary> The sort order for vertex degree. <code>true</code>for ascending degree
		/// order (smaller degrees first), <code>false</code> for descending.
		/// </summary>
		private bool m_ascendingOrder;
		
		/// <summary> Creates a comparator for comparing the degrees of vertices in the
		/// specified graph. The comparator compares in ascending order of degrees
		/// (lowest first).
		/// 
		/// </summary>
		/// <param name="g">graph with respect to which the degree is calculated.
		/// </param>
		public VertexDegreeComparator(UndirectedGraph g):this(g, true)
		{
		}
		
		
		/// <summary> Creates a comparator for comparing the degrees of vertices in the
		/// specified graph.
		/// 
		/// </summary>
		/// <param name="g">graph with respect to which the degree is calculated.
		/// </param>
		/// <param name="ascendingOrder">true - compares in ascending order of degrees
		/// (lowest first), false - compares in descending order of degrees
		/// (highest first).
		/// </param>
		public VertexDegreeComparator(UndirectedGraph g, bool ascendingOrder)
		{
			m_graph = g;
			m_ascendingOrder = ascendingOrder;
		}
		
		/// <summary> Compare the degrees of <code>v1</code> and <code>v2</code>, taking into
		/// account whether ascending or descending order is used.
		/// 
		/// </summary>
		/// <param name="v1">the first vertex to be compared.
		/// </param>
		/// <param name="v2">the second vertex to be compared.
		/// 
		/// </param>
		/// <returns> -1 if <code>v1</code> comes before <code>v2</code>,  +1 if
		/// <code>v1</code> comes after <code>v2</code>, 0 if equal.
		/// </returns>
		public virtual int Compare(System.Object v1, System.Object v2)
		{
			int degree1 = m_graph.degreeOf(v1);
			int degree2 = m_graph.degreeOf(v2);
			
			if ((degree1 < degree2 && m_ascendingOrder) || (degree1 > degree2 && !m_ascendingOrder))
			{
				return - 1;
			}
			else if ((degree1 > degree2 && m_ascendingOrder) || (degree1 < degree2 && !m_ascendingOrder))
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}
}
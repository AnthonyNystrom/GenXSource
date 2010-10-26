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
* UndirectedWeightedEdge.java
* ---------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: UndirectedWeightedEdge.java,v 1.9 2004/11/18 21:44:24 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 10-Aug-2003 : General edge refactoring (BN);
*
*/
using System;
namespace org._3pq.jgrapht.edge
{
	
	/// <summary> An implementation of undirected weighted edge.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 16, 2003
	/// </since>
	[Serializable]
	public class UndirectedWeightedEdge:UndirectedEdge
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <seealso cref="org._3pq.jgrapht.Edge.getWeight()">
		/// </seealso>
		/// <seealso cref="org._3pq.jgrapht.Edge.setWeight(double)">
		/// </seealso>
		override public double Weight
		{
			get
			{
				return m_weight;
			}
			
			set
			{
				m_weight = value;
			}
			
		}
		private const long serialVersionUID = 4120853256903012915L;
		private double m_weight = org._3pq.jgrapht.Edge_Fields.DEFAULT_EDGE_WEIGHT;
		
		/// <seealso cref="UndirectedEdge.UndirectedEdge(Object, Object)">
		/// </seealso>
		public UndirectedWeightedEdge(System.Object sourceVertex, System.Object targetVertex):base(sourceVertex, targetVertex)
		{
		}
		
		
		/// <summary> Constructor for UndirectedWeightedEdge.
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the new edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the new edge.
		/// </param>
		/// <param name="weight">the weight of the new edge.
		/// </param>
		public UndirectedWeightedEdge(System.Object sourceVertex, System.Object targetVertex, double weight):base(sourceVertex, targetVertex)
		{
			m_weight = weight;
		}
	}
}
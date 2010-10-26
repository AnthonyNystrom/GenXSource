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
/* ----------------
* DefaultEdge.java
* ----------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: DefaultEdge.java,v 1.5 2005/04/23 08:09:29 perfecthash Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 10-Aug-2003 : General edge refactoring (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
namespace org._3pq.jgrapht.edge
{
	
	/// <summary> A skeletal implementation of the <tt>Edge</tt> interface, to minimize the
	/// effort required to implement the interface.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	[Serializable]
	public class DefaultEdge : Edge, System.ICloneable
	{
		/// <seealso cref="org._3pq.jgrapht.Edge.getSource()">
		/// </seealso>
		virtual public System.Object Source
		{
			get
			{
				return m_source;
			}
			
		}
		/// <seealso cref="org._3pq.jgrapht.Edge.getTarget()">
		/// </seealso>
		virtual public System.Object Target
		{
			get
			{
				return m_target;
			}
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <seealso cref="org._3pq.jgrapht.Edge.getWeight()">
		/// </seealso>
		/// <seealso cref="org._3pq.jgrapht.Edge.setWeight(double)">
		/// </seealso>
		virtual public double Weight
		{
			get
			{
				return org._3pq.jgrapht.Edge_Fields.DEFAULT_EDGE_WEIGHT;
			}
			
			set
			{
				throw new System.NotSupportedException();
			}
			
		}
		private const long serialVersionUID = 3258408452177932855L;
		private System.Object m_source;
		private System.Object m_target;
		
		/// <summary> Constructor for DefaultEdge.
		/// 
		/// </summary>
		/// <param name="sourceVertex">source vertex of the edge.
		/// </param>
		/// <param name="targetVertex">target vertex of the edge.
		/// </param>
		public DefaultEdge(System.Object sourceVertex, System.Object targetVertex)
		{
			m_source = sourceVertex;
			m_target = targetVertex;
		}
		
		
		/// <seealso cref="Edge.clone()">
		/// </seealso>
		public virtual System.Object Clone()
		{
			try
			{
				return base.MemberwiseClone();
			}
			//UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				// shouldn't happen as we are Cloneable
				throw new System.ApplicationException();
			}
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Edge.containsVertex(java.lang.Object)">
		/// </seealso>
		public virtual bool containsVertex(System.Object v)
		{
			return m_source.Equals(v) || m_target.Equals(v);
		}
		
		
		/// <seealso cref="org._3pq.jgrapht.Edge.oppositeVertex(java.lang.Object)">
		/// </seealso>
		public virtual System.Object oppositeVertex(System.Object v)
		{
			if (v.Equals(m_source))
			{
				return m_target;
			}
			else if (v.Equals(m_target))
			{
				return m_source;
			}
			else
			{
				throw new System.ArgumentException("no such vertex");
			}
		}
	}
}
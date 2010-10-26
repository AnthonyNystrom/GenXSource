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
/* ---------
* Edge.java
* ---------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: Edge.java,v 1.6 2004/05/01 23:15:46 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 06-Nov-2003 : Change edge sharing semantics (JVS);
*
*/
using System;
namespace org._3pq.jgrapht
{
	
	/// <summary> An edge used with graph objects. This is the root interface in the edge
	/// hierarchy.
	/// 
	/// <p>
	/// NOTE: the source and target associations of an Edge must be immutable after
	/// construction for all implementations.  The reason is that once an Edge is
	/// added to a Graph, the Graph representation may be optimized via internal
	/// indexing data structures; if the Edge associations were to change, these
	/// structures would be corrupted.  However, other properties of an edge (such
	/// as weight or label) may be mutable, although this still requires caution:
	/// changes to Edges shared by multiple Graphs may not always be desired, and
	/// indexing mechanisms for these properties may require a change notification
	/// mechanism.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	public struct Edge_Fields{
		/// <summary>The default weight for an edge. </summary>
		public readonly static double DEFAULT_EDGE_WEIGHT = 1.0;
	}
	public interface Edge:System.ICloneable
	{
		//UPGRADE_NOTE: Members of interface 'Edge' were extracted into structure 'Edge_Fields'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1045'"
		/// <summary> Returns the source vertex of this edge.
		/// 
		/// </summary>
		/// <returns> the source vertex of this edge.
		/// </returns>
		System.Object Source
		{
			get;
			
		}
		/// <summary> Returns the target vertex of this edge.
		/// 
		/// </summary>
		/// <returns> the target vertex of this edge.
		/// </returns>
		System.Object Target
		{
			get;
			
		}
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the weight of this edge. If this edge is unweighted the value
		/// <code>1.0</code> is returned.
		/// 
		/// </summary>
		/// <returns> the weight of this element.
		/// </returns>
		/// <summary> Sets the weight of this edge. If this edge is unweighted an
		/// <code>UnsupportedOperationException</code> is thrown.
		/// 
		/// </summary>
		/// <param name="weight">new weight.
		/// 
		/// </param>
		/// <throws>  UnsupportedOperationException if this edge is unweighted. </throws>
		double Weight
		{
			get;
			
			set;
			
		}
		
		
		/// <summary> Creates and returns a shallow copy of this edge. The vertices of this
		/// edge are <i>not</i> cloned.
		/// 
		/// </summary>
		/// <returns> a shallow copy of this edge.
		/// 
		/// </returns>
		/// <seealso cref="Cloneable">
		/// </seealso>
		System.Object Clone();
		
		
		/// <summary> Returns <tt>true</tt> if this edge contains the specified vertex.  More
		/// formally, returns <tt>true</tt> if and only if the following condition
		/// holds:
		/// <pre>
		/// this.getSource().equals(v) || this.getTarget().equals(v)
		/// </pre>
		/// 
		/// </summary>
		/// <param name="v">vertex whose presence in this edge is to be tested.
		/// 
		/// </param>
		/// <returns> <tt>true</tt> if this edge contains the specified vertex.
		/// </returns>
		bool containsVertex(System.Object v);
		
		
		/// <summary> Returns the vertex opposite to the specified vertex.
		/// 
		/// </summary>
		/// <param name="v">the vertex whose opposite is required.
		/// 
		/// </param>
		/// <returns> the vertex opposite to the specified vertex.
		/// 
		/// </returns>
		/// <throws>  IllegalArgumentException if v is neither the source nor the </throws>
		/// <summary>         target vertices of this edge.
		/// </summary>
		/// <throws>  NullPointerException if v is <code>null</code>. </throws>
		System.Object oppositeVertex(System.Object v);
	}
}
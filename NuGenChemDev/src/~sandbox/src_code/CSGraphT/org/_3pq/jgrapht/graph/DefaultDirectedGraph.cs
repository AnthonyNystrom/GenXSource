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
/* -------------------------
* DefaultDirectedGraph.java
* -------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: DefaultDirectedGraph.java,v 1.3 2004/11/19 10:11:33 barak_naveh Exp $
*
* Changes
* -------
* 05-Aug-2003 : Initial revision (BN);
*
*/

using System;
using org._3pq.jgrapht.edge;

namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A directed graph. A directed graph is a non-simple directed graph in which
	/// multiple edges between any two vertices are <i>not</i> permitted, but loops
	/// are.
	/// 
	/// <p>
	/// prefixed 'Default' to avoid name collision with the DirectedGraph interface.
	/// </p>
	/// </summary>
	[Serializable]
	public class DefaultDirectedGraph:AbstractBaseGraph, DirectedGraph
	{
		private const long serialVersionUID = 3544953246956466230L;
		
		/// <summary> Creates a new directed graph.</summary>
		public DefaultDirectedGraph():this(new EdgeFactories.DirectedEdgeFactory())
		{
		}
		
		
		/// <summary> Creates a new directed graph with the specified edge factory.
		/// 
		/// </summary>
		/// <param name="ef">the edge factory of the new graph.
		/// </param>
		public DefaultDirectedGraph(EdgeFactory ef):base(ef, false, true)
		{
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		override public System.Object Clone()
		{
			return null;
		}
	}
}
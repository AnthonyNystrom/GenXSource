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
/* ------------------------------
* UnmodifiableDirectedGraph.java
* ------------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: UnmodifiableDirectedGraph.java,v 1.2 2004/11/19 10:21:13 barak_naveh Exp $
*
* Changes
* -------
* 05-Aug-2003 : Initial revision (BN);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A directed graph that cannot be modified.
	/// 
	/// </summary>
	/// <seealso cref="org._3pq.jgrapht.graph.UnmodifiableGraph">
	/// </seealso>
	[Serializable]
	public class UnmodifiableDirectedGraph:UnmodifiableGraph, DirectedGraph
	{
		private const long serialVersionUID = 3978701783725913906L;
		
		/// <summary> Creates a new unmodifiable directed graph based on the specified backing
		/// graph.
		/// 
		/// </summary>
		/// <param name="g">the backing graph on which an unmodifiable graph is to be
		/// created.
		/// </param>
		public UnmodifiableDirectedGraph(DirectedGraph g):base(g)
		{
		}
	}
}
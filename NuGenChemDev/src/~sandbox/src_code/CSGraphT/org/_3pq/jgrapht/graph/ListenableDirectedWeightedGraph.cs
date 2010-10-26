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
/* ------------------------------------
* ListenableDirectedWeightedGraph.java
* ------------------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: ListenableDirectedWeightedGraph.java,v 1.2 2004/11/19 10:36:18 barak_naveh Exp $
*
* Changes
* -------
* 05-Aug-2003 : Initial revision (BN);
*
*/
using System;
using DirectedGraph = org._3pq.jgrapht.DirectedGraph;
using WeightedGraph = org._3pq.jgrapht.WeightedGraph;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A directed weighted graph which is also {@link
	/// org._3pq.jgrapht.ListenableGraph}.
	/// 
	/// </summary>
	/// <seealso cref="org._3pq.jgrapht.graph.DefaultListenableGraph">
	/// </seealso>
	[Serializable]
	public class ListenableDirectedWeightedGraph:ListenableDirectedGraph, WeightedGraph
	{
		private const long serialVersionUID = 3977582476627621938L;
		
		/// <summary> Creates a new listenable directed weighted graph.</summary>
		public ListenableDirectedWeightedGraph():this(new DefaultDirectedWeightedGraph())
		{
		}
		
		
		/// <summary> Creates a new listenable directed weighted graph.
		/// 
		/// </summary>
		/// <param name="base">the backing graph.
		/// </param>
		public ListenableDirectedWeightedGraph(WeightedGraph base_Renamed):base((DirectedGraph) base_Renamed)
		{
		}
	}
}
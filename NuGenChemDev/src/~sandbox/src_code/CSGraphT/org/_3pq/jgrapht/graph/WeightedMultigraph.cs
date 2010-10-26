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
/* -----------------------
* WeightedMultigraph.java
* -----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: WeightedMultigraph.java,v 1.2 2004/11/19 10:24:49 barak_naveh Exp $
*
* Changes
* -------
* 05-Aug-2003 : Initial revision (BN);
*
*/
using System;
using EdgeFactory = org._3pq.jgrapht.EdgeFactory;
using WeightedGraph = org._3pq.jgrapht.WeightedGraph;
using EdgeFactories = org._3pq.jgrapht.edge.EdgeFactories;
namespace org._3pq.jgrapht.graph
{
	
	/// <summary> A weighted multigraph. A weighted multigraph is a non-simple undirected
	/// graph in which no loops are permitted, but multiple edges between any two
	/// vertices are. The edges of a weighted multigraph have weights. If you're
	/// unsure about multigraphs, see: <a
	/// href="http://mathworld.wolfram.com/Multigraph.html">
	/// http://mathworld.wolfram.com/Multigraph.html</a>.
	/// </summary>
	[Serializable]
	public class WeightedMultigraph:Multigraph, WeightedGraph
	{
		private const long serialVersionUID = 3544671793370640696L;
		
		/// <summary> Creates a new weighted multigraph with the specified edge factory.
		/// 
		/// </summary>
		/// <param name="ef">the edge factory of the new graph.
		/// </param>
		public WeightedMultigraph(EdgeFactory ef):base(ef)
		{
		}
		
		
		/// <summary> Creates a new weighted multigraph.</summary>
		public WeightedMultigraph():this(new EdgeFactories.UndirectedWeightedEdgeFactory())
		{
		}
	}
}
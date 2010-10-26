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
/* --------------------------------
* SimpleDirectedWeightedGraph.java
* --------------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: SimpleDirectedWeightedGraph.java,v 1.3 2004/11/19 10:09:44 barak_naveh Exp $
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
	
	/// <summary> A simple directed weighted graph. A simple directed weighted graph is a
	/// simple directed graph for which edges are assigned weights.
	/// </summary>
	[Serializable]
	public class SimpleDirectedWeightedGraph:SimpleDirectedGraph, WeightedGraph
	{
		private const long serialVersionUID = 3904960841681220919L;
		
		/// <summary> Creates a new simple directed weighted graph with the specified edge
		/// factory.
		/// 
		/// </summary>
		/// <param name="ef">the edge factory of the new graph.
		/// </param>
		public SimpleDirectedWeightedGraph(EdgeFactory ef):base(ef)
		{
		}
		
		
		/// <summary> Creates a new simple directed weighted graph.</summary>
		public SimpleDirectedWeightedGraph():this(new EdgeFactories.DirectedWeightedEdgeFactory())
		{
		}
	}
}
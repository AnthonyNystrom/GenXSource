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
/* --------------------
* UndirectedGraph.java
* --------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: UndirectedGraph.java,v 1.3 2004/05/01 23:15:46 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
*
*/
using System;
namespace org._3pq.jgrapht
{
	
	/// <summary> A graph whose all edges are undirected. This is the root interface of all
	/// undirected graphs.
	/// 
	/// <p>
	/// See <a href="http://mathworld.wolfram.com/Graph.html">
	/// http://mathworld.wolfram.com/Graph.html</a> for more on undirected and on
	/// directed graphs.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	public interface UndirectedGraph:Graph
	{
		/// <summary> Returns the degree of the specified vertex. A degree of a vertex in an
		/// undirected graph is the number of edges touching that vertex.
		/// 
		/// </summary>
		/// <param name="vertex">vertex whose degree is to be calculated.
		/// 
		/// </param>
		/// <returns> the degree of the specified vertex.
		/// </returns>
		int degreeOf(System.Object vertex);
	}
}
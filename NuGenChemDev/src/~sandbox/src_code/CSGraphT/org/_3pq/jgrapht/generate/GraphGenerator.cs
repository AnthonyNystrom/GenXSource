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
/* -------------------
* GraphGenerator.java
* -------------------
* (C) Copyright 2003, by John V. Sichi and Contributors.
*
* Original Author:  John V. Sichi
* Contributor(s):   -
*
* $Id: GraphGenerator.java,v 1.2 2004/11/18 22:02:57 barak_naveh Exp $
*
* Changes
* -------
* 16-Sep-2003 : Initial revision (JVS);
*
*/
using System;
using Graph = org._3pq.jgrapht.Graph;
using VertexFactory = org._3pq.jgrapht.VertexFactory;
namespace org._3pq.jgrapht.generate
{
	
	/// <summary> GraphGenerator defines an interface for generating new graph structures.
	/// 
	/// </summary>
	/// <author>  John V. Sichi
	/// 
	/// </author>
	/// <since> Sep 16, 2003
	/// </since>
	public interface GraphGenerator
	{
		/// <summary> Generate a graph structure. The topology of the generated graph is
		/// dependent on the implementation.  For graphs in which not all vertices
		/// share the same automorphism equivalence class, the generator may
		/// produce a labeling indicating the roles played by generated elements.
		/// This is the purpose of the resultMap parameter.  For example, a
		/// generator for a wheel graph would designate a hub vertex.  Role names
		/// used as keys in resultMap should be declared as public static final
		/// Strings by implementation classes.
		/// 
		/// </summary>
		/// <param name="target">receives the generated edges and vertices; if this is
		/// non-empty on entry, the result will be a disconnected graph
		/// since generated elements will not be connected to existing
		/// elements
		/// </param>
		/// <param name="vertexFactory">called to produce new vertices
		/// </param>
		/// <param name="resultMap">if non-null, receives implementation-specific mappings
		/// from String roles to graph elements (or collections of graph
		/// elements)
		/// </param>
		void  generateGraph(Graph target, VertexFactory vertexFactory, System.Collections.IDictionary resultMap);
	}
}
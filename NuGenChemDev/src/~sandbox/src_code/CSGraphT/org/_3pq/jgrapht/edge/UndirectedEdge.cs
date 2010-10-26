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
* UndirectedEdge.java
* -------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: UndirectedEdge.java,v 1.5 2004/11/18 21:44:24 barak_naveh Exp $
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
	
	/// <summary> A implementation for an undirected edge.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	[Serializable]
	public class UndirectedEdge:DefaultEdge
	{
		private const long serialVersionUID = 3257563988526380337L;
		
		/// <seealso cref="DefaultEdge.DefaultEdge(Object, Object)">
		/// </seealso>
		public UndirectedEdge(System.Object sourceVertex, System.Object targetVertex):base(sourceVertex, targetVertex)
		{
		}
		
		/// <summary> Returns a string representation of this undirected edge. The
		/// representation is a curly-braced pair {v1,v2} where v1,v2 are the two
		/// endpoint vertices of this edge.
		/// 
		/// </summary>
		/// <returns> a string representation of this directed edge.
		/// </returns>
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
			return "{" + Source + "," + Target + "}";
		}
	}
}
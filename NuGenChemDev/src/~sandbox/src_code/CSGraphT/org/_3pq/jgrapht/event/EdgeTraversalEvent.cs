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
* EdgeTraversalEvent.java
* -----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: EdgeTraversalEvent.java,v 1.2 2004/11/18 21:55:00 barak_naveh Exp $
*
* Changes
* -------
* 11-Aug-2003 : Initial revision (BN);
*
*/
using System;

namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> A traversal event for a graph edge.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 11, 2003
	/// </since>
	[Serializable]
	public class EdgeTraversalEvent:System.EventArgs
	{
		private const long serialVersionUID = 4050768173789820979L;
		
		/// <summary>The traversed edge. </summary>
		protected internal Edge m_edge;
		
		/// <summary> Creates a new EdgeTraversalEvent.
		/// 
		/// </summary>
		/// <param name="eventSource">the source of the event.
		/// </param>
		/// <param name="edge">the traversed edge.
		/// </param>
		public EdgeTraversalEvent(System.Object eventSource, Edge edge):base()
		{
			m_edge = edge;
		}
		
		/// <summary> Returns the traversed edge.
		/// 
		/// </summary>
		/// <returns> the traversed edge.
		/// </returns>
		public virtual Edge getEdge()
		{
			return m_edge;
		}
	}
}
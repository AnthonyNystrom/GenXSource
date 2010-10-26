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
* GraphEdgeChangeEvent.java
* -------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphEdgeChangeEvent.java,v 1.2 2004/11/18 21:55:00 barak_naveh Exp $
*
* Changes
* -------
* 10-Aug-2003 : Initial revision (BN);
*
*/
using System;
using Edge = org._3pq.jgrapht.Edge;
namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> An event which indicates that a graph edge has changed, or is about to
	/// change. The event can be used either as an indication <i>after</i> the edge
	/// has been added or removed, or <i>before</i> it is added. The type of the
	/// event can be tested using the {@link
	/// org._3pq.jgrapht.event.GraphChangeEvent#getType()} method.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 10, 2003
	/// </since>
	[Serializable]
	public class GraphEdgeChangeEvent:GraphChangeEvent
	{
		private const long serialVersionUID = 3618134563335844662L;
		
		/// <summary> Before edge added event. This event is fired before an edge is added to
		/// a graph.
		/// </summary>
		public const int BEFORE_EDGE_ADDED = 21;
		
		/// <summary> Before edge removed event. This event is fired before an edge is removed
		/// from a graph.
		/// </summary>
		public const int BEFORE_EDGE_REMOVED = 22;
		
		/// <summary> Edge added event. This event is fired after an edge is added to a graph.</summary>
		public const int EDGE_ADDED = 23;
		
		/// <summary> Edge removed event. This event is fired after an edge is removed from a
		/// graph.
		/// </summary>
		public const int EDGE_REMOVED = 24;
		
		/// <summary>The edge that this event is related to. </summary>
		protected internal Edge m_edge;
		
		/// <summary> Constructor for GraphEdgeChangeEvent.
		/// 
		/// </summary>
		/// <param name="eventSource">the source of this event.
		/// </param>
		/// <param name="type">the event type of this event.
		/// </param>
		/// <param name="e">the edge that this event is related to.
		/// </param>
		public GraphEdgeChangeEvent(System.Object eventSource, int type, Edge e):base(eventSource, type)
		{
			m_edge = e;
		}
		
		/// <summary> Returns the edge that this event is related to.
		/// 
		/// </summary>
		/// <returns> the edge that this event is related to.
		/// </returns>
		public virtual Edge getEdge()
		{
			return m_edge;
		}
	}
}
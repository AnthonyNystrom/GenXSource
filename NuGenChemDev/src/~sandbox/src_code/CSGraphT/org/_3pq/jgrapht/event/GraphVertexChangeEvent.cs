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
/* ---------------------------
* GraphVertexChangeEvent.java
* ---------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphVertexChangeEvent.java,v 1.2 2004/11/18 21:56:21 barak_naveh Exp $
*
* Changes
* -------
* 10-Aug-2003 : Initial revision (BN);
*
*/
using System;
namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> An event which indicates that a graph vertex has changed, or is about to
	/// change. The event can be used either as an indication <i>after</i> the
	/// vertex has  been added or removed, or <i>before</i> it is added. The type
	/// of the event can be tested using the {@link
	/// org._3pq.jgrapht.event.GraphChangeEvent#getType()} method.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 10, 2003
	/// </since>
	[Serializable]
	public class GraphVertexChangeEvent:GraphChangeEvent
	{
		private const long serialVersionUID = 3690189962679104053L;
		
		/// <summary> Before vertex added event. This event is fired before a vertex is added
		/// to a graph.
		/// </summary>
		public const int BEFORE_VERTEX_ADDED = 11;
		
		/// <summary> Before vertex removed event. This event is fired before a vertex is
		/// removed from a graph.
		/// </summary>
		public const int BEFORE_VERTEX_REMOVED = 12;
		
		/// <summary> Vertex added event. This event is fired after a vertex is added to a
		/// graph.
		/// </summary>
		public const int VERTEX_ADDED = 13;
		
		/// <summary> Vertex removed event. This event is fired after a vertex is removed from
		/// a graph.
		/// </summary>
		public const int VERTEX_REMOVED = 14;
		
		/// <summary>The vertex that this event is related to. </summary>
		protected internal System.Object m_vertex;
		
		/// <summary> Creates a new GraphVertexChangeEvent object.
		/// 
		/// </summary>
		/// <param name="eventSource">the source of the event.
		/// </param>
		/// <param name="type">the type of the event.
		/// </param>
		/// <param name="vertex">the vertex that the event is related to.
		/// </param>
		public GraphVertexChangeEvent(System.Object eventSource, int type, System.Object vertex):base(eventSource, type)
		{
			m_vertex = vertex;
		}
		
		/// <summary> Returns the vertex that this event is related to.
		/// 
		/// </summary>
		/// <returns> the vertex that this event is related to.
		/// </returns>
		public virtual System.Object getVertex()
		{
			return m_vertex;
		}
	}
}
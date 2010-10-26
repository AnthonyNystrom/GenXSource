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
* VertexTraversalEvent.java
* -------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: VertexTraversalEvent.java,v 1.2 2004/11/18 21:34:45 barak_naveh Exp $
*
* Changes
* -------
* 11-Aug-2003 : Initial revision (BN);
*
*/
using System;
namespace org._3pq.jgrapht.event_Renamed
{
	/// <summary> A traversal event for a graph vertex.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 11, 2003
	/// </since>
	[Serializable]
	public class VertexTraversalEvent:System.EventArgs
	{
		private const long serialVersionUID = 3688790267213918768L;
		
		/// <summary>The traversed vertex. </summary>
		protected internal System.Object m_vertex;
		
		/// <summary> Creates a new VertexTraversalEvent.
		/// 
		/// </summary>
		/// <param name="eventSource">the source of the event.
		/// </param>
		/// <param name="vertex">the traversed vertex.
		/// </param>
		public VertexTraversalEvent(System.Object eventSource, System.Object vertex):base()
		{
			m_vertex = vertex;
		}
		
		/// <summary> Returns the traversed vertex.
		/// 
		/// </summary>
		/// <returns> the traversed vertex.
		/// </returns>
		public virtual System.Object getVertex()
		{
			return m_vertex;
		}
	}
}
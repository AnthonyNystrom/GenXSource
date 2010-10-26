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
/* ---------------------
* GraphChangeEvent.java
* ---------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphChangeEvent.java,v 1.3 2004/11/18 21:55:00 barak_naveh Exp $
*
* Changes
* -------
* 10-Aug-2003 : Initial revision (BN);
*
*/
using System;
namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> An event which indicates that a graph has changed. This class is a root for
	/// graph change events.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 10, 2003
	/// </since>
	[Serializable]
	public class GraphChangeEvent:System.EventArgs
	{
		private const long serialVersionUID = 3834592106026382391L;
		
		/// <summary>The type of graph change this event indicates. </summary>
		protected internal int m_type;
		
		/// <summary> Creates a new graph change event.
		/// 
		/// </summary>
		/// <param name="eventSource">the source of the event.
		/// </param>
		/// <param name="type">the type of event.
		/// </param>
		public GraphChangeEvent(System.Object eventSource, int type):base()
		{
			m_type = type;
		}
		
		/// <summary> Returns the event type.
		/// 
		/// </summary>
		/// <returns> the event type.
		/// </returns>
		public virtual int getType()
		{
			return m_type;
		}
	}
}
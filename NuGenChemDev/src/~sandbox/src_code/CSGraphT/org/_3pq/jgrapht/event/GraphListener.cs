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
/* ------------------
* GraphListener.java
* ------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphListener.java,v 1.4 2004/11/18 21:54:30 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 10-Aug-2003 : Adaptation to new event model (BN);
*
*/
using System;
namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> A listener that is notified when the graph changes.
	/// 
	/// <p>
	/// If only notifications on vertex set changes are required it is more
	/// efficient to use the VertexSetListener.
	/// </p>
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <seealso cref="org._3pq.jgrapht.event.VertexSetListener">
	/// </seealso>
	/// <since> Jul 18, 2003
	/// </since>
	public delegate void  GraphListenerDelegate(Object sender, org._3pq.jgrapht.event_Renamed.GraphEdgeChangeEvent GraphListenerDelegateParam);
	public delegate void  GraphListenerDelegate2(Object sender, org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent GraphListenerDelegate2Param);
	public interface GraphListener:VertexSetListener
	{
		/// <summary> Notifies that an edge has been added to the graph.
		/// 
		/// </summary>
		/// <param name="e">the edge event.
		/// </param>
		void  edgeAdded(System.Object event_sender, GraphEdgeChangeEvent e);
		
		
		/// <summary> Notifies that an edge has been removed from the graph.
		/// 
		/// </summary>
		/// <param name="e">the edge event.
		/// </param>
		void  edgeRemoved(System.Object event_sender, GraphEdgeChangeEvent e);
	}
}
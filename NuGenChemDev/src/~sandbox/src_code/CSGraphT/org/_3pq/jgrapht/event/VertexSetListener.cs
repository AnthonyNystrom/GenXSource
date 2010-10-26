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
/* ----------------------
* VertexSetListener.java
* ----------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: VertexSetListener.java,v 1.5 2005/04/23 08:09:29 perfecthash Exp $
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
	
	/// <summary> A listener that is notified when the graph's vertex set changes. It should
	/// be used when <i>only</i> notifications on vertex-set changes are of
	/// interest. If all graph  notifications are of interest better use
	/// <code>GraphListener</code>.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <seealso cref="org._3pq.jgrapht.event.GraphListener">
	/// </seealso>
	/// <since> Jul 18, 2003
	/// </since>
	public delegate void  VertexSetListenerDelegate(Object sender, org._3pq.jgrapht.event_Renamed.GraphVertexChangeEvent VertexSetListenerDelegateParam);
	//UPGRADE_ISSUE: Interface 'java.util.EventListener' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilEventListener'"
	public interface VertexSetListener//:EventListener
	{
		/// <summary> Notifies that a vertex has been added to the graph.
		/// 
		/// </summary>
		/// <param name="e">the vertex event.
		/// </param>
		void  vertexAdded(System.Object event_sender, GraphVertexChangeEvent e);
		
		
		/// <summary> Notifies that a vertex has been removed from the graph.
		/// 
		/// </summary>
		/// <param name="e">the vertex event.
		/// </param>
		void  vertexRemoved(System.Object event_sender, GraphVertexChangeEvent e);
	}
}
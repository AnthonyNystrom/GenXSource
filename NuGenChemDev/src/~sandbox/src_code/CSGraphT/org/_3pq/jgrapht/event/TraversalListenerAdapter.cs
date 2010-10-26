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
/* -----------------------------
* TraversalListenerAdapter.java
* -----------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: TraversalListenerAdapter.java,v 1.2 2004/11/18 21:56:10 barak_naveh Exp $
*
* Changes
* -------
* 06-Aug-2003 : Initial revision (BN);
* 11-Aug-2003 : Adaptation to new event model (BN);
*
*/
using System;
namespace org._3pq.jgrapht.event_Renamed
{
	
	/// <summary> An empty do-nothing implementation of the {@link TraversalListener}
	/// interface used for subclasses.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Aug 6, 2003
	/// </since>
	public class TraversalListenerAdapter : TraversalListener
	{
		/// <seealso cref="TraversalListener.connectedComponentFinished(ConnectedComponentTraversalEvent)">
		/// </seealso>
		public virtual void  connectedComponentFinished(ConnectedComponentTraversalEvent e)
		{
		}
		
		
		/// <seealso cref="TraversalListener.connectedComponentStarted(ConnectedComponentTraversalEvent)">
		/// </seealso>
		public virtual void  connectedComponentStarted(ConnectedComponentTraversalEvent e)
		{
		}
		
		
		/// <seealso cref="TraversalListener.edgeTraversed(EdgeTraversalEvent)">
		/// </seealso>
		public virtual void  edgeTraversed(EdgeTraversalEvent e)
		{
		}
		
		
		/// <seealso cref="TraversalListener.vertexTraversed(VertexTraversalEvent)">
		/// </seealso>
		public virtual void  vertexTraversed(VertexTraversalEvent e)
		{
		}
	}
}
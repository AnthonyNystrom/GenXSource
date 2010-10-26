/* ==========================================
* JGraphT : a free Java graph-theory library
* ==========================================
*
* Project Info:  http://jgrapht.sourceforge.net/
* Project Lead:  Barak Naveh (barak_naveh@users.sourceforge.net)
*
* (C) Copyright 2003, by Barak Naveh and Contributors.
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
/* --------------------------
* AbstractGraphIterator.java
* --------------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: AbstractGraphIterator.java,v 1.2 2003/08/11 10:37:44 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
* 11-Aug-2003 : Adaptation to new event model (BN);
*
*/
using System;
using org._3pq.jgrapht.event_Renamed;

namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> An empty implementation of a graph iterator to minimize the effort required
	/// to implement graph iterators.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 19, 2003
	/// </since>
	public abstract class AbstractGraphIterator : GraphIterator
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <seealso cref="GraphIterator.isReuseEvents()">
		/// </seealso>
		/// <seealso cref="GraphIterator.setReuseEvents(boolean)">
		/// </seealso>
		virtual public bool ReuseEvents
		{
			get
			{
				return m_reuseEvents;
			}
			
			set
			{
				m_reuseEvents = value;
			}
			
		}
		private System.Collections.IList m_traversalListeners = new System.Collections.ArrayList();
		private bool m_crossComponentTraversal = true;
		private bool m_reuseEvents = false;
		
		/// <summary> Sets the cross component traversal flag - indicates whether to traverse
		/// the graph across connected components.
		/// 
		/// </summary>
		/// <param name="crossComponentTraversal">if <code>true</code> traverses across
		/// connected components.
		/// </param>
		public virtual void  setCrossComponentTraversal(bool crossComponentTraversal)
		{
			m_crossComponentTraversal = crossComponentTraversal;
		}
		
		
		/// <summary> Test whether this iterator is set to traverse the graph across connected
		/// components.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if traverses across connected components,
		/// otherwise <code>false</code>.
		/// </returns>
		public virtual bool isCrossComponentTraversal()
		{
			return m_crossComponentTraversal;
		}
		
		
		/// <summary> Adds the specified traversal listener to this iterator.
		/// 
		/// </summary>
		/// <param name="l">the traversal listener to be added.
		/// </param>
		public virtual void  addTraversalListener(TraversalListener l)
		{
			if (!m_traversalListeners.Contains(l))
			{
				m_traversalListeners.Add(l);
			}
		}
		
		
		/// <summary> Unsupported.
		/// 
		/// </summary>
		/// <throws>  UnsupportedOperationException </throws>
		//UPGRADE_NOTE: The equivalent of method 'java.util.Iterator.remove' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		public virtual void  remove()
		{
			throw new System.NotSupportedException();
		}
		
		
		/// <summary> Removes the specified traversal listener from this iterator.
		/// 
		/// </summary>
		/// <param name="l">the traversal listener to be removed.
		/// </param>
		public virtual void  removeTraversalListener(TraversalListener l)
		{
			m_traversalListeners.Remove(l);
		}
		
		
		/// <summary> Informs all listeners that the traversal of the current connected
		/// component finished.
		/// 
		/// </summary>
		/// <param name="e">the connected component finished event.
		/// </param>
		//UPGRADE_NOTE: This method is no longer necessary and it can be commented or removed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1271'"
		protected internal virtual void  fireConnectedComponentFinished(ConnectedComponentTraversalEvent e)
		{
			int len = m_traversalListeners.Count;
			
			for (int i = 0; i < len; i++)
			{
				TraversalListener l = (TraversalListener) m_traversalListeners[i];
				l.connectedComponentFinished(e);
			}
		}
		
		
		/// <summary> Informs all listeners that a traversal of a new connected component has
		/// started.
		/// 
		/// </summary>
		/// <param name="e">the connected component started event.
		/// </param>
		//UPGRADE_NOTE: This method is no longer necessary and it can be commented or removed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1271'"
		protected internal virtual void  fireConnectedComponentStarted(ConnectedComponentTraversalEvent e)
		{
			int len = m_traversalListeners.Count;
			
			for (int i = 0; i < len; i++)
			{
				TraversalListener l = (TraversalListener) m_traversalListeners[i];
				l.connectedComponentStarted(e);
			}
		}
		
		
		/// <summary> Informs all listeners that a the specified edge was visited.
		/// 
		/// </summary>
		/// <param name="e">the edge traversal event.
		/// </param>
		//UPGRADE_NOTE: This method is no longer necessary and it can be commented or removed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1271'"
		protected internal virtual void  fireEdgeTraversed(EdgeTraversalEvent e)
		{
			int len = m_traversalListeners.Count;
			
			for (int i = 0; i < len; i++)
			{
				TraversalListener l = (TraversalListener) m_traversalListeners[i];
				l.edgeTraversed(e);
			}
		}
		
		
		/// <summary> Informs all listeners that a the specified vertex was visited.
		/// 
		/// </summary>
		/// <param name="e">the vertex traversal event.
		/// </param>
		//UPGRADE_NOTE: This method is no longer necessary and it can be commented or removed. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1271'"
		protected internal virtual void  fireVertexTraversed(VertexTraversalEvent e)
		{
			int len = m_traversalListeners.Count;
			
			for (int i = 0; i < len; i++)
			{
				TraversalListener l = (TraversalListener) m_traversalListeners[i];
				l.vertexTraversed(e);
			}
		}
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		abstract public System.Boolean MoveNext();
		//UPGRADE_TODO: The following method was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		abstract public void  Reset();
		//UPGRADE_TODO: The following property was automatically generated and it must be implemented in order to preserve the class logic. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1232'"
		abstract public System.Object Current{get;}
	}
}
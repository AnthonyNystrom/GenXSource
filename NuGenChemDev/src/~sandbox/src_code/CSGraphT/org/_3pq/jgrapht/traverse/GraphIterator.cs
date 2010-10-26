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
/* ------------------
* GraphIterator.java
* ------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: GraphIterator.java,v 1.2 2003/08/11 10:37:44 barak_naveh Exp $
*
* Changes
* -------
* 31-Jul-2003 : Initial revision (BN);
* 11-Aug-2003 : Adaptation to new event model (BN);
*
*/
using System;
using org._3pq.jgrapht.event_Renamed;

namespace org._3pq.jgrapht.traverse
{
	
	/// <summary> A graph iterator.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 31, 2003
	/// </since>
	public interface GraphIterator : System.Collections.IEnumerator
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Tests whether the <code>reuseEvents</code> flag is set. If the flag is
		/// set to <code>true</code> this class will reuse previously fired events
		/// and will not create a new object for each event. This option increases
		/// performance but should be used with care, especially in multithreaded
		/// environment.
		/// 
		/// </summary>
		/// <returns> the value of the <code>reuseEvents</code> flag.
		/// </returns>
		/// <summary> Sets a value the <code>reuseEvents</code> flag. If the
		/// <code>reuseEvents</code> flag is set to <code>true</code> this class
		/// will reuse previously fired events and will not create a new object for
		/// each event. This option increases performance but should be used with
		/// care, especially in multithreaded environment.
		/// 
		/// </summary>
		/// <param name="reuseEvents">whether to reuse previously fired event objects
		/// instead of creating a new event object for each event.
		/// </param>
		bool ReuseEvents
		{
			get;
			
			set;
			
		}
		/// <summary> Test whether this iterator is set to traverse the grpah across connected
		/// components.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if traverses across connected components,
		/// otherwise <code>false</code>.
		/// </returns>
		bool isCrossComponentTraversal();
		
		
		/// <summary> Adds the specified traversal listener to this iterator.
		/// 
		/// </summary>
		/// <param name="l">the traversal listener to be added.
		/// </param>
		void  addTraversalListener(TraversalListener l);
		
		
		/// <summary> Unsupported.
		/// 
		/// </summary>
		/// <throws>  UnsupportedOperationException </throws>
		//UPGRADE_NOTE: The equivalent of method 'java.util.Iterator.remove' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
		void  remove();
		
		
		/// <summary> Removes the specified traversal listener from this iterator.
		/// 
		/// </summary>
		/// <param name="l">the traversal listener to be removed.
		/// </param>
		void  removeTraversalListener(TraversalListener l);
	}
}
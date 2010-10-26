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
/* -------------------
* LabeledElement.java
* -------------------
* (C) Copyright 2003, by Barak Naveh and Contributors.
*
* Original Author:  Barak Naveh
* Contributor(s):   -
*
* $Id: LabeledElement.java,v 1.2 2004/05/01 23:15:46 barak_naveh Exp $
*
* Changes
* -------
* 24-Jul-2003 : Initial revision (BN);
*
*/
using System;
namespace org._3pq.jgrapht
{
	
	/// <summary> An graph element (vertex or edge) that can have a label.
	/// 
	/// </summary>
	/// <author>  Barak Naveh
	/// 
	/// </author>
	/// <since> Jul 14, 2003
	/// </since>
	public interface LabeledElement
	{
		//UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
		/// <summary> Returns the element's label, or <code>null</code> if element has no
		/// label.
		/// 
		/// </summary>
		/// <returns> the element's label, or <code>null</code> if element has no
		/// label.
		/// </returns>
		/// <summary> Sets the specified label object to this element.
		/// 
		/// </summary>
		/// <param name="label">a label to set to this element.
		/// </param>
		System.Object Label
		{
			get;
			
			set;
			
		}
		
		
		/// <summary> Tests if this element has a label.
		/// 
		/// </summary>
		/// <returns> <code>true</code> if the element has a label, otherwise
		/// <code>false</code>.
		/// </returns>
		bool hasLabel();
	}
}
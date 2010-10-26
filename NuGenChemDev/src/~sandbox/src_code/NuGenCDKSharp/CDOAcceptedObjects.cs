/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
* Contact: cdk-devel@lists.sourceforge.net
*
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* All we ask is that proper credit is given for our work, which includes
* - but is not limited to - adding the above copyright notice to the beginning
* of your source code files, and to any copyright notice that you may distribute
* with programs based on this work.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;

namespace Org.OpenScience.CDK.IO.CML.CDOPI
{
    /// <summary> List of names (String classes) of objects accepted by CDO.
    /// 
    /// </summary>
    /// <cdk.module>  io </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>  Egon Willighagen <egonw@sci.kun.nl>
    /// </author>
    public class CDOAcceptedObjects
    {

        private System.Collections.ArrayList objects;

        /// <summary> Constructor.</summary>
        public CDOAcceptedObjects()
        {
            objects = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        }

        /// <summary> Adds the name of an accepted object.
        /// 
        /// </summary>
        /// <param name="object">Name of the object
        /// </param>
        public virtual void add(System.String object_Renamed)
        {
            objects.Add(object_Renamed);
        }

        /// <summary> Determine if an object name is contained in this list.
        /// 
        /// </summary>
        /// <param name="object">Name of the object to search in the list
        /// </param>
        /// <returns>         true if the object is in the list, false otherwise
        /// </returns>
        public virtual bool contains(System.String object_Renamed)
        {
            return objects.Contains(object_Renamed);
        }

        /// <summary> Returns the names in this list as a Enumeration class. Each element in the
        /// Enumeration is of type String.
        /// 
        /// </summary>
        /// <returns> The names of the accepted objects
        /// </returns>
        public virtual System.Collections.IEnumerator elements()
        {
            return objects.GetEnumerator();
        }
    }
}
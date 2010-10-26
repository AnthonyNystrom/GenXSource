/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-05-11 22:05:31 +0200 (Thu, 11 May 2006) $    
* $Revision: 6236 $
* 
* Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
* 
* Contact: cdk-devel@lists.sourceforge.net
* 
* This program is free software; you can redistribute it and/or
* modify it under the terms of the GNU Lesser General Public License
* as published by the Free Software Foundation; either version 2.1
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{

    /// <summary> Maintains a set of Ring objects.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  ring, set of </cdk.keyword>
    public interface IRingSet : ISetOfAtomContainers
    {
        /// <summary> Checks - and returns 'true' - if a certain ring is already
        /// stored in this setOfRings.
        /// 
        /// </summary>
        /// <param name="newRing"> The ring to be tested if it is already stored here
        /// </param>
        /// <returns>     true if it is already stored
        /// </returns>
        bool ringAlreadyInSet(IRing newRing);

        /// <summary> Returns a vector of all rings that this bond is part of.
        /// 
        /// </summary>
        /// <param name="bond"> The bond to be checked
        /// </param>
        /// <returns>   A vector of all rings that this bond is part of  
        /// </returns>
        IList getRings(IBond bond);

        /// <summary> Returns a vector of all rings that this atom is part of.
        /// 
        /// </summary>
        /// <param name="atom"> The atom to be checked
        /// </param>
        /// <returns>   A vector of all rings that this bond is part of  
        /// </returns>
        IRingSet getRings(IAtom atom);

        /// <summary> Returns all the rings in the RingSet that share
        /// one or more atoms with a given ring.
        /// 
        /// </summary>
        /// <param name="ring"> A ring with which all return rings must share one or more atoms
        /// </param>
        /// <returns>  All the rings that share one or more atoms with a given ring.   
        /// </returns>
        IList getConnectedRings(IRing ring);

        /// <summary> Adds all rings of another RingSet if they are not allready part of this ring set.
        /// 
        /// </summary>
        /// <param name="ringSet"> the ring set to be united with this one.
        /// </param>
        new void add(IRingSet ringSet);

        /// <summary> True, if at least one of the rings in the ringset contains
        /// the given atom.
        /// 
        /// </summary>
        /// <param name="atom">IAtom to check
        /// </param>
        /// <returns>      true, if the ringset contains the atom
        /// </returns>
        bool contains(IAtom atom);

        /// <summary> True, if this set contains the IAtomContainer.
        /// 
        /// </summary>
        /// <param name="container">IAtomContainer to check
        /// </param>
        /// <returns>           true, if the ringset contains the container
        /// </returns>
        bool contains(IAtomContainer container);
    }
}
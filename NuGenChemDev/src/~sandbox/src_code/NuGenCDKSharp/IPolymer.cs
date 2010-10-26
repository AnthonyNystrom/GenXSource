/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $
* $Revision: 6119 $
* 
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
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
*  */
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Subclass of Molecule to store Polymer specific attributes that a Polymer has.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    /// <cdk.created>  2001-08-06 </cdk.created>
    /// <cdk.keyword>  polymer </cdk.keyword>
    public interface IPolymer : IMolecule
    {
        /// <summary> Return the number of monomers present in the Polymer.
        /// 
        /// </summary>
        /// <returns> number of monomers
        /// </returns>
        int MonomerCount
        {
            get;
        }

        /// <summary> Returns a collection of the names of all <code>Monomer</code>s in this
        /// polymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the monomer names.
        /// </returns>
        ICollection MonomerNames
        {
            get;
        }

        /// <summary> Adds the atom oAtom without specifying a Monomer. Therefore the
        /// atom to this AtomContainer, but not to a certain Monomer (intended
        /// e.g. for HETATMs).
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        new void addAtom(IAtom oAtom);

        /// <summary> Adds the atom oAtom to a specified Monomer.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        /// <param name="oMonomer"> The monomer the atom belongs to
        /// </param>
        void addAtom(IAtom oAtom, IMonomer oMonomer);

        /// <summary> Retrieve a Monomer object by specifying its name.
        /// 
        /// </summary>
        /// <param name="cName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// </returns>
        IMonomer getMonomer(String cName);

        /// <summary> Removes a particular monomer, specified by its name.
        /// 
        /// </summary>
        /// <param name="name">The name of the monomer to be removed
        /// </param>
        void removeMonomer(String name);
    }
}
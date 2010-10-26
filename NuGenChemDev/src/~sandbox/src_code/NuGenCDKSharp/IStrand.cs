/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $
* $Revision: 6119 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
*/
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A Strand is an AtomContainer which stores additional strand specific
    /// informations for a group of Atoms.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <cdk.created>  2004-12-20 </cdk.created>
    /// <author>       Martin Eklund <martin.eklund@farmbio.uu.se>
    /// </author>
    public interface IStrand : IAtomContainer
    {
        /// <summary> Retrieve the strand name.
        /// 
        /// </summary>
        /// <returns> The name of the Strand object
        /// </returns>
        /// <seealso cref="setStrandName(String)">
        /// </seealso>
        /// <summary> Set the name of the Strand object.
        /// 
        /// </summary>
        /// <param name="cStrandName"> The new name for this strand
        /// </param>
        /// <seealso cref="getStrandName()">
        /// </seealso>
        String StrandName
        {
            get;
            set;
        }
        
        /// <summary> Retrieve the strand type.
        /// 
        /// </summary>
        /// <returns> The type of the Strand object
        /// </returns>
        /// <seealso cref="setStrandType(String)">
        /// </seealso>
        /// <summary> Set the type of the Strand object.
        /// 
        /// </summary>
        /// <param name="cStrandType"> The new type for this strand
        /// </param>
        /// <seealso cref="getStrandType()">
        /// </seealso>
        String StrandType
        {
            get;
            set;
        }

        /// <summary> Return the number of monomers present in the Strand.
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

        /// <summary> Returns the monomers in this strand. 
        /// 
        /// </summary>
        /// <returns> hashtable containing the monomers in the strand.
        /// </returns>
        Hashtable Monomers
        {
            get;
        }

        /// <summary> Adds the atom oAtom without specifying a Monomer or a Strand. Therefore the
        /// atom gets added to a Monomer of type UNKNOWN in a Strand of type UNKNOWN.
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        new void addAtom(IAtom oAtom);

        /// <summary> Adds the atom oAtom to a specific Monomer.
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
        /// <param name="name">The name of the monomer to remove
        /// </param>
        void removeMonomer(String name);
    }
}
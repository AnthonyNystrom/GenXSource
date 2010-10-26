/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-05-02 09:46:48 +0200 (Tue, 02 May 2006) $    
* $Revision: 6119 $
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
*  */
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> A BioPolymer is a subclass of a Polymer which is supposed to store
    /// additional informations about the Polymer which are connected to BioPolymers.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Edgar Luttmann <edgar@uni-paderborn.de>
    /// </author>
    /// <cdk.created>  2001-08-06  </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  polymer </cdk.keyword>
    /// <cdk.keyword>  biopolymer </cdk.keyword>
    public interface IBioPolymer : IPolymer
    {
        /// <summary> Return the number of monomers present in BioPolymer.
        /// 
        /// </summary>
        /// <returns> number of monomers
        /// </returns>
        new int MonomerCount
        {
            get;
        }

        /// <summary> Returns a collection of the names of all <code>Monomer</code>s in this
        /// BioPolymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the monomer names.
        /// </returns>
        new ICollection MonomerNames
        {
            get;
        }

        /// <summary> Return the number of strands present in the BioPolymer.
        /// 
        /// </summary>
        /// <returns> number of strands
        /// </returns>
        int StrandCount
        {
            get;
        }

        /// <summary> Returns a collection of the names of all <code>Strand</code>s in this
        /// BioPolymer.
        /// 
        /// </summary>
        /// <returns> a <code>Collection</code> of all the strand names.
        /// </returns>
        ICollection StrandNames
        {
            get;
        }

        /// <summary> Returns a Hashtable containing the strands in the Polymer.
        /// 
        /// </summary>
        /// <returns> hashtable containing the strands in the Polymer
        /// </returns>
        Hashtable Strands
        {
            get;
        }

        /// <summary> Adds the atom oAtom without specifying a Monomer or a Strand. Therefore the
        /// atom to this AtomContainer, but not to a certain Strand or Monomer (intended
        /// e.g. for HETATMs).
        /// 
        /// </summary>
        /// <param name="oAtom"> The atom to add
        /// </param>
        new void addAtom(IAtom oAtom);

        /// <summary> Adds the atom oAtom to a specified Strand, whereas the Monomer is unspecified. Hence
        /// the atom will be added to a Monomer of type UNKNOWN in the specified Strand.
        /// 
        /// </summary>
        /// <param name="oAtom">  The atom to add
        /// </param>
        /// <param name="oStrand">The strand the atom belongs to
        /// </param>
        void addAtom(IAtom oAtom, IStrand oStrand);

        /// <summary> Adds the atom to a specified Strand and a specified Monomer.
        /// 
        /// </summary>
        /// <param name="oAtom">   The atom to add
        /// </param>
        /// <param name="oMonomer">The monomer the atom belongs to
        /// </param>
        /// <param name="oStrand"> The strand the atom belongs to
        /// </param>
        void addAtom(IAtom oAtom, IMonomer oMonomer, IStrand oStrand);

        /// <summary> Retrieve a <code>Monomer</code> object by specifying its name.
        /// 
        /// <p>You have to specify the strand to enable
        /// monomers with the same name in different strands. There is at least one such case: every
        /// strand contains a monomer called "".
        /// 
        /// </summary>
        /// <param name="monName">   The name of the monomer to look for
        /// </param>
        /// <param name="strandName">The name of the strand to look for
        /// </param>
        /// <returns>            The Monomer object which was asked for
        /// </returns>
        IMonomer getMonomer(String monName, String strandName);

        /// <summary> Retrieve a Monomer object by specifying its name.
        /// 
        /// </summary>
        /// <param name="cName"> The name of the monomer to look for
        /// </param>
        /// <returns> The Monomer object which was asked for
        /// </returns>
        IStrand getStrand(String cName);

        /// <summary> Removes a particular strand, specified by its name.
        /// 
        /// </summary>
        /// <param name="name"> The name of the strand to remove
        /// </param>
        void removeStrand(String name);
    }
}
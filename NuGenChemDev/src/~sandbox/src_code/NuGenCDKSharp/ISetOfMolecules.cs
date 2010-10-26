/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-03 10:11:01 +0200 (Wed, 03 May 2006) $
* $Revision: 6125 $
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents a set of Molecules.
    /// 
    /// </summary>
    /// <cdk.module>   interfaces </cdk.module>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-25 </cdk.created>
    public interface ISetOfMolecules : ISetOfAtomContainers
    {
        /// <summary> Returns the array of Molecules of this container.
        /// 
        /// </summary>
        /// <returns>    The array of Molecules of this container 
        /// </returns>
        /// <seealso cref="setMolecules(IMolecule[])">
        /// </seealso>
        /// <summary> Sets the molecules in the ISetOfMolecules, removing previously added
        /// IMolecule's.
        /// 
        /// </summary>
        /// <param name="molecules">New set of molecules
        /// </param>
        /// <seealso cref="getMolecules()">
        /// </seealso>
        IMolecule[] Molecules
        {
            get;
            set;
        }

        /// <summary> Returns the number of Molecules in this Container.
        /// 
        /// </summary>
        /// <returns>     The number of Molecules in this Container
        /// </returns>
        int MoleculeCount
        {
            get;
        }

        /// <summary> Adds an IMolecule to this container.
        /// 
        /// </summary>
        /// <param name="molecule"> The molecule to be added to this container 
        /// </param>
        void addMolecule(IMolecule molecule);

        /// <summary> Adds all molecules in the SetOfMolecules to this container.
        /// 
        /// </summary>
        /// <param name="moleculeSet"> The SetOfMolecules to add
        /// </param>
        new void add(ISetOfMolecules moleculeSet);

        /// <summary> Returns the Molecule at position <code>number</code> in the
        /// container.
        /// 
        /// </summary>
        /// <param name="number"> The position of the Molecule to be returned. 
        /// </param>
        /// <returns>         The Molecule at position <code>number</code> . 
        /// </returns>
        IMolecule getMolecule(int number);
    }
}
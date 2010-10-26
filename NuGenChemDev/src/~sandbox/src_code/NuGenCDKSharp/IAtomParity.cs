/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
*
* Copyright (C) 2000-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Interfaces
{
    /// <summary> Represents the concept of an atom parity identifying the stereochemistry
    /// around an atom, given four neighbouring atoms.
    /// 
    /// </summary>
    /// <cdk.module>  interfaces </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       egonw
    /// </author>
    /// <cdk.created>  2005-08-24 </cdk.created>
    /// <cdk.keyword>  atom parity </cdk.keyword>
    /// <cdk.keyword>  stereochemistry </cdk.keyword>
    public interface IAtomParity : ICloneable
    {
        /// <summary> Returns the atom for which this parity is defined.
        /// 
        /// </summary>
        /// <returns> The atom for which this parity is defined
        /// </returns>
        IAtom Atom
        {
            get;

        }

        /// <summary> Returns the four atoms that define the stereochemistry for
        /// this parity.
        /// 
        /// </summary>
        /// <returns> The four atoms that define the stereochemistry for
        /// this parity
        /// </returns>
        IAtom[] SurroundingAtoms
        {
            get;
        }

        /// <summary> Returns the parity value.
        /// 
        /// </summary>
        /// <returns> The parity value
        /// </returns>
        int Parity
        {
            get;
        }
    }
}
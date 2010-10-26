/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
* 
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) project
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
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <summary> Class with convenience methods that provide methods to manipulate
    /// AminoAcid's.
    /// 
    /// </summary>
    /// <cdk.module>   standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>       Egon Willighagen
    /// </author>
    /// <cdk.created>  2005-08-19 </cdk.created>
    public class AminoAcidManipulator
    {

        /// <summary> Removes the singly bonded oxygen from the acid group of the AminoAcid.
        /// 
        /// </summary>
        /// <param name="acid">AminoAcid from which to remove the oxygen
        /// </param>
        /// <throws>  CDKException when the C-terminus is not defined for the given AminoAcid  </throws>
        public static void removeAcidicOxygen(IAminoAcid acid)
        {
            if (acid.getCTerminus() == null)
                throw new CDKException("Cannot remove oxygen: C-terminus is not defined!");

            IBond[] bonds = acid.getConnectedBonds(acid.getCTerminus());
            // ok, look for the oxygen which is singly bonded
            for (int i = 0; i < bonds.Length; i++)
            {
                if (bonds[i].Order == CDKConstants.BONDORDER_SINGLE)
                {
                    IAtom[] atoms = bonds[i].getAtoms();
                    for (int j = 0; j < atoms.Length; j++)
                    {
                        if (atoms[j].Symbol.Equals("O"))
                        {
                            // yes, we found a singly bonded oxygen!
                            acid.removeAtomAndConnectedElectronContainers(atoms[j]);
                        }
                    }
                }
            }
        }

        /// <summary> Adds the singly bonded oxygen from the acid group of the AminoAcid.
        /// 
        /// </summary>
        /// <param name="acid">        AminoAcid to which to add the oxygen
        /// </param>
        /// <throws>  CDKException when the C-terminus is not defined for the given AminoAcid  </throws>
        public static void addAcidicOxygen(IAminoAcid acid)
        {
            if (acid.getCTerminus() == null)
                throw new CDKException("Cannot add oxygen: C-terminus is not defined!");

            acid.addBond(acid.Builder.newBond(acid.getCTerminus(), acid.Builder.newAtom("O"), CDKConstants.BONDORDER_SINGLE));
        }
    }
}
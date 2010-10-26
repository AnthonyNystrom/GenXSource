/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 06:14:13 +0000 (Fri, 14 Jul 2006) $
* $Revision: 6659 $
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
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="ChemModelManipulator">
    /// </seealso>
    public class SetOfMoleculesManipulator
    {
        public static int getAtomCount(ISetOfAtomContainers set_Renamed)
        {
            return SetOfAtomContainersManipulator.getAtomCount(set_Renamed);
        }

        public static int getBondCount(ISetOfAtomContainers set_Renamed)
        {
            return SetOfAtomContainersManipulator.getBondCount(set_Renamed);
        }

        public static void removeAtomAndConnectedElectronContainers(ISetOfMolecules set_Renamed, IAtom atom)
        {
            SetOfAtomContainersManipulator.removeAtomAndConnectedElectronContainers(set_Renamed, atom);
        }

        public static void removeElectronContainer(ISetOfMolecules set_Renamed, IElectronContainer electrons)
        {
            SetOfAtomContainersManipulator.removeElectronContainer(set_Renamed, electrons);
        }

        /// <summary> Puts all the Molecules of this container together in one 
        /// AtomCcntainer.
        /// 
        /// </summary>
        /// <returns>  The AtomContainer with all the Molecules of this container
        /// 
        /// </returns>
        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getAllInOneContainer(set_Renamed);
        }

        /// <summary> Returns all the AtomContainer's of a SetOfMolecules.</summary>
        public static IAtomContainer[] getAllAtomContainers(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getAllAtomContainers(set_Renamed);
        }

        /// <seealso cref="SetOfAtomContainersManipulator">
        /// </seealso>
        public static double getTotalCharge(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getTotalCharge(set_Renamed);
        }

        /// <seealso cref="SetOfAtomContainersManipulator">
        /// </seealso>
        public static double getTotalFormalCharge(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getTotalFormalCharge(set_Renamed);
        }

        /// <seealso cref="SetOfAtomContainersManipulator">
        /// </seealso>
        public static int getTotalHydrogenCount(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getTotalHydrogenCount(set_Renamed);
        }

        public static System.Collections.ArrayList getAllIDs(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getAllIDs(set_Renamed);
        }

        public static void setAtomProperties(ISetOfMolecules set_Renamed, System.Object propKey, System.Object propVal)
        {
            SetOfAtomContainersManipulator.setAtomProperties(set_Renamed, propKey, propVal);
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfMolecules moleculeSet, IAtom atom)
        {
            return SetOfAtomContainersManipulator.getRelevantAtomContainer(moleculeSet, atom);
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfMolecules moleculeSet, IBond bond)
        {
            return SetOfAtomContainersManipulator.getRelevantAtomContainer(moleculeSet, bond);
        }

        public static System.Collections.IList getAllChemObjects(ISetOfMolecules set_Renamed)
        {
            return SetOfAtomContainersManipulator.getAllChemObjects(set_Renamed);
        }
    }
}
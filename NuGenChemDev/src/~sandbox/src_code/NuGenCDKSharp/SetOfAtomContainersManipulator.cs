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
*  */
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Graph;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="ChemModelManipulator">
    /// </seealso>
    public class SetOfAtomContainersManipulator
    {
        public static int getAtomCount(ISetOfAtomContainers set_Renamed)
        {
            int count = 0;
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                count += acs[i].AtomCount;
            }
            return count;
        }

        public static int getBondCount(ISetOfAtomContainers set_Renamed)
        {
            int count = 0;
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                count += acs[i].getBondCount();
            }
            return count;
        }

        public static void removeAtomAndConnectedElectronContainers(ISetOfAtomContainers set_Renamed, IAtom atom)
        {
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                IAtomContainer container = acs[i];
                if (container.contains(atom))
                {
                    container.removeAtomAndConnectedElectronContainers(atom);
                    IMolecule[] molecules = ConnectivityChecker.partitionIntoMolecules(container).Molecules;
                    if (molecules.Length > 1)
                    {
                        set_Renamed.removeAtomContainer(container);
                        for (int k = 0; k < molecules.Length; k++)
                        {
                            set_Renamed.addAtomContainer(molecules[k]);
                        }
                    }
                    return;
                }
            }
        }

        public static void removeElectronContainer(ISetOfAtomContainers set_Renamed, IElectronContainer electrons)
        {
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                IAtomContainer container = acs[i];
                if (container.contains(electrons))
                {
                    container.removeElectronContainer(electrons);
                    IMolecule[] molecules = ConnectivityChecker.partitionIntoMolecules(container).Molecules;
                    if (molecules.Length > 1)
                    {
                        set_Renamed.removeAtomContainer(container);
                        for (int k = 0; k < molecules.Length; k++)
                        {
                            set_Renamed.addAtomContainer(molecules[k]);
                        }
                    }
                    return;
                }
            }
        }

        /// <summary> Puts all the AtomContainers of this set together in one 
        /// AtomCcntainer.
        /// 
        /// </summary>
        /// <returns>  The AtomContainer with all the AtomContainers of this set
        /// 
        /// </returns>
        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(ISetOfAtomContainers set_Renamed)
        {
            IAtomContainer container = set_Renamed.Builder.newAtomContainer();
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                container.add(acs[i]);
            }
            return container;
        }

        /// <summary> Returns all the AtomContainer's of a SetOfMolecules.</summary>
        public static IAtomContainer[] getAllAtomContainers(ISetOfAtomContainers set_Renamed)
        {
            return set_Renamed.AtomContainers;
        }

        /// <returns> The summed charges of all atoms in this set.
        /// </returns>
        public static double getTotalCharge(ISetOfAtomContainers set_Renamed)
        {
            double charge = 0;
            for (int i = 0; i < set_Renamed.AtomContainerCount; i++)
            {
                int thisCharge = AtomContainerManipulator.getTotalFormalCharge(set_Renamed.getAtomContainer(i));
                double stoich = set_Renamed.getMultiplier(i);
                charge += stoich * thisCharge;
            }
            return charge;
        }

        /// <returns> The summed formal charges of all atoms in this set.
        /// </returns>
        public static double getTotalFormalCharge(ISetOfAtomContainers set_Renamed)
        {
            int charge = 0;
            for (int i = 0; i < set_Renamed.AtomContainerCount; i++)
            {
                int thisCharge = AtomContainerManipulator.getTotalFormalCharge(set_Renamed.getAtomContainer(i));
                double stoich = set_Renamed.getMultiplier(i);
                charge = (int)(charge + stoich * thisCharge);
            }
            return charge;
        }

        /// <returns> The summed implicit hydrogens of all atoms in this set.
        /// </returns>
        public static int getTotalHydrogenCount(ISetOfAtomContainers set_Renamed)
        {
            int hCount = 0;
            for (int i = 0; i < set_Renamed.AtomContainerCount; i++)
            {
                hCount += AtomContainerManipulator.getTotalHydrogenCount(set_Renamed.getAtomContainer(i));
            }
            return hCount;
        }

        public static System.Collections.ArrayList getAllIDs(ISetOfAtomContainers set_Renamed)
        {
            System.Collections.ArrayList idList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            if (set_Renamed != null)
            {
                if (set_Renamed.ID != null)
                    idList.Add(set_Renamed.ID);
                for (int i = 0; i < set_Renamed.AtomContainerCount; i++)
                {
                    idList.Add(AtomContainerManipulator.getAllIDs(set_Renamed.getAtomContainer(i)));
                }
            }
            return idList;
        }

        public static void setAtomProperties(ISetOfAtomContainers set_Renamed, System.Object propKey, System.Object propVal)
        {
            if (set_Renamed != null)
            {
                for (int i = 0; i < set_Renamed.AtomContainerCount; i++)
                {
                    AtomContainerManipulator.setAtomProperties(set_Renamed.getAtomContainer(i), propKey, propVal);
                }
            }
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfAtomContainers containerSet, IAtom atom)
        {
            IAtomContainer[] containers = containerSet.AtomContainers;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].contains(atom))
                {
                    return containers[i];
                }
            }
            return null;
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfAtomContainers containerSet, IBond bond)
        {
            IAtomContainer[] containers = containerSet.AtomContainers;
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].contains(bond))
                {
                    return containers[i];
                }
            }
            return null;
        }

        public static System.Collections.IList getAllChemObjects(ISetOfAtomContainers set_Renamed)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list.Add(set_Renamed);
            IAtomContainer[] acs = set_Renamed.AtomContainers;
            for (int i = 0; i < acs.Length; i++)
            {
                list.Add(acs[i]); // don't recurse into AC's for now
            }
            return list;
        }
    }
}
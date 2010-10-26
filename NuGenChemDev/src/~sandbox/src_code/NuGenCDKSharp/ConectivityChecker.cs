/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-07-12 15:46:26 +0200 (Wed, 12 Jul 2006) $    
* $Revision: 6641 $
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
* 
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.Graph
{
    /// <summary> Tool class for checking whether the (sub)structure in an
    /// AtomContainer is connected.
    /// To check wether an AtomContainer is connected this code
    /// can be used:
    /// <pre>
    /// boolean isConnected = ConnectivityChecker.isConnected(atomContainer);
    /// </pre>
    /// 
    /// <p>A disconnected AtomContainer can be fragmented into connected
    /// fragments by using code like:
    /// <pre>
    /// SetOfMolecules fragments = ConnectivityChecker.partitionIntoMolecules(disconnectedContainer);
    /// int fragmentCount = fragments.getMoleculeCount();
    /// </pre> 
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  connectivity </cdk.keyword>
    public class ConnectivityChecker
    {
        /// <summary> Check whether a set of atoms in an atomcontainer is connected
        /// 
        /// </summary>
        /// <param name="atomContainer"> The AtomContainer to be check for connectedness
        /// </param>
        /// <returns>                 true if the AtomContainer is connected   
        /// </returns>
        public static bool isConnected(IAtomContainer atomContainer)
        {
            IAtomContainer ac = atomContainer.Builder.newAtomContainer();
            IAtom atom = null;
            IMolecule molecule = atomContainer.Builder.newMolecule();
            System.Collections.ArrayList sphere = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int f = 0; f < atomContainer.AtomCount; f++)
            {
                atom = atomContainer.getAtomAt(f);
                atomContainer.getAtomAt(f).setFlag(CDKConstants.VISITED, false);
                ac.addAtom(atomContainer.getAtomAt(f));
            }
            IBond[] bonds = atomContainer.Bonds;
            for (int f = 0; f < bonds.Length; f++)
            {
                bonds[f].setFlag(CDKConstants.VISITED, false);
                ac.addBond(bonds[f]);
            }
            atom = ac.getAtomAt(0);
            sphere.Add(atom);
            atom.setFlag(CDKConstants.VISITED, true);
            PathTools.breadthFirstSearch(ac, sphere, molecule);
            if (molecule.AtomCount == atomContainer.AtomCount)
            {
                return true;
            }
            return false;
        }

        /// <summary> Partitions the atoms in an AtomContainer into covalently connected components.
        /// 
        /// </summary>
        /// <param name="atomContainer"> The AtomContainer to be partitioned into connected components, i.e. molecules
        /// </param>
        /// <returns>                 A SetOfMolecules.
        /// 
        /// </returns>
        /// <cdk.dictref>    blue-obelisk:graphPartitioning </cdk.dictref>
        public static ISetOfMolecules partitionIntoMolecules(IAtomContainer atomContainer)
        {
            IAtomContainer ac = atomContainer.Builder.newAtomContainer();
            IAtom atom = null;
            IElectronContainer eContainer = null;
            IMolecule molecule = null;
            ISetOfMolecules molecules = atomContainer.Builder.newSetOfMolecules();
            System.Collections.ArrayList sphere = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int f = 0; f < atomContainer.AtomCount; f++)
            {
                atom = atomContainer.getAtomAt(f);
                atom.setFlag(CDKConstants.VISITED, false);
                ac.addAtom(atom);
            }
            IElectronContainer[] eContainers = atomContainer.ElectronContainers;
            for (int f = 0; f < eContainers.Length; f++)
            {
                eContainer = eContainers[f];
                eContainer.setFlag(CDKConstants.VISITED, false);
                ac.addElectronContainer(eContainer);
            }
            while (ac.AtomCount > 0)
            {
                atom = ac.getAtomAt(0);
                molecule = atomContainer.Builder.newMolecule();
                sphere.Clear();
                sphere.Add(atom);
                atom.setFlag(CDKConstants.VISITED, true);
                PathTools.breadthFirstSearch(ac, sphere, molecule);
                molecules.addMolecule(molecule);
                ac.remove(molecule);
            }
            return molecules;
        }
    }
}
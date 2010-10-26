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

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <summary> Class with convenience methods that provide methods from
    /// methods from ChemObjects within the ChemModel. For example:
    /// <pre>
    /// ChemModelManipulator.removeAtomAndConnectedElectronContainers(chemModel, atom);
    /// </pre>
    /// will find the Atom in the model by traversing the ChemModel's
    /// SetOfMolecules, Crystal and SetOfReactions fields and remove
    /// it with the removeAtomAndConnectedElectronContainers(Atom) method.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="org.openscience.cdk.AtomContainer.removeAtomAndConnectedElectronContainers(IAtom)">
    /// </seealso>
    public class ChemModelManipulator
    {
        public static int getAtomCount(IChemModel chemModel)
        {
            int count = 0;
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                count += crystal.AtomCount;
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                count += SetOfMoleculesManipulator.getAtomCount(moleculeSet);
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                count += SetOfReactionsManipulator.getAtomCount(reactionSet);
            }
            return count;
        }

        public static int getBondCount(IChemModel chemModel)
        {
            int count = 0;
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                count += crystal.getBondCount();
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                count += SetOfMoleculesManipulator.getBondCount(moleculeSet);
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                count += SetOfReactionsManipulator.getBondCount(reactionSet);
            }
            return count;
        }

        public static void removeAtomAndConnectedElectronContainers(IChemModel chemModel, IAtom atom)
        {
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                if (crystal.contains(atom))
                {
                    crystal.removeAtomAndConnectedElectronContainers(atom);
                }
                return;
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                SetOfMoleculesManipulator.removeAtomAndConnectedElectronContainers(moleculeSet, atom);
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                SetOfReactionsManipulator.removeAtomAndConnectedElectronContainers(reactionSet, atom);
            }
        }

        public static void removeElectronContainer(IChemModel chemModel, IElectronContainer electrons)
        {
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                if (crystal.contains(electrons))
                {
                    crystal.removeElectronContainer(electrons);
                }
                return;
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                SetOfMoleculesManipulator.removeElectronContainer(moleculeSet, electrons);
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                SetOfReactionsManipulator.removeElectronContainer(reactionSet, electrons);
            }
        }

        /// <summary> Puts all the Molecules of this container together in one 
        /// AtomContainer.
        /// 
        /// </summary>
        /// <returns>  The AtomContainer with all the Molecules of this container
        /// 
        /// </returns>
        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(IChemModel chemModel)
        {
            IAtomContainer container = chemModel.Builder.newAtomContainer();
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                container.add(crystal);
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                container.add(SetOfMoleculesManipulator.getAllInOneContainer(moleculeSet));
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                container.add(SetOfReactionsManipulator.getAllInOneContainer(reactionSet));
            }
            return container;
        }

        public static IAtomContainer createNewMolecule(IChemModel chemModel)
        {
            // Add a new molecule either the set of molecules
            IMolecule molecule = chemModel.Builder.newMolecule();
            if (chemModel.SetOfMolecules != null)
            {
                ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
                moleculeSet.addMolecule(molecule);
            }
            else
            {
                ISetOfMolecules moleculeSet = chemModel.Builder.newSetOfMolecules();
                moleculeSet.addMolecule(molecule);
                chemModel.SetOfMolecules = moleculeSet;
            }
            return molecule;
        }

        public static IChemModel newChemModel(IAtomContainer molecule)
        {
            IChemModel model = molecule.Builder.newChemModel();
            ISetOfMolecules moleculeSet = model.Builder.newSetOfMolecules();
            moleculeSet.addAtomContainer(molecule);
            model.SetOfMolecules = moleculeSet;
            return model;
        }

        /// <summary> This badly named methods tries to determine which AtomContainer in the
        /// ChemModel is best suited to contain added Atom's and Bond's.
        /// </summary>
        public static IAtomContainer getRelevantAtomContainer(IChemModel chemModel, IAtom atom)
        {
            IAtomContainer result = null;
            if (chemModel.SetOfMolecules != null)
            {
                ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
                result = SetOfMoleculesManipulator.getRelevantAtomContainer(moleculeSet, atom);
                if (result != null)
                {
                    return result;
                }
            }
            if (chemModel.SetOfReactions != null)
            {
                ISetOfReactions reactionSet = chemModel.SetOfReactions;
                return SetOfReactionsManipulator.getRelevantAtomContainer(reactionSet, atom);
            }
            // This should never happen.
            return null;
        }

        public static IAtomContainer getRelevantAtomContainer(IChemModel chemModel, IBond bond)
        {
            IAtomContainer result = null;
            if (chemModel.SetOfMolecules != null)
            {
                ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
                result = SetOfMoleculesManipulator.getRelevantAtomContainer(moleculeSet, bond);
                if (result != null)
                {
                    return result;
                }
            }
            if (chemModel.SetOfReactions != null)
            {
                ISetOfReactions reactionSet = chemModel.SetOfReactions;
                return SetOfReactionsManipulator.getRelevantAtomContainer(reactionSet, bond);
            }
            // This should never happen.
            return null;
        }

        public static IReaction getRelevantReaction(IChemModel chemModel, IAtom atom)
        {
            IReaction reaction = null;
            if (chemModel.SetOfReactions != null)
            {
                ISetOfReactions reactionSet = chemModel.SetOfReactions;
                reaction = SetOfReactionsManipulator.getRelevantReaction(reactionSet, atom);
            }
            return reaction;
        }

        /// <summary> Returns all the AtomContainer's of a ChemModel.</summary>
        public static IAtomContainer[] getAllAtomContainers(IChemModel chemModel)
        {
            ISetOfMolecules moleculeSet = chemModel.Builder.newSetOfMolecules();
            if (chemModel.SetOfMolecules != null)
            {
                moleculeSet.add(chemModel.SetOfMolecules);
            }
            if (chemModel.SetOfReactions != null)
            {
                moleculeSet.add(SetOfReactionsManipulator.getAllMolecules(chemModel.SetOfReactions));
            }
            return SetOfMoleculesManipulator.getAllAtomContainers(moleculeSet);
        }

        public static void setAtomProperties(IChemModel chemModel, System.Object propKey, System.Object propVal)
        {
            if (chemModel.SetOfMolecules != null)
            {
                SetOfMoleculesManipulator.setAtomProperties(chemModel.SetOfMolecules, propKey, propVal);
            }
            if (chemModel.SetOfReactions != null)
            {
                SetOfReactionsManipulator.setAtomProperties(chemModel.SetOfReactions, propKey, propVal);
            }
            if (chemModel.Crystal != null)
            {
                AtomContainerManipulator.setAtomProperties(chemModel.Crystal, propKey, propVal);
            }
        }

        public static System.Collections.IList getAllChemObjects(IChemModel chemModel)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            list.Add(chemModel);
            ICrystal crystal = chemModel.Crystal;
            if (crystal != null)
            {
                list.Add(crystal);
            }
            ISetOfMolecules moleculeSet = chemModel.SetOfMolecules;
            if (moleculeSet != null)
            {
                list.AddRange(SetOfMoleculesManipulator.getAllChemObjects(moleculeSet));
            }
            ISetOfReactions reactionSet = chemModel.SetOfReactions;
            if (reactionSet != null)
            {
                list.AddRange(SetOfReactionsManipulator.getAllChemObjects(reactionSet));
            }
            return list;
        }
    }
}
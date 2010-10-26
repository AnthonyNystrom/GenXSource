/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 13:07:00 +0200 (Fri, 14 Jul 2006) $
* $Revision: 6671 $
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
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="ChemModelManipulator">
    /// </seealso>
    public class ReactionManipulator
    {

        public static int getAtomCount(IReaction reaction)
        {
            int count = 0;
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                count += reactants[i].AtomCount;
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                count += products[i].AtomCount;
            }
            return count;
        }

        public static int getBondCount(IReaction reaction)
        {
            int count = 0;
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                count += reactants[i].getBondCount();
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                count += products[i].getBondCount();
            }
            return count;
        }

        public static void removeAtomAndConnectedElectronContainers(IReaction reaction, IAtom atom)
        {
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                IMolecule mol = reactants[i];
                if (mol.contains(atom))
                {
                    mol.removeAtomAndConnectedElectronContainers(atom);
                }
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                IMolecule mol = products[i];
                if (mol.contains(atom))
                {
                    mol.removeAtomAndConnectedElectronContainers(atom);
                }
            }
        }

        public static void removeElectronContainer(IReaction reaction, IElectronContainer electrons)
        {
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                IMolecule mol = reactants[i];
                if (mol.contains(electrons))
                {
                    mol.removeElectronContainer(electrons);
                }
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                IMolecule mol = products[i];
                if (mol.contains(electrons))
                {
                    mol.removeElectronContainer(electrons);
                }
            }
        }

        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(IReaction reaction)
        {
            IAtomContainer container = reaction.Builder.newAtomContainer();
            if (reaction == null)
            {
                return container;
            }
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                IMolecule molecule = reactants[i];
                container.add(molecule);
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                IMolecule molecule = products[i];
                container.add(molecule);
            }
            return container;
        }

        public static ISetOfMolecules getAllMolecules(IReaction reaction)
        {
            ISetOfMolecules moleculeSet = reaction.Builder.newSetOfMolecules();
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                moleculeSet.addMolecule(reactants[i]);
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                moleculeSet.addMolecule(products[i]);
            }
            return moleculeSet;
        }

        /// <summary> Returns a new Reaction object which is the reverse of the given
        /// Reaction.
        /// </summary>
        public static IReaction reverse(IReaction reaction)
        {
            IReaction reversedReaction = reaction.Builder.newReaction();
            if (reaction.Direction == IReaction_Fields.BIDIRECTIONAL)
            {
                reversedReaction.Direction = IReaction_Fields.BIDIRECTIONAL;
            }
            else if (reaction.Direction == IReaction_Fields.FORWARD)
            {
                reversedReaction.Direction = IReaction_Fields.BACKWARD;
            }
            else if (reaction.Direction == IReaction_Fields.BACKWARD)
            {
                reversedReaction.Direction = IReaction_Fields.FORWARD;
            }
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                double coefficient = reaction.getReactantCoefficient(reactants[i]);
                reversedReaction.addProduct(reactants[i], coefficient);
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                double coefficient = reaction.getProductCoefficient(products[i]);
                reversedReaction.addReactant(products[i], coefficient);
            }
            return reversedReaction;
        }

        /// <summary> Returns all the AtomContainer's of a Reaction.</summary>
        public static IAtomContainer[] getAllAtomContainers(IReaction reaction)
        {
            return SetOfMoleculesManipulator.getAllAtomContainers(getAllMolecules(reaction));
        }

        public static System.Collections.ArrayList getAllIDs(IReaction reaction)
        {
            System.Collections.ArrayList idList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            if (reaction.ID != null)
                idList.Add(reaction.ID);
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                IMolecule mol = reactants[i];
                idList.AddRange(AtomContainerManipulator.getAllIDs(mol));
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                IMolecule mol = products[i];
                idList.AddRange(AtomContainerManipulator.getAllIDs(mol));
            }
            return idList;
        }

        public static IAtomContainer getRelevantAtomContainer(IReaction reaction, IAtom atom)
        {
            IAtomContainer result = SetOfMoleculesManipulator.getRelevantAtomContainer(reaction.Reactants, atom);
            if (result != null)
            {
                return result;
            }
            return SetOfMoleculesManipulator.getRelevantAtomContainer(reaction.Products, atom);
        }

        public static IAtomContainer getRelevantAtomContainer(IReaction reaction, IBond bond)
        {
            IAtomContainer result = SetOfMoleculesManipulator.getRelevantAtomContainer(reaction.Reactants, bond);
            if (result != null)
            {
                return result;
            }
            return SetOfMoleculesManipulator.getRelevantAtomContainer(reaction.Products, bond);
        }

        public static void setAtomProperties(IReaction reaction, System.Object propKey, System.Object propVal)
        {
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int j = 0; j < reactants.Length; j++)
            {
                AtomContainerManipulator.setAtomProperties(reactants[j], propKey, propVal);
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int j = 0; j < products.Length; j++)
            {
                AtomContainerManipulator.setAtomProperties(products[j], propKey, propVal);
            }
        }

        public static System.Collections.IList getAllChemObjects(IReaction reaction)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            IMolecule[] reactants = reaction.Reactants.Molecules;
            for (int i = 0; i < reactants.Length; i++)
            {
                list.Add(reactants[i]);
            }
            IMolecule[] products = reaction.Products.Molecules;
            for (int i = 0; i < products.Length; i++)
            {
                list.Add(products[i]);
            }
            return list;
        }
        /// <summary> get the IAtom which is mapped
        /// 
        /// </summary>
        /// <param name="reaction">  The IReaction which contains the mapping 
        /// </param>
        /// <param name="chemObject">The IChemObject which will be searched its mapped IChemObject
        /// </param>
        /// <returns>           The mapped IChemObject
        /// </returns>
        public static IChemObject getMappedChemObject(IReaction reaction, IChemObject chemObject)
        {
            IMapping[] mappings = reaction.Mappings;
            for (int i = 0; i < mappings.Length; i++)
            {
                IMapping mapping = mappings[i];
                IChemObject[] map = mapping.RelatedChemObjects;
                if (map[0].Equals(chemObject))
                {
                    return map[1];
                }
                else if (map[1].Equals(chemObject))
                    return map[0];
            }
            return null;
        }
    }
}
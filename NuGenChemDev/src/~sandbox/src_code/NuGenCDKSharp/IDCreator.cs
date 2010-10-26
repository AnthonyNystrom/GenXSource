/* $RCSfile$
* $Author: egonw $
* $Date: 2006-07-02 13:48:44 +0200 (Sun, 02 Jul 2006) $
* $Revision: 6537 $
*
* Copyright (C) 2003-2006  The Chemistry Development Kit (CDK) Project
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
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Tools.Manipulator;

namespace Org.OpenScience.CDK.Tools
{
    /// <summary> Class that provides methods to give unique IDs to ChemObjects.
    /// Methods are implemented for Atom, Bond, AtomContainer, SetOfAtomContainers
    /// and Reaction. It will only create missing IDs. If you want to create new
    /// IDs for all ChemObjects, you need to delete them first.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>    Egon Willighagen
    /// </author>
    /// <cdk.created>   2003-04-01 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>   id, creation </cdk.keyword>
    /// <cdk.bug>       1455341 </cdk.bug>
    public class IDCreator
    {
        /// <summary> A list of taken IDs.</summary>
        private System.Collections.ArrayList tabuList;

        /// <summary> Keep track of numbers.</summary>
        internal int atomCount;
        internal int bondCount;
        internal int moleculeCount;
        internal int reactionCount;

        public IDCreator()
        {
            reset();
        }

        public virtual void reset()
        {
            tabuList = null;
            atomCount = 0;
            bondCount = 0;
            moleculeCount = 0;
            reactionCount = 0;
        }

        /// <summary> Labels the Atom's and Bond's in the AtomContainer using the a1, a2, b1, b2
        /// scheme often used in CML.
        /// 
        /// </summary>
        /// <seealso cref="createIDs(ISetOfAtomContainers)">
        /// </seealso>
        public virtual void createIDs(IAtomContainer container)
        {
            if (tabuList == null)
                tabuList = AtomContainerManipulator.getAllIDs(container);

            if (container.ID == null)
            {
                moleculeCount++;
                while (tabuList.Contains("m" + moleculeCount))
                    moleculeCount++;
                container.ID = "m" + moleculeCount;
            }

            IAtom[] atoms = container.Atoms;
            for (int i = 0; i < atoms.Length; i++)
            {
                IAtom atom = atoms[i];
                if (atom.ID == null)
                {
                    atomCount++;
                    while (tabuList.Contains("a" + atomCount))
                        atomCount++;
                    atoms[i].ID = "a" + atomCount;
                }
            }
            IBond[] bonds = container.Bonds;
            for (int i = 0; i < bonds.Length; i++)
            {
                IBond bond = bonds[i];
                if (bond.ID == null)
                {
                    bondCount++;
                    while (tabuList.Contains("b" + bondCount))
                        bondCount++;
                    bonds[i].ID = "b" + bondCount;
                }
            }
        }

        public virtual void createIDs(ISetOfMolecules containerSet)
        {
            createIDs((ISetOfAtomContainers)containerSet);
        }

        /// <summary> Labels the Atom's and Bond's in each AtomContainer using the a1, a2, b1, b2
        /// scheme often used in CML. It will also set id's for all AtomContainers, naming
        /// them m1, m2, etc.
        /// It will not the SetOfAtomContainers itself.
        /// </summary>
        public virtual void createIDs(ISetOfAtomContainers containerSet)
        {
            if (tabuList == null)
                tabuList = SetOfAtomContainersManipulator.getAllIDs(containerSet);

            if (containerSet.ID == null)
            {
                moleculeCount++;
                while (tabuList.Contains("molSet" + moleculeCount))
                    moleculeCount++;
                containerSet.ID = "molSet" + moleculeCount;
            }

            IAtomContainer[] containers = containerSet.AtomContainers;
            for (int i = 0; i < containers.Length; i++)
            {
                IAtomContainer container = containers[i];
                if (container.ID == null)
                {
                    createIDs(container);
                }
            }
        }

        /// <summary> Labels the reactants and products in the Reaction m1, m2, etc, and the atoms
        /// accordingly, when no ID is given.
        /// </summary>
        public virtual void createIDs(IReaction reaction)
        {
            if (tabuList == null)
                tabuList = ReactionManipulator.getAllIDs(reaction);

            if (reaction.ID == null)
            {
                reactionCount++;
                while (tabuList.Contains("r" + reactionCount))
                    reactionCount++;
                reaction.ID = "r" + reactionCount;
            }

            IAtomContainer[] reactants = reaction.Reactants.AtomContainers;
            for (int i = 0; i < reactants.Length; i++)
            {
                createIDs(reactants[i]);
            }
            IAtomContainer[] products = reaction.Products.AtomContainers;
            for (int i = 0; i < products.Length; i++)
            {
                createIDs(products[i]);
            }
        }

        public virtual void createIDs(ISetOfReactions reactionSet)
        {
            IReaction[] reactions = reactionSet.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                createIDs(reactions[i]);
            }
        }

        public virtual void createIDs(IChemFile file)
        {
            IChemSequence[] sequences = file.ChemSequences;
            for (int i = 0; i < sequences.Length; i++)
            {
                createIDs(sequences[i]);
            }
        }

        public virtual void createIDs(IChemSequence sequence)
        {
            IChemModel[] models = sequence.ChemModels;
            for (int i = 0; i < models.Length; i++)
            {
                createIDs(models[i]);
            }
        }

        public virtual void createIDs(IChemModel model)
        {
            ICrystal crystal = model.Crystal;
            if (crystal != null)
                createIDs(crystal);
            ISetOfMolecules moleculeSet = model.SetOfMolecules;
            if (moleculeSet != null)
                createIDs(moleculeSet);
            ISetOfReactions reactionSet = model.SetOfReactions;
            if (reactionSet != null)
                createIDs(reactionSet);
        }

        public virtual void createIDs(ICrystal crystal)
        {
            createIDs((IAtomContainer)crystal);
        }
    }
}
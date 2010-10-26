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
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <seealso cref="ChemModelManipulator">
    /// </seealso>
    public class SetOfReactionsManipulator
    {
        public static int getAtomCount(ISetOfReactions set_Renamed)
        {
            int count = 0;
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                count += ReactionManipulator.getAtomCount(reactions[i]);
            }
            return count;
        }

        public static int getBondCount(ISetOfReactions set_Renamed)
        {
            int count = 0;
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                count += ReactionManipulator.getBondCount(reactions[i]);
            }
            return count;
        }

        public static void removeAtomAndConnectedElectronContainers(ISetOfReactions set_Renamed, IAtom atom)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                ReactionManipulator.removeAtomAndConnectedElectronContainers(reaction, atom);
            }
        }

        public static void removeElectronContainer(ISetOfReactions set_Renamed, IElectronContainer electrons)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                ReactionManipulator.removeElectronContainer(reaction, electrons);
            }
        }

        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(ISetOfReactions set_Renamed)
        {
            IAtomContainer container = set_Renamed.Builder.newAtomContainer();
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                container.add(ReactionManipulator.getAllInOneContainer(reaction));
            }
            return container;
        }

        public static ISetOfMolecules getAllMolecules(ISetOfReactions set_Renamed)
        {
            ISetOfMolecules moleculeSet = set_Renamed.Builder.newSetOfMolecules();
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                moleculeSet.add(ReactionManipulator.getAllMolecules(reaction));
            }
            return moleculeSet;
        }

        public static System.Collections.ArrayList getAllIDs(ISetOfReactions set_Renamed)
        {
            System.Collections.ArrayList IDlist = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                IDlist.AddRange(ReactionManipulator.getAllIDs(reaction));
            }
            return IDlist;
        }

        /// <summary> Returns all the AtomContainer's of a Reaction.</summary>
        public static IAtomContainer[] getAllAtomContainers(ISetOfReactions set_Renamed)
        {
            return SetOfMoleculesManipulator.getAllAtomContainers(getAllMolecules(set_Renamed));
        }

        public static IReaction getRelevantReaction(ISetOfReactions set_Renamed, IAtom atom)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                IAtomContainer container = ReactionManipulator.getRelevantAtomContainer(reaction, atom);
                if (container != null)
                {
                    // a match!
                    return reaction;
                }
            }
            return null;
        }

        public static IReaction getRelevantReaction(ISetOfReactions set_Renamed, IBond bond)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                IAtomContainer container = ReactionManipulator.getRelevantAtomContainer(reaction, bond);
                if (container != null)
                {
                    // a match!
                    return reaction;
                }
            }
            return null;
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfReactions set_Renamed, IAtom atom)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                IAtomContainer container = ReactionManipulator.getRelevantAtomContainer(reaction, atom);
                if (container != null)
                {
                    // a match!
                    return container;
                }
            }
            return null;
        }

        public static IAtomContainer getRelevantAtomContainer(ISetOfReactions set_Renamed, IBond bond)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                IAtomContainer container = ReactionManipulator.getRelevantAtomContainer(reaction, bond);
                if (container != null)
                {
                    // a match!
                    return container;
                }
            }
            return null;
        }

        public static void setAtomProperties(ISetOfReactions set_Renamed, System.Object propKey, System.Object propVal)
        {
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                ReactionManipulator.setAtomProperties(reaction, propKey, propVal);
            }
        }

        public static System.Collections.IList getAllChemObjects(ISetOfReactions set_Renamed)
        {
            System.Collections.ArrayList list = new System.Collections.ArrayList();
            IReaction[] reactions = set_Renamed.Reactions;
            for (int i = 0; i < reactions.Length; i++)
            {
                IReaction reaction = reactions[i];
                list.AddRange(ReactionManipulator.getAllChemObjects(reaction));
            }
            return list;
        }
    }
}
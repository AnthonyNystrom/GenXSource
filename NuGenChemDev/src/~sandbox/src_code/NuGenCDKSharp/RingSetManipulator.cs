/* $RCSfile$
* $Author: egonw $ 
* $Date: 2006-07-14 06:14:13 +0000 (Fri, 14 Jul 2006) $
* $Revision: 6659 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
using Support;

namespace Org.OpenScience.CDK.Tools.Manipulator
{
    /// <cdk.module>  standard </cdk.module>
    public class RingSetManipulator
    {

        /// <summary> Returns all the atoms and bonds from all the rings in the RingSet 
        /// in one AtomContainer.
        /// 
        /// </summary>
        /// <returns> an AtomContainer with all atoms and bonds from the RingSet
        /// 
        /// </returns>
        /// <deprecated> This method has a serious performace impact. Try to use
        /// other methods.
        /// </deprecated>
        public static IAtomContainer getAllInOneContainer(IRingSet ringSet)
        {
            // FIXME: make RingSet a subclass of IChemObject (see bug #) and clean up
            // the code in the next line
            IAtomContainer container = ringSet.Builder.newAtomContainer();
            for (int i = 0; i < ringSet.AtomContainerCount; i++)
            {
                container.add((IRing)ringSet.getAtomContainer(i));
            }
            return container;
        }

        /// <summary> Sorts the rings in the set by size. The largest ring comes
        /// first.
        /// </summary>
        public static void sort(IRingSet ringSet)
        {
            System.Collections.IList ringList = new System.Collections.ArrayList();
            IAtomContainer[] rings = ringSet.AtomContainers;
            for (int i = 0; i < rings.Length; i++)
            {
                ringList.Add(rings[i]);
            }
            SupportClass.CollectionsSupport.Sort(ringList, new RingSizeComparator(RingSizeComparator.LARGE_FIRST));
            ringSet.removeAllAtomContainers();
            System.Collections.IEnumerator iter = ringList.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (iter.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                ringSet.addAtomContainer((IRing)iter.Current);
            }
        }

        /// <summary> We define the heaviest ring as the one with the highest number of double bonds.
        /// Needed for example for the placement of in-ring double bonds.
        /// 
        /// </summary>
        /// <param name="bond"> A bond which must be contained by the heaviest ring 
        /// </param>
        /// <returns>  The ring with the higest number of double bonds connected to a given bond
        /// </returns>
        public static IRing getHeaviestRing(IRingSet ringSet, IBond bond)
        {
            System.Collections.IList rings = ringSet.getRings(bond);
            IRing ring = null;
            int maxOrderSum = 0;
            for (int i = 0; i < rings.Count; i++)
            {
                if (maxOrderSum < ((IRing)rings[i]).OrderSum)
                {
                    ring = (IRing)rings[i];
                    maxOrderSum = ring.OrderSum;
                }
            }
            return ring;
        }

        /// <summary> Returns the ring with the highest numbers of other rings attached to it.
        /// 
        /// </summary>
        /// <returns> the ring with the highest numbers of other rings attached to it.    
        /// </returns>
        public static IRing getMostComplexRing(IRingSet ringSet)
        {
            int[] neighbors = new int[ringSet.AtomContainerCount];
            IRing ring1, ring2;
            IAtom atom1, atom2;
            int mostComplex = 0, mostComplexPosition = 0;
            /* for all rings in this RingSet */
            for (int i = 0; i < ringSet.AtomContainerCount; i++)
            {
                /* Take each ring */
                ring1 = (IRing)ringSet.getAtomContainer(i);
                /* look at each Atom in this ring whether it is part of any other ring */
                for (int j = 0; j < ring1.AtomCount; j++)
                {
                    atom1 = ring1.getAtomAt(j);
                    /* Look at each of the other rings in the ringset */
                    for (int k = i + 1; k < ringSet.AtomContainerCount; k++)
                    {
                        ring2 = (IRing)ringSet.getAtomContainer(k);
                        if (ring1 != ring2)
                        {
                            for (int l = 0; l < ring2.AtomCount; l++)
                            {
                                atom2 = ring2.getAtomAt(l);
                                if (atom1 == atom2)
                                {
                                    neighbors[i]++;
                                    neighbors[k]++;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] > mostComplex)
                {
                    mostComplex = neighbors[i];
                    mostComplexPosition = i;
                }
            }
            return (IRing)ringSet.getAtomContainer(mostComplexPosition);
        }

        /// <summary> Checks if <code>atom1</code> and <code>atom2</code> share membership in the same ring or ring system.
        /// Membership in the same ring is checked if the RingSet contains the SSSR of a molecule; membership in
        /// the same ring or same ring system is checked if the RingSet contains all rings of a molecule.<BR><BR>
        /// 
        /// <p><B>Important:</B> This method only returns meaningful results if <code>atom1</code> and
        /// <code>atom2</code> are members of the same molecule for which the RingSet was calculated!
        /// 
        /// </summary>
        /// <param name="atom1">  The first atom
        /// </param>
        /// <param name="atom2">  The second atom
        /// </param>
        /// <returns> boolean true if <code>atom1</code> and <code>atom2</code> share membership of at least one ring or ring system, false otherwise
        /// </returns>
        public static bool isSameRing(IRingSet ringSet, IAtom atom1, IAtom atom2)
        {
            IAtomContainer[] rings = ringSet.AtomContainers;
            for (int i = 0; i < rings.Length; i++)
            {
                IRing ring = (IRing)rings[i];
                if (ring.contains(atom1))
                    if (ring.contains(atom2))
                        return true;
            }
            return false;
        }
    }
}
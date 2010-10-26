/*
*  RingPartitioner.java
*
*  $RCSfile$    $Author: egonw $    $Date: 2006-07-14 09:45:29 +0200 (Fri, 14 Jul 2006) $    $Revision: 6663 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  Contact: cdk-devel@lists.sourceforge.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
*  All we ask is that proper credit is given for our work, which includes
*  - but is not limited to - adding the above copyright notice to the beginning
*  of your source code files, and to any copyright notice that you may distribute
*  with programs based on this work.
*
*  This program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public License
*  along with this program; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*
*/
using System;
using Org.OpenScience.CDK.Interfaces;

namespace Org.OpenScience.CDK.RingSearch
{

    /// <summary>  Partitions a RingSet into RingSets of connected rings. Rings which share an
    /// Atom, a Bond or three or more atoms with at least on other ring in the
    /// RingSet are considered connected.
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    public class RingPartitioner
    {

        /// <summary>  Debugging on/off</summary>
        public const bool debug = false;
        // minimum details


        /// <summary>  Partitions a RingSet into RingSets of connected rings. Rings which share
        /// an Atom, a Bond or three or more atoms with at least on other ring in
        /// the RingSet are considered connected.
        /// 
        /// </summary>
        /// <param name="ringSet"> The RingSet to be partitioned
        /// </param>
        /// <returns>          A Vector of connected RingSets
        /// </returns>
        public static System.Collections.ArrayList partitionRings(IRingSet ringSet)
        {
            System.Collections.ArrayList ringSets = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            if (ringSet.AtomContainerCount == 0)
                return ringSets;
            IRingSet tempRingSet = null;
            IRing ring = (IRing)ringSet.getAtomContainer(0);
            if (ring == null)
                return ringSets;
            IRingSet rs = ring.Builder.newRingSet();
            for (int f = 0; f < ringSet.AtomContainerCount; f++)
            {
                rs.addAtomContainer(ringSet.getAtomContainer(f));
            }
            do
            {
                ring = (IRing)rs.getAtomContainer(0);
                IRingSet newRs = ring.Builder.newRingSet();
                newRs.addAtomContainer(ring);
                tempRingSet = walkRingSystem(rs, ring, newRs);
                if (debug)
                {
                    System.Console.Out.WriteLine("found ringset with ringcount: " + tempRingSet.AtomContainerCount);
                }
                ringSets.Add(walkRingSystem(rs, ring, newRs));
            }
            while (rs.AtomContainerCount > 0);

            return ringSets;
        }


        /// <summary>  Converts a RingSet to an AtomContainer.
        /// 
        /// </summary>
        /// <param name="ringSet"> The RingSet to be converted.
        /// </param>
        /// <returns>          The AtomContainer containing the bonds and atoms of the ringSet.
        /// </returns>
        public static IAtomContainer convertToAtomContainer(IRingSet ringSet)
        {
            IRing ring = (IRing)ringSet.getAtomContainer(0);
            if (ring == null)
                return null;
            IAtomContainer ac = ring.Builder.newAtomContainer();
            for (int i = 0; i < ringSet.AtomContainerCount; i++)
            {
                ring = (IRing)ringSet.getAtomContainer(i);
                for (int r = 0; r < ring.getBondCount(); r++)
                {
                    IBond bond = ring.getBondAt(r);
                    if (!ac.contains(bond))
                    {
                        for (int j = 0; j < bond.AtomCount; j++)
                        {
                            ac.addAtom(bond.getAtomAt(j));
                        }
                        ac.addBond(bond);
                    }
                }
            }
            return ac;
        }


        /// <summary>  Perform a walk in the given RingSet, starting at a given Ring and
        /// recursivly searching for other Rings connected to this ring. By doing
        /// this it finds all rings in the RingSet connected to the start ring,
        /// putting them in newRs, and removing them from rs.
        /// 
        /// </summary>
        /// <param name="rs">    The RingSet to be searched
        /// </param>
        /// <param name="ring">  The ring to start with
        /// </param>
        /// <param name="newRs"> The RingSet containing all Rings connected to ring
        /// </param>
        /// <returns>        newRs The RingSet containing all Rings connected to ring
        /// </returns>
        private static IRingSet walkRingSystem(IRingSet rs, IRing ring, IRingSet newRs)
        {
            IRing tempRing;
            System.Collections.IList tempRings = rs.getConnectedRings(ring);
            if (debug)
            {
                System.Console.Out.WriteLine("walkRingSystem -> tempRings.size(): " + tempRings.Count);
            }
            rs.removeAtomContainer(ring);
            System.Collections.IEnumerator iter = tempRings.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (iter.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                tempRing = (IRing)iter.Current;
                if (!newRs.contains(tempRing))
                {
                    newRs.addAtomContainer(tempRing);
                    newRs.add(walkRingSystem(rs, tempRing, newRs));
                }
            }
            return newRs;
        }
    }
}
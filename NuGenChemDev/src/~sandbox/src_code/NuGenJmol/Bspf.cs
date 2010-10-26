/* $RCSfile$
 * $Author: egonw $
 * $Date: 2005-11-10 16:52:44 +0100 (jeu., 10 nov. 2005) $
 *
 * Copyright (C) 2003-2005  Miguel, Jmol Development, www.jmol.org
 *
 * Contact: miguel@jmol.org
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */
using System;

namespace Org.Jmol.Bspt
{
    /// <summary>
    /// A Binary Space Partitioning Forest
    /// <p>
    /// This is simply an array of Binary Space Partitioning Trees identified
    /// by indexes
    /// </summary>
    /// <author>Miguel, miguel@jmol.org</author>
    public sealed class Bspf
    {
        int dimMax;
        Bspt[] bspts;
        SphereIterator[] sphereIterators;

        public Bspf(int dimMax)
        {
            this.dimMax = dimMax;
            bspts = new Bspt[0];
            sphereIterators = new SphereIterator[0];
        }

        public int BsptCount
        {
            get { return bspts.Length; }
        }

        public void addTuple(int bsptIndex, Tuple tuple)
        {
            if (bsptIndex >= bspts.Length) {
                Bspt[] t = new Bspt[bsptIndex + 1];
                Array.Copy(bspts, 0, t, 0, bspts.Length);
                bspts = t;
            }
            Bspt bspt = bspts[bsptIndex];
            if (bspt == null)
                bspt = bspts[bsptIndex] = new Bspt(dimMax);
            bspt.addTuple(tuple);
        }

        public void stats()
        {
            for (int i = 0; i < bspts.Length; ++i)
                bspts[i].stats();
        }

        /*
        public void dump() {
        for (int i = 0; i < bspts.length; ++i) {
        System.out.println(">>>>\nDumping bspt #" + i + "\n>>>>");
        bspts[i].dump();
        }
        System.out.println("<<<<");
        }
        */

        public SphereIterator getSphereIterator(int bsptIndex)
        {
            if (bsptIndex >= sphereIterators.Length)
            {
                SphereIterator[] t = new SphereIterator[bsptIndex + 1];
                System.Array.Copy(sphereIterators, 0, t, 0, sphereIterators.Length);
                sphereIterators = t;
            }
            if (sphereIterators[bsptIndex] == null &&
                bspts[bsptIndex] != null)
                sphereIterators[bsptIndex] = bspts[bsptIndex].allocateSphereIterator();
            return sphereIterators[bsptIndex];
        }
    }
}
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
namespace Org.Jmol.Bspt
{
    /**
    * A leaf of Tuple objects in the bsp tree
    *
    * @author Miguel, miguel@jmol.org
    */
    class Leaf : Element
    {
        internal Tuple[] tuples;

        public Leaf(Bspt bspt)
        {
            this.bspt = bspt;
            count = 0;
            tuples = new Tuple[Bspt.leafCountMax];
        }

        public Leaf(Bspt bspt, Leaf leaf, int countToKeep)
            : this(bspt)
        {
            for (int i = countToKeep; i < Bspt.leafCountMax; ++i)
            {
                tuples[count++] = leaf.tuples[i];
                leaf.tuples[i] = null;
            }
            leaf.count = countToKeep;
        }

        public void sort(int dim)
        {
            for (int i = count; --i > 0; )
            { // this is > not >=
                Tuple champion = tuples[i];
                float championValue = champion.getDimensionValue(dim);
                for (int j = i; --j >= 0; )
                {
                    Tuple challenger = tuples[j];
                    float challengerValue = challenger.getDimensionValue(dim);
                    if (challengerValue > championValue)
                    {
                        tuples[i] = challenger;
                        tuples[j] = champion;
                        champion = challenger;
                        championValue = challengerValue;
                    }
                }
            }
        }

        public override Element addTuple(int level, Tuple tuple)
        {
            if (count < Bspt.leafCountMax)
            {
                tuples[count++] = tuple;
                return this;
            }
            Node node = new Node(bspt, level, this);
            return node.addTuple(level, tuple);
        }

        /*
        void dump(int level) {
        for (int i = 0; i < count; ++i) {
        Tuple t = tuples[i];
        for (int j = 0; j < level; ++j)
        System.out.print(".");
        for (int dim = 0; dim < dimMax-1; ++dim)
        System.out.print("" + t.getDimensionValue(dim) + ",");
        System.out.println("" + t.getDimensionValue(dimMax - 1));
        }
        }

        public String toString() {
        return "leaf:" + count + "\n";
        }
        */
    }
}
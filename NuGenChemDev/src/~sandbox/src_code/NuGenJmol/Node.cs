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
    /// Nodes of the bspt. It is a binary tree so nodes contain two children,
    /// called left and right.
    /// Nodes split along one dimension. The instance variable dim holds
    /// the dimension along which this node is split.
    /// Each child holds the minimum and maximum values for its subtree
    /// when split along the specified dim.
    /// <para>
    /// The current implementation allows for the case where the maximum
    /// left value is == the minimum right value. This can happen when
    /// the tree is filled with coordinate values that contain the same
    /// value along one dimension ... as with very regular crystals
    /// <para>
    /// The tree is not kept balanced.
    /// </summary>
    /// <author>Miguel, miguel@jmol.org</author>
    class Node : Element
    {
        internal int dim;
        internal float minLeft, maxLeft;
        internal Element eleLeft;
        internal float minRight, maxRight;
        internal Element eleRight;

        public Node(Bspt bspt, int level, Leaf leafLeft)
        {
            this.bspt = bspt;
            if (level == bspt.treeDepth)
            {
                bspt.treeDepth = level + 1;
                if (bspt.treeDepth >= Bspt.MAX_TREE_DEPTH)
                    Console.WriteLine("BSPT tree depth too great:" + bspt.treeDepth.ToString());
            }
            if (leafLeft.count != Bspt.leafCountMax)
                throw new NullReferenceException();
            dim = level % bspt.dimMax;
            leafLeft.sort(dim);
            Leaf leafRight = new Leaf(bspt, leafLeft, Bspt.leafCountMax / 2);
            minLeft = leafLeft.tuples[0].getDimensionValue(dim);
            maxLeft = leafLeft.tuples[leafLeft.count - 1].getDimensionValue(dim);
            minRight = leafRight.tuples[0].getDimensionValue(dim);
            maxRight = leafRight.tuples[leafRight.count - 1].getDimensionValue(dim);

            eleLeft = leafLeft;
            eleRight = leafRight;
            count = Bspt.leafCountMax;
        }

        public override Element addTuple(int level, Tuple tuple)
        {
            float dimValue = tuple.getDimensionValue(dim);
            ++count;
            bool addLeft;
            if (dimValue < maxLeft)
            {
                addLeft = true;
            }
            else if (dimValue > minRight)
            {
                addLeft = false;
            }
            else if (dimValue == maxLeft)
            {
                if (dimValue == minRight)
                {
                    if (eleLeft.count < eleRight.count)
                        addLeft = true;
                    else
                        addLeft = false;
                }
                else
                    addLeft = true;
            }
            else if (dimValue == minRight)
                addLeft = false;
            else
            {
                if (eleLeft.count < eleRight.count)
                    addLeft = true;
                else
                    addLeft = false;
            }
            if (addLeft)
            {
                if (dimValue < minLeft)
                    minLeft = dimValue;
                else if (dimValue > maxLeft)
                    maxLeft = dimValue;
                eleLeft = eleLeft.addTuple(level + 1, tuple);
            }
            else
            {
                if (dimValue < minRight)
                    minRight = dimValue;
                else if (dimValue > maxRight)
                    maxRight = dimValue;
                eleRight = eleRight.addTuple(level + 1, tuple);
            }
            return this;
        }

        /*
        void dump(int level) {
        System.out.println("");
        eleLE.dump(level + 1);
        for (int i = 0; i < level; ++i)
        System.out.print("-");
        System.out.println(">" + splitValue);
        eleGE.dump(level + 1);
        }

        public String toString() {
        return eleLE.toString() + dim + ":" +
        splitValue + "\n" + eleGE.toString();
        }
        */
    }
}

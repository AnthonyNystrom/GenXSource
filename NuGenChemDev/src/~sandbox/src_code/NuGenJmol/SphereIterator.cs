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
    /// <summary>
    /// Iterator used for finding all points within a sphere or a hemisphere
    /// <p>
    /// Obtain a SphereIterator by calling Bspt.allocateSphereIterator().
    /// <p>
    /// call initialize(...) or initializeHemizphere(...)
    /// <p>
    /// re-initialize in order to reuse the same SphereIterator
    /// </summary>
    /// <author>Miguel, miguel@jmol.org</author>
    public class SphereIterator
    {
        Bspt bspt;

        Element[] stack;
        int sp;
        int leafIndex;
        Leaf leaf;

        Tuple center;
        float radius;

        float[] centerValues;
        float radius2;
        float _foundDistance2; // the dist squared of a found Element;

        // when set, only the hemisphere sphere .GE. the point
        // (on the first dim) is returned
        bool tHemisphere;

        public SphereIterator(Bspt bspt)
        {
            this.bspt = bspt;
            centerValues = new float[bspt.dimMax];
            stack = new Element[bspt.treeDepth];
        }

        /**
         * initialize to return all points within the sphere defined
         * by center and radius
         *
         * @param center
         * @param radius
         */
        public void initialize(Tuple center, float radius)
        {
            this.center = center;
            this.radius = radius;
            this.radius2 = radius * radius;
            this.tHemisphere = false;
            for (int dim = bspt.dimMax; --dim >= 0; )
                centerValues[dim] = center.getDimensionValue(dim);
            leaf = null;
            stack[0] = bspt.eleRoot;
            sp = 1;
            findLeftLeaf();
        }

        /**
         * initialize to return all points within the hemisphere defined
         * by center and radius.
         *<p>
         * the points returned are those that have a coordinate value >=
         * to center along the first (x) dimension
         *<p>
         * Note that if you are iterating through all points, and two
         * points are within radius and have the same
         * x coordinate, then each will return the other.
         *
         * @param center
         * @param radius
         */
        public void initializeHemisphere(Tuple center, float radius)
        {
            initialize(center, radius);
            tHemisphere = true;
        }

        /**
         * nulls internal references
         */
        public void release()
        {
            for (int i = bspt.treeDepth; --i >= 0; )
                stack[i] = null;
        }

        /**
         * normal iterator predicate
         *
         * @return boolean
         */
        public bool hasMoreElements()
        {
            while (leaf != null)
            {
                for (; leafIndex < leaf.count; ++leafIndex)
                    if (isWithinRadius(leaf.tuples[leafIndex]))
                        return true;
                findLeftLeaf();
            }
            return false;
        }

        /**
         * normal iterator method
         *
         * @return Tuple
         */
        public Tuple nextElement()
        {
            return leaf.tuples[leafIndex++];
        }

        /**
         * After calling nextElement(), allows one to find out
         * the value of the distance squared. To get the distance
         * just take the sqrt.
         *
         * @return float
         */
        public float foundDistance2()
        {
            return _foundDistance2;
        }

        /**
         * does the work
         */
        private void findLeftLeaf()
        {
            leaf = null;
            if (sp == 0)
                return;
            Element ele = stack[--sp];
            while (ele is Node)
            {
                Node node = (Node)ele;
                float centerValue = centerValues[node.dim];
                float maxValue = centerValue + radius;
                float minValue = centerValue;
                if (! tHemisphere || node.dim != 0)
                    minValue -= radius;
                if (minValue <= node.maxLeft && maxValue >= node.minLeft)
                {
                    if (maxValue >= node.minRight && minValue <= node.maxRight)
                        stack[sp++] = node.eleRight;
                    ele = node.eleLeft;
                }
                else if (maxValue >= node.minRight && minValue <= node.maxRight)
                {
                    ele = node.eleRight;
                }
                else
                {
                    if (sp == 0)
                        return;
                    ele = stack[--sp];
                }
            }
            leaf = (Leaf)ele;
            leafIndex = 0;
        }

        /**
         * checks one tuple for distance
         * @param t
         * @return boolean
         */
        private bool isWithinRadius(Tuple t)
        {
            float dist2;
            float distT;
            distT = t.getDimensionValue(0) - centerValues[0];
            if (tHemisphere && distT < 0)
                return false;
            dist2 = distT * distT;
            if (dist2 > radius2)
                return false;
            int dim = bspt.dimMax - 1;
            do
            {
                distT = t.getDimensionValue(dim) - centerValues[dim];
                dist2 += distT * distT;
                if (dist2 > radius2)
                    return false;
            }
            while (--dim > 0);

            this._foundDistance2 = dist2;
            return true;
        }

    }
}
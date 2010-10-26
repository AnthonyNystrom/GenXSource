/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-12 11:05:38 +0200 (Fri, 12 May 2006) $
*
* Copyright (C) 2003-2006  The Jmol Development Team
* Copyright (C) 2003-2006  The CDK Project
*
* Contact: cdk-devel@lists.sf.net
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
*  Foundation, 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA. 
*/
using System;
using System.Collections;

namespace Org.OpenScience.CDK.Graph.Rebond
{
    /// <summary>  BSP-Tree stands for Binary Space Partitioning Tree
    /// The tree partitions n-dimensional space (in our case 3) into little
    /// boxes, facilitating searches for things which are *nearby*.
    /// For some useful background info, search the web for "bsp tree faq".
    /// Our application is somewhat simpler because we are storing points instead
    /// of polygons.
    /// 
    /// <p>We are working with three dimensions. For the purposes of the Bspt code
    /// these dimensions are stored as 0, 1, or 2. Each node of the tree splits
    /// along the next dimension, wrapping around to 0.
    /// <pre>
    /// mySplitDimension = (parentSplitDimension + 1) % 3;
    /// </pre>
    /// A split value is stored in the node. Values which are <= splitValue are
    /// stored down the left branch. Values which are >= splitValue are stored
    /// down the right branch. When this happens, the search must proceed down
    /// both branches.
    /// Planar and crystaline substructures can generate values which are == along
    /// one dimension.
    /// 
    /// <p>To get a good picture in your head, first think about it in one dimension,
    /// points on a number line. The tree just partitions the points.
    /// Now think about 2 dimensions. The first node of the tree splits the plane
    /// into two rectangles along the x dimension. The second level of the tree
    /// splits the subplanes (independently) along the y dimension into smaller
    /// rectangles. The third level splits along the x dimension.
    /// In three dimensions, we are doing the same thing, only working with
    /// 3-d boxes.
    /// 
    /// <p>Three enumerators are provided
    /// <ul>
    /// <li>enumNear(Bspt.Tuple center, double distance)<br>
    /// returns all the points contained in of all the boxes which are within
    /// distance from the center.
    /// <li>enumSphere(Bspt.Tuple center, double distance)<br>
    /// returns all the points which are contained within the sphere (inclusive)
    /// defined by center + distance
    /// <li>enumHemiSphere(Bspt.Tuple center, double distance)<br>
    /// same as sphere, but only the points which are greater along the
    /// x dimension
    /// </ul>
    /// 
    /// </summary>
    /// <author>   Miguel Howard
    /// </author>
    /// <cdk.created>  2003-05 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.module>   standard </cdk.module>
    /// <cdk.keyword>  rebonding </cdk.keyword>
    /// <cdk.keyword>  Binary Space Partitioning Tree </cdk.keyword>
    /// <cdk.keyword>  join-the-dots </cdk.keyword>
    public sealed class Bspt
    {

        private const int leafCount = 4;
        private const int stackDepth = 64; /* this corresponds to the max height of the tree */
        internal int dimMax;
        internal Bspt.Element eleRoot;

        /*
        static double distance(int dim, Tuple t1, Tuple t2) {
        return Math.sqrt(distance2(dim, t1, t2));
        }
		
        static double distance2(int dim, Tuple t1, Tuple t2) {
        double distance2 = 0.0;
        while (--dim >= 0) {
        double distT = t1.getDimValue(dim) - t2.getDimValue(dim);
        distance2 += distT*distT;
        }
        return distance2;
        }
        */

        public Bspt(int dimMax)
        {
            this.dimMax = dimMax;
            this.eleRoot = new Leaf();
        }

        public void addTuple(Bspt.Tuple tuple)
        {
            if (!eleRoot.addTuple(tuple))
            {
                eleRoot = new Node(0, dimMax, (Leaf)eleRoot);
                if (!eleRoot.addTuple(tuple))
                    System.Console.Out.WriteLine("Bspt.addTuple() failed");
            }
        }

        public override System.String ToString()
        {
            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
            return eleRoot.ToString();
        }

        public void dump()
        {
            eleRoot.dump(0);
        }

        public IEnumerator enumeration()
        {
            return new EnumerateAll(this);
        }

        internal class EnumerateAll : System.Collections.IEnumerator
        {
            private System.Object tempAuxObj;

            public bool MoveNext()
            {
                bool result = hasMoreElements();
                if (result)
                {
                    tempAuxObj = nextElement();
                }
                return result;
            }

            public void Reset()
            {
                tempAuxObj = null;
            }

            public object Current
            {
                get { return tempAuxObj; }
            }

            internal Node[] stack;
            internal int sp;
            internal int i;
            internal Leaf leaf;

            public EnumerateAll(Bspt bspt)
            {
                stack = new Node[Bspt.stackDepth];
                sp = 0;
                Bspt.Element ele = bspt.eleRoot;
                while (ele is Node)
                {
                    Node node = (Node)ele;
                    if (sp == Bspt.stackDepth)
                        System.Console.Out.WriteLine("Bspt.EnumerateAll tree stack overflow");
                    stack[sp++] = node;
                    ele = node.eleLE;
                }
                leaf = (Leaf)ele;
                i = 0;
            }

            //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.hasMoreElements' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
            public bool hasMoreElements()
            {
                return (i < leaf.count) || (sp > 0);
            }

            //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.nextElement' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
            public System.Object nextElement()
            {
                if (i == leaf.count)
                {
                    //        System.out.println("-->" + stack[sp-1].splitValue);
                    Bspt.Element ele = stack[--sp].eleGE;
                    while (ele is Node)
                    {
                        Node node = (Node)ele;
                        stack[sp++] = node;
                        ele = node.eleLE;
                    }
                    leaf = (Leaf)ele;
                    i = 0;
                }
                return leaf.tuples[i++];
            }
        }

        public IEnumerator enumNear(Bspt.Tuple center, double distance)
        {
            return new EnumerateNear(this, center, distance);
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'EnumerateNear' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        internal class EnumerateNear : System.Collections.IEnumerator
        {
            private object tempAuxObj;

            public bool MoveNext()
            {
                bool result = hasMoreElements();
                if (result)
                {
                    tempAuxObj = nextElement();
                }
                return result;
            }

            public void Reset()
            {
                tempAuxObj = null;
            }

            public object Current
            {
                get { return tempAuxObj; }
            }

            internal Node[] stack;
            internal int sp;
            internal int i;
            internal Leaf leaf;
            internal double distance;
            internal Bspt.Tuple center;

            public EnumerateNear(Bspt bspt, Bspt.Tuple center, double distance)
            {
                this.distance = distance;
                this.center = center;

                stack = new Node[Bspt.stackDepth];
                sp = 0;
                Bspt.Element ele = bspt.eleRoot;
                while (ele is Node)
                {
                    Node node = (Node)ele;
                    if (center.getDimValue(node.dim) - distance <= node.splitValue)
                    {
                        if (sp == Bspt.stackDepth)
                            System.Console.Out.WriteLine("Bspt.EnumerateNear tree stack overflow");
                        stack[sp++] = node;
                        ele = node.eleLE;
                    }
                    else
                    {
                        ele = node.eleGE;
                    }
                }
                leaf = (Leaf)ele;
                i = 0;
            }

            //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.hasMoreElements' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
            public bool hasMoreElements()
            {
                if (i < leaf.count)
                    return true;
                if (sp == 0)
                    return false;
                Bspt.Element ele = stack[--sp];
                while (ele is Node)
                {
                    Node node = (Node)ele;
                    if (center.getDimValue(node.dim) + distance < node.splitValue)
                    {
                        if (sp == 0)
                            return false;
                        ele = stack[--sp];
                    }
                    else
                    {
                        ele = node.eleGE;
                        while (ele is Node)
                        {
                            Node nodeLeft = (Node)ele;
                            stack[sp++] = nodeLeft;
                            ele = nodeLeft.eleLE;
                        }
                    }
                }
                leaf = (Leaf)ele;
                i = 0;
                return true;
            }

            //UPGRADE_NOTE: The equivalent of method 'java.util.Enumeration.nextElement' is not an override method. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1143'"
            public object nextElement()
            {
                return leaf.tuples[i++];
            }
        }

        public EnumerateSphere enumSphere(Bspt.Tuple center, double distance)
        {
            return new EnumerateSphere(this, center, distance, false);
        }

        public EnumerateSphere enumHemiSphere(Bspt.Tuple center, double distance)
        {
            return new EnumerateSphere(this, center, distance, true);
        }

        public class EnumerateSphere : IEnumerator
        {
            private object tempAuxObj;

            public bool MoveNext()
            {
                bool result = hasMoreElements();
                if (result)
                    tempAuxObj = nextElement();
                return result;
            }

            public void Reset()
            {
                tempAuxObj = null;
            }

            public object Current
            {
                get { return tempAuxObj; }
            }

            internal Node[] stack;
            internal int sp;
            internal int i;
            internal Leaf leaf;
            internal double distance;
            internal double distance2;
            internal Bspt.Tuple center;
            internal double[] centerValues;
            internal double foundDistance2_Renamed_Field; // the dist squared of a found Element;

            Bspt bspt;

            // when set, only the hemisphere sphere .GT. or .EQ. the point
            // (on the first dim) is returned
            internal bool tHemisphere;

            public EnumerateSphere(Bspt bspt, Bspt.Tuple center, double distance, bool tHemisphere)
            {
                this.bspt = bspt;
                this.distance = distance;
                this.distance2 = distance * distance;
                this.center = center;
                this.tHemisphere = tHemisphere;
                centerValues = new double[bspt.dimMax];
                for (int dim = bspt.dimMax; --dim >= 0; )
                    centerValues[dim] = center.getDimValue(dim);
                stack = new Node[Bspt.stackDepth];
                sp = 0;
                Bspt.Element ele = bspt.eleRoot;
                while (ele is Node)
                {
                    Node node = (Node)ele;
                    if (center.getDimValue(node.dim) - distance <= node.splitValue)
                    {
                        if (sp == Bspt.stackDepth)
                            System.Console.Out.WriteLine("Bspt.EnumerateSphere tree stack overflow");
                        stack[sp++] = node;
                        ele = node.eleLE;
                    }
                    else
                    {
                        ele = node.eleGE;
                    }
                }
                leaf = (Leaf)ele;
                i = 0;
            }

            private bool isWithin(Bspt.Tuple t)
            {
                double dist2;
                double distT;
                distT = t.getDimValue(0) - centerValues[0];
                if (tHemisphere && distT < 0)
                {
                    return false;
                }
                dist2 = distT * distT;
                if (dist2 > distance2)
                {
                    return false;
                }
                for (int dim = bspt.dimMax; --dim > 0; )
                {
                    distT = t.getDimValue(dim) - centerValues[dim];
                    dist2 += distT * distT;
                    if (dist2 > distance2)
                    {
                        return false;
                    }
                }
                this.foundDistance2_Renamed_Field = dist2;
                return true;
            }

            private bool hasMoreElements()
            {
                while (true)
                {
                    for (; i < leaf.count; ++i)
                        if (isWithin(leaf.tuples[i]))
                            return true;
                    if (sp == 0)
                        return false;
                    Bspt.Element ele = stack[--sp];
                    while (ele is Node)
                    {
                        Node node = (Node)ele;
                        if (center.getDimValue(node.dim) + distance < node.splitValue)
                        {
                            if (sp == 0)
                                return false;
                            ele = stack[--sp];
                        }
                        else
                        {
                            ele = node.eleGE;
                            while (ele is Node)
                            {
                                Node nodeLeft = (Node)ele;
                                stack[sp++] = nodeLeft;
                                ele = nodeLeft.eleLE;
                            }
                        }
                    }
                    leaf = (Leaf)ele;
                    i = 0;
                }
            }

            private object nextElement()
            {
                return leaf.tuples[i++];
            }

            public double foundDistance2()
            {
                return foundDistance2_Renamed_Field;
            }
        }

        public interface Tuple
        {
            double getDimValue(int dim);
        }

        internal interface Element
        {
            bool LeafWithSpace
            {
                get;

            }
            bool addTuple(Bspt.Tuple tuple);
            void dump(int level);
        }

        internal class Node : Bspt.Element
        {
            public bool LeafWithSpace
            {
                get { return false; }
            }

            internal Bspt.Element eleLE;
            internal int dim;
            internal int dimMax;
            internal double splitValue;
            internal Bspt.Element eleGE;

            public Node(int dim, int dimMax, Leaf leafLE)
            {
                this.eleLE = leafLE;
                this.dim = dim;
                this.dimMax = dimMax;
                this.splitValue = leafLE.getSplitValue(dim);
                this.eleGE = new Leaf(leafLE, dim, splitValue);
            }

            public bool addTuple(Bspt.Tuple tuple)
            {
                if (tuple.getDimValue(dim) < splitValue)
                {
                    if (eleLE.addTuple(tuple))
                        return true;
                    eleLE = new Node((dim + 1) % dimMax, dimMax, (Leaf)eleLE);
                    return eleLE.addTuple(tuple);
                }
                if (tuple.getDimValue(dim) > splitValue)
                {
                    if (eleGE.addTuple(tuple))
                        return true;
                    eleGE = new Node((dim + 1) % dimMax, dimMax, (Leaf)eleGE);
                    return eleGE.addTuple(tuple);
                }
                if (eleLE.LeafWithSpace)
                    eleLE.addTuple(tuple);
                else if (eleGE.LeafWithSpace)
                    eleGE.addTuple(tuple);
                else if (eleLE is Node)
                    eleLE.addTuple(tuple);
                else if (eleGE is Node)
                    eleGE.addTuple(tuple);
                else
                {
                    eleLE = new Node((dim + 1) % dimMax, dimMax, (Leaf)eleLE);
                    return eleLE.addTuple(tuple);
                }
                return true;
            }

            public override System.String ToString()
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                return eleLE.ToString() + dim + ":" + splitValue + "\n" + eleGE.ToString();
            }

            public void dump(int level)
            {
                System.Console.Out.WriteLine("");
                eleLE.dump(level + 1);
                for (int i = 0; i < level; ++i)
                    System.Console.Out.Write("-");
                System.Console.Out.WriteLine(">" + splitValue);
                eleGE.dump(level + 1);
            }
        }

        internal class Leaf : Bspt.Element
        {
            public bool LeafWithSpace
            {
                get { return count < Bspt.leafCount; }
            }

            internal int count;
            internal Bspt.Tuple[] tuples;

            public Leaf()
            {
                count = 0;
                tuples = new Bspt.Tuple[Bspt.leafCount];
            }

            public Leaf(Leaf leaf, int dim, double splitValue)
                : this()
            {
                for (int i = Bspt.leafCount; --i >= 0; )
                {
                    Bspt.Tuple tuple = leaf.tuples[i];
                    double value_Renamed = tuple.getDimValue(dim);
                    if (value_Renamed > splitValue || (value_Renamed == splitValue && ((i & 1) == 1)))
                    {
                        leaf.tuples[i] = null;
                        tuples[count++] = tuple;
                    }
                }
                int dest = 0;
                for (int src = 0; src < Bspt.leafCount; ++src)
                    if (leaf.tuples[src] != null)
                        leaf.tuples[dest++] = leaf.tuples[src];
                leaf.count = dest;
                if (count == 0)
                    tuples[Bspt.leafCount] = null; // explode
            }

            public double getSplitValue(int dim)
            {
                if (count != Bspt.leafCount)
                    tuples[Bspt.leafCount] = null;
                return (tuples[0].getDimValue(dim) + tuples[Bspt.leafCount - 1].getDimValue(dim)) / 2;
            }

            public override System.String ToString()
            {
                return "leaf:" + count + "\n";
            }

            public bool addTuple(Bspt.Tuple tuple)
            {
                if (count == Bspt.leafCount)
                    return false;
                tuples[count++] = tuple;
                return true;
            }

            public void dump(int level)
            {
                //for (int i = 0; i < count; ++i)
                //{
                //    Bspt.Tuple t = tuples[i];
                //    for (int j = 0; j < level; ++j)
                //        System.Console.Out.Write(".");
                //    for (int dim = 0; dim < Enclosing_Instance.dimMax - 1; ++dim)
                //        System.Console.Out.Write("" + t.getDimValue(dim) + ",");
                //    System.Console.Out.WriteLine("" + t.getDimValue(Enclosing_Instance.dimMax - 1));
                //}
            }
        }
    }
}
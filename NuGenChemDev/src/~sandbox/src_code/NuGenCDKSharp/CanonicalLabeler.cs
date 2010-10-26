/* $RCSfile$
* $Author: egonw $
* $Date: 2006-05-11 22:05:31 +0200 (Thu, 11 May 2006) $
* $Revision: 6236 $
*
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) Project
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
using Org.OpenScience.CDK.Smiles;
using Support;

namespace Org.OpenScience.CDK.Graph.Invariant
{
    /// <summary> Canonically lables an atom container implementing
    /// the algorithm published in David Weininger et.al. {@cdk.cite WEI89}.
    /// The Collections.sort() method uses a merge sort which is 
    /// stable and runs in n log(n).
    /// 
    /// </summary>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <author>    Oliver Horlacher <oliver.horlacher@therastrat.com>
    /// </author>
    /// <cdk.created>   2002-02-26 </cdk.created>
    /// <summary> 
    /// </summary>
    /// <cdk.keyword>  canonicalization </cdk.keyword>
    public class CanonicalLabeler
    {
        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassComparator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AnonymousClassComparator : System.Collections.IComparer
        {
            public AnonymousClassComparator(CanonicalLabeler enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }
            private void InitBlock(CanonicalLabeler enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private CanonicalLabeler enclosingInstance;
            public CanonicalLabeler Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public virtual int Compare(System.Object o1, System.Object o2)
            {
                return (int)(((InvPair)o1).Curr - ((InvPair)o2).Curr);
            }
        }
        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassComparator1' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AnonymousClassComparator1 : System.Collections.IComparer
        {
            public AnonymousClassComparator1(CanonicalLabeler enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }
            private void InitBlock(CanonicalLabeler enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private CanonicalLabeler enclosingInstance;
            public CanonicalLabeler Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public virtual int Compare(System.Object o1, System.Object o2)
            {
                return (int)(((InvPair)o1).Last - ((InvPair)o2).Last);
            }
        }

        public CanonicalLabeler()
        {
        }

        /// <summary> Canonicaly label the fragment.
        /// This is an implementation of the algorithm published in
        /// David Weininger et.al. {@cdk.cite WEI89}.
        /// 
        /// <p>The Collections.sort() method uses a merge sort which is 
        /// stable and runs in n log(n).
        /// 
        /// <p>It is assumed that a chemicaly valid AtomContainer is provided: 
        /// this method does not check
        /// the correctness of the AtomContainer. Negative H counts will 
        /// cause a NumberFormatException to be thrown.
        /// </summary>
        //UPGRADE_NOTE: Synchronized keyword was removed from method 'canonLabel'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
        public virtual void canonLabel(IAtomContainer atomContainer)
        {
            lock (this)
            {
                if (atomContainer.AtomCount == 0)
                    return;
                System.Collections.ArrayList vect = createInvarLabel(atomContainer);
                step3(vect, atomContainer);
            }
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step2(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            primeProduct(v, atoms);
            step3(v, atoms);
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step3(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            sortVector(v);
            step4(v, atoms);
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step4(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            rankVector(v);
            step5(v, atoms);
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step5(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            if (!isInvPart(v))
                step2(v, atoms);
            else
                step6(v, atoms);
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step6(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            //On first pass save, partitioning as symmetry classes.
            step7(v, atoms);
        }

        /// <param name="v">the invariance pair vector
        /// </param>
        private void step7(System.Collections.ArrayList v, IAtomContainer atoms)
        {
            if (((InvPair)v[v.Count - 1]).Curr < v.Count)
            {
                breakTies(v);
                step2(v, atoms);
            }
            System.Collections.IEnumerator it = v.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                ((InvPair)it.Current).comit();
            }
        }

        /// <summary> Create initial invariant labeling corresponds to step 1
        /// 
        /// </summary>
        /// <returns> Vector containting the
        /// </returns>
        private System.Collections.ArrayList createInvarLabel(IAtomContainer atomContainer)
        {
            IAtom[] atoms = atomContainer.Atoms;
            IAtom a;
            System.Text.StringBuilder inv;
            System.Collections.ArrayList vect = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int x = 0; x < atoms.Length; x++)
            {
                a = atoms[x];
                inv = new System.Text.StringBuilder();
                inv.Append(atomContainer.getConnectedAtoms(a).Length + a.getHydrogenCount()); //Num connections
                inv.Append(atomContainer.getConnectedAtoms(a).Length); //Num of non H bonds
                inv.Append(a.AtomicNumber); //Atomic number
                if (a.getCharge() < 0)
                    //Sign of charge
                    inv.Append(1);
                else
                    inv.Append(0); //Absolute charge
                inv.Append((int)System.Math.Abs(a.getFormalCharge())); //Hydrogen count
                inv.Append(a.getHydrogenCount());
                vect.Add(new InvPair(System.Int64.Parse(inv.ToString()), a));
            }
            return vect;
        }

        /// <summary> Calculates the product of the neighbouring primes.
        /// 
        /// </summary>
        /// <param name="v">the invariance pair vector
        /// </param>
        private void primeProduct(System.Collections.ArrayList v, IAtomContainer atomContainer)
        {
            System.Collections.IEnumerator it = v.GetEnumerator();
            System.Collections.IEnumerator n;
            InvPair inv;
            IAtom a;
            long summ;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                inv = (InvPair)it.Current;
                System.Collections.IList neighbour = atomContainer.getConnectedAtomsVector(inv.Atom);
                n = neighbour.GetEnumerator();
                summ = 1;
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (n.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    a = (IAtom)n.Current;
                    int next = ((InvPair)a.getProperty(InvPair.INVARIANCE_PAIR)).getPrime();
                    summ = summ * next;
                }
                inv.Last = inv.Curr;
                inv.Curr = summ;
            }
        }

        /// <summary> Sorts the vector according to the current invariance, corresponds to step 3
        /// 
        /// </summary>
        /// <param name="v">the invariance pair vector
        /// </param>
        /// <cdk.todo>     can this be done in one loop? </cdk.todo>
        private void sortVector(System.Collections.ArrayList v)
        {
            SupportClass.CollectionsSupport.Sort(v, new AnonymousClassComparator(this));
            SupportClass.CollectionsSupport.Sort(v, new AnonymousClassComparator1(this));
        }

        /// <summary> Rank atomic vector, corresponds to step 4.
        /// 
        /// </summary>
        /// <param name="v">the invariance pair vector
        /// </param>
        private void rankVector(System.Collections.ArrayList v)
        {
            int num = 1;
            int[] temp = new int[v.Count];
            InvPair last = (InvPair)v[0];
            System.Collections.IEnumerator it = v.GetEnumerator();
            InvPair curr;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (int x = 0; it.MoveNext(); x++)
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                curr = (InvPair)it.Current;
                if (!last.Equals(curr))
                {
                    num++;
                }
                temp[x] = num;
                last = curr;
            }
            it = v.GetEnumerator();
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (int x = 0; it.MoveNext(); x++)
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                curr = (InvPair)it.Current;
                curr.Curr = temp[x];
                curr.setPrime();
            }
        }

        /// <summary> Checks to see if the vector is invariantely partitioned
        /// 
        /// </summary>
        /// <param name="v">the invariance pair vector
        /// </param>
        /// <returns> true if the vector is invariantely partitioned, false otherwise
        /// </returns>
        private bool isInvPart(System.Collections.ArrayList v)
        {
            if (((InvPair)v[v.Count - 1]).Curr == v.Count)
                return true;
            System.Collections.IEnumerator it = v.GetEnumerator();
            InvPair curr;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            while (it.MoveNext())
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                curr = (InvPair)it.Current;
                if (curr.Curr != curr.Last)
                    return false;
            }
            return true;
        }

        /// <summary> Break ties. Corresponds to step 7
        /// 
        /// </summary>
        /// <param name="v">the invariance pair vector
        /// </param>
        private void breakTies(System.Collections.ArrayList v)
        {
            System.Collections.IEnumerator it = v.GetEnumerator();
            InvPair curr;
            InvPair last = null;
            int tie = 0;
            bool found = false;
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (int x = 0; it.MoveNext(); x++)
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                curr = (InvPair)it.Current;
                curr.Curr = curr.Curr * 2;
                curr.setPrime();
                if (x != 0 && !found && curr.Curr == last.Curr)
                {
                    tie = x - 1;
                    found = true;
                }
                last = curr;
            }
            curr = (InvPair)v[tie];
            curr.Curr = curr.Curr - 1;
            curr.setPrime();
        }
    }
}
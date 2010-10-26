/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-05-03 23:24:28 +0200 (Wed, 03 May 2006) $    
* $Revision: 6152 $
*
* Copyright (C) 2001-2006  The Chemistry Development Kit (CDK) project
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

namespace Org.OpenScience.CDK.Graph
{
    /// <summary> Spanning tree of a molecule.
    /// Used to discover the number of cyclic bonds in order to prevent the 
    /// inefficient AllRingsFinder to run for too long.
    /// 
    /// </summary>
    /// <author>    Nina Jeliazkova
    /// </author>
    /// <cdk.todo>  junit test of this </cdk.todo>
    /// <summary> 
    /// </summary>
    /// <cdk.module>   standard </cdk.module>
    /// <cdk.dictref>  blue-obelisk:graphSpanningTree </cdk.dictref>
    public class SpanningTree
    {
        virtual public bool Disconnected
        {
            get
            {
                return disconnected;
            }

        }
        virtual public IRingSet BasicRings
        {
            get
            {
                IRingSet ringset = molecule.Builder.newRingSet();
                IAtomContainer spt = getSpanningTree();
                for (int i = 0; i < E; i++)
                    if (!bondsInTree[i])
                        ringset.addAtomContainer(getRing(spt, molecule.getBondAt(i)));
                spt = null;
                return ringset;
            }

        }
        virtual public IRingSet AllRings
        {
            get
            {
                IRingSet ringset = BasicRings;
                IRing newring = null;
                //System.out.println("Rings "+ringset.size());

                int nBasicRings = ringset.AtomContainerCount;
                for (int i = 0; i < nBasicRings; i++)
                    getBondsInRing(molecule, (IRing)ringset.getAtomContainer(i), cb[i]);



                for (int i = 0; i < nBasicRings; i++)
                {
                    for (int j = i + 1; j < nBasicRings; j++)
                    {
                        //System.out.println("combining rings "+(i+1)+","+(j+1));
                        newring = combineRings(ringset, i, j);
                        //newring = combineRings((Ring)ringset.get(i),(Ring)ringset.get(j));
                        if (newring != null)
                            ringset.addAtomContainer(newring);
                    }
                }

                return ringset;
            }

        }
        virtual public int SpanningTreeSize
        {
            get
            {
                return sptSize;
            }

        }
        /// <returns> Returns the bondsAcyclicCount.
        /// </returns>
        virtual public int BondsAcyclicCount
        {
            get
            {
                return bondsAcyclicCount;
            }

        }
        /// <returns> Returns the bondsCyclicCount.
        /// </returns>
        virtual public int BondsCyclicCount
        {
            get
            {
                return bondsCyclicCount;
            }

        }
        private int[] parent = null;
        private int[][] cb = null;

        protected internal bool[] bondsInTree;

        private int sptSize = 0;
        private int edrSize = 0;

        private int bondsAcyclicCount = 0, bondsCyclicCount = 0;

        private IAtomContainer molecule = null;
        private int E = 0, V = 0;
        private bool disconnected;
        /// <summary> </summary>
        public SpanningTree(IAtomContainer atomContainer)
            : base()
        {
            buildSpanningTree(atomContainer);
        }

        public SpanningTree()
            : base()
        {
        }
        public virtual void clear()
        {
            molecule = null;
            cb = null;
            parent = null;
            sptSize = 0;
            edrSize = 0;
            bondsAcyclicCount = 0;
            bondsCyclicCount = 0;
            bondsInTree = null;
            E = 0;
            V = 0;
            disconnected = false;
        }
        private bool fastfind(int v1, int v2, bool union)
        {
            int i = v1; while (parent[i] > 0)
                i = parent[i];
            int j = v2; while (parent[j] > 0)
                j = parent[j];
            int t;
            while (parent[v1] > 0)
            {
                t = v1; v1 = parent[v1]; parent[t] = i;
            }
            while (parent[v2] > 0)
            {
                t = v2; v2 = parent[v2]; parent[t] = j;
            }
            if (union && (i != j))
            {
                if (parent[j] < parent[i])
                {
                    parent[j] = parent[j] + parent[i] - 1;
                    parent[i] = j;
                }
                else
                {
                    parent[i] = parent[i] + parent[j] - 1;
                    parent[j] = i;
                }
            }
            return (i != j);
        }

        private void fastFindInit(int V)
        {
            parent = new int[V + 1];
            for (int i = 1; i <= V; i++)
            {
                parent[i] = 0;
            }
        }


        /// <summary> Kruskal algorithm</summary>
        /// <param name="atomContainer">
        /// </param>
        public virtual void buildSpanningTree(IAtomContainer atomContainer)
        {
            disconnected = false;
            molecule = atomContainer;

            V = atomContainer.AtomCount;
            E = atomContainer.getBondCount();

            sptSize = 0; edrSize = 0;
            fastFindInit(V);
            for (int i = 0; i < V; i++)
            {
                (atomContainer.getAtomAt(i)).setProperty("ST_ATOMNO", System.Convert.ToString(i + 1));
            }
            IBond bond;
            int v1, v2;
            bondsInTree = new bool[E];

            for (int b = 0; b < E; b++)
            {
                bondsInTree[b] = false;
                bond = atomContainer.getBondAt(b);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                v1 = System.Int32.Parse((bond.getAtomAt(0)).getProperty("ST_ATOMNO").ToString());
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                v2 = System.Int32.Parse((bond.getAtomAt(1)).getProperty("ST_ATOMNO").ToString());
                //this below is a little bit  slower
                //v1 = atomContainer.getAtomNumber(bond.getAtomAt(0))+1; 
                //v2 = atomContainer.getAtomNumber(bond.getAtomAt(1))+1;
                if (fastfind(v1, v2, true))
                {
                    bondsInTree[b] = true;
                    sptSize++;
                    //System.out.println("ST : includes bond between atoms "+v1+","+v2);
                }
                if (sptSize >= (V - 1))
                    break;
            }
            // if atomcontainer is connected then the number of bonds in the spanning tree = (No atoms-1)
            //i.e.  edgesRings = new Bond[E-V+1];
            //but to hold all bonds if atomContainer was disconnected then  edgesRings = new Bond[E-sptSize]; 
            if (sptSize != (V - 1))
                disconnected = true;
            for (int b = 0; b < E; b++)
                if (!bondsInTree[b])
                {
                    //			edgesRings[edrSize] = atomContainer.getBondAt(b);
                    edrSize++;
                }
            cb = new int[edrSize][];
            for (int i2 = 0; i2 < edrSize; i2++)
            {
                cb[i2] = new int[E];
            }
            for (int i = 0; i < edrSize; i++)
                for (int a = 0; a < E; a++)
                    cb[i][a] = 0;
        }
        public virtual IAtomContainer getSpanningTree()
        {
            IAtomContainer ac = molecule.Builder.newAtomContainer();
            for (int a = 0; a < V; a++)
                ac.addAtom(molecule.getAtomAt(a));
            for (int b = 0; b < E; b++)
                if (bondsInTree[b])
                    ac.addBond(molecule.getBondAt(b));
            return ac;
        }

        public static void resetFlags(IAtomContainer ac)
        {
            for (int f = 0; f < ac.AtomCount; f++)
            {
                ac.getAtomAt(f).setFlag(CDKConstants.VISITED, false);
            }
            for (int f = 0; f < ac.ElectronContainerCount; f++)
            {
                ac.getElectronContainerAt(f).setFlag(CDKConstants.VISITED, false);
            }
        }
        public virtual IAtomContainer getPath(IAtomContainer spt, IAtom a1, IAtom a2)
        {

            IAtomContainer path = spt.Builder.newAtomContainer();
            PathTools.resetFlags(spt);
            path.addAtom(a1);
            PathTools.depthFirstTargetSearch(spt, a1, a2, path);
            return path;
        }
        private IRing getRing(IAtomContainer spt, IBond bond)
        {
            IRing ring = spt.Builder.newRing();
            PathTools.resetFlags(spt);
            ring.addAtom(bond.getAtomAt(0));
            PathTools.depthFirstTargetSearch(spt, bond.getAtomAt(0), bond.getAtomAt(1), ring);
            ring.addBond(bond);
            return ring;
        }
        private void getBondsInRing(IAtomContainer mol, IRing ring, int[] bonds)
        {
            for (int i = 0; i < ring.getBondCount(); i++)
            {
                int m = mol.getBondNumber(ring.getBondAt(i));
                bonds[m] = 1;
            }
        }
        public virtual void identifyBonds()
        {
            IAtomContainer spt = getSpanningTree();
            IRing ring;
            int nBasicRings = 0;
            for (int i = 0; i < E; i++)
                if (!bondsInTree[i])
                {
                    ring = getRing(spt, molecule.getBondAt(i));
                    for (int b = 0; b < ring.getBondCount(); b++)
                    {
                        int m = molecule.getBondNumber(ring.getBondAt(b));
                        cb[nBasicRings][m] = 1;
                    }
                    nBasicRings++;
                }
            spt = null; ring = null;
            bondsAcyclicCount = 0; bondsCyclicCount = 0;
            for (int i = 0; i < E; i++)
            {
                int s = 0;
                for (int j = 0; j < nBasicRings; j++)
                    s += cb[j][i];
                switch (s)
                {

                    //acyclic bond
                    case (0):
                        {
                            bondsAcyclicCount++; break;
                        }

                    case (1):
                        {
                            bondsCyclicCount++; break;
                        }

                    default:
                        {
                            bondsCyclicCount++;
                        }
                        break;

                }
            }
        }
        public virtual void printAtoms(IAtomContainer ac)
        {
            for (int i = 0; i < ac.AtomCount; i++)
            {
                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                System.Console.Out.Write(ac.getAtomAt(i).getProperty("ST_ATOMNO").ToString() + ",");
            }
        }
        private IRing combineRings(IRingSet ringset, int i, int j)
        {
            int c = 0;
            for (int b = 0; b < cb[i].Length; b++)
            {
                c = cb[i][b] + cb[j][b];
                if (c > 1)
                    break; //at least one common bond
            }
            if (c < 2)
                return null;
            IRing ring = molecule.Builder.newRing();
            IRing ring1 = (IRing)ringset.getAtomContainer(i);
            IRing ring2 = (IRing)ringset.getAtomContainer(j);
            for (int b = 0; b < cb[i].Length; b++)
            {
                c = cb[i][b] + cb[j][b];
                if ((c == 1) && (cb[i][b] == 1))
                    ring.addBond(molecule.getBondAt(b));
                else if ((c == 1) && (cb[j][b] == 1))
                    ring.addBond(molecule.getBondAt(b));
            }
            for (int a = 0; a < ring1.AtomCount; a++)
                ring.addAtom(ring1.getAtomAt(a));
            for (int a = 0; a < ring2.AtomCount; a++)
                ring.addAtom(ring2.getAtomAt(a));

            return ring;
        }
    }
}
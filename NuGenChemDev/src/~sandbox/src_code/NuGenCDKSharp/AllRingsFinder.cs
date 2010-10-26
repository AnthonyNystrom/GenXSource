/*
*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-06-17 22:02:47 +0200 (Sat, 17 Jun 2006) $
*  $Revision: 6478 $
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
using NuGenCDKSharp;
using Org.OpenScience.CDK.Interfaces;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.RingSearch
{
    /// <summary>  Finds the Set of all Rings. This is an implementation of the algorithm
    /// published in {@cdk.cite HAN96}. Some of the comments refer to pseudo code
    /// fragments listed in this article. The concept is that a regular molecular
    /// graph is converted into a path graph first, i.e. a graph where the edges are
    /// actually pathes, i.e. can list several nodes that are implicitly connecting
    /// the two nodes between the path is formed. The pathes that join one endnode
    /// are step by step fused and the joined nodes deleted from the pathgraph. What
    /// remains is a graph of pathes that have the same start and endpoint and are
    /// thus rings.
    /// 
    /// <p><b>WARNING</b>: This class has now a timeout of 5 seconds, after which it aborts
    /// its ringsearch. The timeout value can be customized by the setTimeout()
    /// method of this class.  
    /// 
    /// </summary>
    /// <author>         steinbeck
    /// </author>
    /// <cdk.created>        3. Juni 2005 </cdk.created>
    /// <cdk.module>     standard </cdk.module>
    public class AllRingsFinder
    {
        public bool debug = false;
        private long timeout = 5000;
        private long startTime;

        /*
        *  used for storing the original atomContainer for
        *  reference purposes (printing)
        */
        internal IAtomContainer originalAc = null;
        internal System.Collections.ArrayList newPathes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        internal System.Collections.ArrayList potentialRings = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        internal System.Collections.ArrayList removePathes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));


        /// <summary>  Returns a ringset containing all rings in the given AtomContainer
        /// 
        /// </summary>
        /// <param name="atomContainer">    The AtomContainer to be searched for rings
        /// </param>
        /// <returns>                   A RingSet with all rings in the AtomContainer
        /// </returns>
        /// <exception cref="CDKException"> An exception thrown if something goes wrong or if the timeout limit is reached
        /// </exception>
        public virtual IRingSet findAllRings(IAtomContainer atomContainer)
        {
            startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            SpanningTree spanningTree;
            try
            {
                spanningTree = new SpanningTree((IAtomContainer)atomContainer.Clone());
            }
            //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
            catch (System.Exception e)
            {
                throw new CDKException("Could not clone IAtomContainer!", e);
            }
            spanningTree.identifyBonds();
            if (spanningTree.BondsCyclicCount < 37)
            {
                findAllRings(atomContainer, false);
            }
            return findAllRings(atomContainer, true);
        }


        /// <summary>  Fings the set of all rings in a molecule
        /// 
        /// </summary>
        /// <param name="atomContainer">    the molecule to be searched for rings
        /// </param>
        /// <param name="useSSSR">          use the SSSRFinder & RingPartitioner as pre-filter
        /// </param>
        /// <returns>                   a RingSet containing the rings in molecule
        /// </returns>
        /// <exception cref="CDKException"> An exception thrown if something goes wrong or if the timeout limit is reached
        /// </exception>
        public virtual IRingSet findAllRings(IAtomContainer atomContainer, bool useSSSR)
        {
            if (startTime == 0)
            {
                startTime = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            }
            System.Collections.ArrayList pathes = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            IRingSet ringSet = atomContainer.Builder.newRingSet();
            IAtomContainer ac = atomContainer.Builder.newAtomContainer();
            originalAc = atomContainer;
            ac.add(atomContainer);
            if (debug)
            {
                System.Console.Out.WriteLine("AtomCount before removal of aliphatic atoms: " + ac.AtomCount);
            }
            removeAliphatic(ac);
            if (debug)
            {
                System.Console.Out.WriteLine("AtomCount after removal of aliphatic atoms: " + ac.AtomCount);
            }
            if (useSSSR)
            {
                SSSRFinder sssrf = new SSSRFinder(atomContainer);
                IRingSet sssr = sssrf.findSSSR();
                System.Collections.ArrayList ringSets = RingPartitioner.partitionRings(sssr);

                for (int r = 0; r < ringSets.Count; r++)
                {
                    IAtomContainer tempAC = RingPartitioner.convertToAtomContainer((IRingSet)ringSets[r]);

                    doSearch(tempAC, pathes, ringSet);
                }
            }
            else
            {
                doSearch(ac, pathes, ringSet);
            }
            atomContainer.setProperty(CDKConstants.ALL_RINGS, ringSet);
            return ringSet;
        }


        /// <summary>  Description of the Method
        /// 
        /// </summary>
        /// <param name="ac">               The AtomContainer to be searched
        /// </param>
        /// <param name="pathes">           A vectoring storing all the pathes
        /// </param>
        /// <param name="ringSet">          A ringset to be extended while we search
        /// </param>
        /// <exception cref="CDKException"> An exception thrown if something goes wrong or if the timeout limit is reached
        /// </exception>
        private void doSearch(IAtomContainer ac, System.Collections.ArrayList pathes, IRingSet ringSet)
        {
            IAtom atom = null;
            /*
            *  First we convert the molecular graph into a a path graph by
            *  creating a set of two membered pathes from all the bonds in the molecule
            */
            initPathGraph(ac, pathes);
            if (debug)
            {
                System.Console.Out.WriteLine("BondCount: " + ac.getBondCount() + ", PathCount: " + pathes.Count);
            }
            do
            {
                atom = selectAtom(ac);
                if (atom != null)
                {
                    remove(atom, ac, pathes, ringSet);
                }
            }
            while (pathes.Count > 0 && atom != null);
            if (debug)
            {
                System.Console.Out.WriteLine("pathes.size(): " + pathes.Count);
            }
            if (debug)
            {
                System.Console.Out.WriteLine("ringSet.size(): " + ringSet.AtomContainerCount);
            }
        }


        /// <summary>  Removes all external aliphatic chains by chopping them off from the
        /// ends
        /// 
        /// </summary>
        /// <param name="ac">               The AtomContainer to work with
        /// </param>
        /// <exception cref="CDKException"> An exception thrown if something goes wrong or if the timeout limit is reached
        /// </exception>
        private void removeAliphatic(IAtomContainer ac)
        {
            bool removedSomething;
            IAtom atom = null;
            do
            {
                removedSomething = false;
                //UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
                for (System.Collections.IEnumerator e = ac.atoms(); e.MoveNext(); )
                {
                    //UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
                    atom = (IAtom)e.Current;
                    if (ac.getBondCount(atom) == 1)
                    {
                        ac.removeAtomAndConnectedElectronContainers(atom);
                        removedSomething = true;
                    }
                }
            }
            while (removedSomething);
        }


        /// <summary>  Removes an atom from the AtomContainer under certain conditions.
        /// See {@cdk.cite HAN96} for details
        /// 
        /// 
        /// </summary>
        /// <param name="atom">             The atom to be removed
        /// </param>
        /// <param name="ac">               The AtomContainer to work on
        /// </param>
        /// <param name="pathes">           The pathes to manipulate
        /// </param>
        /// <param name="rings">            The ringset to be extended
        /// </param>
        /// <exception cref="CDKException"> Thrown if something goes wrong or if the timeout is exceeded
        /// </exception>
        private void remove(IAtom atom, IAtomContainer ac, System.Collections.ArrayList pathes, IRingSet rings)
        {
            Path path1 = null;
            Path path2 = null;
            Path union = null;
            int intersectionSize = 0;
            newPathes.Clear();
            removePathes.Clear();
            potentialRings.Clear();
            if (debug)
            {
                System.Console.Out.WriteLine("*** Removing atom " + originalAc.getAtomNumber(atom) + " ***");
            }

            for (int i = 0; i < pathes.Count; i++)
            {
                path1 = (Path)pathes[i];
                if (path1[0] == atom || path1[path1.Count - 1] == atom)
                {
                    for (int j = i + 1; j < pathes.Count; j++)
                    {
                        //System.out.print(".");
                        path2 = (Path)pathes[j];
                        if (path2[0] == atom || path2[path2.Count - 1] == atom)
                        {
                            intersectionSize = path1.getIntersectionSize(path2);
                            if (intersectionSize < 3)
                            {
                                //if (debug) System.out.println("Joining " + path1.toString(originalAc) + " and " + path2.toString(originalAc));
                                union = Path.join(path1, path2, atom);
                                if (intersectionSize == 1)
                                {
                                    newPathes.Add(union);
                                }
                                else
                                {
                                    potentialRings.Add(union);
                                }
                                //if (debug) System.out.println("Intersection Size: " + intersectionSize);
                                //if (debug) System.out.println("Union: " + union.toString(originalAc));
                                /*
                                *  Now we know that path1 and
                                *  path2 share the Atom atom.
                                */
                                removePathes.Add(path1);
                                removePathes.Add(path2);
                            }
                        }
                        checkTimeout();
                    }
                }
            }
            for (int f = 0; f < removePathes.Count; f++)
            {
                pathes.Remove(removePathes[f]);
            }
            for (int f = 0; f < newPathes.Count; f++)
            {
                pathes.Add(newPathes[f]);
            }
            detectRings(potentialRings, rings, originalAc);
            ac.removeAtomAndConnectedElectronContainers(atom);
            if (debug)
            {
                System.Console.Out.WriteLine("\n" + pathes.Count + " pathes and " + ac.AtomCount + " atoms left.");
            }
        }


        /// <summary>  Checks the pathes if a ring has been found
        /// 
        /// </summary>
        /// <param name="pathes">  The pathes to check for rings
        /// </param>
        /// <param name="ringSet"> The ringset to add the detected rings to
        /// </param>
        /// <param name="ac">      The AtomContainer with the original structure
        /// </param>
        private void detectRings(System.Collections.ArrayList pathes, IRingSet ringSet, IAtomContainer ac)
        {
            Path path = null;
            IRing ring = null;
            IBond bond = null;
            for (int f = 0; f < pathes.Count; f++)
            {
                path = (Path)pathes[f];
                if (path.Count > 3 && path[path.Count - 1] == path[0])
                {
                    if (debug)
                    {
                        System.Console.Out.WriteLine("Removing path " + path.toString(originalAc) + " which is a ring.");
                    }
                    path.RemoveAt(0);
                    ring = ac.Builder.newRing();
                    for (int g = 0; g < path.Count; g++)
                    {
                        ring.addAtom((IAtom)path[g]);
                    }
                    IBond[] bonds = ac.Bonds;
                    for (int g = 0; g < bonds.Length; g++)
                    {
                        bond = bonds[g];
                        if (ring.contains(bond.getAtomAt(0)) && ring.contains(bond.getAtomAt(1)))
                        {
                            ring.addBond(bond);
                        }
                    }
                    ringSet.addAtomContainer(ring);
                }
            }
        }


        /// <summary>  Initialized the path graph
        /// See {@cdk.cite HAN96} for details
        /// 
        /// </summary>
        /// <param name="ac">     The AtomContainer with the original structure
        /// </param>
        /// <param name="pathes"> The pathes to initialize
        /// </param>
        private void initPathGraph(IAtomContainer ac, System.Collections.ArrayList pathes)
        {
            IBond bond = null;
            Path path = null;
            IBond[] bonds = ac.Bonds;
            for (int f = 0; f < bonds.Length; f++)
            {
                bond = bonds[f];
                path = new Path(bond.getAtomAt(0), bond.getAtomAt(1));
                pathes.Add(path);
                if (debug)
                {
                    System.Console.Out.WriteLine("initPathGraph: " + path.toString(originalAc));
                }
            }
        }


        /// <summary>  Selects an optimal atom for removal
        /// See {@cdk.cite HAN96} for details
        /// 
        /// </summary>
        /// <param name="ac"> The AtomContainer to search
        /// </param>
        /// <returns>     The selected Atom
        /// </returns>
        private IAtom selectAtom(IAtomContainer ac)
        {
            int minDegree = 999;
            // :-)
            int degree = minDegree;
            IAtom minAtom = null;
            IAtom atom = null;
            for (int f = 0; f < ac.AtomCount; f++)
            {
                atom = ac.getAtomAt(f);
                degree = ac.getBondCount(atom);

                if (degree < minDegree)
                {
                    minAtom = atom;
                    minDegree = degree;
                }
            }

            return minAtom;
        }


        /// <summary>  Checks if the timeout has been reached and throws an 
        /// exception if so. This is used to prevent this AllRingsFinder
        /// to run for ages in certain rare cases with ring systems of
        /// large size or special topology.
        /// 
        /// </summary>
        /// <exception cref="CDKException"> The exception thrown in case of hitting the timeout
        /// </exception>
        public virtual void checkTimeout()
        {
            long time = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            if (time - startTime > timeout)
            {
                throw new CDKException("Timeout for AllringsFinder exceeded");
            }
        }


        /// <summary>  Sets the timeout value in milliseconds of the AllRingsFinder object
        /// This is used to prevent this AllRingsFinder
        /// to run for ages in certain rare cases with ring systems of
        /// large size or special topology
        /// 
        /// </summary>
        /// <param name="timeout"> The new timeout value
        /// </param>
        /// <returns> a reference to the instance this method was called for
        /// </returns>
        public virtual AllRingsFinder setTimeout(long timeout)
        {
            this.timeout = timeout;
            return this;
        }


        /// <summary>  Gets the timeout values in milliseconds of the AllRingsFinder object
        /// 
        /// </summary>
        /// <returns>    The timeout value
        /// </returns>
        public virtual long getTimeout()
        {
            return timeout;
        }
    }
}
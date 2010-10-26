/* $RCSfile$
* $Author: egonw $    
* $Date: 2006-05-11 22:05:31 +0200 (Thu, 11 May 2006) $    
* $Revision: 6236 $
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
using Org.OpenScience.CDK.Graph.Matrix;

namespace Org.OpenScience.CDK.Graph
{
    /// <summary> Tools class with methods for handling molecular graphs.
    /// 
    /// </summary>
    /// <author>  steinbeck
    /// </author>
    /// <cdk.module>  standard </cdk.module>
    /// <cdk.created>  2001-06-17 </cdk.created>
    public class PathTools
    {
        public const bool debug = false;

        /// <summary> Sums up the columns in a 2D int matrix
        /// 
        /// </summary>
        /// <param name="apsp">The 2D int matrix
        /// </param>
        /// <returns> A 1D matrix containing the column sum of the 2D matrix
        /// </returns>
        public static int[] getInt2DColumnSum(int[][] apsp)
        {
            int[] colSum = new int[apsp.Length];
            int sum;
            for (int i = 0; i < apsp.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < apsp.Length; j++)
                {
                    sum += apsp[i][j];
                }
                colSum[i] = sum;
            }
            return colSum;
        }


        /// <summary> All-Pairs-Shortest-Path computation based on Floyds algorithm Takes an nxn
        /// matrix C of edge costs and produces an nxn matrix A of lengths of shortest
        /// paths.
        /// </summary>
        public static int[][] computeFloydAPSP(int[][] C)
        {
            int i;
            int j;
            int k;
            int n = C.Length;
            int[][] A = new int[n][];
            for (int i2 = 0; i2 < n; i2++)
            {
                A[i2] = new int[n];
            }
            //System.out.println("Matrix size: " + n);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (C[i][j] == 0)
                    {
                        A[i][j] = 999999999;
                    }
                    else
                    {
                        A[i][j] = 1;
                    }
                }
            }
            for (i = 0; i < n; i++)
            {
                A[i][i] = 0;
                // no self cycle
            }
            for (k = 0; k < n; k++)
            {
                for (i = 0; i < n; i++)
                {
                    for (j = 0; j < n; j++)
                    {
                        if (A[i][k] + A[k][j] < A[i][j])
                        {
                            A[i][j] = A[i][k] + A[k][j];
                            //P[i][j] = k;        // k is included in the shortest path
                        }
                    }
                }
            }
            return A;
        }

        /// <summary> All-Pairs-Shortest-Path computation based on Floyds algorithm Takes an nxn
        /// matrix C of edge costs and produces an nxn matrix A of lengths of shortest
        /// paths.
        /// </summary>
        public static int[][] computeFloydAPSP(double[][] C)
        {
            int i;
            int j;
            int n = C.Length;
            int[][] A = new int[n][];
            for (int i2 = 0; i2 < n; i2++)
            {
                A[i2] = new int[n];
            }
            //System.out.println("Matrix size: " + n);
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                {
                    if (C[i][j] == 0)
                    {
                        A[i][j] = 0;
                    }
                    else
                    {
                        A[i][j] = 1;
                    }
                }
            }
            return computeFloydAPSP(A);
        }


        /// <summary> Recursivly perfoms a depth first search in a molecular graphs contained in
        /// the AtomContainer molecule, starting at the root atom and returning when it
        /// hits the target atom.
        /// CAUTION: This recursive method sets the VISITED flag of each atom
        /// does not reset it after finishing the search. If you want to do the
        /// operation on the same collection of atoms more than once, you have
        /// to set all the VISITED flags to false before each operation
        /// by looping of the atoms and doing a
        /// "atom.setFlag((CDKConstants.VISITED, false));"
        /// 
        /// </summary>
        /// <param name="molecule">The
        /// AtomContainer to be searched
        /// </param>
        /// <param name="root">    The root atom
        /// to start the search at
        /// </param>
        /// <param name="target">  The target
        /// </param>
        /// <param name="path">    An
        /// AtomContainer to be filled with the path
        /// </param>
        /// <returns> true if the
        /// target atom was found during this function call
        /// </returns>
        public static bool depthFirstTargetSearch(IAtomContainer molecule, IAtom root, IAtom target, IAtomContainer path)
        {
            IBond[] bonds = molecule.getConnectedBonds(root);
            IAtom nextAtom;
            root.setFlag(CDKConstants.VISITED, true);
            for (int f = 0; f < bonds.Length; f++)
            {
                nextAtom = bonds[f].getConnectedAtom(root);
                if (!nextAtom.getFlag(CDKConstants.VISITED))
                {
                    path.addAtom(nextAtom);
                    path.addBond(bonds[f]);
                    if (nextAtom == target)
                    {
                        return true;
                    }
                    else
                    {
                        if (!depthFirstTargetSearch(molecule, nextAtom, target, path))
                        {
                            // we did not find the target
                            path.removeAtom(nextAtom);
                            path.removeElectronContainer(bonds[f]);
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary> Performs a breadthFirstSearch in an AtomContainer starting with a
        /// particular sphere, which usually consists of one start atom. While
        /// searching the graph, the method marks each visited atom. It then puts all
        /// the atoms connected to the atoms in the given sphere into a new vector
        /// which forms the sphere to search for the next recursive method call. All
        /// atoms that have been visited are put into a molecule container. This
        /// breadthFirstSearch does thus find the connected graph for a given start
        /// atom.
        /// 
        /// </summary>
        /// <param name="ac">      The AtomContainer to be searched
        /// </param>
        /// <param name="sphere">  A sphere of atoms to start the search with
        /// </param>
        /// <param name="molecule">A molecule into which all the atoms and bonds are stored
        /// that are found during search
        /// </param>
        public static void breadthFirstSearch(IAtomContainer ac, System.Collections.ArrayList sphere, IMolecule molecule)
        {
            // System.out.println("Staring partitioning with this ac: " + ac);
            breadthFirstSearch(ac, sphere, molecule, -1);
        }


        /// <summary> Returns the atoms which are closest to an atom in an AtomContainer by bonds.
        /// If number of atoms in or below sphere x&lt;max andnumber of atoms in or below sphere x+1&gt;max then atoms in or below sphere x+1 are returned.
        /// 
        /// </summary>
        /// <param name="ac"> The AtomContainer to examine
        /// </param>
        /// <param name="a">  the atom to start from
        /// </param>
        /// <param name="max">the number of neighbours to return
        /// </param>
        /// <returns> the average bond length
        /// </returns>
        public static IAtom[] findClosestByBond(IAtomContainer ac, IAtom a, int max)
        {
            IMolecule mol = ac.Builder.newMolecule();
            System.Collections.ArrayList v = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            v.Add(a);
            breadthFirstSearch(ac, v, mol, max);
            IAtom[] returnValue = new IAtom[mol.Atoms.Length - 1];
            int k = 0;
            for (int i = 0; i < mol.Atoms.Length; i++)
            {
                if (mol.Atoms[i] != a)
                {
                    returnValue[k] = mol.Atoms[i];
                    k++;
                }
            }
            return (returnValue);
        }


        /// <summary> Performs a breadthFirstSearch in an AtomContainer starting with a
        /// particular sphere, which usually consists of one start atom. While
        /// searching the graph, the method marks each visited atom. It then puts all
        /// the atoms connected to the atoms in the given sphere into a new vector
        /// which forms the sphere to search for the next recursive method call. All
        /// atoms that have been visited are put into a molecule container. This
        /// breadthFirstSearch does thus find the connected graph for a given start
        /// atom.
        /// 
        /// </summary>
        /// <param name="ac">      The AtomContainer to be searched
        /// </param>
        /// <param name="sphere">  A sphere of atoms to start the search with
        /// </param>
        /// <param name="molecule">A molecule into which all the atoms and bonds are stored
        /// that are found during search
        /// </param>
        public static void breadthFirstSearch(IAtomContainer ac, System.Collections.ArrayList sphere, IMolecule molecule, int max)
        {
            IAtom atom;
            IAtom nextAtom;
            System.Collections.ArrayList newSphere = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int f = 0; f < sphere.Count; f++)
            {
                atom = (IAtom)sphere[f];
                //System.out.println("atoms  "+ atom + f);
                //System.out.println("sphere size  "+ sphere.size());
                molecule.addAtom(atom);
                // first copy LonePair's and SingleElectron's of this Atom as they need
                // to be copied too
                IElectronContainer[] eContainers = ac.getConnectedElectronContainers(atom);
                //System.out.println("found #ec's: " + eContainers.length);
                for (int i = 0; i < eContainers.Length; i++)
                {
                    if (!(eContainers[i] is IBond))
                    {
                        // ok, no bond, thus LonePair or SingleElectron
                        // System.out.println("adding non bond " + eContainers[i]);
                        molecule.addElectronContainer(eContainers[i]);
                    }
                }
                // now look at bonds
                IBond[] bonds = ac.getConnectedBonds(atom);
                for (int g = 0; g < bonds.Length; g++)
                {
                    if (!bonds[g].getFlag(CDKConstants.VISITED))
                    {
                        molecule.addBond(bonds[g]);
                        bonds[g].setFlag(CDKConstants.VISITED, true);
                    }
                    nextAtom = bonds[g].getConnectedAtom(atom);
                    if (!nextAtom.getFlag(CDKConstants.VISITED))
                    {
                        //					System.out.println("wie oft???");
                        newSphere.Add(nextAtom);
                        nextAtom.setFlag(CDKConstants.VISITED, true);
                    }
                }
                if (max > -1 && molecule.AtomCount > max)
                    return;
            }
            if (newSphere.Count > 0)
            {
                breadthFirstSearch(ac, newSphere, molecule, max);
            }
        }


        /// <summary> Performs a breadthFirstTargetSearch in an AtomContainer starting with a
        /// particular sphere, which usually consists of one start atom. While
        /// searching the graph, the method marks each visited atom. It then puts all
        /// the atoms connected to the atoms in the given sphere into a new vector
        /// which forms the sphere to search for the next recursive method call.
        /// The method keeps track of the sphere count and returns it as soon
        /// as the target atom is encountered.
        /// 
        /// </summary>
        /// <param name="ac">        The AtomContainer in which the path search is to be performed.
        /// </param>
        /// <param name="sphere">    The sphere of atoms to start with. Usually just the starting atom
        /// </param>
        /// <param name="target">    The target atom to be searched
        /// </param>
        /// <param name="pathLength">The current path length, incremented and passed in recursive calls. Call this method with "zero".
        /// </param>
        /// <param name="cutOff">    Stop the path search when this cutOff sphere count has been reached
        /// </param>
        /// <returns> The shortest path between the starting sphere and the target atom
        /// </returns>
        public static int breadthFirstTargetSearch(IAtomContainer ac, System.Collections.ArrayList sphere, IAtom target, int pathLength, int cutOff)
        {
            if (pathLength == 0)
                resetFlags(ac);
            pathLength++;
            if (pathLength > cutOff)
            {
                return -1;
            }
            IAtom atom;

            IAtom nextAtom;
            System.Collections.ArrayList newSphere = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int f = 0; f < sphere.Count; f++)
            {
                atom = (IAtom)sphere[f];
                IBond[] bonds = ac.getConnectedBonds(atom);
                for (int g = 0; g < bonds.Length; g++)
                {
                    if (!bonds[g].getFlag(CDKConstants.VISITED))
                    {
                        bonds[g].setFlag(CDKConstants.VISITED, true);
                    }
                    nextAtom = bonds[g].getConnectedAtom(atom);
                    if (!nextAtom.getFlag(CDKConstants.VISITED))
                    {
                        if (nextAtom == target)
                        {
                            return pathLength;
                        }
                        newSphere.Add(nextAtom);
                        nextAtom.setFlag(CDKConstants.VISITED, true);
                    }
                }
            }
            if (newSphere.Count > 0)
            {
                return breadthFirstTargetSearch(ac, newSphere, target, pathLength, cutOff);
            }
            return -1;
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

        /// <summary> Returns the radius of the molecular graph.
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to consider
        /// </param>
        /// <returns> The topological radius
        /// </returns>
        public static int getMolecularGraphRadius(IAtomContainer atomContainer)
        {
            int natom = atomContainer.AtomCount;

            int[][] admat = AdjacencyMatrix.getMatrix(atomContainer);
            int[][] distanceMatrix = computeFloydAPSP(admat);

            int[] eta = new int[natom];
            for (int i = 0; i < natom; i++)
            {
                int max = -99999;
                for (int j = 0; j < natom; j++)
                {
                    if (distanceMatrix[i][j] > max)
                        max = distanceMatrix[i][j];
                }
                eta[i] = max;
            }
            int min = 999999;
            for (int i = 0; i < eta.Length; i++)
            {
                if (eta[i] < min)
                    min = eta[i];
            }
            return min;
        }

        /// <summary> Returns the diameter of the molecular graph.
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to consider
        /// </param>
        /// <returns> The topological diameter
        /// </returns>
        public static int getMolecularGraphDiameter(IAtomContainer atomContainer)
        {
            int natom = atomContainer.AtomCount;

            int[][] admat = AdjacencyMatrix.getMatrix(atomContainer);
            int[][] distanceMatrix = computeFloydAPSP(admat);

            int[] eta = new int[natom];
            for (int i = 0; i < natom; i++)
            {
                int max = -99999;
                for (int j = 0; j < natom; j++)
                {
                    if (distanceMatrix[i][j] > max)
                        max = distanceMatrix[i][j];
                }
                eta[i] = max;
            }
            int max2 = -999999;
            for (int i = 0; i < eta.Length; i++)
            {
                if (eta[i] > max2)
                    max2 = eta[i];
            }
            return max2;
        }

        /// <summary> Returns the number of vertices that are a distance 'd' apart.
        /// <p/>
        /// In this method, d is the topological distance (ie edge count).
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to consider
        /// </param>
        /// <param name="distance">     The distance to consider
        /// </param>
        /// <returns> The number of vertices
        /// </returns>
        public static int getVertexCountAtDistance(IAtomContainer atomContainer, int distance)
        {
            int natom = atomContainer.AtomCount;

            int[][] admat = AdjacencyMatrix.getMatrix(atomContainer);
            int[][] distanceMatrix = computeFloydAPSP(admat);

            int n = 0;

            for (int i = 0; i < natom; i++)
            {
                for (int j = 0; j < natom; j++)
                {
                    if (distanceMatrix[i][j] == distance)
                        n++;
                }
            }
            return n / 2;
        }

        /// <summary> Returns a list of atoms in the shortest path between two atoms.
        /// 
        /// This method uses the Djikstra algorithm to find all the atoms in the shortest
        /// path between the two specified atoms. The start and end atoms are also included
        /// in the path returned
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to search in
        /// </param>
        /// <param name="start">The starting atom
        /// </param>
        /// <param name="end">The ending atom
        /// </param>
        /// <returns> A <code>List</code> containing the atoms in the shortest path between <code>start</code> and
        /// <code>end</code> inclusive
        /// </returns>
        public static System.Collections.IList getShortestPath(IAtomContainer atomContainer, IAtom start, IAtom end)
        {
            int natom = atomContainer.AtomCount;
            int endNumber = atomContainer.getAtomNumber(end);
            int startNumber = atomContainer.getAtomNumber(start);
            int[] d = new int[natom];
            int[] previous = new int[natom];
            for (int i = 0; i < natom; i++)
            {
                d[i] = 99999999;
                previous[i] = -1;
            }
            d[atomContainer.getAtomNumber(start)] = 0;

            System.Collections.ArrayList S = new System.Collections.ArrayList();
            System.Collections.ArrayList Q = new System.Collections.ArrayList();
            for (int i = 0; i < natom; i++)
                Q.Add((System.Int32)i);

            while (true)
            {
                if (Q.Count == 0)
                    break;

                // extract min
                int u = 999999;
                int index = 0;
                for (int i = 0; i < Q.Count; i++)
                {
                    int tmp = ((System.Int32)Q[i]);
                    if (d[tmp] < u)
                    {
                        u = d[tmp];
                        index = i;
                    }
                }
                Q.RemoveAt(index);
                S.Add(atomContainer.getAtomAt(u));
                if (u == endNumber)
                    break;

                // relaxation
                IAtom[] connected = atomContainer.getConnectedAtoms(atomContainer.getAtomAt(u));
                for (int i = 0; i < connected.Length; i++)
                {
                    int anum = atomContainer.getAtomNumber(connected[i]);
                    if (d[anum] > d[u] + 1)
                    {
                        // all edges have equals weights
                        d[anum] = d[u] + 1;
                        previous[anum] = u;
                    }
                }
            }

            System.Collections.ArrayList tmp2 = new System.Collections.ArrayList();
            int u2 = endNumber;
            while (true)
            {
                tmp2.Insert(0, atomContainer.getAtomAt(u2));
                u2 = previous[u2];
                if (u2 == startNumber)
                {
                    tmp2.Insert(0, atomContainer.getAtomAt(u2));
                    break;
                }
            }
            return tmp2;
        }

        private static System.Collections.IList allPaths;

        /// <summary> Get a list of all the paths between two atoms.
        /// <p/>
        /// If the two atoms are the same an empty list is returned. Note that this problem
        /// is NP-hard and so can take a long time for large graphs.
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to consider
        /// </param>
        /// <param name="start">        The starting Atom of the path
        /// </param>
        /// <param name="end">          The ending Atom of the path
        /// </param>
        /// <returns> A <code>List</code> containing all the paths between the specified atoms
        /// </returns>
        public static System.Collections.IList getAllPaths(IAtomContainer atomContainer, IAtom start, IAtom end)
        {
            allPaths = new System.Collections.ArrayList();
            if (start.Equals(end))
                return allPaths;
            findPathBetween(atomContainer, start, end, new System.Collections.ArrayList());
            return allPaths;
        }

        private static void findPathBetween(IAtomContainer atomContainer, IAtom start, IAtom end, System.Collections.IList path)
        {
            if (start == end)
            {
                path.Add(start);
                allPaths.Add(new System.Collections.ArrayList(path));
                path.RemoveAt(path.Count - 1);
                return;
            }
            if (path.Contains(start))
                return;
            path.Add(start);
            System.Collections.IList nbrs = atomContainer.getConnectedAtomsVector(start);
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = nbrs.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                findPathBetween(atomContainer, (IAtom)i.Current, end, path);
            }
            path.RemoveAt(path.Count - 1);
        }

        /// <summary> Get the paths starting from an atom of specified length.
        /// <p/>
        /// This method returns a set of paths. Each path is a <code>List</code> of atoms that
        /// make up the path (ie they are sequentially connected).
        /// 
        /// </summary>
        /// <param name="atomContainer">The molecule to consider
        /// </param>
        /// <param name="start">        The starting atom
        /// </param>
        /// <param name="length">       The length of paths to look for
        /// </param>
        /// <returns> A  <code>List</code> containing the paths found
        /// </returns>
        public static System.Collections.IList getPathsOfLength(IAtomContainer atomContainer, IAtom start, int length)
        {
            System.Collections.ArrayList curPath = new System.Collections.ArrayList();
            System.Collections.ArrayList paths = new System.Collections.ArrayList();
            curPath.Add(start);
            paths.Add(curPath);
            for (int i = 0; i < length; i++)
            {
                System.Collections.ArrayList tmpList = new System.Collections.ArrayList();
                for (int j = 0; j < paths.Count; j++)
                {
                    curPath = (System.Collections.ArrayList)paths[j];
                    IAtom lastVertex = (IAtom)curPath[curPath.Count - 1];
                    System.Collections.IList neighbors = atomContainer.getConnectedAtomsVector(lastVertex);
                    for (int k = 0; k < neighbors.Count; k++)
                    {
                        System.Collections.ArrayList newPath = new System.Collections.ArrayList(curPath);
                        if (newPath.Contains(neighbors[k]))
                            continue;
                        newPath.Add(neighbors[k]);
                        tmpList.Add(newPath);
                    }
                }
                paths.Clear();
                paths.AddRange(tmpList);
            }
            return (paths);
        }
    }
}
/*  $RCSfile$
*  $Author: rajarshi $
*  $Date: 2006-05-13 17:41:11 +0200 (Sat, 13 May 2006) $
*  $Revision: 6260 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*
*  This code has been kindly provided by Stephane Werner
*  and Thierry Hanser from IXELIS mail@ixelis.net
*
*  IXELIS sarl - Semantic Information Systems
*  17 rue des C???res 67200 Strasbourg, France
*  Tel/Fax : +33(0)3 88 27 81 39 Email: mail@ixelis.net
*
*  CDK Contact: cdk-devel@lists.sf.net
*
*  This program is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public License
*  as published by the Free Software Foundation; either version 2.1
*  of the License, or (at your option) any later version.
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
using Org.OpenScience.CDK.Isomorphism.MCSS;
using Org.OpenScience.CDK.Isomorphism.Matchers;
using Support;
using Org.OpenScience.CDK.Exception;

namespace Org.OpenScience.CDK.Isomorphism
{
    /// <summary>  This class implements a multipurpose structure comparison tool.
    /// It allows to find maximal common substructure, find the
    /// mapping of a substructure in another structure, and the mapping of
    /// two isomorphic structures.
    /// 
    /// <p>Structure comparison may be associated to bond constraints
    /// (mandatory bonds, e.g. scaffolds, reaction cores,...) on each source graph.
    /// The constraint flexibility allows a number of interesting queries.
    /// The substructure analysis relies on the RGraph generic class (see: RGraph)
    /// This class implements the link between the RGraph model and the
    /// the CDK model in this way the RGraph remains independant and may be used
    /// in other contexts.
    /// 
    /// <p>This algorithm derives from the algorithm described in
    /// {@cdk.cite HAN90} and modified in the thesis of T. Hanser {@cdk.cite Han93}.
    /// 
    /// <p><font color="#FF0000">
    /// warning :  As a result of the adjacency perception used in this algorithm
    /// there is a single limitation : cyclopropane and isobutane are seen as isomorph
    /// This is due to the fact that these two compounds are the only ones where
    /// each bond is connected two each other bond (bonds are fully conected)
    /// with the same number of bonds and still they have different structures
    /// The algotihm could be easily enhanced with a simple atom mapping manager
    /// to provide an atom level overlap definition that would reveal this case.
    /// We decided not to penalize the whole procedure because of one single
    /// exception query. Furthermore isomorphism may be discarded since  the number of atoms are
    /// not the same (3 != 4) and in most case this will be already
    /// screened out by a fingerprint based filtering.
    /// It is possible to add a special treatment for this special query.
    /// Be reminded that this algorithm matches bonds only.
    /// </font>
    /// 
    /// 
    /// </summary>
    /// <author>       Stephane Werner from IXELIS mail@ixelis.net
    /// </author>
    /// <cdk.created>  2002-07-17 </cdk.created>
    /// <cdk.require>  java1.4+ </cdk.require>
    /// <cdk.module>   standard </cdk.module>
    public class UniversalIsomorphismTester
    {
        internal const int ID1 = 0;
        internal const int ID2 = 1;
        private static long start;
        public static long timeout = -1;

        ///////////////////////////////////////////////////////////////////////////
        //                            Query Methods
        //
        // This methods are simple applications of the RGraph model on atom containers
        // using different constrains and search options. They give an exemple of the
        // most common queries but of course it is possible to define other type of
        // queries exploiting the constrain and option combinations
        //

        ////
        // Isomorphism search

        /// <summary>  Tests if  g1 and g2 are isomorph
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     true if the 2 molecule are isomorph
        /// </returns>
        public static bool isIsomorph(IAtomContainer g1, IAtomContainer g2)
        {
            if (g2.AtomCount != g1.AtomCount)
                return false;
            // check single atom case
            if (g2.AtomCount == 1)
            {
                IAtom atom = g1.getAtomAt(0);
                IAtom atom2 = g2.getAtomAt(0);
                if (atom is IQueryAtom)
                {
                    IQueryAtom qAtom = (IQueryAtom)atom;
                    return qAtom.matches(g2.getAtomAt(0));
                }
                else if (atom2 is IQueryAtom)
                {
                    IQueryAtom qAtom = (IQueryAtom)atom2;
                    return qAtom.matches(g1.getAtomAt(0));
                }
                else
                {
                    System.String atomSymbol = atom.Symbol;
                    return g1.getAtomAt(0).Symbol.Equals(atomSymbol);
                }
            }
            return (getIsomorphMap(g1, g2) != null);
        }


        /// <summary>  Returns the first isomorph mapping found or null
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the first isomorph mapping found projected of g1. This is a List of RMap objects containing Ids of matching bonds.
        /// </returns>
        public static System.Collections.IList getIsomorphMap(IAtomContainer g1, IAtomContainer g2)
        {
            System.Collections.IList result = null;

            System.Collections.IList rMapsList = search(g1, g2, getBitSet(g1), getBitSet(g2), false, false);

            if (!(rMapsList.Count == 0))
            {
                result = (System.Collections.IList)rMapsList[0];
            }

            return result;
        }


        /// <summary>  Returns the first isomorph 'atom mapping' found for g2 in g1.
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the first isomorph atom mapping found projected on g1. This is a List of RMap objects containing Ids of matching atoms.
        /// </returns>
        public static System.Collections.IList getIsomorphAtomsMap(IAtomContainer g1, IAtomContainer g2)
        {
            System.Collections.ArrayList list = checkSingleAtomCases(g1, g2);
            if (list == null)
            {
                return (makeAtomsMapOfBondsMap(UniversalIsomorphismTester.getIsomorphMap(g1, g2), g1, g2));
            }
            else if ((list.Count == 0))
            {
                return null;
            }
            else
            {
                return (System.Collections.IList)list[0];
            }
        }


        /// <summary>  Returns all the isomorph 'mappings' found between two
        /// atom containers.
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the list of all the 'mappings'
        /// </returns>
        public static System.Collections.IList getIsomorphMaps(IAtomContainer g1, IAtomContainer g2)
        {
            return search(g1, g2, getBitSet(g1), getBitSet(g2), true, true);
        }


        /////
        // Subgraph search

        /// <summary>  Returns all the subgraph 'bond mappings' found for g2 in g1.
        /// This is an ArrayList of ArrayLists of RMap objects.
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the list of all the 'mappings' found projected of g1
        /// 
        /// </returns>
        public static System.Collections.IList getSubgraphMaps(IAtomContainer g1, IAtomContainer g2)
        {
            return search(g1, g2, new System.Collections.BitArray(64), getBitSet(g2), true, true);
        }


        /// <summary>  Returns the first subgraph 'bond mapping' found for g2 in g1.
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the first subgraph bond mapping found projected on g1. This is a List of RMap objects containing Ids of matching bonds.
        /// </returns>
        public static System.Collections.IList getSubgraphMap(IAtomContainer g1, IAtomContainer g2)
        {
            System.Collections.IList result = null;
            System.Collections.IList rMapsList = search(g1, g2, new System.Collections.BitArray(64), getBitSet(g2), false, false);

            if (!(rMapsList.Count == 0))
            {
                result = (System.Collections.IList)rMapsList[0];
            }

            return result;
        }


        /// <summary>  Returns all subgraph 'atom mappings' found for g2 in g1.
        /// This is an ArrayList of ArrayLists of RMap objects.
        /// 
        /// </summary>
        /// <param name="g1"> first AtomContainer
        /// </param>
        /// <param name="g2"> second AtomContainer
        /// </param>
        /// <returns>     all subgraph atom mappings found projected on g1. This is a List of RMap objects containing Ids of matching atoms.
        /// </returns>
        public static System.Collections.IList getSubgraphAtomsMaps(IAtomContainer g1, IAtomContainer g2)
        {
            System.Collections.ArrayList list = checkSingleAtomCases(g1, g2);
            if (list == null)
            {
                return (makeAtomsMapsOfBondsMaps(UniversalIsomorphismTester.getSubgraphMaps(g1, g2), g1, g2));
            }
            else
            {
                return list;
            }
        }

        /// <summary>  Returns the first subgraph 'atom mapping' found for g2 in g1.
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the first subgraph atom mapping found projected on g1. This is a List of RMap objects containing Ids of matching atoms.
        /// </returns>
        public static System.Collections.IList getSubgraphAtomsMap(IAtomContainer g1, IAtomContainer g2)
        {
            System.Collections.ArrayList list = checkSingleAtomCases(g1, g2);
            if (list == null)
            {
                return (makeAtomsMapOfBondsMap(UniversalIsomorphismTester.getSubgraphMap(g1, g2), g1, g2));
            }
            else if ((list.Count == 0))
            {
                return null;
            }
            else
            {
                return (System.Collections.IList)list[0];
            }
        }


        /// <summary>  Tests if g2 a subgraph of g1
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     true if g2 a subgraph on g1
        /// </returns>
        public static bool isSubgraph(IAtomContainer g1, IAtomContainer g2)
        {
            if (g2.AtomCount > g1.AtomCount)
                return false;
            // test for single atom case
            if (g2.AtomCount == 1)
            {
                IAtom atom = g2.getAtomAt(0);
                for (int i = 0; i < g1.AtomCount; i++)
                {
                    IAtom atom2 = g1.getAtomAt(i);
                    if (atom is IQueryAtom)
                    {
                        IQueryAtom qAtom = (IQueryAtom)atom;
                        if (qAtom.matches(atom2))
                            return true;
                    }
                    else if (atom2 is IQueryAtom)
                    {
                        IQueryAtom qAtom = (IQueryAtom)atom2;
                        if (qAtom.matches(atom))
                            return true;
                    }
                    else
                    {
                        if (atom2.Symbol.Equals(atom.Symbol))
                            return true;
                    }
                }
                return false;
            }
            if (!testSubgraphHeuristics(g1, g2))
                return false;
            return (getSubgraphMap(g1, g2) != null);
        }


        ////
        // Maximum common substructure search

        /// <summary>  Returns all the maximal common substructure between 2 atom containers
        /// 
        /// </summary>
        /// <param name="g1"> first molecule
        /// </param>
        /// <param name="g2"> second molecule
        /// </param>
        /// <returns>     the list of all the maximal common substructure
        /// found projected of g1 (list of AtomContainer )
        /// </returns>
        public static System.Collections.IList getOverlaps(IAtomContainer g1, IAtomContainer g2)
        {
            start = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
            System.Collections.IList rMapsList = search(g1, g2, new System.Collections.BitArray(64), new System.Collections.BitArray(64), true, false);

            // projection on G1
            System.Collections.ArrayList graphList = projectList(rMapsList, g1, ID1);

            // reduction of set of solution (isomorphism and substructure
            // with different 'mappings'

            return getMaximum(graphList);
        }


        /// <summary>  Transforms an AtomContainer into a BitSet (which's size = number of bond
        /// in the atomContainer, all the bit are set to true)
        /// 
        /// </summary>
        /// <param name="ac"> AtomContainer to transform
        /// </param>
        /// <returns>     The bitSet
        /// </returns>
        public static System.Collections.BitArray getBitSet(IAtomContainer ac)
        {
            System.Collections.BitArray bs;
            int n = ac.getBondCount();

            if (n != 0)
            {
                bs = new System.Collections.BitArray((n % 64 == 0 ? n / 64 : n / 64 + 1) * 64);
                for (int i = 0; i < n; i++)
                {
                    SupportClass.BitArraySupport.Set(bs, i);
                }
            }
            else
            {
                bs = new System.Collections.BitArray(64);
            }

            return bs;
        }


        //////////////////////////////////////////////////
        //          Internal methods

        /// <summary>  Builds the RGraph ( resolution graph ), from two atomContainer
        /// (description of the two molecules to compare)
        /// This is the interface point between the CDK model and
        /// the generic MCSS algorithm based on the RGRaph.
        /// 
        /// </summary>
        /// <param name="g1"> Description of the first molecule
        /// </param>
        /// <param name="g2"> Description of the second molecule
        /// </param>
        /// <returns>     the rGraph
        /// </returns>
        public static RGraph buildRGraph(IAtomContainer g1, IAtomContainer g2)
        {
            RGraph rGraph = new RGraph();
            nodeConstructor(rGraph, g1, g2);
            arcConstructor(rGraph, g1, g2);
            return rGraph;
        }


        /// <summary>  General Rgraph parsing method (usually not used directly)
        /// This method is the entry point for the recursive search
        /// adapted to the atom container input.
        /// 
        /// </summary>
        /// <param name="g1">               first molecule
        /// </param>
        /// <param name="g2">               second molecule
        /// </param>
        /// <param name="c1">               initial condition ( bonds from g1 that
        /// must be contains in the solution )
        /// </param>
        /// <param name="c2">               initial condition ( bonds from g2 that
        /// must be contains in the solution )
        /// </param>
        /// <param name="findAllStructure"> if false stop at the first structure found
        /// </param>
        /// <param name="findAllMap">       if true search all the 'mappings' for one same
        /// structure
        /// </param>
        /// <returns>                   a List of Lists of RMap objects that represent the search solutions
        /// </returns>
        public static System.Collections.IList search(IAtomContainer g1, IAtomContainer g2, System.Collections.BitArray c1, System.Collections.BitArray c2, bool findAllStructure, bool findAllMap)
        {

            // reset result
            System.Collections.ArrayList rMapsList = new System.Collections.ArrayList();

            // build the RGraph corresponding to this problem
            RGraph rGraph = buildRGraph(g1, g2);
            // parse the RGraph with the given constrains and options
            rGraph.parse(c1, c2, findAllStructure, findAllMap);
            System.Collections.IList solutionList = rGraph.Solutions;

            // convertions of RGraph's internal solutions to G1/G2 mappings
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = solutionList.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                System.Collections.BitArray set_Renamed = (System.Collections.BitArray)i.Current;
                rMapsList.Add(rGraph.bitSetToRMap(set_Renamed));
            }

            return rMapsList;
        }

        //////////////////////////////////////
        //    Manipulation tools

        /// <summary>  Projects a list of RMap on a molecule
        /// 
        /// </summary>
        /// <param name="rMapList"> the list to project
        /// </param>
        /// <param name="g">        the molecule on which project
        /// </param>
        /// <param name="id">       the id in the RMap of the molecule g
        /// </param>
        /// <returns>           an AtomContainer
        /// </returns>
        public static IAtomContainer project(System.Collections.IList rMapList, IAtomContainer g, int id)
        {
            IAtomContainer ac = g.Builder.newAtomContainer();

            IBond[] bondList = g.Bonds;

            System.Collections.Hashtable table = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
            IAtom a1;
            IAtom a2;
            IAtom a;
            IBond bond;

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = rMapList.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                RMap rMap = (RMap)i.Current;
                if (id == UniversalIsomorphismTester.ID1)
                {
                    bond = bondList[rMap.Id1];
                }
                else
                {
                    bond = bondList[rMap.Id2];
                }

                a = bond.getAtomAt(0);
                a1 = (IAtom)table[a];

                if (a1 == null)
                {
                    try
                    {
                        a1 = (IAtom)a.Clone();
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        SupportClass.WriteStackTrace(e, Console.Error);
                    }
                    ac.addAtom(a1);
                    table[a] = a1;
                }

                a = bond.getAtomAt(1);
                a2 = (IAtom)table[a];

                if (a2 == null)
                {
                    try
                    {
                        a2 = (IAtom)a.Clone();
                    }
                    //UPGRADE_NOTE: Exception 'java.lang.CloneNotSupportedException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
                    catch (System.Exception e)
                    {
                        SupportClass.WriteStackTrace(e, Console.Error);
                    }
                    ac.addAtom(a2);
                    table[a] = a2;
                }
                IBond newBond = g.Builder.newBond(a1, a2, bond.Order);
                newBond.setFlag(CDKConstants.ISAROMATIC, bond.getFlag(CDKConstants.ISAROMATIC));
                ac.addBond(newBond);
            }
            return ac;
        }


        /// <summary>  Project a list of RMapsList on a molecule
        /// 
        /// </summary>
        /// <param name="rMapsList"> list of RMapsList to project
        /// </param>
        /// <param name="g">         the molecule on which project
        /// </param>
        /// <param name="id">        the id in the RMap of the molecule g
        /// </param>
        /// <returns>            a list of AtomContainer
        /// </returns>
        public static System.Collections.ArrayList projectList(System.Collections.IList rMapsList, IAtomContainer g, int id)
        {
            System.Collections.ArrayList graphList = new System.Collections.ArrayList();

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = rMapsList.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                System.Collections.IList rMapList = (System.Collections.IList)i.Current;
                IAtomContainer ac = project(rMapList, g, id);
                graphList.Add(ac);
            }
            return graphList;
        }

        /// <summary>  remove all redundant solution
        /// 
        /// </summary>
        /// <param name="graphList"> the list of structure to clean
        /// </param>
        /// <returns>            the list cleaned
        /// </returns>
        private static System.Collections.ArrayList getMaximum(System.Collections.ArrayList graphList)
        {
            System.Collections.ArrayList reducedGraphList = (System.Collections.ArrayList)graphList.Clone();

            for (int i = 0; i < graphList.Count; i++)
            {
                IAtomContainer gi = (IAtomContainer)graphList[i];

                for (int j = i + 1; j < graphList.Count; j++)
                {
                    IAtomContainer gj = (IAtomContainer)graphList[j];

                    // Gi included in Gj or Gj included in Gi then
                    // reduce the irrelevant solution
                    if (isSubgraph(gj, gi))
                    {
                        SupportClass.ICollectionSupport.Remove(reducedGraphList, gi);
                    }
                    else if (isSubgraph(gi, gj))
                    {
                        SupportClass.ICollectionSupport.Remove(reducedGraphList, gj);
                    }
                }
            }
            return reducedGraphList;
        }

        /// <summary>  Checks for single atom cases before doing subgraph/isomorphism search
        /// 
        /// </summary>
        /// <param name="g1"> AtomContainer to match on
        /// </param>
        /// <param name="g2"> AtomContainer as query
        /// </param>
        /// <returns>     List of List of RMap objects for the Atoms (not Bonds!), null if no single atom case
        /// </returns>
        public static System.Collections.ArrayList checkSingleAtomCases(IAtomContainer g1, IAtomContainer g2)
        {

            if (g2.AtomCount == 1)
            {
                System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
                IAtom atom = g2.getAtomAt(0);
                if (atom is IQueryAtom)
                {
                    IQueryAtom qAtom = (IQueryAtom)atom;
                    for (int i = 0; i < g1.AtomCount; i++)
                    {
                        if (qAtom.matches(g1.getAtomAt(i)))
                            arrayList.Add(new RMap(i, 0));
                    }
                }
                else
                {
                    System.String atomSymbol = atom.Symbol;
                    for (int i = 0; i < g1.AtomCount; i++)
                    {
                        if (g1.getAtomAt(i).Symbol.Equals(atomSymbol))
                            arrayList.Add(new RMap(i, 0));
                    }
                }
                return arrayList;
            }
            else if (g1.AtomCount == 1)
            {
                System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
                IAtom atom = g1.getAtomAt(0);
                for (int i = 0; i < g2.AtomCount; i++)
                {
                    IAtom atom2 = g2.getAtomAt(i);
                    if (atom2 is IQueryAtom)
                    {
                        IQueryAtom qAtom = (IQueryAtom)atom2;
                        if (qAtom.matches(atom))
                            arrayList.Add(new RMap(0, i));
                    }
                    else
                    {
                        if (atom2.Symbol.Equals(atom.Symbol))
                            arrayList.Add(new RMap(0, i));
                    }
                }
                return arrayList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>  This makes maps of matching atoms out of a maps of matching bonds as produced by the get(Subgraph|Ismorphism)Maps methods.
        /// 
        /// </summary>
        /// <param name="l">  The list produced by the getMap method.
        /// </param>
        /// <param name="g1"> The first atom container.
        /// </param>
        /// <param name="g2"> The second one (first and second as in getMap)
        /// </param>
        /// <returns>     A Vector of Vectors of RMap objects of matching Atoms.
        /// </returns>
        public static System.Collections.IList makeAtomsMapsOfBondsMaps(System.Collections.IList l, IAtomContainer g1, IAtomContainer g2)
        {
            if (l == null)
            {
                return l;
            }
            System.Collections.ArrayList result = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int i = 0; i < l.Count; i++)
            {
                System.Collections.ArrayList l2 = (System.Collections.ArrayList)l[i];
                result.Add((System.Collections.ArrayList)makeAtomsMapOfBondsMap(l2, g1, g2));
            }
            return result;
        }

        /// <summary>  This makes a map of matching atoms out of a map of matching bonds as produced by the get(Subgraph|Ismorphism)Map methods.
        /// 
        /// </summary>
        /// <param name="l">  The list produced by the getMap method.
        /// </param>
        /// <param name="g1"> The first atom container.
        /// </param>
        /// <param name="g2"> The second one (first and second as in getMap)
        /// </param>
        /// <returns>     The mapping found projected on g1. This is a List of RMap objects containing Ids of matching atoms.
        /// </returns>
        public static System.Collections.IList makeAtomsMapOfBondsMap(System.Collections.IList l, IAtomContainer g1, IAtomContainer g2)
        {
            if (l == null)
                return (l);
            IBond[] bonds1 = g1.Bonds;
            IBond[] bonds2 = g2.Bonds;
            System.Collections.IList result = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
            for (int i = 0; i < l.Count; i++)
            {
                IBond bond1 = bonds1[((RMap)l[i]).Id1];
                IBond bond2 = bonds2[((RMap)l[i]).Id2];
                IAtom[] atom1 = bond1.getAtoms();
                IAtom[] atom2 = bond2.getAtoms();
                for (int j = 0; j < 2; j++)
                {
                    IBond[] bondsConnectedToAtom1j = g1.getConnectedBonds(atom1[j]);
                    for (int k = 0; k < bondsConnectedToAtom1j.Length; k++)
                    {
                        if (bondsConnectedToAtom1j[k] != bond1)
                        {
                            IBond testBond = bondsConnectedToAtom1j[k];
                            for (int m = 0; m < l.Count; m++)
                            {
                                IBond testBond2;
                                if (((RMap)l[m]).Id1 == g1.getBondNumber(testBond))
                                {
                                    testBond2 = bonds2[((RMap)l[m]).Id2];
                                    for (int n = 0; n < 2; n++)
                                    {
                                        System.Collections.IList bondsToTest = g2.getConnectedBondsVector(atom2[n]);
                                        if (bondsToTest.Contains(testBond2))
                                        {
                                            RMap map;
                                            if (j == n)
                                            {
                                                map = new RMap(g1.getAtomNumber(atom1[0]), g2.getAtomNumber(atom2[0]));
                                            }
                                            else
                                            {
                                                map = new RMap(g1.getAtomNumber(atom1[1]), g2.getAtomNumber(atom2[0]));
                                            }
                                            if (!result.Contains(map))
                                            {
                                                result.Add(map);
                                            }
                                            RMap map2;
                                            if (j == n)
                                            {
                                                map2 = new RMap(g1.getAtomNumber(atom1[1]), g2.getAtomNumber(atom2[1]));
                                            }
                                            else
                                            {
                                                map2 = new RMap(g1.getAtomNumber(atom1[0]), g2.getAtomNumber(atom2[1]));
                                            }
                                            if (!result.Contains(map2))
                                            {
                                                result.Add(map2);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (result);
        }


        /// <summary>  Builds  the nodes of the RGraph ( resolution graph ), from
        /// two atom containers (description of the two molecules to compare)
        /// 
        /// </summary>
        /// <param name="gr">  the target RGraph
        /// </param>
        /// <param name="ac1"> description of the first molecule
        /// </param>
        /// <param name="ac2"> description of the second molecule
        /// </param>
        private static void nodeConstructor(RGraph gr, IAtomContainer ac1, IAtomContainer ac2)
        {
            // resets the target graph.
            gr.clear();
            IBond[] bondsA1 = ac1.Bonds;
            IBond[] bondsA2 = ac2.Bonds;

            // compares each bond of G1 to each bond of G2
            for (int i = 0; i < bondsA1.Length; i++)
            {
                for (int j = 0; j < bondsA2.Length; j++)
                {
                    if (timeout > -1 && ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - start) > timeout)
                        throw new CDKException("Timeout exceeded in getOverlaps");
                    IBond bondA2 = bondsA2[j];
                    if (bondA2 is IQueryBond)
                    {
                        IQueryBond queryBond = (IQueryBond)bondA2;
                        IQueryAtom atom1 = (IQueryAtom)(bondA2.getAtomAt(0));
                        IQueryAtom atom2 = (IQueryAtom)(bondA2.getAtomAt(1));
                        IBond bond = bondsA1[i];
                        if (queryBond.matches(bond))
                        {
                            // ok, bonds match
                            if (atom1.matches(bond.getAtomAt(0)) && atom2.matches(bond.getAtomAt(1)) || atom1.matches(bond.getAtomAt(1)) && atom2.matches(bond.getAtomAt(0)))
                            {
                                // ok, atoms match in either order
                                gr.addNode(new RNode(i, j));
                            }
                        }
                    }
                    else
                    {
                        // if both bonds are compatible then create an association node
                        // in the resolution graph
                        if (((bondsA1[i].Order == bondsA2[j].Order && bondsA1[i].getFlag(CDKConstants.ISAROMATIC) == bondsA2[j].getFlag(CDKConstants.ISAROMATIC)) || (bondsA1[i].getFlag(CDKConstants.ISAROMATIC) && bondsA2[j].getFlag(CDKConstants.ISAROMATIC))) && ((bondsA1[i].getAtomAt(0).Symbol.Equals(bondsA2[j].getAtomAt(0).Symbol) && bondsA1[i].getAtomAt(1).Symbol.Equals(bondsA2[j].getAtomAt(1).Symbol)) || (bondsA1[i].getAtomAt(0).Symbol.Equals(bondsA2[j].getAtomAt(1).Symbol) && bondsA1[i].getAtomAt(1).Symbol.Equals(bondsA2[j].getAtomAt(0).Symbol))))
                        {
                            gr.addNode(new RNode(i, j));
                        }
                    }
                }
            }
        }


        /// <summary>  Build edges of the RGraphs
        /// This method create the edge of the RGraph and
        /// calculates the incompatibility and neighbourhood
        /// relationships between RGraph nodes.
        /// 
        /// </summary>
        /// <param name="gr">  the rGraph
        /// </param>
        /// <param name="ac1"> Description of the first molecule
        /// </param>
        /// <param name="ac2"> Description of the second molecule
        /// </param>
        private static void arcConstructor(RGraph gr, IAtomContainer ac1, IAtomContainer ac2)
        {
            // each node is incompatible with himself
            for (int i = 0; i < gr.Graph.Count; i++)
            {
                RNode x = (RNode)gr.Graph[i];
                SupportClass.BitArraySupport.Set(x.Forbidden, i);
            }

            IBond a1;
            IBond a2;
            IBond b1;
            IBond b2;

            IBond[] bondsA1 = ac1.Bonds;
            IBond[] bondsA2 = ac2.Bonds;

            gr.FirstGraphSize = ac1.getBondCount();
            gr.SecondGraphSize = ac2.getBondCount();

            for (int i = 0; i < gr.Graph.Count; i++)
            {
                RNode x = (RNode)gr.Graph[i];

                // two nodes are neighbours if their adjacency
                // relationship in are equivalent in G1 and G2
                // else they are incompatible.
                for (int j = i + 1; j < gr.Graph.Count; j++)
                {
                    if (timeout > -1 && ((System.DateTime.Now.Ticks - 621355968000000000) / 10000 - start) > timeout)
                        throw new CDKException("Timeout exceeded in getOverlaps");
                    RNode y = (RNode)gr.Graph[j];

                    a1 = bondsA1[((RNode)gr.Graph[i]).RMap.Id1];
                    a2 = bondsA2[((RNode)gr.Graph[i]).RMap.Id2];
                    b1 = bondsA1[((RNode)gr.Graph[j]).RMap.Id1];
                    b2 = bondsA2[((RNode)gr.Graph[j]).RMap.Id2];

                    if (a2 is IQueryBond)
                    {
                        if (a1.Equals(b1) || a2.Equals(b2) || !queryAdjacency(a1, b1, a2, b2))
                        {
                            SupportClass.BitArraySupport.Set(x.Forbidden, j);
                            SupportClass.BitArraySupport.Set(y.Forbidden, i);
                        }
                        else if (hasCommonAtom(a1, b1))
                        {
                            SupportClass.BitArraySupport.Set(x.Extension, j);
                            SupportClass.BitArraySupport.Set(y.Extension, i);
                        }
                    }
                    else
                    {
                        if (a1.Equals(b1) || a2.Equals(b2) || (!getCommonSymbol(a1, b1).Equals(getCommonSymbol(a2, b2))))
                        {
                            SupportClass.BitArraySupport.Set(x.Forbidden, j);
                            SupportClass.BitArraySupport.Set(y.Forbidden, i);
                        }
                        else if (hasCommonAtom(a1, b1))
                        {
                            SupportClass.BitArraySupport.Set(x.Extension, j);
                            SupportClass.BitArraySupport.Set(y.Extension, i);
                        }
                    }
                }
            }
        }


        /// <summary>  Determines if 2 bond have 1 atom in common
        /// 
        /// </summary>
        /// <param name="a"> first bond
        /// </param>
        /// <param name="b"> second bond
        /// </param>
        /// <returns>    the symbol of the common atom or "" if
        /// the 2 bonds have no common atom
        /// </returns>
        private static bool hasCommonAtom(IBond a, IBond b)
        {

            if (a.contains(b.getAtomAt(0)))
            {
                return true;
            }
            else if (a.contains(b.getAtomAt(1)))
            {
                return true;
            }

            return false;
        }

        /// <summary>  Determines if 2 bond have 1 atom in common and returns the common symbol
        /// 
        /// </summary>
        /// <param name="a"> first bond
        /// </param>
        /// <param name="b"> second bond
        /// </param>
        /// <returns>    the symbol of the common atom or "" if
        /// the 2 bonds have no common atom
        /// </returns>
        private static System.String getCommonSymbol(IBond a, IBond b)
        {
            System.String symbol = "";

            if (a.contains(b.getAtomAt(0)))
            {
                symbol = b.getAtomAt(0).Symbol;
            }
            else if (a.contains(b.getAtomAt(1)))
            {
                symbol = b.getAtomAt(1).Symbol;
            }

            return symbol;
        }

        /// <summary>  Determines if 2 bond have 1 atom in common if second is a query AtomContainer
        /// 
        /// </summary>
        /// <param name="a1"> first bond
        /// </param>
        /// <param name="b1"> second bond
        /// </param>
        /// <returns>    the symbol of the common atom or "" if
        /// the 2 bonds have no common atom
        /// </returns>
        private static bool queryAdjacency(IBond a1, IBond b1, IBond a2, IBond b2)
        {

            IAtom atom1 = null;
            IAtom atom2 = null;

            if (a1.contains(b1.getAtomAt(0)))
            {
                atom1 = b1.getAtomAt(0);
            }
            else if (a1.contains(b1.getAtomAt(1)))
            {
                atom1 = b1.getAtomAt(1);
            }

            if (a2.contains(b2.getAtomAt(0)))
            {
                atom2 = b2.getAtomAt(0);
            }
            else if (a2.contains(b2.getAtomAt(1)))
            {
                atom2 = b2.getAtomAt(1);
            }

            if (atom1 != null && atom2 != null)
            {
                return ((IQueryAtom)atom2).matches(atom1);
            }
            else
                return atom1 == null && atom2 == null;
        }

        /// <summary>  Checks some simple heuristics for whether the subgraph query can
        /// realistically be a subgraph of the supergraph. If, for example, the
        /// number of nitrogen atoms in the query is larger than that of the supergraph
        /// it cannot be part of it.
        /// 
        /// </summary>
        /// <param name="ac1"> the supergraph to be checked
        /// </param>
        /// <param name="ac2"> the subgraph to be tested for
        /// </param>
        /// <returns>    true if the subgraph ac2 has a chance to be a subgraph of ac1
        /// 
        /// </returns>

        private static bool testSubgraphHeuristics(IAtomContainer ac1, IAtomContainer ac2)
        {
            int ac1SingleBondCount = 0;
            int ac1DoubleBondCount = 0;
            int ac1TripleBondCount = 0;
            int ac1AromaticBondCount = 0;
            int ac2SingleBondCount = 0;
            int ac2DoubleBondCount = 0;
            int ac2TripleBondCount = 0;
            int ac2AromaticBondCount = 0;
            int ac1SCount = 0;
            int ac1OCount = 0;
            int ac1NCount = 0;
            int ac1FCount = 0;
            int ac1ClCount = 0;
            int ac1BrCount = 0;
            int ac1ICount = 0;
            int ac1CCount = 0;

            int ac2SCount = 0;
            int ac2OCount = 0;
            int ac2NCount = 0;
            int ac2FCount = 0;
            int ac2ClCount = 0;
            int ac2BrCount = 0;
            int ac2ICount = 0;
            int ac2CCount = 0;

            IBond bond;
            IAtom atom;
            for (int i = 0; i < ac1.getBondCount(); i++)
            {
                bond = ac1.getBondAt(i);
                if (bond.getFlag(CDKConstants.ISAROMATIC))
                    ac1AromaticBondCount++;
                else if (bond.Order == 1)
                    ac1SingleBondCount++;
                else if (bond.Order == 2)
                    ac1DoubleBondCount++;
                else if (bond.Order == 3)
                    ac1TripleBondCount++;
            }
            for (int i = 0; i < ac2.getBondCount(); i++)
            {
                bond = ac2.getBondAt(i);
                if (bond is IQueryBond)
                    continue;
                if (bond.getFlag(CDKConstants.ISAROMATIC))
                    ac2AromaticBondCount++;
                else if (bond.Order == 1)
                    ac2SingleBondCount++;
                else if (bond.Order == 2)
                    ac2DoubleBondCount++;
                else if (bond.Order == 3)
                    ac2TripleBondCount++;
            }

            if (ac2SingleBondCount > ac1SingleBondCount)
                return false;
            if (ac2AromaticBondCount > ac1AromaticBondCount)
                return false;
            if (ac2DoubleBondCount > ac1DoubleBondCount)
                return false;
            if (ac2TripleBondCount > ac1TripleBondCount)
                return false;

            for (int i = 0; i < ac1.AtomCount; i++)
            {
                atom = ac1.getAtomAt(i);
                if (atom.Symbol.Equals("S"))
                    ac1SCount++;
                else if (atom.Symbol.Equals("N"))
                    ac1NCount++;
                else if (atom.Symbol.Equals("O"))
                    ac1OCount++;
                else if (atom.Symbol.Equals("F"))
                    ac1FCount++;
                else if (atom.Symbol.Equals("Cl"))
                    ac1ClCount++;
                else if (atom.Symbol.Equals("Br"))
                    ac1BrCount++;
                else if (atom.Symbol.Equals("I"))
                    ac1ICount++;
                else if (atom.Symbol.Equals("C"))
                    ac1CCount++;
            }
            for (int i = 0; i < ac2.AtomCount; i++)
            {
                atom = ac2.getAtomAt(i);
                if (atom is IQueryAtom)
                    continue;
                if (atom.Symbol.Equals("S"))
                    ac2SCount++;
                else if (atom.Symbol.Equals("N"))
                    ac2NCount++;
                else if (atom.Symbol.Equals("O"))
                    ac2OCount++;
                else if (atom.Symbol.Equals("F"))
                    ac2FCount++;
                else if (atom.Symbol.Equals("Cl"))
                    ac2ClCount++;
                else if (atom.Symbol.Equals("Br"))
                    ac2BrCount++;
                else if (atom.Symbol.Equals("I"))
                    ac2ICount++;
                else if (atom.Symbol.Equals("C"))
                    ac2CCount++;
            }

            if (ac1SCount < ac2SCount)
                return false;
            if (ac1NCount < ac2NCount)
                return false;
            if (ac1OCount < ac2OCount)
                return false;
            if (ac1FCount < ac2FCount)
                return false;
            if (ac1ClCount < ac2ClCount)
                return false;
            if (ac1BrCount < ac2BrCount)
                return false;
            if (ac1ICount < ac2ICount)
                return false;
            if (ac1CCount < ac2CCount)
                return false;


            return true;
        }
    }
}
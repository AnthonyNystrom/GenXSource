/*  $RCSfile$
*  $Author: egonw $
*  $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
*  $Revision: 5855 $
*
*  Copyright (C) 1997-2006  The Chemistry Development Kit (CDK) project
*  
*  This code has been kindly provided by Stephane Werner 
*  and Thierry Hanser from IXELIS mail@ixelis.net
*  
*  IXELIS sarl - Semantic Information Systems
*               17 rue des C?dres 67200 Strasbourg, France
*               Tel/Fax : +33(0)3 88 27 81 39 Email: mail@ixelis.net
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
*/
using System;
using Support;

namespace Org.OpenScience.CDK.Isomorphism.MCSS
{
    /// <summary> This class implements the Resolution Graph (RGraph).
    /// The RGraph is a graph based representation of the search problem.
    /// An RGraph is constructred from the two compared graphs (G1 and G2).
    /// Each vertex (node) in the RGraph represents a possible association
    /// from an edge in G1 with an edge in G2. Thus two compatible bonds
    /// in two molecular graphs are represented by a vertex in the RGraph.
    /// Each edge in the RGraph corresponds to a common adjacency relationship
    /// between the 2 couple of compatible edges associated to the 2 RGraph nodes
    /// forming this edge.
    /// 
    /// <p>Example:
    /// <pre>
    /// G1 : C-C=O  and G2 : C-C-C=0
    /// 1 2 3           1 2 3 4
    /// </pre>
    /// 
    /// <p>The resulting RGraph(G1,G2) will contain 3 nodes:
    /// <ul>
    /// <li>Node A : association between bond C-C :  1-2 in G1 and 1-2 in G2
    /// <li>Node B : association between bond C-C :  1-2 in G1 and 2-3 in G2
    /// <li>Node C : association between bond C=0 :  2-3 in G1 and 3-4 in G2
    /// </ul>
    /// The RGraph will also contain one edge representing the 
    /// adjacency between node B and C  that is : bonds 1-2 and 2-3 in G1 
    /// and bonds 2-3 and 3-4 in G2.
    /// 
    /// <p>Once the RGraph has been built from the two compared graphs
    /// it becomes a very interesting tool to perform all kinds of 
    /// structural search (isomorphism, substructure search, maximal common
    /// substructure,....).
    /// 
    /// <p>The  search may be constrained by mandatory elements (e.g. bonds that
    /// have to be present in the mapped common substructures).
    /// 
    /// <p>Performing a query on an RGraph requires simply to set the constrains
    /// (if any) and to invoke the parsing method (parse())
    /// 
    /// <p>The RGraph has been designed to be a generic tool. It may be constructed
    /// from any kind of source graphs, thus it is not restricted to a chemical
    /// context.
    /// 
    /// <p>The RGraph model is indendant from the CDK model and the link between
    /// both model is performed by the RTools class. In this way the RGraph 
    /// class may be reused in other graph context (conceptual graphs,....)
    /// 
    /// <p><b>Important note</b>: This implementation of the algorithm has not been
    /// optimized for speed at this stage. It has been
    /// written with the goal to clearly retrace the 
    /// principle of the underlined search method. There is
    /// room for optimization in many ways including the
    /// the algorithm itself. 
    /// 
    /// <p>This algorithm derives from the algorithm described in
    /// {@cdk.cite HAN90} and modified in the thesis of T. Hanser {@cdk.cite Han93}.
    /// 
    /// </summary>
    /// <author>       Stephane Werner from IXELIS mail@ixelis.net
    /// </author>
    /// <cdk.created>  2002-07-17 </cdk.created>
    /// <cdk.require>  java1.4+ </cdk.require>
    /// <cdk.module>   standard </cdk.module>
    public class RGraph
    {
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the size of the first of the two
        /// compared graphs.
        /// </summary>
        /// <returns> The size of the first of the two compared graphs         
        /// </returns>
        /// <summary>  Sets the size of the first of the two
        /// compared graphs.
        /// </summary>
        /// <param name="n1">The size of the second of the two compared graphs         
        /// </param>
        virtual public int FirstGraphSize
        {
            get
            {
                return firstGraphSize;
            }

            set
            {
                firstGraphSize = value;
            }

        }
        //UPGRADE_NOTE: Respective javadoc comments were merged.  It should be changed in order to comply with .NET documentation conventions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1199'"
        /// <summary>  Returns the size of the second of the two
        /// compared graphs.
        /// </summary>
        /// <returns> The size of the second of the two compared graphs         
        /// </returns>
        /// <summary>  Returns the size of the second of the two
        /// compared graphs.
        /// </summary>
        /// <param name="n2">The size of the second of the two compared graphs         
        /// </param>
        virtual public int SecondGraphSize
        {
            get
            {
                return secondGraphSize;
            }

            set
            {
                secondGraphSize = value;
            }

        }
        /// <summary>  Returns the graph object of this RGraph.</summary>
        /// <returns>      The graph object, a list         
        /// </returns>
        virtual public System.Collections.IList Graph
        {
            get
            {
                return this.graph;
            }

        }
        /// <summary>  Returns the list of solutions.
        /// 
        /// </summary>
        /// <returns>    The solution list 
        /// </returns>
        virtual public System.Collections.IList Solutions
        {
            get
            {
                return solutionList;
            }

        }
        /// <summary>  Sets the 'AllStructres' option. If true
        /// all possible solutions will be generated. If false
        /// the search will stop as soon as a solution is found.
        /// (e.g. when we just want to know if a G2 is
        /// a substructure of G1 or not).
        /// 
        /// </summary>
        /// <param name="findAllStructure"> 
        /// </param>
        virtual public bool AllStructure
        {
            set
            {
                this.findAllStructure = value;
            }

        }
        /// <summary>  Sets the 'finAllMap' option. If true
        /// all possible 'mappings' will be generated. If false
        /// the search will keep only one 'mapping' per structure
        /// association.
        /// 
        /// </summary>
        /// <param name="findAllMap"> 
        /// </param>
        virtual public bool AllMap
        {
            set
            {
                this.findAllMap = value;
            }

        }
        /// <summary> Sets the maxIteration for the RGraph parsing. If set to -1,
        /// then no iteration maximum is taken into account.
        /// 
        /// </summary>
        /// <param name="it"> The new maxIteration value
        /// </param>
        virtual public int MaxIteration
        {
            set
            {
                this.maxIteration = value;
            }

        }
        // an RGraph is a list of RGraph nodes
        // each node keeping track of its
        // neighbours.
        internal System.Collections.IList graph = null;

        // maximal number of iterations before
        // search break
        internal int maxIteration = -1;

        // dimensions of the compared graphs
        internal int firstGraphSize = 0;
        internal int secondGraphSize = 0;

        // constrains 
        internal System.Collections.BitArray c1 = null;
        internal System.Collections.BitArray c2 = null;

        // current solution list
        internal System.Collections.IList solutionList = null;

        // flag to define if we want to get all possible 'mappings'    
        internal bool findAllMap = false;

        // flag to define if we want to get all possible 'structures'
        internal bool findAllStructure = true;

        // working variables
        internal bool stop = false;
        internal int nbIteration = 0;
        internal System.Collections.BitArray graphBitSet = null;

        /// <summary> Constructor for the RGraph object and creates an empty RGraph.</summary>
        public RGraph()
        {
            graph = new System.Collections.ArrayList();
            solutionList = new System.Collections.ArrayList();
            graphBitSet = new System.Collections.BitArray(64);
        }

        /// <summary>  Reinitialisation of the TGraph.</summary>
        public virtual void clear()
        {
            graph.Clear();
            graphBitSet.SetAll(false);
        }

        /// <summary>  Adds a new node to the RGraph.</summary>
        /// <param name="newNode"> The node to add to the graph
        /// </param>
        public virtual void addNode(RNode newNode)
        {
            graph.Add(newNode);
            SupportClass.BitArraySupport.Set(graphBitSet, graph.Count - 1);
        }

        /// <summary>  Parsing of the RGraph. This is the main method
        /// to perform a query. Given the constrains c1 and c2
        /// defining mandatory elements in G1 and G2 and given
        /// the search options, this m?thod builds an initial set
        /// of starting nodes (B) and parses recursively the
        /// RGraph to find a list of solution according to 
        /// these parameters.
        /// 
        /// </summary>
        /// <param name="c1"> constrain on the graph G1
        /// </param>
        /// <param name="c2"> constrain on the graph G2
        /// </param>
        /// <param name="findAllStructure">true if we want all results to be generated   
        /// </param>
        /// <param name="findAllMap">true is we want all possible 'mappings'
        /// </param>
        public virtual void parse(System.Collections.BitArray c1, System.Collections.BitArray c2, bool findAllStructure, bool findAllMap)
        {
            // initialize the list of solution
            solutionList.Clear();

            // builds the set of starting nodes
            // according to the constrains
            System.Collections.BitArray b = buildB(c1, c2);

            // setup options
            AllStructure = findAllStructure;
            AllMap = findAllMap;

            // parse recursively the RGraph
            parseRec(new System.Collections.BitArray((b.Count % 64 == 0 ? b.Count / 64 : b.Count / 64 + 1) * 64), b, new System.Collections.BitArray((b.Count % 64 == 0 ? b.Count / 64 : b.Count / 64 + 1) * 64));
        }

        /// <summary>  Parsing of the RGraph. This is the recursive method
        /// to perform a query. The method will recursively
        /// parse the RGraph thru connected nodes and visiting the
        /// RGraph using allowed adjacency relationship.
        /// 
        /// </summary>
        /// <param name="traversed"> node already parsed
        /// </param>
        /// <param name="extension"> possible extension node (allowed neighbours)
        /// </param>
        /// <param name="forbiden">  node forbiden (set of node incompatible with the current solution)
        /// </param>
        private void parseRec(System.Collections.BitArray traversed, System.Collections.BitArray extension, System.Collections.BitArray forbidden)
        {
            System.Collections.BitArray newTraversed = null;
            System.Collections.BitArray newExtension = null;
            System.Collections.BitArray newForbidden = null;
            System.Collections.BitArray potentialNode = null;

            // if there is no more extension possible we
            // have reached a potential new solution
            if (isEmpty(extension))
            {
                solution(traversed);
            }
            // carry on with each possible extension
            else
            {
                // calculates the set of nodes that may still
                // be reached at this stage (not forbiden)
                potentialNode = ((System.Collections.BitArray)graphBitSet.Clone());
                potentialNode.And(forbidden.Not());
                //UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
                potentialNode.Or(traversed);

                // checks if we must continue the search
                // according to the potential node set
                if (mustContinue(potentialNode))
                {
                    // carry on research and update iteration count
                    nbIteration++;

                    // for each node in the set of possible extension (neighbours of 
                    // the current partial solution, include the node to the solution
                    // and perse recursively the RGraph with the new context.
                    for (int x = nextSetBit(extension, 0); x >= 0 && !stop; x = nextSetBit(extension, x + 1))
                    {
                        // evaluates the new set of forbidden nodes
                        // by including the nodes not compatible with the
                        // newly accepted node.
                        newForbidden = (System.Collections.BitArray)forbidden.Clone();
                        //UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
                        newForbidden.Or(((RNode)graph[x]).forbidden);

                        // if it is the first time we are here then
                        // traversed is empty and we initialize the set of
                        // possible extensions to the extension of the first
                        // accepted node in the solution.
                        if (isEmpty(traversed))
                        {
                            newExtension = (System.Collections.BitArray)(((RNode)graph[x]).extension.Clone());
                        }
                        // else we simply update the set of solution by
                        // including the neighbours of the newly accepted node
                        else
                        {
                            newExtension = (System.Collections.BitArray)extension.Clone();
                            //UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.Or' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
                            newExtension.Or(((RNode)graph[x]).extension);
                        }

                        // extension my not contain forbidden nodes
                        newExtension.And(newForbidden.Not());

                        // create the new set of traversed node
                        // (update current partial solution)
                        // and add x to the set of forbidden node
                        // (a node may only appear once in a solution)
                        newTraversed = (System.Collections.BitArray)traversed.Clone();
                        SupportClass.BitArraySupport.Set(newTraversed, x);
                        SupportClass.BitArraySupport.Set(forbidden, x);

                        // parse recursively the RGraph
                        parseRec(newTraversed, newExtension, newForbidden);
                    }
                }
            }
        }

        private bool isEmpty(System.Collections.BitArray array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i])
                    return false;
            }
            return true;
        }

        private int nextSetBit(System.Collections.BitArray array, int from)
        {
            for (int i = from; i < array.Count; i++)
            {
                if (array[i])
                    return i;
            }
            return -1;
        }

        /// <summary> Checks if a potantial solution is a real one 
        /// (not included in a previous solution)
        /// and add this solution to the solution list
        /// in case of success.
        /// 
        /// </summary>
        /// <param name="traversed"> new potential solution
        /// </param>
        private void solution(System.Collections.BitArray traversed)
        {
            bool included = false;
            System.Collections.BitArray projG1 = projectG1(traversed);
            System.Collections.BitArray projG2 = projectG2(traversed);

            // the solution must follows the search constrains
            // (must contain the mandatory elements in G1 an G2)
            if (isContainedIn(c1, projG1) && isContainedIn(c2, projG2))
            {
                // the solution should not be included in a previous solution
                // at the RGraph level. So we check against all prevous solution
                // On the other hand if a previous solution is included in the
                // new one, the previous solution is removed.
                int index = 0;
                for (System.Collections.IEnumerator i = solutionList.GetEnumerator(); i.MoveNext() && !included; )
                {
                    //UPGRADE_TODO: Method 'java.util.ListIterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilListIteratornext'"
                    System.Collections.BitArray sol = (System.Collections.BitArray)i.Current;

                    //UPGRADE_TODO: Method 'java.util.BitSet.equals' was converted to 'System.Collections.BitArray.Equals' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilBitSetequals_javalangObject'"
                    if (!sol.Equals(traversed))
                    {
                        // if we asked to save all 'mappings' then keep this mapping
                        //UPGRADE_TODO: Method 'java.util.BitSet.equals' was converted to 'System.Collections.BitArray.Equals' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilBitSetequals_javalangObject'"
                        if (findAllMap && (projG1.Equals(projectG1(sol)) || projG2.Equals(projectG2(sol))))
                        {
                            // do nothing
                        }
                        // if the new solution is included mark it as included
                        else if (isContainedIn(projG1, projectG1(sol)) || isContainedIn(projG2, projectG2(sol)))
                        {
                            included = true;
                        }
                        // if the previous solution is contained in the new one, remove the previous solution
                        else if (isContainedIn(projectG1(sol), projG1) || isContainedIn(projectG2(sol), projG2))
                        {
                            //UPGRADE_ISSUE: Method 'java.util.ListIterator.remove' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javautilListIteratorremove'"
                            //i.remove();
                            solutionList.RemoveAt(index);
                        }
                    }
                    else
                    {
                        // solution already exists
                        included = true;
                    }
                    index++;
                }

                if (included == false)
                {
                    // if it is really a new solution add it to the 
                    // list of current solution
                    solutionList.Add(traversed);
                }

                if (!findAllStructure)
                {
                    // if we need only one solution
                    // stop the search process
                    // (e.g. substructure search)
                    stop = true;
                }
            }
        }

        /// <summary>  Determine if there are potential soltution remaining.</summary>
        /// <param name="potentialNode"> set of remaining potential nodes
        /// </param>
        /// <returns>      true if it is worse to continue the search         
        /// </returns>
        private bool mustContinue(System.Collections.BitArray potentialNode)
        {
            bool result = true;
            bool cancel = false;
            System.Collections.BitArray projG1 = projectG1(potentialNode);
            System.Collections.BitArray projG2 = projectG2(potentialNode);

            // if we reached the maximum number of
            // serach iterations than do not continue
            if (maxIteration != -1 && nbIteration >= maxIteration)
            {
                return false;
            }

            // if constrains may no more be fullfilled than stop.
            if (!isContainedIn(c1, projG1) || !isContainedIn(c2, projG2))
            {
                return false;
            }

            // check if the solution potential is not included in an already
            // existing solution
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = solutionList.GetEnumerator(); i.MoveNext() && !cancel; )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                System.Collections.BitArray sol = (System.Collections.BitArray)i.Current;

                // if we want every 'mappings' do not stop
                //UPGRADE_TODO: Method 'java.util.BitSet.equals' was converted to 'System.Collections.BitArray.Equals' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilBitSetequals_javalangObject'"
                if (findAllMap && (projG1.Equals(projectG1(sol)) || projG2.Equals(projectG2(sol))))
                {
                    // do nothing
                }
                // if it is not possible to do better than an already existing solution than stop.
                else if (isContainedIn(projG1, projectG1(sol)) || isContainedIn(projG2, projectG2(sol)))
                {
                    result = false;
                    cancel = true;
                }
            }

            return result;
        }

        /// <summary>  Builds the initial extension set. This is the
        /// set of node tha may be used as seed for the
        /// RGraph parsing. This set depends on the constrains
        /// defined by the user.
        /// </summary>
        /// <param name="c1"> constraint in the graph G1
        /// </param>
        /// <param name="c2"> constraint in the graph G2
        /// </param>
        /// <returns>     
        /// </returns>
        private System.Collections.BitArray buildB(System.Collections.BitArray c1, System.Collections.BitArray c2)
        {
            this.c1 = c1;
            this.c2 = c2;

            System.Collections.BitArray bs = new System.Collections.BitArray(64);

            // only nodes that fulfill the initial constrains
            // are allowed in the initial extension set : B
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = graph.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                RNode rn = (RNode)i.Current;

                if ((c1.Get(rn.rMap.id1) || isEmpty(c1)) && (c2.Get(rn.rMap.id2) || isEmpty(c2)))
                {
                    SupportClass.BitArraySupport.Set(bs, graph.IndexOf(rn));
                }
            }
            return bs;
        }

        /// <summary>  Converts a RGraph bitset (set of RNode)
        /// to a list of RMap that represents the 
        /// mapping between to substructures in G1 and G2
        /// (the projection of the RGraph bitset on G1
        /// and G2).
        /// 
        /// </summary>
        /// <param name="set"> the BitSet
        /// </param>
        /// <returns>      the RMap list
        /// </returns>
        public virtual System.Collections.IList bitSetToRMap(System.Collections.BitArray set_Renamed)
        {
            System.Collections.IList rMapList = new System.Collections.ArrayList();

            for (int x = nextSetBit(set_Renamed, 0); x >= 0; x = nextSetBit(set_Renamed, x + 1))
            {
                RNode xNode = (RNode)graph[x];
                rMapList.Add(xNode.rMap);
            }
            return rMapList;
        }

        /// <summary>  Returns a string representation of the RGraph.</summary>
        /// <returns> the string representation of the RGraph
        /// </returns>
        public override System.String ToString()
        {
            System.String message = "";
            int j = 0;

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator i = graph.GetEnumerator(); i.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                RNode rn = (RNode)i.Current;
                message += ("-------------\n" + "RNode " + j + "\n" + rn.ToString() + "\n");
                j++;
            }
            return message;
        }


        /////////////////////////////////
        // BitSet tools
        /// <summary>  Projects a RGraph bitset on the source graph G1.</summary>
        /// <param name="set"> RGraph BitSet to project
        /// </param>
        /// <returns>      The associate BitSet in G1 
        /// </returns>
        public virtual System.Collections.BitArray projectG1(System.Collections.BitArray set_Renamed)
        {
            System.Collections.BitArray projection = new System.Collections.BitArray((firstGraphSize % 64 == 0 ? firstGraphSize / 64 : firstGraphSize / 64 + 1) * 64);
            RNode xNode = null;

            for (int x = nextSetBit(set_Renamed, 0); x >= 0; x = nextSetBit(set_Renamed, x + 1))
            {
                xNode = (RNode)graph[x];
                SupportClass.BitArraySupport.Set(projection, xNode.rMap.id1);
            }
            return projection;
        }

        /// <summary>  Projects a RGraph bitset on the source graph G2.</summary>
        /// <param name="set"> RGraph BitSet to project
        /// </param>
        /// <returns>      The associate BitSet in G2 
        /// </returns>
        public virtual System.Collections.BitArray projectG2(System.Collections.BitArray set_Renamed)
        {
            System.Collections.BitArray projection = new System.Collections.BitArray((secondGraphSize % 64 == 0 ? secondGraphSize / 64 : secondGraphSize / 64 + 1) * 64);
            RNode xNode = null;

            for (int x = nextSetBit(set_Renamed, 0); x >= 0; x = nextSetBit(set_Renamed, x + 1))
            {
                xNode = (RNode)graph[x];
                SupportClass.BitArraySupport.Set(projection, xNode.rMap.id2);
            }
            return projection;
        }

        /// <summary>  Test if set A is contained in  set B.</summary>
        /// <param name="A"> a bitSet 
        /// </param>
        /// <param name="B"> a bitSet 
        /// </param>
        /// <returns>    true if  A is contained in  B 
        /// </returns>
        private bool isContainedIn(System.Collections.BitArray A, System.Collections.BitArray B)
        {
            bool result = false;

            if (isEmpty(A))
            {
                return true;
            }

            System.Collections.BitArray setA = (System.Collections.BitArray)A.Clone();
            //UPGRADE_NOTE: In .NET BitArrays must be of the same size to allow the 'System.Collections.BitArray.And' operation. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1083'"
            setA.And(B);

            //UPGRADE_TODO: Method 'java.util.BitSet.equals' was converted to 'System.Collections.BitArray.Equals' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilBitSetequals_javalangObject'"
            if (setA.Equals(A))
            {
                result = true;
            }

            return result;
        }
    }
}
/* $RCSfile$
* $Author: egonw $
* $Date: 2006-03-29 10:27:08 +0200 (Wed, 29 Mar 2006) $
* $Revision: 5855 $
* 
* Copyright (C) 2004-2006  The Chemistry Development Kit (CDK) project
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
using org._3pq.jgrapht;
using org._3pq.jgrapht.alg;
using org._3pq.jgrapht.graph;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Ringsearch.Cyclebasis;
using Support;
using System.Collections;

namespace Org.OpenScience.CDK.RingSearch.CycleBasis
{
    /// <summary> A minimum basis of all cycles in a graph.
    /// All cycles in a graph G can be constructed from the basis cycles by binary
    /// addition of their invidence vectors.
    /// 
    /// A minimum cycle basis is a Matroid.
    /// 
    /// </summary>
    /// <author>  Ulrich Bauer <baueru@cs.tum.edu>
    /// 
    /// 
    /// </author>
    /// <cdk.module>  standard </cdk.module>
    /// <summary> 
    /// </summary>
    /// <cdk.builddepends>  jgrapht-0.5.3.jar </cdk.builddepends>
    /// <cdk.depends>  jgrapht-0.5.3.jar </cdk.depends>

    public class CycleBasis
    {

        //private List cycles = new Vector();
        private System.Collections.IList mulitEdgeCycles = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));
        private System.Collections.IList multiEdgeList = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

        private SimpleCycleBasis cachedCycleBasis;

        //private List edgeList = new Vector();
        //private List multiEdgeList = new Vector();
        private UndirectedGraph baseGraph;
        private System.Collections.IList subgraphBases = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

        /// <summary> Constructs a minimum cycle basis of a graph.
        /// 
        /// </summary>
        /// <param name="g">the graph for the cycle basis
        /// </param>
        public CycleBasis(UndirectedGraph g)
        {
            baseGraph = g;

            // We construct a simple graph out of the input (multi-)graph
            // as a subgraph with no multiedges.
            // The removed edges are collected in multiEdgeList
            // Moreover, shortest cycles through these edges are constructed and
            // collected in mulitEdgeCycles

            UndirectedGraph simpleGraph = new UndirectedSubgraph(g, null, null);

            // Iterate over the edges and discard all edges with the same source and target
            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator it = g.edgeSet().GetEnumerator(); it.MoveNext(); )
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                Edge edge = (Edge)((DictionaryEntry)it.Current).Value;
                System.Object u = edge.Source;
                System.Object v = edge.Target;
                System.Collections.IList edges = simpleGraph.getAllEdges(u, v);
                if (edges.Count > 1)
                {
                    // Multiple edges between u and v.
                    // Keep the edge with the least weight


                    Edge minEdge = edge;
                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    for (System.Collections.IEnumerator jt = edges.GetEnumerator(); jt.MoveNext(); )
                    {
                        //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                        Edge nextEdge = (Edge)jt.Current;
                        minEdge = nextEdge.Weight < minEdge.Weight ? nextEdge : minEdge;
                    }

                    //  ...and remove the others.
                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    for (System.Collections.IEnumerator jt = edges.GetEnumerator(); jt.MoveNext(); )
                    {
                        //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                        Edge nextEdge = (Edge)jt.Current;
                        if (nextEdge != minEdge)
                        {
                            // Remove edge from the graph
                            simpleGraph.removeEdge(nextEdge);

                            // Create a new cycle through this edge by finding 
                            // a shortest path between the vertices of the edge
                            //UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
                            CSGraphT.SupportClass.SetSupport edgesOfCycle = new CSGraphT.SupportClass.HashSetSupport();
                            edgesOfCycle.Add(nextEdge);
                            edgesOfCycle.AddAll(DijkstraShortestPath.findPathBetween(simpleGraph, u, v));

                            multiEdgeList.Add(nextEdge);
                            mulitEdgeCycles.Add(new SimpleCycle(baseGraph, edgesOfCycle));
                        }
                    }
                }
            }

            System.Collections.IList biconnectedComponents = new BiconnectivityInspector(simpleGraph).biconnectedSets();

            for (IEnumerator it = biconnectedComponents.GetEnumerator(); it.MoveNext();)
            {
                CSGraphT.SupportClass.SetSupport edges = (CSGraphT.SupportClass.SetSupport)it.Current;
                //IList edges = (IList)it.Current;

                if (edges.Count > 1)
                {
                    CSGraphT.SupportClass.SetSupport vertices = new CSGraphT.SupportClass.HashSetSupport();
                    for (System.Collections.IEnumerator edgeIt = edges.GetEnumerator(); edgeIt.MoveNext(); )
                    {
                        Edge edge = (Edge)((DictionaryEntry)edgeIt.Current).Value;
                        vertices.Add(edge.Source);
                        vertices.Add(edge.Target);
                    }
                    UndirectedGraph subgraph = new UndirectedSubgraph(simpleGraph, vertices, edges);

                    SimpleCycleBasis cycleBasis = new SimpleCycleBasis(subgraph);

                    subgraphBases.Add(cycleBasis);
                }
                else
                {
                    Edge edge = (Edge)((DictionaryEntry)edges.GetEnumerator().Current).Value;
                    multiEdgeList.Add(edge);
                }
            }
        }




        /*	
        public void minimize() {
        if (isMinimized) 
        return;
		
        for (Iterator it = subgraphBases.iterator(); it.hasNext();) {
        SimpleCycleBasis basis = (SimpleCycleBasis) it.next();
        basis.minimize();
        }
		
        isMinimized = true;
        }
        */

        /// <summary> Prints the cycle-edge incidence matrix of the cycle basis.</summary>
        public virtual void printIncidenceMatrix()
        {
            SimpleCycleBasis basis = simpleBasis();

            /*
            Collection edgeList = basis.edges();
            for (int j=0; j<edgeList.size(); j++) {
            System.out.print(((Edge) edgeList.get(j)).getSource());
            }
            System.out.println();
            for (int j=0; j<edgeList.size(); j++) {
            System.out.print(((Edge) edgeList.get(j)).getTarget());
            }
            System.out.println();
            for (int j=0; j<edgeList.size(); j++) {
            System.out.print('-');
            }
            System.out.println();
            */

            bool[][] incidMatr = basis.getCycleEdgeIncidenceMatrix();
            for (int i = 0; i < incidMatr.Length; i++)
            {
                for (int j = 0; j < incidMatr[i].Length; j++)
                {
                    System.Console.Out.Write(incidMatr[i][j] ? 1 : 0);
                }
                System.Console.Out.WriteLine();
            }
        }

        public virtual int[] weightVector()
        {
            SimpleCycleBasis basis = simpleBasis();
            System.Collections.IList cycles = basis.cycles();

            int[] result = new int[cycles.Count];
            for (int i = 0; i < cycles.Count; i++)
            {
                SimpleCycle cycle = (SimpleCycle)cycles[i];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                result[i] = (int)cycle.weight();
            }
            System.Array.Sort(result);

            return result;
        }

        private SimpleCycleBasis simpleBasis()
        {
            if (cachedCycleBasis == null)
            {
                System.Collections.IList cycles = new System.Collections.ArrayList();
                System.Collections.IList edgeList = new System.Collections.ArrayList();

                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                for (System.Collections.IEnumerator it = subgraphBases.GetEnumerator(); it.MoveNext(); )
                {
                    SimpleCycleBasis subgraphBase = (SimpleCycleBasis)it.Current;
                    SupportClass.ICollectionSupport.AddAll(cycles, subgraphBase.cycles());
                    SupportClass.ICollectionSupport.AddAll(edgeList, subgraphBase.edges());
                }

                SupportClass.ICollectionSupport.AddAll(cycles, mulitEdgeCycles);
                SupportClass.ICollectionSupport.AddAll(edgeList, multiEdgeList);

                //edgeList.addAll(baseGraph.edgeSet());


                cachedCycleBasis = new SimpleCycleBasis(cycles, edgeList, baseGraph);
            }

            return cachedCycleBasis;
        }

        /// <summary> Returns the cycles that form the cycle basis.
        /// 
        /// </summary>
        /// <returns> a <Code>Collection</code> of the basis cycles
        /// </returns>

        public virtual System.Collections.ICollection cycles()
        {
            return simpleBasis().cycles();
        }

        /// <summary> Returns the essential cycles of this cycle basis.
        /// A essential cycle is contained in every minimum cycle basis of a graph.
        /// 
        /// </summary>
        /// <returns> a <Code>Collection</code> of the essential cycles
        /// </returns>

        public virtual System.Collections.ICollection essentialCycles()
        {
            //UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
            System.Collections.ICollection result = new CSGraphT.SupportClass.HashSetSupport();
            //minimize();

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator it = subgraphBases.GetEnumerator(); it.MoveNext(); )
            {
                SimpleCycleBasis cycleBasis = (SimpleCycleBasis)it.Current;
                SupportClass.ICollectionSupport.AddAll(result, cycleBasis.essentialCycles());
            }

            return result;
        }

        /// <summary> Returns the essential cycles of this cycle basis.
        /// A relevant cycle is contained in some minimum cycle basis of a graph.
        /// 
        /// </summary>
        /// <returns> a <Code>Map</code> mapping each relevant cycles to the corresponding
        /// basis cycle in this basis
        /// </returns>

        public virtual System.Collections.IDictionary relevantCycles()
        {
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.IDictionary result = new System.Collections.Hashtable();
            //minimize();

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator it = subgraphBases.GetEnumerator(); it.MoveNext(); )
            {
                SimpleCycleBasis cycleBasis = (SimpleCycleBasis)it.Current;
                SupportClass.MapSupport.PutAll(result, cycleBasis.relevantCycles());
            }

            return result;
        }

        /// <summary> Returns the connected components of this cycle basis, in regard to matroid theory.
        /// Two cycles belong to the same commected component if there is a circuit (a minimal 
        /// dependent set) containing both cycles.
        /// 
        /// </summary>
        /// <returns> a <Code>List</code> of <Code>Set</code>s consisting of the cycles in a
        /// equivalence class.
        /// </returns>

        public virtual System.Collections.IList equivalenceClasses()
        {
            System.Collections.IList result = new System.Collections.ArrayList();
            //minimize();

            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
            for (System.Collections.IEnumerator it = subgraphBases.GetEnumerator(); it.MoveNext(); )
            {
                SimpleCycleBasis cycleBasis = (SimpleCycleBasis)it.Current;
                SupportClass.ICollectionSupport.AddAll(result, cycleBasis.equivalenceClasses());
            }

            return result;
        }
    }
}
/* $RCSfile$
* $Author: rajarshi $
* $Date: 2006-06-10 17:37:46 +0200 (Sat, 10 Jun 2006) $
* $Revision: 6421 $
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
using NuGenCDKSharp;
using org._3pq.jgrapht;
using org._3pq.jgrapht.alg;
using org._3pq.jgrapht.graph;
using Org.OpenScience.CDK.Graph;
using Org.OpenScience.CDK.Ringsearch.Cyclebasis;
using Support;

namespace Org.OpenScience.CDK.RingSearch.CycleBasis
{
    /// <summary> Auxiliary class for <code>CycleBasis</code>.
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

    public class SimpleCycleBasis
    {
        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AnonymousClassComparator' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AnonymousClassComparator : System.Collections.IComparer
        {
            public AnonymousClassComparator(SimpleCycleBasis enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }
            private void InitBlock(SimpleCycleBasis enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SimpleCycleBasis enclosingInstance;
            public SimpleCycleBasis Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }
            public virtual int Compare(System.Object o1, System.Object o2)
            {
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                return (int)(((SimpleCycle)o1).weight() - ((SimpleCycle)o2).weight());
            }
        }

        private System.Collections.IList edgeList;
        private System.Collections.IList cycles_Renamed_Field;
        private UndirectedGraph graph;

        private bool isMinimized = false;
        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        private System.Collections.Hashtable edgeIndexMap;

        public SimpleCycleBasis(System.Collections.IList cycles, System.Collections.IList edgeList, UndirectedGraph graph)
        {
            this.edgeList = edgeList;
            this.cycles_Renamed_Field = cycles;
            this.graph = graph;

            edgeIndexMap = createEdgeIndexMap(edgeList);
        }


        public SimpleCycleBasis(UndirectedGraph graph)
        {
            this.cycles_Renamed_Field = new System.Collections.ArrayList();
            this.edgeList = new System.Collections.ArrayList();
            this.graph = graph;

            createMinimumCycleBasis();
        }

        private void createMinimumCycleBasis()
        {
            org._3pq.jgrapht.Graph subgraph = new Subgraph(graph, null, null);

            CSGraphT.SupportClass.SetSupport remainingEdges = new CSGraphT.SupportClass.HashSetSupport(graph.edgeSet());
            //UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
            CSGraphT.SupportClass.SetSupport selectedEdges = new CSGraphT.SupportClass.HashSetSupport();

            while (!(remainingEdges.Count == 0))
            {
                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                Edge edge = (Edge)remainingEdges.GetEnumerator().Current;

                subgraph.removeEdge(edge);

                // Compute a shortest cycle through edge
                System.Collections.IList path = BFSShortestPath.findPathBetween(subgraph, edge.Source, edge.Target);
                path.Add(edge);
                SimpleCycle cycle = new SimpleCycle(graph, path);

                subgraph.addEdge(edge);

                selectedEdges.Add(edge);

                cycles_Renamed_Field.Insert(0, cycle);
                edgeList.Insert(0, edge);

                SupportClass.ICollectionSupport.RemoveAll(remainingEdges, path);
            }

            subgraph.removeAllEdges(selectedEdges);

            // The cycles just created are already minimal, so we can start minimizing at startIndex
            int startIndex = cycles_Renamed_Field.Count;

            // Now we perform a breadth first traversal and build a fundamental tree base
            // ("Kirchhoff base") of the remaining subgraph

            System.Object currentVertex = graph.vertexSet()[0];

            // We build a spanning tree as a directed graph to easily find the parent of a
            // vertex in the tree. This means however that we have to create new Edge objects
            // for the tree and can't just use the Edge objects of the graph, since the
            // the edge in the graph might have a wrong or no direction.

            DirectedGraph spanningTree = new SimpleDirectedGraph();

            //UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
            CSGraphT.SupportClass.SetSupport visitedEdges = new CSGraphT.SupportClass.HashSetSupport();

            // FIFO for the BFS
            //UPGRADE_TODO: Class 'java.util.LinkedList' was converted to 'System.Collections.ArrayList' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilLinkedList'"
            System.Collections.ArrayList vertexQueue = new System.Collections.ArrayList();

            // currentVertex is the root of the spanning tree
            spanningTree.addVertex(currentVertex);

            vertexQueue.Insert(vertexQueue.Count, currentVertex);

            // We need to remember the tree edges so we can add them at once to the
            // index list for the incidence matrix

            System.Collections.IList treeEdges = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

            while (!(vertexQueue.Count == 0))
            {
                System.Object tempObject;
                tempObject = vertexQueue[0];
                vertexQueue.RemoveAt(0);
                currentVertex = tempObject;

                System.Collections.IEnumerator edges = subgraph.edgesOf(currentVertex).GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (edges.MoveNext())
                {
                    // find a neighbour vertex of the current vertex 
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    Edge edge = (Edge)edges.Current;

                    if (!visitedEdges.Contains(edge))
                    {

                        // mark edge as visited
                        visitedEdges.Add(edge);

                        System.Object nextVertex = edge.oppositeVertex(currentVertex);

                        if (!spanningTree.containsVertex(nextVertex))
                        {
                            // tree edge

                            treeEdges.Add(edge);

                            spanningTree.addVertex(nextVertex);

                            // create a new (directed) Edge object (as explained above)
                            spanningTree.addEdge(currentVertex, nextVertex);

                            // add the next vertex to the BFS-FIFO
                            vertexQueue.Insert(vertexQueue.Count, nextVertex);
                        }
                        else
                        {
                            // non-tree edge

                            // This edge defines a cycle together with the edges of the spanning tree
                            // along the path to the root of the tree. We create a new cycle containing 
                            // these edges (not the tree edges, but the corresponding edges in the graph)

                            System.Collections.IList edgesOfCycle = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                            // follow the path to the root of the tree

                            System.Object vertex = currentVertex;

                            // get parent of vertex
                            System.Collections.IList incomingEdgesOfVertex = spanningTree.incomingEdgesOf(vertex);
                            System.Object parent = (incomingEdgesOfVertex.Count == 0) ? null : ((Edge)incomingEdgesOfVertex[0]).oppositeVertex(vertex);

                            while (parent != null)
                            {
                                // add the corresponding edge to the cycle
                                edgesOfCycle.Add(subgraph.getEdge(vertex, parent));

                                // go up the tree
                                vertex = parent;

                                // get parent of vertex
                                incomingEdgesOfVertex = spanningTree.incomingEdgesOf(vertex);
                                parent = (incomingEdgesOfVertex.Count == 0) ? null : ((Edge)incomingEdgesOfVertex[0]).oppositeVertex(vertex);
                            }

                            // do the same thing for nextVertex
                            vertex = nextVertex;

                            // get parent of vertex
                            incomingEdgesOfVertex = spanningTree.incomingEdgesOf(vertex);
                            parent = (incomingEdgesOfVertex.Count == 0) ? null : ((Edge)incomingEdgesOfVertex[0]).oppositeVertex(vertex);

                            while (parent != null)
                            {
                                edgesOfCycle.Add(subgraph.getEdge(vertex, parent));
                                vertex = parent;

                                // get parent of vertex
                                incomingEdgesOfVertex = spanningTree.incomingEdgesOf(vertex);
                                parent = (incomingEdgesOfVertex.Count == 0) ? null : ((Edge)incomingEdgesOfVertex[0]).oppositeVertex(vertex);
                            }

                            // finally, add the non-tree edge to the cycle
                            edgesOfCycle.Add(edge);

                            // add the edge to the index list for the incidence matrix
                            edgeList.Add(edge);

                            SimpleCycle newCycle = new SimpleCycle(graph, edgesOfCycle);

                            cycles_Renamed_Field.Add(newCycle);
                        }
                    }
                }
            }

            // Add all the tree edges to the index list for the incidence matrix
            SupportClass.ICollectionSupport.AddAll(edgeList, treeEdges);

            edgeIndexMap = createEdgeIndexMap(edgeList);

            // Now the index list is ordered: first the non-tree edges, then the tree edge.
            // Moreover, since the cycles and the corresponding non-tree edge have been added
            // to their lists in the same order, the incidence matrix is in upper triangular form.

            // Now we can minimize the cycles created from the tree base
            minimize(startIndex);
        }

        internal virtual bool[][] getCycleEdgeIncidenceMatrix()
        {
            return getCycleEdgeIncidenceMatrix((System.Object[])SupportClass.ICollectionSupport.ToArray(cycles_Renamed_Field));
        }


        internal virtual bool[][] getCycleEdgeIncidenceMatrix(System.Object[] cycleArray)
        {
            bool[][] result = new bool[cycleArray.Length][];
            for (int i = 0; i < cycleArray.Length; i++)
            {
                result[i] = new bool[edgeList.Count];
            }

            for (int i = 0; i < cycleArray.Length; i++)
            {
                SimpleCycle cycle = (SimpleCycle)cycleArray[i];
                for (int j = 0; j < edgeList.Count; j++)
                {
                    Edge edge = (Edge)edgeList[j];
                    result[i][j] = cycle.containsEdge(edge);
                }
            }

            return result;
        }

        //	private void minimize() {
        //		
        //		if (isMinimized) 
        //			return;
        //		
        //		if (cycles.size()==0) 
        //			return;
        //		else 
        //			minimize(0);
        //		
        //		isMinimized = true;
        //	}

        private void minimize(int startIndex)
        {

            if (isMinimized)
                return;

            // Implementation of "Algorithm 1" from [BGdV04]

            bool[][] a = getCycleEdgeIncidenceMatrix();

            for (int i = startIndex; i < cycles_Renamed_Field.Count; i++)
            {
                // "Subroutine 2"

                // Construct kernel vector u
                bool[] u = constructKernelVector(edgeList.Count, a, i);

                // Construct auxiliary graph gu
                AuxiliaryGraph gu = new AuxiliaryGraph(this, graph, u);

                SimpleCycle shortestCycle = (SimpleCycle)cycles_Renamed_Field[i];

                System.Collections.IEnumerator vertexIterator = graph.vertexSet().GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (vertexIterator.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    System.Object vertex = vertexIterator.Current;

                    // check if the vertex is incident to an edge with u[edge] == 1
                    bool shouldSearchCycle = false;

                    System.Collections.ICollection incidentEdges = graph.edgesOf(vertex);

                    System.Collections.IEnumerator edgeIterator = incidentEdges.GetEnumerator();
                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    while (edgeIterator.MoveNext())
                    {
                        //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                        Edge edge = (Edge)edgeIterator.Current;
                        int index = getEdgeIndex(edge);
                        if (u[index])
                        {
                            shouldSearchCycle = true;
                            break;
                        }
                    }

                    if (shouldSearchCycle)
                    {

                        System.Object auxVertex0 = gu.auxVertex0(vertex);
                        System.Object auxVertex1 = gu.auxVertex1(vertex);

                        // Search for shortest path

                        System.Collections.IList auxPath = BFSShortestPath.findPathBetween(gu, auxVertex0, auxVertex1);

                        System.Collections.IList edgesOfNewCycle = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(10));

                        System.Object v = vertex;

                        edgeIterator = auxPath.GetEnumerator();
                        //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                        while (edgeIterator.MoveNext())
                        {
                            //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                            Edge auxEdge = (Edge)edgeIterator.Current;

                            // Get the edge corresponding to the aux. edge
                            Edge e = (Edge)gu.edge(auxEdge);

                            edgesOfNewCycle.Add(e);

                            // Get next vertex on path
                            v = e.oppositeVertex(v);
                        }

                        SimpleCycle newCycle = new SimpleCycle(graph, edgesOfNewCycle);

                        if (newCycle.weight() < shortestCycle.weight())
                        {
                            shortestCycle = newCycle;
                        }
                    }
                }

                cycles_Renamed_Field[i] = shortestCycle;

                // insert the new cycle into the matrix
                for (int j = 1; j < edgeList.Count; j++)
                {
                    a[i][j] = shortestCycle.containsEdge((Edge)edgeList[j]);
                }

                // perform gaussian elimination on the inserted row
                for (int j = 0; j < i; j++)
                {
                    if (a[i][j])
                    {
                        for (int k = 0; k < edgeList.Count; k++)
                        {
                            a[i][k] = (a[i][k] != a[j][k]);
                        }
                    }
                }
            }

            isMinimized = true;
        }

        internal static bool[] constructKernelVector(int size, bool[][] a, int i)
        {
            // Construct kernel vector u by setting u[i] = true ...
            bool[] u = new bool[size];
            u[i] = true;

            // ... u[j] = 0 (false) for j > i (by initialization)...

            // ... and solving A u = 0

            for (int j = i - 1; j >= 0; j--)
            {
                u[j] = false;
                for (int k = i; k > j; k--)
                {
                    u[j] = (u[j] != (a[j][k] && u[k]));
                }
            }
            return u;
        }


        public virtual void printIncidenceMatrix()
        {

            /*
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

            bool[][] incidMatr = getCycleEdgeIncidenceMatrix();
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

            int[] result = new int[cycles_Renamed_Field.Count];
            for (int i = 0; i < cycles_Renamed_Field.Count; i++)
            {
                SimpleCycle cycle = (SimpleCycle)cycles_Renamed_Field[i];
                //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
                result[i] = (int)cycle.weight();
            }
            System.Array.Sort(result);

            return result;
        }

        public virtual System.Collections.IList edges()
        {
            return edgeList;
        }

        public virtual System.Collections.IList cycles()
        {
            return cycles_Renamed_Field;
        }

        internal static bool[][] inverseBinaryMatrix(bool[][] m, int n)
        {

            bool[][] a = new bool[n][];
            for (int i = 0; i < n; i++)
            {
                a[i] = new bool[n];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = m[i][j];
                }
            }

            bool[][] r = new bool[n][];
            for (int i2 = 0; i2 < n; i2++)
            {
                r[i2] = new bool[n];
            }

            for (int i = 0; i < n; i++)
            {
                r[i][i] = true;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (a[j][i])
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if ((k != j) && (a[k][i]))
                            {
                                for (int l = 0; l < n; l++)
                                {
                                    a[k][l] = (a[k][l] != a[j][l]);
                                    r[k][l] = (r[k][l] != r[j][l]);
                                }
                            }
                        }
                        if (i != j)
                        {
                            bool[] swap = a[i];
                            a[i] = a[j];
                            a[j] = swap;
                            swap = r[i];
                            r[i] = r[j];
                            r[j] = swap;
                        }
                        break;
                    }
                }
            }

            return r;
        }

        public virtual System.Collections.ICollection essentialCycles()
        {
            //UPGRADE_TODO: Class 'java.util.HashSet' was converted to 'CSGraphT.SupportClass.HashSetSupport' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashSet'"
            System.Collections.ICollection result = new CSGraphT.SupportClass.HashSetSupport();

            bool[][] a = getCycleEdgeIncidenceMatrix();

            bool[][] ai = inverseBinaryMatrix(a, cycles_Renamed_Field.Count);

            for (int i = 0; i < cycles_Renamed_Field.Count; i++)
            {

                // Construct kernel vector u
                bool[] u = new bool[edgeList.Count];
                for (int j = 0; j < cycles_Renamed_Field.Count; j++)
                {
                    u[j] = ai[j][i];
                }

                // Construct kernel vector u from a column of the inverse of a
                AuxiliaryGraph gu = new AuxiliaryGraph(this, graph, u);

                bool isEssential = true;

                System.Collections.IEnumerator vertexIterator = graph.vertexSet().GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (isEssential && vertexIterator.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    System.Object vertex = vertexIterator.Current;

                    System.Collections.ICollection incidentEdges = graph.edgesOf(vertex);

                    // check if the vertex is incident to an edge with u[edge] == 1
                    bool shouldSearchCycle = false;

                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    for (System.Collections.IEnumerator it = incidentEdges.GetEnumerator(); it.MoveNext(); )
                    {
                        //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                        Edge edge = (Edge)it.Current;
                        int index = getEdgeIndex(edge);
                        if (u[index])
                        {
                            shouldSearchCycle = true;
                            break;
                        }
                    }

                    if (shouldSearchCycle)
                    {

                        System.Object auxVertex0 = gu.auxVertex0(vertex);
                        System.Object auxVertex1 = gu.auxVertex1(vertex);


                        // Search for shortest paths
                        //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                        for (System.Collections.IEnumerator minPaths = new MinimalPathIterator(gu, auxVertex0, auxVertex1); minPaths.MoveNext(); )
                        {
                            //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                            System.Collections.IList auxPath = (System.Collections.IList)minPaths.Current;
                            System.Collections.IList edgesOfNewCycle = new System.Collections.ArrayList(auxPath.Count);

                            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                            for (System.Collections.IEnumerator it = auxPath.GetEnumerator(); it.MoveNext(); )
                            {
                                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                                Edge auxEdge = (Edge)it.Current;

                                // Get the edge corresponding to the aux. edge
                                Edge e = (Edge)gu.edge(auxEdge);

                                edgesOfNewCycle.Add(e);
                            }

                            SimpleCycle cycle = new SimpleCycle(graph, edgesOfNewCycle);

                            if (cycle.weight() > ((SimpleCycle)cycles_Renamed_Field[i]).weight())
                            {
                                break;
                            }

                            if (!cycle.Equals((SimpleCycle)cycles_Renamed_Field[i]))
                            {
                                isEssential = false;
                                break;
                            }
                        }
                    }
                }

                if (isEssential)
                {
                    SupportClass.ICollectionSupport.Add(result, (SimpleCycle)cycles_Renamed_Field[i]);
                }
            }

            return result;
        }


        public virtual System.Collections.IDictionary relevantCycles()
        {
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.IDictionary result = new System.Collections.Hashtable();

            bool[][] a = getCycleEdgeIncidenceMatrix();

            bool[][] ai = inverseBinaryMatrix(a, cycles_Renamed_Field.Count);

            for (int i = 0; i < cycles_Renamed_Field.Count; i++)
            {

                // Construct kernel vector u from a column of the inverse of a
                bool[] u = new bool[edgeList.Count];
                for (int j = 0; j < cycles_Renamed_Field.Count; j++)
                {
                    u[j] = ai[j][i];
                }

                // Construct auxiliary graph gu
                AuxiliaryGraph gu = new AuxiliaryGraph(this, graph, u);

                System.Collections.IEnumerator vertexIterator = graph.vertexSet().GetEnumerator();
                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                while (vertexIterator.MoveNext())
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    System.Object vertex = vertexIterator.Current;

                    System.Collections.ICollection incidentEdges = graph.edgesOf(vertex);

                    // check if the vertex is incident to an edge with u[edge] == 1
                    bool shouldSearchCycle = false;

                    //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                    for (System.Collections.IEnumerator it = incidentEdges.GetEnumerator(); it.MoveNext(); )
                    {
                        //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                        Edge edge = (Edge)it.Current;
                        int index = getEdgeIndex(edge);
                        if (u[index])
                        {
                            shouldSearchCycle = true;
                            break;
                        }
                    }

                    if (shouldSearchCycle)
                    {

                        System.Object auxVertex0 = gu.auxVertex0(vertex);
                        System.Object auxVertex1 = gu.auxVertex1(vertex);

                        // Search for shortest paths

                        //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                        for (System.Collections.IEnumerator minPaths = new MinimalPathIterator(gu, auxVertex0, auxVertex1); minPaths.MoveNext(); )
                        {
                            //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                            System.Collections.IList auxPath = (System.Collections.IList)minPaths.Current;
                            System.Collections.IList edgesOfNewCycle = new System.Collections.ArrayList(auxPath.Count);

                            System.Collections.IEnumerator edgeIterator = auxPath.GetEnumerator();
                            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                            while (edgeIterator.MoveNext())
                            {
                                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                                Edge auxEdge = (Edge)edgeIterator.Current;

                                // Get the edge corresponding to the aux. edge
                                Edge e = (Edge)gu.edge(auxEdge);

                                edgesOfNewCycle.Add(e);
                            }


                            SimpleCycle cycle = new SimpleCycle(graph, edgesOfNewCycle);

                            if (cycle.weight() > ((SimpleCycle)cycles_Renamed_Field[i]).weight())
                            {
                                break;
                            }

                            result[cycle] = (SimpleCycle)cycles_Renamed_Field[i];
                        }
                    }
                }
            }

            return result;
        }


        public virtual System.Collections.IList equivalenceClasses()
        {
            int[] weight = weightVector();

            System.Object[] cyclesArray = (System.Object[])SupportClass.ICollectionSupport.ToArray(cycles_Renamed_Field);
            System.Array.Sort(cyclesArray, new AnonymousClassComparator(this));

            System.Collections.ICollection essentialCycles = this.essentialCycles();

            bool[][] u = new bool[cyclesArray.Length][];
            for (int i = 0; i < cyclesArray.Length; i++)
            {
                u[i] = new bool[edgeList.Count];
            }

            bool[][] a = getCycleEdgeIncidenceMatrix(cyclesArray);
            bool[][] ai = inverseBinaryMatrix(a, cyclesArray.Length);

            for (int i = 0; i < cyclesArray.Length; i++)
            {
                for (int j = 0; j < cyclesArray.Length; j++)
                {
                    u[i][j] = ai[j][i];
                }
            }

            UndirectedGraph h = new SimpleGraph();
            h.addAllVertices(cycles_Renamed_Field);

            ConnectivityInspector connectivityInspector = new ConnectivityInspector(h);

            int left = 0;
            for (int right = 0; right < weight.Length; right++)
            {
                if ((right < weight.Length - 1) && (weight[right + 1] == weight[right]))
                    continue;

                // cyclesArray[left] to cyclesArray[right] have same weight

                // First test (compute pre-classes):
                // Check if there is a cycle that can replace a[i] as well as a[j] in a basis
                // This is done by finding a cycle C with <C,u[i]>=1 and <C,u[j]>=1

                for (int i = left; i <= right; i++)
                {
                    if (SupportClass.ICollectionSupport.Contains(essentialCycles, (SimpleCycle)cyclesArray[i]))
                        continue;

                    for (int j = i + 1; j <= right; j++)
                    {
                        if (SupportClass.ICollectionSupport.Contains(essentialCycles, (SimpleCycle)cyclesArray[j]))
                            continue;

                        // check if cyclesArray[i] and cyclesArray[j] are already in the same class
                        if (connectivityInspector.pathExists(cyclesArray[i], cyclesArray[j]))
                            continue;

                        bool sameClass = false;

                        AuxiliaryGraph2 auxGraph = new AuxiliaryGraph2(this, graph, edgeList, u[i], u[j]);

                        //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                        for (System.Collections.IEnumerator it = graph.vertexSet().GetEnumerator(); it.MoveNext(); )
                        {
                            //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                            System.Object vertex = it.Current;

                            // check if the vertex is incident to an edge with u[edge] == 1
                            bool shouldSearchCycle = false;

                            System.Collections.ICollection incidentEdges = graph.edgesOf(vertex);

                            System.Collections.IEnumerator edgeIterator = incidentEdges.GetEnumerator();
                            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                            while (edgeIterator.MoveNext())
                            {
                                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                                Edge edge = (Edge)edgeIterator.Current;
                                int index = getEdgeIndex(edge);
                                if (u[i][index] || u[j][index])
                                {
                                    shouldSearchCycle = true;
                                    break;
                                }
                            }

                            if (shouldSearchCycle)
                            {

                                System.Object auxVertex00 = auxGraph.auxVertex00(vertex);
                                System.Object auxVertex11 = auxGraph.auxVertex11(vertex);

                                System.Collections.IList auxPath = BFSShortestPath.findPathBetween(auxGraph, auxVertex00, auxVertex11);

                                double pathWeight = auxPath.Count;

                                if (pathWeight == weight[left])
                                {
                                    sameClass = true;
                                    break;
                                }
                            }
                        }

                        if (sameClass)
                        {
                            h.addEdge(cyclesArray[i], cyclesArray[j]);
                        }
                    }
                }

                // Second test (compute equivalence classes):
                // Check if there are two cycle Ci, Cj that can replace a[i], a[j]
                // and have a common cycle a[k] in their basis representation
                // This is done by finding a cycle a[k] with <u[k],u[i]>=1 and <u[k],u[j]>=1

                for (int i = left; i <= right; i++)
                {
                    if (SupportClass.ICollectionSupport.Contains(essentialCycles, (SimpleCycle)cyclesArray[i]))
                        continue;

                    for (int j = i + 1; j <= right; j++)
                    {
                        if (SupportClass.ICollectionSupport.Contains(essentialCycles, (SimpleCycle)cyclesArray[j]))
                            continue;

                        // check if cyclesArray[i] and cyclesArray[j] are already in the same class
                        if (connectivityInspector.pathExists(cyclesArray[i], cyclesArray[j]))
                            continue;

                        bool sameClass = false;

                        for (int k = 0; ((SimpleCycle)cyclesArray[k]).weight() < weight[left]; k++)
                        {

                            AuxiliaryGraph2 auxGraph = new AuxiliaryGraph2(this, graph, edgeList, u[i], u[k]);

                            bool shortestPathFound = false;
                            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                            for (System.Collections.IEnumerator it = graph.vertexSet().GetEnumerator(); it.MoveNext(); )
                            {
                                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                                System.Object vertex = it.Current;

                                System.Object auxVertex00 = auxGraph.auxVertex00(vertex);
                                System.Object auxVertex11 = auxGraph.auxVertex11(vertex);

                                System.Collections.IList auxPath = BFSShortestPath.findPathBetween(auxGraph, auxVertex00, auxVertex11);

                                double pathWeight = auxPath.Count;

                                if (pathWeight == weight[left])
                                {
                                    shortestPathFound = true;
                                    break;
                                }
                            }

                            if (!shortestPathFound)
                                continue;

                            auxGraph = new AuxiliaryGraph2(this, graph, edgeList, u[j], u[k]);

                            //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                            for (System.Collections.IEnumerator it = graph.vertexSet().GetEnumerator(); it.MoveNext(); )
                            {
                                //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                                System.Object vertex = it.Current;

                                System.Object auxVertex00 = auxGraph.auxVertex00(vertex);
                                System.Object auxVertex11 = auxGraph.auxVertex11(vertex);

                                System.Collections.IList auxPath = BFSShortestPath.findPathBetween(auxGraph, auxVertex00, auxVertex11);

                                double pathWeight = auxPath.Count;

                                if (pathWeight == weight[left])
                                {
                                    sameClass = true;
                                    break;
                                }
                            }

                            if (sameClass)
                                break;
                        }

                        if (sameClass)
                        {
                            h.addEdge(cyclesArray[i], cyclesArray[j]);
                        }
                    }
                }

                left = right + 1;
            }

            return connectivityInspector.connectedSets();
        }

        //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
        private System.Collections.Hashtable createEdgeIndexMap(System.Collections.IList edgeList)
        {
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            System.Collections.Hashtable map = new System.Collections.Hashtable();
            for (int i = 0; i < edgeList.Count; i++)
            {
                map[edgeList[i]] = (System.Int32)i;
            }
            return map;
        }

        private int getEdgeIndex(Edge edge)
        {
            //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
            return ((System.Int32)edgeIndexMap[edge]);
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AuxiliaryGraph' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AuxiliaryGraph : SimpleGraph
        {
            private void InitBlock(SimpleCycleBasis enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SimpleCycleBasis enclosingInstance;
            public SimpleCycleBasis Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }

            private const long serialVersionUID = 857337988734567429L;
            // graph to aux. graph
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            internal System.Collections.Hashtable vertexMap0 = new System.Collections.Hashtable();
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            internal System.Collections.Hashtable vertexMap1 = new System.Collections.Hashtable();

            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            internal System.Collections.Hashtable auxVertexMap = new System.Collections.Hashtable();

            // aux. edge to edge
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            internal System.Collections.IDictionary auxEdgeMap = new System.Collections.Hashtable();

            internal org._3pq.jgrapht.Graph g;
            internal bool[] u;

            public AuxiliaryGraph(SimpleCycleBasis enclosingInstance, org._3pq.jgrapht.Graph graph, bool[] u)
            {
                InitBlock(enclosingInstance);
                g = graph;
                this.u = u;
            }

            public virtual System.Collections.IList edgesOf(System.Object auxVertex)
            {

                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                System.Object vertex = auxVertexMap[auxVertex];

                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                for (System.Collections.IEnumerator edgeIterator = g.edgesOf(vertex).GetEnumerator(); edgeIterator.MoveNext(); )
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    Edge edge = (Edge)edgeIterator.Current;
                    int j = enclosingInstance.getEdgeIndex(edge);

                    System.Object vertex1 = edge.Source;
                    System.Object vertex2 = edge.Target;

                    if (u[j])
                    {
                        System.Object vertex1u = auxVertex0(vertex1);
                        System.Object vertex2u = auxVertex1(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex1(vertex1);
                        vertex2u = auxVertex0(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                    else
                    {
                        System.Object vertex1u = auxVertex0(vertex1);
                        System.Object vertex2u = auxVertex0(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex1(vertex1);
                        vertex2u = auxVertex1(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                }

                return base.edgesOf(auxVertex);
            }

            internal virtual System.Object auxVertex0(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap0[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex0 = vertex + "-0";
                    vertexMap0[vertex] = newVertex0;
                    addVertex(newVertex0);
                    auxVertexMap[newVertex0] = vertex;
                    return newVertex0;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap0[vertex];
            }

            internal virtual System.Object auxVertex1(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap1[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex1 = vertex + "-1";
                    vertexMap1[vertex] = newVertex1;
                    addVertex(newVertex1);
                    auxVertexMap[newVertex1] = vertex;
                    return newVertex1;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap1[vertex];
            }

            internal virtual System.Object edge(System.Object auxEdge)
            {
                return auxEdgeMap[auxEdge];
            }
        }

        //UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'AuxiliaryGraph2' to access its enclosing instance. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1019'"
        private class AuxiliaryGraph2 : SimpleGraph
        {
            private void InitBlock(SimpleCycleBasis enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SimpleCycleBasis enclosingInstance;
            public SimpleCycleBasis Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }

            private const long serialVersionUID = 5930876716644738726L;

            // graph to aux. graph
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.Hashtable vertexMap00 = new System.Collections.Hashtable();
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.Hashtable vertexMap01 = new System.Collections.Hashtable();
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.Hashtable vertexMap10 = new System.Collections.Hashtable();
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.Hashtable vertexMap11 = new System.Collections.Hashtable();

            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.Hashtable auxVertexMap = new System.Collections.Hashtable();

            // aux. edge to edge
            //UPGRADE_TODO: Class 'java.util.HashMap' was converted to 'System.Collections.Hashtable' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMap'"
            private System.Collections.IDictionary auxEdgeMap = new System.Collections.Hashtable();

            private org._3pq.jgrapht.Graph g;
            private bool[] ui;
            private bool[] uj;

            internal AuxiliaryGraph2(SimpleCycleBasis enclosingInstance, org._3pq.jgrapht.Graph graph, System.Collections.IList edgeList, bool[] ui, bool[] uj)
            {
                InitBlock(enclosingInstance);
                g = graph;
                this.ui = ui;
                this.uj = uj;
            }

            internal virtual System.Object auxVertex00(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap00[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex = vertex + "-00";
                    vertexMap00[vertex] = newVertex;
                    addVertex(newVertex);
                    auxVertexMap[newVertex] = vertex;
                    return newVertex;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap00[vertex];
            }

            internal virtual System.Object auxVertex01(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap01[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex = vertex + "-01";
                    vertexMap01[vertex] = newVertex;
                    addVertex(newVertex);
                    auxVertexMap[newVertex] = vertex;
                    return newVertex;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap01[vertex];
            }

            internal virtual System.Object auxVertex10(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap10[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex = vertex + "-10";
                    vertexMap10[vertex] = newVertex;
                    addVertex(newVertex);
                    auxVertexMap[newVertex] = vertex;
                    return newVertex;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap10[vertex];
            }

            internal virtual System.Object auxVertex11(System.Object vertex)
            {
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                if (vertexMap11[vertex] == null)
                {
                    //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
                    System.Object newVertex = vertex + "-11";
                    vertexMap11[vertex] = newVertex;
                    addVertex(newVertex);
                    auxVertexMap[newVertex] = vertex;
                    return newVertex;
                }
                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                return vertexMap11[vertex];
            }

            public virtual System.Collections.IList edgesOf(System.Object auxVertex)
            {

                //UPGRADE_TODO: Method 'java.util.HashMap.get' was converted to 'System.Collections.Hashtable.Item' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilHashMapget_javalangObject'"
                System.Object vertex = auxVertexMap[auxVertex];

                //UPGRADE_TODO: Method 'java.util.Iterator.hasNext' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratorhasNext'"
                for (System.Collections.IEnumerator edgeIterator = g.edgesOf(vertex).GetEnumerator(); edgeIterator.MoveNext(); )
                {
                    //UPGRADE_TODO: Method 'java.util.Iterator.next' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilIteratornext'"
                    Edge edge = (Edge)edgeIterator.Current;
                    int k = enclosingInstance.getEdgeIndex(edge);

                    System.Object vertex1 = edge.Source;
                    System.Object vertex2 = edge.Target;

                    if (!ui[k] && !uj[k])
                    {
                        System.Object vertex1u = auxVertex00(vertex1);
                        System.Object vertex2u = auxVertex00(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex01(vertex1);
                        vertex2u = auxVertex01(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex10(vertex1);
                        vertex2u = auxVertex10(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex11(vertex1);
                        vertex2u = auxVertex11(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                    else if (ui[k] && !uj[k])
                    {
                        System.Object vertex1u = auxVertex00(vertex1);
                        System.Object vertex2u = auxVertex10(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex01(vertex1);
                        vertex2u = auxVertex11(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex10(vertex1);
                        vertex2u = auxVertex00(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex11(vertex1);
                        vertex2u = auxVertex01(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                    else if (!ui[k] && uj[k])
                    {
                        System.Object vertex1u = auxVertex00(vertex1);
                        System.Object vertex2u = auxVertex01(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex01(vertex1);
                        vertex2u = auxVertex00(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex10(vertex1);
                        vertex2u = auxVertex11(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex11(vertex1);
                        vertex2u = auxVertex10(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                    else if (ui[k] && uj[k])
                    {
                        System.Object vertex1u = auxVertex00(vertex1);
                        System.Object vertex2u = auxVertex11(vertex2);
                        Edge auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex01(vertex1);
                        vertex2u = auxVertex10(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex10(vertex1);
                        vertex2u = auxVertex01(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;

                        vertex1u = auxVertex11(vertex1);
                        vertex2u = auxVertex00(vertex2);
                        auxEdge = addEdge(vertex1u, vertex2u);
                        auxEdgeMap[auxEdge] = edge;
                    }
                }
                return base.edgesOf(auxVertex);
            }
        }
    }
}
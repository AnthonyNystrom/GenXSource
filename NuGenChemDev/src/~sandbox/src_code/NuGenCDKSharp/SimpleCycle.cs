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
using System.Collections.Generic;
using System.Text;
using org._3pq.jgrapht;
using System.Collections;
using org._3pq.jgrapht.graph;

namespace Org.OpenScience.CDK.Ringsearch.Cyclebasis
{
    /// <summary>
    /// A cycle in a graph.
    /// A cycle in a graph G is a subgraph in which every vertex has even degree.
    /// 
    /// @author Ulrich Bauer <baueru@cs.tum.edu>
    /// 
    /// 
    /// @cdk.module standard
    /// 
    /// @cdk.keyword smallest-set-of-rings
    /// @cdk.keyword ring search
    /// 
    /// @cdk.builddepends jgrapht-0.5.3.jar
    /// @cdk.depends jgrapht-0.5.3.jar
    /// </summary>
    class SimpleCycle : UndirectedSubgraph
    {
        private const long serialVersionUID = -3330742084804445688L;

        /// <summary>
        /// Constructs a cycle in a graph consisting of the specified edges.
        /// </summary>
        /// <param name="g">the graph in which the cycle is contained</param>
        /// <param name="edges">the edges of the cycle</param>
	    public SimpleCycle (UndirectedGraph g, ICollection edges)
		    : base(g, new CSGraphT.SupportClass.HashSetSupport(edges))
        { }

        static private IList inducedVertices(IList edges)
        {
            IList inducedVertices = new CSGraphT.SupportClass.HashSetSupport();
            foreach (Edge edge in edges)
            {
                inducedVertices.Add(edge.Source);
                inducedVertices.Add(edge.Target);
            }
            return inducedVertices;
        }

        /// <summary>
        /// Returns the sum of the weights of all edges in this cycle.
        /// </summary>
        /// <returns>the sum of the weights of all edges in this cycle</returns>
        public double weight()
        {
            double result = 0;
            foreach (Edge edge in edgeSet())
            {
                result += edge.Weight;
            }
            return result;
        }

        /// <summary>
        /// Returns a list of the vertices contained in this cycle.
        /// The vertices are in the order of a traversal of the cycle.
        /// </summary>
        /// <returns>a list of the vertices contained in this cycle</returns>
        public IList vertexList()
        {
            IList vertices = new ArrayList(edgeSet().Count);

            Object startVertex = vertexSet()[0];

            Object vertex = startVertex;
            Object previousVertex = null;
            Object nextVertex = null;

            while (nextVertex != startVertex)
            {
                vertices.Add(vertex);

                Edge edge = (Edge)edgesOf(vertex)[0];
                nextVertex = edge.oppositeVertex(vertex);

                if (nextVertex == previousVertex)
                {
                    edge = (Edge)edgesOf(vertex)[1];
                    nextVertex = edge.oppositeVertex(vertex);
                }

                previousVertex = vertex;
                vertex = nextVertex;
            }

            return vertices;
        }
        public override bool Equals(object obj)
        {
            return (obj is SimpleCycle && edgeSet().Equals(((SimpleCycle)obj).edgeSet()));
        }

        public override string ToString()
        {
            return vertexList().ToString();
        }

        public override int GetHashCode()
        {
            return edgeSet().GetHashCode();
        }
    }
}
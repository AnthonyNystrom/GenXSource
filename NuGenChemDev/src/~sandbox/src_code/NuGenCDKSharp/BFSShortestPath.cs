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
using System.Collections.Generic;
using System.Text;
using org._3pq.jgrapht;
using System.Collections;
using org._3pq.jgrapht.traverse;

namespace NuGenCDKSharp
{
    /// <summary>
    /// Utility class that finds the shortest bond path between two atoms using
    /// a breadth first search.@author uli
    /// 
    /// @cdk.module  standard
    /// @cdk.created 2004-10-19
    /// </summary>
    class BFSShortestPath
    {
        private BFSShortestPath() { } // ensure non-instantiability.

        public static IList findPathBetween(Graph graph, Object startVertex,
                                           Object endVertex)
        {
            MyBreadthFirstIterator iter = new MyBreadthFirstIterator(graph, startVertex);

            while (iter.MoveNext())
            {
                object vertex = iter.Current;
                if (vertex.Equals(endVertex))
                {
                    return createPath(iter, endVertex);
                }
            }

            return null;
        }

        private static IList createPath(MyBreadthFirstIterator iter, Object endVertex)
        {
            ArrayList path = new ArrayList();

            while(true)
            {
                Edge edge = iter.getSpanningTreeEdge(endVertex);
                if(edge == null)
                {
                    break;
                }

                path.Add(edge);
                endVertex = edge.oppositeVertex( endVertex );
            }

            path.Reverse();
            return path;
        }
    
	    private class MyBreadthFirstIterator : BreadthFirstIterator
        {
		    public MyBreadthFirstIterator(Graph g, Object startVertex)
			    : base(g, startVertex)
            { }
    		
	        protected void encounterVertex(Object vertex, Edge edge)
            {
                base.encounterVertex(vertex, edge);
	            putSeenData(vertex, edge);
	        }

	        public Edge getSpanningTreeEdge(Object vertex)
            {
	            return (Edge)getSeenData( vertex );
	        }
    		
	    }
    }
}

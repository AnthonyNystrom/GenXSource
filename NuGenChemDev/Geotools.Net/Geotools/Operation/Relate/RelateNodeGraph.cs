/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

#region Using
using System;
using System.Collections;
using System.Diagnostics;
using Geotools.Graph;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Summary description for RelateNodeGraph.
	/// </summary>
	internal class RelateNodeGraph : IEnumerable
	{
		private NodeMap _nodes = new NodeMap( new RelateNodeFactory() );

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the RelateNodeGraph class.
		/// </summary>
		public RelateNodeGraph()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Implement IEnumerable
		public IEnumerator GetEnumerator()
		{
			return _nodes.GetEnumerator();
		}
		#endregion
		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geomGraph"></param>
		public void Build(GeometryGraph geomGraph)
		{
			// compute nodes for intersections between previously noded edges
			ComputeIntersectionNodes( geomGraph, 0 );
			// Copy the labelling for the nodes in the parent Geometry.  These override
			// any labels determined by intersections.
			CopyNodesAndLabels( geomGraph, 0 );

			// Build EdgeEnds for all intersections.
			EdgeEndBuilder eeBuilder = new EdgeEndBuilder();
			ArrayList eeList = eeBuilder.ComputeEdgeEnds( geomGraph.Edges );
			InsertEdgeEnds( eeList );

			//Trace.WriteLine("==== NodeList ===");
			//Trace.WriteLine( _nodes.ToString() );
		} // public void Build(GeometryGraph geomGraph)

		/// <summary>
		/// Insert nodes for all intersections on the edges of a Geometry.
		/// Label the created nodes the same as the edge label if they do not already have a label.
		///	This allows nodes created by either self-intersections or
		///	mutual intersections to be labelled.
		///	Endpoint nodes will already be labelled from when they were inserted.
		/// Precondition: edge intersections have been computed.
		/// </summary>
		/// <param name="geomGraph"></param>
		/// <param name="argIndex"></param>
		public void ComputeIntersectionNodes( GeometryGraph geomGraph, int argIndex )
		{
			foreach ( object obj in geomGraph.Edges ) 
			{
				Edge e = (Edge) obj;
				int eLoc = e.Label.GetLocation( argIndex );
				foreach ( object objEdgeIntersection in e.EdgeIntersectionList ) 
				{
					EdgeIntersection ei = (EdgeIntersection) objEdgeIntersection;
					RelateNode n = (RelateNode) _nodes.AddNode( ei.Coordinate );
					if ( eLoc == Location.Boundary )
					{
						n.SetLabelBoundary( argIndex );
					}
					else 
					{
						if ( n.Label.IsNull( argIndex ) )
						{
							n.SetLabel( argIndex, Location.Interior );
						} // if ( n.Label.IsNull( argIndex ) )
					}
					//Trace.WriteLine( n.ToString() );
				}
			} // foreach ( object obj in geomGraph.Edges )
		} // public void ComputeIntersectionNodes( GeometryGraph geomGraph, int argIndex )

		/// <summary>
		/// Copy all nodes from an arg geometry into this graph.
		///	The node label in the arg geometry overrides any previously computed
		///	label for that argIndex.
		///	(E.g. a node may be an intersection node with
		///	a computed label of BOUNDARY,
		/// but in the original arg Geometry it is actually
		///	in the interior due to the Boundary Determination Rule)
		/// </summary>
		/// <param name="geomGraph"></param>
		/// <param name="argIndex"></param>
		public void CopyNodesAndLabels( GeometryGraph geomGraph, int argIndex )
		{
			foreach ( DictionaryEntry obj in geomGraph.Nodes ) 
			{
				Node graphNode = (Node) obj.Value;
				Node newNode = _nodes.AddNode( graphNode.GetCoordinate() );
				newNode.SetLabel( argIndex, graphNode.Label.GetLocation( argIndex ) );
				//Trace.WriteLine( _node.ToString() );
			} // foreach ( object obj in geomGraph.Nodes )
		} // public void CopyNodesAndLabels( GeometryGraph geomGraph, int argIndex )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ee"></param>
		public void InsertEdgeEnds( ArrayList ee)
		{
			foreach ( object obj in ee ) 
			{
				EdgeEnd e = (EdgeEnd) obj;
				_nodes.Add( e );
			} // foreach ( object obj in ee )
		} // public void InsertEdgeEnds( ArrayList ee)
		
		#endregion

	} // public class RelateNodeGraph
}

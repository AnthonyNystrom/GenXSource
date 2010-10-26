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

#region Using statements
using System;
using System.Collections;
using System.Diagnostics;
using Geotools.Geometries;
using Geotools.Algorithms;
using Geotools.Graph;
using Geotools.Graph.Index;
#endregion

namespace Geotools.Operation.Relate
{
	/// <summary>
	/// Note that RelateComputer does not need to build a complete graph structure to compute
	/// the IntersectionMatrix.  The relationship between the geometries can
	/// be computed by simply examining the labelling of edges incident on each node.
	/// </summary>
	/// <remarks>
	/// RelateComputer does not currently support arbitrary GeometryCollections.
	/// This is because GeometryCollections can contain overlapping Polygons.
	/// In order to correct compute relate on overlapping Polygons, they
	/// would first need to be noded and merged (if not explicitly, at least
	/// implicitly).
	/// </remarks>
	internal class RelateComputer
	{
		private static LineIntersector _li = new RobustLineIntersector();		// this was public...???TODO: check to see why it was public.
		private static PointLocator _ptLocator = new PointLocator();

		private GeometryGraph[] _arg = null;  // the arg(s) of the operation ArrayList of GeometryGraph[]
		private NodeMap _nodes = new NodeMap( new RelateNodeFactory() );
		// this intersection matrix will hold the results compute for the relate
		//private IntersectionMatrix _im = null;		don't know what this is for...
		private ArrayList _isolatedEdges = new ArrayList();

		// the intersection point found (if any)
		private Coordinate _invalidPoint = null;

		#region Constructor
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="arg">Array of GeometryGraph objects.</param>
		public RelateComputer( GeometryGraph[] arg ) 
		{
			_arg = arg;
		} // public RelateComputer(GeometryGraph[] arg)
		#endregion

		#region Properties
		/// <summary>
		/// Gets the intersection point, or null if none was found.
		/// </summary>
		/// <returns>Returns the intersection point or null in none found.</returns>
		public Coordinate GetInvalidPoint() 
		{ 
			return _invalidPoint; 
		} // public Coordinate GetInvalidPoint()

		#endregion

		#region Public Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsNodeConsistentArea()
		{
			SegmentIntersector intersector = _arg[0].ComputeSelfNodes( _li );
			if ( intersector.HasProperIntersection ) 
			{
				_invalidPoint = intersector.ProperIntersectionPoint;
				return false;
			}

			// compute intersections between edges
			ComputeIntersectionNodes( 0 );
			
			 //Copy the labelling for the nodes in the parent Geometry.  These override
			 //any labels determined by intersections.
			CopyNodesAndLabels( 0 );

			// Build EdgeEnds for all intersections.
			EdgeEndBuilder eeBuilder = new EdgeEndBuilder();
			ArrayList ee0 = eeBuilder.ComputeEdgeEnds( _arg[0].Edges );
			InsertEdgeEnds( ee0 );

			//Trace.WriteLine("==== NodeList ===");
			//Trace.WriteLine( _nodes.ToString() );

			return IsNodeEdgeAreaLabelsConsistent();
		} // public bool IsNodeConsistentArea()

		/// <summary>
		/// Checks for two duplicate rings in an area.
		/// Duplicate rings are rings that are topologically equal
		/// (that is, which have the same sequence of points up to point order).
		/// If the area is topologically consistent (determined by calling the IsNodeConsistentArea,
		/// duplicate rings can be found by checking for EdgeBundles which contain
		/// more than one EdgeEnd.
		///	(This is because topologically consistent areas cannot have two rings sharing
		///	the same line segment, unless the rings are equal).
		/// The start point of one of the equal rings will be placed in
		/// invalidPoint.
		/// </summary>
		/// <returns>Returns true if this area Geometry is topologically consistent but has two 
		/// duplicate rings.</returns>
		public bool HasDuplicateRings()
		{
			foreach ( DictionaryEntry entry in _nodes ) 
			{
				RelateNode node = (RelateNode) entry.Value;
				foreach ( object objEdgeEnd in node.Edges ) 
				{
					EdgeEndBundle eeb = (EdgeEndBundle) objEdgeEnd;
					if ( eeb.EdgeEnds.Count > 1 ) 
					{
						_invalidPoint = eeb.Edge.GetCoordinate();
						return true;
					}
				}
			} // foreach ( object obj in _nodes )
			return false;
		} // public bool HasDuplicateRings()

		/// <summary>
		/// Computes the Intersection matrix for the geometries.
		/// </summary>
		/// <returns></returns>
		public IntersectionMatrix ComputeIM()
		{
			IntersectionMatrix im = new IntersectionMatrix();

			// since Geometries are finite and embedded in a 2-D space, the EE element must always be 2
			im.Set( Location.Exterior, Location.Exterior, 2);

			// if the Geometries don't overlap there is nothing to do
			if ( !_arg[0].Geometry.GetEnvelopeInternal().Intersects( 
				  _arg[1].Geometry.GetEnvelopeInternal() ) ) 
			{
				ComputeDisjointIM( im );
				return im;
			}

			_arg[0].ComputeSelfNodes( _li );
			_arg[1].ComputeSelfNodes( _li );

			// compute intersections between edges of the two input geometries
			SegmentIntersector intersector = _arg[0].ComputeEdgeIntersections( _arg[1], _li, false );
			ComputeIntersectionNodes(0);
			ComputeIntersectionNodes(1);

			 // Copy the labelling for the nodes in the parent Geometries.  These override
			 // any labels determined by intersections between the geometries.
			CopyNodesAndLabels(0);
			CopyNodesAndLabels(1);

			// complete the labelling for any nodes which only have a label for a single geometry
			LabelIsolatedNodes();

			// If a proper intersection was found, we can set a lower bound on the IM.
			ComputeProperIntersectionIM( intersector, im );

			 // Now process improper intersections
			 // (eg where one or other of the geometrys has a vertex at the intersection point)
			 // We need to compute the edge graph at all nodes to determine the IM.

			// build EdgeEnds for all intersections
			EdgeEndBuilder eeBuilder = new EdgeEndBuilder();
			ArrayList ee0 = eeBuilder.ComputeEdgeEnds( _arg[0].Edges );
			InsertEdgeEnds( ee0 );
			ArrayList ee1 = eeBuilder.ComputeEdgeEnds( _arg[1].Edges );
			InsertEdgeEnds( ee1 );

			//Trace.WriteLine("==== NodeList ===");
			//Trace.WriteLine( _nodes.ToString() );

			LabelNodeEdges();

			 // Compute the labeling for isolated components
			 // <br>
			 // Isolated components are components that do not touch any other components in the graph.
			 // They can be identified by the fact that they will
			 // contain labels containing ONLY a single element, the one for their parent geometry.
			 // We only need to check components contained in the input graphs, since
			 // isolated components will not have been replaced by new components formed by intersections.
			//Trace.WriteLine("Graph A isolated edges - ");
			LabelIsolatedEdges(0, 1);
			//Trace.WriteLine("Graph B isolated edges - ");
			LabelIsolatedEdges(1, 0);

			// update the IM from all components
			UpdateIM( im );
			return im;
		} // public IntersectionMatrix ComputeIM()

		#endregion

		#region Private Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ee"></param>
		private void InsertEdgeEnds( ArrayList ee )
		{
			foreach ( object obj in ee ) 
			{
				EdgeEnd e = (EdgeEnd) obj;
				_nodes.Add( e );
			}
		} // private void InsertEdgeEnds( ArrayList ee )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="intersector"></param>
		/// <param name="im"></param>
		private void ComputeProperIntersectionIM( SegmentIntersector intersector, IntersectionMatrix im )
		{
			// If a proper intersection is found, we can set a lower bound on the IM.
			int dimA = _arg[0].Geometry.GetDimension();
			int dimB = _arg[1].Geometry.GetDimension();
			bool hasProper         = intersector.HasProperIntersection;
			bool hasProperInterior = intersector.HasProperInteriorIntersection;

			// For Geometrys of dim 0 there can never be proper intersections.

			 // If edge segments of Areas properly intersect, the areas must properly overlap.
			if ( dimA == 2 && dimB == 2 ) 
			{
				if ( hasProper )
				{
					im.SetAtLeast( "212101212" );
				}
			} // if ( dimA == 2 && dimB == 2 )
			// If an Line segment properly intersects an edge segment of an Area,
			// it follows that the Interior of the Line intersects the Boundary of the Area.
			// If the intersection is a proper <i>interior</i> intersection, then
			// there is an Interior-Interior intersection too.
			// Note that it does not follow that the Interior of the Line intersects the Exterior
			// of the Area, since there may be another Area component which contains the rest of the Line.
			else if ( dimA == 2 && dimB == 1 ) 
			{
				if ( hasProper )
				{
					im.SetAtLeast( "FFF0FFFF2" );
				}
				if ( hasProperInterior )
				{
					im.SetAtLeast( "1FFFFF1FF" );
				}
			} // else if ( dimA == 2 && dimB == 1 )
			else if ( dimA == 1 && dimB == 2 ) 
			{
				if ( hasProper )
				{
					im.SetAtLeast("F0FFFFFF2");
				}
				if ( hasProperInterior )
				{
					im.SetAtLeast("1F1FFFFFF");
				}
			} // else if ( dimA == 1 && dimB == 2 )
				// If edges of LineStrings properly intersect *in an interior point*, all
				//	we can deduce is that
				//	the interiors intersect.  (We can NOT deduce that the exteriors intersect,
				//	since some other segments in the geometries might cover the points in the
				//	neighbourhood of the intersection.)
				//	It is important that the point be known to be an interior point of
				//	both Geometries, since it is possible in a self-intersecting geometry to
				//	have a proper intersection on one segment that is also a boundary point of another segment.
			else if ( dimA == 1 && dimB == 1 ) 
			{
				if ( hasProperInterior )
				{
					im.SetAtLeast( "0FFFFFFFF" );
				}
			} // else if ( dimA == 1 && dimB == 1 )
		} // private void ComputeProperIntersectionIM( SegmentIntersector intersector, IntersectionMatrix im )

		/// <summary>
		/// Copy all nodes from an arg geometry into ths graph.  The node label in the arg geometry
		/// overrides any previously computed label for that argIndex.  (e.g. a node may be an intersection
		/// node with a computed label of BOUNDARY, but in the original arg Geometry it is actually in the 
		/// interior due to the Boundary Determination Rule.)
		/// </summary>
		/// <param name="argIndex"></param>
		private void CopyNodesAndLabels(int argIndex)
		{
			foreach ( DictionaryEntry entry in _arg[argIndex].Nodes ) 
			{
				Node graphNode = (Node) entry.Value;
				Node newNode = _nodes.AddNode( graphNode.GetCoordinate() );
				newNode.SetLabel( argIndex, graphNode.Label.GetLocation( argIndex ) );
				//Trace.WriteLine( _node.ToString() );
			} // foreach ( object obj in _arg[argIndex].Nodes )
		} // private void CopyNodesAndLabels(int argIndex)

		/// <summary>
		/// Insert nodes for all intersections on the edges of a Geometry.
		/// Label the created nodes the same as the edge label if they do not already have a label.
		/// This allows nodes created by either self-intersections or
		///	mutual intersections to be labelled.
		///	Endpoint nodes will already be labelled from when they were inserted.
		/// </summary>
		/// <param name="argIndex"></param>
		private void ComputeIntersectionNodes( int argIndex )
		{
			foreach ( object obj in _arg[argIndex].Edges ) 
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
						}
					}
					//Trace.WriteLine( n.ToString() );
				}
			} // foreach ( object obj in _arg[argIndex].Edges )
		} // private void ComputeIntersectionNodes( int argIndex )

		/// <summary>
		/// For all intersections on the edges of a Geometry,
		/// label the corresponding node IF it doesn't already have a label.
		/// This allows nodes created by either self-intersections or
		///	mutual intersections to be labelled.
		///	Endpoint nodes will already be labelled from when they were inserted.
		/// </summary>
		/// <param name="argIndex"></param>
		private void LabelIntersectionNodes( int argIndex )
		{
			foreach ( object obj in _arg[argIndex].Edges ) 
			{
				Edge e = (Edge) obj;
				int eLoc = e.Label.GetLocation( argIndex );
				foreach ( object objEdgeIntersection in e.EdgeIntersectionList  ) 
				{
					EdgeIntersection ei = (EdgeIntersection) objEdgeIntersection;
					RelateNode n = (RelateNode) _nodes.Find( ei.Coordinate );
					if ( n.Label.IsNull( argIndex ) ) 
					{
						if ( eLoc == Location.Boundary )
						{
							n.SetLabelBoundary( argIndex );
						}
						else
						{
							n.SetLabel( argIndex, Location.Interior );
						}
					}
					//Trace.WriteLine( n.ToString() );
				} // foreach ( object objEdgeIntersection in e.EdgeIntersectionList )
			} // foreach ( object obj in _arg[argIndex].Edges )
		} // private void LabelIntersectionNodes( int argIndex )

		/// <summary>
		/// If the Geometries are disjoint, we need to enter their dimension and
		/// boundary dimension in the Ext rows in the IM.
		/// </summary>
		/// <param name="im"></param>
		private void ComputeDisjointIM( IntersectionMatrix im )
		{
			Geometry ga = _arg[0].Geometry;
			if ( !ga.IsEmpty() ) 
			{
				im.Set( Location.Interior, Location.Exterior, ga.GetDimension() );
				im.Set( Location.Boundary, Location.Exterior, ga.GetBoundaryDimension() );
			}
			Geometry gb = _arg[1].Geometry;
			if ( !gb.IsEmpty() ) 
			{
				im.Set( Location.Exterior, Location.Interior, gb.GetDimension() );
				im.Set( Location.Exterior, Location.Boundary, gb.GetBoundaryDimension() );
			}
		} // private void ComputeDisjointIM( IntersectionMatrix im )

		/// <summary>
		/// Check all nodes to see if their labels are consistent. If any are not, return false.
		/// </summary>
		/// <returns></returns>
		private bool IsNodeEdgeAreaLabelsConsistent()
		{
			foreach ( DictionaryEntry entry in _nodes ) 
			{
				RelateNode node = (RelateNode) entry.Value;
				if ( !node.Edges.IsAreaLabelsConsistent() ) 
				{
					_invalidPoint = (Coordinate) node.Coordinate.Clone();
					return false;
				}
			} // foreach ( object obj in _nodes )
			return true;
		} // private bool IsNodeEdgeAreaLabelsConsistent()

		private void LabelNodeEdges()
		{
			foreach ( DictionaryEntry entry in _nodes ) 
			{
				RelateNode node = (RelateNode) entry.Value;
				node.Edges.ComputeLabelling( _arg );
				//Trace.WriteLine( node.Edges.ToString() );
				//Trace.WriteLine( node.ToString() );
			} 
		} // private void LabelNodeEdges()

		/// <summary>
		/// Update the IM with the sum of the IMs for each component.
		/// </summary>
		/// <param name="im"></param>
		private void UpdateIM( IntersectionMatrix im )
		{
			//Trace.WriteLine( im.ToString() );
			foreach ( object obj in _isolatedEdges ) 
			{
				Edge e = (Edge) obj;
				e.UpdateIM( im );
				//Trace.WriteLine( im.ToString() );
			} // foreach ( object obj in _isolatedEdges )

			foreach ( DictionaryEntry entry in _nodes ) 
			{
				RelateNode node = (RelateNode) entry.Value;
				node.UpdateIM( im );
				//Trace.WriteLine( im.ToString() );
				node.UpdateIMFromEdges( im );
				//Trace.WriteLine( im.ToString() );
				//Trace.WriteLine( node.ToString() );
			} // foreach ( object obj in _nodes )

		} // private void UpdateIM( IntersectionMatrix im )

		/// <summary>
		/// Processes isolated edges by computing their labelling and adding them
		/// to the isolated edges list.
		///	Isolated edges are guaranteed not to touch the boundary of the target (since if they
		/// did, they would have caused an intersection to be computed and hence would
		///	not be isolated)
		/// </summary>
		/// <param name="thisIndex"></param>
		/// <param name="targetIndex"></param>
		private void LabelIsolatedEdges( int thisIndex, int targetIndex )
		{
			foreach ( object obj in _arg[thisIndex].Edges ) 
			{
				Edge e = (Edge) obj;
				if ( e.IsIsolated() ) 
				{
					LabelIsolatedEdge( e, targetIndex, _arg[targetIndex].Geometry );
					_isolatedEdges.Add( e );
				}
			} // foreach ( object obj in _arg[thisIndex].Edges )
		} // private void LabelIsolatedEdges( int thisIndex, int targetIndex )

		/// <summary>
		/// Label an isolated edge of a graph with its relationship to the target geometry.
		/// If the target has dim 2 or 1, the edge can either be in the interior or the exterior.
		/// If the target has dim 0, the edge must be in the exterior.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="targetIndex"></param>
		/// <param name="target"></param>
		private void LabelIsolatedEdge(Edge e, int targetIndex, Geometry target)
		{
			// this won't work for GeometryCollections with both dim 2 and 1 geoms
			if ( target.GetDimension() > 0) 
			{
				// since edge is not in boundary, may not need the full generality of PointLocator?
				// Possibly should use ptInArea locator instead?  We probably know here
				// that the edge does not touch the bdy of the target Geometry
				int loc = _ptLocator.Locate( e.GetCoordinate(), target );
				e.Label.SetAllLocations( targetIndex, loc );
			} // if ( target.Dimension > 0)
			else 
			{
				e.Label.SetAllLocations( targetIndex, Location.Exterior );
			}
			//Trace.WriteLine( e.Label.ToString() );

		} // private void LabelIsolatedEdge(Edge e, int targetIndex, Geometry target)

		/// <summary>
		/// Isolated nodes are nodes whose labels are incomplete
		/// (e.g. the location for one Geometry is null).
		/// This is the case because nodes in one graph which don't intersect
		/// nodes in the other are not completely labelled by the initial process
		///	of adding nodes to the nodeList.
		///	To complete the labelling we need to check for nodes that lie in the
		///	interior of edges, and in the interior of areas.
		/// </summary>
		private void LabelIsolatedNodes()
		{
			foreach ( DictionaryEntry entry in _nodes ) 
			{
				Node n = (Node) entry.Value;
				Label label = n.Label;

				// isolated nodes should always have at least one geometry in their label
				if(!( label.GetGeometryCount() > 0))
				{
					throw new InvalidOperationException("Node with empty label found." );
				}
				if ( n.IsIsolated() ) 
				{
					if ( label.IsNull( 0 ) )
					{
						LabelIsolatedNode( n, 0 );
					}
					else
					{
						LabelIsolatedNode( n, 1 );
					}
				} // if ( n.IsIsolated )
			} // foreach ( object obj in _nodes )

		} // private void LabelIsolatedNodes()
		
		/// <summary>
		/// Label an isolated node with its relationship to the target geometry.
		/// </summary>
		/// <param name="n"></param>
		/// <param name="targetIndex"></param>
		private void LabelIsolatedNode( Node n, int targetIndex )
		{
			int loc = _ptLocator.Locate( n.GetCoordinate(), _arg[targetIndex].Geometry );
			n.Label.SetAllLocations( targetIndex, loc );
			//Trace.WriteLine( n.Label.ToString() );
		} // private void LabelIsolatedNode( Node n, int targetIndex )
		#endregion

	} // public class RelateComputer
}

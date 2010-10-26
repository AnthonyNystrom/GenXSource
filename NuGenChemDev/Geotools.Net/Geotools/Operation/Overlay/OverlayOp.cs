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
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for OverlayOp.
	/// </summary>
	internal class OverlayOp : GeometryGraphOperation
	{
		public const int Intersection  = 1;
		public const int Union         = 2;
		public const int Difference    = 3;
		public const int SymDifference = 4;

		private PointLocator _ptLocator = new PointLocator();

		private GeometryFactory _geomFact;
		private Geometry _resultGeom;

		private PlanarGraph _graph;
		private EdgeList _edgeList     = new EdgeList();

		private ArrayList _resultPolyList   = new ArrayList();
		private ArrayList _resultLineList   = new ArrayList();
		private ArrayList _resultPointList  = new ArrayList();

		
		#region Constructors
		public OverlayOp( Geometry g0, Geometry g1 ) : base( g0, g1 )
		{
			_graph = new PlanarGraph( new OverlayNodeFactory() );
			_geomFact = new GeometryFactory( g0.PrecisionModel, g0.GetSRID() );
		} // public OverlayOp( Geometry g0, Geometry g1 ) : base( g0, g1 )

		#endregion

		#region Static Methods
		/// <summary>
		/// Initializes a new instance of the OverlayOp class.
		/// </summary>
		public static Geometry Overlay( Geometry geom0, Geometry geom1, int opCode )
		{
			OverlayOp gov = new OverlayOp( geom0, geom1 );
			Geometry geomOv = gov.GetResultGeometry( opCode );
			return geomOv;
		} // public static Geometry Overlay( Geometry geom0, Geometry geom1, int opCode )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="label"></param>
		/// <param name="opCode"></param>
		/// <returns></returns>
		public static bool IsResultOfOp( Label label, int opCode )
		{
			int loc0 = label.GetLocation( 0 );
			int loc1 = label.GetLocation( 1 );
			return IsResultOfOp( loc0, loc1, opCode );
		} // public static bool IsResultOfOp( Label label, int opCode )		

		/// <summary>
		/// This method will handle arguments of Location.NULL correctly.
		/// </summary>
		/// <param name="loc0"></param>
		/// <param name="loc1"></param>
		/// <param name="opCode"></param>
		/// <returns>Returns true if the locations correspond to the opCode.</returns>
		public static bool IsResultOfOp( int loc0, int loc1, int opCode )
		{
			if ( loc0 == Location.Boundary ) 
			{
				loc0 = Location.Interior;
			}
			if ( loc1 == Location.Boundary )
			{
				loc1 = Location.Interior;
			}
			switch ( opCode ) 
			{
				case Intersection:
					return loc0 == Location.Interior
						&& loc1 == Location.Interior;
				case Union:
					return loc0 == Location.Interior
						|| loc1 == Location.Interior;
				case Difference:
					return loc0 == Location.Interior
						&& loc1 != Location.Interior;
				case SymDifference:
					return   ( loc0 == Location.Interior &&  loc1 != Location.Interior )
						|| ( loc0 != Location.Interior &&  loc1 == Location.Interior );
			} // switch ( opCode )
			return false;
		} // public static bool IsResultOfOp( int loc0, int loc1, int opCode )	
		#endregion

		#region Properties
		/// <summary>
		/// Gets the PlanarGraph object.
		/// </summary>
		public PlanarGraph Graph
		{
			get
			{ 
				return _graph;
			}
		}
		#endregion

		#region Method

		/// <summary>
		/// 
		/// </summary>
		/// <param name="funcCode"></param>
		/// <returns></returns>
		public Geometry GetResultGeometry( int funcCode )
		{
			ComputeOverlay( funcCode );
			return _resultGeom;
		} // public Geometry GetResultGeometry( int funcCode )

		private void ComputeOverlay( int opCode )
		{
			// copy points from input Geometries.
			// This ensures that any Point geometries
			// in the input are considered for inclusion in the result set
			CopyPoints(0);
			CopyPoints(1);

			// node the input Geometries
			_arg[0].ComputeSelfNodes( _li );
			_arg[1].ComputeSelfNodes( _li );

			// compute intersections between edges of the two input geometries
			_arg[0].ComputeEdgeIntersections( _arg[1], _li, true );

			ArrayList baseSplitEdges = new ArrayList();
			_arg[0].ComputeSplitEdges( baseSplitEdges );
			_arg[1].ComputeSplitEdges( baseSplitEdges );
			ArrayList splitEdges = new ArrayList( baseSplitEdges );


			/* NO LONGER USED
			// if we are working in fixed precision, we must renode to ensure noding is complete
			if (makePrecise) {
			  List splitEdges1 = completeEdgeNoding(baseSplitEdges);
			  splitEdges = completeEdgeNoding(splitEdges1);
			} */

			// add the noded edges to this result graph
			InsertUniqueEdges( baseSplitEdges );

			ComputeLabelsFromDepths();
			ReplaceCollapsedEdges();

			//Trace.WriteLine( edgeList.ToString());

			_graph.AddEdges( _edgeList );
			ComputeLabelling();
			LabelIncompleteNodes();

			 // The ordering of building the result Geometries is important.
			 // Areas must be built before lines, which must be built before points.
			 // This is so that lines which are covered by areas are not included
			 // explicitly, and similarly for points.

			FindResultAreaEdges( opCode );
			CancelDuplicateResultEdges();
			PolygonBuilder polyBuilder = new PolygonBuilder( _geomFact, _cga );
			polyBuilder.Add( _graph );
			_resultPolyList = polyBuilder.GetPolygons();

			LineBuilder lineBuilder = new LineBuilder( this, _geomFact, _ptLocator );
			_resultLineList = lineBuilder.Build( opCode );

			PointBuilder pointBuilder = new PointBuilder( this, _geomFact, _ptLocator );
			_resultPointList = pointBuilder.Build( opCode );

			// gather the results from all calculations into a single Geometry for the result set
			_resultGeom = ComputeGeometry( _resultPointList, _resultLineList, _resultPolyList );
		} //private void ComputeOverlay( int opCode )

		private void InsertUniqueEdges( ArrayList edges )
		{
			foreach ( object obj in edges )
			{
				Edge e = (Edge) obj;
				InsertUniqueEdge( e );
			}
		} // private void InsertUniqueEdges( ArrayList edges )

		/// <summary>
		/// Insert an edge from one of the noded input graphs.
		/// Checks edges that are inserted to see if an
		/// identical edge already exists.
		/// If so, the edge is not inserted, but its label is merged
		/// with the existing edge.
		/// </summary>
		/// <param name="e"></param>
		protected void InsertUniqueEdge( Edge e )
		{
			// Trace.WriteLine( e.ToString() );
			int foundIndex = _edgeList.FindEdgeIndex( e );
			// If an identical edge already exists, simply update its label
			if ( foundIndex >= 0 ) 
			{
				Edge existingEdge = (Edge) _edgeList[foundIndex];
				Label existingLabel = existingEdge.Label;

				Label labelToMerge = e.Label;
				// check if new edge is in reverse direction to existing edge
				// if so, must flip the label before merging it
				if ( !existingEdge.IsPointwiseEqual( e ) ) 
				{
					labelToMerge = new Label( e.Label );
					labelToMerge.Flip();
				}

				Depth depth = existingEdge.Depth;
				// if this is the first duplicate found for this edge, initialize the depths
				//
				if ( depth.IsNull() ) 
				{
					depth.Add( existingLabel );
				}
				//
				depth.Add( labelToMerge );
				existingLabel.Merge( labelToMerge );
				//Trace.WriteLine("inserted edge: ");
				//Trace.WriteLine( e.ToString() );
				//Trace.WriteLine("existing edge: ");
				//Trace.WriteLine( existingEdge.ToString() );

			} // if ( foundIndex >= 0 )
			else 
			{  
				// no matching existing edge was found
				// add this new edge to the list of edges in this graph
				//e.setName(name + edges.size());
				//e.getDepth().add(e.getLabel());
				_edgeList.Add( e );
			} // else
		} // protected void InsertUniqueEdge( Edge e )

		/**
		 * If either of the GeometryLocations for the existing label is
		 * exactly opposite to the one in the labelToMerge,
		 * this indicates a dimensional collapse has happened.
		 * In this case, convert the label for that Geometry to a Line label
		 */
		/* NOT NEEDED?
	   private void CheckDimensionalCollapse(Label labelToMerge, Label existingLabel)
	   {
		 if (existingLabel.isArea() && labelToMerge.isArea()) {
		   for (int i = 0; i < 2; i++) {
			 if (! labelToMerge.isNull(i)
				 &&  labelToMerge.getLocation(i, Position.LEFT)  == existingLabel.getLocation(i, Position.RIGHT)
				 &&  labelToMerge.getLocation(i, Position.RIGHT) == existingLabel.getLocation(i, Position.LEFT) )
			 {
			   existingLabel.toLine(i);
			 }
		   }
		 }
	   }
	   */

		/// <summary>
		/// Update the labels for edges according to their depths.
		/// For each edge, the depths are first normalized.
		/// Then, if the depths for the edge are equal,
		/// this edge must have collapsed into a line edge.
		/// If the depths are not equal, update the label
		/// with the locations corresponding to the depths
		/// (i.e. a depth of 0 corresponds to a Location of EXTERIOR,
		/// a depth of 1 corresponds to INTERIOR)
		/// </summary>
		private void ComputeLabelsFromDepths()
		{
			foreach ( object obj in _edgeList ) 
			{
				Edge e = (Edge) obj;
				Label lbl = e.Label;
				Depth depth = e.Depth;

				 // Only check edges for which there were duplicates,
				 // since these are the only ones which might
				 // be the result of dimensional collapses.

				if ( !depth.IsNull() ) 
				{
					depth.Normalize();
					for ( int i = 0; i < 2; i++ ) 
					{
						if ( !lbl.IsNull( i ) && lbl.IsArea() && !depth.IsNull( i ) ) 
						{

							 // if the depths are equal, this edge is the result of
							 // the dimensional collapse of two or more edges.
							 // It has the same location on both sides of the edge,
							 // so it has collapsed to a line.

							if ( depth.GetDelta( i ) == 0 ) 
							{
								lbl.ToLine( i );
							}
							else 
							{

								 // This edge may be the result of a dimensional collapse,
								 // but it still has different locations on both sides.  The
								 // label of the edge must be updated to reflect the resultant
								 // side locations indicated by the depth values.

								if (depth.IsNull( i, Position.Left ))
								{
									throw new InvalidOperationException( "Depth of LEFT side has not been initialized" );
								}
								lbl.SetLocation( i, Position.Left,   depth.GetLocation( i, Position.Left ) );
								if (depth.IsNull( i, Position.Right ))
								{
									throw new InvalidCastException("Depth of RIGHT side has not been initialized" );
								}
								lbl.SetLocation( i, Position.Right,  depth.GetLocation( i, Position.Right ) );
							} // else
						} // if ( !lbl.IsNull( i ) && lbl.IsArea() && !depth.IsNull( i ) )
					}  // for ( int i = 0; i < 2; i++ ) 
				} // if ( !depth.IsNull() )
			} // foreach ( object obj in _edgeList )
		} // private void ComputeLabelsFromDepths()

		/// <summary>
		/// If edges which have undergone dimensional collapse are found,
		/// replace them with a new edge which is a L edge
		/// </summary>
		private void ReplaceCollapsedEdges()
		{
			ArrayList newEdges = new ArrayList( _edgeList );
			foreach ( object obj in _edgeList )
			{
				Edge e = (Edge) obj;
				if ( e.IsCollapsed() ) 
				{
					newEdges.Remove( obj );
					newEdges.Add( e.GetCollapsedEdge() );
				}
			}
			_edgeList.Clear();		// remove all elements and then add back in.
			_edgeList.AddRange( newEdges );
		} // private void ReplaceCollapsedEdges()

		/// <summary>
		/// Copy all nodes from an arg geometry into this graph.
		/// The node label in the arg geometry overrides any previously computed
		/// label for that argIndex.
		/// (E.g. a node may be an intersection node with
		/// a previously computed label of BOUNDARY,
		/// but in the original arg Geometry it is actually
		/// in the interior due to the Boundary Determination Rule)
		/// </summary>
		/// <param name="argIndex"></param>
		private void CopyPoints( int argIndex )
		{
			foreach ( DictionaryEntry obj in _arg[argIndex].Nodes ) 
			{
				Node graphNode = (Node) obj.Value;
				Node newNode = _graph.AddNode( graphNode.GetCoordinate() );
				newNode.SetLabel( argIndex, graphNode.Label.GetLocation( argIndex ) );
			}
		} // private void CopyPoints( int argIndex )

		/// <summary>
		/// Compute initial labelling for all DirectedEdges at each node.
		/// In this step, DirectedEdges will acquire a complete labelling
		/// (i.e. one with labels for both Geometries)
		/// only if they
		/// are incident on a node which has edges for both Geometries
		/// </summary>
		private void ComputeLabelling()
		{
			foreach ( DictionaryEntry obj in _graph.Nodes )
			{
				Node node = (Node) obj.Value;
				//if (node.getCoordinate().equals(new Coordinate(222, 100)) ) Debug.addWatch(node.getEdges());
				node.Edges.ComputeLabelling( _arg );
			}
			MergeSymLabels();
			UpdateNodeLabelling();
		} // private void ComputeLabelling()

		/// <summary>
		/// For nodes which have edges from only one Geometry incident on them,
		/// the previous step will have left their dirEdges with no labelling for the other
		/// Geometry.  However, the sym dirEdge may have a labelling for the other
		/// Geometry, so merge the two labels.
		/// </summary>
		private void MergeSymLabels()
		{
			foreach ( DictionaryEntry obj in _graph.Nodes ) 
			{
				Node node = (Node) obj.Value;
				((DirectedEdgeStar) node.Edges).MergeSymLabels();
				//node.print(System.out);
			} // foreach ( object obj in _graph.Nodes )
		} // private void MergeSymLabels()

		private void UpdateNodeLabelling()
		{
			// update the labels for nodes
			// The label for a node is updated from the edges incident on it
			// (Note that a node may have already been labelled
			// because it is a point in one of the input geometries)
			foreach ( DictionaryEntry obj in _graph.Nodes ) 
			{
				Node node = (Node) obj.Value;
				Label lbl = ( (DirectedEdgeStar)node.Edges ).Label;
				node.Label.Merge( lbl );
			} // foreach ( object obj in _graph.Nodes )
		} // private void UpdateNodeLabelling()

		/// <summary>
		/// Incomplete nodes are nodes whose labels are incomplete.
		/// (e.g. the location for one Geometry is null).
		/// These are either isolated nodes,
		/// or nodes which have edges from only a single Geometry incident on them.
		/// </summary>
		/// <remarks>Isolated nodes are found because nodes in one graph which don't intersect
		/// nodes in the other are not completely labelled by the initial process
		/// of adding nodes to the nodeList.
		/// To complete the labelling we need to check for nodes that lie in the
		/// interior of edges, and in the interior of areas.
		/// 
		/// When each node labelling is completed, the labelling of the incident
		/// edges is updated, to complete their labelling as well.
		/// </remarks>
		private void LabelIncompleteNodes()
		{
			foreach ( DictionaryEntry obj in _graph.Nodes )
			{
				Node n = (Node) obj.Value;
				Label label = n.Label;
				if ( n.IsIsolated() ) 
				{
					if ( label.IsNull( 0 ) )
					{
						LabelIncompleteNode( n, 0 );
					}
					else
					{
						LabelIncompleteNode( n, 1 );
					}
				} // if ( n.IsIsolated )

				// now update the labelling for the DirectedEdges incident on this node
				( (DirectedEdgeStar)n.Edges ).UpdateLabelling( label );
			} // foreach ( object obj in _graph.Nodes )
		} // private void LabelIncompleteNodes()

		/// <summary>
		/// Label an isolated node with its relationship to the target geometry.
		/// </summary>
		/// <param name="n"></param>
		/// <param name="targetIndex"></param>
		private void LabelIncompleteNode( Node n, int targetIndex )
		{
			int loc = _ptLocator.Locate( n.GetCoordinate(), _arg[targetIndex].Geometry );
			n.Label.SetLocation( targetIndex, loc );
		} // private void LabelIncompleteNode( Node n, int targetIndex )

		/// <summary>
		/// Find all edges whose label indicates that they are in the result area(s),
		/// according to the operation being performed.  Since we want polygon shells to be
		/// oriented CW, choose dirEdges with the interior of the result on the RHS.
		/// Mark them as being in the result.
		/// Interior Area edges are the result of dimensional collapses.
		/// They do not form part of the result area boundary.
		/// </summary>
		/// <param name="opCode"></param>
		private void FindResultAreaEdges( int opCode )
		{
			foreach ( object obj in _graph.EdgeEnds ) 
			{
				DirectedEdge de = (DirectedEdge) obj;
				// mark all dirEdges with the appropriate label
				Label label = de.Label;
				if ( label.IsArea()
				  && !de.IsInteriorAreaEdge
				  && IsResultOfOp(	label.GetLocation( 0, Position.Right ),	label.GetLocation( 1, Position.Right ),	opCode )  ) 
				{
					de.InResult = true;
				}
			} // foreach ( object obj in _graph.EdgeEnds )
		} // private void FindResultAreaEdges( int opCode )

		/// <summary>
		/// If both a dirEdge and its sym are marked as being in the result, cancel
		/// them out.
		/// </summary>
		private void CancelDuplicateResultEdges()
		{
			// remove any dirEdges whose sym is also included
			// (they "cancel each other out")
			foreach ( object obj in _graph.EdgeEnds ) 
			{
				DirectedEdge de = (DirectedEdge) obj;
				DirectedEdge sym = de.Sym;
				if ( de.InResult && sym.InResult ) 
				{
					de.InResult = false;
					sym.InResult = false;
					//Trace.WriteLine("cancelled ");
					//Trace.WriteLine( de.ToString() );
					//Trace.WriteLine( sym.ToString() );
				}
			} // foreach ( object obj in _graph.EdgeEnds )
		} // private void CancelDuplicateResultEdges()

		/// <summary>
		/// This method is used to decide if a point node should be included in the result or not.
		/// </summary>
		/// <param name="coord"></param>
		/// <returns>true if the coord point is covered by a result Line or Area geometry</returns>
		public bool IsCoveredByLA(Coordinate coord)
		{
			if ( IsCovered( coord, _resultLineList) )
			{
				return true;
			}
			if ( IsCovered( coord, _resultPolyList ) )
			{
				return true;
			}
			return false;
		} // public bool IsCoveredByLA(Coordinate coord)
		
		/// <summary>
		/// This method is used to decide if an L edge should be included in the result or not.
		/// </summary>
		/// <param name="coord"></param>
		/// <returns>True if the coord point is covered by a result Area geometry</returns>
		public bool IsCoveredByA( Coordinate coord )
		{
			if ( IsCovered( coord, _resultPolyList ) )
			{
				return true;
			}
			return false;
		} // public bool IsCoveredByA( Coordinate coord )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coord"></param>
		/// <param name="geomList"></param>
		/// <returns>
		/// True if the coord is located in the interior or boundary of a geometry in the list.
		/// </returns>
		private bool IsCovered( Coordinate coord, ArrayList geomList )
		{
			foreach ( object obj in geomList )
			{
				Geometry geom = (Geometry) obj;
				int loc = _ptLocator.Locate( coord, geom );
				if ( loc != Location.Exterior )
				{
					return true;
				}
			} // foreach ( object obj in geomList )
			return false;
		} // private bool IsCovered( Coordinate coord, ArrayList geomList )

		private Geometry ComputeGeometry( ArrayList _resultPointList, ArrayList _resultLineList, ArrayList _resultPolyList )
		{
			ArrayList geomList = new ArrayList();
			// element geometries of the result are always in the order P,L,A
			geomList.AddRange( _resultPointList );
			geomList.AddRange( _resultLineList );
			geomList.AddRange( _resultPolyList );
			// build the most specific geometry possible
			return _geomFact.BuildGeometry( geomList );
		} // private Geometry ComputeGeometry( ArrayList _resultPointList, ArrayList _resultLineList, ArrayList _resultPolyList )
		#endregion

	} // public class OverlayOp : Operation
}

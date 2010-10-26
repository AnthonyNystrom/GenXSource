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
using Geotools.Geometries;
using Geotools.Graph;
using Geotools.Algorithms;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for LineBuilder.
	/// </summary>
	internal class LineBuilder
	{
		private OverlayOp _op;
		private GeometryFactory _geometryFactory;
		private PointLocator _ptLocator;

		private ArrayList _lineEdgesList    = new ArrayList();
		private ArrayList _resultLineList   = new ArrayList();

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		/// <param name="geometryFactory"></param>
		/// <param name="ptLocator"></param>
		public LineBuilder( OverlayOp op, GeometryFactory geometryFactory, PointLocator ptLocator ) 
		{
			_op = op;
			_geometryFactory = geometryFactory;
			_ptLocator = ptLocator;
		} // public LineBuilder( OverlayOp op, GeometryFactory geometryFactory, PointLocator ptLocator )
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Returns a list of the LineStrings in the result of the specified overlay operation.
		/// </summary>
		/// <param name="opCode"></param>
		/// <returns></returns>
		public ArrayList Build( int opCode )
		{
			FindCoveredLineEdges();
			CollectLines( opCode );
			//LabelIsolatedLines(lineEdgesList);
			BuildLines(opCode);
			return _resultLineList;
		} // public ArrayList Build( int opCode )

		/// <summary>
		/// Find and mark L edges which are "covered" by the result area (if any).
		/// L edges at nodes which also have A edges can be checked by checking their
		/// depth at that node.  L edges at nodes which do not have A edges can be
		/// checked by doing a point-in-polygon test with the previously computed result areas.
		/// </summary>
		private void FindCoveredLineEdges()
		{
			// first set covered for all L edges at nodes which have A edges too
			foreach( DictionaryEntry obj in _op.Graph.Nodes )
			{
				Node node = (Node) obj.Value;
				//Trace.WriteLine( node.ToString() );
				( (DirectedEdgeStar) node.Edges ).FindCoveredLineEdges();
			}

			 // For all L edges which weren't handled by the above,
			 // use a point-in-poly test to determine whether they are covered
			foreach ( object obj in _op.Graph.EdgeEnds )  
			{
				DirectedEdge de = (DirectedEdge) obj;
				Edge e = de.Edge;
				if ( de.IsLineEdge && !e.IsCoveredSet ) 
				{
					bool isCovered = _op.IsCoveredByA( de.Coordinate );
					e.SetCovered = isCovered;
				}
			}
		} // private void FindCoveredLineEdges()

		private void CollectLines( int opCode )
		{
			foreach ( object obj in _op.Graph.EdgeEnds ) 
			{
				DirectedEdge de = (DirectedEdge) obj;
				CollectLineEdge( de, opCode, _lineEdgesList );
				CollectBoundaryTouchEdge( de, opCode, _lineEdgesList );
			}
		} // private void CollectLines( int opCode )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="de"></param>
		/// <param name="opCode"></param>
		/// <param name="edges"></param>
		public void CollectLineEdge( DirectedEdge de, int opCode, ArrayList edges )
		{
			Label label = de.Label;
			Edge e = de.Edge;
			// include L edges which are in the result
			if ( de.IsLineEdge ) 
			{
				if ( !de.Visited && OverlayOp.IsResultOfOp( label, opCode ) && !e.IsCovered ) 
				{
					//Trace.WriteLine("de: " + de.Label.ToString() );
					//Trace.WriteLine("edge: " + e.Label.ToString() );

					edges.Add( e );
					de.SetVisitedEdge( true );
				} // if ( !de.Visited && OverlayOp.IsResultOfOp( label, opCode ) && !e.IsCovered )
			} // if ( de.IsLineEdge )
		} // public void CollectLineEdge( DirectedEdge de, int opCode, ArrayList edges )

		/// <summary>
		/// Collect edges from Area input which should be in the result by which have not been included
		/// in a result area.  This happens ONLY:
		/// &lt;ul&gt;
		///		&lt;li&gt;during an intersection when the boundaries of two areas touch in a line segment.
		///		&lt;li&gt;OR as a result of a dimensional collapse.
		///	&lt;/ul&gt;
		/// </summary>
		/// <param name="de"></param>
		/// <param name="opCode"></param>
		/// <param name="edges"></param>
		public void CollectBoundaryTouchEdge( DirectedEdge de, int opCode, ArrayList edges )
		{
			Label label = de.Label;
			// this smells like a bit of a hack, but it seems to work...
			if (   !de.IsLineEdge
				&& !de.IsInteriorAreaEdge  // added to handle dimensional collapses
				&& !de.Edge.IsInResult
				&& !de.Visited
				&& OverlayOp.IsResultOfOp( label, opCode )
				&& opCode == OverlayOp.Intersection )
			{
				edges.Add( de.Edge );
				de.SetVisitedEdge( true );
			}
		} // public void CollectBoundaryTouchEdge( DirectedEdge de, int opCode, ArrayList edges )

		private void BuildLines(int opCode)
		{
			// need to simplify lines?
			foreach ( object obj in _lineEdgesList ) 
			{
				Edge e = (Edge) obj;
				Label label = e.Label;
				//Trace.WriteLine( e.ToString() );
				//Trace.WriteLine( label.ToString() );
				//if ( OverlayGraph.IsResultOfOp(label, opCode) ) {
				LineString line = _geometryFactory.CreateLineString( e.Coordinates );
				_resultLineList.Add( line );
				e.IsInResult = true;
				//}
			} // foreach ( object obj in _lineEdgesList ) 
		} // private void BuildLines(int opCode)

		private void LabelIsolatedLines( ArrayList edgesList )
		{
			foreach (object obj in edgesList )
			{
				Edge e = (Edge) obj;
				Label label = e.Label;
				if ( e.IsIsolated() ) 
				{
					if ( label.IsNull( 0 ) )
					{
						LabelIsolatedLine( e, 0 );
					}
					else
					{
						LabelIsolatedLine( e, 1 );
					}
				} // if ( e.IsIsolated )
			} // foreach (object obj in edgesList )
		} // private void LabelIsolatedLines( ArrayList edgesList )

		/// <summary>
		/// Label an isolated node with its relationship to the target geometry.
		/// </summary>
		/// <param name="e"></param>
		/// <param name="targetIndex"></param>
		private void LabelIsolatedLine( Edge e, int targetIndex )
		{
			int loc = _ptLocator.Locate( e.GetCoordinate(), _op.GetArgGeometry( targetIndex) );
			e.Label.SetLocation( targetIndex, loc );
		} // private void LabelIsolatedLine( Edge e, int targetIndex )

		#endregion

	} // public class LineBuilder
}

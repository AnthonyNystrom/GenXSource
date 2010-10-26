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
using System.Collections.Specialized;
using System.Diagnostics;
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph.Index;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// This method implements the Boundary Determination Rule for determining whether a component 
	/// (node or edge) that appears multiple times in elements of a MultiGeometry is in the boundary 
	/// or the interior of the Geometry.
	/// </summary>
	/// <remarks>
	/// The SFS uses the "Mod-2 Rule", which this function implements. An alternative (and possibly more 
	/// intuitive) rule would be * the "At Most One Rule": 
	/// 
	/// isInBoundary = (componentCount == 1)
	/// 
	/// </remarks>
	internal class GeometryGraph : PlanarGraph
	{
		
		/// <summary>
		/// The Geometry represented by this graph.
		/// </summary>
		private Geometry _parentGeometry;

		/// <summary>
		/// The precision model of the Edges added to this graph. Used when Edges are added instead of
		/// constructed with a Geometry. 
		/// </summary>
		private PrecisionModel _precisionModel = null;

		/// <summary>
		/// The ID of the Spatial Reference System of the Edges added to this graph. Used when Edges are 
		/// added instead of constructed with a Geometry. 
		/// </summary>
		private int _SRID;

		/// <summary>
		/// The lineEdgeMap is a map of the linestring components of the _parentGeometry to the
		/// edges which are derived from them.  This is used to efficiently perform findEdge queries.
		/// </summary>
		private HybridDictionary _lineEdgeMap = new HybridDictionary();

		
		/// <summary>
		/// If this flag is true, the Boundary Determination Rule will be used when deciding
		/// whether nodes are in the boundary or not.
		/// </summary>
		private bool _useBoundaryDeterminationRule = false;

		/// <summary>
		/// The index of this geometry as an argument to a spatial function (used for labelling ).
		/// </summary>
		private int _argIndex;

		/// <summary>
		/// List of nodes.
		/// </summary>
		private ArrayList _boundaryNodes;

		private EdgeSetIntersector _edgeSetIntersector = new SimpleMCSweepLineIntersector();
		private bool _hasTooFewPoints = false;
		private Coordinate _invalidPoint = null;

		#region Properties
		public PrecisionModel PrecisionModel
		{
			get
			{
				return _precisionModel;
			}
		}
		public int SRID
		{
			get
			{
				return _SRID;
			}
		}


		public Geometry Geometry
		{
			get
			{
				return _parentGeometry;
			}
		}	
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the GeometryGraph class.
		/// </summary>
		public GeometryGraph( int argIndex, Geometry parentGeometry ) 
		{
			_argIndex = argIndex;
			_parentGeometry = parentGeometry;
			if ( parentGeometry != null ) 
			{
				_precisionModel = parentGeometry.PrecisionModel;
				_SRID = parentGeometry.GetSRID();
				Add( parentGeometry );
			}
		} // public GeometryGraph( int argIndex, Geometry parentGeometry )
	
		/// <summary>
		/// This constructor is used by clients that wish to add Edges explicitly, rather than adding a Geometry.  (An example is BufferOp).
		/// </summary>
		/// <param name="argIndex"></param>
		/// <param name="precisionModel"></param>
		/// <param name="SRID"></param>
		public GeometryGraph(int argIndex, PrecisionModel precisionModel, int SRID) : this( argIndex, null )
		{
			_precisionModel = precisionModel;
			_SRID = SRID;
		} // public GeometryGraph(int argIndex, PrecisionModel precisionModel, int SRID) : this( argIndex, null )

		#endregion

		#region Static methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="boundaryCount"></param>
		/// <returns></returns>
		public static bool IsInBoundary( int boundaryCount )
		{
			// the "Mod-2 Rule"
			return boundaryCount % 2 == 1;
		} // public static bool IsInBoundary( int boundaryCount )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="boundaryCount"></param>
		/// <returns></returns>
		public static int DetermineBoundary( int boundaryCount )
		{
			return IsInBoundary( boundaryCount ) ? Location.Boundary : Location.Interior;
		} // public static int DetermineBoundary( int boundaryCount )

		#endregion

		#region Methods
		private EdgeSetIntersector CreateEdgeSetIntersector()
		{
			// various options for computing intersections, from slowest to fastest

			//private EdgeSetIntersector esi = new SimpleEdgeSetIntersector();
			//private EdgeSetIntersector esi = new MonotoneChainIntersector();
			//private EdgeSetIntersector esi = new NonReversingChainIntersector();
			//private EdgeSetIntersector esi = new SimpleSweepLineIntersector();
			//private EdgeSetIntersector esi = new MCSweepLineIntersector();

			//return new SimpleEdgeSetIntersector();
			return new SimpleMCSweepLineIntersector();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ArrayList GetBoundaryNodes()
		{
			if (_boundaryNodes == null)
			{
				_boundaryNodes = Nodes.GetBoundaryNodes( _argIndex );
			}
			return _boundaryNodes;
		} // public ArrayList GetBoundaryNodes()

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Coordinates GetBoundaryPoints()
		{
			ArrayList coll = GetBoundaryNodes();
			Coordinates pts = new Coordinates();
			foreach( object objNode in coll )
			{
				Node node = (Node) objNode;
				pts.Add( (Coordinate) node.Coordinate.Clone() );
			}
			return pts;
		} // public Coordinates GetBoundaryPoints()

		/// <summary>
		/// 
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public Edge FindEdge( LineString line )
		{
			return (Edge) _lineEdgeMap[line];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="edgelist"></param>
		public void ComputeSplitEdges( ArrayList edgelist )
		{ 
			foreach(object objEdge in _edges)
			{
				Edge e = (Edge) objEdge;
				e.EdgeIntersectionList.AddSplitEdges( edgelist );
			}

		} // public void ComputeSplitEdges( ArrayList edgelist )


		private void Add(Geometry g)
		{
			if ( g.IsEmpty() ) return;

			// check if this Geometry should obey the Boundary Determination Rule
			// all collections except MultiPolygons obey the rule
			if ( g is GeometryCollection  && !(g is MultiPolygon) )
			{
				_useBoundaryDeterminationRule = true;
			}
			if ( g is Polygon )
			{
				AddPolygon( (Polygon) g );
			}
				// LineString also handles LinearRings
			else if ( g is LineString )
			{
				AddLineString( (LineString) g );
			}
			else if ( g is Point )
			{
				AddPoint( (Point) g );
			}
			else if ( g is MultiPoint )
			{
				AddCollection( (MultiPoint) g );
			}
			else if ( g is MultiLineString )
			{
				AddCollection( (MultiLineString) g );
			}
			else if ( g is MultiPolygon )
			{
				AddCollection( (MultiPolygon) g );
			}
			else if ( g is GeometryCollection )
			{
				AddCollection( (GeometryCollection) g );
			}
			else
			{
				throw new NotSupportedException(g.GetType().Name);
			}
		}


		private void AddCollection(IGeometryCollection gc)
		{
			for( int i = 0; i < gc.GetNumGeometries(); i++ )
			{
				Geometry g = (Geometry)gc.GetGeometryN( i );
				Add( g );
			}
		} // private void AddCollection(OGC.Geometries.IGeometryCollection gc)

		/// <summary>
		/// Add a Point to the graph.
		/// </summary>
		/// <param name="p"></param>
		private void AddPoint( Point p )
		{
			Coordinate coord = p.GetCoordinate();
			InsertPoint( _argIndex, coord, Location.Interior );
		} // private void AddPoint( Point p )
		
		/// <summary>
		///  The left and right topological location arguments assume that the ring is oriented CW.
		///  If the ring is in the opposite orientation, the left and right locations must be interchanged.
		/// </summary>
		/// <param name="lr"></param>
		/// <param name="cwLeft"></param>
		/// <param name="cwRight"></param>
		private void AddPolygonRing( LinearRing lr, int cwLeft, int cwRight )
		{
			Coordinates coord = Coordinates.RemoveRepeatedPoints(lr.GetCoordinates());
			if (coord.Count<4)
			{
				_hasTooFewPoints=true;
				_invalidPoint=coord[0];
			}
			int left  = cwLeft;
			int right = cwRight;
			if ( _cga.IsCCW( coord ) ) 
			{
				left = cwRight;
				right = cwLeft;
			}
			Edge e = new Edge(coord,
				new Label(_argIndex, Location.Boundary, left, right));
			_lineEdgeMap[ lr]= e;

			InsertEdge( e );
			// insert the endpoint as a node, to mark that it is on the boundary
			InsertPoint( _argIndex, coord[0], Location.Boundary );
		} // private void AddPolygonRing( LinearRing lr, int cwLeft, int cwRight )

		private void AddPolygon( Polygon p )
		{
			AddPolygonRing(
				(LinearRing) p.GetExteriorRing(),
				Location.Exterior,
				Location.Interior );

			for ( int i = 0; i < p.GetNumInteriorRing(); i++ ) 
			{
				// Holes are topologically labelled opposite to the shell, since
				// the interior of the polygon lies on their opposite side
				// (on the left, if the hole is oriented CW)
				LinearRing interiorRing = p.GetInteriorRingN( i );
				AddPolygonRing(
					interiorRing,
					Location.Interior,
					Location.Exterior);
			}
		}

		private void AddLineString(LineString line)
		{
			Coordinates coord = line.GetCoordinates();

			// add the edge for the LineString
			// line edges do not have locations for their left and right sides
			Edge e = new Edge( coord, new Label( _argIndex, Location.Interior ) );
			_lineEdgeMap[ line]=e ;
			InsertEdge( e );
			/**
			 * Add the boundary points of the LineString, if any.
			 * Even if the LineString is closed, add both points as if they were endpoints.
			 * This allows for the case that the node already exists and is a boundary point.
			 */
			if( coord.Count < 2 )
			{
				throw new InvalidOperationException("Found LineString with single point");
			}
			InsertBoundaryPoint( _argIndex, coord[0] );
			InsertBoundaryPoint( _argIndex, coord[coord.Count- 1] );
		} // private void AddLineString(LineString line)

		/// <summary>
		/// Add an Edge computed externally.  The label on the Edge is assumed to do be correct.
		/// </summary>
		/// <param name="e"></param>
		public void AddEdge(Edge e)
		{
			InsertEdge( e );
			Coordinates coord = e.Coordinates;
			// insert the endpoint as a node, to mark that it is on the boundary
			InsertPoint( _argIndex, coord[0], Location.Boundary );
			InsertPoint( _argIndex, coord[coord.Count - 1], Location.Boundary );
		} // public void AddEdge(Edge e)

		/// <summary>
		/// Add a point computed externally.  The point is assumed to be a
		/// Point Geometry part, which has a location of INTERIOR.
		/// </summary>
		/// <param name="pt"></param>
		public void AddPoint(Coordinate pt)
		{
			InsertPoint( _argIndex, pt, Location.Interior );
		} // public void AddPoint(Coordinate pt)


		/// <summary>
		/// 
		/// </summary>
		/// <param name="li"></param>
		/// <returns></returns>
		public SegmentIntersector ComputeSelfNodes(LineIntersector li)
		{
			SegmentIntersector si = new SegmentIntersector( li, true, false);

			//EdgeSetIntersector esi = new MCQuadIntersector();
			_edgeSetIntersector.ComputeIntersections( _edges, si );
			//Trace.WriteLine( "SegmentIntersector # tests = " + si._numTests );
			AddSelfIntersectionNodes( _argIndex );
			return si;
		} // public SegmentIntersector computeSelfNodes(LineIntersector li)


		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="li"></param>
		/// <param name="includeProper"></param>
		/// <returns></returns>
		public SegmentIntersector ComputeEdgeIntersections(
			GeometryGraph g,
			LineIntersector li,
			bool includeProper)
		{
			SegmentIntersector si = new SegmentIntersector( li, includeProper, true);
			si.SetBoundaryNodes( GetBoundaryNodes(), g.GetBoundaryNodes() );

			EdgeSetIntersector esi = new SimpleMCSweepLineIntersector();
			esi.ComputeIntersections( _edges, g.Edges, si );

			/*
			foreach ( object obj in g )
			{
				Edge e = (Edge) obj;
				Trace.WriteLine( e.EdgeIntersectionList.ToString() );
			}
			*/			
			return si;
		} // public SegmentIntersector ComputeEdgeIntersections(...

		private void InsertPoint( int argIndex, Coordinate coord, int onLocation )
		{
			Node n = _nodes.AddNode( coord );
			Label lbl = n.Label;
			if (lbl == null) 
			{
				n.Label = new Label( argIndex, onLocation );
			}
			else
			{
				lbl.SetLocation( argIndex, onLocation );
			}
		} // private void InsertPoint( int argIndex, Coordinate coord, int onLocation )

		/// <summary>
		///  Adds points using the mod-2 rule of SFS.  This is used to add the boundary
		///  points of dim-1 geometries (Curves/MultiCurves).  According to the SFS,
		///  an endpoint of a Curve is on the boundary
		///  iff if it is in the boundaries of an odd number of Geometries
		/// </summary>
		/// <param name="argIndex"></param>
		/// <param name="coord"></param>
		private void InsertBoundaryPoint( int argIndex, Coordinate coord )
		{
			Node n = _nodes.AddNode( coord );
			Label lbl = n.Label;
			// the new point to insert is on a boundary
			int boundaryCount = 1;
			// determine the current location for the point (if any)
			int loc = Location.Null;
			if ( lbl != null )
			{
				loc = lbl.GetLocation( argIndex, Position.On);
			}
			if ( loc == Location.Boundary )
			{
				boundaryCount++;
			}

			// determine the boundary status of the point according to the Boundary Determination Rule
			int newLoc = DetermineBoundary( boundaryCount );
			lbl.SetLocation( argIndex, newLoc );
		} // private void InsertBoundaryPoint( int argIndex, Coordinate coord )

		private void AddSelfIntersectionNodes(int argIndex)
		{
			foreach( object objEdge in _edges )
			{
				Edge e = (Edge) objEdge;
				int eLoc = e.Label.GetLocation( argIndex );
				//for (Iterator eiIt = e.eiList.iterator(); eiIt.hasNext(); ) 
				foreach( object objEdgeIntersection in e.EdgeIntersectionList )
				{
					EdgeIntersection ei = (EdgeIntersection) objEdgeIntersection;
					AddSelfIntersectionNode( argIndex, ei.Coordinate, eLoc );
				}
			}
		} // private void AddSelfIntersectionNodes(int argIndex)

		/// <summary>
		/// Add a node for a self-intersection.
		/// </summary>
		/// <remarks>
		/// If the node is a potential boundary node (e.g. came from an edge which
		/// is a boundary) then insert it as a potential boundary node.
		/// Otherwise, just add it as a regular node.
		/// </remarks>
		/// <param name="argIndex"></param>
		/// <param name="coord"></param>
		/// <param name="loc"></param>
		private void AddSelfIntersectionNode(int argIndex, Coordinate coord, int loc)
		{
			// if this node is already a boundary node, don't change it
			if ( IsBoundaryNode( argIndex, coord ) ) return;

			if ( loc == Location.Boundary && _useBoundaryDeterminationRule )
			{
				InsertBoundaryPoint( argIndex, coord );
			}
			else
			{
				InsertPoint( argIndex, coord, loc );
			}
		} // private void AddSelfIntersectionNode(int argIndex, Coordinate coord, int loc)

		public bool HasTooFewPoints() 
		{
			return _hasTooFewPoints;
		}
		public Coordinate GetInvalidPoint() 
		{ 
			return _invalidPoint;
		}
		#endregion

	}
}

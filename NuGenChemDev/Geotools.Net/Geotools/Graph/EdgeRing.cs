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
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// Summary description for EdgeRing.
	/// </summary>
	internal abstract class EdgeRing
	{
		protected DirectedEdge _startDe; // the directed edge which starts the list of edges for this EdgeRing
		private int _maxNodeDegree = -1;
		private ArrayList _edges = new ArrayList(); // the DirectedEdges making up this EdgeRing
		private Coordinates _pts = new Coordinates();
		private Label _label = new Label( Location.Null ); // label stores the locations of each geometry on the face surrounded by this ring
		private LinearRing _ring;  // the ring created for this EdgeRing
		private bool _isHole;
		private EdgeRing _shell;   // if non-null, the ring is a hole and this EdgeRing is its containing shell
		private ArrayList _holes = new ArrayList(); // a list of EdgeRings which are holes in this EdgeRing

		protected GeometryFactory _geometryFactory;
		protected CGAlgorithms _cga;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeRing class.
		/// </summary>
		public EdgeRing( DirectedEdge start, GeometryFactory geometryFactory, CGAlgorithms cga ) 
		{
			_geometryFactory = geometryFactory;
			_cga = cga;
			ComputePoints(start);
			ComputeRing();
		}
		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public bool IsIsolated
		{
			get
			{
				return ( _label.GetGeometryCount() == 1 );
			}
		} // public bool IsIsolated

		/// <summary>
		/// 
		/// </summary>
		public bool IsHole
		{
			get
			{
				return _isHole;
			}
		} // public bool IsHole

		/// <summary>
		/// 
		/// </summary>
		public Label Label
		{
			get
			{
				return _label;
			}
		} // public Label Label

		/// <summary>
		/// 
		/// </summary>
		public bool IsShell 
		{
			get
			{
				return _shell == null;
			}
		} // public bool IsShell

		/// <summary>
		/// 
		/// </summary>
		public EdgeRing Shell
		{
			get
			{
				return _shell; 
			}
			set
			{
				_shell = value;
				if ( _shell != null ) 
				{
					_shell.AddHole( this );
				}
			}
		} // public EdgeRing Shell
		
		/// <summary>
		/// Returns the list of DirectedEdges that make up this EdgeRing.
		/// </summary>
		/// <returns></returns>
		public ArrayList Edges
		{
			get
			{
				return _edges; 
			}
		} // public ArrayList Edges
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public Coordinate GetCoordinate(int i)
		{ 
			return _pts[i];
		} // public Coordinate GetCoordinate(int i)

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public LinearRing GetLinearRing()
		{
			return _ring;
		} // public LinearRing GetLinearRing()

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ring"></param>
		public void AddHole(EdgeRing ring)
		{
			_holes.Add(ring);
		} // public void AddHole(EdgeRing ring)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="geometryFactory"></param>
		/// <returns></returns>
		public Polygon ToPolygon(GeometryFactory geometryFactory)
		{
			LinearRing[] holeLR = new LinearRing[ _holes.Count ];
			for (int i = 0; i < _holes.Count; i++) 
			{
				holeLR[i] = ( (EdgeRing)_holes[i] ).GetLinearRing();
			}
			Polygon poly = geometryFactory.CreatePolygon( GetLinearRing(), holeLR );
			return poly;
		}
		
		/// <summary>
		/// Compute a LinearRing from the point list previously collected.
		/// Test if the ring is a hole (i.e. if it is CCW) and set the hole flag 
		/// accordingly.
		/// </summary>
		public void ComputeRing()
		{
			if ( _ring != null ) return;   // don't compute more than once
			// create ring using geometry factory.
			_ring = _geometryFactory.CreateLinearRing( _pts );
			_isHole = _cga.IsCCW( _ring.GetCoordinates() );

		} // public void ComputeRing()

		abstract public DirectedEdge GetNext( DirectedEdge de );
		abstract public void SetEdgeRing( DirectedEdge de, EdgeRing er );


		/// <summary>
		/// Collect all the points from the DirectedEdges of this ring into a contiguous list.
		/// </summary>
		/// <param name="start"></param>
		protected void ComputePoints(DirectedEdge start)
		{
			_startDe = start;
			DirectedEdge de = start;
			bool isFirstEdge = true;
			do 
			{
				if ( de == null )
				{
					throw new InvalidOperationException("Found null Directed Edge.");
				}
				_edges.Add( de );

				Label label = de.Label;

				// Java used an assert here...
				if ( !label.IsArea() )
				{
					throw new InvalidOperationException("Label IsArea() is false when true is expected.");
				}
				
				MergeLabel( label );
				AddPoints( de.Edge, de.IsForward, isFirstEdge );
				isFirstEdge = false;
				SetEdgeRing( de, this );
				de = GetNext( de );			// GetNext will be implemented in the inherited class...this class is abstract.
			} while ( de != _startDe );
		} // protected void ComputePoints(DirectedEdge start)

		/// <summary>
		/// Returns the max node degree.  If not computed, will compute, otherwise returns the previously
		/// calculated value.
		/// </summary>
		/// <returns></returns>
		public int GetMaxNodeDegree()
		{
			if ( _maxNodeDegree < 0 )
			{
				ComputeMaxNodeDegree();
			}
			return _maxNodeDegree;
		} // public int GetMaxNodeDegree()

		private void ComputeMaxNodeDegree()
		{
			_maxNodeDegree = 0;
			DirectedEdge de = _startDe;
			do 
			{
				Node node = de.Node;
				int degree = ( (DirectedEdgeStar) node.Edges).GetOutgoingDegree(this);
				if ( degree > _maxNodeDegree )
				{
					_maxNodeDegree = degree;
				}
				de = GetNext(de);
			} while ( de != _startDe );

			_maxNodeDegree *= 2;
		} // private void ComputeMaxNodeDegree()

		/// <summary>
		/// 
		/// </summary>
		public void SetInResult()
		{
			DirectedEdge de = _startDe;
			do 
			{
				de.Edge.IsInResult = true;
				de = de.Next;
			} while ( de != _startDe );
		} // public void SetInResult()

		protected void MergeLabel( Label deLabel )
		{
			MergeLabel( deLabel, 0 );
			MergeLabel( deLabel, 1 );
		} // protected void MergeLabel( Label deLabel )

		/// <summary>
		/// Merge the RHS label from a DirectedEdge into the label for this EdgeRing.
		/// from a node which is NOT an intersection node between the Geometries
		/// (e.g. the end node of a LinearRing).  In this case the DirectedEdge label
		/// does not contribute any information to the overall labelling, and is simply skipped.
		/// </summary>
		/// <param name="deLabel"></param>
		/// <param name="geomIndex"></param>
		protected void MergeLabel( Label deLabel, int geomIndex )
		{
			int loc = deLabel.GetLocation( geomIndex, Position.Right );	// get the location for this label.

			// no information to be had from this label
			if ( loc == Location.Null ) return;

			// if there is no current RHS value, set it
			if ( _label.GetLocation( geomIndex ) == Location.Null ) 
			{
				_label.SetLocation( geomIndex, loc );
				return;
			}
		} // protected void MergeLabel( Label deLabel, int geomIndex )


		/// <summary>
		/// Adds the edges coordinates to this EdgeRings coordinates.
		/// </summary>
		/// <param name="edge"></param>
		/// <param name="isForward"></param>
		/// <param name="isFirstEdge"></param>
		protected void AddPoints( Edge edge, bool isForward, bool isFirstEdge )
		{
			Coordinates edgePts = edge.Coordinates;
			if ( isForward ) 
			{
				int startIndex = 1;
				if ( isFirstEdge )
				{
					startIndex = 0;
				}
				for ( int i = startIndex; i < edgePts.Count; i++ ) 
				{
					_pts.Add( edgePts[i] );
				}
			} // if ( isForward )
			else 
			{ // is backward
				int startIndex = edgePts.Count - 2;
				if ( isFirstEdge )
				{
					startIndex = edgePts.Count - 1;
				}
				for ( int i = startIndex; i >= 0; i-- ) 
				{
					_pts.Add( edgePts[i] );
				}
			} // else
		} // protected void AddPoints( Edge edge, bool isForward, bool isFirstEdge )
	
		/// <summary>
		/// This method will cause the ring to be computed. It will also check any holes, if they have been assigned.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool ContainsPoint(Coordinate p)
		{
			LinearRing shell = GetLinearRing();
			Envelope env = shell.GetEnvelopeInternal();
			if ( !env.Contains( p ) )
			{
				return false;
			}
			if ( !_cga.IsPointInRing( p, shell.GetCoordinates() ) )
			{
				return false;
			}
			foreach ( EdgeRing hole in _holes ) 
			{
				if ( hole.ContainsPoint( p ) )
				{
					return false;
				}
			}
			return true;
		} // public bool ContainsPoint(Coordinate p)
		#endregion

	} // public abstract class EdgeRing
}

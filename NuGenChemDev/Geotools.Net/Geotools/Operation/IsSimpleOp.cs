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
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
using Geotools.Graph.Index;
#endregion

namespace Geotools.Operation
{
	/// <summary>
	///  This class tests whether some kinds of Geometry are simple.
	///  Note that only Geometry's for which their definition allows them
	///  to be simple or non-simple are tested.  (E.g. Polygons must be simple
	///  by definition, so no test is provided.  To test whether a given Polygon is valid,
	///  use Geometry.IsValid
	/// </summary>
	internal class IsSimpleOp
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public IsSimpleOp() 
		{
		}

		/// <summary>
		/// Tests if LineString is simple.
		/// </summary>
		/// <param name="geometry">Geometry to test.  Must be of type LineString.</param>
		/// <returns>Returns True if geometry is simple.</returns>
		public bool IsSimple( LineString geometry )
		{
			return IsSimpleLinearGeometry(geometry);
		}

		/// <summary>
		/// Tests if MultiLineString is simple.
		/// </summary>
		/// <param name="geometry">Geometry to test.  Must be of type MultiLineString.</param>
		/// <returns>Returns True if geometry is simple.</returns>
		public bool IsSimple( MultiLineString geometry )
		{
			return IsSimpleLinearGeometry( geometry );
		}

		/// <summary>
		/// Tests if MultiPoint is simple.
		/// </summary>
		/// <param name="mp">Geometry to test.  Must be of type MultiPoint.</param>
		/// <returns>Returns True if geometry is simple.</returns>
		/// <remarks>A MultiPoint is simple if and only if it has no repeated points.</remarks>
		public bool IsSimple( MultiPoint mp )
		{
			if ( mp.IsEmpty() )
			{
				return true;
			}
			SortedList points = new SortedList();
			for ( int i = 0; i < mp.GetNumGeometries(); i++ ) 
			{
				Point pt = (Point) mp.GetGeometryN( i );
				Coordinate p = pt.GetCoordinate();
				if ( points.Contains( p ) )
				{
					return false;
				}
				points.Add( p, p );
			} //for ( int i = 0; i < mp.NumGeometries; i++ )
			return true;
		} // public bool IsSimple( MultiPoint mp )

		/// <summary>
		/// Tests to see if geometry is simple.
		/// </summary>
		/// <param name="geom">Geometry to test.</param>
		/// <returns>Returns true if geometry is simple, false otherwise.</returns>
		private bool IsSimpleLinearGeometry( Geometry geom )
		{
			if( geom.IsEmpty() )
			{
				return true;
			}
			GeometryGraph graph = new GeometryGraph( 0, geom );
			LineIntersector li = new RobustLineIntersector();
			SegmentIntersector si = graph.ComputeSelfNodes( li );
			// if no self-intersection, must be simple
			if( !si.HasIntersection )
			{
				return true;
			}
			if( si.HasProperIntersection )
			{
				return false;
			}
			if( HasNonEndpointIntersection( graph ) )
			{
				return false;
			}
			if( HasClosedEndpointIntersection( graph ) )
			{
				return false;
			}
			return true;
		} // private bool IsSimpleLinearGeometry( Geometry geom )

		/// <summary>
		/// For all edges, check if there are any intersections which are NOT at an endpoint.
		/// The Geometry is not simple if there are intersections not at endpoints.
		/// </summary>
		/// <param name="graph"></param>
		/// <returns></returns>
		private bool HasNonEndpointIntersection( GeometryGraph graph )
		{
			foreach ( object obj in graph.Edges )
			{
				Edge e = (Edge) obj;
				int maxSegmentIndex = e.MaximumSegmentIndex;
				foreach ( object eiObj in e.EdgeIntersectionList )
				{
					EdgeIntersection ei = (EdgeIntersection) eiObj;
					if ( !ei.IsEndPoint( maxSegmentIndex ) )
					{
						return true;
					}
				}
			} // foreach ( object obj in graph.Edges )
			return false;
		} // private bool HasNonEndpointIntersection(GeometryGraph graph)

		internal class EndpointInfo 
		{
			public Coordinate _pt;
			public bool _isClosed;
			public int _degree;

			public EndpointInfo( Coordinate pt )
			{
				_pt = pt;
				_isClosed = false;
				_degree = 0;
			}

			public void AddEndpoint( bool isClosed )
			{
				_degree++;
				_isClosed |= isClosed;
			}

		} // class EndpointInfo

		/// <summary>
		/// Test that no edge intersection is the endpoint of a closed line.  To check this we
		/// compute the degree of each endpoint.  The degree of endpoints of closed lines must be
		/// exactly 2.
		/// </summary>
		/// <param name="graph">The GeometryGraph to test.</param>
		/// <returns></returns>
		private bool HasClosedEndpointIntersection( GeometryGraph graph )
		{
			SortedList endPoints = new SortedList();
			foreach ( object obj in graph.Edges )
			{
				Edge e = (Edge) obj;
				int maxSegmentIndex = e.MaximumSegmentIndex;
				bool isClosed = e.IsClosed;
				Coordinate p0 = e.GetCoordinate( 0 );
				AddEndpoint( ref endPoints, p0, isClosed );
				Coordinate p1 = e.GetCoordinate( e.NumPoints - 1 );
				AddEndpoint( ref endPoints, p1, isClosed );
			}

			foreach( DictionaryEntry entry in endPoints )
			{
				EndpointInfo eiInfo = (EndpointInfo) entry.Value;
				if ( eiInfo._isClosed && eiInfo._degree != 2)
				{
					return true;
				}
			}
			return false;
		} // private bool HasClosedEndpointIntersection( GeometryGraph graph )

		/// <summary>
		/// Add an endpoint to the map, creating an entry for it if none exists.
		/// </summary>
		/// <param name="endPoints"></param>
		/// <param name="p"></param>
		/// <param name="isClosed"></param>
		private void AddEndpoint(ref SortedList endPoints, Coordinate p, bool isClosed )
		{
			EndpointInfo eiInfo = (EndpointInfo) endPoints[ p ];
			if ( eiInfo == null ) 
			{
				eiInfo = new EndpointInfo( p );
				endPoints.Add( p, eiInfo );
			}
			eiInfo.AddEndpoint( isClosed );
		} // private void AddEndpoint(ref IDictionary endPoints, Coordinate p, bool isClosed )
	}  // public class IsSimpleOp
}

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
using Geotools.Geometries;
using Geotools.Graph;

#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for PointLocator.
	/// </summary>
	internal class PointLocator
	{
		/// <summary>
		/// 
		/// </summary>
		protected CGAlgorithms _cga = new RobustCGAlgorithms();

		bool _isIn;         // true if the point lies in or on any Geometry element
		int _numBoundaries;    // the number of sub-elements whose boundaries the point lies in

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PointLocator class.
		/// </summary>
		public PointLocator()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		///		Determines the topological relationship (location) of a single point
		///		to a Geometry.  It handles both single-element and multi-element Geometries.  
		///		The algorithm for multi-part Geometries is more complex, since it has 
		///		to take into account the boundaryDetermination rule.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="geom"></param>
		/// <returns>Returns the location of the point relative to the input Geometry.</returns>
		public int Locate( Coordinate p, Geometry geom )
		{
			if ( geom.IsEmpty() ) return Location.Exterior;

			if ( geom is LineString )
			{
				return Locate( p, (LineString) geom );
			}
			if ( geom is LinearRing )
			{
				return Locate( p, (LinearRing) geom );
			}
			else if ( geom is Polygon ) 
			{
				return Locate( p, (Polygon) geom );
			}

			_isIn = false;
			_numBoundaries = 0;
			ComputeLocation( p, geom );
			if ( GeometryGraph.IsInBoundary( _numBoundaries ) )
			{
				return Location.Boundary;
			}
			if ( _numBoundaries > 0 || _isIn)
			{
				return Location.Interior;
			}
			return Location.Exterior;
		} // public int Locate( Coordinate p, Geometry geom )

		private void ComputeLocation(Coordinate p, Geometry geom)
		{
			if ( geom is LineString )
			{
				UpdateLocationInfo( Locate( p, (LineString) geom ) );
			}
			if ( geom is LinearRing ) 
			{
				UpdateLocationInfo( Locate( p, (LinearRing) geom ) );
			}
			else if ( geom is Polygon ) 
			{
				UpdateLocationInfo( Locate( p, (Polygon) geom ) );
			}
			else if ( geom is MultiLineString ) 
			{
				MultiLineString ml = (MultiLineString) geom;
				for ( int i = 0; i < ml.GetNumGeometries(); i++ ) 
				{
					LineString l = (LineString) ml.GetGeometryN( i );
					UpdateLocationInfo( Locate( p, l ) );
				} // for ( int i = 0; i < ml.NumGeometries; i++ )
			}
			else if ( geom is MultiPolygon ) 
			{
				MultiPolygon mpoly = (MultiPolygon) geom;
				for (int i = 0; i < mpoly.GetNumGeometries(); i++) 
				{
					Polygon poly = (Polygon) mpoly.GetGeometryN( i );
					UpdateLocationInfo( Locate( p, poly ) );
				} // for (int i = 0; i < mpoly.NumGeometries; i++)
			}
			else if ( geom is GeometryCollection ) 
			{
				GeometryCollection gc = geom as GeometryCollection;
				foreach( Geometry g2 in gc )
				{
					if ( g2 != geom )
					{
						ComputeLocation( p, g2 );
					}
				} // foreach( Geometry g2 in gc )
			} // else if ( geom is GeometryCollection )				
										 
		} // private void ComputeLocation(Coordinate p, Geometry geom)

		/// <summary>
		/// 
		/// </summary>
		/// <param name="loc"></param>
		private void UpdateLocationInfo( int loc )
		{

			if ( loc == Location.Interior )
			{
				_isIn = true;
			}
			if ( loc == Location.Boundary ) 
			{
				_numBoundaries++;
			}
		} //private void UpdateLocationInfo( int loc )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="l"></param>
		/// <returns></returns>
		private int Locate( Coordinate p, LineString l )
		{
			Coordinates pt = l.GetCoordinates();
			if ( !l.IsClosed() ) 
			{
				if ( p.Equals( pt[0] ) || p.Equals( pt[pt.Count - 1] ) ) 
				{
					return Location.Boundary;
				}
			}
			if ( _cga.IsOnLine( p, pt ) )
			{
				return Location.Interior;
			}
			return Location.Exterior;
		} // private int Locate( Coordinate p, LineString l )

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="ring"></param>
		/// <returns></returns>
		private int Locate( Coordinate p, LinearRing ring )
		{
			if ( _cga.IsOnLine( p, ring.GetCoordinates() ) ) 
			{
				return Location.Boundary;
			}
			if ( _cga.IsPointInRing( p, ring.GetCoordinates() ) )
			{
				return Location.Interior;
			}
			return Location.Exterior;
		} // private int Locate( Coordinate p, LinearRing ring )

		private int Locate( Coordinate p, Polygon poly )
		{
			if ( poly.IsEmpty() ) 
			{
				return Location.Exterior;
			}

			LinearRing shell = (LinearRing) poly.GetExteriorRing();

			int shellLoc = Locate( p, shell );

			if ( shellLoc == Location.Exterior )
			{
				return Location.Exterior;
			}
			if ( shellLoc == Location.Boundary )
			{
				return Location.Boundary;
			}

			// now test if the point lies in or on the holes
			for ( int i = 0; i < poly.GetNumInteriorRing(); i++ ) 
			{
				int holeLoc = Locate( p, poly.GetInteriorRingN( i ) );
				if ( holeLoc == Location.Interior )
				{
					return Location.Exterior;
				}
				if ( holeLoc == Location.Boundary )
				{
					return Location.Boundary;
				}
			} // for ( int i = 0; i < poly.NumInteriorRings; i++ )
			return Location.Interior;
		} // private int Locate( Coordinate p, Polygon poly )

		#endregion

	} // public class PointLocator
}

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
using System.IO;
using Geotools.IO;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// Creates Geometries using a precision model and SRID.
	/// </summary>
	public class GeometryFactory
	{
		private PrecisionModel _precisionModel;
		private int _SRID;

		#region Constructors
		/// <summary>
		/// Creates a GeometryFactory that creates Geometrys using the given precisionModel and SRID.
		/// </summary>
		/// <param name="precisionModel">the specification of the grid of allowable points for Geometrys created by this GeometryFactory.</param>
		/// <param name="SRID">The ID of the Spatial Reference System used by Geometry created by this GeometryFactory.</param>
		public GeometryFactory(PrecisionModel precisionModel, int SRID) 
		{
			_precisionModel = precisionModel;
			_SRID = SRID;
		}

		/// <summary>
		/// Creates a GeometryFactoryImpl that creates Geometrys using Floating precisionModel and an SRID of 0.
		/// </summary>
		public GeometryFactory() : this(new PrecisionModel(),0)
		{
		}
		#endregion

		#region Implementation of IGeometryFactory
		/// <summary>
		/// Creates a new geometry object given a string that contains well-known text.
		/// </summary>
		/// <param name="wkt">String containing well-known text.</param>
		/// <returns>A new geometry object.</returns>
		public Geometry CreateFromWKT(string wkt)
		{
			if (wkt==null)
			{
				throw new ArgumentNullException("wkt");
			}
			if (wkt.Trim()=="")
			{
				throw new ArgumentException("String must contain well known text representation of geometry.");
			}
			GeometryWKTReader reader = new GeometryWKTReader(this);
			return reader.Create(wkt);
		}

		/// <summary>
		/// Creates a new geometry object from a well known binary object.
		/// </summary>
		/// <param name="wkb">The wkb object.</param>
		/// <returns>A new geometry object.</returns>
		public Geometry CreateFromWKB(object wkb)
		{
			//TODO: awc? is this method required?
			if (wkb == null)
			{
				throw new ArgumentNullException("wkb");
			}
			byte[] bArray = (byte[])wkb;
			return CreateFromWKB(bArray);
		}

		/// <summary>
		/// Creates a new geometry object from a byte array that contains well-known binary.
		/// </summary>
		/// <param name="bArray">byte[] containing well-known binary.</param>
		/// <returns>A new geometry object.</returns>
		public Geometry CreateFromWKB(byte[] bArray)
		{
			if (bArray == null)
			{
				throw new ArgumentNullException("bArray");
			}
			MemoryStream memStream = new MemoryStream(bArray);
			BinaryReader bReader = new BinaryReader(memStream);
			return CreateFromWKB(bReader);
		}
		
		/// <summary>
		/// Creates a new geometry object from a BinaryReader that contains well-known binary.
		/// </summary>
		/// <param name="bReader">The BinaryReader that contains well-known binary.</param>
		/// <returns>A new geometry object.</returns>
		public Geometry CreateFromWKB(BinaryReader bReader)
		{
			if (bReader == null)
			{
				throw new ArgumentNullException("bReader");
			}
			GeometryWKBReader wkbReader = new GeometryWKBReader(this);
			return wkbReader.Create(bReader);
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ID of the Spatial Reference System used by all Geometrys created by this GeometryFactory
		/// </summary>
		public int SRID
		{
			get
			{
				return _SRID;
			}
		}

		/// <summary>
		/// The specification of the grid of allowable points for all Geometrys created by this GeometryFactory.
		/// </summary>
		public PrecisionModel PrecisionModel
		{
			get
			{
				return _precisionModel;
			}
		}
		#endregion

		#region Methods - that convert collections to arrays
		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="points">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static Point[] ToPointArray(ArrayList points) 
		{
			Point[] pointArray = new Point[points.Count];
			return (Point[]) points.ToArray(typeof(Point));
		}

		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="geometrys">The list of Geometrys to convert.</param>
		/// <returns>The List in array format.</returns>
		public static Geometry[] ToGeometryArray(ArrayList geometrys) 
		{
			Geometry[] geometryArray = new Geometry[geometrys.Count];
			return (Geometry[]) geometrys.ToArray(typeof(Geometry));
		}


		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="linearRings">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static LinearRing[] ToLinearRingArray(ArrayList linearRings) 
		{
			LinearRing[] linearRingArray = new LinearRing[linearRings.Count];
			return (LinearRing[]) linearRings.ToArray(typeof(LinearRing));
			//throw new NotImplementedException();
		}

	
		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="lineStrings">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static LineString[] ToLineStringArray(ArrayList lineStrings) 
		{
			LineString[] lineStringArray = new LineString[lineStrings.Count];
			return (LineString[]) lineStrings.ToArray(typeof(LineString));
		}

	
		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="polygons">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static Polygon[] ToPolygonArray(ArrayList polygons) 
		{
			Polygon[] polygonArray = new Polygon[polygons.Count];
			return (Polygon[]) polygons.ToArray(typeof(Polygon));
		}

	
		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="multiPolygons">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static MultiPolygon[] ToMultiPolygonArray(ArrayList multiPolygons) 
		{
			MultiPolygon[] multiPolygonArray = new MultiPolygon[multiPolygons.Count];
			return (MultiPolygon[]) multiPolygons.ToArray(typeof(MultiPolygon));
		}

		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <param name="multiLineStrings">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static MultiLineString[] ToMultiLineStringArray(ArrayList multiLineStrings) 
		{
			MultiLineString[] multiLineStringArray = new MultiLineString[multiLineStrings.Count];
			return (MultiLineString[]) multiLineStrings.ToArray(typeof(MultiLineString));
		}

		/// <summary>
		/// Converts the List to an array.
		/// </summary>
		/// <remarks>Not implemented.</remarks>
		/// <param name="multiPoints">The List to convert.</param>
		/// <returns>The List in array format.</returns>
		public static MultiPoint[] ToMultiPointArray(ArrayList multiPoints) 
		{
			//MultiPoint[] multiPointArray = new MultiPoint[multiPoints.Count];
			//return (MultiPoint[]) multiPoints.ToArray(multiPointArray);
			throw new NotImplementedException();
		}

		/// <summary>
		/// Converts an envnelope to a geometry.
		/// </summary>
		/// <param name="envelope">The Envelope to convert to a Geometry.</param>
		/// <param name="precisionModel">The specification of the grid of allowable points for the new Geometry.</param>
		/// <param name="SRID">The ID of the Spatial Reference System used by the Envelope.</param>
		/// <returns>an empty Point (for null Envelopes), a Point (when min x = max x and min y = max y) 
		/// or a Polygon (in all other cases)</returns>
		/// <exception cref="TopologyException"> if coordinates is not a closed linestring, that is, if the 
		/// first and last coordinates are not equal.</exception>
		public static Geometry ToGeometry(Envelope envelope, PrecisionModel precisionModel,	int SRID) 
		{
			
			if ( envelope.IsNull() ) 
			{
				return new Point( null, precisionModel, SRID );
			}
			if ( envelope.MinX == envelope.MaxX && envelope.MinY == envelope.MaxY ) 
			{
				return new Point( new Coordinate(envelope.MinX, envelope.MinY), precisionModel, SRID );
			}
			Coordinates coords = new Coordinates();
			coords.Add( new Coordinate( envelope.MinX, envelope.MinY ) );
			coords.Add(	new Coordinate( envelope.MaxX, envelope.MinY ) );
			coords.Add( new Coordinate( envelope.MaxX, envelope.MaxY ) );
			coords.Add( new Coordinate( envelope.MinX, envelope.MaxY ) );
			coords.Add( new Coordinate( envelope.MinX, envelope.MinY ) );
			return new Polygon( new LinearRing( coords, precisionModel, SRID), precisionModel, SRID);
		}
		#endregion

		#region Create factory methods

		/// <summary>
		/// Creates a point given a coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate to be used to create the point.</param>
		/// <returns>A new point.</returns>
		public Point CreatePoint(Coordinate coordinate) 
		{
			return new Point(coordinate, _precisionModel, _SRID);
		}

		/// <summary>
		/// Create a new multilinestring given an array of linestrings.
		/// </summary>
		/// <param name="lineStrings">The array of linestrings to be used to create the multilinestring.</param>
		/// <returns>A new multilinestring.</returns>
		public MultiLineString CreateMultiLineString(LineString[] lineStrings) 
		{
			return new MultiLineString(lineStrings, _precisionModel, _SRID);
		}

		/// <summary>
		/// Creates a new geometry collection given an array of geometries.
		/// </summary>
		/// <param name="geometries">The array of geometries to be used to create the collection.</param>
		/// <returns>A new geometry collection containing all the geometries in the geometry array.</returns>
		public GeometryCollection CreateGeometryCollection(Geometry[] geometries) 
		{
			return new GeometryCollection(geometries, _precisionModel, _SRID);
		}

		/// <summary>
		/// Creates a new multipolygon given an array of polygons.
		/// </summary>
		/// <param name="polygons">The array of polygons to be used to create the multipolygon.</param>
		/// <returns>A new multipolygon.</returns>
		public MultiPolygon CreateMultiPolygon(Polygon[] polygons) 
		{
			return new MultiPolygon(polygons, _precisionModel, _SRID);
		}

		/// <summary>
		/// Creates a new linear ring given a set of coordinates.
		/// </summary>
		/// <remarks>Throws an argument exception if the number of coordiates is less than 1.</remarks>
		/// <param name="coordinates">The coordinates to be used to create the linear ring.</param>
		/// <returns>A new linear ring.</returns>
		public LinearRing CreateLinearRing(Coordinates coordinates) 
		{
			
			LinearRing linearRing = new LinearRing(coordinates, _precisionModel, _SRID);
			if (coordinates != null && coordinates.Count > 0 && !coordinates[0].Equals2D(coordinates[coordinates.Count - 1])) 
			{
				throw new ArgumentException("LinearRing not closed");
			}
			return linearRing;
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new multipoint given an array of points.
		/// </summary>
		/// <param name="point">The array of points to be used to create the multipoint.</param>
		/// <returns>A new multipoint.</returns>
		public MultiPoint CreateMultiPoint(Point[] point) 
		{
			return new MultiPoint(point, _precisionModel, _SRID);
		}

		/// <summary>
		/// Creates a new multipoint given a set of points.
		/// </summary>
		/// <param name="points">The set of points to be used to create the multipoint.</param>
		/// <returns>A new multipoint.</returns>
		public MultiPoint CreateMultiPoint(Coordinates points) 
		{
			// for each point create the point object
			Point[] pts = new Point[ points.Count ];
			int index = 0;
			foreach ( Coordinate coord in points )
			{
				pts[index] = new Point( coord, _precisionModel, _SRID );
				index++;
			}
			return new MultiPoint( pts, _precisionModel, _SRID );
		}

		/// <summary>
		/// Creates a new multipoint given a set of coordinates.
		/// </summary>
		/// <remarks>Not Implemented.</remarks>
		/// <param name="coordinates">The set of coordinates to be used to create the multipoint.</param>
		/// <returns>A new multipoint.</returns>
		public MultiPoint CreateMultiPoint(Coordinate[] coordinates) 
		{
			/*
			if (coordinates == null) 
			{
				coordinates = new Coordinate[]{};
			}
			ArrayList points = new ArrayList();
			for (int i = 0; i < coordinates.length; i++) 
			{
				points.add(createPoint(coordinates[i]));
			}
			return createMultiPoint((Point[]) points.ToArray(new Point[]{}));
			*/
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a polygon given a linearring representing the shell of the polygon.
		/// </summary>
		/// <param name="shell">The linearring to be used to create the shell of the polygon.</param>
		/// <returns>A new polygon.</returns>
		public Polygon CreatePolygon(LinearRing shell)
		{
			return new Polygon(shell, _precisionModel, _SRID);	
		}

		/// <summary>
		/// Creates a polygon given a linearring representing the shell of 
		/// the polygon and an array of linearrings representing the interior holes of the polygon..
		/// </summary>
		/// <param name="shell">The linearring to be used to create the shell of the polygon.</param>
		/// <param name="holes">The array of linearrings to be used to create the interior 
		/// holes of the polygon.</param>
		/// <returns></returns>
		public Polygon CreatePolygon(LinearRing shell, LinearRing[] holes) 
		{
			return new Polygon(shell, holes, _precisionModel, _SRID);
		}

		/// <summary>
		/// Build an appropriate Geometry, MultiGeometry, or
		/// GeometryCollection to contain the Geometrys in it.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item><term>If geomList contains a single Polygon, the Polygon is returned.</term></item>
		///		<item><term>If geomList contains several Polygons, a MultiPolygon is returned.</term></item>
		///		<item><term>If geomList contains some Polygons and some LineStrings, a GeometryCollection is returned.</term></item>
		///		<item><term>If geomList is empty, an empty GeometryCollection is returned.</term></item>
		///	</list>
		/// </remarks>
		/// <param name="geomList">The Geometries to combine.</param>
		/// <returns>
		///	A Geometry of the "smallest", "most type-specific" class that can contain the elements of geomList.
		///	</returns>
		public Geometry BuildGeometry(ArrayList geomList) 
		{
			
			Type geomType = null;
			bool isHeterogeneous = false;
			bool isCollection = geomList.Count > 1;;
			foreach(Geometry geom in geomList)
			{
				Type partType = geom.GetType();
				if (geomType == null) 
				{
					geomType = partType;
				}
				if (partType != geomType) 
				{
					isHeterogeneous = true;
				}
			}

			// for the empty geometry, return an empty GeometryCollection
			if (geomType == null) 
			{
				return CreateGeometryCollection(null);
			}
			if (isHeterogeneous) 
			{
				return CreateGeometryCollection(ToGeometryArray(geomList));
			}
			Geometry geom0 = (Geometry) geomList[0];
			if (isCollection) 
			{
				if (geom0 is Polygon) 
				{
					return CreateMultiPolygon(ToPolygonArray(geomList));
				}
				else if (geom0 is LineString) 
				{
					return CreateMultiLineString(ToLineStringArray(geomList));
				}
				else if (geom0 is Point) 
				{
					return CreateMultiPoint(ToPointArray(geomList));
				}
				throw new InvalidOperationException("Should never reach here.");
			}
			return geom0;
		}

		/// <summary>
		/// Creates a new Linestring given a set of coordinates.
		/// </summary>
		/// <param name="coordinates">The set of coordinates to be used to create the linestring.</param>
		/// <returns>A new line string.</returns>
		public LineString CreateLineString(Coordinates coordinates) 
		{
			return new LineString(coordinates, _precisionModel, _SRID);
		}
		#endregion
	}
}

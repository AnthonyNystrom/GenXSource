/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
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
using System.IO;
using System.Collections;
using System.Diagnostics;
using Geotools.Geometries;
using Geotools.Utilities;
#endregion

namespace Geotools.IO
{
	/// <summary>
	///  Converts a Well-known Binary string to a Geometry.
	/// </summary>
	/// <remarks>The Well-known Binary format is defined in the 
	///  OpenGIS Simple Features Specification for SQL.
	/// </remarks> 
	public class GeometryWKBReader
	{
		private GeometryFactory _geometryFactory;
		private BinaryReader _bReader;

		#region Constructors

		/// <summary>
		/// Creates a GeometryWKTReader that creates objects using the given GeometryFactory.
		/// </summary>
		/// <param name="geometryFactory">The factory used to create Geometries.</param>
		public GeometryWKBReader(GeometryFactory geometryFactory) 
		{
			_geometryFactory = geometryFactory;
		}		
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Creates a memory stream from the supplied byte array.  Then calls the other create 
		/// method passing in the new binary reader created using the new memory stream.
		/// </summary>
		/// <param name="bArray">The byte array to be used to create the memory stream.</param>
		/// <returns>A Geometry.</returns>
		public Geometry Create(byte[] bArray)
		{
			//Create a memory stream using the suppiled byte array.
			MemoryStream memStream = new MemoryStream(bArray);

			//Create a new binary reader using the newly created memorystream.
			BinaryReader bReader = new BinaryReader(memStream);

			//Call the main create function.
			return Create(bReader);
		}

		/// <summary>
		/// Converts a Well-known binary representation to a Geometry.
		/// </summary>
		/// <param name="bReader">
		/// A byte array containing the definition of the geometry to be created.
		/// </param>
		/// <returns>
		/// Returns the Geometry specified by wellKnownBinary.  Throws an exception if there is a 
		/// parsing problem.
		/// </returns>
		public Geometry Create(BinaryReader bReader)
		{
			_bReader = bReader;

			//Get the first byte in the array.  This specifies if the WellKnownBinary is in
			//XDR(Big Endian) format of NDR(Little Endian) format.
            int format = _bReader.ReadByte();

			//If the format type is 0 it is XDR
			if(format == 0)
			{
				return Create();
			}
			//If the format type is 1 it is NDR
			if(format == 1)
			{
				return Create();
			}
			//If the format is neither 0 nor 1 there is a problem throw an exception
			throw new ArgumentException("Format type not recognized");
		}

		#endregion

		/// <summary>
		/// Converts a Well-known binary representation in NDR format to a Geometry.
		/// </summary>
		/// <returns>Returns the Geometry specified by wellKnownBinary.</returns>
		private Geometry Create()
		{
			//Get the type of this geometry.
			int typeGeom = (int)_bReader.ReadUInt32();
			
			switch(typeGeom)
			{
				//Type 1 is a point
				case 1:
					return CreateWKBPoint();
				//Type 2 is a LineString
				case 2:
					return CreateWKBLineString();
				//Type 3 is a Polygon
				case 3:
					return CreateWKBPolygon();
				//Type 4 is a MultiPoint
				case 4:
					return CreateWKBMultiPoint();
				//Type 5 is a MultiLineString
				case 5:
					return CreateWKBMultiLineString();
				//Type 6 is a MultiPolygon
				case 6:
					return CreateWKBMultiPolygon();
				//Type 7 is a GeometryCollection
				case 7:
					return CreateWKBGeometryCollection();
				//If the type is not 1-7 there is a problem throw an exception
				default:
					throw new ArgumentException("Geometry type not recognized");
			}
		}

		/// <summary>
		/// Creates a point from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBPoint()
		{
			//Create the x coordinate.
			double x = _bReader.ReadDouble();

			//Create the y coordinate.
			double y = _bReader.ReadDouble();

			//Create the coordinates.
			Coordinate coord = new Coordinate(x, y);

			//Create and return the point.
			return _geometryFactory.CreatePoint(coord);
		}

		private Coordinates ReadCoordinates()
		{
			//Get the number of points in this linestring.
			int numPoints = (int)_bReader.ReadUInt32();

			//Create an Array for the coordinates.
			Coordinates coords = new Coordinates();

			//Loop around the number of points.
			for(int i = 0; i < numPoints; i++)
			{
				//Create a new point.
				Point point = CreateWKBPoint() as Point;

				//Add the coordinates of the point to the coordinate.
				Coordinate coord = new Coordinate(point.X, point.Y);

				//Add the coordinate to the coordinates array.
				coords.Add(coord);
			}
			return coords;
		}
		/// <summary>
		/// Creates a linestring from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private LineString CreateWKBLineString()
		{
			Coordinates coords = ReadCoordinates();
			//Create and return the linestring.
			return _geometryFactory.CreateLineString(coords);
		}

		/// <summary>
		/// Creates a linar ring from WKB.
		/// </summary>
		/// <returns></returns>
		private LinearRing CreateWKBLinearRing()
		{
			Coordinates coords = ReadCoordinates();
			//Create and return the linearring.
			return _geometryFactory.CreateLinearRing(coords);
		}
		/// <summary>
		/// Creates a Polygon from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBPolygon()
		{
			//Get the Number of rings in this Polygon.
			int numRings = (int)_bReader.ReadUInt32();
			Debug.Assert(numRings>=1, "Number of rings in polygon must be 1 or more.");

			LinearRing shell = CreateWKBLinearRing();
			
			//Create a new array of linearrings for the interior rings.
			LinearRing[] interiorRings = new LinearRing[numRings-1];
			for(int i = 0; i < numRings-1; i++)
			{
				interiorRings[i] = CreateWKBLinearRing();
			}
				
			//Create and return the Poylgon.
			return _geometryFactory.CreatePolygon(shell, interiorRings);
		}

		/// <summary>
		/// Creates a Multipoint from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiPoint()
		{
			//Get the number of points in this multipoint.
			int numPoints = (int)_bReader.ReadUInt32();

			//Create a new array for the points.
			Point[] points = new Point[numPoints];

			//Loop on the number of points.
			for(int i = 0; i < numPoints; i++)
			{
				// read Point header
				_bReader.ReadByte();
				_bReader.ReadUInt32();
				//Create the next point and add it to the point array.
				points[i] = (Point)CreateWKBPoint();
			}
			//Create and return the MultiPoint.
			return _geometryFactory.CreateMultiPoint(points);
		}

		/// <summary>
		/// Creates a multilinestring from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiLineString()
		{
			//Get the number of linestrings in this multilinestring.
			int numLineStrings = (int)_bReader.ReadUInt32();

            //Create a new array for the linestrings .
			LineString[] lineStrings = new LineString[numLineStrings];
            
			//Loop on the number of linestrings.
			for(int i = 0; i < numLineStrings; i++)
			{
				//read Point header
				_bReader.ReadByte();
				_bReader.ReadUInt32();

				//Create the next linestring and add it to the array.
				lineStrings[i] = (LineString)CreateWKBLineString();
			}
			//Create and return the MultiLineString.
			return _geometryFactory.CreateMultiLineString(lineStrings);
		}

		/// <summary>
		/// Creates a multipolygon from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBMultiPolygon()
		{
			//Get the number of Polygons.
			int numPolygons = (int)_bReader.ReadUInt32();

			//Create a new array for the Polygons.
			Polygon[] polygons = new Polygon[numPolygons];

			//Loop on the number of polygons.
			for(int i = 0; i < numPolygons; i++)
			{
				// read polygon header
				_bReader.ReadByte();
				_bReader.ReadUInt32();
				//Create the next polygon and add it to the array.
				polygons[i] = (Polygon)CreateWKBPolygon();
			}
			//Create and return the MultiPolygon.
			return _geometryFactory.CreateMultiPolygon(polygons);
		}

		/// <summary>
		/// Creates a geometrycollection from the wkb.
		/// </summary>
		/// <returns>A geometry.</returns>
		private Geometry CreateWKBGeometryCollection()
		{
			//The next byte in the array tells the number of geometries in this collection.
			int numGeometries = (int)_bReader.ReadUInt32();

			//Create a new array for the geometries.
			Geometry[] geometries = new Geometry[numGeometries];

			//Loop on the number of geometries.
			for(int i = 0; i < numGeometries; i++)
			{
				//Call the main create function with the next geometry.
				geometries[i] = Create();
			}
			//Create and return the next geometry.
			return _geometryFactory.CreateGeometryCollection(geometries);
		}
	}
}

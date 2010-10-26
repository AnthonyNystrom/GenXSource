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
using Geotools.Geometries;
using Geotools.Utilities;
#endregion

namespace Geotools.IO
{
	/// <summary>
	///  Converts a Well-known Binary string to a Geometry.
	/// </summary>
	/// <remarks>The Well-known
	///  <para>Binary format is defined in the 
	///  OpenGIS Simple Features Specification for SQL</para>
	/// </remarks> 
	public class GeometryWKBWriter
	{
		private BinaryWriter _bWriter;
		private GeometryFactory _geometryFactory;

		#region Constructors

		/// <summary>
		/// The constructor for the binary writer.
		/// </summary>
		/// <param name="geometryFactory">The geometry used by this method.</param>
		public GeometryWKBWriter(GeometryFactory geometryFactory)
		{
			_geometryFactory = geometryFactory;
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// The binary write method.
		/// </summary>
		/// <param name="geometry">The geometry to be written.</param>
		/// <param name="bWriter">The binary writer to be used.</param>
		/// <param name="format">The format employed (big/little endian).</param>
		public BinaryWriter Write(Geometry geometry, BinaryWriter bWriter, byte format)
		{
			//Create the binary writer.
			_bWriter = bWriter;

			//Write the format.
			_bWriter.Write(format);

			//Write the type of this geometry
			WriteType(geometry);

			//Write the geometry
			WriteGeometry(geometry, format);

			return _bWriter;
		}

		/// <summary>
		/// Writes the type number for this geometry.
		/// </summary>
		/// <param name="geometry">The geometry to determine the type of.</param>
		private void WriteType(Geometry geometry)
		{
			//Determine the type of the geometry.
			switch( geometry.GetGeometryType() )
			{
				//Points are type 1.
				case "Point":
					_bWriter.Write(1);
					break;
				//Linestrings are type 2.
				case "LineString":
					_bWriter.Write(2);
					break;
				//Polygons are type 3.
				case "Polygon":
					_bWriter.Write(3);
					break;
				//Mulitpoints are type 4.
				case "MultiPoint":
					_bWriter.Write(4);
					break;
				//Multilinestrings are type 5.
				case "MultiLineString":
					_bWriter.Write(5);
					break;
				//Multipolygons are type 6.
				case "MultiPolygon":
					_bWriter.Write(6);
					break;
				//Geometrycollections are type 7.
				case "GeometryCollection":
					_bWriter.Write(7);
					break;
				//If the type is not of the above 7 throw an exception.
				default:
					throw new ArgumentException("Invalid Geometry Type");
			}
		}

		/// <summary>
		/// Writes the geometry to the binary writer.
		/// </summary>
		/// <param name="geometry">The geometry to be written.</param>
		private void WriteGeometry(Geometry geometry, byte format)
		{
			switch( geometry.GetGeometryType() )
			{
				//Write the point.
				case "Point":
					Point point = (Point)geometry;
					WritePoint(point);
					break;
				//Write the Linestring.
				case "LineString":
					LineString ls = (LineString)geometry;
					WriteLineString(ls, format);
					break;
				//Write the Polygon.
				case "Polygon":
					Polygon poly = (Polygon)geometry;
					WritePolygon(poly, format);
					break;
				//Write the Multipoint.
				case "MultiPoint":
					MultiPoint mp = (MultiPoint)geometry;
					WriteMultiPoint(mp, format);
					break;
				//Write the Multilinestring.
				case "MultiLineString":
					MultiLineString mls = (MultiLineString)geometry;
					WriteMultiLineString(mls, format);
					break;
				//Write the Multipolygon.
				case "MultiPolygon":
					MultiPolygon mPoly = (MultiPolygon)geometry;
					WriteMultiPolygon(mPoly, format);
					break;
				//Write the Geometrycollection.
				case "GeometryCollection":
					GeometryCollection gc = (GeometryCollection)geometry;
					WriteGeometryCollection(gc, format);
					break;
				//If the type is not of the aboce 7 throw an exception.
				default:
					throw new ArgumentException("Invalid Geometry Type");
			}
		}

		/// <summary>
		/// Writes a point.
		/// </summary>
		/// <param name="point">The point to be written.</param>
		private void WritePoint(Point point)
		{
			//Write the x coordinate.
			_bWriter.Write(point.X);
			//Write the y coordinate.
			_bWriter.Write(point.Y);
		}

		/// <summary>
		/// Writes a linestring.
		/// </summary>
		/// <param name="ls">The linestring to be written.</param>
		private void WriteLineString(LineString ls, byte format)
		{
			//Write the number of points in this linestring.
			_bWriter.Write( ls.GetNumPoints() );

			//Loop on each set of coordinates.
			foreach(Coordinate coord in ls.GetCoordinates() )
			{
				//Create a new point from the coordinates & write it.
				WritePoint(_geometryFactory.CreatePoint(coord));
			}
		}

		/// <summary>
		/// Writes a polygon.
		/// </summary>
		/// <param name="poly">The polygon to be written.</param>
		private void WritePolygon(Polygon poly, byte format)
		{
			//Get the number of rings in this polygon.
			int numRings = poly.GetNumInteriorRing() + 1;

			//Write the number of rings to the stream (add one for the shell)
			_bWriter.Write(numRings);

			//Get the shell of this polygon.
			WriteLineString(poly.Shell, format);

			//Loop on the number of rings - 1 because we already wrote the shell.
			for(int i = 0; i < numRings-1; i++)
			{
				//Populate the linearRing.
				LinearRing lr = poly.GetInteriorRingN( i );

				//Write the (lineString)LinearRing.
                WriteLineString((LineString)lr, format);
			}
		}

		/// <summary>
		/// Writes a multipoint.
		/// </summary>
		/// <param name="mp">The multipoint to be written.</param>
		private void WriteMultiPoint(MultiPoint mp, byte format)
		{
			//Get the number of points in this multipoint.
			int numPoints = mp.GetNumPoints();

			//Write the number of points.
			_bWriter.Write(numPoints);

			//Loop on the number of points.
			for(int i = 0; i < numPoints; i++)
			{
				//write the multipoint header
				_bWriter.Write(format);
				_bWriter.Write(4);

				_bWriter.Write(numPoints);

				//Write each point.
				WritePoint((Point)mp[i]);
			}
		}

		/// <summary>
		/// Writes a multilinestring.
		/// </summary>
		/// <param name="mls">The multilinestring to be written.</param>
		private void WriteMultiLineString(MultiLineString mls, byte format)
		{
			//Get the number of linestrings in this multilinestring.
			int numLineStrings = mls.GetNumGeometries();

			//Write the number of linestrings.
			_bWriter.Write(numLineStrings);

			//Loop on the number of linestrings.
			for(int i = 0; i < numLineStrings; i++)
			{
				//Write each linestring.
				WriteLineString((LineString)mls[i], format);
			}
		}

		/// <summary>
		/// Writes a multipolygon.
		/// </summary>
		/// <param name="mp">The mulitpolygon to be written.</param>
		private void WriteMultiPolygon(MultiPolygon mp, byte format)
		{
			//Get the number of polygons in this multipolygon.
			int numpolygons = mp.GetNumGeometries();

			//Write the number of polygons.
			_bWriter.Write(numpolygons);

			//Loop on the number of polygons.
			for(int i = 0; i < numpolygons; i++)
			{
				//Write the polygon header
				_bWriter.Write(format);
				_bWriter.Write(6);

				//Write each polygon.
				WritePolygon((Polygon)mp[i], format);
			}
		}

		/// <summary>
		/// Writes a geometrycollection.
		/// </summary>
		/// <param name="gc">The geometrycollection to be written.</param>
		private void WriteGeometryCollection(GeometryCollection gc, byte format)
		{
			//Get the number of geometries in this geometrycollection.
			int numGeometries = gc.GetNumGeometries();

			//Write the number of geometries.
			_bWriter.Write(numGeometries);

			//Loop on the number of geometries.
			for(int i = 0; i < numGeometries; i++)
			{
				//Write the type of each geometry.
				WriteType(gc[i]);

				//Write each geometry.
				WriteGeometry(gc[i], format);
			}
		}
		#endregion
	}
}

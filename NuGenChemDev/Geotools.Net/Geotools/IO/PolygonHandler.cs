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
using System.Collections;
using System.Diagnostics;
using Geotools.Algorithms;
using Geotools.Geometries;

#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Converts a Shapefile point to a OGIS Polygon.
	/// </summary>
	public class PolygonHandler : ShapeHandler
	{
		protected static CGAlgorithms _cga = new RobustCGAlgorithms();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PolygonHandler class.
		/// </summary>
		public PolygonHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		/// <summary>
		/// The ShapeType this handler handles.
		/// </summary>
		public override ShapeType ShapeType
		{
			get
			{
				return ShapeType.Polygon;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reads a stream and converts the shapefile record to an equilivent geometry object.
		/// </summary>
		/// <param name="file">The stream to read.</param>
		/// <param name="geometryFactory">The geometry factory to use when making the object.</param>
		/// <returns>The Geometry object that represents the shape file record.</returns>
		public override Geometry Read(BigEndianBinaryReader file, GeometryFactory geometryFactory)
		{
			int shapeTypeNum = file.ReadInt32();
			ShapeType shapeType = (ShapeType)Enum.Parse(typeof(ShapeType),shapeTypeNum.ToString());
			if (shapeType != ShapeType.Polygon)
			{
				throw new ShapefileException("Attempting to load a non-polygon as polygon.");
			}

			//read and for now ignore bounds.
			double[] box = new double[4];
			for (int i = 0; i < 4; i++) 
			{
				box[i] = file.ReadDouble();
			}

			int[] partOffsets;
        
			int numParts = file.ReadInt32();
			int numPoints = file.ReadInt32();
			partOffsets = new int[numParts];
			for (int i = 0; i < numParts; i++)
			{
				partOffsets[i] = file.ReadInt32();
			}

			ArrayList shells = new ArrayList();
			ArrayList holes = new ArrayList();

			int start, finish, length;
			for (int part = 0; part < numParts; part++)
			{
				start = partOffsets[part];
				if (part == numParts - 1)
				{
					finish = numPoints;
				}
				else 
				{
					finish = partOffsets[part + 1];
				}
				length = finish - start;
				Coordinates points = new Coordinates();
				points.Capacity=length;
				for (int i = 0; i < length; i++)
				{
					Coordinate external = new Coordinate(file.ReadDouble(), file.ReadDouble() );
					Coordinate internalCoord = geometryFactory.PrecisionModel.ToInternal(external);
					points.Add(internalCoord);
				}
				LinearRing ring = geometryFactory.CreateLinearRing(points);
				//Debug.Assert(ring.IsValid()==false,"Ring is not valid.");
				if (_cga.IsCCW(points))
				{
					holes.Add(ring);
				}
				else 
				{
					shells.Add(ring);
				}
			}

			//now we have a list of all shells and all holes
			ArrayList holesForShells = new ArrayList(shells.Count);
			for (int i = 0; i < shells.Count; i++)
			{
				holesForShells.Add(new ArrayList());
			}
			//find homes
			for (int i = 0; i < holes.Count; i++)
			{
				LinearRing testRing = (LinearRing) holes[i];
				LinearRing minShell = null;
				Envelope minEnv = null;
				Envelope testEnv = testRing.GetEnvelopeInternal();
				Coordinate testPt = testRing.GetCoordinateN(0);
				LinearRing tryRing;
				for (int j = 0; j < shells.Count; j++)
				{
					tryRing = (LinearRing) shells[j];
					Envelope tryEnv = tryRing.GetEnvelopeInternal();
					if (minShell != null) 
					{
						minEnv = minShell.GetEnvelopeInternal();
					}
					bool isContained = false;
					Coordinates coordList = tryRing.GetCoordinates() ;
					if (tryEnv.Contains(testEnv)
						&& (_cga.IsPointInRing(testPt,coordList ) ||
						(PointInList(testPt,coordList)))) 
					{
						isContained = true;
					}
					// check if this new containing ring is smaller than the
					// current minimum ring
					if (isContained) 
					{
						if (minShell == null
							|| minEnv.Contains(tryEnv)) 
						{
							minShell = tryRing;
						}
					}
				}
				//if (minShell==null)
				//{
				//	throw new InvalidOperationException("Could not find shell for a hole. Try a different precision model.");
				//}
			}
			Polygon[] polygons = new Polygon[shells.Count];
			for (int i = 0; i < shells.Count; i++)
			{
				polygons[i] = geometryFactory.CreatePolygon((LinearRing) shells[i], (LinearRing[])((ArrayList) holesForShells[i]).ToArray(typeof(LinearRing)));
			}
        
			if (polygons.Length == 1)
			{
				return polygons[0];
			}
			//it's a multi part
			return geometryFactory.CreateMultiPolygon(polygons);

		}
		/// <summary>
		/// Writes a Geometry to the given binary wirter.
		/// </summary>
		/// <param name="geometry">The geometry to write.</param>
		/// <param name="file">The file stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public override void Write(Geometry geometry, System.IO.BinaryWriter file, GeometryFactory geometryFactory)
		{
			if (geometry.IsValid()==false)
			{
				Trace.WriteLine("Invalid polygon being written.");
			}
			GeometryCollection multi;
			if(geometry is GeometryCollection)
			{
				multi = (GeometryCollection) geometry;
			}
			else 
			{
				GeometryFactory gf = new GeometryFactory(geometry.PrecisionModel, geometry.GetSRID());
				//multi = new MultiPolygon(new Polygon[]{(Polygon) geometry}, geometry.PrecisionModel, geometry.GetSRID());
				multi = gf.CreateMultiPolygon( new Polygon[]{(Polygon) geometry} );
			}
			//file.setLittleEndianMode(true);
			file.Write(int.Parse(Enum.Format(typeof(ShapeType),this.ShapeType,"d")));
        
			Envelope box = multi.GetEnvelopeInternal();
			Envelope bounds = ShapeHandler.GetEnvelopeExternal(geometryFactory.PrecisionModel,  box);
			file.Write(bounds.MinX);
			file.Write(bounds.MinY);
			file.Write(bounds.MaxX);
			file.Write(bounds.MaxY);
        
			int numParts = GetNumParts(multi);
			int numPoints = multi.GetNumPoints();
			file.Write(numParts);
			file.Write(numPoints);
        
			
			// write the offsets to the points
			int offset=0;
			for (int part = 0; part < multi.Count; part++)
			{
				// offset to the shell points
				Polygon polygon = (Polygon)multi[part];
				file.Write(offset);
				offset = offset + polygon.Shell.GetNumPoints();
				// offstes to the holes
				foreach (LinearRing ring in polygon.Holes)
				{
					file.Write(offset);
					offset = offset + ring.GetNumPoints();
				}	
			}


			// write the points 
			for (int part = 0; part < multi.Count; part++)
			{
				Polygon poly = (Polygon)multi[part];
				Coordinates points = poly.Shell.GetCoordinates();
				if (_cga.IsCCW(points)==true)
				{
					//points = points.ReverseCoordinateOrder();
				}
				WriteCoords(points, file, geometryFactory);
				foreach(LinearRing ring in poly.Holes)
				{
					Coordinates points2 = ring.GetCoordinates();
					if (_cga.IsCCW(points2)==false)
					{
						//points2 = points2.ReverseCoordinateOrder();
					}
					WriteCoords(points2, file, geometryFactory);
				}
			}
		}

		public void WriteCoords(Coordinates points, System.IO.BinaryWriter file, GeometryFactory geometryFactory)
		{
			Coordinate external;
			foreach (Coordinate point in points)
			{
				external = geometryFactory.PrecisionModel.ToExternal(point);
				file.Write(external.X);
				file.Write(external.Y);
			}

		}
		/// <summary>
		/// Gets the length of the shapefile record using the geometry passed in.
		/// </summary>
		/// <param name="geometry">The geometry to get the length for.</param>
		/// <returns>The length in bytes this geometry is going to use when written out as a shapefile record.</returns>
		public override int GetLength(Geometry geometry)
		{
			int numParts=GetNumParts(geometry);
			return (22 + (2 * numParts) + geometry.GetNumPoints() * 8);
		}
		
		private int GetNumParts(Geometry geometry)
		{
			int numParts=0;
			if (geometry is MultiPolygon)
			{
				foreach(Polygon poly in (MultiPolygon) geometry)
				{
					numParts = numParts + poly.Holes.Length+1;
				}
				//numParts = 0;((MultiPolygon) geometry).GetNumGeometries();
			}
			else if (geometry is Polygon)
			{
				numParts = ((Polygon) geometry).Holes.Length+1;
			}
			else
			{
				throw new InvalidOperationException("Should not get here.");
			}
			return numParts;
		}

		/// <summary>
		/// Test if a point is in a list of coordinates.
		/// </summary>
		/// <param name="testPoint">TestPoint the point to test for.</param>
		/// <param name="pointList">PointList the list of points to look through.</param>
		/// <returns>true if testPoint is a point in the pointList list.</returns>
		private bool PointInList(Coordinate testPoint, Coordinates pointList) 
		{
			foreach(Coordinate p in pointList)
			{
				if (p.Equals2D(testPoint))
				{
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}

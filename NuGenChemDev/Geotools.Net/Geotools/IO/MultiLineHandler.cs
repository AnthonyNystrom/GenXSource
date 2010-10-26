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
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Converts a Shapefile multi-line to a OGIS LineString/ MultiLineString.
	/// </summary>
	public class MultiLineHandler : ShapeHandler
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MultiLineHandler class.
		/// </summary>
		public MultiLineHandler() : base()
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Returns the ShapeType the handler handles.
		/// </summary>
		public override ShapeType ShapeType
		{
			get
			{
				return ShapeType.Arc;
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
			if (shapeType != ShapeType.Arc)
			{
				throw new ShapefileException("Attempting to load a non-arc as arc.");
			}
			//read and for now ignore bounds.
			double[] box = new double[4];
			for (int i = 0; i < 4; i++) 
			{
				double d= file.ReadDouble();
				box[i] =d;
			}


        
			int numParts = file.ReadInt32();
			int numPoints = file.ReadInt32();
			int[] partOffsets = new int[numParts];
			for (int i = 0; i < numParts; i++)
			{
				partOffsets[i] = file.ReadInt32();
			}
			
			LineString[] lines = new LineString[numParts];
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
				Coordinate external;
				for (int i = 0; i < length; i++)
				{
					external = new Coordinate(file.ReadDouble(),file.ReadDouble());
					points.Add( geometryFactory.PrecisionModel.ToInternal(external));
				}
				lines[part] = geometryFactory.CreateLineString(points);

			}
			return geometryFactory.CreateMultiLineString(lines);
		}

		/// <summary>
		/// Writes to the given stream the equilivent shape file record given a Geometry object.
		/// </summary>
		/// <param name="geometry">The geometry object to write.</param>
		/// <param name="file">The stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public override void Write(Geometry geometry, System.IO.BinaryWriter file, GeometryFactory geometryFactory)
		{
			MultiLineString multi = (MultiLineString) geometry;
			file.Write(int.Parse(Enum.Format(typeof(ShapeType),this.ShapeType,"d")));
        
			Envelope box = multi.GetEnvelopeInternal();
			file.Write(box.MinX);
			file.Write(box.MinY);
			file.Write(box.MaxX);
			file.Write(box.MaxY);
        
			int numParts = multi.GetNumGeometries();
			int numPoints = multi.GetNumPoints();
        
			file.Write(numParts);		
			file.Write(numPoints);
      
			//LineString[] lines = new LineString[numParts];
        
			// write the offsets
			int offset=0;
			for (int i = 0; i < numParts; i++)
			{
				Geometry g =  multi.GetGeometryN(i);
				file.Write( offset );
				offset = offset + g.GetNumPoints();
			}
        
			Coordinate	external;
			for (int part = 0; part < numParts; part++)
			{
				Coordinates points = multi.GetGeometryN(part).GetCoordinates();
				for (int i = 0; i < points.Count; i++)
				{
					external = geometryFactory.PrecisionModel.ToExternal(points[i]);
					file.Write(external.X);
					file.Write(external.Y);
				}
			}
		}


		/// <summary>
		/// Gets the length in bytes the Geometry will need when written as a shape file record.
		/// </summary>
		/// <param name="geometry">The Geometry object to use.</param>
		/// <returns>The length in bytes the Geometry will use when represented as a shape file record.</returns>
		public override int GetLength(Geometry geometry)
		{
			int numParts=GetNumParts(geometry);
			return (22 + (2 * numParts) + geometry.GetNumPoints() * 8);
		}

		private int GetNumParts(Geometry geometry)
		{
			int numParts=1;
			if (geometry is MultiLineString)
			{
				numParts = ((MultiLineString)geometry).Count;
			}
			return numParts;
		}
		#endregion
	}
}
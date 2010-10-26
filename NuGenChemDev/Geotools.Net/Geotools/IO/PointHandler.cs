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
	/// Converts a Shapefile point to a OGIS Point.
	/// </summary>
	public class PointHandler : ShapeHandler
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PointHandler class.
		/// </summary>
		public PointHandler() : base()
		{
		}
		#endregion

		/// <summary>
		/// The shape type this handler handles (point).
		/// </summary>
		public override ShapeType ShapeType
		{
			get
			{
				return ShapeType.Point;
			}
		}
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
			if (shapeType != ShapeType.Point)
			{
				throw new ShapefileException("Attempting to load a point as point.");
			}
			double x= file.ReadDouble();
			double y= file.ReadDouble();
			Coordinate external = new Coordinate(x,y);
			
			return geometryFactory.CreatePoint( geometryFactory.PrecisionModel.ToInternal(external) );

		}

		
		/// <summary>
		/// Writes to the given stream the equilivent shape file record given a Geometry object.
		/// </summary>
		/// <param name="geometry">The geometry object to write.</param>
		/// <param name="file">The stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public override void Write(Geometry geometry, System.IO.BinaryWriter file, GeometryFactory geometryFactory)
		{
			file.Write(int.Parse(Enum.Format(typeof(ShapeType), this.ShapeType,"d")));
			Coordinate external = geometryFactory.PrecisionModel.ToExternal( geometry.GetCoordinates()[0] );
			file.Write(external.X);
			file.Write(external.Y);
		}

		/// <summary>
		/// Gets the length in bytes the Geometry will need when written as a shape file record.
		/// </summary>
		/// <param name="geometry">The Geometry object to use.</param>
		/// <returns>The length in bytes the Geometry will use when represented as a shape file record.</returns>
		public override int GetLength(Geometry geometry)
		{
			return 10;//the length of two doubles in 16bit words + the shapeType
		}
		#endregion

	}
}

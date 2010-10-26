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
	/// Abstract class that defines the interfaces that other 'Shape' handlers must implement.
	/// </summary>
	public abstract class ShapeHandler 
	{
		public ShapeHandler()
		{
		}
		/// <summary>
		/// Returns the ShapeType the handler handles.
		/// </summary>
		public abstract  ShapeType ShapeType{get;}
		/// <summary>
		/// Reads a stream and converts the shapefile record to an equilivent geometry object.
		/// </summary>
		/// <param name="file">The stream to read.</param>
		/// <param name="geometryFactory">The geometry factory to use when making the object.</param>
		/// <returns>The Geometry object that represents the shape file record.</returns>
		public abstract  Geometry Read(BigEndianBinaryReader file, GeometryFactory geometryFactory);
		/// <summary>
		/// Writes to the given stream the equilivent shape file record given a Geometry object.
		/// </summary>
		/// <param name="geometry">The geometry object to write.</param>
		/// <param name="file">The stream to write to.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public abstract  void Write(Geometry geometry, System.IO.BinaryWriter file,  GeometryFactory geometryFactory);
		/// <summary>
		/// Gets the length in bytes the Geometry will need when written as a shape file record.
		/// </summary>
		/// <param name="geometry">The Geometry object to use.</param>
		/// <returns>The length in bytes the Geometry will use when represented as a shape file record.</returns>
		public abstract  int GetLength(Geometry geometry); //length in 16bit words

		public static Envelope GetEnvelopeExternal(PrecisionModel precisionModel, Envelope envelope)
		{
			// get envelopse in external coordinates
			Coordinate min = new Coordinate(envelope.MinX, envelope.MinY); 
			Coordinate max = new Coordinate(envelope.MaxX, envelope.MaxY);
			min = precisionModel.ToExternal(min);
			max = precisionModel.ToExternal(max);
			Envelope bounds = new Envelope(min.X, max.X, min.Y, max.Y);
			return bounds;
		}
	}
}

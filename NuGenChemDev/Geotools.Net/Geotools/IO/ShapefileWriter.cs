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
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// This class writes ESRI Shapefiles.
	/// </summary>
	public class ShapefileWriter
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ShapefileWriter class.
		/// </summary>
		public ShapefileWriter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Methods
		
		/// <summary>
		/// Writes a shapefile to disk.
		/// </summary>
		/// <remarks>
		/// <para>Assumes the type given for the first geometry is the same for all subsequent geometries.
		/// For example, is, if the first Geometry is a Multi-polygon/ Polygon, the subsequent geometies are
		/// Muli-polygon/ polygon and not lines or points.</para>
		/// <para>The dbase file for the corresponding shapefile contains one column called row. It contains 
		/// the row number.</para>
		/// </remarks>
		/// <param name="filename">The filename to write to (minus the .shp extension).</param>
		/// <param name="geometryCollection">The GeometryCollection to write.</param>
		/// <param name="geometryFactory">The geometry factory to use.</param>
		public static void Write(string filename, GeometryCollection geometryCollection, GeometryFactory geometryFactory)
		{
			System.IO.FileStream shpStream = new System.IO.FileStream(filename+".shp", System.IO.FileMode.Create);
			System.IO.FileStream shxStream = new System.IO.FileStream(filename+".shx", System.IO.FileMode.Create);
			BigEndianBinaryWriter shpBinaryWriter = new BigEndianBinaryWriter(shpStream);
			BigEndianBinaryWriter shxBinaryWriter = new BigEndianBinaryWriter(shxStream);
			
			// assumes
			ShapeHandler handler = Shapefile.GetShapeHandler(Shapefile.GetShapeType(geometryCollection[0]));

			Geometry body;
			int numShapes = geometryCollection.GetNumGeometries();
			// calc the length of the shp file, so it can put in the header.
			int shpLength =50;
			for (int i = 0; i < numShapes; i++) 
			{
				body = geometryCollection[i];
				shpLength += 4; // length of header in WORDS
				shpLength += handler.GetLength(body); // length of shape in WORDS
			}

			int shxLength = 50+ (4*numShapes);


			// write the .shp header
			ShapefileHeader shpHeader = new ShapefileHeader();
			shpHeader.FileLength = shpLength;

			// get envelopse in external coordinates
			Envelope env = geometryCollection.GetEnvelopeInternal();
			Envelope bounds = ShapeHandler.GetEnvelopeExternal(geometryFactory.PrecisionModel,  env);
			shpHeader.Bounds = bounds;


			// assumes Geometry type of the first item will the same for all other items
			// in the collection.
			shpHeader.ShapeType = Shapefile.GetShapeType( geometryCollection[0] );
			shpHeader.Write(shpBinaryWriter);

			// write the .shx header
			ShapefileHeader shxHeader = new ShapefileHeader();
			shxHeader.FileLength = shxLength;
			shxHeader.Bounds = shpHeader.Bounds;
			
			// assumes Geometry type of the first item will the same for all other items
			// in the collection.
			shxHeader.ShapeType = Shapefile.GetShapeType( geometryCollection[0] );
			shxHeader.Write(shxBinaryWriter);



			// write the individual records.
			int _pos = 50; // header length in WORDS
			for (int i = 0; i < numShapes; i++) 
			{
				body = geometryCollection[i];
				int recordLength = handler.GetLength(body);
				Debug.Assert( Shapefile.GetShapeType(body)!=shpHeader.ShapeType, String.Format("Item {0} in the GeometryCollection is not the same Shapetype as Item 0.",i));
				shpBinaryWriter.WriteIntBE(i+1);
				shpBinaryWriter.WriteIntBE(recordLength);
				
				
				shxBinaryWriter.WriteIntBE(_pos);
				shxBinaryWriter.WriteIntBE(recordLength);
				
				_pos += 4; // length of header in WORDS
				handler.Write(body, shpBinaryWriter,  geometryFactory);
				_pos += recordLength; // length of shape in WORDS
			}

			shxBinaryWriter.Flush();
			shxBinaryWriter.Close();
			shpBinaryWriter.Flush();
			shpBinaryWriter.Close();

			Debug.Assert(_pos!=shpLength,"File length in header and actual file length do not match.");
			//stream.Close();
			//Trace.WriteLineIf(Shapefile.TraceSwitch.Enabled,"File length pos:"+_pos*2+" bytes");
			//Trace.WriteLineIf(Shapefile.TraceSwitch.Enabled,"File length pos "+_pos+ " words");

			WriteDummyDbf(filename+".dbf", numShapes);	
		}

		public static void WriteDummyDbf(string filename, int recordCount)
		{
			DbaseFileHeader dbfHeader = new DbaseFileHeader();

			dbfHeader.AddColumn("row",'N',11,0);
			
			DbaseFileWriter dbfWriter = new DbaseFileWriter(filename);
			dbfWriter.Write(dbfHeader);
			for (int i=0; i < recordCount; i++)
			{
				ArrayList columnValues = new ArrayList();
				columnValues.Add((double)i);
				dbfWriter.Write(columnValues);
			}
			dbfWriter.Close();
		}

		#endregion

	}
}

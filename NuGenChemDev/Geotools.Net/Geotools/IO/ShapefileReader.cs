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
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	

	/// <summary>
	/// This class represnts an ESRI Shapefile.
	/// </summary>
	public class ShapefileReader : IEnumerable
	{
		/// <summary>
		/// Summary description for ShapefileEnumerator.
		/// </summary>
		private class ShapefileEnumerator : IEnumerator
		{
			private ShapefileReader _parent;
			private Geometry _geometry;
			private ShapeHandler _handler;
			private BigEndianBinaryReader _shpBinaryReader = null;

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the ShapefileEnumerator class.
			/// </summary>
			public ShapefileEnumerator(ShapefileReader shapefile)
			{
				
				_parent = shapefile;

				// create a file stream for each enumerator that is given out. This allows the same file
				// to have one or more enumerator. If we used the parents stream - than only one IEnumerator 
				// could be given out.
				FileStream stream = new FileStream(_parent._filename, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
				_shpBinaryReader = new BigEndianBinaryReader(stream);
				
				// skip header - since parent has already read this.
				_shpBinaryReader.ReadBytes(100);
				ShapeType type = _parent._mainHeader.ShapeType;
				_handler = Shapefile.GetShapeHandler(type);
				if (_handler == null) 
				{
					throw new NotSupportedException("Unsuported shape type:" + type);
				}
			}
			#endregion

			#region Implementation of IEnumerator
			public void Reset()
			{
				throw new InvalidOperationException();
			}

			public bool MoveNext()
			{
				if  (_shpBinaryReader.PeekChar()!=-1)
				{
					int recordNumber = _shpBinaryReader.ReadIntBE();
					int contentLength = _shpBinaryReader.ReadIntBE();
					if (Shapefile.TraceSwitch.Enabled)
					{
						Trace.WriteLine("Record number :"+recordNumber);
						Trace.WriteLine("contentLength :"+contentLength);
					}
					_geometry  = _handler.Read(_shpBinaryReader, _parent._geometryFactory);
					return true;
				}
				else
				{
					// reached end of file, so close the reader.
					_shpBinaryReader.Close();
					return false;
				}
			}

			public object Current
			{
				get
				{
					return _geometry;
				}
			}
			#endregion
		}

		

		
		
		private ShapefileHeader _mainHeader = null;
		private GeometryFactory _geometryFactory=null;
		private string _filename;

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the Shapefile class with the given parameters.
		/// </summary>
		/// <param name="filename">The filename of the shape file to read (with .shp).</param>
		/// <param name="geometryFactory">The GeometryFactory to use when creating Geometry objects.</param>
		public ShapefileReader(string filename, GeometryFactory geometryFactory)
		{
			if (filename==null)
			{
				throw new ArgumentNullException("filename");
			}
			if (geometryFactory==null)
			{
				throw new ArgumentNullException("geometryFactory");
			}
			_filename = filename;
			Trace.WriteLineIf(Shapefile.TraceSwitch.Enabled,"Reading filename:"+filename);
			
			_geometryFactory = geometryFactory;

			// read header information. note, we open the file, read the header information and then
			// close the file. This means the file is not opened again until GetEnumerator() is requested.
			// For each call to GetEnumerator() a new BinaryReader is created.
			FileStream stream = new FileStream(filename, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
			BigEndianBinaryReader shpBinaryReader = new BigEndianBinaryReader(stream);
			_mainHeader = new ShapefileHeader(shpBinaryReader);
			shpBinaryReader.Close();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the bounds of the shape file.
		/// </summary>
		public ShapefileHeader Header
		{
			get
			{
				return _mainHeader;
			}
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		/// Reads the shapefile and returns a GeometryCollection representing all the records in the shapefile.
		/// </summary>
		/// <returns>GeometryCollection representing every record in the shapefile.</returns>
		public GeometryCollection ReadAll()
		{
			ArrayList list = new ArrayList();
			ShapeType type = _mainHeader.ShapeType;
			ShapeHandler handler = Shapefile.GetShapeHandler(type);
			if (handler == null) 
			{
				throw new NotSupportedException("Unsupported shape type:" + type);
			}

			int i=0;
			foreach (Geometry geometry in this)
			{
				list.Add(geometry);
				i++;
			}
			
			Geometry[] geomArray = GeometryFactory.ToGeometryArray(list);
			return _geometryFactory.CreateGeometryCollection(geomArray);
			//return geometryCollection;
		}
		


	
		#endregion

		#region Implementation of IEnumerable
		public IEnumerator GetEnumerator()
		{
			return new ShapefileEnumerator(this);
		}
		#endregion

	}
}

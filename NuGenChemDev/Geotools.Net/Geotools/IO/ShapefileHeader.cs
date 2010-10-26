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
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Class that represents a shape file header record.
	/// </summary>
	public class ShapefileHeader
	{
		private int _fileCode = Shapefile.ShapefileId;
		private int _fileLength = -1;
		private int _version = 1000;
		private ShapeType _shapeType = ShapeType.Undefined;
		private Envelope _bounds;

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the ShapefileHeader class with values read in from the stream.
		/// </summary>
		/// <remarks>Reads the header information from the stream.</remarks>
		/// <param name="shpBinaryReader">BigEndianBinaryReader stream to the shapefile.</param>
		public ShapefileHeader(BigEndianBinaryReader shpBinaryReader)
		{
			if (shpBinaryReader==null)
			{
				throw new ArgumentNullException("shpBinaryReader");
			}

			_fileCode = shpBinaryReader.ReadIntBE();	
			if (_fileCode!=Shapefile.ShapefileId)
			{
				throw new ShapefileException("The first four bytes of this file indicate this is not a shape file.");
			}
			// skip 5 unsed bytes
			shpBinaryReader.ReadIntBE();
			shpBinaryReader.ReadIntBE();
			shpBinaryReader.ReadIntBE();
			shpBinaryReader.ReadIntBE();
			shpBinaryReader.ReadIntBE();

			_fileLength = shpBinaryReader.ReadIntBE();

			_version = shpBinaryReader.ReadInt32();
			Debug.Assert(_version==1000, "Shapefile version", String.Format("Expecting only one version (1000), but got {0}",_version));
			int shapeType = shpBinaryReader.ReadInt32();
			_shapeType = (ShapeType)Enum.Parse(typeof(ShapeType),shapeType.ToString());

			//read in and store the bounding box
			double[] coords = new double[4];
			for (int i = 0; i < 4; i++)
			{
				coords[i]=shpBinaryReader.ReadDouble();
			}
			_bounds = new Envelope(coords[0], coords[2], coords[1], coords[3]);
			
			// skip z and m bounding boxes.
			for (int i=0; i < 4; i++)
			{
				shpBinaryReader.ReadDouble();	
			}
		}

		/// <summary>
		/// Initializes a new instance of the ShapefileHeader class.
		/// </summary>
		public ShapefileHeader()
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets and sets the bounds of the shape file.
		/// </summary>
		public Envelope Bounds
		{
			get
			{
				return _bounds;
			}
			set
			{
				_bounds = value;
			}
		}
		/// <summary>
		/// Gets and sets the shape file type i.e. polygon, point etc...
		/// </summary>
		public ShapeType ShapeType
		{
			get
			{
				return _shapeType;
			}
			set
			{
				_shapeType = value;
			}
		}

		/// <summary>
		/// Gets and sets the shapefile version.
		/// </summary>
		public int Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		/// <summary>
		/// Gets and sets the length of the shape file in words.
		/// </summary>
		public int FileLength
		{
			get
			{
				return _fileLength;
			}
			set
			{
				_fileLength = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Writes a shapefile header to the given stream;
		/// </summary>
		/// <param name="file">The binary writer to use.</param>
		public void Write(BigEndianBinaryWriter file) 
		{
			if (file==null)
			{
				throw new ArgumentNullException("file");
			}
			if (_fileLength==-1)
			{
				throw new InvalidOperationException("The header properties need to be set before writing the header record.");
			}
			int pos = 0;
			//file.setLittleEndianMode(false);
			file.WriteIntBE(_fileCode);
			pos += 4;
			for (int i = 0; i < 5; i++)
			{
				file.WriteIntBE(0);//Skip unused part of header
				pos += 4;
			}
			file.WriteIntBE(_fileLength);
			pos += 4;
			//file.setLittleEndianMode(true);
			file.Write(_version);
			pos += 4;
			
			file.Write(int.Parse(Enum.Format(typeof(ShapeType),_shapeType,"d")));
			pos += 4;
			//write the bounding box
			file.Write(_bounds.MinX);
			file.Write(_bounds.MinY);
			file.Write(_bounds.MaxX);
			file.Write(_bounds.MaxY);

			pos += 8 * 4;
        
			//skip remaining unused bytes
			//file.setLittleEndianMode(false);//well they may not be unused forever...
			for (int i = 0; i < 4; i++)
			{
				file.Write(0.0);//Skip unused part of header
				pos += 8;
			}
			Trace.WriteLineIf(Shapefile.TraceSwitch.Enabled,"Header pos:"+pos);
		}
		#endregion

	}
}

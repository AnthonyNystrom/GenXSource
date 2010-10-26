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
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Geotools.Geometries;
using Geotools.IO;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Creates a IDataReader that can be used to enumerate through an ESRI shape file.
	/// </summary>
	/// <remarks>
	/// <para>The first field (called Geometry) contains a the well known binary representing the corresponding shape record.
	/// The remaining fields correspond to the fields in the .dbf file.</para>
	/// <para>
	/// To create a ShapefileDataReader, use the static methods on the Shapefile class.
	/// </para>
	/// </remarks>
	public class ShapefileDataReader : IDataReader, IDataRecord, IEnumerable
	{
		
		internal class ShapefileDataReaderEnumerator : IEnumerator
		{
			ShapefileDataReader _parent;
			public ShapefileDataReaderEnumerator(ShapefileDataReader parent)
			{
				_parent = parent;
			}

			#region Implementation of IEnumerator
			public void Reset()
			{
				throw new NotImplementedException();
			}

			public bool MoveNext()
			{
				return _parent.Read();
			}

			public object Current
			{
				get
				{
					return new RowStructure( _parent._dbaseFields, _parent._columnValues );
				}
			}
			#endregion
		}
		bool _open=false;
		DbaseFieldDescriptor[] _dbaseFields;
		ArrayList _columnValues;
		DbaseFileReader _dbfReader;
		ShapefileReader _shpReader;
		IEnumerator _dbfEnumerator;
		IEnumerator _shpEnumerator;
		Geometry _shpRecord = null;
		GeometryFactory _geometryFactory;
		ShapefileHeader _shpHeader;
		DbaseFileHeader _dbfHeader;
		bool _moreRecords = false;
		int _recordCount=0;
		byte[] _wkb;
		GeometryWKBWriter _wkbWriter;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ShapefileDataReader class.
		/// </summary>
		/// <param name="filename">The shapefile to read (minus the .shp extension)</param>
		///<param name="geometryFactory">The GeometryFactory to use.</param>
		public ShapefileDataReader(string filename, GeometryFactory geometryFactory) 
		{
			if (filename==null)
			{
				throw new ArgumentNullException("filename");
			}
			if (geometryFactory==null)
			{
				throw new ArgumentNullException("geometryFactory");
			}
			_geometryFactory = geometryFactory;
			_open=true;
			

			if (filename.ToLower().EndsWith(".shp"))
			{
				filename = filename.ToLower().Replace(".shp","");
			}
			
			 _dbfReader = new DbaseFileReader(filename+".dbf");
			 _shpReader = new ShapefileReader(filename+".shp", geometryFactory);

			_dbfHeader =  _dbfReader.GetHeader();
			_recordCount = _dbfHeader.NumRecords;

			_wkbWriter = new GeometryWKBWriter(_geometryFactory);
			
			// copy dbase fields to our own array. Insert into the first position, the shape column
			_dbaseFields = new DbaseFieldDescriptor[_dbfHeader.Fields.Length + 1];
			_dbaseFields[0] = DbaseFieldDescriptor.ShapeField();
			//_dbaseFields[1] = DbaseFieldDescriptor.IdField();
			for(int i=0; i < _dbfHeader.Fields.Length; i++)
			{
				_dbaseFields[i+1] = _dbfHeader.Fields[i];
			}
			
			_shpHeader = _shpReader.Header;
			
			_dbfEnumerator = _dbfReader.GetEnumerator();
			_shpEnumerator = _shpReader.GetEnumerator();

			_moreRecords = true;
		}

		#endregion

		#region Implementation of IDataReader


		/// <summary>
		/// Advances the data reader to the next result, when reading the shapefile.
		/// </summary>
		/// <returns>false.</returns>
		public bool NextResult()
		{
			return false;

		}

		/// <summary>
		/// Closes the IDataReader 0bject.
		/// </summary>
		public void Close()
		{
			_open=false;
		}

		/// <summary>
		/// Advances the IDataReader to the next record.
		/// </summary>
		/// <returns>true if there are more rows; otherwise, false.</returns>
		public bool Read()
		{
			bool moreDbfRecords = _dbfEnumerator.MoveNext();
			bool moreShpRecords = _shpEnumerator.MoveNext();

			if (moreDbfRecords==false)
			{
				int a=0;
				a++;
			}
			if (moreShpRecords==false)
			{
				int b=0;
				b++;
			}
			_moreRecords = moreDbfRecords && moreShpRecords;
		
			//bool moreRecords = _currentRecord < _dbfReader.GetHeader().NumRecords;
			

			//Debug.Assert(moreDbfRecords==moreShpRecords==moreRecords,"Differing number of records in shape and .dbf file");

			// get current shape 
			_shpRecord = (Geometry)_shpEnumerator.Current;

			// convert to wkb
			MemoryStream stream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			_wkbWriter.Write(_shpRecord, binaryWriter,1);
			binaryWriter.Close();
			_wkb = stream.GetBuffer();

			// get current dbase record
			_columnValues = (ArrayList)_dbfEnumerator.Current;

			// insert wkb as first record
			_columnValues.Insert(0,_wkb );
			
			//Debug.Assert(moreDbfRecords!=moreShpRecords,"Number of records in .dbf and .shp do not match.");

			return (moreDbfRecords && moreDbfRecords);
		}

		/// <summary>
		/// Returns a DataTable that describes the column metadata of the IDataReader.
		/// </summary>
		/// <returns>A DataTable that describes the column metadata.</returns>
		public System.Data.DataTable GetSchemaTable()
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Not applicable for this data reader.
		/// </summary>
		/// <value>Always -1 for this data reader.</value>
		public int RecordsAffected
		{
			/*
				* RecordsAffected is only applicable to batch statements
				* that include inserts/updates/deletes. The sample always
				* returns -1.
				*/
			get
			{
				return -1;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the data reader is closed.
		/// </summary>
		/// <value>true if the data reader is closed; otherwise, false.</value>
		/// <remarks>IsClosed and RecordsAffected are the only properties that you can call after the IDataReader is closed.</remarks>
		public bool IsClosed
		{
			get
			{
				return !_open;
			}
		}

		/// <summary>
		/// Always return a value of zero since nesting is not supported.
		/// </summary>
		/// <value>The level of nesting.</value>
		/// <remarks>The outermost table has a depth of zero.</remarks>
		public int Depth
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Gets the numbers of records in the Shapefile.
		/// </summary>
		public int RecordCount
		{
			get
			{
				return _recordCount;
			}
		}
		#endregion

		#region Implementation of IDisposable
		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			throw new NotSupportedException();
		}
		#endregion

		#region Implementation of IDataRecord
		public int GetInt32(int i)
		{
			return (int)_columnValues[i];
		}

		public object GetValue(int i)
		{
			return _columnValues[i];
		}

		public bool IsDBNull(int i)
		{
			return (GetValue(i)==null);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return 0;
		}

		public byte GetByte(int i)
		{
			return 0;
		}

		public System.Type GetFieldType(int i)
		{
			return _dbaseFields[i].Type;
		}

		public decimal GetDecimal(int i)
		{
			return 0;
		}

		public int GetValues(object[] values)
		{
			int i=0;
			for( i= 0; i < values.Length; i++)
			{
				values[i] = _columnValues[i];
			}
			return i;
		}

		public string GetName(int i)
		{
			return this._dbaseFields[i].Name;
		}

		public long GetInt64(int i)
		{
			return (long)_columnValues[i];
		}

		public double GetDouble(int i)
		{
			return (double)_columnValues[i];
		}

		public bool GetBoolean(int i)
		{
			return (bool)_columnValues[i];
		}

		public System.Guid GetGuid(int i)
		{
			return new System.Guid();
		}

		public System.DateTime GetDateTime(int i)
		{
			return new System.DateTime();
		}

		public int GetOrdinal(string name)
		{
			for(int i=0; i < _dbfReader.GetHeader().NumFields; i++)
			{
				if (0==CultureAwareCompare(_dbfReader.GetHeader().Fields[i].Name,name))
				{
					return i;
				}
			}
			// Throw an exception if the ordinal cannot be found.
			throw new IndexOutOfRangeException("Could not find specified column in results.");

		}

		public string GetDataTypeName(int i)
		{
			return _dbaseFields[i].Type.ToString();
		}

		public float GetFloat(int i)
		{
			return (float)_columnValues[i];
		}

		public System.Data.IDataReader GetData(int i)
		{
			/*
				* The sample code does not support this method. Normally,
				* this would be used to expose nested tables and
				* other hierarchical data.
				*/
			throw new NotSupportedException("GetData not supported.");

		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			//HACK:
			string str = _columnValues[i].ToString();
			str.CopyTo((int)fieldoffset,buffer,0,length);
			return length;
			 
		}

		public string GetString(int i)
		{
			return _columnValues[i].ToString();
		}

		public char GetChar(int i)
		{
			return (char)_columnValues[i];
		}

		public short GetInt16(int i)
		{
			return (short)_columnValues[i];
		}

		public object this[string name]
		{
			get
			{
				// add one allow for the fact the first column is the WKB geometry
				return _columnValues[this.GetOrdinal(name)+1];
			}
		}

		public object this[int i]
		{
			get
			{
				return _columnValues[i];
			}
		}

		public int FieldCount
		{
			get
			{
				return _dbaseFields.Length;
			}
		}
		#endregion

		#region Implementation of IEnumerable
		public System.Collections.IEnumerator GetEnumerator()
		{
			return new ShapefileDataReaderEnumerator(this);
		}
		#endregion
		
		/// <summary>
		/// Gets the header for the Shapefile.
		/// </summary>
		public ShapefileHeader ShapeHeader
		{
			get
			{
				return _shpHeader;
			}
		}

		/// <summary>
		/// Gets the header for the Dbase file.
		/// </summary>
		public DbaseFileHeader DbaseHeader
		{
			get
			{
				return this._dbfHeader;
			}
		}

		/*
		 * Implementation specific methods.
		 */
		private int CultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}
	}
}

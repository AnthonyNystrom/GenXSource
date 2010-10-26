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
using System.IO;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Class that allows records in a dbase file to be enumerated.
	/// </summary>
	public class DbaseFileReader  : IEnumerable
	{

		private class DbaseFileEnumerator : IEnumerator
		{
			DbaseFileReader _parent;
			ArrayList _arrayList;
			int _iCurrentRecord=0;
			private BinaryReader _dbfStream = null;
			private int _readPosition = 0;
			private DbaseFileHeader _header = null;
			protected string[] _fieldNames = null;

			public DbaseFileEnumerator(DbaseFileReader parent)
			{
				_parent = parent;
				FileStream stream = new FileStream(parent._filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				_dbfStream = new BinaryReader(stream);
				ReadHeader();
			}
			#region Implementation of IEnumerator
			public void Reset()
			{
				throw new InvalidOperationException();
			}

			public bool MoveNext()
			{
				/*bool finished = true;
				if (_iCurrentRecord <_header.NumRecords)
				{
					_arrayList = this.Read();
					if (_arrayList!=null)
					{
						finished=false;
					}
					
				}
				else
				{
					this._dbfStream.Close();
				}
				_iCurrentRecord++;
				bool end = _arrayList == null;
				//return !end;
				return finished;*/
				_iCurrentRecord++;
				if (_iCurrentRecord <=_header.NumRecords)
				{
					_arrayList = this.Read();
				}
				bool more= true;
				if (_iCurrentRecord>_header.NumRecords)
				{
					this._dbfStream.Close();			
					more = false;
				}
				return more;
			}

			public object Current
			{
				get
				{
					return _arrayList;
				}
			}

			protected void ReadHeader()
			{
				_header = new DbaseFileHeader();
				// read the header
				_header.ReadHeader(_dbfStream);

				// how many records remain
				_readPosition = _header.HeaderLength;
			}

			/// <summary>
			/// Read a single dbase record
			/// </summary>
			/// <returns>return the read shapefile record or null if there are no more records</returns>
			private ArrayList Read()  
			{
				ArrayList attrs = null;
        
				bool foundRecord = false;
				while (!foundRecord) 
				{
					// retrieve the record length
					int tempNumFields = _header.NumFields;
            
					// storage for the actual values
					attrs = new ArrayList(tempNumFields);
            
					// read the deleted flag
					char tempDeleted = (char) _dbfStream.ReadChar();
            
					// read the record length
					int tempRecordLength = 1; // for the deleted character just read.
            
					// read the Fields
					for (int j=0; j<tempNumFields; j++)
					{
                
						// find the length of the field.
						int tempFieldLength = _header.Fields[j].Length;
						tempRecordLength = tempRecordLength + tempFieldLength;
                
						// find the field type
						char tempFieldType = _header.Fields[j].DbaseType;
                
						// read the data.
						object tempObject = null;
						switch (tempFieldType)
						{
							case 'L': // logical data type, one character (T,t,F,f,Y,y,N,n)
								char tempChar = (char) _dbfStream.ReadByte();
								if ((tempChar == 'T') || (tempChar == 't') || (tempChar == 'Y') || (tempChar == 'y'))
								{
									tempObject = true;
								}
								else 
								{
									tempObject = false;
								}
								break;
							case 'C': // character record.
								char[] sbuffer = new char[tempFieldLength];
								sbuffer= _dbfStream.ReadChars(tempFieldLength);
								// use an encoding to ensure all 8 bits are loaded
								// tempObject = new string(sbuffer, "ISO-8859-1").Trim();
								

								//HACK: this can be made more efficient
								tempObject = new string(sbuffer).Trim().Replace("\0","");//.ToCharArray();
								
								break;
                        
							case 'D': // date data type.
								char[] ebuffer = new char[8];
								ebuffer  = _dbfStream.ReadChars(8);
								string tempString = new string(ebuffer, 0, 4);
								int year = int.Parse(tempString);
								tempString = new string(ebuffer, 4, 2);
								int month = int.Parse(tempString) - 1;
								tempString = new string(ebuffer, 6, 2);
								int day = int.Parse(tempString);
								tempObject = new DateTime(year, month, day);
								break;
                        
							case 'N': // number
							case 'F': // floating point number
								char[] fbuffer = new char[tempFieldLength];
								fbuffer = _dbfStream.ReadChars(tempFieldLength);
								tempString = new string(fbuffer);
								try 
								{ 
									tempObject = Double.Parse(tempString.Trim());
								}
								catch (FormatException) 
								{
									// if we can't format the number, just save it as
									// a string
									tempObject = tempString;
								}
								break;
                        
							default:
								throw new NotSupportedException("Do not know how to parse Field type "+tempFieldType);
						}
						attrs.Add(tempObject);
					}
            
					// ensure that the full record has been read.
					if (tempRecordLength < _header.RecordLength)
					{
						byte[] tempbuff = new byte[_header.RecordLength-tempRecordLength];
						tempbuff = _dbfStream.ReadBytes(_header.RecordLength-tempRecordLength);
					}
            
					// add the row if it is not deleted.
					if (tempDeleted != '*')
					{
						foundRecord = true;
					}
				}
				return attrs;
			}
			#endregion
		}

		private DbaseFileHeader _header = null;
		private string _filename;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DbaseFileReader class.
		/// </summary>
		public DbaseFileReader(string filename) 
		{
			if (filename==null)
			{
				throw new ArgumentNullException(filename);
			}
			// check for the file existing here, otherwise we will not get an error
			//until we read the first record or read the header.
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(String.Format("Could not find file \"{0}\"",filename));
			}
			_filename = filename;
			
		}
		
		#endregion

		#region Methods
		/// <summary>
		/// Gets the header information for the dbase file.
		/// </summary>
		/// <returns>DbaseFileHeader contain header and field information.</returns>
		public DbaseFileHeader GetHeader() 
		{
			if (_header==null)
			{
				FileStream stream = new FileStream(_filename, System.IO.FileMode.Open);
				BinaryReader dbfStream = new BinaryReader(stream);

				_header = new DbaseFileHeader();
				// read the header
				_header.ReadHeader(dbfStream);

				dbfStream.Close();
				stream.Close();

			}
			return _header;
		}
		
		#endregion

		#region Implementation of IEnumerable
		/// <summary>
		/// Gets the object that allows iterating through the members of the collection.
		/// </summary>
		/// <returns>An object that implements the IEnumerator interface.</returns>
		public System.Collections.IEnumerator GetEnumerator()
		{
			return new DbaseFileEnumerator(this);
		}
		#endregion

		
	}
}

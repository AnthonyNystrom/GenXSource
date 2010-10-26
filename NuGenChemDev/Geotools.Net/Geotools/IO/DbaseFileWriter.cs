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
	/// This class aids in the writing of Dbase IV files. 
	/// </summary>
	/// <remarks>
	/// Attribute information of an ESRI Shapefile is written using Dbase IV files.
	/// </remarks>
	public class DbaseFileWriter
	{
		string _filename;
		BinaryWriter _writer;
		bool headerWritten = false;
		bool recordsWritten = false;
		DbaseFileHeader _header;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DbaseFileWriter class.
		/// </summary>
		public DbaseFileWriter(string filename)
		{
			if (filename==null)
			{
				throw new ArgumentNullException("filename");
			}
			_filename = filename;
			FileStream filestream  = new FileStream(filename, FileMode.Create,FileAccess.Write,FileShare.Write);
			_writer = new BinaryWriter(filestream);
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public void Write(DbaseFileHeader header)
		{
			if (header==null)
			{
				throw new ArgumentNullException("header");
			}
			if (recordsWritten)
			{
				throw new InvalidOperationException("Records have already been written. Header file needs to be written first.");
			}
			headerWritten = true;
			header.WriteHeader(_writer);
			_header = header;
			
		}
		public void Write(ArrayList columnValues)
		{
			if (columnValues==null)
			{
				throw new ArgumentNullException("columnValues");
			}
			if (headerWritten==false)
			{
				throw new InvalidOperationException("Header records need to be written first.");
			}
			int i=0;
			_writer.Write((byte)0x20); // the deleted flag
			foreach(object columnValue in columnValues)
			{
				
				if (columnValue is double)
				{
					Write((double)columnValue, _header.Fields[i].Length, _header.Fields[i].DecimalCount);
				}
				else if (columnValue is float)
				{
					Write((float)columnValue,  _header.Fields[i].Length, _header.Fields[i].DecimalCount);
				}
				else if (columnValue is bool)
				{
					Write((bool)columnValue);
				}
				else if (columnValue is string)
				{
					int length = _header.Fields[i].Length;
					Write((string)columnValue, length);
				}
				else if (columnValue is DateTime)
				{
					Write((DateTime)columnValue);
				}
				i++;
			}
		}
		public void Write(double number, int length, int decimalCount)
		{
			// write with 19 chars.
			string format="{0:";
			for(int i=0; i<decimalCount;i++)
			{
				if (i==0)
				{
					format=format+"0.";
				}
				format=format+"0";
			}
			format=format+"}";
			string str = String.Format(format,number);
			for (int i=0; i< length-str.Length; i++)
			{
				_writer.Write((byte)0x20);
			}
			//_writer.Write(str);
			//_writer.Write((byte)0x20);
			foreach(char c in str)
			{
				_writer.Write(c);
			}
			
		}
		public void Write(float number, int length, int decimalCount)
		{
			_writer.Write(String.Format("{0:000000000.000000000}",number));
		}
		public void Write(string text, int length)
		{
			// ensure string is not too big
			text = text.PadRight(length,' ');
			string dbaseString = text.Substring(0,length);

			// will extra chars get written??
			//_writer.Write(dbaseString);
			foreach(char c in dbaseString)
			{
				_writer.Write(c);
			}

			int extraPadding = length - dbaseString.Length;
			for(int i=0; i < extraPadding; i++)
			{
				_writer.Write((byte)0x20);
			}
		}
		public void Write(DateTime date)
		{
			_writer.Write(date.Year-1900);
			_writer.Write(date.Month);
			_writer.Write(date.Day);
		}
		public void Write(bool flag)
		{
			if (flag)
			{
				_writer.Write("T");
			}
			else 
			{
				_writer.Write("F");
			}
		}

		public void Close()
		{
			_writer.Close();
		}

		 
		#endregion

	}
}

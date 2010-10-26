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
using System.IO;
using System.Text;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Extends the BinaryWriter class to allow the writing of integers in the Big Endian format.
	/// </summary>
	public class BigEndianBinaryWriter : BinaryWriter
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the BigEndianBinaryWriter class.
		/// </summary>
		public BigEndianBinaryWriter() : base()
		{
			
		}
		/// <summary>
		/// Initializes a new instance of the BigEndianBinaryWriter class based on the supplied stream and using UTF-8 as the encoding for strings.
		/// </summary>
		/// <param name="output">The supplied stream.</param>
		public BigEndianBinaryWriter(Stream output) : base(output)
		{
		}
		/// <summary>
		/// Initializes a new instance of the BigEndianBinaryWriter class based on the supplied stream and a specific character encoding.
		/// </summary>
		/// <param name="output">The supplied stream.</param>
		/// <param name="encoding">The character encoding.</param>
		public BigEndianBinaryWriter(Stream output, Encoding encoding): base(output,encoding)
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Reads a 2-byte signed integer using the big-endian layout from the current stream and advances the current position of the stream by two bytes.
		/// </summary>
		/// <param name="integer">The four-byte signed integer to write.</param>
		public void WriteIntBE(int integer)
		{
			byte i1=(byte)(integer >> 24);
			byte i2=(byte)(integer >> 16);
			byte i3=(byte)(integer >> 8);
			byte i4=(byte)(integer & 255);
			
			this.Write(i1);
			this.Write(i2);
			this.Write(i3);
			this.Write(i4);		
		}
		#endregion

	}
}

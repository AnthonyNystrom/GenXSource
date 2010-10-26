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
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Class for holding the information assicated with a dbase field.
	/// </summary>
	public class DbaseFieldDescriptor
	{
		// Field Name
		private string _name;
        
		// Field Type (C N L D or M)
		private char _type;
        
		// Field Data Address offset from the start of the record.
		private int _dataAddress;
        
		// Length of the data in bytes
		private int _length;
        
		// Field decimal count in Binary, indicating where the decimal is
		private int _decimalCount;

		#region Static methods
		public static char GetDbaseType(Type type)
		{
			DbaseFieldDescriptor dbaseColumn = new DbaseFieldDescriptor();
			if (type==typeof(string))
			{
				return 'C';
			}
			else if (type==typeof(double))
			{
			}
			else if (type==typeof(float))
			{
			}
			else if (type==typeof(bool))
			{
				return 'L';
			}
			else if (type==typeof(DateTime))
			{
				return 'D';
			}
			throw new NotSupportedException(String.Format("{0} does not have a corresponding dbase type.",type.Name));
		}
		public static DbaseFieldDescriptor ShapeField()
		{
			DbaseFieldDescriptor shpfield = new DbaseFieldDescriptor();
			shpfield.Name="Geometry";
			shpfield._type='B';
			return shpfield;
		}

		public static DbaseFieldDescriptor IdField()
		{
			DbaseFieldDescriptor shpfield = new DbaseFieldDescriptor();
			shpfield.Name="Row";
			shpfield._type='I';
			return shpfield;
		}

		#endregion

		#region Properties
		// Field Name
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
        
		// Field Type (C N L D or M)
		public char DbaseType
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
        
		// Field Data Address offset from the start of the record.
		public int DataAddress
		{
			get
			{
				return _dataAddress;
			}
			set
			{
				_dataAddress = value;
			}
		}
        
		// Length of the data in bytes
		public int Length
		{
			get
			{
				return _length;
			}
			set
			{
				_length = value;
			}
		}
        
		// Field decimal count in Binary, indicating where the decimal is
		public int DecimalCount
		{
			get
			{
				return _decimalCount;
			}
			set
			{
				_decimalCount = value;
			}
		}

		/// <summary>
		/// Returns the equivalent CLR type for this field.
		/// </summary>
		public Type Type
		{
			get
			{
				Type type;
				switch ( _type )
				{
					case 'L': // logical data type, one character (T,t,F,f,Y,y,N,n)
						type = typeof(bool);
						break;
					case 'C': // char or string
						type = typeof(string);
						break;
					case 'D': // date
						type = typeof(DateTime);
						break;
					case 'N': // numeric
						type = typeof(double);
						break;
					case 'F': // double
						type = typeof(float);
						break;
					case 'B': // BLOB - not a dbase but this will hold the WKB for a geometry object.
						type = typeof(byte[]);
							break;
					default:
						throw new NotSupportedException("Do not know how to parse Field type "+_type);
				}
				return type;
			}	
		}
		#endregion
	}
}

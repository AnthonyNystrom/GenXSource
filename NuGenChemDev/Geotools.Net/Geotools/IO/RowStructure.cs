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
using System.ComponentModel;

#endregion

namespace Geotools.IO
{
	/// <summary>
	/// Implements ICustomTypeDescriptor so we can simulate a row object having a property for every field.
	/// </summary>
	/// <remarks>
	/// For an explaination of ICustomTypeDescriptor see http://www.devx.com/dotnet/Article/7874
	/// By implementing this interface, we are able to simulate that an object has lots of properties.
	/// These properties are determined dynamically at run-time. When enumerating throught the 
	/// ShapefileDataReader, RowStructure is the object that gets returned. 
	/// <code>
	/// foreach(object obj in shpDataReader)
	/// {
	///		if (obj.GetType().Name!="RowStructure")
	///		{
	///			// this proves the type returned by shpDataReader
	///		} 
	/// }
	/// </code>
	/// </remarks>
	internal struct RowStructure : ICustomTypeDescriptor 
	{
		DbaseFieldDescriptor[] _dbaseFields;
		private ArrayList _columnValues;
		
		public RowStructure(DbaseFieldDescriptor[] dbaseFields, ArrayList columnValues) 
		{
			_dbaseFields = dbaseFields;
			_columnValues  = columnValues;
		}

		public  ArrayList ColumnValues
		{
			get
			{
				return _columnValues;
			}
		}

		#region ICustomTypeDescriptor implementation
		public AttributeCollection GetAttributes() 
		{
			return AttributeCollection.Empty;
		}

		public string GetClassName() 
		{
			return null;
		}

		public string GetComponentName() 
		{
			return null;
		}

		public TypeConverter GetConverter() 
		{
			return null;
		}
		
		public object GetEditor(Type t) 
		{
			return null;
		}

		public EventDescriptor GetDefaultEvent() 
		{
			return null;
		}

		public EventDescriptorCollection GetEvents(Attribute[] a) 
		{
			return GetEvents();
		}

		public EventDescriptorCollection GetEvents() 
		{
			return EventDescriptorCollection.Empty;
		}

		public object GetPropertyOwner(PropertyDescriptor pd) 
		{
			return null;
		}

		public PropertyDescriptor GetDefaultProperty() 
		{
			return null;
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] a) 
		{
			return GetProperties();
		}

		public PropertyDescriptorCollection GetProperties() 
		{
			// add an extra field at the beginning - this will hold the WKT for the Geometry object.
			PropertyDescriptor[] pd = new PropertyDescriptor[_dbaseFields.Length];

			// the regular fields
			for (int i = 0; i < _dbaseFields.Length; i++)
			{
				pd[i] = new ColumnStructure( _dbaseFields[i], i );
			}
			return new PropertyDescriptorCollection(pd);
		}
		#endregion

	}
}

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
using System.ComponentModel;
#endregion

namespace Geotools.IO
{
	/// <summary>
	/// This class is used in conjunction with RowStructure. 
	/// </summary>
	/// <remarks>
	/// For an explaination of PropertyDescriptor see http://www.devx.com/dotnet/Article/7874
	/// and the remarks for RowStructure. This class inherits from PropertyDescriptor. 
	/// The PropertyDescriptor describes a property - in this case a dynamically generated property.
	/// </remarks>
	internal class ColumnStructure : PropertyDescriptor
	{
		private DbaseFieldDescriptor _dbaseField;
		private int _index;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ColumnStructure class.
		/// </summary>
		public ColumnStructure(DbaseFieldDescriptor dbaseField, int index):base(dbaseField.Name, null)
		{
			_dbaseField = dbaseField;
			_index=index;	
		}
		#endregion

		#region Properties
		public override System.Type ComponentType
		{
			get
			{
				return typeof(RowStructure);
			}
		}

		public override System.Type PropertyType
		{
			get
			{
				// return the type of the dbase field.
				return _dbaseField.Type;
			}
		}

		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}
		#endregion

		#region Methods
		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override void ResetValue(object component)
		{
			
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}

		public override void SetValue(object component, object value)
		{
		
		}

		public override object GetValue(object component)
		{
			// gets the 'parent' and gets a value of out of the ColumnValues property.
			return ((RowStructure)component).ColumnValues[_index];
		}


		// awc: Added this propety, because when creating a DataSet from the DataReader, we need
		// to know how long the field length is in the dbase file so we can create a column
		// of the appropriate length in the database.
		public int Length
		{
			get
			{
				return _dbaseField.Length;
			}
		}
		#endregion
	}
}

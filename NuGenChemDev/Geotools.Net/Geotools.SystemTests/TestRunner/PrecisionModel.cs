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
#region using Statements

using System;

#endregion

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// A class to simulate the precisionModel element in the XML test data file.
	/// An PrecisionModel has a type, a scale, an offsetX, and an offsetY.
	/// All the members are implemented as strings.
	/// This object is simply a container to hold the attributes of the 
	/// precisionModel element.
	/// </summary>
	public class PrecisionModel
	{

		#region Members

		private string _type = string.Empty;
		private string _scale = string.Empty;
		private string _offSetX = string.Empty;
		private string _offSetY = string.Empty;

		#endregion Members

		#region Constructors

		public PrecisionModel()
		{
		}

		#endregion Constructors

		#region Properties
		
		public string Type
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

		public string Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				_scale = value;
			}
		}

		public string OffSetX
		{
			get
			{
				return _offSetX;
			}
			set
			{
				_offSetX = value;
			}
		}

		public string OffSetY
		{
			get
			{
				return _offSetY;
			}
			set
			{
				_offSetY = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Override ToString function that returns a string
		/// representation of the PrecisionModel object.
		/// </summary>
		/// <returns>A string representation of the PrecisionModel object</returns>
		public override string ToString()
		{
			string temp = "Type:  " + this._type + "\n";
			if(_type != "FLOATING")
			{
				temp += "Scale:  " + this._scale + "\n";
				temp += "OffsetX:  " + this._offSetX + "\n";
				temp += "OffsetY:  " + this._offSetY + "\n";
			}

			return temp;
		}

		#endregion Methods
	}
}

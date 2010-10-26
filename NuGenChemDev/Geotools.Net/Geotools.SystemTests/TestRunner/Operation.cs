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

using System;

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// A class to simulate the op element in the XML test data file.
	/// An Operation has a name, an arg1, an arg2, an arg3, and an 
	/// expected result. All the members are implemented as strings.
	/// This object is simply a container to hold the attributes of the 
	/// op element.
	/// </summary>
	public class Operation
	{

		#region Members

		private string _name = string.Empty;
		private string _arg1 = string.Empty;
		private string _arg2 = string.Empty;
		private string _arg3 = string.Empty;
		private string _expectedResult = string.Empty;

		#endregion Members

		#region Constructors

		public Operation()
		{
		}

		#endregion Constructors

		#region Properties

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

		public string Arg1
		{
			get
			{
				return _arg1;
			}
			set
			{
				_arg1 = value;
			}
		}

		public string Arg2
		{
			get
			{
				return _arg2;
			}
			set
			{
				_arg2 = value;
			}
		}

		public string Arg3
		{
			get
			{
				return _arg3;
			}
			set
			{
				_arg3 = value;
			}
		}

		public string ExpectedResult
		{
			get
			{
				return _expectedResult;
			}
			set
			{
				_expectedResult = value;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Overrided ToString function.
		/// </summary>
		/// <returns>
		/// A string representation of this object which is the a string containing
		/// the contents of its members.
		/// </returns>
		public override string ToString()
		{
			string temp = "";
			temp += "Operation Name: " + _name + "\n";
			temp += "Arg1: " + _arg1 + "\n";
			temp += "Arg2: " + _arg2 + "\n";
			temp += "Arg3: " + _arg3 + "\n";
			temp += "Expected Result: " + _expectedResult + "\n";

			return temp;
		}

		#endregion Methods
	}
}

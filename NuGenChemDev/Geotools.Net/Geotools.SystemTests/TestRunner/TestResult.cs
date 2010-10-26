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
using Geotools.Geometries;

#endregion

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// Summary description for TestResult.
	/// </summary>
	public class TestResult
	{

		#region Members

		private string	_testCaseDescription = string.Empty;
		private string	_operation = string.Empty;
		private bool	_passFailWKT = false;
		private bool	_passFailTopographyEquals = false;
		private string	_aGeometryType = string.Empty;
		private string	_bGeometryType = string.Empty;
		private Geometry _resultGeometry = null;
		private string	_resultGeometryType = string.Empty;
		private string	_resultGeometryWKT = string.Empty;
		private bool	_predicateResult = false;
		private string	_predicateResultString = string.Empty;
		private bool	_exceptionThrown = false;
		private string	_exceptionMessage = string.Empty;
		private string	_expectedWKT = string.Empty;
		private string	_aGeometryWKT = string.Empty;
		private string	_bGeometryWKT = string.Empty;
		private string	_propertyTestResult = string.Empty;
		private float	_testDuration = 0.0F;

		#endregion Members

		#region Constructors

		public TestResult()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#endregion Constructors

		#region Properties

		public float TestDuration
		{
			get
			{
				return _testDuration;
			}
			set
			{
				_testDuration = value;
			}
		}

		public string PropertyTestResult
		{
			get
			{
				return _propertyTestResult;
			}
			set
			{
				_propertyTestResult = value;
			}
		}

		public string AGeometryWKT
		{
			get
			{
				return _aGeometryWKT;
			}
			set
			{
				_aGeometryWKT = value;
			}
		}

		public string BGeometryWKT
		{
			get
			{
				return _bGeometryWKT;
			}
			set
			{
				_bGeometryWKT = value;
			}
		}

		public string TestCaseDescription
		{
			get
			{
				return _testCaseDescription;
			}
			set
			{
				_testCaseDescription = value;
			}
		}

		public string ExpectedWKT
		{
			get
			{
				return _expectedWKT;
			}
			set
			{
				_expectedWKT = value;
			}
		}

		public string AGeometryType
		{
			get
			{
				return _aGeometryType;
			}
			set
			{
				_aGeometryType = value;
			}
		}

		public string BGeometryType
		{
			get
			{
				return _bGeometryType;
			}
			set
			{
				_bGeometryType = value;
			}
		}

		public string PredicateResultString
		{
			get
			{
				return _predicateResultString;
			}
		}

		public bool PredicateResult
		{
			get
			{
				return _predicateResult;
			}
			set
			{
				_predicateResult = value;
			}
		}

		public bool ExceptionThrown
		{
			get
			{
				return _exceptionThrown;
			}
			set
			{
				_exceptionThrown = value;
			}
		}

		public string ExceptionMessage
		{
			get
			{
				return _exceptionMessage;
			}
			set
			{
				_exceptionMessage = value;
			}
		}

		public string ResultGeometryType
		{
			get
			{
				return _resultGeometryType;
			}
			set
			{
				_resultGeometryType = value;
			}
		}

		public string Operation
		{
			get
			{
				return _operation;
			}
			set
			{
				_operation = value;
			}
		}

		public Geometry ResultGeometry
		{
			get
			{
				return _resultGeometry;
			}
			set
			{
				_resultGeometry = value;
			}
		}

		public bool PassFailWKT
		{
			get
			{
				return _passFailWKT;
			}
			set
			{
				_passFailWKT = value;
			}
		}

		public bool PassFailTopographyEquals
		{
			get
			{
				return _passFailTopographyEquals;
			}
			set
			{
				_passFailTopographyEquals = value;
			}
		}

		public string ResultGeometryWKT
		{
			get
			{
				return _resultGeometryWKT;
			}
			set
			{
				_resultGeometryWKT = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the _predicateResultString to the string representation of the boolean
		/// _predicateResultString. Then sets it to all lowercase to match the way it 
		/// is in the XML test files.
		/// </summary>
		public void SetPredicateResultString()
		{
			// Get the string version of the boolean variable...
			this._predicateResultString = _predicateResult.ToString();
			// Set it to all lowercase...
			this._predicateResultString = this._predicateResultString.ToLower();
		}

		/// <summary>
		/// Overridden ToString. Returns a string representation of the object in a formatted
		/// style.
		/// </summary>
		/// <returns>A string containing all the information for the object.</returns>
		public override string ToString()
		{
			string returnString = "Test results for " + _testCaseDescription + "\n";
			returnString += "\tOperation: " + _operation + "\n";
			returnString += "\tGeometry A: " + _aGeometryType + "\n";
			returnString += "\tGeometry B: " + _bGeometryType + "\n";
			returnString += "\tReturned Geometry: " + _resultGeometry.GetType() + "\n";

			string tempExpectedWKT = Test.FormatWKTString(_expectedWKT);
			returnString += "\n\tExpected WKT: " + tempExpectedWKT + "\n";
			string tempResultWKT = Test.FormatWKTString(_resultGeometryWKT);
			returnString += "\tResult WKT:   " + tempResultWKT + "\n";
			returnString += "\tWKT Pass/Fail: " + _passFailWKT.ToString()+ "\n";
			returnString += "\tEqualsTopology Pass/Fail: " + _passFailTopographyEquals.ToString() + "\n";
			returnString += "\tPredicate Result: " + _predicateResultString + "\n";
			return returnString;
		}
		#endregion
	}
}

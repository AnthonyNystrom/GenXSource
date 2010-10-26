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
using System.Collections;

#endregion

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// This class represents the root node of the XML test data file.
	/// There is one run element in each test data file. A Run element
	/// contains a PrecisionModel and a number of TestCases.
	/// </summary>
	public class Run : IEnumerable
	{

		#region Members

		// the name and path of the XML test file...
		private string _filename = string.Empty;
		// the description of the run
		private string _description = string.Empty;
		// the Precision Model for the tests in this run
		private PrecisionModel _precisionModel = null;
		// An ArrayList to hold the TestCase objects in the Run
		private ArrayList _testCases = null;

		#endregion Members

		#region Constructors

		public Run()
		{
			// get an ArrayList object to hold the test cases...
			_testCases = new ArrayList();
		}

		#endregion Constructors

		#region Properties

		public string Filename
		{
			get
			{
				return _filename;
			}
			set
			{
				_filename = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public long TestCaseCount
		{
			get
			{
				return _testCases.Count;
			}
		}

		public PrecisionModel PrecisionModel
		{
			get
			{
				return _precisionModel;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Override ToString function that returns a string
		/// representation of the Run object.
		/// </summary>
		/// <returns>A string representation of the Run object</returns>
		public override string ToString()
		{
			// get a temp string variable to hold the Run data...
			string temp = "This XML file contains:\n";
			// add the description...
			temp += "Description: \n  " + _description + "\n";
			// add the Precision Model...
			temp += "Precision Model: \n  ";
			temp += this._precisionModel.ToString() + "\n";

			// return the string representation...
			return temp;
		}

		/// <summary>
		/// A procedure to set the members of the precision model for this Run object.
		/// Note: this is not an UrbanScience.Geographic.Geometries.PrecisionModel.
		/// </summary>
		/// <param name="type">String containing the type of precision model. Either "FIXED"
		/// or "FLOATING"</param>
		/// <param name="scale">String containing the scale for the fixed precision model.</param>
		/// <param name="offSetX">String containing the offset for the x coordinate.</param>
		/// <param name="offSetY">String containing the offset for the y coordinate.</param>
		public void SetPrecisionModel(string type, string scale, string offSetX, string offSetY)
		{
			// Simply get a new PrecisionModel object and assign the values...
			_precisionModel = new PrecisionModel();
			_precisionModel.Type = type;
			_precisionModel.Scale = scale;
			_precisionModel.OffSetX = offSetX;
			_precisionModel.OffSetY = offSetY;
		}


		/// <summary>
		/// Procedure to add a TestCase object to the ArrayList.
		/// </summary>
		/// <param name="tc">The TestCase object to add.</param>
		public void AddTestCase(TestCase tc)
		{
			// Simply call the Add method on the ArrayList object and pass
			// the TestCase object to it...
			_testCases.Add(tc);
		}


		#endregion Methods

		#region IEnumerable Implementation

		/// <summary>
		/// A GetEnumerator implementation to allow for iterating through the TestCase objects.
		/// </summary>
		/// <returns>IEnumberator object.</returns>
		public IEnumerator GetEnumerator()
		{
			// return the enumerator in the ArrayList object...
			return _testCases.GetEnumerator();
		}

		#endregion IEnumerable Implementation
	}
}

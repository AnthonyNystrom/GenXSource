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
using Geotools.Geometries;

#endregion

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// A class to simulate the case element in the XML test data files.
	/// A TestCase has a description, an "A" geometry, a "B" geometry, and
	/// a number of tests. Note: the "B" geometry may be null due to some functions
	/// are only performed on the "A" geometry.
	/// </summary>
	public class TestCase : IEnumerable
	{
		#region Members

		private string _description = string.Empty;
		private string _aGeometry = string.Empty;
		private string _bGeometry = string.Empty;
		private ArrayList _tests = null;

		#endregion

		#region Constructors

		public TestCase()
		{
			// get a new ArrayList object to hold the tests in this TestCase...
			_tests = new ArrayList();
		}

		#endregion

		#region Properties

		public string A
		{
			get
			{
				return _aGeometry;
			}
			set
			{
				_aGeometry = value;
			}
		}

		public string B
		{
			get
			{
				return _bGeometry;
			}
			set
			{
				_bGeometry = value;
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

		public long TestCount
		{
			get
			{
				return _tests.Count;
			}
		}

		#endregion Properties

		#region IEnumerable Implementation

		/// <summary>
		/// A GetEnumerator implementation to allow for iterating through the Test objects.
		/// </summary>
		/// <returns>IEnumberator object.</returns>
		public IEnumerator GetEnumerator()
		{
			// return the enumerator in the ArrayList object...
			return _tests.GetEnumerator();
		}

		#endregion

		#region Methods

		/// <summary>
		/// A procedure to add a new Test object to this TestCase. The new Test
		/// object is created within this procedure and added to the ArrayList
		/// object that holds the Tests.
		/// </summary>
		/// <param name="opName">A string containing the name of the operation to be 
		/// performed in the test.</param>
		/// <param name="arg1">A string containing the first argument for the operation.
		/// This will be either the letter "a" for the A geometry or the letter "b" for
		/// the "B" geometry.</param>
		/// <param name="arg2">A string containing the second argument for the operation.
		/// This will be either the letter "a" for the A geometry or the letter "b" for
		/// the "B" geometry.</param>
		/// <param name="arg3">A string containing the expected relate pattern for the operation.
		/// The expected relate pattern is a representation of the Dimensionally Extended - 9
		/// Intersection Model (DE-9IM) and is of the form 1020F1102.</param>
		/// <param name="expectedResult"></param>
		public void AddTest(string opName, string arg1, string arg2, string arg3, string aGeometryWKT, string bGeometryWKT, string expectedResult)
		{
			// Simply call the Add method on the ArrayList object and create the new
			// Test object in the parameter list using the procedure arguments...
			_tests.Add(new Test(opName, arg1, arg2, arg3, aGeometryWKT, bGeometryWKT, expectedResult));

		}

		/// <summary>
		/// Override ToString function that returns a string
		/// representation of the TestCase object.
		/// </summary>
		/// <returns>A string representation of the TestCase object</returns>
		public override string ToString()
		{
			string temp = "";
			temp += "Test Case Description: " + _description + "\n";
			temp += "A Geometry: " + Test.FormatWKTString(_aGeometry) + "\n";
			temp += "B Geometry: " + Test.FormatWKTString(_bGeometry) + "\n";
			return temp;
		}

		/// <summary>
		/// Creates the "A" geometry for this TestCase by instantiating a GeometryFactory object and 
		/// calling its CreateFromWKT method. The geometry object is created from its Well-known text.
		/// </summary>
		/// <param name="precisionModel">The precision model for this run.</param>
		/// <returns>OGC.SimpleFeatures.IGeometry object.</returns>
		public IGeometry CreateAGeometry(Geotools.Geometries.PrecisionModel precisionModel)
		{
			// create the GeometryFactory object...
			Geotools.Geometries.GeometryFactory geometryFactory = new Geotools.Geometries.GeometryFactory(precisionModel, -1);
			// create the geometry and return it...
			return geometryFactory.CreateFromWKT(this._aGeometry);
		}

		/// <summary>
		/// Creates the "B" geometry for this TestCase by instantiating a GeometryFactory object and 
		/// calling its CreateFromWKT method. The geometry object is created from its Well-known text.
		/// </summary>
		/// <param name="precisionModel">The precision model for this run.</param>
		/// <returns>OGC.SimpleFeatures.IGeometry object.</returns>
		
		public IGeometry CreateBGeometry(Geotools.Geometries.PrecisionModel precisionModel)
		{
			// create the GeometryFactory object...
			Geotools.Geometries.GeometryFactory geometryFactory = new Geotools.Geometries.GeometryFactory(precisionModel, -1);
			// create the geometry and return it...
			return geometryFactory.CreateFromWKT(this._bGeometry);
		}

		#endregion Methods
	}
}

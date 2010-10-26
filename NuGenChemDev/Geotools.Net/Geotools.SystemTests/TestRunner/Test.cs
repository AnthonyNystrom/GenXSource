#region using Statements

using System;
using Geotools.Geometries;

#endregion using Statements

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// 
	/// </summary>
	public class Test
	{
		#region Members

		private Operation _op = null;
		private TestResult _testResult = null;
		private PerformanceTimer _timer = null;
		private bool _passed = false;

		#endregion Members

		#region Constructors

		public Test(string opName, string arg1, string arg2, string arg3, string aGeometryWKT, string bGeometryWKT, string expectedResult)
		{
			_op = new Operation();
			_op.Name = opName;
			_op.Arg1 = arg1;
			_op.Arg2 = arg2;
			_op.Arg3 = arg3;
			_op.ExpectedResult = expectedResult;
			_testResult = new TestResult();
			_testResult.ExpectedWKT = this.Operation.ExpectedResult;
			_testResult.AGeometryWKT = aGeometryWKT;
			_testResult.BGeometryWKT = bGeometryWKT;
			_timer = new PerformanceTimer();
		}

		public Test(string opName, string arg1, string arg2, string arg3, string expectedResult)
		{
			_op = new Operation();
			_op.Name = opName;
			_op.Arg1 = arg1;
			_op.Arg2 = arg2;
			_op.Arg3 = arg3;
			_op.ExpectedResult = expectedResult;
			_testResult = new TestResult();
			_testResult.ExpectedWKT = this.Operation.ExpectedResult;
			_timer = new PerformanceTimer();
		}

		#endregion Constructors

		#region Properties

		public bool Passed
		{
			get
			{
				return _passed;
			}
		}

			public Operation Operation
		{
			get
			{
				return _op;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Override ToString() method that returns a string representation of this object.
		/// All the information concerning the test is in the Operation object.
		/// </summary>
		/// <returns>The Operation object ToString() method.</returns>
		public override string ToString()
		{
			// first, format the WKT string in the operation object...
			Test.FormatWKTString(this._op.ExpectedResult);
			// return the ToString on the Operation object...
			return this._op.ToString();
		}

		/// <summary>
		/// Runs a test. Each test is executed in a try/catch block to catch any exception that might be thrown.
		/// </summary>
		/// <param name="aGeometry">The A geometry object</param>
		/// <param name="bGeometry">The B geometry object</param>
		/// <param name="detailedExceptionMessages">boolean to see if detailed exception messages are desired.</param>
		/// <returns>A TestResult object containing the results of the test.</returns>
		public TestResult Run(Geometry aGeometry, Geometry bGeometry, bool detailedExceptionMessages)
		{
			// Grab the types of the a and b geometries and store them
			// in the TestResult object...
			this._testResult.AGeometryType = aGeometry.GetGeometryType();
			// make sure the bGeometry is not null...
			if(bGeometry != null)
			{
				this._testResult.BGeometryType = bGeometry.GetGeometryType();
			}

			// Grab the name of the test to run...
			string testToRun = this.Operation.Name;

			// set up arg1...
			string arg1 = "A";
			if(this._op.Arg1 == "B")
			{
				arg1 = "B";
			}

			// make it lowercase...
			testToRun = testToRun.ToLower();

			// run the tests in a try/catch block to trap any exceptions that may
			// be thrown...
			try
			{
				// determine which test to run and then call the appropriate function...
				switch(testToRun)
				{
					case "getboundary":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "convexhull":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "intersection":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "union":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "difference":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "symdifference":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "buffer":
						RunGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "dimension":
						RunGeometryPropertyTest(aGeometry, bGeometry, arg1, testToRun);
						break;
					case "numpoints":
						RunGeometryPropertyTest(aGeometry, bGeometry, arg1, testToRun);
						break;
					case "overlaps":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "contains":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "within":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "crosses":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "intersects":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "touches":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "disjoint":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "relate":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "equals":
						RunBooleanReturnGeometryFunction(aGeometry, bGeometry, testToRun);
						break;
					case "isvalid":
						RunBooleanReturnPropertyFunction(aGeometry, bGeometry, arg1, testToRun);
						break;
					case "isempty":
						RunBooleanReturnPropertyFunction(aGeometry, bGeometry, arg1, testToRun);
						break;
					case "issimple":
						RunBooleanReturnPropertyFunction(aGeometry, bGeometry, arg1, testToRun);
						break;
					default:
						break;
				}
			}
			catch(System.Exception e)
			{
				// load the details of the exception in the TestResult...
				SetExceptionData(e, detailedExceptionMessages);
			}

			if(this._testResult.PassFailWKT == true)
			{
				_passed = true;
			}
			else
			{
				_passed = false;
			}

			return this._testResult;
		}

		/// <summary>
		/// Calls the methods (property get) on the geometries that are properties of the geometry.
		/// </summary>
		/// <param name="a">The A geometry object.</param>
		/// <param name="arg">A string containing the name of the geometry. "A" or "B"</param>
		/// <param name="testToRun">The property to call.</param>
		private void RunGeometryPropertyTest(Geometry a, Geometry b, string arg, string testToRun)
		{
			Geotools.Geometries.Geometry c = a;
			if (arg == "B")
			{
				arg = "B";
				c = b;
			}

			switch(testToRun)
			{
				case "numpoints":
					int numberOfPoints = 0;
					// set the name of the operation for viewing in the textbox...
					this._testResult.Operation = arg + ".NumPoints";
					// call the property function...
					numberOfPoints = c.GetNumPoints();
					// store the string version of the value returned for comparison later...
					this._testResult.PropertyTestResult = numberOfPoints.ToString();
					// test for pass or failure...
					TestForPassFailPropertyTestResult();
					break;
				case "dimension":
					int dimension = 0;
					// set the name of the operation for viewing in the textbox...
					this._testResult.Operation = arg + ".Dimension";
					dimension = c.GetDimension();
					// store the string version of the value returned for comparison later...
					this._testResult.PropertyTestResult = dimension.ToString();
					// test for pass or failure...
					TestForPassFailPropertyTestResult();
					break;
			}
		}

		/// <summary>
		/// Private method that compares the results and expected values of a TestResult and 
		/// sets the pass or failure variable depending on the comparison.
		/// This method is used to compare the contents of the OtherResult variables in the Test
		/// Result object.
		/// </summary>
		private void TestForPassFailPropertyTestResult()
		{
			// compare the strings and set pass or fail...
			if(this._op.ExpectedResult == this._testResult.PropertyTestResult)
			{
				this._testResult.PassFailWKT = true;
			}
		}

		/// <summary>
		/// Private method that checks whether the getBoundary test passes or fails by comparing the WKT that was returned
		/// and the expected WKT in the object. Also sets the PassFail member of the TestResult object.
		/// The reason there is a special function for the GetBoundary method is that if the return
		/// geometry is an empty GeometryCollection, this procedure appends the word "EMPTY" to the result
		/// WKT string so that it will match the expected value.
		/// </summary>
		private void TestForPassFailGetBoundary()
		{
			// check to see if the ResultGeometry is empty...
			if(this._testResult.ResultGeometry.IsEmpty())
			{
				// if it is, grab the geometry type...
				this._testResult.ResultGeometryType = this._testResult.ResultGeometry.GetGeometryType();
				// make it all uppercase...
				this._testResult.ResultGeometryType = this._testResult.ResultGeometryType.ToUpper();
				// append the word "EMPTY" to the WKT...
				this._testResult.ResultGeometryWKT = this._testResult.ResultGeometryType + " EMPTY";
				// compare the WKT strings...
				if(CompareWKTStrings(this._op.ExpectedResult, this._testResult.ResultGeometryWKT))
				{
					this._testResult.PassFailWKT = true;
				}
			}
			// if it's not empty, just grab the WKT and compare...
			else
			{
				this._testResult.ResultGeometryWKT = this._testResult.ResultGeometry.ToText();
				if(CompareWKTStrings(this._op.ExpectedResult, this._testResult.ResultGeometryWKT))
				{
					this._testResult.PassFailWKT = true;
				}
			}
		}

		/// <summary>
		/// Methods that tests for pass or failure of a test. This is used when
		/// a geometry method returns a boolean value from the method called.
		/// </summary>
		private void TestForPassFailBooleanResult()
		{
			// simply compare the expected and actual returned values. If they
			// are equal, set the TestResult to passed.
			if(this._op.ExpectedResult == this._testResult.PredicateResultString)
			{
				this._testResult.PassFailWKT = true;
			}
		}

		/// <summary>
		/// Private method that checks whether a test passes or fails by comparing the WKT that was returned
		/// and the expected WKT in the object. Also sets the PassFail member of the TestResult object.
		/// </summary>
		private void TestForPassFail()
		{
			Geometry result = this._testResult.ResultGeometry;

			// use the PrecisionModel of the returned geometry when creating the expected geometry...
			Geotools.Geometries.PrecisionModel pm = result.GetPrecisionModel();
			// Create a geometry from the expectedWKT...
			GeometryFactory gf = new GeometryFactory(pm, 0);

			// create the expected geometry, normalize it, and grab the wkt for it...
			Geometry expected = (Geometry)gf.CreateFromWKT(this._testResult.ExpectedWKT);
			expected.Normalize();
			string expectedNormalized = expected.ToText();

			string actualNormalized = "";
			if(this._testResult.ResultGeometry.IsEmpty())
			{
				string type = this._testResult.ResultGeometry.GetGeometryType();
				type = type.ToUpper();
				actualNormalized = type + " EMPTY";
			}
			else
			{
				// normalize the returned geometry and grab the wkt from it...
				this._testResult.ResultGeometry.Normalize();
				actualNormalized = this._testResult.ResultGeometry.ToText();
			}

			// format the two wkt's...
			expectedNormalized = Test.FormatWKTString(expectedNormalized);
			actualNormalized = Test.FormatWKTString(actualNormalized);

			// make the comparison...
			if(expectedNormalized == actualNormalized)
			{
				this._testResult.PassFailWKT = true;
			}
		}

		/// <summary>
		/// Runs the methods on the geometries that return a boolean property.
		/// </summary>
		/// <param name="aGeometry">the A Geometry object</param>
		/// <param name="bGeometry">The B geometry object</param>
		/// <param name="arg">A string containing the name (A or B) of the geometry to call the method on.</param>
		/// <param name="testToRun">The name of the test to run.</param>
		private void RunBooleanReturnPropertyFunction(Geometry aGeometry, Geometry bGeometry, string arg, string testToRun)
		{

			string argToRun="A";

			Geotools.Geometries.Geometry c = aGeometry;
			if (arg == "B")
			{
				argToRun = "B";
				c = bGeometry;
			}
			_timer.Start();
			switch(testToRun)
			{
				case "isempty":
					this._testResult.Operation = argToRun + ".IsEmpty";
					this._testResult.PredicateResult = c.IsEmpty();
					this._testResult.SetPredicateResultString();
					TestForPassFailBooleanResult();
					break;
				case "issimple":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = argToRun + ".IsSimple";
					// call the predicate on the geometry and store the result...
					this._testResult.PredicateResult = c.IsSimple();
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "isvalid":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = argToRun + ".IsValid";
					// call the predicate on the geometry and store the result...
					this._testResult.PredicateResult = c.IsValid();
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
			}
			// stop the timer...
			_timer.Stop();
			this._testResult.TestDuration = _timer.Seconds;
		}

		/// <summary>
		/// Runs a test on a geometry that returns a boolean value.
		/// </summary>
		/// <param name="aGeometry">The A geometry object.</param>
		/// <param name="bGeometry">The B geometry object.</param>
		/// <param name="testToRun">The name of the test to run.</param>
		private void RunBooleanReturnGeometryFunction(Geometry aGeometry, Geometry bGeometry, string testToRun)
		{
			// start the timer...
			_timer.Start();
			// determine which test to run...
			switch(testToRun)
			{
				case "disjoint":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Disjoint B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Disjoint(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "relate":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Relate B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Relate(bGeometry, this._op.Arg3);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "equals":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Equals B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Equals(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "intersects":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Intersects B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Intersects(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "touches":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Touches B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Touches(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "crosses":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Crosses B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Crosses(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "within":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Within B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Within(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "contains":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Contains B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Contains(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "overlaps":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A Overlaps B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.Overlaps(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				case "equalstopology":
					// set the name of the operation in the TestResult...
					this._testResult.Operation = "A EqualsTopology B";
					// call the method on the geometry and store the result...
					this._testResult.PredicateResult = aGeometry.EqualsTopology(bGeometry);
					// set the string value of the predicate result for comparison...
					this._testResult.SetPredicateResultString();
					// test for pass or fail...
					TestForPassFailBooleanResult();
					break;
				default:
					break;
			}
			// stop the timer...
			_timer.Stop();
			this._testResult.TestDuration = _timer.Seconds;
		}

		/// <summary>
		/// This procedure runs all the methods that return geometry objects.
		/// </summary>
		/// <param name="aGeometry">The A geometry</param>
		/// <param name="bGeometry">The B geometry</param>
		/// <param name="testToRun">The method to call on the A geometry</param>
		private void RunGeometryFunction(Geometry aGeometry, Geometry bGeometry, string testToRun)
		{
			_timer.Start();
			// check which test to run...
			switch(testToRun)
			{
				case "convexhull":
					// set the name of the operation for use later...
					this._testResult.Operation = "A.ConvexHull()";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.ConvexHull();
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				case "getboundary":
					// set the name of the operation for use later...
					this._testResult.Operation = "A.GetBoundary()";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.GetBoundary();
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					//TestForPassFailGetBoundary();
					TestForPassFail();
					break;
				case "intersection":
					// set the name of the operation for use later...
					this._testResult.Operation = "A Intersection B";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.Intersection(bGeometry);
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				case "union":
					// set the name of the operation for use later...
					this._testResult.Operation = "A Union B";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.Union(bGeometry);
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				case "difference":
					// set the name of the operation for use later...
					this._testResult.Operation = "A Difference B";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.Difference(bGeometry);
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				case "symdifference":
					// set the name of the operation for use later...
					this._testResult.Operation = "A SymDifference B";
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.SymDifference(bGeometry);
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				case "buffer":
					// set the name of the operation for use later...
					this._testResult.Operation = "A.Buffer";
					// grab the distance of the buffer...
					double bufferDistance = double.Parse(this._op.Arg2);
					// call the method and store the returned geometry in 
					// the TestResult object...
					this._testResult.ResultGeometry = aGeometry.Buffer(bufferDistance);
					SetResultGeometryType(this._testResult.ResultGeometry);
					// Test for pass or fail...
					TestForPassFail();
					break;
				default:
					break;
			}
			// stop the timer...
			_timer.Stop();
			this._testResult.TestDuration = _timer.Seconds;
		}

		/// <summary>
		/// Procedure that loads the exception data into the TestResult object when an
		/// exception is thrown.
		/// </summary>
		/// <param name="e">The exception object.</param>
		/// <param name="detailedExceptionMessages">boolean that determines if detailed exception
		/// messages should be used.
		/// </param>
		private void SetExceptionData(System.Exception e, bool detailedExceptionMessages)
		{
			// Set the ExceptionThrown member to true...
			this._testResult.ExceptionThrown = true;
			// Grab the first part of the exception message...
			this._testResult.ExceptionMessage = e.Message + "\n";
			// if we want detailed exception messages, grab call the ToString()
			// method on the exception object...
			if(detailedExceptionMessages)
			{
				this._testResult.ExceptionMessage += e.ToString();
			}
			// Alert the user that an exception was thrown for this test...
			this._testResult.ResultGeometryWKT = "Exception thrown for this test.";
		}

		/// <summary>
		/// This is a temporary method that is being used to test if two geometries are equal.
		/// This is being used because the normalize methods on the geometries has not been implemented
		/// and we are getting false failures due to the WKT strings being different.
		/// This method creates two geometry objects from well-known text strings and then calls
		/// the EqualsTopology method on one of them to see if they are equal.
		/// </summary>
		/// <param name="wkt1">The well-known text string for the first geometry.</param>
		/// <param name="wkt2">The well-known text string for the second geometry.</param>
		/// <returns>
		/// True if the geometries are equal otherwise returns false.</returns>
		private bool TestTopologyEquals(string wkt1, string wkt2)
		{
			// create the GeometryFactory object...
			Geotools.Geometries.GeometryFactory geometryFactory = new Geotools.Geometries.GeometryFactory();
			// create the two geometries from the well-known text strings...
			Geometry a = (Geometry)geometryFactory.CreateFromWKT(wkt1);
			Geometry b = (Geometry)geometryFactory.CreateFromWKT(wkt2);

			// Call the EqualsTopology method and return...
			return a.EqualsTopology(b);
		}
			
		/// <summary>
		/// A static method that is used to compare 2 well-known text strings. The input strings
		/// are not changed by this method. The comparison is made after removal of all spaces,
		/// newline characters, tab characters, and carriage return characters.
		/// </summary>
		/// <param name="a">The first well-known text string.</param>
		/// <param name="b">The second well-known text string.</param>
		/// <returns>
		/// True if the strings match, otherwise returns false.
		/// </returns>
		public static bool CompareWKTStrings(string a, string b)
		{
			// replace all the spaces in the strings...
			string a1 = a.Replace(" ","").ToUpper();
			string b1 = b.Replace(" ","").ToUpper();

			// replace the newline characters in the strings...
			a1 = a1.Replace("\n","");
			b1 = b1.Replace("\n","");

			// replace all the tabs in the strings...
			a1 = a1.Replace("\t","");
			b1 = b1.Replace("\t","");

			// replace all the carriage returns in the strings...
			a1 = a1.Replace("\r","");
			b1 = b1.Replace("\r","");

			// return true or false based on the comparison...
			return a1 == b1;
		}

		/// <summary>
		/// A static method that removes all the unwanted spaces, tabs, newline, and carriage return
		/// characters in a well-known text string. The string that is passed in is not changed.
		/// </summary>
		/// <param name="a">The well-known text string to format.</param>
		/// <returns>
		/// A well-known text string that has the spaces, tabs, newline, and carriage return
		/// characters removed.
		/// </returns>
		public static string FormatWKTString(string a)
		{
			// make sure the string is a well-known text string and
			// not an exception notice or true or false...
			if(a == "Exception thrown for this test.")
			{
				return a;
			}
			if(a == "true")
			{
				return a;
			}
			if(a == "false")
			{
				return a;
			}
			// replace all the spaces in the strings...
	
			// a1 is a buffer. We will build the WKT string in a1...
			string a1 = "";
			// To remove only the spaces that are not separating the coordinates, 
			// loop through each character in the string...
			for(int i = 0; i < a.Length; i++)
			{
				// if we find a space...
				if(a.Substring(i, 1) == " ")
				{
					// check to see is the character on each side of the space is
					// a number. If it is, then we want to keep this space...
					if((char.IsNumber(a, i - 1) && char.IsNumber(a, i + 1)))
					{
						a1 += a.Substring(i, 1);
					}
					// otherwise, check to see if the character to the left of the space
					// is the comma separating the coordinate pairs. If it is, we want to
					// keep this space...
					else if(a.Substring(i - 1, 1) == ",")
					{
						a1 += a.Substring(i, 1);
					}
				}
				// otherwise, it's not a space so we want to keep the character...
				else
				{
					a1 += a.Substring(i, 1);
				}
			}	

			// replace the newline characters in the strings...
			a1 = a1.Replace("\n","");

			// replace all the tabs in the strings...
			a1 = a1.Replace("\t","");

			// replace all the carriage returns in the strings...
			a1 = a1.Replace("\r","");

			// do a check for the empty GeometryCollection...
			if(a1 == "GEOMETRYCOLLECTIONEMPTY")
			{
				a1 = "GEOMETRYCOLLECTION EMPTY";
			}
			else if(a1 == "MULTIPOINTEMPTY")
			{
				a1 = "MULTIPOINT EMPTY";
			}

			// return the formatted string...
			return a1;
		}

		private void SetResultGeometryType(Geometry g)
		{
			this._testResult.ResultGeometryType = g.GetGeometryType();
		}

		#endregion Methods
	}

}

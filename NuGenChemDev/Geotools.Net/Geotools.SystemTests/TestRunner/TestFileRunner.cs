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
using System.Configuration;
using System.Collections;
using Geotools.Geometries;


#endregion using Statements

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// The TestRunner object runs the tests that are in the XML files.
	/// It takes care of reading the files, creating the Run, TestCase, Test, and TestResult objects.
	/// It also gathers all the information from the tests, analyzes the test results, and returns
	/// the results in a string variable for display in a textbox or command line.
	/// </summary>
	public class TestFileRunner
	{

		#region Members

		private float _elapsedTime = 0.0F;

		#endregion

		#region Constructors

		public TestFileRunner()
		{
		}
	
		#endregion Constructors

		#region Private Methods

		/// <summary>
		/// Private function that reads all selected XML files and creates
		/// an ArrayList of Run objects containing all needed information to run each test
		/// in each XML test file.
		/// </summary>
		/// <param name="selectedFiles"></param>
		/// <returns>An ArrayList containing a Run object for each XML test file</returns>
		private ArrayList CreateRuns(string [] selectedFiles)
		{
			// check to make sure there are filenames in the string array...
			if(selectedFiles.Length == 0)
			{
				throw new ArgumentException("Argument length is 0.", "selectedFiles");
			}

			// get an ArrayList to hold the runs...
			ArrayList runs = new ArrayList();

			// get a TestFileReader object to read the XML test files...
			TestFileReader tReader = new TestFileReader();

			// loop through all the filenames and create the Run objects for each file...
			for(int i = 0; i < selectedFiles.Length; i++)
			{
				string fileName = selectedFiles[i];

				// create the Run object from the file...
				Run newRun = tReader.ReadTestFile(fileName);
				// store the XML filename in the Run object to display in the textbox later...
				newRun.Filename = fileName;

				// add the new Run to the runs ArrayList...
				runs.Add(newRun);
			}
			return runs;
		}

		/// <summary>
		/// A private method to create a precision model object based on the type in
		/// the Run object that is passed in.
		/// </summary>
		/// <param name="r">The Run object.</param>
		/// <returns>UrbanScience.Geographic.Geometries.PrecisionModel</returns>
		private Geotools.Geometries.PrecisionModel CreatePrecisionModel(Run r)
		{
			// if the type is FLOATING, call the constructor with no parameters...
			if(r.PrecisionModel.Type == "FLOATING")
			{
				return new Geotools.Geometries.PrecisionModel();
			}
			else
			{
				// otherwise call the constructor and pass in the attributes of the precision model...
				return new Geotools.Geometries.PrecisionModel(double.Parse(r.PrecisionModel.Scale), 
																			 double.Parse(r.PrecisionModel.OffSetX),
																			 double.Parse(r.PrecisionModel.OffSetY));
			}
		}

		/// <summary>
		/// A private method to extract the information contained in the Run object
		/// and return.
		/// </summary>
		/// <param name="r">The Run object from which to extract the information.</param>
		/// <returns>A string containing the information about the Run object passed in.</returns>
		private string GetRunInformation(Run r)
		{
			// Extract the information for the current Run in a string and return it...
			// The XML test file associated with this Run object...
			string temp = "Filename: " + r.Filename + "\n";
			// The description for the Run...
			temp += "Run description: " + r.Description + "\n";
			// The number of TestCase objects in this run...
			temp += "Total test cases in this run: " + r.TestCaseCount + "\n";

			// count all the tests in the Run...
			long totalTests = 0;
			foreach(TestCase tc in r)
			{
				totalTests += tc.TestCount;
			}
			
			// add the number of tests in the run to the return string...
			temp += "Total tests in this run: " + totalTests.ToString() + "\n";

			// return the string...
			return temp;
		}

		/// <summary>
		/// A private method that runs all the tests.
		/// </summary>
		/// <param name="r">An ArrayList containing all the Run ojects</param>
		/// <param name="numTests">A long to count the number of tests run.</param>
		/// <param name="returnString">The string that will be returned. Needed to load the 
		/// information for each run.</param>
		/// <returns>An ArrayList containing TestResult objects for each test that was run.</returns>
		private ArrayList RunAllTests(ArrayList runs, bool detailedExceptionMessages, bool removeWKTWhitespace, ref string returnString)
		{
			// get an ArrayList to hold the TestResult objects...
			ArrayList results = new ArrayList();

			// get a long to count the number of tests in each Run and set it to 0...
			long numTests = 0;

			// now loop through all the Runs and run the tests that are inside each Run...
			foreach(Run r in runs)
			{
				// each Run needs a precision model...
				Geotools.Geometries.PrecisionModel precisionModel = CreatePrecisionModel(r);
				
				// Store the information for the current Run in a string and display it in the textbox...
				returnString += GetRunInformation(r);

				// We need two geometry objects for the tests. An "A" and a "B". Declared at this
				// scope level so we can have access to them in the Test object foreach loop...
				Geometry aGeometry = null;
				Geometry bGeometry = null;

				// now loop through each TestCase in the Run and run the tests in each TestCase...
				foreach(TestCase tc in r)
				{
					// grab the number of tests in the TestCase object...
					numTests = tc.TestCount;

					// get the geometry objects for the test...

					// make sure there's a WKT string in the A property...
					if(tc.A != "")
					{
						aGeometry = (Geometry)tc.CreateAGeometry(precisionModel);
					}
					// make sure the test needs a "B" geometry. If it does, then create one...
					if(tc.B != "")
					{
						bGeometry = (Geometry)tc.CreateBGeometry(precisionModel);
					}

					// Now loop through each Test in the current TestCase and run it...
					foreach(Test t in tc)
					{
						// Get a TestResult object to store the results of each test.
						// Pass in true because we always want detailed exception messages...
						TestResult testResult = t.Run(aGeometry, bGeometry, detailedExceptionMessages);
						// Add the description of the current TestCase to the TestResult object...
						testResult.TestCaseDescription = tc.Description;
						// Add the TestResult object to the ArrayList of results...
						results.Add(testResult);
					}
				}
				// Analyze the test results...
				returnString += AnalyzeTestResults(results, removeWKTWhitespace);
				results.Clear();
			}
			returnString += "Total elapsed time for all tests: " + _elapsedTime.ToString() + " seconds.\n";
			return results;
		}

		/// <summary>
		/// private method that goes through all the TestResult objects and builds a string
		/// containing the results of the tests that were run.
		/// </summary>
		/// <param name="results">An ArrayList containing all the TestResult objects for one Run object.</param>
		/// <param name="removeWKTWhitespace">boolean to indicate whether or not to remove the whitespace from WKT</param>
		/// <returns>a string containing the formatted results of the tests.</returns>
		private string AnalyzeTestResults(ArrayList results, bool removeWKTWhitespace)
		{
			// get a couple of variables to count the passed and failed tests...
			int passedTests = 0;
			int failedTests = 0;
			int exceptionsThrown = 0;

			// a string to build the messages that will be returned...
			string returnString = string.Empty;
			string failedTestString = string.Empty;

			// step through each TestResult in the results ArrayList and count the 
			// passed and failed tests. Then build the message string to be displayed in
			// the textbox...
			foreach(TestResult tr in results)
			{
				_elapsedTime += tr.TestDuration;

				// if the test has passed, increment the passed test variable...
				if(tr.PassFailWKT == true)
				{
					passedTests++;
				}
				else
				{
					// otherwise the test has failed, so build the failed test string...
					// grab the test description from the TestResult object...
					failedTestString += "Test Case Description: " + tr.TestCaseDescription + "\n";
					// grab the 2 geometries that were tested...
					failedTestString += "A Geometry: " + Test.FormatWKTString(tr.AGeometryWKT) + "\n";
					failedTestString += "B Geometry: " + Test.FormatWKTString(tr.BGeometryWKT) + "\n";
					// grab the operation that was ran...
					failedTestString += "Operation: " + tr.Operation + "\n";
					// check to see if the user has checked or unchecked the remove spaces checkbox...
					if(removeWKTWhitespace)
					{
						// if it is checked, remove the spaces from the WKT and then add it to the 
						// message string...
						failedTestString += "Expected Result: " + Test.FormatWKTString(tr.ExpectedWKT) + "\n";
						failedTestString += "Returned Result:  " + Test.FormatWKTString(tr.ResultGeometryWKT) + tr.PredicateResultString;
						failedTestString += tr.PropertyTestResult + "\n\n";
					}
					else
					{
						// otherwise, the checkbox is not checked so just add the WKT to the message string...
						failedTestString += "Expected Result: " + tr.ExpectedWKT + "\n";
						failedTestString += "Returned Result:  " + tr.ResultGeometryWKT + tr.PredicateResultString;
						failedTestString += tr.PropertyTestResult + "\n\n";
					}
					// increment the failed test counter...
					failedTests++;

					// check to see if an exception was thrown during the test...
					if(tr.ExceptionThrown)
					{
						// if so, grab the exception details and add them to the message string...
						failedTestString += "\tException Details:\n";
						failedTestString += "\t" + tr.ExceptionMessage + "\n";
						// increment the exception thrown counter...
						exceptionsThrown++;
					}
				}
			}
			// Display the totals in the textbox...
			returnString += "\nTotals:\n";
			returnString += "Passed Tests: " + passedTests + "\n";
			returnString += "Failed Tests: " + failedTests + "\n";
			returnString += "Exceptions Thrown: " + exceptionsThrown + "\n\n";

			// if any tests failed, add the failed test string to the textbox...
			if(failedTests > 0)
			{
				returnString += "Failed Operations: \n" + failedTestString + "\n";
			}

			// add a row of stars to separate the results of each test file...
			returnString += CreateRunSeparator();

			return returnString;
		}

		/// <summary>
		/// A private method that creates a string of asterisks that will be used
		/// to separate the results of each test run when displayed in the UI textbox.
		/// </summary>
		/// <param name="numStars">The number of asterisks to generate.</param>
		/// <returns>A string containing a line of asterisks.</returns>
		private string CreateRunSeparator()
		{
			// give the number of stars a default value...
			int numStars = 30;

			// Now check the config file and see if there is a setting there to override the default value...
			if(ConfigurationSettings.AppSettings["NumberOfAsterisksInRunSeparator"] != null)
			{
				numStars = int.Parse(ConfigurationSettings.AppSettings["NumberOfAsterisksInRunSeparator"]);
			}

			// get a string to hold the asterisks...
			string returnString = string.Empty;
			// generate the asterisks...
			for(int i = 0; i < numStars; i++)
			{
				returnString += "*";
			}
			
			// append a couple of newlines for good measure...
			returnString += "\n\n";

			// return the string...
			return returnString;
		}

		#endregion Private Methods

		#region Public Methods

		/// <summary>
		/// Public method that runs the tests that are passed into it in the selectedFiles string array.
		/// </summary>
		/// <param name="selectedFiles">A string array containing the test file name the user selected.</param>
		/// <param name="detailedExceptionMessages">boolean for whether or not to print detailed exception messages</param>
		/// <param name="removeWKTWhitespace">boolean for whether or not to remove the whitespace from the WKT</param>
		/// <returns>return a string containing all the test data.</returns>
		public string RunTests(string [] selectedFiles, bool detailedExceptionMessages, bool removeWKTWhitespace)
		{
			// get a string to hold the information for each run.
			// This string will be returned to the caller...
			string returnString = string.Empty;

			// get an ArrayList to hold the Run objects...
			ArrayList runs = CreateRuns(selectedFiles);			

			// get an ArrayList to hold the TestResults and run the tests...
			ArrayList results = RunAllTests(runs, detailedExceptionMessages, removeWKTWhitespace, ref returnString);
			
			// return the string...
			return returnString;
		}

		/// <summary>
		/// Public method that is used to display the Run information and tests inside an XML test file.
		/// </summary>
		/// <param name="selectedFiles">An ArrayList containing the filenames selected by the user.</param>
		/// <returns>A string containing the information for all the selected files.</returns>
		public string DisplayTestInformation(string [] selectedFiles)
		{
			// get a string to hold the information for each run.
			// This string will be returned to the caller...
			string returnString = string.Empty;

			// get an ArrayList to hold the Run objects...
			ArrayList runs = CreateRuns(selectedFiles);		

			// loop through the runs in the runs ArrayList and print out their contents...
			foreach(Run r in runs)
			{
				// a counter for the number of tests...
				int numTests = 0;
				// call the ToString method on each run and print the results to the textbox...
				returnString += r.ToString() + "\n";
				// loop through each TestCase in the current run...
				foreach(TestCase tc in r)
				{
					// call the ToString on each TestCase and print the results to the textbox...
					returnString += tc.ToString() + "\n";
					// loop through each Test in the current TestCase...
					foreach(Test t in tc)
					{
						// call the ToString on each Test and print the results to the textbox... 
						returnString += "Test " + ++numTests + "\n" + t.ToString() + "\n";
					}
				}
				// add a row of stars to separate the results of each test file...
				returnString += CreateRunSeparator();
			}

			// return the data...
			return returnString;
		}

		#endregion Public Methods
	
	}
}

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
using System.Xml;
using System.IO;

#endregion using Statements

namespace Geotools.SystemTests.TestRunner
{
	/// <summary>
	/// The TestFileReader class is used to read in the XML test files and create
	/// the test objects (Run, TestCase, Test, etc.) used to run the tests.
	/// </summary>
	public class TestFileReader
	{

		#region Constructors

		public TestFileReader()
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		#endregion Constructors

		#region Methods
		
		/// <summary>
		/// This method builds the entire Run object for the current test file.
		/// The Run object has a description, a precision model, and a number of 
		/// test cases. Each test case contains a number of tests which each contain
		/// one operation to perform.
		/// </summary>
		/// <param name="tr">
		/// tr is an XmlTextReader object that already contains the XML from the test file.
		/// </param>
		/// <returns>
		/// Returns a complete Run object to the caller.
		/// </returns>
		private Run BuildRunObject(XmlTextReader tr)
		{
			// make sure the XmlTextReader is not null...
			if(tr == null)
			{
				throw new System.ArgumentNullException("The XML reader is null!");
			}
			// make sure we know where we are in the Xml file. We should be at
			// the first run element...
			else if(tr.Name != "run")
			{
				throw new System.ArgumentException("The XML reader is not pointing to the <run> element");
			}

			// Get Run object to build...
			Run tmpRun = new Run();
			
			// The XmlTextReader comes in to this method "pointing" to the 
			// <run> element so do another read to look for the description element...
			tr.Read();

			// make sure the current node is an element...
			if(tr.NodeType == XmlNodeType.Element)
			{
				// we're looking for the description node of the run...
				if(tr.Name == "desc")
				{
					// if the name is desc, then we have a description
					// for the run. So do another read, make sure it is
					// a text node and set the description field in the Run
					// object...
					tr.Read();
					// make sure the current node is text...
					if(tr.NodeType == XmlNodeType.Text)
					{
						// if it is text, then grab
						tmpRun.Description = tr.Value;
						// do 2 more reads to move to the precisionModel node...
						tr.Read();
						tr.Read();
					}
				}
			}
			
			// if there is no description, then the next node should be
			// the precision model for the run. Check to make sure...
			if(tr.Name == "precisionModel")
			{
				// Get some variables to hold the attributes of the precision model...
				string pmType = "";
				string pmScale = "";
				string pmOffSetX = "";
				string pmOffSetY = "";
											
				// make sure the node has attributes...
				if (tr.HasAttributes)
				{
					// move through the attributes and store the values in
					// the variables...
					while (tr.MoveToNextAttribute())
					{
						if(tr.Name == "type")
						{
							pmType = tr.Value;
						}
						else if(tr.Name == "scale")
						{
							pmScale = tr.Value;
						}
						else if(tr.Name == "offsetx")
						{
							pmOffSetX = tr.Value;
						}
						else if(tr.Name == "offsety")
						{
							pmOffSetY = tr.Value;
						}
					}
				}
				// Add the precision model to the Run object...
				tmpRun.SetPrecisionModel(pmType, pmScale, pmOffSetX, pmOffSetY);
			}
			// return the Run object...
			return tmpRun;
		}


		/// <summary>
		/// BuildTestCase builds a TestCase object from the current XML file.
		/// </summary>
		/// <param name="tr">
		/// tr is an XmlTextReader object that already contains the XML from the test file.
		/// </param>
		/// <returns>Returns a complete TestCase object to the caller.
		/// </returns>
		private TestCase BuildTestCase(XmlTextReader tr)
		{
			// make sure the XmlTextReader is not null...
			if(tr == null)
			{
				throw new System.ArgumentNullException("The XML reader is null!");
			}
				// make sure we know where we are in the Xml file. We should be at
				// the first <case> element...
			else if(tr.Name != "case")
			{
				throw new System.ArgumentException("The XML reader is not pointing to the <case> element");
			}

			// Get a TestCase object to build and return...
			TestCase tc = new TestCase();
			
			// we know that we're at a <case> element so do a read to move
			// to the <desc> element...
			tr.Read();

			// we know we've hit the opening <case> element to get here, so now
			// read the file until we've hit the closing </case> element...
			while(tr.Name != "case")
			{
				// make sure the current node is an element...
				if(tr.NodeType == XmlNodeType.Element)
				{
					// grab the description of the TestCase...
					if(tr.Name == "desc")
					{
						// do a read to get to the text portion of the description...
						tr.Read();
						tc.Description = tr.Value;
					}
					// grab the a geometry...
					else if(tr.Name == "a")
					{
						// do a read to move to the text part of the a geometry...
						tr.Read();
						tc.A = tr.Value;
						tc.A = tc.A.Trim();
					}
					// grab the b geometry...
					else if(tr.Name == "b")
					{
						// do a read to move to the text part of the b geometry...
						tr.Read();
						tc.B = tr.Value;
						tc.B = tc.B.Trim();
					}
					// grab the test...
					else if(tr.Name == "test")
					{
						// do a read to move to the <op> element...
						tr.Read();
						// make sure we're at the <op> element...
						if(tr.Name == "op")
						{
							// get the attributes of the <op> element...
							if (tr.HasAttributes)
							{
								string name = string.Empty;
								string arg1 = string.Empty;
								string arg2 = string.Empty;
								string arg3 = string.Empty;
								string result = string.Empty;
								// move through the attributes...
								while (tr.MoveToNextAttribute())
								{
									if(tr.Name == "name")
									{
										name = tr.Value;
									}
									else if(tr.Name == "arg1")
									{
										arg1 = tr.Value;
									}
									else if(tr.Name == "arg2")
									{
										arg2 = tr.Value;
									}
									else if(tr.Name == "arg3")
									{
										arg3 = tr.Value;
									}
								}
								// do another read to move to the expected result...
								tr.Read();
								// trim the spaces of the result...
								result  = tr.Value;
								result = result.Trim();
								result = Test.FormatWKTString(result);

								// add the Test to the TestCase...
								tc.AddTest(name, arg1, arg2, arg3, tc.A, tc.B, result);
							}
						}
					}
				}
				// do a read to the next node...
				tr.Read();
			}
			// return the complete TestCase object...
			return tc;
		}

		/// <summary>
		/// ReadTestFile starts the process of building the Run opject by creating
		/// the Run and XmlTextReader objects. It then opens the XML file via the XmlTextReader
		/// and does a read on the XmlTextReader
		/// object to position the current node to the first run element. It then calls
		/// the BuildRunObject private method to build the Run object. It continues to read
		/// the XML file finding each case element and the calls the private method BuileTestCase.
		/// After each TestCase object is built, it is added to the Run object.
		/// </summary>
		/// <param name="fileName">
		/// fileName contains the complete path to the XML test file.
		/// </param>
		/// <returns>
		/// Returns a Run object containing all the test cases, geometries, tests, and operations
		/// from the XML test file.
		/// </returns>
		public Run ReadTestFile(string fileName)
		{
			// Verify the parameter...
			if(fileName == "")
			{
				throw new System.ArgumentException("The XML filename is empty!");
			}
			if(fileName == null)
			{
				throw new System.ArgumentNullException("The XML filename is null!");
			}
			if(!File.Exists(fileName))
			{
				throw new System.ArgumentException("The XML file does not exist!");
			}

			// Get a Run object to hold the information and return
			// to caller...
			Run tmpRun = new Run();
			
			// Get an XmlTextReader object to read the file...
			XmlTextReader tr = new XmlTextReader(fileName);
			// Turn off the whitespace handling...
			tr.WhitespaceHandling = WhitespaceHandling.None;

			// start reading the file...
			while(tr.Read())
			{
				// make sure the node type is an element...
				if(tr.NodeType == XmlNodeType.Element)
				{
					// if we're at the run element, build the Run object...
					if(tr.Name == "run")
					{
						tmpRun = BuildRunObject(tr);
					}
					// if we're at a case element, build the test case...
					else if(tr.Name == "case")
					{
						TestCase tc = BuildTestCase(tr);
						tmpRun.AddTestCase(tc);
					}
				}
			}
			
			// return the complete Run object...
			return tmpRun;
		}

		#endregion Methods

	}
}

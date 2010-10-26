#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/EnvelopeTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: EnvelopeTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 4     11/04/02 12:11p Rabergman
 * Changed Namespace
 * 
 * 3     10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 2     9/18/02 11:05a Rabergman
 * Added: SetUp, TearDown & 2 Initialize test.
 * 
 * 1     8/23/02 11:12a Rabergman
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Geometries
{
	/// <summary>
	/// Summary description for EnvelopeTest.
	/// </summary>
	[TestFixture]
	public class EnvelopeTest 
	{
//		double _x1 = 1.0;
//		double _y1 = 2.0;
//		double _x2 = 3.0;
//		double _y2 = 4.0;

		

		public void test_Constructor1()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a null envelope
//			Envelope env = new Envelope();
//
//			//set the envelope to null so that we can test that it exists
//			env.SetToNull();
//
//			AssertEquals("Constructor1: ", true, env.IsNull());
		}

		public void test_Constructor2()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope with four doubles passed in
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("Constructor2-1: ", 1.0, env.MinX);
//			AssertEquals("Constructor2-2: ", 2.0, env.MinY);
//			AssertEquals("Constructor2-3: ", 3.0, env.MaxX);
//			AssertEquals("Constructor2-4: ", 4.0, env.MaxY);
		}

		public void test_Constructor3()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create two new coordniates to use to create the new envelope
//			Coordinate coordMin = new Coordinate(_x1, _y1);
//			Coordinate coordMax = new Coordinate(_x2, _y2);
//
//			//create a new envelope with two coordinates passed in
//			Envelope env = new Envelope(coordMin, coordMax);
//
//			AssertEquals("Constructor3-1: ", 1.0, env.MinX);
//			AssertEquals("Constructor3-2: ", 2.0, env.MinY);
//			AssertEquals("Constructor3-3: ", 3.0, env.MaxX);
//			AssertEquals("Constructor3-4: ", 4.0, env.MaxY);
//
//			//create a null coordinate to be sure the nulls are set to zero
//			Coordinate coordNull = new Coordinate();
//
//			Envelope env2 = new Envelope(coordMax, coordNull);
//			AssertEquals("Constructor3-1: ", 0.0, env2.MinX);
//			AssertEquals("Constructor3-2: ", 0.0, env2.MinY);
//			AssertEquals("Constructor3-3: ", 3.0, env2.MaxX);
//			AssertEquals("Constructor3-4: ", 4.0, env2.MaxY);
		}

		public void test_Constructor4()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new coordniate to use to create the new envelope
//			Coordinate coord = new Coordinate(_x1, _y1);
//
//			//create a new envelope with coordinate passed in
//			Envelope env = new Envelope(coord);
//
//			AssertEquals("Constructor4-1: ", 1.0, env.MinX);
//			AssertEquals("Constructor4-2: ", 2.0, env.MinY);
//			//the max values should be the same as the min values
//			AssertEquals("Constructor4-3: ", 1.0, env.MaxX);
//			AssertEquals("Constructor4-4: ", 2.0, env.MaxY);
		}

		public void test_Constructor5()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new coordniate to use to create the new envelope
//			Coordinate coord = new Coordinate(_x1, _y1);
//
//			//create a new envelope with coordinate passed in
//			Envelope env = new Envelope(coord);
//
//			//create a new envelope using the envelope created earlier
//			Envelope env2 = new Envelope(env);
//
//			AssertEquals("Constructor5-1: ", true, env.Equals(env2));
		}

		public void test_Initialize1()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new coordniate to use to create the new envelope
//			Coordinate coord = new Coordinate(_x1, _y1);
//
//			//create a new envelope with coordinate passed in
//			Envelope env = new Envelope(coord);
//
//			env.Initialize();
//
//			AssertEquals("Initialize1-1: ", true, env.IsNull());
		}

		public void test_Initialize2()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new coordniate to use to create the new envelope
//			Coordinate coord = new Coordinate(0.0, 0.0);
//
//			//create a new envelope with coordinate passed in
//			Envelope env = new Envelope(coord);
//
//			//reinitialize the x & y values
//			env.Initialize(_x1, _x2, _y1, _y2);
//
//			//check that they are as expected
//			AssertEquals("Initialize2-1: ", 1.0, env.MinX);
//			AssertEquals("Initialize2-2: ", 2.0, env.MinY);
//			AssertEquals("Initialize2-3: ", 3.0, env.MaxX);
//			AssertEquals("Initialize2-4: ", 4.0, env.MaxY);
//
//			//reinitialize the x & y values
//			env.Initialize(double.NaN, _x2, _y1, _y2);
//
//			//check that they are as expected (because the x1 value was NotANumber the value of x2 is used)
//			AssertEquals("Initialize2-5: ", 3.0, env.MinX);
//
//			//reinitialize the x & y values
//			env.Initialize(double.NaN, double.NaN, _y1, _y2);
//
//			//check that they are as expected (because both x values are NaN - NaN is returned)
//			AssertEquals("Initialize2-5: ", double.NaN, env.MinX);
		}

		public void test_Initialize3()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new coordniate to use to create the new envelope
//			Coordinate coord = new Coordinate(0.0, 0.0);
//
//			//create a new envelope with coordinate passed in
//			Envelope env = new Envelope(coord);
//
//			//create a new envelope to use to initialize the other envelope
//			Envelope env2 = new Envelope(_x1, _x2, _y1, _y2);
//
//			//initialize the envelope
//			env.Initialize(env2);
//
//			//check that they are as expected
//			AssertEquals("Initialize3-1: ", 1.0, env.MinX);
//			AssertEquals("Initialize3-2: ", 2.0, env.MinY);
//			AssertEquals("Initialize3-3: ", 3.0, env.MaxX);
//			AssertEquals("Initialize3-4: ", 4.0, env.MaxY);
		}

		public void test_Initialize4()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create new coordniates to use to create the new envelope
//			Coordinate coord = new Coordinate(_x1, _y1);
//			Coordinate coord2 = new Coordinate(_x2, _y2);
//
//			//create a new envelope to use to initialize the other envelope
//			Envelope env = new Envelope();
//
//			//initialize the envelope
//			env.Initialize(coord, coord2);
//
//			//check that they are as expected
//			AssertEquals("Initialize4-1: ", 1.0, env.MinX);
//			AssertEquals("Initialize4-2: ", 2.0, env.MinY);
//			AssertEquals("Initialize4-3: ", 3.0, env.MaxX);
//			AssertEquals("Initialize4-4: ", 4.0, env.MaxY);
		}

		public void test_Initialize5()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create new coordniates to use to create the new envelope
//			Coordinate coord = new Coordinate(_x1, _y1);
//
//			//create a new envelope to use to initialize the other envelope
//			Envelope env = new Envelope();
//
//			//initialize the envelope
//			env.Initialize(coord);
//
//			//check that they are as expected
//			AssertEquals("Initialize5-1: ", 1.0, env.MinX);
//			AssertEquals("Initialize5-2: ", 2.0, env.MinY);
//			AssertEquals("Initialize5-3: ", 1.0, env.MaxX);
//			AssertEquals("Initialize5-4: ", 2.0, env.MaxY);
		}

		public void test_SetToNull()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//set the envelope to be null
//			env.SetToNull();
//
//			AssertEquals("SetToNull: ", true, env.IsNull());
		}

		public void test_IsNull()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//should return a false - it is not null right now...
//			AssertEquals("IsNull1: ", false, env.IsNull());
//
//			//now set the envelope to null
//			env.SetToNull();
//            
//			//Now it should return a true.
//			AssertEquals("IsNull2: ", true, env.IsNull());
		}

		public void test_width()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("Width: ", 2.0, env.Width);
		}

		public void test_Height()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("Height: ", 2.0, env.Height);
		}

		public void test_MinX()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("MinX: ", 1.0, env.MinX);
		}

		public void test_MaxX()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("MaxX: ", 3.0, env.MaxX);
		}

		public void test_MinY()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("MinY: ", 2.0, env.MinY);
		}

		public void test_MaxY()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("MaxY: ", 4.0, env.MaxY);
		}

		public void test_ExpandToInclude1()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//create a new coordinate to use for expanding
//			Coordinate coord = new Coordinate(5.0, 5.0);
//
//			//Expand the envelope (max coords are larger)
//			env.ExpandToInclude(coord);
//
//			//check the maxX & maxY to be sure it worked
//			AssertEquals("ExpandToInclude1-1: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude1-2: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude1-3: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude1-4: ", 2.0, env.MinY);
//
//			//create a new coordinate to that is within the envelope
//			Coordinate coord2 = new Coordinate(2.0, 3.0);
//
//			//Expand the envelope (both coords are within the current envelope)
//			env.ExpandToInclude(coord2);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude1-5: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude1-6: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude1-7: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude1-8: ", 2.0, env.MinY);
//
//			//create a null coordinate
//			Coordinate coord3 = new Coordinate();
//
//			//Expand the envelope (min coords are smaller)
//			env.ExpandToInclude(coord3);
//
//			//check the maxX & maxY to be sure it worked
//			AssertEquals("ExpandToInclude1-9: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude1-10: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude1-11: ", 0.0, env.MinX);
//			AssertEquals("ExpandToInclude1-12: ", 0.0, env.MinY);
		}

		public void test_ExpandToInclude2()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//Expand the envelope (max coords are larger)
//			env.ExpandToInclude(5.0, 5.0);
//
//			//check the maxX & maxY to be sure it worked
//			AssertEquals("ExpandToInclude2-1: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-2: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-3: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude2-4: ", 2.0, env.MinY);
//
//			//Expand the envelope (both coords are within the current envelope)
//			env.ExpandToInclude(2.0, 3.0);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude2-5: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-6: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-7: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude2-8: ", 2.0, env.MinY);
//
//			//Expand the envelope(both coords are smaller)
//			env.ExpandToInclude(0.5, 0.5);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude2-9: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-10: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-11: ", 0.5, env.MinX);
//			AssertEquals("ExpandToInclude2-12: ", 0.5, env.MinY);
//
//			//Expand the envelope (both coords are NotANumber)
//			env.ExpandToInclude(double.NaN, double.NaN);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude2-13: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-14: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-15: ", 0.5, env.MinX);
//			AssertEquals("ExpandToInclude2-16: ", 0.5, env.MinY);
//		
//			//Expand the envelope(one coord is smaller)
//			env.ExpandToInclude(0.0, 0.5);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude2-17: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-18: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-19: ", 0.0, env.MinX);
//			AssertEquals("ExpandToInclude2-20: ", 0.5, env.MinY);
		}

		public void test_ExpandToInclude3()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//			//create another envelope for expanding
//			Envelope env2 = new Envelope(1.0, 5.0, 3.0, 5.0);
//
//			//Expand the envelope (max coords are larger)
//			env.ExpandToInclude(env2);
//
//			//check the maxX & maxY to be sure it worked
//			AssertEquals("ExpandToInclude3-1: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude3-2: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude3-3: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude3-4: ", 2.0, env.MinY);
//
//			//another envelope
//			Envelope env3 = new Envelope(1.0, 2.0, 2.0, 3.0);
//
//			//Expand the envelope (both coords are within the current envelope)
//			env.ExpandToInclude(env3);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude3-5: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude3-6: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude3-7: ", 1.0, env.MinX);
//			AssertEquals("ExpandToInclude3-8: ", 2.0, env.MinY);
//
//			//another envelope
//			Envelope env4 = new Envelope(0.5, 2.0, 0.5, 3.0);
//
//			//Expand the envelope(both coords are smaller)
//			env.ExpandToInclude(env4);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude3-9: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude3-10: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude3-11: ", 0.5, env.MinX);
//			AssertEquals("ExpandToInclude3-12: ", 0.5, env.MinY);
//
//			//another envelope
//			Envelope env5 = new Envelope();
//
//			//Expand the envelope (both coords are NotANumber)
//			env.ExpandToInclude(double.NaN, double.NaN);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude3-13: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude3-14: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude3-15: ", 0.5, env.MinX);
//			AssertEquals("ExpandToInclude3-16: ", 0.5, env.MinY);
//		
//			//another envelope
//			Envelope env6 = new Envelope(0.0, 2.0, 0.5, 3.0);
//
//			//Expand the envelope(one coord is smaller)
//			env.ExpandToInclude(env6);
//
//			//check the maxX & maxY to be sure nothing changed
//			AssertEquals("ExpandToInclude2-17: ", 5.0, env.MaxX);
//			AssertEquals("ExpandToInclude2-18: ", 5.0, env.MaxY);
//			AssertEquals("ExpandToInclude2-19: ", 0.0, env.MinX);
//			AssertEquals("ExpandToInclude2-20: ", 0.5, env.MinY);
		}

		public void test_Contains1()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//create a new coordinate to test against (true)
//			Coordinate coord = new Coordinate(2.0, 2.0);
//			//create another coordinate for testing (false)
//			Coordinate coord2 = new Coordinate(1.0, 1.0);
//			//create a null coordinate for testing
//			Coordinate coord3 = new Coordinate();
//
//			AssertEquals("Contains1-1: ", true, env.Contains(coord));
//			AssertEquals("Contains1-2: ", false, env.Contains(coord2));
//			AssertEquals("Contains1-3: ", false, env.Contains(coord3));
		}

		public void test_Contains2()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("Contains2-1: ", false, env.Contains(0.0, 0.0));
//			AssertEquals("Contains2-2: ", true, env.Contains(1.5, 2.5));
//			AssertEquals("Contains2-3: ", false, env.Contains(double.NaN, double.NaN));
		}

		public void test_Contains3()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//create another envelope for testing(true)
//			Envelope env2 = new Envelope(1.5, 2.5, 2.5, 3.5);
//			//create another envelope for testing (false doesn't contain any points)
//			Envelope env3 = new Envelope(4.0, 4.0, 5.0, 5.0);
//			//create another envelope for testing (false contain some of the points)
//			Envelope env4 = new Envelope(2.0, 4.0, 2.0, 4.0);
//			//create a null envelope for testing
//			Envelope env5 = new Envelope();
//
//			AssertEquals("Contains3-1: ", true, env.Contains(env2));
//			AssertEquals("Contains3-1: ", false, env.Contains(env3));
//			AssertEquals("Contains3-3: ", false, env.Contains(env4));
//			AssertEquals("Contains3-4: ", false, env.Contains(env5));
		}

		public void test_Overlaps1()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//create a new coordinate to test against (true)
//			Coordinate coord = new Coordinate(2.0, 2.0);
//			//create another coordinate for testing (false)
//			Coordinate coord2 = new Coordinate(1.0, 1.0);
//			//create a null coordinate for testing
//			Coordinate coord3 = new Coordinate();
//
//			AssertEquals("Overlaps1-1: ", true, env.Overlaps(coord));
//			AssertEquals("Overlaps1-2: ", false, env.Overlaps(coord2));
//			AssertEquals("Overlaps1-3: ", false, env.Overlaps(coord3));
		}

		public void test_Overlaps2()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope with four doubles passed in
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("Overlaps2-1: ", false, env.Overlaps(0.0, 0.0));
//			AssertEquals("Overlaps2-2: ", true, env.Overlaps(2.0, 2.0));
//			AssertEquals("Overlaps2-3: ", false, env.Overlaps(double.NaN, double.NaN));
		}
	
		public void test_Overlaps3()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			//create another envelope for testing(true)
//			Envelope env2 = new Envelope(1.5, 2.5, 2.5, 3.5);
//			//create another envelope for testing (false doesn't contain any points)
//			Envelope env3 = new Envelope(4.0, 4.0, 5.0, 5.0);
//			//create another envelope for testing (false contain some of the points)
//			Envelope env4 = new Envelope(2.0, 4.0, 2.0, 4.0);
//			//create a null envelope for testing
//			Envelope env5 = new Envelope();
//
//			AssertEquals("Overlaps3-1: ", true, env.Overlaps(env2));
//			AssertEquals("Overlaps3-1: ", false, env.Overlaps(env3));
//			AssertEquals("Overlaps3-3: ", true, env.Overlaps(env4));
//			AssertEquals("Overlaps3-4: ", false, env.Overlaps(env5));
		}

		public void test_Equals()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//			//create another envelope to test against
//			Envelope env2 = new Envelope(_x1, _x2, _y1, _y2);
//			//create another envelope to test against
//			Envelope env3 = new Envelope(_x1, _x2, _y1, 5.0);
//			//create a null envelope to test against
//			Envelope env4 = new Envelope();
//
//			AssertEquals("Equals-1: ", true, env.Equals(env2));
//			AssertEquals("Equals-2: ", false, env.Equals(env3));
//			AssertEquals("Equals-3: ", false, env.Equals(env4));
		}

		public void test_GetHashCode()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//			//create another envelope for testing
//			Envelope env2 = new Envelope(_x1, _x2, _y1, _y2);
//			//create another envelope for testing
//			Envelope env3 = new Envelope(_x1, _x2, _y1, 5.0);
//			//create another envelope for testing against a null evnvelope
//			Envelope env4 = new Envelope();
//
//			AssertEquals("GetHashCode-1: ", true , env.GetHashCode()==env2.GetHashCode());
//			AssertEquals("GetHashCode-1: ", false, env.GetHashCode()==env3.GetHashCode());
//			AssertEquals("GetHashCode-1: ", false, env.GetHashCode()==env4.GetHashCode());
		}

		public void test_ToString()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			//create a new envelope with four doubles passed in
//			Envelope env = new Envelope(_x1, _x2, _y1, _y2);
//
//			AssertEquals("ToString: ", "Env[1 : 3, 2 : 4]", env.ToString());
		}

		public void test_Intersects1()
		{
//			Fail("Not implemented");
		}

		public void test_Intersects2()
		{
//			Fail("Not implemented");
		}

		public void test_Distance1()
		{
//			Fail("Not implemented");
		}

		public void test_Distance2()
		{
//			Fail("Not implemented");
		}
	}
}

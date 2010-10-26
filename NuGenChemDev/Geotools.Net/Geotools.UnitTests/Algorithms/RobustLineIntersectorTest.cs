#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Algorithms/RobustLineIntersectorTest.cs,v 1.1 2003/01/02 20:27:23 awcoats Exp $
 * $Log: RobustLineIntersectorTest.cs,v $
 * Revision 1.1  2003/01/02 20:27:23  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:00p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     11/04/02 3:19p Rabergman
 * Changed namespaces
 * 
 * 4     10/31/02 11:18a Awcoats
 * 
 * 3     10/21/02 2:23p Rabergman
 * Removed test for objects that are now internal.
 * 
 * 2     10/15/02 11:42a Rabergman
 * Removed Null tests because null checks were removed from the code
 * 
 * 1     9/23/02 10:45a Lakoeppe
 * Initial check in.
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Algorithms;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Algorithms
{
	/// <summary>
	/// Summary description for RobustLineIntersectorTest.
	/// </summary>
	[TestFixture]
	public class RobustLineIntersectorTest
	{
		#region Tests
		public void test_ComputeIntersectionNullTests()
		{
			//(Note: Removed Null tests because null checks were removed from the code.)
//			Coordinate point = null;
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//			RobustLineIntersector robustLineIntersector = new RobustLineIntersector();
//			bool caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection( point, linePoint1, linePoint2 );
//				Assertion.Fail("Argument null exception should have been thrown.");
//			}
//			catch(ArgumentNullException)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Null checking", true, caught );
//
//			point = new Coordinate(3.3, 3.3);
//			linePoint1 = null;
//			caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection( point, linePoint1, linePoint2 );
//				Assertion.Fail("Argument null exception should have been thrown.");
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Null checking", true, caught );
//
//			linePoint1 = linePoint2;
//			linePoint2 = null;
//			caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection( point, linePoint1, linePoint2 );
//				Assertion.Fail("Argument null exception should have been thrown.");
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Null checking", true, caught );
//
		}

		public void test_ComputeIntersection()  // Point and line
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			Coordinate point = new Coordinate( 5.0, 5.0 );
//
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//
//			RobustLineIntersector robustLineIntersector = new RobustLineIntersector();
//			robustLineIntersector.ComputeIntersection( point, linePoint1, linePoint2 );
//
//			Assertion.AssertEquals("Intersection Result:", true, robustLineIntersector.HasIntersection() );
//			Assertion.AssertEquals("Intersection Result Value:", 1, robustLineIntersector.GetIntersectionNum() );
//
//			point = new Coordinate( 4.0, 5.0 );
//			robustLineIntersector.ComputeIntersection( point, linePoint1, linePoint2 );
//			Assertion.AssertEquals("Intersection Result:", false, robustLineIntersector.HasIntersection() );
//			Assertion.AssertEquals("Intersection Result Value:", 0, robustLineIntersector.GetIntersectionNum() );

		}

		public void test_ComputeIntersection2()	// Line and Line
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			Coordinate linePoint0 = new Coordinate( 1.0, 6.0 );
//			Coordinate linePoint1 = new Coordinate( 6.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint3 = new Coordinate( 6.0, 6.0 );
//
//			RobustLineIntersector robustLineIntersector = new RobustLineIntersector();
//			robustLineIntersector.ComputeIntersection(linePoint0, linePoint1, linePoint2, linePoint3 );
//
//			Assertion.AssertEquals("Intersection Result:", true, robustLineIntersector.HasIntersection() );
//
//			linePoint0 = new Coordinate( 1.0, 2.0 );
//			linePoint1 = new Coordinate( 6.0, 7.0 );
//
//			robustLineIntersector.ComputeIntersection( linePoint0, linePoint1, linePoint2, linePoint3 );
//			Assertion.AssertEquals("Intersection Result:", false, robustLineIntersector.HasIntersection() );
//
//			linePoint0 = new Coordinate( 3.5, 3.5 );
//			linePoint1 = new Coordinate( 8.0, 8.0 );
//
//			robustLineIntersector.ComputeIntersection( linePoint0, linePoint1, linePoint2, linePoint3 );
//			Assertion.AssertEquals("Intersection Result:", true, robustLineIntersector.HasIntersection() );
//			Assertion.AssertEquals("Intersection Result:", false, robustLineIntersector.IsProper() );
//			Assertion.AssertEquals("Intersection Result Value:", 2, robustLineIntersector.GetIntersectionNum() );
		}

		public void test_ComputeIntersection2NullTest()		// Line and Line null tests
		{
			//(Note: Removed Null tests because null checks were removed from the code.)
//			// first param
//			Coordinate linePoint0 = null;
//			Coordinate linePoint1 = new Coordinate( 6.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint3 = new Coordinate( 6.0, 6.0 );
//			RobustLineIntersector robustLineIntersector = new RobustLineIntersector();
//			bool caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection(linePoint0, linePoint1, linePoint2, linePoint3 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Line Intersection Null Test:", true, caught );
//
//			// second param
//			linePoint0 = new Coordinate( 1.0, 6.0 );
//			linePoint1 = null;
//			caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection(linePoint0, linePoint1, linePoint2, linePoint3 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Line Intersection Null Test:", true, caught );
//
//			// third param
//			linePoint1 = new Coordinate( 6.0, 1.0 );
//			linePoint2 = null;
//			caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection(linePoint0, linePoint1, linePoint2, linePoint3 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Line Intersection Null Test:", true, caught );
//
//			// fourth param
//			linePoint2 = new Coordinate( 1.0, 1.0 );
//			linePoint3 = null;
//			caught = false;
//			try
//			{
//				robustLineIntersector.ComputeIntersection(linePoint0, linePoint1, linePoint2, linePoint3 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("Line Intersection Null Test:", true, caught );
		}

		public void test_ComputeEdgeDistance()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			Coordinate point = new Coordinate( 5.0, 5.0 );
//
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//
//			double dVal = RobustLineIntersector.ComputeEdgeDistance( point, linePoint1, linePoint2 );
//			Assertion.AssertEquals("Edge Distance:", 4.0, dVal );
//
		}

		public void test_ComputeEdgeDistanceNullTest()		// Point and Line null tests
		{
			//(Note: Removed Null tests because null checks were removed from the code.)
//			// first param
//			Coordinate point = null;
//
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//			bool caught = false;
//			try
//			{
//				RobustLineIntersector.ComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeEdgeDistance Null Test:", true, caught );
//
//			// second param
//			point = new Coordinate( 5.0, 5.0 );
//			linePoint1 = null;
//			caught = false;
//			try
//			{
//				RobustLineIntersector.ComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeEdgeDistance Null Test:", true, caught );
//
//			// third param
//			linePoint1 = new Coordinate( 1.0, 1.0 );
//			linePoint2 = null;
//			caught = false;
//			try
//			{
//				RobustLineIntersector.ComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeEdgeDistance Null Test:", true, caught );
//
		}

		
		
		public void test_NonRobustComputeEdgeDistance()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			Coordinate point = new Coordinate( 5.0, 5.0 );
//
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//
//			double dVal = RobustLineIntersector.NonRobustComputeEdgeDistance( point, linePoint1, linePoint2 );
//			Assertion.AssertEquals("Edge Distance:", 5.6568542494923806, dVal );
		}

		
		public void test_ComputeNonRobustEdgeDistanceNullTest()		// Point and Line null tests
		{
//(Note: Removed Null tests because null checks were removed from the code.)
//			// first param
//			Coordinate point = null;
//
//			Coordinate linePoint1 = new Coordinate( 1.0, 1.0 );
//			Coordinate linePoint2 = new Coordinate( 6.0, 6.0 );
//			bool caught = false;
//			try
//			{
//				RobustLineIntersector.NonRobustComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeNonRobustEdgeDistance Null Test:", true, caught );
//
//			// second param
//			point = new Coordinate( 5.0, 5.0 );
//			linePoint1 = null;
//			caught = false;
//			try
//			{
//				RobustLineIntersector.NonRobustComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeNonRobustEdgeDistance Null Test:", true, caught );
//
//			// third param
//			linePoint1 = new Coordinate( 1.0, 1.0 );
//			linePoint2 = null;
//			caught = false;
//			try
//			{
//				RobustLineIntersector.NonRobustComputeEdgeDistance( point, linePoint1, linePoint2 );
//
//			}
//			catch(ArgumentNullException e)
//			{
//				caught = true;
//			}
//			Assertion.AssertEquals("ComputeNonRobustEdgeDistance Null Test:", true, caught );
//
		}
		
		#endregion

	}
}

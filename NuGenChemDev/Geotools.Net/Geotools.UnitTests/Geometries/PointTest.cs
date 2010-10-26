#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/PointTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: PointTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 14    12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 13    12/09/02 11:38a Awcoats
 * changed x and y propeties to X and Y.
 * 
 * 12    11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 11    10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 10    10/04/02 4:49p Rabergman
 * 
 * 9     10/04/02 1:11p Rabergman
 * changed test_SetEmpty due to changes made during code review.
 * 
 * 8     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 7     9/25/02 12:30p Rabergman
 * Daily check in
 * 
 * 6     9/17/02 9:58a Rabergman
 * Changed Apply tests.  Removed Normalize test.
 * 
 * 5     9/09/02 1:47p Rabergman
 * Changed test_Envelope
 * 
 * 4     8/29/02 12:40p Rabergman
 * Removed comments
 * 
 * 3     8/23/02 11:53a Rabergman
 * Fixed Clone()
 * 
 * 2     8/21/02 2:49p Rabergman
 * completed test cases
 * 
 * 1     8/16/02 11:27a Rabergman
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
	/// Summary description for PointTest.
	/// </summary>
	[TestFixture]
	public class PointTest 
	{
		//set up needed variables
		Coordinate _coor = new Coordinate(1.0,2.0);
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		int _sRID = 3;

		

		public void test_constructor()
		{
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);

			//create a new point
			Point point = gf.CreatePoint(_coor);

			//Make sure the values
			Assertion.AssertEquals("Const-x: ", 1.0, point.X);
			Assertion.AssertEquals("Const-y: ", 2.0, point.Y);
		}

		public void test_Clone()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);
			//clone that point
			Point point2 = point.Clone() as Point;

			//Test that they are not the same point
			Assertion.AssertEquals("Clone-1: ", false, point==point2);

			//Test that they have the same coordinates
			Assertion.AssertEquals("Clone-2: ", true, point.X==point2.X);
			Assertion.AssertEquals("Clone-3: ", true, point.Y==point2.Y);
		}

		public void test_Coords()
		{
			PrecisionModel precisionModel = new PrecisionModel();

			//Set the coord to be empty
			Coordinate coord = new Coordinate();
			coord = null;
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(coord);
			
			//test that true is returned because the coordinates are empty.
			Assertion.AssertEquals("Coords - true: ", true, point.IsEmpty());

			//Put something into the coordinates
			Coordinate coordinate = new Coordinate(1.0,2.0);
			
			//create a new point
			point = gf.CreatePoint(coordinate);
			
			//test that false is returned because the coordinates are not empty.
			Assertion.AssertEquals("Coords - false: ", false, point.IsEmpty());
		}

		public void test_SetEmpty()
		{
//(Note: this method was removed with OGIS)
//			//create a new point
//			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
//			Point point = gf.CreatePoint(_coor);
//          Assertion.AssertEquals("SetEmpty: ", false, point.IsEmpty());
//			try
//			{
//				point.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
		}

		public void test_Envelope()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);
			Geometry geom = point.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-1: ", false, geom.IsEmpty());

			//get the coordinates out of the geometry
			Coordinates coords = geom.GetCoordinates();
			//a point is returned as the envelope of a point
			Assertion.AssertEquals("Envelope-2: ", 1.0, coords[0].X);
			Assertion.AssertEquals("Envelope-3: ", 2.0, coords[0].Y);
		}

		public void test_Extent2D()
		{
//(Note: this method was removed with OGIS)
//			//create a new point
//			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
//			Point point = gf.CreatePoint(_coor);
//			
//			double x1;
//			double y1;
//			double x2;
//			double y2;
//
//			//Set the coordinate variables to be the extents
//			point.Extent2D(out x1, out y1, out x2, out y2);
//
//			Assertion.AssertEquals("Extent2D1: ", 1.0, x1);
//			Assertion.AssertEquals("Extent2D2: ", 2.0, y1);
//			Assertion.AssertEquals("Extent2D3: ", 1.0, x2);
//			Assertion.AssertEquals("Extent2D4: ", 2.0, y2);
//
//			//Set point to be empty to test the else
//			Coordinate testCoord = null;
//			Point point2 = gf.CreatePoint(testCoord);
//
//			//Set the coordinate variables to be the extents
//			point2.Extent2D(out x1, out y1, out x2, out y2);
//            
//			//Check the values again they should all be zero now
//			Assertion.AssertEquals("Extent2D1: ", 0.0, x1);
//			Assertion.AssertEquals("Extent2D2: ", 0.0, y1);
//			Assertion.AssertEquals("Extent2D3: ", 0.0, x2);
//			Assertion.AssertEquals("Extent2D4: ", 0.0, y2);
		}

		public void test_X()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			Assertion.AssertEquals("X: ", 1.0, point.X);
		}

		public void test_Y()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			Assertion.AssertEquals("Y: ", 2.0, point.Y);
		}

		public void test_IsEmpty()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			Assertion.AssertEquals("IsEmpty1: ", false, point.IsEmpty());

			//Set point to be empty to test the else
			Coordinate testCoord = null;
			Point point2 = gf.CreatePoint(testCoord);
			
			Assertion.AssertEquals("IsEmpty2: ", true, point2.IsEmpty());
		}

		public void test_Dimension()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//check for zero - dimension always returns 0 for a point
			Assertion.AssertEquals("Dimension: ", 0, point.GetDimension());
		}

		public void test_IsSimple()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//check for true - IsSimple always returns true for a point
			Assertion.AssertEquals("IsSimple: ", true, point.IsSimple());
		}

		public void test_GeometryType()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//check for "point" - GeometryType always returns "point" for a point
			Assertion.AssertEquals("GeometryType: ", "Point", point.GetGeometryType());
		}

		public void test_Coordinates()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//returns a coordinates object
			Coordinates coordinates = point.GetCoordinates();

			//test to make sure only one coordinate is returned for a point
			Assertion.AssertEquals("Coordinates1: ", 1, coordinates.Count);
			
			foreach(Coordinate coord in coordinates)
			{
				//test to make sure that the coordinates returned are equal to the coordinates sent
				Assertion.AssertEquals("test :", true, coord.Equals(_coor));

				//testing each coordinate just to be sure
				Assertion.AssertEquals("Coordinates2: ", _coor.X, coord.X);
				Assertion.AssertEquals("Coordinates3: ", _coor.Y, coord.Y);
			}
		}

		public void test_NumPoints()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			Assertion.AssertEquals("NumPoints1: ", 1, point.GetNumPoints());

			//Create a null coordinate to test with
			Coordinate testCoord = new Coordinate();
			testCoord = null;
			
			//create a point with a null coordinate to test else case
			Point point2 = gf.CreatePoint(testCoord);

			Assertion.AssertEquals("NumPoints2: ", 0, point2.GetNumPoints());
		}

		public void test_GetBoundary()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//get the boundary of the point 
			Geometry geoColl = point.GetBoundary();

			//should return true always - a point has no boundary
			Assertion.AssertEquals("GetBoundary: ", true, geoColl.IsEmpty());
		}

		public void test_GetBoundryDimension()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);

			//GetBoundryDimension allways returns false(-1) for Point
			Assertion.AssertEquals("GetBoundryDimension: ", -1, point.GetBoundaryDimension());
		}

		public void test_Equals()
		{
			//create a new point to test for a true result
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);
			Point testPoint1 = gf.CreatePoint(_coor);

			//create a new coordinate and point to test for a false result
			Coordinate coord = new Coordinate(2.0, 1.0);
			Point testPoint2 = gf.CreatePoint(coord);

			//create a new type of geometry to test for another aoptional pest control procedure

			Assertion.AssertEquals("EqualsTrue: ", true, point.Equals(testPoint1));
			Assertion.AssertEquals("EqualsFalse: ", false, point.Equals(testPoint2));
		}

		public void test_ApplyCoordinateFilter()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);
			//CoordinateFilter filter = new CoordinateFilter();

			//todo(Ronda): Apply
			//point.Apply(filter);
		}
		
		public void test_ApplyFilterGeometryFilter()
		{
			//create a new point
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(_coor);
			//GeometryFilter filter = new GeometryFilter();

			//todo(Ronda): Apply
			//point.Apply(filter);
		}

		public void test_Normalize()
		{
		}

		public void test_CompareToSameClass()
		{
//These tests can only be run if the method CompareToSameClass is changed from protected to public
//			//create new points
//			Point point = gf.CreatePoint(_coor);
//
//			//create a new coordinate to test against
//			Coordinate coord1 = new Coordinate(0.0, 0.0);
//			Point point1 = new Point(coord1, _precMod, _sRID);
//
//			//create a new coordinate to test against
//			Coordinate coord2 = new Coordinate(1.0, 2.0);
//			Point point2 = new Point(coord2, _precMod, _sRID);
//
//			//create a new coordinate to test against
//			Coordinate coord3 = new Coordinate(2.0, 3.0);
//			Point point3 = new Point(coord3, _precMod, _sRID);
//
//			//create a new coordinate to test against
//			Coordinate coord4 = new Coordinate(0.0, 3.0);
//			Point point4 = new Point(coord4, _precMod, _sRID);		
//
//			//create a new coordinate to test against
//			Coordinate coord5 = new Coordinate(2.0, 0.0);
//			Point point5 = new Point(coord5, _precMod, _sRID);
//
//			//create a new coordinate to test against
//			Coordinate coord6 = new Coordinate(2.0, 2.0);
//			Point point6 = new Point(coord6, _precMod, _sRID);
//
//			//create a new coordinate to test against
//			Coordinate coord7 = new Coordinate(1.0, 3.0);
//			Point point7 = new Point(coord7, _precMod, _sRID);
//
//			//Compare to a point where both coordinates are less than those of the original point
//			Assertion.AssertEquals("CompareToSameClass1: ", 1, point.CompareToSameClass(point1));
//
//			//Compare to a point where both coordinates are equal to those of the original point
//			Assertion.AssertEquals("CompareToSameClass2: ", 0, point.CompareToSameClass(point2));
//
//			//Compare to a point where both coordinates are greater than those of the original point
//			Assertion.AssertEquals("CompareToSameClass3: ", -1, point.CompareToSameClass(point3));
//
//			//Compare to a point where the Y is greater than and the X is less than those of the original point
//			Assertion.AssertEquals("CompareToSameClass4: ", 1, point.CompareToSameClass(point4));
//
//			//Compare to a point where the X is greater than and the Y is less than those of the original point
//			Assertion.AssertEquals("CompareToSameClass5: ", -1, point.CompareToSameClass(point5));
//
//			//Compare to a point where the Y is greater than and the X is equal to those of the original point
//			Assertion.AssertEquals("CompareToSameClass6: ", -1, point.CompareToSameClass(point6));
//
//			//Compare to a point where the X is greater than and the Y is equal to those of the original point
//			Assertion.AssertEquals("CompareToSameClass7: ", -1, point.CompareToSameClass(point7));
		}

		public void  test_ComputeEnvelopeInternal()
		{
//These tests can only be run if the method ComputeEnvelopeInternal is changed from protected to public
//			//create new points
//			Point point = gf.CreatePoint(_coor);
//
//			Envelope env = point.ComputeEnvelopeInternal();
//
//			//the max x value for a point should equal the x value
//            Assertion.AssertEquals("ComputeEnvInt1: ", 1.0, env.MaxX);
//			//the min x value for a point should equal the x value
//			Assertion.AssertEquals("ComputeEnvInt2: ", 1.0, env.MinX);
//
//			//the max y value for a point should equal the y value
//			Assertion.AssertEquals("ComputeEnvInt3: ", 2.0, env.MaxY);
//			//the min y value for a point should equal the y value
//			Assertion.AssertEquals("ComputeEnvInt4: ", 2.0, env.MinY);
//
//			Point point2 = new Point(null, _precMod, _sRID);
//
//			Envelope env2 = point2.ComputeEnvelopeInternal();
//
//			//the max x value for a point with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt5: ", 0.0, env2.MaxX);
//			//the min x value for a point with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt6: ", 0.0, env2.MinX);
//
//			//the max y value for a point with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt7: ", 0.0, env2.MaxY);
//			//the min y value for a point with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt8: ", 0.0, env2.MinY);
		}

	}
}

#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/MultiPointTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: MultiPointTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 13    12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 12    12/09/02 11:38a Awcoats
 * changed x and y propeties to X and Y.
 * 
 * 11    11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 10    10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 9     10/08/02 12:25p Rabergman
 * Removed Project test
 * 
 * 8     10/04/02 4:49p Rabergman
 * 
 * 7     10/04/02 1:11p Rabergman
 * changed test_SetEmpty due to changes made during code review.
 * 
 * 6     10/04/02 9:00a Rabergman
 * Changed Coordinates to points
 * 
 * 5     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 4     9/27/02 3:04p Rabergman
 * Daily check in
 * 
 * 3     9/25/02 3:31p Rabergman
 * implemented test_geometry & added to test_envelope
 * 
 * 2     9/25/02 12:30p Rabergman
 * Daily check in
 * 
 * 1     9/17/02 3:07p Rabergman
 * 
 * 3     9/09/02 1:50p Rabergman
 * Added _ to front of member variable.  Implemented test_Clone &
 * test_GemoetryType
 * 
 * 2     9/05/02 2:26p Rabergman
 * added several test cases
 * 
 * 1     8/21/02 2:50p Rabergman
 * 
 */ 
#endregion

#region Using
using System;
using System.Collections;
using NUnit.Framework;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Geometries
{
	/// <summary>
	/// Summary description for MultiPointTest.
	/// </summary>
	[TestFixture]
	public class MultiPointTest 
	{
		//set up needed variables
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		int _sRID = 3;
		Point[] _coords = new Point[]{};

		

		/// <summary>
		/// Method to create a MultiPoint for testing purposes
		/// </summary>
		/// <returns>A MultiPoint</returns>
		private MultiPoint CreateTester1()
		{
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);

			Point[] points = new Point[23];

			for(int i = 0; i < 12; i++)
			{
				points[i] = gf.CreatePoint(new Coordinate(i, i));
			}
			points[12] = gf.CreatePoint(new Coordinate(11, 12));
			points[13] = gf.CreatePoint(new Coordinate(10, 13));
			points[14] = gf.CreatePoint(new Coordinate(9, 14));
			points[15] = gf.CreatePoint(new Coordinate(8, 15));
			points[16] = gf.CreatePoint(new Coordinate(9, 16));
			points[17] = gf.CreatePoint(new Coordinate(10, 17));
			points[18] = gf.CreatePoint(new Coordinate(11, 18));
			points[19] = gf.CreatePoint(new Coordinate(12, 19));
			points[20] = gf.CreatePoint(new Coordinate(11, 20));
			points[21] = gf.CreatePoint(new Coordinate(10, 21));
			points[22] = gf.CreatePoint(new Coordinate(9, 22));

			MultiPoint mp = gf.CreateMultiPoint(points);
			return mp;
		}

		/// <summary>
		/// Method to create a MultiPoint for testing purposes
		/// </summary>
		/// <returns>A MultiPoint</returns>
		private MultiPoint CreateTester2()
		{
			Point[] points = new Point[18];

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);

			points[0] = gf.CreatePoint(new Coordinate(2, 2));
			points[1] = gf.CreatePoint(new Coordinate(3, 3));
			points[2] = gf.CreatePoint(new Coordinate(4, 4));
			points[3] = gf.CreatePoint(new Coordinate(5, 5));
			points[4] = gf.CreatePoint(new Coordinate(6, 6));
			points[5] = gf.CreatePoint(new Coordinate(7, 7));
			points[6] = gf.CreatePoint(new Coordinate(8, 8));
			points[7] = gf.CreatePoint(new Coordinate(9, 7));
			points[8] = gf.CreatePoint(new Coordinate(10, 6));
			points[9] = gf.CreatePoint(new Coordinate(10, 5));
			points[10] = gf.CreatePoint(new Coordinate(9, 4));
			points[11] = gf.CreatePoint(new Coordinate(8, 3));
			points[12] = gf.CreatePoint(new Coordinate(7, 4));
			points[13] = gf.CreatePoint(new Coordinate(7, 5));
			points[14] = gf.CreatePoint(new Coordinate(6, 6));
			points[15] = gf.CreatePoint(new Coordinate(5, 7));
			points[16] = gf.CreatePoint(new Coordinate(4, 8));
			points[17] = gf.CreatePoint(new Coordinate(3, 9));

			MultiPoint mp = gf.CreateMultiPoint(points);
			return mp;
		}

		/// <summary>
		/// Method to create a MultiPoint for testing purposes
		/// </summary>
		/// <returns>A MultiPoint</returns>
		private MultiPoint CreateTester3()
		{
			Point[] points = new Point[21];

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);

			points[0] = gf.CreatePoint(new Coordinate(10, 13));
			points[1] = gf.CreatePoint(new Coordinate(11, 13));
			points[2] = gf.CreatePoint(new Coordinate(12, 13));
			points[3] = gf.CreatePoint(new Coordinate(13, 14));
			points[4] = gf.CreatePoint(new Coordinate(14, 15));
			points[5] = gf.CreatePoint(new Coordinate(15, 16));
			points[6] = gf.CreatePoint(new Coordinate(15, 17));
			points[7] = gf.CreatePoint(new Coordinate(15, 18));
			points[8] = gf.CreatePoint(new Coordinate(14, 19));
			points[9] = gf.CreatePoint(new Coordinate(13, 20));
			points[10] = gf.CreatePoint(new Coordinate(12, 21));
			points[11] = gf.CreatePoint(new Coordinate(11, 21));
			points[12] = gf.CreatePoint(new Coordinate(10, 21));
			points[13] = gf.CreatePoint(new Coordinate(9, 20));
			points[14] = gf.CreatePoint(new Coordinate(8, 19));
			points[15] = gf.CreatePoint(new Coordinate(7, 18));
			points[16] = gf.CreatePoint(new Coordinate(7, 17));
			points[17] = gf.CreatePoint(new Coordinate(7, 16));
			points[18] = gf.CreatePoint(new Coordinate(8, 15));
			points[19] = gf.CreatePoint(new Coordinate(9, 14));
			points[20] = gf.CreatePoint(new Coordinate(10, 13));

			MultiPoint mp = gf.CreateMultiPoint(points);
			return mp;
		}

		/// <summary>
		/// Method to create a MultiPoint for testing purposes
		/// </summary>
		/// <returns>A MultiPoint</returns>
		private MultiPoint CreateTester4()
		{
			Point[] points = new Point[32];

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);

			points[0] = gf.CreatePoint(new Coordinate(2, 2));
			points[1] = gf.CreatePoint(new Coordinate(3, 1));
			points[2] = gf.CreatePoint(new Coordinate(4, 2));
			points[3] = gf.CreatePoint(new Coordinate(5, 3));
			points[4] = gf.CreatePoint(new Coordinate(6, 4));
			points[5] = gf.CreatePoint(new Coordinate(7, 5));
			points[6] = gf.CreatePoint(new Coordinate(7, 6));
			points[7] = gf.CreatePoint(new Coordinate(7, 7));
			points[8] = gf.CreatePoint(new Coordinate(7, 8));
			points[9] = gf.CreatePoint(new Coordinate(7, 9));
			points[10] = gf.CreatePoint(new Coordinate(6, 10));
			points[11] = gf.CreatePoint(new Coordinate(5, 11));
			points[12] = gf.CreatePoint(new Coordinate(6, 12));
			points[13] = gf.CreatePoint(new Coordinate(7, 13));
			points[14] = gf.CreatePoint(new Coordinate(8, 14));
			points[15] = gf.CreatePoint(new Coordinate(9, 13));
			points[16] = gf.CreatePoint(new Coordinate(10, 12));
			points[17] = gf.CreatePoint(new Coordinate(10, 11));
			points[18] = gf.CreatePoint(new Coordinate(10, 10));
			points[19] = gf.CreatePoint(new Coordinate(10, 9));
			points[20] = gf.CreatePoint(new Coordinate(9, 8));
			points[21] = gf.CreatePoint(new Coordinate(8, 7));
			points[22] = gf.CreatePoint(new Coordinate(7, 7));
			points[23] = gf.CreatePoint(new Coordinate(6, 7));
			points[24] = gf.CreatePoint(new Coordinate(5, 8));
			points[25] = gf.CreatePoint(new Coordinate(4, 8));
			points[26] = gf.CreatePoint(new Coordinate(3, 7));
			points[27] = gf.CreatePoint(new Coordinate(2, 6));
			points[28] = gf.CreatePoint(new Coordinate(1, 5));
			points[29] = gf.CreatePoint(new Coordinate(2, 4));
			points[30] = gf.CreatePoint(new Coordinate(1, 3));
			points[31] = gf.CreatePoint(new Coordinate(2, 2));

			MultiPoint mp = gf.CreateMultiPoint(points);
			return mp;
		}

		public void test_Constructor()
		{
			//create a MultiPoint
			MultiPoint mp1 = CreateTester1();
			Assertion.AssertEquals("Constructor-1: ", false, mp1.IsEmpty());
			Assertion.AssertEquals("Constructor-1a: ", 23, mp1.GetNumPoints());

			//create a MultiPoint
			MultiPoint mp2 = CreateTester2();
			Assertion.AssertEquals("Constructor-2: ", false, mp2.IsEmpty());
			Assertion.AssertEquals("Constructor-2a: ", 18, mp2.GetNumPoints());

			//create a MultiPoint
			MultiPoint mp3 = CreateTester3();
			Assertion.AssertEquals("Constructor-3: ", false, mp3.IsEmpty());
			Assertion.AssertEquals("Constructor-3a: ", 21, mp3.GetNumPoints());

			//create a MultiPoint
			MultiPoint mp4 = CreateTester4();
			Assertion.AssertEquals("Constructor-4: ", false, mp4.IsEmpty());
			Assertion.AssertEquals("Constructor-4a: ", 32, mp4.GetNumPoints());
		}

		public void test_Clone()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();
			//clone that multipoint
			MultiPoint mp2 = mp.Clone() as MultiPoint;

			//Test that they are not the same multipoint
			Assertion.AssertEquals("Clone-1: ", false, mp==mp2);

			//Test that they have the same coordinates
			for(int i = 0; i < mp.GetNumGeometries(); i++)
			{
				Assertion.AssertEquals("Clone-2: ", true, mp.GetCoordinate(i).X.Equals(mp2.GetCoordinate(i).X));
				Assertion.AssertEquals("Clone-3: ", true, mp.GetCoordinate(i).Y.Equals(mp2.GetCoordinate(i).Y));
			}
		}

		public void test_geometry()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			Point point = mp.GetGeometryN(2) as Point;

			Assertion.AssertEquals("geometry-1: ", 2.0, point.X);
			Assertion.AssertEquals("geometry-1: ", 2.0, point.Y);
		}

		public void test_SetEmpty()
		{
//(Note: this method was removed with OGIS)
//			//create a new multipoint
//			MultiPoint mp = CreateTester1();
//
//			Assertion.AssertEquals("SetEmpty-1: ", false, mp.IsEmpty());
//			try
//			{
//				mp.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
		}

		public void test_Envelope()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();
			Geometry geom = mp.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-1: ", false, geom.IsEmpty());

			//get the coordinates out of the geometry
			Coordinates coords = geom.GetCoordinates();
			//check the first set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-2: ", 0.0, coords[0].X);
			Assertion.AssertEquals("Envelope-3: ", 0.0, coords[0].Y);

			//check the second set of coordinates (maxX, minY)
			Assertion.AssertEquals("Envelope-4: ", 12.0, coords[1].X);
			Assertion.AssertEquals("Envelope-5: ", 0.0, coords[1].Y);

			//check the third set of coordinates (maxX, maxY)
			Assertion.AssertEquals("Envelope-6: ", 12.0, coords[2].X);
			Assertion.AssertEquals("Envelope-7: ", 22.0, coords[2].Y);

			//check the forth set of coordinates (minX, maxY)
			Assertion.AssertEquals("Envelope-8: ", 0.0, coords[3].X);
			Assertion.AssertEquals("Envelope-9: ", 22.0, coords[3].Y);

			//check the fifth set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-10: ", 0.0, coords[4].X);
			Assertion.AssertEquals("Envelope-11: ", 0.0, coords[4].Y);

			//create a new multipoint
			mp = CreateTester3();
			geom = mp.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-12: ", false, geom.IsEmpty());

			//get the coordinates out of the geometry
			coords = geom.GetCoordinates();
			//check the first set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-13: ", 7.0, coords[0].X);
			Assertion.AssertEquals("Envelope-14: ", 13.0, coords[0].Y);

			//check the second set of coordinates (maxX, minY)
			Assertion.AssertEquals("Envelope-15: ", 15.0, coords[1].X);
			Assertion.AssertEquals("Envelope-16: ", 13.0, coords[1].Y);

			//check the third set of coordinates (maxX, maxY)
			Assertion.AssertEquals("Envelope-17: ", 15.0, coords[2].X);
			Assertion.AssertEquals("Envelope-18: ", 21.0, coords[2].Y);

			//check the forth set of coordinates (minX, maxY)
			Assertion.AssertEquals("Envelope-19: ", 7.0, coords[3].X);
			Assertion.AssertEquals("Envelope-20: ", 21.0, coords[3].Y);

			//check the fifth set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-21: ", 7.0, coords[4].X);
			Assertion.AssertEquals("Envelope-22: ", 13.0, coords[4].Y);

			coords = new Coordinates();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			coords = new Coordinates();
			mp = gf.CreateMultiPoint(coords);
			geom = mp.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-23: ", true, geom.IsEmpty());
		}

		public void test_Extent2D()
		{
//(Note: this method was removed with OGIS)
//			//create a new multipoint
//			MultiPoint mp = CreateTester2();
//			
//			double x1;
//			double y1;
//			double x2;
//			double y2;
//
//			//Set the coordinate variables to be the extents
//			mp.Extent2D(out x1, out y1, out x2, out y2);
//
//			Assertion.AssertEquals("Extent2D1: ", 2.0, x1);
//			Assertion.AssertEquals("Extent2D2: ", 2.0, y1);
//			Assertion.AssertEquals("Extent2D3: ", 10.0, x2);
//			Assertion.AssertEquals("Extent2D4: ", 9.0, y2);
//
//			//Set mp to be empty to test the else
//			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
//			Coordinates coords = new Coordinates();
//			MultiPoint mp2 = gf.CreateMultiPoint(coords);
//
//			//Set the coordinate variables to be the extents
//			mp2.Extent2D(out x1, out y1, out x2, out y2);
//            
//			//Check the values again they should all be zero now
//			Assertion.AssertEquals("Extent2D1: ", 0.0, x1);
//			Assertion.AssertEquals("Extent2D2: ", 0.0, y1);
//			Assertion.AssertEquals("Extent2D3: ", 0.0, x2);
//			Assertion.AssertEquals("Extent2D4: ", 0.0, y2);
		}

		public void test_Project()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			//TODO: Not Implemented Yet
			//Assertion.AssertEquals("Project: ", null, mp.Project(null));
		}
        
		public void test_NumGeometries()
		{
			//create a new multipoint
			MultiPoint mp1 = CreateTester1();
			//create a new multipoint
			MultiPoint mp2 = CreateTester2();
			//create a new multipoint
			MultiPoint mp3 = CreateTester3();
			//create a new multipoint
			MultiPoint mp4 = CreateTester4();

			Assertion.AssertEquals("NumGeometries-1: ", 23, mp1.GetNumGeometries());
			Assertion.AssertEquals("NumGeometries-2: ", 18, mp2.GetNumGeometries());
			Assertion.AssertEquals("NumGeometries-3: ", 21, mp3.GetNumGeometries());
			Assertion.AssertEquals("NumGeometries-4: ", 32, mp4.GetNumGeometries());
		}

		public void test_IsEmpty()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			Assertion.AssertEquals("IsEmpty1: ", false, mp.IsEmpty());

			//Set multipoint to be empty to test the else
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Coordinates coords = new Coordinates();
			MultiPoint mp2 = gf.CreateMultiPoint(coords);
			
			Assertion.AssertEquals("IsEmpty2: ", true, mp2.IsEmpty());
		}

		public void test_Dimension()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			//check for zero - dimension always returns 0 for a multipoint
			Assertion.AssertEquals("Dimension: ", 0, mp.GetDimension());
		}

		public void test_IsSimple()
		{
			//create a new multipoint
			MultiPoint mp1 = CreateTester1();
			//create a new multipoint
			MultiPoint mp2 = CreateTester2();
			//create a new multipoint
			MultiPoint mp3 = CreateTester3();
			//create a new multipoint
			MultiPoint mp4 = CreateTester4();

			Assertion.AssertEquals("IsSimple-1: ", true, mp1.IsSimple());
			Assertion.AssertEquals("IsSimple-2: ", false, mp2.IsSimple());
			Assertion.AssertEquals("IsSimple-3: ", false, mp3.IsSimple());
			Assertion.AssertEquals("IsSimple-4: ", false, mp4.IsSimple());
		}

		public void test_CoordinateN()
		{
			//create a new multipoint
			MultiPoint mp1 = CreateTester1();
			Coordinate coord = new Coordinate(2.0, 2.0);

			Assertion.AssertEquals("CoordinateN-1: ", coord, mp1.GetCoordinate(2));
			coord = new Coordinate(5.0, 5.0);
			Assertion.AssertEquals("CoordinateN-2: ", coord, mp1.GetCoordinate(5));
			coord = new Coordinate(11.0, 12.0);
			Assertion.AssertEquals("CoordinateN-3: ", coord, mp1.GetCoordinate(12));
			coord = new Coordinate(11.0, 20.0);
			Assertion.AssertEquals("CoordinateN-4: ", coord, mp1.GetCoordinate(20));
		}

		public void test_GeometryType()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			//check for "multipoint" - GeometryType always returns "multipoint" for a multipoint
			Assertion.AssertEquals("GeometryType: ", "MultiPoint", mp.GetGeometryType());
		}

		public void test_Coordinates()
		{
			MultiPoint mp1 = CreateTester1();
			Coordinates coords = mp1.GetCoordinates();

			for(int i = 0; i < coords.Count; i++)
			{
				//Assertion.AssertEquals("Coordinates-1: ", true, coords[i].Equals(_coords[i]));
			}
		}

		public void test_NumPoints()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			Assertion.AssertEquals("NumPoints1: ", 23, mp.GetNumPoints());

			//Create a null coordinate to test with
			Coordinates testCoords = new Coordinates();
			
			//create a multipoint with a null coordinate to test else case
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			testCoords = new Coordinates();
			MultiPoint mp2 = gf.CreateMultiPoint(testCoords);

			Assertion.AssertEquals("NumPoints2: ", 0, mp2.GetNumPoints());
		}

		public void test_GetBoundary()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			//get the boundary of the multipoint 
			Geometry geoColl = mp.GetBoundary();

			//should return true always - a multipoint has no boundary
			Assertion.AssertEquals("GetBoundary: ", true, geoColl.IsEmpty());
		}

		public void test_GetBoundryDimension()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();

			//GetBoundryDimension allways returns false(-1) for MultiPoint
			Assertion.AssertEquals("GetBoundryDimension: ", -1, mp.GetBoundaryDimension());
		}

		public void test_Equals()
		{
			//create a new multipoint to test for a true result
			MultiPoint mp = CreateTester1();
			MultiPoint testPoints1 = CreateTester1();

			//create a new multipoint to test for a false result
			MultiPoint testPoints2 = CreateTester2();

			//create a new type of geometry to test for another aoptional pest control procedure

			Assertion.AssertEquals("EqualsTrue: ", true, mp.Equals(testPoints1));
			Assertion.AssertEquals("EqualsFalse: ", false, mp.Equals(testPoints2));
		}

		public void test_ApplyCoordinateFilter()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();
			//CoordinateFilter filter = new CoordinateFilter();

			//todo(Ronda): Apply
			//mp.Apply(filter);
		}
		
		public void test_ApplyFilterGeometryFilter()
		{
			//create a new multipoint
			MultiPoint mp = CreateTester1();
			//GeometryFilter filter = new GeometryFilter();

			//todo(Ronda): Apply
			//mp.Apply(filter);
		}

		public void test_Normalize()
		{
		}

		public void test_CompareToSameClass()
		{
//These tests can only be run if the method CompareToSameClass is changed from protected to public
			//create new points
			MultiPoint mp = CreateTester3();

			//create a new multipoint to test against
			MultiPoint mp1 = CreateTester3();

			//create a new multipoint to test against
			MultiPoint mp2 = CreateTester1();

			//create a new multipoint to test against
			MultiPoint mp3 = CreateTester2();

			//create a new multipoint to test against
			MultiPoint mp4 = CreateTester4();		

			//Compare to a multipoint that is equal to the original multipoint
			Assertion.AssertEquals("CompareToSameClass1: ", 0, mp.CompareToSameClass(mp1));

			//Compare to a multipoint that is greater than the original multipoint
			Assertion.AssertEquals("CompareToSameClass2: ", -1, mp.CompareToSameClass(mp2));

			//Compare to a multipoint that is less than the original multipoint
			Assertion.AssertEquals("CompareToSameClass3: ", 1, mp.CompareToSameClass(mp3));

			//Compare to a multipoint that is greater than the original multipoint
			Assertion.AssertEquals("CompareToSameClass3: ", -1, mp.CompareToSameClass(mp4));
		}

		public void  test_ComputeEnvelopeInternal()
		{
//These tests can only be run if the method ComputeEnvelopeInternal is changed from protected to public
//			//create new points
//			MultiPoint mp = CreateTester1();
//
//			Envelope env = mp.ComputeEnvelopeInternal();
//
//			//the max x value for the multipoint
//            Assertion.AssertEquals("ComputeEnvInt1: ", 12.0, env.MaxX);
//			//the min y value for the multipoint
//			Assertion.AssertEquals("ComputeEnvInt2: ", 1.0, env.MinX);
//
//			//the max x value for the multipoint
//			Assertion.AssertEquals("ComputeEnvInt3: ", 22.0, env.MaxY);
//			//the min y value for the multipoint
//			Assertion.AssertEquals("ComputeEnvInt4: ", 1.0, env.MinY);
//
//			MultiPoint mp2 = gf.CreateMultiPoint(null);
//
//			Envelope env2 = mp2.ComputeEnvelopeInternal();
//
//			//the max x value for a multipoint with null coordinates should always return -1
//			Assertion.AssertEquals("ComputeEnvInt5: ", -1.0, env2.MaxX);
//			//the min x value for a multipoint with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt6: ", 0.0, env2.MinX);
//
//			//the max y value for a multipoint with null coordinates should always return -1
//			Assertion.AssertEquals("ComputeEnvInt7: ", -1.0, env2.MaxY);
//			//the min y value for a multipoint with null coordinates should always return zero
//			Assertion.AssertEquals("ComputeEnvInt8: ", 0.0, env2.MinY);
		}
	}
}


#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/LineStringTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: LineStringTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 15    12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 14    12/09/02 11:38a Awcoats
 * changed x and y propeties to X and Y.
 * 
 * 13    11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 12    10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 11    10/15/02 10:54a Rabergman
 * Uncommented previously broken tests
 * 
 * 10    10/04/02 4:49p Rabergman
 * 
 * 9     10/04/02 1:10p Rabergman
 * changed test_SetEmpty due to changes made during code review.
 * 
 * 8     10/03/02 4:01p Rabergman
 * Commented out some broken tests until IsSimple is fixed.
 * Removed test_ComputeEnvelopeInternal.
 * 
 * 7     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 6     9/27/02 3:04p Rabergman
 * Daily check in
 * 
 * 5     9/25/02 12:30p Rabergman
 * Daily check in
 * 
 * 4     9/17/02 9:57a Rabergman
 * Added test for: SetEmpty, Point, Extent, IsEmpty, Dimension.
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
	/// Summary description for LineStringTest.
	/// </summary>
	[TestFixture]
	public class LineStringTest
	{
		//set up needed variables
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		int _sRID = 3;
		Coordinates _coords = new Coordinates();

		

		/// <summary>
		/// Method to create a Simple NonClosed Linestring for testing purposes
		/// </summary>
		/// <returns>A lineString</returns>
		private LineString SimpleOpen()
		{
			_coords = new Coordinates();
			Coordinate coord = new Coordinate(0.0,0.0);
			for(int i = 1; i < 12; i++)
			{
				coord = new Coordinate((double)i, (double)i);
				_coords.Add(coord);
			}
			coord = new Coordinate(11, 12);
			_coords.Add(coord);
			coord = new Coordinate(10, 13);
			_coords.Add(coord);
			coord = new Coordinate(9, 14);
			_coords.Add(coord);
			coord = new Coordinate(8, 15);
			_coords.Add(coord);
			coord = new Coordinate(9, 16);
			_coords.Add(coord);
			coord = new Coordinate(10, 17);
			_coords.Add(coord);
			coord = new Coordinate(11, 18);
			_coords.Add(coord);
			coord = new Coordinate(12, 19);
			_coords.Add(coord);
			coord = new Coordinate(11, 20);
			_coords.Add(coord);
			coord = new Coordinate(10, 21);
			_coords.Add(coord);
			coord = new Coordinate(9, 22);
			_coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(_coords);
			return ls;
		}

		/// <summary>
		/// Method to create a NonSimple NonClosed Linestring for testing purposes
		/// </summary>
		/// <returns>A lineString</returns>
		private LineString NonSimpleOpen()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(0, 0);

			coord = new Coordinate(2, 2);
			coords.Add(coord);
			coord = new Coordinate(3, 3);
			coords.Add(coord);
			coord = new Coordinate(4, 4);
			coords.Add(coord);
			coord = new Coordinate(5, 5);
			coords.Add(coord);
			coord = new Coordinate(6, 6);
			coords.Add(coord);
			coord = new Coordinate(7, 7);
			coords.Add(coord);
			coord = new Coordinate(8, 8);
			coords.Add(coord);
			coord = new Coordinate(9, 7);
			coords.Add(coord);
			coord = new Coordinate(10, 6);
			coords.Add(coord);
			coord = new Coordinate(10, 5);
			coords.Add(coord);
			coord = new Coordinate(9, 4);
			coords.Add(coord);
			coord = new Coordinate(8, 3);
			coords.Add(coord);
			coord = new Coordinate(7, 4);
			coords.Add(coord);
			coord = new Coordinate(7, 5);
			coords.Add(coord);
			coord = new Coordinate(6, 6);
			coords.Add(coord);
			coord = new Coordinate(5, 7);
			coords.Add(coord);
			coord = new Coordinate(4, 8);
			coords.Add(coord);
			coord = new Coordinate(3, 9);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords);
			return ls;
		}

		/// <summary>
		/// Method to create a Simple Closed Linestring for testing purposes
		/// </summary>
		/// <returns>A lineString</returns>
		private LineString SimpleClosed()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord ;//Coordinate(0, 0);

			coord = new Coordinate(10, 13);
			coords.Add(coord);
			coord = new Coordinate(11, 13);
			coords.Add(coord);
			coord = new Coordinate(12, 13);
			coords.Add(coord);
			coord = new Coordinate(13, 14);
			coords.Add(coord);
			coord = new Coordinate(14, 15);
			coords.Add(coord);
			coord = new Coordinate(15, 16);
			coords.Add(coord);
			coord = new Coordinate(15, 17);
			coords.Add(coord);
			coord = new Coordinate(15, 18);
			coords.Add(coord);
			coord = new Coordinate(14, 19);
			coords.Add(coord);
			coord = new Coordinate(13, 20);
			coords.Add(coord);
			coord = new Coordinate(12, 21);
			coords.Add(coord);
			coord = new Coordinate(11, 21);
			coords.Add(coord);
			coord = new Coordinate(10, 21);
			coords.Add(coord);
			coord = new Coordinate(9, 20);
			coords.Add(coord);
			coord = new Coordinate(8, 19);
			coords.Add(coord);
			coord = new Coordinate(7, 18);
			coords.Add(coord);
			coord = new Coordinate(7, 17);
			coords.Add(coord);
			coord = new Coordinate(7, 16);
			coords.Add(coord);
			coord = new Coordinate(8, 15);
			coords.Add(coord);
			coord = new Coordinate(9, 14);
			coords.Add(coord);
			coord = new Coordinate(10, 13);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords);
			return ls;
		}

		/// <summary>
		/// Method to create a NonSimple Closed Linestring for testing purposes
		/// </summary>
		/// <returns>A lineString</returns>
		private LineString NonSimpleClosed()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(0, 0);

			coord = new Coordinate(2, 2);
			coords.Add(coord);
			coord = new Coordinate(3, 1);
			coords.Add(coord);
			coord = new Coordinate(4, 2);
			coords.Add(coord);
			coord = new Coordinate(5, 3);
			coords.Add(coord);
			coord = new Coordinate(6, 4);
			coords.Add(coord);
			coord = new Coordinate(7, 5);
			coords.Add(coord);
			coord = new Coordinate(7, 6);
			coords.Add(coord);
			coord = new Coordinate(7, 7);
			coords.Add(coord);
			coord = new Coordinate(7, 8);
			coords.Add(coord);
			coord = new Coordinate(7, 9);
			coords.Add(coord);
			coord = new Coordinate(6, 10);
			coords.Add(coord);
			coord = new Coordinate(5, 11);
			coords.Add(coord);
			coord = new Coordinate(6, 12);
			coords.Add(coord);
			coord = new Coordinate(7, 13);
			coords.Add(coord);
			coord = new Coordinate(8, 14);
			coords.Add(coord);
			coord = new Coordinate(9, 13);
			coords.Add(coord);
			coord = new Coordinate(10, 12);
			coords.Add(coord);
			coord = new Coordinate(10, 11);
			coords.Add(coord);
			coord = new Coordinate(10, 10);
			coords.Add(coord);
			coord = new Coordinate(10, 9);
			coords.Add(coord);
			coord = new Coordinate(9, 8);
			coords.Add(coord);
			coord = new Coordinate(8, 7);
			coords.Add(coord);
			coord = new Coordinate(7, 7);
			coords.Add(coord);
			coord = new Coordinate(6, 7);
			coords.Add(coord);
			coord = new Coordinate(5, 8);
			coords.Add(coord);
			coord = new Coordinate(4, 8);
			coords.Add(coord);
			coord = new Coordinate(3, 7);
			coords.Add(coord);
			coord = new Coordinate(2, 6);
			coords.Add(coord);
			coord = new Coordinate(1, 5);
			coords.Add(coord);
			coord = new Coordinate(2, 4);
			coords.Add(coord);
			coord = new Coordinate(1, 3);
			coords.Add(coord);
			coord = new Coordinate(2, 2);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords);
			return ls;
		}

		private LineString ThrowsException()
		{
			Coordinates coords = new Coordinates();

			Coordinate coord = new Coordinate(1,1);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords);

			return ls;
		}

		public void test_Constructor()
		{
			//create a simple nonclosed linestring
			LineString lsSO = SimpleOpen();
			Assertion.AssertEquals("Constructor-1: ", false, lsSO.IsEmpty());

			//create a nonsimple nonclosed linestring
			LineString lsNSO = NonSimpleOpen();
			Assertion.AssertEquals("Constructor-2: ", false, lsNSO.IsEmpty());

			//create a simple closed linestring
			LineString lsSC = SimpleClosed();
			Assertion.AssertEquals("Constructor-3: ", false, lsSC.IsEmpty());

			//create a nonsimple closed linestring
			LineString lsNSC = NonSimpleClosed();
			Assertion.AssertEquals("Constructor-4: ", false, lsNSC.IsEmpty());

			try
			{
				//create a linestring with null elements
				LineString ls = ThrowsException();
				Assertion.Fail("should never reach here");
			}
			catch(ArgumentException)
			{
			}
		}

		#region Test Implementation of ILineString methods

		public void test_Clone()
		{
			LineString ls = SimpleOpen();
			LineString ls2 = ls.Clone() as LineString;

			Assertion.AssertEquals("Clone: ", true, ls.Equals(ls2));
		}

		public void test_SetEmpty()
		{
//(Note: this method was removed with OGIS)
//			LineString ls = SimpleClosed();
//			//make sure this linestring is not empty
//			Assertion.AssertEquals("SetEmpty-1: ", false, ls.IsEmpty());
//			try
//			{
//				ls.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
		}

		public void test_Point()
		{
			LineString ls = SimpleOpen();

            Point point = ls.GetPointN(4);

            Assertion.AssertEquals("Point-1: ", 5.0, point.X);
			Assertion.AssertEquals("Point-2: ", 5.0, point.Y);
		}

		public void test_EndPoint()
		{
			LineString ls = SimpleOpen();
			Coordinate coord = new Coordinate(0,0);
			Coordinate coord2 = new Coordinate(9.0, 22.0);
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(coord);
			Point point2 = gf.CreatePoint(coord2);
			Point point3 = gf.CreatePoint(coord);

			point = ls.GetEndPoint() as Point;

			Assertion.AssertEquals("EndPoint-1: ", true, point.Equals(point2));
			Assertion.AssertEquals("EndPoint-2: ", false, point.Equals(point3));
		}

		public void test_Envelope()
		{
			LineString ls = SimpleOpen();

			//put the envelope into a geometry
			Geometry geom = ls.GetEnvelope() as Geometry;

			//make sure there is something in the geometry
			Assertion.AssertEquals("Envelope-1: ", false, geom.IsEmpty());
            
			//get the coordinates out of the geometry
			Coordinates coords = geom.GetCoordinates();

			//check the first set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-2: ", 1.0, coords[0].X);
			Assertion.AssertEquals("Envelope-3: ", 1.0, coords[0].Y);

			//check the second set of coordinates (maxX, minY)
			Assertion.AssertEquals("Envelope-4: ", 12.0, coords[1].X);
			Assertion.AssertEquals("Envelope-5: ", 1.0, coords[1].Y);

			//check the third set of coordinates (maxX, maxY)
			Assertion.AssertEquals("Envelope-6: ", 12.0, coords[2].X);
			Assertion.AssertEquals("Envelope-7: ", 22.0, coords[2].Y);

			//check the forth set of coordinates (minX, maxY)
			Assertion.AssertEquals("Envelope-8: ", 1.0, coords[3].X);
			Assertion.AssertEquals("Envelope-9: ", 22.0, coords[3].Y);

			//check the fifth set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-10: ", 1.0, coords[4].X);
			Assertion.AssertEquals("Envelope-11: ", 1.0, coords[4].Y);

			Coordinates coords2 = new Coordinates();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls2 = gf.CreateLineString(coords2);
			geom = ls2.GetEnvelope() as Geometry;

			//make sure there is nothing in the geometry
			Assertion.AssertEquals("Envelope-12: ", true, geom.IsEmpty());
		}

		public void test_Extent2D()
		{
//(Note: this method was removed with OGIS)
//			//set up needed variables
//			double x1;
//			double y1;
//			double x2;
//			double y2;
//
//			//check extents on a simple open linestring
//			LineString ls1 = SimpleOpen();
//
//			//Get the extents
//			ls1.Extent2D(out x1, out y1, out x2, out y2);
//
//			//check to be sure the proper extents are returned
//			Assertion.AssertEquals("Extent2D-1: ", 1.0, x1);
//			Assertion.AssertEquals("Extent2D-2: ", 1.0, y1);
//			Assertion.AssertEquals("Extent2D-3: ", 12.0, x2);
//			Assertion.AssertEquals("Extent2D-4: ", 22.0, y2);
//
//			//check extents on a nonsimple closed linestring
//			LineString ls2 = NonSimpleClosed();
//
//			//Get the extents
//			ls2.Extent2D(out x1, out y1, out x2, out y2);
//
//			//check to be sure the proper extents are returned
//			Assertion.AssertEquals("Extent2D-5: ", 1.0, x1);
//			Assertion.AssertEquals("Extent2D-6: ", 1.0, y1);
//			Assertion.AssertEquals("Extent2D-7: ", 10.0, x2);
//			Assertion.AssertEquals("Extent2D-8: ", 14.0, y2);
		}

		public void test_StartPoint()
		{
			LineString ls = SimpleOpen();
			Coordinate coord = new Coordinate(0, 0);
			Coordinate coord2 = new Coordinate(1.0, 1.0);
			
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(coord);
			Point point2 = gf.CreatePoint(coord2);
			Point point3 = gf.CreatePoint(coord);

			point = ls.GetStartPoint() as Point;

			Assertion.AssertEquals("StartPoint-1: ", true, point.Equals(point2));
			Assertion.AssertEquals("StartPoint-2: ", false, point.Equals(point3));
		}

		public void test_PointN()
		{
			LineString ls = SimpleOpen();

			Coordinate coord = new Coordinate(11.0, 12.0);
			Coordinate coord2 = new Coordinate(11.0, 20.0);
			Coordinate coord3 = new Coordinate(0.0, 0.0);
			
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(coord);
			Point point2 = gf.CreatePoint(coord2);
			Point point3 = gf.CreatePoint(coord);
	
			Point testPoint = ls.GetPointN(11) as Point;
			Assertion.AssertEquals("PointN: ", true, testPoint.Equals(point));
		}

		public void test_IsClosed()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleClosed();
			LineString ls4 = NonSimpleClosed();

			Assertion.AssertEquals("IsClosed-1: ", false, ls1.IsClosed());
			Assertion.AssertEquals("IsClosed-2: ", false, ls2.IsClosed());
			Assertion.AssertEquals("IsClosed-3: ", true, ls3.IsClosed());
			Assertion.AssertEquals("IsClosed-4: ", true, ls3.IsClosed());
		}

		public void test_IsRing()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleClosed();
			LineString ls4 = NonSimpleClosed();

			Assertion.AssertEquals("IsRing-1: ", false, ls1.IsRing());
			Assertion.AssertEquals("IsRing-2: ", false, ls2.IsRing());
			Assertion.AssertEquals("IsRing-3: ", true, ls3.IsRing());
			Assertion.AssertEquals("IsRing-4: ", false, ls4.IsRing());
		}

		public void test_IsEmpty()
		{
			Coordinates coords = new Coordinates();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords);
			Assertion.AssertEquals("IsEmpty-1: ", true, ls.IsEmpty());
			ls = SimpleOpen();
			Assertion.AssertEquals("IsEmpty-2: ", false, ls.IsEmpty());
		}

		public void test_Dimesion()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();

			Assertion.AssertEquals("Dimension-1: ", 1, ls1.GetDimension());
			Assertion.AssertEquals("Dimension-2: ", 1, ls2.GetDimension());
		}

		public void test_IsSimple()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleClosed();
			LineString ls4 = NonSimpleClosed();

			Assertion.AssertEquals("IsSimple-1: ", true, ls1.IsSimple());
			Assertion.AssertEquals("IsSimple-2: ", false, ls2.IsSimple());
			Assertion.AssertEquals("IsSimple-3: ", true, ls3.IsSimple());
			Assertion.AssertEquals("IsSimple-4: ", false, ls4.IsSimple());
		}

		public void test_NumPoints()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleClosed();
			LineString ls4 = NonSimpleClosed();
			Coordinates testCoords = new Coordinates();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls5 = gf.CreateLineString(testCoords);

			Assertion.AssertEquals("NumPoints-1: ", 22, ls1.GetNumPoints());
			Assertion.AssertEquals("NumPoints-2: ", 18, ls2.GetNumPoints());
			Assertion.AssertEquals("NumPoints-3: ", 21, ls3.GetNumPoints());
			Assertion.AssertEquals("NumPoints-4: ", 32, ls4.GetNumPoints());
			Assertion.AssertEquals("NumPoints-5: ", 0, ls5.GetNumPoints());
		}

		public void test_GetCoordinates()
		{
			LineString ls1 = SimpleOpen();
			Coordinates coords = ls1.GetCoordinates();
			Coordinates coords2 = new Coordinates();

			Assertion.AssertEquals("GetCoordinates-1: ", true, _coords.Equals(coords)); 
			Assertion.AssertEquals("GetCoordinates-1: ", false, coords2.Equals(coords)); 
		}

		public void test_CoordinateN()
		{
			LineString ls1 = SimpleOpen();
			Coordinate coord1 = new Coordinate(11.0, 12.0);
			Coordinate coord2 = new Coordinate(8.0, 15.0);
			Coordinate coord3 = new Coordinate(11.0, 18.0);
			Coordinate coord4 = new Coordinate(10.0, 21.0);

			Coordinate coord5 = ls1.GetCoordinateN(11);
			Coordinate coord6 = ls1.GetCoordinateN(14);
			Coordinate coord7 = ls1.GetCoordinateN(17);
			Coordinate coord8 = ls1.GetCoordinateN(20);

			Assertion.AssertEquals("CoordinateN-1: ", true, coord1.Equals(coord5));
			Assertion.AssertEquals("CoordinateN-2: ", true, coord2.Equals(coord6));
			Assertion.AssertEquals("CoordinateN-3: ", true, coord3.Equals(coord7));
			Assertion.AssertEquals("CoordinateN-4: ", true, coord4.Equals(coord8));
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		public void test_Equals()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleOpen();
			Coordinate coord = new Coordinate(5.0, 2.0);
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			Point point = gf.CreatePoint(coord);

			Assertion.AssertEquals("Equals-1: ", true, ls1.Equals(ls3));
			//todo(Ronda): fix when geometry.equals is working
			Assertion.AssertEquals("Equals-2: ", false, ls1.Equals(ls2));
			Assertion.AssertEquals("Equals-3: ", true, ls3.Equals(ls1));
			Assertion.AssertEquals("Equals-4: ", false, ls2.Equals(point));
		}

		public void test_GetHashCode()
		{
			LineString ls1 = SimpleOpen();
			LineString ls2 = NonSimpleOpen();
			LineString ls3 = SimpleOpen();
			int hash1 = ls1.GetHashCode();
			int hash2 = ls2.GetHashCode();
			int hash3 = ls3.GetHashCode();

			Assertion.AssertEquals("GetHashCode-1: ", true, hash1.Equals(hash3));
			Assertion.AssertEquals("GetHashCode-2: ", true, hash3.Equals(hash1));
			Assertion.AssertEquals("GetHashCode-3: ", false, hash1.Equals(hash2));
		}

		public void test_ToString()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(1.0, 2.0);
			coords.Add(coord);
			coord = new Coordinate(3.0, 4.0);
			coords.Add(coord);
			
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls1 = gf.CreateLineString(coords);
			Assertion.AssertEquals("ToString-1: ", "LineString:(1, 2, NaN),(3, 4, NaN)", ls1.ToString());

			coord = new Coordinate(2.4, 4.9);
			coords.Add(coord);
			LineString ls2 = gf.CreateLineString(coords);
			Assertion.AssertEquals("ToString-2: ", "LineString:(1, 2, NaN),(3, 4, NaN),(2.4, 4.9, NaN)", ls2.ToString());

			coord = new Coordinate(1.0, 1.0);
			coords.Add(coord);
			LineString ls3 = gf.CreateLineString(coords);
			Assertion.AssertEquals("ToString-3: ", "LineString:(1, 2, NaN),(3, 4, NaN),(2.4, 4.9, NaN),(1, 1, NaN)", ls3.ToString());
		}

		#endregion

		public void test_GeometryType()
		{
			LineString ls = SimpleOpen();

			Assertion.AssertEquals("GetType: ", "LineString", ls.GetGeometryType());
		}

		public void test_Coordinates()
		{
			LineString ls1 = SimpleOpen();
			Coordinates coords = ls1.GetCoordinates();

			for(int i = 0; i < coords.Count; i++)
			{
				Assertion.AssertEquals("Coordinates: ", true, coords[i].Equals(_coords[i]));
			}
		}

		public void test_GetBoundary()
		{
			//try on a simple-open linestring
			LineString ls = SimpleOpen();
			Geometry geom =  ls.GetBoundary();
            
			Assertion.AssertEquals("GetBoundary-1: ", false, geom.IsEmpty());

			Assertion.AssertEquals("GetBoundary-2: ", 2, geom.GetNumPoints());

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			MultiPoint mp = gf.CreateMultiPoint(geom.GetCoordinates());
			for(int i = 0; i < mp.GetNumPoints(); i++)
			{
				switch(i)
				{
					case 0:
						Assertion.AssertEquals("GetBoundary-3: ", 1.0, mp.GetCoordinate(i).X);
						Assertion.AssertEquals("GetBoundary-4: ", 1.0, mp.GetCoordinate(i).Y);
						break;
					case 1:
						Assertion.AssertEquals("GetBoundary-5: ", 9.0, mp.GetCoordinate(i).X);
						Assertion.AssertEquals("GetBoundary-6: ", 22.0, mp.GetCoordinate(i).Y);
						break;
					default:
						Assertion.Fail("This should never be reached");
						break;
				}
			}

			//try on a simple-closed linestring
			ls = SimpleClosed();
			geom =  ls.GetBoundary();
            
			Assertion.AssertEquals("GetBoundary-7: ", true, geom.IsEmpty());

			Assertion.AssertEquals("GetBoundary-8: ", 0, geom.GetNumPoints());

			//try on a nonsimple-open linestring
			ls = NonSimpleOpen();
			geom =  ls.GetBoundary();
            
			Assertion.AssertEquals("GetBoundary-9: ", false, geom.IsEmpty());

			Assertion.AssertEquals("GetBoundary-10: ", 2, geom.GetNumPoints());

			mp = gf.CreateMultiPoint(geom.GetCoordinates());

			for(int i = 0; i < mp.GetNumPoints(); i++)
			{
				switch(i)
				{
					case 0:
						Assertion.AssertEquals("GetBoundary-11: ", 2.0, mp.GetCoordinate(i).X);
						Assertion.AssertEquals("GetBoundary-12: ", 2.0, mp.GetCoordinate(i).Y);
						break;
					case 1:
						Assertion.AssertEquals("GetBoundary-13: ", 3.0, mp.GetCoordinate(i).X);
						Assertion.AssertEquals("GetBoundary-14: ", 9.0, mp.GetCoordinate(i).Y);
						break;
					default:
						Assertion.Fail("This should never be reached");
						break;
				}
			}

			//try on a simple-closed linestring
			ls = NonSimpleClosed();
			geom =  ls.GetBoundary();
            
			Assertion.AssertEquals("GetBoundary-15: ", true, geom.IsEmpty());

			Assertion.AssertEquals("GetBoundary-16: ", 0, geom.GetNumPoints());
		}

		public void test_GetBoundaryDimension()
		{
			LineString ls = SimpleOpen();
			//should always return zero because the dimension of a multipoint is 0
			Assertion.AssertEquals("GetBoundaryDimension: ", 0, ls.GetBoundaryDimension());
		}

		public void test_ApplyCoordinateFilter()
		{
			//create a new Linestring
			LineString ls = SimpleOpen();
			//CoordinateFilter filter = new CoordinateFilter();

			//todo(Ronda): Apply
			//ls.Apply(filter as ICoordinateFilter);
		}
		
		public void test_ApplyFilterGeometryFilter()
		{
			//create a new Linestring
			LineString ls = SimpleOpen();
			//GeometryFilter filter = new GeometryFilter();

			//todo(Ronda): Apply
			//ls.Apply(filter);
		}

		public void test_CompareToSameClass()
		{
			LineString ls = SimpleOpen();
			LineString ls2 = SimpleOpen();
			LineString ls3 = SimpleClosed();
			Point[] coords  = new Point[]{};
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			MultiPoint mp = gf.CreateMultiPoint(coords);

			Assertion.AssertEquals("CompareToSameClass-1: ", 0, ls.CompareToSameClass(ls2));
			Assertion.AssertEquals("CompareToSameClass-2: ", 1, ls.CompareToSameClass(ls3));
			Assertion.AssertEquals("CompareToSameClass-3: ", 1, ls.CompareToSameClass(mp));
			Assertion.AssertEquals("CompareToSameClass-4: ", -1, ls3.CompareToSameClass(ls));
			Assertion.AssertEquals("CompareToSameClass-5: ", -1, mp.CompareToSameClass(ls));
		}
	}
}

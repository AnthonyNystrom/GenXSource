#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/MultiLineStringTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: MultiLineStringTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 10    12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 9     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 8     10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 7     10/08/02 12:31p Rabergman
 * Fixed Dimension
 * 
 * 6     10/04/02 4:49p Rabergman
 * 
 * 5     10/04/02 1:10p Rabergman
 * changed test_SetEmpty and test_Extent2D due to changes made during code
 * review.
 * 
 * 4     10/03/02 4:02p Rabergman
 * Commented ot broken tests until IsSimple is fixed.
 * 
 * 3     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 2     9/27/02 3:04p Rabergman
 * Daily check in
 * 
 * 1     9/25/02 3:01p Rabergman
 * 
 * 8     9/18/02 11:03a Rabergman
 * Added: test_GetGeometries & Test_ComputeEnvelopeInternal
 * 
 * 7     9/17/02 9:55a Rabergman
 * Removed test for Normalize.  Changed CompareToSameClass.
 * 
 * 6     9/09/02 1:46p Rabergman
 * Fixed tests that were not working due to parts of the class not having
 * been implemented - bondarydimension & numpoints.
 * 
 * 5     9/05/02 2:25p Rabergman
 * Added comapretosameclass
 * 
 * 4     9/04/02 1:04p Rabergman
 * added enumerator
 * 
 * 3     9/03/02 1:50p Rabergman
 * Fixed exception being thrown
 * 
 * 2     9/03/02 11:42a Rabergman
 * Added tests
 * 
 * 1     8/29/02 12:40p Rabergman
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
	/// Summary description for MultiLineStringTest.
	/// </summary>
	[TestFixture]
	public class MultiLineStringTest 
	{
		//set up needed variables
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		int _sRID = 3;

		

		private MultiLineString CreateMLS()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString lineString = gf.CreateLineString(coords);
			LineString[] ls = new LineString[10];
			for(int i = 0; i < 10; i++)
			{
				coords = new Coordinates();
				for(int j = i; j < i+10; j++)
				{
					coord = new Coordinate();
					coord.X = (double)j;
					coord.Y = (double)j+5;
					coords.Add(coord);
				}
				lineString = gf.CreateLineString(coords);
				ls[i] = lineString;
			}
			MultiLineString multiLS = gf.CreateMultiLineString(ls);

			return multiLS;
		}

		private MultiLineString CreateMLS1()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString lineString = gf.CreateLineString(coords);
			LineString[] ls = new LineString[10];
			int c = 0;
			for(int i = 10; i > 0; i--)
			{
				for(int j = i; j < i+10; j++)
				{
					coord = new Coordinate();
					coord.X = (double)j;
					coord.Y = (double)j+5;
					coords.Add(coord);
				}
				lineString = gf.CreateLineString(coords);
				ls[c] = lineString;
				c++;
			}
			MultiLineString multiLS = gf.CreateMultiLineString(ls);

			return multiLS;
		}

		private MultiLineString closedMLS()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString lineString = gf.CreateLineString(coords);
			LineString[] ls = new LineString[2];

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
			ls[0] = gf.CreateLineString(coords);

			coords = new Coordinates();
			coord = new Coordinate(5, 1);
			coords.Add(coord);
			coord = new Coordinate(6, 2);
			coords.Add(coord);
			coord = new Coordinate(7, 3);
			coords.Add(coord);
			coord = new Coordinate(6, 4);
			coords.Add(coord);
			coord = new Coordinate(5, 5);
			coords.Add(coord);
			coord = new Coordinate(4, 4);
			coords.Add(coord);
			coord = new Coordinate(3, 3);
			coords.Add(coord);
			coord = new Coordinate(4, 2);
			coords.Add(coord);
			coord = new Coordinate(5, 1);
			coords.Add(coord);
			ls[1] = gf.CreateLineString(coords);

			MultiLineString multiLS = gf.CreateMultiLineString(ls);

			return multiLS;
		}

		private MultiLineString nonSimpleMLS()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate();
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString lineString = gf.CreateLineString(coords);
			LineString[] ls = new LineString[1];

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
			ls[0] = gf.CreateLineString(coords);

			MultiLineString multiLS = gf.CreateMultiLineString(ls);

			return multiLS;
		}

		public void test_Constructor()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();
			MultiLineString multiLS2 = CreateMLS1();

			Assertion.AssertEquals("Constructor-1: ", false, multiLS.IsEmpty());
			Assertion.AssertEquals("Constructor-2: ", false, multiLS2.IsEmpty());
		}

		public void test_GeometryType()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			Assertion.AssertEquals("GeometryType-1: ", "MultiLineString", multiLS.GetGeometryType());
		}

		public void test_Coordinates()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			//this geometry conatins 10 sets of coordinates
			Assertion.AssertEquals("Coordinates-1: ", 100, multiLS.GetCoordinates().Count);

			//now try it with a null geometry collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			Assertion.AssertEquals("Coordinates-2: ", 0, multiLS.GetCoordinates().Count);

			//now try it with a different geometry collection
			multiLS = CreateMLS1();

			//1000 sets of coordinates
			Assertion.AssertEquals("Cordinates-3: ", 1000, multiLS.GetCoordinates().Count);
		}

		public void test_GetBoundary()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			//todo:GetBoundary not tested... waiting for GeometryGraph to be tested first
			//Geometry geom = multiLS.GetBoundary();
		}

		public void test_GetBoundaryDimension()
		{
			//create a multilinestring
			MultiLineString multiLS = CreateMLS();

			//this returns a zero because it is not closed
			Assertion.AssertEquals("GetBoundaryDimension-1: ", 0, multiLS.GetBoundaryDimension());

			//now try it with a null multilinestring
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			//this returns a zero because it is not closed
			Assertion.AssertEquals("GetBoundaryDimension-2: ", 0, multiLS.GetBoundaryDimension());

			//now try it with a closed multilinestring
			multiLS = closedMLS();

			//this returns a -1 because it is closed
			Assertion.AssertEquals("GetBoundaryDimension-3: ", -1, multiLS.GetBoundaryDimension());
		}

		public void test_EqualExact()
		{
			//create a geomerty collection
			MultiLineString multiLS1 = CreateMLS();
			//create another geometry collection that is null
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			MultiLineString multiLS2 = gf.CreateMultiLineString(null);
			//create another geometry collection that is different
			MultiLineString multiLS3 = CreateMLS1();
			//create another geometry collection that is different
			MultiLineString multiLS4 = closedMLS();
			//create another geometry collection that is the same as the first
			MultiLineString multiLS5 = CreateMLS();

			Assertion.AssertEquals("Equals-1: ", true , multiLS1.Equals(multiLS5));
			Assertion.AssertEquals("Equals-2: ", false, multiLS1.Equals(multiLS2));
			Assertion.AssertEquals("Equals-3: ", false, multiLS1.Equals(multiLS3));
			Assertion.AssertEquals("Equals-4: ", false, multiLS1.Equals(multiLS4));
		}
		
		public void test_Apply1()
		{
			//can't be tested
		}

		public void test_Apply2()
		{
			//can't be tested
		}

		public void test_Normalize()
		{
			//shouldn't be tested
		}

		public void test_CompareToSameClass()
		{
			//create a geometry collection
			MultiLineString multiLS1 = CreateMLS();

			//create another geometry collection
			MultiLineString multiLS2 = CreateMLS1();

			//create another geometry collection
			MultiLineString multiLS3 = CreateMLS1();

			//todo:uncomment these...(they were too slow)
//			Assertion.AssertEquals("CompareToSameClass1: ", 0, multiLS1.CompareToSameClass(multiLS1));
//			Assertion.AssertEquals("CompareToSameClass2: ", -1, multiLS1.CompareToSameClass(multiLS3));
//			Assertion.AssertEquals("CompareToSameClass3: ", 1, multiLS3.CompareToSameClass(multiLS1));
		}

		public void test_ComputeEnvelopeInternal()
		{
			//create a geometry collection
			MultiLineString multiLS1 = CreateMLS();
//These tests cannot be run unless ComputeEnvelopeInternal is made public
//			Assertion.AssertEquals("ComputeEnvelopeInternal-1: ", 0.0, multiLS1.ComputeEnvelopeInternal().MinX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-2: ", 5.0, multiLS1.ComputeEnvelopeInternal().MinY);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-3: ", 18.0, multiLS1.ComputeEnvelopeInternal().MaxX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-4: ", 23.0, multiLS1.ComputeEnvelopeInternal().MaxY);
		}

		public void test_Clone()
		{
			//create a geometry collection
			MultiLineString multiLS1 = CreateMLS();
			MultiLineString multiLS2 = multiLS1.Clone() as MultiLineString;

			Assertion.AssertEquals("Clone: ", true, multiLS1.Equals(multiLS2));
		}

		public void test_IsEmpty()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			Assertion.AssertEquals("IsEmpty-1: ", false, multiLS.IsEmpty());

			//now try it with a null geometry collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			Assertion.AssertEquals("IsEmpty-2: ", true, multiLS.IsEmpty());

			//now try it with a different geometry collection
			multiLS = CreateMLS1();

			Assertion.AssertEquals("IsEmpty-3: ", false, multiLS.IsEmpty());

			//now try it again
			multiLS = closedMLS();

			Assertion.AssertEquals("IsEmpty-4: ", false, multiLS.IsEmpty());
		}

		public void test_Dimension()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			//returns 1, always returns a zero
			Assertion.AssertEquals("Dimension: ", 1, multiLS.GetDimension());
		}

		public void test_IsSimple()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			Assertion.AssertEquals("IsSimple-1: ", false, multiLS.IsSimple());

			//now try it with a null geometry collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			Assertion.AssertEquals("IsSimple-2: ", true, multiLS.IsSimple());

			//now try it with a different geometry collection
			multiLS = CreateMLS1();

			//TODO: This is really slow!!!!!!!!!  Why?
			//Assertion.AssertEquals("IsSimple-3: ", false, multiLS.IsSimple());

			//now try it with a mixed geometry collection
			multiLS = nonSimpleMLS();

			Assertion.AssertEquals("IsSimple-4: ", false, multiLS.IsSimple());

			//now try it with a closed geometry collection
			multiLS = closedMLS();

			//TODO: Uncomment when IsSimple is working.
			Assertion.AssertEquals("IsSimple-5: ", true, multiLS.IsSimple());
		}

		public void test_NumPoints()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			Assertion.AssertEquals("NumPoints-1: ", 100, multiLS.GetNumPoints());

			//now try it with a null geometry collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			Assertion.AssertEquals("NumPoints-2: ", 0, multiLS.GetNumPoints());

			//now try it with a different geometry collection
			multiLS = closedMLS();

			Assertion.AssertEquals("NumPoints-3: ", 30, multiLS.GetNumPoints());

			//now try it with a mixed geometry collection
			multiLS = nonSimpleMLS();

			Assertion.AssertEquals("NumPoints-4: ", 18, multiLS.GetNumPoints());
		}

		public void test_Extent2D()
		{
//(Note: this method was removed with OGIS)
//			//create a geomerty collection
//			MultiLineString multiLS = CreateMLS();
//
//			//initalize all the out variables
//			double minX = double.NaN;
//			double minY = double.NaN;
//			double maxX = double.NaN;
//			double maxY = double.NaN;
//
//			//get the extent
//			multiLS.Extent2D(out minX, out minY, out maxX, out maxY);
//
//			//check the return values
//			Assertion.AssertEquals("Extent2D-1: ", 0.0, minX);
//			Assertion.AssertEquals("Extent2D-2: ", 5.0, minY);
//			Assertion.AssertEquals("Extent2D-3: ", 18.0, maxX);
//			Assertion.AssertEquals("Extent2D-4: ", 23.0, maxY);
//
//			//now try it with a null geometry collection
//			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
//			multiLS = gf.CreateMultiLineString(null);
//
//			//get the extent again
//			multiLS.Extent2D(out minX, out minY, out maxX, out maxY);
//
//			//check the return values (an empty envelope returns, 0,0,-1,-1)
//			Assertion.AssertEquals("Extent2D-5: ", 0.0, minX);
//			Assertion.AssertEquals("Extent2D-6: ", 0.0, minY);
//			Assertion.AssertEquals("Extent2D-7: ", 0.0, maxX);
//			Assertion.AssertEquals("Extent2D-8: ", 0.0, maxY);
		}

		public void test_Project()
		{
//(Note: this method was removed with OGIS)
//			//create a geomerty collection
//			MultiLineString multiLS = CreateMLS();
//
//			//TODO: change this when project is working
//			Assertion.AssertEquals("Project-1: ", null, multiLS.Project(null));
		}

		public void test_SetEmpty()
		{
//			//create a new collection
//			MultiLineString multiLS = CreateMLS();
//
//			Assertion.AssertEquals("SetEmpty-1: ", false, multiLS.IsEmpty);
//			try
//			{
//				multiLS.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
		}

		public void test_Geometry()
		{
			LineString[] linestrings = new LineString[2];
			Coordinates coords1 = new Coordinates();
			Coordinate coord = new Coordinate(5,3);
			coords1.Add(coord);
			coord = new Coordinate(4,5);
			coords1.Add(coord);
			coord = new Coordinate(3,4);
			coords1.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LineString ls = gf.CreateLineString(coords1);
			linestrings[0] = ls;
			
			Coordinates coords2 = new Coordinates();
			coord = new Coordinate(2,7);
			coords2.Add(coord);
			coord = new Coordinate(9,2);
			coords2.Add(coord);
			coord = new Coordinate(7,9);
			coords2.Add(coord);

			ls = gf.CreateLineString(coords2);
			linestrings[1] = ls;

			MultiLineString mls = gf.CreateMultiLineString(linestrings);

			Assertion.AssertEquals("Geometry-1: ", "LineString:(5, 3, NaN),(4, 5, NaN),(3, 4, NaN)", mls.GetGeometryN(0).ToString());
			Assertion.AssertEquals("Geometry-2: ", "LineString:(2, 7, NaN),(9, 2, NaN),(7, 9, NaN)", mls.GetGeometryN(1).ToString());
		}

		public void test_Envelope()
		{
			//create a new collection
			MultiLineString multiLS = CreateMLS();

			//put the envelope into a geometry
			Geometry env = multiLS.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-1: ", false, env.IsEmpty());
            
			Coordinates coords = env.GetCoordinates();
			//check the first set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-2: ", 0.0, coords[0].X);
			Assertion.AssertEquals("Envelope-3: ", 5.0, coords[0].Y);

			//check the second set of coordinates (maxX, minY)
			Assertion.AssertEquals("Envelope-4: ", 18.0, coords[1].X);
			Assertion.AssertEquals("Envelope-5: ", 5.0, coords[1].Y);

			//check the third set of coordinates (maxX, maxY)
			Assertion.AssertEquals("Envelope-6: ", 18.0, coords[2].X);
			Assertion.AssertEquals("Envelope-7: ", 23.0, coords[2].Y);

			//check the forth set of coordinates (minX, maxY)
			Assertion.AssertEquals("Envelope-8: ", 0.0, coords[3].X);
			Assertion.AssertEquals("Envelope-9: ", 23.0, coords[3].Y);

			//check the fifth set of coordinates (minX, minY)
			Assertion.AssertEquals("Envelope-10: ", 0.0, coords[4].X);
			Assertion.AssertEquals("Envelope-11: ", 5.0, coords[4].Y);

			//create a null collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			//put the envelope into a geometry
			env = multiLS.GetEnvelope() as Geometry;

			//make sure there is something in the envelope
			Assertion.AssertEquals("Envelope-12: ", true, env.IsEmpty());
		}

		public void test_NumGeometries()
		{
			//create a geomerty collection
			MultiLineString multiLS = CreateMLS();

			Assertion.AssertEquals("NumGeometries-1: ", 10, multiLS.GetNumGeometries());

			//now try it with a null geometry collection
			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			multiLS = gf.CreateMultiLineString(null);

			Assertion.AssertEquals("NumGeometries-2: ", 0, multiLS.GetNumGeometries());

			//now try it with a different geometry collection
			multiLS = closedMLS();

			Assertion.AssertEquals("NumGeometries-3: ", 2, multiLS.GetNumGeometries());

			//now try it with a mixed geometry collection
			multiLS = nonSimpleMLS();

			Assertion.AssertEquals("NumGeometries-4: ", 1, multiLS.GetNumGeometries());
		}

	}
}

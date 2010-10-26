#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/PolygonTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: PolygonTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 9     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 8     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 7     10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 6     10/09/02 1:40p Awcoats
 * Added area tests.
 * 
 * 5     10/04/02 4:49p Rabergman
 * 
 * 4     10/04/02 1:12p Rabergman
 * changed test_SetEmpty, test_IsEmpty, test_Holes & test_Count due to
 * changes made during code review.
 * 
 * 3     10/02/02 2:05p Rabergman
 * Daily check in
 * 
 * 2     10/01/02 12:08p Rabergman
 * completed
 * 
 * 1     9/30/02 11:36a Rabergman
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
	/// Summary description for CoordinateTest.
	/// </summary>
	public class PolygonTest 
	{
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		GeometryFactory _gf = new GeometryFactory();
		int _sRID = 3;
		Coordinates _coords1 = new Coordinates();
		Coordinates _coords2 = new Coordinates();
		LinearRing _exterior1;
		LinearRing _exterior2;
		LinearRing _interior2;

		private Polygon Poly1()
		{
			_coords1 = new Coordinates();
			Coordinate coord = new Coordinate(5, 1);
			_coords1.Add(coord);
			coord = new Coordinate(6, 2);
			_coords1.Add(coord);
			coord = new Coordinate(7, 3);
			_coords1.Add(coord);
			coord = new Coordinate(6, 4);
			_coords1.Add(coord);
			coord = new Coordinate(5, 5);
			_coords1.Add(coord);
			coord = new Coordinate(4, 4);
			_coords1.Add(coord);
			coord = new Coordinate(3, 3);
			_coords1.Add(coord);
			coord = new Coordinate(4, 2);
			_coords1.Add(coord);
			coord = new Coordinate(5, 1);
			_coords1.Add(coord);

			_gf = new GeometryFactory(_precMod, _sRID);
			_exterior1 = _gf.CreateLinearRing(_coords1);
			Polygon polygon = _gf.CreatePolygon(_exterior1);

			return polygon;
		}

		private Polygon Poly2()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(10, 13);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(11, 13);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(12, 13);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(13, 14);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(14, 15);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(15, 16);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(15, 17);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(15, 18);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(14, 19);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(13, 20);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(12, 21);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(11, 21);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(10, 21);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(9, 20);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(8, 19);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(7, 18);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(7, 17);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(7, 16);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(8, 15);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(9, 14);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(10, 13);
			_coords2.Add(coord);
			coords.Add(coord);

			_gf = new GeometryFactory(_precMod, _sRID);
			_exterior2 = _gf.CreateLinearRing(coords);

			coords = new Coordinates();
			coord = new Coordinate(10, 16);
			_coords2.Add(coord);
			coords.Add(coord);
			coord = new Coordinate(11, 17);
			coords.Add(coord);
			_coords2.Add(coord);
			coord = new Coordinate(10, 18);
			coords.Add(coord);
			_coords2.Add(coord);
			coord = new Coordinate(9, 17);
			coords.Add(coord);
			_coords2.Add(coord);
			coord = new Coordinate(10, 16);
			coords.Add(coord);
			_coords2.Add(coord);

			_interior2 = _gf.CreateLinearRing(coords);
			LinearRing[] linearRings = new LinearRing[1];
			linearRings[0] = _interior2;

			_gf = new GeometryFactory();
			Polygon polygon = _gf.CreatePolygon(_exterior2, linearRings);

			return polygon;
		}

		

		public void test_Constructor1()
		{
			Polygon polygon = Poly1();
			Assertion.AssertEquals("Constructor1: ", false, polygon.IsEmpty());
		}

		public void test_Constructor2()
		{
			Polygon polygon = Poly2();
			Assertion.AssertEquals("Constructor2: ", false, polygon.IsEmpty());
		}

		public void test_Clone()
		{
			Polygon poly1 = Poly2();
			Polygon poly2 = poly1.Clone() as Polygon;
			Assertion.AssertEquals("Clone: ", true, poly1.Equals(poly2));

			poly1 = Poly1();
			poly2 = poly1.Clone() as Polygon;
			Assertion.AssertEquals("Clone: ", true, poly1.Equals(poly2));
		}

		public void test_InteriorRing()
		{
			Polygon poly1 = Poly1();
			Polygon poly2 = Poly2();

			LinearRing ilr2;

			ilr2 = _interior2;

			LinearRing ilr = poly1.GetInteriorRingN(0);
			Assertion.AssertEquals("InteriorRing-1: ", null, ilr);

			ilr = poly2.GetInteriorRingN(0);
			for(int i = 0; i < ilr.GetNumPoints(); i++)
			{
				Point point = ilr.GetPointN(i) as Point;
				Point point2 = _interior2.GetPointN(i) as Point;
				Assertion.AssertEquals("InteriorRing-2: ", true, point.Equals(point2));
			}
		}

		public void test_SetEmpty()
		{
//(Note: this method was removed with OGIS)
//			Polygon poly1 = Poly1();
//			Polygon poly2 = Poly2();
//
//			Assertion.AssertEquals("SetEmpty-1:", false, poly1.IsEmpty());
//			Assertion.AssertEquals("SetEmpty-2:", false, poly2.IsEmpty());
//
//			try
//			{
//				poly1.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
//
//			try
//			{
//				poly2.SetEmpty();
//				Fail("Exception should have been thrown");
//			}
//			catch(NotSupportedException)
//			{
//			}
		}

		public void test_ExteriorRing()
		{
			Polygon poly = Poly1();

			LinearRing lr = poly.GetExteriorRing() as LinearRing;

			Assertion.AssertEquals("ExteriorRing-1: ", true, lr.Equals(_exterior1));
			Assertion.AssertEquals("ExteriorRing-2: ", false, lr.Equals(_exterior2));

			poly = Poly2();
			lr = poly.GetExteriorRing() as LinearRing;
			Assertion.AssertEquals("ExteriorRing-3: ", true, lr.Equals(_exterior2));
			Assertion.AssertEquals("ExteriorRing-4: ", false, lr.Equals(_exterior1));
		}

		public void test_PointOnSurface()
		{
			//not supported
		}

		public void test_Envelope()
		{
			Polygon polygon = Poly1();

			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(3.0, 1.0);
			coords.Add(coord);
			coord = new Coordinate(7.0, 1.0);
			coords.Add(coord);
			coord = new Coordinate(7.0, 5.0);
			coords.Add(coord);
			coord = new Coordinate(3.0, 5.0);
			coords.Add(coord);
			coord = new Coordinate(3.0, 1.0);
			coords.Add(coord);

			Geometry env = polygon.GetEnvelope() as Geometry;
			Coordinates coords2 = env.GetCoordinates();
			Assertion.AssertEquals("Envelope-1: ", coords[0], coords2[0]);
			Assertion.AssertEquals("Envelope-2: ", coords[1], coords2[1]);
			Assertion.AssertEquals("Envelope-3: ", coords[2], coords2[2]);
			Assertion.AssertEquals("Envelope-4: ", coords[3], coords2[3]);
			Assertion.AssertEquals("Envelope-5: ", coords[4], coords2[4]);

			polygon = Poly2();
			
			coords = new Coordinates();
			coord = new Coordinate(7.0, 13.0);
			coords.Add(coord);
			coord = new Coordinate(15.0, 13.0);
			coords.Add(coord);
			coord = new Coordinate(15.0, 21.0);
			coords.Add(coord);
			coord = new Coordinate(7.0, 21.0);
			coords.Add(coord);
			coord = new Coordinate(7.0, 13.0);
			coords.Add(coord);

			env = polygon.GetEnvelope() as Geometry;
			coords2 = env.GetCoordinates();
			Assertion.AssertEquals("Envelope-6: ", coords[0], coords2[0]);
			Assertion.AssertEquals("Envelope-7: ", coords[1], coords2[1]);
			Assertion.AssertEquals("Envelope-8: ", coords[2], coords2[2]);
			Assertion.AssertEquals("Envelope-9: ", coords[3], coords2[3]);
			Assertion.AssertEquals("Envelope-10: ", coords[4], coords2[4]);
		}

		public void test_Extent2D()
		{
//(Note: this method was removed with OGIS)
//			Polygon polygon = Poly1();
//
//			//initalize all the out variables
//			double minX = double.NaN;
//			double minY = double.NaN;
//			double maxX = double.NaN;
//			double maxY = double.NaN;
//
//			polygon.Extent2D(out minX, out minY, out maxX, out maxY);
//			Assertion.AssertEquals("Extent2D-1: ", 3.0, minX);
//			Assertion.AssertEquals("Extent2D-2: ", 1.0, minY);
//			Assertion.AssertEquals("Extent2D-3: ", 7.0, maxX);
//			Assertion.AssertEquals("Extent2D-4: ", 5.0, maxY);
//
//			polygon = Poly2();
//			polygon.Extent2D(out minX, out minY, out maxX, out maxY);
//
//			Assertion.AssertEquals("Extent2D-5: ", 7.0, minX);
//			Assertion.AssertEquals("Extent2D-6: ", 13.0, minY);
//			Assertion.AssertEquals("Extent2D-7: ", 15.0, maxX);
//			Assertion.AssertEquals("Extent2D-8: ", 21.0, maxY);
		}

		public void test_Centroid()
		{
			//not supported
		}

		public void test_Project()
		{
			//not implemented
		}

		public void test_IsEmpty()
		{
			Polygon poly = Poly1();
			Assertion.AssertEquals("IsEmpty-1: ", false, poly.IsEmpty());

			Coordinates coords = new Coordinates();
			_gf = new GeometryFactory(_precMod, _sRID);
			LinearRing lr = _gf.CreateLinearRing(coords);
			poly = _gf.CreatePolygon(lr);
			Assertion.AssertEquals("IsEmpty-2: ", true, poly.IsEmpty());
		}

		public void test_Dimension()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("Dimension: ", 2, poly.GetDimension());
		}

		public void test_NumInteriorRings()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("NumInteriorRings-1: ", 0, poly.GetNumInteriorRing());

			poly = Poly2();

			Assertion.AssertEquals("NumInteriorRings-2: ", 1, poly.GetNumInteriorRing());
		}

		public void test_Area()
		{
			//not supported
		}

		public void test_IsSimple()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("IsSimple-1: ", true, poly.IsSimple());
		}

		public void test_Shell()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("Shell-1: ", _exterior1, poly.Shell);

			poly = Poly2();

			Assertion.AssertEquals("Shell-2: ", _exterior2, poly.Shell);
		}

		public void test_Holes()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("Holes-1: ", 0, poly.Holes.Length);

			poly = Poly2();

			Assertion.AssertEquals("Holes-2: ", _interior2, poly.Holes[0]);
		}

		public void test_Count()
		{
			Polygon poly = Poly2();

			Assertion.AssertEquals("Count-1: ", 26, poly.GetNumPoints());
		}

		public void test_GeometryType()
		{
			Polygon poly = Poly1();

			Assertion.AssertEquals("GeometryType: ", "Polygon", poly.GetGeometryType());
		}

		public void test_Coordinates()
		{
			Polygon polygon = Poly1();

			Coordinates coords = polygon.GetCoordinates();

			for(int i = 0; i < coords.Count; i++)
			{
				Assertion.AssertEquals("Coordinates-1: ", true, coords[i].Equals(_coords1[i]));
			}

			polygon = Poly2();

			coords = polygon.GetCoordinates();

			for(int i = 0; i < coords.Count; i++)
			{
				Assertion.AssertEquals("Coordinates-1: ", true, coords[i].Equals(_coords2[i]));
			}
		}

        public void test_NumPoints()
		{
			Polygon poly = Poly1();
			Assertion.AssertEquals("NumPoints-1: ", 9, poly.GetNumPoints());

			poly = Poly2();
			Assertion.AssertEquals("NumPoints-2: ", 26, poly.GetNumPoints());
		}

		public void test_GetBoundary()
		{
			Polygon poly = Poly1();
			MultiLineString mls = poly.GetBoundary() as MultiLineString;
			Assertion.AssertEquals("GetBoundary-1: ", 1, mls.GetNumGeometries());
			Assertion.AssertEquals("GetBoundary-2: ", 9, mls.GetNumPoints());
			Coordinates coords = mls.GetCoordinates();
			for(int i = 0; i < 9; i++)
			{
				Assertion.AssertEquals("GetBoundary-3: ", true, coords[i].Equals(_exterior1.GetCoordinateN(i)));
			}
		}

		public void test_GetBoundaryDimension()
		{
			Polygon poly = Poly1();
			Assertion.AssertEquals("GetBoundaryDimension:" , 1, poly.GetBoundaryDimension());
		}

		public void test_Equals()
		{
			Polygon poly = Poly2();

			Assertion.AssertEquals("Equals: ", true, poly.Equals(poly));

			Polygon poly2 = Poly1();

			Assertion.AssertEquals("Equals: ", false, poly.Equals(poly2));

		}

		public void test_ApplyCoordinateFilter()
		{
			//CoordinateFilter filter = new CoordinateFilter();
			//Polygon poly = Poly2();
			//poly.Apply(filter);
		}

		public void test_ApplyGeometryFilter()
		{
			//GeometryFilter filter = new GeometryFilter();
			//Polygon poly = Poly1();
			//poly.Apply(filter);
		}

		public void test_Normalize()
		{
			//not supported
		}

		public void test_CompareToSameClass()
		{
			Polygon poly1 = Poly1();
			Polygon poly2 = Poly2();

			Assertion.AssertEquals("CompareToSameClass-1: ", 0, poly1.CompareToSameClass(Poly1()));
			Assertion.AssertEquals("CompareToSameClass-2: ", -1, poly1.CompareToSameClass(poly2));

			Assertion.AssertEquals("CompareToSameClass-3: ", 0, poly2.CompareToSameClass(Poly2()));
			Assertion.AssertEquals("CompareToSameClass-4: ", 1, poly2.CompareToSameClass(poly1));
		}

		public void test_ComputeEnvelopeinternal()
		{
			Polygon poly1 = Poly1();
			Polygon poly2 = Poly2();
//These test cannot be run unless changed to public.
//			Envelope env = poly1.ComputeEnvelopeInternal();
//			Assertion.AssertEquals("ComputeEnvelopeInternal-1: ", 3.0, env.MinX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-2: ", 1.0, env.MinY);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-3: ", 7.0, env.MaxX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-4: ", 5.0, env.MaxY);
//
//			env = poly2.ComputeEnvelopeInternal();
//			Assertion.AssertEquals("ComputeEnvelopeInternal-5: ", 7.0, env.MinX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-6: ", 13.0, env.MinY);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-7: ", 15.0, env.MaxX);
//			Assertion.AssertEquals("ComputeEnvelopeInternal-8: ", 21.0, env.MaxY);
		}

		/// <summary>
		/// Tests a regular polygon
		/// </summary>
		public void TestArea1()
		{
			string wkt = "POLYGON(( 5 5, 10 5, 10 10, 5 10, 5 5))";
			Polygon polygon = (Polygon)_gf.CreateFromWKT(wkt);
			double area = polygon.GetArea();
			Assertion.AssertEquals("Polygon area",25.0,area);
		}

		/// <summary>
		/// Test a polygon with a hole
		/// </summary>
		public void TestArea2()
		{
			string wkt = "POLYGON(( 5 5, 10 5, 10 10, 5 10, 5 5),(6 6, 7 6, 7 7, 6 7, 6 6))";
			Polygon polygon = (Polygon)_gf.CreateFromWKT(wkt);
			double area = polygon.GetArea();
			Assertion.AssertEquals("Polygon area",24.0,area);
		}

		/// <summary>
		/// Test an empty polygon
		/// </summary>
		public void TestArea3()
		{
			string wkt = "POLYGON EMPTY";
			Polygon polygon = (Polygon)_gf.CreateFromWKT(wkt);
			double area = polygon.GetArea();
			Assertion.AssertEquals("Polygon area",0.0,area);
		}
	}
}

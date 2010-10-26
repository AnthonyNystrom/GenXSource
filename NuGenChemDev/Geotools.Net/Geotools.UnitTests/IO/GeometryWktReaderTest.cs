#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/GeometryWktReaderTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: GeometryWktReaderTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 11    12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 10    12/09/02 11:56a Awcoats
 * changed x to X and y to Y.
 * 
 * 9     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 8     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 7     10/18/02 2:31p Awcoats
 * chaned names of interfaces. Removed CT_ prefix, and added I
 * 
 * 6     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 5     10/08/02 11:41a Awcoats
 * 
 * 4     10/07/02 2:24p Awcoats
 * 
 * 3     9/25/02 2:18p Awcoats
 * 
 * 2     9/25/02 9:20a Awcoats
 * 
 * 1     9/24/02 3:44p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
using Geotools.IO;
using Geotools.UnitTests.Utilities;
//using Geotools.IO;
#endregion

namespace Geotools.UnitTests.IO
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.GeometryWktReaderTest class
	/// </summary>
	[TestFixture]
	public class GeometryWktReaderTest 
	{
		


		#region Parsing
		public void TestGetNextWord()
		{
			string wkt = "POINT *( 3  4 )";

			GeometryFactory factory = new GeometryFactory();
			try
			{
				IGeometry geometry = factory.CreateFromWKT(wkt);
				Assertion.Fail("parse exception");
			}
			catch(ParseException)
			{
			}
		}

		public void Test1()
		{
			string wkt = null;

			GeometryFactory factory = new GeometryFactory();
			try
			{
				IGeometry geometry = factory.CreateFromWKT(wkt);
				Assertion.Fail("parse exception");
			}
			catch(ArgumentNullException)
			{
			}
		}
		public void Test2()
		{
			string wkt = "  ";

			GeometryFactory factory = new GeometryFactory();
			try
			{
				IGeometry geometry = factory.CreateFromWKT(wkt);
				Assertion.Fail("ArgumentException should have been thrown.");
			}
			catch(ArgumentException)
			{
			}
		}

		public void Test3()
		{
			string wkt = "MULTIPOINT EMPTY2 ";
			GeometryFactory factory = new GeometryFactory();
			try
			{
				IGeometry geometry = factory.CreateFromWKT(wkt);
				Assertion.Fail("EMPTY2 is not valid.");
			}
			catch(ParseException)
			{
			}
		}


		#endregion

		#region Point
		public void TestWktReadPoint1()
		{
			string wkt = "POINT ( 3 4 )";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IPoint point = (IPoint)geometry;
			Assertion.AssertEquals("empty",false,point.IsEmpty());
			Assertion.AssertEquals("x",3.0,point.GetX());
			Assertion.AssertEquals("y",4.0,point.GetY());
			string wkt2 = ((Point)point).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}

		public void TestWktReadPoint2()
		{
			string wkt = "POINT ( 3  , 4 )";

			GeometryFactory factory = new GeometryFactory();
			try
			{
				IGeometry geometry = factory.CreateFromWKT(wkt);
				Assertion.Fail("Should fail because of the comma.");
			}
			catch(ParseException)
			{
			}
		}

		public void TestWktReadPoint3()
		{
			string wkt = "POINT EMPTY";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IPoint point = (IPoint)geometry;
			Assertion.AssertEquals("empty",true,point.IsEmpty());
			string wkt2 = ((Point)point).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}
		#endregion

		#region MultiPoint
		public void TestWktReadMultiPoint1()
		{
			string wkt = "MULTIPOINT( 23 24, 34 35)";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IGeometryCollection multipoint = (IGeometryCollection)geometry;
			Assertion.AssertEquals("empty",false,multipoint.IsEmpty());
			string wkt2 = ((MultiPoint)multipoint).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}

		public void TestWktReadMultiPoint2()
		{
			string wkt = "MULTIPOINT EMPTY";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IGeometryCollection multipoint = (IGeometryCollection)geometry;
			Assertion.AssertEquals("empty",true,multipoint.IsEmpty());
			string wkt2 = ((MultiPoint)geometry).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}
		#endregion

		#region LineString
		public void TestLineString()
		{
			//                         1     2       3     4     5
			string wkt = "LINESTRING(50 31, 54 31, 54 29, 50 29, 50 31 )";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			ILineString linestring = (ILineString)geometry;
			Assertion.AssertEquals("numpoints",5,linestring.GetNumPoints());
			Assertion.AssertEquals("x1",50.0,linestring.GetPointN(0).X);
			Assertion.AssertEquals("y1",31.0,linestring.GetPointN(0).Y);
			Assertion.AssertEquals("x2",54.0,linestring.GetPointN(1).X);
			Assertion.AssertEquals("y2",31.0,linestring.GetPointN(1).Y);
			Assertion.AssertEquals("x3",54.0,linestring.GetPointN(2).X);
			Assertion.AssertEquals("y3",29.0,linestring.GetPointN(2).Y);
			Assertion.AssertEquals("x4",50.0,linestring.GetPointN(3).X);
			Assertion.AssertEquals("y4",29.0,linestring.GetPointN(3).Y);
			Assertion.AssertEquals("x5",50.0,linestring.GetPointN(4).X);
			Assertion.AssertEquals("y5",31.0,linestring.GetPointN(4).Y);
			string wkt2 = ((LineString)linestring).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}
		#endregion

		#region POLYGON
		
		public void TestPolygon1()
		{
			string wkt = "POLYGON( ( 50 31, 54 31, 54 29, 50 29, 50 31) )";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IPolygon polygon = (IPolygon)geometry;
			string wkt2 = ((Polygon)polygon).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));

		}
		public void TestPolygon2()
		{
			string wkt = "POLYGON( ( 1 1, 10 1, 10 10, 1 10, 1 1),(4 4, 5 4, 5 5, 4 5, 4 4 ))";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			IPolygon polygon = (IPolygon)geometry;
			string wkt2 = ((Polygon)polygon).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));

		}
		#endregion

		#region MULTILINESTRING
		public void TestMultiLineString1()
		{
			string wkt = "MULTILINESTRING (( 10.05  10.28 , 20.95  20.89 ),( 20.95  20.89, 31.92 21.45)) ";

			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			MultiLineString multilineString = (MultiLineString)geometry;
			Assertion.AssertEquals("Multilinestring 1",2,multilineString.GetNumGeometries());
			LineString linestring1 = (LineString)multilineString.GetGeometryN(0);
			LineString linestring2 = (LineString)multilineString.GetGeometryN(1);
			Assertion.AssertEquals("MLS 1",10.05,linestring1.GetCoordinates()[0].X);
			Assertion.AssertEquals("MLS 2",10.28,linestring1.GetCoordinates()[0].Y);
			Assertion.AssertEquals("MLS 3",20.95,linestring1.GetCoordinates()[1].X);
			Assertion.AssertEquals("MLS 4",20.89,linestring1.GetCoordinates()[1].Y);
			Assertion.AssertEquals("MLS 1",20.95,linestring2.GetCoordinates()[0].X);
			Assertion.AssertEquals("MLS 2",20.89,linestring2.GetCoordinates()[0].Y);
			Assertion.AssertEquals("MLS 3",31.92,linestring2.GetCoordinates()[1].X);
			Assertion.AssertEquals("MLS 4",21.45,linestring2.GetCoordinates()[1].Y);
			string wkt2 = ((MultiLineString)multilineString).ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
			
		}
		#endregion

		#region MULTIPOLYGON
		public void TestMultiPolygon1()
		{
			string wkt = "MULTIPOLYGON (((10 10, 10 20, 20 20, 20 15 , 10 10), (50 40, 50 50, 60 50, 60 40, 50 40)))";
			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			MultiPolygon multiPolygon = (MultiPolygon)geometry;
		
			//Assertion.AssertEquals("Multilinestring 1",2,multiPolygon.NumGeometries);
			IGeometry g = multiPolygon.GetGeometryN(0);
			Polygon poly1 = (Polygon)multiPolygon.GetGeometryN(0);
			LinearRing shell = poly1.Shell;
			LinearRing hole = poly1.Holes[0];
			Assertion.AssertEquals("MPS 1",10.0,shell.GetCoordinates()[0].X);
			Assertion.AssertEquals("MPS 2",10.0,shell.GetCoordinates()[0].Y);
			Assertion.AssertEquals("MPS 3",10.0,shell.GetCoordinates()[1].X);
			Assertion.AssertEquals("MPS 4",20.0,shell.GetCoordinates()[1].Y);
			Assertion.AssertEquals("MPS 5",20.0,shell.GetCoordinates()[2].Y);
			Assertion.AssertEquals("MPS 6",20.0,shell.GetCoordinates()[2].Y);
			Assertion.AssertEquals("MPS 7",20.0,shell.GetCoordinates()[3].X);
			Assertion.AssertEquals("MPS 8",15.0,shell.GetCoordinates()[3].Y);
			Assertion.AssertEquals("MPS 9",10.0,shell.GetCoordinates()[4].X);
			Assertion.AssertEquals("MPS 10",10.0,shell.GetCoordinates()[4].Y);

			Assertion.AssertEquals("MPS 11",50.0,hole.GetCoordinates()[0].X);
			Assertion.AssertEquals("MPS 12",40.0,hole.GetCoordinates()[0].Y);
			Assertion.AssertEquals("MPS 13",50.0,hole.GetCoordinates()[1].X);
			Assertion.AssertEquals("MPS 14",50.0,hole.GetCoordinates()[1].Y);
			Assertion.AssertEquals("MPS 15",60.0,hole.GetCoordinates()[2].X);
			Assertion.AssertEquals("MPS 16",50.0,hole.GetCoordinates()[2].Y);
			Assertion.AssertEquals("MPS 17",60.0,hole.GetCoordinates()[3].X);
			Assertion.AssertEquals("MPS 18",40.0,hole.GetCoordinates()[3].Y);
			Assertion.AssertEquals("MPS 19",50.0,hole.GetCoordinates()[4].X);
			Assertion.AssertEquals("MPS 20",40.0,hole.GetCoordinates()[4].Y);

			string wkt2 = multiPolygon.ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
			
		}
		#endregion

		#region GEOMETRYCOLLECTION
		public void TestGeomtryCollection()
		{
			string wkt = "GEOMETRYCOLLECTION(POINT ( 3 4 ),LINESTRING(50 31, 54 31, 54 29, 50 29, 50 31 ))";
			GeometryFactory factory = new GeometryFactory();
			IGeometry geometry = factory.CreateFromWKT(wkt);
			GeometryCollection geometryCollection = (GeometryCollection)geometry;
			Point point = (Point)geometryCollection[0];
			LineString  linestring= (LineString)geometryCollection[1];
			Assertion.AssertEquals("GeometryCollection 1",3.0,point.X);
			Assertion.AssertEquals("GeometryCollection 2",4.0,point.Y);
			Assertion.AssertEquals("GeometryCollection 3",5,linestring.GetNumPoints());
			string wkt2 = geometryCollection.ToText();
			Assertion.AssertEquals("wkt",true,Compare.WktStrings(wkt,wkt2));
		}
		#endregion

		
		
	}
}


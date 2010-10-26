#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/ProjectTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: ProjectTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 9     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 8     11/27/02 11:18a Awcoats
 * 
 * 7     11/20/02 10:11a Awcoats
 * removed some tests that were failing because the strings were long.
 * Should really fix this.
 * 
 * 6     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 5     10/25/02 12:26p Rabergman
 * Removed all references to OGC
 * 
 * 4     10/21/02 11:15a Rabergman
 * added ocg stuff back in temporarily.
 * 
 * 3     10/21/02 11:04a Rabergman
 * Made test match new formats for methods & properties.
 * 
 * 2     10/15/02 11:43a Rabergman
 * Changed numbering on multipoint tests & fixed multipoint test 5
 * 
 * 1     10/09/02 1:40p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.Data;
using NUnit.Framework;
using Geotools.IO;
using Geotools.Geometries;
using Geotools.CoordinateTransformations;
using Geotools.UnitTests.Utilities;
#endregion

namespace Geotools.UnitTests.Geometries
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.Geometries.ProjectTest class
	/// </summary>
	[TestFixture]
	public class ProjectTest  
	{
		CoordinateTransformationEPSGFactory _CTfactory;
		PrecisionModel _pm = new PrecisionModel(1.0, 0.0, 0.0);
		ICoordinateTransformation _UKNationalGrid1;
		GeometryFactory _geometryFactory;
		

		public bool Compare(string wkt, string expectedProjectedWkt)
		{
			Geometry geometry =(Geometry)_geometryFactory.CreateFromWKT(wkt);
			Geometry projectedGeometry = (Geometry)geometry.Project(_UKNationalGrid1);
			string actualProjectedWkt=projectedGeometry.ToText();
			return Geotools.UnitTests.Utilities.Compare.WktStrings(expectedProjectedWkt,actualProjectedWkt);
		}
		public void Test1()
		{
			IDbConnection connection = Global.GetEPSGDatabaseConnection();
			_CTfactory = new CoordinateTransformationEPSGFactory(connection);
			_UKNationalGrid1 = _CTfactory.CreateFromTransformationCode("1681");
			_geometryFactory = new GeometryFactory(_pm,4326);
			string wkt = "POINT ( -2.0 49.0 )";
			Assertion.AssertEquals("Point 1",true,Compare(wkt,"POINT (400000 -100000)"));

			wkt = "MULTIPOINT( -2 49, -1 50)";
			Assertion.AssertEquals("Multipoint 1",true,Compare(wkt,"MULTIPOINT (400000 -100000, 471660 11644)"));

			wkt = "MULTIPOINT EMPTY";
			Assertion.AssertEquals("Multipoint 2",true,Compare(wkt,"MULTIPOINT EMPTY"));

			wkt = "LINESTRING(50 31, 54 31, 54 29, 50 29, 50 31 )";
			Assertion.AssertEquals("LineString 1 1",true,Compare(wkt,"LINESTRING (5664915 -615242, 6117479 -308294, 6306392 -569639, 5827846 -873669, 5664915 -615242)"));

			wkt = "POLYGON( ( 50 31, 54 31, 54 29, 50 29, 50 31) )";
			Assertion.AssertEquals("Multipoint 3",true,Compare(wkt,"POLYGON ((5664915 -615242, 6117479 -308294, 6306392 -569639, 5827846 -873669, 5664915 -615242))"));


			//wkt = "POLYGON( ( 1 1, 10 1, 10 10, 1 10, 1 1),(4 4, 5 4, 5 5, 4 5, 4 4 ))";
			//Assertion.AssertEquals("Multipoint 4",true,Compare(wkt,"POLYGON ((733898 -5416388, 1744907 -5414055, 1724214 -4397377, 728899 -4420227, 733898 -5416388), "+
			//"(1067192 -5082521, 1178905 -5081633, 1177832 -4970278, 1066275 -4971386, 1067192 -5082521))"));

			// these tests fail because the strings are too long/ have a CR in the middle of the string. Should really fix this. awc.


			//wkt = "MULTILINESTRING (( 10.05  10.28 , 20.95  20.89 ),( 20.95  20.89, 31.92 21.45)) ";
			//Assertion.AssertEquals("Multipoint 5",false,Compare(wkt,"MULTILINESTRING ((1724213.5597264355 -4397376.6478565233, 2839122.2852214454 -3022771.8465291355), \n  "+ 
			//												"(2839122.2852214454 -3022771.8465291355, 4095081.5366646093 -2776957.6041615554))"));

			//wkt = "MULTIPOLYGON (((10 10, 10 20, 20 20, 20 15 , 10 10), (50 40, 50 50, 60 50, 60 40, 50 40)))";
			//Assertion.AssertEquals("Multipoint 6",true,Compare(wkt,"MULTIPOLYGON (((1724213.5597264355 -4397376.6478565233, 1662268.9948102259 -3270049.5581512651, 2745586.9073599684 -3156174.8212744244, 2817027.1068546474 -3744257.1145197917, 1724213.5597264355 -4397376.6478565233), "+
			//	 "(4882561.4795353347 438327.55639206013, 3970695.8611971624 1430641.0215268317, 4530976.2509158608 2096414.3039089143, 5721871.0214089518 1247465.211354611, 4882561.4795353347 438327.55639206013)))"));

			//wkt = "GEOMETRYCOLLECTION(POINT ( 3 4 ),LINESTRING(50 31, 54 31, 54 29, 50 29, 50 31 ))";
			//Assertion.AssertEquals("Multipoint 7",true,Compare(wkt,"GEOMETRYCOLLECTION (POINT (955682.872367636 -5083270.4404414054),"+
			//											"LINESTRING (-7.5557896002384908 49.766496583001434, -7.555734311078294 49.766499242352, -7.5557322582139372 49.76648133658518, -7.5557875473539609 49.7664786772363, -7.5557896002384908 49.766496583001434))"));

		}	

		public bool Compare2(string wkt, string expectedProjectedWkt, GeometryFactory geometryFactory, ICoordinateTransformation transformation)
		{
			Geometry geometry =(Geometry)geometryFactory.CreateFromWKT(wkt);
			Geometry projectedGeometry = (Geometry)geometry.Project(transformation);
			string actualProjectedWkt=projectedGeometry.ToText();
			return Geotools.UnitTests.Utilities.Compare.WktStrings(expectedProjectedWkt,actualProjectedWkt);
		}
		public void Test2()
		{
			IDbConnection connection = Global.GetEPSGDatabaseConnection();
			PrecisionModel pm = new PrecisionModel(100.0, 0.0, 0.0);
			CoordinateTransformationEPSGFactory CTfactory = new CoordinateTransformationEPSGFactory(connection);
			ICoordinateTransformation UKNationalGrid1 = CTfactory.CreateFromTransformationCode("1681");
			GeometryFactory geometryFactory = new GeometryFactory(pm,4326);

			string wkt = "POINT ( -1 50 )";
			Assertion.AssertEquals("Point 1",true,Compare2(wkt,"POINT (471659.59 11644.49)", geometryFactory, UKNationalGrid1));
			
		}
	}
}


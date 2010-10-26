#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/GeometryWktWriterTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: GeometryWktWriterTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 4     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 3     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 2     10/16/02 12:28p Rabergman
 * Removed i=0
 * 
 * 1     9/25/02 2:17p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using Geotools.Geometries;
using Geotools.IO;
using NUnit.Framework;
#endregion

namespace Geotools.UnitTests.IO
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.GeometryWktWriterTest class
	/// </summary>
	[TestFixture]
	public class GeometryWktWriterTest  
	{
		GeometryFactory _factory = new GeometryFactory();
		GeometryWKTWriter _writer = new GeometryWKTWriter();
		

		#region Point
		public void TestPoint1()
		{
			Coordinate cooordinate = new Coordinate(1.0,2.0);
			IPoint point = _factory.CreatePoint( cooordinate );
			string wkt = _writer.WriteFormatted(point);
			Assertion.AssertEquals("point wkt1","POINT (1 2)",wkt);
		}

		public void TestPoint2()
		{
			Coordinate cooordinate = new Coordinate(1.2,2.3);
			IPoint point = _factory.CreatePoint( cooordinate );
			string wkt = _writer.WriteFormatted(point);
			Assertion.AssertEquals("point wkt1","POINT (1.2 2.3)",wkt);
		}
		#endregion

		#region MultiPoint
		public void TestMultiPoint1()
		{
			Coordinates coordinates = new Coordinates();
			coordinates.Add( new Coordinate(1,2) );
			coordinates.Add( new Coordinate(1.2,2.3) );
			MultiPoint multipoint = _factory.CreateMultiPoint( coordinates );
			string wkt = _writer.WriteFormatted(multipoint);
			Assertion.AssertEquals("multi point","MULTIPOINT (1 2, 1.2 2.3)",wkt);
		}
		#endregion

		#region LineString

		public void TestLineString1()
		{
			Coordinates coordinates = new Coordinates();
			coordinates.Add( new Coordinate(1,2) );
			coordinates.Add( new Coordinate(1.2,2.3) );
			LineString linestring = _factory.CreateLineString( coordinates );
			string wkt = _writer.WriteFormatted(linestring);
			Assertion.AssertEquals("multi point","LINESTRING (1 2, 1.2 2.3)",wkt);
		}
		#endregion

		#region POLYGON
		//TOD: write POLYGON to WKT
		#endregion

		#region MULTILINESTRING
		//TOD: write MULTILINESTRING to WKT
		#endregion

		#region MULTIPOLYGON
		//TOD: write MULTIPOLYGON to WKT
		#endregion

		#region GEOMETRY
		//TOD: write GEOMETRY to WKT
		#endregion

		#region LINEARRING
		//TOD: write LINEARRING to WKT
		#endregion

		#region GEOMETRYCOLLECTION
		//TOD: write GEOMETRYCOLLECTION to WKT
		#endregion
	}
}


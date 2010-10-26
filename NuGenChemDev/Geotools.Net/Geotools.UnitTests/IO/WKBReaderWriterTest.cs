#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/WKBReaderWriterTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: WKBReaderWriterTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 4     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 3     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 2     10/29/02 11:27a Awcoats
 * 
 * 1     10/11/02 5:04p Rabergman
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
using Geotools.IO;
using Geotools.UnitTests.Utilities;
#endregion

namespace UrbansScience.Geographic.UnitTests.IO
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	[TestFixture]
	public class WKBReader 
	{
		

		#region Properties
		#endregion

		#region Methods
		#endregion

		#region Point

		public void test_Point()
		{
			string wkt= "POINT(40 60)";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("Point: ", true, result);
		}

		#endregion

		#region LineString
		public void test_Linestring()
		{
			string wkt= "LINESTRING(40 60, 40 320, 420 320, 420 60, 40 60)";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("Linestring: ", true, result);
		}
		#endregion

		#region Polygon
		public void test_Polygon()
		{
			string wkt= "POLYGON((40 60, 40 320, 420 320, 420 60, 40 60))";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("Polygon: ", true, result);
		}
		#endregion

		#region MultiPoint
		public void test_MultiPoint()
		{
			string wkt= "MULTIPOINT(40 60, 40 320, 420 320, 420 60, 40 60)";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("MultiPoint: ", true, result);
		}
		#endregion

		#region MultiLineString
		public void test_MultiLineString()
		{
			string wkt= "MULTILINESTRING((40 60, 40 320, 420 320, 420 60, 40 60),(20 30, 30 40))";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("MultiLineString: ", true, result);
		}
		#endregion

		#region MultiPolygon
		public void test_MultiPolygon()
		{
			string wkt= "MULTIPOLYGON(((10 13, 11 13, 12 13, 13 14, 14 15, 15 16, 15 17, 15 18, 14 19, 13 20, 12 21, 11 21, 10 21, 9 20, 8 19, 7 18, 7 17, 7 16, 8 15, 9 14, 10 13),(10 16, 11 17, 10 18, 9 17, 10 16)),((13 16, 14 17, 13 18, 12 17, 13 16)))";
			bool result = ReaderWriterTestHelper.TestHelper(wkt);
			Assertion.AssertEquals("MultiPolygon: ", true, result);
		}
		#endregion
	}
}

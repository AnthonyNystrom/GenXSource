#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/Shapefile/TestShapefileWriter.cs,v 1.1 2003/01/02 20:37:30 awcoats Exp $
 * $Log: TestShapefileWriter.cs,v $
 * Revision 1.1  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 3     12/27/02 1:09p Awcoats
 * nunit 1.0 to 2.0
 * 
 * 2     12/09/02 11:56a Awcoats
 * 
 * 1     11/27/02 10:28a Awcoats
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

namespace Geotools.UnitTests.IO.Shapefile
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.Shapefile.TestShapefileWriter class
	/// </summary>
	public class TestShapefileWriter  
	{
	
		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
			//TODO: Unit test constructor TestShapefileWriter myTestShapefileWriter = new TestShapefileWriter();
		}


		public void Test_TestRoundTripPolygons1() 
		{
			//TestRoundTrip("countries");
		}
		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_TestRoundTripPolygons2() 
		{
			//TestRoundTrip("statepop");
		}
		public void Test_TestRoundTripPolygons3() 
		{
			//TestRoundTrip("kreise");
		}
		public void Test_TestRoundTripPoints() 
		{
			//TestRoundTrip("uscities");
		}
		public void Test_TestRoundTripLines() 
		{
			//TestRoundTrip("roads");
		}

		public void TestRoundTrip(string filename)
		{
			//
			// can't round trip since I added the ToExternal/ ToInternal to the shapefile readers and writers.
			// 
			PrecisionModel pm = new PrecisionModel();
			GeometryFactory geometryFactory = new GeometryFactory(pm,-1);

			int differenceCount=0;
			string testName="";
			string srcShpFilename= Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\"+filename;
			string destShpFilename = Global.GetUnitTestRootDirectory()+@"\IO\Shapefile\Testfiles\testroundtrip"+filename;

			// do the round trip
			ShapefileReader shpReader = new ShapefileReader(srcShpFilename+".shp", geometryFactory);
			GeometryCollection shapes = shpReader.ReadAll();

			ShapefileWriter.Write(destShpFilename,shapes, geometryFactory);

			// perfom binary compare on the .shp 
			testName = String.Format("Test round trip .shp - {0}",filename);
			differenceCount = Compare.BinaryCompare(srcShpFilename+".shp", destShpFilename+".shp");
			Assertion.AssertEquals(testName,0,differenceCount);

			// perfom binary compare on the .shx file
			testName = String.Format("Test round trip .shx - {0}",filename);
			differenceCount = Compare.BinaryCompare(srcShpFilename+".shx", destShpFilename+".shx");
			Assertion.AssertEquals(testName,0,differenceCount);

		}
	}
}


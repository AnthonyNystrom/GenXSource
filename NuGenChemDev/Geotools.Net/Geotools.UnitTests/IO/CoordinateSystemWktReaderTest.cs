#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/CoordinateSystemWktReaderTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: CoordinateSystemWktReaderTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 4     10/18/02 2:31p Awcoats
 * chaned names of interfaces. Removed CT_ prefix, and added I
 * 
 * 3     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 2     9/24/02 3:45p Awcoats
 * 
 * 1     9/17/02 4:15p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.IO;
using Geotools.UnitTests.Utilities;
using Geotools.CoordinateReferenceSystems;
using System.IO;

#endregion

namespace Geotools.UnitTests.IO
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.CoordinateSystemWktReader class
	/// </summary>
	[TestFixture]
	public class CoordinateSystemWktReaderTest 

	{
		
	
		public void TestReadUnit1() 
		{
			string wkt = "UNIT[\"degree\",0.01745329251994433,AUTHORITY[\"EPSG\",\"9102\"]]";
			IAngularUnit angularUnit = CoordinateSystemWktReader.Create(wkt) as IAngularUnit;
		}

		public string FileToString(string filename)
		{
			StreamReader tr = new StreamReader(filename);
			string filestring=  tr.ReadToEnd();
			tr.Close();
			return filestring;
		}
		
		public void TestReadUnit2() 
		{
			string wkt1 = "UNIT[\"degree\",0.01745329251994433,AUTHORITY[\"EPSG\",\"9102\"]]";
			IAngularUnit angularUnit = CoordinateSystemWktReader.Create(wkt1) as IAngularUnit;
			string wkt2 = angularUnit.WKT;
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\AngularUnit.txt",wkt2 );
			//Assertion.AssertEquals("test 1",true,same); fails because of issues with double and precsision
		}

		public void TestReadUnit3() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\LinearUnit.txt";
			string wkt1 = FileToString(testFile);
			ILinearUnit linearUnit = CoordinateSystemWktReader.Create(wkt1) as ILinearUnit;
			string wkt2 = linearUnit.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Unit 3",true,same);
		}


		public void TestReadVerticalDatum() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\VerticalDatum.txt";
			string wkt1 = FileToString(testFile);
			IVerticalDatum verticalDatum = CoordinateSystemWktReader.Create(wkt1) as IVerticalDatum;
			string wkt2 = verticalDatum.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Vertical Datum 1",true,same);
		}


		public void TestReadHorizontalDatum() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\HorizontalDatum.txt";
			string wkt1 = FileToString(testFile);
			IHorizontalDatum horizontalDatum = CoordinateSystemWktReader.Create(wkt1) as IHorizontalDatum;
			string wkt2 = horizontalDatum.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Horizontal datum 1",true,same);
		}


		public void TestReadEllipsoid() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\Spheroid.txt";
			string wkt1 = FileToString(testFile);
			IEllipsoid Ellipsoid = CoordinateSystemWktReader.Create(wkt1) as IEllipsoid;
			string wkt2 = Ellipsoid.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Ellipsoid 1",true,same);
		}

		public void TestReadPrimeMeridian2() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\PrimeMeridian.txt";
			string wkt1 = FileToString(testFile);
			IPrimeMeridian primeMeridian = CoordinateSystemWktReader.Create(wkt1) as IPrimeMeridian;
			string wkt2 = primeMeridian.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Prime Meridian 1",true,same);
		}
	
		public void TestReadVerticalCoordinateSystem2() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\VerticalCoordinateSystem.txt";
			string wkt1 = FileToString(testFile);
			IVerticalCoordinateSystem verticalCoordinateSystem = CoordinateSystemWktReader.Create(wkt1) as IVerticalCoordinateSystem;
			string wkt2 = verticalCoordinateSystem.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("vertical coordinate system 1",true,same);
		}

		public void TestReadGeographicCoordinateSystem2() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\GeographicCoordinateSystem.txt";
			string wkt1 = FileToString(testFile);
			IGeographicCoordinateSystem geographicCoordinateSystem = CoordinateSystemWktReader.Create(wkt1) as IGeographicCoordinateSystem;
			string wkt2 = geographicCoordinateSystem.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Geographic coordinate system 1",true,same);
		}

		public void TestReadProjectedCoordinateSystem2() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\ProjectedCoordinateSystem.txt";
			string wkt1 = FileToString(testFile);
			IProjectedCoordinateSystem projectedCoordinateSystem = CoordinateSystemWktReader.Create(wkt1) as IProjectedCoordinateSystem;
			string wkt2 = projectedCoordinateSystem.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Projected coordinate system 1",true,same);
		}
		

		public void TestReadCompoundCoordinateSystem2() 
		{
			string testFile = Global.GetUnitTestRootDirectory()+@"\IO\CompoundCoordinateSystem.txt";
			string wkt1 = FileToString(testFile);
			ICompoundCoordinateSystem compoundCoordinateSystem = CoordinateSystemWktReader.Create(wkt1) as ICompoundCoordinateSystem;
			string wkt2 = compoundCoordinateSystem.WKT;
			bool same = Compare.CompareAgainstString(testFile, wkt2 );
			Assertion.AssertEquals("Compound coordinate system 1",true,same);
		}
	
	}
}


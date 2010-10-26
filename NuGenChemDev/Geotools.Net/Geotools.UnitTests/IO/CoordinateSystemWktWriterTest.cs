#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/CoordinateSystemWktWriterTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: CoordinateSystemWktWriterTest.cs,v $
 * Revision 1.2  2003/01/02 20:37:30  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 4     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 3     10/18/02 2:31p Awcoats
 * chaned names of interfaces. Removed CT_ prefix, and added I
 * 
 * 2     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 1     9/17/02 4:15p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.CodeDom.Compiler;
using NUnit.Framework;
using Geotools.CoordinateReferenceSystems;
using Geotools.IO;
using Geotools.UnitTests.Utilities;
#endregion

namespace Geotools.UnitTests
{

	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystemWktWriterTest class
	/// </summary>
	[TestFixture]
	public class CoordinateSystemWktWriterTest 
	{
		CoordinateSystemEPSGFactory _factory = new CoordinateSystemEPSGFactory( Global.GetEPSGDatabaseConnection() );

		

	
		public void TestWriteCompoundCoordinateSystem() 
		{
			ICompoundCoordinateSystem compoundsCoordinateSsytem = _factory.CreateCompoundCoordinateSystem("7405");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			//CoordinateSystemWktWriter.WriteCompoundCoordinateSystem(compoundsCoordinateSsytem, indentedTextWriter);
			CoordinateSystemWktWriter.Write(compoundsCoordinateSsytem, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\CompoundCoordinateSystem.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

	
		public void TestWriteUnit1() 
		{
			ILinearUnit linearUnit = _factory.CreateLinearUnit("9001");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(linearUnit, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\LinearUnit.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}
		
		public void TestWriteUnit2() 
		{
			IAngularUnit AngularUnit = _factory.CreateAngularUnit("9101");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(AngularUnit, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\Radians.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

	
		public void TestWriteVerticalDatum() 
		{
			IVerticalDatum verticalDatum = _factory.CreateVerticalDatum("5101");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(verticalDatum, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\VerticalDatum.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		
		public void TestWriteDatum() 
		{
			IHorizontalDatum horizontalDatum = _factory.CreateHorizontalDatum("6277");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(horizontalDatum, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\HorizontalDatum.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWriteEllipsoid()
		{
			IEllipsoid ellipsoid = _factory.CreateEllipsoid("7001");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(ellipsoid, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\Spheroid.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}
		public void TestWritePrimeMeridian()
		{
			IPrimeMeridian PrimeMeridian = _factory.CreatePrimeMeridian("8901");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(PrimeMeridian, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\PrimeMeridian.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWriteGeographicCoordinateSystem()
		{
			IGeographicCoordinateSystem GeographicCoordinateSystem = _factory.CreateGeographicCoordinateSystem("4277");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(GeographicCoordinateSystem, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\GeographicCoordinateSystem.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWriteProjectedCoordinateSystem()
		{
			IProjectedCoordinateSystem ProjectedCoordinateSystem = _factory.CreateProjectedCoordinateSystem("27700");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(ProjectedCoordinateSystem, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\ProjectedCoordinateSystem.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWriteVerticalCoordinateSystem()
		{
			IVerticalCoordinateSystem VerticalCoordinateSystem = _factory.CreateVerticalCoordinateSystem("5701");
			TextWriter textwriter = new StringWriter();
			IndentedTextWriter indentedTextWriter = new IndentedTextWriter(textwriter);
			CoordinateSystemWktWriter.Write(VerticalCoordinateSystem, indentedTextWriter);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\VerticalCoordinateSystem.txt", textwriter.ToString() );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWrite1()
		{
			ICompoundCoordinateSystem compoundsCoordinateSsytem = _factory.CreateCompoundCoordinateSystem("7405");
			string wkt = CoordinateSystemWktWriter.Write(compoundsCoordinateSsytem);
			bool same = Compare.CompareAgainstString(Global.GetUnitTestRootDirectory()+@"\IO\CompoundCoordinateSystem.txt", wkt );
			Assertion.AssertEquals("test 1",true,same);
		}

		public void TestWrite2()
		{
			ICompoundCoordinateSystem compoundsCoordinateSsytem = _factory.CreateCompoundCoordinateSystem("7405");
			string wkt1 = CoordinateSystemWktWriter.Write(compoundsCoordinateSsytem);
			string wkt2 = compoundsCoordinateSsytem.WKT;
			Assertion.AssertEquals("test 1",wkt1,wkt2);
		}

		
		

	}
	
}


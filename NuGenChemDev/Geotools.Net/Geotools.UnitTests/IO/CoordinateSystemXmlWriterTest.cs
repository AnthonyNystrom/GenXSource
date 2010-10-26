#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/IO/CoordinateSystemXmlWriterTest.cs,v 1.2 2003/01/02 20:37:30 awcoats Exp $
 * $Log: CoordinateSystemXmlWriterTest.cs,v $
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
 * 2     9/25/02 9:18a Awcoats
 * 
 * 1     9/17/02 4:15p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.CoordinateReferenceSystems;
using Geotools.IO;
#endregion

namespace Geotools.UnitTests.IO
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.IO.CoordinateSystemXmlWriter class
	/// </summary>
	[TestFixture]
	public class CoordinateSystemXmlWriterTest  
	{
		CoordinateSystemEPSGFactory _factory = new CoordinateSystemEPSGFactory( Global.GetEPSGDatabaseConnection() );

		

		/* these tests should not throw any exceptions. 
		 * to test whether the correct Xml is produced, this is part of the XmlReader tests - where
		 * xml is read, written, and the results compared to the original file i.e. round-tripping.
		 * the results should be the same.
		 * */ 
	
		public void TestWriteLinearUnit() 
		{
			ILinearUnit linearUnit = _factory.CreateLinearUnit("9001");
			string test1= CoordinateSystemXmlWriter.Write(linearUnit);
		}

		public void TestWriteAngularUnit() 
		{
			IAngularUnit angularUnit = _factory.CreateAngularUnit("9101");
			string test1= CoordinateSystemXmlWriter.Write(angularUnit);
		}

		public void TestCompoundCoordinateSystem() 
		{
			ICompoundCoordinateSystem compoundCoordinateSystem = _factory.CreateCompoundCoordinateSystem("7405");
			string test1= CoordinateSystemXmlWriter.Write(compoundCoordinateSystem);	
		}

		public void TestProjectedCoordinateSystem() 
		{
			IProjectedCoordinateSystem projectedCoordinateSystem = _factory.CreateProjectedCoordinateSystem("27700");
			string test1= CoordinateSystemXmlWriter.Write(projectedCoordinateSystem);
		}

	}
}


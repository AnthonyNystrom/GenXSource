#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/LinearUnitTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: LinearUnitTest.cs,v $
 * Revision 1.2  2003/01/02 20:31:57  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:00p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 4     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 3     10/18/02 12:54p Rabergman
 * Removed tests due to internal classes
 * 
 * 2     9/24/02 3:45p Awcoats
 * 
 * 1     8/02/02 11:01a Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.CoordinateReferenceSystems;
#endregion

namespace Geotools.UnitTests.CoordinateSystems
{
	/// <summary>
	/// Tests the basic functionality of the UrbanScience.OpenGIS.UnitTests.LinearUnitTest class
	/// </summary>
	[TestFixture]
	public class LinearUnitTest 
	{
		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			OGC.CoordinateSystems.ILinearUnit linearunit = new LinearUnit(1.0,"remarks","authority","authoritycode","name","alias","abbreviation");
//			AssertEquals("ctor 1. ", "abbreviation", linearunit.Abbreviation);
//			AssertEquals("ctor 2. ", "alias", linearunit.Alias);
//			AssertEquals("ctor 1. ", "authority", linearunit.Authority);
//			AssertEquals("ctor 1. ", "authoritycode", linearunit.AuthorityCode);
//			AssertEquals("ctor 1. ", "name", linearunit.Name);
//			AssertEquals("ctor 1. ", 1.0, linearunit.MetersPerUnit);
//			AssertEquals("ctor 1. ", "remarks", linearunit.Remarks);
		}

		public void Test_Constructor2() 
		{
//			OGC.CoordinateSystems.ILinearUnit linearunit = new LinearUnit(1.0);
//			AssertEquals("ctor 1. ", 1.0, linearunit.MetersPerUnit);
		}
	}
}


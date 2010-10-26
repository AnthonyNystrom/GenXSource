#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/AngularUnitTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: AngularUnitTest.cs,v $
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
 * 3     10/18/02 12:40p Rabergman
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
	/// Tests the basic functionality of the UrbanScience.OpenGIS.UnitTests.AngularUnitTest class
	/// </summary>
	[TestFixture]
	public class AngularUnitTest 
	{
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			OGC.CoordinateSystems.IAngularUnit angularunit = new AngularUnit(1.0,"remarks","authority","authoritycode","name","alias","abbreviation");
//			Assertion.AssertEquals("ctor 1. ", "abbreviation", angularunit.Abbreviation);
//			Assertion.AssertEquals("ctor 2. ", "alias", angularunit.Alias);
//			Assertion.AssertEquals("ctor 3. ", "authority", angularunit.Authority);
//			Assertion.AssertEquals("ctor 4. ", "authoritycode", angularunit.AuthorityCode);
//			Assertion.AssertEquals("ctor 5. ", "name", angularunit.Name);
//			Assertion.AssertEquals("ctor 6. ", 1.0, angularunit.RadiansPerUnit);
//			Assertion.AssertEquals("ctor 7. ", "remarks", angularunit.Remarks);
//			//Assertion.AssertEquals("ctor 8. ", null, angularunit.WKT);
//			//Assertion.AssertEquals("ctor 9. ", null, angularunit.XML);		
		}

	
		public void Test_Constructor2() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			OGC.CoordinateSystems.IAngularUnit angularunit = new AngularUnit(1.0);
//			Assertion.AssertEquals("ctor 1. ", 1.0, angularunit.RadiansPerUnit);
		}
	}
}


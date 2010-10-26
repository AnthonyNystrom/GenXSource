#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/DatumTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: DatumTest.cs,v $
 * Revision 1.2  2003/01/02 20:31:57  awcoats
 * *** empty log message ***
 *
 * 
 * 7     12/27/02 1:00p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 6     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 5     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 4     10/18/02 12:47p Rabergman
 * Removed tests due to internal classes
 * 
 * 3     9/24/02 3:45p Awcoats
 * 
 * 2     9/13/02 8:43a Awcoats
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
	/// Tests the basic functionality of the UrbanScience.OpenGIS.UnitTests.DatumTest class
	/// </summary>
	[TestFixture]
	public class DatumTest 
	{

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//			OGC.CoordinateSystems.IDatum datum = new Datum(IDatumType.IHD_Geocentric,"remarks","authority","authoritycode","name","alias","abbreviation");
//			Assertion.AssertEquals("ctor 0. ", IDatumType.IHD_Geocentric, datum.DatumType);
//			Assertion.AssertEquals("ctor 1. ", "abbreviation", datum.Abbreviation);
//			Assertion.AssertEquals("ctor 2. ", "alias", datum.Alias);
//			Assertion.AssertEquals("ctor 3. ", "authority", datum.Authority);
//			Assertion.AssertEquals("ctor 4. ", "authoritycode", datum.AuthorityCode);
//			Assertion.AssertEquals("ctor 5. ", "name", datum.Name);
//			Assertion.AssertEquals("ctor 6. ", "remarks", datum.Remarks);
//			//Assertion.AssertEquals("ctor 7. ", null, datum.WKT);
//			//Assertion.AssertEquals("ctor 8. ", null, datum.XML);	
		}

		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_TestConstructor2()
		{
//			OGC.CoordinateSystems.IDatum datum = new Datum(IDatumType.IHD_Geocentric);
//			Assertion.AssertEquals("ctor 0. ", IDatumType.IHD_Geocentric, datum.DatumType);
		}
	}
}


#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/CompoundCoordinateSystemTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: CompoundCoordinateSystemTest.cs,v $
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
 * 3     10/18/02 12:42p Rabergman
 * Removed tests due to internal classes
 * 
 * 2     9/24/02 3:45p Awcoats
 * 
 * 1     9/18/02 11:25a Awcoats
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
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.CompoundCoordinateSystemTest class
	/// </summary>
	[TestFixture]
	public class CompoundCoordinateSystemTest 
	{
		CoordinateSystemEPSGFactory _factory;
		

		public CompoundCoordinateSystemTest()
		{
			_factory = new CoordinateSystemEPSGFactory(Global.GetEPSGDatabaseConnection());
		}
	
		

	
		public void Test_Constructor1() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			ICoordinateSystem headCRS = _factory.CreateProjectedCoordinateSystem("27700");
//			ICoordinateSystem tailCRS = _factory.CreateVerticalCoordinateSystem("5701");
//			CompoundCoordinateSystem compoundCS = new CompoundCoordinateSystem(headCRS,
//				tailCRS,
//				"remarks",
//				"authority",
//				"code",
//				"name",
//				"alias",
//				"abbreviation");
//			Assertion.AssertEquals("ctor1.",headCRS,compoundCS.HeadCS);
//			Assertion.AssertEquals("ctor2.",tailCRS,compoundCS.TailCS);
//			Assertion.AssertEquals("ctor3.","remarks",compoundCS.Remarks);
//			Assertion.AssertEquals("ctor3.","authority",compoundCS.Authority);
//			Assertion.AssertEquals("ctor3.","code",compoundCS.AuthorityCode);
//			Assertion.AssertEquals("ctor3.","name",compoundCS.Name);
//			Assertion.AssertEquals("ctor3.","alias",compoundCS.Alias);
//			Assertion.AssertEquals("ctor3.","abbreviation",compoundCS.Abbreviation);
		}

		
		public void Test_Constructor2() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test sre functioning properly otherwise.)
//			ICoordinateSystem headCRS = null;
//			ICoordinateSystem tailCRS = _factory.CreateVerticalCoordinateSystem("5701");
//			try
//			{
//				CompoundCoordinateSystem compoundCS = new CompoundCoordinateSystem(headCRS,
//					tailCRS,
//					"remarks",
//					"authority",
//					"code",
//					"name",
//					"alias",
//					"abbreviation");
//				Assertion.Fail("Argumentexception should be thrown.");
//			}
//			catch(ArgumentNullException)
//			{
//			}
		}
		
		public void Test_Constructor3() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test sre functioning properly otherwise.)
//			ICoordinateSystem headCRS = _factory.CreateProjectedCoordinateSystem("27700");;
//			ICoordinateSystem tailCRS = null;
//			try
//			{
//				CompoundCoordinateSystem compoundCS = new CompoundCoordinateSystem(headCRS,
//					tailCRS,
//					"remarks",
//					"authority",
//					"code",
//					"name",
//					"alias",
//					"abbreviation");
//				Assertion.Fail("Argumentexception should be thrown.");
//			}
//			catch(ArgumentNullException)
//			{
//			}
		}

		
	}
}


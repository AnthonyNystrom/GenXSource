#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/VerticalCoordinateSystemTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: VerticalCoordinateSystemTest.cs,v $
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
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.VerticalCoordinateSystemTest class
	/// </summary>
	[TestFixture]
	public class VerticalCoordinateSystemTest 
	{

		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			VerticalDatum datum = VerticalDatum.Ellipsoidal;
//			IAxisInfo axis = AxisInfo.Altitude;
//			ILinearUnit unit = LinearUnit.Meters;
//			
//			VerticalCoordinateSystem vcs = new VerticalCoordinateSystem("test1",datum, axis, unit);
//			AssertEquals("Test1",datum, vcs.VerticalDatum);
//			AssertEquals("Test2",1.0,vcs.VerticalUnit.MetersPerUnit);
//			AssertEquals("ctor. 3",unit, vcs.VerticalUnit);
//			AssertEquals("ctor. 4",axis, vcs.GetAxis(0));
		}


		public void Test_StaticConstructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			IVerticalCoordinateSystem vcs = VerticalCoordinateSystem.Ellipsoidal;
//			AssertEquals("Test1","Ellipsoidal",vcs.Name);
		}

	}
}


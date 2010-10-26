#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/HorizontalDatumTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: HorizontalDatumTest.cs,v $
 * Revision 1.2  2003/01/02 20:31:57  awcoats
 * *** empty log message ***
 *
 * 
 * 9     12/27/02 1:00p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 8     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 7     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 6     10/18/02 12:54p Rabergman
 * Removed tests due to internal classes
 * 
 * 5     9/24/02 3:45p Awcoats
 * 
 * 4     9/18/02 5:33p Awcoats
 * 
 * 3     9/13/02 8:43a Awcoats
 * 
 * 2     8/15/02 11:21a Awcoats
 * 
 * 1     8/14/02 2:21p Awcoats
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
	/// Tests the basic functionality of the UrbanScience.OpenGIS.UnitTests.CoordinateSystems.HorizontalDatum class
	/// </summary>
	[TestFixture]
	public class HorizontalDatumTest  
	{
		
		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			IEllipsoid ellipsoid = new Ellipsoid(20926348,-1.0,294.26068,true, new LinearUnit(1));
//		
//			IWGS84ConversionInfo wgsInfo = new IWGS84ConversionInfo();
//			wgsInfo.Dx=1.0;
//			HorizontalDatum horizontalDatum = new HorizontalDatum("name",IDatumType.IHD_Geocentric,ellipsoid, ref wgsInfo);
//			AssertEquals("test 1","name",horizontalDatum.Name);
//			AssertEquals("test 2",IDatumType.IHD_Geocentric,horizontalDatum.DatumType);
//			AssertEquals("test 3",ellipsoid,horizontalDatum.Ellipsoid);
//			AssertEquals("test 4",wgsInfo,horizontalDatum.WGS84Parameters);
		}

	}
}


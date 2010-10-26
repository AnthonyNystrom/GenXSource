#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/ProjectedCoordinateSystemTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: ProjectedCoordinateSystemTest.cs,v $
 * Revision 1.2  2003/01/02 20:31:57  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:00p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 4     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 3     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 2     10/18/02 12:54p Rabergman
 * Removed tests due to internal classes
 * 
 * 1     9/24/02 3:44p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;

using Geotools.CoordinateReferenceSystems;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.UnitTests.CoordinateSystems
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.ProjectedCoordinateSystemTest class
	/// </summary>
	public class ProjectedCoordinateSystemTest 
	{
		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			ICoordinateSystemFactory csFactory = new CoordinateSystemFactory();
//			IAngularUnit angularUnit = new AngularUnit(1);
//			ILinearUnit linearUnit = new LinearUnit(1);
//			IEllipsoid ellipsoid = csFactory.CreateFlattenedSphere("test",1,2, linearUnit );
//			IAxisInfo axis0 = new IAxisInfo();
//			axis0.Name="axis0name";
//			IAxisInfo axis1 = new IAxisInfo();
//			axis1.Name="axis1name";
//			IWGS84ConversionInfo wgs = new IWGS84ConversionInfo();
//			
//			IPrimeMeridian primeMeridian = csFactory.CreatePrimeMeridian("name", angularUnit,2.0);
//			IHorizontalDatum horizontalDatum = csFactory.CreateHorizontalDatum("datum",IDatumType.IHD_Geocentric,ellipsoid, ref wgs);
//			IGeographicCoordinateSystem gcs = csFactory.CreateGeographicCoordinateSystem("name",angularUnit, horizontalDatum, primeMeridian, ref axis0, ref axis1);
//
//			
//			//PrimeMeridian primeMeridian = new PrimeMeridian("name", angularUnit, 0.5);
//			IAxisInfo[] axisArray = new IAxisInfo[2];
//			axisArray[0]=axis0;
//			axisArray[1]=axis1;
//
//			ProjectionParameter[] paramList = new ProjectionParameter[1];
//			paramList[0].Name="test";
//			paramList[0].Value=2.2;
//
//			Projection projection = new Projection("mercator",paramList,"class","remarks","authority","authoritycode");
//
//			ProjectedCoordinateSystem pjc = new ProjectedCoordinateSystem(horizontalDatum,
//				axisArray,gcs, linearUnit, projection,
//				"remarks","authority","authorityCode","name","alias","abbreviation");
//
//		
//			AssertEquals("Test 1",linearUnit,pjc.LinearUnit);
//			AssertEquals("Test 2",horizontalDatum,pjc.HorizontalDatum);
//			AssertEquals("Test 3",axis0,pjc.GetAxis(0));
//			AssertEquals("Test 4",axis1,pjc.GetAxis(1));
//			AssertEquals("Test 5",gcs,pjc.GeographicCoordinateSystem);
//
//			AssertEquals("Test 6", "abbreviation", pjc.Abbreviation);
//			AssertEquals("Test 7", "alias", pjc.Alias);
//			AssertEquals("Test 8", "authority", pjc.Authority);
//			AssertEquals("Test 9", "authorityCode", pjc.AuthorityCode);
//			AssertEquals("Test 10", "name", pjc.Name);
//			AssertEquals("Test 11", "remarks", pjc.Remarks);
		}

	}
}


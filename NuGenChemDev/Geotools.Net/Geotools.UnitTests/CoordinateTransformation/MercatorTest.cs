#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateTransformation/MercatorTest.cs,v 1.2 2003/01/02 20:32:13 awcoats Exp $
 * $Log: MercatorTest.cs,v $
 * Revision 1.2  2003/01/02 20:32:13  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 4     10/18/02 1:43p Awcoats
 * interface name change.
 * 
 * 3     10/18/02 12:56p Rabergman
 * Removed tests due to internal classes
 * 
 * 2     9/24/02 3:45p Awcoats
 * 
 * 1     9/18/02 5:23p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.CoordinateReferenceSystems;
using Geotools.CoordinateTransformations;
using Geotools.Utilities;
#endregion

namespace Geotools.UnitTests.CoordinateSystems
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.MercatorTest class
	/// </summary>
	[TestFixture]
	public class MercatorTest 
	{
		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			double major = 6378137.0;
//			double minor = 6356752.314140;
//			Ellipsoid ellipsoid = new Ellipsoid(major,minor,-1.0,false,LinearUnit.Meters);
//			ParameterList parameters = new ParameterList();
//			parameters.Add("latitude_of_natural_origin",49.0);
//			parameters.Add("longitude_of_natural_origin",-2.0);
//			parameters.Add("scale_factor_at_natural_origin",0.999601272);
//			parameters.Add("false_easting",400000.0);
//			parameters.Add("false_northing",-100000.0);
//			parameters.Add("semi_major",ellipsoid.SemiMajorAxis);
//			parameters.Add("semi_minor",ellipsoid.SemiMinorAxis);
//
//
//			MercatorProjection meractor = new MercatorProjection(parameters);
//
//			double long1 = 1.5;
//			double lat1 = 52.2;
//			PT_CoordinatePoint pt = new PT_CoordinatePoint();
//			pt.Ord = new Double[2];
//			pt.Ord[0] = long1;//AngularUnit.DegreesToRadians(long1);
//			pt.Ord[1] = lat1;//AngularUnit.DegreesToRadians(lat1);
//
//			PT_CoordinatePoint result1 = meractor.Transform(ref pt);
//
//			double metersX = (double)result1.Ord[0];
//			double metersY = (double)result1.Ord[1];
//
//			//AssertEquals("Mercator Transform X",2757786.0359389116,metersX);
//		//	AssertEquals("Mercator Transform Y",2384981.5331703457,metersY);
//
//			PT_CoordinatePoint result2 = meractor.Inverse().Transform(ref result1);
//				
//			double long2=(double)result2.Ord[0];
//			double lat2= (double)result2.Ord[1];
//
//			//AssertEquals("Mercator InverseTransformPoint X",34.3,long2);
//			//AssertEquals("Mercator InverseTransformPoint Y",32.199999999999996,lat2);	
		}
		
		public void Test_InverseTransform()
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			ParameterList parameters = new ParameterList();
//			parameters.Add("latitude_of_natural_origin",2.0);
//			parameters.Add("longitude_of_natural_origin",49.0);
//			parameters.Add("scale_factor_at_natural_origin",0.999601272);
//			parameters.Add("false_easting",400000.0);
//			parameters.Add("false_northing",-100000.0);
//			parameters.Add("semi_major",6377563.396);
//			parameters.Add("semi_minor",6377563.396);
//
//
//			MercatorProjection meractor = new MercatorProjection(parameters);
//			double metersX = 2757786.0359389116;
//			double metersY = 2384981.5331703457;
//			PT_CoordinatePoint pt = new PT_CoordinatePoint();
//			pt.Ord = new Double[2];
//			pt.Ord[0] = metersX;
//			pt.Ord[1] = metersY;
//			
//			// this is the line we want to test. Given a projection, get the inverse and transform a coordinate.
//			PT_CoordinatePoint result2 = meractor.Inverse().Transform(ref pt);
//			double long2= Radians.ToDegrees((double)result2.Ord[0]);
//			double lat2= Radians.ToDegrees((double)result2.Ord[1]);
//
//			//AssertEquals("Mercator InverseTransformPoint X",34.3,long2);
//			//AssertEquals("Mercator InverseTransformPoint Y",32.199999999999996,lat2);	
		}
		/// <summary>
		/// Test getting and setting the properties
		/// </summary>
		public void Test_TestProperties() 
		{
			//TODO: Unit test getting and setting properties
		}
	}
}


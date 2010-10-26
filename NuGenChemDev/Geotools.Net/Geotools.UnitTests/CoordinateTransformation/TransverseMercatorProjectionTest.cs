#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateTransformation/TransverseMercatorProjectionTest.cs,v 1.2 2003/01/02 20:32:13 awcoats Exp $
 * $Log: TransverseMercatorProjectionTest.cs,v $
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
	public class TransverseMercatorProjectionTest 
	{

		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			ParameterList parameters = new ParameterList();
//			parameters.Add("latitude_of_natural_origin",49.0);
//			parameters.Add("longitude_of_natural_origin",-2.0);
//			parameters.Add("scale_factor_at_natural_origin",0.999601272);
//			parameters.Add("false_easting",400000.0);
//			parameters.Add("false_northing",-100000.0);
//			parameters.Add("semi_major",6377563.396);
//			parameters.Add("semi_minor",6377563.396);
//
//			TransverseMercatorProjection meractor = new TransverseMercatorProjection(parameters);
//
//			double long1 = -2.0;
//			double lat1 = 49.0;
//			PT_CoordinatePoint pt = new PT_CoordinatePoint();
//			pt.Ord = new Double[2];
//			pt.Ord.SetValue(long1,0);
//			pt.Ord.SetValue(lat1,1);
//
//			PT_CoordinatePoint result1 = meractor.Transform(ref pt);
//
//			double metersX = (double)result1.Ord[0];
//			double metersY = (double)result1.Ord[1];
//
//			AssertEquals("Transverse Mercator Transform X","400000",metersX.ToString());
//			AssertEquals("Transverse Mercator Transform Y","-100000",metersY.ToString());
//
//			PT_CoordinatePoint result2 = meractor.Inverse().Transform(ref result1);
//				
//			double long2= (double)result2.Ord[0];
//			double lat2= (double)result2.Ord[1];
//
//
//			AssertEquals("Transverse Mercator InverseTransformPoint X","-2",long2.ToString());
//			AssertEquals("TransverseMercator InverseTransformPoint Y","49",lat2.ToString());	
		}
		
	
	}
}


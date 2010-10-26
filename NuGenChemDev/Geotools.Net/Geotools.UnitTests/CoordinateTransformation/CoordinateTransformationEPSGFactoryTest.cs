#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateTransformation/CoordinateTransformationEPSGFactoryTest.cs,v 1.2 2003/01/02 20:32:12 awcoats Exp $
 * $Log: CoordinateTransformationEPSGFactoryTest.cs,v $
 * Revision 1.2  2003/01/02 20:32:12  awcoats
 * *** empty log message ***
 *
 * 
 * 8     1/02/03 10:51a Awcoats
 * fixed unit test failure regarding open connection,
 * 
 * 7     12/27/02 1:15p Awcoats
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
 * 3     9/25/02 2:00p Awcoats
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
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Geotools.Positioning;
using Geotools.CoordinateReferenceSystems;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.UnitTests.CoordinateTransformation
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.CoordinateTransformationEPSGFactoryTest class
	/// </summary>
	[TestFixture]
	public class CoordinateTransformationEPSGFactoryTest
	{
		CoordinateTransformationEPSGFactory _CTfactory;
		CoordinateSystemEPSGFactory _CRSfactory;
		
		public CoordinateTransformationEPSGFactoryTest() 
		{
			IDbConnection connection = Global.GetEPSGDatabaseConnection();
			_CTfactory = new CoordinateTransformationEPSGFactory(connection);
			_CRSfactory = new CoordinateSystemEPSGFactory(connection);
		}
	
		

		#region CreateFromTransformationCode
		public void TestCreateFromTransformationCode1()
		{
			ICoordinateTransformation UKNationalGrid1 = _CTfactory.CreateFromTransformationCode("1036");

			double long1 = -2;
			double lat1 = 49;
			CoordinatePoint pt = new CoordinatePoint();
			pt.Ord = new Double[2];
			pt.Ord[0] = long1;
			pt.Ord[1] = lat1;

			CoordinatePoint result1 = UKNationalGrid1.MathTransform.Transform( pt);

			double metersX = (double)result1.Ord[0];
			double metersY = (double)result1.Ord[1];

			Assertion.AssertEquals("Transverse Mercator Transform X","400000",metersX.ToString());
			Assertion.AssertEquals("Transverse Mercator Transform Y","-100000",metersY.ToString());

			CoordinatePoint result2 = UKNationalGrid1.MathTransform.GetInverse().Transform( result1);
				
			double long2= (double)result2.Ord[0];
			double lat2= (double)result2.Ord[1];

			Assertion.AssertEquals("Transverse Mercator InverseTransformPoint X","-2",long2.ToString());
			Assertion.AssertEquals("TransverseMercator InverseTransformPoint Y","49",lat2.ToString());	
		}
		public void TestCreateFromTransformationCode2()
		{
			ICoordinateTransformation UKNationalGrid1 = _CTfactory.CreateFromTransformationCode("1681");
			double long1 = 2.5;
			double lat1 = 53.2;
			CoordinatePoint pt = new CoordinatePoint();
			pt.Ord = new Double[2];
			pt.Ord[0] = long1;
			pt.Ord[1] = lat1;

			CoordinatePoint result1 = UKNationalGrid1.MathTransform.Transform(pt);

			double metersX = (double)result1.Ord[0];
			double metersY = (double)result1.Ord[1];
		}
		public void TestCreateFromTransformationCode3()
		{
			try 
			{
				ICoordinateTransformation UKNationalGrid1 = _CTfactory.CreateFromTransformationCode("-1");
				Assertion.Fail("Excpetion should be thrown.");
			}
			catch(ArgumentException)
			{
				
			}
			
		}
		#endregion

		#region CreateCoordinateOperation
		public void TestCreateCoordinateOperation()
		{
			//ICoordinateTransformation utm32 = _CTfactory.CreateFromCoordinateSystemCodes("32632","4326");
			//TODO: see if the numbers returned are correct.
		}
		#endregion
	}
}


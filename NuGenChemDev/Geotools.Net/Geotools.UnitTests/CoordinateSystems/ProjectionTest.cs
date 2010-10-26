#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateSystems/ProjectionTest.cs,v 1.2 2003/01/02 20:31:57 awcoats Exp $
 * $Log: ProjectionTest.cs,v $
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

#endregion

namespace Geotools.UnitTests.CoordinateSystems
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.ProjectionTest class
	/// </summary>
	[TestFixture]
	public class ProjectionTest
	{
		

		/// <summary>
		/// Tests the constructor
		/// </summary>
		public void Test_Constructor() 
		{
//(Note: Test must be commented out because this class has been made internal.  All test are functioning properly otherwise.)
//			ProjectionParameter[] paramList = new ProjectionParameter[2];
//			paramList[0].Name="test";
//			paramList[0].Value=2.2;
//			paramList[1].Name="test2";
//			paramList[1].Value=2.3;
//
//			Projection projection = new Projection("mercator",paramList,"class","remarks","authority","authoritycode");
//
//			AssertEquals("test 1","mercator",projection.Name);
//			AssertEquals("test 2","class",projection.ClassName);
//			AssertEquals("test 3",2,projection.NumParameters);
//			AssertEquals("test 4a",2.2,projection.get_Parameter(0).Value);
//			AssertEquals("test 4b","test",projection.get_Parameter(0).Name);
//			AssertEquals("test 5a",2.3,projection.get_Parameter(1).Value);
//			AssertEquals("test 5b","test2",projection.get_Parameter(1).Name);
//			AssertEquals("test 6","remarks",projection.Remarks);
//			AssertEquals("test 7","authority",projection.Authority);
//			AssertEquals("Test 8","authoritycode",projection.AuthorityCode);
		}
	}
}


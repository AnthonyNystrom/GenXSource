#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/CoordinateTransformation/ParameterList.cs,v 1.2 2003/01/02 20:32:13 awcoats Exp $
 * $Log: ParameterList.cs,v $
 * Revision 1.2  2003/01/02 20:32:13  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:15p Awcoats
 * 
 * 4     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 3     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
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
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.UnitTests.CoordinateSystems
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.CoordinateSystems.ParameterList class
	/// </summary>
	[TestFixture]
	public class ParameterListTest 
	{
		
		public void Test_Test1()
		{
			ParameterList paramList = new ParameterList();
			paramList.Add("central_meridian",34.0);
			double centralMeridian1 = paramList.GetDouble("central_meridian");
			Assertion.AssertEquals("Get 1",34.0,centralMeridian1);
			double centralMeridian2 = paramList.GetDouble("central_meridianmissing",4.0);
			Assertion.AssertEquals("Get 1",4.0,centralMeridian2);
		}
	}
}


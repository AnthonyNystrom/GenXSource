#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/GeometryTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: GeometryTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 4     11/27/02 11:18a Awcoats
 * added one more test to check for different preciion model.s
 * 
 * 3     11/04/02 3:20p Rabergman
 * Changed namespaces
 * 
 * 2     11/04/02 12:11p Rabergman
 * Changed Namespace
 * 
 * 1     9/17/02 3:07p Rabergman
 * 
 * 1     8/21/02 2:49p Rabergman
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Geometries
{
	/// <summary>
	/// Summary description for CoordinateTest.
	/// </summary>
	[TestFixture]
	public class GeometryTest 
	{
		//set up variables that will be reused often.
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);

		

		public void test_Constructor()
		{
		}

	}
}

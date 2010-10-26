#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/CoordinatesTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: CoordinatesTest.cs,v $
 * Revision 1.1  2003/01/02 20:32:36  awcoats
 * *** empty log message ***
 *
 * 
 * 4     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
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
	public class CoordinatesTest 
	{
		

		private Coordinates Coords1()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate();
			for(int i = 0; i< 10; i++)
			{
				coord = new Coordinate(i, i+10);
				coords.Add(coord);
			}
			return coords;
		}

		public void test_Constructor()
		{
			Coordinates coords = new Coordinates();
			Assertion.AssertEquals("Constructor-1: ", 0, coords.Count);

			coords = Coords1();
			Assertion.AssertEquals("Constructor-2: ", 10, coords.Count);
		}

		public void test_Count()
		{
			Coordinates coords = Coords1();
			Assertion.AssertEquals("Constructor-2: ", 10, coords.Count);
		}

		public void test_Add()
		{
			Coordinates coords = Coords1();
			Coordinate coord = new Coordinate(4.0, 2.0);
			coords.Add(coord);
			Assertion.AssertEquals("Add-1: ", 11, coords.Count);
			Assertion.AssertEquals("Add-2: ", 4.0, coords[10].X);
			Assertion.AssertEquals("Add-3: ", 2.0, coords[10].Y);

			coord = new Coordinate();
			coords.Add(coord);
			Assertion.AssertEquals("Add-4: ", 12, coords.Count);
			//Zeros are added for null coordinates
			Assertion.AssertEquals("Add-5: ", 0.0, coords[11].X);
			Assertion.AssertEquals("Add-6: ", 0.0, coords[11].Y);
		}

		public void test_this()
		{
			Coordinates coords = Coords1();

			Assertion.AssertEquals("this-1: ", 1.0, coords[1].X);
			Assertion.AssertEquals("this-2: ", 11.0, coords[1].Y);
		}

		public void test_GetEnumerator()
		{
			Coordinates coords = Coords1();

			int counter = 0;
			foreach(Coordinate coord in coords)
			{
				if(coord != null)
				{
					counter++;
				}
			}
			Assertion.AssertEquals("GetEnumerator: ", true, counter > 0);
		}

		public void test_ToString()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(2.0, 4.1);
			coords.Add(coord);
			coord = new Coordinate(6.2, 8.4);
			coords.Add(coord);

			Assertion.AssertEquals("ToString-1: ", "(2, 4.1, NaN),(6.2, 8.4, NaN)", coords.ToString());
		}

		public void test_ReverseCoordinateOrder()
		{
			Coordinates coords = Coords1();
			Coordinates coordsReverse = coords.ReverseCoordinateOrder();
			for(int i = 0; i < coords.Count; i++)
			{
				Assertion.AssertEquals("ReverseCoordinateOrder: ", true, coords[i].Equals(coordsReverse[9-i]));
			}
		}

		public void test_HasRepeatedPoints()
		{
//			Fail("Not implemented");
		}

		public void test_RemoveRepeatedPoints()
		{
//			Fail("Not implemented");
		}

		public void test_Equals()
		{
//			Fail("Not implemented");
		}
	}
}

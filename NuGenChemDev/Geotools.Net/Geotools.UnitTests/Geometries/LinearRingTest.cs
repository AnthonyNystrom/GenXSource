#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Geometries/LinearRingTest.cs,v 1.1 2003/01/02 20:32:36 awcoats Exp $
 * $Log: LinearRingTest.cs,v $
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
 * 2     10/21/02 12:19p Rabergman
 * Completed tests
 * 
 * 1     9/30/02 11:36a Rabergman
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
	public class LinearRingTest 
	{
		PrecisionModel _precMod = new PrecisionModel(1.0, 2.0, 3.0);
		int _sRID = 3;
		Coordinates _coords = new Coordinates();

		/// <summary>
		/// Method to create a Simple LinearRing for testing purposes
		/// </summary>
		/// <returns>A LinearRing</returns>
		private LinearRing Simple()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord ;//Coordinate(0, 0);

			coord = new Coordinate(10, 13);
			coords.Add(coord);
			coord = new Coordinate(11, 13);
			coords.Add(coord);
			coord = new Coordinate(12, 13);
			coords.Add(coord);
			coord = new Coordinate(13, 14);
			coords.Add(coord);
			coord = new Coordinate(14, 15);
			coords.Add(coord);
			coord = new Coordinate(15, 16);
			coords.Add(coord);
			coord = new Coordinate(15, 17);
			coords.Add(coord);
			coord = new Coordinate(15, 18);
			coords.Add(coord);
			coord = new Coordinate(14, 19);
			coords.Add(coord);
			coord = new Coordinate(13, 20);
			coords.Add(coord);
			coord = new Coordinate(12, 21);
			coords.Add(coord);
			coord = new Coordinate(11, 21);
			coords.Add(coord);
			coord = new Coordinate(10, 21);
			coords.Add(coord);
			coord = new Coordinate(9, 20);
			coords.Add(coord);
			coord = new Coordinate(8, 19);
			coords.Add(coord);
			coord = new Coordinate(7, 18);
			coords.Add(coord);
			coord = new Coordinate(7, 17);
			coords.Add(coord);
			coord = new Coordinate(7, 16);
			coords.Add(coord);
			coord = new Coordinate(8, 15);
			coords.Add(coord);
			coord = new Coordinate(9, 14);
			coords.Add(coord);
			coord = new Coordinate(10, 13);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LinearRing lr = gf.CreateLinearRing(coords);
			return lr;
		}

		/// <summary>
		/// Method to create a NonSimple LinearRing for testing purposes
		/// </summary>
		/// <returns>A LinearRing</returns>
		private LinearRing NonSimple()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(0, 0);

			coord = new Coordinate(2, 2);
			coords.Add(coord);
			coord = new Coordinate(3, 1);
			coords.Add(coord);
			coord = new Coordinate(4, 2);
			coords.Add(coord);
			coord = new Coordinate(5, 3);
			coords.Add(coord);
			coord = new Coordinate(6, 4);
			coords.Add(coord);
			coord = new Coordinate(7, 5);
			coords.Add(coord);
			coord = new Coordinate(7, 6);
			coords.Add(coord);
			coord = new Coordinate(7, 7);
			coords.Add(coord);
			coord = new Coordinate(7, 8);
			coords.Add(coord);
			coord = new Coordinate(7, 9);
			coords.Add(coord);
			coord = new Coordinate(6, 10);
			coords.Add(coord);
			coord = new Coordinate(5, 11);
			coords.Add(coord);
			coord = new Coordinate(6, 12);
			coords.Add(coord);
			coord = new Coordinate(7, 13);
			coords.Add(coord);
			coord = new Coordinate(8, 14);
			coords.Add(coord);
			coord = new Coordinate(9, 13);
			coords.Add(coord);
			coord = new Coordinate(10, 12);
			coords.Add(coord);
			coord = new Coordinate(10, 11);
			coords.Add(coord);
			coord = new Coordinate(10, 10);
			coords.Add(coord);
			coord = new Coordinate(10, 9);
			coords.Add(coord);
			coord = new Coordinate(9, 8);
			coords.Add(coord);
			coord = new Coordinate(8, 7);
			coords.Add(coord);
			coord = new Coordinate(7, 7);
			coords.Add(coord);
			coord = new Coordinate(6, 7);
			coords.Add(coord);
			coord = new Coordinate(5, 8);
			coords.Add(coord);
			coord = new Coordinate(4, 8);
			coords.Add(coord);
			coord = new Coordinate(3, 7);
			coords.Add(coord);
			coord = new Coordinate(2, 6);
			coords.Add(coord);
			coord = new Coordinate(1, 5);
			coords.Add(coord);
			coord = new Coordinate(2, 4);
			coords.Add(coord);
			coord = new Coordinate(1, 3);
			coords.Add(coord);
			coord = new Coordinate(2, 2);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LinearRing lr = gf.CreateLinearRing(coords);
			return lr;
		}

		private LinearRing ThrowsException()
		{
			Coordinates coords = new Coordinates();
			Coordinate coord ;//Coordinate(0, 0);

			coord = new Coordinate(10, 13);
			coords.Add(coord);
			coord = new Coordinate(11, 13);
			coords.Add(coord);
			coord = new Coordinate(12, 13);
			coords.Add(coord);
			coord = new Coordinate(13, 14);
			coords.Add(coord);
			coord = new Coordinate(14, 15);
			coords.Add(coord);
			coord = new Coordinate(15, 16);
			coords.Add(coord);
			coord = new Coordinate(15, 17);
			coords.Add(coord);
			coord = new Coordinate(15, 18);
			coords.Add(coord);
			coord = new Coordinate(14, 19);
			coords.Add(coord);
			coord = new Coordinate(13, 20);
			coords.Add(coord);
			coord = new Coordinate(12, 21);
			coords.Add(coord);
			coord = new Coordinate(11, 21);
			coords.Add(coord);
			coord = new Coordinate(10, 21);
			coords.Add(coord);
			coord = new Coordinate(9, 20);
			coords.Add(coord);
			coord = new Coordinate(8, 19);
			coords.Add(coord);
			coord = new Coordinate(7, 18);
			coords.Add(coord);
			coord = new Coordinate(7, 17);
			coords.Add(coord);
			coord = new Coordinate(7, 16);
			coords.Add(coord);
			coord = new Coordinate(8, 15);
			coords.Add(coord);
			coord = new Coordinate(9, 14);
			coords.Add(coord);

			GeometryFactory gf = new GeometryFactory(_precMod, _sRID);
			LinearRing lr = gf.CreateLinearRing(coords);
			return lr;
		}

		

		
		public void test_Coordinates()
		{
			LinearRing lr = Simple();
			Assertion.AssertEquals("Constructor1: ", false, lr.Equals(null));

			lr = NonSimple();
			Assertion.AssertEquals("Constructor1: ", false, lr.Equals(null));

			try
			{
				//create a linearRing that is not closed
				LinearRing ls = ThrowsException();
				Assertion.Fail("should never reach here");
			}
			catch(ArgumentException)
			{
			}
		}

		public void test_IsSimple()
		{
			LinearRing lr = Simple();
			Assertion.AssertEquals("IsSimple: ", true, lr.IsSimple());
		}

		public void test_GetGeometryType()
		{
			LinearRing lr = Simple();
			Assertion.AssertEquals("GetGeometryType: ", "LinearRing", lr.GetGeometryType());
		}

		public void test_IsClosed()
		{
			LinearRing lr = Simple();
			Assertion.AssertEquals("IsClosed: ", true, lr.IsClosed());
		}

		public void test_Clone()
		{
			LinearRing lr = Simple();
			LinearRing lr2 = (LinearRing)lr.Clone();

			Assertion.AssertEquals("Clone-1: ", false, lr == lr2);
			Assertion.AssertEquals("Clone-2: ", true, lr.Equals(lr2));
		}
	}
}

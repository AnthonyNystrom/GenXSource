#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Operation/Intersect/PointLine.cs,v 1.1 2003/01/02 20:37:51 awcoats Exp $
 * $Log: PointLine.cs,v $
 * Revision 1.1  2003/01/02 20:37:51  awcoats
 * *** empty log message ***
 *
 * 
 * 5     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 4     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 3     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 2     10/07/02 5:47p Lakoeppe
 * tests corrected and comments added.
 * 
 * 1     10/07/02 1:06p Awcoats
 * 
 */ 
#endregion

#region Using
using System;
using NUnit.Framework;
using Geotools.Geometries;
#endregion

namespace Geotools.UnitTests.Operation.Intersect
{
	/// <summary>
	/// Tests the basic functionality of the Geotools.UnitTests.Operation.Intersect.PointLine class
	/// </summary>
	[TestFixture]
	public class PointLine 
	{
		GeometryFactory _factory = new GeometryFactory();
		

		public void TestPointLine1()
		{
			// test when the point being tested in a node on the line	
			Coordinate coordLine1 = new Coordinate(5.0,5.0);
			Coordinate coordLine2 = new Coordinate(10.0,10.0);
			Coordinate coordPoint1 = new Coordinate(5.0,5.0);

			Coordinates coordinates = new Coordinates();
			coordinates.Add(coordLine1);
			coordinates.Add(coordLine2);

			Point point1 = _factory.CreatePoint(coordPoint1);

			LineString linestring = _factory.CreateLineString(coordinates);
			Assertion.AssertEquals("intersects",true,point1.Intersects(linestring));
			Assertion.AssertEquals("disjoint",false,point1.Disjoint(linestring));
			Assertion.AssertEquals("contains",false,point1.Contains(linestring));
			Assertion.AssertEquals("within",false,point1.Within(linestring));

			// always returns false when a point is involves
			Assertion.AssertEquals("crosses",false,point1.Crosses(linestring));

			Assertion.AssertEquals("touches",true,point1.Touches(linestring));
			
			// always returns false when a point is involved
			Assertion.AssertEquals("overlaps",false,point1.Overlaps(linestring));
		}
		public void TestPointLine2()
		{
			// test when the point being tested in a node on the line	
			Coordinate coordLine1 = new Coordinate(5.0,5.0);
			Coordinate coordLine2 = new Coordinate(10.0,10.0);
			Coordinate coordPoint1 = new Coordinate(6.0,6.0);

			Coordinates coordinates = new Coordinates();
			coordinates.Add(coordLine1);
			coordinates.Add(coordLine2);

			Point point1 = _factory.CreatePoint(coordPoint1);

			LineString linestring = _factory.CreateLineString(coordinates);
			Assertion.AssertEquals("intersects",true,point1.Intersects(linestring)); 
			Assertion.AssertEquals("intersects",true,linestring.Intersects(point1)); 
			Assertion.AssertEquals("disjoint",false,point1.Disjoint(linestring)); 
			Assertion.AssertEquals("contains",false,point1.Contains(linestring));
			Assertion.AssertEquals("OppositeContains", true, linestring.Contains( point1 ));
			Assertion.AssertEquals("within",true,point1.Within(linestring));		// point1 is within linestring and linestring contains point.

			// always returns false when a point is involves
			Assertion.AssertEquals("crosses",false,point1.Crosses(linestring));

			Assertion.AssertEquals("touches",false,point1.Touches(linestring));		// false because point is in the interior of linestring and not the boundary. The boundary is the endpoints.
			
			// always returns false when a point is involved
			Assertion.AssertEquals("overlaps",false,point1.Overlaps(linestring));

		}
	}
}


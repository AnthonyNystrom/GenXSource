#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Operation/Intersect/PointPoint.cs,v 1.1 2003/01/02 20:37:51 awcoats Exp $
 * $Log: PointPoint.cs,v $
 * Revision 1.1  2003/01/02 20:37:51  awcoats
 * *** empty log message ***
 *
 * 
 * 4     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 3     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 2     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
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
	/// Tests the basic functionality of the Geotools.UnitTests.Operation.Intersect.PointPoint class
	/// </summary>
	[TestFixture]
	public class PointPoint 
	{
		GeometryFactory _factory = new GeometryFactory();
		
		public void TestPointPoint1() 
		{
			// test two points that are the same location
			Coordinate coord1 = new Coordinate(5.0,7.0);
			Coordinate coord2 = new Coordinate(5.0,7.0);
			Point pt1 = _factory.CreatePoint(coord1);
			Point pt2 = _factory.CreatePoint(coord2);

			Assertion.AssertEquals("intersects",true,pt1.Intersects(pt2));
			Assertion.AssertEquals("disjoint",false,pt1.Disjoint(pt2));
			Assertion.AssertEquals("contains",true,pt1.Contains(pt2));
			Assertion.AssertEquals("within",true,pt1.Within(pt2));


			//Cross returns 1 (TRUE) if the intersection results in a geometry whose dimension 
			//is one less than the maximum dimension of the two source geometries and the
			//intersection set is interior to both source geometries. Cross returns t (TRUE)
			//for only a multipoint/polygon, multipoint/linestring, linestring/linestring, 
			//linestring/polygon, and linestring/multipolygon comparisons. 
			Assertion.AssertEquals("crosses",false,pt1.Crosses(pt2));

			// Touch returns 1 (TRUE) if none of the points common to both geometries 
			// intersect the interiors of both geometries. At least one geometry must
			// be a linestring, polygon, multilinestring or multipolygon. 
			Assertion.AssertEquals("touches",false,pt1.Touches(pt2));


			//This pattern matrix applies to linestring/linestring and multilinestring/
			//multilinestring overlays. In this case the intersection of the geometries
			//must result in a geometry that has a dimension of 1 (another linestring).
			//If the dimension of the intersection of the interiors had been 1 the overlay
			//predicate would return FALSE, however the cross predicate would have returned TRUE. 
			Assertion.AssertEquals("overlaps",false,pt1.Overlaps(pt2));
			
		}

		public void TestPointPoint2() 
		{
			// test two points that are the same location
			Coordinate coord1 = new Coordinate(5.0,7.0);
			Coordinate coord2 = new Coordinate(5.0,8.0);
			Point pt1 = _factory.CreatePoint(coord1);
			Point pt2 = _factory.CreatePoint(coord2);

			Assertion.AssertEquals("intersects",false,pt1.Intersects(pt2));
			Assertion.AssertEquals("disjoint",true,pt1.Disjoint(pt2));
			Assertion.AssertEquals("contains",false,pt1.Contains(pt2));
			Assertion.AssertEquals("within",false,pt1.Within(pt2));


			// will always return false because point vs point is not a valid op
			// see comments above.
			Assertion.AssertEquals("crosses",false,pt1.Crosses(pt2));
			Assertion.AssertEquals("touches",false,pt1.Touches(pt2));
			Assertion.AssertEquals("overlaps",false,pt1.Overlaps(pt2));
			
		}

		public void TestPointPoint3() 
		{
			// test two points that are the same location
			Coordinate coord1 = new Coordinate(5.0,7.0);
			Coordinate coord2 = new Coordinate();
			Point pt1 = _factory.CreatePoint(coord1);
			Point pt2 = _factory.CreatePoint(coord2);

			Assertion.AssertEquals("intersects",false,pt1.Intersects(pt2));
			Assertion.AssertEquals("disjoint",true,pt1.Disjoint(pt2));
			Assertion.AssertEquals("contains",false,pt1.Contains(pt2));
			Assertion.AssertEquals("within",false,pt1.Within(pt2));


			// will always return false because point vs point is not a valid op
			// see comments above.
			Assertion.AssertEquals("crosses",false,pt1.Crosses(pt2));
			Assertion.AssertEquals("touches",false,pt1.Touches(pt2));
			Assertion.AssertEquals("overlaps",false,pt1.Overlaps(pt2));
			
		}

		public void TestPointPoint4() 
		{
			// test two null points. 
			Coordinate coord1 = new Coordinate();
			Coordinate coord2 = new Coordinate();
			Point pt1 = _factory.CreatePoint(coord1);
			Point pt2 = _factory.CreatePoint(coord2);

			Assertion.AssertEquals("intersects",true,pt1.Intersects(pt2));
			Assertion.AssertEquals("disjoint",false,pt1.Disjoint(pt2));
			Assertion.AssertEquals("contains",true,pt1.Contains(pt2));
			Assertion.AssertEquals("within",true,pt1.Within(pt2));


			// will always return false because point vs point is not a valid op
			// see comments above.
			Assertion.AssertEquals("crosses",false,pt1.Crosses(pt2));
			Assertion.AssertEquals("touches",false,pt1.Touches(pt2));
			Assertion.AssertEquals("overlaps",false,pt1.Overlaps(pt2));
			
		}

		
	}
}


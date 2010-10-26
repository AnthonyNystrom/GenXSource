#region SourceSafe Comments
/* 
 * $Header: /cvsroot/geotoolsnet/GeotoolsNet/Geotools.UnitTests/Operation/Intersect/LineLine.cs,v 1.1 2003/01/02 20:37:50 awcoats Exp $
 * $Log: LineLine.cs,v $
 * Revision 1.1  2003/01/02 20:37:50  awcoats
 * *** empty log message ***
 *
 * 
 * 6     12/27/02 1:01p Awcoats
 * changes  when moving from NUnit 1.0 to Nunit 2.0
 * 
 * 5     11/04/02 3:20p Rabergman
 * Changed using namespaces
 * 
 * 4     10/31/02 11:01a Awcoats
 * changed namespace from UrbanScience.Geographic to Geotools.
 * 
 * 3     10/16/02 12:28p Rabergman
 * removed i=0
 * 
 * 2     10/08/02 9:22a Lakoeppe
 * corrected overlap test.
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
	public class LineLineTest  
	{
		GeometryFactory _factory = new GeometryFactory();
		

		public void TestLineLine1()
		{
			// test when the point being tested in a node on the line	
			Coordinate coordLine1 = new Coordinate(5.0,5.0);
			Coordinate coordLine2 = new Coordinate(10.0,10.0);
			Coordinate coordLine3 = new Coordinate(5.0, 10.0);
			Coordinate coordLine4 = new Coordinate(10.0,5.0);


			Coordinates coordinates1 = new Coordinates();
			coordinates1.Add(coordLine1);
			coordinates1.Add(coordLine2);

			
			Coordinates coordinates2 = new Coordinates();
			coordinates2.Add(coordLine3);
			coordinates2.Add(coordLine4);

			

			LineString linestring1 = _factory.CreateLineString(coordinates1);
			LineString linestring2 = _factory.CreateLineString(coordinates2);

			Geometry pt =  linestring1.Intersection(linestring2);

			Assertion.AssertEquals("intersects",true,linestring1.Intersects(linestring2));
			Assertion.AssertEquals("disjoint",false,linestring1.Disjoint(linestring2));
			Assertion.AssertEquals("contains",false,linestring1.Contains(linestring2));
			Assertion.AssertEquals("within",false,linestring1.Within(linestring2));

			
			Assertion.AssertEquals("crosses",true,linestring1.Crosses(linestring2));

			Assertion.AssertEquals("touches",false,linestring1.Touches(linestring2));
			
			// always returns false when a point is involved
			Assertion.AssertEquals("overlaps",false,linestring1.Overlaps(linestring2));

		}
		
	}
}


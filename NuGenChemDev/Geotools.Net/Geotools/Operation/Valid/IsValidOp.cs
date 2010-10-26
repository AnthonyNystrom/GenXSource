/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
 *  Copyright 2001 Vivid Solutions)
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

#region Using
using System;
using System.Collections;
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
using Geotools.Operation;
using System.Diagnostics;
#endregion


namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Summary description for IsValidOp.
	/// </summary>
	internal class IsValidOp 
	{
		private bool _isChecked = false;
		private TopologyValidationError _validErr;



		/// <summary>
		///  Find a point from the list of testCoords
		///  that is NOT a node in the edge for the list of searchCoords
		/// </summary>
		/// <param name="testCoords"></param>
		/// <param name="searchRing"></param>
		/// <param name="graph"></param>
		/// <returns>return the point found, or null if none found</returns>
		public static Coordinate FindPtNotNode(
			Coordinates testCoords,
			LinearRing searchRing,
			GeometryGraph graph)
		{
			
			// find edge corresponding to searchRing.
			Edge searchEdge = graph.FindEdge(searchRing );
			// find a point in the testCoords which is not a node of the searchRing
			EdgeIntersectionList eiList = searchEdge.EdgeIntersectionList;
			// somewhat inefficient - is there a better way? (Use a node map, for instance?)
			for (int i = 0 ; i < testCoords.Count; i++) 
			{
				Coordinate pt = testCoords[i];
				if (! eiList.IsIntersection(pt))
				{
					return pt;
				}
			}
			return null;
		}

		private static  CGAlgorithms _cga = new RobustCGAlgorithms();

		private Geometry _parentGeometry;  // the base Geometry to be validated
		

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the IsValidOp class.
		/// </summary>
		public IsValidOp(Geometry parentGeometry) 
		{
			this._parentGeometry = parentGeometry;
		}
		#endregion

		#region Static methods 
			
		private void CheckConnectedInteriors(GeometryGraph graph)
		{
			
			ConnectedInteriorTester cit = new ConnectedInteriorTester(graph);
			if (! cit.IsInteriorsConnected())
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.DisconnectedInterior,
					cit.GetCoordinate());
			}	
		}
		#endregion

		#region Properties
		#endregion


		#region Methods
		public bool IsValid()
		{

			CheckValid(_parentGeometry);
			return _validErr == null;
		}

		public TopologyValidationError GetValidationError()
		{
			CheckValid(_parentGeometry);
			return _validErr;
		}
		private void CheckValid(Geometry g)
		{
			
			if (_isChecked)
			{
				return;
			}
			_validErr = null;
			if ( g.IsEmpty() )
			{
					return;
			}
			if (g is Point)   
			{
				return;
			}
			else if (g is MultiPoint)
			{
				return;
			}
				// LineString also handles LinearRings
			else if (g is LineString) 
			{
				CheckValid( (LineString) g);
			}
			else if (g is Polygon)    
			{
				CheckValid( (Polygon) g);
			}
			else if (g is MultiPolygon) 
			{
				CheckValid( (MultiPolygon) g);
			}
			else if (g is GeometryCollection)
			{
				CheckValid( (GeometryCollection) g);
			}
			else  throw new NotSupportedException(g.GetType().Name);

		}
		private void CheckValid(LineString g)
		{
			GeometryGraph graph = new GeometryGraph(0, g);
			CheckTooFewPoints(graph);
		}
		private void CheckValid(Polygon g)
		{
			GeometryGraph graph = new GeometryGraph(0, g);
			
			CheckTooFewPoints(graph);
			if (_validErr != null) return;
			CheckConsistentArea(graph);
			if (_validErr != null) return;
			CheckNoSelfIntersectingRings(graph);
			if (_validErr != null) return;
			CheckHolesInShell(g, graph);
			if (_validErr != null) return;
			//SLOWcheckHolesNotNested(g);
			CheckHolesNotNested(g, graph);
			if (_validErr != null) return;
			CheckConnectedInteriors(graph);	
		}
		private void CheckValid(MultiPolygon g)
		{
			GeometryGraph graph = new GeometryGraph(0, g);

			CheckConsistentArea(graph);
			if (_validErr != null) return;
			CheckNoSelfIntersectingRings(graph);
			if (_validErr != null) return;

			for (int i = 0; i < g.GetNumGeometries(); i++) 
			{
				Polygon p = (Polygon) g.GetGeometryN(i);
				CheckHolesInShell(p, graph);
				if (_validErr != null) return;
			}
			for (int i = 0; i < g.GetNumGeometries(); i++) 
			{
				Polygon p = (Polygon) g.GetGeometryN(i);
				CheckHolesNotNested(p, graph);
				if (_validErr != null) return;
			}
			CheckShellsNotNested(g, graph);
			if (_validErr != null) return;
			CheckConnectedInteriors(graph);
			
		}

		private void CheckValid(GeometryCollection gc)
		{
			for (int i = 0; i < gc.GetNumGeometries(); i++) 
			{
				Geometry g = gc.GetGeometryN(i);
				CheckValid(g);
				if (_validErr != null) return;
			}
		}
		private void CheckTooFewPoints(GeometryGraph graph)
		{
			if (graph.HasTooFewPoints()) 
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.TooFewPoints,
					graph.GetInvalidPoint());
				return;
			}
		}


		private void CheckConsistentArea(GeometryGraph graph)
		{
			
			ConsistentAreaTester cat = new ConsistentAreaTester(graph);
			
			bool isValidArea = cat.IsNodeConsistentArea();
			if (! isValidArea) 
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.SelfIntersection,
					cat.GetInvalidPoint());
				return;
			}
			if (cat.HasDuplicateRings()) 
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.DuplicateRings,
					cat.GetInvalidPoint());
			}
		}
		private void CheckNoSelfIntersectingRings(GeometryGraph graph)
		{
			
			foreach (object obj in graph.Edges ) 
			{
				Edge e = (Edge) obj;
				CheckSelfIntersectingRing(e.EdgeIntersectionList);
				if (_validErr != null)
				{
					return;
				}
			}
		}
	
		/// <summary>
		/// Check that a ring does not self-intersect, except at its endpoints.
		/// Algorithm is to count the number of times each node along edge occurs.
		/// If any occur more than once, that must be a self-intersection.
		/// </summary>
		/// <param name="eiList"></param>
		private void CheckSelfIntersectingRing(EdgeIntersectionList eiList)
		{
			
			//Set nodeSet = new TreeSet(); awc  don't need sorted list, just a hashtable
			Hashtable nodeSet = new Hashtable();
			bool isFirst = true;
			//for (Iterator i = eiList.iterator(); i.hasNext(); ) 
			foreach(EdgeIntersection ei in eiList)
			{
				//EdgeIntersection ei = (EdgeIntersection) i.next();
				if (isFirst) 
				{
					isFirst = false;
					continue;
				}
				if (nodeSet.Contains(ei.Coordinate)) 
				{
					_validErr = new TopologyValidationError(
						TopologyValidationError.RingSelfIntersection,
						ei.Coordinate);
					return;
				}
				else 
				{
					//TODO: awc - should probably use hashcode
					nodeSet.Add(ei.Coordinate, ei.Coordinate);
				}
			}
		}

	
	
		/// <summary>
		/// Test that each hole is inside the polygon shell.
		/// This routine assumes that the holes have previously been tested
		/// to ensure that all vertices lie on the shell or inside it.
		/// A simple test of a single point in the hole can be used,
		/// provide the point is chosen such that it does not lie on the
		/// boundary of the shell.
		/// </summary>
		/// <param name="p">The polygon to be tested for hole inclusion.</param>
		/// <param name="graph">Graph a GeometryGraph incorporating the polygon.</param>
		private void CheckHolesInShell(Polygon p, GeometryGraph graph)
		{
			LinearRing shell = (LinearRing) p.GetExteriorRing();
			Coordinates shellPts = shell.GetCoordinates();

			//PointInRing pir = new SimplePointInRing(shell);
			//PointInRing pir = new SIRtreePointInRing(shell);
			IPointInRing pir = new MCPointInRing(shell);

			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{

				LinearRing hole = (LinearRing) p.GetInteriorRingN(i);
				Coordinate holePt = FindPtNotNode(hole.GetCoordinates(), shell, graph);
				if (holePt == null)
				{
					throw new InvalidOperationException("Unable to find a hole point not a vertex of the shell.");
				}

				bool outside = ! pir.IsInside(holePt);
				if ( outside ) 
				{
					_validErr = new TopologyValidationError(
						TopologyValidationError.HoleOutsideShell,
						holePt);
					return;
				}
			}
		}
		private void OLDCheckHolesInShell(Polygon p)
		{
			// awc: probably do'nt need this - but copy it anyway
			/*LinearRing shell =  (LinearRing) p.getExteriorRing();
			Coordinate[] shellPts = shell.getCoordinates();
			for (int i = 0; i < p.getNumInteriorRing(); i++) 
			{
				Coordinate holePt = findPtNotNode(p.getInteriorRingN(i).getCoordinates(), shell, arg[0]);
				Assert.isTrue(holePt != null, "Unable to find a hole point not a vertex of the shell");
				bool onBdy = cga.isOnLine(holePt, shellPts);
				bool inside = cga.isPointInPolygon(holePt, shellPts);
				bool outside = ! (onBdy || inside);
				if ( outside ) 
				{
					_validErr = new TopologyValidationError(
						TopologyValidationError.HOLE_OUTSIDE_SHELL,
						holePt);
					return;
				}
			}*/
			throw new NotImplementedException();
		}

		/// <summary>
		/// Tests that no hole is nested inside another hole.
		/// </summary>
		/// <remarks>
		/// This routine assumes that the holes are disjoint.
		/// To ensure this, holes have previously been tested
		/// to ensure that:
		/// <ul>
		/// <li>they do not partially overlap
		/// (Checked by CheckRelateConsistency)</li>
		/// <li>they are not identical
		/// (Checked by CheckRelateConsistency)</li>
		/// <li>they do not touch at a vertex
		/// (Checked by ????)</li>
		/// </ul>
		/// </remarks>
		/// <param name="p"></param>
		/// <param name="graph"></param>
		private void CheckHolesNotNested(Polygon p, GeometryGraph graph)
		{
			
			QuadtreeNestedRingTester nestedTester = new QuadtreeNestedRingTester(graph);
			//SimpleNestedRingTester nestedTester = new SimpleNestedRingTester(_arg[0]);
			//SweeplineNestedRingTester nestedTester = new SweeplineNestedRingTester(_arg[0]);

			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{
				LinearRing innerHole = p.GetInteriorRingN( i );
				nestedTester.Add(innerHole);
			}
			bool isNonNested = nestedTester.IsNonNested();
			if ( ! isNonNested ) 
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.NestedHoles,
					nestedTester.GetNestedPoint());
			}
		}
/*
		private void SLOWCheckHolesNotNested(Polygon p)
		{
			
			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{
				LinearRing innerHole = p.GetInteriorRingN( i );
				Coordinates innerHolePts = innerHole.GetCoordinates();
				for (int j = 0; j < p.GetNumInteriorRing(); j++) 
				{
					// don't test hole against itself!
					if (i == j) continue;

					LinearRing searchHole = p.GetInteriorRingN( j );

					// if envelopes don't overlap, holes are not nested
					if (! innerHole.GetEnvelopeInternal().Overlaps(searchHole.GetEnvelopeInternal()))
						continue;

					Coordinates searchHolePts = searchHole.GetCoordinates();
					Coordinate innerholePt = FindPtNotNode(innerHolePts, searchHole, _arg[0]);
					//Assert.isTrue(innerholePt != null, "Unable to find a hole point not a node of the search hole");
					bool inside = _cga.IsPointInPolygon(innerholePt, searchHolePts);
					if ( inside ) 
					{
						_validErr = new TopologyValidationError(
							TopologyValidationError.NestedHoles,
							innerholePt);
						return;
					}
				}
			}
		}
*/		
		/// <summary>
		///  Test that no element polygon is wholly in the interior of another element polygon.
		/// </summary>
		/// <remarks>
		///  TODO: It handles the case that one polygon is nested inside a hole of another.
		///  
		///  Preconditions:
		///  <ul>
		///  <li>shells do not partially overlap</li>
		///  <li>shells do not touch along an edge</li>
		///  <li>no duplicate rings exist</li>
		///  </ul>
		///  This routine relies on the fact that while polygon shells may touch at one or
		///  more vertices, they cannot touch at ALL vertices.
		/// </remarks>
		/// <param name="mp"></param>
		/// <param name="graph"></param>
		private void CheckShellsNotNested(MultiPolygon mp, GeometryGraph graph)
		{
			
			for (int i = 0; i < mp.GetNumGeometries(); i++) 
			{
				Polygon p = (Polygon) mp.GetGeometryN(i);
				LinearRing shell = (LinearRing) p.GetExteriorRing();
				for (int j = 0; j < mp.GetNumGeometries(); j++) 
				{
					if (i == j) continue;
					Polygon p2 = (Polygon) mp.GetGeometryN(j);
					CheckShellNotNested(shell, p2, graph);
					if (_validErr != null) return;
				}
			}
			
		}

		/// <summary>
		/// Check if a shell is incorrectly nested within a polygon.  This is the case
		/// if the shell is inside the polygon shell, but not inside a polygon hole.
		/// If the shell is inside a polygon hole, the nesting is valid.)
		/// 
		/// The algorithm used relies on the fact that the rings must be properly contained.
		/// E.g. they cannot partially overlap (this has been previously Checked by
		/// CheckRelateConsistency
		/// </summary>
		/// <param name="shell"></param>
		/// <param name="p"></param>
		/// <param name="graph"></param>
		private void CheckShellNotNested(LinearRing shell, Polygon p, GeometryGraph graph)
		{
			
			Coordinates shellPts = shell.GetCoordinates();
			// test if shell is inside polygon shell
			LinearRing polyShell =  (LinearRing) p.GetExteriorRing();
			Coordinates polyPts = polyShell.GetCoordinates();
			Coordinate shellPt = FindPtNotNode(shellPts, polyShell, graph);
			// if no point could be found, we can assume that the shell is outside the polygon
			if (shellPt == null)
				return;
			bool insidePolyShell = _cga.IsPointInRing(shellPt, polyPts);
			if (! insidePolyShell) return;

			// if no holes, this is an error!
			if (p.GetNumInteriorRing() <= 0) 
			{
				_validErr = new TopologyValidationError(
					TopologyValidationError.NestedShells,
					shellPt);
				return;
			}

			for (int i = 0; i < p.GetNumInteriorRing(); i++) 
			{
				LinearRing hole = p.GetInteriorRingN( i );
				CheckShellInsideHole(shell, hole, graph);
				if (_validErr != null) return;
			}
			
		}

	
		/// <summary>
		/// This routine Checks to see if a shell is properly contained in a hole.
		/// </summary>
		/// <param name="shell"></param>
		/// <param name="hole"></param>
		/// <param name="graph"></param>
		private void CheckShellInsideHole(LinearRing shell, LinearRing hole, GeometryGraph graph)
		{
			
			Coordinates shellPts = shell.GetCoordinates();
			Coordinates holePts = hole.GetCoordinates();
			// TODO: improve performance of this - by sorting pointlists for instance?
			Coordinate shellPt = FindPtNotNode(shellPts, hole, graph);
			// if point is on shell but not hole, Check that the shell is inside the hole
			if (shellPt != null) 
			{
				bool insideHole = _cga.IsPointInRing(shellPt, holePts);
				if (! insideHole)
					_validErr = new TopologyValidationError(
						TopologyValidationError.NestedShells,
						shellPt);
				return;
			}
			Coordinate holePt = FindPtNotNode(holePts, shell, graph);
			// if point is on hole but not shell, Check that the hole is outside the shell
			if (holePt != null) 
			{
				bool insideShell = _cga.IsPointInRing(holePt, shellPts);
				if (insideShell) 
				{
					_validErr = new TopologyValidationError(
						TopologyValidationError.NestedShells,
						holePt);
				}
				return;
			}
			throw new InvalidOperationException("Points in shell and hole appear to be equal.");
		}

		#endregion

	}
}

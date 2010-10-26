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
using System.Diagnostics;
using System.Collections.Specialized;
using Geotools.Algorithms;
using Geotools.Geometries;
using Geotools.Graph;
#endregion

namespace Geotools.Operation.Overlay
{
	/// <summary>
	/// Summary description for PolygonBuilder.
	/// </summary>
	internal class PolygonBuilder
	{
		private GeometryFactory _geometryFactory;
		private CGAlgorithms _cga;
		private ArrayList _shellList = new ArrayList();

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the PolygonBuilder class.
		/// </summary>
		public PolygonBuilder( GeometryFactory geometryFactory, CGAlgorithms cga )
		{
			_geometryFactory = geometryFactory;
			_cga = cga;
		} // public PolygonBuilder( GeometryFactory geometryFactory, CGAlgorithms cga )
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Add a complete graph.
		/// The graph is assumed to contain one or more polygons,
		/// possibly with holes.
		/// </summary>
		/// <param name="graph"></param>
		public void Add( PlanarGraph graph )
		{
			ArrayList nodes = new ArrayList();
			foreach( DictionaryEntry entry in graph.Nodes.NodeList )
			{
				Node node = (Node) entry.Value;
				nodes.Add( node );
			}
			Add( graph.EdgeEnds, nodes );
		} // public void Add( PlanarGraph graph )

		/// <summary>
		/// Add a set of edges and nodes, which form a graph.
		/// The graph is assumed to contain one or more polygons,
		/// possibly with holes.
		/// </summary>
		/// <param name="dirEdges"></param>
		/// <param name="nodes"></param>
		public void Add( ArrayList dirEdges, ArrayList nodes )
		{
			PlanarGraph.LinkResultDirectedEdges( nodes );
			ArrayList maxEdgeRings = BuildMaximalEdgeRings( dirEdges );
			ArrayList freeHoleList = new ArrayList();
			ArrayList edgeRings = BuildMinimalEdgeRings( maxEdgeRings, ref freeHoleList );
			SortShellsAndHoles( edgeRings, ref _shellList, ref freeHoleList );
			PlaceFreeHoles( freeHoleList );
			//Assert: every hole on freeHoleList has a shell assigned to it
		} // public void Add( ArrayList dirEdges, ArrayList nodes )

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ArrayList GetPolygons()
		{
			ArrayList resultPolyList = ComputePolygons();
			return resultPolyList;
		} // public ArrayList GetPolygons()

   	    /// <summary>
		/// For all DirectedEdges in result, form them into MaximalEdgeRings
		/// </summary>
		/// <param name="dirEdges"></param>
		/// <returns></returns>
		private ArrayList BuildMaximalEdgeRings( ArrayList dirEdges )
		{
			ArrayList maxEdgeRings  = new ArrayList();
			foreach ( object obj in dirEdges ) 
			{
				DirectedEdge de = (DirectedEdge) obj;
				if ( de.InResult && de.Label.IsArea() ) 
				{
					// if this edge has not yet been processed
					if ( de.EdgeRing == null ) 
					{
						MaximalEdgeRing er = new MaximalEdgeRing( de, _geometryFactory, _cga );
						maxEdgeRings.Add( er );
						//Trace.WriteLine("max node degree = " + er.getMaxDegree() );
					} // if ( de.EdgeRing == null )
				} // if ( de.InResult && de.Label.IsArea() )
			} // foreach ( object obj in dirEdges )
			return maxEdgeRings;
		} // private ArrayList BuildMaximalEdgeRings( ArrayList dirEdges )

		private ArrayList BuildMinimalEdgeRings( ArrayList maxEdgeRings, ref ArrayList freeHoleList )
		{
			ArrayList edgeRings = new ArrayList();
			foreach ( object obj in maxEdgeRings )
			{
				MaximalEdgeRing er = (MaximalEdgeRing) obj;
				if ( er.GetMaxNodeDegree() > 2 ) 
				{
					er.LinkDirectedEdgesForMinimalEdgeRings();
					ArrayList minEdgeRings = er.BuildMinimalRings();
					// at this point we can go ahead and attempt to place holes, if this EdgeRing is a polygon
					//computePoints(minEdgeRings);
					EdgeRing shell = FindShell( minEdgeRings );
					if ( shell != null ) 
					{
						PlacePolygonHoles( shell, minEdgeRings );
						_shellList.Add( shell );
					}
					else 
					{
						freeHoleList.AddRange( minEdgeRings );
					}
				}
				else 
				{
					edgeRings.Add( er );
				}
			} // foreach ( object obj in maxEdgeRings )
			return edgeRings;
		} // private ArrayList BuildMinimalEdgeRings( ArrayList maxEdgeRings, ArrayList shellList, ArrayList freeHoleList )

		/// <summary>
		/// This method takes a list of MinimalEdgeRings derived from a MaximalEdgeRing,
		/// and tests whether they form a Polygon.  This is the case if there is a single shell
		/// in the list.  In this case the shell is returned.
		/// The other possibility is that they are a series of connected holes, in which case
		/// no shell is returned.
		/// </summary>
		/// <param name="minEdgeRings"></param>
		/// <returns>
		/// The shell EdgeRing, if there is one
		/// Null, if all the rings are holes
		/// </returns>
		private EdgeRing FindShell( ArrayList minEdgeRings )
		{
			int shellCount = 0;
			EdgeRing shell = null;
			foreach ( object obj in minEdgeRings )
			{
				EdgeRing er = (MinimalEdgeRing) obj;
				if ( !er.IsHole ) 
				{
					shell = er;
					shellCount++;
				}
			}
			if(!(shellCount <= 1))
			{
				throw new InvalidOperationException("Found two shells in MinimalEdgeRing list." );
			}
			return shell;
		} // private EdgeRing FindShell( ArrayList minEdgeRings )

		/// <summary>
		/// This method assigns the holes for a Polygon (formed from a list of
		/// MinimalEdgeRings) to its shell.
		/// Determining the holes for a MinimalEdgeRing polygon serves two purposes:
		/// <ul>
		/// <li>it is faster than using a point-in-polygon check later on.</li>
		/// <li>it ensures correctness, since if the PIP test was used the point
		/// chosen might lie on the shell, which might return an incorrect result from the
		/// PIP test</li>
		/// </ul>
		/// </summary>
		/// <param name="shell"></param>
		/// <param name="minEdgeRings"></param>
		private void PlacePolygonHoles( EdgeRing shell, ArrayList minEdgeRings )
		{
			foreach ( object obj in minEdgeRings )
			{
				MinimalEdgeRing er = (MinimalEdgeRing) obj;
				if ( er.IsHole ) 
				{
					er.Shell = shell;
				}
			} // foreach ( object obj in minEdgeRings )
		} // private void PlacePolygonHoles( EdgeRing shell, ArrayList minEdgeRings )

		/// <summary>
		/// For all rings in the input list,
		/// determine whether the ring is a shell or a hole
		/// and add it to the appropriate list.
		/// Due to the way the DirectedEdges were linked,
		/// a ring is a shell if it is oriented CW, a hole otherwise.
		/// </summary>
		/// <param name="edgeRings"></param>
		/// <param name="shellList"></param>
		/// <param name="freeHoleList"></param>
		private void SortShellsAndHoles(ArrayList edgeRings, ref ArrayList shellList, ref ArrayList freeHoleList)
		{
			foreach ( object obj in edgeRings )
			{
				EdgeRing er = (EdgeRing) obj;
				er.SetInResult();
				if ( er.IsHole ) 
				{
					freeHoleList.Add( er );
				}
				else 
				{
					shellList.Add( er );
				}
			} // foreach ( object obj in edgeRings )
		} // private void SortShellsAndHoles(ArrayList edgeRings, ArrayList shellList, ArrayList freeHoleList)

		/// <summary>
		/// This method determines finds a containing shell for all holes
		/// which have not yet been assigned to a shell.</summary>
		/// <remarks>
		/// These "free" holes should
		/// all be <b>properly</b> contained in their parent shells, so it is safe to use the
		/// FindEdgeRingContaining method.
		/// (This is the case because any holes which are NOT
		/// properly contained (i.e. are connected to their
		/// parent shell) would have formed part of a MaximalEdgeRing
		/// and been handled in a previous step).
		/// </remarks>
		/// <param name="freeHoleList"></param>
		private void PlaceFreeHoles( ArrayList freeHoleList )
		{
			foreach ( object obj in freeHoleList )
			{
				EdgeRing hole = (EdgeRing) obj;
				// only place this hole if it doesn't yet have a shell
				if ( hole.Shell == null ) 
				{
					EdgeRing shell = FindEdgeRingContaining( hole );
					if( shell == null)
					{
							throw new InvalidOperationException("Unable to assign hole to a shell." );
					}
					hole.Shell = shell;
				} // if ( hole.Shell == null )
			} // foreach ( object obj in freeHoleList )
		} // private void PlaceFreeHoles( ArrayList freeHoleList )

		/// <summary>
		/// Find the innermost enclosing shell EdgeRing containing the argument EdgeRing, if any.
		/// The innermost enclosing ring is the <i>smallest</i> enclosing ring.</summary>
		/// <remarks>
		/// <para>The algorithm used depends on the fact that:
		/// 
		///  ring A contains ring B iff envelope(ring A) contains envelope(ring B)
		/// </para>
		/// <para>This routine is only safe to use if the chosen point of the hole
		/// is known to be properly contained in a shell
		/// (which is guaranteed to be the case if the hole does not touch its shell)</para>
		/// </remarks>
		/// <param name="testEr"></param>
		/// <returns>
		/// Containing EdgeRing, if there is one
		/// Null if no containing EdgeRing is found
		/// </returns>
		private EdgeRing FindEdgeRingContaining( EdgeRing testEr )
		{
			LinearRing testRing = testEr.GetLinearRing();
			Envelope testEnv = testRing.GetEnvelopeInternal();
			Coordinate testPt = testRing.GetCoordinateN(0);

			EdgeRing minShell = null;
			Envelope minEnv = null;
			foreach ( object obj in _shellList )
			{
				EdgeRing tryShell = (EdgeRing) obj;
				LinearRing tryRing = tryShell.GetLinearRing();
				Envelope tryEnv = tryRing.GetEnvelopeInternal();
				if ( minShell != null )
				{
					minEnv = minShell.GetLinearRing().GetEnvelopeInternal();
				}
				bool isContained = false;
				if ( tryEnv.Contains( testEnv )
					&& _cga.IsPointInRing( testPt, tryRing.GetCoordinates() ) )
				{
					isContained = true;
				}

				// check if this new containing ring is smaller than the current minimum ring
				if ( isContained ) 
				{
					if ( minShell == null
						|| minEnv.Contains( tryEnv ) ) 
					{
						minShell = tryShell;
					}
				}
			} // foreach ( object obj in _shellList )
			return minShell;
		} // private EdgeRing FindEdgeRingContaining( EdgeRing testEr )

		private ArrayList ComputePolygons()
		{
			ArrayList resultPolyList = new ArrayList();
			// add Polygons for all shells
			foreach ( object obj in _shellList )
			{
				EdgeRing er = (EdgeRing) obj;
				Polygon poly = er.ToPolygon( _geometryFactory );
				resultPolyList.Add( poly );
			} // foreach ( object obj in _shellList )
			return resultPolyList;
		} // private ArrayList ComputePolygons()

		/// <summary>
		/// Checks the current set of shells (with their associated holes) to
		/// see if any of them contain the point.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public bool ContainsPoint( Coordinate p )
		{
			foreach ( object obj in _shellList )
			{
				EdgeRing er = (EdgeRing) obj;
				if ( er.ContainsPoint( p ) )
				{
					return true;
				}
			}
			return false;
		} // public bool ContainsPoint( Coordinate p )
		#endregion

	} // public class PolygonBuilder
}

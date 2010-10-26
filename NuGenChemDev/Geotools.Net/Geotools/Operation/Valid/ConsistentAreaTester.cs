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
using Geotools.Graph;
using Geotools.Graph.Index;
using Geotools.Geometries;
using Geotools.Operation.Relate;
#endregion

namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Summary description for ConsistentAreaTester.
	/// </summary>
	internal class ConsistentAreaTester
	{
		private static LineIntersector _li = new RobustLineIntersector();

		private GeometryGraph _geomGraph;
		private RelateNodeGraph _nodeGraph = new RelateNodeGraph();

		// the intersection point found (if any)
		private Coordinate _invalidPoint;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the ConsistentAreaTester class.
		/// </summary>
		public ConsistentAreaTester(GeometryGraph geomGraph)
		{
			 this._geomGraph = geomGraph;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
	
		/// <summary>
		/// 
		/// </summary>
		/// <returns>return the intersection point, or <code>null</code> if none was found</returns>
		public Coordinate GetInvalidPoint() 
		{
			return _invalidPoint; 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool IsNodeConsistentArea()
		{
			
			SegmentIntersector intersector = _geomGraph.ComputeSelfNodes(_li);
			if (intersector.HasProperIntersection) 
			{
				_invalidPoint = intersector.ProperIntersectionPoint;
				return false;
			}

			_nodeGraph.Build(_geomGraph);

			return IsNodeEdgeAreaLabelsConsistent();
			
		}

		/// <summary>
		/// Check all nodes to see if their labels are consistent.
		///
		/// </summary>
		/// <returns> If any are not, return false</returns>
		private bool IsNodeEdgeAreaLabelsConsistent()
		{
		foreach(DictionaryEntry node in _nodeGraph ) 
			{
				RelateNode relatenode = (RelateNode) node.Value;
				if (! relatenode.Edges.IsAreaLabelsConsistent()) 
				{
					_invalidPoint = (Coordinate) relatenode.Coordinate.Clone();
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Checks for two duplicate rings in an area.
		/// Duplicate rings are rings that are topologically equal
		/// (that is, which have the same sequence of points up to point order).
		/// If the area is topologically consistent (determined by calling the
		/// IsNodeConsistentArea,
		/// duplicate rings can be found by checking for EdgeBundles which contain
		/// more than one EdgeEnd.
		/// (This is because topologically consistent areas cannot have two rings sharing
		/// the same line segment, unless the rings are equal).
		/// The start point of one of the equal rings will be placed in
		/// invalidPoint.
		/// </summary>
		/// <returns>return true if this area Geometry is topologically consistent but has two duplicate rings</returns>
		public bool HasDuplicateRings()
		{
			foreach(DictionaryEntry node in _nodeGraph) 
			{
				RelateNode relateNode = (RelateNode) node.Value;
				foreach(object ees in relateNode.Edges)
				{
					// awc not sure about all this casting
					EdgeEndBundle eeb = (EdgeEndBundle) ees;
					if (eeb.EdgeEnds.Count > 1) 
					{
						_invalidPoint = eeb.Edge.GetCoordinate(0);
						return true;
					}
				}
			}
			return false;
		}

		#endregion

	}
}

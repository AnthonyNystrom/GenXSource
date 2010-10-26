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
using Geotools.Geometries;
#endregion

namespace Geotools.Graph.Index
{
	/// <summary>
	/// Summary description for SweepLineSegment.
	/// </summary>
	internal class SweepLineSegment
	{

		Edge _edge;
		Coordinates _pts;
		int _ptIndex;
		int _geomIndex;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SweepLineSegment class.
		/// </summary>
		public SweepLineSegment( Edge edge,  int ptIndex, int geomIndex ) 
		{
			_edge = edge;
			_ptIndex = ptIndex;
			_geomIndex = geomIndex;
			_pts = edge.Coordinates;
		}
		#endregion

		#region Properties

		public int PointIndex
		{
			get
			{
				return _ptIndex;
			}
		}

		/// <summary>
		/// Returns the edge object of this class.
		/// </summary>
		public Edge Edge
		{
			get
			{
				return _edge;
			}
		}

		/// <summary>
		/// Returns the minimum X for the line.
		/// </summary>
		public double MinX
		{
			get
			{
				double x1 = _pts[_ptIndex].X;
				double x2 = _pts[_ptIndex + 1].X;
				return x1 < x2 ? x1 : x2;
			}
		}

		/// <summary>
		/// Returns the maximum X for the line.
		/// </summary>
		public double MaxX
		{
			get
			{
				double x1 = _pts[_ptIndex].X;
				double x2 = _pts[_ptIndex + 1].X;
				return x1 > x2 ? x1 : x2;
			}
		}
		

		#endregion

		#region Methods

		/// <summary>
		/// Computes the intersections.
		/// </summary>
		public void ComputeIntersections(SweepLineSegment sweepLineSegment, SegmentIntersector segmentIntersector)
		{
			segmentIntersector.AddIntersections( _edge, _ptIndex, sweepLineSegment.Edge, sweepLineSegment.PointIndex );
		}
		#endregion

	}
}

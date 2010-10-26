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
#endregion

namespace Geotools.Operation.Distance
{
	/// <summary>
	/// DistanceOp computes the distance between two Geometries.
	/// Currently the algorithms used are straightforward O(n^2)
	/// comparisons.  These could definitely be improved on.
	/// </summary>
	internal class DistanceOp
	{
		private static PointLocator _ptLocator = new PointLocator();
		public static double Distance(Geometry g0, Geometry g1)
		{
			DistanceOp distOp = new DistanceOp(g0, g1);
			return distOp.Distance();
		}

		
		private Geometry[] _geom;
		private double _minDistance = Double.MaxValue;

		public DistanceOp(Geometry g0, Geometry g1)
		{
			this._geom = new Geometry[2];
			_geom[0] = g0;
			_geom[1] = g1;
		}

		
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the DistanceOp class.
		/// </summary>
		public DistanceOp()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public double Distance()
		{
			ComputeMinDistance();
			return _minDistance;
		}
		private void UpdateMinDistance(double dist)
		{
			if (dist < _minDistance)
			{
				_minDistance = dist;
			}
		}
		private void ComputeMinDistance()
		{
			ArrayList polys0 = PolygonExtracterFilter.GetPolygons(_geom[0]);
			ArrayList polys1 = PolygonExtracterFilter.GetPolygons(_geom[1]);

			if (polys1.Count > 0) 
			{
				ArrayList insidePts0 = ConnectedElementPointFilter.GetCoordinates(_geom[0]);
				ComputeInside(insidePts0, polys1);
				if (_minDistance <= 0.0) return;
			}
			if (polys0.Count > 0) 
			{
				ArrayList insidePts1 = ConnectedElementPointFilter.GetCoordinates(_geom[1]);
				ComputeInside(insidePts1, polys0);
				if (_minDistance <= 0.0) return;
			}

			ArrayList lines0 = LineExtracterFilter.GetLines(_geom[0]);
			ArrayList lines1 = LineExtracterFilter.GetLines(_geom[1]);

			ArrayList pts0 = PointExtracterFilter.GetPoints(_geom[0]);
			ArrayList pts1 = PointExtracterFilter.GetPoints(_geom[1]);
			if (pts0.Count > 0 || pts1.Count > 0) 
			{
				throw new NotImplementedException("Points not yet implemented.");
			}

			ComputeMinDistance(lines0, lines1);
			if (_minDistance <= 0.0) return;

			ComputeMinDistance(pts0, pts1);
		}

		private void ComputeInside(ArrayList pts, ArrayList polys)
		{
			for (int i = 0; i < pts.Count; i++) 
			{
				Coordinate pt = (Coordinate) pts[i];
				for (int j = 0; j < polys.Count; j++) 
				{
					Polygon poly = (Polygon) polys[j];
					ComputeInside(pt, poly);
					if (_minDistance <= 0.0) return;
				}
			}
		}

		private void ComputeInside(Coordinate pt, Polygon poly)
		{
			if (Location.Exterior != _ptLocator.Locate(pt, poly))
			{
				_minDistance = 0.0;
			}
		}

		private void ComputeMinDistance(ArrayList lines0, ArrayList lines1)
		{
			for (int i = 0; i < lines0.Count; i++) 
			{
				LineString line0 = (LineString) lines0[i];
				for (int j = 0; j < lines1.Count; j++) 
				{
					LineString line1 = (LineString) lines1[j];
					ComputeMinDistance(line0, line1);
				}
			}
		}
		private void ComputeMinDistance(LineString line0, LineString line1)
		{
			if (line0.GetEnvelopeInternal().Distance(line1.GetEnvelopeInternal())> _minDistance)
				return;
			Coordinates coord0 = line0.GetCoordinates();
			Coordinates coord1 = line1.GetCoordinates();
			// brute force approach!
			for (int i = 0; i < coord0.Count - 1; i++) 
			{
				for (int j = 0; j < coord1.Count - 1; j++) 
				{
					double dist = CGAlgorithms.DistanceLineLine(
						coord0[i], coord0[i + 1],
						coord1[j], coord1[j + 1] );
					UpdateMinDistance(dist);
				}
			}
		}

		#endregion

	}
}

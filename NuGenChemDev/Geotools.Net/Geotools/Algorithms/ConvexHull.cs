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

#region Using Statements
using System;
using System.Collections;
using Geotools.Geometries;
using Geotools.Utilities;
#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for ConvexHull.
	/// </summary>
	internal class ConvexHull
	{
		private PointLocator _pointLocator = new PointLocator();
		private CGAlgorithms _cga;
		private Geometry _geometry;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the ConvexHull with the specified CGAlgorithms class.
		/// </summary>
		/// <param name="cga"></param>
		public ConvexHull(CGAlgorithms cga)
		{
			this._cga = cga;
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Gets the convex hull for the given geometry.
		/// </summary>
		/// <param name="geometry"></param>
		/// <returns></returns>
		public Geometry GetConvexHull(Geometry geometry) 
		{

			this._geometry = geometry;
			UniqueCoordinateArrayFilter filter = new UniqueCoordinateArrayFilter();
			geometry.Apply(filter);
			Coordinates pts = filter.GetCoordinates();

			if (pts.Count == 0) 
			{
				return new GeometryCollection(new Geometry[]{},
					geometry.PrecisionModel, geometry.GetSRID());
			}
			if (pts.Count == 1) 
			{
				return new Point(pts[0], geometry.PrecisionModel, geometry.GetSRID());
			}
			if (pts.Count == 2) 
			{
				return new LineString(pts, geometry.PrecisionModel, geometry.GetSRID());
			}

			// sort points for Graham scan.
			Coordinates pspts;
			if (pts.Count > 10) 
			{
				//Probably should be somewhere between 50 and 100?
				Coordinates rpts = Reduce(pts);
				pspts = PreSort(rpts);
			}
			else 
			{
				pspts = PreSort(pts);
			}

			// Use Graham scan to find convex hull.
			Stack cHS = GrahamScan(pspts);

			// Convert stack to an array.
			Coordinates cH = ToCoordinateArray(cHS);

			// Convert array to linear ring.
			//awcreturn lineOrPolygon(cH);
			return LineOrPolygon(cH);

		}

		/// <summary>
		///		An alternative to Stack.ToArray, which is not present in earlier versions of Java.
		/// </summary>
		/// <param name="stack"></param>
		/// <returns></returns>
		protected Coordinates ToCoordinateArray(Stack stack) 
		{
			
			Coordinates coordinates = new Coordinates();
			foreach(object obj in stack)
			{
				Coordinate coordinate = (Coordinate)obj;
				coordinates.Add( coordinate);
			}
			// because taking stuff of a stack, need to reverse resulting array.
			// JTS code used array indexing to make new array, but .Net's Stack does not allow array indexing.
			coordinates.Reverse();
			return coordinates;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		private Coordinates Reduce(Coordinates pts) 
		{
	
			BigQuad bigQuad = this.BigQuadrant(pts);

			// Build a linear ring defining a big poly.
			Coordinates bigPoly = new Coordinates();
			bigPoly.Add(bigQuad._westmost);
			if (!bigPoly.Contains(bigQuad._northmost)) 
			{
				bigPoly.Add(bigQuad._northmost);
			}
			if (!bigPoly.Contains(bigQuad._eastmost)) 
			{
				bigPoly.Add(bigQuad._eastmost);
			}
			if (!bigPoly.Contains(bigQuad._southmost)) 
			{
				bigPoly.Add(bigQuad._southmost);
			}
			if (bigPoly.Count < 3) 
			{
				return pts;
			}
			bigPoly.Add(bigQuad._westmost);
			LinearRing bQ = new LinearRing(bigPoly,
				_geometry.PrecisionModel, _geometry.GetSRID());

			// load an array with all points not in the big poly
			// and the defining points.
			Coordinates reducedSet = new Coordinates(bigPoly);
			for (int i = 0; i < pts.Count; i++) 
			{
				if (_pointLocator.Locate(pts[i], bQ) == Location.Exterior) 
				{
					reducedSet.Add(pts[i]);
				}
			}
			//Coordinates rP = (Coordinates) reducedSet.toArray(new Coordinate[0]);

			// Return this array as the reduced problem.
			return reducedSet;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		private Coordinates PreSort(Coordinates pts) 
		{
	
			Coordinate t;

			// find the lowest point in the set. If two or more points have
			// the same minimum y coordinate choose the one with the minimu x.
			// This focal point is put in array location pts[0].
			for (int i = 1; i < pts.Count; i++) 
			{
				if ((pts[i].Y < pts[0].Y) || ((pts[i].Y == pts[0].Y) && (pts[i].X < pts[0].X))) 
				{
					t = pts[0];
					pts[0] = pts[i];
					pts[i] = t;
				}
			}

			// sort the points radially around the focal point.
			RadialSort(pts);
			return pts;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		private Stack GrahamScan(Coordinates c) 
		{

			Coordinate p;
			//Coordinate p1;
			//Coordinate p2;
			Stack ps = new Stack();
			ps.Push(c[0]);
			ps.Push(c[1]);
			ps.Push(c[2]);
			for (int i = 3; i < c.Count; i++) 
			{
				p = (Coordinate) ps.Pop();
				while (_cga.ComputeOrientation((Coordinate) ps.Peek(), p, c[i]) > 0) 
				{
					p = (Coordinate) ps.Pop();
				}
				ps.Push(p);
				ps.Push(c[i]);
				p=c[i];
			}
			ps.Push(c[0]);
			return ps;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		private void RadialSort(Coordinates p) 
		{
		
			// A selection sort routine, assumes the pivot point is
			// the first point (i.e., p[0]).
			Coordinate t;
			for (int i = 1; i < (p.Count - 1); i++) 
			{
				int min = i;
				for (int j = i + 1; j < p.Count; j++) 
				{
					if (PolarCompare(p[0], p[j], p[min]) < 0) 
					{
						min = j;
					}
				}
				t = p[i];
				p[i] = p[min];
				p[min] = t;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="o"></param>
		/// <param name="p"></param>
		/// <param name="q"></param>
		/// <returns></returns>
		private int PolarCompare(Coordinate o, Coordinate p, Coordinate q) 
		{
		
			// Given two points p and q compare them with respect to their radial
			// ordering about point o. -1, 0 or 1 depending on whether p is less than,
			// equal to or greater than q. First checks radial ordering then if both
			// points lie on the same line, check distance to o.
			double dxp = p.X - o.X;
			double dyp = p.Y - o.Y;
			double dxq = q.X - o.X;
			double dyq = q.Y - o.Y;
			double alph = Math.Atan2(dxp, dyp);
			double beta = Math.Atan2(dxq, dyq);
			if (alph < beta) 
			{
				return -1;
			}
			if (alph > beta) 
			{
				return 1;
			}
			double op = dxp * dxp + dyp * dyp;
			double oq = dxq * dxq + dyq * dyq;
			if (op < oq) 
			{
				return -1;
			}
			if (op > oq) 
			{
				return 1;
			}
			return 0;	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c1"></param>
		/// <param name="c2"></param>
		/// <param name="c3"></param>
		/// <returns>
		///		Returns if the three coordinates are collinear and c2 lies between c1 and c3 inclusive
		///	</returns>
		private bool IsBetween(Coordinate c1, Coordinate c2, Coordinate c3) 
		{
			if (_cga.ComputeOrientation(c1, c2, c3) != 0) 
			{
				return false;
			}
			if (c1.X != c3.X) 
			{
				if (c1.X <= c2.X && c2.X <= c3.X) 
				{
					return true;
				}
				if (c3.X <= c2.X && c2.X <= c1.X) 
				{
					return true;
				}
			}
			if (c1.Y != c3.Y) 
			{
				if (c1.Y <= c2.Y && c2.Y <= c3.Y) 
				{
					return true;
				}
				if (c3.Y <= c2.Y && c2.Y <= c1.Y) 
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pts"></param>
		/// <returns></returns>
		private BigQuad BigQuadrant(Coordinates pts) 
		{
		
			BigQuad bigQuad = new BigQuad();
			bigQuad._northmost = pts[0];
			bigQuad._southmost = pts[0];
			bigQuad._westmost = pts[0];
			bigQuad._eastmost = pts[0];
			for (int i = 1; i < pts.Count; i++) 
			{
				if (pts[i].X < bigQuad._westmost.X) 
				{
					bigQuad._westmost = pts[i];
				}
				if (pts[i].X > bigQuad._eastmost.X) 
				{
					bigQuad._eastmost = pts[i];
				}
				if (pts[i].Y < bigQuad._southmost.Y) 
				{
					bigQuad._southmost = pts[i];
				}
				if (pts[i].Y > bigQuad._northmost.Y) 
				{
					bigQuad._northmost = pts[i];
				}
			}
			return bigQuad;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="coordinates">
		///		The vertices of a linear ring, which may or may not be flattened (i.e. vertices collinear)
		///	</param>
		/// <returns>
		///		A 2-vertex LineStringif the vertices are collinear; otherwise, a Polygon with 
		///		unnecessary (collinear) vertices removed
		///	</returns>
		private Geometry LineOrPolygon(Coordinates coordinates) 
		{
			coordinates = CleanRing(coordinates);
			if (coordinates.Count == 3) 
			{
				Coordinates coords = new Coordinates();
				coords.Add(coordinates[0]);
				coords.Add(coordinates[1]);
				return new LineString(coords,
					_geometry.PrecisionModel, _geometry.GetSRID());
			}

			LinearRing linearRing = new LinearRing(coordinates,
				_geometry.PrecisionModel, _geometry.GetSRID());
			return new Polygon(linearRing, _geometry.PrecisionModel, _geometry.GetSRID());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="original">
		///		The vertices of a linear ring, which may or may not be flattened (i.e. vertices collinear)
		/// </param>
		/// <returns>
		///		The coordinates with unnecessary (collinear) vertices removed
		/// </returns>
		private Coordinates CleanRing(Coordinates original) 
		{
			if (!(original[0]== original[original.Count - 1]))
			{
				throw new InvalidProgramException("Start and end points of ring are not the same.");
			}
			Coordinates cleanedRing = new Coordinates();
			Coordinate previousDistinctCoordinate = null;
			for (int i = 0; i <= original.Count - 2; i++) 
			{
				Coordinate currentCoordinate = original[i];
				Coordinate nextCoordinate = original[i+1];
				if (currentCoordinate.Equals(nextCoordinate)) 
				{
					continue;
				}
				if (previousDistinctCoordinate != null
					&& IsBetween(previousDistinctCoordinate, currentCoordinate, nextCoordinate)) 
				{
					continue;
				}
				cleanedRing.Add(currentCoordinate);
				previousDistinctCoordinate = currentCoordinate;
			}
			cleanedRing.Add(original[original.Count - 1]);
			//Coordinates cleanedRingCoordinates = new Coordinate[cleanedRing.Count];
			//return (Coordinates) cleanedRing.toArray(cleanedRingCoordinates);
			return cleanedRing;

		}

		private class BigQuad 
		{
			public Coordinate _northmost;
			public Coordinate _southmost;
			public Coordinate _westmost;
			public Coordinate _eastmost;
		}

		#endregion
	}
}

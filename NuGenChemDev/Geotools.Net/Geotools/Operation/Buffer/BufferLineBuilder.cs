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
using Geotools.Graph;
using Geotools.Geometries;
using Geotools.Algorithms;
#endregion

namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// Summary description for BufferLineBuilder.
	/// </summary>
	internal class BufferLineBuilder
	{

		internal static int DefaultQuadrantSegments
		{
			get 
			{
				return 6;    // controls point density in fillets
			}
		}
		// should specify max segment length instead?  or # points?

		/**
		 * The minimum segment length allowed to occur in the buffer line.  This is
		 * required to avoid problems with dimensional collapse when the buffer line
		 * is rounded to the precision grid.  The value to use has been determined empirically.
		 * It is independent of the buffer distance and the PrecisionModel scale factor.
		 * It does affect how true the resulting buffer is - geometries with line
		 * segments that are similar in size to the minimum segment length will
		 * produce "choppy" buffers
		 */
		private static double _minSegmentLength = 10.0;
		private static bool _useMinSegmentLength = false;

		private static ArrayList arrayTypeCoordinate= new ArrayList();//[0];

		private CGAlgorithms _cga;
		private LineIntersector _li;
		private LoopFilter _loopFilter= new LoopFilter();

		private double _angleInc;
		private Coordinates _ptList;
		private double _distance = 0.0;
		private bool _makePrecise;

		/// <summary>
		/// A BufferLineBuilder only builds a single ring for all input values, but it
		/// returns it in an array for compatibility with BufferMultiLineBuilder.
		/// </summary>
		private ArrayList _lineList;
		private Coordinate _s0, _s1, _s2;
		private LineSegment _seg0= new LineSegment();
		private LineSegment _seg1 = new LineSegment();
		private LineSegment _offset0 = new LineSegment();
		private LineSegment _offset1 = new LineSegment();
		private int _side = 0;


		#region Constructors

	
		public BufferLineBuilder(CGAlgorithms _cga, LineIntersector li, bool makePrecise, int quadrantSegments)
		{
			
			this._cga = _cga;
			this._li = li;
			this._makePrecise = makePrecise;
			_angleInc = Math.PI / 2.0 / quadrantSegments;
			_lineList = new ArrayList();
			// ensure array has exactly one element
			_lineList.Add(arrayTypeCoordinate);
			
		}
		public BufferLineBuilder(CGAlgorithms _cga, LineIntersector li, bool makePrecise)
			: this(_cga, li, makePrecise, DefaultQuadrantSegments)
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Computes a facet angle that is no grater than angleInc, but divides the totalAngle
		/// into equal slices.
		/// </summary>
		/// <param name="angleInc"></param>
		/// <param name="totalAngle"></param>
		/// <returns></returns>
		private static double FacetAngle(double angleInc, double totalAngle)
		{	
			int nSlices = (int) (totalAngle / angleInc) + 1;
			return totalAngle / nSlices;
		}

		/// <summary>
		/// Computes the angle between two vectors (p-pa) and (p-pb) using the relation:
		/// a.b = |a| |b| cos theta, where a.b = ax.bx + ay.by
		/// </summary>
		/// <param name="pa"></param>
		/// <param name="p"></param>
		/// <param name="pb"></param>
		/// <returns></returns>
		private static double AngleBetween(Coordinate pa, Coordinate p, Coordinate pb)
		{
			
			double aDx = pa.X - p.X;
			double aDy = pa.Y - p.Y;
			double bDx = pb.X - p.X;
			double bDy = pb.Y - p.Y;
			double aDotB = aDx * bDx + aDy * bDy;
			double aSize = Math.Sqrt(aDx * aDx + aDy * aDy);
			double bSize = Math.Sqrt(bDx * bDx + bDy * bDy);
			double cosTheta = aDotB / aSize / bSize;
			double theta = Math.Acos(cosTheta);
			return theta;
		}


		/// <summary>
		/// This method handles single points as well as lines. Lines are assumed to NOT be closed 
		/// (the function will not fail for closed lines, but will generate superfluous line caps).
		/// </summary>
		/// <param name="inputPts"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public ArrayList GetLineBuffer(Coordinates inputPts, double distance)
		{
			
			Init(distance);
			if (inputPts.Count <= 1) 
			{
				AddCircle(inputPts[0], distance);
			}
			else
				ComputeLineBuffer(inputPts);
	
			_lineList[0]=this.GetCoordinates();
			return _lineList;
		}

		/// <summary>
		/// This method handles the degenerate cases of single points and lines, as well as rings.
		/// </summary>
		/// <param name="inputPts"></param>
		/// <param name="side"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public ArrayList GetRingBuffer(Coordinates inputPts, int side, double distance)
		{
			
			Init(distance);
			if (inputPts.Count <= 1)
				AddCircle(inputPts[0], distance);
			else if (inputPts.Count == 2)
				ComputeLineBuffer(inputPts);
			else
				ComputeRingBuffer(inputPts, side);
			_lineList[0]=this.GetCoordinates();
			return _lineList;
		}

		private void Init(double distance)
		{
			
			this._distance = distance;
			_ptList = new Coordinates();
			
		}

		private Coordinates GetCoordinates()
		{
			
			// check that points are a ring - add the startpoint again if they are not
			if (_ptList.Count > 1) 
			{
				Coordinate start  = (Coordinate) _ptList[0];
				Coordinate end    = (Coordinate) _ptList[1];
				if (! start.Equals(end) ) AddPt(start);
			}

			// awc - should this return a copy????
			return _ptList;
			//Coordinate[] coord = (Coordinate[]) ptList.toArray(arrayTypeCoordinate);
			//return coord;
			
			
		}

		private void ComputeLineBuffer(Coordinates inputPts)
		{
			
			int n = inputPts.Count - 1;

			// compute points for left side of line
			InitSideSegments(inputPts[0], inputPts[1], Position.Left);
			for (int i = 2; i <= n; i++) 
			{
				AddNextSegment(inputPts[i], true);
			}
			AddLastSegment();
			AddLineEndCap(inputPts[n - 1], inputPts[n]);

			// compute points for right side of line
			InitSideSegments(inputPts[n], inputPts[n - 1], Position.Left);
			for (int i = n - 2; i >= 0; i--) 
			{
				AddNextSegment(inputPts[i], true);
			}
			AddLastSegment();
			AddLineEndCap(inputPts[1], inputPts[0]);

			ClosePts();
		}

		private void ComputeRingBuffer(Coordinates inputPts, int side)
		{
			
			int n = inputPts.Count - 1;
			InitSideSegments(inputPts[n - 1], inputPts[0], side);
			for (int i = 1; i <= n; i++) 
			{
				bool addStartPoint = i != 1;
				AddNextSegment(inputPts[i], addStartPoint);
			}
			ClosePts();
			
		}

		private void AddPt(Coordinate pt)
		{
			
			Coordinate bufPt = new Coordinate(pt);
			if (_makePrecise)
				bufPt.MakePrecise();
			// don't add duplicate points
			Coordinate lastPt = null;
			if (_ptList.Count >= 1)
				lastPt = (Coordinate) _ptList[_ptList.Count - 1];
			if (lastPt != null && bufPt.Equals(lastPt)) return;

			// if new segment is shorter than tolerance length, skip it
			if (_useMinSegmentLength) 
			{
				if (lastPt != null && bufPt.Distance(lastPt) < _minSegmentLength) return;
			}

			_ptList.Add(bufPt);
			//System.out.println(bufPt);
			
		}


		private void ClosePts()
		{
			
			if (_ptList.Count < 1) return;
			Coordinate startPt = new Coordinate((Coordinate) _ptList[0]);
			Coordinate lastPt = (Coordinate) _ptList[_ptList.Count - 1];
			Coordinate last2Pt = null;
			if (_ptList.Count >= 2)
				last2Pt = (Coordinate) _ptList[_ptList.Count - 2];

			 // If the last point is too close to the start point,
			 // check point n and point n-1 to see which is further from startPoint, and use
			 // whichever is further as the last point
			if (_useMinSegmentLength) 
			{
				if (startPt.Distance(lastPt) < _minSegmentLength && last2Pt != null) 
				{
					if (startPt.Distance(lastPt) < startPt.Distance(last2Pt)) 
					{
						_ptList.RemoveAt(_ptList.Count - 1);
					}
				}
			}
			if (startPt.Equals(lastPt)) return;
			_ptList.Add(startPt);
			
		}

		private void InitSideSegments(Coordinate s1, Coordinate s2, int side)
		{
			
			this._s1 = s1;
			this._s2 = s2;
			this._side = side;
			_seg1.SetCoordinates(s1, s2);
			ComputeOffsetSegment(_seg1, _side, _distance, _offset1);
			
		}

		private void AddNextSegment(Coordinate p, bool addStartPoint)
		{
			
			_s0 = _s1;
			_s1 = _s2;
			_s2 = p;
			_seg0.SetCoordinates(_s0, _s1);
			ComputeOffsetSegment(_seg0, _side, _distance, _offset0);
			_seg1.SetCoordinates(_s1, _s2);
			ComputeOffsetSegment(_seg1, _side, _distance, _offset1);

			// do nothing if points are equal
			if (_s1.Equals(_s2)) return;

			int orientation = _cga.ComputeOrientation(_s0, _s1, _s2);
			bool outsideTurn =
				(orientation == CGAlgorithms.CLOCKWISE        && _side == Position.Left)
				||  (orientation == CGAlgorithms.COUNTERCLOCKWISE && _side == Position.Right);

			if (orientation == 0) 
			{ // lines are collinear
				_li.ComputeIntersection( _s0, _s1,
					_s1, _s2  );
				int numInt = _li.GetIntersectionNum();
				 // if numInt is < 2, the lines are parallel and in the same direction.
				 // In this case the point can be ignored, since the offset lines will also be
				 // parallel.
				if (numInt >= 2) 
				{
					 // segments are collinear but reversing.  Have to add an "end-cap" fillet
					 // all the way around to other direction
					 // This case should ONLY happen for LineStrings, so the orientation is always CW.
					 // (Polygons can never have two consecutive segments which are parallel but reversed,
					 // because that would be a self intersection.
					AddFillet(_s1, _offset0.P1, _offset1.P0, CGAlgorithms.CLOCKWISE, _distance);
				}
			}
			else if (outsideTurn) 
			{
				// add a fillet to connect the endpoints of the offset segments
				if (addStartPoint) AddPt(_offset0.P1);
				AddFillet(_s1, _offset0.P1, _offset1.P0, orientation, _distance);
				AddPt(_offset1.P0);
			}
			else 
			{ // inside turn
				 // add intersection point of offset segments (if any)
				_li.ComputeIntersection( _offset0.P0, _offset0.P1,
					_offset1.P0, _offset1.P1  );
				if (_li.HasIntersection()) 
				{
					AddPt(_li.GetIntersection(0));
				}
					// TESTING - fix problem with narrow angles
				else 
				{
					 // If no intersection, it means the angle is so small and the offset so large
					 // that the offsets segments don't intersect.  The offset segment won't appear in
					 // the final buffer.  However, we can't just drop the segment, since this might
					 // mean the buffer line wouldn't track the buffer correctly around the corner.
					// add both endpoint of this segment and startpoint of next
					AddPt(_offset0.P1);
					AddPt(_offset1.P0);
				}
			}
			
		}

		/// <summary>
		/// Add last offset point.
		/// </summary>
		private void AddLastSegment()
		{
			AddPt(_offset1.P1);
		}

		private void ComputeOffsetSegment(LineSegment seg, int side, double distance, LineSegment offset)
		{
			
			int sideSign = side == Position.Left ? 1 : -1;
			double dx = seg.P1.X - seg.P0.X;
			double dy = seg.P1.Y - seg.P0.Y;
			double len = Math.Sqrt(dx * dx + dy * dy);
			// u is the vector that is the length of the offset, in the direction of the segment
			double ux = sideSign * distance * dx / len;
			double uy = sideSign * distance * dy / len;
			offset.P0.X = seg.P0.X - uy;
			offset.P0.Y = seg.P0.Y + ux;
			offset.P1.X = seg.P1.X - uy;
			offset.P1.Y = seg.P1.Y + ux;
			
		}

		/// <summary>
		/// Add an end cap around point p1, terminating a line segment coming from p0
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		private void AddLineEndCap(Coordinate p0, Coordinate p1)
		{
			
			LineSegment seg = new LineSegment(p0, p1);

			LineSegment offsetL = new LineSegment();
			ComputeOffsetSegment(seg, Position.Left, _distance, offsetL);
			LineSegment offsetR = new LineSegment();
			ComputeOffsetSegment(seg, Position.Right, _distance, offsetR);

			double dx = p1.X - p0.X;
			double dy = p1.Y - p0.Y;
			double angle = Math.Atan2(dy, dx);

			AddPt(offsetL.P1);
			AddFillet(p1, angle + Math.PI / 2, angle - Math.PI / 2, CGAlgorithms.CLOCKWISE, _distance);
			AddPt(offsetR.P1);
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p">p is base point of curve</param>
		/// <param name="p0">p0 is start point of fillet curve</param>
		/// <param name="p1">p1 is endpoint of fillet curve</param>
		/// <param name="direction"></param>
		/// <param name="distance"></param>
		private void AddFillet(Coordinate p, Coordinate p0, Coordinate p1, int direction, double distance)
		{
			
			double dx0 = p0.X - p.X;
			double dy0 = p0.Y - p.Y;
			double startAngle = Math.Atan2(dy0, dx0);
			double dx1 = p1.X - p.X;
			double dy1 = p1.Y - p.Y;
			double endAngle = Math.Atan2(dy1, dx1);

			if (direction == CGAlgorithms.CLOCKWISE) 
			{
				if (startAngle <= endAngle) startAngle += 2.0 * Math.PI;
			}
			else 
			{    // direction == COUNTERCLOCKWISE
				if (startAngle >= endAngle) startAngle -= 2.0 * Math.PI;
			}
			AddPt(p0);
			AddFillet(p, startAngle, endAngle, direction, distance);
			AddPt(p1);
			
		}

		/// <summary>
		/// Add points for a fillet angle.  The start and end point for the fillet are not added -
		/// it is assumed that the caller will add them
		/// </summary>
		/// <param name="p"></param>
		/// <param name="startAngle"></param>
		/// <param name="endAngle"></param>
		/// <param name="direction">direction is -1 for a CW angle, 1 for a CCW angle</param>
		/// <param name="distance"></param>
		private void AddFillet(Coordinate p, double startAngle, double endAngle, int direction, double distance)
		{
			
			int directionFactor = direction < 0 ? -1 : 1;

			double totalAngle = Math.Abs(startAngle - endAngle);
			int nSegs = (int) (totalAngle / _angleInc + 0.5);

			if (nSegs < 1) return;    // no segments because angle is less than increment - nothing to do!

			double initAngle, currAngleInc;
			// choose initAngle so that the segments at each end of the fillet are equal length
			//initAngle = (totalAngle - (nSegs - 1) * angleInc) / 2;
			// currAngleInc = angleInc;

			// choose angle increment so that each segment has equal length
			initAngle = 0.0;
			currAngleInc = totalAngle / nSegs;

			double currAngle = initAngle;
			Coordinate pt = new Coordinate();
			while (currAngle < totalAngle) 
			{
				double angle = startAngle + directionFactor * currAngle;
				pt.X = p.X + _distance * Math.Cos(angle);
				pt.Y = p.Y + _distance * Math.Sin(angle);
				AddPt(pt);
				currAngle += currAngleInc;
			}
			
		}

		/// <summary>
		/// Adds a CW circle around a point.
		/// </summary>
		/// <param name="p"></param>
		/// <param name="distance"></param>
		private void AddCircle(Coordinate p, double distance)
		{
			
			// add start point
			Coordinate pt = new Coordinate(p.X + _distance, p.Y);
			AddPt(pt);
			AddFillet(p, 0.0, 2.0 * Math.PI, -1, _distance);
			
		}		
		#endregion

	}
}

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


using System;
using System.Collections;
using Geotools.Graph;
using Geotools.Algorithms;
using Geotools.Geometries;


namespace Geotools.Operation.Buffer
{
	/// <summary>
	/// Summary description for BufferMultiLineBuilder.
	/// </summary>
	internal class BufferMultiLineBuilder
	{

		private static double QUADRANT_SEGMENTS = 6;    // controls point density in fillets
		// should specify max segment length instead?  or # points?

		/*
		 The minimum segment length allowed to occur in the buffer line.  This is
		 required to avoid problems with dimensional collapse when the buffer line
		 is rounded to the precision grid.  The value to use has been determined empirically.
		 It is independent of the buffer _distance and the PrecisionModel scale factor.
		 It does affect how true the resulting buffer is - geometries with
		*/
//		private static double minSegmentLength = 10.0;
//		private static Coordinates arrayTypeCoordinate = new Coordinates();

		private CGAlgorithms _cga;
		private LineIntersector _li;

		private double _angleInc;
		private double _distance = 0.0;
		LineList _lineList = new LineList();
		private Coordinate _so, _s1, _s2;
		private LineSegment _seg0 = new LineSegment();
		private LineSegment _seg1 = new LineSegment();
		private LineSegment _offset0 = new LineSegment();
		private LineSegment _offset1 = new LineSegment();
		private int _side = 0;



		/// <summary>
		/// Initializes a new instance of the BufferMultiLineBuilder class.
		/// </summary>
		/// <param name="cga"></param>
		/// <param name="li"></param>
		public BufferMultiLineBuilder(CGAlgorithms cga, LineIntersector li)
		{	
			_cga = cga;
			_li = li;
			_angleInc = Math.PI / 2.0 / QUADRANT_SEGMENTS;		
		} // public BufferMultiLineBuilder(CGAlgorithms cga, LineIntersector li)

	

	

		/// <summary>
		/// Computes a facet angle that is no greater than angleInc, but divides the totalAngle
		/// into equal slices.
		/// </summary>
		/// <param name="angleInc"></param>
		/// <param name="totalAngle"></param>
		/// <returns></returns>
		private static double FacetAngle(double angleInc, double totalAngle)
		{
			int nSlices = (int) (totalAngle / angleInc) + 1;
			return totalAngle / nSlices;
		} // private static double FacetAngle(double angleInc, double totalAngle)

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
	
		} // private static double AngleBetween(Coordinate pa, Coordinate p, Coordinate pb)

		/// <summary>
		/// This method handles single points as well as lines. Lines are assumed 
		/// to NOT be closed (the function will not fail for closed lines, but will generate superfluous line caps).
		/// </summary>
		/// <param name="inputPts"></param>
		/// <param name="_distance"></param>
		/// <returns></returns>
		public ArrayList GetLineBuffer(Coordinates inputPts, double _distance)
		{
			
			Init(_distance);
			if (inputPts.Count <= 1) 
			{
				AddCircle(inputPts[0], _distance);
			}
			else
			{
				ComputeLineBuffer(inputPts);
			}
			return _lineList.GetLines();
	
		} // public ArrayList GetLineBuffer(Coordinates inputPts, double _distance)

		/// <summary>
		/// This method handles rings.
		/// </summary>
		/// <param name="inputPts"></param>
		/// <param name="side"></param>
		/// <param name="distance"></param>
		/// <returns></returns>
		public ArrayList GetRingBuffer(Coordinates inputPts, int side, double distance)
		{
			
			Init(_distance);
			if (inputPts.Count <= 1)
			{
				AddCircle(inputPts[0], distance);
			}
			else if (inputPts.Count == 2)
			{
				ComputeLineBuffer(inputPts);
			}
			else
			{
				ComputeRingBuffer(inputPts, side);
			}
			return _lineList.GetLines();

		} // public ArrayList GetRingBuffer(Coordinates inputPts, int side, double distance)

		
		private void Init(double distance)
		{
			_distance = distance;
		} // private void Init(double distance)

		/*  This code was already commented out in the java code.
		public List getLines()
		{
		  return lineList.getLines();
		}
		*/

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputPts"></param>
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

			_lineList.EndEdge();
			
		} // private void ComputeLineBuffer(Coordinates inputPts)

		private void ComputeRingBuffer(Coordinates inputPts, int side)
		{
			
			int n = inputPts.Count - 1;
			InitSideSegments(inputPts[n - 1], inputPts[0], side);
			for (int i = 1; i <= n; i++) 
			{
				bool addStartPoint = i != 1;
				AddNextSegment(inputPts[i], addStartPoint);
			}
			_lineList.EndEdge();
			
		} // private void ComputeRingBuffer(Coordinates inputPts, int side)

		private void InitSideSegments(Coordinate s1, Coordinate s2, int side)
		{
			
			this._s1 = s1;
			this._s2 = s2;
			this._side = side;
			_seg1.SetCoordinates(_s1, _s2);
			ComputeOffsetSegment(_seg1, side, _distance, _offset1);
			
		} // private void InitSideSegments(Coordinate s1, Coordinate s2, int side)

		private void AddNextSegment(Coordinate p, bool addStartPoint)
		{
			
			_so = _s1;
			_s1 = _s2;
			_s2 = p;
			_seg0.SetCoordinates(_so, _s1);
			ComputeOffsetSegment(_seg0, _side, _distance, _offset0);
			_seg1.SetCoordinates(_s1, _s2);
			ComputeOffsetSegment(_seg1, _side, _distance, _offset1);

			// do nothing if points are equal
			if (_s1.Equals(_s2)) return;

			int orientation = _cga.ComputeOrientation(_so, _s1, _s2);
			//if (side == Position.Right) {
			//  orientation *= -1;
			//}
			bool outsideTurn =
				(orientation == CGAlgorithms.CLOCKWISE        && _side == Position.Left)
				||  (orientation == CGAlgorithms.COUNTERCLOCKWISE && _side == Position.Right);

			if (orientation == 0) 
			{ // lines are collinear
				_li.ComputeIntersection( _so, _s1,
					_s1, _s2  );
				int numInt = _li.GetIntersectionNum();

				 // if numInt is < 2, the lines are parallel and in the same direction.
				 // In this case the point can be ignored, since the offset lines will also be
				 // parallel.
				if (numInt >= 2) 
				{

					 // segments are collinear but reversing.  Have to Add an "end-cap" fillet
					 // all the way around to other direction
					 // This case should ONLY happen for LineStrings, so the orientation is always CW.
					 // (Polygons can never have two consecutive segments which are parallel but reversed,
					 // because that would be a self intersection.

					AddFillet(_s1, _offset0.P1, _offset1.P0, CGAlgorithms.CLOCKWISE, _distance);
				}
			}
			else if (outsideTurn) 
			{
				// Add a fillet to connect the endpoints of the offset segments
				// Add the endpoints of the flanking offset segments as well
				if (addStartPoint) _lineList.AddPt(_offset0.P1);
				AddFillet(_s1, _offset0.P1, _offset1.P0, orientation, _distance);
				_lineList.AddPt(_offset1.P0);
				// Add a point to define the offset for the segment being Added
				_lineList.AddPt(_offset1.P1);
			}
			else 
			{ // inside turn

				 // Add intersection point of offset segments (if any)
				_li.ComputeIntersection( _offset0.P0, _offset0.P1,
					_offset1.P0, _offset1.P1  );
				if (_li.HasIntersection()) 
				{
					_lineList.AddPt(_li.GetIntersection(0));
				}
					// TESTING - fix problem with narrow angles
				else 
				{
					 // inside turn but offsets don't intersect one another.
					 // This means the angle is so acute the offsets actually lie inside the
					 // buffer polygon.
					 // In this case Add the endpoints of the offsets, but ensure
					 // they are in two different edges of the set of buffer edges.
					_lineList.AddPt(_offset0.P1);
					_lineList.EndEdge();
					_lineList.AddPt(_offset1.P0);
				} // if (_li.HasIntersection()) 
			} // else inside turn
			
		} // private void AddNextSegment(Coordinate p, bool addStartPoint)

		/// <summary>
		/// Add last offset point.
		/// </summary>
		private void AddLastSegment()
		{
			_lineList.AddPt(_offset1.P1);
		} // private void AddLastSegment()

		private void ComputeOffsetSegment(LineSegment seg, int side, double _distance, LineSegment offset)
		{
			
			int sideSign = side == Position.Left ? 1 : -1;
			double dx = seg.P1.X - seg.P0.X;
			double dy = seg.P1.Y - seg.P0.Y;
			double len = Math.Sqrt(dx * dx + dy * dy);
			// u is the vector that is the length of the offset, in the direction of the segment
			double ux = sideSign * _distance * dx / len;
			double uy = sideSign * _distance * dy / len;
			offset.P0.X = seg.P0.X - uy;
			offset.P0.Y = seg.P0.Y + ux;
			offset.P1.X = seg.P1.X - uy;
			offset.P1.Y = seg.P1.Y + ux;
		
		} // private void ComputeOffsetSegment(LineSegment seg, int side, double _distance, LineSegment offset)

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

			_lineList.AddPt(offsetL.P1);
			AddFillet(p1, angle + Math.PI / 2, angle - Math.PI / 2, CGAlgorithms.CLOCKWISE, _distance);
			_lineList.AddPt(offsetR.P1);
			
		} // private void AddLineEndCap(Coordinate p0, Coordinate p1)

		/// <summary>
		/// Add a fillet based at a given point.
		/// </summary>
		/// <param name="p">p is base point of curve</param>
		/// <param name="p0">p0 is start point of fillet curve</param>
		/// <param name="p1">p1 is endpoint of fillet curve</param>
		/// <param name="direction"></param>
		/// <param name="_distance"></param>
		private void AddFillet(Coordinate p, Coordinate p0, Coordinate p1, int direction, double _distance)
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
			// Add the endpoints for the fillet, with the fillet points inbetween them
			_lineList.AddPt(p0);
			AddFillet(p, startAngle, endAngle, direction, _distance);
			_lineList.AddPt(p1);
			
		} // private void AddFillet(Coordinate p, Coordinate p0, Coordinate p1, int direction, double _distance)

		/// <summary>
		/// Add points for a fillet angle.  The start and end point for the fillet are not Added -
		/// it is assumed that the caller will Add them
		/// </summary>
		/// <param name="p"></param>
		/// <param name="startAngle"></param>
		/// <param name="endAngle"></param>
		/// <param name="direction">direction is -1 for a CW angle, 1 for a CCW angle</param>
		/// <param name="_distance"></param>
		private void AddFillet(Coordinate p, double startAngle, double endAngle, int direction, double _distance)
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
				_lineList.AddPt(pt);
				currAngle += currAngleInc;
			} // while (currAngle < totalAngle) 
			
		} // private void AddFillet(Coordinate p, double startAngle, double endAngle, int direction, double _distance)

		/// <summary>
		/// Adds a CW circle around a point
		/// </summary>
		/// <param name="p"></param>
		/// <param name="_distance"></param>
		private void AddCircle(Coordinate p, double _distance)
		{
			
			// Add start point
			Coordinate pt = new Coordinate(p.X + _distance, p.Y);
			_lineList.AddPt(pt);
			AddFillet(p, 0.0, 2.0 * Math.PI, -1, _distance);
			
		} // private void AddCircle(Coordinate p, double _distance)


	} // public class BufferMultiLineBuilder
}

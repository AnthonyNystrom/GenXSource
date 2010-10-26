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
using Geotools.Index.IntervalTree;
using Geotools.Index.STRTree;


#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// 
	/// </summary>
	internal class SIRtreePointInRing : IPointInRing
	{
		private LinearRing _ring;
		private SIRtree _sirTree;
		private int _crossings = 0;  // number of segment/ray crossings

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the IntTreePointInRing with the specified properties.
		/// </summary>
		/// <param name="ring">The LinearRing to use.</param>
		public SIRtreePointInRing(LinearRing ring)
		{

			this._ring = ring;
			BuildIndex();

		}

		#endregion


		#region Methods

		/// <summary>
		/// 
		/// </summary>
		private void BuildIndex()
		{
		
			Envelope env = _ring.GetEnvelopeInternal();
			_sirTree = new SIRtree();

			Coordinates pts = Coordinates.RemoveRepeatedPoints(_ring.GetCoordinates());
			for (int i = 1; i < pts.Count; i++) 
			{
				LineSegment seg = new LineSegment(pts[i - 1], pts[i]);
				_sirTree.Insert(seg.P0.Y, seg.P1.Y, seg);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pt"></param>
		/// <returns></returns>
		public bool IsInside(Coordinate pt)
		{
			
			_crossings = 0;

			// test all segments intersected by vertical ray at pt

			ArrayList segs = _sirTree.Query(pt.Y);
			//System.out.println("query size = " + segs.size());

			//for (Iterator i = segs.iterator(); i.hasNext(); ) 
			foreach(object obj in segs)
			{
				LineSegment seg = (LineSegment)obj;
				TestLineSegment(pt, seg);
			}

			//
			//  p is inside if number of crossings is odd.
			//
			if ((_crossings % 2) == 1) 
			{
				return true;
			}
			return false;
		}

		private void TestLineSegment(Coordinate p, LineSegment seg) 
		{
		
			double xInt;  // x intersection of segment with ray
			double x1;    // translated coordinates
			double y1;
			double x2;
			double y2;

			//
			//  Test if segment crosses ray from test point in positive x direction.
			//
			Coordinate p1 = seg.P0;
			Coordinate p2 = seg.P1;
			x1 = p1.X - p.X;
			y1 = p1.Y - p.Y;
			x2 = p2.X - p.X;
			y2 = p2.Y - p.Y;

			if (((y1 > 0) && (y2 <= 0)) ||
				((y2 > 0) && (y1 <= 0))) 
			{
				//
				//  segment straddles x axis, so compute intersection.
				//
				xInt = RobustDeterminant.SignOfDet2x2(x1, y1, x2, y2) / (y2 - y1);
				//xsave = xInt;
				//
				//  crosses ray if strictly positive intersection.
				//
				if (0.0 < xInt) 
				{
					_crossings++;
				}
			}
		}
		#endregion
	}
}

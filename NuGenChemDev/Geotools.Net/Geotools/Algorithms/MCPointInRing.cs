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
using Geotools.Index.Chain;
using Geotools.Index.STRTree;
using Geotools.Index.BinTree;
#endregion

namespace Geotools.Algorithms
{
	/// <summary>
	/// Summary description for MCPointInRing.
	/// </summary>
	internal class MCPointInRing : IPointInRing
	{
		private class MCSelecter : MonotoneChainSelectAction
		{
			MCPointInRing _nested;
			Coordinate _p;

			public MCSelecter(MCPointInRing nested,Coordinate p)
			{
				_nested = nested;
				this._p = p;
			}

			public override void Select(LineSegment ls)
			{
				_nested.TestLineSegment(_p, ls);
			}
		}
		
		private LinearRing _ring;
		private BinTree _tree;
		private Geotools.Index.BinTree.Interval _interval = new Geotools.Index.BinTree.Interval();		// needs to be fully qualified.
		private int _crossings = 0;  // number of segment/ray crossings

		#region Constructor
		/// <summary>
		/// 
		/// </summary>
		public MCPointInRing(LinearRing ring)
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
			_tree = new BinTree();

			Coordinates pts = Coordinates.RemoveRepeatedPoints(_ring.GetCoordinates());
			ArrayList mcList = MonotoneChainBuilder.GetChains(pts);

			for (int i = 0; i < mcList.Count; i++) 
			{
				MonotoneChain mc = (MonotoneChain) mcList[i];
				Envelope mcEnv = mc.GetEnvelope();
				_interval.Min = mcEnv.MinY;
				_interval.Max = mcEnv.MaxY;
				_tree.Insert( _interval, mc );
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

			// test all segments intersected by ray from pt in positive x direction
			Envelope rayEnv = new Envelope(Double.NegativeInfinity, Double.PositiveInfinity, pt.Y, pt.Y);

			_interval.Min = pt.Y;
			_interval.Max = pt.Y;
			ArrayList segs = _tree.Query( _interval );

			//System.out.println("query size = " + segs.size());

			MCSelecter mcSelecter = new MCSelecter(this,pt);
			foreach(object obj in segs)
			{
				MonotoneChain mc = (MonotoneChain) obj;
				TestMonotoneChain(rayEnv, mcSelecter, mc);
			}

			//p is inside if number of crossings is odd.
			 
			if ((_crossings % 2) == 1) 
			{
				return true;
			}
			return false;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rayEnv"></param>
		/// <param name="mcSelecter"></param>
		/// <param name="mc"></param>
		private void TestMonotoneChain(Envelope rayEnv, MCSelecter mcSelecter, MonotoneChain mc)
		{
			mc.Select(rayEnv, mcSelecter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		/// <param name="seg"></param>
		private void TestLineSegment(Coordinate p, LineSegment seg)
		{
			
			double xInt;  // x intersection of segment with ray
			double x1;    // translated coordinates
			double y1;
			double x2;
			double y2;

			//Test if segment crosses ray from test point in positive x direction.
			Coordinate p1 = seg.P0;
			Coordinate p2 = seg.P1;
			x1 = p1.X - p.X;
			y1 = p1.Y - p.Y;
			x2 = p2.X - p.X;
			y2 = p2.Y - p.Y;

			if (((y1 > 0) && (y2 <= 0)) ||
				((y2 > 0) && (y1 <= 0))) 
			{
				//segment straddles x axis, so compute intersection.
				xInt = RobustDeterminant.SignOfDet2x2(x1, y1, x2, y2) / (y2 - y1);
				//xsave = xInt;
				//crosses ray if strictly positive intersection.
				if (0.0 < xInt) 
				{
					_crossings++;
				}
			}
		}
		#endregion
	}
}

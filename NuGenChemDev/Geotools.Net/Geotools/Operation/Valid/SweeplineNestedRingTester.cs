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
using Geotools.Index.Sweepline;

#endregion

namespace Geotools.Operation.Valid
{
	/// <summary>
	/// Summary description for SweeplineNestedRingTester.
	/// </summary>
	internal class SweeplineNestedRingTester
	{
		class OverlapAction: ISweepLineOverlapAction
		{
			bool _isNonNested = true;
			SweeplineNestedRingTester _nestedClass;


			/* need this constructor, so the call to IsInside compiles in the Overlap function.
			 * C# does not give access to fields to nested classes (apparently Java does).
			 */
			public OverlapAction(SweeplineNestedRingTester nestedClass)
			{
				_nestedClass = nestedClass;
			}
			public bool IsNonNested
			{
				get
				{
					return _isNonNested;
				}
			}
			public void Overlap(SweepLineInterval s0, SweepLineInterval s1)
			{
				
				LinearRing innerRing = (LinearRing) s0.Item;
				LinearRing searchRing = (LinearRing) s1.Item;
				if (innerRing == searchRing) return;

				if (_nestedClass.IsInside(innerRing, searchRing))
					_isNonNested = false;
				
				//throw new NotImplementedException();
			}

		}

		private static CGAlgorithms _cga = new RobustCGAlgorithms();

		private GeometryGraph _graph;  // used to find non-node vertices
		private ArrayList _rings = new ArrayList();
		private Envelope _totalEnv = new Envelope();
		private SweepLineIndex _sweepLine;
		private Coordinate _nestedPt = null;


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the SweeplineNestedRingTester class.
		/// </summary>
		public SweeplineNestedRingTester(GeometryGraph graph)
		{
			this._graph = graph;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods
		public Coordinate GetNestedPoint()
		{
			return _nestedPt;
		}

		public void Add(LinearRing ring)
		{
			_rings.Add(ring);
		}
		public bool IsNonNested()
		{
			
			BuildIndex();

			OverlapAction action = new OverlapAction(this);

			_sweepLine.ComputeOverlaps(action);
			return action.IsNonNested;

		}
		private void BuildIndex()
		{
			_sweepLine = new SweepLineIndex();

			for (int i = 0; i < _rings.Count; i++) 
			{
				LinearRing ring = (LinearRing) _rings[i];
				Envelope env = ring.GetEnvelopeInternal();
				SweepLineInterval sweepInt = new SweepLineInterval(env.MinX, env.MaxX, ring);
				_sweepLine.Add(sweepInt);
			}
			
		}
		private bool IsInside(LinearRing innerRing, LinearRing searchRing)
		{
			
			Coordinates innerRingPts = innerRing.GetCoordinates();
			Coordinates searchRingPts = searchRing.GetCoordinates();

			if (! innerRing.GetEnvelopeInternal().Intersects(searchRing.GetEnvelopeInternal()))
				return false;

			Coordinate innerRingPt = IsValidOp.FindPtNotNode(innerRingPts, searchRing, _graph);
			//Assert.isTrue(innerRingPt != null, "Unable to find a ring point not a node of the search ring");

			bool isInside = _cga.IsPointInRing(innerRingPt, searchRingPts);
			if (isInside) 
			{
				_nestedPt = innerRingPt;
				return true;
			}
			return false;
		}

		#endregion

	}
}

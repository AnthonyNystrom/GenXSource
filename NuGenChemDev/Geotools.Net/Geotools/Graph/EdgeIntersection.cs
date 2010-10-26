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
using System.IO;
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Graph
{
	/// <summary>
	/// An EdgeIntersection represents a point on an
	/// edge which intersects with another edge.
	/// </summary>
	/// <remarks>
	/// The intersection may either be a single point, or a line segment
	/// (in which case this point is the start of the line segment)
	/// The label attached to this intersection point applies to
	/// the edge from this point forwards, until the next
	/// intersection or the end of the edge.
	/// The intersection point must be precise.
	/// </remarks>
	internal class EdgeIntersection
	{
		protected Coordinate _coordinate;   // the point of intersection
		protected int _segmentIndex;   // the index of the containing line segment in the parent edge
		public double _distance;        // the edge distance of this point along the containing line segment

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the EdgeIntersection class.
		/// </summary>
		public EdgeIntersection(Coordinate coord, int segmentIndex, double dist) 
		{
			//this.edge = edge;
			_coordinate = new Coordinate(coord);
			_segmentIndex = segmentIndex;
			_distance = dist;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The edge distance of this point along the containing line segment.
		/// </summary>
		public double Distance
		{
			get
			{
				return _distance;
			}
		}
		/// <summary>
		/// The index of the containing line segment in the parent edge.
		/// </summary>
		public int SegmentIndex
		{
			get
			{
				return _segmentIndex;
			}
		}

		/// <summary>
		/// The point of intersection.
		/// </summary>
		public Coordinate Coordinate
		{
			get
			{
				return _coordinate;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Compares segmentIndex and distance with this objects values.
		/// </summary>
		/// <param name="segmentIndex"></param>
		/// <param name="distance"></param>
		/// <returns>Returns -1 if this EdgeIntersection is located before the argument location.
		///			 Returns 0 if this EdgeIntersection is at the argument location.
		///			 Returns 1 if this EdgeIntersection is located after the argument location.
		///	</returns>
		public int Compare(int segmentIndex, double distance)
		{
			int returnValue = 0;
			if ( _segmentIndex < segmentIndex )
			{
				returnValue = -1;
			}
			else if ( _segmentIndex > segmentIndex )
			{
				returnValue = 1;
			}
			else if ( _distance < distance )
			{
				returnValue = -1;
			}
			else if ( _distance > distance )
			{
				returnValue = 1;
			}
			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maxSegmentIndex"></param>
		/// <returns></returns>
		public bool IsEndPoint(int maxSegmentIndex)
		{
			if ( _segmentIndex == 0 && _distance == 0.0) return true;
			if ( _segmentIndex == maxSegmentIndex) return true;
			return false;
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( _coordinate.ToString() );
			sb.Append(" seg # = " + _segmentIndex);
			sb.Append(" dist = " + _distance );
			return sb.ToString();
		} // public override string ToString()
		#endregion

	}
}

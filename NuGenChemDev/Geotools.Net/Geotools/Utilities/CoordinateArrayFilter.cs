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
using System.Diagnostics;
using Geotools.Geometries;
#endregion

namespace Geotools.Utilities
{
	/// <summary>
	///  A CoordinateArrayFilter returns an array containing every
	///  coordinate in a Geometry.
	/// </summary>
	public class CoordinateArrayFilter : ICoordinateFilter
	{

		Coordinates _pts = new Coordinates();
		int _n = 0;

		#region Constructors
		/// <summary>
		/// Constructs a CoordinateArrayFilter
		/// </summary>
		/// <param name="size">The number of points that the CoordinateArrayFilter will collect.</param>
		public CoordinateArrayFilter(int size) 
		{
			_n = size;
		}

		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Returns the gathered Coordinates.
		/// </summary>
		/// <returns></returns>
		public Coordinates GetCoordinates() 
		{
			// awc: not sure if the size the coordinates returned have to match the size
			// set in the constructor.
			Debug.Assert(_pts.Count==_n,"size set in ctor. does not match count of array returned");
			return _pts;

		}

		public void Filter(Coordinate coord) 
		{
			_pts.Add(coord);
		}

		#endregion

	}
}

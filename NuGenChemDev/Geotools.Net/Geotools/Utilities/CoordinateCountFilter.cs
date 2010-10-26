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

namespace Geotools.Utilities
{
	/// <summary>
	/// Determines the number of coordinates.
	/// </summary>
	public class CoordinateCountFilter : ICoordinateFilter
	{
		private int _n = 0;

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the CoordinateCountFilter class.
		/// </summary>
		public CoordinateCountFilter()
		{
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Returns the result of the filtering.
		/// </summary>
		/// <returns>Returns the number of points found by this CoordinateCountFilter.</returns>
		public int GetCount() 
		{
			return _n;
		}

		public void Filter(Coordinate coord) 
		{
			_n++;
		}

		#endregion

	}
}

/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
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
#endregion

namespace Geotools.Positioning
{
	/// <summary>
	/// A box defined by two positions
	/// </summary>
	/// <remarks>
	/// The two positions must have the same dimension.
	/// Each of the ordinate values in the minimum point must be less than or equal
	/// to the corresponding ordinate value in the maximum point.  Please note that
	/// these two points may be outside the valid domain of their coordinate system.
	/// (Of course the points and envelope do not explicitly reference a coordinate
	/// system, but their implicit coordinate system is defined by their context.)
	/// </remarks>
	public struct Envelope
	{
		/// <summary>
		/// Point containing minimum ordinate values. 
		/// </summary>
		public CoordinatePoint MaxCP;
		/// <summary>
		/// Point containing maximum ordinate values.
		/// </summary>
		public CoordinatePoint MinCP;
	}
}

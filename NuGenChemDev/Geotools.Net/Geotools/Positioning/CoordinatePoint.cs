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
	/// A position defined by a list of numbers.
	/// </summary>
	/// <remarks>
	/// The ordinate values are indexed from 0 to (NumDim-1), where NumDim is the
	/// dimension of the coordinate system the coordinate point belongs in.
	/// </remarks>
	public struct CoordinatePoint
	{
		/// <summary>
		/// The ordinates of the coordinate point.
		/// </summary>
		public double[] Ord;
 
	}
}

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
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A closed, simple LineString. Consecutive points are not allowed
	///  to be equal.
	/// </summary>
	/// <remarks>
	/// <para>
	///  LinearRings are the fundamental building block for Polygon
	///  s. LinearRings may not be degenerate; that is, a LinearRing
	///  must have at least 3 points. Other non-degeneracy criteria are implied by
	///  the requirement that LinearRings be simple. For instance, not
	///  all the points may be collinear. The SFS does not specify a requirement on
	///  the orientation of a LinearRing, and JTS follows this by
	///  allowing them to be oriented either clockwise or counter-clockwise.</para>
	///
	/// <para>
	///  If the LinearRing is empty, IsClosed and IsRing return true.</para>
	///  </remarks>
	public interface ILinearRing : ILineString
	{
	}
}

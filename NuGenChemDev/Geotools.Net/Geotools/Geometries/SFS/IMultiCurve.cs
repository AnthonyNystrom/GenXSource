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
	/// A GeometryCollection of Curves.
	/// </summary>
	///
	///<remarks>
	///  <para>A MultiCurve is simple if its Curves are simple
	///  and if its only self-intersections are boundary-boundary intersections (that
	///  is, not boundary-interior intersections). </para>
	///
	///  <para>The SFS specifies using a "Mod-2" rule for determining the boundary of a
	///  MultiCurve. A point is on the boundary of the MultiCurve iff it is on the
	///  boundary of an odd number of elements of the MultiCurve. It should be noted
	///  that this leads to cases where the set of points in the SFS boundary is
	///  larger than either intuition or point-set topology would indicate. That is,
	///  a point with an odd number > 1 of edges incident on it is on the boundary
	///  according to the SFS rule, but might not intuitively be considered as part
	///  of the boundary. This also is inconsistent with the topological definition
	///  of boundary, which is "the set of points which are not contained in any open
	///  subset of the set of points in the Geometry". </para>
	///
	///  <para>If a MultiCurves Curves are all closed, then the
	///  MultiCurves boundary is empty. </para>
	///
	///  <para>GetDimension returns 1 even if the MultiCurve is
	///  empty.</para></remarks>
	public interface IMultiCurve : IGeometryCollection
	{
		/// <summary>
		/// Returns true if the start point and the end point of each of this MultiCurve's curve are equal.
		/// </summary>
		/// <returns>Returns true if this MultiCurve's Curves are all closed.</returns>
		bool IsClosed();
	}
}

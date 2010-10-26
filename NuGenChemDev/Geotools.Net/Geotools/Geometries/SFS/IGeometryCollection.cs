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
	///  A Geometry that is a collection of one or more Geometrys.
	/// </summary>
	/// <remarks>
	/// <para>
	///  GetDimension returns -1 if the GeometryCollection
	///  contains no Geometrys. Otherwise, it returns the maximum
	///  dimension of its Geometrys. </para>
	///
	/// <para>
	///  A GeometryCollection is simple if all its elements are simple
	///  and the only intersections between any two elements occur at points that are
	///  on the boundaries of both elements. </para>
	///
	/// <para>
	///  According to the SFS Section 2.1.13.1, "The boundary of an arbitrary
	///  collection of geometries whose interiors are disjoint consist of geometries
	///  drawn from the boundaries of the element geometries by application of the
	///  Mod-2 rule."</para>
	///  </remarks>
	public interface IGeometryCollection : IGeometry
	{
		/// <summary>
		/// Returns this GeometryCollections element count.
		/// </summary>
		/// <returns>Returns the number of Geometries in this collection.</returns>
		int GetNumGeometries();

		/// <summary>
		/// Returns the element at the given index.
		/// </summary>
		/// <param name="n">The index of the Geometry to return.</param>
		/// <returns>Returns the nth geometry in this collection.</returns>
		Geometry GetGeometryN(int n);
	}
}

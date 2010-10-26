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
	/// A one-dimensional geometric object. Curves may not be degenerate. That is,
	/// non-empty Curves must have at least 2 points, and no two consecutive points
	/// may be equal.
	/// </summary>
	/// 
	/// <remarks>
	/// <para>
	/// IsSimple returns true if the Curve does not pass through the same point more than once. </para>
	///
	///<para>
	/// The boundary of a closed curve is the empty geometry. The boundary of a
	/// non-closed curve is the two endpoints. </para>
	///
	/// For a precise definition of a curve, see the 
	/// <A HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features
	/// Specification for SQL</A>.
	///</remarks>
	public interface ICurve : IGeometry
	{
		/// <summary>
		/// Returns the first point with which this curve object was constructed.
		/// </summary>
		/// <returns>Returns the start point or null if this curve is empty.</returns>
		Point GetStartPoint();

		/// <summary>
		/// Returns the last point with which this Curve object was
		/// constructed.
		/// </summary>
		/// <returns>Returns the end point or null if this curve is empty
		/// </returns>
		Point GetEndPoint();

		/// <summary>
		/// Returns true if the start point and the end point are equal.
		/// </summary>
		/// <returns>Returns whether the start and end point are equal.  Classes implementing curve
		/// should document what IsClosed returns if the curve is empty.
		/// </returns>
		bool IsClosed();

		/// <summary>
		/// Returns true if this curve is closed and simple.
		/// </summary>
		/// <returns>
		/// Returns true if this curve is closed and does not pass through the same point more than once.
		/// </returns>
		bool IsRing();
	}
}

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
	///  A Curve with linear interpolation between points.
	/// </summary>
	///  <remarks>
	///  <para>
	///  We are using the definition of LineString given in the
	///  <A HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features
	///  Specification for SQL</A> . This differs in an important way from some other
	///  spatial models (e.g. the one use by ESRI ArcSDE). The difference is that
	///  LineStrings may be non-simple. They may self-intersect in
	///  points or line segments. </para>
	///
	///   <para>In fact boundary points of a curve (e.g. the endpoints) may intersect the
	///  interior of the curve, resulting in a curve that is technically
	///  topologically closed but not closed according to the SFS. In this case
	///  topologically the point of intersection would not be on the boundary of the
	///  curve. However, according to the SFS definition the point is considered to
	///  be on the boundary, and JTS follows this definition. </para>
	///
	///   <para>If the LineString is empty, isClosed and isRing
	///  return false. </para>
	///
	///   <para>A LineString is simple if it does not pass through the same
	///  point twice (excepting the endpoints, which may be identical)</para>
	///  </remarks>
	public interface ILineString : ICurve
	{
		/// <summary>
		/// Returns this LineStrings point count.
		/// </summary>
		/// <returns>Return the number of Points in this LineString</returns>
		int GetNumPoints();

		/// <summary>
		/// Returns the Point at the given index.
		/// </summary>
		/// <param name="n">The index of the Point to return.</param>
		/// <returns>Returns the nth Point in this LineString.</returns>
		Point GetPointN(int n);

		/// <summary>
		/// Returns the Coordinate at the given index.
		/// </summary>
		/// <param name="n">The index of the Coordinate to return.</param>
		/// <returns>The nth Coordinate in this LineString.</returns>
		Coordinate GetCoordinateN(int n);
	}
}

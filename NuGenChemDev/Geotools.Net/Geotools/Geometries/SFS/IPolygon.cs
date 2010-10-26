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
	///  A simple, planar Surface bounded by one exterior LinearRing
	///  (the "shell") and zero or more interior LinearRings (the
	///  "holes").
	/// </summary>
	///
	///	 <remarks>
	///  The shell and holes of a Polygon are LinearRings.
	///  The SFS definition of Polygon has the following implications:
	///
	///  <UL>
	///    <LI> The shell and holes cannot self-intersect</LI>
	///    <LI> Holes can touch the shell or another hole at a single point only.
	///    This means that holes cannot intersect one another at multiple points or
	///    in a line segment.</LI>
	///    <LI> Polygon interiors must be connected</LI>
	///    <LI> There is no requirement that a point where a hole touches the shell
	///    be a vertex.</LI>
	///  </UL>
	///  <para>
	///  Note that the SFS definition of Polygon differs from that in
	///  some other commonly used spatial models. For instance, the ESRI ArcSDE
	///  spatial model allows shells to self-intersect at vertices, but does not
	///  allow holes to touch the shell. The SFS and the ArcSDE model are equivalent
	///  in the sense that they allow describing exactly the same set of areas.
	///  However, they may require different polygon structures to describe the same
	///  area. </para>
	///
	///  <para>Polygons do not have cut lines, spikes or punctures. </para>
	///
	///  <para>Two boundary rings may intersect at one point at most. </para>
	///
	///  <para>Empty Polygons may not contain holes. </para>
	///
	///  <para>Since the shell and holes of Polygons are LinearRing
	///  s, there is no requirement on their orientation. They may be oriented either
	///  clockwise or counterclockwise. </para>
	///
	///  <para>For a precise definition of a polygon, see the
	///  <A HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features
	///  Specification for SQL</A> .</para>
	///  </remarks>
	public interface IPolygon : ISurface
	{
		/// <summary>
		/// Returns the shell.
		/// </summary>
		/// <returns>Returns the exterior boundary, or null if this Polygon is the empty geometry.</returns>
		LinearRing GetExteriorRing();

		/// <summary>
		/// Returns the number of holes.
		/// </summary>
		/// <returns>Returns the number of interior boundaries.</returns>
		int GetNumInteriorRing();

		/// <summary>
		/// Returns the hole at the given index.
		/// </summary>
		/// <param name="n">The index of the interior boundary to return.</param>
		/// <returns>The nth interior LinearRing in this Polygon.</returns>
		LinearRing GetInteriorRingN(int n);
	}
}

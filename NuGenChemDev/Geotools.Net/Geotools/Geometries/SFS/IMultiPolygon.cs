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
	/// <para>
	/// A MultiSurface of Polygons. MultiPolygons do not have cut lines, spikes or punctures. 
	/// </summary>
	/// <remarks>
	/// For a precise definition of multipolygons, see the 
	/// <A HREF="http://www.opengis.org/techno/specs.htm">
	///  OpenGIS Simple Features Specification for SQL</A>.</para>
	/// <para>
	///  The element Polygons in a MultiPolygon may touch
	///  at only a finite number of points (e.g. they may not touch in a line
	///  segment). The interiors of the elements must be disjoint (e.g. they may not
	///  cross). There is no requirement that a point of intersection be a vertex.
	///  </para>
	/// </remarks>
	public interface IMultiPolygon : IMultiSurface
	{
	}
}

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
	/// A MultiCurve of LineStrings.
	/// </summary>
	/// <remarks>
	/// <para>
	///  A MultiLineString is simple iff all of its element LineStrings are simple and the only intersections 
	///  between any two elements occur at points that are on the boundaries of both LineStrings.</para>
	///
	///  <para>The boundary of a non-empty MultiLineString is a MultiPoint
	///  obtained by applying the Mod-2 rule to the boundaries of the element LineStrings.</para>
	/// </remarks>
	public interface IMultiLineString : IMultiCurve
	{
	}
}

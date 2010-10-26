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
using System.Collections;
using Geotools.Geometries;
#endregion

namespace Geotools.Operation.Distance
{
	/// <summary>
	/// A LineExtracterFilter extracts all the component LineStrings from a Geometry
	/// and returns them in a list
	/// </summary>
	internal class LineExtracterFilter : IGeometryComponentFilter
	{
		public static ArrayList GetLines(Geometry geom)
		{
			ArrayList lines = new ArrayList();
			geom.Apply(new LineExtracterFilter(lines));
			return lines;
		}
		private ArrayList _lines;

		LineExtracterFilter(ArrayList lines)
		{
			this._lines = lines;
		}

		public void Filter(Geometry geom)
		{
			if (geom is LineString) _lines.Add(geom);
		}
	}
}

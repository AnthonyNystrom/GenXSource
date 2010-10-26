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
	/// A PointExtracterFilter extracts all the component Points from a Geometry
	/// and returns them in a list
	/// </summary>
	internal class PointExtracterFilter : IGeometryFilter
	{
		
		public static ArrayList GetPoints(Geometry geom)
		{
			ArrayList pts = new ArrayList();
			geom.Apply(new PointExtracterFilter(pts));
			return pts;
		}

		private ArrayList _pts;

		PointExtracterFilter(ArrayList pts)
		{
			this._pts = pts;
		}

		public void Filter(Geometry geom)
		{
			if (geom is Point) _pts.Add(geom);
		}
	}
}

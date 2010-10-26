/*
 *  Copyright (C) 2002 Urban Science Applications, Inc.
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
	/// The filter flips the Y coordinate. 
	/// </summary>
	/// <remarks>
	/// <para>This filter is useful when displaying geometries that are in projected meters
	/// and are being displayed using SVG. The origin for SVG is top-left; the origin 
	/// for projected meters is bottom-left. This resulting in images that are 'upside-down'.
	/// This filter flips the Y coordinates to make the image not 'upside-down'.</para>
	/// <para>geometry.GeometryChanged() does not have to be called, since the extents of 
	/// the geoemtry should remain the same.</para>
	/// </remarks>
	public class FlipYFilter : IGeometryComponentFilter
	{
		double _max = 0;
		double _min=0.0;

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the FlipYFilter class.
		/// </summary>
		/// <param name="max">The maximum value.</param>
		/// <param name="min">The minimum value.</param>
		public FlipYFilter(double max, double min)
		{
			_max = max;
			_min = min;
		}
		#endregion

		#region Properties
		#endregion

		#region Implementation of IGeometryFilter
		/// <summary>
		/// Applies the flip filter.
		/// </summary>
		/// <remarks>
		/// The following calculation is performed on the Y coordinate.
		/// <code>
		/// coord.Y = (_max - coord.Y) + _min;
		/// </code>
		/// </remarks>
		/// <param name="geometry">The geometry object to apply the filter to.</param>
		public void Filter(Geometry geometry)
		{
			/*if (geometry is Polygon)
			{
				Polygon polygon = (Polygon)geometry;
				Filter(polygon.Shell);
				foreach(LinearRing linearring in polygon.Holes)
				{
					Filter(linearring);
				}
			}
			else*/ if (geometry is LinearRing || geometry is LineString || geometry is Point)
			{
				Coordinates coords = geometry.GetCoordinates();
				for (int i=0; i < coords.Count; i++)
				{
					coords[i].Y = (_max - coords[i].Y) + _min;
				}
			}
			/*else if (geometry is GeometryCollection)
			{
				Filter(geometry);
			}
			else
			{
				throw new NotSupportedException(geometry.GetType().Name);
			}
			geometry.GeometryChanged();*/
		}
		#endregion

	}
}

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
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// Projects the current geometry. 
	/// </summary>
	/// <remarks>
	/// <para>Using Project, creates a new Geometry. This filter will projected the coordinates
	/// of an existing geometry without creating a new object.</para>
	/// <para>geometry.GeometryChanged() should be called once this method has completed since the extents of the geometry 
	/// have changed.</para>
	/// </remarks>
	public class ProjectionFilter : IGeometryComponentFilter
	{
		
		ICoordinateTransformation _coordinateTransform;

		#region Constructors
	

		/// <summary>
		/// Initializes a new instance of the FlipYFilter class.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformation to apply.</param>
		public ProjectionFilter(ICoordinateTransformation coordinateTransform)
		{
			_coordinateTransform = coordinateTransform;
		}
		#endregion

		#region Implementation of IGeometryComponentFilter
		/// <summary>
		/// Applies the projection to the coordinates.
		/// </summary>
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
			}*/
			if (geometry is LinearRing || geometry is LineString || geometry is Point)
			{	
				int sourceSRID = int.Parse(_coordinateTransform.SourceCS.AuthorityCode);
				int targetSRID = int.Parse(_coordinateTransform.TargetCS.AuthorityCode);
				MapProjection projection = (MapProjection)_coordinateTransform.MathTransform;

				Coordinates projectedCoordinates = new Coordinates();
				double x=0.0;
				double y=0.0;
				Coordinate coordinate;
				for(int i=0; i < geometry.GetCoordinates().Count; i++)
				{
					coordinate = geometry.GetCoordinates()[i];
					if (geometry.GetSRID() == sourceSRID)
					{
						projection.MetersToDegrees(coordinate.X, coordinate.Y, out x, out y);	
					}
					else if (geometry.GetSRID() == targetSRID)
					{
					
						projection.DegreesToMeters(coordinate.X, coordinate.Y, out x, out y);
					}
					coordinate.X = x;
					coordinate.Y = y;
				}
			}
			/*else
			{
				throw new NotSupportedException(geometry.GetType().Name);
			}*/
		}
		#endregion

	}
}

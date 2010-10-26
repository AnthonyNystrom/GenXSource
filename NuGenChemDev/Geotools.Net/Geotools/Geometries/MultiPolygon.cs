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
using Geotools.Operation;
using Geotools.CoordinateTransformations;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A collection of polygons.
	/// </summary>
	public class MultiPolygon : GeometryCollection, IMultiPolygon
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the MultiPolygon class.
		/// </summary>
		/// <param name="polygons">
		/// The Polygons for this MultiPolygon, or null or an empty array to create 
		/// the empty geometry.  Elements may be empty Polygons, but not nulls. The 
		/// polygons must conform to the assertions specified in the 
		/// OpenGIS Simple Features Specification.
		/// </param>
		/// <param name="precisionModel">The specification of the grid of allowable 
		/// points for this MultiPolygon.</param>
		/// <param name="SRID">
		///	The ID of the Spatial Reference System used by this MultiPolygon.
		/// </param>
		internal MultiPolygon(Polygon[] polygons, PrecisionModel precisionModel, int SRID) : base(polygons, precisionModel, SRID)
		{
			if(polygons == null)
			{
				polygons = new Polygon[]{};
			}
			if (HasNullElements(polygons))
			{
				throw new ArgumentNullException("Polygon array must not contain null elements.");
			}
			_geometries = polygons;
		}
		#endregion

		#region Properties
		#endregion

		#region Methods

		/// <summary>
		/// Returns the dimension of this Geometry.  Always 2 for a MultiPolygon.
		/// </summary>
		/// <returns>2 the dimension of a multipolygon is always 2.</returns>
		public override int GetDimension()
		{
			return 2;
		}

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.False if the boundary
		/// is the empty geometry.
		/// </returns>
		public override int GetBoundaryDimension()
		{
			return 1;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
			return "MultiPolygon";
		}

		/// <summary>
		/// Returns True.
		/// </summary>
		/// <returns>True.</returns>
		public override bool IsSimple()
		{
			return true;
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry  is empty.
		///</summary>
		///<remarks>For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."</remarks>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry</returns>
		public override Geometry GetBoundary()
		{
			if( IsEmpty() )
			{
				return _geometryFactory.CreateGeometryCollection(null);
			}
			//LineString[] allRings =  new LineString[_geometries.Length];
			ArrayList allRings = new ArrayList();
			for (int i = 0; i < _geometries.Length; i++) 
			{
				Polygon polygon = (Polygon) _geometries[i];
				GeometryCollection rings = (GeometryCollection) polygon.GetBoundary();
				for (int j = 0; j < rings.GetNumGeometries(); j++) 
				{
					//allRings[j] = rings[j-i] as LineString;
					allRings.Add( rings.GetGeometryN(j) );
				}
			}
			return _geometryFactory.CreateMultiLineString(GeometryFactory.ToLineStringArray(allRings));
		}

		/// <summary>
		/// Creates an exact copy of this MulitPolygon.
		/// </summary>
		/// <returns>A new MultiPolygon containing a copy of the MultiPolygon cloned.</returns>
		public override Geometry Clone()
		{
			return _geometryFactory.CreateMultiPolygon((Polygon[])_geometries.Clone());
		}
		
		/// <summary>
		/// Projects all the points in a multipolygon and returns a new multipolygon object.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformation to use for projection.</param>
		/// <returns>The projected multipolygon object.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			if (coordinateTransform==null)
			{
				throw new ArgumentNullException("coordinateTransform");
			}
			if (!(coordinateTransform.MathTransform is Geotools.CoordinateTransformations.MapProjection))
			{
				throw new ArgumentException("transform must be a MapProjection.");
			}

			Polygon[] projectedPolygons = new Polygon[_geometries.Length];
			IGeometry projectedPolygon;
			for(int i=0; i<_geometries.Length; i++)
			{
				projectedPolygon = _geometries[i].Project(coordinateTransform);
				projectedPolygons[i] = (Polygon)projectedPolygon;				
			}
			return _geometryFactory.CreateMultiPolygon(projectedPolygons);
		}

		#endregion

	}
}

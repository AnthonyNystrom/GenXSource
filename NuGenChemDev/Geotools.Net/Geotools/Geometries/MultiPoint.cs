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
	/// A collection of points.
	/// </summary>
	public class MultiPoint : GeometryCollection, IMultiPoint
	{
		#region Constructors
		/// <summary>
		/// Initializes a MultiPoint.
		/// </summary>
		/// <param name="points">
		/// The Points for this MultiPoint, or null or an empty array to create the empty geometry.  
		/// Elements may be empty Points, but not nulls.
		/// </param>
		/// <param name="precisionModel">
		/// The specification of the grid of allowable points for this MultiPoint.
		/// </param>
		/// <param name="SRID">
		/// The ID of the Spatial Reference System used by this.
		/// </param>
		internal MultiPoint(Point[] points, PrecisionModel precisionModel, int SRID) 
			: base(points, precisionModel, SRID)
		{
			if(points == null)
			{
				points = new Point[]{};
			}
			if (HasNullElements(points))
			{
				throw new ArgumentNullException("point array must not contain null elements");
			}
			_geometries = points;
		}

		#endregion

		#region Properties
		#endregion

		#region Methods
		/// <summary>
		/// Returns the dimension of this Geometry.  Always zero for a MultiPoint.
		/// </summary>
		/// <returns>0 the dimension of a multipoint is always 0.</returns>
		public override int GetDimension()
		{
			return 0;
		}

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.FALSE if the boundary
		/// is the empty geometry.
		/// </returns>
		public override int GetBoundaryDimension()
		{
			return Geotools.Geometries.Dimension.False;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
			return "MultiPoint";
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry  is empty. For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."  
		///</summary>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry</returns>
		public override Geometry GetBoundary()
		{
			return _geometryFactory.CreateGeometryCollection(null);
		}

		/// <summary>
		/// Returns true if no two points in the multipoint are equal.
		/// </summary>
		/// <returns>True if no two points in the multipoint are equal.</returns>
		public override bool IsSimple()
		{
			return (new IsSimpleOp()).IsSimple( this );
		}

		///<summary>
		///  Returns false if the Geometry is invlaid.  Subclasses provide their own definition of "valid". 
		///  If  this Geometry is empty, returns true.  
		///</summary>
		///<returns>Returns true if this Geometry is valid.</returns>
		public override bool IsValid()
		{
			return true;
		}

		/// <summary>
		/// Creates a copy of this MultiPoint
		/// </summary>
		/// <returns>A copy of this MulitPoint</returns>
		public override Geometry Clone()
		{
			return new MultiPoint( (Point[])_geometries.Clone(), _precisionModel, _SRID);
		}

		/// <summary>
		/// Gets the Coordinate at the requested index.
		/// </summary>
		/// <param name="index">The index of the Coordinate being requested.</param>
		/// <returns>A coordinate at the requested index.</returns>
		public Coordinate GetCoordinate(int index)
		{
			return ((Point)_geometries[index]).GetCoordinate();
		}
	
		/// <summary>
		/// Projects all the points in a multipoint and returns a new multipoint object.
		/// </summary>
		/// <param name="coordinateTransform">The coordinate transformation to use for projection.</param>
		/// <returns>The projected multi-point object.</returns>
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

			Point[] projectedPoints = new Point[_geometries.Length];
			IGeometry projectedPoint;
			for(int i=0; i<_geometries.Length; i++)
			{
				projectedPoint = _geometries[i].Project(coordinateTransform);
				projectedPoints[i] = (Point)projectedPoint;				
			}
			return _geometryFactory.CreateMultiPoint(projectedPoints);
		}
		#endregion
	}
}

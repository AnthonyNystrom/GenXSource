 /*  Copyright (C) 2002 Urban Science Applications, Inc. (translated from Java Topology Suite, 
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
using Geotools.CoordinateTransformations;
using Geotools.Geometries;
using Geotools.Utilities;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A Point comprised of a coordinate (x,y).
	/// </summary>
	public class Point : Geometry, IPoint
	{
		/// <summary>
		/// The Coordinate wrapped by this Point.
		/// </summary>
		protected Coordinate _coordinate;

		#region Constructors
		/// <summary>
		/// Initializes aPoint with the given coordinate.the coordinate on which to base this Point,
		///  or null to create the empty geometry.
		/// </summary>
		/// <param name="coordinate">An arraylist containing the coordinates of the point</param>
		/// <param name="precisionModel">The specification of the grid of allowable points for this Point</param>
		/// <param name="SRID">The ID of the Spatial Reference System used by this Point</param>
		internal Point(Coordinate coordinate, PrecisionModel precisionModel, int SRID) : base( precisionModel, SRID)
		{
			this._coordinate = coordinate;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Returns the x-value.
		/// </summary>
		public double X
		{
			get
			{
				return _coordinate.X;
			}
		}
		/// <summary>
		/// Returns the y-value.
		/// </summary>
		public double Y
		{
			get
			{
				return _coordinate.Y;
			}
		}
		#endregion

		#region Methods

		///<summary>
		///  Returns this Geometry's vertices.
		///</summary>
		///<remarks>Do not modify the array, as it may be the actual array stored
		///  by this Geometry.  The Geometries contained by composite Geometries must be Geometry's;
		///  that is, they must implement get Coordinates.</remarks>
		///<returns>Returns the vertices of this Geometry</returns>
		public override Coordinates GetCoordinates()
		{
			// point does not have a coordinates list so will need to create one and add the coordinate.
			Coordinates pointCoord = new Coordinates();
			if ( !IsEmpty() )
			{
				pointCoord.Add( _coordinate );
			}
			return pointCoord;
		}

		/// <summary>
		/// Gets the number of points in this point (0 or 1).
		/// </summary>
		/// <returns>0 or 1 depending on if this point is empty or not.</returns>
		public override int GetNumPoints() 
		{
			return IsEmpty() ? 0 : 1;
		}

		/// <summary>
		/// Returns true if the coordinate is null, false otherwise
		/// </summary>
		/// <returns>True if the coordinate is null.</returns>
		public override bool IsEmpty()
		{
			return _coordinate == null;
		}

		/// <summary>
		/// Returns true a point is simple
		/// </summary>
		/// <returns>True a point is always simple.</returns>
		public override bool IsSimple()
		{
			return true;
		}

		///<summary>
		///  Returns false if the Geometry is invlaid.  
		///</summary>
		///<returns>Returns true if this Geometry is valid.</returns>
		public override bool IsValid() 
		{
			return true;
		}

		/// <summary>
		/// Returns 0 a point has no dimensions
		/// </summary>
		/// <returns>0 a point has no dimension.</returns>
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
			// not unit tested.
			return Geotools.Geometries.Dimension.False;
		}

		/// <summary>
		/// Returns the x-value.
		/// </summary>
		/// <returns>Returns the x-coordinate.</returns>
		public double GetX()
		{
			if ( _coordinate == null ) 
			{
				throw new TopologyException("getX called on empty Point");
			}
			return _coordinate.X;
		}

		/// <summary>
		/// Returns the y-value.
		/// </summary>
		/// <returns>Returns the y-coordinate.</returns>
		public double GetY()
		{
			if ( _coordinate == null ) 
			{
				throw new TopologyException("getY called on empty Point");
			}
			return _coordinate.Y;
		}

		/// <summary>
		/// Returns the coordinate of the point.
		/// </summary>
		/// <returns>The coordinates of this point.</returns>
		public override Coordinate GetCoordinate()
		{
			return _coordinate;
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
			return "Point";
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
			// not unit tested;
			return _geometryFactory.CreateGeometryCollection(null);
		}

		///<summary>
		/// Returns the minimum and maximum x and y values in this Geometry, or a null Envelope if this Geometry
		/// is empty.
		///</summary>
		///<remarks>Unlike getEnvelopeInternal, this method calculates the Envelope each time it is called; 
		/// getEnvelopeInternal caches the result of this method.</remarks>
		///<returns>
		///Returns this Geometry's bounding box; if the Geometry is empty, Envelope.IsNull will 
		///return true
		///</returns>
		protected override Envelope ComputeEnvelopeInternal()
		{
			if ( IsEmpty() ) 
			{
				return new Envelope();
			}
			return new Envelope(_coordinate.X, _coordinate.X, _coordinate.Y, _coordinate.Y);
		}

		///<summary>
		/// Returns true if the two Geometrys have the same class and if the data which they store
		/// internally are equal.
		///</summary>
		///<remarks>This method is stricter equality than equals. If this and the other 
		/// Geometrys are composites and any children are not Geometrys, returns  false.</remarks>
		///<param name="obj">The Geometry with which to compare this Geometry.</param>
		///<returns>
		/// Returns true if this and the other Geometry are of the same class and have equal 
		/// internal data.
		///</returns>
		public override bool Equals(object obj)
		{
			Geometry geometry = obj as Geometry;
			if ( geometry != null )
			{
				if( !IsEquivalentClass(geometry) )
				{
					return false;
				}
				if( IsEmpty() && geometry.IsEmpty() )
				{
					return true;
				}
				Point otherPoint = geometry as Point;
				if(otherPoint != null)
				{
					return (otherPoint.GetCoordinate().Equals(_coordinate));
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the hash code for this object.
		/// </summary>
		/// <returns>The hash code for this linestring.</returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		///<summary>
		///  Performs an operation with or on this Geometry's coordinates.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry's coordinates</param>
		public override void Apply(ICoordinateFilter filter)
		{
			if ( IsEmpty() ) return;
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			filter.Filter( _coordinate );
		}

		///<summary>
		///  Performs an operation with or on this Geometry and it's children.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public override void Apply(IGeometryFilter filter)
		{
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			filter.Filter( this );
		}

		/// <summary>
		/// Performs an operation with or on this Geometry's components.
		/// </summary>
		/// <param name="filter">The filter to apply to this Geometry's components.</param>
		public override void Apply(IGeometryComponentFilter filter)
		{
			filter.Filter( this );
		}
	
		/// <summary>
		/// Returns a copy of this point
		/// </summary>
		/// <returns>A new geometry that is a copy of the original.</returns>
		public override Geometry Clone()
		{
			return _geometryFactory.CreatePoint((Coordinate)_coordinate.Clone());
		}

		/// <summary>
		/// Normalize is not valid for a point because there is no ordering.
		/// </summary>
		public override void Normalize()
		{ 
		}

		///<summary>
		/// Returns whether this Geometry is greater than, equal to, or less than another Geometry having 
		/// the same class.  
		///</summary>
		///<param name="other">A Geometry having the same class as this Geometry.</param>
		///<returns>Returns a positive number, 0, or a negative number, depending on whether this object is 
		/// greater than, equal to, or less than obj.</returns>
		public override int CompareToSameClass(object other)
		{
			Point point = (Point)other;
			return _coordinate.CompareTo(point._coordinate);
		}

		/// <summary>
		/// Returns a string representation of this object. Holes are excluded.
		/// </summary>
		/// <returns>A string containing the coordinates of the point in the form (x, y).</returns>
		public override string ToString()
		{
			return this.GetGeometryType() + ":" + _coordinate.ToString();
		}

		/// <summary>
		/// Projects a geometry using the given transformation. 
		/// </summary>
		/// <param name="coordinateTransform">The transformation to use.</param>
		/// <returns>A projected point object.</returns>
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

			int sourceSRID = int.Parse(coordinateTransform.SourceCS.AuthorityCode);
			int targetSRID = int.Parse(coordinateTransform.TargetCS.AuthorityCode);
			MapProjection projection = (MapProjection)coordinateTransform.MathTransform;
			int newSRID = GetNewSRID(coordinateTransform);

			double x=0.0;
			double y=0.0;
			Coordinate external = _geometryFactory.PrecisionModel.ToExternal( new Coordinate(this._coordinate) );
			if (this.GetSRID()==sourceSRID)
			{
				projection.MetersToDegrees( external.X, external.Y, out x, out y);
			}
			else if (this.GetSRID()==targetSRID)
			{
				projection.DegreesToMeters(external.X, external.Y, out x, out y);
			}
			
			Coordinate projectedCoordinate = _geometryFactory.PrecisionModel.ToInternal(new Coordinate(x,y) );
			return new Point( projectedCoordinate, this.PrecisionModel, newSRID);
		} // public override IGeometry Project(OGC.CoordinateTransformations.CT_Coordinat
		#endregion

	}
}

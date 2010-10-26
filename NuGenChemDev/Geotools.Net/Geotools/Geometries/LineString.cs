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
using System.Text;
using Geotools.CoordinateTransformations;
using Geotools.Operation;
using Geotools.Algorithms;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A linestring consists of a set of two or more connected points.
	/// </summary>
	public class LineString : Geometry, ILineString
	{
		/// <summary>
		///  The points of this LineString.
		/// </summary>
		protected Coordinates _points;

		#region Constructors

		/// <summary>
		/// Constructs a LineString with the given points.
		/// </summary>
		/// <param name="points">
		/// The points of the linestring, or null to create the empty geometry. This array must not contain null 
		/// elements. Consecutive points may not be equal.
		/// </param>
		/// <param name="precisionModel">The specification of the grid of allowable points for this LineString.</param>
		/// <param name="SRID"> The ID of the Spatial Reference System used by this LineString.</param>
		internal LineString(Coordinates points, PrecisionModel precisionModel, int SRID) : base( precisionModel, SRID)
		{
			if (points == null) 
			{
				points = new Coordinates();
			}
			if (HasNullElements(points))
			{
				throw new ArgumentNullException("point array must not contain null elements");
			}
			if (points.Count == 1)
			{
				throw new ArgumentException("point array must contain 0 or >1 elements");
			}
			this._points = points;
		}
		#endregion

		#region Properties		
		#endregion

		#region Methods

		/// <summary>
		/// Retrieves the first coordinate in the linestring.
		/// </summary>
		/// <returns>The first coordinate in the linestring.</returns>
		public override Coordinate GetCoordinate()
		{
			if ( IsEmpty() ) return null;
			return _points[0];
		}

		/// <summary>
		/// Gets the set of coordinates for this linestring.
		/// </summary>
		/// <returns>The coordinates of the linestring.</returns>
		public override Coordinates GetCoordinates()
		{
			return _points;
		}

		/// <summary>
		/// Gets the nth coordinate in this linestring.
		/// </summary>
		/// <param name="n">The index of the coordinate to retrieve.</param>
		/// <returns>The nth coordinate.</returns>
		public Coordinate GetCoordinateN(int n)
		{
			return _points[n];
		}

		/// <summary>
		/// Always returns a 1 because all linestrings have a dimension of 1.
		/// </summary>
		/// <returns>1 because all linestrings have a dimension of 1.</returns>
		public override int GetDimension()
		{
			return 1;
		}

		///<summary>
		///  Returns the dimension of this Geometrys inherent boundary.  
		///</summary>
		///<returns>
		/// Returns the dimension of the boundary of the class implementing this interface, 
		/// whether or not this object is the empty geometry. Returns Dimension.FALSE if the boundary
		/// is the empty geometry.  Returns zero always because the boundary is a multipoint and the dimension 
		/// of a multipoint is always 0.
		/// </returns>
		public override int GetBoundaryDimension()
		{
			if ( IsClosed() ) 
			{
				return Geotools.Geometries.Dimension.False;
			}
			return 0;
		}

		/// <summary>
		/// Determines if this linestring is empty.
		/// </summary>
		/// <returns>True if the linestring is empty.</returns>
		public override bool IsEmpty()
		{
			if( _points != null )
			{
				return _points.Count == 0;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Gets the number of points in this linestring.
		/// </summary>
		/// <returns>An integer caontaining the number of points in the linestring.</returns>
		public override int GetNumPoints()
		{
			if( _points != null )
			{
				return _points.Count;
			}
			return 0;
		}

		/// <summary>
		/// Gets the nth point in the linestring.
		/// </summary>
		/// <param name="n">The index of the point to be retrieved.</param>
		/// <returns>The point at position n.</returns>
		public Point GetPointN(int n) 
		{
			return _geometryFactory.CreatePoint( _points[n] );
		}

		/// <summary>
		/// Gets the starting point of the linestring.
		/// </summary>
		/// <returns>The first point in the linestring.</returns>
		public Point GetStartPoint()
		{
			if ( IsEmpty() )
			{
				return null;
			}
			else
			{
				return GetPointN(0);
			}
		}

		/// <summary>
		/// Get the last point in the linestring.
		/// </summary>
		/// <returns>The last point in the linestring.</returns>
		public Point GetEndPoint()
		{
			if ( IsEmpty() )
			{
				return null;
			}
			else
			{
				return GetPointN( GetNumPoints()-1 );
			}
		}

		/// <summary>
		/// Determines if this linestring is closed.
		/// </summary>
		/// <returns>True if the linestring is closed.</returns>
		public virtual bool IsClosed()
		{
				if( IsEmpty() )
				{
					return false;
				}
				else
				{
					return GetCoordinateN(0).Equals2D( GetCoordinateN( GetNumPoints()-1) );
				}
		}

		/// <summary>
		/// Determines if this linestring is closed and simple therefore a ring.
		/// </summary>
		/// <returns>True or false if this linestring is a ring.</returns>
		public bool IsRing()
		{
			return IsClosed() && IsSimple();
		}

		///<summary>
		///  Returns the type of this geometry.  
		///</summary>
		///<returns>Returns the type of this geometry.</returns>
		public override string GetGeometryType()
		{
				return "LineString";
		}

		/// <summary>
		/// Returns the length of this LineString.
		/// </summary>
		/// <returns>Returns the length of this LineString.</returns>
		public override double GetLength()
		{
			return CGAlgorithms.Length( _points );
		}


		/// <summary>
		/// Determines if this linestring is simple (does not intersect itself).
		/// </summary>
		/// <returns>true if the linestring is simple.</returns>
		public override bool IsSimple()
		{
			return (new IsSimpleOp()).IsSimple(this);
		}

		///<summary>
		///  Returns the boundary, or the empty geometry if this Geometry is empty.
		///</summary>
		///<remarks>For a discussion
		///  of this function, see the OpenGIS Simple Features Specification. As stated in SFS 
		///  Section 2.1.13.1, "the boundary  of a Geometry is a set of Geometries of the next lower dimension."</remarks>
		///<returns>Returns the closure of the combinatorial boundary of this Geometry.</returns>
		public override Geometry GetBoundary()
		{
			if ( IsEmpty() ) 
			{
				return _geometryFactory.CreateGeometryCollection(null);
			}

			if( this.IsClosed() )
			{
				//if the LineString is closed return an empty multipoint
				return _geometryFactory.CreateMultiPoint( new Coordinates() );
			}
			Coordinates coords = new Coordinates();
			Coordinate coord = new Coordinate(this.GetStartPoint().X, this.GetStartPoint().Y);
			coords.Add(coord);
			coord = new Coordinate(this.GetEndPoint().X, this.GetEndPoint().Y);
			coords.Add(coord);
			return _geometryFactory.CreateMultiPoint( coords );
		}

		/// <summary>
		/// Determines if pt is a member of the line string.
		/// </summary>
		/// <param name="pt">The point to be used for comparision.</param>
		/// <returns>True if the point is in the linestring.</returns>
		public bool IsCoordinate( Coordinate pt)
		{
			foreach(Coordinate coord in _points)
			{
				if(coord.Equals(pt))
				{
					return true;
				}
			}
			return false;
		}

		///<summary>
		/// Returns the minimum and maximum x and y values in this Geometry, or a null Envelope if this Geometry
		/// is empty.
		///</summary>
		///<remarks>Unlike Geometry.GetEnvelopeInternal, this method calculates the Envelope each time it is called; 
		/// getEnvelopeInternal caches the result of this method.</remarks>
		///<returns>
		///	Returns this Geometrys bounding box; if the Geometry is empty, Envelope.IsNull will return true.
		///</returns>
		protected override Envelope ComputeEnvelopeInternal()
		{
			if ( IsEmpty() ) 
			{
				return new Envelope();
			}
			double minx = _points[0].X;
			double miny = _points[0].Y;
			double maxx = _points[0].X;
			double maxy = _points[0].Y;
			for (int i = 1; i < _points.Count; i++) 
			{
				minx = Math.Min(minx, _points[i].X);
				maxx = Math.Max(maxx, _points[i].X);
				miny = Math.Min(miny, _points[i].Y);
				maxy = Math.Max(maxy, _points[i].Y);
			}
			return new Envelope(minx, maxx, miny, maxy);
		}

		///<summary>
		/// Returns true if the two Geometrys have the same class and if the data which they store
		/// internally are equal.
		///</summary>
		///<remarks>This method is stricter equality than equals. If this and the other 
		/// Geometrys are composites and any children are not Geometrys, returns false.</remarks>
		///<param name="obj">The Geometry with which to compare this Geometry.</param>
		///<returns>
		/// Returns true if this and the other Geometry are of the same class and have equal 
		/// internal data.
		///</returns>
		public override bool Equals( object obj)
		{
			Geometry geometry = obj as Geometry;
			if ( geometry != null )
			{
				if ( !IsEquivalentClass( geometry ) )
				{
					return false;
				}
				LineString otherLS = geometry as LineString;
				if ( otherLS != null )
				{
					if ( _points.Count != otherLS.GetCoordinates().Count )
					{
						return false;
					}
					for ( int i = 0; i < _points.Count; i++ )
					{
						if ( !_points[i].Equals( otherLS.GetCoordinateN(i)  ) )
						{
							return false;
						}
					}
					return true;
				}
			} // if ( geometry != null )
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
		///<param name="filter">The filter to apply to this Geometry's coordinates.</param>
		public override void Apply(ICoordinateFilter filter)
		{
			if (filter==null)
			{
				throw new ArgumentNullException("filter");
			}
			foreach(Coordinate coord in _points)
			{
				filter.Filter( coord );
			}
		}

		///<summary>
		///  Performs an operation with or on this Geometry and it's children.  
		///</summary>
		///<param name="filter">The filter to apply to this Geometry (and it's children, if it is a 
		///GeometryCollection).</param>
		public override void Apply(IGeometryFilter filter)
		{
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
		/// Creates a copy of this LineString.
		/// </summary>
		/// <returns>A copy of this LineString</returns>
		public override Geometry Clone()
		{
			return _geometryFactory.CreateLineString(_points.Clone());
		}

		///<summary>
		/// Converts this Geometry to normal form (or canonical form).
		///</summary>
		///<remarks>Normal form is a unique representation
		/// for Geometry's. It can be used to test whether two Geometrys are equal in a way that is 
		/// independent of the ordering of the coordinates within them. Normal form equality is a stronger 
		/// condition than topological equality, but weaker than pointwise equality. The definitions for 
		/// normal form use the standard lexicographical ordering for coordinates. Sorted in order of 
		/// coordinates means the obvious extension of this ordering to sequences of coordinates.
		///</remarks>
		public override void Normalize() 
		{
			for ( int i = 0; i < _points.Count; i++ ) 
			{
				int j = _points.Count - 1 - i;
				if ( !_points[i].Equals( _points[j] ) ) 
				{
					if ( _points[i].CompareTo( _points[j] ) > 0 ) 
					{
						ReversePointOrder( _points );
					}
					return;
				}
			}
		}
			
		///<summary>
		/// Returns whether this Geometry is greater than, equal to, or less than another Geometry having 
		/// the same class.  
		///</summary>
		///<param name="obj">A Geometry having the same class as this Geometry.</param>
		///<returns>Returns a positive number, 0, or a negative number, depending on whether this object is 
		/// greater than, equal to, or less than obj.</returns>
		public override int CompareToSameClass(object obj)
		{
			Geometry other = obj as Geometry;   
			return Compare( this.GetCoordinates(), other.GetCoordinates() );
		}

		/// <summary>
		/// Returns a string representation of this object.
		/// </summary>
		/// <returns>A string containing the coordinates of the linestring seperated by :.</returns>
		public override string ToString()
		{
			return this.GetGeometryType() + ":" + _points.ToString();
		}

		/// <summary>
		/// Projects a geometry using the given transformation. 
		/// </summary>
		/// <param name="coordinateTransform">The transformation to use.</param>
		/// <returns>A projected line string object.</returns>
		public override Geometry Project(ICoordinateTransformation  coordinateTransform)
		{
			if (coordinateTransform==null)
			{
				throw new ArgumentNullException("coordinateTransform");
			}
			if (!(coordinateTransform.MathTransform is Geotools.CoordinateTransformations.MapProjection))
			{
				throw new ArgumentException("coordinateTransform must be a MapProjection.");
			}

			int sourceSRID = int.Parse(coordinateTransform.SourceCS.AuthorityCode);
			int targetSRID = int.Parse(coordinateTransform.TargetCS.AuthorityCode);
			MapProjection projection = (MapProjection)coordinateTransform.MathTransform;
			int newSRID = GetNewSRID(coordinateTransform);

			Coordinates projectedCoordinates = new Coordinates();
			double x=0.0;
			double y=0.0;
			Coordinate projectedCoordinate;
			Coordinate external;
			Coordinate coordinate;
			for(int i=0; i < _points.Count; i++)
			{
				coordinate = _points[i];
				external = _geometryFactory.PrecisionModel.ToExternal( coordinate );
				if (this._SRID==sourceSRID)
				{
					projection.MetersToDegrees(external.X, external.Y, out x, out y);	
				}
				else if (this._SRID==targetSRID)
				{
					
					projection.DegreesToMeters(external.X, external.Y, out x, out y);
				}
				projectedCoordinate = _geometryFactory.PrecisionModel.ToInternal(new Coordinate( x, y) );
				projectedCoordinates.Add( projectedCoordinate );
			}
			return new LineString( projectedCoordinates, this.PrecisionModel, newSRID);
		}



		#endregion
	}
}

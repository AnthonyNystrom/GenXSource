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
using Geotools.Algorithms;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A LineSegment represents a two-point line..
	/// </summary>
	internal class LineSegment : IComparable
	{
		protected Coordinate _p0;
		protected Coordinate _p1;

		#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		internal LineSegment( Coordinate p0, Coordinate p1 ) 
		{
			if ( p0 == null )
			{
				_p0 = new Coordinate();
			}
			else
			{
				_p0 = p0;
			}

			if ( p1 == null )
			{
				_p1 = new Coordinate();
			}
			else
			{
				_p1 = p1;
			}
		} // public LineSegment( Coordinate p0, Coordinate p1 ) 

		/// <summary>
		/// Constructor.
		/// </summary>
		internal LineSegment() : this( new Coordinate(), new Coordinate() )
		{
		}
		
		internal LineSegment( LineSegment ls ) : this( ls.P0, ls.P1 )
		{
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets/Sets Coordinate P0.
		/// </summary>
		public Coordinate P0
		{
			get
			{
				return _p0;
			}
			set
			{
				_p0 = value;
			}
		}

		/// <summary>
		/// Gets/Sets Coordinate P1.
		/// </summary>
		public Coordinate P1
		{
			get
			{
				return _p1;
			}
			set
			{
				_p1 = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the coordinates for this line segment based on the incoming LineSegment.
		/// </summary>
		/// <param name="ls"></param>
		public void SetCoordinates( LineSegment ls )
		{
			SetCoordinates( ls.P0, ls.P1 );
		}

		/// <summary>
		/// Sets the coordinates for this line segment.
		/// </summary>
		/// <param name="p0">First Coordinate in the line segment.</param>
		/// <param name="p1">Second Coordinate in the line segment.</param>
		public void SetCoordinates( Coordinate p0, Coordinate p1 )
		{
			if ( p0 == null )
			{
				throw new ArgumentNullException( "p0", "Coordinate is null");
			}
			if ( p1 == null )
			{
				throw new ArgumentNullException( "p1", "Coordinate is null");
			}
			_p0.X = p0.X;
			_p0.Y = p0.Y;
			_p1.X = p1.X;
			_p1.Y = p1.Y;
		} // public void SetCoordinates( Coordinate p0, Coordinate p1 )

		public Coordinate GetCoordinate( int i )
		{
			if ( i == 0 ) return _p0;
			return _p1;
		}

		/// <summary>
		/// Computes the length of the line segment. 
		/// </summary>
		/// <returns>Returns the length of the line segment.</returns>
		public double GetLength()
		{
			return _p0.Distance( _p1 );
		}

		/// <summary>
		/// Reverses the direction of the line segment.
		/// </summary>
		public void Reverse()
		{
			Coordinate temp = _p0;
			_p0 = _p1;
			_p1 = temp;
		}

		/// <summary>
		/// Puts the line segment into a normalized form.  This is useful for using line segments in maps and indexes
		/// when topological equality rather that exact equality is desired.
		/// </summary>
		public void Normalize()
		{
			if ( _p1.CompareTo( _p0 ) < 0 ) Reverse();
		}

		/// <summary>
		/// Returns the angle this segment makes with the x-axis (in radians).
		/// </summary>
		/// <returns>Returns the angle this segment makes with the x-axis (in radians).</returns>
		public double Angle()
		{
			return Math.Atan2( _p1.Y - _p0.Y, _p1.X - _p0.X );
		}

		/// <summary>
		/// Computes the distance between this line segment and another one.
		/// </summary>
		/// <param name="ls">Other LineSegment with which to calculate the distance.</param>
		/// <returns></returns>
		public double Distance(LineSegment ls)
		{
			return CGAlgorithms.DistanceLineLine( _p0, _p1, ls.P0, ls.P1 );
		}

		/// <summary>
		/// Computes the distance between this line segment and a point.
		/// </summary>
		/// <param name="p">The point from which to calculate the distance from this line segment.</param>
		/// <returns>Returns the distance between this line segment and a point.</returns>
		public double Distance( Coordinate p )
		{
			return CGAlgorithms.DistancePointLine( p, _p0, _p1 );
		}

		/// <summary>
		/// Compute the projection factor for the projection of the point p onto this LineSegment.
		/// The projection factor is the constant k by which the vector for this segment must be
		/// multiplied to equal the vector for the projection of p.
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public double ProjectionFactor(Coordinate p)
		{
			if (p.Equals( _p0 ) ) return 0.0;
			if (p.Equals( _p1 ) ) return 1.0;
			// Otherwise, use comp.graphics.algorithms Frequently Asked Questions method
			/*     	      AC dot AB
						   r = ---------
								 ||AB||^2
						r has the following meaning:
						r=0 P = A
						r=1 P = B
						r<0 P is on the backward extension of AB
						r>1 P is on the forward extension of AB
						0<r<1 P is interior to AB
				*/
			double dx = _p1.X - _p0.X;
			double dy = _p1.Y - _p0.Y;
			double len2 = dx * dx + dy * dy;
			double r = ( (p.X - _p0.X) * dx + (p.Y - _p0.Y) * dy ) / len2;
			return r;
		}

		/// <summary>
		/// Compute the projection of a point onto the line determined by this line segment.
		/// </summary>
		/// <remarks>Note that the projected point my lie outside the line segment.  If this is the case,
		/// the projection factor will lie outside the range [0.0, 1.0].</remarks>
		/// <param name="p"></param>
		/// <returns>Returns the computed projection of a point onto the line determined by this line segment.</returns>
		public Coordinate Project(Coordinate p)
		{
			if ( p.Equals( _p0 ) || p.Equals( _p1 ) ) return new Coordinate( p );

			double r = ProjectionFactor( p );
			Coordinate coord = new Coordinate();
			coord.X = _p0.X + r * ( _p1.X - _p0.X );
			coord.Y = _p0.Y + r * ( _p1.Y - _p0.Y );
			return coord;
		}

		/// <summary>
		/// Project a line segment onto this line segment and return the resulting
		/// line segment.  The returned line segment will be a subset of
		/// the target line line segment.  This subset may be null, if
		/// the segments are oriented in such a way that there is no projection.
		/// </summary>
		/// <remarks>Note that the returned line may have zero length (i.e. the same endpoints).
		/// This can happen for instance if the lines are perpendicular to one another.</remarks>
		/// <param name="seg">The line segment to project.</param>
		/// <returns>Returns the projected line segment, or null if there is no overlap.</returns>
		public LineSegment Project( LineSegment seg )
		{
			double pf0 = ProjectionFactor( seg.P0 );
			double pf1 = ProjectionFactor( seg.P1 );
			// check if segment projects at all
			if (pf0 >= 1.0 && pf1 >= 1.0) return null;
			if (pf0 <= 0.0 && pf1 <= 0.0) return null;

			Coordinate newp0 = Project( seg.P0 );
			if (pf0 < 0.0) newp0 = _p0;
			if (pf0 > 1.0) newp0 = _p1;

			Coordinate newp1 = Project( seg.P1 );
			if (pf1 < 0.0) newp1 = _p0;
			if (pf1 > 1.0) newp1 = _p1;

			return new LineSegment( newp0, newp1 );
		}

		/// <summary>
		/// Computes the closest point on this line segment to another point.
		/// </summary>
		/// <param name="p">The point to which find the closest point on the line segment.</param>
		/// <returns>Returns a coordinate which is the closest point on the line segment to the point p.</returns>
		public Coordinate ClosestPoint(Coordinate p)
		{
			double factor = ProjectionFactor(p);
			if (factor > 0 && factor < 1) 
			{
				return Project(p);
			}
			double dist0 = _p0.Distance(p);
			double dist1 = _p1.Distance(p);
			if (dist0 < dist1)
				return _p0;
			return _p1;
		}

		/// <summary>
		/// Returns true if other has the same values for its points.
		/// </summary>
		/// <param name="o">The other LineSegment with which to do the comparison.</param>
		/// <returns>Returns true if other is a LineSegment with the same values for the x and y ordinates.</returns>
		public override bool Equals( object o ) 
		{
			LineSegment other = o as LineSegment;
			if ( other == null ) 
			{
				 return false;
			}
			return _p0.Equals( other.P0 ) && _p1.Equals( other.P1 );
		}

		/// <summary>
		/// Required when the Equals method is implemented.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Compares this object with the specified object for order.  Uses the standard lexicographic ordering for
		/// the points in the LineSegment.
		/// </summary>
		/// <param name="o">The LineSegment with which this LineSegment is being compared.</param>
		/// <returns>Returns a negative integer, zero, or a positive integer as this LineSegment is less than,
		/// equal to, or greater that the specified LineSegment.</returns>
		public int CompareTo( Object o ) 
		{
			LineSegment other = (LineSegment) o;
			int comp0 = _p0.CompareTo( other.P0 );
			if (comp0 != 0) return comp0;
			return _p1.CompareTo( other.P1 );
		}

		/// <summary>
		/// Returns true if other is topologically equals to this LineSegment (e.g. irrespective of orientation).
		/// </summary>
		/// <param name="other">The other LineSegment with which to do the comparison.</param>
		/// <returns>Returns true if other is a LineSegment with the same values for the x and y ordinates.</returns>
		public bool EqualsTopology( LineSegment other )
		{
			return
				_p0.Equals( other.P0 ) && _p1.Equals( other.P1 )
				|| _p0.Equals( other.P1 ) && _p1.Equals( other.P0 );
		}

		public override string ToString()
		{
			return "LINESTRING( " +
				_p0.X + " " + _p0.Y
				+ ", " +
				_p1.X + " " + _p1.Y + ")";
		}

		#endregion

	}
}

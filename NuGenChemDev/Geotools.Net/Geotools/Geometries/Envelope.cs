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

#region Using statements
using System;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// An Envelope defines a rectangulare region of the 2D coordinate plane.
	/// </summary>
	/// <remarks>
	/// It is often used to represent the bounding box of a Geometry,
	/// e.g. the minimum and maximum x and y values of the Coordinates.
	/// Note that Envelopes support infinite or half-infinite regions, by using the values of
	/// Double.PositiveInfinity and Double.NegativeInfinity.
	/// When Envelope objects are created or initialized,
	/// the supplied extent values are automatically sorted into the correct order.
	/// </para>
	public class Envelope
	{
		/// <summary>
		/// The minimum x-coordinate
		/// </summary>
		private double _minX;
		/// <summary>
		/// The maximum x-coordinate
		/// </summary>
		private double _maxX;
		/// <summary>
		/// The minimum y-coordinate
		/// </summary>
		private double _minY;
		/// <summary>
		/// The maximum y-coordinate
		/// </summary>
		private double _maxY;

		#region Constructors
		/// <summary>
		/// Creates a null Envelope.
		/// </summary>
		public Envelope()
		{
			Initialize();
		}
		/// <summary>
		/// Creates an <code>Envelope</code> for a region defined by maximum and minimum values.
		/// </summary>
		/// <param name="x1">The first x-value</param>
		/// <param name="x2">The second x-value</param>
		/// <param name="y1">The first y-value</param>
		/// <param name="y2">The second y-value</param>
		public Envelope(double x1, double x2, double y1, double y2)
		{
			Initialize(x1, x2, y1, y2);
		}
		/// <summary>
		/// Creates an Envelope for a region defined by two Coordinates.
		/// </summary>
		/// <param name="p1">The first Coordinate.</param>
		/// <param name="p2">The second Coordinate.</param>
		internal Envelope(Coordinate p1, Coordinate p2)
		{
			Initialize(p1, p2);
		}

		/// <summary>
		/// Creates an Envelope for a region defined by a single coordinate.
		/// </summary>
		/// <param name="p">The coordinate for which to create the envelope.</param>
		public Envelope(Coordinate p)
		{
			Initialize(p);
		}

		/// <summary>
		/// Creates an Envelope from an existing Envelope.
		/// </summary>
		/// <param name="env">The Envelope to initialize from.</param>
		public Envelope(Envelope env)
		{
			Initialize(env);
		}

		#endregion

		#region Properties
		/// <summary>
		/// Determines the difference between the maximum and minimum x values.
		/// </summary>
		/// <returns>Max x - min x, or 0 if this is a null Envelope</returns>
		public double Width 
		{
			get
			{
				if (IsNull()) 
				{
					return 0;
				}
				return _maxX - _minX;
			}
		}

		/// <summary>
		/// Determines the difference between the maximum and minimum y values.
		/// </summary>
		/// <returns>Max y - min y, or 0 if this is a null Envelope</returns>
		public double Height 
		{
			get
			{
				if (IsNull()) 
				{
					return 0;
				}
				return _maxY - _minY;
			}
		}

		/// <summary>
		/// Returns the Envelope's minimum x-value. min x > max x indicates that this is a 
		/// null Envelope.
		/// </summary>
		/// <returns>The minimum x-coordinate</returns>
		public double MinX 
		{
			get
			{
				return _minX;
			}
		}

		/// <summary>
		/// Returns the Envelope's maximum x-value. min x > max x
		/// indicates that this is a null Envelope.
		/// </summary>
		/// <returns>The maximum x-coordinate</returns>
		public double MaxX 
		{
			get
			{
				return _maxX;
			}
		}

		/// <summary>
		/// Returns the Envelope's minimum y-value. min y > max y
		/// indicates that this is a null Envelope.
		/// </summary>
		/// <returns>The minimum y-coordinate</returns>
		public double MinY
		{
			get
			{
				return _minY;
			}
		}

		/// <summary>
		/// Returns the Envelopes maximum y-value. min y > max y
		/// indicates that this is a null Envelope.
		/// </summary>
		/// <returns>The maximum y-coordinate</returns>
		public double MaxY 
		{
			get
			{
				return _maxY;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initialize to a null Envelope.
		/// </summary>
		public void Initialize()
		{
			SetToNull();
		}
		/// <summary>
		/// Initialize an Envelope for a region defined by maximum and minimum values.
		/// </summary>
		/// <param name="x1">The first x-value</param>
		/// <param name="x2">The second x-value</param>
		/// <param name="y1">The first y-value</param>
		/// <param name="y2">The second y-value</param>
		public void Initialize(double x1, double x2, double y1, double y2)
		{
			if (x1 < x2) 
			{
				_minX = x1;
				_maxX = x2;
			}
			else 
			{
				_minX = x2;
				_maxX = x1;
			}
			if (y1 < y2) 
			{
				_minY = y1;
				_maxY = y2;
			}
			else 
			{
				_minY = y2;
				_maxY = y1;
			}
		}
		/// <summary>
		/// Initialize an Envelope to a region defined by two Coordinates.
		/// </summary>
		/// <param name="p1">The first Coordinate</param>
		/// <param name="p2">The second Coordinate</param>
		public void Initialize(Coordinate p1, Coordinate p2)
		{
			Initialize(p1.X, p2.X, p1.Y, p2.Y);
		}
		/// <summary>
		/// Initialize an Envelope to a region defined by a single Coordinate.
		/// </summary>
		/// <param name="p">The Coordinates</param>
		public void Initialize(Coordinate p)
		{
			Initialize(p.X, p.X, p.Y, p.Y);
		}
		/// <summary>
		/// Initialize an Envelope from an existing Envelope.
		/// </summary>
		/// <param name="env">The Envelope to initialize from</param>
		public void Initialize(Envelope env)
		{
			Initialize(env._minX, env._maxX, env._minY, env._maxY);
		}

		/// <summary>
		/// Makes this envelope a "null" envelope, that is, the envelope of the empty
		/// geometry.
		/// </summary>
		public void SetToNull() 
		{
			_minX =  0;
			_maxX = -1;
			_minY =  0;
			_maxY = -1;
		}
		/// <summary>
		/// Determines if this Envelope is a "null" envelope.
		/// </summary>
		/// <returns>
		/// True if this Envelope is uninitialized or is the envelope of the empty geometry.
		/// </returns>
		public bool IsNull() 
		{
			return _maxX < _minX;
		}

		/// <summary>
		/// Enlarges the boundary of the <code>Envelope</code> so that it contains
		/// (x,y).
		/// </summary>
		/// <remarks>Does nothing if (x,y) is already on or within the boundaries.</remarks>
		/// <param name="p">The Coordinates</param>
		public void ExpandToInclude(Coordinate p)
		{
			ExpandToInclude(p.X, p.Y);
		}

		/// <summary>
		/// Enlarges the boundary of the <code>Envelope</code> so that it contains
		/// (x,y).
		/// </summary>
		/// <remarks>Does nothing if (x,y) is already on or within the boundaries.</remarks>
		/// <param name="x">The value to lower the minimum x to or to raise the maximum x to</param>
		/// <param name="y">The value to lower the minimum y to or to raise the maximum y to</param>
		public void ExpandToInclude(double x, double y) 
		{
			if (IsNull()) 
			{
				_minX = x;
				_maxX = x;
				_minY = y;
				_maxY = y;
			}
			else 
			{
				if (x < _minX) 
				{
					_minX = x;
				}
				if (x > _maxX) 
				{
					_maxX = x;
				}
				if (y < _minY) 
				{
					_minY = y;
				}
				if (y > _maxY) 
				{
					_maxY = y;
				}
			}
		}

		/// <summary>
		/// Enlarges the boundary of the Envelope so that it contains other.
		/// </summary>
		/// <remarks>Does nothing if other is wholly on or within the boundaries.</remarks>
		/// <param name="other">The Envelope to merge with</param>
		public void ExpandToInclude(Envelope other) 
		{
			if (other.IsNull()) 
			{
				return;
			}
			if (IsNull()) 
			{
				_minX = other.MinX;
				_maxX = other.MaxX;
				_minY = other.MinY;
				_maxY = other.MaxY;
			}
			else 
			{
				if (other._minX < _minX) 
				{
					_minX = other._minX;
				}
				if (other._maxX > _maxX) 
				{
					_maxX = other._maxX;
				}
				if (other._minY < _minY) 
				{
					_minY = other._minY;
				}
				if (other._maxY > _maxY) 
				{
					_maxY = other._maxY;
				}
			}
		}

		/// <summary>
		/// Returns true if the given point lies in or on the envelope.
		/// </summary>
		/// <param name="p">The point which this Envelope is being checked for containing</param>
		/// <returns>
		/// True if the point lies in the interior or on the boundary of this Envelope.</returns>
		public bool Contains(Coordinate p) 
		{
			return Contains(p.X, p.Y);
		}

		/// <summary>
		/// Returns true if the given point lies in or on the envelope.
		/// </summary>
		/// <param name="x">The x-coordinate of the point which this <code>Envelope</code> is being checked for containing</param>
		/// <param name="y">The y-coordinate of the point which this <code>Envelope</code> is being checked for containing</param>
		/// <returns>True if (x, y) lies in the interior or on the boundary of this Envelope.</returns>
		public bool Contains(double x, double y) 
		{
			return x >= _minX &&
				x <= _maxX &&
				y >= _minY &&
				y <= _maxY;
		}

		/// <summary>Check if the region defined by other overlaps (intersects) the region of this Envelope.</summary>
		/// <param name="other">The Envelope which this Envelope is being checked for overlapping</param>
		/// <returns>True if the Envelope's overlap</returns>
		public bool Intersects(Envelope other) 
		{
			return !(other.MinX > _maxX ||
				other.MaxX < _minX ||
				other.MinY > _maxY ||
				other.MaxY < _minY);
		}

		/// <summary>
		/// Check if the Envelope other overlaps (lies inside) the region of this Envelope.
		/// </summary>
		/// <param name="other">The Envelope to be tested</param>
		/// <returns>True if the Envelope other overlaps this Envelope</returns>
		public bool Overlaps(Envelope other)
		{
			return ( Intersects( other ) );
		}

		/// <summary>
		/// Check if the point p overlaps (lies inside) the region of this Envelope.
		/// </summary>
		/// <param name="p">The Coordinate to be tested</param>
		/// <returns>True if the point overlaps this Envelope</returns>
		public bool Intersects( Coordinate p )
		{
			return ( Intersects( p.X, p.Y ) );
		}

		/// <summary>
		/// Check if the point p overlaps (lies inside) the region of this Envelope.
		/// </summary>
		/// <param name="p">The Coordinate to be tested</param>
		/// <returns>True if the point overlaps this Envelope</returns>
		public bool Overlaps(Coordinate p) 
		{
			return Intersects( p );
		}

		/// <summary>
		/// Check if the point (x, y) overlaps (lies inside) the region of this Envelope.
		/// </summary>
		/// <param name="x">The x-ordinate of the point</param>
		/// <param name="y">The y-ordinate of the point</param>
		/// <returns>True if the point overlaps this Envelope</returns>
		public bool Intersects(double x, double y) 
		{
			return ! (x > _maxX ||
				x < _minX ||
				y > _maxY ||
				y < _minY);
		}

		/// <summary>
		/// Check if the point (x, y) overlaps (lies inside) the region of this Envelope.
		/// </summary>
		/// <param name="x">The x coordinate of the point to be tested.</param>
		/// <param name="y">The y coordinate of the point to be tested.</param>
		/// <returns>True if the point overlaps this Envelope</returns>
		public bool Overlaps(double x, double y)
		{
			return Intersects( x, y );
		}

		/// <summary>
		/// Returns true if the Envelope other lies wholely inside this Envelope (inclusive of the boundary).
		/// </summary>
		/// <param name="other">The Envelope which this Envelope is being checked for containing</param>
		/// <returns>True if other is contained in this Envelope</returns>
		public bool Contains(Envelope other) 
		{
			return other.MinX >= _minX &&
				other.MaxX <= _maxX &&
				other.MinY >= _minY &&
				other.MaxY <= _maxY;
		}

		/// <summary>
		/// Computes the distance between this and another Envelope.
		/// </summary>
		/// <remarks>
		/// The distance between overlapping Envelopes is 0.  Otherwise, the 
		/// distance is the Euclidean distance between the closest points.
		/// </remarks>
		/// <param name="env">The other envelope to be used for the comparison.</param>
		/// <returns>A double containing the distance between the two envelopes.</returns>
		public double Distance(Envelope env)
		{
			if ( Intersects(env) ) return 0;
			if ( _maxX < env.MinX) 
			{
				// this is left of env
				if (MaxY < env.MinY) 
				{
					// this is below left of env
					return Distance(_maxX, MaxY, env.MinX, env.MinY);
				}
				else if (_minY > env.MaxY) 
				{
					// this is above left of env
					return Distance(_maxX, _minY, env.MinX, env.MaxY);
				}
				else 
				{
					// this is directly left of env
					return env.MinX - _maxX;
				}
			}
			else 
			{
				// this is right of env
				if (_maxY < env.MinY) 
				{
					// this is below right of env
					return Distance(_minX, _maxY, env.MaxX, env.MinY);
				}
				else if (_minY > env.MaxY) 
				{
					// this is above right of env
					return Distance(_minX, _minY, env.MaxX, env.MaxY);
				}
				else 
				{
					// this is directly right of env
					return _minX - env.MaxX;
				}
			}
		}

		/// <summary>
		/// Determines if the two objects are of the same type and if they contain the elements.
		/// </summary>
		/// <param name="other">The other object to be compared.</param>
		/// <returns>A bool containing either true of false based on the equality of the two objects.</returns>
		public override bool Equals(Object other) 
		{
			bool returnValue = false;
			Envelope env = other as Envelope;
			if ( env != null )
			{
				if ( IsNull() )
				{
					returnValue = env.IsNull();
				}
				else
				{
					returnValue = _maxX == env.MaxX &&
						_maxY == env.MaxY &&
						_minX == env.MinX &&
						_minY == env.MinY;
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Returns a unique integer for this object.
		/// </summary>
		/// <remarks>Used with hash tables.</remarks>
		/// <returns>An integer containing the hash code.</returns>
		public override int GetHashCode()
		{
			return  ( ( (_maxX.GetHashCode() ^ _minX.GetHashCode()) ^ _maxY.GetHashCode() ) ^ _minY.GetHashCode() );
		}

		/// <summary>
		/// Returns a string representation of this Envelope.
		/// </summary>
		/// <remarks>The format is: Env[minX : maxX, minY : maxY]</remarks>
		/// <returns>A string containing the set of coordinates.</returns>
		public override String ToString()
		{
			return "Env[" + _minX + " : " + _maxX + ", " + _minY + " : " + _maxY + "]";
		}

		#endregion

		#region Static Methods
		private static double Distance( double x0, double y0, double x1, double y1 )
		{
			double dx = x1-x0;
			double dy = y1-y0;
			return Math.Sqrt( dx*dx + dy*dy );
		}
		#endregion
	}
}

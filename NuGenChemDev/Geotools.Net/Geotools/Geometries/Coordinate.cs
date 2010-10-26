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

#region Using Statements
using System;
#endregion


namespace Geotools.Geometries
{
	/// <summary>
	/// Coordinate is the lightweight class used to store coordinates.  
	/// </summary>
	/// <remarks>
	/// <para>It is distinct from Point, which is a Geometry class.  Unlike objects of type 
	/// Point (which contain additional information such as envelope,a precision model, and 
	/// spatial reference system information), a Coordinate only contains ordinate values and 
	/// accessor methods.</para>
	/// 
	/// <para>Coordinates are two-dimensional points, with an additional z-ordinate.  JTS, from which this code has been
	/// converted, does not support any operations on the z-ordinate except the basic accessor functions.  
	/// Constructed coordinates will have a z-ordinate of NaN.  The standard comparison functions will ignore
	/// the z-ordinate.</para>
	/// </remarks>
	public class Coordinate : IComparable, ICloneable
	{
		private double _x=0.0;
		private double _y=0.0;
		private double _z=Double.NaN;

		#region Constructors
		/// <summary>
		/// Public constructor which Initializes a coodinate at (0,0).
		/// </summary>
		/// <remarks>Initializes a coodinate at (0,0)</remarks>
		public Coordinate()
		{
		}

		/// <summary>
		/// Public constructor which Initializes a coordinate at (x, y).
		/// </summary>
		/// <param name="x">X-coordinate value.</param>
		/// <param name="y">Y-coordinate value.</param>
		/// <param name="z">Z-coordinate value.</param>
		/// <remarks>Initializes a coordinate at (x, y)</remarks>
		public Coordinate( double x, double y, double z )
		{
			_x = x;
			_y = y;
			_z = z;
		}

		/// <summary>
		/// Public constructor which Initializes a coordinate having the same values as Coordinate other.
		/// </summary>
		/// <param name="other">Coordinate from which to construct new object.</param>
		/// <remarks>Initializes a coordinate having the same values as Coordinate other</remarks>
		public Coordinate( Coordinate other )
		{
			_x = other.X;
			_y = other.Y;
			_z = other.Z;
		}

		/// <summary>
		/// public constructor which Initializes a Coordinate at (x,y,NaN).
		/// </summary>
		/// <param name="x">The x - coordinate value.</param>
		/// <param name="y">The y - coordinate value.</param>
		/// <remarks>Initializes a Coordinate at (x,y,NaN)</remarks>
		public Coordinate( double x, double y )
		{
			_x = x;
			_y = y;
			_z = Double.NaN;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The x-coordinate.
		/// </summary>
		public double X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}
		/// <summary>
		/// The y-coordinate.
		/// </summary>
		public double Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
			}
		}
		/// <summary>
		/// The z-coorindate.
		/// </summary>
		public double Z
		{
			get
			{
				return _z;
			}
			set
			{
				_z = value;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Creates a copy of this object.
		/// </summary>
		/// <returns>Returns an exact copy of this object.</returns>
		/// <remarks>Creates a new object that is an exact replica of the first.</remarks>
		public object Clone()
		{
			return new Coordinate( _x, _y, _z );
		}

		/// <summary>
		/// Copies the values of other into this object.
		/// </summary>
		/// <param name="other">Coordinate from which to copy values.</param>
		public void CopyCoordinate( Coordinate other )
		{
			_x = other.X;
			_y = other.Y;
			_z = other.Z;
		}

		/// <summary>
		///  Determines if the planar projections of the two Coordinates are equal.
		/// </summary>
		/// <param name="other">A Coordinate with which to do the 2D comparison.</param>
		/// <returns>
		/// True if the x- and y-coordinates are equal; the z-coordinates do not have to be equal.
		/// </returns>
		public bool Equals2D(Coordinate other) 
		{
			bool returnValue = true;
			if ( _x != other.X || _y != other.Y ) 
			{
				returnValue =  false;
			}
			return returnValue;
		} // public bool Equals2D(Coordinate other)

		/// <summary>
		/// Compares this object to the parameter object.
		/// </summary>
		/// <remarks>OtherCoordinate should also be of type coordinate in 
		/// order to obtain an accurate result.</remarks>
		/// <param name="otherCoordinate">Coordinate used for comparison.</param>
		/// <returns>Returns true is otherCoordinate's x and y values are equal to this object's.  Returns
		/// false otherwise.</returns>
		public override bool Equals( object otherCoordinate )
		{
			bool returnValue = false;
			Coordinate c = otherCoordinate as Coordinate;
			if ( c != null )
			{
				if  ( ( _x == c.X ) && ( _y == c.Y ) &&
					( ( _z == c.Z ) || ( Double.IsNaN(_z) && Double.IsNaN(c.Z) ) ) )
				{
					returnValue = true;
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
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.  Since Coordinates are 2.5D,
		/// this routine ignores the z value when making the comparison.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///		<list type="table">
		///		<listheader><term>Value</term><description>Meaning</description></listheader>
		///			<item><term>Less than zero</term><description><I>a</I> This instance is less than obj. <I>b</I>.</description></item>
		///			<item><term>Zero</term><description><I>a</I> This instance is equal to obj. <I>b</I>.</description></item>
		///			<item><term>Greater than zero</term><description><I>a</I> This instance is greater than obj. <I>b</I>.</description></item>
		///		</list>
		///	</returns>
		/// <remarks>
		/// If obj is not of type Coordinate, then an ArgumentException will be thrown.
		/// </remarks>
		public int CompareTo(object obj)
		{
			int returnValue = 0;
			Coordinate other = obj as Coordinate;
			if ( other != null )
			{
				if (_x < other.X) 
				{
					returnValue = -1;
				}
				else if (_x > other.X) 
				{
					returnValue = 1;
				}
				else if (_y < other.Y) 
				{
					returnValue = -1;
				}
				else if (_y > other.Y) 
				{
					returnValue = 1;
				}
				else
				{
					returnValue = 0;
				}
			}
			else
			{
				throw new ArgumentException("Argument obj is not of type Coordinate", "obj" );
			}
			return returnValue;
		}

		/// <summary>
		/// Determines if two coordinates have the same values for x, y, and z.
		/// </summary>
		/// <param name="obj">Object to compare with the instance of this object.</param>
		/// <returns>Returns true if obj is a Coordinate with the same values for x, y,and z.</returns>
		/// <remarks>If the object to compare to is not a coordinate false is returned.</remarks>
		public bool Equals3D(object obj) 
		{
			bool returnValue = false;
			Coordinate c = obj as Coordinate;
			if ( c != null )
			{
				if  ( ( _x == c.X ) && ( _y == c.Y ) &&
					( ( _z == c.Z ) || ( Double.IsNaN(_z) && Double.IsNaN(c.Z) ) ) )
				{
					returnValue = true;
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Converts a coordinate to a string. of the form (x,y,z).
		/// </summary>
		/// <returns>Returns a string of the form (x, y, z)</returns>
		/// <remarks>The string is in the form (x, y, z)</remarks>
		public override string ToString() 
		{
			return "(" + _x + ", " + _y + ", " + _z + ")";
		}

		/// <summary>
		/// Changes Coordinate to use Fixed Precision Model.
		/// </summary>
		public void MakePrecise()
		{
			_x = PrecisionModel.MakePrecise( _x );
			_y = PrecisionModel.MakePrecise( _y );
		}

		/// <summary>
		/// Calculates the distance between this point and the supplied otherCoordinate.
		/// </summary>
		/// <param name="otherCoordinate">The coordinate to calculate distance from this point.</param>
		/// <returns>A double containing the distance between the two coordinates.</returns>
		public double Distance( Coordinate otherCoordinate)
		{
			double dx = _x - otherCoordinate.X;
			double dy = _y - otherCoordinate.Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}

		#endregion

	} // public class Coordinate : IComparable, ICloneable
}

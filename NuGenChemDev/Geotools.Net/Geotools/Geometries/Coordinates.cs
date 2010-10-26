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
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// Coordinates class is a typed collection class for the Coordinate class.
	/// </summary>
	public class Coordinates : ArrayList
	{
		/// <summary>
		/// Creates a new, empty instance of Coordinates.
		/// </summary>
		public Coordinates():base()
		{
		}

		/// <summary>
		/// Creates a new instance of Coordinates adding each coordinate in coords to this instance.
		/// </summary>
		/// <param name="coords">The set of coordinates to be used to create this set.</param>
		public Coordinates(Coordinates coords) : base()
		{
			foreach(Coordinate coord in coords)
			{
				Add(coord, true);
			}
		}

		/// <summary>
		/// Adds a new coordinate to this set of coordinates.
		/// </summary>
		/// <param name="coord">The coordinate to be added to the set.</param>
		/// <param name="allowRepeated">
		/// A flag used to determine if repeat coordinates will be added to the set.
		/// </param>
		/// <returns>If the add is successful, an integer containing the index of the newly added coordinate.
		///   If repeats are not allowed and the coordinate to be added is a repeat a -1 is returned.</returns>
		public int Add(object coord, bool allowRepeated)
		{
			if (!allowRepeated)
			{
				if (this.Count>=1)
				{
					Coordinate last = this[this.Count-1];
					if (last.Equals(coord)) 
					{
						return -1;
					}
				}
			}
			return base.Add(coord);
		}
		/// <summary>
		/// Represents the Coordinate object entry at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the entry to locate in the collection.</param>
		public new Coordinate this [int index]
		{
			get
			{
				return (Coordinate)base[index];
			}
			set
			{
				base[index]=value;
			}
		}
		
		/// <summary>
		/// Creates an exact replica of this set of coordinates.
		/// </summary>
		/// <returns>A new set of coordinates containing the same coordinates as the original.</returns>
		public new Coordinates Clone()
		{
			Coordinates coords = new Coordinates();
			foreach(Coordinate coord in this)
			{
				coords.Add(coord);
			}
			return coords;
		}
	
		/// <summary>
		/// Returns a string representation of the Coordinates object.
		/// </summary>
		/// <remarks>The format is: (x1, y1, NaN),(x2, y2, NaN)...</remarks>
		/// <returns>A string containing the set of coordinates.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach( object obj in this )
			{
				Coordinate coord = obj as Coordinate;
				sb.Append( coord.ToString() + "," );
			}
			//sb.Remove( sb.Length -1, 1 );		// remove the last comma
			return sb.ToString().TrimEnd(new char[]{','});
		}

		/// <summary>
		/// Reverses the order of the coordinates in this set of coordinates.
		/// </summary>
		/// <returns>The set of coordinates with the corder of the coordinates reversed.</returns>
		public Coordinates ReverseCoordinateOrder()
		{
			Coordinates coordinates = new Coordinates();
			for(int i = 0; i < base.Count; i++)
			{
				coordinates.Add(base[base.Count-1-i]);
			}
			return coordinates;
		}

		/// <summary>
		/// Determines if this set of coordinates contains repeating points.
		/// </summary>
		/// <param name="coord">THe set of coorinates to be examined.</param>
		/// <returns>A bool based on the presence of repeating points.</returns>
		public static bool HasRepeatedPoints(Coordinates coord)
		{
			for (int i = 1; i < coord.Count; i++) 
			{
				if (coord[i - 1].Equals( coord[i] ) ) 
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// If the coordinate array has repeated points, constructs new array
		/// containing no repeated points.
		/// </summary>
		/// <param name="coord">The set of coordinates to be examined.</param>
		/// <returns>A new set of coordinates with no repeating points.</returns>
		public static Coordinates RemoveRepeatedPoints(Coordinates coord)
		{
			if ( !HasRepeatedPoints(coord) ) 
			{
				return coord;
			}
			Coordinates newCoords = new Coordinates();
			newCoords.Add( coord[0] );		// add the first coordinate
			for ( int i = 1; i < coord.Count; i++ )
			{
				if ( !coord[i-1].Equals( coord[i] ) )		// if this coord does not equal the last coords, add otherwise skip.
				{
					newCoords.Add( coord[i],false );
				}
			}
			return newCoords;
		}

		/// <summary>
		/// Determines if the two objects are of the same type and if they contain the elements.
		/// </summary>
		/// <param name="obj">The other object to be compared.</param>
		/// <returns>A bool containing either true of false based on the equality of the two objects.</returns>
		public override bool Equals ( object obj )
		{
			Coordinates other = obj as Coordinates;
			if ( this == other ) return true;			// if they are the same reference objects, then equals is true for sure.
			if ( other == null ) return false;			// not same type or null object then must be false.
			if ( this.Count != other.Count ) return false;		// must be of same length
			for ( int i = 0; i < this.Count; i++ )
			{
				if ( !this[i].Equals( other[i] ) ) return false;
			}
			return true;
		}

		/// <summary>
		/// Returns a unique integer for this object.
		/// </summary>
		/// <remarks>Used with hash tables.</remarks>
		/// <returns>An integer containing the hash code.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}

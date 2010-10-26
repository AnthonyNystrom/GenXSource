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
using System.Collections;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// Dimension describes the dimension for a geometry.  Has a variety of representations for each
	/// dimension type.
	/// </summary>
	internal class Dimension
	{
		/// <summary>
		/// Dimension value of a point (0).
		/// </summary>
		public static int P
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Dimension value of a curve (1).
		/// </summary>
		public static int L
		{
			get
			{
				return 1;
			}
		}
		
		/// <summary>
		/// Dimension value of a surface (2).
		/// </summary>
		public static int A 
		{
			get
			{
				return 2;
			}
		}

		/// <summary>
		/// Dimension value of the empty geometry (-1).
		/// </summary>
		public static int False
		{
			get
			{
				return -1;
			}
		}

		/// <summary>
		/// Dimension value of non-empty geometries (= {P, L, A}).
		/// </summary>
		public static int True
		{
			get
			{
				return -2;
			}
		}

		/// <summary>
		/// Dimension value for any dimension (= {False, TRUE}).
		/// </summary>
		public static int DontCare
		{
			get
			{
				return -3;
			}
		}

		/// <summary>
		/// Converts the dimension value to a dimension symbol.
		/// </summary>
		/// <param name="dimensionValue">A number that can be stored in the IntersectionMatrix.
		/// Possible values are {TRUE, False, DONTCARE, 0, 1, 2}</param>
		/// <returns>Returns the string symbol representation of the dimension.
		/// Possible values are {T, F, * , 0, 1, 2}</returns>
		public static char ToDimensionSymbol(int dimensionValue) 
		{
			char returnValue;
			switch (dimensionValue) 
			{
				case -1:
					returnValue = 'F';
					break;
				case -2:
					returnValue = 'T';
					break;
				case -3:
					returnValue = '*';
					break;
				case 0:
					returnValue = '0';
					break;
				case 1:
					returnValue = '1';
					break;
				case 2:
					returnValue = '2';
					break;
				default:
					throw new ArgumentException("Invalid dimension value", "dimensionValue");
			}
			return returnValue;

		} // public static string ToDimensionSymbol(int dimensionValue)

		/// <summary>
		/// Converts the dimension symbol to a dimension value, for example '*' => -3.
		/// </summary>
		/// <param name="dimensionSymbol">A character for use in the string representation of an
		/// IntersectionMatrix.  Possible values are {T, F, *, 0, 1, 2}</param>
		/// <returns>A number that can be stored in the IntersectionMatrix.  Possible values are {-2, -1, -3, 0, 1, 2}.</returns>
		public static int ToDimensionValue( char dimensionSymbol ) 
		{
			int dimensionValue;
			switch ( char.ToUpper( dimensionSymbol ) ) 
			{
				case 'F':
					dimensionValue = False;
					break;
				case 'T':
					dimensionValue = True;
					break;
				case '*':
					dimensionValue = DontCare;
					break;
				case '0':
					dimensionValue = P;
					break;
				case '1':
					dimensionValue = L;
					break;
				case '2':
					dimensionValue = A;
					break;
				default:
					throw new ArgumentException("Invalid dimension symbol", "dimensionSymbol");
			} // switch

			return dimensionValue;
		} // public static int ToDimensionValue( string dimensionSymbol )


		/// <summary>
		/// Converts symbol to string representation.
		/// </summary>
		/// <param name="dimensionSymbol">A character for use in the string representation of an
		/// IntersectionMatrix.  Possible values are {T, F, *, 0, 1, 2}</param>
		/// <returns>Returns the appropriate string {"TRUE", "False", "DONTCARE", "P", "L", "A"}</returns>
		public static string ToDimensionString( char dimensionSymbol )
		{
			string dimensionString = "";
			switch ( char.ToUpper( dimensionSymbol ) ) 
			{
				case 'F':
					dimensionString = "False";
					break;
				case 'T':
					dimensionString = "True";
					break;
				case '*':
					dimensionString = "DontCare";
					break;
				case '0':
					dimensionString = "P";
					break;
				case '1':
					dimensionString = "L";
					break;
				case '2':
					dimensionString = "A";
					break;
				default:
					throw new ArgumentException("Invalid dimension symbol", "dimensionSymbol");
			} // switch

			return dimensionString;
		} // public static string ToDimensionString( string dimensionSymbol )

		/// <summary>
		/// Converts the integer representation to a string representation.
		/// </summary>
		/// <param name="dimensionValue">The dimension value. Possible values are {-2, -1, -3, 0, 1, 2}</param>
		/// <returns>Returns the appropriate string {"TRUE", "False", "DONTCARE", "P", "L", "A"}</returns>
		public static string ToDimensionString( int dimensionValue )
		{
			string dimensionString = "";
			switch (dimensionValue) 
			{
				case -1:
					dimensionString = "False";
					break;
				case -2:
					dimensionString = "True";
					break;
				case -3:
					dimensionString = "DontCare";
					break;
				case 0:
					dimensionString = "P";
					break;
				case 1:
					dimensionString = "L";
					break;
				case 2:
					dimensionString = "A";
					break;
				default:
					throw new ArgumentException("Invalid dimension value", "dimensionValue");
			}
			return dimensionString;

		} // public static string ToDimensionString( int dimensionValue )

	} // internal class Dimension

} // namespace Geotools.Geometry

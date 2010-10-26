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
using System.Text;
using Geotools.Geometries;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	/// A Dimensionally Extended Nine-Intersection Model (DE-9IM) matrix. 
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class can used to represent both computed DE-9IM's (like 212FF1FF2) as well as
	/// patterns for matching them (like T*T******).</para>
	/// Methods are provided to:
	/// <list type="bullet">
	///		<listheader><term>Items</term></listheader>
	///		<item><term>Set and query the elements of the matrix in a convenient fashion</term></item>
	///		<item><term>Convert to and from the standard string representation (specified in SFS Section 2.1.13.2).</term></item>
	///		<item><term>Test to see if a matrix matches a given pattern string.</term></item>
	///	</list>	
	///	For a description of the DE-9IM, see the <A HREF="http://www.opengis.org/techno/specs.htm">OpenGIS Simple Features Specification for SQL.</A>
	///  </remarks>
	public class IntersectionMatrix : ICloneable
	{
		private int[,] _matrix;


		#region Constructors
		/// <summary>
		/// Constructs an IntersectionMatrix with FALSE dimension values.
		/// </summary>
		internal IntersectionMatrix()
		{
			_matrix = new int[3,3];
			SetAll( Dimension.False );
		}

		/// <summary>
		/// Constructor an IntersectionMatrix with the given dimension symbols.
		/// </summary>
		/// <param name="elements">String of demension values.</param>
		internal IntersectionMatrix( string elements ) 
		{
			_matrix = new int[3,3];
			Set( elements );
		}

		/// <summary>
		/// Constructs an IntersectionMatrix with the same elements as other.
		/// </summary>
		/// <param name="other">The IntersectionMatrix to copy.</param>
		internal IntersectionMatrix( IntersectionMatrix other ) : this()
		{
			_matrix[Location.Interior, Location.Interior] = other.Matrix[Location.Interior, Location.Interior];
			_matrix[Location.Interior, Location.Boundary] = other.Matrix[Location.Interior, Location.Boundary];
			_matrix[Location.Interior, Location.Exterior] = other.Matrix[Location.Interior, Location.Exterior];
			_matrix[Location.Boundary, Location.Interior] = other.Matrix[Location.Boundary, Location.Interior];
			_matrix[Location.Boundary ,Location.Boundary] = other.Matrix[Location.Boundary, Location.Boundary];
			_matrix[Location.Boundary, Location.Exterior] = other.Matrix[Location.Boundary, Location.Exterior];
			_matrix[Location.Exterior, Location.Interior] = other.Matrix[Location.Exterior, Location.Interior];
			_matrix[Location.Exterior, Location.Boundary] = other.Matrix[Location.Exterior, Location.Boundary];
			_matrix[Location.Exterior, Location.Exterior] = other.Matrix[Location.Exterior, Location.Exterior];
		}

		#endregion

		#region Properties
		
		/// <summary>
		/// The matrix for this object.
		/// </summary>
		public int[,] Matrix
		{
			get
			{
				return _matrix;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns an exact copy of this object.
		/// </summary>
		/// <returns>An exact copy of this object.</returns>
		public object Clone()
		{
			return new IntersectionMatrix( this );
		} // public object Clone()

		/// <summary>
		/// Changes the elements of this IntersectionMatrix to the dimension symbols in dimensionSymbols.
		/// </summary>
		/// <param name="dimensionSymbols">Nine dimension symbols to which to set this IntersectionMatrix.</param>
		public void Set(string dimensionSymbols) 
		{
			if ( dimensionSymbols.Length != 9 ) 
			{
				throw new ArgumentException("Should be length 9: " + dimensionSymbols);
			}

			for (int i = 0; i < dimensionSymbols.Length; i++) 
			{
				int row = i / 3;
				int col = i % 3;
				_matrix[row,col] = Dimension.ToDimensionValue( dimensionSymbols[i] );
			}
		} // public void Set(string dimensionSymbols)

		/// <summary>
		/// Changes the value of one of this IntersectionMatrix elements.
		/// </summary>
		/// <param name="row">The row of this IntersectionMatrix indicating the interior, boundary
		/// or exterior of the first Geometry.</param>
		/// <param name="column">The column of this IntersectionMatrix indicating the interior,
		/// boundary or exterior of the second Geometry.</param>
		/// <param name="dimensionValue">The new value of the element.</param>
		public void Set(int row, int column, int dimensionValue) 
		{
			_matrix[row,column] = dimensionValue;
		} // public void Set(int row, int column, int dimensionValue)


		/// <summary>
		/// Changes the specified element to minimumDimensionValue if the element is less.
		/// </summary>
		/// <param name="row">The row of this IntersectionMatrix indicating the interior, boundary
		/// or exterior of the first Geometry.</param>
		/// <param name="column">The column of this IntersectionMatrix indicating the interior,
		/// boundary or exterior of the second Geometry.</param>
		/// <param name="minimumDimensionValue">The dimension value with which to compare the element.
		/// The order of dimension values from least to greatest is (DONTCARE, TRUE, FALSE, 0, 1, 2).</param>
		public void SetAtLeast(int row, int column, int minimumDimensionValue) 
		{
			if ( _matrix[row,column] < minimumDimensionValue ) 
			{
				_matrix[row,column] = minimumDimensionValue;
			}
		} // public void SetAtLeast(int row, int column, int minimumDimensionValue)

		/// <summary>
		/// If row >= 0 and column >= 0, changes the specified element to minimumDimensionValue if the element is
		/// less.  Does nothing if row &lt; 0 or column &lt; 0.
		/// </summary>
		/// <param name="row">The row of this IntersectionMatrix indicating the interior, boundary
		/// or exterior of the first Geometry.</param>
		/// <param name="column">The column of this IntersectionMatrix indicating the interior,
		/// boundary or exterior of the second Geometry.</param>
		/// <param name="minimumDimensionValue">The dimension value with which to compare the element.
		/// The order of dimension values from least to greatest is (DONTCARE, TRUE, FALSE, 0, 1, 2).</param>
		public void SetAtLeastIfValid(int row, int column, int minimumDimensionValue) 
		{
			if ( row >= 0 && column >= 0 ) 
			{
				SetAtLeast(row, column, minimumDimensionValue);
			}
		} // public void SetAtLeastIfValid(int row, int column, int minimumDimensionValue)

		/// <summary>
		/// For each element in this IntersectionMatrix, changes the element to the corresponding
		/// minimum dimension symbol if the element is less.
		/// </summary>
		/// <param name="minimumDimensionSymbols">The nine dimension symbols with which to compare the
		/// elements of this IntersectionMatrix.  The order of dimension values from least to greatest is
		/// (DONTCARE, TRUE, FALSE, 0, 1, 2).</param>
		public void SetAtLeast(String minimumDimensionSymbols) 
		{

			if ( minimumDimensionSymbols.Length != 9 ) 
			{
				throw new ArgumentException("Should be length 9: " + minimumDimensionSymbols);
			}
			
			for (int i = 0; i < minimumDimensionSymbols.Length; i++) 
			{
				int row = i / 3;
				int col = i % 3;
				SetAtLeast(row, col, Dimension.ToDimensionValue( minimumDimensionSymbols[i] ) );
			}
		}	// public void SetAtLeast(String minimumDimensionSymbols)

		/// <summary>
		/// Changes the elements of this IntersecionMatrix to dimensionValue.
		/// </summary>
		/// <param name="dimensionValue">The dimension value to which to set this IntersectionMatrixs elements.
		/// Possible values are:(TRUE, FALSE, DONTCARE, 0,1,2).</param>
		public void SetAll(int dimensionValue) 
		{
			for (int ai = 0; ai < 3; ai++) 
			{
				for (int bi = 0; bi < 3; bi++) 
				{
					_matrix[ai,bi] = dimensionValue;
				}
			}
		} // public void SetAll(int dimensionValue)

		/// <summary>
		/// Returns the value of one of this IntersectionMatrixs elements.
		/// </summary>
		/// <param name="row">The row of this IntersectionMatrix, indicating the interior,
		/// boundary, or exterior of the first Geometry.</param>
		/// <param name="column">The column of this IntersectionMatrix indicating the interior,
		/// boundary or exterior of the second Geometry.</param>
		/// <returns>Returns the dimension value at the given matrix position.</returns>
		public int Get(int row, int column) 
		{
			return _matrix[row,column];
		} // public int Get(int row, int column)

		/// <summary>
		/// Returns true if this IntersectionMatrix is FF*FF****.
		/// </summary>
		/// <returns>Returns true if the two geometries related by this IntersectionMatrix are disjoint.</returns>
		public bool IsDisjoint() 
		{
			return
				_matrix[Location.Interior,Location.Interior] == Dimension.False &&
				_matrix[Location.Interior,Location.Boundary] == Dimension.False &&
				_matrix[Location.Boundary,Location.Interior] == Dimension.False &&
				_matrix[Location.Boundary,Location.Boundary] == Dimension.False;
		} // public boolean IsDisjoint()

		/// <summary>
		/// Returns true if IsDisjoint returns false.
		/// </summary>
		/// <returns>Returns true if the two Geometries related by this IntersectionMatrix intersect.</returns>
		public bool IsIntersects() 
		{
			return !IsDisjoint();
		} // public bool IsIntersects()

		/// <summary>
		/// Returns true if this IntersectionMatrix is FT*******, F**T*****, or F***T****.
		/// </summary>
		/// <param name="dimensionOfGeometryA">The dimension of the first Geometry.</param>
		/// <param name="dimensionOfGeometryB">The dimension of the second Geometry.</param>
		/// <returns>Returns true if the two Geometries related by this IntersectionMatrix touch.  Returns
		/// false if both Geometries are points.</returns>
		public bool IsTouches(int dimensionOfGeometryA, int dimensionOfGeometryB) 
		{
			if (dimensionOfGeometryA > dimensionOfGeometryB) 
			{
				//no need to get transpose because pattern matrix is symmetrical
				return IsTouches(dimensionOfGeometryB, dimensionOfGeometryA);
			}
			if (( dimensionOfGeometryA == Dimension.A && dimensionOfGeometryB == Dimension.A) ||
				( dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.L) ||
				( dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.A) ||
				( dimensionOfGeometryA == Dimension.P && dimensionOfGeometryB == Dimension.A) ||
				( dimensionOfGeometryA == Dimension.P && dimensionOfGeometryB == Dimension.L)) 
			{
				return _matrix[Location.Interior,Location.Interior] == Dimension.False &&
					(  Matches(_matrix[Location.Interior,Location.Boundary], 'T')
					|| Matches(_matrix[Location.Boundary,Location.Interior], 'T')
					|| Matches(_matrix[Location.Boundary,Location.Boundary], 'T'));
			}
			return false;
		} // public bool IsTouches(int dimensionOfGeometryA, int dimensionOfGeometryB)

		/// <summary>
		/// Returns true if this IntersectionMatrix is:
		/// &lt;ul&gt;
		///		&lt;li&gt; T*T****** (for a point and a curve, a point and an area or a line and an area)
		///		&lt;li&gt; 0******** (for two curves)
		///	&lt;/ul&gt;
		/// </summary>
		/// <param name="dimensionOfGeometryA">The dimension of the first Geometry.</param>
		/// <param name="dimensionOfGeometryB">The dimension of the second Geometry.</param>
		/// <returns>Returns true if the two Geometries related by this IntersectionMatrix cross.
		/// For this function to return true, the Geometries must be a point and a curve; a point and a
		/// surface; two curves; or a curve and a surface.</returns>
		public bool IsCrosses(int dimensionOfGeometryA, int dimensionOfGeometryB) 
		{
			if ( (dimensionOfGeometryA == Dimension.P && dimensionOfGeometryB == Dimension.L) ||
				 (dimensionOfGeometryA == Dimension.P && dimensionOfGeometryB == Dimension.A) ||
				 (dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.A) ) 
			{
				return Matches( _matrix[Location.Interior,Location.Interior], 'T' ) &&
					   Matches( _matrix[Location.Interior,Location.Exterior], 'T' );
			}
			if ( (dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.P) ||
				 (dimensionOfGeometryA == Dimension.A && dimensionOfGeometryB == Dimension.P) ||
				 (dimensionOfGeometryA == Dimension.A && dimensionOfGeometryB == Dimension.L) ) 
			{
				return Matches( _matrix[Location.Interior,Location.Interior], 'T' ) &&
					   Matches( _matrix[Location.Exterior,Location.Interior], 'T' );
			}
			if (dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.L) 
			{
				return _matrix[Location.Interior,Location.Interior] == 0;
			}
			return false;
		} // public bool IsCrosses(int dimensionOfGeometryA, int dimensionOfGeometryB)

		/// <summary>
		/// Returns true if this IntersectionMatrix is T*F**F***.
		/// </summary>
		/// <returns>Returns true if the first Geometry is within the second.</returns>
		public bool IsWithin() 
		{
			return Matches( _matrix[Location.Interior,Location.Interior], 'T')		&&
				   _matrix[Location.Interior,Location.Exterior] == Dimension.False	&&
				   _matrix[Location.Boundary,Location.Exterior] == Dimension.False;
		} // public bool IsWithin()

		/// <summary>
		/// Returns true if this IntersectionMatrix is T*****FF*.
		/// </summary>
		/// <returns>Returns true if the first Geometry contains the second.</returns>
		public bool IsContains() 
		{
			return Matches( _matrix[Location.Interior,Location.Interior], 'T' )		&&
				   _matrix[Location.Exterior,Location.Interior] == Dimension.False	&&
				   _matrix[Location.Exterior,Location.Boundary] == Dimension.False;
		} // public bool IsContains()

		/// <summary>
		/// Returns true if this IntersectionMatrix is T*F**FFF*.
		/// </summary>
		/// <param name="dimensionOfGeometryA">The dimension of the first Geometry.</param>
		/// <param name="dimensionOfGeometryB">The dimension of the second Geometry.</param>
		/// <returns>Returns true if the two Geometries related by this IntersectionMatrix are equal;
		/// the Geometries must have the same dimension for this function to return true.</returns>
		public bool IsEquals(int dimensionOfGeometryA, int dimensionOfGeometryB) 
		{
			if (dimensionOfGeometryA != dimensionOfGeometryB) 
			{
				return false;
			}
			return Matches( _matrix[Location.Interior,Location.Interior], 'T' ) &&
				_matrix[Location.Exterior,Location.Interior] == Dimension.False &&
				_matrix[Location.Interior,Location.Exterior] == Dimension.False &&
				_matrix[Location.Exterior,Location.Boundary] == Dimension.False &&
				_matrix[Location.Boundary,Location.Exterior] == Dimension.False;
		} // public bool IsEquals(int dimensionOfGeometryA, int dimensionOfGeometryB)

		/// <summary>
		/// Returns true if this IntersectionMatrix is:
		/// &lt;ul&gt;
		///		&lt;li&gt;	T*T***T**	(for two points or two surfaces)
		///		&lt;li&gt;	1*T***T**	(for two curves)
		///	&lt;/ul&gt;
		/// </summary>
		/// <param name="dimensionOfGeometryA">The dimension of the first Geometry.</param>
		/// <param name="dimensionOfGeometryB">The dimension of the second Geometry.</param>
		/// <returns>Returns true if the two Geometries related by this IntersectionMatrix overlap.  For this
		/// function to return true, the Geometries must be two points, two curves or two surfaces.</returns>
		public bool IsOverlaps(int dimensionOfGeometryA, int dimensionOfGeometryB) 
		{
			if ( (dimensionOfGeometryA == Dimension.P && dimensionOfGeometryB == Dimension.P) ||
				 (dimensionOfGeometryA == Dimension.A && dimensionOfGeometryB == Dimension.A) ) 
			{
				return Matches( _matrix[Location.Interior,Location.Interior], 'T' ) &&
					   Matches( _matrix[Location.Interior,Location.Exterior], 'T' ) &&
					   Matches( _matrix[Location.Exterior,Location.Interior], 'T' );
			}
			if (dimensionOfGeometryA == Dimension.L && dimensionOfGeometryB == Dimension.L) 
			{
				return _matrix[Location.Interior,Location.Interior] == 1 &&
						Matches( _matrix[Location.Interior,Location.Exterior], 'T' ) &&
						Matches( _matrix[Location.Exterior,Location.Interior], 'T' );
			}
			return false;
		} // public bool IsOverlaps(int dimensionOfGeometryA, int dimensionOfGeometryB)

		/// <summary>
		/// Transposes this IntersectionMatrix.
		/// </summary>
		/// <returns>Returns this IntersectionMatrix as a convenience.</returns>
		public IntersectionMatrix Transpose() 
		{
			int temp = _matrix[1,0];
			_matrix[1,0] = _matrix[0,1];
			_matrix[0,1] = temp;
			temp = _matrix[2,0];
			_matrix[2,0] = _matrix[0,2];
			_matrix[0,2] = temp;
			temp = _matrix[2,1];
			_matrix[2,1] = _matrix[1,2];
			_matrix[1,2] = temp;
			return this;
		} // public IntersectionMatrix Transpose()

		/// <summary>
		/// Returns a nine-character string representation of this IntersectionMatrix.
		/// </summary>
		/// <returns>Returns the nine dimension symbols of this IntersectionMatrix in row-major order.</returns>
		public override string ToString() 
		{
			StringBuilder buf = new StringBuilder(9);
			for (int ai = 0; ai < 3; ai++) 
			{
				for (int bi = 0; bi < 3; bi++) 
				{
					buf.Insert( (3*ai)+bi, Dimension.ToDimensionSymbol( _matrix[ai,bi]) ) ;
				}
			}
			return buf.ToString();
		} // public string ToString()

		/// <summary>
		/// Returns true if the dimension value satisfies the dimension symbol.
		/// </summary>
		/// <param name="actualDimensionValue">A number that can be stored in the IntersectionMatrix. Possible
		/// values are {-2, -1, -3, 0, 1, 2}</param>
		/// <param name="requiredDimensionSymbol">A character used in the string representation of an
		/// IntersectionMatrix.  Possible values are {"T", "F", "*", "0", "1", "2"}</param>
		/// <returns>True if the dimension value satisfies the dimension symbol.</returns>
		public static bool Matches( int actualDimensionValue, char requiredDimensionSymbol ) 
		{
			if (requiredDimensionSymbol == '*') 
			{
				return true;
			}
			if ( requiredDimensionSymbol == 'T' && (actualDimensionValue >= 0 || actualDimensionValue
				== Dimension.True) ) 
			{
				return true;
			}
			if (requiredDimensionSymbol == 'F' && actualDimensionValue == Dimension.False) 
			{
				return true;
			}
			if (requiredDimensionSymbol == '0' && actualDimensionValue == Dimension.P) 
			{
				return true;
			}
			if (requiredDimensionSymbol == '1' && actualDimensionValue == Dimension.L) 
			{
				return true;
			}
			if (requiredDimensionSymbol == '2' && actualDimensionValue == Dimension.A) 
			{
				return true;
			}
			return false;
		} // public static bool Matches( int actualDimensionValue, char requiredDimensionSymbol )

		/// <summary>
		/// Returns true if each of the actual dimension symbols satisfies the
		/// corresponding required dimension symbol.
		/// </summary>
		/// <param name="actualDimensionSymbols">Nine dimension symbols to validate. Possible values are
		/// {T, F, * , 0, 1, 2}</param>
		/// <param name="requiredDimensionSymbols">Nine dimension symbols to validate against. Possible values are
		/// {T, F, * , 0, 1, 2}</param>
		/// <returns>Returns true if each of the required dimension
		///  symbols encompass the corresponding actual dimension symbol.</returns>
		public static bool Matches(string actualDimensionSymbols, string requiredDimensionSymbols) 
		{
			IntersectionMatrix m = new IntersectionMatrix( actualDimensionSymbols );
			return m.Matches( requiredDimensionSymbols );
		}

		/// <summary>
		///  Returns whether the elements of this IntersectionMatrix
		///  satisfies the required dimension symbols.
		/// </summary>
		/// <param name="requiredDimensionSymbols">Nine dimension symbols with which to
		/// compare the elements of this IntersectionMatrix. Possible
		/// values are {T, F, * , 0, 1, 2}</param>
		/// <returns>Returns true if this IntersectionMatrix matches the required dimension symbols.</returns>
		public bool Matches( string requiredDimensionSymbols ) 
		{
			if ( requiredDimensionSymbols.Length != 9 ) 
			{
				throw new ArgumentException("Should be length 9: " + requiredDimensionSymbols);
			}

			for (int ai = 0; ai < 3; ai++) 
			{
				for (int bi = 0; bi < 3; bi++) 
				{
					if ( !Matches( _matrix[ai,bi], requiredDimensionSymbols[ (3 * ai) + bi] ) ) 
					{
						return false;
					}
				}
			}
			return true;
		} // public bool Matches(string requiredDimensionSymbols)

		#endregion
	}
}

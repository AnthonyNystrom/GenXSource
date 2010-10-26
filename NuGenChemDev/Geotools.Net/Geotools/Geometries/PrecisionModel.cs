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
using Geotools.Geometries;
#endregion

namespace Geotools.Geometries
{
	/// <summary>
	///	 <para>All numerical computation takes place under some form of precision model.  There are
	///	 several possible types of precision models:</para>
	///	 </summary>
	///	 
	///	 <remarks>
	/// <list type="table">
    ///
	/// <item><term>Fixed</term><description>Coordinates are represended as points on a grid with uniform spacing. (This can be assumed to be the integer grid, with the use of appropriate scale and offset factors).  Computed coordinates are rounded to this grid.</description></item>
	/// <item><term>Floating</term><description>Coordinates are represented as floting-point numbers.  Computed coordinates may have more digits of precision that the input values (up the maximum allowed by the finite floating-point representation.</description></item>
	/// </list>
	/// 
	///  <para>For the Fixed precision model, this class converts a coordinate to and from a "precise" coordinate; 
	///  that is, one whose precision is known exactly. In other words, specifies the grid of allowable
	///  points for all Geometries.</para>
	///
	///  <para>Under the Fixed precision model, vertices are assumed to be precise in JTS. That is, 
	///  the coordinates of vertices are assumed to be rounded to the defined precision model. Input
	///  routines will be responsible for rounding coordinates to the precision model
	///  before creating JTS structures. Non-constructive internal operations will
	///  assume that coordinates are rounded to the precision model.</para>
	///
	///  <para>JTS methods will not handle inputs with different precision models.</para>
	///
	///  <para>The Fixed Precision Model will be specified by a scale factor and an offset point.
	///  The scale factor specifies how many JTS units represent one world unit.
	///  World coordinates are mapped to JTS coordinates according to the following
	///  equations:
	///  <list type="bullet">
	///  <item><term>Pt.x = truncate( (inputPt.x - offset.x) * scale )</term><description>Your Description</description></item>
	///  <item><term>Pt.y = truncate( (inputPt.y - offset.y) * scale )</term><description>Your Description</description></item>
	///  </list>
	///  Scaled coordinates will be represented internally as integral
	///  double-precision values. This is known as the "precise internal
	///  representation". </para>
	///  
	///  <para>Under the Floating Precision Model, coordinates can have the full precision available with .NET
	///  double-precision floating point numbers.  Input coordinates are not assumed to be rounded off, and
	///  internal operations which comput constructed points do not round off the coordinates.  Note that this
	///  does not mean that constructed points are exact; they are still limited to the precision of double-precision
	///  numbers, and hence may still be only an approximation to the exact point.</para>
	///  
	///  <para>For .NET Framework Class: The Double value type represents a double-precision 64-bit number with
	///  values ranging from negative 1.79769313486232e308 to positive 1.79769313486232e308, as well as 
	///  positive or negative zero, PositiveInfinity, NegativeInfinity, and Not-a-Number (NaN).
	///  Double complies with the IEC 60559:1989 (IEEE 754) standard for binary floating-point arithmetic.
	///  </para>
	/// </remarks>	 
	public class PrecisionModel
	{
		// The types of Precision Model which could be implemented.
		// (Note that JTS does not necessarily support all of these.)

		/// <summary>
		/// Fixed Precision implies that coordinates have a  fixed number of decimal places.
		/// </summary>
		protected const int FIXED = 1;

		/// <summary>
		/// Floating precision corresponds to the usual notation of floating-point representation.
		/// </summary>
		protected const int FLOATING = 2;

		/// <summary>
		/// The maximum precise value representable in double.  Since IEEE 754 allows a double-precision 64-bit
		/// number is 1.79769313486232e308.
		/// </summary>
		/// <remarks>The maximum precise value is the Math.Floor( Double.MaxValue ).
		/// See .NET Framework documentation on System.Double.</remarks>
		protected double _maximumPreciseValue = Double.MaxValue; 

		/// <summary>
		/// The type of PrecisionModel this represents.  Defaults to floating precision model.
		/// </summary>
		protected int _modelType = FLOATING;

		/// <summary>
		/// Amount by which to multiply a coordinate after subtracting the _offset, to obtain a precise coordinate.
		/// </summary>
		/// <remarks>If _scale is 0, this PrecisionModel represents a floating precision model.  Coordinates are left with
		/// the implicit precision of the floating-point representation.
		/// </remarks>
		protected double _scale = 0;

		/// <summary>
		/// Amount by which to subtract the x-coordinate before multiplying by the scale, to obtain a precise coordinate.
		/// </summary>
		protected double _offsetX = 0;

		/// <summary>
		/// Amount by which to subtract the y-coordinate before multiplying by the scale, to obtain a precise coordinate.
		/// </summary>
		protected double _offsetY = 0;

		#region Constructors
		/// <summary>
		/// Constructor for Floating Precision Model.
		/// </summary>
		public PrecisionModel()
		{
			_modelType = FLOATING;
		}

		/// <summary>
		/// Constructor for Fixed Precision Model.
		/// </summary>
		/// <remarks>Fixed-precision coordinates are represented as precise
		/// internal coordinates, which are integers stored in double-precision. Input coordinates are 
		/// transformed into precise internal coordinates according to the given scale, x-offset and y-offset.
		/// </remarks>
		/// <param name="scale">Amount by which to multiply a coordinate after subtracting
		///  the offset, to obtain a precise coordinate</param>
		/// <param name="offsetX">Amount by which to subtract the x-coordinate before
		///  multiplying by the scale, to obtain a precise coordinate.</param>
		/// <param name="offsetY">Amount by which to subtract the y-coordinate before
		///  multiplying by the scale, to obtain a precise coordinate.</param>
		public PrecisionModel(double scale, double offsetX, double offsetY)
		{
			_modelType = FIXED;
			_scale = scale;
			_offsetX = offsetX;
			_offsetY = offsetY;
		}

		/// <summary>
		/// Copy constructor to create a new PrecisionModel from an existing one.
		/// </summary>
		/// <param name="pm">The existing precisionmodel to use to create the new one.</param>
		public PrecisionModel(PrecisionModel pm) 
		{
			_modelType = pm.ModelType;
			_scale = pm.Scale;
			_offsetX = pm.OffsetX;
			_offsetY = pm.OffsetY;
		}

		#endregion

		#region Properties
		/// <summary>
		/// Represents the model type of this precision object.
		/// </summary>
		public int ModelType
		{
			get
			{
				return _modelType;
			}
		}

		/// <summary>
		/// Amount by which to multiply a coordinate after subtracting the offset,
		/// to obtain a precise coordinate.  If scale is 0, this PrecisionModel represents 
		/// a floating precision model.  Coordinates are left with the implicit precision of the
		/// floating-point representation.
		/// </summary>
		public double Scale
		{
			get
			{
				return _scale;
			}
		}

		/// <summary>
		/// Amount by which to subtract the x-coordinate before multiplying by the
		/// scale, to obtain a precise coordinate.
		/// </summary>
		public double OffsetX
		{
			get
			{
				return _offsetX;
			}
		}

		/// <summary>
		/// Amount by which to subtract the y-coordinate before multiplying by the
		/// scale, to obtain a precise coordinate.
		/// </summary>
		public double OffsetY
		{
			get
			{
				return _offsetY;
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// This method "fixes" an ordinate value to the Fixed PrecisionModel grid.
		/// </summary>
		/// <param name="val">The double to be used for comparison.</param>
		/// <returns>
		/// Returns the closest floating-point value to val that is equal to a mathematical integer.
		/// Returns the largest (closest to positive infinity) double value that is not greater than 
		/// the argument and is equal to a mathematical integer. Special cases: If the argument value 
		/// is already equal to a mathematical integer, then the result is the same as the argument. 
		/// If the argument is NaN or an infinity or positive zero or negative zero, then the result is the same as the argument.
		/// </returns>
		public static double MakePrecise(double val)
		{
			return Math.Round( val );
		}
		
		/// <summary>
		/// Returns true if the model type of this object is Floating and false otherwise.
		/// </summary>
		/// <returns>Returns true if the model type of this object is Floating and false otherwise.</returns>
		public bool IsFloating()
		{
			return _modelType == FLOATING;
		}

		/// <summary>
		/// Sets internal to Fixed Precise representation of external.
		/// </summary>
		/// <param name="externalCoordinate">The original coordinate.</param>
		/// <param name="internalCoordinate">The coordinate whose values will be changed to the
		/// fixed precise representation of external.</param>
		public void ToInternal(Coordinate externalCoordinate, Coordinate internalCoordinate) 
		{
			if ( IsFloating() ) 
			{
				internalCoordinate.X = externalCoordinate.X;
				internalCoordinate.Y = externalCoordinate.Y;
			}
			else 
			{
				internalCoordinate.X = MakePrecise((externalCoordinate.X - _offsetX)*_scale);
				internalCoordinate.Y = MakePrecise((externalCoordinate.Y - _offsetY)*_scale);
			}

			internalCoordinate.Z = externalCoordinate.Z;
		}

		/// <summary>
		/// Returns the fixed precise representation of externalCoordinate.
		/// </summary>
		/// <param name="externalCoordinate">The original coordinate.</param>
		/// <returns>Returns the coordinate whose values will be changed to the fixed precise representation
		/// of externalCoordinate.</returns>
		public Coordinate ToInternal( Coordinate externalCoordinate ) 
		{
			Coordinate internalCoordinate = new Coordinate();
			ToInternal(externalCoordinate, internalCoordinate);
			return internalCoordinate;
		}


		/// <summary>
		/// Returns the external representation of internalCoordinate.
		/// </summary>
		/// <param name="internalCoordinate">The originalCoordinate.</param>
		/// <returns>Returns the coordinate whose values will be changed to the external
		/// representation of internalCoordinate.</returns>
		public Coordinate ToExternal(Coordinate internalCoordinate) 
		{
			Coordinate externalCoordinate = new Coordinate();
			ToExternal(internalCoordinate, externalCoordinate);
			return externalCoordinate;
		}

		/// <summary>
		/// Sets externalCoordinate to the external representation of internalCoordinate.
		/// </summary>
		/// <param name="internalCoordinate">The original coordinate.</param>
		/// <param name="externalCoordinate">The coordinate whose values will be changed to the external
		/// representation of internalCoordinate.</param>
		public void ToExternal(Coordinate internalCoordinate, Coordinate externalCoordinate) 
		{
			if (IsFloating()) 
			{
				externalCoordinate.X = internalCoordinate.X;
				externalCoordinate.Y = internalCoordinate.Y;
			}
			else 
			{
				externalCoordinate.X = (internalCoordinate.X / _scale) + _offsetX;
				externalCoordinate.Y = (internalCoordinate.Y / _scale) + _offsetY;
			}
			externalCoordinate.Z = internalCoordinate.Z;
		}

		/// <summary>
		/// Sets p1 to the value of external point p0 rounded to this precision model.
		/// </summary>
		/// <param name="p0">The original coordinate.</param>
		/// <param name="p1">The coordinate whose values will be changed to the external representation of internal.</param>
		public void Round(Coordinate p0, Coordinate p1) 
		{
			ToInternal(p0, p1);
			ToExternal(p1, p1);
		}


		/// <summary>
		/// Writes out the string representation of this object.  If IsFloating() is true, then
		/// ToString()returns "Floating".  If not true, then ToString() returns "Fixed" plus the 
		/// scale and offset values. 
		/// </summary>
		/// <returns>Returns a description of this object.</returns>
		public override string ToString()
		{
			string description = String.Empty;
			if ( IsFloating() ) 
			{
				description = "Floating";
			}
			else 
			{
				description = "Fixed ( Scale = " + _scale;
				description += "   Offset: X = " + _offsetX;
				description += ", Y = " + _offsetY;
				description += " )";
			}
			return description;
		}

		/// <summary>
		/// Returns true if obj is of type PrecisionModel and Scale, OffsetX and OffsetY are equal.
		/// </summary>
		/// <param name="obj">Object to compare with this object.</param>
		/// <returns>Returns true if objects are equal.</returns>
		public override bool Equals( object obj ) 
		{
			bool returnValue = false;
			PrecisionModel pm = obj as PrecisionModel;
			if ( pm != null )
			{
				if (   ( _modelType == pm.ModelType ) && ( _scale == pm.Scale ) &&
			 	       ( _offsetX == pm.OffsetX ) && ( _offsetY == pm.OffsetY )   )
				{
					returnValue = true;
				}
			}
			return returnValue;
		}

		/// <summary>
		/// Returns a unique integer value for this object.
		/// </summary>
		/// <returns>Returns unique integer value for this object.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	#endregion

	} // public class PrecisionModel
}

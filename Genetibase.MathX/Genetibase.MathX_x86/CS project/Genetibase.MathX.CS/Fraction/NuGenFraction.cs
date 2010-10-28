using System;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Genetibase.MathX
{

	/// <summary>
	/// Classes Contained:
	///     NuGenFraction
	///     NuGenFractionException
	/// </summary>
	/// <summary>
	/// Class name: NuGenFraction
	/// Properties:
	///     Numerator: Set/Get value for Numerator
	///     Denominator:  Set/Get value for Numerator
	///     [Note: If you Set either Property, the NuGenFraction should be passed to ReduceFraction at some point.]
	/// 
	/// Constructors:
	///     no arguments:   initializes fraction as 0/0 = NaN, so don't do that!
	///     (Numerator, Denominator): initializes fraction with the given numerator and denominator 
	///                               values and reduces
	///     (long): initializes fraction with the given long value
	///     (double):   initializes fraction with the given double value
	///     (string):   initializes fraction with the given string value
	///                 the string can be an in the form of and integer, double or fraction.
	///                 e.g it can be like "123" or "123.321" or "123/456"
	/// 
	/// Public Methods (Description is given with respective methods' definitions)
	///     NuGenFraction ToFraction(long)
	///     NuGenFraction ToFraction(double)
	///     NuGenFraction ToFraction(string)
	///     Int32 ToInt32()
	///     Int64 ToInt64()
	///     double ToDouble()
	///     (override) string ToString()
	///     NuGenFraction Inverse()
	///     NuGenFraction Inverted(long)
	///     NuGenFraction Inverted(double)
	///     ReduceFraction(ref NuGenFraction)
	///     CrossReducePair(ref NuGenFraction, ref NuGenFraction)
	///     (override) Equals(object)
	///     (override) GetHashCode()
	/// 
	/// Overloaded Operators (overloaded for Fractions, long and double)
	///     Unary: -
	///     Binary: +,-,*,/
	///     
	/// 
	/// Overloaded user-defined conversions
	///     Implicit:   From long/double/string to NuGenFraction
	///     Explicit:   From NuGenFraction to long/double/string
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct NuGenFraction : IComparable, IFormattable
	{

		#region Constructors

		/// <summary>
		/// Construct a NuGenFraction from an integral value
		/// </summary>
		/// <param name="wholeNumber">The value (eventual numerator)</param>
		/// <remarks>The denominator will be 1</remarks>
		public NuGenFraction(long wholeNumber)
		{
						
			if (wholeNumber == long.MinValue)
				wholeNumber++;	// prevent serious issues later..

			m_Numerator = wholeNumber;
			m_Denominator = 1;
			// no reducing required, we're a whole number
		}
	    
		/// <summary>
		/// Construct a NuGenFraction from a floating-point value
		/// </summary>
		/// <param name="floatingPointNumber">The value</param>
		public NuGenFraction(double floatingPointNumber)
		{
			this = ToFraction(floatingPointNumber);
		}
	        
		/// <summary>
		/// Construct a NuGenFraction from a string in any legal format
		/// </summary>
		/// <param name="inValue">A string with a legal fraction input format</param>
		/// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
		/// <see>ToFraction(string strValue)</see>
		public NuGenFraction(string inValue)
		{
			this = ToFraction(inValue);
		}
	        
		/// <summary>
		/// Construct a NuGenFraction from a numerator, denominator pair
		/// </summary>
		/// <param name="numerator">The numerator (top number)</param>
		/// <param name="denominator">The denominator (bottom number)</param>
		/// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
		public NuGenFraction(long numerator, long denominator)
		{
						
			if (numerator == long.MinValue)
				numerator++;	// prevent serious issues later..

			if (denominator == long.MinValue)
				denominator++;	// prevent serious issues later..

			m_Numerator = numerator;
			m_Denominator = denominator;
			ReduceFraction(ref this);
		}

		/// <summary>
		/// Private constructor to synthesize a NuGenFraction for indeterminates (NaN and infinites)
		/// </summary>
		/// <param name="type">Kind of inderterminate</param>
		private NuGenFraction(Indeterminates type)
		{
			m_Numerator = (long)type;
			m_Denominator = 0;
			// do NOT reduce, we're clean as can be!
		}

		#endregion

		#region Properties

		/// <summary>
		/// The 'top' part of the fraction
		/// </summary>
		/// <example>For 3/4ths, this is the 3</example>
		public long Numerator
		{
			get 
			{
				return m_Numerator;
			}
			set
			{
				m_Numerator = value;
			}
		}

		/// <summary>
		/// The 'bottom' part of the fraction
		/// </summary>
		/// <example>For 3/4ths, this is the 4</example>
		public long Denominator
		{
			get
			{
				return m_Denominator;
			}
			set
			{
				m_Denominator = value;
			}
		}

		#endregion

		#region Expose constants

		public static readonly NuGenFraction NaN = new NuGenFraction(Indeterminates.NaN);
		public static readonly NuGenFraction PositiveInfinity = new NuGenFraction(Indeterminates.PositiveInfinity);
		public static readonly NuGenFraction NegativeInfinity = new NuGenFraction(Indeterminates.NegativeInfinity);
		public static readonly NuGenFraction Zero = new NuGenFraction(0,1);
		public static readonly NuGenFraction Epsilon = new NuGenFraction(1, Int64.MaxValue);

		private static readonly double EpsilonDouble = 1.0 / Int64.MaxValue;

		public static readonly NuGenFraction MaxValue = new NuGenFraction(Int64.MaxValue, 1);
		public static readonly NuGenFraction MinValue = new NuGenFraction(Int64.MinValue, 1);

		#endregion

		#region Explicit conversions
		#region To primitives

		/// <summary>
		/// Get the integral value of the NuGenFraction object as int/Int32
		/// </summary>
		/// <returns>The (approximate) integer value</returns>
		/// <remarks>If the value is not a true integer, the fractional part is discarded
		/// (truncated toward zero). If the valid exceeds the range of an Int32 and exception is thrown.</remarks>
		/// <exception cref="NuGenFractionException">Will throw a NuGenFractionException for NaN, PositiveInfinity
		/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.</exception>
		/// <exception cref="OverflowException">Will throw a System.OverflowException if the value is too
		/// large or small to be represented as an Int32.</exception>
		public Int32 ToInt32()
		{
						
			if (this.m_Denominator == 0)
			{
				throw new NuGenFractionException(string.Format("Cannot convert {0} to Int32", IndeterminateTypeName(this.m_Numerator)), new System.NotFiniteNumberException());
			}

			long bestGuess = this.m_Numerator / this.m_Denominator;

			if (bestGuess > Int32.MaxValue || bestGuess < Int32.MinValue)
			{
				throw new NuGenFractionException("Cannot convert to Int32", new System.OverflowException());
			}

			return (Int32)bestGuess;
		}

		/// <summary>
		/// Get the integral value of the NuGenFraction object as long/Int64
		/// </summary>
		/// <returns>The (approximate) integer value</returns>
		/// <remarks>If the value is not a true integer, the fractional part is discarded
		/// (truncated toward zero). If the valid exceeds the range of an Int32, no special
		/// handling is guaranteed.</remarks>
		/// <exception cref="NuGenFractionException">Will throw a NuGenFractionException for NaN, PositiveInfinity
		/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.</exception>
		public Int64 ToInt64()
		{
						
			if (this.m_Denominator == 0)
			{
				throw new NuGenFractionException(string.Format("Cannot convert {0} to Int64", IndeterminateTypeName(this.m_Numerator)), new System.NotFiniteNumberException());
			}

			return this.m_Numerator / this.m_Denominator;
		}

		/// <summary>
		/// Get the value of the NuGenFraction object as double with full support for NaNs and infinities
		/// </summary>
		/// <returns>The decimal representation of the NuGenFraction, or double.NaN, double.NegativeInfinity
		/// or double.PositiveInfinity</returns>
		public double ToDouble()
		{
						
			if (this.m_Denominator == 1)
				return this.m_Numerator;

			else if (this.m_Denominator == 0)
			{

				switch (NormalizeIndeterminate(this.m_Numerator))
				{

					case Indeterminates.NegativeInfinity:
						return double.NegativeInfinity;

					case Indeterminates.PositiveInfinity:
						return double.PositiveInfinity;

					case Indeterminates.NaN:
					default:    // this can't happen
						return double.NaN;
				}
			}

			else
			{
				return (double)this.m_Numerator / (double)this.m_Denominator;
			}
		}

		/// <summary>
		/// Get the value of the NuGenFraction as a string, with proper representation for NaNs and infinites
		/// </summary>
		/// <returns>The string representation of the NuGenFraction, or the culture-specific representations of
		/// NaN, PositiveInfinity or NegativeInfinity.</returns>
		/// <remarks>The current culture determines the textual representation the Indeterminates</remarks>
		public override string ToString()
		{
						
			if (this.m_Denominator == 1)
			{
				return this.m_Numerator.ToString();
			}

			else if (this.m_Denominator == 0)
			{
				return IndeterminateTypeName(this.m_Numerator);
			}

			else
			{
				return this.m_Numerator.ToString() + "/" + this.m_Denominator.ToString();
			}
		}

		#endregion

		#region From primitives

		/// <summary>
		/// Converts a long value to the exact NuGenFraction
		/// </summary>
		/// <param name="inValue">The long to convert</param>
		/// <returns>An exact representation of the value</returns>
		public static NuGenFraction ToFraction(long inValue)
		{
			return new NuGenFraction(inValue);
		}

		/// <summary>
		/// Converts a double value to the approximate NuGenFraction
		/// </summary>
		/// <param name="inValue">The double to convert</param>
		/// <returns>A best-fit representation of the value</returns>
		/// <remarks>Supports double.NaN, double.PositiveInfinity and double.NegativeInfinity</remarks>
		public static NuGenFraction ToFraction(double inValue)
		{
						
			// it's one of the indeterminates... which?
			if (double.IsNaN(inValue))
				return NaN;

			else if (double.IsNegativeInfinity(inValue))
				return NegativeInfinity;

			else if (double.IsPositiveInfinity(inValue))
				return PositiveInfinity;

			else if (inValue == 0.0d)
				return Zero;

			if (inValue > Int64.MaxValue)
				throw new OverflowException(string.Format("Double {0} too large", inValue));

			if (inValue < -Int64.MaxValue)
				throw new OverflowException(string.Format("Double {0} too small", inValue));

			if (-EpsilonDouble < inValue && inValue < EpsilonDouble)
				throw new ArithmeticException(string.Format("Double {0} cannot be represented", inValue));

			int sign = Math.Sign(inValue);
			inValue = Math.Abs(inValue);

			return ConvertPositiveDouble(sign, inValue);
		}

		/// <summary>
		/// Converts a string to the corresponding reduced fraction
		/// </summary>
		/// <param name="inValue">The string representation of a fractional value</param>
		/// <returns>The NuGenFraction that represents the string</returns>
		/// <remarks>Four forms are supported, as a plain integer, as a double, or as Numerator/Denominator
		/// and the representations for NaN and the infinites</remarks>
		/// <example>"123" = 123/1 and "1.25" = 5/4 and "10/36" = 5/13 and NaN = 0/0 and
		/// PositiveInfinity = 1/0 and NegativeInfinity = -1/0</example>
		public static NuGenFraction ToFraction(string inValue)
		{
						
			if (inValue == null || inValue == string.Empty)
				throw new ArgumentNullException("inValue");

			// could also be NumberFormatInfo.InvariantInfo
			NumberFormatInfo info = NumberFormatInfo.CurrentInfo;

			// Is it one of the special symbols for NaN and such...
			string trimmedValue = inValue.Trim();

			if (trimmedValue == info.NaNSymbol)
				return NaN;

			else if (trimmedValue == info.PositiveInfinitySymbol)
				return PositiveInfinity;

			else if (trimmedValue == info.NegativeInfinitySymbol)
				return NegativeInfinity;

			else
			{
				// Not special, is it a NuGenFraction?
				int slashPos = inValue.IndexOf('/');

				if (slashPos > -1)
				{
					// string is in the form of Numerator/Denominator
					long numerator = Convert.ToInt64(inValue.Substring(0, slashPos));
					long denominator = Convert.ToInt64(inValue.Substring(slashPos + 1));

					return new NuGenFraction(numerator, denominator);
				}

				else
				{
					// the string is not in the form of a fraction
					// hopefully it is double or integer, do we see a decimal point?
					int decimalPos = inValue.IndexOf(info.CurrencyDecimalSeparator);

					if (decimalPos > -1)
						return new NuGenFraction(Convert.ToDouble(inValue));

					else
						return new NuGenFraction(Convert.ToInt64(inValue));
				}
			}
		}

		#endregion
		#endregion

		#region Indeterminate classifications

		/// <summary>
		/// Determines if a NuGenFraction represents a Not-a-Number
		/// </summary>
		/// <returns>True if the NuGenFraction is a NaN</returns>
		public bool IsNaN()
		{
						
			if (this.m_Denominator == 0 
				&& NormalizeIndeterminate(this.m_Numerator) == Indeterminates.NaN)
				return true;

			else
				return false;
		}

		/// <summary>
		/// Determines if a NuGenFraction represents Any Infinity
		/// </summary>
		/// <returns>True if the NuGenFraction is Positive Infinity or Negative Infinity</returns>
		public bool IsInfinity()
		{
						
			if (this.m_Denominator == 0
				&& NormalizeIndeterminate(this.m_Numerator) != Indeterminates.NaN)
				return true;

			else
				return false;
		}

		/// <summary>
		/// Determines if a NuGenFraction represents Positive Infinity
		/// </summary>
		/// <returns>True if the NuGenFraction is Positive Infinity</returns>
		public bool IsPositiveInfinity()
		{
						
			if (this.m_Denominator == 0
				&& NormalizeIndeterminate(this.m_Numerator) == Indeterminates.PositiveInfinity)
				return true;

			else
				return false;
		}

		/// <summary>
		/// Determines if a NuGenFraction represents Negative Infinity
		/// </summary>
		/// <returns>True if the NuGenFraction is Negative Infinity</returns>
		public bool IsNegativeInfinity()
		{
						
			if (this.m_Denominator == 0
				&& NormalizeIndeterminate(this.m_Numerator) == Indeterminates.NegativeInfinity)
				return true;

			else
				return false;
		}

		#endregion

		#region Inversion

		/// <summary>
		/// Inverts a NuGenFraction
		/// </summary>
		/// <returns>The inverted NuGenFraction (with Denominator over Numerator)</returns>
		/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
		public NuGenFraction Inverse()
		{
			// don't use the obvious constructor because we do not want it normalized at this time
			NuGenFraction frac = new NuGenFraction();

			frac.m_Numerator = this.m_Denominator;
			frac.m_Denominator = this.m_Numerator;
			return frac;
		}

		/// <summary>
		/// Creates an inverted NuGenFraction
		/// </summary>
		/// <returns>The inverted NuGenFraction (with Denominator over Numerator)</returns>
		/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
		public static NuGenFraction Inverted(long value)
		{
			NuGenFraction frac = new NuGenFraction(value);
			return frac.Inverse();
		}

		/// <summary>
		/// Creates an inverted NuGenFraction
		/// </summary>
		/// <returns>The inverted NuGenFraction (with Denominator over Numerator)</returns>
		/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
		public static NuGenFraction Inverted(double value)
		{
			NuGenFraction frac = new NuGenFraction(value);
			return frac.Inverse();
		}

		#endregion

		#region Operators
		#region Unary Negation operator

		/// <summary>
		/// Negates the NuGenFraction
		/// </summary>
		/// <param name="left">The NuGenFraction to negate</param>
		/// <returns>The negative version of the NuGenFraction</returns>
		public static NuGenFraction operator -(NuGenFraction left)
		{
			return Negate(left);
		}

		#endregion

		#region Addition operators

		public static NuGenFraction operator +(NuGenFraction left, NuGenFraction right)
		{
			return Add(left, right);
		}
	    
		public static NuGenFraction operator +(long left, NuGenFraction right)
		{
			return Add(new NuGenFraction(left), right);
		}
	    
		public static NuGenFraction operator +(NuGenFraction left, long right)
		{
			return Add(left, new NuGenFraction(right));
		}

		public static NuGenFraction operator +(double left, NuGenFraction right)
		{
			return Add(ToFraction(left), right);
		}
	    
		public static NuGenFraction operator +(NuGenFraction left, double right)
		{
			return Add(left, ToFraction(right));
		}

		#endregion

		#region Subtraction operators

		public static NuGenFraction operator -(NuGenFraction left, NuGenFraction right)
		{
			return Add(left, - right);
		}
	    
		public static NuGenFraction operator -(long left, NuGenFraction right)
		{
			return Add(new NuGenFraction(left), - right);
		}
	    
		public static NuGenFraction operator -(NuGenFraction left, long right)
		{
			return Add(left, new NuGenFraction(- right));
		}

		public static NuGenFraction operator -(double left, NuGenFraction right)
		{
			return Add(ToFraction(left), - right);
		}
	    
		public static NuGenFraction operator -(NuGenFraction left, double right)
		{
			return Add(left, ToFraction(- right));
		}

		#endregion

		#region Multiplication operators

		public static NuGenFraction operator *(NuGenFraction left, NuGenFraction right)
		{
			return Multiply(left, right);
		}
	    
		public static NuGenFraction operator *(long left, NuGenFraction right)
		{
			return Multiply(new NuGenFraction(left), right);
		}
	    
		public static NuGenFraction operator *(NuGenFraction left, long right)
		{
			return Multiply(left, new NuGenFraction(right));
		}
	    
		public static NuGenFraction operator *(double left, NuGenFraction right)
		{
			return Multiply(ToFraction(left), right);
		}
	    
		public static NuGenFraction operator *(NuGenFraction left, double right)
		{
			return Multiply(left, ToFraction(right));
		}

		#endregion

		#region Division operators

		public static NuGenFraction operator /(NuGenFraction left, NuGenFraction right)
		{
			return Multiply(left, right.Inverse());
		}
	    
		public static NuGenFraction operator /(long left, NuGenFraction right)
		{
			return Multiply(new NuGenFraction(left), right.Inverse());
		}
	    
		public static NuGenFraction operator /(NuGenFraction left, long right)
		{
			return Multiply(left, Inverted(right));
		}
	    
		public static NuGenFraction operator /(double left, NuGenFraction right)
		{
			return Multiply(ToFraction(left), right.Inverse());
		}
	    
		public static NuGenFraction operator /(NuGenFraction left, double right)
		{
			return Multiply(left, Inverted(right));
		}

		#endregion

		#region Modulus operators

		public static NuGenFraction operator %(NuGenFraction left, NuGenFraction right)
		{
			return Modulus(left, right);
		}
	    
		public static NuGenFraction operator %(long left, NuGenFraction right)
		{
			return Modulus(new NuGenFraction(left), right);
		}
	    
		public static NuGenFraction operator %(NuGenFraction left, long right)
		{
			return Modulus(left, right);
		}
	    
		public static NuGenFraction operator %(double left, NuGenFraction right)
		{
			return Modulus(ToFraction(left), right);
		}
	    
		public static NuGenFraction operator %(NuGenFraction left, double right)
		{
			return Modulus(left, right);
		}

		#endregion

		#region Equal operators

		public static bool operator ==(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareEquality(right, false);
		}

		public static bool operator ==(NuGenFraction left, long right)
		{
			return left.CompareEquality(new NuGenFraction(right), false);
		}

		public static bool operator ==(NuGenFraction left, double right)
		{
			return left.CompareEquality(new NuGenFraction(right), false);
		}

		#endregion

		#region Not-equal operators

		public static bool operator !=(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareEquality(right, true);
		}

		public static bool operator !=(NuGenFraction left, long right)
		{
			return left.CompareEquality(new NuGenFraction(right), true);
		}
	        
		public static bool operator !=(NuGenFraction left, double right)
		{
			return left.CompareEquality(new NuGenFraction(right), true);
		}

		#endregion

		#region Inequality operators

		/// <summary>
		/// Compares two Fractions to see if left is less than right
		/// </summary>
		/// <param name="left">The first NuGenFraction</param>
		/// <param name="right">The second NuGenFraction</param>
		/// <returns>True if <paramref name="left">left</paramref> is less
		/// than <paramref name="right">right</paramref></returns>
		/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
		/// <exception cref="NuGenFractionException">Throws an error if overflows occur while computing the 
		/// difference with an InnerException of OverflowException</exception>
		public static bool operator <(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Compares two Fractions to see if left is greater than right
		/// </summary>
		/// <param name="left">The first NuGenFraction</param>
		/// <param name="right">The second NuGenFraction</param>
		/// <returns>True if <paramref name="left">left</paramref> is greater
		/// than <paramref name="right">right</paramref></returns>
		/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
		/// <exception cref="NuGenFractionException">Throws an error if overflows occur while computing the 
		/// difference with an InnerException of OverflowException</exception>
		public static bool operator >(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Compares two Fractions to see if left is less than or equal to right
		/// </summary>
		/// <param name="left">The first NuGenFraction</param>
		/// <param name="right">The second NuGenFraction</param>
		/// <returns>True if <paramref name="left">left</paramref> is less than or 
		/// equal to <paramref name="right">right</paramref></returns>
		/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
		/// <exception cref="NuGenFractionException">Throws an error if overflows occur while computing the 
		/// difference with an InnerException of OverflowException</exception>
		public static bool operator <=(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareTo(right) <= 0;
		}
	        
		/// <summary>
		/// Compares two Fractions to see if left is greater than or equal to right
		/// </summary>
		/// <param name="left">The first NuGenFraction</param>
		/// <param name="right">The second NuGenFraction</param>
		/// <returns>True if <paramref name="left">left</paramref> is greater than or 
		/// equal to <paramref name="right">right</paramref></returns>
		/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
		/// <exception cref="NuGenFractionException">Throws an error if overflows occur while computing the 
		/// difference with an InnerException of OverflowException</exception>
		public static bool operator >=(NuGenFraction left, NuGenFraction right)
		{
			return left.CompareTo(right) >= 0;
		}

		#endregion

		#region Implict conversion from primitive operators

		/// <summary>
		/// Implicit conversion of a long integral value to a NuGenFraction
		/// </summary>
		/// <param name="value">The long integral value to convert</param>
		/// <returns>A NuGenFraction whose denominator is 1</returns>
		public static implicit operator NuGenFraction(long value)
		{
			return new NuGenFraction(value);
		}

		/// <summary>
		/// Implicit conversion of a double floating point value to a NuGenFraction
		/// </summary>
		/// <param name="value">The double value to convert</param>
		/// <returns>A reduced NuGenFraction</returns>
		public static implicit operator NuGenFraction(double value)
		{
			return new NuGenFraction(value);
		}

		/// <summary>
		/// Implicit conversion of a string to a NuGenFraction
		/// </summary>
		/// <param name="value">The string to convert</param>
		/// <returns>A reduced NuGenFraction</returns>
		public static implicit operator NuGenFraction(string value)
		{
			return new NuGenFraction(value);
		}

		#endregion

		#region Explicit converstion to primitive operators

		/// <summary>
		/// Explicit conversion from a NuGenFraction to an integer
		/// </summary>
		/// <param name="frac">the NuGenFraction to convert</param>
		/// <returns>The integral representation of the NuGenFraction</returns>
		public static explicit operator int(NuGenFraction frac)
		{
			return frac.ToInt32();
		}

		/// <summary>
		/// Explicit conversion from a NuGenFraction to an integer
		/// </summary>
		/// <param name="frac">The NuGenFraction to convert</param>
		/// <returns>The integral representation of the NuGenFraction</returns>
		public static explicit operator long(NuGenFraction frac)
		{
			return frac.ToInt64();
		}

		/// <summary>
		/// Explicit conversion from a NuGenFraction to a double floating-point value
		/// </summary>
		/// <param name="frac">The NuGenFraction to convert</param>
		/// <returns>The double representation of the NuGenFraction</returns>
		public static explicit operator double(NuGenFraction frac)
		{
			return frac.ToDouble();
		}

		/// <summary>
		/// Explicit conversion from a NuGenFraction to a string
		/// </summary>
		/// <param name="frac">the NuGenFraction to convert</param>
		/// <returns>The string representation of the NuGenFraction</returns>
		public static implicit operator string(NuGenFraction frac)
		{
			return frac.ToString();
		}

		#endregion
		#endregion

		#region Equals and GetHashCode overrides

		/// <summary>
		/// Compares for equality the current NuGenFraction to the value passed.
		/// </summary>
		/// <param name="obj">A  NuGenFraction,</param>
		/// <returns>True if the value equals the current fraction, false otherwise (including for
		/// non-NuGenFraction types or null object.</returns>
		public override bool Equals(object obj)
		{
						
			if (obj == null	|| ! (obj is NuGenFraction))
				return false;

			try 
			{
				NuGenFraction right = (NuGenFraction)obj;
				return this.CompareEquality(right, false);
			}

			catch
			{
				// can't throw in an Equals!
				return false;
			}
		}
	        
		/// <summary>
		/// Returns a hash code generated from the current NuGenFraction
		/// </summary>
		/// <returns>The hash code</returns>
		/// <remarks>Reduces (in-place) the NuGenFraction first.</remarks>
		public override int GetHashCode()
		{
			// insure we're as close to normalized as possible first
			ReduceFraction(ref this);

			int numeratorHash = this.m_Numerator.GetHashCode();
			int denominatorHash = this.m_Denominator.GetHashCode();

			return (numeratorHash ^ denominatorHash);
		}

		#endregion

		#region IComparable member and type-specific version

		/// <summary>
		/// Compares an object to this NuGenFraction
		/// </summary>
		/// <param name="obj">The object to compare against (null is less than everything)</param>
		/// <returns>-1 if this is less than <paramref name="obj"></paramref>,
		///  0 if they are equal,
		///  1 if this is greater than <paramref name="obj"></paramref></returns>
		/// <remarks>Will convert an object from longs, doubles, and strings as this is a value-type.</remarks>
		public int CompareTo(object obj)
		{
						
			if (obj == null)
				return 1;	// null is less than anything

			NuGenFraction right;

			if (obj is NuGenFraction)
				right = (NuGenFraction)obj;

			else if (obj is long)
				right = (long)obj;

			else if (obj is double)
				right = (double)obj;

			else if (obj is string)
				right = (string)obj;

			else
				throw new ArgumentException("Must be convertible to NuGenFraction", "obj");

			return this.CompareTo(right);
		}

		/// <summary>
		/// Compares this NuGenFraction to another NuGenFraction
		/// </summary>
		/// <param name="right">The NuGenFraction to compare against</param>
		/// <returns>-1 if this is less than <paramref name="right"></paramref>,
		///  0 if they are equal,
		///  1 if this is greater than <paramref name="right"></paramref></returns>
		public int CompareTo(NuGenFraction right)
		{
						
			// if left is an indeterminate, punt to the helper...
			if (this.m_Denominator == 0)
			{
				return IndeterminantCompare(NormalizeIndeterminate(this.m_Numerator), right);
			}

			// if right is an indeterminate, punt to the helper...
			if (right.m_Denominator == 0)
			{
				// note sign-flip...
				return - IndeterminantCompare(NormalizeIndeterminate(right.m_Numerator), this);
			}

			// they're both normal Fractions
			CrossReducePair(ref this, ref right);

			try
			{
				checked
				{
					long leftScale = this.m_Numerator * right.m_Denominator;
					long rightScale = this.m_Denominator * right.m_Numerator;

					if (leftScale < rightScale)
						return -1;

					else if (leftScale > rightScale)
						return 1;

					else
						return 0;
				}
			}

			catch (Exception e)
			{
				throw new NuGenFractionException(string.Format("CompareTo({0}, {1}) error", this, right), e);
			}
		}

		#endregion

		#region IFormattable Members

		string System.IFormattable.ToString(string format, IFormatProvider formatProvider)
		{
			return this.m_Numerator.ToString(format, formatProvider) + "/" + this.m_Denominator.ToString(format, formatProvider);
		}

		#endregion

		#region Reduction

		/// <summary>
		/// Reduces (simplifies) a NuGenFraction by dividing down to lowest possible denominator (via GCD)
		/// </summary>
		/// <param name="frac">The NuGenFraction to be reduced [WILL BE MODIFIED IN PLACE]</param>
		/// <remarks>Modifies the input arguments in-place! Will normalize the NaN and infinites
		/// representation. Will set Denominator to 1 for any zero numerator. Moves sign to the
		/// Numerator.</remarks>
		/// <example>2/4 will be reduced to 1/2</example>
		public static void ReduceFraction(ref NuGenFraction frac)
		{
						
			// clean up the NaNs and infinites
			if (frac.m_Denominator == 0)
			{
				frac.m_Numerator = (long)NormalizeIndeterminate(frac.m_Numerator);
				return;
			}

			// all forms of zero are alike.
			if (frac.m_Numerator == 0)
			{
				frac.m_Denominator = 1;
				return;
			}
	            
			long iGCD = GCD(frac.m_Numerator, frac.m_Denominator);
			frac.m_Numerator /= iGCD;
			frac.m_Denominator /= iGCD;
	        
			// if negative sign in denominator
			if ( frac.m_Denominator < 0 )
			{
				//move negative sign to numerator
				frac.m_Numerator = - frac.m_Numerator;
				frac.m_Denominator = - frac.m_Denominator;  
			}
		}

		/// <summary>
		/// Cross-reduces a pair of Fractions so that we have the best GCD-reduced values for multiplication
		/// </summary>
		/// <param name="frac1">The first NuGenFraction [WILL BE MODIFIED IN PLACE]</param>
		/// <param name="frac2">The second NuGenFraction [WILL BE MODIFIED IN PLACE]</param>
		/// <remarks>Modifies the input arguments in-place!</remarks>
		/// <example>(3/4, 5/9) = (1/4, 5/3)</example>
		public static void CrossReducePair(ref NuGenFraction frac1, ref NuGenFraction frac2)
		{
						
			// leave the indeterminates alone!
			if (frac1.m_Denominator == 0 || frac2.m_Denominator == 0)
				return;

			long gcdTop = GCD(frac1.m_Numerator, frac2.m_Denominator);
			frac1.m_Numerator = frac1.m_Numerator / gcdTop;
			frac2.m_Denominator = frac2.m_Denominator / gcdTop;

			long gcdBottom = GCD(frac1.m_Denominator, frac2.m_Numerator);
			frac2.m_Numerator = frac2.m_Numerator / gcdBottom;
			frac1.m_Denominator = frac1.m_Denominator / gcdBottom;
		}

		#endregion

		#region Implementation
		#region Convert a double to a fraction

		private static NuGenFraction ConvertPositiveDouble(int sign, double inValue)
		{
			// Shamelessly stolen from http://homepage.smc.edu/kennedy_john/CONFRAC.PDF
			// with AccuracyFactor == double.Episilon
			long fractionNumerator = (long)inValue;

			double fractionDenominator = 1;
			double previousDenominator = 0;
			double remainingDigits = inValue;
			int maxIterations = 594;	// found at http://www.ozgrid.com/forum/archive/index.php/t-22530.html

			while (remainingDigits != Math.Floor(remainingDigits) 
				&& Math.Abs(inValue - (fractionNumerator / fractionDenominator)) > double.Epsilon)
			{
				remainingDigits = 1.0 / (remainingDigits - Math.Floor(remainingDigits));

				double scratch = fractionDenominator;

				fractionDenominator =(Math.Floor(remainingDigits) * fractionDenominator) + previousDenominator;
				fractionNumerator = (long)(inValue * fractionDenominator + 0.5);

				previousDenominator = scratch;

				if (maxIterations-- < 0)
					break;
			}

			return new NuGenFraction(fractionNumerator * sign, (long)fractionDenominator);
		}

		#endregion

		#region Equality helper

		/// <summary>
		/// Compares for equality the current NuGenFraction to the value passed.
		/// </summary>
		/// <param name="right">A NuGenFraction to compare against</param>
		/// <param name="notEqualCheck">If true, we're looking for not-equal</param>
		/// <returns>True if the <paramref name="right"></paramref> equals the current 
		/// fraction, false otherwise. If comparing two NaNs, they are always equal AND
		/// not-equal.</returns>
		private bool CompareEquality(NuGenFraction right, bool notEqualCheck)
		{
			// insure we're normalized first
			ReduceFraction(ref this);

			// now normalize the comperand
			ReduceFraction(ref right);

			if (this.m_Numerator == right.m_Numerator && this.m_Denominator == right.m_Denominator)
			{

				// special-case rule, two NaNs are always both equal
				if (notEqualCheck && this.IsNaN())
					return true;

				else
					return ! notEqualCheck;
			}

			else
			{
				return notEqualCheck;
			}
		}

		#endregion

		#region Comparison helper

		/// <summary>
		/// Determines how this NuGenFraction, of an indeterminate type, compares to another NuGenFraction
		/// </summary>
		/// <param name="leftType">What kind of indeterminate</param>
		/// <param name="right">The other NuGenFraction to compare against</param>
		/// <returns>-1 if this is less than <paramref name="right"></paramref>,
		///  0 if they are equal,
		///  1 if this is greater than <paramref name="right"></paramref></returns>
		/// <remarks>NaN is less than anything except NaN and Negative Infinity. Negative Infinity is less
		/// than anything except Negative Infinity. Positive Infinity is greater than anything except
		/// Positive Infinity.</remarks>
		private static int IndeterminantCompare(Indeterminates leftType, NuGenFraction right)
		{
						
			switch (leftType)
			{

				case Indeterminates.NaN:

					// A NaN is...
					if (right.IsNaN())
						return 0;	// equal to a NaN

					else if (right.IsNegativeInfinity())
						return 1;	// great than Negative Infinity

					else
						return -1;	// less than anything else

				case Indeterminates.NegativeInfinity:

					// Negative Infinity is...
					if (right.IsNegativeInfinity())
						return 0;	// equal to Negative Infinity

					else
						return -1;	// less than anything else

				case Indeterminates.PositiveInfinity:

					if (right.IsPositiveInfinity())
						return 0;	// equal to Positive Infinity

					else
						return 1;	// greater than anything else

				default:
					// this CAN'T happen, something VERY wrong is going on...
					return 0;
			}
		}

		#endregion

		#region Math helpers

		/// <summary>
		/// Negates the NuGenFraction
		/// </summary>
		/// <param name="frac">Value to negate</param>
		/// <returns>A new NuGenFraction that is sign-flipped from the input</returns>
		private static NuGenFraction Negate(NuGenFraction frac)
		{
			// for a NaN, it's still a NaN
			return new NuGenFraction( - frac.m_Numerator, frac.m_Denominator);
		}

		/// <summary>
		/// Adds two Fractions
		/// </summary>
		/// <param name="left">A NuGenFraction</param>
		/// <param name="right">Another NuGenFraction</param>
		/// <returns>Sum of the Fractions. Returns NaN if either NuGenFraction is a NaN.</returns>
		/// <exception cref="NuGenFractionException">Will throw if an overflow occurs when computing the
		/// GCD-normalized values.</exception>
		private static NuGenFraction Add(NuGenFraction left, NuGenFraction right)
		{
						
			if (left.IsNaN() || right.IsNaN())
				return NaN;

			long gcd = GCD(left.m_Denominator, right.m_Denominator); // cannot return less than 1
			long leftDenominator = left.m_Denominator / gcd;
			long rightDenominator = right.m_Denominator / gcd;

			try
			{
				checked
				{
					long numerator = left.m_Numerator * rightDenominator + right.m_Numerator * leftDenominator;
					long denominator = leftDenominator * rightDenominator * gcd;

					return new NuGenFraction(numerator, denominator);
				}
			}

			catch (Exception e)
			{
				throw new NuGenFractionException("Add error", e);
			}
		}
	    
		/// <summary>
		/// Multiplies two Fractions
		/// </summary>
		/// <param name="left">A NuGenFraction</param>
		/// <param name="right">Another NuGenFraction</param>
		/// <returns>Product of the Fractions. Returns NaN if either NuGenFraction is a NaN.</returns>
		/// <exception cref="NuGenFractionException">Will throw if an overflow occurs. Does a cross-reduce to 
		/// insure only the unavoidable overflows occur.</exception>
		private static NuGenFraction Multiply(NuGenFraction left, NuGenFraction right)
		{
						
			if (left.IsNaN() || right.IsNaN())
				return NaN;

			// this would be unsafe if we were not a ValueType, because we would be changing the
			// caller's values.  If we change back to a class, must use temporaries
			CrossReducePair(ref left, ref right);

			try
			{
				checked
				{
					long numerator = left.m_Numerator * right.m_Numerator;
					long denominator = left.m_Denominator * right.m_Denominator;

					return new NuGenFraction(numerator, denominator);
				}
			}

			catch (Exception e)
			{
				throw new NuGenFractionException("Multiply error", e);
			}
		}

		/// <summary>
		/// Returns the modulus (remainder after dividing) two Fractions
		/// </summary>
		/// <param name="left">A NuGenFraction</param>
		/// <param name="right">Another NuGenFraction</param>
		/// <returns>Modulus of the Fractions. Returns NaN if either NuGenFraction is a NaN.</returns>
		/// <exception cref="NuGenFractionException">Will throw if an overflow occurs. Does a cross-reduce to 
		/// insure only the unavoidable overflows occur.</exception>
		private static NuGenFraction Modulus(NuGenFraction left, NuGenFraction right)
		{
						
			if (left.IsNaN() || right.IsNaN())
				return NaN;

			try
			{
				checked
				{
					// this will discard any fractional places...
					Int64 quotient = (Int64)(left / right);
					NuGenFraction whole = new NuGenFraction(quotient * right.m_Numerator, right.m_Denominator);
					return left - whole;
				}
			}

			catch (Exception e)
			{
				throw new NuGenFractionException("Modulus error", e);
			}
		}

		/// <summary>
		/// Computes the greatest common divisor for two values
		/// </summary>
		/// <param name="left">One value</param>
		/// <param name="right">Another value</param>
		/// <returns>The greatest common divisor of the two values</returns>
		/// <example>(6, 9) returns 3 and (11, 4) returns 1</example>
		private static long GCD(long left, long right)
		{
						
			// take absolute values
			if (left < 0)
				left = - left;

			if (right < 0)
				right = - right;
	            
			// if we're dealing with any zero or one, the GCD is 1
			if (left < 2 || right < 2)
				return 1;

			do
			{

				if (left < right)
				{
					long temp = left;  // swap the two operands
					left = right;
					right = temp;
				}

				left %= right;
			} while (left != 0);

			return right;
		}

		#endregion

		#region Indeterminate helpers

		/// <summary>
		/// Gives the culture-related representation of the indeterminate types NaN, PositiveInfinity
		/// and NegativeInfinity
		/// </summary>
		/// <param name="numerator">The value in the numerator</param>
		/// <returns>The culture-specific string representation of the implied value</returns>
		/// <remarks>Only the sign and zero/non-zero information is relevant.</remarks>
		private static string IndeterminateTypeName(long numerator)
		{
			// could also be NumberFormatInfo.InvariantInfo
			System.Globalization.NumberFormatInfo info = NumberFormatInfo.CurrentInfo;

			switch (NormalizeIndeterminate(numerator))
			{

				case Indeterminates.PositiveInfinity:
					return info.PositiveInfinitySymbol;

				case Indeterminates.NegativeInfinity:
					return info.NegativeInfinitySymbol;

				default:    // if this happens, something VERY wrong is going on...

				case Indeterminates.NaN:
					return info.NaNSymbol;
			}
		}

		/// <summary>
		/// Gives the normalize representation of the indeterminate types NaN, PositiveInfinity
		/// and NegativeInfinity
		/// </summary>
		/// <param name="numerator">The value in the numerator</param>
		/// <returns>The normalized version of the indeterminate type</returns>
		/// <remarks>Only the sign and zero/non-zero information is relevant.</remarks>
		private static Indeterminates NormalizeIndeterminate(long numerator)
		{
						
			switch (Math.Sign(numerator))
			{

				case 1:
					return Indeterminates.PositiveInfinity;

				case -1:
					return Indeterminates.NegativeInfinity;

				default:    // if this happens, your Math.Sign function is BROKEN!

				case 0:
					return Indeterminates.NaN;
			}
		}

		// These are used to represent the indeterminate with a Denominator of zero
		private enum Indeterminates
		{
			NaN = 0
				, PositiveInfinity = 1
					  , NegativeInfinity = -1
		}

		#endregion

		#region Member variables

		private long m_Numerator;
		private long m_Denominator;

		#endregion
		#endregion

	}   //end class NuGenFraction

	/// <summary>
	/// Exception class for NuGenFraction, derived from System.Exception
	/// </summary>
	public class NuGenFractionException : Exception
	{

		/// <summary>
		/// Constructs a NuGenFractionException
		/// </summary>
		/// <param name="Message">String associated with the error message</param>
		/// <param name="InnerException">Actual inner exception caught</param>
		public NuGenFractionException(string Message, Exception InnerException) : base(Message, InnerException)
		{
			
		}
	}   //end class NuGenFractionException
}   //end namespace Mehroz

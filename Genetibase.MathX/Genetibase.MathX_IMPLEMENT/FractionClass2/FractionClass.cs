	/*
	* Author: Syed Mehroz Alam
	* Email: smehrozalam@yahoo.com
	* URL: Programming Home "http://www.geocities.com/smehrozalam/" 
	*
	*/

	using System;
	using System.Runtime.InteropServices;
	using System.Globalization;

	namespace Mehroz
	{
		/// <summary>
		/// Classes Contained:
		///     Fraction
		///     FractionException
		/// </summary>
	    
		/// Class name: Fraction
		/// Developed by: Syed Mehroz Alam
		/// Email: mailto:smehrozalam@yahoo.com
		/// URL: http://www.geocities.com/smehrozalam/
		/// Changes: Marc C. Brooks  mailto:IDisposable@gmail.com
		///          Jeffery Sax    http://www.extremeoptimization.com
		/// Version: 2.2
		/// 
		/// What's new in version 2.0:
		///     *   Changed Numerator and Denominator from Int32(integer) to Int64(long) for increased range
		///     *   renamed ConvertToString() to (overloaded) ToString()
		///     *   added the capability of detecting/raising overflow exceptions
		///     *   Fixed the bug that very small numbers e.g. 0.00000001 could not be converted to fraction
		///     *   Other minor bugs fixed
		/// 
		/// What's new in version 2.1
		///     *   overloaded user-defined conversions to/from Fractions
		///
		/// What's new in version 2.2 - Marc C. Brooks   mailto:IDisposable@gmail.com
		///     *   less overflows by finding the GCD for Add [Jeffery Sax] and Multiply [Marc C. Brooks]
		///     *   understands and handles NaN, PositiveInfinity, NegativeInfinity just like double [Marc C. Brooks]
		///     *   fixed several uses of int where long was correct [Marc C. Brooks]
		///     *   made value-type (struct) [Jeffery Sax]
		///     *   added ToInt32(), ToInt64() which throw for invalid (NaN, PositiveInfinity, NegativeInfinity) [Marc C. Brooks]
		///     *   removed redundant Value property [Jeffery Sax]
		///     *   added explicit conversion to Int32 and Int64 [Marc C. Brooks]
		///     *   better handling of exceptions [Marc C. Brooks]
		///     *   reorganize code, add XML doc and regions [Marc C. Brooks]
		///     *   proper implementations of Equals [Marc C. Brooks, Jeffery Sax]
		///     *   uses Math.Log(xx,2) and Math.Pow(xx,2) to get the best accuracy possible when converting doubles [Marc C. Brooks, Jeffery Sax]
		///
		/// What's new in version 2.3 - Marc C. Brooks   mailto:IDisposable@gmail.com   01/10/2005
		///     *   fixed double-to-fraction logic to use continued fraction rules to get best possible precistion [bug fix for Syed Mehroz Alam, idea from  Jeffery Sax]
		///     *	added static readonly values for NaN, PositiveInfinity, NegativeInfinity [idea from Jeffery Sax]
		///     *	moved comparisons into an implementation of IComparer [idea from Jeffery Sax]
		///     *	no longer throws for NaN(s) involved in Add, Subtract, Multiply, Divide operations [idea from Jeffery Sax]
		///     *   added static readonly values for Zero, MinValue, MaxValue, Epsilon to better mirror double
		///     *   added IsInfinity to better mirror double.
		///     *   added Modulus and % operators
		///
		/// Properties:
		///     Numerator: Set/Get value for Numerator
		///     Denominator:  Set/Get value for Numerator
		///     [Note: If you Set either Property, the Fraction should be passed to ReduceFraction at some point.]
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
		///     Fraction ToFraction(long)
		///     Fraction ToFraction(double)
		///     Fraction ToFraction(string)
		///     Int32 ToInt32()
		///     Int64 ToInt64()
		///     double ToDouble()
		///     (override) string ToString()
		///     Fraction Inverse()
		///     Fraction Inverted(long)
		///     Fraction Inverted(double)
		///     ReduceFraction(ref Fraction)
		///     CrossReducePair(ref Fraction, ref Fraction)
		///     (override) Equals(object)
		///     (override) GetHashCode()
		/// 
		/// Overloaded Operators (overloaded for Fractions, long and double)
		///     Unary: -
		///     Binary: +,-,*,/
		///     Relational and Logical Operators: ==,!=,<,>,<=,>= (only == and != for long and doubles)
		/// 
		/// Overloaded user-defined conversions
		///     Implicit:   From long/double/string to Fraction
		///     Explicit:   From Fraction to long/double/string
		/// </summary>
		[Serializable, StructLayout(LayoutKind.Sequential)]
		public struct Fraction : IComparable, IFormattable
		{
			#region Constructors
			/// <summary>
			/// Construct a Fraction from an integral value
			/// </summary>
			/// <param name="wholeNumber">The value (eventual numerator)</param>
			/// <remarks>The denominator will be 1</remarks>
			public Fraction(long wholeNumber)
			{
				if (wholeNumber == long.MinValue)
					wholeNumber++;	// prevent serious issues later..

				m_Numerator = wholeNumber;
				m_Denominator = 1;
				// no reducing required, we're a whole number
			}
	    
			/// <summary>
			/// Construct a Fraction from a floating-point value
			/// </summary>
			/// <param name="floatingPointNumber">The value</param>
			public Fraction(double floatingPointNumber)
			{
				this = ToFraction(floatingPointNumber);
			}
	        
			/// <summary>
			/// Construct a Fraction from a string in any legal format
			/// </summary>
			/// <param name="inValue">A string with a legal fraction input format</param>
			/// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
			/// <see>ToFraction(string strValue)</see>
			public Fraction(string inValue)
			{
				this = ToFraction(inValue);
			}
	        
			/// <summary>
			/// Construct a Fraction from a numerator, denominator pair
			/// </summary>
			/// <param name="numerator">The numerator (top number)</param>
			/// <param name="denominator">The denominator (bottom number)</param>
			/// <remarks>Will reduce the fraction to smallest possible denominator</remarks>
			public Fraction(long numerator, long denominator)
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
			/// Private constructor to synthesize a Fraction for indeterminates (NaN and infinites)
			/// </summary>
			/// <param name="type">Kind of inderterminate</param>
			private Fraction(Indeterminates type)
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
			public static readonly Fraction NaN = new Fraction(Indeterminates.NaN);
			public static readonly Fraction PositiveInfinity = new Fraction(Indeterminates.PositiveInfinity);
			public static readonly Fraction NegativeInfinity = new Fraction(Indeterminates.NegativeInfinity);
			public static readonly Fraction Zero = new Fraction(0,1);
			public static readonly Fraction Epsilon = new Fraction(1, Int64.MaxValue);
			private static readonly double EpsilonDouble = 1.0 / Int64.MaxValue;
			public static readonly Fraction MaxValue = new Fraction(Int64.MaxValue, 1);
			public static readonly Fraction MinValue = new Fraction(Int64.MinValue, 1);
			#endregion

			#region Explicit conversions
			#region To primitives
			/// <summary>
			/// Get the integral value of the Fraction object as int/Int32
			/// </summary>
			/// <returns>The (approximate) integer value</returns>
			/// <remarks>If the value is not a true integer, the fractional part is discarded
			/// (truncated toward zero). If the valid exceeds the range of an Int32 and exception is thrown.</remarks>
			/// <exception cref="FractionException">Will throw a FractionException for NaN, PositiveInfinity
			/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.</exception>
			/// <exception cref="OverflowException" Will throw a System.OverflowException if the value is too
			/// large or small to be represented as an Int32.</exception>
			public Int32 ToInt32()
			{
				if (this.m_Denominator == 0)
				{
					throw new FractionException(string.Format("Cannot convert {0} to Int32", IndeterminateTypeName(this.m_Numerator)), new System.NotFiniteNumberException());
				}

				long bestGuess = this.m_Numerator / this.m_Denominator;

				if (bestGuess > Int32.MaxValue || bestGuess < Int32.MinValue)
				{
					throw new FractionException("Cannot convert to Int32", new System.OverflowException());
				}

				return (Int32)bestGuess;
			}

			/// <summary>
			/// Get the integral value of the Fraction object as long/Int64
			/// </summary>
			/// <returns>The (approximate) integer value</returns>
			/// <remarks>If the value is not a true integer, the fractional part is discarded
			/// (truncated toward zero). If the valid exceeds the range of an Int32, no special
			/// handling is guaranteed.</remarks>
			/// <exception cref="FractionException">Will throw a FractionException for NaN, PositiveInfinity
			/// or NegativeInfinity with the InnerException set to a System.NotFiniteNumberException.</exception>
			public Int64 ToInt64()
			{
				if (this.m_Denominator == 0)
				{
					throw new FractionException(string.Format("Cannot convert {0} to Int64", IndeterminateTypeName(this.m_Numerator)), new System.NotFiniteNumberException());
				}

				return this.m_Numerator / this.m_Denominator;
			}

			/// <summary>
			/// Get the value of the Fraction object as double with full support for NaNs and infinities
			/// </summary>
			/// <returns>The decimal representation of the Fraction, or double.NaN, double.NegativeInfinity
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
			/// Get the value of the Fraction as a string, with proper representation for NaNs and infinites
			/// </summary>
			/// <returns>The string representation of the Fraction, or the culture-specific representations of
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
			/// Converts a long value to the exact Fraction
			/// </summary>
			/// <param name="inValue">The long to convert</param>
			/// <returns>An exact representation of the value</returns>
			public static Fraction ToFraction(long inValue)
			{
				return new Fraction(inValue);
			}

			/// <summary>
			/// Converts a double value to the approximate Fraction
			/// </summary>
			/// <param name="inValue">The double to convert</param>
			/// <returns>A best-fit representation of the value</returns>
			/// <remarks>Supports double.NaN, double.PositiveInfinity and double.NegativeInfinity</remarks>
			public static Fraction ToFraction(double inValue)
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
			/// <returns>The Fraction that represents the string</returns>
			/// <remarks>Four forms are supported, as a plain integer, as a double, or as Numerator/Denominator
			/// and the representations for NaN and the infinites</remarks>
			/// <example>"123" = 123/1 and "1.25" = 5/4 and "10/36" = 5/13 and NaN = 0/0 and
			/// PositiveInfinity = 1/0 and NegativeInfinity = -1/0</example>
			public static Fraction ToFraction(string inValue)
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
					// Not special, is it a Fraction?
					int slashPos = inValue.IndexOf('/');

					if (slashPos > -1)
					{
						// string is in the form of Numerator/Denominator
						long numerator = Convert.ToInt64(inValue.Substring(0, slashPos));
						long denominator = Convert.ToInt64(inValue.Substring(slashPos + 1));

						return new Fraction(numerator, denominator);
					}
					else
					{
						// the string is not in the form of a fraction
						// hopefully it is double or integer, do we see a decimal point?
						int decimalPos = inValue.IndexOf(info.CurrencyDecimalSeparator);

						if (decimalPos > -1)
							return new Fraction(Convert.ToDouble(inValue));
						else
							return new Fraction(Convert.ToInt64(inValue));
					}
				}
			}
			#endregion
			#endregion

			#region Indeterminate classifications
			/// <summary>
			/// Determines if a Fraction represents a Not-a-Number
			/// </summary>
			/// <returns>True if the Fraction is a NaN</returns>
			public bool IsNaN()
			{
				if (this.m_Denominator == 0 
					&& NormalizeIndeterminate(this.m_Numerator) == Indeterminates.NaN)
					return true;
				else
					return false;
			}

			/// <summary>
			/// Determines if a Fraction represents Any Infinity
			/// </summary>
			/// <returns>True if the Fraction is Positive Infinity or Negative Infinity</returns>
			public bool IsInfinity()
			{
				if (this.m_Denominator == 0
					&& NormalizeIndeterminate(this.m_Numerator) != Indeterminates.NaN)
					return true;
				else
					return false;
			}

			/// <summary>
			/// Determines if a Fraction represents Positive Infinity
			/// </summary>
			/// <returns>True if the Fraction is Positive Infinity</returns>
			public bool IsPositiveInfinity()
			{
				if (this.m_Denominator == 0
					&& NormalizeIndeterminate(this.m_Numerator) == Indeterminates.PositiveInfinity)
					return true;
				else
					return false;
			}

			/// <summary>
			/// Determines if a Fraction represents Negative Infinity
			/// </summary>
			/// <returns>True if the Fraction is Negative Infinity</returns>
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
			/// Inverts a Fraction
			/// </summary>
			/// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
			/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
			public Fraction Inverse()
			{
				// don't use the obvious constructor because we do not want it normalized at this time
				Fraction frac = new Fraction();

				frac.m_Numerator = this.m_Denominator;
				frac.m_Denominator = this.m_Numerator;
				return frac;
			}

			/// <summary>
			/// Creates an inverted Fraction
			/// </summary>
			/// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
			/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
			public static Fraction Inverted(long value)
			{
				Fraction frac = new Fraction(value);
				return frac.Inverse();
			}

			/// <summary>
			/// Creates an inverted Fraction
			/// </summary>
			/// <returns>The inverted Fraction (with Denominator over Numerator)</returns>
			/// <remarks>Does NOT throw for zero Numerators as later use of the fraction will catch the error.</remarks>
			public static Fraction Inverted(double value)
			{
				Fraction frac = new Fraction(value);
				return frac.Inverse();
			}
			#endregion

			#region Operators
			#region Unary Negation operator
			/// <summary>
			/// Negates the Fraction
			/// </summary>
			/// <param name="left">The Fraction to negate</param>
			/// <returns>The negative version of the Fraction</returns>
			public static Fraction operator -(Fraction left)
			{
				return Negate(left);
			}
			#endregion

			#region Addition operators
			public static Fraction operator +(Fraction left, Fraction right)
			{
				return Add(left, right);
			}
	    
			public static Fraction operator +(long left, Fraction right)
			{
				return Add(new Fraction(left), right);
			}
	    
			public static Fraction operator +(Fraction left, long right)
			{
				return Add(left, new Fraction(right));
			}

			public static Fraction operator +(double left, Fraction right)
			{
				return Add(ToFraction(left), right);
			}
	    
			public static Fraction operator +(Fraction left, double right)
			{
				return Add(left, ToFraction(right));
			}
			#endregion

			#region Subtraction operators
			public static Fraction operator -(Fraction left, Fraction right)
			{
				return Add(left, - right);
			}
	    
			public static Fraction operator -(long left, Fraction right)
			{
				return Add(new Fraction(left), - right);
			}
	    
			public static Fraction operator -(Fraction left, long right)
			{
				return Add(left, new Fraction(- right));
			}

			public static Fraction operator -(double left, Fraction right)
			{
				return Add(ToFraction(left), - right);
			}
	    
			public static Fraction operator -(Fraction left, double right)
			{
				return Add(left, ToFraction(- right));
			}
			#endregion

			#region Multiplication operators
			public static Fraction operator *(Fraction left, Fraction right)
			{
				return Multiply(left, right);
			}
	    
			public static Fraction operator *(long left, Fraction right)
			{
				return Multiply(new Fraction(left), right);
			}
	    
			public static Fraction operator *(Fraction left, long right)
			{
				return Multiply(left, new Fraction(right));
			}
	    
			public static Fraction operator *(double left, Fraction right)
			{
				return Multiply(ToFraction(left), right);
			}
	    
			public static Fraction operator *(Fraction left, double right)
			{
				return Multiply(left, ToFraction(right));
			}
			#endregion

			#region Division operators
			public static Fraction operator /(Fraction left, Fraction right)
			{
				return Multiply(left, right.Inverse());
			}
	    
			public static Fraction operator /(long left, Fraction right)
			{
				return Multiply(new Fraction(left), right.Inverse());
			}
	    
			public static Fraction operator /(Fraction left, long right)
			{
				return Multiply(left, Inverted(right));
			}
	    
			public static Fraction operator /(double left, Fraction right)
			{
				return Multiply(ToFraction(left), right.Inverse());
			}
	    
			public static Fraction operator /(Fraction left, double right)
			{
				return Multiply(left, Inverted(right));
			}
			#endregion

			#region Modulus operators
			public static Fraction operator %(Fraction left, Fraction right)
			{
				return Modulus(left, right);
			}
	    
			public static Fraction operator %(long left, Fraction right)
			{
				return Modulus(new Fraction(left), right);
			}
	    
			public static Fraction operator %(Fraction left, long right)
			{
				return Modulus(left, right);
			}
	    
			public static Fraction operator %(double left, Fraction right)
			{
				return Modulus(ToFraction(left), right);
			}
	    
			public static Fraction operator %(Fraction left, double right)
			{
				return Modulus(left, right);
			}
			#endregion

			#region Equal operators
			public static bool operator ==(Fraction left, Fraction right)
			{
				return left.CompareEquality(right, false);
			}

			public static bool operator ==(Fraction left, long right)
			{
				return left.CompareEquality(new Fraction(right), false);
			}

			public static bool operator ==(Fraction left, double right)
			{
				return left.CompareEquality(new Fraction(right), false);
			}
			#endregion

			#region Not-equal operators
			public static bool operator !=(Fraction left, Fraction right)
			{
				return left.CompareEquality(right, true);
			}

			public static bool operator !=(Fraction left, long right)
			{
				return left.CompareEquality(new Fraction(right), true);
			}
	        
			public static bool operator !=(Fraction left, double right)
			{
				return left.CompareEquality(new Fraction(right), true);
			}
			#endregion

			#region Inequality operators
			/// <summary>
			/// Compares two Fractions to see if left is less than right
			/// </summary>
			/// <param name="left">The first Fraction</param>
			/// <param name="right">The second Fraction</param>
			/// <returns>True if <paramref name="left">left</paramref> is less
			/// than <paramref name="right">right</paramref></returns>
			/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
			/// <exception cref="FractionException">Throws an error if overflows occur while computing the 
			/// difference with an InnerException of OverflowException</exception>
			public static bool operator <(Fraction left, Fraction right)
			{
				return left.CompareTo(right) < 0;
			}

			/// <summary>
			/// Compares two Fractions to see if left is greater than right
			/// </summary>
			/// <param name="left">The first Fraction</param>
			/// <param name="right">The second Fraction</param>
			/// <returns>True if <paramref name="left">left</paramref> is greater
			/// than <paramref name="right">right</paramref></returns>
			/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLess</see></remarks>
			/// <exception cref="FractionException">Throws an error if overflows occur while computing the 
			/// difference with an InnerException of OverflowException</exception>
			public static bool operator >(Fraction left, Fraction right)
			{
				return left.CompareTo(right) > 0;
			}

			/// <summary>
			/// Compares two Fractions to see if left is less than or equal to right
			/// </summary>
			/// <param name="left">The first Fraction</param>
			/// <param name="right">The second Fraction</param>
			/// <returns>True if <paramref name="left">left</paramref> is less than or 
			/// equal to <paramref name="right">right</paramref></returns>
			/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
			/// <exception cref="FractionException">Throws an error if overflows occur while computing the 
			/// difference with an InnerException of OverflowException</exception>
			public static bool operator <=(Fraction left, Fraction right)
			{
				return left.CompareTo(right) <= 0;
			}
	        
			/// <summary>
			/// Compares two Fractions to see if left is greater than or equal to right
			/// </summary>
			/// <param name="left">The first Fraction</param>
			/// <param name="right">The second Fraction</param>
			/// <returns>True if <paramref name="left">left</paramref> is greater than or 
			/// equal to <paramref name="right">right</paramref></returns>
			/// <remarks>Special handling for indeterminates exists. <see>IndeterminateLessEqual</see></remarks>
			/// <exception cref="FractionException">Throws an error if overflows occur while computing the 
			/// difference with an InnerException of OverflowException</exception>
			public static bool operator >=(Fraction left, Fraction right)
			{
				return left.CompareTo(right) >= 0;
			}
			#endregion

			#region Implict conversion from primitive operators
			/// <summary>
			/// Implicit conversion of a long integral value to a Fraction
			/// </summary>
			/// <param name="value">The long integral value to convert</param>
			/// <returns>A Fraction whose denominator is 1</returns>
			public static implicit operator Fraction(long value)
			{
				return new Fraction(value);
			}

			/// <summary>
			/// Implicit conversion of a double floating point value to a Fraction
			/// </summary>
			/// <param name="value">The double value to convert</param>
			/// <returns>A reduced Fraction</returns>
			public static implicit operator Fraction(double value)
			{
				return new Fraction(value);
			}

			/// <summary>
			/// Implicit conversion of a string to a Fraction
			/// </summary>
			/// <param name="value">The string to convert</param>
			/// <returns>A reduced Fraction</returns>
			public static implicit operator Fraction(string value)
			{
				return new Fraction(value);
			}
			#endregion

			#region Explicit converstion to primitive operators
			/// <summary>
			/// Explicit conversion from a Fraction to an integer
			/// </summary>
			/// <param name="frac">the Fraction to convert</param>
			/// <returns>The integral representation of the Fraction</returns>
			public static explicit operator int(Fraction frac)
			{
				return frac.ToInt32();
			}

			/// <summary>
			/// Explicit conversion from a Fraction to an integer
			/// </summary>
			/// <param name="frac">The Fraction to convert</param>
			/// <returns>The integral representation of the Fraction</returns>
			public static explicit operator long(Fraction frac)
			{
				return frac.ToInt64();
			}

			/// <summary>
			/// Explicit conversion from a Fraction to a double floating-point value
			/// </summary>
			/// <param name="frac">The Fraction to convert</param>
			/// <returns>The double representation of the Fraction</returns>
			public static explicit operator double(Fraction frac)
			{
				return frac.ToDouble();
			}

			/// <summary>
			/// Explicit conversion from a Fraction to a string
			/// </summary>
			/// <param name="frac">the Fraction to convert</param>
			/// <returns>The string representation of the Fraction</returns>
			public static implicit operator string(Fraction frac)
			{
				return frac.ToString();
			}
			#endregion
			#endregion

			#region Equals and GetHashCode overrides
			/// <summary>
			/// Compares for equality the current Fraction to the value passed.
			/// </summary>
			/// <param name="obj">A  Fraction,</param>
			/// <returns>True if the value equals the current fraction, false otherwise (including for
			/// non-Fraction types or null object.</returns>
			public override bool Equals(object obj)
			{
				if (obj == null	|| ! (obj is Fraction))
					return false;

				try 
				{
					Fraction right = (Fraction)obj;
					return this.CompareEquality(right, false);
				}
				catch
				{
					// can't throw in an Equals!
					return false;
				}
			}
	        
			/// <summary>
			/// Returns a hash code generated from the current Fraction
			/// </summary>
			/// <returns>The hash code</returns>
			/// <remarks>Reduces (in-place) the Fraction first.</remarks>
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
			/// Compares an object to this Fraction
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

				Fraction right;

				if (obj is Fraction)
					right = (Fraction)obj;
				else if (obj is long)
					right = (long)obj;
				else if (obj is double)
					right = (double)obj;
				else if (obj is string)
					right = (string)obj;
				else
					throw new ArgumentException("Must be convertible to Fraction", "obj");

				return this.CompareTo(right);
			}

			/// <summary>
			/// Compares this Fraction to another Fraction
			/// </summary>
			/// <param name="right">The Fraction to compare against</param>
			/// <returns>-1 if this is less than <paramref name="right"></paramref>,
			///  0 if they are equal,
			///  1 if this is greater than <paramref name="right"></paramref></returns>
			public int CompareTo(Fraction right)
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
					throw new FractionException(string.Format("CompareTo({0}, {1}) error", this, right), e);
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
			/// Reduces (simplifies) a Fraction by dividing down to lowest possible denominator (via GCD)
			/// </summary>
			/// <param name="frac">The Fraction to be reduced [WILL BE MODIFIED IN PLACE]</param>
			/// <remarks>Modifies the input arguments in-place! Will normalize the NaN and infinites
			/// representation. Will set Denominator to 1 for any zero numerator. Moves sign to the
			/// Numerator.</remarks>
			/// <example>2/4 will be reduced to 1/2</example>
			public static void ReduceFraction(ref Fraction frac)
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
			/// <param name="frac1">The first Fraction [WILL BE MODIFIED IN PLACE]</param>
			/// <param name="frac2">The second Fraction [WILL BE MODIFIED IN PLACE]</param>
			/// <remarks>Modifies the input arguments in-place!</remarks>
			/// <example>(3/4, 5/9) = (1/4, 5/3)</example>
			public static void CrossReducePair(ref Fraction frac1, ref Fraction frac2)
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
			private static Fraction ConvertPositiveDouble(int sign, double inValue)
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

				return new Fraction(fractionNumerator * sign, (long)fractionDenominator);
			}
			#endregion

			#region Equality helper
			/// <summary>
			/// Compares for equality the current Fraction to the value passed.
			/// </summary>
			/// <param name="right">A Fraction to compare against</param>
			/// <param name="notEqualCheck">If true, we're looking for not-equal</param>
			/// <returns>True if the <paramref name="right"></paramref> equals the current 
			/// fraction, false otherwise. If comparing two NaNs, they are always equal AND
			/// not-equal.</returns>
			private bool CompareEquality(Fraction right, bool notEqualCheck)
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
			/// Determines how this Fraction, of an indeterminate type, compares to another Fraction
			/// </summary>
			/// <param name="leftType">What kind of indeterminate</param>
			/// <param name="right">The other Fraction to compare against</param>
			/// <returns>-1 if this is less than <paramref name="right"></paramref>,
			///  0 if they are equal,
			///  1 if this is greater than <paramref name="right"></paramref></returns>
			/// <remarks>NaN is less than anything except NaN and Negative Infinity. Negative Infinity is less
			/// than anything except Negative Infinity. Positive Infinity is greater than anything except
			/// Positive Infinity.</remarks>
			private static int IndeterminantCompare(Indeterminates leftType, Fraction right)
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
			/// Negates the Fraction
			/// </summary>
			/// <param name="frac">Value to negate</param>
			/// <returns>A new Fraction that is sign-flipped from the input</returns>
			private static Fraction Negate(Fraction frac)
			{
				// for a NaN, it's still a NaN
				return new Fraction( - frac.m_Numerator, frac.m_Denominator);
			}

			/// <summary>
			/// Adds two Fractions
			/// </summary>
			/// <param name="left">A Fraction</param>
			/// <param name="right">Another Fraction</param>
			/// <returns>Sum of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
			/// <exception cref="FractionException">Will throw if an overflow occurs when computing the
			/// GCD-normalized values.</exception>
			private static Fraction Add(Fraction left, Fraction right)
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

						return new Fraction(numerator, denominator);
					}
				}
				catch (Exception e)
				{
					throw new FractionException("Add error", e);
				}
			}
	    
			/// <summary>
			/// Multiplies two Fractions
			/// </summary>
			/// <param name="left">A Fraction</param>
			/// <param name="right">Another Fraction</param>
			/// <returns>Product of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
			/// <exception cref="FractionException">Will throw if an overflow occurs. Does a cross-reduce to 
			/// insure only the unavoidable overflows occur.</exception>
			private static Fraction Multiply(Fraction left, Fraction right)
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

						return new Fraction(numerator, denominator);
					}
				}
				catch (Exception e)
				{
					throw new FractionException("Multiply error", e);
				}
			}

			/// <summary>
			/// Returns the modulus (remainder after dividing) two Fractions
			/// </summary>
			/// <param name="left">A Fraction</param>
			/// <param name="right">Another Fraction</param>
			/// <returns>Modulus of the Fractions. Returns NaN if either Fraction is a NaN.</returns>
			/// <exception cref="FractionException">Will throw if an overflow occurs. Does a cross-reduce to 
			/// insure only the unavoidable overflows occur.</exception>
			private static Fraction Modulus(Fraction left, Fraction right)
			{
				if (left.IsNaN() || right.IsNaN())
					return NaN;

				try
				{
					checked
					{
						// this will discard any fractional places...
						Int64 quotient = (Int64)(left / right);
						Fraction whole = new Fraction(quotient * right.m_Numerator, right.m_Denominator);
						return left - whole;
					}
				}
				catch (Exception e)
				{
					throw new FractionException("Modulus error", e);
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
		}   //end class Fraction

		/// <summary>
		/// Exception class for Fraction, derived from System.Exception
		/// </summary>
		public class FractionException : Exception
		{
			/// <summary>
			/// Constructs a FractionException
			/// </summary>
			/// <param name="Message">String associated with the error message</param>
			/// <param name="InnerException">Actual inner exception caught</param>
			public FractionException(string Message, Exception InnerException) : base(Message, InnerException)
			{
			}
		}   //end class FractionException
	}   //end namespace Mehroz

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
#endregion

namespace Geotools.Index.Quadtree
{
	/// <summary>
	/// DoubleBits manipulates Double numbers.
	/// </summary>
	/// <remarks>
	/// DoubleBits manipulates Double numbers
	/// by using bit manipulation and bit-field extraction.
	/// For some operations (such as determining the exponent)
	/// this is more accurate than using mathematical operations
	/// which suffer from round-off error)
	/// The algorithms and constants in this class
	/// apply only to IEEE-754 double-precision floating point format.
	/// </remarks>
	internal class DoubleBits
	{
		private double _x;
		private long _xBits;

		#region Constructors
		public DoubleBits()
		{
		}
		public DoubleBits( double x )
		{
			_x = x;
			_xBits = DoubleToLongBits(x);
		}
		#endregion

		#region Methods
		public double GetDouble()
		{
			return LongBitsToDouble( _xBits );
			
		}
	
		/// <summary>
		/// Determines the exponent for the number.
		/// </summary>
		/// <returns></returns>
		public int BiasedExponent()
		{
			int signExp = (int) ( _xBits >> 52 );
			int exp = signExp & 0x07ff;
			return exp;
		}


		/// <summary>
		/// Determines the exponent for the number.
		/// </summary>
		/// <returns></returns>
		public int GetExponent()
		{
			return BiasedExponent() - Convert.ToInt32( ExponentBias );
		}

		public void ZeroLowerBits( int nBits )
		{
			long invMask = (1L << nBits) - 1L;
			long mask = ~ invMask;
			_xBits &= mask;
		}

		public int GetBit( int i )
		{
			long mask = (1L << i);
			return (_xBits & mask) != 0 ? 1 : 0;
		}
			  
		/// <summary>
		/// This computes the number of common most-significan bits in the mantissa.
		/// It does not count the hidden bit, which is alway 1.  It does not determine whether the
		/// numbers have the same exponent - if they do not, the value computed by this function
		/// is meaningless.
		/// </summary>
		/// <param name="db"></param>
		/// <returns>Returns the number of common most-significant mantissa bits.</returns>
		public int NumCommonMantissaBits( DoubleBits db )
		{
			for (int i = 0; i < 52; i++)
			{
				int bitIndex = i + 12;
				if ( GetBit(i) != db.GetBit(i) )
					return i;
			}
			return 52;
		}


		/// <summary>
		/// A representation of the Double bits formatted for easy readability.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			// need this??awccc
			/*string numStr = (Convert.ToByte( _xBits )).ToString();
			// 64 zeroes!
			string zero64 = "0000000000000000000000000000000000000000000000000000000000000000";
			string padStr =  zero64 + numStr;
			string bitStr = padStr.substring(padStr.length() - 64);
			string str = bitStr.Substring(0, 1) + "  "
				+ bitStr.substring(1, 12) + "(" + GetExponent() + ") "
				+ bitStr.substring(12)
				+ " [ " + x + " ]";
			*/
			return _xBits.ToString();
		}

		#endregion

		#region Static Methods
		public static double ExponentBias
		{
			get
			{
				return 1023;
			}
		}
		public static double PowerOf2(int exp)
		{
			if (exp > 1023 || exp < -1022)
			{
				throw new ArgumentOutOfRangeException("Exponent out of bounds");
			}
			long expBias = exp + Convert.ToInt64( ExponentBias );
			long bits = (long) expBias << 52;
			return LongBitsToDouble(bits);
		}

		public static int Exponent(double d)
		{
			DoubleBits db = new DoubleBits(d);
			return db.GetExponent();
		}

		public static double TruncateToPowerOfTwo(double d)
		{
			DoubleBits db = new DoubleBits(d);
			db.ZeroLowerBits(52);
			return db.GetDouble();
		}

		public static String ToBinaryString(double d)
		{
			DoubleBits db = new DoubleBits(d);
			return db.ToString();
		}

		public static double MaximumCommonMantissa(double d1, double d2)
		{
			if (d1 == 0.0 || d2 == 0.0) return 0.0;

			DoubleBits db1 = new DoubleBits(d1);
			DoubleBits db2 = new DoubleBits(d2);

			if (db1.GetExponent() != db2.GetExponent()) return 0.0;

			int maxCommon = db1.NumCommonMantissaBits(db2);
			db1.ZeroLowerBits(64 - (12 + maxCommon));
			return db1.GetDouble();
		}
		/// <summary>
		/// Simulation of the Java function
		/// </summary>
		/// <remarks>
		/// http://java.sun.com/products/jdk/1.1/docs/api/java.lang.Double.html#longBitsToDouble(long)
		/// 
		/// </remarks>
		/// <param name="bits"></param>
		/// <returns></returns>
		public static double LongBitsToDouble(long bits)
		{
			return BitConverter.Int64BitsToDouble(bits);
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// http://java.sun.com/products/jdk/1.1/docs/api/java.lang.Double.html#longBitsToDouble(long)
		/// </remarks>
		/// <param name="number"></param>
		/// <returns></returns>
		public static long DoubleToLongBits(double number)
		{
			return BitConverter.DoubleToInt64Bits(number);
		}

		#endregion

	}
}

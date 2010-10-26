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
	
	/**
	 * <code>BinaryPower</code> computes exponents and powers of 2.
	 * It uses algorithsm which use mathematical operations
	 * rather than bit manipulation, and as such is vulnerable
	 * to errors due to round-off error.
	 * It is preferable to use the <code>DoubleBits</code> class.
	 */

	//<<TODO:REFACTOR?>> If it is preferable to use DoubleBits, should we not delete
	//this class? [Jon Aquino]

	internal class BinaryPower
	{
		private static double LOG_2 = Math.Log(2.0);
		
	

		/// <summary>
		/// Computes the exponent for the largest power of two
		/// less than the absolute value of the argument.
		/// In other words, finds the value n such that
		/// 2&#136;n &lt;= abs(num) &lt; 2&#136;(n+1) 
		/// </summary>
		/// <param name="num"></param>
		/// <returns>The exponent.</returns>
		public static int Exponent(double num)
		{
			num = Math.Abs(num);
			double log = Math.Log(num);
			double log2 = log / LOG_2;
			int exp = (int) Math.Floor(log2);
			
			int exp2 = DoubleBits.Exponent(num);
			if (exp != exp2) 
			{
				//System.out.println(DoubleBits.toBinaryString(num));
				//System.out.println(num + " pow2 mismatch: " + exp + "   DoubleBits: " + exp2);
				double pow2exp = DoubleBits.PowerOf2(exp);
				double pow2exp2 = DoubleBits.PowerOf2(exp2);
				//System.out.println(pow2exp + "   pow2exp2 = " + pow2exp2);
			}
			return exp;
		}

		/**
		 * This value indicates the expected range of requests
		 */
		private static int MAX_POWER = 100;

		private double[] _pow2Pos = new double[MAX_POWER];
		private double[] _pow2Neg = new double[MAX_POWER];

		public BinaryPower()
		{
			Init();
		}

		/**
		 * initialize the cache of powers
		 */
		private void Init()
		{
			double posPow2 = 1.0;
			double negPow2 = 1.0;
			for (int i = 0; i < MAX_POWER; i++) 
			{
				_pow2Pos[i] = posPow2;
				posPow2 *= 2.0;

				_pow2Neg[i] = negPow2;
				negPow2 /= 2.0;
			}
		}

		/**
		 * Computes the signed largest power of two
		 * less than the absolute value of the argument.
		 * In other words, finds the value
		 *
		 * sgn(n) * 2^n
		 *
		 * such that
		 *
		 * 2^n <= abs(num) < 2^(n+1)
		 *
		 * @return the power of two
		 */
		public double Power(int exp)
		{
			double pow;
			if (exp >= MAX_POWER || exp <= -MAX_POWER)
				pow = Math.Pow(2.0, (double) exp);
			// use the precomputed values
			if (exp >= 0)
				pow = _pow2Pos[exp];
			else
				pow = _pow2Neg[-exp];
			//if (pow != DoubleBits.powerOf2(exp))
			//	System.out.println("pow2 " + exp + " mismatch: " + pow + " DoubleBits: " + DoubleBits.powerOf2(exp));
			return pow;
		}

	}
}

#region MathNet Numerics, Copyright �2006 Christoph Ruegg

// MathNet Numerics, part of MathNet
//
// Copyright (c) 2006,	Christoph R�egg, http://www.cdrnet.net
// 
// Partially inpired by
//   - Microsoft MSR F#
//   - Modern Computer Algebra 2nd Ed, Gathen J. et al, ISBN 0-521-82646-2
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.

#endregion

/*
 * TODO:
 *  - DivMod
 *  - ToString
 *  - Parse/FromString
 *  - Unit Test
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Numerics
{
    /// <summary>
    /// [STUB] The mathematical set of natural numbers (including zero), supporting an arbitrary number of digits.
    /// </summary>
    public class Natural : IEquatable<Natural>, IComparable<Natural>
    {
        private const byte _radixBits = 32;
        private const ulong _radix = ((ulong)1) << _radixBits;
        private const uint _bound32 = 1; //(uint)Math.Ceiling(32d / _radixBits);
        private const uint _bound64 = 2; //(uint)Math.Ceiling(64d / _radixBits);

        private uint _bound;
        private uint[] _coeff;

        private Natural(uint bound)
        {
            //_bound = 0; //bound;
            _coeff = new uint[bound];
        }
        private Natural(uint[] coeff)
        {
            _coeff = coeff;
            _bound = (uint)coeff.LongLength;
            Normalize();
        }
        private Natural(uint bound, uint[] coeff)
        {
            _coeff = coeff;
            _bound = Math.Min(bound, (uint)coeff.LongLength);
            Normalize();
        }

        private uint this[uint idx]
        {
            get { return idx < _bound ? _coeff[idx] : 0; }
            set
            {
                if(idx >= _bound)
                {
                    if(value == 0)
                        return;
                    ExtendCapacity(idx + 1);
                    _bound = idx + 1;
                }
                _coeff[idx] = value;
            }
        }

        /// <summary>
        /// The biggest <seealso cref="Radix"/> exponent with a non-zero coefficient.
        /// </summary>
        [CLSCompliant(false)]
        public uint Degree
        {
            get { return _bound - 1; }
        }

        /// <summary>
        /// The number system/base of this number.
        /// </summary>
        [CLSCompliant(false)]
        public ulong Radix
        {
            get { return _radix; }
        }

        /// <summary>
        /// Normalizes the bound to get rid of leading zeros.
        /// </summary>
        private void Normalize()
        {
            for(uint i = _bound - 1; i >= 0; i--)
                if(_coeff[i] != 0)
                {
                    _bound = i + 1;
                    return;
                }
            _bound = 0;
        }

        /// <summary>
        /// Resizes the coefficient array if required.
        /// </summary>
        /// <param name="requiredBound">The bound that has to be supported.</param>
        private void ExtendCapacity(uint requiredBound)
        {
            if(requiredBound > _coeff.LongLength)
            {
                uint[] nc = new uint[requiredBound];
                for(uint i = 0; i < _bound; i++)
                    nc[i] = _coeff[i];
                _coeff = nc;
            }
        }

        #region Iterators
        #endregion

        #region Conversion
        [CLSCompliant(false)]
        public static Natural From(ulong number)
        {
            Natural n = new Natural(_bound64);
            n.AddCoefficientInplace(number, 0);
            return n;
        }
        [CLSCompliant(false)]
        public static Natural From(uint number)
        {
            Natural n = new Natural(_bound32);
            n.AddCoefficientInplace(number, 0);
            return n;
        }
        #endregion
        #region Constants
        public static Natural Zero
        {
            get { return new Natural(0); } //TODO: cache
        }
        public static Natural One
        {
            get { return From(1); } //TODO: cache
        }
        public static Natural Two
        {
            get { return From(2); } //TODO: cache
        }
        #endregion

        public bool IsZero
        {
            get { return _bound == 0; }
        }

        #region Addition
        public static Natural operator +(Natural a, Natural b)
        {
            return a.Add(b);
        }

        public Natural Add(Natural number)
        {
            return Add(number, 0);
        }
        [CLSCompliant(false)]
        public Natural Add(Natural number, uint carry)
        {
            uint len = 1 + Math.Max(_bound, number._bound);
            Natural ret = new Natural(len);
            for(uint i = 0; i < len; i++) //include len-1, where a[i]=b[i]=0 but carry may be > 0
            {
                ulong sum = (ulong)this[i] + number[i] + carry;
                carry = 0;
                while(sum >= _radix)
                {
                    sum -= _radix;
                    carry++;
                }
                ret[i] = (uint)sum;
            }
            ret.Normalize();
            return ret;
        }

        private void AddCoefficientInplace(ulong coeff, uint exponent)
        {
            long sum = (long)_coeff[exponent] + (long)coeff;
            while(sum >= (long)_radix)
            {
                long r;
                sum = _coeff[exponent + 1] + Math.DivRem(sum, (long)_radix, out r);
                _coeff[exponent++] = (uint)r;
            }
            _coeff[exponent] = (uint)sum;
            Normalize();
        }
        #endregion
        #region Subtraction
        public static Natural operator -(Natural a, Natural b)
        {
            return a.Subtract(b);
        }

        public Natural Subtract(Natural number)
        {
            bool underflow;
            return Subtract(number, 0, out underflow);
        }
        public Natural Subtract(Natural number, out bool underflow)
        {
            return Subtract(number, 0, out underflow);
        }
        [CLSCompliant(false)]
        public Natural Subtract(Natural number, uint carry, out bool underflow)
        {
            uint len = Math.Max(_bound, number._bound);
            Natural ret = new Natural(len);
            for(uint i = 0; i < len; i++)
            {
                long sum = (long)this[i] - number[i] - carry;
                carry = 0;
                while(sum < 0)
                {
                    sum += (long)_radix;
                    carry++;
                }
                ret[i] = (uint)sum;
            }
            if(carry != 0)
            {
                underflow = true;
                return Zero;
            }
            underflow = false;
            ret.Normalize();
            return ret;
        }

        private void SubtractCoefficientInplace(ulong coeff, uint exponent)
        {
            bool underflow;
            SubtractCoefficientInplace(coeff, exponent, out underflow);
        }
        private void SubtractCoefficientInplace(ulong coeff, uint exponent, out bool underflow)
        {
            long sum = (long)_coeff[exponent] - (long)coeff;
            while(sum < 0)
            {
                if(exponent >= _bound) // underflow
                {
                    _bound = 0; // set value to zero
                    underflow = true;
                    return;
                }
                long r;
                sum = _coeff[exponent + 1] - Math.DivRem(-sum, (long)_radix, out r);
                _coeff[exponent++] = (r == 0) ? 0 : (uint)(_radix-(ulong)r);
            }
            _coeff[exponent] = (uint)sum;
            underflow = false;
            Normalize();
        }
        #endregion
        #region Shifting
        /// <summary>
        /// Multiplies this number with the <seealso cref="Radix"/> to the power of the given exponent (fast shifting operation)
        /// </summary>
        private Natural ShiftUp(uint exponent)
        {
            uint len = _bound + exponent;
            Natural ret = new Natural(len);
            for(uint i = 0; i < _bound; i++)
                ret._coeff[i + exponent] = _coeff[i];
            return ret;
        }

        /// <summary>
        /// Divides this number by the <seealso cref="Radix"/> to the power of the given exponent (fast shifting operation)
        /// </summary>
        /// <param name="exponent"></param>
        /// <returns></returns>
        private Natural ShiftDown(uint exponent)
        {
            if(exponent >= _bound)
                return Zero;
            uint len = _bound - exponent;
            Natural ret = new Natural(len);
            for(uint i = 0; i < _bound; i++)
                ret._coeff[i] = _coeff[i + exponent];
            return ret;
        }
        
        /// <summary>
        /// Set all coefficients of exponents higher than or equal to the given exponent to zero.
        /// </summary>
        private Natural Restrict(uint exponent)
        {
            return new Natural(exponent, _coeff);
        }
        #endregion
        #region Multiplication
        public static Natural operator *(Natural a, Natural b)
        {
            return a.Multiply(b);
        }

        [CLSCompliant(false)]
        public Natural Multiply(uint factor)
        {
            uint len = _bound + _bound32;
            Natural ret = new Natural(len);
            for(uint i = 0; i < _bound; i++)
                ret.AddCoefficientInplace((ulong)_coeff[i] * factor, i);
            ret.Normalize();
            return ret;
        }

        public Natural Multiply(Natural number)
        {
            if(Math.Max(_bound, number._bound) < 12)
                return MultiplySmall(number);
            else
                return MultiplyLarge(number);
        }
        /// <summary>
        /// Multiplies two small naturals with the school book algorithm; O(n^2)
        /// </summary>
        private Natural MultiplySmall(Natural number)
        {
            uint len = _bound + number._bound + 2;
            Natural ret = new Natural(len);
            for(uint i = 0; i < _bound; i++)
                for(uint j = 0; j < number._bound; j++)
                    ret.AddCoefficientInplace((ulong)_coeff[i] * number._coeff[j], i + j);
            ret.Normalize();
            return ret;
        }
        /// <summary>
        /// Multiplies two large naturals with the karatsuba algorithm; O(n^1.59).
        /// Could be extended with FFT based algorithms by Sch�nhage/Strassen in future versions; O(nlognloglogn).
        /// </summary>
        private Natural MultiplyLarge(Natural number)
        {
            uint len = Math.Max(_bound, number._bound);
            uint k = (uint)Math.Ceiling(0.5*len);
            Natural a0 = Restrict(k);
            Natural a1 = ShiftDown(k);
            Natural b0 = number.Restrict(k);
            Natural b1 = number.ShiftDown(k);
            Natural q0 = a0 * b0;
            Natural q1 = (a0 + a1) * (b0 + b1);
            Natural q2 = a1 * b1;
            Natural p0 = q1 - (q0 + q2);
            return (q2.ShiftUp(k) + p0).ShiftUp(k) + q0;
        }
        #endregion
        #region Division
        //private Tuple<Natural, Natural> DivmodSmall(Natural number)
        //{
        //    Natural b = this;
        //    Natural a = number;  // b / a

        //    uint dega = a.Degree;
        //    uint degb = b.Degree;

        //    if(degb < dega)
        //        return new Tuple<Natural, Natural>(Natural.Zero, b);
        //    if(a.IsZero)
        //        throw new DivideByZeroException();
        //    if(b.IsZero)
        //        return new Tuple<Natural, Natural>(Natural.Zero, Natural.Zero);


        //}
        #endregion

        #region Equatable, Comparable, Min/Max, Operators

        public bool Equals(Natural other)
        {
            if(_bound != other._bound)
                return false;
            for(uint i = 0; i < _bound; i++)
                if(_coeff[i] != other._coeff[i])
                    return false;
            return true;
        }

        public int CompareTo(Natural other)
        {
            if(_bound < other._bound)
                return -1;
            if(_bound > other._bound)
                return 1;
            for(uint i = _bound - 1; i >= 0; i--)
            {
                if(_coeff[i] < other._coeff[i])
                    return -1;
                if(_coeff[i] > other._coeff[i])
                    return 1;
            }
            return 0;
        }

        public static bool operator <(Natural a, Natural b)
        {
            return a.CompareTo(b) == -1;
        }
        public static bool operator >(Natural a, Natural b)
        {
            return a.CompareTo(b) == 1;
        }
        public static bool operator <=(Natural a, Natural b)
        {
            return a.CompareTo(b) != 1;
        }
        public static bool operator >=(Natural a, Natural b)
        {
            return a.CompareTo(b) != -1;
        }

        public static Natural Min(Natural a, Natural b)
        {
            return a.CompareTo(b) == -1 ? a : b;
        }
        public static Natural Max(Natural a, Natural b)
        {
            return a.CompareTo(b) == -1 ? b : a;
        }
        #endregion
    }
}

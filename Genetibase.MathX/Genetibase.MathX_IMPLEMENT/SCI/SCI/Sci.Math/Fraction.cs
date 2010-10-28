/* *****************************************************************************
 * AUTHOR       : Coskun OBA
 * EMAIL        : oba.coskun@hotmail.com
 * 
 * DATE         : JANUARY 2007
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Sci.Math
{
    #region Fraction
    public struct Fraction
    {
        private int m_numerator;
        private int m_denominator;

        public Fraction(int number)
        {
            this.m_numerator = number;
            this.m_denominator = 1;
            ReduceFraction(ref this);
        }
        public Fraction(int numerator, int denominator)
        {
            this.m_numerator = numerator;
            this.m_denominator = denominator;

            if (denominator < 0)
            {
                this.m_numerator = -numerator;
                this.m_denominator = -denominator;
            }
            ReduceFraction(ref this);
        }

        #region Properties
        public int Numerator
        {
            get { return m_numerator; }
            set { m_numerator = value; }
        }
        public int Denominator
        {
            get { return m_denominator; }
            set 
            {
                m_denominator = value;
                if (m_denominator < 0)
                {
                    m_numerator = -m_numerator;
                    m_denominator = -m_denominator;
                }
            }
        }
        #endregion

        #region Special Values
        public static readonly Fraction Zero = new Fraction(0, 1);
        public static readonly Fraction NaN = new Fraction();
        #endregion

        #region Static Methods
        public static Fraction Inverse(Fraction f)
        {
            return new Fraction(f.m_denominator, f.m_numerator);
        }
        public static int GCD(int m, int n)
        {
            return (n == 0) ? System.Math.Abs(m) : GCD(n, m % n);
        }
        public static void ReduceFraction(ref Fraction f)
        {
            int gcd = GCD(f.m_numerator, f.m_denominator);
            f.m_numerator /= gcd;
            f.m_denominator /= gcd;
        }
        #endregion

        #region Operator Overloading
        public static implicit operator Fraction(int number)
        {
            return new Fraction(number, 1);
        }
        public static bool operator ==(Fraction f1, Fraction f2)
        {
            int left = f1.m_numerator * f2.m_denominator;
            int right = f1.m_denominator * f2.m_numerator;

            return (left.Equals(right));
        }
        public static bool operator !=(Fraction f1, Fraction f2)
        {
            return !(f1 == f2);
        }
        public static Fraction operator -(Fraction f)
        {
            return new Fraction(-f.m_numerator, f.m_denominator);
        }
        public static Fraction operator +(Fraction f1, Fraction f2)
        {
            if (f1.m_denominator.Equals(f2.m_denominator))
            {
                return new Fraction(f1.m_numerator + f2.m_numerator, f1.m_denominator);
            }

            int numerator = f1.m_numerator * f2.m_denominator +
                                f1.m_denominator * f2.m_numerator;
            int denominator = f1.m_denominator * f2.m_denominator;

            return new Fraction(numerator, denominator);
        }        
        public static Fraction operator -(Fraction f1, Fraction f2)
        {
            return f1 + (-f2);
        }
        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            int numerator = f1.m_numerator * f2.m_numerator;
            int denominator = f1.m_denominator * f2.m_denominator;

            return new Fraction(numerator, denominator);
        }        
        public static Fraction operator /(Fraction f1, Fraction f2)
        {
            int numerator = f1.m_numerator * f2.m_denominator;
            int denominator = f1.m_denominator * f2.m_numerator;

            return new Fraction(numerator, denominator);
        }
        #endregion

        #region Overriden Methods
        public override bool Equals(object obj)
        {
            return ((obj is Fraction)) ? this == (Fraction)obj : false;
        }
        public override int GetHashCode()
        {
            return m_numerator.GetHashCode()^m_denominator.GetHashCode();
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder(m_numerator.ToString());
            if (m_denominator != 0 &&  m_denominator != 1)
            {
                s.Append("/");
                s.Append(m_denominator.ToString());
            }
            return s.ToString();
        }
        #endregion
    }
    #endregion
}

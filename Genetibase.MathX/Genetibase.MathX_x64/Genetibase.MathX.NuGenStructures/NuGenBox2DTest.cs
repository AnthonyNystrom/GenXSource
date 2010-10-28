using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenBox2D.
    /// </summary>
    public struct NuGenBox2D
    {

        public static NuGenBox2D Empty = new NuGenBox2D(NuGenVector.HUGE_NuGenPnt2D, -NuGenVector.HUGE_NuGenPnt2D);

        public NuGenBox2D(NuGenPnt2D lower, NuGenPnt2D upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public NuGenPnt2D Lower
        {
            get
            {
                return lower;
            }
            set
            {
                lower = value;
            }
        }

        public NuGenPnt2D Upper
        {
            get
            {
                return upper;
            }
            set
            {
                upper = value;
            }
        }

        public NuGenPnt2D LL
        {
            get
            {
                return new NuGenPnt2D(lower._x[0], lower._x[1]);
            }
        }

        public NuGenPnt2D LU
        {
            get
            {
                return new NuGenPnt2D(lower._x[0], upper._x[1]);
            }
        }

        public NuGenPnt2D UL
        {
            get
            {
                return new NuGenPnt2D(upper._x[0], lower._x[1]);
            }
        }

        public NuGenPnt2D UU
        {
            get
            {
                return new NuGenPnt2D(upper._x[0], upper._x[1]);
            }
        }

        public int MinDim
        {
            get
            {
                NuGenVec2D dim = upper - lower;
                return (dim[0] < dim[1]) ? 0 : 1;
            }
        }

        public int MaxDim
        {
            get
            {
                NuGenVec2D dim = upper - lower;
                return (dim[0] > dim[1]) ? 0 : 1;
            }
        }

        public double Minimum
        {
            get
            {
                NuGenVec2D dim = upper - lower;
                return (dim[0] < dim[1]) ? dim[0] : dim[1];
            }
        }

        public double Maximum
        {
            get
            {
                NuGenVec2D dim = upper - lower;
                return (dim[0] > dim[1]) ? dim[0] : dim[1];
            }
        }

        public bool IsInside(NuGenPnt2D p)
        {
            return lower < p && upper > p;
        }

        public bool IsInsideOrOnBorder(NuGenPnt2D p)
        {
            return lower <= p && upper >= p;
        }

        public bool IsOnBorder(NuGenPnt2D p)
        {
            return IsInsideOrOnBorder(p) && !IsInside(p);
        }

        public bool IsInside(NuGenBox2D b)
        {
            return lower < b.lower && upper > b.upper;
        }

        public bool IsInsideOrOnBorder(NuGenBox2D b)
        {
            return lower <= b.lower && upper >= b.upper;
        }

        public bool IsOnBorder(NuGenBox2D b)
        {
            return IsInsideOrOnBorder(b) && !IsInside(b);
        }

        public int Relation(int dim, float val)
        {

            if (upper[dim] < val) return -1;
            if (lower[dim] > val) return 1;
            return 0;
        }

        public static NuGenBox2D operator +(NuGenBox2D b, NuGenPnt2D p)
        {
            return new NuGenBox2D(NuGenPnt2D.Min(b.lower, p), NuGenPnt2D.Max(b.upper, p));
        }

        public static NuGenBox2D operator +(NuGenBox2D b, NuGenBox2D c)
        {
            return new NuGenBox2D(NuGenPnt2D.Min(b.lower, c.lower), NuGenPnt2D.Max(b.upper, c.upper));
        }

        public static bool ApproxEqual(NuGenBox2D a, NuGenBox2D b)
        {
            return
                NuGenVector.ApproxEquals(a.Lower, b.Lower) &&
                NuGenVector.ApproxEquals(a.Upper, b.Upper);
        }

        public override bool Equals(object obj)
        {
            NuGenBox2D x = (NuGenBox2D)obj;
            return (lower == x.Lower && upper == x.Upper);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "NuGenBox2D(" + lower + ", " + upper + ")";
        }

        internal NuGenPnt2D lower;
        internal NuGenPnt2D upper;

    }

    //[TestFixture]
    public class NuGenBox2DTest
    {

        //[Test]
        public void InitTest()
        {
            NuGenBox2D b = new NuGenBox2D(new NuGenPnt2D(1, 2), new NuGenPnt2D(4, 5));
            //Assert.AreEqual(new NuGenPnt2D(1,2), b.Lower);
            //Assert.AreEqual(new NuGenPnt2D(4,5), b.Upper);

            //Assert.AreEqual(new NuGenPnt2D(1,2), b.LL);
            //Assert.AreEqual(new NuGenPnt2D(1,5), b.LU);
            //Assert.AreEqual(new NuGenPnt2D(4,2), b.UL);
            //Assert.AreEqual(new NuGenPnt2D(4,5), b.UU);
        }

    }

}

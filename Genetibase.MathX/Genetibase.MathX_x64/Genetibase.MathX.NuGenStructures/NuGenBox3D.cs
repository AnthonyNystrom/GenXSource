using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenBox3D.
    /// </summary>
    public struct NuGenBox3D
    {

        public static NuGenBox3D Empty = new NuGenBox3D(NuGenVector.HUGE_NuGenPnt3D, -NuGenVector.HUGE_NuGenPnt3D);

        public NuGenBox3D(NuGenPnt3D lower, NuGenPnt3D upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public NuGenPnt3D Lower
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

        public NuGenPnt3D Upper
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

        public NuGenPnt3D LLL
        {
            get
            {
                return new NuGenPnt3D(lower._x[0], lower._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3D LLU
        {
            get
            {
                return new NuGenPnt3D(lower._x[0], lower._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3D LUL
        {
            get
            {
                return new NuGenPnt3D(lower._x[0], upper._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3D LUU
        {
            get
            {
                return new NuGenPnt3D(lower._x[0], upper._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3D ULL
        {
            get
            {
                return new NuGenPnt3D(upper._x[0], lower._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3D ULU
        {
            get
            {
                return new NuGenPnt3D(upper._x[0], lower._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3D UUL
        {
            get
            {
                return new NuGenPnt3D(upper._x[0], upper._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3D UUU
        {
            get
            {
                return new NuGenPnt3D(upper._x[0], upper._x[1], upper._x[2]);
            }
        }

        public int MinDim
        {
            get
            {
                NuGenVec3D dim = upper - lower;
                return (dim[0] < dim[1])
                    ? ((dim[0] < dim[2]) ? 0 : 2)
                    : ((dim[1] < dim[2]) ? 1 : 2);
            }
        }

        public int MaxDim
        {
            get
            {
                NuGenVec3D dim = upper - lower;
                return (dim[0] > dim[1])
                    ? ((dim[0] > dim[2]) ? 0 : 2)
                    : ((dim[1] > dim[2]) ? 1 : 2);
            }
        }

        public double Minimum
        {
            get
            {
                NuGenVec3D dim = upper - lower;
                return (dim[0] < dim[1])
                    ? ((dim[0] < dim[2]) ? dim[0] : dim[2])
                    : ((dim[1] < dim[2]) ? dim[1] : dim[2]);
            }
        }

        public double Maximum
        {
            get
            {
                NuGenVec3D dim = upper - lower;
                return (dim[0] > dim[1])
                    ? ((dim[0] > dim[2]) ? dim[0] : dim[2])
                    : ((dim[1] > dim[2]) ? dim[1] : dim[2]);
            }
        }

        public bool IsInside(NuGenPnt3D p)
        {
            return lower < p && upper > p;
        }

        public bool IsInsideOrOnBorder(NuGenPnt3D p)
        {
            return lower <= p && upper >= p;
        }

        public bool IsOnBorder(NuGenPnt3D p)
        {
            return IsInsideOrOnBorder(p) && !IsInside(p);
        }

        public bool IsInside(NuGenBox3D b)
        {
            return lower < b.lower && upper > b.upper;
        }

        public bool IsInsideOrOnBorder(NuGenBox3D b)
        {
            return lower <= b.lower && upper >= b.upper;
        }

        public bool IsOnBorder(NuGenBox3D b)
        {
            return IsInsideOrOnBorder(b) && !IsInside(b);
        }

        public int Relation(int dim, double val)
        {

            if (upper[dim] < val) return -1;
            if (lower[dim] > val) return 1;
            return 0;
        }

        public bool Intersect(NuGenRay3D r)
        {
            bool inside = true;
            int[] quadrant = new int[3];
            int i;
            int whichPlane;

            double[] maxT = new double[3];
            double[] candidatePlane = new double[3];
            NuGenVec3D coord = new NuGenVec3D(0, 0, 0);

            for (i = 0; i < 3; i++)
            {

                if (r.Point[i] < lower[i])
                {
                    quadrant[i] = 1;
                    candidatePlane[i] = lower[i];
                    inside = false;
                }

                else if (r.Point[i] > upper[i])
                {
                    quadrant[i] = 0;
                    candidatePlane[i] = upper[i];
                    inside = false;
                }

                else
                {
                    quadrant[i] = 2;
                }
            }

            if (inside) return true;

            for (i = 0; i < 3; i++)
            {

                if (quadrant[i] != 2 && r.Direction[i] != 0.0)
                {
                    maxT[i] = (candidatePlane[i] - r.Point[i]) / r.Direction[i];
                }

                else
                {
                    maxT[i] = -1.0;
                }
            }

            whichPlane = 0;

            for (i = 1; i < 3; i++)
            {

                if (maxT[whichPlane] < maxT[i]) whichPlane = i;
            }

            if (maxT[whichPlane] < 0.0) return false;

            for (i = 0; i < 3; i++)
            {

                if (whichPlane != i)
                {
                    coord[i] = r.Point[i] + maxT[whichPlane] * r.Direction[i];

                    if (coord[i] < lower[i] || coord[i] > upper[i])
                    {
                        return false;
                    }
                }

                else
                {
                    coord[i] = candidatePlane[i];
                }
            }

            return true;
        }

        public static NuGenBox3D operator +(NuGenBox3D b, NuGenPnt3D p)
        {
            return new NuGenBox3D(NuGenPnt3D.Min(b.lower, p), NuGenPnt3D.Max(b.upper, p));
        }

        public static NuGenBox3D operator +(NuGenBox3D b, NuGenBox3D c)
        {
            return new NuGenBox3D(NuGenPnt3D.Min(b.lower, c.lower), NuGenPnt3D.Max(b.upper, c.upper));
        }

        public static bool ApproxEqual(NuGenBox3D a, NuGenBox3D b)
        {
            return
                NuGenVector.ApproxEquals(a.Lower, b.Lower) &&
                NuGenVector.ApproxEquals(a.Upper, b.Upper);
        }

        public override bool Equals(object obj)
        {
            NuGenBox3D x = (NuGenBox3D)obj;
            return (lower == x.Lower && upper == x.Upper);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "NuGenBox3D(" + lower + ", " + upper + ")";
        }

        internal NuGenPnt3D lower;
        internal NuGenPnt3D upper;

    }


}

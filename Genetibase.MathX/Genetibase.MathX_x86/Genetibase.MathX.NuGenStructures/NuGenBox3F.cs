using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenBox3F.
    /// </summary>
    public struct NuGenBox3F
    {

        public static NuGenBox3F Empty = new NuGenBox3F(NuGenVector.HUGE_NuGenPnt3F, -NuGenVector.HUGE_NuGenPnt3F);

        public NuGenBox3F(NuGenPnt3F lower, NuGenPnt3F upper)
        {
            this.lower = lower;
            this.upper = upper;
        }

        public NuGenPnt3F Lower
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

        public NuGenPnt3F Upper
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

        public NuGenPnt3F LLL
        {
            get
            {
                return new NuGenPnt3F(lower._x[0], lower._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3F LLU
        {
            get
            {
                return new NuGenPnt3F(lower._x[0], lower._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3F LUL
        {
            get
            {
                return new NuGenPnt3F(lower._x[0], upper._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3F LUU
        {
            get
            {
                return new NuGenPnt3F(lower._x[0], upper._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3F ULL
        {
            get
            {
                return new NuGenPnt3F(upper._x[0], lower._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3F ULU
        {
            get
            {
                return new NuGenPnt3F(upper._x[0], lower._x[1], upper._x[2]);
            }
        }

        public NuGenPnt3F UUL
        {
            get
            {
                return new NuGenPnt3F(upper._x[0], upper._x[1], lower._x[2]);
            }
        }

        public NuGenPnt3F UUU
        {
            get
            {
                return new NuGenPnt3F(upper._x[0], upper._x[1], upper._x[2]);
            }
        }

        public int MinDim
        {
            get
            {
                NuGenVec3F dim = upper - lower;
                return (dim[0] < dim[1])
                    ? ((dim[0] < dim[2]) ? 0 : 2)
                    : ((dim[1] < dim[2]) ? 1 : 2);
            }
        }

        public int MaxDim
        {
            get
            {
                NuGenVec3F dim = upper - lower;
                return (dim[0] > dim[1])
                    ? ((dim[0] > dim[2]) ? 0 : 2)
                    : ((dim[1] > dim[2]) ? 1 : 2);
            }
        }

        public float Minimum
        {
            get
            {
                NuGenVec3F dim = upper - lower;
                return (dim[0] < dim[1])
                    ? ((dim[0] < dim[2]) ? dim[0] : dim[2])
                    : ((dim[1] < dim[2]) ? dim[1] : dim[2]);
            }
        }

        public float Maximum
        {
            get
            {
                NuGenVec3F dim = upper - lower;
                return (dim[0] > dim[1])
                    ? ((dim[0] > dim[2]) ? dim[0] : dim[2])
                    : ((dim[1] > dim[2]) ? dim[1] : dim[2]);
            }
        }

        public bool IsInside(NuGenPnt3F p)
        {
            return lower < p && upper > p;
        }

        public bool IsInsideOrOnBorder(NuGenPnt3F p)
        {
            return lower <= p && upper >= p;
        }

        public bool IsOnBorder(NuGenPnt3F p)
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

        public int Relation(int dim, float val)
        {

            if (upper[dim] < val) return -1;
            if (lower[dim] > val) return 1;
            return 0;
        }

        public bool Intersect(NuGenRay3F r)
        {
            bool inside = true;
            int[] quadrant = new int[3];
            int i;
            int whichPlane;
            float[] max = new float[3];
            float[] candidatePlane = new float[3];
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
                    max[i] = (candidatePlane[i] - r.Point[i]) / r.Direction[i];
                }

                else
                {
                    max[i] = -1.0f;
                }
            }

            whichPlane = 0;

            for (i = 1; i < 3; i++)
            {

                if (max[whichPlane] < max[i]) whichPlane = i;
            }

            if (max[whichPlane] < 0.0) return false;

            for (i = 0; i < 3; i++)
            {

                if (whichPlane != i)
                {
                    coord[i] = r.Point[i] + max[whichPlane] * r.Direction[i];

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

        public static NuGenBox3F operator +(NuGenBox3F b, NuGenPnt3F p)
        {
            return new NuGenBox3F(NuGenPnt3F.Min(b.lower, p), NuGenPnt3F.Max(b.upper, p));
        }

        public static NuGenBox3F operator +(NuGenBox3F b, NuGenBox3F c)
        {
            return new NuGenBox3F(NuGenPnt3F.Min(b.lower, c.lower), NuGenPnt3F.Max(b.upper, c.upper));
        }

        public static bool ApproxEqual(NuGenBox3F a, NuGenBox3F b)
        {
            return
                NuGenVector.ApproxEquals(a.Lower, b.Lower) &&
                NuGenVector.ApproxEquals(a.Upper, b.Upper);
        }

        public override bool Equals(object obj)
        {
            NuGenBox3F x = (NuGenBox3F)obj;
            return (lower == x.Lower && upper == x.Upper);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "NuGenBox3F(" + lower + ", " + upper + ")";
        }

        internal NuGenPnt3F lower;
        internal NuGenPnt3F upper;

    }


}

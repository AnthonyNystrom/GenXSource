using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenScale3F.
    /// </summary>
    public struct NuGenScale3F
    {

        public static NuGenScale3F Identity = new NuGenScale3F(1, 1, 1);

        public NuGenScale3F(float x, float y, float z)
        {
            v = new NuGenVec3F(x, y, z);
        }

        public NuGenScale3F(NuGenVec3F shift)
        {
            v = shift;
        }

        public float x
        {
            get
            {
                return v._x[0];
            }
        }

        public float y
        {
            get
            {
                return v._x[1];
            }
        }

        public float z
        {
            get
            {
                return v._x[2];
            }
        }

        public NuGenTrafo3F ToNuGenTrafo3F()
        {
            return new NuGenTrafo3F(
                v._x[0], 0, 0, 0,
                0, v._x[1], 0, 0,
                0, 0, v._x[2], 0,
                0, 0, 0, 1
                );
        }

        public static implicit operator NuGenTrafo3F(NuGenScale3F s)
        {
            return s.ToNuGenTrafo3F();
        }

        public static NuGenVec3F operator *(NuGenVec3F v, NuGenScale3F t)
        {
            return new NuGenVec3F(
                v._x[0] * t.v._x[0],
                v._x[1] * t.v._x[1],
                v._x[2] * t.v._x[2]
                );
        }

        public static NuGenPnt3F operator *(NuGenPnt3F p, NuGenScale3F t)
        {
            return new NuGenPnt3F(
                p._x[0] * t.v._x[0],
                p._x[1] * t.v._x[1],
                p._x[2] * t.v._x[2]
                );
        }

        public static NuGenRay3F operator *(NuGenRay3F r, NuGenScale3F t)
        {
            return new NuGenRay3F(r.p * t, r.v * t);
        }

        public static NuGenBox3F operator *(NuGenBox3F b, NuGenScale3F t)
        {
            return new NuGenBox3F(b.Lower * t, b.Upper * t);
        }

        public static NuGenTrafo3F operator *(NuGenShift3F a, NuGenScale3F b)
        {
            return a.ToNuGenTrafo3F() * b.ToNuGenTrafo3F();
        }

        public static NuGenTrafo3F operator *(NuGenRot3F a, NuGenScale3F b)
        {
            return a.ToNuGenTrafo3F() * b.ToNuGenTrafo3F();
        }

        internal NuGenVec3F v;

    }

}

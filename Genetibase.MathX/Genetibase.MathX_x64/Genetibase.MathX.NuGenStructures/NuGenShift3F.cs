using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenShift3F.
    /// </summary>
    public struct NuGenShift3F
    {

        public static NuGenShift3F Identity = new NuGenShift3F(0, 0, 0);

        public NuGenShift3F(float x, float y, float z)
        {
            v = new NuGenVec3F(x, y, z);
        }

        public NuGenShift3F(NuGenVec3F shift)
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
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                v._x[0], v._x[1], v._x[2], 1
                );
        }

        public static implicit operator NuGenTrafo3F(NuGenShift3F r)
        {
            return r.ToNuGenTrafo3F();
        }

        public static NuGenShift3F operator *(NuGenShift3F a, NuGenShift3F b)
        {
            return new NuGenShift3F(a.v + b.v);
        }

        public static NuGenTrafo3F operator *(NuGenScale3F a, NuGenShift3F b)
        {
            return new NuGenTrafo3F(
                a.x, 0, 0, 0,
                0, a.y, 0, 0,
                0, 0, a.z, 0,
                b.x, b.y, b.z, 1
                );
        }

        public static NuGenTrafo3F operator *(NuGenRot3F a, NuGenShift3F b)
        {
            return a.ToNuGenTrafo3F() * b.ToNuGenTrafo3F();
        }

        public static NuGenTrafo3F operator *(NuGenTrafo3F a, NuGenShift3F b)
        {
            a._x[12] += b.x;
            a._x[13] += b.y;
            a._x[14] += b.z;
            return a;
        }

        public static NuGenVec3F operator *(NuGenVec3F v, NuGenShift3F t)
        {
            return v;
        }

        public static NuGenPnt3F operator *(NuGenPnt3F p, NuGenShift3F t)
        {
            return p + t.v;
        }

        public static NuGenRay3F operator *(NuGenRay3F r, NuGenShift3F t)
        {
            return new NuGenRay3F(r.p * t, r.v * t);
        }

        public static NuGenBox3F operator *(NuGenBox3F b, NuGenShift3F t)
        {
            return new NuGenBox3F(b.Lower + t.v, b.Upper + t.v);
        }

        internal NuGenVec3F v;

    }

}

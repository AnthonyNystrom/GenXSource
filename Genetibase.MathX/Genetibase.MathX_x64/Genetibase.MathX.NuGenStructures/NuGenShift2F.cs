using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenShift2F.
    /// </summary>
    public struct NuGenShift2F
    {

        public static NuGenShift2F Identity = new NuGenShift2F(0, 0);

        public NuGenShift2F(float x, float y)
        {
            v = new NuGenVec2F(x, y);
        }

        public NuGenShift2F(NuGenVec2F shift)
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

        public NuGenTrafo2F ToNuGenTrafo2F()
        {
            return new NuGenTrafo2F(
                1, 0, 0,
                0, 1, 0,
                v._x[0], v._x[1], 1
                );
        }

        public static implicit operator NuGenTrafo2F(NuGenShift2F r)
        {
            return r.ToNuGenTrafo2F();
        }

        public static NuGenShift2F operator *(NuGenShift2F a, NuGenShift2F b)
        {
            return new NuGenShift2F(a.v + b.v);
        }

        public static NuGenTrafo2F operator *(Scale2F a, NuGenShift2F b)
        {
            return new NuGenTrafo2F(
                a.x, 0, 0,
                0, a.y, 0,
                b.x, b.y, 1
                );
        }

        public static NuGenTrafo2F operator *(NuGenRot2F a, NuGenShift2F b)
        {
            return a.ToNuGenTrafo2F() * b.ToNuGenTrafo2F();
        }

        public static NuGenTrafo2F operator *(NuGenTrafo2F a, NuGenShift2F b)
        {
            a._x[6] += b.x;
            a._x[7] += b.y;
            return a;
        }

        public static NuGenVec2F operator *(NuGenVec2F v, NuGenShift2F t)
        {
            return v;
        }

        public static NuGenPnt2F operator *(NuGenPnt2F p, NuGenShift2F t)
        {
            return p + t.v;
        }

        private NuGenVec2F v;

    }


}

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// Scale2F.
    /// </summary>
    public struct Scale2F
    {

        public static Scale2F Identity = new Scale2F(1, 1);

        public Scale2F(float x, float y)
        {
            v = new NuGenVec2F(x, y);
        }

        public Scale2F(NuGenVec2F scale)
        {
            v = scale;
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
                v._x[0], 0, 0,
                0, v._x[1], 0,
                0, 0, 1
                );
        }

        public static implicit operator NuGenTrafo2F(Scale2F s)
        {
            return s.ToNuGenTrafo2F();
        }

        public static NuGenVec2F operator *(NuGenVec2F v, Scale2F t)
        {
            return new NuGenVec2F(
                v._x[0] * t.v._x[0],
                v._x[1] * t.v._x[1]
                );
        }

        public static NuGenPnt2F operator *(NuGenPnt2F p, Scale2F t)
        {
            return new NuGenPnt2F(
                p._x[0] * t.v._x[0],
                p._x[1] * t.v._x[1]
                );
        }

        public static NuGenBox2F operator *(NuGenBox2F b, Scale2F t)
        {
            return new NuGenBox2F(b.Lower * t, b.Upper * t);
        }

        public static NuGenTrafo2F operator *(NuGenShift2F a, Scale2F b)
        {
            return a.ToNuGenTrafo2F() * b.ToNuGenTrafo2F();
        }

        public static NuGenTrafo2F operator *(NuGenRot2F a, Scale2F b)
        {
            return a.ToNuGenTrafo2F() * b.ToNuGenTrafo2F();
        }

        private NuGenVec2F v;

    }

}

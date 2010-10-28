using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenRot2F.
    /// </summary>
    public struct NuGenRot2F
    {

        public static NuGenRot2F Identity = new NuGenRot2F(0);

        public NuGenRot2F(float radians)
        {
            r = radians;
            cos = (float)Math.Cos(radians);
            sin = (float)Math.Sin(radians);
        }

        public NuGenTrafo2F ToNuGenTrafo2F()
        {
            return new NuGenTrafo2F(
                cos, sin, 0,
                -sin, cos, 0,
                0, 0, 1
                );
        }

        public static implicit operator NuGenTrafo2F(NuGenRot2F r)
        {
            return r.ToNuGenTrafo2F();
        }

        public static NuGenVec2F operator *(NuGenVec2F v, NuGenRot2F r)
        {
            return v * r.ToNuGenTrafo2F();
        }

        public static NuGenPnt2F operator *(NuGenPnt2F p, NuGenRot2F r)
        {
            return p * r.ToNuGenTrafo2F();
        }

        public static NuGenTrafo2F operator *(NuGenShift2F t, NuGenRot2F r)
        {
            return t.ToNuGenTrafo2F() * r.ToNuGenTrafo2F();
        }

        public static NuGenTrafo2F operator *(Scale2F t, NuGenRot2F r)
        {
            return t.ToNuGenTrafo2F() * r.ToNuGenTrafo2F();
        }

        private float r;
        private float cos;
        private float sin;

    }

}

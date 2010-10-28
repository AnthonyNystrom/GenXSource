using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenRot3F.
    /// </summary>
    public struct NuGenRot3F
    {

        public static NuGenRot3F Identity = new NuGenRot3F(0, NuGenVec3F.UnitZ);

        public NuGenRot3F(float radians, NuGenVec3F axis)
        {
            axis.Normalize();
            r = (float)Math.Cos(radians / 2.0);
            v = axis.Normalized * (float)Math.Sin(radians / 2.0);
        }

        public NuGenRot3F(NuGenVec3F from, NuGenVec3F to)
        {
            from.Normalize();
            to.Normalize();

            float cost =
                NuGenVec3F.Dot(from, to) /
                (float)Math.Sqrt(NuGenVec3F.Dot(from, to) * NuGenVec3F.Dot(to, to));

            if (cost > 0.99999)
            {
                r = 1;
                v = new NuGenVec3F(0, 0, 0);
            }

            else if (cost < -0.99999)
            {
                NuGenVec3F frm = from.Normalized;
                v = NuGenVec3F.Cross(frm, NuGenVec3F.UnitX);

                if (v.Length < 0.00001) v = NuGenVec3F.Cross(frm, NuGenVec3F.UnitY);
                r = 0;
                v.Normalize();
            }

            else
            {
                r = (float)Math.Sqrt(0.5 * (1.0 + cost));
                v = NuGenVec3F.Cross(from, to);
                v *= (float)Math.Sqrt((0.5 * (1.0 - cost)) / NuGenVec3F.Dot(v, v));
            }
        }

        public NuGenTrafo3F ToNuGenTrafo3F()
        {
            return new NuGenTrafo3F(
                1.0f - 2.0f * (v[1] * v[1] + v[2] * v[2]),
                2.0f * (v[0] * v[1] + v[2] * r),
                2.0f * (v[0] * v[2] - v[1] * r),
                0.0f,

                2.0f * (v[0] * v[1] - v[2] * r),
                1.0f - 2.0f * (v[0] * v[0] + v[2] * v[2]),
                2.0f * (v[1] * v[2] + v[0] * r),
                0.0f,

                2.0f * (v[0] * v[2] + v[1] * r),
                2.0f * (v[1] * v[2] - v[0] * r),
                1.0f - 2.0f * (v[0] * v[0] + v[1] * v[1]),
                0.0f,

                0.0f, 0.0f, 0.0f, 1.0f

                );
        }

        public static implicit operator NuGenTrafo3F(NuGenRot3F r)
        {
            return r.ToNuGenTrafo3F();
        }

        public static NuGenVec3F operator *(NuGenVec3F v, NuGenRot3F r)
        {
            return v * r.ToNuGenTrafo3F();
        }

        public static NuGenPnt3F operator *(NuGenPnt3F p, NuGenRot3F r)
        {
            return p * r.ToNuGenTrafo3F();
        }

        public static NuGenRay3F operator *(NuGenRay3F r, NuGenRot3F t)
        {
            return new NuGenRay3F(r.p * t, r.v * t);
        }

        public static NuGenBox3F operator *(NuGenBox3F b, NuGenRot3F r)
        {
            NuGenBox3F result = NuGenBox3F.Empty;
            result += b.LLL * r;
            result += b.LLU * r;
            result += b.LUL * r;
            result += b.LUU * r;
            result += b.ULL * r;
            result += b.ULU * r;
            result += b.UUL * r;
            result += b.UUU * r;
            return result;
        }

        public static NuGenTrafo3F operator *(NuGenShift3F t, NuGenRot3F r)
        {
            return t.ToNuGenTrafo3F() * r.ToNuGenTrafo3F();
        }

        public static NuGenTrafo3F operator *(NuGenScale3F t, NuGenRot3F r)
        {
            return t.ToNuGenTrafo3F() * r.ToNuGenTrafo3F();
        }

        private NuGenVec3F v;
        private float r;

    }

}

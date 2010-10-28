using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenRot3D.
    /// </summary>
    public struct NuGenRot3D
    {

        public static NuGenRot3D Identity = new NuGenRot3D(0, NuGenVec3D.UnitZ);

        public NuGenRot3D(double radians, NuGenVec3D axis)
        {
            axis.Normalize();
            r = Math.Cos(radians / 2.0);
            v = axis.Normalized * Math.Sin(radians / 2.0);
        }

        public NuGenRot3D(NuGenVec3D from, NuGenVec3D to)
        {
            from.Normalize();
            to.Normalize();

            double cost =
                NuGenVec3D.Dot(from, to) /
                Math.Sqrt(NuGenVec3D.Dot(from, to) * NuGenVec3D.Dot(to, to));

            if (cost > 0.99999)
            {
                r = 1;
                v = new NuGenVec3D(0, 0, 0);
            }

            else if (cost < -0.99999)
            {
                NuGenVec3D frm = from.Normalized;
                v = NuGenVec3D.Cross(frm, NuGenVec3D.UnitX);

                if (v.Length < 0.00001) v = NuGenVec3D.Cross(frm, NuGenVec3D.UnitY);
                r = 0;
                v.Normalize();
            }

            else
            {
                r = Math.Sqrt(0.5 * (1.0 + cost));
                v = NuGenVec3D.Cross(from, to);
                v *= Math.Sqrt((0.5 * (1.0 - cost)) / NuGenVec3D.Dot(v, v));
            }
        }

        public NuGenTrafo3D ToNuGenTrafo3D()
        {
            return new NuGenTrafo3D(
                1.0 - 2.0 * (v[1] * v[1] + v[2] * v[2]),
                2.0 * (v[0] * v[1] + v[2] * r),
                2.0 * (v[0] * v[2] - v[1] * r),
                0.0,

                2.0 * (v[0] * v[1] - v[2] * r),
                1.0 - 2.0 * (v[0] * v[0] + v[2] * v[2]),
                2.0 * (v[1] * v[2] + v[0] * r),
                0.0,

                2.0 * (v[0] * v[2] + v[1] * r),
                2.0 * (v[1] * v[2] - v[0] * r),
                1.0 - 2.0 * (v[0] * v[0] + v[1] * v[1]),
                0.0,

                0.0, 0.0, 0.0, 1.0

                );
        }

        public static implicit operator NuGenTrafo3D(NuGenRot3D r)
        {
            return r.ToNuGenTrafo3D();
        }

        public static NuGenVec3D operator *(NuGenVec3D v, NuGenRot3D r)
        {
            return v * r.ToNuGenTrafo3D();
        }

        public static NuGenPnt3D operator *(NuGenPnt3D p, NuGenRot3D r)
        {
            return p * r.ToNuGenTrafo3D();
        }

        public static NuGenRay3D operator *(NuGenRay3D r, NuGenRot3D t)
        {
            return new NuGenRay3D(r.p * t, r.v * t);
        }

        public static NuGenBox3D operator *(NuGenBox3D b, NuGenRot3D r)
        {
            NuGenBox3D result = NuGenBox3D.Empty;
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

        public static NuGenTrafo3D operator *(NuGenShift3D t, NuGenRot3D r)
        {
            return t.ToNuGenTrafo3D() * r.ToNuGenTrafo3D();
        }

        public static NuGenTrafo3D operator *(NuGenScale3D t, NuGenRot3D r)
        {
            return t.ToNuGenTrafo3D() * r.ToNuGenTrafo3D();
        }

        private NuGenVec3D v;
        private double r;

    }

}

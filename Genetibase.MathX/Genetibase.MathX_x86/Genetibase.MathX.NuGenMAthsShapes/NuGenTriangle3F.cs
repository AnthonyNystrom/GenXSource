using System;

namespace Genetibase.MathX.NuGenMAthsShapes
{

    /// <summary>
    /// NuGenTriangle3F.
    /// </summary>
    public struct NuGenTriangle3F
    {

        public NuGenTriangle3F(NuGenPnt3F a, NuGenPnt3F b, NuGenPnt3F c)
        {
            p0 = a; p1 = b; p2 = c;
        }

        public NuGenPnt3F this[int index]
        {
            get
            {

                switch (index)
                {

                    case 0: return p0;
                    case 1: return p1;
                    case 2: return p2;
                    default: return p0;
                }
            }
            set
            {

                switch (index)
                {

                    case 0: p0 = value; break;
                    case 1: p1 = value; break;
                    case 2: p2 = value; break;
                }
            }
        }

        public bool Intersect(NuGenRay3F ray, ref float t, ref float u, ref float v, ref NuGenVec3F normal)
        {
            NuGenVec3F e1 = p1 - p0;
            NuGenVec3F e2 = p2 - p0;
            NuGenVec3F p = NuGenVec3F.Cross(ray.Direction, e2);

            float a = NuGenVec3F.Dot(e1, p);

            if (a > -NuGenVector.TINY_FLOAT && a < NuGenVector.TINY_FLOAT) return false;
            float f = 1.0f / a;
            NuGenVec3F s = ray.Point - p0;
            u = f * NuGenVec3F.Dot(s, p);

            if (u < 0.0f || u > 1.0f) return false;
            NuGenVec3F q = NuGenVec3F.Cross(s, e1);
            v = f * NuGenVec3F.Dot(ray.Direction, q);

            if (v < 0.0f || (u + v) > 1.0f) return false;
            t = f * NuGenVec3F.Dot(e2, q);
            normal = NuGenVec3F.Cross(e1, e2); normal.Normalize();
            return true;
        }

        public NuGenPnt3F p0;
        public NuGenPnt3F p1;
        public NuGenPnt3F p2;

    }

}

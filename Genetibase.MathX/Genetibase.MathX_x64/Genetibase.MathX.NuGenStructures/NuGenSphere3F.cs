using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenSphere3F.
    /// </summary>
    public struct NuGenSphere3F
    {

        public static NuGenSphere3F UnitSphere = new NuGenSphere3F(new NuGenPnt3F(0, 0, 0), 1.0f);

        public NuGenSphere3F(NuGenPnt3F center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public NuGenPnt3F Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        public bool Intersect(NuGenRay3F ray, ref float t, ref NuGenVec3F normal)
        {
            NuGenVec3F l = center - ray.Point;
            float s = NuGenVec3F.Dot(l, ray.Direction);
            float l2 = l.SquaredLength;
            float rr = radius * radius;

            if (s < 0.0f && l2 > rr) return false;
            float m2 = l2 - s * s;

            if (m2 > rr) return false;
            float q = (float)Math.Sqrt(rr - m2);

            if (l2 > rr) t = s - q;

            else t = s + q;
            normal = (ray.Point + ray.Direction * t) - center;
            normal.Normalize();
            return true;
        }

        private NuGenPnt3F center;
        private float radius;

    }

}

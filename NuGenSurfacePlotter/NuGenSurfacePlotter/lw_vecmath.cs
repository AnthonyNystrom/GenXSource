using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class LwVecMath
    {
        public static float dot(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        public static void cross(float[] a, float[] b, float[] c)
        {
            c[0] = a[1] * b[2] - a[2] * b[1];
            c[1] = a[2] * b[0] - a[0] * b[2];
            c[2] = a[0] * b[1] - a[1] * b[0];
        }

        public static void normalize(float[] v)
        {
            float r = (float)Math.Sqrt(dot(v, v));
            if (r > 0)
            {
                v[0] /= r;
                v[1] /= r;
                v[2] /= r;
            }
        }

        public static float vecangle(float[] a, float[] b)
        {
            return (float)Math.Acos(dot(a, b));
        }
    }
}
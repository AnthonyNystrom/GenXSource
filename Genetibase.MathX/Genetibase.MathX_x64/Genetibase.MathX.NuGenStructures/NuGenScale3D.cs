using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenScale3D.
    /// </summary>
    public struct NuGenScale3D
    {

        public static NuGenScale3D Identity = new NuGenScale3D(1, 1, 1);

        public NuGenScale3D(double x, double y, double z)
        {
            v = new NuGenVec3D(x, y, z);
        }

        public NuGenScale3D(NuGenVec3D shift)
        {
            v = shift;
        }

        public double x
        {
            get
            {
                return v._x[0];
            }
        }

        public double y
        {
            get
            {
                return v._x[1];
            }
        }

        public double z
        {
            get
            {
                return v._x[2];
            }
        }

        public NuGenTrafo3D ToNuGenTrafo3D()
        {
            return new NuGenTrafo3D(
                v._x[0], 0, 0, 0,
                0, v._x[1], 0, 0,
                0, 0, v._x[2], 0,
                0, 0, 0, 1
                );
        }

        public static implicit operator NuGenTrafo3D(NuGenScale3D s)
        {
            return s.ToNuGenTrafo3D();
        }

        public static NuGenVec3D operator *(NuGenVec3D v, NuGenScale3D t)
        {
            return new NuGenVec3D(
                v._x[0] * t.v._x[0],
                v._x[1] * t.v._x[1],
                v._x[2] * t.v._x[2]
                );
        }

        public static NuGenPnt3D operator *(NuGenPnt3D p, NuGenScale3D t)
        {
            return new NuGenPnt3D(
                p._x[0] * t.v._x[0],
                p._x[1] * t.v._x[1],
                p._x[2] * t.v._x[2]
                );
        }

        public static NuGenRay3D operator *(NuGenRay3D r, NuGenScale3D t)
        {
            return new NuGenRay3D(r.p * t, r.v * t);
        }

        public static NuGenBox3D operator *(NuGenBox3D b, NuGenScale3D t)
        {
            return new NuGenBox3D(b.Lower * t, b.Upper * t);
        }

        public static NuGenTrafo3D operator *(NuGenShift3D a, NuGenScale3D b)
        {
            return a.ToNuGenTrafo3D() * b.ToNuGenTrafo3D();
        }

        public static NuGenTrafo3D operator *(NuGenRot3D a, NuGenScale3D b)
        {
            return a.ToNuGenTrafo3D() * b.ToNuGenTrafo3D();
        }

        private NuGenVec3D v;

    }

}

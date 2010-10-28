using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenVec2L.
    /// </summary>
    public struct NuGenVec2L
    {

        public static NuGenVec2L Identity = new NuGenVec2L(0);
        public static NuGenVec2L UnitX = new NuGenVec2L(1, 0);
        public static NuGenVec2L UnitY = new NuGenVec2L(0, 1);

        public NuGenVec2L(long xy)
        {
            _x = new long[2];
            _x[0] = xy; _x[1] = xy;
        }

        public NuGenVec2L(long x, long y)
        {
            _x = new long[2];
            _x[0] = x; _x[1] = y;
        }

        public NuGenVec2L(ref long[] x)
        {
            _x = new long[2];
            _x[0] = x[0]; _x[1] = x[1];
        }

        public long this[int index]
        {
            get
            {
                return _x[index];
            }
            set
            {
                _x[index] = value;
            }
        }

        public long x
        {
            get
            {
                return _x[0];
            }
            set
            {
                _x[0] = value;
            }
        }

        public long y
        {
            get
            {
                return _x[1];
            }
            set
            {
                _x[1] = value;
            }
        }

        public static explicit operator NuGenVec2L(NuGenVec2F v)
        {
            return new NuGenVec2L((long)v.x, (long)v.y);
        }

        public static explicit operator NuGenVec2L(NuGenVec2D v)
        {
            return new NuGenVec2L((long)v.x, (long)v.y);
        }

        public static NuGenVec2L operator +(NuGenVec2L v, long f)
        {
            return new NuGenVec2L(v[0] + f, v[1] + f);
        }

        public static NuGenVec2L operator +(NuGenVec2L v, NuGenVec2L w)
        {
            return new NuGenVec2L(v[0] + w[0], v[1] + w[1]);
        }

        public static NuGenVec2L operator -(NuGenVec2L v, long f)
        {
            return new NuGenVec2L(v[0] - f, v[1] - f);
        }

        public static NuGenVec2L operator -(NuGenVec2L v, NuGenVec2L w)
        {
            return new NuGenVec2L(v[0] - w[0], v[1] - w[1]);
        }

        public static NuGenVec2L operator -(NuGenVec2L v)
        {
            return new NuGenVec2L(-v[0], -v[1]);
        }

        public static NuGenVec2L operator *(NuGenVec2L v, long f)
        {
            return new NuGenVec2L(v[0] * f, v[1] * f);
        }

        public static NuGenVec2L operator *(NuGenVec2L v, NuGenVec2L w)
        {
            return new NuGenVec2L(v[0] * w[0], v[1] * w[1]);
        }

        public static NuGenVec2L operator /(NuGenVec2L v, long f)
        {
            return new NuGenVec2L(v[0] / f, v[1] / f);
        }

        public static NuGenVec2L operator %(NuGenVec2L v, long f)
        {
            return new NuGenVec2L(v[0] % f, v[1] % f);
        }

        public static long Dot(NuGenVec2L u, NuGenVec2L v)
        {
            return u[0] * v[0] + u[1] * v[1];
        }

        public double SquaredLength
        {
            get
            {
                return _x[0] * _x[0] + _x[1] * _x[1];
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(SquaredLength);
            }
        }

        public int MinDim
        {
            get
            {
                return (_x[0] < _x[1]) ? 0 : 1;
            }
        }

        public int MaxDim
        {
            get
            {
                return (_x[0] > _x[1]) ? 0 : 1;
            }
        }

        public long Minimum
        {
            get
            {
                return (_x[0] < _x[1]) ? _x[0] : _x[1];
            }
        }

        public long Maximum
        {
            get
            {
                return (_x[0] > _x[1]) ? _x[0] : _x[1];
            }
        }

        public static bool operator <(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] < b._x[0] &&
                a._x[1] < b._x[1]
                );
        }

        public static bool operator <=(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] <= b._x[0] &&
                a._x[1] <= b._x[1]
                );
        }

        public static bool operator ==(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] == b._x[0] &&
                a._x[1] == b._x[1]
                );
        }

        public static bool operator !=(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] != b._x[0] ||
                a._x[1] != b._x[1]
                );
        }

        public static bool operator >=(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] >= b._x[0] &&
                a._x[1] >= b._x[1]
                );
        }

        public static bool operator >(NuGenVec2L a, NuGenVec2L b)
        {
            return (
                a._x[0] > b._x[0] &&
                a._x[1] > b._x[1]
                );
        }

        public static NuGenVec2L Min(NuGenVec2L a, NuGenVec2L b)
        {
            return new NuGenVec2L(
                Math.Min(a._x[0], b._x[0]),
                Math.Min(a._x[1], b._x[1])
                );
        }

        public static NuGenVec2L Max(NuGenVec2L a, NuGenVec2L b)
        {
            return new NuGenVec2L(
                Math.Max(a._x[0], b._x[0]),
                Math.Max(a._x[1], b._x[1])
                );
        }

        internal long[] _x;

        public override bool Equals(object obj)
        {
            NuGenVec2L x = (NuGenVec2L)obj;
            return (
                _x[0] == x._x[0] &&
                _x[1] == x._x[1]
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "NuGenVec2L(" + _x[0] + ", " + _x[1] + ")";
        }

        static NuGenVec2L()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenVec2I.
    /// </summary>
    public struct NuGenVec2I
    {

        public static NuGenVec2I Identity = new NuGenVec2I(0);
        public static NuGenVec2I UnitX = new NuGenVec2I(1, 0);
        public static NuGenVec2I UnitY = new NuGenVec2I(0, 1);

        public NuGenVec2I(int xy)
        {
            _x = new int[2];
            _x[0] = xy; _x[1] = xy;
        }

        public NuGenVec2I(int x, int y)
        {
            _x = new int[2];
            _x[0] = x; _x[1] = y;
        }

        public NuGenVec2I(ref int[] x)
        {
            _x = new int[2];
            _x[0] = x[0]; _x[1] = x[1];
        }

        public int this[int index]
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

        public int x
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

        public int y
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

        public static explicit operator NuGenVec2I(NuGenVec2F v)
        {
            return new NuGenVec2I((int)v.x, (int)v.y);
        }

        public static explicit operator NuGenVec2I(NuGenVec2D v)
        {
            return new NuGenVec2I((int)v.x, (int)v.y);
        }

        public static NuGenVec2I operator +(NuGenVec2I v, int f)
        {
            return new NuGenVec2I(v[0] + f, v[1] + f);
        }

        public static NuGenVec2I operator +(NuGenVec2I v, NuGenVec2I w)
        {
            return new NuGenVec2I(v[0] + w[0], v[1] + w[1]);
        }

        public static NuGenVec2I operator -(NuGenVec2I v, int f)
        {
            return new NuGenVec2I(v[0] - f, v[1] - f);
        }

        public static NuGenVec2I operator -(NuGenVec2I v, NuGenVec2I w)
        {
            return new NuGenVec2I(v[0] - w[0], v[1] - w[1]);
        }

        public static NuGenVec2I operator -(NuGenVec2I v)
        {
            return new NuGenVec2I(-v[0], -v[1]);
        }

        public static NuGenVec2I operator *(NuGenVec2I v, int f)
        {
            return new NuGenVec2I(v[0] * f, v[1] * f);
        }

        public static NuGenVec2I operator *(NuGenVec2I v, NuGenVec2I w)
        {
            return new NuGenVec2I(v[0] * w[0], v[1] * w[1]);
        }

        public static NuGenVec2I operator /(NuGenVec2I v, int f)
        {
            return new NuGenVec2I(v[0] / f, v[1] / f);
        }

        public static NuGenVec2I operator %(NuGenVec2I v, int f)
        {
            return new NuGenVec2I(v[0] % f, v[1] % f);
        }

        public static int Dot(NuGenVec2I u, NuGenVec2I v)
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

        public int Minimum
        {
            get
            {
                return (_x[0] < _x[1]) ? _x[0] : _x[1];
            }
        }

        public int Maximum
        {
            get
            {
                return (_x[0] > _x[1]) ? _x[0] : _x[1];
            }
        }

        public static bool operator <(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] < b._x[0] &&
                a._x[1] < b._x[1]
                );
        }

        public static bool operator <=(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] <= b._x[0] &&
                a._x[1] <= b._x[1]
                );
        }

        public static bool operator ==(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] == b._x[0] &&
                a._x[1] == b._x[1]
                );
        }

        public static bool operator !=(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] != b._x[0] ||
                a._x[1] != b._x[1]
                );
        }

        public static bool operator >=(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] >= b._x[0] &&
                a._x[1] >= b._x[1]
                );
        }

        public static bool operator >(NuGenVec2I a, NuGenVec2I b)
        {
            return (
                a._x[0] > b._x[0] &&
                a._x[1] > b._x[1]
                );
        }

        public static NuGenVec2I Min(NuGenVec2I a, NuGenVec2I b)
        {
            return new NuGenVec2I(
                Math.Min(a._x[0], b._x[0]),
                Math.Min(a._x[1], b._x[1])
                );
        }

        public static NuGenVec2I Max(NuGenVec2I a, NuGenVec2I b)
        {
            return new NuGenVec2I(
                Math.Max(a._x[0], b._x[0]),
                Math.Max(a._x[1], b._x[1])
                );
        }

        internal int[] _x;

        public override bool Equals(object obj)
        {
            NuGenVec2I x = (NuGenVec2I)obj;
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
            return "NuGenVec2I(" + _x[0] + ", " + _x[1] + ")";
        }

        static NuGenVec2I()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;
        }
    }
    
}

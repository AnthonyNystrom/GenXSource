using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenPnt3F.
    /// </summary>
    public struct NuGenPnt3F
    {

        public static NuGenPnt3F Identity = new NuGenPnt3F(0, 0, 0);

        public NuGenPnt3F(float xyz)
        {
            _x = new float[3];
            _x[0] = xyz; _x[1] = xyz; _x[2] = xyz;
        }

        public NuGenPnt3F(float x, float y, float z)
        {
            _x = new float[3];
            _x[0] = x;
            _x[1] = y;
            _x[2] = z;
        }

        public NuGenPnt3F(ref float[] x)
        {
            _x = new float[3];
            _x[0] = x[0]; _x[1] = x[1]; _x[2] = x[2];
        }

        public float this[int index]
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

        public float x
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

        public float y
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

        public float z
        {
            get
            {
                return _x[2];
            }
            set
            {
                _x[2] = value;
            }
        }

        public static explicit operator NuGenPnt3F(NuGenPnt3D v)
        {
            return new NuGenPnt3F((float)v.x, (float)v.y, (float)v.z);
        }

        public static explicit operator NuGenPnt3F(NuGenVec3F v)
        {
            return new NuGenPnt3F(v.x, v.y, v.z);
        }

        public static NuGenPnt3F operator +(NuGenPnt3F v, float f)
        {
            return new NuGenPnt3F(v[0] + f, v[1] + f, v[2] + f);
        }

        public static NuGenPnt3F operator +(NuGenPnt3F v, NuGenPnt3F w)
        {
            return new NuGenPnt3F(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
        }

        public static NuGenPnt3F operator +(NuGenPnt3F v, NuGenVec3F w)
        {
            return new NuGenPnt3F(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
        }

        public static NuGenPnt3F operator -(NuGenPnt3F v)
        {
            return new NuGenPnt3F(-v[0], -v[1], -v[2]);
        }

        public static NuGenPnt3F operator -(NuGenPnt3F v, float f)
        {
            return new NuGenPnt3F(v[0] - f, v[1] - f, v[2] - f);
        }

        public static NuGenVec3F operator -(NuGenPnt3F v, NuGenPnt3F w)
        {
            return new NuGenVec3F(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
        }

        public static NuGenPnt3F operator -(NuGenPnt3F v, NuGenVec3F w)
        {
            return new NuGenPnt3F(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
        }

        public static NuGenPnt3F operator *(NuGenPnt3F v, float f)
        {
            return new NuGenPnt3F(v[0] * f, v[1] * f, v[2] * f);
        }

        public static NuGenPnt3F operator /(NuGenPnt3F v, float f)
        {
            return new NuGenPnt3F(v[0] / f, v[1] / f, v[2] / f);
        }

        public int MinDim
        {
            get
            {
                return (_x[0] < _x[1])
                    ? ((_x[0] < _x[2]) ? 0 : 2)
                    : ((_x[1] < _x[2]) ? 1 : 2);
            }
        }

        public int MaxDim
        {
            get
            {
                return (_x[0] > _x[1])
                    ? ((_x[0] > _x[2]) ? 0 : 2)
                    : ((_x[1] > _x[2]) ? 1 : 2);
            }
        }

        public float Minimum
        {
            get
            {
                return (_x[0] < _x[1])
                    ? ((_x[0] < _x[2]) ? _x[0]
                    : _x[2]) : ((_x[1] < _x[2]) ? _x[1] : _x[2]);
            }
        }

        public float Maximum
        {
            get
            {
                return (_x[0] > _x[1])
                    ? ((_x[0] > _x[2]) ? _x[0]
                    : _x[2]) : ((_x[1] > _x[2]) ? _x[1] : _x[2]);
            }
        }

        public static bool operator <(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] < b._x[0] &&
                a._x[1] < b._x[1] &&
                a._x[2] < b._x[2]
                );
        }

        public static bool operator <=(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] <= b._x[0] &&
                a._x[1] <= b._x[1] &&
                a._x[2] <= b._x[2]
                );
        }

        public static bool operator ==(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] == b._x[0] &&
                a._x[1] == b._x[1] &&
                a._x[2] == b._x[2]
                );
        }

        public static bool operator !=(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] != b._x[0] ||
                a._x[1] != b._x[1] ||
                a._x[2] != b._x[2]
                );
        }

        public static bool operator >=(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] >= b._x[0] &&
                a._x[1] >= b._x[1] &&
                a._x[2] >= b._x[2]
                );
        }

        public static bool operator >(NuGenPnt3F a, NuGenPnt3F b)
        {
            return (
                a._x[0] > b._x[0] &&
                a._x[1] > b._x[1] &&
                a._x[2] > b._x[2]
                );
        }

        public static NuGenPnt3F Min(NuGenPnt3F a, NuGenPnt3F b)
        {
            return new NuGenPnt3F(
                Math.Min(a._x[0], b._x[0]),
                Math.Min(a._x[1], b._x[1]),
                Math.Min(a._x[2], b._x[2])
                );
        }

        public static NuGenPnt3F Max(NuGenPnt3F a, NuGenPnt3F b)
        {
            return new NuGenPnt3F(
                Math.Max(a._x[0], b._x[0]),
                Math.Max(a._x[1], b._x[1]),
                Math.Max(a._x[2], b._x[2])
                );
        }

        internal float[] _x;

        public override bool Equals(object obj)
        {
            NuGenPnt3F x = (NuGenPnt3F)obj;
            return (
                _x[0] == x._x[0] &&
                _x[1] == x._x[1] &&
                _x[2] == x._x[2]
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "NuGenPnt3F(" + _x[0] + ", " + _x[1] + ", " + _x[2] + ")";
        }

    }


}

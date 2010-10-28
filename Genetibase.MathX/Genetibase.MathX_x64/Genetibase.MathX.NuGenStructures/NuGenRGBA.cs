using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{
    /// <summary>
    /// NuGenRGBA.
    /// </summary>
    public struct NuGenRGBA
    {

        public NuGenRGBA(float r, float g, float b)
        {
            _x = new float[4];
            _x[0] = r;
            _x[1] = g;
            _x[2] = b;
            _x[3] = 1.0f;
        }

        public NuGenRGBA(float r, float g, float b, float a)
        {
            _x = new float[4];
            _x[0] = r;
            _x[1] = g;
            _x[2] = b;
            _x[3] = a;
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

        public float r
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

        public float g
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

        public float b
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

        public float a
        {
            get
            {
                return _x[3];
            }
            set
            {
                _x[3] = value;
            }
        }

        public static NuGenRGBA operator +(NuGenRGBA v, float f)
        {
            return new NuGenRGBA(v[0] + f, v[1] + f, v[2] + f, v[3] + f);
        }

        public static NuGenRGBA operator +(NuGenRGBA v, NuGenRGBA w)
        {
            return new NuGenRGBA(v[0] + w[0], v[1] + w[1], v[2] + w[2], v[3] + w[3]);
        }

        public static NuGenRGBA operator -(NuGenRGBA v, float f)
        {
            return new NuGenRGBA(v[0] - f, v[1] - f, v[2] - f, v[3] - f);
        }

        public static NuGenRGBA operator -(NuGenRGBA v, NuGenRGBA w)
        {
            return new NuGenRGBA(v[0] - w[0], v[1] - w[1], v[2] - w[2], v[3] - w[3]);
        }

        public static NuGenRGBA operator *(NuGenRGBA v, float f)
        {
            return new NuGenRGBA(v[0] * f, v[1] * f, v[2] * f, v[3] * f);
        }

        public static NuGenRGBA operator /(NuGenRGBA v, float f)
        {
            return new NuGenRGBA(v[0] / f, v[1] / f, v[2] / f, v[3] / f);
        }

        public void Clamp()
        {

            if (_x[0] < 0) _x[0] = 0;

            else if (_x[0] > 1) _x[0] = 1;

            if (_x[1] < 0) _x[1] = 0;

            else if (_x[1] > 1) _x[1] = 1;

            if (_x[2] < 0) _x[2] = 0;

            else if (_x[2] > 1) _x[2] = 1;

            if (_x[3] < 0) _x[3] = 0;

            else if (_x[3] > 1) _x[3] = 1;
        }

        public NuGenRGBA Clamped
        {
            get
            {
                NuGenRGBA result = this;
                result.Clamp();
                return result;
            }
        }

        public float SquaredLength
        {
            get
            {
                return _x[0] * _x[0] + _x[1] * _x[1] + _x[2] * _x[2] + _x[3] * _x[3];
            }
        }

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(
                    _x[0] * _x[0] + _x[1] * _x[1] + _x[2] * _x[2] + _x[3] * _x[3]
                    );
            }
        }

        public static implicit operator float[](NuGenRGBA c)
        {
            return c._x;
        }

        internal float[] _x;

    }
    
}

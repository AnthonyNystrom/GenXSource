using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.MathX.NuGenStructures
{

    /// <summary>
    /// NuGenTrafo2F.
    /// </summary>
    public struct NuGenTrafo2F
    {

        public static NuGenTrafo2F Identity = new NuGenTrafo2F(
            1, 0, 0, 0, 1, 0, 0, 0, 1
            );

        public NuGenTrafo2F(
            float x00, float x01, float x02,
            float x10, float x11, float x12,
            float x20, float x21, float x22
            )
        {
            _x = new float[9];
            _x[0] = x00; _x[1] = x01; _x[2] = x02;
            _x[3] = x10; _x[4] = x11; _x[5] = x12;
            _x[6] = x20; _x[7] = x21; _x[8] = x22;
        }

        public NuGenTrafo2F(NuGenRot2F r)
        {
            this = r.ToNuGenTrafo2F();
        }

        public float this[int i, int j]
        {
            get
            {
                return _x[i * 3 + j];
            }
            set
            {
                _x[i * 3 + j] = value;
            }
        }

        public NuGenTrafo2F Transposed
        {
            get
            {
                return new NuGenTrafo2F(
                    _x[0], _x[3], _x[6],
                    _x[1], _x[4], _x[7],
                    _x[2], _x[5], _x[8]
                    );
            }
        }

        public void Transpose()
        {
            this = this.Transposed;
        }

        public float Det
        {
            get
            {
                return
                    _x[0] * _x[4] * _x[8] + _x[1] * _x[5] * _x[6] + _x[2] * _x[3] * _x[7]
                    - _x[6] * _x[4] * _x[2] - _x[7] * _x[5] * _x[0] - _x[8] * _x[3] * _x[1];
            }
        }

        public NuGenTrafo2F Adjoint
        {
            get
            {
                return new NuGenTrafo2F(
                    NuGenVector.Det2x2(
                    _x[4], _x[5],
                    _x[7], _x[8]), // 00
                    -NuGenVector.Det2x2(
                    _x[3], _x[5],
                    _x[6], _x[8]), // 01
                    NuGenVector.Det2x2(
                    _x[3], _x[4],
                    _x[6], _x[7]), // 02

                    -NuGenVector.Det2x2(
                    _x[1], _x[2],
                    _x[7], _x[8]), // 10
                    NuGenVector.Det2x2(
                    _x[0], _x[2],
                    _x[6], _x[8]), // 11
                    -NuGenVector.Det2x2(
                    _x[0], _x[1],
                    _x[6], _x[7]), // 12

                    NuGenVector.Det2x2(
                    _x[1], _x[2],
                    _x[4], _x[5]), // 20
                    -NuGenVector.Det2x2(
                    _x[0], _x[2],
                    _x[3], _x[5]), // 21
                    NuGenVector.Det2x2(
                    _x[0], _x[1],
                    _x[3], _x[4])  // 22

                    );
            }
        }

        public NuGenTrafo2F Inverse
        {
            get
            {
                float d = Det;

                if (d == 0) throw new ArithmeticException("Singular Matrix");
                d = 1.0f / d;
                return Adjoint.Transposed * d;
            }
        }

        public void Invert()
        {
            this = Inverse;
        }

        public static NuGenTrafo2F operator +(NuGenTrafo2F t, float f)
        {
            return new NuGenTrafo2F(
                t._x[0] + f, t._x[1] + f, t._x[2] + f,
                t._x[3] + f, t._x[4] + f, t._x[5] + f,
                t._x[6] + f, t._x[7] + f, t._x[8] + f
                );
        }

        public static NuGenTrafo2F operator -(NuGenTrafo2F t, float f)
        {
            return new NuGenTrafo2F(
                t._x[0] - f, t._x[1] - f, t._x[2] - f,
                t._x[3] - f, t._x[4] - f, t._x[5] - f,
                t._x[6] - f, t._x[7] - f, t._x[8] - f
                );
        }

        public static NuGenTrafo2F operator *(NuGenTrafo2F t, float f)
        {
            return new NuGenTrafo2F(
                t._x[0] * f, t._x[1] * f, t._x[2] * f,
                t._x[3] * f, t._x[4] * f, t._x[5] * f,
                t._x[6] * f, t._x[7] * f, t._x[8] * f
                );
        }

        public static NuGenTrafo2F operator /(NuGenTrafo2F t, float f)
        {
            return new NuGenTrafo2F(
                t._x[0] / f, t._x[1] / f, t._x[2] / f,
                t._x[3] / f, t._x[4] / f, t._x[5] / f,
                t._x[6] / f, t._x[7] / f, t._x[8] / f
                );
        }

        public static NuGenPnt2F operator *(NuGenPnt2F p, NuGenTrafo2F t)
        {
            float f = 1.0f / (p[0] * t._x[2] + p[1] * t._x[5] + t._x[8]);
            return new NuGenPnt2F(
                (p[0] * t._x[0] + p[1] * t._x[3] + t._x[6]) * f,
                (p[0] * t._x[1] + p[1] * t._x[4] + t._x[7]) * f
                );
        }

        public static NuGenVec2F operator *(NuGenVec2F v, NuGenTrafo2F t)
        {
            return new NuGenVec2F(
                v[0] * t._x[0] + v[1] * t._x[3],
                v[0] * t._x[1] + v[1] * t._x[4]
                );
        }

        public static NuGenBox2F operator *(NuGenBox2F b, NuGenTrafo2F t)
        {
            NuGenBox2F result = NuGenBox2F.Empty;
            result += b.LL * t;
            result += b.LU * t;
            result += b.UL * t;
            result += b.UU * t;
            return result;
        }

        public static NuGenTrafo2F operator *(NuGenTrafo2F a, NuGenTrafo2F b)
        {
            return new NuGenTrafo2F(

                a._x[0] * b._x[0] + a._x[1] * b._x[3] + a._x[2] * b._x[6],
                a._x[0] * b._x[1] + a._x[1] * b._x[4] + a._x[2] * b._x[7],
                a._x[0] * b._x[2] + a._x[1] * b._x[5] + a._x[2] * b._x[8],

                a._x[3] * b._x[0] + a._x[4] * b._x[3] + a._x[5] * b._x[6],
                a._x[3] * b._x[1] + a._x[4] * b._x[4] + a._x[5] * b._x[7],
                a._x[3] * b._x[2] + a._x[4] * b._x[5] + a._x[5] * b._x[8],

                a._x[6] * b._x[0] + a._x[7] * b._x[3] + a._x[8] * b._x[6],
                a._x[6] * b._x[1] + a._x[7] * b._x[4] + a._x[8] * b._x[7],
                a._x[6] * b._x[2] + a._x[7] * b._x[5] + a._x[8] * b._x[8]

                );
        }

        internal float[] _x;

        public float[] Array
        {
            get
            {
                return _x;
            }
        }

    }


}

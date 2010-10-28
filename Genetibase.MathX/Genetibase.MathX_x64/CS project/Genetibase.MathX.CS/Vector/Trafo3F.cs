
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenTrafo3F.
	/// </summary>
	public struct NuGenTrafo3F
	{

		public static NuGenTrafo3F Identity = new NuGenTrafo3F(
			1,0,0,0,
			0,1,0,0,
			0,0,1,0,
			0,0,0,1
			);

		public NuGenTrafo3F(
			float x00, float x01, float x02, float x03,
			float x10, float x11, float x12, float x13,
			float x20, float x21, float x22, float x23,
			float x30, float x31, float x32, float x33
			)
		{
			_x = new float[16];
			_x[ 0] = x00; _x[ 1] = x01; _x[ 2] = x02; _x[ 3] = x03;
			_x[ 4] = x10; _x[ 5] = x11; _x[ 6] = x12; _x[ 7] = x13;
			_x[ 8] = x20; _x[ 9] = x21; _x[10] = x22; _x[11] = x23;
			_x[12] = x30; _x[13] = x31; _x[14] = x32; _x[15] = x33;
		}

		public NuGenTrafo3F(NuGenRot3F r)
		{
			this = r.ToNuGenTrafo3F();
		}

		public NuGenTrafo3F(NuGenTrafo2F t)
		{
			_x = new float[16];
			_x[ 0] = t[0,0]; _x[ 1] = t[0,1]; _x[ 2] = 0.0f; _x[ 3] = t[0,2];
			_x[ 4] = t[1,0]; _x[ 5] = t[1,1]; _x[ 6] = 0.0f; _x[ 7] = t[1,2];
			_x[ 8] = 0.0f; _x[ 9] = 0.0f; _x[10] = 1.0f; _x[11] = 0.0f;
			_x[12] = t[2,0]; _x[13] = t[2,1]; _x[14] = 0.0f; _x[15] = t[2,2];
		}

		public static explicit operator NuGenTrafo3F(NuGenTrafo3D t)
		{
			return new NuGenTrafo3F(
				(float)t._x[ 0], (float)t._x[ 1], (float)t._x[ 2], (float)t._x[ 3],
				(float)t._x[ 4], (float)t._x[ 5], (float)t._x[ 6], (float)t._x[ 7], 
				(float)t._x[ 8], (float)t._x[ 9], (float)t._x[10], (float)t._x[11], 
				(float)t._x[12], (float)t._x[13], (float)t._x[14], (float)t._x[15]
				);
		}

		public float this [int i]
		{
			get 
			{
				return _x[i]; 
			}
			set 
			{
				_x[i] = value; 
			}
		}

		public float this [int i, int j]
		{
			get 
			{
				return _x[i*4+j]; 
			}
			set 
			{
				_x[i*4+j] = value; 
			}
		}

		public NuGenTrafo3F Transposed
		{
			get
			{
				return new NuGenTrafo3F(
					_x[0], _x[4], _x[ 8], _x[12],
					_x[1], _x[5], _x[ 9], _x[13],
					_x[2], _x[6], _x[10], _x[14],
					_x[3], _x[7], _x[11], _x[15]
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
				return NuGenVector.Det4x4(_x); 
			}
		}

		public NuGenTrafo3F Adjoint
		{
			get
			{
				return new NuGenTrafo3F(
					NuGenVector.Det3x3(
					_x[ 5], _x[ 6], _x[ 7],
					_x[ 9], _x[10], _x[11],
					_x[13], _x[14], _x[15]), // 00
					- NuGenVector.Det3x3(
					_x[ 4], _x[ 6], _x[ 7],
					_x[ 8], _x[10], _x[11],
					_x[12], _x[14], _x[15]), // 01
					NuGenVector.Det3x3(
					_x[ 4], _x[ 5], _x[ 7],
					_x[ 8], _x[ 9], _x[11],
					_x[12], _x[13], _x[15]), // 02
					- NuGenVector.Det3x3(
					_x[ 4], _x[ 5], _x[ 6],
					_x[ 8], _x[ 9], _x[10],
					_x[12], _x[13], _x[14]), // 03

					NuGenVector.Det3x3(
					_x[ 1], _x[ 2], _x[ 3],
					_x[ 9], _x[10], _x[11],
					_x[13], _x[14], _x[15]), // 10
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 2], _x[ 3],
					_x[ 8], _x[10], _x[11],
					_x[12], _x[14], _x[15]), // 11
					NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 3],
					_x[ 8], _x[ 9], _x[11],
					_x[12], _x[13], _x[15]), // 12
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 2],
					_x[ 8], _x[ 9], _x[10],
					_x[12], _x[13], _x[14]), // 13

					NuGenVector.Det3x3(
					_x[ 1], _x[ 2], _x[ 3],
					_x[ 5], _x[ 6], _x[ 7],
					_x[13], _x[14], _x[15]), // 20
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 2], _x[ 3],
					_x[ 4], _x[ 6], _x[ 7],
					_x[12], _x[14], _x[15]), // 21
					NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 3],
					_x[ 4], _x[ 5], _x[ 7],
					_x[12], _x[13], _x[15]), // 22
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 2],
					_x[ 4], _x[ 5], _x[ 6],
					_x[12], _x[13], _x[14]), // 23

					NuGenVector.Det3x3(
					_x[ 1], _x[ 2], _x[ 3],
					_x[ 5], _x[ 6], _x[ 7],
					_x[ 9], _x[10], _x[11]), // 24
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 2], _x[ 3],
					_x[ 4], _x[ 6], _x[ 7],
					_x[ 8], _x[10], _x[11]), // 25
					NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 3],
					_x[ 4], _x[ 5], _x[ 7],
					_x[ 8], _x[ 9], _x[11]), // 26
					- NuGenVector.Det3x3(
					_x[ 0], _x[ 1], _x[ 2],
					_x[ 4], _x[ 5], _x[ 6],
					_x[ 8], _x[ 9], _x[10])  // 27
					);
			}
		}

		public NuGenTrafo3F Inverse
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

		public static NuGenTrafo3F operator+(NuGenTrafo3F t, float f)
		{
			return new NuGenTrafo3F(
				t._x[ 0] + f, t._x[ 1] + f, t._x[ 2] + f, t._x[ 3] + f,
				t._x[ 4] + f, t._x[ 5] + f, t._x[ 6] + f, t._x[ 7] + f,
				t._x[ 8] + f, t._x[ 9] + f, t._x[10] + f, t._x[11] + f,
				t._x[12] + f, t._x[13] + f, t._x[14] + f, t._x[15] + f
				);
		}

		public static NuGenTrafo3F operator-(NuGenTrafo3F t, float f)
		{
			return new NuGenTrafo3F(
				t._x[ 0] - f, t._x[ 1] - f, t._x[ 2] - f, t._x[ 3] - f,
				t._x[ 4] - f, t._x[ 5] - f, t._x[ 6] - f, t._x[ 7] - f,
				t._x[ 8] - f, t._x[ 9] - f, t._x[10] - f, t._x[11] - f,
				t._x[12] - f, t._x[13] - f, t._x[14] - f, t._x[15] - f
				);
		}

		public static NuGenTrafo3F operator*(NuGenTrafo3F t, float f)
		{
			return new NuGenTrafo3F(
				t._x[ 0] * f, t._x[ 1] * f, t._x[ 2] * f, t._x[ 3] * f,
				t._x[ 4] * f, t._x[ 5] * f, t._x[ 6] * f, t._x[ 7] * f,
				t._x[ 8] * f, t._x[ 9] * f, t._x[10] * f, t._x[11] * f,
				t._x[12] * f, t._x[13] * f, t._x[14] * f, t._x[15] * f
				);
		}

		public static NuGenTrafo3F operator/(NuGenTrafo3F t, float f)
		{
			return new NuGenTrafo3F(
				t._x[ 0] / f, t._x[ 1] / f, t._x[ 2] / f, t._x[ 3] / f,
				t._x[ 4] / f, t._x[ 5] / f, t._x[ 6] / f, t._x[ 7] / f,
				t._x[ 8] / f, t._x[ 9] / f, t._x[10] / f, t._x[11] / f,
				t._x[12] / f, t._x[13] / f, t._x[14] / f, t._x[15] / f
				);
		}

		public static NuGenPnt3F operator*(NuGenPnt3F p, NuGenTrafo3F t)
		{
			float f = 1.0f / (p[0]*t[0,3]+p[1]*t[1,3]+p[2]*t[2,3]+t[3,3]);
			return new NuGenPnt3F(
				(p[0] * t[0,0] + p[1] * t[1,0] + p[2] * t[2,0] + t[3,0]) * f,
				(p[0] * t[0,1] + p[1] * t[1,1] + p[2] * t[2,1] + t[3,1]) * f,
				(p[0] * t[0,2] + p[1] * t[1,2] + p[2] * t[2,2] + t[3,2]) * f
				);
		}

		public static NuGenVec3F operator*(NuGenVec3F v, NuGenTrafo3F t)
		{
			return new NuGenVec3F(
				v._x[0] * t._x[0] + v._x[1] * t._x[4] + v._x[2] * t._x[ 8],
				v._x[0] * t._x[1] + v._x[1] * t._x[5] + v._x[2] * t._x[ 9],
				v._x[0] * t._x[2] + v._x[1] * t._x[6] + v._x[2] * t._x[10]
				);
		}

		public static NuGenRay3F operator*(NuGenRay3F r, NuGenTrafo3F t)
		{
			return new NuGenRay3F(r.p * t, r.v * t);
		}

		public static NuGenBox3F operator*(NuGenBox3F b, NuGenTrafo3F t)
		{
			NuGenBox3F result = NuGenBox3F.Empty;
			result += b.LLL * t;
			result += b.LLU * t;
			result += b.LUL * t;
			result += b.LUU * t;
			result += b.ULL * t;
			result += b.ULU * t;
			result += b.UUL * t;
			result += b.UUU * t;
			return result;
		}

		public static NuGenTrafo3F operator*(NuGenTrafo3F a, NuGenTrafo3F b)
		{
			return new NuGenTrafo3F(

				a._x[ 0]*b._x[ 0]+a._x[ 1]*b._x[ 4]+a._x[ 2]*b._x[ 8]+a._x[ 3]*b._x[12],
				a._x[ 0]*b._x[ 1]+a._x[ 1]*b._x[ 5]+a._x[ 2]*b._x[ 9]+a._x[ 3]*b._x[13],
				a._x[ 0]*b._x[ 2]+a._x[ 1]*b._x[ 6]+a._x[ 2]*b._x[10]+a._x[ 3]*b._x[14],
				a._x[ 0]*b._x[ 3]+a._x[ 1]*b._x[ 7]+a._x[ 2]*b._x[11]+a._x[ 3]*b._x[15],

				a._x[ 4]*b._x[ 0]+a._x[ 5]*b._x[ 4]+a._x[ 6]*b._x[ 8]+a._x[ 7]*b._x[12],
				a._x[ 4]*b._x[ 1]+a._x[ 5]*b._x[ 5]+a._x[ 6]*b._x[ 9]+a._x[ 7]*b._x[13],
				a._x[ 4]*b._x[ 2]+a._x[ 5]*b._x[ 6]+a._x[ 6]*b._x[10]+a._x[ 7]*b._x[14],
				a._x[ 4]*b._x[ 3]+a._x[ 5]*b._x[ 7]+a._x[ 6]*b._x[11]+a._x[ 7]*b._x[15],

				a._x[ 8]*b._x[ 0]+a._x[ 9]*b._x[ 4]+a._x[10]*b._x[ 8]+a._x[11]*b._x[12],
				a._x[ 8]*b._x[ 1]+a._x[ 9]*b._x[ 5]+a._x[10]*b._x[ 9]+a._x[11]*b._x[13],
				a._x[ 8]*b._x[ 2]+a._x[ 9]*b._x[ 6]+a._x[10]*b._x[10]+a._x[11]*b._x[14],
				a._x[ 8]*b._x[ 3]+a._x[ 9]*b._x[ 7]+a._x[10]*b._x[11]+a._x[11]*b._x[15],

				a._x[12]*b._x[ 0]+a._x[13]*b._x[ 4]+a._x[14]*b._x[ 8]+a._x[15]*b._x[12],
				a._x[12]*b._x[ 1]+a._x[13]*b._x[ 5]+a._x[14]*b._x[ 9]+a._x[15]*b._x[13],
				a._x[12]*b._x[ 2]+a._x[13]*b._x[ 6]+a._x[14]*b._x[10]+a._x[15]*b._x[14],
				a._x[12]*b._x[ 3]+a._x[13]*b._x[ 7]+a._x[14]*b._x[11]+a._x[15]*b._x[15]

				);
		}

		internal float [] _x;

		public float [] Array 
		{
			get 
			{
				return _x; 
			} 
		}

	}
   
}

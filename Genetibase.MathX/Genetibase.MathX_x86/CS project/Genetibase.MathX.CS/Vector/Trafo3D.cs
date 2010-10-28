
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenTrafo3D.
	/// </summary>
	public struct NuGenTrafo3D
	{

		public static NuGenTrafo3D Identity = new NuGenTrafo3D(
			1,0,0,0,
			0,1,0,0,
			0,0,1,0,
			0,0,0,1
			);

		public NuGenTrafo3D(

			double x00, double x01, double x02, double x03,
			double x10, double x11, double x12, double x13,
			double x20, double x21, double x22, double x23,
			double x30, double x31, double x32, double x33
			)
		{
			_x = new double[16];
			_x[ 0] = x00; _x[ 1] = x01; _x[ 2] = x02; _x[ 3] = x03;
			_x[ 4] = x10; _x[ 5] = x11; _x[ 6] = x12; _x[ 7] = x13;
			_x[ 8] = x20; _x[ 9] = x21; _x[10] = x22; _x[11] = x23;
			_x[12] = x30; _x[13] = x31; _x[14] = x32; _x[15] = x33;
		}

		public NuGenTrafo3D(NuGenRot3D r)
		{
			this = r.ToNuGenTrafo3D();
		}

		public NuGenTrafo3D(NuGenTrafo2D t)
		{
			_x = new double[16];
			_x[ 0] = t[0,0]; _x[ 1] = t[0,1]; _x[ 2] = 0.0; _x[ 3] = t[0,2];
			_x[ 4] = t[1,0]; _x[ 5] = t[1,1]; _x[ 6] = 0.0; _x[ 7] = t[1,2];
			_x[ 8] = 0.0; _x[ 9] = 0.0; _x[10] = 1.0; _x[11] = 0.0;
			_x[12] = t[2,0]; _x[13] = t[2,1]; _x[14] = 0.0; _x[15] = t[2,2];
		}
        
		public static implicit operator NuGenTrafo3D(NuGenTrafo3F t)
		{
			return new NuGenTrafo3D(
				t._x[ 0], t._x[ 1], t._x[ 2], t._x[ 3],
				t._x[ 4], t._x[ 5], t._x[ 6], t._x[ 7], 
				t._x[ 8], t._x[ 9], t._x[10], t._x[11], 
				t._x[12], t._x[13], t._x[14], t._x[15]
				);
		}

		public double this [int i]
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

		public double this [int i, int j]
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

		public NuGenTrafo3D Transposed
		{
			get
			{
				return new NuGenTrafo3D(
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

		public double Det
		{
			get 
			{
				return NuGenVector.Det4x4(_x); 
			}
		}

		public NuGenTrafo3D Adjoint
		{
			get
			{
				return new NuGenTrafo3D(
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

		public NuGenTrafo3D Inverse
		{
			get
			{
								
				double d = Det;

				if (d == 0) throw new ArithmeticException("Singular Matrix");
				d = 1.0 / d;
				return Adjoint.Transposed * d;
			}
		}

		public void Invert()
		{
			this = Inverse;
		}

		public static NuGenTrafo3D operator+(NuGenTrafo3D t, double f)
		{
			return new NuGenTrafo3D(
				t._x[ 0] + f, t._x[ 1] + f, t._x[ 2] + f, t._x[ 3] + f,
				t._x[ 4] + f, t._x[ 5] + f, t._x[ 6] + f, t._x[ 7] + f,
				t._x[ 8] + f, t._x[ 9] + f, t._x[10] + f, t._x[11] + f,
				t._x[12] + f, t._x[13] + f, t._x[14] + f, t._x[15] + f
				);
		}

		public static NuGenTrafo3D operator-(NuGenTrafo3D t, double f)
		{
			return new NuGenTrafo3D(
				t._x[ 0] - f, t._x[ 1] - f, t._x[ 2] - f, t._x[ 3] - f,
				t._x[ 4] - f, t._x[ 5] - f, t._x[ 6] - f, t._x[ 7] - f,
				t._x[ 8] - f, t._x[ 9] - f, t._x[10] - f, t._x[11] - f,
				t._x[12] - f, t._x[13] - f, t._x[14] - f, t._x[15] - f
				);
		}

		public static NuGenTrafo3D operator*(NuGenTrafo3D t, double f)
		{
			return new NuGenTrafo3D(
				t._x[ 0] * f, t._x[ 1] * f, t._x[ 2] * f, t._x[ 3] * f,
				t._x[ 4] * f, t._x[ 5] * f, t._x[ 6] * f, t._x[ 7] * f,
				t._x[ 8] * f, t._x[ 9] * f, t._x[10] * f, t._x[11] * f,
				t._x[12] * f, t._x[13] * f, t._x[14] * f, t._x[15] * f
				);
		}

		public static NuGenTrafo3D operator/(NuGenTrafo3D t, double f)
		{
			return new NuGenTrafo3D(
				t._x[ 0] / f, t._x[ 1] / f, t._x[ 2] / f, t._x[ 3] / f,
				t._x[ 4] / f, t._x[ 5] / f, t._x[ 6] / f, t._x[ 7] / f,
				t._x[ 8] / f, t._x[ 9] / f, t._x[10] / f, t._x[11] / f,
				t._x[12] / f, t._x[13] / f, t._x[14] / f, t._x[15] / f
				);
		}

		public static NuGenPnt3D operator*(NuGenPnt3D p, NuGenTrafo3D t)
		{
						
			double f = 1.0 / (p[0]*t[0,3]+p[1]*t[1,3]+p[2]*t[2,3]+t[3,3]);
			return new NuGenPnt3D(
				(p[0] * t[0,0] + p[1] * t[1,0] + p[2] * t[2,0] + t[3,0]) * f,
				(p[0] * t[0,1] + p[1] * t[1,1] + p[2] * t[2,1] + t[3,1]) * f,
				(p[0] * t[0,2] + p[1] * t[1,2] + p[2] * t[2,2] + t[3,2]) * f
				);
		}

		public static NuGenVec3D operator*(NuGenVec3D v, NuGenTrafo3D t)
		{
			return new NuGenVec3D(
				v._x[0] * t._x[0] + v._x[1] * t._x[4] + v._x[2] * t._x[ 8],
				v._x[0] * t._x[1] + v._x[1] * t._x[5] + v._x[2] * t._x[ 9],
				v._x[0] * t._x[2] + v._x[1] * t._x[6] + v._x[2] * t._x[10]
				);
		}

		public static NuGenRay3D operator*(NuGenRay3D r, NuGenTrafo3D t)
		{
			return new NuGenRay3D(r.p * t, r.v * t);
		}

		public static NuGenBox3D operator*(NuGenBox3D b, NuGenTrafo3D t)
		{
			NuGenBox3D result = NuGenBox3D.Empty;
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

		public static NuGenTrafo3D operator*(NuGenTrafo3D a, NuGenTrafo3D b)
		{
			return new NuGenTrafo3D(

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

		internal double [] _x;

		public double [] Array 
		{
			get 
			{
				return _x; 
			} 
		}

	}

}

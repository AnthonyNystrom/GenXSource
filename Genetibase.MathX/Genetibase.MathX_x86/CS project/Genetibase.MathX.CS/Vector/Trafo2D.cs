
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenTrafo2D.
	/// </summary>
	public struct NuGenTrafo2D
	{

		public static NuGenTrafo2D Identity = new NuGenTrafo2D(
			1,0,0, 0,1,0, 0,0,1
			);

		public NuGenTrafo2D(

			double x00, double x01, double x02,
			double x10, double x11, double x12,
			double x20, double x21, double x22
			)
		{
			_x = new double[9];
			_x[ 0] = x00; _x[ 1] = x01; _x[ 2] = x02;
			_x[ 3] = x10; _x[ 4] = x11; _x[ 5] = x12;
			_x[ 6] = x20; _x[ 7] = x21; _x[ 8] = x22;
		}

		public NuGenTrafo2D(NuGenRot2D r)
		{
			this = r.ToNuGenTrafo2D();
		}

		public double this [int i, int j]
		{
			get 
			{
				return _x[i*3+j]; 
			}
			set 
			{
				_x[i*3+j] = value; 
			}
		}

		public NuGenTrafo2D Transposed
		{
			get
			{
				return new NuGenTrafo2D(
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

		public double Det
		{
			get
			{
				return
					_x[0] * _x[4] * _x[8] + _x[1] * _x[5] * _x[6] + _x[2] * _x[3] * _x[7]
					- _x[6] * _x[4] * _x[2] - _x[7] * _x[5] * _x[0] - _x[8] * _x[3] * _x[1];
			}
		}

		public NuGenTrafo2D Adjoint
		{
			get
			{
				return new NuGenTrafo2D(
					NuGenVector.Det2x2(
					_x[ 4], _x[ 5],
					_x[ 7], _x[ 8]), // 00
					- NuGenVector.Det2x2(
					_x[ 3], _x[ 5],
					_x[ 6], _x[ 8]), // 01
					NuGenVector.Det2x2(
					_x[ 3], _x[ 4],
					_x[ 6], _x[ 7]), // 02

					- NuGenVector.Det2x2(
					_x[ 1], _x[ 2],
					_x[ 7], _x[ 8]), // 10
					NuGenVector.Det2x2(
					_x[ 0], _x[ 2],
					_x[ 6], _x[ 8]), // 11
					- NuGenVector.Det2x2(
					_x[ 0], _x[ 1],
					_x[ 6], _x[ 7]), // 12

					NuGenVector.Det2x2(
					_x[ 1], _x[ 2],
					_x[ 4], _x[ 5]), // 20
					- NuGenVector.Det2x2(
					_x[ 0], _x[ 2],
					_x[ 3], _x[ 5]), // 21
					NuGenVector.Det2x2(
					_x[ 0], _x[ 1],
					_x[ 3], _x[ 4])  // 22
                    
					);
			}
		}

		public NuGenTrafo2D Inverse
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

		public static NuGenTrafo2D operator+(NuGenTrafo2D t, double f)
		{
			return new NuGenTrafo2D(
				t._x[ 0] + f, t._x[ 1] + f, t._x[ 2] + f,
				t._x[ 3] + f, t._x[ 4] + f, t._x[ 5] + f,
				t._x[ 6] + f, t._x[ 7] + f, t._x[ 8] + f
				);
		}

		public static NuGenTrafo2D operator-(NuGenTrafo2D t, double f)
		{
			return new NuGenTrafo2D(
				t._x[ 0] - f, t._x[ 1] - f, t._x[ 2] - f,
				t._x[ 3] - f, t._x[ 4] - f, t._x[ 5] - f,
				t._x[ 6] - f, t._x[ 7] - f, t._x[ 8] - f
				);
		}

		public static NuGenTrafo2D operator*(NuGenTrafo2D t, double f)
		{
			return new NuGenTrafo2D(
				t._x[ 0] * f, t._x[ 1] * f, t._x[ 2] * f,
				t._x[ 3] * f, t._x[ 4] * f, t._x[ 5] * f,
				t._x[ 6] * f, t._x[ 7] * f, t._x[ 8] * f
				);
		}

		public static NuGenTrafo2D operator/(NuGenTrafo2D t, double f)
		{
			return new NuGenTrafo2D(
				t._x[ 0] / f, t._x[ 1] / f, t._x[ 2] / f,
				t._x[ 3] / f, t._x[ 4] / f, t._x[ 5] / f,
				t._x[ 6] / f, t._x[ 7] / f, t._x[ 8] / f
				);
		}

		public static NuGenPnt2D operator*(NuGenPnt2D p, NuGenTrafo2D t)
		{
						
			double f = 1.0 / (p[0]*t._x[2]+p[1]*t._x[5]+t._x[8]);
			return new NuGenPnt2D(
				(p[0] * t._x[0] + p[1] * t._x[3] + t._x[6]) * f,
				(p[0] * t._x[1] + p[1] * t._x[4] + t._x[7]) * f
				);
		}

		public static NuGenVec2D operator*(NuGenVec2D v, NuGenTrafo2D t)
		{
			return new NuGenVec2D(
				v[0] * t._x[0] + v[1] * t._x[3],
				v[0] * t._x[1] + v[1] * t._x[4]
				);
		}

		public static NuGenBox2D operator*(NuGenBox2D b, NuGenTrafo2D t)
		{
			NuGenBox2D result = NuGenBox2D.Empty;
			result += b.LL * t;
			result += b.LU * t;
			result += b.UL * t;
			result += b.UU * t;
			return result;
		}

		public static NuGenTrafo2D operator*(NuGenTrafo2D a, NuGenTrafo2D b)
		{
			return new NuGenTrafo2D(

				a._x[ 0]*b._x[ 0]+a._x[ 1]*b._x[ 3]+a._x[ 2]*b._x[ 6],
				a._x[ 0]*b._x[ 1]+a._x[ 1]*b._x[ 4]+a._x[ 2]*b._x[ 7],
				a._x[ 0]*b._x[ 2]+a._x[ 1]*b._x[ 5]+a._x[ 2]*b._x[ 8],

				a._x[ 3]*b._x[ 0]+a._x[ 4]*b._x[ 3]+a._x[ 5]*b._x[ 6],
				a._x[ 3]*b._x[ 1]+a._x[ 4]*b._x[ 4]+a._x[ 5]*b._x[ 7],
				a._x[ 3]*b._x[ 2]+a._x[ 4]*b._x[ 5]+a._x[ 5]*b._x[ 8],

				a._x[ 6]*b._x[ 0]+a._x[ 7]*b._x[ 3]+a._x[ 8]*b._x[ 6],
				a._x[ 6]*b._x[ 1]+a._x[ 7]*b._x[ 4]+a._x[ 8]*b._x[ 7],
				a._x[ 6]*b._x[ 2]+a._x[ 7]*b._x[ 5]+a._x[ 8]*b._x[ 8]

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

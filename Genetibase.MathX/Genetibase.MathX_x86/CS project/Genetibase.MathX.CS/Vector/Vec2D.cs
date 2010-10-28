
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenVec2D.
	/// </summary>
	public struct NuGenVec2D
	{

		public static NuGenVec2D Identity = new NuGenVec2D(0);
		public static NuGenVec2D UnitX = new NuGenVec2D(1,0);
		public static NuGenVec2D UnitY = new NuGenVec2D(0,1);

		public NuGenVec2D(double xy)
		{
			_x = new double[2];
			_x[0] = xy; _x[1] = xy;
		}

		public NuGenVec2D(double x, double y)
		{
			_x = new double[2];
			_x[0] = x; _x[1] = y;
		}

		public NuGenVec2D(ref double [] x)
		{
			_x = new double[2];
			_x[0] = x[0]; _x[1] = x[1];
		}

		public double this [int index]
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

		public double x
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

		public double y
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

		public static implicit operator NuGenVec2D(NuGenVec2F v)
		{
			return new NuGenVec2D(v.x, v.y);
		}

		public static implicit operator NuGenVec2D(NuGenPnt2D v)
		{
			return new NuGenVec2D(v.x, v.y);
		}

		public static NuGenVec2D operator+(NuGenVec2D v, double f)
		{
			return new NuGenVec2D(v[0] + f, v[1] + f);
		}

		public static NuGenVec2D operator+(NuGenVec2D v, NuGenVec2D w)
		{
			return new NuGenVec2D(v[0] + w[0], v[1] + w[1]);
		}

		public static NuGenVec2D operator-(NuGenVec2D v, double f)
		{
			return new NuGenVec2D(v[0] - f, v[1] - f);
		}

		public static NuGenVec2D operator-(NuGenVec2D v, NuGenVec2D w)
		{
			return new NuGenVec2D(v[0] - w[0], v[1] - w[1]);
		}

		public static NuGenVec2D operator-(NuGenVec2D v)
		{
			return new NuGenVec2D(-v[0], -v[1]);
		}

		public static NuGenVec2D operator*(NuGenVec2D v, double f)
		{
			return new NuGenVec2D(v[0] * f, v[1] * f);
		}

		public static NuGenVec2D operator*(NuGenVec2D v, NuGenVec2D w)
		{
			return new NuGenVec2D(v[0] * w[0], v[1] * w[1]);
		}

		public static NuGenVec2D operator/(NuGenVec2D v, double f)
		{
			return new NuGenVec2D(v[0] / f, v[1] / f);
		}

		public static double Dot(NuGenVec2D u, NuGenVec2D v)
		{
			return u[0] * v[0] + u[1] * v[1];
		}

		public void Normalize()
		{
			this /= Length;
		}

		public NuGenVec2D Normalized
		{
			get 
			{
				return this / Length; 
			}
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
				return (double)Math.Sqrt(SquaredLength); 
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

		public double Minimum
		{
			get
			{
				return (_x[0] < _x[1]) ? _x[0] : _x[1];
			}
		}

		public double Maximum
		{
			get
			{
				return (_x[0] > _x[1]) ? _x[0] : _x[1];
			}
		}

		public static bool operator<(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1]
				);
		}

		public static bool operator<=(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1]
				);
		}

		public static bool operator==(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1]
				);
		}

		public static bool operator!=(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1]
				);
		}

		public static bool operator>=(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1]
				);
		}

		public static bool operator>(NuGenVec2D a, NuGenVec2D b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1]
				);
		}

		public static NuGenVec2D Min(NuGenVec2D a, NuGenVec2D b)
		{
			return new NuGenVec2D(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1])
				);
		}

		public static NuGenVec2D Max(NuGenVec2D a, NuGenVec2D b)
		{
			return new NuGenVec2D(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1])
				);
		}

		internal double [] _x;

		public override bool Equals(object obj)
		{
			NuGenVec2D x = (NuGenVec2D)obj;
			return (
				_x[0] == x._x[0] &&
				_x[1] == x._x[1]
				);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return "NuGenVec2D(" + _x[0] + ", " + _x[1] + ")";
		}

		static NuGenVec2D()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				System.Globalization.CultureInfo.InvariantCulture;
		}
	}
    
}

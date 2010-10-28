
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenPnt2D.
	/// </summary>
	public struct NuGenPnt2D
	{

		public static NuGenPnt2D Identity = new NuGenPnt2D(0,0);

		public NuGenPnt2D(double xy)
		{
			_x = new double[2];
			_x[0] = xy; _x[1] = xy;
		}

		public NuGenPnt2D(double x, double y)
		{
			_x = new double[2];
			_x[0] = x; _x[1] = y;
		}

		public NuGenPnt2D(ref double [] x)
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

		public static implicit operator NuGenPnt2D(NuGenPnt2F v)
		{
			return new NuGenPnt2D(v.x, v.y);
		}

		public static explicit operator NuGenPnt2D(NuGenVec2D v)
		{
			return new NuGenPnt2D(v.x, v.y);
		}

		public static NuGenPnt2D operator+(NuGenPnt2D v, double f)
		{
			return new NuGenPnt2D(v[0] + f, v[1] + f);
		}

		public static NuGenPnt2D operator+(NuGenPnt2D v, NuGenVec2D w)
		{
			return new NuGenPnt2D(v[0] + w[0], v[1] + w[1]);
		}

		public static NuGenPnt2D operator-(NuGenPnt2D v)
		{
			return new NuGenPnt2D(-v[0], -v[1]);
		}

		public static NuGenPnt2D operator-(NuGenPnt2D v, double f)
		{
			return new NuGenPnt2D(v[0] - f, v[1] - f);
		}

		public static NuGenVec2D operator-(NuGenPnt2D v, NuGenPnt2D w)
		{
			return new NuGenVec2D(v[0] - w[0], v[1] - w[1]);
		}

		public static NuGenPnt2D operator*(NuGenPnt2D v, double f)
		{
			return new NuGenPnt2D(v[0] * f, v[1] * f);
		}

		public static NuGenPnt2D operator/(NuGenPnt2D v, double f)
		{
			return new NuGenPnt2D(v[0] / f, v[1] / f);
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

		public static bool operator<(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1]
				);
		}

		public static bool operator<=(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1]
				);
		}

		public static bool operator==(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1]
				);
		}

		public static bool operator!=(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1]
				);
		}

		public static bool operator>=(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1]
				);
		}

		public static bool operator>(NuGenPnt2D a, NuGenPnt2D b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1]
				);
		}

		public static NuGenPnt2D Min(NuGenPnt2D a, NuGenPnt2D b)
		{
			return new NuGenPnt2D(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1])
				);
		}

		public static NuGenPnt2D Max(NuGenPnt2D a, NuGenPnt2D b)
		{
			return new NuGenPnt2D(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1])
				);
		}

		internal double [] _x;

		public override bool Equals(object obj)
		{
			NuGenPnt2D x = (NuGenPnt2D)obj;
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
			return "NuGenPnt2D(" + _x[0] + ", " + _x[1] + ")";
		}

	}

}

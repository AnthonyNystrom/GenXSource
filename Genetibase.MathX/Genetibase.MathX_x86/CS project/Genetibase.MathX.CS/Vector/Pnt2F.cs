
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenPnt2F.
	/// </summary>
	public struct NuGenPnt2F
	{

		public static NuGenPnt2F Identity = new NuGenPnt2F(0,0);

		public NuGenPnt2F(float xy)
		{
			_x = new float[2];
			_x[0] = xy; _x[1] = xy;
		}

		public NuGenPnt2F(float x, float y)
		{
			_x = new float[2];
			_x[0] = x; _x[1] = y;
		}

		public NuGenPnt2F(ref float [] x)
		{
			_x = new float[2];
			_x[0] = x[0]; _x[1] = x[1];
		}

		public float this [int index]
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

		public static explicit operator NuGenPnt2F(NuGenPnt2D v)
		{
			return new NuGenPnt2F((float)v.x, (float)v.y);
		}

		public static explicit operator NuGenPnt2F(NuGenVec2F v)
		{
			return new NuGenPnt2F(v.x, v.y);
		}

		public static NuGenPnt2F operator+(NuGenPnt2F v, float f)
		{
			return new NuGenPnt2F(v[0] + f, v[1] + f);
		}

		public static NuGenPnt2F operator+(NuGenPnt2F v, NuGenVec2F w)
		{
			return new NuGenPnt2F(v[0] + w[0], v[1] + w[1]);
		}

		public static NuGenPnt2F operator-(NuGenPnt2F v)
		{
			return new NuGenPnt2F(-v[0], -v[1]);
		}

		public static NuGenPnt2F operator-(NuGenPnt2F v, float f)
		{
			return new NuGenPnt2F(v[0] - f, v[1] - f);
		}

		public static NuGenVec2F operator-(NuGenPnt2F v, NuGenPnt2F w)
		{
			return new NuGenVec2F(v[0] - w[0], v[1] - w[1]);
		}

		public static NuGenPnt2F operator*(NuGenPnt2F v, float f)
		{
			return new NuGenPnt2F(v[0] * f, v[1] * f);
		}

		public static NuGenPnt2F operator/(NuGenPnt2F v, float f)
		{
			return new NuGenPnt2F(v[0] / f, v[1] / f);
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

		public float Minimum
		{
			get
			{
				return (_x[0] < _x[1]) ? _x[0] : _x[1];
			}
		}

		public float Maximum
		{
			get
			{
				return (_x[0] > _x[1]) ? _x[0] : _x[1];
			}
		}

		public static bool operator<(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1]
				);
		}

		public static bool operator<=(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1]
				);
		}

		public static bool operator==(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1]
				);
		}

		public static bool operator!=(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1]
				);
		}

		public static bool operator>=(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1]
				);
		}

		public static bool operator>(NuGenPnt2F a, NuGenPnt2F b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1]
				);
		}

		public static NuGenPnt2F Min(NuGenPnt2F a, NuGenPnt2F b)
		{
			return new NuGenPnt2F(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1])
				);
		}

		public static NuGenPnt2F Max(NuGenPnt2F a, NuGenPnt2F b)
		{
			return new NuGenPnt2F(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1])
				);
		}

		internal float [] _x;

		public override bool Equals(object obj)
		{
			NuGenPnt2F x = (NuGenPnt2F)obj;
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
			return "NuGenPnt2F(" + _x[0] + ", " + _x[1] + ")";
		}

	}

}

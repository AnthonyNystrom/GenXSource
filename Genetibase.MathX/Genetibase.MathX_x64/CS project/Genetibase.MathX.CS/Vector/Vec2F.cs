
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenVec2F.
	/// </summary>
	public struct NuGenVec2F
	{

		public static NuGenVec2F Identity = new NuGenVec2F(0);
		public static NuGenVec2F UnitX = new NuGenVec2F(1,0);
		public static NuGenVec2F UnitY = new NuGenVec2F(0,1);

		public NuGenVec2F(float xy)
		{
			_x = new float[2];
			_x[0] = xy; _x[1] = xy;
		}

		public NuGenVec2F(float x, float y)
		{
			_x = new float[2];
			_x[0] = x; _x[1] = y;
		}

		public NuGenVec2F(ref float [] x)
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

		public static explicit operator NuGenVec2F(NuGenVec2D v)
		{
			return new NuGenVec2F((float)v.x, (float)v.y);
		}

		public static implicit operator NuGenVec2F(NuGenPnt2F v)
		{
			return new NuGenVec2F(v.x, v.y);
		}

		public static NuGenVec2F operator+(NuGenVec2F v, float f)
		{
			return new NuGenVec2F(v[0] + f, v[1] + f);
		}

		public static NuGenVec2F operator+(NuGenVec2F v, NuGenVec2F w)
		{
			return new NuGenVec2F(v[0] + w[0], v[1] + w[1]);
		}

		public static NuGenVec2F operator-(NuGenVec2F v, float f)
		{
			return new NuGenVec2F(v[0] - f, v[1] - f);
		}

		public static NuGenVec2F operator-(NuGenVec2F v, NuGenVec2F w)
		{
			return new NuGenVec2F(v[0] - w[0], v[1] - w[1]);
		}

		public static NuGenVec2F operator-(NuGenVec2F v)
		{
			return new NuGenVec2F(-v[0], -v[1]);
		}

		public static NuGenVec2F operator*(NuGenVec2F v, float f)
		{
			return new NuGenVec2F(v[0] * f, v[1] * f);
		}

		public static NuGenVec2F operator/(NuGenVec2F v, float f)
		{
			return new NuGenVec2F(v[0] / f, v[1] / f);
		}

		public static float Dot(NuGenVec2F u, NuGenVec2F v)
		{
			return u[0] * v[0] + u[1] * v[1];
		}

		public void Normalize()
		{
			this /= Length;
		}

		public NuGenVec2F Normalized
		{
			get 
			{
				return this / Length; 
			}
		}

		public float SquaredLength
		{
			get 
			{
				return _x[0] * _x[0] + _x[1] * _x[1]; 
			}
		}

		public float Length
		{
			get 
			{
				return (float)Math.Sqrt(SquaredLength); 
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

		public static bool operator<(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1]
				);
		}

		public static bool operator<=(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1]
				);
		}

		public static bool operator==(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1]
				);
		}

		public static bool operator!=(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1]
				);
		}

		public static bool operator>=(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1]
				);
		}

		public static bool operator>(NuGenVec2F a, NuGenVec2F b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1]
				);
		}

		public static NuGenVec2F Min(NuGenVec2F a, NuGenVec2F b)
		{
			return new NuGenVec2F(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1])
				);
		}

		public static NuGenVec2F Max(NuGenVec2F a, NuGenVec2F b)
		{
			return new NuGenVec2F(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1])
				);
		}

		internal float [] _x;

		public override bool Equals(object obj)
		{
			NuGenVec2F x = (NuGenVec2F)obj;
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
			return "NuGenVec2F(" + _x[0] + ", " + _x[1] + ")";
		}

		static NuGenVec2F()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				System.Globalization.CultureInfo.InvariantCulture;
		}
	}
    
}

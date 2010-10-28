
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenVec3F.
	/// </summary>
	public struct NuGenVec3F
	{

		public static NuGenVec3F Identity = new NuGenVec3F(0);
		public static NuGenVec3F UnitX = new NuGenVec3F(1,0,0);
		public static NuGenVec3F UnitY = new NuGenVec3F(0,1,0);
		public static NuGenVec3F UnitZ = new NuGenVec3F(0,0,1);

		public NuGenVec3F(float xyz)
		{
			_x = new float[3];
			_x[0] = xyz; _x[1] = xyz; _x[2] = xyz;
		}

		public NuGenVec3F(float x, float y, float z)
		{
			_x = new float[3];
			_x[0] = x; _x[1] = y; _x[2] = z;
		}

		public NuGenVec3F(ref float [] x)
		{
			_x = new float[3];
			_x[0] = x[0]; _x[1] = x[1]; _x[2] = x[2];
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

		public static explicit operator NuGenVec3F(NuGenVec3D v)
		{
			return new NuGenVec3F((float)v.x, (float)v.y, (float)v.z);
		}

		public static implicit operator NuGenVec3F(NuGenPnt3F v)
		{
			return new NuGenPnt3F(v.x, v.y, v.z);
		}

		public static NuGenVec3F operator+(NuGenVec3F v, float f)
		{
			return new NuGenVec3F(v[0] + f, v[1] + f, v[2] + f);
		}

		public static NuGenVec3F operator+(NuGenVec3F v, NuGenVec3F w)
		{
			return new NuGenVec3F(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
		}

		public static NuGenVec3F operator-(NuGenVec3F v, float f)
		{
			return new NuGenVec3F(v[0] - f, v[1] - f, v[2] - f);
		}

		public static NuGenVec3F operator-(NuGenVec3F v, NuGenVec3F w)
		{
			return new NuGenVec3F(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
		}

		public static NuGenVec3F operator-(NuGenVec3F v)
		{
			return new NuGenVec3F(-v[0], -v[1], -v[2]);
		}

		public static NuGenVec3F operator*(NuGenVec3F v, float f)
		{
			return new NuGenVec3F(v[0] * f, v[1] * f, v[2] * f);
		}

		public static NuGenVec3F operator/(NuGenVec3F v, float f)
		{
			return new NuGenVec3F(v[0] / f, v[1] / f, v[2] / f);
		}

		public static float Dot(NuGenVec3F u, NuGenVec3F v)
		{
			return u[0] * v[0] + u[1] * v[1] + u[2] * v[2];
		}

		public static NuGenVec3F Cross(NuGenVec3F a, NuGenVec3F b)
		{
			return new NuGenVec3F(
				a.y * b.z - a.z * b.y,
				a.z * b.x - a.x * b.z,
				a.x * b.y - a.y * b.x
				);
		}

		public void Normalize()
		{
			this /= Length;
		}

		public NuGenVec3F Normalized
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
				return _x[0] * _x[0] + _x[1] * _x[1] + _x[2] * _x[2]; 
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

		public static bool operator<(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1] &&
				a._x[2] < b._x[2]
				);
		}

		public static bool operator<=(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1] &&
				a._x[2] <= b._x[2]
				);
		}

		public static bool operator==(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1] &&
				a._x[2] == b._x[2]
				);
		}

		public static bool operator!=(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1] ||
				a._x[2] != b._x[2]
				);
		}

		public static bool operator>=(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1] &&
				a._x[2] >= b._x[2]
				);
		}

		public static bool operator>(NuGenVec3F a, NuGenVec3F b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1] &&
				a._x[2] > b._x[2]
				);
		}

		public static NuGenVec3F Min(NuGenVec3F a, NuGenVec3F b)
		{
			return new NuGenVec3F(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1]),
				Math.Min(a._x[2], b._x[2])
				);
		}

		public static NuGenVec3F Max(NuGenVec3F a, NuGenVec3F b)
		{
			return new NuGenVec3F(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1]),
				Math.Max(a._x[2], b._x[2])
				);
		}

		internal float [] _x;

		public override bool Equals(object obj)
		{
			NuGenVec3F x = (NuGenVec3F)obj;
			return (
				_x[0] == x._x[0] &&
				_x[1] == x._x[1] &&
				_x[2] == x._x[2]
				);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return "NuGenVec3F(" + _x[0] + ", " + _x[1] + ", " + _x[2] + ")";
		}

		static NuGenVec3F()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				System.Globalization.CultureInfo.InvariantCulture;
		}
	}
    
}

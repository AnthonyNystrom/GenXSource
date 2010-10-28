
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenVec3D.
	/// </summary>
	public struct NuGenVec3D
	{

		public static NuGenVec3D Identity = new NuGenVec3D(0);
		public static NuGenVec3D UnitX = new NuGenVec3D(1,0,0);
		public static NuGenVec3D UnitY = new NuGenVec3D(0,1,0);
		public static NuGenVec3D UnitZ = new NuGenVec3D(0,0,1);

		public NuGenVec3D(double xyz)
		{
			_x = new double[3];
			_x[0] = xyz; _x[1] = xyz; _x[2] = xyz;
		}

		public NuGenVec3D(double x, double y, double z)
		{
			_x = new double[3];
			_x[0] = x; _x[1] = y; _x[2] = z;
		}

		public NuGenVec3D(ref double [] x)
		{
			_x = new double[3];
			_x[0] = x[0]; _x[1] = x[1]; _x[2] = x[2];
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

		public double z
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

		public static implicit operator NuGenVec3D(NuGenVec3F v)
		{
			return new NuGenVec3D(v.x, v.y, v.z);
		}

		public static implicit operator NuGenVec3D(NuGenPnt3D v)
		{
			return new NuGenVec3D(v.x, v.y, v.z);
		}

		public static NuGenVec3D operator+(NuGenVec3D v, double f)
		{
			return new NuGenVec3D(v[0] + f, v[1] + f, v[2] + f);
		}

		public static NuGenVec3D operator+(NuGenVec3D v, NuGenVec3D w)
		{
			return new NuGenVec3D(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
		}

		public static NuGenVec3D operator-(NuGenVec3D v, double f)
		{
			return new NuGenVec3D(v[0] - f, v[1] - f, v[2] - f);
		}

		public static NuGenVec3D operator-(NuGenVec3D v, NuGenVec3D w)
		{
			return new NuGenVec3D(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
		}

		public static NuGenVec3D operator-(NuGenVec3D v)
		{
			return new NuGenVec3D(-v[0], -v[1], -v[2]);
		}

		public static NuGenVec3D operator*(NuGenVec3D v, double f)
		{
			return new NuGenVec3D(v[0] * f, v[1] * f, v[2] * f);
		}

		public static NuGenVec3D operator/(NuGenVec3D v, double f)
		{
			return new NuGenVec3D(v[0] / f, v[1] / f, v[2] / f);
		}

		public static double Dot(NuGenVec3D u, NuGenVec3D v)
		{
			return u[0] * v[0] + u[1] * v[1] + u[2] * v[2];
		}

		public static NuGenVec3D Cross(NuGenVec3D a, NuGenVec3D b)
		{
			return new NuGenVec3D(
				a.y * b.z - a.z * b.y,
				a.z * b.x - a.x * b.z,
				a.x * b.y - a.y * b.x
				);
		}

		public void Normalize()
		{
			this /= Length;
		}

		public NuGenVec3D Normalized
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
				return _x[0] * _x[0] + _x[1] * _x[1] + _x[2] * _x[2]; 
			}
		}

		public double Length
		{
			get 
			{
				return Math.Sqrt(SquaredLength); 
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

		public double Minimum
		{
			get
			{
				return (_x[0] < _x[1])
					? ((_x[0] < _x[2]) ? _x[0] : _x[2])
					: ((_x[1] < _x[2]) ? _x[1] : _x[2]);
			}
		}

		public double Maximum
		{
			get
			{
				return (_x[0] > _x[1])
					? ((_x[0] > _x[2]) ? _x[0] : _x[2])
					: ((_x[1] > _x[2]) ? _x[1] : _x[2]);
			}
		}

		public static bool operator<(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1] &&
				a._x[2] < b._x[2]
				);
		}

		public static bool operator<=(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1] &&
				a._x[2] <= b._x[2]
				);
		}

		public static bool operator==(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1] &&
				a._x[2] == b._x[2]
				);
		}

		public static bool operator!=(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1] ||
				a._x[2] != b._x[2]
				);
		}

		public static bool operator>=(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1] &&
				a._x[2] >= b._x[2]
				);
		}

		public static bool operator>(NuGenVec3D a, NuGenVec3D b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1] &&
				a._x[2] > b._x[2]
				);
		}

		public static NuGenVec3D Min(NuGenVec3D a, NuGenVec3D b)
		{
			return new NuGenVec3D(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1]),
				Math.Min(a._x[2], b._x[2])
				);
		}

		public static NuGenVec3D Max(NuGenVec3D a, NuGenVec3D b)
		{
			return new NuGenVec3D(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1]),
				Math.Max(a._x[2], b._x[2])
				);
		}

		internal double [] _x;

		public override bool Equals(object obj)
		{
			NuGenVec3D x = (NuGenVec3D)obj;
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
			return "NuGenVec3D(" + _x[0] + ", " + _x[1] + ", " + _x[2] + ")";
		}

		static NuGenVec3D()
		{
			System.Threading.Thread.CurrentThread.CurrentCulture =
				System.Globalization.CultureInfo.InvariantCulture;
		}
	}
    
}

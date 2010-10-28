
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenPnt3D.
	/// </summary>
	public struct NuGenPnt3D
	{

		public NuGenPnt3D(double x, double y, double z)
		{
			_x = new double[3];
			_x[0] = x;
			_x[1] = y;
			_x[2] = z;
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

		public static implicit operator NuGenPnt3D(NuGenPnt3F v)
		{
			return new NuGenPnt3D(v.x, v.y, v.z);
		}

		public static explicit operator NuGenPnt3D(NuGenVec3D v)
		{
			return new NuGenPnt3D(v.x, v.y, v.z);
		}

		public static NuGenPnt3D operator+(NuGenPnt3D v, double f)
		{
			return new NuGenPnt3D(v[0] + f, v[1] + f, v[2] + f);
		}

		public static NuGenPnt3D operator+(NuGenPnt3D v, NuGenPnt3D w)
		{
			return new NuGenPnt3D(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
		}

		public static NuGenPnt3D operator+(NuGenPnt3D v, NuGenVec3D w)
		{
			return new NuGenPnt3D(v[0] + w[0], v[1] + w[1], v[2] + w[2]);
		}

		public static NuGenPnt3D operator-(NuGenPnt3D v)
		{
			return new NuGenPnt3D(-v[0], -v[1], -v[2]);
		}

		public static NuGenPnt3D operator-(NuGenPnt3D v, double f)
		{
			return new NuGenPnt3D(v[0] - f, v[1] - f, v[2] - f);
		}

		public static NuGenVec3D operator-(NuGenPnt3D v, NuGenPnt3D w)
		{
			return new NuGenVec3D(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
		}

		public static NuGenPnt3D operator-(NuGenPnt3D v, NuGenVec3D w)
		{
			return new NuGenPnt3D(v[0] - w[0], v[1] - w[1], v[2] - w[2]);
		}

		public static NuGenPnt3D operator*(NuGenPnt3D v, double f)
		{
			return new NuGenPnt3D(v[0] * f, v[1] * f, v[2] * f);
		}

		public static NuGenPnt3D operator/(NuGenPnt3D v, double f)
		{
			return new NuGenPnt3D(v[0] / f, v[1] / f, v[2] / f);
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

		public static bool operator<(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] < b._x[0] &&
				a._x[1] < b._x[1] &&
				a._x[2] < b._x[2]
				);
		}

		public static bool operator<=(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] <= b._x[0] &&
				a._x[1] <= b._x[1] &&
				a._x[2] <= b._x[2]
				);
		}

		public static bool operator==(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] == b._x[0] &&
				a._x[1] == b._x[1] &&
				a._x[2] == b._x[2]
				);
		}

		public static bool operator!=(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] != b._x[0] ||
				a._x[1] != b._x[1] ||
				a._x[2] != b._x[2]
				);
		}

		public static bool operator>=(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] >= b._x[0] &&
				a._x[1] >= b._x[1] &&
				a._x[2] >= b._x[2]
				);
		}

		public static bool operator>(NuGenPnt3D a, NuGenPnt3D b)
		{
			return (
				a._x[0] > b._x[0] &&
				a._x[1] > b._x[1] &&
				a._x[2] > b._x[2]
				);
		}

		public static NuGenPnt3D Min(NuGenPnt3D a, NuGenPnt3D b)
		{
			return new NuGenPnt3D(
				Math.Min(a._x[0], b._x[0]),
				Math.Min(a._x[1], b._x[1]),
				Math.Min(a._x[2], b._x[2])
				);
		}

		public static NuGenPnt3D Max(NuGenPnt3D a, NuGenPnt3D b)
		{
			return new NuGenPnt3D(
				Math.Max(a._x[0], b._x[0]),
				Math.Max(a._x[1], b._x[1]),
				Math.Max(a._x[2], b._x[2])
				);
		}

		internal double [] _x;

		public override bool Equals(object obj)
		{
			NuGenPnt3D x = (NuGenPnt3D)obj;
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
			return "NuGenPnt3D(" + _x[0] + ", " + _x[1] + ", " + _x[2] + ")";
		}

	}

}

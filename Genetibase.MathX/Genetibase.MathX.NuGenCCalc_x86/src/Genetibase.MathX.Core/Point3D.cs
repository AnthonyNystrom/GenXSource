using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Genetibase.MathX.Core
{
	/// <summary><para>Represents a point in tridimensional space.</para></summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Point3D 
	{
		private double _x;
		private double _y;
		private double _z;


		public double X
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
			}
		}
		public double Y
		{
			get
			{
				return this._y;
			}
			set
			{
				this._y = value;
			}
		}

		public double Z
		{
			get
			{
				return this._z;
			}
			set
			{
				this._z = value;
			}
		}

		public Point3D(double x, double y, double z)
		{
			this._x = x;
			this._y = y;
			this._z = z;

		}

		public override bool Equals(object obj)
		{
			if (base.GetType() != obj.GetType())
			{
				return false;
			}
			return (((Point3D) obj) == this);
		}
		public override int GetHashCode()
		{
			return (this._x.GetHashCode() ^ (this._y.GetHashCode() - 1) ^ (this._z.GetHashCode() - 2));
		}
		public static bool operator ==(Point3D point1, Point3D point2)
		{
			if (point1._x != point2._x)
			{
				return false;
			}
			if (point1._y != point2._y)
			{
				 return false;
			}
			return (point1._z == point2._z);
		}
		public static bool operator !=(Point3D point1, Point3D point2)
		{
			if (point1._x != point2._x)
			{
				return true;
			}
			if (point1._y != point2._y)
			{
				return true;
			}
			return (point1._z != point2._z);
		}
		public static Point3D operator +(Point3D a, Point3D b)
		{
			return new Point3D(a._x + b._x, a._y + b._y,a._z + b._z);
		}
		public static Point3D operator -(Point3D a, Point3D b)
		{
			return new Point3D(a._x - b._x, a._y - b._y,a._z - b._z);
		}
		public static Point3D Add(Point3D a, Point3D b)
		{
			return a+b;
		}
		public static Point3D Subtract(Point3D a, Point3D b)
		{
			return a-b;
		}
		public static double Distance(Point3D a, Point3D b)
		{
			return Math.Sqrt((a._z - b._z)*(a._z - b._z)+
				Math.Sqrt((a._x - b._x)*(a._x - b._x) + (a._y - b._y)*(a._y - b._y)));
		}
	}
}


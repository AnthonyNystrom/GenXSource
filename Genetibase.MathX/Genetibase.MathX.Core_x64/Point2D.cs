using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// 	<para>Represents an ordered pair of floating-point x- and y-coordinates that defines a
	/// point in a two-dimensional plane.</para>
	/// </summary>
	[Serializable, StructLayout(LayoutKind.Sequential)]
	public struct Point2D 
	{
		private double _x;
		private double _y;

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

		public Point2D(PointF point) : this (point.X,point.Y)
		{
		}

		public Point2D(double x, double y)
		{
			this._x = x;
			this._y = y;
		}

//		public static Point2D FromIntersection(Line line1, Line line2)
//		{
//			double num1 = line1.Parameters[1];
//			double num2 = line2.Parameters[1];
//			double num3 = num2 - num1;
//			if (num3 == 0)
//			{
//				throw new DivideByZeroException();
//			}
//			double num4 = line1.Parameters[0];
//			double num5 = line2.Parameters[0];
//			return new Point2D((num4 - num5) / num3, ((num2 * num4) - (num1 * num5)) / num3);
//		}
		public override bool Equals(object obj)
		{
			if (base.GetType() != obj.GetType())
			{
				return false;
			}
			return (((Point2D) obj) == this);
		}
		public override int GetHashCode()
		{
			return (this._x.GetHashCode() ^ (this._y.GetHashCode() - 1));
		}

		public static explicit operator PointF(Point2D point)
		{
			return new PointF((float)point._x, (float)point._y);
		}

		public static bool operator ==(Point2D point1, Point2D point2)
		{
			if (point1._x != point2._x)
			{
				return false;
			}
			return (point1._y == point2._y);
		}
		public static bool operator !=(Point2D point1, Point2D point2)
		{
			if (point1._x != point2._x)
			{
				return true;
			}
			return (point1._y != point2._y);
		}
		public static Point2D operator +(Point2D a, Point2D b)
		{
			return new Point2D(a._x + b._x, a._y + b._y);
		}
		public static Point2D operator -(Point2D a, Point2D b)
		{
			return new Point2D(a._x - b._x, a._y - b._y);
		}
		public static Point2D Add(Point2D a, Point2D b)
		{
			return new Point2D(a._x + b._x, a._y + b._y);
		}
		public static Point2D Subtract(Point2D a, Point2D b)
		{
			return new Point2D(a._x - b._x, a._y - b._y);
		}
		public static double Distance(Point2D a, Point2D b)
		{
			return Math.Sqrt((a._x - b._x)*(a._x - b._x) + (a._y - b._y)*(a._y - b._y));
		}
//		public static double ManhattanDistance(Point2D a, Point2D b)
//		{
//			return (Math.Abs((double) (a._x - b._x)) + Math.Abs((double) (a._y - b._y)));
//		}
	}
}


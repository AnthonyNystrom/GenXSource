
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenShift2D.
	/// </summary>
	public struct NuGenShift2D
	{

		public static NuGenShift2D Identity = new NuGenShift2D(0,0);

		public NuGenShift2D(double x, double y)
		{
			v = new NuGenVec2D(x,y);
		}

		public NuGenShift2D(NuGenVec2D shift)
		{
			v = shift;
		}

		public double x 
		{
			get 
			{
				return v._x[0]; 
			} 
		}

		public double y 
		{
			get 
			{
				return v._x[1]; 
			} 
		}

		public NuGenTrafo2D ToNuGenTrafo2D()
		{
			return new NuGenTrafo2D(
				1, 0, 0,
				0, 1, 0,
				v._x[0], v._x[1], 1
				);
		}

		public static implicit operator NuGenTrafo2D(NuGenShift2D r)
		{
			return r.ToNuGenTrafo2D();
		}

		public static NuGenShift2D operator*(NuGenShift2D a, NuGenShift2D b)
		{
			return new NuGenShift2D(a.v + b.v);
		}

		public static NuGenTrafo2D operator*(NuGenNuGenScale2D a, NuGenShift2D b)
		{
			return new NuGenTrafo2D(
				a.x, 0, 0,
				0, a.y, 0,
				b.x, b.y, 1
				);
		}

		public static NuGenTrafo2D operator*(NuGenRot2D a, NuGenShift2D b)
		{
			return a.ToNuGenTrafo2D() * b.ToNuGenTrafo2D();
		}

		public static NuGenTrafo2D operator*(NuGenTrafo2D a, NuGenShift2D b)
		{
			a._x[6] += b.x;
			a._x[7] += b.y;
			return a;
		}

		public static NuGenVec2D operator*(NuGenVec2D v, NuGenShift2D t)
		{
			return v;
		}

		public static NuGenPnt2D operator*(NuGenPnt2D p, NuGenShift2D t)
		{
			return p + t.v;
		}

		private NuGenVec2D v;

	}

}


using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenNuGenScale2D.
	/// </summary>
	public struct NuGenNuGenScale2D
	{

		public static NuGenNuGenScale2D Identity = new NuGenNuGenScale2D(1,1);

		public NuGenNuGenScale2D(double x, double y)
		{
			v = new NuGenVec2D(x,y);
		}

		public NuGenNuGenScale2D(NuGenVec2D scale)
		{
			v = scale;
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
				v._x[0], 0, 0,
				0, v._x[1], 0,
				0, 0, 1
				);
		}

		public static implicit operator NuGenTrafo2D(NuGenNuGenScale2D s)
		{
			return s.ToNuGenTrafo2D();
		}

		public static NuGenVec2D operator*(NuGenVec2D v, NuGenNuGenScale2D t)
		{
			return new NuGenVec2D(
				v._x[0] * t.v._x[0],
				v._x[1] * t.v._x[1]
				);
		}

		public static NuGenPnt2D operator*(NuGenPnt2D p, NuGenNuGenScale2D t)
		{
			return new NuGenPnt2D(
				p._x[0] * t.v._x[0],
				p._x[1] * t.v._x[1]
				);
		}

		public static NuGenBox2D operator*(NuGenBox2D b, NuGenNuGenScale2D t)
		{
			return new NuGenBox2D(b.Lower * t, b.Upper * t);
		}

		public static NuGenTrafo2D operator*(NuGenShift2D a, NuGenNuGenScale2D b)
		{
			return a.ToNuGenTrafo2D() * b.ToNuGenTrafo2D();
		}

		public static NuGenTrafo2D operator*(NuGenRot2D a, NuGenNuGenScale2D b)
		{
			return a.ToNuGenTrafo2D() * b.ToNuGenTrafo2D();
		}

		private NuGenVec2D v;

	}

}

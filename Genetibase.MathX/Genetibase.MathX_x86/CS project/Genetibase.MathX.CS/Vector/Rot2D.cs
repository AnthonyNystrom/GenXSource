
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenRot2D.
	/// </summary>
	public struct NuGenRot2D
	{

		public static NuGenRot2D Identity = new NuGenRot2D(0);

		public NuGenRot2D(double radians)
		{
			r = radians;
			cos = Math.Cos(radians);
			sin = Math.Sin(radians);
		}

		public NuGenTrafo2D ToNuGenTrafo2D()
		{
			return new NuGenTrafo2D(
				cos, sin, 0,
				-sin, cos, 0,
				0, 0, 1
				);
		}

		public static implicit operator NuGenTrafo2D(NuGenRot2D r)
		{
			return r.ToNuGenTrafo2D();
		}

		public static NuGenVec2D operator*(NuGenVec2D v, NuGenRot2D r)
		{
			return v * r.ToNuGenTrafo2D();
		}

		public static NuGenPnt2D operator*(NuGenPnt2D p, NuGenRot2D r)
		{
			return p * r.ToNuGenTrafo2D();
		}

		public static NuGenTrafo2D operator*(NuGenShift2D t, NuGenRot2D r)
		{
			return t.ToNuGenTrafo2D() * r.ToNuGenTrafo2D();
		}

		public static NuGenTrafo2D operator*(NuGenNuGenScale2D t, NuGenRot2D r)
		{
			return t.ToNuGenTrafo2D() * r.ToNuGenTrafo2D();
		}

		private double r;
		private double cos;
		private double sin;

	}

}

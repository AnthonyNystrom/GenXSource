
using System;

namespace Genetibase.MathX.Shapes
{

	/// <summary>
	/// NuGenTriangle3D.
	/// </summary>
	public struct NuGenTriangle3D
	{

		public NuGenTriangle3D(NuGenPnt3D a, NuGenPnt3D b, NuGenPnt3D c)
		{
			p0 = a; p1 = b; p2 = c;
		}

		public NuGenPnt3D this [int index]
		{
			get
			{
								
				switch (index)
				{

					case 0: return p0;
					case 1: return p1;
					case 2: return p2;
					default: return p0;
				}
			}
			set
			{
								
				switch (index)
				{

					case 0: p0 = value; break;
					case 1: p1 = value; break;
					case 2: p2 = value; break;
				}
			}
		}

		public bool Intersect(
			NuGenRay3D ray, ref double t, ref double u, ref double v, ref NuGenVec3D normal
			)
		{
			NuGenVec3D e1 = p1 - p0;
			NuGenVec3D e2 = p2 - p0;
			NuGenVec3D p = NuGenVec3D.Cross(ray.Direction, e2);

			double a = NuGenVec3D.Dot(e1, p);

			if (a > -NuGenVector.TINY_DOUBLE && a < NuGenVector.TINY_DOUBLE) return false;

			double f = 1.0 / a;
			NuGenVec3D s = ray.Point - p0;
			u = f * NuGenVec3D.Dot(s, p);

			if (u < 0.0 || u > 1.0) return false;
			NuGenVec3D q = NuGenVec3D.Cross(s, e1);
			v = f * NuGenVec3D.Dot(ray.Direction, q);

			if (v < 0.0 || (u + v) > 1.0) return false;
			t = f * NuGenVec3D.Dot(e2, q);
			normal = NuGenVec3D.Cross(e1, e2); normal.Normalize();
			return true;
		}

		public NuGenPnt3D p0;
		public NuGenPnt3D p1;
		public NuGenPnt3D p2;

	}

}

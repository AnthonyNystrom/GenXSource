
using System;

namespace Genetibase.MathX.Shapes
{

	/// <summary>
	/// NuGenSphere3D.
	/// </summary>
	public struct NuGenSphere3D
	{

		public static NuGenSphere3D UnitSphere = new NuGenSphere3D(new NuGenPnt3D(0,0,0), 1.0);

		public NuGenSphere3D(NuGenPnt3D center, double radius)
		{
			this.center = center;
			this.radius = radius;
		}

		public NuGenPnt3D Center
		{
			get 
			{
				return center; 
			}
			set 
			{
				center = value; 
			}
		}

		public double Radius
		{
			get 
			{
				return radius; 
			}
			set 
			{
				radius = value; 
			}
		}

		public bool Intersect(NuGenRay3D ray, ref double t, ref NuGenVec3D normal)
		{
			NuGenVec3D l = center - ray.Point;

			double s = NuGenVec3D.Dot(l, ray.Direction);
			double l2 = l.SquaredLength;
			double rr = radius * radius;

			if (s < 0.0 && l2 > rr) return false;

			double m2 = l2 - s * s;

			if (m2 > rr) return false;

			double q = Math.Sqrt(rr - m2);

			if (l2 > rr) t = s - q;

			else t = s + q;
			normal = (ray.Point + ray.Direction * t) - center;
			normal.Normalize();
			return true;
		}

		private NuGenPnt3D center;
		private double radius;

	}

}

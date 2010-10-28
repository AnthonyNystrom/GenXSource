
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenShift3D.
	/// </summary>
	public struct NuGenShift3D
	{

		public static NuGenShift3D Identity = new NuGenShift3D(0,0,0);

		public NuGenShift3D(double x, double y, double z)
		{
			v = new NuGenVec3D(x,y,z);
		}

		public NuGenShift3D(NuGenVec3D shift)
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

		public double z 
		{
			get 
			{
				return v._x[2]; 
			} 
		}

		public NuGenTrafo3D ToNuGenTrafo3D()
		{
			return new NuGenTrafo3D(
				1, 0, 0, 0,
				0, 1, 0, 0,
				0, 0, 1, 0,
				v._x[0], v._x[1], v._x[2], 1
				);
		}

		public static implicit operator NuGenTrafo3D(NuGenShift3D r)
		{
			return r.ToNuGenTrafo3D();
		}

		public static NuGenShift3D operator*(NuGenShift3D a, NuGenShift3D b)
		{
			return new NuGenShift3D(a.v + b.v);
		}

		public static NuGenTrafo3D operator*(NuGenScale3D a, NuGenShift3D b)
		{
			return new NuGenTrafo3D(
				a.x, 0, 0, 0,
				0, a.y, 0, 0,
				0, 0, a.z, 0,
				b.x, b.y, b.z, 1
				);
		}

		public static NuGenTrafo3D operator*(NuGenRot3D a, NuGenShift3D b)
		{
			return a.ToNuGenTrafo3D() * b.ToNuGenTrafo3D();
		}

		public static NuGenTrafo3D operator*(NuGenTrafo3D a, NuGenShift3D b)
		{
			a._x[12] += b.x;
			a._x[13] += b.y;
			a._x[14] += b.z;
			return a;
		}

		public static NuGenVec3D operator*(NuGenVec3D v, NuGenShift3D t)
		{
			return v;
		}

		public static NuGenPnt3D operator*(NuGenPnt3D p, NuGenShift3D t)
		{
			return p + t.v;
		}

		public static NuGenRay3D operator*(NuGenRay3D r, NuGenShift3D t)
		{
			return new NuGenRay3D(r.p * t, r.v * t);
		}

		public static NuGenBox3D operator*(NuGenBox3D b, NuGenShift3D t)
		{
			return new NuGenBox3D(b.Lower + t.v, b.Upper + t.v);
		}

		internal NuGenVec3D v;

	}

}

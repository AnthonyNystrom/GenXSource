
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenRay3D.
	/// </summary>
	public struct NuGenRay3D
	{

		public NuGenRay3D(NuGenPnt3D p, NuGenVec3D v)
		{
			this.p = p;
			this.v = v;
		}

		public NuGenPnt3D Point
		{
			get 
			{
				return p; 
			}
			set 
			{
				p = value; 
			}
		}

		public NuGenVec3D Direction
		{
			get 
			{
				return v; 
			}
			set 
			{
				v = value; 
			}
		}

		public static implicit operator NuGenRay3D(NuGenRay3F r)
		{
			return new NuGenRay3D(r.Point, r.Direction);
		}

		public NuGenPnt3D GetPointOnRay(double t)
		{
			return p + v * t;
		}

		internal NuGenPnt3D p;
		internal NuGenVec3D v;

	}
    
}

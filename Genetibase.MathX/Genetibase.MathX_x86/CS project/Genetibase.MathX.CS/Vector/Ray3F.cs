
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenRay3F.
	/// </summary>
	public struct NuGenRay3F
	{

		public NuGenRay3F(NuGenPnt3F p, NuGenVec3F v)
		{
			this.p = p;
			this.v = v;
		}

		public NuGenPnt3F Point
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

		public NuGenVec3F Direction
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

		public static explicit operator NuGenRay3F(NuGenRay3D r)
		{
			return new NuGenRay3F((NuGenPnt3F)r.p, (NuGenVec3F)r.v);
		}

		public NuGenPnt3F GetPointOnRay(float t)
		{
			return p + v * t;
		}

		internal NuGenPnt3F p;
		internal NuGenVec3F v;

	}
    
}

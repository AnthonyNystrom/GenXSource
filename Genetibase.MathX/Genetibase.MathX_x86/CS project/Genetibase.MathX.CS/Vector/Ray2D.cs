
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenRay2D.
	/// </summary>
	public struct NuGenRay2D
	{

		public NuGenRay2D(NuGenPnt2D p, NuGenVec2D v)
		{
			this.p = p;
			this.v = v;
		}

		public NuGenPnt2D Point
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

		public NuGenVec2D Direction
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

		public static implicit operator NuGenRay2D(NuGenRay2F r)
		{
			return new NuGenRay2D(r.Point, r.Direction);
		}

		public NuGenPnt2D GetPointOnRay(double t)
		{
			return p + v * t;
		}

		internal NuGenPnt2D p;
		internal NuGenVec2D v;

	}
    
}

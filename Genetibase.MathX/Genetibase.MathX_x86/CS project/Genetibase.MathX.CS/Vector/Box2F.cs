
using System;

namespace Genetibase.MathX
{

	/// <summary>
	/// NuGenBox2F.
	/// </summary>
	public struct NuGenBox2F
	{

		public static NuGenBox2F Empty = new NuGenBox2F(NuGenVector.HUGE_NuGenPnt2F, -NuGenVector.HUGE_NuGenPnt2F);

		public NuGenBox2F(NuGenPnt2F lower, NuGenPnt2F upper)
		{
			this.lower = lower;
			this.upper = upper;
		}

		public NuGenPnt2F Lower
		{
			get 
			{
				return lower; 
			}
			set 
			{
				lower = value; 
			}
		}

		public NuGenPnt2F Upper
		{
			get 
			{
				return upper; 
			}
			set 
			{
				upper = value; 
			}
		}

		public NuGenPnt2F LL 
		{
			get 
			{
				return new NuGenPnt2F(lower._x[0], lower._x[1]); 
			} 
		}

		public NuGenPnt2F LU 
		{
			get 
			{
				return new NuGenPnt2F(lower._x[0], upper._x[1]); 
			} 
		}

		public NuGenPnt2F UL 
		{
			get 
			{
				return new NuGenPnt2F(upper._x[0], lower._x[1]); 
			} 
		}

		public NuGenPnt2F UU 
		{
			get 
			{
				return new NuGenPnt2F(upper._x[0], upper._x[1]); 
			} 
		}

		public int MinDim
		{
			get
			{
				NuGenVec2F dim = upper - lower;
				return (dim[0] < dim[1]) ? 0 : 1;
			}
		}

		public int MaxDim
		{
			get
			{
				NuGenVec2F dim = upper - lower;
				return (dim[0] > dim[1]) ? 0 : 1;
			}
		}

		public float Minimum
		{
			get
			{
				NuGenVec2F dim = upper - lower;
				return (dim[0] < dim[1]) ? dim[0] : dim[1];
			}
		}

		public float Maximum
		{
			get
			{
				NuGenVec2F dim = upper - lower;
				return (dim[0] > dim[1]) ? dim[0] : dim[1];
			}
		}

		public bool IsInside(NuGenPnt2F p)
		{
			return lower < p && upper > p;
		}

		public bool IsInsideOrOnBorder(NuGenPnt2F p)
		{
			return lower <= p && upper >= p;
		}

		public bool IsOnBorder(NuGenPnt2F p)
		{
			return IsInsideOrOnBorder(p) && !IsInside(p);
		}

		public bool IsInside(NuGenBox2F b)
		{
			return lower < b.lower && upper > b.upper;
		}

		public bool IsInsideOrOnBorder(NuGenBox2F b)
		{
			return lower <= b.lower && upper >= b.upper;
		}

		public bool IsOnBorder(NuGenBox2F b)
		{
			return IsInsideOrOnBorder(b) && !IsInside(b);
		}

		public int Relation(int dim, float val)
		{
						
			if (upper[dim] < val) return -1;
			if (lower[dim] > val) return 1;
			return 0;
		}

		public static NuGenBox2F operator+(NuGenBox2F b, NuGenPnt2F p)
		{
			return new NuGenBox2F(NuGenPnt2F.Min(b.lower, p), NuGenPnt2F.Max(b.upper, p));
		}

		public static NuGenBox2F operator+(NuGenBox2F b, NuGenBox2F c)
		{
			return new NuGenBox2F(NuGenPnt2F.Min(b.lower, c.lower), NuGenPnt2F.Max(b.upper, c.upper));
		}

		public static bool ApproxEqual(NuGenBox2F a, NuGenBox2F b)
		{
			return
				NuGenVector.ApproxEquals(a.Lower, b.Lower) &&
				NuGenVector.ApproxEquals(a.Upper, b.Upper);
		}

		public override bool Equals(object obj)
		{
			NuGenBox2F x = (NuGenBox2F)obj;
			return (lower == x.Lower && upper == x.Upper);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return "NuGenBox2F(" + lower + ", " + upper + ")";
		}

		internal NuGenPnt2F lower;
		internal NuGenPnt2F upper;

	}

}

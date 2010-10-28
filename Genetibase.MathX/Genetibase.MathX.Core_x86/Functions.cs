using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents sealed class with set of elementary mathematical functions.</summary>
	public sealed class Functions
	{
		private Functions()
		{}

		/// <summary>Represents sealed class with set of trigonometric functions.</summary>
		public sealed class Trigonometric
		{
			private Trigonometric()
			{}

			public static double Sin(double x)
			{
				return Math.Sin(x);
			}

			public static double Cos(double x)
			{
				return Math.Cos(x);
			}

			public static double Tan(double x)
			{
				return Math.Tan(x);
			}

			public static double Cot(double x)
			{
				return 1/Math.Tan(x);
			}
			
			public static double Sec(double x)
			{
				return 1/Math.Cos(x);
			}

			public static double Csc(double x)
			{
				return 1/Math.Sin(x);
			}

			// Inverse functions

			public static double Asin(double x)
			{
				return Math.Asin(x);
			}

			public static double Acos(double x)
			{
				return Math.Acos(x);
			}

			public static double Atan(double x)
			{
				return Math.Atan(x);
			}

			public static double Acot(double x)
			{
				return Math.PI/2 - Math.Atan(x);
			}
			
			public static double Asec(double x)
			{
				return Math.Acos(1/x);
			}

			public static double Acsc(double x)
			{
				return Math.Asin(1/x);
			}
		}

		/// <summary>Represents sealed class with set of hyperbolic functions.</summary>
		public sealed class Hyperbolic
		{
			private Hyperbolic()
			{}

			public static double Sinh(double x)
			{
				return Math.Sinh(x);
			}

			public static double Cosh(double x)
			{
				return Math.Cosh(x);
			}

			public static double Tanh(double x)
			{
				return Math.Tanh(x);
			}

			public static double Coth(double x)
			{
				return 1/Math.Tanh(x);
			}

			public static double Sech(double x)
			{
				return 1/Math.Cosh(x);
			}

			public static double Csch(double x)
			{
				return 1/Math.Sinh(x);
			}
		
			// Inverse functions

			public static double Asinh(double x)
			{
				return Math.Log(x + Math.Sqrt((x * x) + 1));
			}

			public static double Acosh(double x)
			{
				return Math.Log(x + Math.Sqrt((x * x) - 1));
			}

			public static double Atanh(double x)
			{
				return 0.5 * Math.Log((1 + x)/(1 - x));
			}

			public static double Acoth(double x)
			{
				return 0.5 * Math.Log((x + 1)/(x - 1));
			}

			public static double Asech(double x)
			{
				return Acosh(1/x);
			}

			public static double Acsch(double x)
			{
				return Asinh(1/x);
			}

		}




	}
}

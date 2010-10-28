using System;
using System.Runtime.InteropServices;

namespace Genetibase.MathX.Core
{

	/// <summary>Differentiates numerically defined function.</summary>
	public sealed class NumericalDifferentiator
	{
		// Methods
		private NumericalDifferentiator(RealFunction function)
		{
			this._function = function;
		}

		private double BackwardDerivative(double x)
		{
			return NumericalDifferentiator.BackwardDerivative(this._function, x);
		}

		private double FormardDerivative(double x)
		{
			return NumericalDifferentiator.ForwardDerivative(this._function, x);
		}

		public static double BackwardDerivative(RealFunction targetFunction, double x)
		{
			double num1;
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			return NumericalDifferentiator.BackwardDerivative(targetFunction, x, out num1);
		}

		public static double BackwardDerivative(RealFunction targetFunction, double x, out double estimatedError)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			double step = Constants.SqrtEpsilon;
			double[] xCoord = new double[3];
			double[] yCoord = new double[3];
			int i = 0;
			while (i < 3)
			{
				xCoord[i] = x + ((i - 2) * step);
				yCoord[i] = targetFunction(xCoord[i]);
				i++;
			}
			for (int j = 1; j < 4; j++)
			{
				for (i = 0; i < (3 - j); i++)
				{
					yCoord[i] = (yCoord[i + 1] - yCoord[i]) / (xCoord[i + j] - xCoord[i]);
				}
			}
			double sum = Math.Abs((double) ((yCoord[0] + yCoord[1]) + yCoord[2]));
			if (sum < Constants.SqrtEpsilon100)
			{
				sum = Constants.SqrtEpsilon100;
			}
			step = Math.Sqrt(Constants.SqrtEpsilon / (2 * sum));
			if (step > Constants.SqrtEpsilon100)
			{
				step = Constants.SqrtEpsilon100;
			}
			estimatedError = Math.Abs((double) ((10 * sum) * step));
			return ((targetFunction(x) - targetFunction(x - step)) / step);
		}

		private double CentralDerivative(double x)
		{
			return NumericalDifferentiator.CentralDerivative(this._function, x);
		}

		public static double CentralDerivative(RealFunction targetFunction, double x)
		{
			double num1;
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			return NumericalDifferentiator.CentralDerivative(targetFunction, x, out num1);
		}

		public static double CentralDerivative(RealFunction targetFunction, double x, out double estimatedError)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			double step = Constants.SqrtEpsilon;
			double[] xCoord = new double[4];
			double[] yCoord = new double[4];
			int i = 0;
			while (i < 4)
			{
				xCoord[i] = x + ((i - 2) * step);
				yCoord[i] = targetFunction(xCoord[i]);
				i++;
			}
			for (int j = 1; j < 5; j++)
			{
				for (i = 0; i < (4 - j); i++)
				{
					yCoord[i] = (yCoord[i + 1] - yCoord[i]) / (xCoord[i + j] - xCoord[i]);
				}
			}
			double sum = Math.Abs((double) (((yCoord[0] + yCoord[1]) + yCoord[2]) + yCoord[3]));
			if (sum < Constants.SqrtEpsilon100)
			{
				sum = Constants.SqrtEpsilon100;
			}
			step = Math.Pow(Constants.SqrtEpsilon / (2 * sum), 0.33333333333333331);
			if (step > Constants.SqrtEpsilon100)
			{
				step = Constants.SqrtEpsilon100;
			}
			estimatedError = Math.Abs((double) (((100 * sum) * step) * step));
			return ((targetFunction(x + step) - targetFunction(x - step)) / (2 * step));
		}

		public static RealFunction CreateBackwardDelegate(RealFunction targetFunction)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			NumericalDifferentiator differentiator1 = new NumericalDifferentiator(targetFunction);
			return new RealFunction(differentiator1.BackwardDerivative);
		}

		public static RealFunction CreateDelegate(RealFunction targetFunction)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			NumericalDifferentiator differentiator1 = new NumericalDifferentiator(targetFunction);
			return new RealFunction(differentiator1.CentralDerivative);
		}

		public static RealFunction CreateForwardDelegate(RealFunction targetFunction)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			NumericalDifferentiator differentiator1 = new NumericalDifferentiator(targetFunction);
			return new RealFunction(differentiator1.FormardDerivative);
		}

		public static double Derivative(RealFunction targetFunction, double x)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			return NumericalDifferentiator.Derivative(targetFunction, x, DifferencesDirection.Central);
		}

		public static double Derivative(RealFunction targetFunction, double x, DifferencesDirection direction)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			switch (direction)
			{
				case DifferencesDirection.Backward:
				{
					return NumericalDifferentiator.BackwardDerivative(targetFunction, x);
				}
				case DifferencesDirection.Central:
				{
					return NumericalDifferentiator.CentralDerivative(targetFunction, x);
				}
				case DifferencesDirection.Forward:
				{
					return NumericalDifferentiator.ForwardDerivative(targetFunction, x);
				}
			}
			throw new ArgumentOutOfRangeException("direction");
		}

		public static double ForwardDerivative(RealFunction targetFunction, double x)
		{
			double num1;
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			return NumericalDifferentiator.ForwardDerivative(targetFunction, x, out num1);
		}

		public static double ForwardDerivative(RealFunction targetFunction, double x, out double estimatedError)
		{
			if (targetFunction == null)
			{
				throw new ArgumentNullException("targetFunction");
			}
			double step = Constants.SqrtEpsilon;
			double[] xCoord = new double[3];
			double[] yCoord = new double[3];
			int i = 0;
			while (i < 3)
			{
				xCoord[i] = x + (i * step);
				yCoord[i] = targetFunction(xCoord[i]);
				i++;
			}
			for (int j = 1; j < 4; j++)
			{
				for (i = 0; i < (3 - j); i++)
				{
					yCoord[i] = (yCoord[i + 1] - yCoord[i]) / (xCoord[i + j] - xCoord[i]);
				}
			}
			double sum = Math.Abs((double) ((yCoord[0] + yCoord[1]) + yCoord[2]));
			if (sum < Constants.SqrtEpsilon100)
			{
				sum = Constants.SqrtEpsilon100;
			}
			step = Math.Sqrt(Constants.SqrtEpsilon / (2 * sum));
			if (step > Constants.SqrtEpsilon100)
			{
				step = Constants.SqrtEpsilon100;
			}
			estimatedError = Math.Abs((double) ((10 * sum) * step));
			return ((targetFunction(x + step) - targetFunction(x)) / step);
		}


		// Fields
		private RealFunction _function;
	}
}


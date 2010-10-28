using System;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	
	public class ConstantFunctionPlotter
	{
		private ConstantFunction _function;

		public ConstantFunctionPlotter(Constant function) 
		{
			_function = function.ValueAt;
		}

		private double[] Plot (double min, double max, double step, int numPoints)
		{
			double[] result = (double[])Array.CreateInstance(typeof(double),numPoints);
			
			double x = min;
			for ( int i = 0; i < numPoints; i++ )
			{
				result[i] = _function();
				x += step;
			}

			return result;
		}

		public double[] Plot(double min, double max, double step)
		{
			return Plot(min, max, step, 1 + (int)((max - min)/step));
		}

		public double[] Plot(double min, double max, int numPoints)
		{
			return Plot(min,max,(max - min) / (numPoints - 1),numPoints);
		}

		public double[] Plot(double[] values)
		{
			double[] result = (double[])Array.CreateInstance(typeof(double),values.Length);

			for (int i = 0; i < values.Length; i++)
				result[i] = _function();	
			return result;
		}

	}
}

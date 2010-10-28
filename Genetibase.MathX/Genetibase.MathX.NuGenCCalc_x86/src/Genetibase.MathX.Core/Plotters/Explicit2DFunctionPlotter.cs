using System;
using System.Drawing;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Explicit2DFunctionPlotter.
	/// </summary>
	public class Explicit2DFunctionPlotter
	{
		private RealFunction _function;

		public Explicit2DFunctionPlotter(Explicit2DFunction function) 
		{
			_function = function.ValueAt;
		}


		private double[] Plot (double min, double max, double step, int numPoints)
		{
			double[] result = (double[])Array.CreateInstance(typeof(double),numPoints);
			
			double x = min;
			for ( int i = 0; i < numPoints; i++ )
			{
				result[i] = _function(x);
				x += step;
			}

			return result;
		}

		public double[] Plot (Point2D a, Point2D b, Size areaSize)
		{
			if (areaSize.Height == 0 || areaSize.Width == 0)
				throw new ArgumentException("Height or Width cannot be zero","areaSize");

			int numPoints = 1000;
			return Plot(Math.Min(a.X,b.X),Math.Max(a.X,b.X),Math.Abs(a.X - b.X) / (numPoints - 1),numPoints);
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
				result[i] = _function(values[i]);	
			return result;
		}






	}
}

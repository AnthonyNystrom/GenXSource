using System;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Explicit2DFunctionPlotter.
	/// </summary>
	public class Parameter2DFunctionPlotter
	{
		private Parameter2DFunctionDelegate _function;

		public Parameter2DFunctionPlotter(Parameter2DFunction function) 
		{		
			_function = function.ValueAt;
		}

		private Point2D[] Plot (double min, double max, double step, int numPoints)
		{
			Point2D[] result = (Point2D[])Array.CreateInstance(typeof(Point2D),numPoints);
			
			double t = min;
			for ( int i = 0; i < numPoints; i++ )
			{
				result[i] = _function(t);
				t += step;
			}

			return result;
		}

		public Point2D[] Plot(double min, double max, double step)
		{
			return Plot(min, max, step, 1 + (int)((max - min)/step));
		}

		public Point2D[] Plot(double min, double max, int numPoints)
		{
			return Plot(min,max,(max - min) / (numPoints - 1),numPoints);
		}

		public Point2D[] Plot(double[] values)
		{
			Point2D[] result = (Point2D[])Array.CreateInstance(typeof(Point2D),values.Length);

			for (int i = 0; i < values.Length; i++)
				result[i] = _function(values[i]);	
			return result;
		}






	}
}

using System;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Explicit2DFunctionPlotter.
	/// </summary>
	public class Parameter3DFunctionPlotter
	{
		private Parameter3DFunctionDelegate _function;

		public Parameter3DFunctionPlotter(Parameter3DFunction function) 
		{	
			_function = function.ValueAt;
		}

		private Point3D[] Plot (double min, double max, double step, int numPoints)
		{
			Point3D[] result = (Point3D[])Array.CreateInstance(typeof(Point3D),numPoints);
			
			double t = min;
			for ( int i = 0; i < numPoints; i++ )
			{
				result[i] = _function(t);
				t += step;
			}

			return result;
		}

		public Point3D[] Plot(double min, double max, double step)
		{
			return Plot(min, max, step, 1 + (int)((max - min)/step));
		}

		public Point3D[] Plot(double min, double max, int numPoints)
		{
			return Plot(min,max,(max - min) / (numPoints - 1),numPoints);
		}

		public Point3D[] Plot(double[] values)
		{
			Point3D[] result = (Point3D[])Array.CreateInstance(typeof(Point3D),values.Length);

			for (int i = 0; i < values.Length; i++)
				result[i] = _function(values[i]);	
			return result;
		}

	}
}

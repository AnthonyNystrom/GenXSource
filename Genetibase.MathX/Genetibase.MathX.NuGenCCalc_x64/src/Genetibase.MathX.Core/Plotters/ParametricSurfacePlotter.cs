using System;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Explicit2DFunctionPlotter.
	/// </summary>
	public class ParametricSurfacePlotter
	{
		private ParametricSurfaceDelegate _function;		

		public ParametricSurfacePlotter(ParametricSurface function) 
		{	
			_function = function.ValueAt;
		}

		public Point3D[] Plot (double minU, double maxU, double minV, double maxV, int numPoints)
		{
			Point3D[] result = (Point3D[])Array.CreateInstance(typeof(Point3D),numPoints);

			double areaU = (maxU-minU);
			double areaV = (maxV-minV);
						
			double stepSize = Math.Sqrt(areaU*areaV / numPoints);
									
			double u = minU;
			double v = minV;
			for (int i = 0; i < numPoints; i++)
			{
				result[i] = _function(u,v);                
				u += stepSize;
				if ( u > maxU )
				{
					v += stepSize;
					u = minU;
				}							
			}

			return result;
		}
		
	}
}

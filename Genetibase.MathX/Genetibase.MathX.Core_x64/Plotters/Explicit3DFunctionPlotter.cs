using System;
using System.Drawing;
using Genetibase.MathX.Core;


namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Explicit2DFunctionPlotter.
	/// </summary>
	public class Explicit3DFunctionPlotter
	{
		private BivariateRealFunction _function;

		public Explicit3DFunctionPlotter(Explicit3DFunction function)
		{	
			_function = function.ValueAt;
		}


		public double[,] Plot(Point2D p1 , Point2D p2 , Size grid)
		{
			if (grid.Height == 0 || grid.Width == 0)
				throw new ArgumentException("Height or Width cannot be zero","grid");

			double[,] result = (double[,])Array.CreateInstance(typeof(double),grid.Height,grid.Width);

			double gridStepX = Math.Abs(p2.X - p1.X) / ( grid.Width - 1 );
			double gridStepY = Math.Abs(p2.Y - p1.Y) / ( grid.Height - 1 );

			double y = p1.Y;
			for (int j = 0; j < grid.Height; j++)
			{
				double x = p1.X;
				for (int i = 0; i < grid.Width; i++)
				{
					result[j,i] = _function(x,y);
					x += gridStepX;
				}
				y += gridStepY;
			}

			return result;
		}

	}
}

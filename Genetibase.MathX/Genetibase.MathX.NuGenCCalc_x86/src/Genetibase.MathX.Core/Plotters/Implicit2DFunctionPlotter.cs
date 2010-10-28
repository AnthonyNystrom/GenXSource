using System;
using System.Drawing;
using Genetibase.MathX.Core;

namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Implicit2DFunctionPlotter.
	/// </summary>
	public class Implicit2DFunctionPlotter
	{
		private BivariateRealFunction _function;

		private int _gridFactor = 20;
		private bool[,] _areaMatrix = null;
		private SizeF _pixelSize;
        private Size _areaSize;
		private Point2D _areaPointA;
		private Point2D _areaPointB;
		private int _totalPoints;

		public Implicit2DFunctionPlotter(Implicit2DFunction function)
		{
			_function = function.ValueAt;
		}
		


		public int GridFactor
		{
			get
			{
				return _gridFactor;
			}
			set
			{
				_gridFactor = value;
			}
		}

		public Point2D[] Plot (Point2D a, Point2D b, Size areaSize)
		{
			if (areaSize.Height == 0 || areaSize.Width == 0)
				throw new ArgumentException("Height or Width cannot be zero","areaSize");


			_areaPointA = new Point2D(Math.Min(a.X,b.X),Math.Min(a.Y,b.Y));
			_areaPointB = new Point2D(Math.Max(a.X,b.X),Math.Max(a.Y,b.Y));

			_areaSize = areaSize;
			_areaMatrix = (bool[,])Array.CreateInstance(typeof(bool),areaSize.Height,areaSize.Width);
			_pixelSize = new SizeF((float)Math.Abs(_areaPointB.X - _areaPointA.X)/areaSize.Width,(float)Math.Abs(_areaPointB.Y - _areaPointA.Y)/areaSize.Height);
		
			CalculateMatrix();

			Point2D[] result = (Point2D[])Array.CreateInstance(typeof(Point2D),_totalPoints);

			if (_totalPoints > 0)
			{
				int curPoint = 0;

				double y = _areaPointA.Y + _pixelSize.Height / 2;
				for (int j = 0; j < _areaSize.Height; j++)
				{
					double x = _areaPointA.X + _pixelSize.Width / 2;
					for (int i = 0; i < _areaSize.Width; i++)
					{
						if (_areaMatrix[j,i])
						{
							result[curPoint] = new Point2D(x,y);
							curPoint++;
						}

						x += _pixelSize.Width;
					}
					y += _pixelSize.Height;
				}
			
			}

			return result;
		}


		private void CalculateMatrix()
		{
			double gridStep = 0;
			int gridWidth;
			int gridHeight;

			_totalPoints = 0;

			if (Math.Abs(_areaPointA.Y - _areaPointB.Y) > Math.Abs(_areaPointA.X - _areaPointB.X))
			{
				gridStep = Math.Abs(_areaPointA.Y - _areaPointB.Y) / _gridFactor;
				gridHeight = _gridFactor;
				gridWidth = (int)(Math.Abs(_areaPointA.X - _areaPointB.X) / gridStep);
				gridWidth = ((double)(Math.Abs(_areaPointA.X - _areaPointB.X) % gridStep)) > 0 ? gridWidth + 1 : gridWidth;
			}
			else
			{
				gridStep = Math.Abs(_areaPointA.X - _areaPointB.X) / _gridFactor;
				gridWidth = _gridFactor;
				gridHeight = (int)(Math.Abs(_areaPointA.Y - _areaPointB.Y) / gridStep);
				gridHeight = ((double)(Math.Abs(_areaPointA.Y - _areaPointB.Y) % gridStep)) > 0 ? gridHeight + 1 : gridHeight;
			}

			double[] row1 = (double[])Array.CreateInstance(typeof(double),gridWidth + 1);
			double[] row2 = (double[])Array.CreateInstance(typeof(double),gridWidth + 1);

			double[] rowA; 
			double[] rowB;
			
			double y = _areaPointA.Y;
			double x = _areaPointA.X;
			for (int j = 0; j <= gridHeight; j++)
			{
				if ( j % 2 == 0)
				{
					rowA = row1;
					rowB = row2;
				}
				else
				{
					rowA = row2;
					rowB = row1;
				}

				x = _areaPointA.X;
				for (int i = 0; i <= gridWidth; i++)
				{
					rowA[i] = _function(x,y);
					x += gridStep;
				}

				if ( j > 0 )
				{
					double y1 = y - gridStep;
					x = _areaPointA.X;
					for (int i = 0; i < gridWidth; i++)
					{
						if (CheckScope(rowB[i],rowB[i+1],rowA[i],rowA[i+1]))
						{
							CalculateMatrixScope(new Point2D(x,y1),
								new Point2D(x + gridStep,y),
								rowB[i],rowB[i+1],rowA[i],rowA[i+1]);
						}
						x += gridStep;
					}
				}

				y += gridStep;
			}

		}

		private void CalculateMatrixScope (Point2D p1, Point2D p2, double a, double b, double c, double d)
		{
			double sizeX = Math.Abs(p1.X - p2.X);
			double sizeY = Math.Abs(p1.Y - p2.Y);

			double halfSizeX = sizeX / 2;
			double halfSizeY = sizeY / 2;

			Point2D pCenter = new Point2D ( p1.X + halfSizeX , p1.Y + halfSizeY );

			if ((sizeX < _pixelSize.Width)&&(sizeY < _pixelSize.Height))
			{
				int gridX = (int)(( pCenter.X - _areaPointA.X) / _pixelSize.Width);
				int gridY = (int)(( pCenter.Y - _areaPointA.Y) / _pixelSize.Height);
				if (!_areaMatrix[gridY,gridX])
				{
					_areaMatrix[gridY,gridX] = true;
					_totalPoints++;
				}
				
				return;
			}

			double ab = _function( pCenter.X , p1.Y );
			double cd = _function( pCenter.X , p2.Y );
			double ac = _function( p1.X , pCenter.Y );
			double bd = _function( p2.X , pCenter.Y );
			double center = _function( pCenter.X , pCenter.Y );
			
			if (CheckScope(a , ab , ac , center))
			CalculateMatrixScope( p1 , pCenter , 
				a , ab , ac , center );
			
			if (CheckScope(center , bd , cd , d))
			CalculateMatrixScope( pCenter , p2 , 
				center , bd , cd , d );

			if (CheckScope(ab , b , center , bd))
			CalculateMatrixScope(new Point2D( pCenter.X , p1.Y ),
				new Point2D( p2.X , pCenter.Y ),
				ab , b , center , bd );

			if (CheckScope(ac , center , c , cd))
			CalculateMatrixScope(new Point2D( p1.X , pCenter.Y ),
				new Point2D( pCenter.X , p2.Y ),
				ac , center , c , cd );
		}

		private bool CheckScope(double a, double b, double c, double d)
		{
			bool check = a >= 0;
			return (check != (b >= 0)) || (check != (c >= 0)) || (check != (d >= 0));
		}




	}
}

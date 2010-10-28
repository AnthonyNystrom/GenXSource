using System;
using System.Drawing;
using Genetibase.MathX.Core;
using System.Collections;

namespace Genetibase.MathX.Core.Plotters
{
	/// <summary>
	/// Summary description for Implicit3DFunctionPlotter.
	/// </summary>
	public class Implicit3DFunctionPlotter
	{
		private TrivariateRealFunction _function;

		private int _gridFactor = 20;
		
		private ArrayList _points = new ArrayList();

		private double _pixelSizeX;
		private double _pixelSizeY;
		private double _pixelSizeZ;

		private Point3D _areaPointA;
		private Point3D _areaPointB;

		public Implicit3DFunctionPlotter(Implicit3DFunction function) 
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

		public Point3D[] Plot (Point3D a, Point3D b, double pixelSizeX, double pixelSizeY, double pixelSizeZ )
		{
			_areaPointA = new Point3D(Math.Min(a.X,b.X),Math.Min(a.Y,b.Y),Math.Min(a.Z,b.Z));
			_areaPointB = new Point3D(Math.Max(a.X,b.X),Math.Max(a.Y,b.Y),Math.Max(a.Z,b.Z));

			_pixelSizeX = pixelSizeX;
			_pixelSizeY = pixelSizeY;
			_pixelSizeZ = pixelSizeZ;
			
			CalculateCube();

			Point3D[] result = (Point3D[])_points.ToArray(typeof(Point3D));

			return result;
		}


		private void CalculateCube()
		{
			double gridStep = 0;
			
			int gridSizeX;
			int gridSizeY;
			int gridSizeZ;

			_points.Clear();

			double cubeSizeX = Math.Abs(_areaPointA.X - _areaPointB.X);
			double cubeSizeY = Math.Abs(_areaPointA.Y - _areaPointB.Y);
			double cubeSizeZ = Math.Abs(_areaPointA.Z - _areaPointB.Z);

			if ( ( cubeSizeX >= cubeSizeY ) && ( cubeSizeX >= cubeSizeZ) )
			{
				gridStep = cubeSizeX / _gridFactor;
				gridSizeX = _gridFactor;

				gridSizeY = (int)(cubeSizeY / gridStep);
				gridSizeY = (double)(cubeSizeY % gridStep) > 0 ? gridSizeY + 1 : gridSizeY;

				gridSizeZ = (int)(cubeSizeZ / gridStep);
				gridSizeZ = (double)(cubeSizeZ % gridStep) > 0 ? gridSizeZ + 1 : gridSizeZ;			
			}
			else
				if ( ( cubeSizeY >= cubeSizeX ) && ( cubeSizeY >= cubeSizeZ) )
			{
				gridStep = cubeSizeY / _gridFactor;
				gridSizeY = _gridFactor;

				gridSizeX = (int)(cubeSizeX / gridStep);
				gridSizeX = (double)(cubeSizeX % gridStep) > 0 ? gridSizeX + 1 : gridSizeX;

				gridSizeZ = (int)(cubeSizeZ / gridStep);
				gridSizeZ = (double)(cubeSizeZ % gridStep) > 0 ? gridSizeZ + 1 : gridSizeZ;						
			}
			else
			{
				gridStep = cubeSizeZ / _gridFactor;
				gridSizeZ = _gridFactor;

				gridSizeX = (int)(cubeSizeX / gridStep);
				gridSizeX = (double)(cubeSizeX % gridStep) > 0 ? gridSizeX + 1 : gridSizeX;

				gridSizeY = (int)(cubeSizeY / gridStep);
				gridSizeY = (double)(cubeSizeY % gridStep) > 0 ? gridSizeY + 1 : gridSizeY;
			}
			
			double[,] matrix1 = (double[,])Array.CreateInstance(typeof(double), gridSizeY + 1 , gridSizeX + 1 );
			double[,] matrix2 = (double[,])Array.CreateInstance(typeof(double), gridSizeY + 1 , gridSizeX + 1 );

			double[,] matrixA; 
			double[,] matrixB;
			
			double z = _areaPointA.Z;
			for (int k = 0; k <= gridSizeZ; k++)
			{
				if ( k % 2 == 0)
				{
					matrixA = matrix1;
					matrixB = matrix2;
				}
				else
				{
					matrixA = matrix2;
					matrixB = matrix1;
				}
				
				double y = _areaPointA.Y;
				for (int j = 0; j <= gridSizeY; j++)
				{
					double x = _areaPointA.X;
					for (int i = 0; i <= gridSizeX; i++)
					{
						matrixA[j,i] = _function(x,y,z);
						x += gridStep;
					}
					y += gridStep;
				}

				if ( k > 0 )
				{
					y = _areaPointA.Y;
					for (int j = 0; j < gridSizeY; j++)
					{
						double x = _areaPointA.X;
						for (int i = 0; i < gridSizeX; i++)
						{
							if (CheckScope(
								matrixB[j,i],
								matrixB[j,i+1],
								matrixB[j+1,i],
								matrixB[j+1,i+1],
								matrixA[j,i],
								matrixA[j,i+1],
								matrixA[j+1,i],
								matrixA[j+1,i+1]))
							{
								CalculateCubeScope(
									new Point3D(x,y,z - gridStep),
									new Point3D(x + gridStep, y + gridStep ,z),
									matrixB[j,i],
									matrixB[j,i+1],
									matrixB[j+1,i],
									matrixB[j+1,i+1],
									matrixA[j,i],
									matrixA[j,i+1],
									matrixA[j+1,i],
									matrixA[j+1,i+1]);
							}
							x += gridStep;
						}
						y += gridStep;
					}
				}

				z += gridStep;
			}

		}

		private void CalculateCubeScope (Point3D p1, Point3D p2,
			double a, double b, double c, double d, 
			double a1, double b1, double c1, double d1)
		{
			double sizeX = Math.Abs(p1.X - p2.X);
			double sizeY = Math.Abs(p1.Y - p2.Y);
			double sizeZ = Math.Abs(p1.Z - p2.Z);

			double halfSizeX = sizeX / 2;
			double halfSizeY = sizeY / 2;
			double halfSizeZ = sizeZ / 2;

			Point3D pCenter = new Point3D ( p1.X + halfSizeX , p1.Y + halfSizeY, p1.Z + halfSizeZ );

			if ((sizeX < _pixelSizeX)
				&&(sizeY < _pixelSizeY)
				&&(sizeZ < _pixelSizeZ)
				)
			{
				_points.Add(pCenter);
				return;
			}

			double center = _function( pCenter.X , pCenter.Y, pCenter.Z );

			double ab = _function ( pCenter.X , p1.Y , p1.Z );
			double ac = _function ( p1.X , pCenter.Y , p1.Z );
			double cd = _function ( pCenter.X , p2.Y , p1.Z );
			double bd = _function ( p2.X , pCenter.Y , p1.Z );
			
			double a1b1 = _function ( pCenter.X , p1.Y , p2.Z );
			double a1c1 = _function ( p1.X , pCenter.Y , p2.Z );
			double c1d1 = _function ( pCenter.X , p2.Y , p2.Z );
			double b1d1 = _function ( p2.X , pCenter.Y , p2.Z );
			
			double aa1 = _function ( p1.X , p1.Y , pCenter.Z );
			double bb1 = _function ( p2.X , p1.Y , pCenter.Z );
			double cc1 = _function ( p1.X , p2.Y , pCenter.Z );
			double dd1 = _function ( p2.X , p2.Y , pCenter.Z );

			double ab_a1b1 = _function ( pCenter.X , p1.Y , pCenter.Z );
			double bd_b1d1 = _function ( p2.X , pCenter.Y , pCenter.Z );
			double cd_c1d1 = _function ( pCenter.X , p2.Y , pCenter.Z );
			double ac_a1c1 = _function ( p1.X , pCenter.Y , pCenter.Z );

			double abcd = _function ( pCenter.X , pCenter.Y , p1.Z );
			double a1b1c1d1 = _function ( pCenter.X , pCenter.Y , p2.Z );


			if (CheckScope( a, ab, ac, abcd, aa1, ab_a1b1, ac_a1c1, center))
				CalculateCubeScope( p1, pCenter,
					a, ab, ac, abcd, aa1, ab_a1b1, ac_a1c1, center);

			if (CheckScope( ab, b, abcd, bd, ab_a1b1, bb1, center, bd_b1d1))
				CalculateCubeScope( new Point3D ( pCenter.X, p1.Y, p1.Z),
					new Point3D ( p2.X, pCenter.Y, pCenter.Z),
					ab, b, abcd, bd, ab_a1b1, bb1, center, bd_b1d1);

			if (CheckScope( ac, abcd, c, cd, ac_a1c1, center, cc1,cd_c1d1))
				CalculateCubeScope( new Point3D(p1.X,pCenter.Y,p1.Z), new Point3D(pCenter.X,p2.Y,pCenter.Z),
					ac, abcd, c, cd, ac_a1c1, center, cc1,cd_c1d1);

			if (CheckScope( abcd, bd, cd, d, center, bd_b1d1, cd_c1d1, dd1))
				CalculateCubeScope( new Point3D(pCenter.X,pCenter.Y,p1.Z), 
					new Point3D(p2.X,p2.Y,pCenter.Z),
					abcd, bd, cd, d, center, bd_b1d1, cd_c1d1, dd1);


			if (CheckScope( aa1, ab_a1b1, ac_a1c1, center, a1, a1b1, a1c1, a1b1c1d1))
				CalculateCubeScope( new Point3D (p1.X, p1.Y, pCenter.Z), 
					new Point3D (pCenter.X, pCenter.Y, p2.Z),
					aa1, ab_a1b1, ac_a1c1, center, a1, a1b1, a1c1, a1b1c1d1);

			if (CheckScope( ab_a1b1, bb1, center, bd_b1d1, a1b1, b1, a1b1c1d1, b1d1))
				CalculateCubeScope( new Point3D ( pCenter.X, p1.Y, pCenter.Z),
					new Point3D ( p2.X, pCenter.Y, p2.Z),
					ab_a1b1, bb1, center, bd_b1d1, a1b1, b1, a1b1c1d1, b1d1);

			if (CheckScope( ac_a1c1, center, cc1,cd_c1d1, a1c1, a1b1c1d1, c1, c1d1))
				CalculateCubeScope( new Point3D(p1.X,pCenter.Y,pCenter.Z), 
					new Point3D(pCenter.X,p2.Y,p2.Z),
					ac_a1c1, center, cc1,cd_c1d1, a1c1, a1b1c1d1, c1, c1d1);

			if (CheckScope( center, bd_b1d1, cd_c1d1, dd1, a1b1c1d1, b1d1, c1d1, d1))
				CalculateCubeScope( new Point3D(pCenter.X,pCenter.Y,pCenter.Z), 
					new Point3D(p2.X,p2.Y,p2.Z),
					center, bd_b1d1, cd_c1d1, dd1, a1b1c1d1, b1d1, c1d1, d1);
		}

		private bool CheckScope(double a, double b, double c, double d, double e, double f, double g, double h)
		{
			bool check = a > 0;
			return (check != (b > 0)) 
				|| (check != (c > 0)) 
				|| (check != (d > 0)) 
				|| (check != (e > 0)) 
				|| (check != (f > 0))
				|| (check != (g > 0)) 
				|| (check != (h > 0));
	
		}
	}
}
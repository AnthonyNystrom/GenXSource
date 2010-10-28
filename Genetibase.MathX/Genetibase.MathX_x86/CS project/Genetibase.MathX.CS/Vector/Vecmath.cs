
using System;
using System.Globalization;

namespace Genetibase.MathX
{

	/// <summary>
	/// Math.
	/// </summary>
	public struct NuGenVector
	{

		// float
		public static float TINY_FLOAT = 4.76837158203125E-7f;
		public static float HUGE_FLOAT = 2.147483648E+9f;

		// double
		public static double TINY_DOUBLE = 8.8817841970012523233891E-16;
		public static double HUGE_DOUBLE = 5.78960446186580977117855E+76;

		// NuGenPnt2F
		public static NuGenPnt2F HUGE_NuGenPnt2F = new NuGenPnt2F(HUGE_FLOAT, HUGE_FLOAT);

		// NuGenPnt2D
		public static NuGenPnt2D HUGE_NuGenPnt2D = new NuGenPnt2D(HUGE_DOUBLE, HUGE_DOUBLE);

		// NuGenPnt3F
		public static NuGenPnt3F HUGE_NuGenPnt3F = new NuGenPnt3F(HUGE_FLOAT, HUGE_FLOAT, HUGE_FLOAT);

		// NuGenPnt3D
		public static NuGenPnt3D HUGE_NuGenPnt3D = new NuGenPnt3D(HUGE_DOUBLE, HUGE_DOUBLE, HUGE_DOUBLE);

		//Approximately equals ...
		public static bool ApproxEquals(float a, float b)
		{
			return Math.Abs(a - b) < NuGenVector.TINY_FLOAT;
		}

		public static bool ApproxEquals(double a, double b)
		{
			return Math.Abs(a - b) < NuGenVector.TINY_DOUBLE;
		}

		public static bool ApproxEquals(NuGenVec2F a, NuGenVec2F b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_FLOAT;
		}

		public static bool ApproxEquals(NuGenVec2D a, NuGenVec2D b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_DOUBLE;
		}

		public static bool ApproxEquals(NuGenVec3F a, NuGenVec3F b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[2] - b._x[2]) < NuGenVector.TINY_FLOAT;
		}

		public static bool ApproxEquals(NuGenVec3D a, NuGenVec3D b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[2] - b._x[2]) < NuGenVector.TINY_DOUBLE;
		}

		public static bool ApproxEquals(NuGenPnt2F a, NuGenPnt2F b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_FLOAT;
		}

		public static bool ApproxEquals(NuGenPnt2D a, NuGenPnt2D b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_DOUBLE;
		}

		public static bool ApproxEquals(NuGenPnt3F a, NuGenPnt3F b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_FLOAT &&
				Math.Abs(a._x[2] - b._x[2]) < NuGenVector.TINY_FLOAT;
		}

		public static bool ApproxEquals(NuGenPnt3D a, NuGenPnt3D b)
		{
			return
				Math.Abs(a._x[0] - b._x[0]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[1] - b._x[1]) < NuGenVector.TINY_DOUBLE &&
				Math.Abs(a._x[2] - b._x[2]) < NuGenVector.TINY_DOUBLE;
		}

		public static bool ApproxEquals(NuGenTrafo2D a, NuGenTrafo2D b)
		{
						
			for (int i = 0; i < 9; i++)
			{

				if (Math.Abs(a._x[0] - b._x[0]) > NuGenVector.TINY_DOUBLE) return false;
			}
			return true;
		}

		public static bool ApproxEquals(NuGenTrafo2F a, NuGenTrafo2F b)
		{
						
			for (int i = 0; i < 9; i++)
			{

				if (Math.Abs(a._x[0] - b._x[0]) > NuGenVector.TINY_FLOAT) return false;
			}
			return true;
		}

		public static bool ApproxEquals(NuGenTrafo3D a, NuGenTrafo3D b)
		{
						
			for (int i = 0; i < 16; i++)
			{

				if (Math.Abs(a._x[0] - b._x[0]) > NuGenVector.TINY_DOUBLE) return false;
			}
			return true;
		}

		public static bool ApproxEquals(NuGenTrafo3F a, NuGenTrafo3F b)
		{
						
			for (int i = 0; i < 16; i++)
			{

				if (Math.Abs(a._x[0] - b._x[0]) > NuGenVector.TINY_FLOAT) return false;
			}
			return true;
		}

		// radians / degrees
		internal static double RAD_PER_DEG = Math.PI / 180.0;
		internal static double DEG_PER_RAD = 180.0 / Math.PI;

		public static double Rad(double degrees)
		
		{
			return degrees * RAD_PER_DEG; 
		}

		public static float Rad(float degrees)
		
		{
			return degrees * (float)RAD_PER_DEG; 
		}

		public static double Deg(double radians)
		
		{
			return radians * DEG_PER_RAD; 
		}

		public static float Deg(float radians)
		
		{
			return radians * (float)DEG_PER_RAD; 
		}

		// min/max
		public static double Min(double a, double b, double c)
		
		{
			return Math.Min(Math.Min(a, b), c); 
		}

		public static float Min(float a, float b, float c)
		
		{
			return Math.Min(Math.Min(a, b), c); 
		}

		public static long Min(long a, long b, long c)
		
		{
			return Math.Min(Math.Min(a, b), c); 
		}

		public static int Min(int a, int b, int c)
		
		{
			return Math.Min(Math.Min(a, b), c); 
		}

		public static double Max(double a, double b, double c)
		
		{
			return Math.Max(Math.Max(a, b), c); 
		}

		public static float Max(float a, float b, float c)
		
		{
			return Math.Max(Math.Max(a, b), c); 
		}

		public static long Max(long a, long b, long c)
		
		{
			return Math.Max(Math.Max(a, b), c); 
		}

		public static int Max(int a, int b, int c)
		
		{
			return Math.Max(Math.Max(a, b), c); 
		}

		// det
		public static double Det2x2(

			double x00, double x01,
			double x10, double x11
			)
		{
			return x00 * x11 - x10 * x01;
		}

		public static float Det2x2(
			float x00, float x01,
			float x10, float x11
			)
		{
			return x00 * x11 - x10 * x01;
		}

		public static double Det3x3(

			double x00, double x01, double x02,
			double x10, double x11, double x12,
			double x20, double x21, double x22
			)
		{
			return x00*x11*x22 + x01*x12*x20 + x02*x10*x21
				- x20*x11*x02 - x21*x12*x00 - x22*x10*x01;
		}

		public static float Det3x3(
			float x00, float x01, float x02,
			float x10, float x11, float x12,
			float x20, float x21, float x22
			)
		{
			return x00*x11*x22 + x01*x12*x20 + x02*x10*x21
				- x20*x11*x02 - x21*x12*x00 - x22*x10*x01;
		}

		public static double Det4x4(

			double x00, double x01, double x02, double x03,
			double x10, double x11, double x12, double x13,
			double x20, double x21, double x22, double x23,
			double x30, double x31, double x32, double x33
			)
		{
			return 
				x00 * Det3x3(x11,x12,x13, x21,x22,x23, x31,x32,x33) -
				x01 * Det3x3(x10,x12,x13, x20,x22,x23, x30,x32,x33) +
				x02 * Det3x3(x10,x11,x13, x20,x21,x23, x30,x31,x33) -
				x03 * Det3x3(x10,x11,x12, x20,x21,x22, x30,x31,x32);
		}

		public static double Det4x4(double [] x)
		{
			return 
				x[0] * Det3x3(x[5],x[6],x[7], x[9],x[10],x[11], x[13],x[14],x[15]) -
				x[1] * Det3x3(x[4],x[6],x[7], x[8],x[10],x[11], x[12],x[14],x[15]) +
				x[2] * Det3x3(x[4],x[5],x[7], x[8],x[9],x[11], x[12],x[13],x[15]) -
				x[3] * Det3x3(x[4],x[5],x[6], x[8],x[9],x[10], x[12],x[13],x[14]);
		}

		public static float Det4x4(
			float x00, float x01, float x02, float x03,
			float x10, float x11, float x12, float x13,
			float x20, float x21, float x22, float x23,
			float x30, float x31, float x32, float x33
			)
		{
			return 
				x00 * Det3x3(x11,x12,x13, x21,x22,x23, x31,x32,x33) -
				x01 * Det3x3(x10,x12,x13, x20,x22,x23, x30,x32,x33) +
				x02 * Det3x3(x10,x11,x13, x20,x21,x23, x30,x31,x33) -
				x03 * Det3x3(x10,x11,x12, x20,x21,x22, x30,x31,x32);
		}

		public static float Det4x4(float [] x)
		{
			return 
				x[0] * Det3x3(x[5],x[6],x[7], x[9],x[10],x[11], x[13],x[14],x[15]) -
				x[1] * Det3x3(x[4],x[6],x[7], x[8],x[10],x[11], x[12],x[14],x[15]) +
				x[2] * Det3x3(x[4],x[5],x[7], x[8],x[9],x[11], x[12],x[13],x[15]) -
				x[3] * Det3x3(x[4],x[5],x[6], x[8],x[9],x[10], x[12],x[13],x[14]);
		}

	}

}

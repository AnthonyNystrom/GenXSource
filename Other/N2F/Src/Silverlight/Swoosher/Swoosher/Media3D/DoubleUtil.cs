using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Next2Friends.Swoosher.Media3D
{
	internal static class DoubleUtil
	{
		// Fields
		internal const double DBL_EPSILON = 2.2204460492503131E-16;
		internal const float FLT_MIN = 1.175494E-38f;

		// Methods
/*		public static bool AreClose( double value1, double value2 )
		{
			if ( value1 == value2 )
			{
				return true;
			}
			double num = ( ( Math.Abs( value1 ) + Math.Abs( value2 ) ) + 10 ) * 2.2204460492503131E-16;
			double num2 = value1 - value2;
			if ( -num < num2 )
			{
				return ( num > num2 );
			}
			return false;
		}

		public static bool AreClose( Point point1, Point point2 )
		{
			if ( AreClose( point1.X, point2.X ) )
			{
				return AreClose( point1.Y, point2.Y );
			}
			return false;
		}

		public static bool AreClose( Rect rect1, Rect rect2 )
		{
			if ( rect1.IsEmpty )
			{
				return rect2.IsEmpty;
			}
			if ( ( !rect2.IsEmpty && AreClose( rect1.X, rect2.X ) ) && ( AreClose( rect1.Y, rect2.Y ) && AreClose( rect1.Height, rect2.Height ) ) )
			{
				return AreClose( rect1.Width, rect2.Width );
			}
			return false;
		}

		public static bool AreClose( Size size1, Size size2 )
		{
			if ( AreClose( size1.Width, size2.Width ) )
			{
				return AreClose( size1.Height, size2.Height );
			}
			return false;
		}

		public static bool AreClose( Vector vector1, Vector vector2 )
		{
			if ( AreClose( vector1.X, vector2.X ) )
			{
				return AreClose( vector1.Y, vector2.Y );
			}
			return false;
		}

		public static int DoubleToInt( double val )
		{
			if ( 0 >= val )
			{
				return (int)( val - 0.5 );
			}
			return (int)( val + 0.5 );
		}

		public static bool GreaterThan( double value1, double value2 )
		{
			if ( value1 > value2 )
			{
				return !AreClose( value1, value2 );
			}
			return false;
		}

		public static bool GreaterThanOrClose( double value1, double value2 )
		{
			if ( value1 <= value2 )
			{
				return AreClose( value1, value2 );
			}
			return true;
		}

		public static bool IsBetweenZeroAndOne( double val )
		{
			if ( GreaterThanOrClose( val, 0 ) )
			{
				return LessThanOrClose( val, 1 );
			}
			return false;
		}

		public static bool IsNaN( double value )
		{
			NanUnion union = new NanUnion
			{
				DoubleValue = value
			};
			ulong num = union.UintValue & 18442240474082181120L;
			ulong num2 = union.UintValue & ( (ulong)0xfffffffffffffL );
			if ( ( num != 0x7ff0000000000000L ) && ( num != 18442240474082181120L ) )
			{
				return false;
			}
			return ( num2 != 0L );
		}
*/
		public static bool IsOne( double value )
		{
			return ( Math.Abs( (double)( value - 1 ) ) < 2.2204460492503131E-15 );
		}

		public static bool IsZero( double value )
		{
			return ( Math.Abs( value ) < 2.2204460492503131E-15 );
		}
/*
		public static bool LessThan( double value1, double value2 )
		{
			if ( value1 < value2 )
			{
				return !AreClose( value1, value2 );
			}
			return false;
		}

		public static bool LessThanOrClose( double value1, double value2 )
		{
			if ( value1 >= value2 )
			{
				return AreClose( value1, value2 );
			}
			return true;
		}

		public static bool RectHasNaN( Rect r )
		{
			if ( ( !IsNaN( r.X ) && !IsNaN( r.Y ) ) && ( !IsNaN( r.Height ) && !IsNaN( r.Width ) ) )
			{
				return false;
			}
			return true;
		}

		// Nested Types
		[StructLayout( LayoutKind.Explicit )]
		private struct NanUnion
		{
			// Fields
			[FieldOffset( 0 )]
			internal double DoubleValue;
			[FieldOffset( 0 )]
			internal ulong UintValue;
		}*/
	}
}
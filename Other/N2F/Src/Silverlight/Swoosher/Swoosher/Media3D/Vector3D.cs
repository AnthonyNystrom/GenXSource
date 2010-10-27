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
	public struct Vector3D// : IFormattable
	{
		internal double _x;
		internal double _y;
		internal double _z;

		public Vector3D( double x, double y, double z )
		{
			this._x = x;
			this._y = y;
			this._z = z;
		}

		public double Length
		{
			get
			{
				return Math.Sqrt( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) );
			}
		}
		public double LengthSquared
		{
			get
			{
				return ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) );
			}
		}
		public void Normalize()
		{
			double num = Math.Abs( this._x );
			double num3 = Math.Abs( this._y );
			double num2 = Math.Abs( this._z );
			if ( num3 > num )
			{
				num = num3;
			}
			if ( num2 > num )
			{
				num = num2;
			}
			this._x /= num;
			this._y /= num;
			this._z /= num;
			double num4 = Math.Sqrt( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) );
			this = (Vector3D)( this / num4 );
		}

		public static double AngleBetween( Vector3D vector1, Vector3D vector2 )
		{
			double num;
			vector1.Normalize();
			vector2.Normalize();
			if ( DotProduct( vector1, vector2 ) < 0 )
			{
				Vector3D vectord2 = -vector1 - vector2;
				num = 3.1415926535897931 - ( 2 * Math.Asin( vectord2.Length / 2 ) );
			}
			else
			{
				Vector3D vectord = vector1 - vector2;
				num = 2 * Math.Asin( vectord.Length / 2 );
			}
			return M3DUtil.RadiansToDegrees( num );
		}

		public static Vector3D operator -( Vector3D vector )
		{
			return new Vector3D( -vector._x, -vector._y, -vector._z );
		}

		public void Negate()
		{
			this._x = -this._x;
			this._y = -this._y;
			this._z = -this._z;
		}

		public static Vector3D operator +( Vector3D vector1, Vector3D vector2 )
		{
			return new Vector3D( vector1._x + vector2._x, vector1._y + vector2._y, vector1._z + vector2._z );
		}

		public static Vector3D Add( Vector3D vector1, Vector3D vector2 )
		{
			return new Vector3D( vector1._x + vector2._x, vector1._y + vector2._y, vector1._z + vector2._z );
		}

		public static Vector3D operator -( Vector3D vector1, Vector3D vector2 )
		{
			return new Vector3D( vector1._x - vector2._x, vector1._y - vector2._y, vector1._z - vector2._z );
		}

		public static Vector3D Subtract( Vector3D vector1, Vector3D vector2 )
		{
			return new Vector3D( vector1._x - vector2._x, vector1._y - vector2._y, vector1._z - vector2._z );
		}

		public static Point3D operator +( Vector3D vector, Point3D point )
		{
			return new Point3D( vector._x + point._x, vector._y + point._y, vector._z + point._z );
		}

		public static Point3D Add( Vector3D vector, Point3D point )
		{
			return new Point3D( vector._x + point._x, vector._y + point._y, vector._z + point._z );
		}

		public static Point3D operator -( Vector3D vector, Point3D point )
		{
			return new Point3D( vector._x - point._x, vector._y - point._y, vector._z - point._z );
		}

		public static Point3D Subtract( Vector3D vector, Point3D point )
		{
			return new Point3D( vector._x - point._x, vector._y - point._y, vector._z - point._z );
		}

		public static Vector3D operator *( Vector3D vector, double scalar )
		{
			return new Vector3D( vector._x * scalar, vector._y * scalar, vector._z * scalar );
		}

		public static Vector3D Multiply( Vector3D vector, double scalar )
		{
			return new Vector3D( vector._x * scalar, vector._y * scalar, vector._z * scalar );
		}

		public static Vector3D operator *( double scalar, Vector3D vector )
		{
			return new Vector3D( vector._x * scalar, vector._y * scalar, vector._z * scalar );
		}

		public static Vector3D Multiply( double scalar, Vector3D vector )
		{
			return new Vector3D( vector._x * scalar, vector._y * scalar, vector._z * scalar );
		}

		public static Vector3D operator /( Vector3D vector, double scalar )
		{
			return (Vector3D)( vector * ( 1 / scalar ) );
		}

		public static Vector3D Divide( Vector3D vector, double scalar )
		{
			return (Vector3D)( vector * ( 1 / scalar ) );
		}

		public static Vector3D operator *( Vector3D vector, Matrix3D matrix )
		{
			return matrix.Transform( vector );
		}

		public static Vector3D Multiply( Vector3D vector, Matrix3D matrix )
		{
			return matrix.Transform( vector );
		}

		public static double DotProduct( Vector3D vector1, Vector3D vector2 )
		{
			return DotProduct( ref vector1, ref vector2 );
		}

		internal static double DotProduct( ref Vector3D vector1, ref Vector3D vector2 )
		{
			return ( ( ( vector1._x * vector2._x ) + ( vector1._y * vector2._y ) ) + ( vector1._z * vector2._z ) );
		}

		public static Vector3D CrossProduct( Vector3D vector1, Vector3D vector2 )
		{
			Vector3D vectord;
			CrossProduct( ref vector1, ref vector2, out vectord );
			return vectord;
		}

		internal static void CrossProduct( ref Vector3D vector1, ref Vector3D vector2, out Vector3D result )
		{
			result._x = ( vector1._y * vector2._z ) - ( vector1._z * vector2._y );
			result._y = ( vector1._z * vector2._x ) - ( vector1._x * vector2._z );
			result._z = ( vector1._x * vector2._y ) - ( vector1._y * vector2._x );
		}

		public static explicit operator Point3D( Vector3D vector )
		{
			return new Point3D( vector._x, vector._y, vector._z );
		}

		public static explicit operator Size3D( Vector3D vector )
		{
			return new Size3D( Math.Abs( vector._x ), Math.Abs( vector._y ), Math.Abs( vector._z ) );
		}

		public static bool operator ==( Vector3D vector1, Vector3D vector2 )
		{
			if ( ( vector1.X == vector2.X ) && ( vector1.Y == vector2.Y ) )
			{
				return ( vector1.Z == vector2.Z );
			}
			return false;
		}

		public static bool operator !=( Vector3D vector1, Vector3D vector2 )
		{
			return ( vector1.X != vector2.X || vector1.Y != vector2.Y || vector1.Z != vector2.Z );
		}

		public static bool Equals( Vector3D vector1, Vector3D vector2 )
		{
			if ( vector1.X.Equals( vector2.X ) && vector1.Y.Equals( vector2.Y ) )
			{
				return vector1.Z.Equals( vector2.Z );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Vector3D ) )
			{
				return false;
			}
			Vector3D vectord = (Vector3D)o;
			return Equals( this, vectord );
		}

		public bool Equals( Vector3D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			return ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() );
		}

/*		public static Vector3D Parse( string source )
		{
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			Vector3D vectord = new Vector3D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
			helper.LastTokenRequired();
			return vectord;
		}
*/
		public double X
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
			}
		}
		public double Y
		{
			get
			{
				return this._y;
			}
			set
			{
				this._y = value;
			}
		}
		public double Z
		{
			get
			{
				return this._z;
			}
			set
			{
				this._z = value;
			}
		}
/*		public override string ToString()
		{
			return this.ConvertToString( null, null );
		}

		public string ToString( IFormatProvider provider )
		{
			return this.ConvertToString( null, provider );
		}

		string IFormattable.ToString( string format, IFormatProvider provider )
		{
			return this.ConvertToString( format, provider );
		}

		internal string ConvertToString( string format, IFormatProvider provider )
		{
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator( provider );
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._z } );
		}*/
	}
}
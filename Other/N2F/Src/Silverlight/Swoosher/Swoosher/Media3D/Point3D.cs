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
	public struct Point3D// : IFormattable
	{
		internal double _x;
		internal double _y;
		internal double _z;
		public Point3D( double x, double y, double z )
		{
			this._x = x;
			this._y = y;
			this._z = z;
		}

		public void Offset( double offsetX, double offsetY, double offsetZ )
		{
			this._x += offsetX;
			this._y += offsetY;
			this._z += offsetZ;
		}

		public static Point3D operator +( Point3D point, Vector3D vector )
		{
			return new Point3D( point._x + vector._x, point._y + vector._y, point._z + vector._z );
		}

		public static Point3D Add( Point3D point, Vector3D vector )
		{
			return new Point3D( point._x + vector._x, point._y + vector._y, point._z + vector._z );
		}

		public static Point3D operator -( Point3D point, Vector3D vector )
		{
			return new Point3D( point._x - vector._x, point._y - vector._y, point._z - vector._z );
		}

		public static Point3D Subtract( Point3D point, Vector3D vector )
		{
			return new Point3D( point._x - vector._x, point._y - vector._y, point._z - vector._z );
		}

		public static Vector3D operator -( Point3D point1, Point3D point2 )
		{
			return new Vector3D( point1._x - point2._x, point1._y - point2._y, point1._z - point2._z );
		}

		public static Vector3D Subtract( Point3D point1, Point3D point2 )
		{
			Vector3D result = new Vector3D();
			Subtract( ref point1, ref point2, out result );
			return result;
		}

		internal static void Subtract( ref Point3D p1, ref Point3D p2, out Vector3D result )
		{
			result._x = p1._x - p2._x;
			result._y = p1._y - p2._y;
			result._z = p1._z - p2._z;
		}

		public static Point3D operator *( Point3D point, Matrix3D matrix )
		{
			return matrix.Transform( point );
		}

		public static Point3D Multiply( Point3D point, Matrix3D matrix )
		{
			return matrix.Transform( point );
		}

		public static explicit operator Vector3D( Point3D point )
		{
			return new Vector3D( point._x, point._y, point._z );
		}

		public static explicit operator Point4D( Point3D point )
		{
			return new Point4D( point._x, point._y, point._z, 1 );
		}

		public static bool operator ==( Point3D point1, Point3D point2 )
		{
			if ( ( point1.X == point2.X ) && ( point1.Y == point2.Y ) )
			{
				return ( point1.Z == point2.Z );
			}
			return false;
		}

		public static bool operator !=( Point3D point1, Point3D point2 )
		{
			return !Equals( point1, point2 );
		}

		public static bool Equals( Point3D point1, Point3D point2 )
		{
			if ( point1.X.Equals( point2.X ) && point1.Y.Equals( point2.Y ) )
			{
				return point1.Z.Equals( point2.Z );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Point3D ) )
			{
				return false;
			}
			Point3D pointd = (Point3D)o;
			return Equals( this, pointd );
		}

		public bool Equals( Point3D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			return ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() );
		}

/*		public static Point3D Parse( string source )
		{
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			Point3D pointd = new Point3D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
			helper.LastTokenRequired();
			return pointd;
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
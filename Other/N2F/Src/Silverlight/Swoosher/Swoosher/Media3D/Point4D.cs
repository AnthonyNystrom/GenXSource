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
	public struct Point4D// : IFormattable
	{
		internal double _x;
		internal double _y;
		internal double _z;
		internal double _w;
		public Point4D( double x, double y, double z, double w )
		{
			this._x = x;
			this._y = y;
			this._z = z;
			this._w = w;
		}

		public void Offset( double deltaX, double deltaY, double deltaZ, double deltaW )
		{
			this._x += deltaX;
			this._y += deltaY;
			this._z += deltaZ;
			this._w += deltaW;
		}

		public static Point4D operator +( Point4D point1, Point4D point2 )
		{
			return new Point4D( point1._x + point2._x, point1._y + point2._y, point1._z + point2._z, point1._w + point2._w );
		}

		public static Point4D Add( Point4D point1, Point4D point2 )
		{
			return new Point4D( point1._x + point2._x, point1._y + point2._y, point1._z + point2._z, point1._w + point2._w );
		}

		public static Point4D operator -( Point4D point1, Point4D point2 )
		{
			return new Point4D( point1._x - point2._x, point1._y - point2._y, point1._z - point2._z, point1._w - point2._w );
		}

		public static Point4D Subtract( Point4D point1, Point4D point2 )
		{
			return new Point4D( point1._x - point2._x, point1._y - point2._y, point1._z - point2._z, point1._w - point2._w );
		}

		public static Point4D operator *( Point4D point, Matrix3D matrix )
		{
			return matrix.Transform( point );
		}

		public static Point4D Multiply( Point4D point, Matrix3D matrix )
		{
			return matrix.Transform( point );
		}

		public static bool operator ==( Point4D point1, Point4D point2 )
		{
			if ( ( ( point1.X == point2.X ) && ( point1.Y == point2.Y ) ) && ( point1.Z == point2.Z ) )
			{
				return ( point1.W == point2.W );
			}
			return false;
		}

		public static bool operator !=( Point4D point1, Point4D point2 )
		{
			return !Equals( point1, point2 );
		}

		public static bool Equals( Point4D point1, Point4D point2 )
		{
			if ( ( point1.X.Equals( point2.X ) && point1.Y.Equals( point2.Y ) ) && point1.Z.Equals( point2.Z ) )
			{
				return point1.W.Equals( point2.W );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Point4D ) )
			{
				return false;
			}
			Point4D pointd = (Point4D)o;
			return Equals( this, pointd );
		}

		public bool Equals( Point4D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			return ( ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() ) ^ this.W.GetHashCode() );
		}

/*		public static Point4D Parse( string source )
		{
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			Point4D pointd = new Point4D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
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
		public double W
		{
			get
			{
				return this._w;
			}
			set
			{
				this._w = value;
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
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._z, this._w } );
		}*/
	}
}
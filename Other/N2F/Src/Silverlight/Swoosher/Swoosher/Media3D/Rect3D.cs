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
	public struct Rect3D// : IFormattable
	{
		internal static readonly Rect3D Infinite;
		private static readonly Rect3D s_empty;
		internal double _x;
		internal double _y;
		internal double _z;
		internal double _sizeX;
		internal double _sizeY;
		internal double _sizeZ;
		public Rect3D( Point3D location, Size3D size )
		{
			if ( size.IsEmpty )
			{
				this = s_empty;
			}
			else
			{
				this._x = location._x;
				this._y = location._y;
				this._z = location._z;
				this._sizeX = size._x;
				this._sizeY = size._y;
				this._sizeZ = size._z;
			}
		}

		public Rect3D( double x, double y, double z, double sizeX, double sizeY, double sizeZ )
		{
			if ( ( ( sizeX < 0 ) || ( sizeY < 0 ) ) || ( sizeZ < 0 ) )
			{
				throw new ArgumentException( "Dimension cannot be negative" );
			}
			this._x = x;
			this._y = y;
			this._z = z;
			this._sizeX = sizeX;
			this._sizeY = sizeY;
			this._sizeZ = sizeZ;
		}

		internal Rect3D( Point3D point1, Point3D point2 )
		{
			this._x = Math.Min( point1._x, point2._x );
			this._y = Math.Min( point1._y, point2._y );
			this._z = Math.Min( point1._z, point2._z );
			this._sizeX = Math.Max( point1._x, point2._x ) - this._x;
			this._sizeY = Math.Max( point1._y, point2._y ) - this._y;
			this._sizeZ = Math.Max( point1._z, point2._z ) - this._z;
		}

		internal Rect3D( Point3D point, Vector3D vector )
			: this( point, point + vector )
		{
		}

		public static Rect3D Empty
		{
			get
			{
				return s_empty;
			}
		}
		public bool IsEmpty
		{
			get
			{
				return ( this._sizeX < 0 );
			}
		}
		public Point3D Location
		{
			get
			{
				return new Point3D( this._x, this._y, this._z );
			}
			set
			{
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
				this._x = value._x;
				this._y = value._y;
				this._z = value._z;
			}
		}
		public Size3D Size
		{
			get
			{
				if ( this.IsEmpty )
				{
					return Size3D.Empty;
				}
				return new Size3D( this._sizeX, this._sizeY, this._sizeZ );
			}
			set
			{
				if ( value.IsEmpty )
				{
					this = s_empty;
				}
				else
				{
					if ( this.IsEmpty )
					{
						throw new InvalidOperationException( "Cannot modify empty rectangle" );
					}
					this._sizeX = value._x;
					this._sizeY = value._y;
					this._sizeZ = value._z;
				}
			}
		}
		public double SizeX
		{
			get
			{
				return this._sizeX;
			}
			set
			{
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
				}
				this._sizeX = value;
			}
		}
		public double SizeY
		{
			get
			{
				return this._sizeY;
			}
			set
			{
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
				}
				this._sizeY = value;
			}
		}
		public double SizeZ
		{
			get
			{
				return this._sizeZ;
			}
			set
			{
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
				}
				this._sizeZ = value;
			}
		}
		public double X
		{
			get
			{
				return this._x;
			}
			set
			{
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
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
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
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
				if ( this.IsEmpty )
				{
					throw new InvalidOperationException( "Cannot modify empty rectangle" );
				}
				this._z = value;
			}
		}
		public bool Contains( Point3D point )
		{
			return this.Contains( point._x, point._y, point._z );
		}

		public bool Contains( double x, double y, double z )
		{
			if ( this.IsEmpty )
			{
				return false;
			}
			return this.ContainsInternal( x, y, z );
		}

		public bool Contains( Rect3D rect )
		{
			if ( ( !this.IsEmpty && !rect.IsEmpty ) && ( ( ( ( this._x <= rect._x ) && ( this._y <= rect._y ) ) && ( ( this._z <= rect._z ) && ( ( this._x + this._sizeX ) >= ( rect._x + rect._sizeX ) ) ) ) && ( ( this._y + this._sizeY ) >= ( rect._y + rect._sizeY ) ) ) )
			{
				return ( ( this._z + this._sizeZ ) >= ( rect._z + rect._sizeZ ) );
			}
			return false;
		}

		public bool IntersectsWith( Rect3D rect )
		{
			if ( ( !this.IsEmpty && !rect.IsEmpty ) && ( ( ( ( rect._x <= ( this._x + this._sizeX ) ) && ( ( rect._x + rect._sizeX ) >= this._x ) ) && ( ( rect._y <= ( this._y + this._sizeY ) ) && ( ( rect._y + rect._sizeY ) >= this._y ) ) ) && ( rect._z <= ( this._z + this._sizeZ ) ) ) )
			{
				return ( ( rect._z + rect._sizeZ ) >= this._z );
			}
			return false;
		}

		public void Intersect( Rect3D rect )
		{
			if ( ( this.IsEmpty || rect.IsEmpty ) || !this.IntersectsWith( rect ) )
			{
				this = Empty;
			}
			else
			{
				double num3 = Math.Max( this._x, rect._x );
				double num2 = Math.Max( this._y, rect._y );
				double num = Math.Max( this._z, rect._z );
				this._sizeX = Math.Min( (double)( this._x + this._sizeX ), (double)( rect._x + rect._sizeX ) ) - num3;
				this._sizeY = Math.Min( (double)( this._y + this._sizeY ), (double)( rect._y + rect._sizeY ) ) - num2;
				this._sizeZ = Math.Min( (double)( this._z + this._sizeZ ), (double)( rect._z + rect._sizeZ ) ) - num;
				this._x = num3;
				this._y = num2;
				this._z = num;
			}
		}

		public static Rect3D Intersect( Rect3D rect1, Rect3D rect2 )
		{
			rect1.Intersect( rect2 );
			return rect1;
		}

		public void Union( Rect3D rect )
		{
			if ( this.IsEmpty )
			{
				this = rect;
			}
			else if ( !rect.IsEmpty )
			{
				double num3 = Math.Min( this._x, rect._x );
				double num2 = Math.Min( this._y, rect._y );
				double num = Math.Min( this._z, rect._z );
				this._sizeX = Math.Max( (double)( this._x + this._sizeX ), (double)( rect._x + rect._sizeX ) ) - num3;
				this._sizeY = Math.Max( (double)( this._y + this._sizeY ), (double)( rect._y + rect._sizeY ) ) - num2;
				this._sizeZ = Math.Max( (double)( this._z + this._sizeZ ), (double)( rect._z + rect._sizeZ ) ) - num;
				this._x = num3;
				this._y = num2;
				this._z = num;
			}
		}

		public static Rect3D Union( Rect3D rect1, Rect3D rect2 )
		{
			rect1.Union( rect2 );
			return rect1;
		}

		public void Union( Point3D point )
		{
			this.Union( new Rect3D( point, point ) );
		}

		public static Rect3D Union( Rect3D rect, Point3D point )
		{
			rect.Union( new Rect3D( point, point ) );
			return rect;
		}

		public void Offset( Vector3D offsetVector )
		{
			this.Offset( offsetVector._x, offsetVector._y, offsetVector._z );
		}

		public void Offset( double offsetX, double offsetY, double offsetZ )
		{
			if ( this.IsEmpty )
			{
				throw new InvalidOperationException( "Cannot call method" );
			}
			this._x += offsetX;
			this._y += offsetY;
			this._z += offsetZ;
		}

		public static Rect3D Offset( Rect3D rect, Vector3D offsetVector )
		{
			rect.Offset( offsetVector._x, offsetVector._y, offsetVector._z );
			return rect;
		}

		public static Rect3D Offset( Rect3D rect, double offsetX, double offsetY, double offsetZ )
		{
			rect.Offset( offsetX, offsetY, offsetZ );
			return rect;
		}

		private bool ContainsInternal( double x, double y, double z )
		{
			if ( ( ( ( x >= this._x ) && ( x <= ( this._x + this._sizeX ) ) ) && ( ( y >= this._y ) && ( y <= ( this._y + this._sizeY ) ) ) ) && ( z >= this._z ) )
			{
				return ( z <= ( this._z + this._sizeZ ) );
			}
			return false;
		}

		private static Rect3D CreateEmptyRect3D()
		{
			return new Rect3D { _x = double.PositiveInfinity, _y = double.PositiveInfinity, _z = double.PositiveInfinity, _sizeX = double.NegativeInfinity, _sizeY = double.NegativeInfinity, _sizeZ = double.NegativeInfinity };
		}

		private static Rect3D CreateInfiniteRect3D()
		{
			return new Rect3D { _x = -3.4028234663852886E+38, _y = -3.4028234663852886E+38, _z = -3.4028234663852886E+38, _sizeX = 6.8056469327705772E+38, _sizeY = 6.8056469327705772E+38, _sizeZ = 6.8056469327705772E+38 };
		}

		public static bool operator ==( Rect3D rect1, Rect3D rect2 )
		{
			if ( ( ( ( rect1.X == rect2.X ) && ( rect1.Y == rect2.Y ) ) && ( ( rect1.Z == rect2.Z ) && ( rect1.SizeX == rect2.SizeX ) ) ) && ( rect1.SizeY == rect2.SizeY ) )
			{
				return ( rect1.SizeZ == rect2.SizeZ );
			}
			return false;
		}

		public static bool operator !=( Rect3D rect1, Rect3D rect2 )
		{
			return !Equals( rect1, rect2 );
		}

		public static bool Equals( Rect3D rect1, Rect3D rect2 )
		{
			if ( rect1.IsEmpty )
			{
				return rect2.IsEmpty;
			}
			if ( ( ( rect1.X.Equals( rect2.X ) && rect1.Y.Equals( rect2.Y ) ) && ( rect1.Z.Equals( rect2.Z ) && rect1.SizeX.Equals( rect2.SizeX ) ) ) && rect1.SizeY.Equals( rect2.SizeY ) )
			{
				return rect1.SizeZ.Equals( rect2.SizeZ );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Rect3D ) )
			{
				return false;
			}
			Rect3D rectd = (Rect3D)o;
			return Equals( this, rectd );
		}

		public bool Equals( Rect3D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			if ( this.IsEmpty )
			{
				return 0;
			}
			return ( ( ( ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() ) ^ this.SizeX.GetHashCode() ) ^ this.SizeY.GetHashCode() ) ^ this.SizeZ.GetHashCode() );
		}

/*		public static Rect3D Parse( string source )
		{
			Rect3D empty;
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			if ( str == "Empty" )
			{
				empty = Empty;
			}
			else
			{
				empty = new Rect3D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
			}
			helper.LastTokenRequired();
			return empty;
		}

		public override string ToString()
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
			if ( this.IsEmpty )
			{
				return "Empty";
			}
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator( provider );
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._z, this._sizeX, this._sizeY, this._sizeZ } );
		}
*/
		static Rect3D()
		{
			Infinite = CreateInfiniteRect3D();
			s_empty = CreateEmptyRect3D();
		}
	}
}
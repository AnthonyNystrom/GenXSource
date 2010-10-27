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
	public struct Size3D// : IFormattable
	{
		private static readonly Size3D s_empty;
		internal double _x;
		internal double _y;
		internal double _z;
		public Size3D( double x, double y, double z )
		{
			if ( ( ( x < 0 ) || ( y < 0 ) ) || ( z < 0 ) )
			{
				throw new ArgumentException( "Dimension cannot be negative" );
			}
			this._x = x;
			this._y = y;
			this._z = z;
		}

		public static Size3D Empty
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
				return ( this._x < 0 );
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
					throw new InvalidOperationException( "Cannot modify empty size" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
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
					throw new InvalidOperationException( "Cannot modify empty size" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
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
					throw new InvalidOperationException( "Cannot modify empty size" );
				}
				if ( value < 0 )
				{
					throw new ArgumentException( "Dimension cannot be negative" );
				}
				this._z = value;
			}
		}
		public static explicit operator Vector3D( Size3D size )
		{
			return new Vector3D( size._x, size._y, size._z );
		}

		public static explicit operator Point3D( Size3D size )
		{
			return new Point3D( size._x, size._y, size._z );
		}

		private static Size3D CreateEmptySize3D()
		{
			return new Size3D { _x = double.NegativeInfinity, _y = double.NegativeInfinity, _z = double.NegativeInfinity };
		}

		public static bool operator ==( Size3D size1, Size3D size2 )
		{
			if ( ( size1.X == size2.X ) && ( size1.Y == size2.Y ) )
			{
				return ( size1.Z == size2.Z );
			}
			return false;
		}

		public static bool operator !=( Size3D size1, Size3D size2 )
		{
			return !Equals( size1, size2 );
		}

		public static bool Equals( Size3D size1, Size3D size2 )
		{
			if ( size1.IsEmpty )
			{
				return size2.IsEmpty;
			}
			if ( size1.X.Equals( size2.X ) && size1.Y.Equals( size2.Y ) )
			{
				return size1.Z.Equals( size2.Z );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Size3D ) )
			{
				return false;
			}
			Size3D sized = (Size3D)o;
			return Equals( this, sized );
		}

		public bool Equals( Size3D value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			if ( this.IsEmpty )
			{
				return 0;
			}
			return ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() );
		}

/*		public static Size3D Parse( string source )
		{
			Size3D empty;
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			if ( str == "Empty" )
			{
				empty = Empty;
			}
			else
			{
				empty = new Size3D( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
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
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._z } );
		}
*/
		static Size3D()
		{
			s_empty = CreateEmptySize3D();
		}
	}
}
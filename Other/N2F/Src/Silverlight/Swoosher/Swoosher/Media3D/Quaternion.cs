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
	public struct Quaternion// : IFormattable
	{
		internal double _x;
		internal double _y;
		internal double _z;
		internal double _w;
		private bool _isNotDistinguishedIdentity;
		private static int c_identityHashCode;
		private static Quaternion s_identity;
		public Quaternion( double x, double y, double z, double w )
		{
			this._x = x;
			this._y = y;
			this._z = z;
			this._w = w;
			this._isNotDistinguishedIdentity = true;
		}

		public Quaternion( Vector3D axisOfRotation, double angleInDegrees )
		{
			angleInDegrees = angleInDegrees % 360;
			double num2 = angleInDegrees * 0.017453292519943295;
			double length = axisOfRotation.Length;
			if ( length == 0 )
			{
				throw new InvalidOperationException( "Zero axis specified" );
			}
			Vector3D vectord = (Vector3D)( ( axisOfRotation / length ) * Math.Sin( 0.5 * num2 ) );
			this._x = vectord.X;
			this._y = vectord.Y;
			this._z = vectord.Z;
			this._w = Math.Cos( 0.5 * num2 );
			this._isNotDistinguishedIdentity = true;
		}

		public static Quaternion Identity
		{
			get
			{
				return s_identity;
			}
		}
		public Vector3D Axis
		{
			get
			{
				if ( this.IsDistinguishedIdentity || ( ( ( this._x == 0 ) && ( this._y == 0 ) ) && ( this._z == 0 ) ) )
				{
					return new Vector3D( 0, 1, 0 );
				}
				Vector3D vectord = new Vector3D( this._x, this._y, this._z );
				vectord.Normalize();
				return vectord;
			}
		}
		public double Angle
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 0;
				}
				double y = Math.Sqrt( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) );
				double x = this._w;
				if ( y > double.MaxValue )
				{
					double num = Math.Max( Math.Abs( this._x ), Math.Max( Math.Abs( this._y ), Math.Abs( this._z ) ) );
					double num5 = this._x / num;
					double num4 = this._y / num;
					double num3 = this._z / num;
					y = Math.Sqrt( ( ( num5 * num5 ) + ( num4 * num4 ) ) + ( num3 * num3 ) );
					x = this._w / num;
				}
				return ( Math.Atan2( y, x ) * 114.59155902616465 );
			}
		}
		public bool IsNormalized
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return true;
				}
				double num = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
				return DoubleUtil.IsOne( num );
			}
		}
		public bool IsIdentity
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return true;
				}
				if ( ( ( this._x == 0 ) && ( this._y == 0 ) ) && ( this._z == 0 ) )
				{
					return ( this._w == 1 );
				}
				return false;
			}
		}
		public void Conjugate()
		{
			if ( !this.IsDistinguishedIdentity )
			{
				this._x = -this._x;
				this._y = -this._y;
				this._z = -this._z;
			}
		}

		public void Invert()
		{
			if ( !this.IsDistinguishedIdentity )
			{
				this.Conjugate();
				double num = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
				this._x /= num;
				this._y /= num;
				this._z /= num;
				this._w /= num;
			}
		}

		public void Normalize()
		{
			if ( !this.IsDistinguishedIdentity )
			{
				double d = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
				if ( d > double.MaxValue )
				{
					double num2 = 1 / Max( Math.Abs( this._x ), Math.Abs( this._y ), Math.Abs( this._z ), Math.Abs( this._w ) );
					this._x *= num2;
					this._y *= num2;
					this._z *= num2;
					this._w *= num2;
					d = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
				}
				double num = 1 / Math.Sqrt( d );
				this._x *= num;
				this._y *= num;
				this._z *= num;
				this._w *= num;
			}
		}

		public static Quaternion operator +( Quaternion left, Quaternion right )
		{
			if ( right.IsDistinguishedIdentity )
			{
				if ( left.IsDistinguishedIdentity )
				{
					return new Quaternion( 0, 0, 0, 2 );
				}
				left._w += 1;
				return left;
			}
			if ( left.IsDistinguishedIdentity )
			{
				right._w += 1;
				return right;
			}
			return new Quaternion( left._x + right._x, left._y + right._y, left._z + right._z, left._w + right._w );
		}

		public static Quaternion Add( Quaternion left, Quaternion right )
		{
			return ( left + right );
		}

		public static Quaternion operator -( Quaternion left, Quaternion right )
		{
			if ( right.IsDistinguishedIdentity )
			{
				if ( left.IsDistinguishedIdentity )
				{
					return new Quaternion( 0, 0, 0, 0 );
				}
				left._w -= 1;
				return left;
			}
			if ( left.IsDistinguishedIdentity )
			{
				return new Quaternion( -right._x, -right._y, -right._z, 1 - right._w );
			}
			return new Quaternion( left._x - right._x, left._y - right._y, left._z - right._z, left._w - right._w );
		}

		public static Quaternion Subtract( Quaternion left, Quaternion right )
		{
			return ( left - right );
		}

		public static Quaternion operator *( Quaternion left, Quaternion right )
		{
			if ( left.IsDistinguishedIdentity )
			{
				return right;
			}
			if ( right.IsDistinguishedIdentity )
			{
				return left;
			}
			double x = ( ( ( left._w * right._x ) + ( left._x * right._w ) ) + ( left._y * right._z ) ) - ( left._z * right._y );
			double y = ( ( ( left._w * right._y ) + ( left._y * right._w ) ) + ( left._z * right._x ) ) - ( left._x * right._z );
			double z = ( ( ( left._w * right._z ) + ( left._z * right._w ) ) + ( left._x * right._y ) ) - ( left._y * right._x );
			return new Quaternion( x, y, z, ( ( ( left._w * right._w ) - ( left._x * right._x ) ) - ( left._y * right._y ) ) - ( left._z * right._z ) );
		}

		public static Quaternion Multiply( Quaternion left, Quaternion right )
		{
			return ( left * right );
		}

		private void Scale( double scale )
		{
			if ( this.IsDistinguishedIdentity )
			{
				this._w = scale;
				this.IsDistinguishedIdentity = false;
			}
			else
			{
				this._x *= scale;
				this._y *= scale;
				this._z *= scale;
				this._w *= scale;
			}
		}

		private double Length()
		{
			if ( this.IsDistinguishedIdentity )
			{
				return 1;
			}
			double d = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
			if ( d > double.MaxValue )
			{
				double num = Math.Max( Math.Max( Math.Abs( this._x ), Math.Abs( this._y ) ), Math.Max( Math.Abs( this._z ), Math.Abs( this._w ) ) );
				double num5 = this._x / num;
				double num4 = this._y / num;
				double num3 = this._z / num;
				double num2 = this._w / num;
				return ( Math.Sqrt( ( ( ( num5 * num5 ) + ( num4 * num4 ) ) + ( num3 * num3 ) ) + ( num2 * num2 ) ) * num );
			}
			return Math.Sqrt( d );
		}

		public static Quaternion Slerp( Quaternion from, Quaternion to, double t )
		{
			return Slerp( from, to, t, true );
		}

		public static Quaternion Slerp( Quaternion from, Quaternion to, double t, bool useShortestPath )
		{
			double num2;
			double num3;
			if ( from.IsDistinguishedIdentity )
			{
				from._w = 1;
			}
			if ( to.IsDistinguishedIdentity )
			{
				to._w = 1;
			}
			double num4 = from.Length();
			double num9 = to.Length();
			from.Scale( 1 / num4 );
			to.Scale( 1 / num9 );
			double d = ( ( ( from._x * to._x ) + ( from._y * to._y ) ) + ( from._z * to._z ) ) + ( from._w * to._w );
			if ( useShortestPath )
			{
				if ( d < 0 )
				{
					d = -d;
					to._x = -to._x;
					to._y = -to._y;
					to._z = -to._z;
					to._w = -to._w;
				}
			}
			else if ( d < -1 )
			{
				d = -1;
			}
			if ( d > 1 )
			{
				d = 1;
			}
			if ( d > 0.999999 )
			{
				num3 = 1 - t;
				num2 = t;
			}
			else if ( d < -0.9999999999 )
			{
				to = new Quaternion( -from.Y, from.X, -from.W, from.Z );
				double num8 = t * 3.1415926535897931;
				num3 = Math.Cos( num8 );
				num2 = Math.Sin( num8 );
			}
			else
			{
				double num7 = Math.Acos( d );
				double num6 = Math.Sqrt( 1 - ( d * d ) );
				num3 = Math.Sin( ( 1 - t ) * num7 ) / num6;
				num2 = Math.Sin( t * num7 ) / num6;
			}
			double num5 = num4 * Math.Pow( num9 / num4, t );
			num3 *= num5;
			num2 *= num5;
			return new Quaternion( ( num3 * from._x ) + ( num2 * to._x ), ( num3 * from._y ) + ( num2 * to._y ), ( num3 * from._z ) + ( num2 * to._z ), ( num3 * from._w ) + ( num2 * to._w ) );
		}

		private static double Max( double a, double b, double c, double d )
		{
			if ( b > a )
			{
				a = b;
			}
			if ( c > a )
			{
				a = c;
			}
			if ( d > a )
			{
				a = d;
			}
			return a;
		}

		public double X
		{
			get
			{
				return this._x;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
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
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
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
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._z = value;
			}
		}
		public double W
		{
			get
			{
				if ( this.IsDistinguishedIdentity )
				{
					return 1;
				}
				return this._w;
			}
			set
			{
				if ( this.IsDistinguishedIdentity )
				{
					this = s_identity;
					this.IsDistinguishedIdentity = false;
				}
				this._w = value;
			}
		}
		private bool IsDistinguishedIdentity
		{
			get
			{
				return !this._isNotDistinguishedIdentity;
			}
			set
			{
				this._isNotDistinguishedIdentity = !value;
			}
		}
		private static int GetIdentityHashCode()
		{
			double num2 = 0;
			double num = 1;
			return ( num2.GetHashCode() ^ num.GetHashCode() );
		}

		private static Quaternion GetIdentity()
		{
			return new Quaternion( 0, 0, 0, 1 ) { IsDistinguishedIdentity = true };
		}

		public static bool operator ==( Quaternion quaternion1, Quaternion quaternion2 )
		{
			if ( quaternion1.IsDistinguishedIdentity || quaternion2.IsDistinguishedIdentity )
			{
				return ( quaternion1.IsIdentity == quaternion2.IsIdentity );
			}
			if ( ( ( quaternion1.X == quaternion2.X ) && ( quaternion1.Y == quaternion2.Y ) ) && ( quaternion1.Z == quaternion2.Z ) )
			{
				return ( quaternion1.W == quaternion2.W );
			}
			return false;
		}

		public static bool operator !=( Quaternion quaternion1, Quaternion quaternion2 )
		{
			return !Equals( quaternion1, quaternion2 );
		}

		public static bool Equals( Quaternion quaternion1, Quaternion quaternion2 )
		{
			if ( quaternion1.IsDistinguishedIdentity || quaternion2.IsDistinguishedIdentity )
			{
				return ( quaternion1.IsIdentity == quaternion2.IsIdentity );
			}
			if ( ( quaternion1.X.Equals( quaternion2.X ) && quaternion1.Y.Equals( quaternion2.Y ) ) && quaternion1.Z.Equals( quaternion2.Z ) )
			{
				return quaternion1.W.Equals( quaternion2.W );
			}
			return false;
		}

		public override bool Equals( object o )
		{
			if ( ( o == null ) || !( o is Quaternion ) )
			{
				return false;
			}
			Quaternion quaternion = (Quaternion)o;
			return Equals( this, quaternion );
		}

		public bool Equals( Quaternion value )
		{
			return Equals( this, value );
		}

		public override int GetHashCode()
		{
			if ( this.IsDistinguishedIdentity )
			{
				return c_identityHashCode;
			}
			return ( ( ( this.X.GetHashCode() ^ this.Y.GetHashCode() ) ^ this.Z.GetHashCode() ) ^ this.W.GetHashCode() );
		}

/*		public static Quaternion Parse( string source )
		{
			Quaternion identity;
			IFormatProvider cultureInfo = CultureInfo.GetCultureInfo( "en-us" );
			TokenizerHelper helper = new TokenizerHelper( source, cultureInfo );
			string str = helper.NextTokenRequired();
			if ( str == "Identity" )
			{
				identity = Identity;
			}
			else
			{
				identity = new Quaternion( Convert.ToDouble( str, cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ), Convert.ToDouble( helper.NextTokenRequired(), cultureInfo ) );
			}
			helper.LastTokenRequired();
			return identity;
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
			if ( this.IsIdentity )
			{
				return "Identity";
			}
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator( provider );
			return string.Format( provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", new object[] { numericListSeparator, this._x, this._y, this._z, this._w } );
		}
*/
		static Quaternion()
		{
			c_identityHashCode = GetIdentityHashCode();
			s_identity = GetIdentity();
		}
	}
}
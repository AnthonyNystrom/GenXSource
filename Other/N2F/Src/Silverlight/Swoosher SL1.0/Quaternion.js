function Quaternion()
{
}

Quaternion.prototype =
{
	isDistinguishedIdentity: true,

	dup: function()
	{
		var q = new Quaternion();
		q.setElements( this.elements );
		return q;
	},

	// Maps the quaternion to another quaternion according to the given function
	map: function(fn) {
		var elements = [];
		this.each(function(x, i) {
			elements.push(fn(x, i));
		});
		return Quaternion.create(elements);
	},

	// Calls the iterator for each element of the quaternion in turn
	each: function(fn) {
		var n = this.elements.length, k = n, i;
		do { i = k - n;
			fn(this.elements[i], i+1);
		} while (--n);
	},

	setAxisAngle: function ( axisOfRotation, angleInDegrees )
	{
		angleInDegrees = angleInDegrees % 360;
		var num2 = angleInDegrees * 0.017453292519943295;
		var length = axisOfRotation.length();
		if ( length == 0 )
		{
			throw new Error( "Zero axis specified" );
		}
		var vectord = axisOfRotation.multiply( 1 / length ).multiply( Math.sin( 0.5 * num2 ) );

		this.setElements( [ vectord.elements[0], vectord.elements[1], vectord.elements[2], Math.cos( 0.5 * num2 ) ] );
		return this;
	},

	axis: function()
	{
		if ( this.isDistinguishedIdentity || ( ( ( this.elements[0] == 0 ) && ( this.elements[1] == 0 ) ) && ( this.elements[2] == 0 ) ) )
		{
			return $V( [ 0, 1, 0 ] );
		}
		return $V( [ this.elements[0], this.elements[1], this.elements[2] ] ).toUnitVector();
	},

	angle: function()
	{
		if ( this.isDistinguishedIdentity )
		{
			return 0;
		}
		var y = Math.sqrt( ( ( this.elements[0] * this.elements[0] ) + ( this.elements[1] * this.elements[1] ) ) + ( this.elements[2] * this.elements[2] ) );
		var x = this.elements[3];
		if ( y > 999999 )
		{
			var num = Math.max( Math.abs( this.elements[0] ), Math.max( Math.abs( this.elements[1] ), Math.abs( this.elements[2] ) ) );
			var num5 = this.elements[0] / num;
			var num4 = this.elements[1] / num;
			var num3 = this.elements[2] / num;
			y = Math.sqrt( ( ( num5 * num5 ) + ( num4 * num4 ) ) + ( num3 * num3 ) );
			x = this.elements[3] / num;
		}
		return ( Math.atan2( y, x ) * 114.59155902616465 );
	},
/*
	var IsNormalized = function IsNormalized()
	{
		if ( this.isDistinguishedIdentity )
		{
			return true;
		}
		var num = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
		return DoubleUtil.IsOne( num );
	}

	public bool IsIdentity
	{
		get
		{
			if ( this.isDistinguishedIdentity )
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
		if ( !this.isDistinguishedIdentity )
		{
			this._x = -this._x;
			this._y = -this._y;
			this._z = -this._z;
		}
	}

	public void Invert()
	{
		if ( !this.isDistinguishedIdentity )
		{
			this.Conjugate();
			double num = ( ( ( this._x * this._x ) + ( this._y * this._y ) ) + ( this._z * this._z ) ) + ( this._w * this._w );
			this._x /= num;
			this._y /= num;
			this._z /= num;
			this._w /= num;
		}
	}
*/
	normalize: function()
	{
		var q = this.dup();

		if ( !this.isDistinguishedIdentity )
		{
			var d = ( ( ( q.elements[0] * q.elements[0] ) + ( q.elements[1] * q.elements[1] ) ) + ( q.elements[2] * q.elements[2] ) ) + ( q.elements[3] * q.elements[3] );
			if ( d > 999999 )
			{
				var num2 = 1 / Max( Math.abs( q.elements[0] ), Math.abs( q.elements[1] ), Math.abs( q.elements[2] ), Math.abs( q.elements[3] ) );
				q.elements[0] *= num2;
				q.elements[1] *= num2;
				q.elements[2] *= num2;
				q.elements[3] *= num2;
				d = ( ( ( q.elements[0] * q.elements[0] ) + ( q.elements[1] * q.elements[1] ) ) + ( q.elements[2] * q.elements[2] ) ) + ( q.elements[3] * q.elements[3] );
			}
			var num = 1 / Math.sqrt( d );
			q.elements[0] *= num;
			q.elements[1] *= num;
			q.elements[2] *= num;
			q.elements[3] *= num;
		}

		return q;
	},
/*
	public static Quaternion operator +( Quaternion left, Quaternion right )
	{
		if ( right.isDistinguishedIdentity )
		{
			if ( left.isDistinguishedIdentity )
			{
				return new Quaternion( 0, 0, 0, 2 );
			}
			left._w += 1;
			return left;
		}
		if ( left.isDistinguishedIdentity )
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
		if ( right.isDistinguishedIdentity )
		{
			if ( left.isDistinguishedIdentity )
			{
				return new Quaternion( 0, 0, 0, 0 );
			}
			left._w -= 1;
			return left;
		}
		if ( left.isDistinguishedIdentity )
		{
			return new Quaternion( -right._x, -right._y, -right._z, 1 - right._w );
		}
		return new Quaternion( left._x - right._x, left._y - right._y, left._z - right._z, left._w - right._w );
	}

	public static Quaternion Subtract( Quaternion left, Quaternion right )
	{
		return ( left - right );
	}
*/

	multiply: function( q )
	{
		if ( this.isDistinguishedIdentity )
		{
			return q.dup();
		}
		if ( q.isDistinguishedIdentity )
		{
			return this.dup();
		}
		var result = new Quaternion();
		result.setElements( [
			( ( ( this.elements[3] * q.elements[0] ) + ( this.elements[0] * q.elements[3] ) ) + ( this.elements[1] * q.elements[2] ) ) - ( this.elements[2] * q.elements[1] ),
			( ( ( this.elements[3] * q.elements[1] ) + ( this.elements[1] * q.elements[3] ) ) + ( this.elements[2] * q.elements[0] ) ) - ( this.elements[0] * q.elements[2] ),
			( ( ( this.elements[3] * q.elements[2] ) + ( this.elements[2] * q.elements[3] ) ) + ( this.elements[0] * q.elements[1] ) ) - ( this.elements[1] * q.elements[0] ),
			( ( ( this.elements[3] * q.elements[3] ) - ( this.elements[0] * q.elements[0] ) ) - ( this.elements[1] * q.elements[1] ) ) - ( this.elements[2] * q.elements[2] ) ] );

		return result;
	},
/*
	public static Quaternion operator *( Quaternion left, Quaternion right )
	{
		if ( left.isDistinguishedIdentity )
		{
			return right;
		}
		if ( right.isDistinguishedIdentity )
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
*/
	scale: function( s )
	{
		if ( this.isDistinguishedIdentity )
		{
			this.elements[3] = s;
			this.isDistinguishedIdentity = false;
		}
		else
		{
			this.elements[0] *= s;
			this.elements[1] *= s;
			this.elements[2] *= s;
			this.elements[3] *= s;
		}
	},

	length: function()
	{
		if ( this.isDistinguishedIdentity )
		{
			return 1;
		}
		var d = ( ( ( this.elements[0] * this.elements[0] ) + ( this.elements[1] * this.elements[1] ) ) + ( this.elements[2] * this.elements[2] ) ) + ( this.elements[3] * this.elements[3] );
		if ( d > 999999 )
		{
			var num = Math.max( Math.max( Math.abs( this.elements[0] ), Math.abs( this.elements[1] ) ), Math.max( Math.abs( this.elements[2] ), Math.abs( this.elements[2] ) ) );
			var num5 = this.elements[0] / num;
			var num4 = this.elements[1] / num;
			var num3 = this.elements[2] / num;
			var num2 = this.elements[3] / num;
			return ( Math.sqrt( ( ( ( num5 * num5 ) + ( num4 * num4 ) ) + ( num3 * num3 ) ) + ( num2 * num2 ) ) * num );
		}
		return Math.sqrt( d );
	},
/*
	public static Quaternion Slerp( Quaternion from, Quaternion to, double t )
	{
		return Slerp( from, to, t, true );
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
			if ( this.isDistinguishedIdentity )
			{
				this = s_identity;
				this.isDistinguishedIdentity = false;
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
			if ( this.isDistinguishedIdentity )
			{
				this = s_identity;
				this.isDistinguishedIdentity = false;
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
			if ( this.isDistinguishedIdentity )
			{
				this = s_identity;
				this.isDistinguishedIdentity = false;
			}
			this._z = value;
		}
	}
	public double W
	{
		get
		{
			if ( this.isDistinguishedIdentity )
			{
				return 1;
			}
			return this._w;
		}
		set
		{
			if ( this.isDistinguishedIdentity )
			{
				this = s_identity;
				this.isDistinguishedIdentity = false;
			}
			this._w = value;
		}
	}
	private bool isDistinguishedIdentity
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
		return new Quaternion( 0, 0, 0, 1 ) { isDistinguishedIdentity = true };
	}

	public static bool operator ==( Quaternion quaternion1, Quaternion quaternion2 )
	{
		if ( quaternion1.isDistinguishedIdentity || quaternion2.isDistinguishedIdentity )
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
		return ( quaternion1 != quaternion2 );
	}

	public static bool Equals( Quaternion quaternion1, Quaternion quaternion2 )
	{
		if ( quaternion1.isDistinguishedIdentity || quaternion2.isDistinguishedIdentity )
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
		if ( this.isDistinguishedIdentity )
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
  // Set vector's elements from an array
  setElements: function(els) {
    this.elements = (els.elements || els).slice();
	this.isDistinguishedIdentity = false;
    return this;
  }
}

Quaternion.create = function( axis, angle )
{
	var q = new Quaternion();
	return q.setAxisAngle( axis, angle );
};

Quaternion.Slerp = function ( from, to, t, useShortestPath )
{
	from = from.dup();
	to = to.dup();

	var num2;
	var num3;
	if ( from.isDistinguishedIdentity )
	{
		from.elements[3] = 1;
	}
	if ( to.isDistinguishedIdentity )
	{
		to.elements[3] = 1;
	}
	var num4 = from.length();
	var num9 = to.length();
	from.scale( 1 / num4 );
	to.scale( 1 / num9 );
	var d = ( ( ( from.elements[0] * to.elements[0] ) + ( from.elements[1] * to.elements[1] ) ) + ( from.elements[2] * to.elements[2] ) ) + ( from.elements[3] * to.elements[3] );
	if ( useShortestPath )
	{
		if ( d < 0 )
		{
			d = -d;
			to.elements[0] = -to.elements[0];
			to.elements[1] = -to.elements[1];
			to.elements[2] = -to.elements[2];
			to.elements[3] = -to.elements[3];
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
		to = new Quaternion();
		to.setElements( [ -from.elements[1], from.elements[0], -from.elements[3], from.elements[2] ] );
		var num8 = t * 3.1415926535897931;
		num3 = Math.cos( num8 );
		num2 = Math.sin( num8 );
	}
	else
	{
		var num7 = Math.acos( d );
		var num6 = Math.sqrt( 1 - ( d * d ) );
		num3 = Math.sin( ( 1 - t ) * num7 ) / num6;
		num2 = Math.sin( t * num7 ) / num6;
	}
	var num5 = num4 * Math.pow( num9 / num4, t );
	num3 *= num5;
	num2 *= num5;
	var q = new Quaternion();
	q.setElements( [
		( num3 * from.elements[0] ) + ( num2 * to.elements[0] ),
		( num3 * from.elements[1] ) + ( num2 * to.elements[1] ),
		( num3 * from.elements[2] ) + ( num2 * to.elements[2] ),
		( num3 * from.elements[3] ) + ( num2 * to.elements[3] ) ] );
	return q;
};

Quaternion.DegreesToRadians = function ( degrees )
{
	return ( degrees / 57.295779513082323 );
};

Quaternion.RadiansToDegrees = function ( radians )
{
	return ( radians * 57.295779513082323 );
};

Vector.AngleBetween = function( vector1, vector2 )
{
	var num;
	vector1 = vector1.toUnitVector();
	vector2 = vector2.toUnitVector();
	if ( vector1.dot( vector2 ) < 0 )
	{
		var vectord2 = $V( [ 0, 0, 0 ] ).subtract( vector1 ).subtract( vector2 );
		num = 3.1415926535897931 - ( 2 * Math.asin( vectord2.modulus() / 2 ) );
	}
	else
	{
		var vectord = vector1.subtract( vector2 );
		num = 2 * Math.asin( vectord.modulus() / 2 );
	}
	return Quaternion.RadiansToDegrees( num );
}

Vector.prototype.length = function()
{
	return Math.sqrt( ( ( this.elements[0] * this.elements[0] ) + ( this.elements[1] * this.elements[1] ) ) + ( this.elements[2] * this.elements[2] ) );
}

var $Q = Quaternion.create;
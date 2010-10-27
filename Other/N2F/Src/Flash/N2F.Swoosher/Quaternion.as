package 
{
	public class Quaternion
	{
		public var elements : Array;
		public var isDistinguishedIdentity : Boolean = true;

		public function dup()
		{
			var q = new Quaternion();
			q.setElements( this.elements );
			return q;
		}

		// Maps the quaternion to another quaternion according to the given function
		public function map(fn)
		{
			var elements = [];
			this.each(function(x, i) {
			elements.push(fn(x, i));
			});
			return Quaternion.create(elements,undefined);
		}

		// Calls the iterator for each element of the quaternion in turn
		public function each(fn)
		{
			var n = this.elements.length, k = n, i;
			do
			{
				i = k - n;
				fn(this.elements[i], i+1);
			} while (--n);
		}

		public function setAxisAngle( axisOfRotation, angleInDegrees )
		{
			angleInDegrees = angleInDegrees % 360;
			var num2 = angleInDegrees * 0.017453292519943295;
			var length = axisOfRotation.length();
			if ( length == 0 )
			{
				throw new Error("Zero axis specified");
			}
			var vectord = axisOfRotation.multiply( 1 / length ).multiply( Math.sin( 0.5 * num2 ) );

			this.setElements( [ vectord.elements[0], vectord.elements[1], vectord.elements[2], Math.cos( 0.5 * num2 ) ] );
			return this;
		}

		public function axis()
		{
			if ( this.isDistinguishedIdentity || ( ( ( this.elements[0] == 0 ) && ( this.elements[1] == 0 ) ) && ( this.elements[2] == 0 ) ) )
			{
				return Vector.create([0,1,0]);
			}
			return Vector.create([this.elements[0],this.elements[1],this.elements[2]]).toUnitVector();
		}

		public function angle()
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
			return Math.atan2(y,x) * 114.59155902616465;
		}

		public function normalize()
		{
			var q = this.dup();

			if ( !this.isDistinguishedIdentity )
			{
				var d = ( ( ( q.elements[0] * q.elements[0] ) + ( q.elements[1] * q.elements[1] ) ) + ( q.elements[2] * q.elements[2] ) ) + ( q.elements[3] * q.elements[3] );
				if ( d > 999999 )
				{
					var num2 = 1 / Math.max( Math.abs( q.elements[0] ), Math.abs( q.elements[1] ), Math.abs( q.elements[2] ), Math.abs( q.elements[3] ) );
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
		}

		public function multiply( q )
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
		}

		public function scale( s )
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
		}

		public function length()
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
		}

		// Set vector's elements from an array
		public function setElements(els)
		{
			this.elements = (els.elements || els).slice();
			this.isDistinguishedIdentity = false;
			return this;
		}

		// Returns a string representation of the vector
		public function inspect()
		{
			return '[' + this.elements.join(', ') + ']';
		}

		public static function create( axis, angle )
		{
			var q = new Quaternion();
			return q.setAxisAngle(axis,angle);
		}

		public static function Slerp( from, to, t, useShortestPath )
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
		}

		public static function DegreesToRadians( degrees )
		{
			return (degrees / 57.295779513082323);
		}

		public static function RadiansToDegrees( radians )
		{
			return (radians * 57.295779513082323);
		}
	}
}
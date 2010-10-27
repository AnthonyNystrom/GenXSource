package
{
	public class Camera
	{
		public var position : Vector = Vector.create( [ 0, 0, -10 ] );
		public var lookDirection : Vector = Vector.create( [ 0, 0, 1 ] );
		public var upDirection : Vector = Vector.create( [ 0, 1, 0 ] );
		public var rotation : Quaternion = Quaternion.create( Vector.create( [ 0, 1, 0 ] ), 0 );

		public var aspectRatio : Number = 4 / 3;
		public var fov : Number = 60;
		public var near : Number = 1;
		public var far : Number = 1000;

		public function rotationMatrix()
		{
			return Matrix.Rotate( this.rotation, Vector.create( [ 0, 0, 0 ] ) );
		}

		public function viewMatrix()
		{
			var rot = this.rotationMatrix();

			return Camera.CreateViewMatrix(
				Camera.Transform( rot, this.position ),
				Camera.Transform( rot, this.lookDirection ),
				this.upDirection );
		}

		public function projectionMatrix()
		{
			var zn = this.near;
			var zf = this.far;

			var num2 = Math.tan( Camera.DegreesToRadians( this.fov ) / 2 );
			var num5 = this.aspectRatio / num2;
			var num4 = 1 / num2;
			var num = /*( zf != double.PositiveInfinity ) ?*/ ( zf / ( zn - zf ) ); //: -1;
			return Matrix.create( [ [ num4, 0, 0, 0 ], [ 0, num5, 0, 0 ], [ 0, 0, num, -1 ], [ 0, 0, zn * num, 0 ] ] );
		}

		public function displayMatrix( width, height )
		{
			return this.viewMatrix()
				.multiply( this.projectionMatrix() )
				.multiply( this.homogeneousToViewportTransform3D( width, height ) );
		}

		public function frustum()
		{
			return Frustum.create( this.viewMatrix().multiply( this.projectionMatrix() ) );
		}

		public function homogeneousToViewportTransform3D( width, height )
		{
			var halfWidth = width / 2;
			var halfHeight = height / 2;
			return Matrix.create( [ [ halfWidth, 0, 0, 0 ], [ 0, -halfHeight, 0, 0 ], [ 0, 0, 1, 0 ], [ halfWidth, halfHeight, 0, 1 ] ] );
		}

		public static function DegreesToRadians( degrees )
		{
			return ( degrees * 0.017453292519943295 );
		}

		public static function CreateViewMatrix( position, lookDirection, upDirection )
		{
			var vectord2 = lookDirection.multiply( -1 ).toUnitVector();
			var vectord3 = upDirection.cross( vectord2 ).toUnitVector();
			var vectord4 = vectord2.cross( vectord3 );
			var vectord = position;
			var offsetX = -vectord3.dot( vectord );
			var offsetY = -vectord4.dot( vectord );
			var offsetZ = -vectord2.dot( vectord );

			return Matrix.create( [
					   [ vectord3.elements[0], vectord4.elements[0], vectord2.elements[0], 0 ],
					   [ vectord3.elements[1], vectord4.elements[1], vectord2.elements[1], 0 ],
					   [ vectord3.elements[2], vectord4.elements[2], vectord2.elements[2], 0 ],
					   [ offsetX, offsetY, offsetZ, 1 ] ] );
		}

		public static function Transform( m, v )
		{
			var x = v.elements[0];
			var y = v.elements[1];
			var z = v.elements[2];
		
			var vec = Vector.create( [
				( ( ( x * m.elements[0][0] ) + ( y * m.elements[1][0] ) ) + ( z * m.elements[2][0] ) ) + m.elements[3][0],
				( ( ( x * m.elements[0][1] ) + ( y * m.elements[1][1] ) ) + ( z * m.elements[2][1] ) ) + m.elements[3][1],
				( ( ( x * m.elements[0][2] ) + ( y * m.elements[1][2] ) ) + ( z * m.elements[2][2] ) ) + m.elements[3][2] ] );
		
			if ( m.elements[0][3] == 0 && m.elements[1][3] == 0 && m.elements[2][3] == 0 && m.elements[3][3] == 1 )
				return vec;
		
			var num4 = ( ( ( x * m.elements[0][3] ) + ( y * m.elements[1][3] ) ) + ( z * m.elements[2][3] ) ) + m.elements[3][3];
			return vec.multiply( 1 / num4 );
		}
	}
}
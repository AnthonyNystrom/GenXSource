/// <reference path="intellisense.js" />
/// <reference path="Sylvester/sylvester.js" />

function Camera()
{
}

Camera.prototype =
{
	position: $V( [ 0, 0, -10 ] ),
	lookDirection: $V( [ 0, 0, 1 ] ),
	upDirection: $V( [ 0, 1, 0 ] ),
	rotation: $Q( $V( [ 0, 1, 0 ] ), 0 ),

	aspectRatio: 4 / 3,
	fov: 60,
	near: 1,
	far: 1000,

	rotationMatrix: function()
	{
		return Matrix.Rotate( this.rotation, $V( [ 0, 0, 0 ] ) );
/*		return Matrix.Rotation(
			Camera.DegreesToRadians( this.rotation.angle() ),
			this.rotation.axis() );*/
	},

	viewMatrix: function()
	{
		var rot = this.rotationMatrix();

		return Camera.CreateViewMatrix(
			Camera.Transform( rot, this.position ),
			Camera.Transform( rot, this.lookDirection ),
			this.upDirection );
	},

	projectionMatrix: function()
	{
		var zn = this.near;
		var zf = this.far;

		var num2 = Math.tan( Camera.DegreesToRadians( this.fov ) / 2 );
		var num5 = this.aspectRatio / num2;
		var num4 = 1 / num2;
		var num = /*( zf != double.PositiveInfinity ) ?*/ ( zf / ( zn - zf ) ); //: -1;
		return new $M( [ [ num4, 0, 0, 0 ], [ 0, num5, 0, 0 ], [ 0, 0, num, -1 ], [ 0, 0, zn * num, 0 ] ] );
	},

	displayMatrix: function( width, height )
	{
		return this.viewMatrix()
			.multiply( this.projectionMatrix() )
			.multiply( this.homogeneousToViewportTransform3D( width, height ) );
	},

	frustum: function()
	{
		return Frustum.create( this.viewMatrix().multiply( this.projectionMatrix() ) );
	},

	homogeneousToViewportTransform3D: function( width, height )
	{
		var halfWidth = width / 2;
		var halfHeight = height / 2;
		return new $M( [ [ halfWidth, 0, 0, 0 ], [ 0, -halfHeight, 0, 0 ], [ 0, 0, 1, 0 ], [ halfWidth, halfHeight, 0, 1 ] ] );
	}
}

Camera.DegreesToRadians = function( degrees )
{
	return ( degrees * 0.017453292519943295 );
}

Camera.CreateViewMatrix = function( position, lookDirection, upDirection )
{
	var vectord2 = lookDirection.multiply( -1 ).toUnitVector();
	var vectord3 = upDirection.cross( vectord2 ).toUnitVector();
	var vectord4 = vectord2.cross( vectord3 );
	var vectord = position;
	var offsetX = -vectord3.dot( vectord );
	var offsetY = -vectord4.dot( vectord );
	var offsetZ = -vectord2.dot( vectord );

	return new $M( [
			   [ vectord3.elements[0], vectord4.elements[0], vectord2.elements[0], 0 ],
			   [ vectord3.elements[1], vectord4.elements[1], vectord2.elements[1], 0 ],
			   [ vectord3.elements[2], vectord4.elements[2], vectord2.elements[2], 0 ],
			   [ offsetX, offsetY, offsetZ, 1 ] ] );
}

Camera.Transform = function( m, v )
{
	var x = v.elements[0];
	var y = v.elements[1];
	var z = v.elements[2];

	var vec = $V( [
		( ( ( x * m.elements[0][0] ) + ( y * m.elements[1][0] ) ) + ( z * m.elements[2][0] ) ) + m.elements[3][0],
		( ( ( x * m.elements[0][1] ) + ( y * m.elements[1][1] ) ) + ( z * m.elements[2][1] ) ) + m.elements[3][1],
		( ( ( x * m.elements[0][2] ) + ( y * m.elements[1][2] ) ) + ( z * m.elements[2][2] ) ) + m.elements[3][2] ] );

	if ( m.elements[0][3] == 0 && m.elements[1][3] == 0 && m.elements[2][3] == 0 && m.elements[3][3] == 1 )
		return vec;

	var num4 = ( ( ( x * m.elements[0][3] ) + ( y * m.elements[1][3] ) ) + ( z * m.elements[2][3] ) ) + m.elements[3][3];
	return vec.multiply( 1 / num4 );
};

Matrix.Rotate = function( quaternion, center )
{
	var matrixd = Matrix.Diagonal( [ 1, 1, 1, 1 ] );
//	matrixd.IsDistinguishedIdentity = false;
	var num12 = quaternion.elements[0] + quaternion.elements[0];
	var num2 = quaternion.elements[1] + quaternion.elements[1];
	var num = quaternion.elements[2] + quaternion.elements[2];
	var num11 = quaternion.elements[0] * num12;
	var num10 = quaternion.elements[0] * num2;
	var num9 = quaternion.elements[0] * num;
	var num8 = quaternion.elements[1] * num2;
	var num7 = quaternion.elements[1] * num;
	var num6 = quaternion.elements[2] * num;
	var num5 = quaternion.elements[3] * num12;
	var num4 = quaternion.elements[3] * num2;
	var num3 = quaternion.elements[3] * num;
	matrixd.elements[0][0] = 1 - ( num8 + num6 );
	matrixd.elements[0][1] = num10 + num3;
	matrixd.elements[0][2] = num9 - num4;
	matrixd.elements[1][0] = num10 - num3;
	matrixd.elements[1][1] = 1 - ( num11 + num6 );
	matrixd.elements[1][2] = num7 + num5;
	matrixd.elements[2][0] = num9 + num4;
	matrixd.elements[2][1] = num7 - num5;
	matrixd.elements[2][2] = 1 - ( num11 + num8 );
	if ( ( ( center.elements[0] != 0 ) || ( center.elements[1] != 0 ) ) || ( center.elements[2] != 0 ) )
	{
		matrixd._offsetX = ( ( ( -center.elements[0] * matrixd.elements[0][0] ) - ( center.elements[1] * matrixd.elements[1][0] ) ) - ( center.elements[2] * matrixd.elements[2][0] ) ) + center.elements[0];
		matrixd._offsetY = ( ( ( -center.elements[0] * matrixd.elements[0][1] ) - ( center.elements[1] * matrixd.elements[1][1] ) ) - ( center.elements[2] * matrixd.elements[2][1] ) ) + center.elements[1];
		matrixd._offsetZ = ( ( ( -center.elements[0] * matrixd.elements[0][2] ) - ( center.elements[1] * matrixd.elements[1][2] ) ) - ( center.elements[2] * matrixd.elements[2][2] ) ) + center.elements[2];
	}
	return matrixd;
};
function Frustum()
{
}

Frustum.prototype =
{
	planes: [],

	setPlanes: function( m )
	{
		this.planes = [];

		// Left clipping plane
		this.addPlane( $V( [ m.e( 1, 4 ) + m.e( 1, 1 ), m.e( 2, 4 ) + m.e( 2, 1 ), m.e( 3, 4 ) + m.e( 3, 1 ) ] ), m.e( 4, 4 ) + m.e( 4, 1 ) );

		// Right clipping plane
		this.addPlane( $V( [ m.e( 1, 4 ) - m.e( 1, 1 ), m.e( 2, 4 ) - m.e( 2, 1 ), m.e( 3, 4 ) - m.e( 3, 1 ) ] ), m.e( 4, 4 ) - m.e( 4, 1 ) );

		// Top clipping plane
		this.addPlane( $V( [ m.e( 1, 4 ) - m.e( 1, 2 ), m.e( 2, 4 ) - m.e( 2, 2 ), m.e( 3, 4 ) - m.e( 3, 2 ) ] ), m.e( 4, 4 ) - m.e( 4, 2 ) );

		// Bottom clipping plane
		this.addPlane( $V( [ m.e( 1, 4 ) + m.e( 1, 2 ), m.e( 2, 4 ) + m.e( 2, 2 ), m.e( 3, 4 ) + m.e( 3, 2 ) ] ), m.e( 4, 4 ) + m.e( 4, 2 ) );

		// Near clipping plane
		this.addPlane( $V( [ m.e( 1, 3 ), m.e( 2, 3 ), m.e( 3, 3 ) ] ), m.e( 4, 3 ) );

		// Far clipping plane
		this.addPlane( $V( [ m.e( 1, 4 ) - m.e( 1, 3 ), m.e( 2, 4 ) - m.e( 2, 3 ), m.e( 3, 4 ) - m.e( 3, 3 ) ] ), m.e( 4, 4 ) - m.e( 4, 3 ) );
	},

	addPlane: function( n, d )
	{
		this.planes.push( CPlane.create( n, d ) );
	},

	distance: function( point )
	{
		var maximum = 0;

		for ( var index = 0; index < this.planes.length; ++ index )
		{
			var plane = this.planes[ index ];
			var distance = plane.distance( point );
			if ( distance < 0 ) maximum = Math.max( maximum, -distance );
		}

		return maximum;
	},

	contains: function( point )
	{
		for ( var index = 0; index < this.planes.length; ++ index )
		{
			var plane = this.planes[ index ];
			if ( plane.distance( point ) < 0.00001 ) return false;
		}

		return true;
	}
};

Frustum.create = function( m )
{
	var f = new Frustum();
	f.setPlanes( m );
	return f;
};
function BSPTree( parent, items, partitionHandler )
{
	this.initialize( parent, items, partitionHandler );
}

BSPTree.prototype =
{
	parent: null,
	partition: null,
	items: null,
	front: null,
	back: null,
	bounds: null,

	initialize: function( parent, items, partitionHandler )
	{
		this.items = [];
		this.parent = parent;
		this.bounds = BoundingBox.create( items );
		this.partition = BSPTree.DeterminePartition( items, this.bounds, partitionHandler );

		if ( items.length == 1 )
		{
			this.items.push( items[ 0 ] );
		}
		else
		{
			var backItems = [];
			var frontItems = [];

			for ( var index in items )
			{
				var item = items[ index ];
				var distance = item.intersects( this.partition );

				if ( distance < 0 ) backItems.push( item );
				else if ( distance > 0 ) frontItems.push( item );
				else backItems.push( item );
			}

			if ( backItems.length > 0 ) this.back = new BSPTree( this, backItems, partitionHandler );
			if ( frontItems.length > 0 ) this.front = new BSPTree( this, frontItems, partitionHandler );
		}
	},

	select: function( viewpoint )
	{
		var ordered = [];
		this.selectItems( viewpoint, ordered );
		return ordered;
	},

	selectItems: function( viewpoint, ordered )
	{
		var distance = this.partition.distance( viewpoint );
//		var distance = this.partition.distanceFromRel( viewpoint );

		if ( distance < 0 )
		{
			if ( this.front != null ) this.front.selectItems( viewpoint, ordered );
		}
		else if ( this.back != null ) this.back.selectItems( viewpoint, ordered );

		for ( var index in this.items ) ordered.push( this.items[ index ] );

		if ( distance < 0 )
		{
			if ( this.back != null ) this.back.selectItems( viewpoint, ordered );
		}
		else if ( this.front != null ) this.front.selectItems( viewpoint, ordered );
	}
};

BSPTree.DeterminePartition = function( items, bounds, partitionHandler )
{
	var size = bounds.max.subtract( bounds.min );
	var length;
	var normal;

	if ( size.elements[2] >= size.elements[0] && size.elements[2] >= size.elements[1] )
	{
		length = size.elements[2];
		normal = $V( [ 0, 0, 1 ] );
	}
	else if ( size.elements[0] >= size.elements[1] && size.elements[1] >= size.elements[2] )
	{
		length = size.elements[0];
		normal = $V( [ 1, 0, 0 ] );
	}
	else
	{
		length = size.elements[1];
		normal = $V( [ 0, 1, 0 ] );
	}

	var position = ( partitionHandler != null ) ? partitionHandler( length, normal ) : 0.5;

	return CPlane.createAnchor( bounds.min.add( size.multiply( position ) ), normal );
//	return $P( bounds.min.add( size.multiply( position ) ), normal );
};
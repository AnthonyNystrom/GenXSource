function BoundingBox()
{
}

BoundingBox.prototype =
{
	initialized: false,
	min: null,
	max: null,

	merge: function( positions )
	{
        for ( var index in positions )
        {
			var p = positions[ index ];

            if ( !this.initialized )
            {
                this.min = p.dup();
                this.max = p.dup();
                this.initialized = true;
            }
            else
            {
                if ( p.elements[0] < this.min.elements[0] ) this.min.elements[0] = p.elements[0];
                if ( p.elements[1] < this.min.elements[1] ) this.min.elements[1] = p.elements[1];
                if ( p.elements[2] < this.min.elements[2] ) this.min.elements[2] = p.elements[2];
                if ( p.elements[0] > this.max.elements[0] ) this.max.elements[0] = p.elements[0];
                if ( p.elements[1] > this.max.elements[1] ) this.max.elements[1] = p.elements[1];
                if ( p.elements[2] > this.max.elements[2] ) this.max.elements[2] = p.elements[2];
            }
        }
    }
};

BoundingBox.create = function( items )
{
	var bounds = new BoundingBox();

	for ( var index in items )
	{
		var item = items[ index ];
		bounds.merge( item.bounds() );
	}

	return bounds;
};
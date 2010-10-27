package
{
	public class BSPTree
	{
		var parent : BSPTree;
		var partition : CPlane;
		var partitionHandler : Function;
		var items : Array;
		var front : BSPTree;
		var back : BSPTree;
		var bounds : BoundingBox;
		var maxDepth : int;

		public function BSPTree( parent : BSPTree, items : *, maxDepth : int, partitionHandler : Function )
		{
			this.items = [];
			this.parent = parent;
			this.maxDepth = maxDepth;
			this.partitionHandler = partitionHandler;
			this.bounds = BoundingBox.Create( items );
			this.partition = BSPTree.DeterminePartition( items, this.bounds, partitionHandler );

			Add( items );
		}

		public function Add( items : * ) : void
		{
			var backItems = [];
			var frontItems = [];

			for ( var index in items )
			{
				var item = items[ index ];
				var distance = item.intersects( this.partition );

				if ( maxDepth > 0 )
				{
					if ( distance <= 0 ) backItems.push( item );
					else if ( distance > 0 ) frontItems.push( item );
				}
				else
				{
					this.items.push( item );
				}
			}

			if ( backItems.length > 0 )
			{
				if ( this.back ) this.back.Add( backItems );
				else this.back = new BSPTree( this, backItems, maxDepth - 1, partitionHandler );
			}

			if ( frontItems.length > 0 )
			{
				if ( this.front ) this.front.Add( frontItems );
				else this.front = new BSPTree( this, frontItems, maxDepth - 1, partitionHandler );
			}
		}

		public function SelectItems( viewpoint : * ) : *
		{
			var ordered = [];
			Select( viewpoint, ordered );
			return ordered;
		}

		public function Select( viewpoint : * , ordered : * ) : void
		{
			var distance = this.partition.distance( viewpoint );
	//		var distance = this.partition.distanceFromRel( viewpoint );
	
			if ( distance < 0 )
			{
				if ( this.front != null ) this.front.Select( viewpoint, ordered );
			}
			else if ( this.back != null ) this.back.Select( viewpoint, ordered );
	
			for ( var index in this.items ) ordered.push( this.items[ index ] );
	
			if ( distance < 0 )
			{
				if ( this.back != null ) this.back.Select( viewpoint, ordered );
			}
			else if ( this.front != null ) this.front.Select( viewpoint, ordered );
		}

		public static function DeterminePartition( items : *, bounds : *, partitionHandler : Function ) : *
		{
			var size = bounds.max.subtract( bounds.min );

			var length;
			var normal;

			if ( size.elements[2] >= size.elements[0] && size.elements[2] >= size.elements[1] )
			{
				length = size.elements[2];
				normal = Vector.create( [ 0, 0, 1 ] );
			}
			else if ( size.elements[0] >= size.elements[1] && size.elements[1] >= size.elements[2] )
			{
				length = size.elements[0];
				normal = Vector.create( [ 1, 0, 0 ] );
			}
			else
			{
				length = size.elements[1];
				normal = Vector.create( [ 0, 1, 0 ] );
			}

			var position = ( partitionHandler != null ) ? partitionHandler( length, normal ) : 0.5;

			return CPlane.createAnchor( bounds.min.add( size.multiply( position ) ), normal );
		}
	}
}
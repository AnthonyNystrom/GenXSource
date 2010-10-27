/// <reference path="intellisense.js" />
/// <reference path="Sylvester/sylvester.js" />
package
{
	import flash.display.*;
	import flash.events.*;
	import flash.net.*;

	public class BSPImage
	{
		private static var nextUniqueId : int = 0;

		public function BSPImage( position, url )
		{
			this.uniqueId = nextUniqueId ++;
			this.position = position;
		}

		public var position = Vector.create( [ 0, 0, 0 ] );
		public var image : SelectableImage = null;
		public var loaded : Boolean = false;
		public var itemClick : Function = null;
		public var itemLoaded : Function = null;
		public var itemFailed : Function = null;
		public var visited : Boolean = false;
		public var selected : Boolean = false;
		public var viewDistance : Number = 1.5;
		public var depth : int = -1;
		public var uniqueId : int;

		public function get url() : String
		{
			return image ? image.url : null;
		}

		public function set url( url : String ) : void
		{
			if ( image )
			{
				image.parent.removeChild( image );
				image.removeEventListener( MouseEvent.CLICK, onClick );
				image = null;
			}

			if ( url )
			{
				image = new SelectableImage( url, this.onLoaded );
				image.addEventListener( MouseEvent.CLICK, onClick );
			}
		}

		public function intersects( plane )
		{
			return plane.distance( this.position );
		}

		public function bounds()
		{
			var size = Vector.create( [ 1, 1, 1 ] );

			return {
				min: this.position.subtract( size ),
				max: this.position.add( size )
			};
		}

		public function onClick( e )
		{
			if ( this.itemClick != null ) this.itemClick( this, e );
		}

		public function onLoaded( sender, e )
		{
			if ( this.itemLoaded != null ) this.itemLoaded( this, e );
		}

		public function onFailed( sender, e )
		{
			if ( this.itemFailed != null ) this.itemFailed( this, e );
		}

		public function refreshState()
		{
			this.image.SetBaseAlpha( this.visited && !this.selected ? 0.5 : 1 );
//			this.image.SetDimmed( this.visited && !this.selected );
		}
	}
}
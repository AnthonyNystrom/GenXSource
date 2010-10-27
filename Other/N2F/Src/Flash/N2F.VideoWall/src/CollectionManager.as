package  
{
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.external.ExternalInterface;
	import ContentSource;
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class CollectionManager extends EventDispatcher
	{
		public static var CHANGE : String = "CHANGE";

		private var items : Array = [];
		private var itemOrder : Array = [];

		public function CollectionManager( initMethod : String, selectMethod : String )
		{
			if ( ExternalInterface.available )
			{
				ExternalInterface.addCallback( "insert", Insert );
				ExternalInterface.addCallback( "add", Add );
				ExternalInterface.addCallback( "remove", Remove );

				//if ( root.loaderInfo.parameters.fadeSize ) fadeSize = root.loaderInfo.parameters.fadeSize;

				if ( initMethod )
				{
					var initData : * = ExternalInterface.call( initMethod, "setData" );
					if ( initData.length && initData.length > 0 ) SetData( initData );
				}
			}
			else
			{
				SetData( DummyData() );
			}
		}

		public function SetData( thumbnails : * ):void
		{
			if ( thumbnails )
			{
				for (var index : * in thumbnails)
				{
					Add( thumbnails[ index ] );
				}
			}
		}

		public function Insert( item : *, index : int = 0 ) : *
		{
			try
			{
				if ( item.uniqueId && items[ item.uniqueId ] ) Remove( item.uniqueId );

				var source : ContentSource = CreateItem( item );
				itemOrder.splice( 0, 0, source.Id );
				items[ source.Id ] = source;

				OnChanged();

				return source.Id;
			}
			catch ( e : Error )
			{
				if ( ExternalInterface.available ) ExternalInterface.call( "alert", e.message );
			}

			return null;
		}

		public function Add( item : * ) : *
		{
			try
			{
				if ( item.uniqueId && items[ item.uniqueId ] ) Remove( item.uniqueId );

				var source : ContentSource = CreateItem( item );
				itemOrder.push( source.Id );
				items[ source.Id ] = source;

				OnChanged();

				return source.Id;
			}
			catch ( e : Error )
			{
				trace( e );
				if ( ExternalInterface.available ) ExternalInterface.call( "alert", e.message );
			}

			return null;
		}

		private function CreateItem( item : * ) : ContentSource
		{
			return new ContentSource( item );
		}

		public function Remove( uniqueId : * ) : void
		{
			try
			{
				itemOrder.splice( itemOrder.indexOf( uniqueId ), 1 );
				delete items[ uniqueId ];

				OnChanged();
			}
			catch ( e : Error ) {}
		}

		public function get Items() : Array
		{
			var copy : Array = [];

			for ( var i : * in itemOrder.slice() )
			{
				copy.push( items[ itemOrder[ i ] ] );
			}

			return copy;
		}

		private function DummyData() : Array
		{
			var items : Array = [];

			for ( var index : int = 0; index < 7; ++ index )
			{
				items.push( {
					thumbnailUrl: "http://farm2.static.flickr.com/1429/1430528819_edb63b79a6_m.jpg",
					url: "http://www.next2friends.com/user/BobStrogg/video/MjEzNmEzZmY1MmIxNDhkNz.flv",
					title: "Anthony:US",
					isLive: false } );

				items.push( {
					thumbnailUrl: "http://www.next2friends.com/user//Olibs/vthmb/ZDkxNzBkZjIwNjU0NDkxZD.jpg",
					url: "http://www.next2friends.com/user//Olibs/video/ZDkxNzBkZjIwNjU0NDkxZD.flv",
					title: "lawrence:GB",
					isLive: true} );
			}

			return items;
		}

		private function OnChanged() : void
		{
			dispatchEvent( new Event( CHANGE ) );
		}
	}
}
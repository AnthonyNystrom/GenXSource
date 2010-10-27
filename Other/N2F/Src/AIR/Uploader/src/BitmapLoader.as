package
{
	import flash.display.Bitmap;
	import flash.display.LoaderInfo;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.net.URLRequest;
	import flash.net.URLStream;
	import flash.utils.ByteArray;
	import flash.utils.Dictionary;
	
	import mx.controls.Alert;
	import mx.core.FlexLoader;
	import mx.core.UIComponent;
 
 	[Event( name="progress", type="flash.events.ProgressEvent" )]
	[Event( name="complete", type="flash.events.Event" )]
	public class BitmapLoader extends UIComponent
	{
		[Bindable] public var thumbnail : Boolean = false;
		[Bindable] public var maxImageWidth : int = 240;
		[Bindable] public var maxImageHeight : int = 160;
		[Bindable] public var bitmap : Bitmap = null;
		[Bindable] public var error : String = null;

		private var request : URLRequest = null;
		private var bufferedStream : BufferedStream = null;
		private var tryParseExif : Boolean;
		private var cacheKey : String;

		private static var cache : Dictionary = new Dictionary();
		private static var cacheOrder : DLinkedList = new DLinkedList();

		public function BitmapLoader() : void
		{
			super();
		}

		public function ReleaseAll() : void
		{
			CloseStream();
			SetBitmap( null );
		}

		[Bindable( event="progress", event="complete" )] public function get stream() : Object
		{
			return bufferedStream.stream;
		}

		[Bindable( event="propertyChange" )] public function get url() : String
		{
			return ( request != null ) ? request.url : null;
		}

		public function set url( url : String ) : void
		{
			if ( request == null || request.url != url )
			{
				SetBitmap( null );

				if ( url != null )
				{
					cacheKey = url + "_" + maxImageWidth + "_" + maxImageHeight;
					CloseStream();
/*
					if ( cacheKey in cache )
					{
						trace( "Using cached item" );
						setTimeout( function() : void
						{
							SetBitmap( new Bitmap( cache[ cacheKey ] as BitmapData, "auto", true ) );
						}, 250 );
					}
					else*/
					{
						request = new URLRequest( url );
						callLater( Load );
					}
				}
				else SetBitmap( null );
			}
		}

		public function Uncache() : void
		{
/*			if ( url != null )
			{
				var item : DLinkedListNode = cacheOrder.find( cacheKey );

				if ( item != null )
				{
					cacheOrder.remove( cacheKey );
					delete cache[ cacheKey ];
				}
			}*/
		}

		private function Load() : void
		{
			try
			{
				tryParseExif = thumbnail;
				var stream : URLStream = new URLStream();
				OpenStream( stream );
				stream.load( request );
			}
			catch ( error : Error )
			{
				// TODO: Handle error!
			}
		}

        private function OpenStream( stream : URLStream ) : void
        {
        	bufferedStream = new BufferedStream( stream );

            stream.addEventListener( Event.COMPLETE, OnComplete );
//	            stream.addEventListener( HTTPStatusEvent.HTTP_STATUS, OnHTTPStatus );
	            stream.addEventListener( IOErrorEvent.IO_ERROR, OnLoadError );
//	            stream.addEventListener( Event.OPEN, OnOpen );
            stream.addEventListener( ProgressEvent.PROGRESS, OnProgress );
//	            stream.addEventListener( SecurityErrorEvent.SECURITY_ERROR, OnSecurityError );
        }

        private function CloseStream() : void
        {
        	if ( bufferedStream != null )
        	{
	        	var stream : URLStream = bufferedStream.stream as URLStream;

	            stream.removeEventListener( Event.COMPLETE, OnComplete );
//		            stream.removeEventListener( HTTPStatusEvent.HTTP_STATUS, OnHTTPStatus );
					stream.removeEventListener( IOErrorEvent.IO_ERROR, OnLoadError );
//	            	stream.removeEventListener( Event.OPEN, OnOpen );
            	stream.removeEventListener( ProgressEvent.PROGRESS, OnProgress );
//	            	stream.removeEventListener( SecurityErrorEvent.SECURITY_ERROR, OnSecurityError );

				bufferedStream.close();
				bufferedStream = null;
        	}
        }

		private function OnProgress( event : ProgressEvent ) : void
		{
			if ( TryParseEXIF() ) event.bytesLoaded = event.bytesTotal;
			dispatchEvent( event );
		}

		private function TryParseEXIF() : Boolean
		{
			if ( tryParseExif )
			{
				tryParseExif = false;

				// TODO: Use custom URLStream to block stream read if data not loaded yet...
				bufferedStream.position = 0;
				var bytes : ByteArray = null;

				try { bytes = Exif.Parse( bufferedStream ); }
				catch ( e : Error ) { Alert.show( e.message ); }

				if ( bytes != null && bytes.length > 0 )
				{
					CloseStream();
					LoadBytes( bytes );
					return true;
				}
			}

			return false;
		}

		private function OnComplete( event : Event ) : void
		{
			TryParseEXIF();

			if ( bufferedStream != null )
			{
				var bytes : ByteArray = bufferedStream.getBuffer();
				CloseStream();
				if ( bytes != null && bytes.length > 0 ) LoadBytes( bytes );
			}
		}

		private function LoadBytes( bytes : ByteArray ) : void
		{
			var loader : FlexLoader = new FlexLoader();
			loader.contentLoaderInfo.addEventListener( Event.COMPLETE, OnLoaded );
			loader.contentLoaderInfo.addEventListener( IOErrorEvent.IO_ERROR, OnLoadError );
			loader.loadBytes( bytes );
		}

		private function OnLoaded( event : Event ) : void
		{
			var loaderInfo : LoaderInfo = event.target as LoaderInfo;

			if ( loaderInfo != null && loaderInfo.content != null )
			{
				var newBitmap : Bitmap = loaderInfo.content as Bitmap;
				if ( loaderInfo.loader != null ) loaderInfo.loader.unload();

				var size : Number = Math.sqrt( newBitmap.width * newBitmap.height );
				var maxSize : Number = Math.sqrt( maxImageWidth * maxImageHeight );
				var scale : Number = Math.min( 1, maxSize / size );

				SetBitmap( ( scale < 1 )
					? ImageUtility.Resize( newBitmap, newBitmap.width * scale, newBitmap.height * scale )
					: newBitmap );

/*				if ( thumbnail )
				{
					cache[ cacheKey ] = bitmap.bitmapData;
					cacheOrder.push( cacheKey );

					if ( cacheOrder.length > 100 )
					{
						trace( "Removing cached item" );
						var removeUrl : String = cacheOrder.shift().value;
						if ( removeUrl in cache ) delete cache[ removeUrl ];
					}
				}*/
			}
		}

		private function OnLoadError( event : IOErrorEvent ) : void
		{
			CloseStream();
			error = event.text;
		}

		private function SetBitmap( bitmap : Bitmap ) : void
		{
			if ( bitmap != null ) bitmap.smoothing = true;
			this.error = null;
			this.bitmap = bitmap;
			this.invalidateProperties();

			if ( bitmap != null ) dispatchEvent( new Event( "complete" ) );
		}
	}
}
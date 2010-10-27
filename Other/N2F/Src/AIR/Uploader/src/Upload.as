package
{
//	import com.next2friends.PhotoOrganise;
	
	import flash.events.ErrorEvent;
	import flash.events.Event;
	import flash.events.HTTPStatusEvent;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.filesystem.File;
	import flash.net.URLLoader;
	import flash.net.URLLoaderDataFormat;
	import flash.net.URLRequest;
	import flash.net.URLRequestHeader;
	import flash.net.URLRequestMethod;
	import flash.utils.ByteArray;
	import flash.utils.Dictionary;
	import flash.utils.setTimeout;
	
	import mx.collections.ArrayCollection;
	import mx.controls.Image;
	import mx.core.UIComponent;
	import mx.graphics.codec.JPEGEncoder;
	import flash.display.Bitmap;
	import mx.messaging.channels.StreamingAMFChannel;
	import flash.display.BitmapData;

 	[Event( name="progress", type="flash.events.ProgressEvent" )]
	[Event( name="complete", type="flash.events.Event" )]
	[Event( name="error", type="flash.events.Event" )]
	public class Upload extends UIComponent
	{
		[Bindable] public static var pending : ArrayCollection = new ArrayCollection();
		[Bindable] public static var active : ArrayCollection = new ArrayCollection();
		[Bindable] public static var uploads : Dictionary = new Dictionary();

		private var filename : String;
		private var url : String;
		private var collectionID : String;

		public function Upload( item : Object ) : void
		{
			filename = item.Name;
			url = item.Url;
			collectionID = item.CollectionID;

			uploads[ url ] = this;
		}

		public function Prepare() : void
		{
			var image : Image = new Image();

			var remove : Function = function Remove() : void
			{
				image.removeEventListener( ProgressEvent.PROGRESS, onProgress );
				image.removeEventListener( Event.COMPLETE, onComplete );
				image.removeEventListener( IOErrorEvent.IO_ERROR, onError );

				var index : int = active.getItemIndex( url );
				if ( index >= 0 ) active.removeItemAt( index );
				delete uploads[ url ];
			}

			var onProgress : Function = function OnProgress( event : ProgressEvent ) : void
			{
				dispatchEvent( event )
			}

			var onComplete : Function = function OnComplete( event : Event ) : void
			{
				var imageBytes : ByteArray = ImageUtility.Encode( image, 800, 600 );
				image.source = null;

				Start( imageBytes );
			}

			var onError : Function = function OnError( event : Event ) : void
			{
				remove();
				dispatchEvent( event );
			}

			image.addEventListener( ProgressEvent.PROGRESS, onProgress );
			image.addEventListener( Event.COMPLETE, onComplete );
			image.addEventListener( IOErrorEvent.IO_ERROR, onError );

			image.load( url );
		}

		private function Start( imageBytes : ByteArray ) : void
		{
			var loader : URLLoader = new URLLoader();

			var remove : Function = function Remove() : void
			{
				loader.removeEventListener( ProgressEvent.PROGRESS, onProgress );
				loader.removeEventListener( Event.COMPLETE, onComplete );
				loader.removeEventListener( IOErrorEvent.IO_ERROR, onIOError );
				loader.removeEventListener( SecurityErrorEvent.SECURITY_ERROR, onSecurityError );
				loader.removeEventListener( HTTPStatusEvent.HTTP_STATUS, onHTTPStatus );

				var index : int = active.getItemIndex( url );
				if ( index >= 0 ) active.removeItemAt( index );
				delete uploads[ url ];
			}

			var onProgress : Function = function OnProgress( event : ProgressEvent ) : void
			{
				dispatchEvent( event )
			}

			var onComplete : Function = function OnComplete( event : Event ) : void
			{
				remove();

				dispatchEvent( new ProgressEvent( ProgressEvent.PROGRESS, true, false, 100, 100 ) );
				dispatchEvent( event );
			}

			var onIOError : Function = function OnError( event : IOErrorEvent ) : void
			{
				remove();
				dispatchEvent( event );
			}

			var onSecurityError : Function = function OnError( event : SecurityError ) : void
			{
				remove();
				dispatchEvent( new ErrorEvent( SecurityErrorEvent.SECURITY_ERROR, true, false, event.message ) );
			}

			var onHTTPStatus : Function = function OnError( event : HTTPStatusEvent ) : void
			{
				if ( event.status == 500 )
				{
					remove();
					dispatchEvent( event );
				}
			}

			var onError : Function = function OnError( event : Event ) : void
			{
				remove();
				dispatchEvent( event );
			}

			var variables : Object = {
				webPhotoCollectionID: collectionID,
				TakenDT: new Date()
			};

			var request : URLRequest = new URLRequest( "http://services.next2friends.com/N2FWebservices/handler1.ashx" );
			request.method = URLRequestMethod.POST;
			request.contentType = 'multipart/form-data; boundary=' + UploadPostHelper.getBoundary();
			request.data = UploadPostHelper.getPostData( filename, imageBytes, variables );
			request.requestHeaders.push( new URLRequestHeader( 'Cache-Control', 'no-cache' ) );

			loader.addEventListener( ProgressEvent.PROGRESS, onProgress );
			loader.addEventListener( Event.COMPLETE, onComplete );
			loader.addEventListener( IOErrorEvent.IO_ERROR, onIOError );
			loader.addEventListener( SecurityErrorEvent.SECURITY_ERROR, onSecurityError );
			loader.addEventListener( HTTPStatusEvent.HTTP_STATUS, onHTTPStatus );
			loader.dataFormat = URLLoaderDataFormat.BINARY;

			loader.load( request );
		}

		public static function StartUploads() : void
		{
			var loadNext : Function = function Load() : void
			{
				if ( pending.length > 0 && active.length < 1 )
				{
					pending.removeItemAt( 0 ).Prepare();
				}

				setTimeout( loadNext, 1000 );
			}

			loadNext();				
		}

		public static function UploadPhotos( files : Object, collectionID : String ) : void
		{
			for ( var fileID : Object in files )
			{
				var file : File = files[ fileID ] as File;

				if ( file != null )
				{
					pending.addItem( new Upload( {
						Name: file.name,
						Url: file.url,
						CollectionID : collectionID
					} ) );
				}
			}
		}

		public static function UploadPhoto( name : String, url : String, bitmap : Bitmap, collectionID : String ) : Upload
		{
			active.addItem( url );

			var encoder : JPEGEncoder = new JPEGEncoder( 80 );

			var upload : Upload = new Upload( {
				Name: name,
				Url: url,
				CollectionID : collectionID
			} );

			upload.Start( encoder.encode( bitmap.bitmapData ) );

			return upload;
		} 
	}
}
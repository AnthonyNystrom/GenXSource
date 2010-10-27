package  
{
	import flash.events.AsyncErrorEvent;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.IOErrorEvent;
	import flash.events.NetStatusEvent;
	import flash.media.SoundTransform;
	import flash.media.Video;
	import flash.net.NetConnection;
	import flash.net.NetStream;
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class VideoStream extends EventDispatcher
	{
		public static var METADATARECEIVED : String = "METADATARECEIVED";
		public static var FINISHED : String = "FINISHED";

		private var stream : NetStream;
		private var playing : Boolean;
		private var duration : Number;

		public function VideoStream( connection : NetConnection )
		{
			var me : VideoStream = this;

			stream = new NetStream( connection );

			stream.addEventListener( NetStatusEvent.NET_STATUS, OnStreamStatus );
			stream.addEventListener( AsyncErrorEvent.ASYNC_ERROR, OnStreamAsyncError );
			stream.addEventListener( IOErrorEvent.IO_ERROR, OnStreamIOError );

			stream.bufferTime = 1;
			stream.checkPolicyFile = true;
			stream.client = me;
		}

		public function get Stream() : NetStream { return stream; }

		private function OnStreamStatus( event : NetStatusEvent ) : void
		{ 
			Trace( "Stream Status" );
			Trace( event.info.code );

			switch ( event.info.code )
			{
				case "NetStream.Play.Start":
				{
					playing = true;
					break;
				}

				case "NetStream.Play.Stop":
				{
					playing = false;
					OnFinished();
					break;
				}

				case "NetStream.Seek.InvalidTime":
				{
					Seek( Stream.time );
					break;
				}
			}
		} 

		private function OnStreamAsyncError( event : AsyncErrorEvent ) : void
		{
			Trace( "Stream Async Error" );
		}

		private function OnStreamIOError( event : IOErrorEvent ) : void
		{
			Trace( "Stream IO Error" );
		}

		private function OnFinished() : void
		{
			dispatchEvent( new Event( FINISHED ) );
		}

		public function Play( name : String ) : void
		{
			Trace( "Playing " + name );
			stream.play( name, 0, -1 );
		}

		public function Pause() : void
		{
			playing = false;
			Trace( "Pause" );
			stream.pause();
		}

		public function Resume() : void
		{
			playing = true;
			Trace( "Resume" );
			stream.resume();
		}

		public function Seek( position : Number ) : void
		{
			stream.seek( position );
		}

		public function get Volume() : Number
		{
			return ( stream != null ) ? stream.soundTransform.volume : 0;
		}

		public function set Volume( value : Number ) : void
		{
			if ( stream != null ) stream.soundTransform = new SoundTransform( value );
		}

		private function OnTrace( event : TraceEvent ) : void
		{
			dispatchEvent( new TraceEvent( event.Message ) );
		}

		private function Trace( message : String ) : void
		{
			OnTrace( new TraceEvent( message ) );
		}

		public function onImageData( ... args ) : void
		{
			Trace( "onImageData" );
		}

		public function onTextData( ... args ) : void
		{
			Trace( "onTextData" );
		}

		public function onMetaData( meta : Object ) : void
		{
			this.duration = meta.duration;
			Trace( "onMetaData: " + meta.width + ", " + meta.height );
			dispatchEvent( new Event( METADATARECEIVED ) );
		}

		public function onCuePoint( info : Object ):void 
		{
			Trace( "onCuePoint" );
		}
   
		public function onTransition( info : Object, ... args ) : void 
		{
			Trace( "onTransition" );
		}

		public function onBWCheck( counter : Number ) : Number
		{
			Trace( "onBWCheck" );
			return ++ counter;
		}

		public function onBWDone( bandwidth : Number, ... args ) : void
		{
			Trace( "Bandwidth: " + bandwidth + " / " + args );
			dispatchEvent( new BandwidthCheckEvent( bandwidth ) );
		}

		public function onPlayStatus( status : Object ) : void
		{
			Trace( "onPlayStatus: " + status );
			OnFinished();
		}

		public function onLastSecond( info : Object ) : void
		{
			Trace( "onLastSecond: " + info );
		}

		public function close( ... args ) : void
		{
			Trace( "Close" );
		}

		public function get Duration() : Number { return duration; }
	}
}
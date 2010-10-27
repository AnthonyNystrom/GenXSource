package  
{
	import alducente.services.WebService;
	import flash.display.Bitmap;
	import flash.display.Graphics;
	import flash.display.SpreadMethod;
	import flash.display.Sprite;
	import flash.events.ErrorEvent;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.events.NetStatusEvent;
	import flash.events.SecurityErrorEvent;
	import flash.geom.Matrix;
	import flash.media.SoundTransform;
	import flash.media.Video;
	import flash.net.NetConnection;
	import flash.net.NetStream;

	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class VideoPlayer extends Sprite
	{
		public static var CONNECT : String = "CONNECT";
		public static var ERROR : String = "ERROR";
		public static var FINISHED : String = "FINISHED";

		private var video : Video;
		private var buttonPanel : Sprite = null;
		private var ns : NetStream;

		public function VideoPlayer(
			width : Number,
			height : Number,
			server : String )
		{
			video = new Video( width, height );
			addChild( video );

			var me : VideoPlayer = this;

			var onNetStatus : Function = function( event : NetStatusEvent ) : void
			{
				if ( event.info.code == "NetConnection.Connect.Success" )
				{
					trace( "Connected" );
					ns = new NetStream( nc );
					ns.checkPolicyFile = true;
					ns.client = me;
					video.attachNetStream( ns );
					OnConnect();
				}
			};

			var onNetSecurityError : Function = function( event : SecurityErrorEvent ) : void
			{
				trace( "netSecurityError: " + event );
				OnError( event.text );
			};

			var nc : NetConnection = new NetConnection();
			nc.addEventListener( NetStatusEvent.NET_STATUS, onNetStatus );
			nc.addEventListener( SecurityErrorEvent.SECURITY_ERROR, onNetSecurityError );
			nc.client = me;

			try
			{
				nc.connect( server, "XXXXXXXX" );
			}
			catch ( error : Error )
			{
				OnError( error.message );
			}
		}

		private function OnConnect() : void
		{
			dispatchEvent( new Event( CONNECT ) );
		}

		private function OnError( text : String ) : void
		{
			dispatchEvent( new ErrorEvent( ERROR, false, false, text ) );
		}

		private function OnFinished() : void
		{
			dispatchEvent( new Event( FINISHED ) );
		}

		public function Play( name : String ) : void
		{
			ns.play( name );
		}

		public function Pause() : void
		{
			ns.pause();
		}

		public function Resume() : void
		{
			ns.resume();
		}

		public function get Volume() : Number
		{
			return ( ns != null ) ? ns.soundTransform.volume : 0;
		}

		public function set Volume( value : Number ) : void
		{
			if ( ns != null ) ns.soundTransform = new SoundTransform( value );
		}

		public function get AspectRatio() : Number { return video ? video.videoWidth / video.videoHeight : 4 / 3; }

		public function onMetaData( meta : Object ) : void
		{
			trace( "onMetaData: " + meta );

			for ( var i : * in meta )
			{
				trace( i + ": " + meta[ i ] );
			}
		}

		public function onBWDone() : void
		{
			trace( "onBWDone" );
		}

		public function onPlayStatus( status : Object ) : void
		{
			trace( "onPlayStatus: " + status );
			OnFinished();
		}
	}	
}
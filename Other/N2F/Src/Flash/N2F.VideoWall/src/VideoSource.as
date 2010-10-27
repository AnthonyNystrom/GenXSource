package  
{
	import flash.events.ErrorEvent;
	import flash.events.Event;
	import flash.events.EventDispatcher;
	import flash.events.NetStatusEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.NetConnection;
	import flash.net.NetStream;

	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class VideoSource extends EventDispatcher
	{
		public static var CONNECT : String = "CONNECT";
		public static var ERROR : String = "ERROR";

		private var connection : NetConnection;
		private var stream : NetStream;

		public function VideoSource( serverUrl : String = null )
		{
        	connection = new NetConnection();
			connection.addEventListener( NetStatusEvent.NET_STATUS, OnNetStatus );
			connection.addEventListener( SecurityErrorEvent.SECURITY_ERROR, OnNetSecurityError );
			connection.client = this;
		}

		public function Connect( serverUrl : String = null ) : void
		{
 	        connection.connect( serverUrl );
		}

		public function CreateStream() : VideoStream
		{
			if ( !connection ) return null;

			var stream : VideoStream = new VideoStream( connection );
			stream.addEventListener( TraceEvent.TRACE, OnTrace );

			return stream;
		}

		private function OnNetStatus( event : NetStatusEvent ) : void
		{
			if ( event.info.code == "NetConnection.Connect.Success" )
			{
				Trace( "Connected" );
				OnConnect();
			}
		}

		private function OnConnect() : void
		{
			dispatchEvent( new Event( CONNECT ) );
		}

		private function OnNetSecurityError( event : SecurityErrorEvent ) : void
		{
			Trace( "netSecurityError: " + event );
			OnError( event.text );
		}

		private function OnError( text : String ) : void
		{
			dispatchEvent( new ErrorEvent( ERROR, false, false, text ) );
		}

		private function OnTrace( event : TraceEvent ) : void
		{
			dispatchEvent( new TraceEvent( event.Message ) );
		}

		private function Trace( message : String ) : void
		{
			OnTrace( new TraceEvent( message ) );
		}
	}
}
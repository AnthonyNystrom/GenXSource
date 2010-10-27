package
{
	import flash.events.Event;
	import flash.events.HTTPStatusEvent;
	import flash.events.IOErrorEvent;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
//	import flash.system.Shell;
	
	public class NetworkStatus
	{
		// Detect network status
		// Adapted from http://www.adobe.com/devnet/air/flex/articles/flickr_floater_06.html
		[Bindable] public static var networked : Boolean = false;
		private var initialized : Boolean = false;

		public static function initialize() : void
		{
//			Shell.shell.addEventListener( Event.NETWORK_CHANGE, checkNetworkConnection );
			checkNetworkConnection();
		}

		private static function checkNetworkConnection( event : Event = null ) : void
		{
			var headRequest:URLRequest = new URLRequest();
			headRequest.method = "HEAD";
			headRequest.url = "http://api.flickr.com/services/rest/?method=flickr.test.echo"; 
			var connectTest:URLLoader = new URLLoader(headRequest);
			connectTest.addEventListener( HTTPStatusEvent.HTTP_STATUS, connectHttpStatusHandler );
			connectTest.addEventListener( Event.COMPLETE, connectCompleteHandler );
			connectTest.addEventListener( IOErrorEvent.IO_ERROR, connectErrorHandler );
		}

		private static function connectHttpStatusHandler( event : * = null ) : void
		{
			networked = ( event.status == "0" );
		}

		private static function connectErrorHandler( event : IOErrorEvent ) : void
		{
			networked = false;
		}

		private static function connectCompleteHandler( event : Event ) : void
		{
			networked = true;
		}
	}
}
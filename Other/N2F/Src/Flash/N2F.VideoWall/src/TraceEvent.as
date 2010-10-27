package  
{
	import flash.events.Event;

	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class TraceEvent extends Event
	{
		public static var TRACE : String = "TRACE";

		private var message : String;

		public function TraceEvent( message : String )
		{
			super( "TRACE" );
			this.message = message;
		}

		public function get Message() : String { return message; }
	}
}
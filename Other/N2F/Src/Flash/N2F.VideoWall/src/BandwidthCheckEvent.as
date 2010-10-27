package  
{
	import flash.events.Event;
	
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class BandwidthCheckEvent extends Event
	{
		public static var BANDWIDTHCHECKDONE : String = "BANDWIDTHCHECKDONE";
		public var Bandwidth : Number;

		public function BandwidthCheckEvent( bandwidth : Number )
		{
			super( BANDWIDTHCHECKDONE );
			this.Bandwidth = bandwidth;
		}
	}
}
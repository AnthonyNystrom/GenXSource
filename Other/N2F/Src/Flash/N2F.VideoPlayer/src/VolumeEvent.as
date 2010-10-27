package  
{
	import flash.events.Event;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class VolumeEvent extends Event
	{
		public static var VOLUME : String = "VOLUME";

		public var volume : Number;

		public function VolumeEvent( volume : Number )
		{
			super( "VOLUME" );
			this.volume = volume;
		}
	}
}
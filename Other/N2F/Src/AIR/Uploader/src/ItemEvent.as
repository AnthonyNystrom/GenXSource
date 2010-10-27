package
{
	import flash.events.Event;

	public class ItemEvent extends Event
	{
		public var item : Object;
		public var param : Object;

		public function ItemEvent( type : String, item : Object = null, param : Object = null ) : void
		{
			this.item = item;
			this.param = param;
			super( type, true, true );
		}
	}
}
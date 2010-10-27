package
{
	import flash.events.Event;

	public class NewItemEvent extends Event
	{
		public var name : String;

		public function NewItemEvent( name : String ) : void
		{
			this.name = name;
			super( "NewItemEvent" );
		}
	}
}
package  
{
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class RowDefinition extends GridDefinitionBase
	{
		public function RowDefinition( height : * = null )
		{
			super( height );
		}

		public function get Height() : * { return size; }
		public function set Height( value : * ) : void { size = value; }
	}	
}
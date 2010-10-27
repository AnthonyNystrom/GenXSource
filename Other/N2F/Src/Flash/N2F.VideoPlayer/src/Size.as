package  
{
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class Size 
	{
		public var Width : Number;
		public var Height : Number;
		
		public function Size( width : Number = 0, height : Number = 0 )
		{
			this.Width = width;
			this.Height = height;
		}

		public function toString() : String 
		{
			return "{Width:" + Width + ",Height:" + Height + "}";
		}
	}
}
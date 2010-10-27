package
{
	public class Silverlight
	{
		public static function createDelegate( thisObject : *, fn : Function, argArray : Array = null )
		{
			return function() { fn.apply( thisObject, argArray ) };
		}
	}
}
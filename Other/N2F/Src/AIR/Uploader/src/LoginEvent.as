package
{
	import flash.events.Event;

	public class LoginEvent extends Event
	{
		public var email : String;
		public var password : String;

		public function LoginEvent( email : String, password : String ) : void
		{
			this.email = email;
			this.password = password;
			super( "LoginEvent" );
		}
	}
}
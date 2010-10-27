package
{
	import alducente.services.WebService;
	import alducente.services.WSDL;
	import flash.events.*;
	import flash.external.*;
	import flash.utils.Timer;

	public class LiveService
	{
		private namespace soapNS = "http://schemas.xmlsoap.org/soap/envelope/";
		private namespace n2fNS = "http://www.next2friends.com/";
		private var photoServiceUrl : String;
		private var videoServiceUrl : String;
		private var nickname : String;
		private var fToken : String;
		private var timer : Timer;
		private var onStatus : Function;
		private var cache : Array = [];

		public function LiveService(
			photoServiceUrl : String,
			videoServiceUrl : String,
			nickname : String,
			fToken : String,
			onStatus : Function ) : void
		{
			this.photoServiceUrl = photoServiceUrl;
			this.videoServiceUrl = videoServiceUrl;
			this.nickname = nickname;
			this.fToken = fToken;

			this.onStatus = onStatus;
			WebService.onStatus = onStatus;
		}

		public function Start( onPhotoResponse : Function, onVideoResponse : Function ) : void
		{
			if ( timer != null ) timer.stop();

			if ( videoServiceUrl && onVideoResponse != null ) StartVideoPoll( onVideoResponse );
			if ( photoServiceUrl && onPhotoResponse != null ) GetProfilePhoto( onPhotoResponse, nickname );
		}

		public function StartVideoPoll( onVideoResponse : Function ) : void
		{
			var vs : WebService = new WebService();

			var onConnect : Function = function( e : Event ) : void
			{
				OnStatus( "Connected to video server" );

				vs.removeEventListener( Event.CONNECT, onConnect );
				if ( timer ) timer.stop();

				var onTimer : Function = function( e : TimerEvent ) : void
				{
					var onResponse : Function = function( xml : XML ) : void
					{
						OnStatus( "Received video response: " + xml );
						use namespace n2fNS;
						var body : XML = GetSoapResult( xml );
						var res : XMLList = body ? body.*.* : null;
						var liveID : String = ( res.length() > 0 ) ? res[ 0 ].LiveID[ 0 ].text() : null;
						if ( onVideoResponse != null ) onVideoResponse( liveID );
					};

					if ( !fToken ) vs.FlashVideos( onResponse, nickname );
					else vs.FlashVideosFR( onResponse, nickname, fToken );
				};

				timer = new Timer( 5000 );
				timer.addEventListener( "timer", onTimer );
				timer.start();
			};

			OnStatus( "Connecting to video server at " + videoServiceUrl );
			vs.addEventListener( Event.CONNECT, onConnect );
			vs.connect( videoServiceUrl );
		}

		public function GetProfilePhoto( onPhotoResponse : Function, nickname : String ) : void
		{
			if ( cache[ nickname ] )
			{
				onPhotoResponse( cache[ nickname ] );
			}
			else
			{
				try
				{
					var ms : WebService = new WebService();

					var onConnect : Function = function( e : Event ) : void
					{
						OnStatus( "Connected to member server" );

						ms.removeEventListener( Event.CONNECT, onConnect );

						var onResponse : Function = function( xml : XML ) : void
						{
							OnStatus( "Received photo response: " + xml );
							use namespace n2fNS;
							var body : XML = GetSoapResult( xml );
							var res : XMLList = body ? body.GetProfilePhotoUrlResponse.GetProfilePhotoUrlResult : null;
							onPhotoResponse( res && res.length() > 0 ? res[ 0 ].text() : null );
							if ( res && res.length() > 0 ) cache[ nickname ] = res[ 0 ].text();
						};

						OnStatus( "Requesting photo for " + nickname );
						ms.GetProfilePhotoUrl( onResponse, nickname );
					};

					OnStatus( "Connecting to member server at " + photoServiceUrl );
					ms.addEventListener( Event.CONNECT, onConnect );
					ms.connect( photoServiceUrl );
				}
				catch ( e : Error )
				{
					OnStatus( "ERROR: " + e.message );
				}
			}
		}

		private static function GetSoapResult( xml : XML ) : XML
		{
			use namespace soapNS;
			return xml ? xml.Body[ 0 ] : null;
		}

		private function OnStatus( message : String ) : void
		{
			if ( onStatus != null ) onStatus( message );
			trace( message );
		}
	}
}
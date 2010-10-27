package 
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import com.chriscavanagh.Silverlayout.Controls.Primitives.*;
	import com.chriscavanagh.Silverlayout.Documents.*;
	import com.blitzagency.xray.logger.util.ObjectTools;
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.display.StageAlign;
	import flash.display.StageDisplayState;
	import flash.display.StageScaleMode;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	import flash.filters.ColorMatrixFilter;
	import flash.net.navigateToURL;
	import flash.net.URLRequest;
	import flash.system.Security;
	import flash.text.TextFormat;
	
	public class Main extends Sprite
	{
		private var liveService : LiveService;

		private var layoutContainer : FrameworkElement;
		private var controls : Border;
		private var userDetail : UserDetail;
		private var status : TextBlock;
		private var player : VideoPlayer;
		private var embedDialog : EmbedDialog;
		private var thumbnailViewer : ThumbnailViewer;

		private var ready : Boolean = false;
		private var playingLiveID : String = null;
		private var lastVolume : Number;
		private var isMuted : Boolean = false;

		private var nickname : String;
		private var startupLiveID : String = null;
		private var debugMethod : String = null;

		internal static var statusPopup : Popup;

		public function Main():void
		{
			stage.scaleMode = StageScaleMode.NO_SCALE;
			stage.align = StageAlign.TOP_LEFT;
			Security.allowDomain( "*" );

			var params : Object = this.root.loaderInfo.parameters;

			nickname = params[ "nickname" ];// ? params[ "nickname" ] : "lawrence";
			startupLiveID = params[ "liveid" ] ? params[ "liveid" ] : null;
			debugMethod = params[ "debugMethod" ] ? params[ "debugMethod" ] : null;

			var photoServiceUrl : String = params[ "memberserviceurl" ] ? params[ "memberserviceurl" ] : "http://services.next2friends.com/livewidget/MemberService.asmx?wsdl";
			var videoServiceUrl : String = params[ "videoserviceurl" ] ? params[ "videoserviceurl" ] : "http://services.next2friends.com/livewidget/VideoService.asmx?wsdl";
			var server : String = params[ "server" ] ? params[ "server" ] : "rtmp://69.21.114.99/DeviceStream";
			var fToken : String = params[ "ftoken" ] ? params[ "ftoken" ] : null;
			var photoUrl : String = params[ "photo" ] ? params[ "photo" ] : null;
			var embedCode : String = params[ "embedcode" ] ? params[ "embedcode" ] : "<object></object>";
			var directLink : String = params[ "directlink" ] ? params[ "directlink" ] : "http://www.next2friends.com";

			trace( "USING LOCAL RED5 SERVER" );
			nickname = "lawrence";
			server = "rtmp://localhost/oflaDemo";
			startupLiveID = "DarkKnight.flv";

			layoutContainer = new FrameworkElement();
			addChild( layoutContainer );

			player = new VideoPlayer(
				stage.stageWidth,
				stage.stageHeight,
				server );

			player.addEventListener( VideoPlayer.CONNECT, function() : void { OnPlayerReady( player ); } );
			player.addEventListener( VideoPlayer.ERROR, function() : void { OnPlayerError( player ); } );
			player.addEventListener( VideoPlayer.FINISHED, function() : void { OnPlayerFinished( player ); } );

			var playerContainer : MediaElement = new MediaElement( player );
			playerContainer.HorizontalAlignment = "Center";
			playerContainer.VerticalAlignment = "Center";
			layoutContainer.addChild( playerContainer );

			var controls : Border = new Border();
			controls.HorizontalAlignment = "Stretch";
			controls.VerticalAlignment = "Stretch";
			controls.BackgroundAlpha = 0;
			controls.Background = 0xFFFFFF;
			controls.Padding = 10;
			controls.Opacity = 0;
			layoutContainer.addChild( controls );

			var volume : VolumeControls = new VolumeControls( player.soundTransform.volume );
			volume.HorizontalAlignment = "Right";
			volume.VerticalAlignment = "Top";
			volume.addEventListener( VolumeControls.MUTE, OnToggleMute );
			volume.addEventListener( VolumeEvent.VOLUME, OnVolume );
			controls.addChild( volume );

			var bottomLeft : StackPanel = new StackPanel();
			bottomLeft.Orientation = "Horizontal";
			bottomLeft.HorizontalAlignment = "Left";
			bottomLeft.VerticalAlignment = "Bottom";
			controls.addChild( bottomLeft );

			var fullScreen : ContentControl = new ContentControl( new FullScreenButtonOffNormal() );
			fullScreen.HorizontalAlignment = "Left";
			fullScreen.VerticalAlignment = "Bottom";
			fullScreen.Height = 20;
			fullScreen.buttonMode = true;
			fullScreen.Opacity = 0.8;
			fullScreen.addEventListener( MouseEvent.ROLL_OVER, function( e : Event ) : void { fullScreen.Opacity = 1; } );
			fullScreen.addEventListener( MouseEvent.ROLL_OUT, function( e : Event ) : void { fullScreen.Opacity = 0.8; } );
			fullScreen.addEventListener( MouseEvent.CLICK, ToggleFullscreen );
			bottomLeft.addChild( fullScreen );

			var embedIcon : Button = new Button( "Embed", new TextFormat( "Arial", 10, 0xFFFFFF ) );
			embedIcon.HorizontalAlignment = "Left";
			embedIcon.VerticalAlignment = "Bottom";
			embedIcon.Background = 0xA0A0E0;
			bottomLeft.addChild( embedIcon );

			var grid : FrameworkElement = new FrameworkElement();
			grid.Padding = 10;
			layoutContainer.addChild( grid );

			userDetail = new UserDetail();
			userDetail.HorizontalAlignment = "Left";
			userDetail.VerticalAlignment = "Top";
			userDetail.buttonMode = true;
			userDetail.addEventListener( MouseEvent.CLICK, OnProfileSelect );
			grid.addChild( userDetail );

			var bottom : StackPanel = new StackPanel();
			bottom.VerticalAlignment = "Bottom";
			grid.addChild( bottom );

			thumbnailViewer = new ThumbnailViewer();
			thumbnailViewer.HorizontalAlignment = "Center";
			thumbnailViewer.VerticalAlignment = "Bottom";
			thumbnailViewer.Padding = 5;
			thumbnailViewer.Opacity = 0;
			bottom.addChild( thumbnailViewer );

			var logo : ContentControl = new ContentControl( new N2FLogo() );
			logo.HorizontalAlignment = "Right";
			logo.VerticalAlignment = "Bottom";
			logo.Height = 20;
			logo.Opacity = 0.5;
			logo.addEventListener( MouseEvent.ROLL_OVER, function( e : Event ) : void { logo.Opacity = 1; } );
			logo.addEventListener( MouseEvent.ROLL_OUT, function( e : Event ) : void { logo.Opacity = 0.5; } );
			logo.addEventListener( MouseEvent.CLICK, NavigateToSite );
			bottom.addChild( logo );

			status = new TextBlock();
			status.HorizontalAlignment = "Center";
			status.VerticalAlignment = "Center";
			status.TextFormat = new TextFormat( "Arial", 16, 0xFFFFFF );
			grid.addChild( status );

			embedDialog = new EmbedDialog( embedCode, directLink );
			embedDialog.Opacity = 0;
			embedDialog.addEventListener( Event.CLOSE, function( event : Event ) : void { embedDialog.Opacity = 0; } );
			grid.addChild( embedDialog );

			embedIcon.addEventListener( MouseEvent.CLICK, function( event : Event ) : void
			{
				embedDialog.Opacity = ( embedDialog.Opacity == 0 ) ? 1 : 0;
			} );

			statusPopup = CreateStatusPopup();
			grid.addChild( statusPopup );

			OnResize( null );

			stage.addEventListener( Event.FULLSCREEN, OnResize );
			stage.addEventListener( Event.RESIZE, OnResize );

			stage.addEventListener( MouseEvent.MOUSE_OVER, function( event : MouseEvent ) : void
			{
				controls.Opacity = 1;
			} );

			stage.addEventListener( MouseEvent.MOUSE_OUT, function( event : MouseEvent ) : void
			{
				controls.Opacity = 0;
			} );

			if ( !nickname )
			{
				status.Text = nickname ? nickname + " is not a member" : "Unknown member";
				status.TextFormat.color = 0xFF8080;
				return;
			}

			status.Text = "Connecting to Next2Friends...";

			try
			{
				if ( photoUrl ) OnPhotoResponse( photoUrl );

				liveService = new LiveService(
					photoServiceUrl,
					videoServiceUrl,
					nickname,
					fToken,
					OnTrace );

				liveService.Start(
					photoUrl ? null : OnPhotoResponse,
					startupLiveID ? null : OnVideoResponse );
			}
			catch ( e : Error )
			{
				var message : String = "ERROR: " + e.message;
				OnTrace( message );
				status.Text = message;
			}
		}

		private function CreateStatusPopup() : Popup
		{
			var popup : Popup = new Popup( 2000, 0x50FF50 );
			popup.HorizontalAlignment = "Right";
			popup.VerticalAlignment = "Bottom";

			return popup;
		}

		private function OnPlayerReady( player : VideoPlayer ) : void
		{
			OnTrace( "Player ready" );
			ready = true;

			if ( startupLiveID ) OnVideoResponse( startupLiveID );
		}

		private function OnPlayerError( player : VideoPlayer ) : void
		{
			OnTrace( "Player error" );
			playingLiveID = null;
		}

		private function OnPlayerFinished( player : VideoPlayer ) : void
		{
			thumbnailViewer.Opacity = 1;
			OnTrace( "Player finished" );
			playingLiveID = null;
		}

		private function OnPhotoResponse ( url : String ) : void
		{
			OnTrace( "Photo URL: " + url );
			userDetail.Load( url );
			userDetail.SetText( nickname );
		}

		private function OnVideoResponse( liveID : String ) : void
		{
			if ( ready )
			{
//				if ( liveID == null ) liveID = "DarkKnight.flv";

				if ( playingLiveID != null && liveID == null )
				{
					OnTrace( "Playback stopped." );
					player.Pause();
					playingLiveID = null;
				}
				else if ( liveID != null && liveID != playingLiveID )
				{
					OnTrace( "Playing " + liveID + "..." );
					player.Play( liveID );

					playingLiveID = liveID;
				}

				status.Text = ( liveID == null ) ? nickname + " is not broadcasting" : "";
			}
		}

		private function ToggleFullscreen( event : Event ) : void
		{
			stage.displayState = ( stage.displayState == StageDisplayState.NORMAL )
				? StageDisplayState.FULL_SCREEN
				: StageDisplayState.NORMAL;

			OnTrace( "Fullscreen toggled (" + ( stage.displayState == StageDisplayState.FULL_SCREEN ) + ")" );
		}

		private function OnResize( event : Event ) : void
		{
			layoutContainer.Width = stage.stageWidth;
			layoutContainer.Height = stage.stageHeight;
		}

		private function OnToggleMute( event : Event ) : void
		{
			if ( isMuted )
			{
				isMuted = false;
				player.Volume = lastVolume;
			}
			else
			{
				isMuted = true;
				lastVolume = player.Volume;
				player.Volume = 0;
			}

			OnTrace( "Mute toggled (" + isMuted + ")" );
		}

		private function OnVolume( event : VolumeEvent ) : void
		{
			OnTrace( "Volume: " + event.volume );
			player.Volume = event.volume;
		}

		private function OnProfileSelect( event : Event ) : void
		{
			NavigateTo( "http://www.next2friends.com/users/" + nickname );
		}

		private function NavigateToSite( event : Event ) : void
		{
			NavigateTo( "http://www.next2friends.com" );
		}

		private function NavigateTo( url : String ) : void
		{
			OnTrace( "Navigating to " + url + "..." );

			var request : URLRequest = new URLRequest( url );

			try
			{
				navigateToURL( request, '_blank' );
			}
			catch ( e : Error )
			{
				status.Text = e.message;
			}
		}

		private function OnTrace( message : String ) : void
		{
			if ( debugMethod && ExternalInterface.available )
			{
				ExternalInterface.call( debugMethod, message );
			}

			trace( message );
		}
	}
}
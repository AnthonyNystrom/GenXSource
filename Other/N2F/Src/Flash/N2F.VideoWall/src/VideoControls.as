package  
{
	import com.chriscavanagh.Silverlayout.Controls.*;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.text.TextFormat;
	
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class VideoControls extends Grid
	{
		public static var PLAY : String = "PLAY";
		public static var PAUSE : String = "PAUSE";
		public static var POSITIONCHANGEBEGIN : String = "POSITIONCHANGEBEGIN";
		public static var POSITIONCHANGEEND : String = "POSITIONCHANGEEND";
		public static var POSITIONCHANGED : String = "POSITIONCHANGED";
		public static var VOLUMECHANGED : String = "VOLUMECHANGED";
		public static var MUTECHANGED : String = "MUTECHANGED";

		private var playButton : Button;
		private var pauseButton : Button;
		private var scrub : Slider;
		private var text : TextBlock;
		private var volume : VolumeControls;
		private var isPlaying : Boolean;

		public function VideoControls()
		{
			AddColumn( new ColumnDefinition( "Auto" ) );
			AddColumn( new ColumnDefinition( 5 ) );
			AddColumn( new ColumnDefinition() );
			AddColumn( new ColumnDefinition( 5 ) );
			AddColumn( new ColumnDefinition( 100 ) );
			AddColumn( new ColumnDefinition( 5 ) );
			AddColumn( new ColumnDefinition( 80 ) );

			playButton = CreateButton( new PlayIcon() );
			playButton.Width = 30;
			playButton.Height = 30;
			playButton.CornerRadius = 30;
			playButton.addEventListener( MouseEvent.CLICK, OnPlay );
			addChild( playButton );

			pauseButton = CreateButton( new PauseIcon() );
			pauseButton.Width = 30;
			pauseButton.Height = 30;
			pauseButton.CornerRadius = 30;
			pauseButton.Visibility = "Hidden";
			pauseButton.addEventListener( MouseEvent.CLICK, OnPause );
			addChild( pauseButton );

			scrub = new Slider();
			scrub.GridColumn = 2;
			scrub.VerticalAlignment = "Center";
			scrub.addEventListener( Slider.VALUECHANGEBEGIN, OnScrubBegin );
			scrub.addEventListener( Slider.VALUECHANGEEND, OnScrubEnd );
			scrub.addEventListener( Slider.VALUECHANGED, OnScrub );
			addChild( scrub );

			text = new TextBlock( "", new TextFormat( "Arial", 14, 0xFFFFFF ) );Arial
			text.GridColumn = 4;
			text.HorizontalAlignment = "Center";
			text.VerticalAlignment = "Center";
			text.TextWrapping = "None";
			addChild( text );

			volume = new VolumeControls( 1 );
			volume.GridColumn = 6;
			volume.addEventListener( VolumeControls.VOLUMECHANGED, OnVolumeChanged );
			volume.addEventListener( VolumeControls.MUTE, OnMuteChanged );
			addChild( volume );
		}

		private function CreateButton( icon : Sprite ) : Button
		{
			var content : ContentControl = new ContentControl( icon );
			content.HorizontalAlignment = "Center";
			content.VerticalAlignment = "Center";

			var button : Button = new Button();
			button.HorizontalAlignment = "Center";
			button.VerticalAlignment = "Center";
			button.Padding = 10;
			button.addChild( content );

			return button;
		}

		private function OnPlay( event : Event ) : void
		{
			dispatchEvent( new Event( PLAY ) );
		}

		private function OnPause( event : Event ) : void
		{
			dispatchEvent( new Event( PAUSE ) );
		}

		private function OnScrubBegin( event : Event ) : void
		{
			dispatchEvent( new Event( POSITIONCHANGEBEGIN ) );
		}

		private function OnScrubEnd( event : Event ) : void
		{
			dispatchEvent( new Event( POSITIONCHANGEEND ) );
		}

		private function OnScrub( event : Event ) : void
		{
			dispatchEvent( new Event( POSITIONCHANGED ) );
		}

		private function OnVolumeChanged( event : Event ) : void
		{
			dispatchEvent( new Event( VOLUMECHANGED ) );
		}

		private function OnMuteChanged( event : Event ) : void
		{
			dispatchEvent( new Event( MUTECHANGED ) );
		}

		public function SetText( text : String ) : void
		{
			this.text.Text = text;
		}

		public function get Position() : Number { return scrub.Value; }

		public function set Position( value : Number ) : void { scrub.Value = value; }

		public function get Volume() : Number { return volume.Volume; }

		public function set Volume( value : Number ) : void { volume.Volume = value; }

		public function get IsMute() : Boolean { return volume.IsMute; }

		public function get IsPlaying() : Boolean { return isPlaying; }

		public function set IsPlaying( value : Boolean ) : void
		{
			if ( value != isPlaying )
			{
				isPlaying = value;
				playButton.Visibility = value ? "Hidden" : "Visible";
				pauseButton.Visibility = value ? "Visible" : "Hidden";
				trace( "IsPlaying: " + value );
			}
		}
	}
}
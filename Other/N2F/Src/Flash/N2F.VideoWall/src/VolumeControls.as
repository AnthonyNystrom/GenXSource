package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.geom.Rectangle;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class VolumeControls extends Grid
	{
		public static var MUTE : String = "MUTE";
		public static var VOLUMECHANGED : String = "VOLUMECHANGED";

		private var slider : Slider;
		private var mute : ContentControl;
		private var volume : Number;
		private var isMute : Boolean;

		public function VolumeControls( volume : Number )
		{
			this.volume = volume;
			this.isMute = false;

			AddColumn( new ColumnDefinition() );
			AddColumn( new ColumnDefinition( 5 ) );
			AddColumn( new ColumnDefinition( "Auto" ) );

			slider = new Slider();
			slider.VerticalAlignment = "Center";
			slider.addEventListener( Slider.VALUECHANGED, OnValueChanged );
			addChild( slider );

			mute = CreateMute();
			mute.VerticalAlignment = "Center";
			mute.GridColumn = 2;
			addChild( mute );
		}

		private function CreateMute() : ContentControl
		{
			var mute : ContentControl = new ContentControl( new MuteIcon() );
			mute.HorizontalAlignment = "Center";
			mute.VerticalAlignment = "Center";
			mute.Width = 30;
			mute.Height = 20;
			mute.buttonMode = true;
			mute.addEventListener( MouseEvent.CLICK, OnToggleMute );

			return mute;
		}

		public function get IsMute() : Boolean { return isMute; }

		public function get Volume() : Number { return volume; }

		public function set Volume( volume : Number ) : void
		{
			volume = Math.max( 0, Math.min( 1, volume ) );

			if ( volume != this.volume )
			{
				this.volume = volume;
				slider.Value = volume;
			}
		}

		private function OnValueChanged( event : Event ) : void
		{
			dispatchEvent( new Event( VOLUMECHANGED ) );
		}

		private function OnToggleMute( event : Event ) : void
		{
			isMute = !isMute;
			slider.Opacity = isMute ? 0.2 : 1;
			//panel.Opacity = BaseAlpha;
			dispatchEvent( new Event( MUTE ) );
		}

		private function get BaseAlpha() : Number
		{
			return isMute ? 0.8 : 1;
		}
	}
}
package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.ContentControl;
	import com.chriscavanagh.Silverlayout.Controls.StackPanel;
	import flash.display.DisplayObject;
	import flash.display.Graphics;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.geom.Rectangle;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class VolumeControls extends FrameworkElement
	{
		public static var MUTE : String = "MUTE";

		private var panel : FrameworkElement;
		private var slider : ContentControl;
		private var thumb : VolumeThumb;
		private var mute : ContentControl;
		private var volume : Number;
		private var isMute : Boolean;

		public function VolumeControls( volume : Number )
		{
			this.volume = volume;
			this.isMute = false;

			panel = CreatePanel();
			addChild( panel );

			panel.addChild( CreatePadding( 5 ) );

			slider = CreateSlider();
			panel.addChild( slider );

			panel.addChild( CreatePadding( 10 ) );

			mute = CreateMute();
			panel.addChild( mute );

			thumb = CreateThumb();
			thumb.x = slider.Width * volume;
			panel.addChild( thumb );
		}

		private function CreatePanel() : FrameworkElement
		{
			var panel : StackPanel = new StackPanel();
			panel.Orientation = "Horizontal";
			panel.Height = 20;
			panel.Opacity = BaseAlpha - 0.2;
			panel.addEventListener( MouseEvent.ROLL_OVER, function( e : Event ) : void { panel.Opacity = BaseAlpha; } );
			panel.addEventListener( MouseEvent.ROLL_OUT, function( e : Event ) : void { panel.Opacity = BaseAlpha - 0.2; } );

			var g : Graphics = panel.graphics;
			g.beginFill( 0xFFFFFF, 0 );
			g.drawRect( 0, 0, panel.Width, panel.Height );
			g.endFill();

			return panel;
		}

		private function CreatePadding( width : Number ) : FrameworkElement
		{
			var padding : FrameworkElement = new FrameworkElement();
			padding.VerticalAlignment = "Center";
			padding.Width = width;
			padding.Height = 20;

			return padding;
		}

		private function CreateSlider() : ContentControl
		{
			var slider : ContentControl = new ContentControl( new VolumeBar() );
			slider.HorizontalAlignment = "Left";
			slider.VerticalAlignment = "Center";
			slider.Width = 75;
			slider.Height = 5;
			slider.buttonMode = true;

			var onClick : Function = function( event : MouseEvent ) : void
			{
				if ( !isMute )
				{
					thumb.x = event.localX + 1;
					SetVolume( event.localX / event.target.width );
				}
			};

			slider.addEventListener( MouseEvent.CLICK, onClick );

			return slider;
		}

		private function CreateMute() : ContentControl
		{
			var mute : ContentControl = new ContentControl( new MuteIcon() );
			mute.VerticalAlignment = "Center";
			mute.Width = 30;
			mute.Height = 20;
			mute.buttonMode = true;
			mute.addEventListener( MouseEvent.CLICK, OnToggleMute );

			return mute;
		}

		private function CreateThumb() : VolumeThumb
		{
			var thumb : VolumeThumb = new VolumeThumb();
			thumb.width = 10;
			thumb.height = 20;
			thumb.x = 0;
			thumb.y = 1;
			thumb.buttonMode = true;

			var dragging : Boolean = false;
			var onStopDrag : Function;

			onStopDrag = function( event : MouseEvent ) : void
			{
				stage.removeEventListener( MouseEvent.MOUSE_MOVE, onMove );
				stage.removeEventListener( MouseEvent.MOUSE_UP, onStopDrag );
				thumb.stopDrag();
				dragging = false;
			};

			var onStartDrag : Function = function( event : MouseEvent ) : void
			{
				if ( !isMute )
				{
					stage.addEventListener( MouseEvent.MOUSE_MOVE, onMove );
					stage.addEventListener( MouseEvent.MOUSE_UP, onStopDrag );

					thumb.startDrag( false, new Rectangle( 0, thumb.y, slider.width, 0 ) );
					dragging = true;
				}
			};

			var onMove : Function = function( event : MouseEvent ) : void
			{
				SetVolume( thumb.x / slider.width );
			};

			thumb.addEventListener( MouseEvent.MOUSE_DOWN, onStartDrag );

			return thumb;
		}

		private function SetVolume( volume : Number ) : void
		{
			volume = Math.max( 0, Math.min( 1, volume ) );

			if ( volume != this.volume )
			{
				dispatchEvent( new VolumeEvent( this.volume = volume ) );
			}
		}

		private function OnToggleMute( event : Event ) : void
		{
			isMute = !isMute;
			panel.Opacity = BaseAlpha;
			dispatchEvent( new Event( MUTE ) );
		}

		private function get BaseAlpha() : Number
		{
			return isMute ? 0.8 : 1;
		}
	}
}
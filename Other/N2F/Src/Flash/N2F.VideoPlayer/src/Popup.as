package  
{
	import flash.events.Event;
	import flash.events.TimerEvent;
	import flash.text.TextFormat;
	import flash.utils.Timer;
	
	/**
	 * ...
	 * @author Chris Cavanagh
	 */
	public class Popup extends Border
	{
		private var timer : Timer;
		private var textFormat : TextFormat;

		public function Popup( timeout : Number, background : uint )
		{
			Background = background;
			BackgroundAlpha = 0.9;
			CornerRadius = 10;
			Padding = 10;
			Opacity = 0;

			timer = new Timer( timeout );
			timer.addEventListener( TimerEvent.TIMER, function( event : Event ) : void { Opacity = 0; timer.stop(); } );
		}

		public function Show( message : String ) : void
		{
			while ( numChildren > 0 ) removeChildAt( 0 );

			if ( !textFormat ) textFormat = new TextFormat( "Arial", 14, 0xFFFFFF );

			addChild( new TextBlock( message, textFormat ) );
			Opacity = 1;
			timer.start();
		}
	}
}
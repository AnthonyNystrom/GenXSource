package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import flash.events.Event;
	import flash.events.MouseEvent;
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class ThumbnailViewer extends StackPanel
	{
		private var leftArrow : ContentControl;
		private var rightArrow : ContentControl;

		public function ThumbnailViewer() 
		{
			Orientation = "Horizontal";

			leftArrow = new ContentControl( new LeftArrow() );
			leftArrow.Height = 60;
			leftArrow.Padding = 3;
			leftArrow.HorizontalAlignment = "Left";
			leftArrow.VerticalAlignment = "Center";
			leftArrow.Opacity = 0.5;
			leftArrow.useHandCursor = true;
			leftArrow.buttonMode = true;
			addChild( leftArrow );
			leftArrow.addEventListener( MouseEvent.ROLL_OVER, function( event : Event ) : void { leftArrow.Opacity = 1; } );
			leftArrow.addEventListener( MouseEvent.ROLL_OUT, function( event : Event ) : void { leftArrow.Opacity = 0.5; } );

			for ( var i : int = 0; i < 4; ++ i )
			{
				var thumbnail : Image = new Image( "http://farm2.static.flickr.com/1429/1430528819_edb63b79a6_t.jpg" );
				thumbnail.Padding = 3;
				thumbnail.Height = 60;
				thumbnail.HorizontalAlignment = "Left";
				thumbnail.VerticalAlignment = "Bottom";
				addChild( thumbnail );
			}

			rightArrow = new ContentControl( new RightArrow() );
			rightArrow.Height = 60;
			rightArrow.Padding = 3;
			rightArrow.HorizontalAlignment = "Left";
			rightArrow.VerticalAlignment = "Center";
			rightArrow.Opacity = 0.5;
			rightArrow.useHandCursor = true;
			rightArrow.buttonMode = true;
			addChild( rightArrow );
			rightArrow.addEventListener( MouseEvent.ROLL_OVER, function( event : Event ) : void { rightArrow.Opacity = 1; } );
			rightArrow.addEventListener( MouseEvent.ROLL_OUT, function( event : Event ) : void { rightArrow.Opacity = 0.5; } );
		}
	}
}
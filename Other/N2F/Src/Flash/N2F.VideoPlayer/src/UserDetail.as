package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import alducente.services.WebService;
	import flash.display.Graphics;
	import flash.display.Loader;
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.net.URLRequest;
	import flash.text.Font;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.text.TextLineMetrics;

	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class UserDetail extends StackPanel
	{
		private var image : Image;
		private var loaded : Boolean;
		private var textBlock : TextBlock;
		private var text : String;
		private var changed : Boolean = false;

		public function UserDetail() 
		{
			loaded = false;

			var border : Border = new Border();
			border.HorizontalAlignment = "Left";
			border.Thickness = 2;
			border.StrokeColor = 0xFFFFFF;
			border.Opacity = 0;
			addChild( border );

			image = new Image();
			image.Width = 50;
			image.Height = 50;
			image.addEventListener( Image.LOADED, function( event : Event ) : void { border.Opacity = 1; } );
			border.addChild( image );

			textBlock = new TextBlock();
			textBlock.HorizontalAlignment = "Left";
			textBlock.TextFormat = new TextFormat( "Arial", 14, 0xFFFFFF );
			addChild( textBlock );
		}

		public function Load( url : String ) : void
		{
			image.Source = url;
		}

		public function SetText( text : String ) : void
		{
			textBlock.Text = text;
		}
	}
}
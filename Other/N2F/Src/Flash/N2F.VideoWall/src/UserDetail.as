package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import alducente.services.WebService;
	import flash.display.Bitmap;
	import flash.display.BitmapData;
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
		private var border : Border;
		private var image : Image;
		private var loaded : Boolean;
		private var textBlock : TextBlock;
		private var text : String;
		private var changed : Boolean = false;
		private var flag : Image;
		private var cache : Array = [];

		public function UserDetail() 
		{
			loaded = false;

			border = new Border();
			border.HorizontalAlignment = "Center";
			border.Thickness = 2;
			border.StrokeColor = 0xFFFFFF;
			border.Opacity = 0;
			addChild( border );

			image = new Image();
			image.Width = 50;
			image.Height = 50;
			image.addEventListener( Image.LOADED, function( event : Event ) : void { border.Opacity = 1; OnComplete(); } );
			border.addChild( image );

			var panel : StackPanel = new StackPanel();
			panel.HorizontalAlignment = "Center";
			panel.VerticalAlignment = "Top";
			panel.Orientation = "Horizontal";
			addChild( panel );

			textBlock = new TextBlock();
			textBlock.HorizontalAlignment = "Left";
			textBlock.VerticalAlignment = "Center";
			textBlock.TextFormat = new TextFormat( "Arial", 14, 0xFFFFFF );
			panel.addChild( textBlock );

			flag = new Image();
			flag.HorizontalAlignment = "Left";
			flag.VerticalAlignment = "Center";
			flag.Opacity = 0;
			flag.addEventListener( Image.LOADED, function( event : Event ) : void { flag.Opacity = 1; OnComplete(); } );
			panel.addChild( flag );
		}

		public function Load( url : String ) : void
		{
			if ( cache[ url ] )
			{
				image.Content = new Bitmap( cache[ url ] );
				border.Opacity = 1;
//				image.Invalidate();
			}
			else if ( url )
			{
				image.Source = url;

				var onLoaded : Function;

				onLoaded = function( event : Event ) : void
				{
					var loader : Loader = image.Content as Loader;

					cache[ url ] = ( image.Content as Loader ).content as BitmapData;
					image.removeEventListener( Image.LOADED, onLoaded );
				};

				image.addEventListener( Image.LOADED, onLoaded );
			}
		}

		public function SetText( text : String ) : void
		{
			textBlock.Text = text;
			trace( ":" + text );
		}

		public function SetNationality( nationality : String ) : void
		{
			flag.Source = "http://www.next2friends.com/images/flags/" + nationality + ".gif";
		}

		private function OnComplete() : void
		{
			dispatchEvent( new Event( Event.COMPLETE ) );
		}
	}
}
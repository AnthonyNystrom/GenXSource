package  
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.geom.Rectangle;
	import flash.text.AntiAliasType;
	import flash.text.GridFitType;
	import flash.text.TextField;
	import flash.text.TextFormat;
	import flash.text.TextLineMetrics;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class TextBlock extends FrameworkElement
	{
		protected var text : String;
		protected var textFormat : flash.text.TextFormat;
		protected var textWrapping : String;
		protected var textField : TextField;

		protected var actualSize : Size;

		public function TextBlock( text : String = null, textFormat : flash.text.TextFormat = null )
		{
			super();

			actualSize = new Size();
			HorizontalAlignment = "Left";
			VerticalAlignment = "Top";
			textWrapping = "Wrap";

			mouseEnabled = false;
			mouseChildren = false;

			textField = new TextField();
			textField.embedFonts = true;
			textField.antiAliasType = AntiAliasType.ADVANCED;
			textField.gridFitType = GridFitType.SUBPIXEL;
			textField.selectable = false;
			addChild( textField );

			if ( text ) Text = text;
			if ( textFormat ) TextFormat = textFormat;
		}

		protected function ApplyProperties( size : Size = null ) : void
		{
			textField.width = ( size && size.Width > 0 ) ? size.Width : NaN;
			//trace( text + " " + size );
			textField.wordWrap = ( textWrapping.toLowerCase() == "wrap" );
			textField.multiline = textField.wordWrap;
			textField.text = text ? text : "";

			if ( textFormat ) textField.setTextFormat( textFormat );
		}

		override protected function OnRenderOverride(event:Event):void 
		{
			ApplyProperties( actualSize );
			if ( actualSize && actualSize.Height > 0 ) textField.height = actualSize.Height;

			super.OnRenderOverride( event );
		}

		public override function MeasureOverride( availableSize : Size ) : Size 
		{
			ApplyProperties( availableSize );

			actualSize = new Size(
				( HorizontalAlignment.toLowerCase() == "stretch" ) ? availableSize.Width : Width ? Width : 0,
				( VerticalAlignment.toLowerCase() == "stretch" ) ? availableSize.Height : Height ? Height : 0 );

			for ( var i : int = 0; i < textField.numLines; ++ i )
			{
				var metrics : TextLineMetrics = textField.getLineMetrics( i );
				actualSize.Width = Math.max( actualSize.Width, metrics.width + 6 );
				actualSize.Height += metrics.height + metrics.descent + 2;
			}

			return actualSize;
		}

		public function get Text() : String { return text; }
		public function set Text( value : String ) : void { text = value; Invalidate(); }
		public function get TextFormat() : flash.text.TextFormat { return textFormat; }
		public function set TextFormat( value : flash.text.TextFormat ) : void { textFormat = value; Invalidate(); }
		public function get TextWrapping() : String { return textWrapping; }
		public function set TextWrapping( value : String ) : void { textWrapping = value; Invalidate(); }
	}
}
package  
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.FocusEvent;
	import flash.geom.Rectangle;
	import flash.net.LocalConnection;
	import flash.text.AntiAliasType;
	import flash.text.GridFitType;
	import flash.text.TextField;
	import flash.text.TextFieldType;
	import flash.text.TextFormat;
	import flash.text.TextLineMetrics;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class TextBox extends TextBlock
	{
		public function TextBox( text : String = null, textFormat : flash.text.TextFormat = null )
		{
			super( text, textFormat );

			mouseEnabled = true;
			mouseChildren = true;

			TextWrapping = "NoWrap";

			textField.type = TextFieldType.INPUT;
			textField.selectable = true;
			textField.borderColor = 0xC0C0C0;
			textField.border = true;

			addEventListener( FocusEvent.FOCUS_IN, function( event : Event ) : void
			{
				var isEmpty : Boolean = ( !textField.text || textField.text == "" );

				if ( isEmpty )
				{
					// Hack - TextFields with embedded fonts are "difficult" to focus if the text is empty
					textField.text = " ";
					if ( TextFormat ) textField.setTextFormat( TextFormat );
				}

				textField.setSelection( 0, textField.text.length + 1 );
				if ( isEmpty ) textField.text = "";
			} );

			var me : TextBox = this;

			textField.addEventListener( Event.CHANGE, function( event : Event ) : void
			{
				me.text = textField.text;
			} );
		}

		public override function MeasureOverride( availableSize : Size ) : Size 
		{
			ApplyProperties( availableSize );

			var isEmpty : Boolean = ( !Text || Text == "" );

			if ( isEmpty )
			{
				textField.text = "W";
				if ( TextFormat ) textField.setTextFormat( TextFormat );
			}

			var defaultSize : TextLineMetrics = textField.getLineMetrics( 0 );

			actualSize = new Size(
				( HorizontalAlignment.toLowerCase() == "stretch" ) ? availableSize.Width : Width ? Width : defaultSize.width + 4,
				( VerticalAlignment.toLowerCase() == "stretch" ) ? availableSize.Height : Height ? Height : defaultSize.height + 4 );

			return actualSize;
		}
	}
}
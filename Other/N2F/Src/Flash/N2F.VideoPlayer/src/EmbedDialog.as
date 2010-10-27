package  
{
	import com.chriscavanagh.Silverlayout.*;
	import com.chriscavanagh.Silverlayout.Controls.*;
	import com.chriscavanagh.Silverlayout.Documents.*;
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.sampler.NewObjectSample;
	import flash.system.System;
	import flash.text.TextField;
	import flash.text.TextFormat;
	
	/**
	* ...
	* @author Chris Cavanagh
	*/
	public class EmbedDialog extends FrameworkElement
	{
		private var textFormat : flash.text.TextFormat = new flash.text.TextFormat( "Arial", 10, 0x000000 );
		private var hoverTextFormat : flash.text.TextFormat = new flash.text.TextFormat( "Arial", 10, 0x000000, null, null, true );
		private var buttonTextFormat : flash.text.TextFormat = new flash.text.TextFormat( "Arial", 12, 0xFFFFFF )
		private var buttonBackground : uint = 0xA00000;

		public function EmbedDialog( embedCode : String, directLink : String )
		{
			HorizontalAlignment = "Center";
			VerticalAlignment = "Center";

			var border : Border = new Border();
			border.Width = 200;
			border.Background = 0xFFFFFF;
			border.BackgroundAlpha = 0.9;
			border.CornerRadius = 20;
			border.Padding = 5;
			addChild( border );

			var closeButton : Button = new Button( "X", buttonTextFormat );
			closeButton.HorizontalAlignment = "Right";
			closeButton.VerticalAlignment = "Top";
			closeButton.Background = buttonBackground;
			closeButton.CornerRadius = 20;
			closeButton.addEventListener( MouseEvent.CLICK, function( event : Event ) : void { Opacity = 0; } );
			border.addChild( closeButton );

			var verticalStack : StackPanel = new StackPanel();
			verticalStack.HorizontalAlignment = "Top";
			border.addChild( verticalStack );

			var menu : StackPanel = CreateMenu();
			verticalStack.addChild( menu );

			// Pages

			var pages : FrameworkElement = new FrameworkElement();
			pages.VerticalAlignment = "Bottom";
			verticalStack.addChild( pages );

			var embedPage : FrameworkElement = CreateEmbedPage( embedCode );
			embedPage.Visibility = "Collapsed";
			pages.addChild( embedPage );

			var getLinkPage : FrameworkElement = CreateGetLinkPage( directLink );
			getLinkPage.Visibility = "Collapsed";
			pages.addChild( getLinkPage );

			var emailPage : FrameworkElement = CreateEmailPage();
			emailPage.Visibility = "Collapsed";
			pages.addChild( emailPage );

			var setPage : Function = function( page : FrameworkElement ) : void
			{
				FrameworkElement.Suspend();

				embed.Background = ( page == embed ) ? 0xB0B0FF : undefined;
				embedPage.Visibility = ( page == embed ) ? "Visible" : "Collapsed";

				getLink.Background = ( page == getLink ) ? 0xB0B0FF : undefined;
				getLinkPage.Visibility = ( page == getLink ) ? "Visible" : "Collapsed";

				email.Background = ( page == email ) ? 0xB0B0FF : undefined;
				emailPage.Visibility = ( page == email ) ? "Visible" : "Collapsed";

				FrameworkElement.Resume();
				Invalidate();
			};

			// Menu items

			var embed : Hyperlink = new Hyperlink( "Embed", textFormat, hoverTextFormat, function( event : Event ) : void { setPage( embed ); } );
			menu.addChild( embed );

			var getLink : Hyperlink = new Hyperlink( "Get link", textFormat, hoverTextFormat, function( event : Event ) : void { setPage( getLink ); } );
			menu.addChild( getLink );

			var email : Hyperlink = new Hyperlink( "Email", textFormat, hoverTextFormat, function( event : Event ) : void { setPage( email ); } );
			menu.addChild( email );

			setPage( embed );
		}

		private function CreateMenu() : StackPanel
		{
			var menu : StackPanel = new StackPanel();
			menu.HorizontalAlignment = "Left";
			menu.VerticalAlignment = "Top";
			menu.Orientation = "Horizontal";

			return menu;
		}

		private function CreateEmbedPage( embedCode : String ) : FrameworkElement
		{
			var page : StackPanel = new StackPanel();

			page.addChild( new TextBlock(
				"Use the embed code below to put this video on your own website.",
				textFormat ) );

			var panel : Grid = new Grid();
			panel.VerticalAlignment = "Top";
			panel.AddColumn( new ColumnDefinition() );
			panel.AddColumn( new ColumnDefinition( 5 ) );
			panel.AddColumn( new ColumnDefinition( "Auto" ) );
			page.addChild( panel );

			var textBox : TextBox = new TextBox( embedCode, textFormat );
			textBox.GridColumn = 0;
			textBox.HorizontalAlignment = "Stretch";
			textBox.VerticalAlignment = "Center";
			panel.addChild( textBox );

			var copyButton : Button = new Button( "Copy code", buttonTextFormat );
			copyButton.GridColumn = 2;
			copyButton.HorizontalAlignment = "Right";
			copyButton.VerticalAlignment = "Center";
			copyButton.Background = buttonBackground;
			copyButton.addEventListener( MouseEvent.CLICK, function( event : Event ) : void { CopyToClipboard( textBox.Text ); } );
			panel.addChild( copyButton );

			return page;
		}

		private function CreateGetLinkPage( directLink : String ) : FrameworkElement
		{
			var page : StackPanel = new StackPanel();

			page.addChild( new TextBlock(
				"Use the link below to show this video from your own website.",
				textFormat ) );

			var panel : Grid = new Grid();
			panel.VerticalAlignment = "Top";
			panel.AddColumn( new ColumnDefinition() );
			panel.AddColumn( new ColumnDefinition( 5 ) );
			panel.AddColumn( new ColumnDefinition( "Auto" ) );
			page.addChild( panel );

			var textBox : TextBox = new TextBox( directLink, textFormat );
			textBox.GridColumn = 0;
			textBox.HorizontalAlignment = "Left";
			textBox.VerticalAlignment = "Center";
			textBox.Width = 125;
			panel.addChild( textBox );

			var copyButton : Button = new Button( "Copy link", buttonTextFormat );
			copyButton.GridColumn = 2;
			copyButton.HorizontalAlignment = "Right";
			copyButton.VerticalAlignment = "Center";
			copyButton.Background = buttonBackground;
			copyButton.addEventListener( MouseEvent.CLICK, function( event : Event ) : void { CopyToClipboard( textBox.Text ); } );
			panel.addChild( copyButton );

			return page;
		}

		private function CreateEmailPage() : FrameworkElement
		{
			var page : StackPanel = new StackPanel();

			var nameEmail : Grid = new Grid();
			nameEmail.VerticalAlignment = "Top";
			nameEmail.AddColumn( new ColumnDefinition( "Auto" ) );
			nameEmail.AddColumn( new ColumnDefinition( 5 ) );
			nameEmail.AddColumn( new ColumnDefinition() );
			page.addChild( nameEmail );

			var namePrompt : FrameworkElement = CreatePrompt( "Your name" );
			namePrompt.GridColumn = 0;
			namePrompt.Width = 92;
			nameEmail.addChild( namePrompt );

			var emailPrompt : FrameworkElement = CreatePrompt( "Your email" );
			emailPrompt.GridColumn = 2;
			nameEmail.addChild( emailPrompt );
	
			var friendEmails : FrameworkElement = CreatePrompt( "Friends' email addresses" );
			page.addChild( friendEmails );

			var message : FrameworkElement = CreatePrompt( "Message", 50 );
			page.addChild( message );

			var verticalSpacer : FrameworkElement = new FrameworkElement();
			verticalSpacer.Height = 5;
			verticalSpacer.VerticalAlignment = "Top";
			page.addChild( verticalSpacer );

			var sendButton : Button = new Button( "Send message", buttonTextFormat );
			sendButton.HorizontalAlignment = "Left";
			sendButton.VerticalAlignment = "Bottom";
			sendButton.Background = buttonBackground;
			page.addChild( sendButton );

			sendButton.addEventListener( MouseEvent.CLICK, function( event : Event ) : void
			{
				SendMessage( namePrompt.Tag.Text, emailPrompt.Tag.Text, friendEmails.Tag.Text, message.Tag.Text );
			} );

			return page;
		}

		private function CreatePrompt( labelText : String, height : * = null ) : FrameworkElement
		{
			var panel : StackPanel = new StackPanel();

			var label : TextBlock = new TextBlock( labelText, textFormat );
			label.VerticalAlignment = "Center";
			panel.addChild( label );

			var textBox : TextBox = new TextBox( null, textFormat );
			textBox.HorizontalAlignment = "Stretch";
			if ( height ) textBox.Height = height;
			panel.addChild( textBox );

			panel.Tag = textBox;

			return panel;
		}

		private function CopyToClipboard( content : String ) : void
		{
			System.setClipboard( content );
			Main.statusPopup.Show( "Copied to clipboard" );
		}

		private function SendMessage( name : String, email : String, friends : String, message : String ) : void
		{
			Main.statusPopup.Show( "Message sent" );
		}
	}
}
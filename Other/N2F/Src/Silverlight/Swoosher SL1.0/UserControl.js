/// <reference path="intellisense.js" />

function UserControl()
{
}

UserControl.prototype =
{
	load: function Load( plugIn, url, createNameScope, onCompleted )
	{
		var downloader = plugIn.createObject( "Downloader" );

		downloader.addEventListener( "Completed", Silverlight.createDelegate( this, function( sender, args )
		{
			var xaml = sender.getResponseText( "" );
			args = { xaml: plugIn.Content.CreateFromXaml( xaml, createNameScope ) };
			onCompleted( sender, args );
		} ) );

		downloader.open( "GET", url );
		downloader.send();
	}
}

var UserControl = new UserControl();